Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ProductManager

    Public Sub AddProduct(categoryId As Integer, supplierId As Integer, brandName As String, genericName As String, cprNumber As String, qtyPerBox As Integer, reorderPoint As Integer, retailPrice As Decimal, wholesalePrice As Decimal, dosageStrength As String, unitOfMeasure As String, manufacturerName As String)
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim query As String = "INSERT INTO product (category_ID, supplier_ID, brand_name, generic_name, CPR_number, quantity_per_box, reorder_point, retail_price, wholesale_price, dosage_strength, unit_of_measure, manufacturer_name) " &
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

    Public Function GetProducts(Optional searchKeyword As String = "") As DataTable
        Dim dt As New DataTable()
        Try
            Using conn = GetConnection()
                conn.Open()
                Dim query As String = "SELECT p.product_ID AS 'ID', " &
                                  "c.category_name AS 'Category', " &
                                  "s.company_name AS 'Supplier', " &
                                  "p.brand_name AS 'Brand Name', " &
                                  "p.generic_name AS 'Generic Name', " &
                                  "p.CPR_number AS 'CPR Number', " &
                                  "p.quantity_per_box AS 'Qty/Box', " &
                                  "p.reorder_point AS 'Reorder Point', " &
                                  "p.retail_price AS 'Retail Price', " &
                                  "p.wholesale_price AS 'Wholesale Price', " &
                                  "p.dosage_strength AS 'Dosage', " &
                                  "p.unit_of_measure AS 'Unit', " &
                                  "p.manufacturer_name AS 'Manufacturer', " &
                                  "p.is_active AS 'Active', " &
                                  "p.date_added AS 'Date Added' " &
                                  "FROM product p " &
                                  "INNER JOIN product_category c ON p.category_ID = c.category_ID " &
                                  "INNER JOIN supplier s ON p.supplier_ID = s.supplier_ID"

                If Not String.IsNullOrEmpty(searchKeyword) Then
                    query &= " WHERE p.brand_name LIKE @search OR p.generic_name LIKE @search OR p.CPR_number LIKE @search"
                End If

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
                Dim query As String = "UPDATE product SET retail_price = @ret, wholesale_price = @whole WHERE product_ID = @id"

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

End Class