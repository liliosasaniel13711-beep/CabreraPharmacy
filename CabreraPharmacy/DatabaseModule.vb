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

End Module
