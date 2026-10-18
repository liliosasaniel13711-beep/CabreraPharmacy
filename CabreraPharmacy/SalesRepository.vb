Imports MySql.Data.MySqlClient
Imports System.Data

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

    Public Function SaveSale(sale As SaleRecord) As Long
        If sale Is Nothing Then Throw New ArgumentNullException(NameOf(sale))
        If CurrentUserID <= 0 Then Throw New InvalidOperationException("There is no signed-in cashier for this transaction.")
        If sale.Items Is Nothing OrElse sale.Items.Count = 0 Then Throw New InvalidOperationException("A transaction must contain at least one item.")

        Using connection = GetConnection()
            connection.Open()
            Using dbTransaction = connection.BeginTransaction()
                Try
                    Const insertSale = "INSERT INTO pharmacy_sales (sale_mode, cashier_user_id, customer_type, customer_name, customer_id_number, subtotal, discount_amount, total_amount, amount_received, change_amount, prescription_no, prescribing_practioner, prc_liscense, prescription_date, purchase_order_no, purchase_order_date, payment_method, payment_provider, payment_account_name, payment_reference_number, wholesale_client_id) " &
                                       "VALUES (@saleMode, @cashierId, @customerType, @customerName, @customerId, @subtotal, @discount, @total, @cash, @change, @rxNumber, @prescriber, @prescriberLicense, @rxDate, @poNumber, @poDate, @paymentMethod, @paymentProvider, @paymentAccountName, @paymentReference, @wholesaleClientId)"
                    Dim saleId As Long
                    Using command As New MySqlCommand(insertSale, connection, dbTransaction)
                        command.Parameters.AddWithValue("@saleMode", sale.SaleMode)
                        command.Parameters.AddWithValue("@cashierId", CurrentUserID)
                        command.Parameters.AddWithValue("@customerType", sale.CustomerType)
                        AddNullable(command, "@customerName", sale.CustomerName)
                        AddNullable(command, "@customerId", sale.CustomerIdNumber)
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

                        DeductInventory(connection, dbTransaction, item.ProductId, quantityToDeduct)

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
                        End Using
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

    Private Shared Sub DeductInventory(connection As MySqlConnection, transaction As MySqlTransaction, productId As Integer, quantityToDeduct As Integer)
        If quantityToDeduct < 1 Then Throw New InvalidOperationException("The sale quantity must be greater than zero.")

        Dim batches As New List(Of KeyValuePair(Of Long, Integer))()
        Dim availableQuantity As Integer = 0
        Const selectBatches = "SELECT batch_id, quantity_remaining FROM inventory_batch " &
                              "WHERE product_ID = @productId AND quantity_remaining > 0 AND expiry_date >= CURDATE() " &
                              "ORDER BY expiry_date, batch_id FOR UPDATE"
        Using command As New MySqlCommand(selectBatches, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            Using reader = command.ExecuteReader()
                While reader.Read()
                    Dim batchId = reader.GetInt64("batch_id")
                    Dim batchQuantity = reader.GetInt32("quantity_remaining")
                    batches.Add(New KeyValuePair(Of Long, Integer)(batchId, batchQuantity))
                    availableQuantity += batchQuantity
                End While
            End Using
        End Using

        If availableQuantity < quantityToDeduct Then
            Throw New InvalidOperationException("Not enough unexpired stock is available to complete this sale. Available: " &
                                                availableQuantity.ToString(Globalization.CultureInfo.InvariantCulture) &
                                                "; requested: " & quantityToDeduct.ToString(Globalization.CultureInfo.InvariantCulture) & ".")
        End If

        Dim remainingToDeduct = quantityToDeduct
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
            remainingToDeduct -= quantityFromBatch
        Next

        Const synchronizeProductStock = "UPDATE product SET stock_quantity = " &
                                        "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch WHERE product_ID = @productId) " &
                                        "WHERE product_ID = @productId"
        Using command As New MySqlCommand(synchronizeProductStock, connection, transaction)
            command.Parameters.AddWithValue("@productId", productId)
            If command.ExecuteNonQuery() <> 1 Then
                Throw New InvalidOperationException("Could not synchronize the product stock total.")
            End If
        End Using
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
            Const query = "SELECT ps.sale_id, ps.created_at, ps.sale_mode, ps.customer_type, " &
                          "ps.customer_name, ps.customer_id_number, ps.subtotal, ps.discount_amount, " &
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

End Class
