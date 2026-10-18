Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ProductManager

    Public Sub AddProduct(categoryId As Integer, supplierId As Integer, brandName As String, genericName As String, cprNumber As String, qtyPerBox As Integer, reorderPoint As Integer, retailPrice As Decimal, wholesalePrice As Decimal, dosageStrength As String, unitOfMeasure As String, manufacturerName As String)
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim query As String = "INSERT INTO product (category_ID, supplier_ID, brand_name, generic_name, CPR_number, quantity_per_box, reorder_point, retail_price, wholesale_price_per_box, dosage_strength, unit_of_measure, manufacturer_name) " &
                                      "VALUES (@cat, @sup, @brand, @gen, @cpr, @qty, @reorder, @ret, @whole, @dos, @unit, @mfg)"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@cat", categoryId)
                    cmd.Parameters.AddWithValue("@sup", supplierId)
                    cmd.Parameters.AddWithValue("@brand", brandName)
                    cmd.Parameters.AddWithValue("@gen", genericName)
                    cmd.Parameters.AddWithValue("@cpr", cprNumber)
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
                Dim query As String = "SELECT p.product_ID AS 'ID', " &
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
                If Not String.IsNullOrEmpty(searchKeyword) Then conditions.Add("(p.brand_name LIKE @search OR p.generic_name LIKE @search OR p.CPR_number LIKE @search)")
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
        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                Try
                    Const checkQuery As String = "SELECT product_ID, expiry_date FROM inventory_batch WHERE batch_id = @batchId FOR UPDATE"
                    Dim productId As Integer
                    Using command As New MySqlCommand(checkQuery, connection, transaction)
                        command.Parameters.AddWithValue("@batchId", batchId)
                        Using reader = command.ExecuteReader()
                            If Not reader.Read() Then Return False
                            productId = reader.GetInt32("product_ID")
                            If reader.GetDateTime("expiry_date").Date >= Date.Today Then
                                Throw New InvalidOperationException("Only batches that are already expired can be removed here.")
                            End If
                        End Using
                    End Using

                    Const deleteQuery As String = "DELETE FROM inventory_batch WHERE batch_id = @batchId"
                    Using command As New MySqlCommand(deleteQuery, connection, transaction)
                        command.Parameters.AddWithValue("@batchId", batchId)
                        If command.ExecuteNonQuery() <> 1 Then Return False
                    End Using

                    Const updateStock As String = "UPDATE product SET stock_quantity = " &
                        "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch WHERE product_ID = @productId) " &
                        "WHERE product_ID = @productId"
                    Using command As New MySqlCommand(updateStock, connection, transaction)
                        command.Parameters.AddWithValue("@productId", productId)
                        command.ExecuteNonQuery()
                    End Using
                    transaction.Commit()
                    Return True
                Catch
                    Try
                        transaction.Rollback()
                    Catch
                    End Try
                    Throw
                End Try
            End Using
        End Using
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
