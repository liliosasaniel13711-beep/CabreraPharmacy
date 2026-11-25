Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Windows.Forms

Public Class SaleLineRecord
    Public Property ProductId As Integer
    Public Property BrandName As String
    Public Property GenericName As String
    Public Property Dosage As String
    Public Property Unit As String
    Public Property Quantity As Integer
    Public Property UnitPrice As Decimal
    Public Property LineTotal As Decimal
    Public Property IsPrescription As Boolean
End Class

Public Class SaleRecord
    Public Property SaleMode As String
    Public Property WholesaleClientId As Integer?
    Public Property CustomerType As String
    Public Property CustomerName As String
    Public Property CustomerIdNumber As String
    Public Property CustomerTin As String
    Public Property Subtotal As Decimal
    Public Property DiscountAmount As Decimal
    Public Property TotalAmount As Decimal
    Public Property AmountReceived As Decimal
    Public Property ChangeAmount As Decimal
    Public Property PrescriptionNumber As String
    Public Property PrescriberName As String
    Public Property PrescriberLicense As String
    Public Property PrescriptionDate As DateTime?
    Public Property PurchaseOrderNumber As String
    Public Property PurchaseOrderDate As DateTime?
    Public Property DeliveryDate As DateTime?
    Public Property PaymentMethod As String
    Public Property PaymentProvider As String
    Public Property PaymentAccountName As String
    Public Property PaymentReferenceNumber As String
    Public Property Items As List(Of SaleLineRecord)
End Class

Public Class SaleSummary
    Public Property TodaySales As Decimal
    Public Property RetailSales As Decimal
    Public Property RetailTransactions As Integer
    Public Property WholesaleSales As Decimal
    Public Property WholesaleTransactions As Integer
End Class

Public Class SalesRepository

    Private Shared customerTinColumnEnsured As Boolean = False

    Private Shared Sub EnsureCustomerTinColumn(connection As MySqlConnection)
        If customerTinColumnEnsured Then Return
        Try
            Using cmd As New MySqlCommand("ALTER TABLE pharmacy_sales ADD COLUMN IF NOT EXISTS customer_tin VARCHAR(30) DEFAULT NULL AFTER customer_id_number;", connection)
                cmd.ExecuteNonQuery()
            End Using
            customerTinColumnEnsured = True
        Catch
            Try
                Using cmd As New MySqlCommand("ALTER TABLE pharmacy_sales ADD COLUMN customer_tin VARCHAR(30) DEFAULT NULL AFTER customer_id_number;", connection)
                    cmd.ExecuteNonQuery()
                End Using
            Catch
                ' Column already exists
            End Try
            customerTinColumnEnsured = True
        End Try
    End Sub

    Public Function SaveSale(sale As SaleRecord) As Long
        If sale Is Nothing Then Throw New ArgumentNullException(NameOf(sale))
        If CurrentUserID <= 0 Then Throw New InvalidOperationException("There is no signed-in cashier for this transaction.")
        If sale.Items Is Nothing OrElse sale.Items.Count = 0 Then Throw New InvalidOperationException("A transaction must contain at least one item.")

        Using connection = GetConnection()
            connection.Open()
            EnsureCustomerTinColumn(connection)
            Using dbTransaction = connection.BeginTransaction()
                Try
                    Dim allocationTrackingEnabled = BatchControlRepository.IsMigrationApplied(connection, dbTransaction)
                    If Not allocationTrackingEnabled Then
                        Throw New InvalidOperationException("Batch allocation controls are missing or incomplete. Sales are unavailable until V001 is verified and applied by an authorized administrator.")
                    End If
                    Const insertSale = "INSERT INTO pharmacy_sales (sale_mode, cashier_user_id, customer_type, customer_name, customer_id_number, customer_tin, subtotal, discount_amount, total_amount, amount_received, change_amount, prescription_no, prescribing_practioner, prc_liscense, prescription_date, purchase_order_no, purchase_order_date, payment_method, payment_provider, payment_account_name, payment_reference_number, wholesale_client_id) " &
                                       "VALUES (@saleMode, @cashierId, @customerType, @customerName, @customerId, @customerTin, @subtotal, @discount, @total, @cash, @change, @rxNumber, @prescriber, @prescriberLicense, @rxDate, @poNumber, @poDate, @paymentMethod, @paymentProvider, @paymentAccountName, @paymentReference, @wholesaleClientId)"
                    Dim saleId As Long
                    Using command As New MySqlCommand(insertSale, connection, dbTransaction)
                        command.Parameters.AddWithValue("@saleMode", sale.SaleMode)
                        command.Parameters.AddWithValue("@cashierId", CurrentUserID)
                        command.Parameters.AddWithValue("@customerType", sale.CustomerType)
                        AddNullable(command, "@customerName", sale.CustomerName)
                        AddNullable(command, "@customerId", sale.CustomerIdNumber)
                        AddNullable(command, "@customerTin", sale.CustomerTin)
                        command.Parameters.AddWithValue("@subtotal", sale.Subtotal)
                        command.Parameters.AddWithValue("@discount", sale.DiscountAmount)
                        command.Parameters.AddWithValue("@total", sale.TotalAmount)
                        command.Parameters.AddWithValue("@cash", sale.AmountReceived)
                        command.Parameters.AddWithValue("@change", sale.ChangeAmount)
                        AddNullable(command, "@rxNumber", sale.PrescriptionNumber)
                        AddNullable(command, "@prescriber", sale.PrescriberName)
                        AddNullable(command, "@prescriberLicense", sale.PrescriberLicense)
                        AddNullable(command, "@rxDate", sale.PrescriptionDate)
                        AddNullable(command, "@poNumber", sale.PurchaseOrderNumber)
                        AddNullable(command, "@poDate", sale.PurchaseOrderDate)
                        AddNullable(command, "@paymentMethod", sale.PaymentMethod)
                        AddNullable(command, "@paymentProvider", sale.PaymentProvider)
                        AddNullable(command, "@paymentAccountName", sale.PaymentAccountName)
                        AddNullable(command, "@paymentReference", sale.PaymentReferenceNumber)
                        AddNullable(command, "@wholesaleClientId", sale.WholesaleClientId)
                        command.ExecuteNonQuery()
                    End Using

                    Using idCommand As New MySqlCommand("SELECT LAST_INSERT_ID()", connection, dbTransaction)
                        saleId = Convert.ToInt64(idCommand.ExecuteScalar())
                    End Using

                    Const insertItem = "INSERT INTO pharmacy_sale_items (sale_id, product_id, brand_name, generic_name, dosage, unit, quantity, unit_price, line_total, is_prescription) " &
                                       "VALUES (@saleId, @productId, @brand, @generic, @dosage, @unit, @quantity, @price, @lineTotal, @isPrescription)"
                    For Each item In sale.Items
                        Dim quantityToDeduct = item.Quantity
                        If String.Equals(sale.SaleMode, "Wholesale", StringComparison.OrdinalIgnoreCase) Then
                            quantityToDeduct *= GetQuantityPerBox(connection, dbTransaction, item.ProductId)
                        End If

                        Dim allocations = DeductInventory(connection, dbTransaction, item.ProductId, quantityToDeduct, allocationTrackingEnabled)

                        Dim saleItemId As Long
                        Using command As New MySqlCommand(insertItem, connection, dbTransaction)
                            command.Parameters.AddWithValue("@saleId", saleId)
                            command.Parameters.AddWithValue("@productId", item.ProductId)
                            command.Parameters.AddWithValue("@brand", item.BrandName)
                            command.Parameters.AddWithValue("@generic", item.GenericName)
                            AddNullable(command, "@dosage", item.Dosage)
                            AddNullable(command, "@unit", item.Unit)
                            command.Parameters.AddWithValue("@quantity", item.Quantity)
                            command.Parameters.AddWithValue("@price", item.UnitPrice)
                            command.Parameters.AddWithValue("@lineTotal", item.LineTotal)
                            command.Parameters.AddWithValue("@isPrescription", item.IsPrescription)
                            command.ExecuteNonQuery()
                            saleItemId = command.LastInsertedId
                        End Using

                        RecordBatchAllocations(connection, dbTransaction, saleItemId, allocations)
                    Next

                    dbTransaction.Commit()
                    Return saleId
                Catch
                    Try
                        dbTransaction.Rollback()
                    Catch
                    End Try
                    Throw
                End Try
            End Using
        End Using
    End Function

    Private Shared Function GetQuantityPerBox(connection As MySqlConnection, transaction As MySqlTransaction, productId As Integer) As Integer
        Const query = "SELECT quantity_per_box FROM product WHERE product_ID = @productId FOR UPDATE"
        Using command As New MySqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            Dim result = command.ExecuteScalar()
            If result Is Nothing OrElse Convert.IsDBNull(result) Then
                Throw New InvalidOperationException("The product could not be found while checking its package size.")
            End If

            Dim quantityPerBox = Convert.ToInt32(result)
            If quantityPerBox < 1 Then Throw New InvalidOperationException("The product has an invalid quantity per box.")
            Return quantityPerBox
        End Using
    End Function

    Private Shared Function DeductInventory(connection As MySqlConnection,
                                            transaction As MySqlTransaction,
                                            productId As Integer,
                                            quantityToDeduct As Integer,
                                            allocationTrackingEnabled As Boolean) As List(Of KeyValuePair(Of Long, Integer))
        If quantityToDeduct < 1 Then Throw New InvalidOperationException("The sale quantity must be greater than zero.")

        Const productStatusQuery = "SELECT is_active, brand_name FROM product WHERE product_ID = @productId FOR UPDATE"
        Dim medicineName As String
        Using command As New MySqlCommand(productStatusQuery, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            Using reader = command.ExecuteReader()
                If Not reader.Read() Then
                    Throw New InvalidOperationException("The product could not be found while checking its sale status.")
                End If
                If Not Convert.ToBoolean(reader("is_active")) Then
                    Throw New InvalidOperationException("This product is inactive and cannot be sold.")
                End If
                medicineName = Convert.ToString(reader("brand_name"))
            End Using
        End Using

        Dim batches As New List(Of KeyValuePair(Of Long, Integer))()
        Dim availableQuantity As Integer = 0
        If Not allocationTrackingEnabled Then Throw New InvalidOperationException("Batch allocation controls are missing or incomplete; sale cannot be committed safely.")
        Dim saleableQuantityExpression = "GREATEST(0, ib.quantity_remaining - " &
                                     "COALESCE((SELECT SUM(q.quantity) FROM inventory_batch_quarantines q WHERE q.batch_id = ib.batch_id AND q.status = 'Active'), 0) - " &
                                     "COALESCE((SELECT SUM(a.quantity) FROM inventory_batch_actions a WHERE a.batch_id = ib.batch_id AND a.status = 'Pending'), 0))"
        Dim selectBatches = "SELECT ib.batch_id, " & saleableQuantityExpression & " AS available_quantity FROM inventory_batch ib " &
                            "WHERE ib.product_ID = @productId AND ib.quantity_remaining > 0 AND ib.expiry_date > CURDATE() " &
                            "ORDER BY ib.expiry_date, ib.batch_id FOR UPDATE"
        Using command As New MySqlCommand(selectBatches, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            Using reader = command.ExecuteReader()
                While reader.Read()
                    Dim batchId = reader.GetInt64("batch_id")
                    Dim batchQuantity = reader.GetInt32("available_quantity")
                    If batchQuantity > 0 Then
                        batches.Add(New KeyValuePair(Of Long, Integer)(batchId, batchQuantity))
                        availableQuantity += batchQuantity
                    End If
                End While
            End Using
        End Using

        If availableQuantity < quantityToDeduct Then
            Throw New InvalidOperationException("Insufficient Saleable Stock: Only " &
                                                availableQuantity.ToString(Globalization.CultureInfo.InvariantCulture) &
                                                " eligible units of " & medicineName & " are available. Requested: " &
                                                quantityToDeduct.ToString(Globalization.CultureInfo.InvariantCulture) & ".")
        End If

        Dim remainingToDeduct = quantityToDeduct
        Dim allocations As New List(Of KeyValuePair(Of Long, Integer))()
        For Each batch In batches
            If remainingToDeduct = 0 Then Exit For
            Dim quantityFromBatch = Math.Min(remainingToDeduct, batch.Value)
            Const updateBatch = "UPDATE inventory_batch SET quantity_remaining = quantity_remaining - @quantity " &
                                "WHERE batch_id = @batchId AND quantity_remaining >= @quantity"
            Using command As New MySqlCommand(updateBatch, connection, transaction)
                command.Parameters.AddWithValue("@quantity", quantityFromBatch)
                command.Parameters.AddWithValue("@batchId", batch.Key)
                If command.ExecuteNonQuery() <> 1 Then
                    Throw New InvalidOperationException("Stock changed during checkout. Reload the transaction and try again.")
                End If
            End Using
            allocations.Add(New KeyValuePair(Of Long, Integer)(batch.Key, quantityFromBatch))
            remainingToDeduct -= quantityFromBatch
        Next
        If remainingToDeduct <> 0 Then Throw New InvalidOperationException("Inventory allocation was incomplete; the sale has been rolled back.")

        Const synchronizeProductStock = "UPDATE product SET stock_quantity = " &
                                        "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch WHERE product_ID = @productId) " &
                                        "WHERE product_ID = @productId"
        Using command As New MySqlCommand(synchronizeProductStock, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            If command.ExecuteNonQuery() <> 1 Then
                Throw New InvalidOperationException("Could not synchronize the product stock total.")
            End If
        End Using
        Return allocations
    End Function

    Private Shared Sub RecordBatchAllocations(connection As MySqlConnection,
                                              transaction As MySqlTransaction,
                                              saleItemId As Long,
                                              allocations As IEnumerable(Of KeyValuePair(Of Long, Integer)))
        Const query As String = "INSERT INTO pharmacy_sale_item_batches (sale_item_id, batch_id, quantity) " &
                                "VALUES (@saleItemId, @batchId, @quantity)"
        For Each allocation In allocations
            Using command As New MySqlCommand(query, connection, transaction)
                command.Parameters.AddWithValue("@saleItemId", saleItemId)
                command.Parameters.AddWithValue("@batchId", allocation.Key)
                command.Parameters.AddWithValue("@quantity", allocation.Value)
                command.ExecuteNonQuery()
            End Using
        Next
    End Sub

    Public Function GetSummary() As SaleSummary
        Using connection = GetConnection()
            connection.Open()
            Const query = "SELECT " &
                          "COALESCE(SUM(CASE WHEN DATE(created_at) = CURDATE() THEN total_amount ELSE 0 END), 0) AS TodaySales, " &
                          "COALESCE(SUM(CASE WHEN sale_mode = 'Retail' THEN total_amount ELSE 0 END), 0) AS RetailSales, " &
                          "COALESCE(SUM(CASE WHEN sale_mode = 'Retail' THEN 1 ELSE 0 END), 0) AS RetailTransactions, " &
                          "COALESCE(SUM(CASE WHEN sale_mode = 'Wholesale' THEN total_amount ELSE 0 END), 0) AS WholesaleSales, " &
                          "COALESCE(SUM(CASE WHEN sale_mode = 'Wholesale' THEN 1 ELSE 0 END), 0) AS WholesaleTransactions " &
                          "FROM pharmacy_sales"
            Using command As New MySqlCommand(query, connection), reader = command.ExecuteReader()
                reader.Read()
                Return New SaleSummary With {
                    .TodaySales = reader.GetDecimal("TodaySales"),
                    .RetailSales = reader.GetDecimal("RetailSales"),
                    .RetailTransactions = reader.GetInt32("RetailTransactions"),
                    .WholesaleSales = reader.GetDecimal("WholesaleSales"),
                    .WholesaleTransactions = reader.GetInt32("WholesaleTransactions")
                }
            End Using
        End Using
    End Function

    Public Function GetTransactionReport(Optional saleMode As String = Nothing,
                                         Optional cashierUserId As Integer? = Nothing,
                                         Optional fromDate As DateTime? = Nothing,
                                         Optional throughDateExclusive As DateTime? = Nothing) As DataTable
        Dim table As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Const query = "SELECT ps.sale_id AS TransactionID, ps.created_at AS SaleDate, " &
                          "COALESCE(NULLIF(wc.company_name, ''), NULLIF(ps.customer_name, ''), 'Walk-in') AS Customer, " &
                          "COALESCE(NULLIF(u.full_name, ''), u.username, 'Unknown cashier') AS Cashier, " &
                          "ps.sale_mode AS SaleType, ps.total_amount AS Total, 'Completed' AS Status " &
                          "FROM pharmacy_sales ps " &
                          "LEFT JOIN wholesale_clients wc ON wc.client_id = ps.wholesale_client_id " &
                          "LEFT JOIN users u ON u.user_id = ps.cashier_user_id " &
                          "WHERE (@mode IS NULL OR ps.sale_mode = @mode) " &
                          "AND (@cashierId IS NULL OR ps.cashier_user_id = @cashierId) " &
                          "AND (@fromDate IS NULL OR ps.created_at >= @fromDate) " &
                          "AND (@throughDateExclusive IS NULL OR ps.created_at < @throughDateExclusive) " &
                          "ORDER BY ps.created_at DESC, ps.sale_id DESC"
            Using command As New MySqlCommand(query, connection)
                AddNullable(command, "@mode", If(String.IsNullOrWhiteSpace(saleMode), Nothing, saleMode))
                AddNullable(command, "@cashierId", If(cashierUserId.HasValue, CType(cashierUserId.Value, Object), Nothing))
                AddNullable(command, "@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), Nothing))
                AddNullable(command, "@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), Nothing))
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Public Function GetSalesReportItemDetails(Optional saleMode As String = Nothing,
                                             Optional fromDate As DateTime? = Nothing,
                                             Optional throughDateExclusive As DateTime? = Nothing) As DataTable
        Dim table As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Const query = "SELECT ps.sale_id AS TransactionID, ps.created_at AS SaleDate, " &
                          "COALESCE(NULLIF(ps.customer_name, ''), 'Walk-in') AS Customer, " &
                          "COALESCE(NULLIF(u.full_name, ''), u.username, 'Unknown cashier') AS Cashier, " &
                          "ps.sale_mode AS SaleType, si.brand_name AS BrandName, si.generic_name AS GenericName, " &
                          "si.dosage AS Dosage, si.unit AS Unit, si.quantity AS Quantity, " &
                          "si.unit_price AS UnitPrice, si.line_total AS LineTotal " &
                          "FROM pharmacy_sales ps " &
                          "INNER JOIN pharmacy_sale_items si ON si.sale_id = ps.sale_id " &
                          "LEFT JOIN users u ON u.user_id = ps.cashier_user_id " &
                          "WHERE (@mode IS NULL OR ps.sale_mode = @mode) " &
                          "AND (@fromDate IS NULL OR ps.created_at >= @fromDate) " &
                          "AND (@throughDateExclusive IS NULL OR ps.created_at < @throughDateExclusive) " &
                          "ORDER BY ps.created_at DESC, ps.sale_id DESC, si.sale_item_id"
            Using command As New MySqlCommand(query, connection)
                AddNullable(command, "@mode", If(String.IsNullOrWhiteSpace(saleMode), Nothing, saleMode))
                AddNullable(command, "@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), Nothing))
                AddNullable(command, "@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), Nothing))
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Public Function GetSaleReceiptDetails(saleId As Long) As DataTable
        Dim table As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            EnsureCustomerTinColumn(connection)
            Const query = "SELECT ps.sale_id, ps.created_at, ps.sale_mode, ps.customer_type, " &
                          "ps.customer_name, ps.customer_id_number, ps.customer_tin, ps.subtotal, ps.discount_amount, " &
                          "ps.total_amount, ps.amount_received, ps.change_amount, ps.prescription_no, " &
                          "ps.prescribing_practioner, ps.prc_liscense, ps.prescription_date, " &
                          "ps.purchase_order_no, ps.purchase_order_date, ps.payment_method, ps.payment_provider, ps.payment_account_name, ps.payment_reference_number, " &
                          "wc.company_name AS wholesale_company_name, wc.fda_lto_no AS wholesale_customer_license, " &
                          "CONCAT_WS(' ', NULLIF(wc.first_name, ''), NULLIF(wc.last_name, '')) AS contact_person, " &
                          "wc.contact_no_1 AS contact_number, " &
                          "CONCAT_WS(', ', NULLIF(wc.shipping_address_line, ''), NULLIF(wc.barangay, ''), NULLIF(wc.city_municipality, ''), NULLIF(wc.province, ''), NULLIF(wc.postal_code, '')) AS customer_address, " &
                          "COALESCE(NULLIF(u.full_name, ''), u.username, 'Unknown cashier') AS cashier_name, " &
                          "COALESCE(NULLIF(u.staff_initials, ''), '') AS cashier_initials, " &
                          "si.sale_item_id, si.product_id, si.brand_name, si.generic_name, si.dosage, si.unit, " &
                          "si.quantity, si.unit_price, si.line_total, si.is_prescription " &
                          "FROM pharmacy_sales ps " &
                          "LEFT JOIN wholesale_clients wc ON wc.client_id = ps.wholesale_client_id " &
                          "LEFT JOIN users u ON u.user_id = ps.cashier_user_id " &
                          "LEFT JOIN pharmacy_sale_items si ON si.sale_id = ps.sale_id " &
                          "WHERE ps.sale_id = @saleId ORDER BY si.sale_item_id"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@saleId", saleId)
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(table)
                End Using
            End Using
        End Using
        Return table
    End Function

    Private Shared Sub AddNullable(command As MySqlCommand, parameterName As String, value As Object)
        command.Parameters.AddWithValue(parameterName, If(value Is Nothing, DBNull.Value, value))
    End Sub

    ' =========================================================
    ' UNIFIED REPORTS, EXPENSES & SYNC BACKEND
    ' =========================================================

    Public Function GetInventoryStockReport() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim nearDays = ExpiryMonitorSettings.LoadNearExpiryDays()
                Const query = "SELECT " &
                              "p.brand_name AS 'Brand Name', " &
                              "p.generic_name AS 'Generic Name', " &
                              "c.category_name AS 'Category', " &
                              "p.unit_of_measure AS 'Unit', " &
                              "p.stock_quantity AS 'Stock Qty', " &
                              "p.reorder_point AS 'Reorder Point', " &
                              "COALESCE(MIN(CASE WHEN b.quantity_remaining > 0 THEN b.batch_no END), p.CPR_number) AS 'Batch / CPR', " &
                              "MIN(CASE WHEN b.quantity_remaining > 0 THEN b.expiry_date END) AS 'Earliest Expiry', " &
                              "s.company_name AS 'Supplier', " &
                              "CASE " &
                              "  WHEN EXISTS (SELECT 1 FROM inventory_batch eb WHERE eb.product_ID = p.product_ID AND eb.quantity_remaining > 0 AND eb.expiry_date <= CURDATE()) THEN 'Expired' " &
                              "  WHEN EXISTS (SELECT 1 FROM inventory_batch nb WHERE nb.product_ID = p.product_ID AND nb.quantity_remaining > 0 AND nb.expiry_date > CURDATE() AND nb.expiry_date <= DATE_ADD(CURDATE(), INTERVAL @nearDays DAY)) THEN 'Near Expiry' " &
                              "  WHEN p.stock_quantity <= p.reorder_point THEN 'Low Stock' " &
                              "  ELSE 'In Stock' " &
                              "END AS 'Status' " &
                              "FROM product p " &
                              "INNER JOIN product_category c ON p.category_ID = c.category_ID " &
                              "INNER JOIN supplier s ON p.supplier_ID = s.supplier_ID " &
                              "LEFT JOIN inventory_batch b ON p.product_ID = b.product_ID " &
                              "WHERE p.is_active = 1 " &
                              "GROUP BY p.product_ID, p.brand_name, p.generic_name, c.category_name, p.unit_of_measure, p.stock_quantity, p.reorder_point, p.CPR_number, s.company_name " &
                              "ORDER BY p.brand_name ASC"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@nearDays", nearDays)
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load inventory stock report: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function GetNearExpiryReport(Optional thresholdDays As Integer = 90) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT " &
                              "b.batch_no AS 'Batch No', " &
                              "p.brand_name AS 'Brand Name', " &
                              "p.generic_name AS 'Generic Name', " &
                              "c.category_name AS 'Category', " &
                              "b.quantity_remaining AS 'Remaining Qty', " &
                              "b.expiry_date AS 'Expiry Date', " &
                              "DATEDIFF(b.expiry_date, CURDATE()) AS 'Days Left', " &
                              "COALESCE(s.company_name, 'N/A') AS 'Supplier', " &
                              "'Near Expiry' AS 'Status' " &
                              "FROM inventory_batch b " &
                              "INNER JOIN product p ON b.product_ID = p.product_ID " &
                              "INNER JOIN product_category c ON p.category_ID = c.category_ID " &
                              "LEFT JOIN supplier s ON COALESCE(b.supplier_ID, p.supplier_ID) = s.supplier_ID " &
                              "WHERE b.quantity_remaining > 0 " &
                              "AND b.expiry_date > CURDATE() " &
                              "AND b.expiry_date <= DATE_ADD(CURDATE(), INTERVAL @days DAY) " &
                              "ORDER BY b.expiry_date ASC, p.brand_name ASC"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@days", thresholdDays)
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load near-expiry report: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function GetExpiredStockReport() As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT " &
                              "b.batch_no AS 'Batch No', " &
                              "p.brand_name AS 'Brand Name', " &
                              "p.generic_name AS 'Generic Name', " &
                              "c.category_name AS 'Category', " &
                              "b.quantity_remaining AS 'Expired Qty', " &
                              "b.expiry_date AS 'Expiry Date', " &
                              "DATEDIFF(CURDATE(), b.expiry_date) AS 'Days Expired', " &
                              "COALESCE(s.company_name, 'N/A') AS 'Supplier', " &
                              "'Expired' AS 'Status' " &
                              "FROM inventory_batch b " &
                              "INNER JOIN product p ON b.product_ID = p.product_ID " &
                              "INNER JOIN product_category c ON p.category_ID = c.category_ID " &
                              "LEFT JOIN supplier s ON COALESCE(b.supplier_ID, p.supplier_ID) = s.supplier_ID " &
                              "WHERE b.quantity_remaining > 0 " &
                              "AND b.expiry_date <= CURDATE() " &
                              "ORDER BY b.expiry_date ASC, p.brand_name ASC"
                Using cmd As New MySqlCommand(query, conn)
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load expired stock report: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function GetFinancialReport(Optional fromDate As DateTime? = Nothing, Optional throughDateExclusive As DateTime? = Nothing) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Date", GetType(DateTime))
        dt.Columns.Add("Transactions", GetType(Integer))
        dt.Columns.Add("Retail Revenue", GetType(Decimal))
        dt.Columns.Add("Wholesale Revenue", GetType(Decimal))
        dt.Columns.Add("Discounts", GetType(Decimal))
        dt.Columns.Add("Gross Revenue", GetType(Decimal))
        dt.Columns.Add("Expenses", GetType(Decimal))
        dt.Columns.Add("Net Profit", GetType(Decimal))

        Try
            Dim byDate As New SortedDictionary(Of DateTime, Decimal())()
            Using conn = GetConnection()
                conn.Open()

                Const salesSql = "SELECT DATE(created_at) AS SaleDay, " &
                                 "COUNT(sale_id) AS TxCount, " &
                                 "COALESCE(SUM(CASE WHEN sale_mode = 'Retail' THEN total_amount ELSE 0 END), 0) AS RetailTotal, " &
                                 "COALESCE(SUM(CASE WHEN sale_mode = 'Wholesale' THEN total_amount ELSE 0 END), 0) AS WholesaleTotal, " &
                                 "COALESCE(SUM(discount_amount), 0) AS DiscountTotal, " &
                                 "COALESCE(SUM(total_amount), 0) AS GrossTotal " &
                                 "FROM pharmacy_sales " &
                                 "WHERE (@fromDate IS NULL OR created_at >= @fromDate) " &
                                 "AND (@throughDateExclusive IS NULL OR created_at < @throughDateExclusive) " &
                                 "GROUP BY DATE(created_at)"
                Using cmd As New MySqlCommand(salesSql, conn)
                    cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), DBNull.Value))
                    Using r = cmd.ExecuteReader()
                        While r.Read()
                            Dim d = Convert.ToDateTime(r("SaleDay")).Date
                            byDate(d) = {
                                Convert.ToDecimal(r("TxCount")),
                                Convert.ToDecimal(r("RetailTotal")),
                                Convert.ToDecimal(r("WholesaleTotal")),
                                Convert.ToDecimal(r("DiscountTotal")),
                                Convert.ToDecimal(r("GrossTotal")),
                                0D
                            }
                        End While
                    End Using
                End Using

                Const expSql = "SELECT expense_date AS ExpDay, COALESCE(SUM(amount), 0) AS ExpTotal " &
                               "FROM pharmacy_expenses " &
                               "WHERE (@fromDate IS NULL OR expense_date >= DATE(@fromDate)) " &
                               "AND (@throughDateExclusive IS NULL OR expense_date < DATE(@throughDateExclusive)) " &
                               "GROUP BY expense_date"
                Using cmd As New MySqlCommand(expSql, conn)
                    cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), DBNull.Value))
                    Using r = cmd.ExecuteReader()
                        While r.Read()
                            Dim d = Convert.ToDateTime(r("ExpDay")).Date
                            If Not byDate.ContainsKey(d) Then
                                byDate(d) = {0D, 0D, 0D, 0D, 0D, 0D}
                            End If
                            byDate(d)(5) = Convert.ToDecimal(r("ExpTotal"))
                        End While
                    End Using
                End Using
            End Using

            For Each kvp In byDate.Reverse()
                Dim vals = kvp.Value
                Dim gross = vals(4)
                Dim exp = vals(5)
                dt.Rows.Add(kvp.Key, CInt(vals(0)), vals(1), vals(2), vals(3), gross, exp, gross - exp)
            Next
        Catch ex As Exception
            MessageBox.Show("Failed to load financial report: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function GetEmployeeSalesReport(Optional fromDate As DateTime? = Nothing, Optional throughDateExclusive As DateTime? = Nothing) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT " &
                              "u.full_name AS 'Employee Name', " &
                              "u.role AS 'Role', " &
                              "COUNT(ps.sale_id) AS 'Total Transactions', " &
                              "SUM(ps.total_amount) AS 'Total Sales Revenue' " &
                              "FROM users u " &
                              "INNER JOIN pharmacy_sales ps ON u.user_id = ps.cashier_user_id " &
                              "WHERE (@fromDate IS NULL OR ps.created_at >= @fromDate) " &
                              "AND (@throughDateExclusive IS NULL OR ps.created_at < @throughDateExclusive) " &
                              "GROUP BY u.user_id, u.full_name, u.role " &
                              "ORDER BY SUM(ps.total_amount) DESC"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), DBNull.Value))
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Failed to load employee sales: " & ex.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return dt
    End Function

    Public Function GetExpenseReport(Optional fromDate As DateTime? = Nothing, Optional throughDateExclusive As DateTime? = Nothing) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT e.expense_date AS 'Date', e.expense_category AS 'Category', e.description AS 'Description', e.amount AS 'Amount', u.full_name AS 'Recorded By' " &
                              "FROM pharmacy_expenses e " &
                              "INNER JOIN users u ON e.recorded_by = u.user_id " &
                              "WHERE (@fromDate IS NULL OR e.expense_date >= DATE(@fromDate)) " &
                              "AND (@throughDateExclusive IS NULL OR e.expense_date < DATE(@throughDateExclusive)) " &
                              "ORDER BY e.expense_date DESC, e.expense_id DESC"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@fromDate", If(fromDate.HasValue, CType(fromDate.Value, Object), DBNull.Value))
                    cmd.Parameters.AddWithValue("@throughDateExclusive", If(throughDateExclusive.HasValue, CType(throughDateExclusive.Value, Object), DBNull.Value))
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Handled in UI
        End Try
        Return dt
    End Function

    Public Sub AddExpense(category As String, description As String, amount As Decimal, Optional expenseDate As DateTime? = Nothing)
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "INSERT INTO pharmacy_expenses (expense_category, description, amount, expense_date, recorded_by) " &
                              "VALUES (@category, @desc, @amount, @expDate, @userId)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@category", category)
                    cmd.Parameters.AddWithValue("@desc", If(String.IsNullOrWhiteSpace(description), DBNull.Value, description))
                    cmd.Parameters.AddWithValue("@amount", amount)
                    cmd.Parameters.AddWithValue("@expDate", If(expenseDate.HasValue, expenseDate.Value.Date, Date.Today))
                    cmd.Parameters.AddWithValue("@userId", If(CurrentUserID > 0, CurrentUserID, 3))
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Database error while saving expense: " & ex.Message)
        End Try
    End Sub

    Public Function GetTodayTotalExpenses() As Decimal
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT COALESCE(SUM(amount), 0) FROM pharmacy_expenses WHERE expense_date = CURDATE()"
                Using cmd As New MySqlCommand(query, conn)
                    Return Convert.ToDecimal(cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Return 0D
        End Try
    End Function

    Public Function GetSyncSignature() As String
        Try
            Using conn = GetConnection()
                conn.Open()
                Const sql = "SELECT CONCAT_WS('|', " &
                            "(SELECT COALESCE(MAX(sale_id), 0) FROM pharmacy_sales), " &
                            "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch), " &
                            "(SELECT COALESCE(MAX(batch_id), 0) FROM inventory_batch), " &
                            "(SELECT COALESCE(MAX(expense_id), 0) FROM pharmacy_expenses), " &
                            "(SELECT COUNT(*) FROM users WHERE is_active = 1))"
                Using cmd As New MySqlCommand(sql, conn)
                    Return Convert.ToString(cmd.ExecuteScalar())
                End Using
            End Using
        Catch
            Return String.Empty
        End Try
    End Function

End Class