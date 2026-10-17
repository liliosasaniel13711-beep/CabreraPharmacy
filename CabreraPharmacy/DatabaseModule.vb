Imports MySql.Data.MySqlClient

Module DatabaseModule

    Public ConnectionString As String =
       "Server=localhost;Database=cabreradb;Uid=root;Pwd=;Allow Zero Datetime=True;Convert Zero Datetime=True;"

    Public Function GetConnection() As MySqlConnection
        Return New MySqlConnection(ConnectionString)
    End Function

    Public Sub EnsureDatabase()
        Using connection = GetConnection()
            connection.Open()

            Const createTable = "CREATE TABLE IF NOT EXISTS users (user_id INT AUTO_INCREMENT PRIMARY KEY, username VARCHAR(50) NOT NULL UNIQUE, password_hash VARCHAR(255) NOT NULL, full_name VARCHAR(100) NOT NULL, role VARCHAR(20) NOT NULL, is_active BOOLEAN NOT NULL DEFAULT TRUE, created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)"

            Using command As New MySqlCommand(createTable, connection)
                command.ExecuteNonQuery()
            End Using

            Const createActivityTable = "CREATE TABLE IF NOT EXISTS user_activity (activity_id BIGINT AUTO_INCREMENT PRIMARY KEY, user_id INT NOT NULL, username VARCHAR(50) NOT NULL, full_name VARCHAR(100) NOT NULL, role VARCHAR(20) NOT NULL, activity VARCHAR(100) NOT NULL, created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, INDEX idx_user_activity_user_time (user_id, created_at))"
            Using command As New MySqlCommand(createActivityTable, connection)
                command.ExecuteNonQuery()
            End Using

            Const createSalesTable = "CREATE TABLE IF NOT EXISTS pharmacy_sales (sale_id BIGINT AUTO_INCREMENT PRIMARY KEY, sale_mode VARCHAR(12) NOT NULL, cashier_user_id INT NOT NULL, customer_type VARCHAR(30) NOT NULL, customer_name VARCHAR(100) NULL, customer_id_number VARCHAR(50) NULL, subtotal DECIMAL(12,2) NOT NULL, discount_amount DECIMAL(12,2) NOT NULL, total_amount DECIMAL(12,2) NOT NULL, amount_received DECIMAL(12,2) NOT NULL, change_amount DECIMAL(12,2) NOT NULL, prescription_no VARCHAR(50) NULL, prescribing_practioner VARCHAR(100) NULL, prc_liscense VARCHAR(50) NULL, prescription_date DATE NULL, created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, INDEX idx_pharmacy_sales_date (created_at), INDEX idx_pharmacy_sales_mode (sale_mode), INDEX idx_pharmacy_sales_cashier (cashier_user_id, created_at)) ENGINE=InnoDB"
            Using command As New MySqlCommand(createSalesTable, connection)
                command.ExecuteNonQuery()
            End Using

            DropSalesSnapshotColumnIfExists(connection, "cashier_username")
            DropSalesSnapshotColumnIfExists(connection, "cashier_full_name")
            RenameSalesColumnIfNeeded(connection, "prescription_number", "prescription_no", "VARCHAR(50) NULL")
            RenameSalesColumnIfNeeded(connection, "prescriber_name", "prescribing_practioner", "VARCHAR(100) NULL")
            RenameSalesColumnIfNeeded(connection, "prescriber_license", "prc_liscense", "VARCHAR(50) NULL")

            Const createSaleItemsTable = "CREATE TABLE IF NOT EXISTS pharmacy_sale_items (sale_item_id BIGINT AUTO_INCREMENT PRIMARY KEY, sale_id BIGINT NOT NULL, product_id INT NOT NULL, brand_name VARCHAR(150) NOT NULL, generic_name VARCHAR(150) NOT NULL, dosage VARCHAR(100) NULL, unit VARCHAR(50) NULL, quantity INT NOT NULL, unit_price DECIMAL(12,2) NOT NULL, line_total DECIMAL(12,2) NOT NULL, is_prescription BOOLEAN NOT NULL DEFAULT FALSE, INDEX idx_pharmacy_sale_items_sale (sale_id), INDEX idx_pharmacy_sale_items_product (product_id), CONSTRAINT fk_pharmacy_sale_items_sale FOREIGN KEY (sale_id) REFERENCES pharmacy_sales(sale_id) ON DELETE CASCADE) ENGINE=InnoDB"
            Using command As New MySqlCommand(createSaleItemsTable, connection)
                command.ExecuteNonQuery()
            End Using

            AddColumnIfMissing(connection, "full_name", "VARCHAR(100) NOT NULL DEFAULT ''")
            AddColumnIfMissing(connection, "first_name", "VARCHAR(50) NULL")
            AddColumnIfMissing(connection, "last_name", "VARCHAR(50) NULL")
            AddColumnIfMissing(connection, "staff_initials", "VARCHAR(5) NULL")
            AddColumnIfMissing(connection, "role", "VARCHAR(20) NOT NULL DEFAULT 'Staff'")
            AddColumnIfMissing(connection, "password_hash", "VARCHAR(255) NOT NULL DEFAULT ''")
            AddColumnIfMissing(connection, "is_active", "BOOLEAN NOT NULL DEFAULT TRUE")
            AddColumnIfMissing(connection, "created_at", "DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP")

            Using nameCommand As New MySqlCommand(
                "UPDATE users SET full_name = username WHERE full_name = '' OR full_name IS NULL",
                connection)
                nameCommand.ExecuteNonQuery()
            End Using

            Using countCommand As New MySqlCommand("SELECT COUNT(*) FROM users", connection)
                If Convert.ToInt32(countCommand.ExecuteScalar()) = 0 Then
                    Using insertCommand As New MySqlCommand(
                        "INSERT INTO users (username, full_name, role, password_hash, is_active) " &
                        "VALUES (@username, @fullName, 'Super Admin', @passwordHash, TRUE)",
                        connection)

                        insertCommand.Parameters.AddWithValue("@username", "superadmin")
                        insertCommand.Parameters.AddWithValue("@fullName", "System Super Administrator")
                        insertCommand.Parameters.AddWithValue("@passwordHash", HashPassword("ChangeMe123!"))
                        insertCommand.ExecuteNonQuery()
                    End Using
                End If
            End Using

            'The existing database defines the role as ENUM('Staff','Admin','Super Admin').
            'Repair the initial account created by an earlier version that used SuperAdmin.
            Using repairRoleCommand As New MySqlCommand(
                "UPDATE users SET role = 'Super Admin' WHERE username = 'superadmin' AND (role = '' OR role IS NULL)",
                connection)
                repairRoleCommand.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub RecordUserActivity(activity As String)
        If CurrentUserID <= 0 Then Return

        Using connection = GetConnection()
            connection.Open()

            Const sql = "INSERT INTO user_activity (user_id, username, full_name, role, activity) " &
                        "VALUES (@userId, @username, @fullName, @role, @activity)"
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@userId", CurrentUserID)
                command.Parameters.AddWithValue("@username", If(CurrentUsername, String.Empty))
                command.Parameters.AddWithValue("@fullName", If(CurrentFullName, String.Empty))
                command.Parameters.AddWithValue("@role", If(CurrentRole, String.Empty))
                command.Parameters.AddWithValue("@activity", activity)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub AddColumnIfMissing(connection As MySqlConnection,
                                   columnName As String,
                                   definition As String)

        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS " &
                            "WHERE TABLE_SCHEMA = DATABASE() " &
                            "AND TABLE_NAME = 'users' " &
                            "AND COLUMN_NAME = @columnName"

        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", columnName)

            If Convert.ToInt32(existsCommand.ExecuteScalar()) > 0 Then
                Return
            End If
        End Using

        Using alterCommand As New MySqlCommand(
            "ALTER TABLE users ADD COLUMN " & columnName & " " & definition,
            connection)

            alterCommand.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub DropSalesSnapshotColumnIfExists(connection As MySqlConnection, columnName As String)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS " &
                            "WHERE TABLE_SCHEMA = DATABASE() " &
                            "AND TABLE_NAME = 'pharmacy_sales' " &
                            "AND COLUMN_NAME = @columnName"
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", columnName)
            If Convert.ToInt32(existsCommand.ExecuteScalar()) = 0 Then Return
        End Using

        Using alterCommand As New MySqlCommand("ALTER TABLE pharmacy_sales DROP COLUMN " & columnName, connection)
            alterCommand.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub RenameSalesColumnIfNeeded(connection As MySqlConnection,
                                          oldColumnName As String,
                                          newColumnName As String,
                                          definition As String)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS " &
                            "WHERE TABLE_SCHEMA = DATABASE() " &
                            "AND TABLE_NAME = 'pharmacy_sales' " &
                            "AND COLUMN_NAME = @columnName"

        Dim oldExists As Boolean
        Dim newExists As Boolean
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", oldColumnName)
            oldExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
            existsCommand.Parameters("@columnName").Value = newColumnName
            newExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
        End Using

        If oldExists AndAlso Not newExists Then
            Using alterCommand As New MySqlCommand(
                "ALTER TABLE pharmacy_sales CHANGE COLUMN " & oldColumnName & " " & newColumnName & " " & definition,
                connection)
                alterCommand.ExecuteNonQuery()
            End Using
        End If
    End Sub

End Module
