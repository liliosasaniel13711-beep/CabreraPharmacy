Imports MySql.Data.MySqlClient
Imports System.Data

Public Class BatchControlRepository
    Public Const MigrationVersion As String = "V001"

    Public Shared Function IsAuthorizedUser() As Boolean
        Return CurrentUserID > 0 AndAlso
               (String.Equals(CurrentRole, "Admin", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(CurrentRole, "Super Admin", StringComparison.OrdinalIgnoreCase))
    End Function

    Public Shared Function IsMigrationApplied() As Boolean
        Using connection = GetConnection()
            connection.Open()
            Return IsMigrationApplied(connection, Nothing)
        End Using
    End Function

    Public Shared Function IsMigrationApplied(connection As MySqlConnection, transaction As MySqlTransaction) As Boolean
        Const tableQuery = "SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME IN ('pharmacy_feature_migrations', 'pharmacy_sale_item_batches', 'inventory_batch_quarantines', 'inventory_batch_actions')"
        Using command As New MySqlCommand(tableQuery, connection, transaction)
            If Convert.ToInt32(command.ExecuteScalar()) <> 4 Then Return False
        End Using

        Const migrationQuery = "SELECT COUNT(*) FROM pharmacy_feature_migrations WHERE migration_version = @version"
        Using command As New MySqlCommand(migrationQuery, connection, transaction)
            command.Parameters.AddWithValue("@version", MigrationVersion)
            Return Convert.ToInt32(command.ExecuteScalar()) > 0
        End Using
    End Function

    Public Function GetQuarantineRecords(Optional activeOnly As Boolean = False) As DataTable
        EnsureAuthorized()
        EnsureMigrationApplied()
        Dim records As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "SELECT q.quarantine_id AS 'Quarantine ID', p.brand_name AS 'Product', " &
                                   "b.batch_no AS 'Batch Number', b.expiry_date AS 'Expiry Date', " &
                                   "q.quantity AS 'Quantity', q.reason AS 'Reason', q.status AS 'Status', " &
                                   "q.created_at AS 'Quarantined At', COALESCE(created.full_name, created.username) AS 'Responsible User', " &
                                   "q.released_at AS 'Released At', COALESCE(released.full_name, released.username) AS 'Released By', " &
                                   "q.release_reason AS 'Release Reason' " &
                                   "FROM inventory_batch_quarantines q " &
                                   "INNER JOIN inventory_batch b ON b.batch_id = q.batch_id " &
                                   "INNER JOIN product p ON p.product_ID = b.product_ID " &
                                   "INNER JOIN users created ON created.user_id = q.created_by " &
                                   "LEFT JOIN users released ON released.user_id = q.released_by " &
                                   "WHERE (@activeOnly = 0 OR q.status = 'Active') " &
                                   "ORDER BY q.created_at DESC, q.quarantine_id DESC"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@activeOnly", If(activeOnly, 1, 0))
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(records)
                End Using
            End Using
        End Using
        Return records
    End Function

    Public Function GetBatchActions() As DataTable
        EnsureAuthorized()
        EnsureMigrationApplied()
        Dim actions As New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "SELECT a.action_id AS 'Action ID', p.brand_name AS 'Product', " &
                                   "b.batch_no AS 'Batch Number', b.expiry_date AS 'Expiry Date', " &
                                   "a.action_type AS 'Action', a.quantity AS 'Quantity', a.reason AS 'Reason', " &
                                   "a.status AS 'Status', a.requested_at AS 'Requested At', " &
                                   "COALESCE(requester.full_name, requester.username) AS 'Requested By', " &
                                   "a.confirmed_at AS 'Confirmed At', COALESCE(confirmer.full_name, confirmer.username) AS 'Confirmed By', " &
                                   "a.confirmation_note AS 'Confirmation Note', a.cancelled_at AS 'Cancelled At', " &
                                   "COALESCE(canceller.full_name, canceller.username) AS 'Cancelled By', a.cancellation_reason AS 'Cancellation Reason' " &
                                   "FROM inventory_batch_actions a " &
                                   "INNER JOIN inventory_batch b ON b.batch_id = a.batch_id " &
                                   "INNER JOIN product p ON p.product_ID = b.product_ID " &
                                   "INNER JOIN users requester ON requester.user_id = a.requested_by " &
                                   "LEFT JOIN users confirmer ON confirmer.user_id = a.confirmed_by " &
                                   "LEFT JOIN users canceller ON canceller.user_id = a.cancelled_by " &
                                   "ORDER BY a.requested_at DESC, a.action_id DESC"
            Using command As New MySqlCommand(query, connection)
                Using adapter As New MySqlDataAdapter(command)
                    adapter.Fill(actions)
                End Using
            End Using
        End Using
        Return actions
    End Function

    Public Sub CreateQuarantine(batchId As Integer, quantity As Integer, reason As String)
        EnsureAuthorized()
        ValidateQuantityAndReason(quantity, reason)

        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                EnsureMigrationApplied(connection, transaction)
                Dim remaining = LockBatchQuantity(connection, transaction, batchId)
                Dim reserved = GetReservedQuantity(connection, transaction, batchId)
                If quantity > remaining - reserved Then
                    Throw New InvalidOperationException("The requested quarantine exceeds unreserved physical stock.")
                End If

                Const query As String = "INSERT INTO inventory_batch_quarantines (batch_id, quantity, reason, status, created_by) " &
                                       "VALUES (@batchId, @quantity, @reason, 'Active', @userId)"
                Using command As New MySqlCommand(query, connection, transaction)
                    command.Parameters.AddWithValue("@batchId", batchId)
                    command.Parameters.AddWithValue("@quantity", quantity)
                    command.Parameters.AddWithValue("@reason", reason.Trim())
                    command.Parameters.AddWithValue("@userId", CurrentUserID)
                    command.ExecuteNonQuery()
                End Using
                transaction.Commit()
            End Using
        End Using
    End Sub

    Public Sub ReleaseQuarantine(quarantineId As Long, releaseReason As String)
        EnsureAuthorized()
        If String.IsNullOrWhiteSpace(releaseReason) Then Throw New ArgumentException("A release reason is required.", NameOf(releaseReason))
        If releaseReason.Trim().Length > 500 Then Throw New ArgumentException("The release reason cannot exceed 500 characters.", NameOf(releaseReason))

        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                EnsureMigrationApplied(connection, transaction)
                Const batchQuery As String = "SELECT batch_id FROM inventory_batch_quarantines WHERE quarantine_id = @id"
                Dim batchId As Integer
                Using command As New MySqlCommand(batchQuery, connection, transaction)
                    command.Parameters.AddWithValue("@id", quarantineId)
                    Dim result = command.ExecuteScalar()
                    If result Is Nothing OrElse Convert.IsDBNull(result) Then Throw New InvalidOperationException("The quarantine record no longer exists.")
                    batchId = Convert.ToInt32(result)
                End Using
                LockBatchQuantity(connection, transaction, batchId)

                Const releaseQuery As String = "UPDATE inventory_batch_quarantines SET status = 'Released', released_by = @userId, released_at = CURRENT_TIMESTAMP, release_reason = @reason " &
                                              "WHERE quarantine_id = @id AND status = 'Active'"
                Using command As New MySqlCommand(releaseQuery, connection, transaction)
                    command.Parameters.AddWithValue("@userId", CurrentUserID)
                    command.Parameters.AddWithValue("@reason", releaseReason.Trim())
                    command.Parameters.AddWithValue("@id", quarantineId)
                    If command.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("This quarantine record is no longer active.")
                End Using
                transaction.Commit()
            End Using
        End Using
    End Sub

    Public Sub RequestBatchAction(batchId As Integer, actionType As String, quantity As Integer, reason As String)
        EnsureAuthorized()
        ValidateQuantityAndReason(quantity, reason)
        If Not String.Equals(actionType, "Supplier Return", StringComparison.Ordinal) AndAlso
           Not String.Equals(actionType, "Disposal", StringComparison.Ordinal) Then
            Throw New ArgumentException("The batch action type is not supported.", NameOf(actionType))
        End If

        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                EnsureMigrationApplied(connection, transaction)
                Dim remaining = LockBatchQuantity(connection, transaction, batchId)
                Dim reserved = GetReservedQuantity(connection, transaction, batchId)
                If quantity > remaining - reserved Then
                    Throw New InvalidOperationException("The requested action exceeds stock not already quarantined or reserved by a pending action.")
                End If

                Const query As String = "INSERT INTO inventory_batch_actions (batch_id, action_type, quantity, reason, status, requested_by) " &
                                       "VALUES (@batchId, @actionType, @quantity, @reason, 'Pending', @userId)"
                Using command As New MySqlCommand(query, connection, transaction)
                    command.Parameters.AddWithValue("@batchId", batchId)
                    command.Parameters.AddWithValue("@actionType", actionType)
                    command.Parameters.AddWithValue("@quantity", quantity)
                    command.Parameters.AddWithValue("@reason", reason.Trim())
                    command.Parameters.AddWithValue("@userId", CurrentUserID)
                    command.ExecuteNonQuery()
                End Using
                transaction.Commit()
            End Using
        End Using
    End Sub

    Public Sub ConfirmBatchAction(actionId As Long, confirmationNote As String)
        EnsureAuthorized()
        If confirmationNote IsNot Nothing AndAlso confirmationNote.Trim().Length > 500 Then
            Throw New ArgumentException("The confirmation note cannot exceed 500 characters.", NameOf(confirmationNote))
        End If

        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                EnsureMigrationApplied(connection, transaction)
                Dim batchId As Integer
                Const batchQuery As String = "SELECT batch_id FROM inventory_batch_actions WHERE action_id = @id"
                Using command As New MySqlCommand(batchQuery, connection, transaction)
                    command.Parameters.AddWithValue("@id", actionId)
                    Dim result = command.ExecuteScalar()
                    If result Is Nothing OrElse Convert.IsDBNull(result) Then Throw New InvalidOperationException("The batch action no longer exists.")
                    batchId = Convert.ToInt32(result)
                End Using

                Dim remaining = LockBatchQuantity(connection, transaction, batchId)
                Dim actionQuantity As Integer
                Const actionQuery As String = "SELECT quantity, status FROM inventory_batch_actions WHERE action_id = @id FOR UPDATE"
                Using command As New MySqlCommand(actionQuery, connection, transaction)
                    command.Parameters.AddWithValue("@id", actionId)
                    Using reader = command.ExecuteReader()
                        If Not reader.Read() Then Throw New InvalidOperationException("The batch action no longer exists.")
                        If Not String.Equals(reader.GetString("status"), "Pending", StringComparison.OrdinalIgnoreCase) Then
                            Throw New InvalidOperationException("Only pending batch actions can be confirmed.")
                        End If
                        actionQuantity = reader.GetInt32("quantity")
                    End Using
                End Using
                If actionQuantity < 1 OrElse remaining < actionQuantity Then
                    Throw New InvalidOperationException("The remaining batch quantity no longer covers this action.")
                End If

                Const updateBatch As String = "UPDATE inventory_batch SET quantity_remaining = quantity_remaining - @quantity " &
                                              "WHERE batch_id = @batchId AND quantity_remaining >= @quantity"
                Using command As New MySqlCommand(updateBatch, connection, transaction)
                    command.Parameters.AddWithValue("@quantity", actionQuantity)
                    command.Parameters.AddWithValue("@batchId", batchId)
                    If command.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("The batch quantity changed; refresh and try again.")
                End Using

                Const updateAction As String = "UPDATE inventory_batch_actions SET status = 'Confirmed', confirmed_by = @userId, confirmed_at = CURRENT_TIMESTAMP, confirmation_note = @note " &
                                              "WHERE action_id = @id AND status = 'Pending'"
                Using command As New MySqlCommand(updateAction, connection, transaction)
                    command.Parameters.AddWithValue("@userId", CurrentUserID)
                    command.Parameters.AddWithValue("@note", If(String.IsNullOrWhiteSpace(confirmationNote), CType(DBNull.Value, Object), confirmationNote.Trim()))
                    command.Parameters.AddWithValue("@id", actionId)
                    If command.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("The action status changed; refresh and try again.")
                End Using
                SynchronizeProductStock(connection, transaction, batchId)
                transaction.Commit()
            End Using
        End Using
    End Sub

    Public Sub CancelBatchAction(actionId As Long, cancellationReason As String)
        EnsureAuthorized()
        If String.IsNullOrWhiteSpace(cancellationReason) Then Throw New ArgumentException("A cancellation reason is required.", NameOf(cancellationReason))
        If cancellationReason.Trim().Length > 500 Then Throw New ArgumentException("The cancellation reason cannot exceed 500 characters.", NameOf(cancellationReason))

        Using connection = GetConnection()
            connection.Open()
            Using transaction = connection.BeginTransaction()
                EnsureMigrationApplied(connection, transaction)
                Dim batchId As Integer
                Const batchQuery As String = "SELECT batch_id FROM inventory_batch_actions WHERE action_id = @id"
                Using command As New MySqlCommand(batchQuery, connection, transaction)
                    command.Parameters.AddWithValue("@id", actionId)
                    Dim result = command.ExecuteScalar()
                    If result Is Nothing OrElse Convert.IsDBNull(result) Then Throw New InvalidOperationException("The batch action no longer exists.")
                    batchId = Convert.ToInt32(result)
                End Using
                LockBatchQuantity(connection, transaction, batchId)

                Const query As String = "UPDATE inventory_batch_actions SET status = 'Cancelled', cancelled_by = @userId, cancelled_at = CURRENT_TIMESTAMP, cancellation_reason = @reason " &
                                        "WHERE action_id = @id AND status = 'Pending'"
                Using command As New MySqlCommand(query, connection, transaction)
                    command.Parameters.AddWithValue("@userId", CurrentUserID)
                    command.Parameters.AddWithValue("@reason", cancellationReason.Trim())
                    command.Parameters.AddWithValue("@id", actionId)
                    If command.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("Only pending batch actions can be cancelled.")
                End Using
                transaction.Commit()
            End Using
        End Using
    End Sub

    Private Shared Sub EnsureAuthorized()
        If Not IsAuthorizedUser() Then Throw New UnauthorizedAccessException("Only Admin and Super Admin users can manage batch controls.")
    End Sub

    Private Shared Sub EnsureMigrationApplied()
        If Not IsMigrationApplied() Then Throw New InvalidOperationException("Batch controls are unavailable until migration V001 has been applied.")
    End Sub

    Private Shared Sub EnsureMigrationApplied(connection As MySqlConnection, transaction As MySqlTransaction)
        If Not IsMigrationApplied(connection, transaction) Then Throw New InvalidOperationException("Batch controls are unavailable until migration V001 has been applied.")
    End Sub

    Private Shared Sub ValidateQuantityAndReason(quantity As Integer, reason As String)
        If quantity < 1 Then Throw New ArgumentOutOfRangeException(NameOf(quantity), "Quantity must be greater than zero.")
        If String.IsNullOrWhiteSpace(reason) Then Throw New ArgumentException("A reason is required.", NameOf(reason))
        If reason.Trim().Length > 500 Then Throw New ArgumentException("The reason cannot exceed 500 characters.", NameOf(reason))
    End Sub

    Private Shared Function LockBatchQuantity(connection As MySqlConnection, transaction As MySqlTransaction, batchId As Integer) As Integer
        Const query As String = "SELECT quantity_remaining FROM inventory_batch WHERE batch_id = @batchId FOR UPDATE"
        Using command As New MySqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@batchId", batchId)
            Dim result = command.ExecuteScalar()
            If result Is Nothing OrElse Convert.IsDBNull(result) Then Throw New InvalidOperationException("The selected batch no longer exists.")
            Return Convert.ToInt32(result)
        End Using
    End Function

    Private Shared Function GetReservedQuantity(connection As MySqlConnection, transaction As MySqlTransaction, batchId As Integer) As Integer
        Const query As String = "SELECT " &
                                "COALESCE((SELECT SUM(quantity) FROM inventory_batch_quarantines WHERE batch_id = @batchId AND status = 'Active'), 0) + " &
                                "COALESCE((SELECT SUM(quantity) FROM inventory_batch_actions WHERE batch_id = @batchId AND status = 'Pending'), 0)"
        Using command As New MySqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@batchId", batchId)
            Return Convert.ToInt32(command.ExecuteScalar())
        End Using
    End Function

    Private Shared Sub SynchronizeProductStock(connection As MySqlConnection, transaction As MySqlTransaction, batchId As Integer)
        Const query As String = "UPDATE product SET stock_quantity = " &
                                "(SELECT COALESCE(SUM(quantity_remaining), 0) FROM inventory_batch WHERE product_ID = " &
                                "(SELECT product_ID FROM inventory_batch WHERE batch_id = @batchId)) " &
                                "WHERE product_ID = (SELECT product_ID FROM inventory_batch WHERE batch_id = @batchId)"
        Using command As New MySqlCommand(query, connection, transaction)
            command.Parameters.AddWithValue("@batchId", batchId)
            If command.ExecuteNonQuery() <> 1 Then Throw New InvalidOperationException("Could not synchronize the physical product stock total.")
        End Using
    End Sub
End Class
