Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ProductManager

    Private Shared itemPlacementColumnEnsured As Boolean = False

    Private Shared Sub EnsureItemPlacementColumn(conn As MySqlConnection)
        If itemPlacementColumnEnsured Then Return
        Try
            Using cmd As New MySqlCommand("ALTER TABLE product ADD COLUMN IF NOT EXISTS item_placement VARCHAR(100) DEFAULT 'Unassigned' AFTER CPR_number;", conn)
                cmd.ExecuteNonQuery()
            End Using
            itemPlacementColumnEnsured = True
        Catch
            Try
                Using cmd As New MySqlCommand("ALTER TABLE product ADD COLUMN item_placement VARCHAR(100) DEFAULT 'Unassigned' AFTER CPR_number;", conn)
                    cmd.ExecuteNonQuery()
                End Using
            Catch
                ' Column already exists
            End Try
            itemPlacementColumnEnsured = True
        End Try
    End Sub

    Public Sub AddProduct(categoryId As Integer, supplierId As Integer, brandName As String, genericName As String, cprNumber As String, qtyPerBox As Integer, reorderPoint As Integer, retailPrice As Decimal, wholesalePrice As Decimal, dosageStrength As String, unitOfMeasure As String, manufacturerName As String, Optional itemPlacement As String = "Unassigned")
        Try
            Using conn = GetConnection()
                conn.Open()
                EnsureItemPlacementColumn(conn)
                Dim query As String = "INSERT INTO product (category_ID, supplier_ID, brand_name, generic_name, CPR_number, item_placement, quantity_per_box, reorder_point, retail_price, wholesale_price_per_box, dosage_strength, unit_of_measure, manufacturer_name) " &
                                      "VALUES (@cat, @sup, @brand, @gen, @cpr, @placement, @qty, @reorder, @ret, @whole, @dos, @unit, @mfg)"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@cat", categoryId)
                    cmd.Parameters.AddWithValue("@sup", supplierId)
                    cmd.Parameters.AddWithValue("@brand", brandName)
                    cmd.Parameters.AddWithValue("@gen", genericName)
                    cmd.Parameters.AddWithValue("@cpr", cprNumber)
                    cmd.Parameters.AddWithValue("@placement", If(String.IsNullOrWhiteSpace(itemPlacement), "Unassigned", itemPlacement.Trim()))
                    cmd.Parameters.AddWithValue("@qty", qtyPerBox)
                    cmd.Parameters.AddWithValue("@reorder", reorderPoint)
                    cmd.Parameters.AddWithValue("@ret", retailPrice)
                    cmd.Parameters.AddWithValue("@whole", wholesalePrice)
                    cmd.Parameters.AddWithValue("@dos", dosageStrength)
                    cmd.Parameters.AddWithValue("@unit", unitOfMeasure)
                    cmd.Parameters.AddWithValue("@mfg", manufacturerName)

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to add product: " & ex.Message, MsgBoxStyle.Critical, "Create Error")
        End Try
    End Sub

    Public Function GetProducts(Optional searchKeyword As String = "", Optional includeInactive As Boolean = True) As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                EnsureItemPlacementColumn(conn)
                Dim query As String = "SELECT p.product_ID AS 'ID', " &
                                  "COALESCE(NULLIF(p.item_placement, ''), 'Unassigned') AS 'Item Placement', " &
                                  "c.category_name AS 'Category', " &
                                  "parent.category_name AS 'Parent Category', " &
                                  "s.company_name AS 'Supplier', " &
                                  "p.brand_name AS 'Brand Name', " &
                                  "p.generic_name AS 'Generic Name', " &
                                  "p.CPR_number AS 'CPR Number', " &
                                  "p.quantity_per_box AS 'Qty/Box', " &
                                  "COALESCE((SELECT SUM(pb.quantity_remaining) FROM inventory_batch pb WHERE pb.product_ID = p.product_ID), 0) AS 'Stock Quantity', " &
                                  "(SELECT MIN(pb.expiry_date) FROM inventory_batch pb WHERE pb.product_ID = p.product_ID AND pb.quantity_remaining > 0) AS 'Expiry Date', " &
                                  "p.reorder_point AS 'Reorder Point', " &
                                  "p.retail_price AS 'Retail Price', " &
                                  "p.wholesale_price_per_box AS 'Wholesale Price', " &
                                  "p.dosage_strength AS 'Dosage', " &
                                  "p.unit_of_measure AS 'Unit', " &
                                  "p.manufacturer_name AS 'Manufacturer', " &
                                  "p.is_active AS 'Active', " &
                                  "c.prescription_required AS 'Prescription Required', " &
                                  "p.date_added AS 'Date Added' " &
                                  "FROM product p " &
                                  "INNER JOIN product_category c ON p.category_ID = c.category_ID " &
                                  "LEFT JOIN product_category parent ON c.parent_category_id = parent.category_ID " &
                                  "INNER JOIN supplier s ON p.supplier_ID = s.supplier_ID"

                Dim conditions As New List(Of String)
                If Not includeInactive Then conditions.Add("p.is_active = 1")
                If Not String.IsNullOrEmpty(searchKeyword) Then conditions.Add("(p.brand_name LIKE @search OR p.generic_name LIKE @search OR p.CPR_number LIKE @search OR p.item_placement LIKE @search)")
                If conditions.Count > 0 Then query &= " WHERE " & String.Join(" AND ", conditions)

                Using cmd As New MySqlCommand(query, conn)
                    If Not String.IsNullOrEmpty(searchKeyword) Then
                        cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
                    End If

                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to load inventory: " & ex.Message, MsgBoxStyle.Critical, "Read Error")
        End Try
        Return dt
    End Function

    Public Function GetStaffInventorySummary(productId As Integer, nearExpiryDays As Integer) As DataTable
        If productId <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(productId))
        If nearExpiryDays < 1 OrElse nearExpiryDays > 3650 Then Throw New ArgumentOutOfRangeException(NameOf(nearExpiryDays))

        Dim summary As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Dim allocationTrackingEnabled = BatchControlRepository.IsMigrationApplied(connection, Nothing)
            Dim reservedQuantity = "0"
            If allocationTrackingEnabled Then
                reservedQuantity = "COALESCE((SELECT SUM(q.quantity) FROM inventory_batch_quarantines q WHERE q.batch_id = ib.batch_id AND q.status = 'Active'), 0) + " &
                                   "COALESCE((SELECT SUM(a.quantity) FROM inventory_batch_actions a WHERE a.batch_id = ib.batch_id AND a.status = 'Pending'), 0)"
            End If

            Dim eligibleQuantity = "GREATEST(0, ib.quantity_remaining - (" & reservedQuantity & "))"
            Dim saleableTotal = "COALESCE((SELECT SUM(" & eligibleQuantity & ") FROM inventory_batch ib " &
                                 "WHERE ib.product_ID = p.product_ID AND ib.quantity_remaining > 0 AND ib.expiry_date > CURDATE()), 0)"
            Dim expiredTotal = "COALESCE((SELECT SUM(ib.quantity_remaining) FROM inventory_batch ib " &
                                "WHERE ib.product_ID = p.product_ID AND ib.quantity_remaining > 0 AND ib.expiry_date <= CURDATE()), 0)"
            Dim batchEligibleQuantity = If(allocationTrackingEnabled, eligibleQuantity, "ib.quantity_remaining")
            Dim query = "SELECT p.product_ID AS 'Product ID', p.brand_name AS 'Medicine Name', " &
                        "p.reorder_point AS 'Reorder Point', " &
                        "CASE WHEN p.is_active = 1 THEN " & saleableTotal & " ELSE 0 END AS 'Saleable Quantity', " &
                        expiredTotal & " AS 'Expired Quantity', " &
                        "ib.batch_no AS 'Batch Number', ib.expiry_date AS 'Expiry Date', " &
                        "DATEDIFF(ib.expiry_date, CURDATE()) AS 'Days Remaining', " &
                        batchEligibleQuantity & " AS 'Near-Expiry Quantity' " &
                        "FROM product p LEFT JOIN inventory_batch ib ON ib.product_ID = p.product_ID " &
                        "AND ib.quantity_remaining > 0 AND ib.expiry_date > CURDATE() " &
                        "AND DATEDIFF(ib.expiry_date, CURDATE()) <= @days " &
                        "AND " & batchEligibleQuantity & " > 0 " &
                        "WHERE p.product_ID = @productId ORDER BY ib.expiry_date, ib.batch_id"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@days", nearExpiryDays)
                command.Parameters.AddWithValue("@productId", productId)
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(summary)
                End Using
            End Using
        End Using
        Return summary
    End Function

    Public Function GetActiveSuppliers() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection()
            conn.Open()
            Const query = "SELECT supplier_ID, company_name FROM supplier WHERE is_active = 1 ORDER BY company_name"
            Using cmd As New MySqlCommand(query, conn)
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Function GetActiveCategories() As DataTable
        Dim dt As New DataTable()
        Using conn = GetConnection()
            conn.Open()
            Const query = "SELECT c.category_ID, CONCAT(COALESCE(CONCAT(p.category_name, ' - '), ''), c.category_name) AS display_name " &
                          "FROM product_category c " &
                          "LEFT JOIN product_category p ON c.parent_category_id = p.category_ID " &
                          "WHERE c.is_active = 1 AND c.parent_category_id IS NOT NULL " &
                          "ORDER BY display_name"
            Using cmd As New MySqlCommand(query, conn)
                Using da As New MySqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using
        Return dt
    End Function

    Public Sub UpdateItemPlacement(productId As Integer, itemPlacement As String)
        If productId <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(productId))
        Using conn = GetConnection()
            conn.Open()
            EnsureItemPlacementColumn(conn)
            Const query = "UPDATE product SET item_placement = @placement WHERE product_ID = @id"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@placement", If(String.IsNullOrWhiteSpace(itemPlacement), "Unassigned", itemPlacement.Trim()))
                cmd.Parameters.AddWithValue("@id", productId)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub RestockProductBatch(productId As Integer,
                                   batchNo As String,
                                   expiryDate As Date,
                                   quantityAdded As Integer,
                                   supplierId As Integer?,
                                   Optional itemPlacement As String = Nothing)
        If productId <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(productId))
        If String.IsNullOrWhiteSpace(batchNo) Then Throw New ArgumentException("Batch number is required.", NameOf(batchNo))
        If quantityAdded < 1 Then Throw New ArgumentOutOfRangeException(NameOf(quantityAdded), "Restock quantity must be at least 1.")
        If expiryDate.Date <= Date.Today Then Throw New ArgumentException("Expiry date must be in the future.", NameOf(expiryDate))

        Using conn = GetConnection()
            conn.Open()
            EnsureItemPlacementColumn(conn)
            Using tx = conn.BeginTransaction()
                Try
                    Const insertBatch = "INSERT INTO inventory_batch (product_ID, batch_no, expiry_date, quantity_received, quantity_remaining, date_received, supplier_ID) " &
                                        "VALUES (@productId, @batchNo, @expiryDate, @qty, @qty, CURDATE(), @supplierId)"
                    Using cmd As New MySqlCommand(insertBatch, conn, tx)
                        cmd.Parameters.AddWithValue("@productId", productId)
                        cmd.Parameters.AddWithValue("@batchNo", batchNo.Trim().ToUpperInvariant())
                        cmd.Parameters.AddWithValue("@expiryDate", expiryDate.Date)
                        cmd.Parameters.AddWithValue("@qty", quantityAdded)
                        cmd.Parameters.AddWithValue("@supplierId", If(supplierId.HasValue AndAlso supplierId.Value > 0, CType(supplierId.Value, Object), DBNull.Value))
                        cmd.ExecuteNonQuery()
                    End Using

                    Const syncStock = "UPDATE product SET stock_quantity = " &
                                      "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch WHERE product_ID = @productId) " &
                                      "WHERE product_ID = @productId"
                    Using cmd As New MySqlCommand(syncStock, conn, tx)
                        cmd.Parameters.AddWithValue("@productId", productId)
                        cmd.ExecuteNonQuery()
                    End Using

                    If itemPlacement IsNot Nothing Then
                        Const updatePlacement = "UPDATE product SET item_placement = @placement WHERE product_ID = @productId"
                        Using cmd As New MySqlCommand(updatePlacement, conn, tx)
                            cmd.Parameters.AddWithValue("@placement", If(String.IsNullOrWhiteSpace(itemPlacement), "Unassigned", itemPlacement.Trim()))
                            cmd.Parameters.AddWithValue("@productId", productId)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If

                    tx.Commit()
                Catch
                    Try
                        tx.Rollback()
                    Catch
                    End Try
                    Throw
                End Try
            End Using
        End Using
    End Sub

    Public Sub UpdatePrices(productId As Integer, newRetailPrice As Decimal, newWholesalePrice As Decimal)
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim query As String = "UPDATE product SET retail_price = @ret, wholesale_price_per_box = @whole WHERE product_ID = @id"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@ret", newRetailPrice)
                    cmd.Parameters.AddWithValue("@whole", newWholesalePrice)
                    cmd.Parameters.AddWithValue("@id", productId)

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to update prices: " & ex.Message, MsgBoxStyle.Critical, "Update Error")
        End Try
    End Sub

    Public Sub ArchiveProduct(productId As Integer)
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim query As String = "UPDATE product SET is_active = 0 WHERE product_ID = @id"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", productId)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Failed to archive product: " & ex.Message, MsgBoxStyle.Critical, "Delete Error")
        End Try
    End Sub

    Public Function GetProductBatches(productId As Integer) As DataTable
        Dim batches As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "SELECT batch_id AS 'Batch ID', batch_no AS 'Batch No', expiry_date AS 'Expiry Date', quantity_remaining AS 'Remaining' " &
                                    "FROM inventory_batch WHERE product_ID = @id ORDER BY expiry_date, batch_id"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@id", productId)
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(batches)
                End Using
            End Using
        End Using
        Return batches
    End Function

    Public Function RemoveExpiredBatch(batchId As Long) As Boolean
        Throw New InvalidOperationException("Expired batches are retained for inventory history and cannot be permanently removed.")
    End Function

    Public Function VerifyUserPin(userId As Integer, pin As String) As Boolean
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "SELECT pin_code FROM users WHERE user_id = @userId AND is_active = 1 LIMIT 1"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@userId", userId)
                Dim storedValue = command.ExecuteScalar()
                If storedValue Is Nothing OrElse storedValue Is DBNull.Value Then Return False
                Return VerifyPassword(pin, Convert.ToString(storedValue))
            End Using
        End Using
    End Function

    Public Function SetProductActive(productId As Integer, isActive As Boolean) As Boolean
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "UPDATE product SET is_active = @active WHERE product_ID = @id"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@active", If(isActive, 1, 0))
                command.Parameters.AddWithValue("@id", productId)
                Return command.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

End Class