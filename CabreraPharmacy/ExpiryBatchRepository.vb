Imports MySql.Data.MySqlClient
Imports System.Data

Public Class ExpiryBatchRepository

    Public Function GetExpiryBatches(nearExpiryDays As Integer, Optional nearExpiryOnly As Boolean = False) As DataTable
        If nearExpiryDays < 1 OrElse nearExpiryDays > 3650 Then
            Throw New ArgumentOutOfRangeException(NameOf(nearExpiryDays), "The near-expiry threshold must be between 1 and 3650 days.")
        End If

        Dim batches As New DataTable()
        Dim saleableQuantityExpression = "CASE WHEN p.is_active = 1 AND ib.expiry_date > CURDATE() THEN ib.quantity_remaining ELSE 0 END"
        Dim isNearExpiryExpression = "ib.expiry_date > CURDATE() AND DATEDIFF(ib.expiry_date, CURDATE()) <= @days"
        Dim statusExpression = "CASE WHEN ib.expiry_date <= CURDATE() THEN 'Expired' " &
                               "WHEN p.is_active = 0 THEN 'Product Inactive' " &
                               "WHEN " & isNearExpiryExpression & " THEN 'Near Expiry' ELSE 'In Stock' END"
        If BatchControlRepository.IsMigrationApplied() Then
            Dim quarantineQuantity = "COALESCE((SELECT SUM(q.quantity) FROM inventory_batch_quarantines q WHERE q.batch_id = ib.batch_id AND q.status = 'Active'), 0)"
            Dim pendingQuantity = "COALESCE((SELECT SUM(a.quantity) FROM inventory_batch_actions a WHERE a.batch_id = ib.batch_id AND a.status = 'Pending'), 0)"
            Dim reservedQuantity = "(" & quarantineQuantity & " + " & pendingQuantity & ")"
            Dim eligibleQuantity = "GREATEST(0, ib.quantity_remaining - " & reservedQuantity & ")"
            saleableQuantityExpression = "CASE WHEN p.is_active = 1 AND ib.expiry_date > CURDATE() THEN " & eligibleQuantity & " ELSE 0 END"
            statusExpression = "CASE WHEN ib.expiry_date <= CURDATE() THEN 'Expired' " &
                               "WHEN p.is_active = 0 THEN 'Product Inactive' " &
                               "WHEN " & eligibleQuantity & " <= 0 AND " & quarantineQuantity & " > 0 THEN 'Quarantined' " &
                               "WHEN " & eligibleQuantity & " <= 0 THEN 'Action Pending' " &
                               "WHEN " & reservedQuantity & " > 0 AND " & isNearExpiryExpression & " THEN 'Near Expiry - Partially Reserved' " &
                               "WHEN " & reservedQuantity & " > 0 THEN 'In Stock - Partially Reserved' " &
                               "WHEN " & isNearExpiryExpression & " THEN 'Near Expiry' ELSE 'In Stock' END"
        End If

        Using connection = GetConnection()
            connection.Open()
            Dim query As String = "SELECT p.product_ID AS 'Product ID', " &
                                  "ib.batch_id AS 'Batch ID', " &
                                  "p.brand_name AS 'Product Name', " &
                                  "ib.batch_no AS 'Batch Number', " &
                                  "ib.expiry_date AS 'Expiry Date', " &
                                  "CASE WHEN ib.expiry_date > CURDATE() THEN DATEDIFF(ib.expiry_date, CURDATE()) ELSE NULL END AS 'Days Remaining', " &
                                  "CASE WHEN ib.expiry_date <= CURDATE() THEN DATEDIFF(CURDATE(), ib.expiry_date) ELSE NULL END AS 'Days Past Expiry', " &
                                  "ib.quantity_remaining AS 'Physical Quantity', " &
                                  saleableQuantityExpression & " AS 'Saleable Quantity', " &
                                  "COALESCE(batchSupplier.company_name, productSupplier.company_name, 'Supplier unavailable') AS 'Supplier', " &
                                  statusExpression & " AS 'Status' " &
                                  "FROM inventory_batch ib " &
                                  "INNER JOIN product p ON p.product_ID = ib.product_ID " &
                                  "LEFT JOIN supplier batchSupplier ON batchSupplier.supplier_ID = ib.supplier_ID " &
                                  "LEFT JOIN supplier productSupplier ON productSupplier.supplier_ID = p.supplier_ID " &
                                  "WHERE ib.quantity_remaining > 0 AND (@nearExpiryOnly = 0 OR ib.expiry_date <= CURDATE() OR " & isNearExpiryExpression & ") " &
                                  "ORDER BY ib.expiry_date, p.brand_name, ib.batch_id"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@days", nearExpiryDays)
                command.Parameters.AddWithValue("@nearExpiryOnly", If(nearExpiryOnly, 1, 0))
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(batches)
                End Using
            End Using
        End Using
        Return batches
    End Function

End Class
