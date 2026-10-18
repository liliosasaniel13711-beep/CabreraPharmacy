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

            RenameProductBatchesTableIfNeeded(connection)
            DropProductCategoryColumnIfExists(connection, "discount_eligible")
            DropProductCategoryColumnIfExists(connection, "tax_percentage")
            DropProductCategoryColumnIfExists(connection, "storage_condition")
            DropProductCategoryColumnIfExists(connection, "category_type")

            Const createTable = "CREATE TABLE IF NOT EXISTS users (user_id INT AUTO_INCREMENT PRIMARY KEY, username VARCHAR(50) NOT NULL UNIQUE, password_hash VARCHAR(255) NOT NULL, full_name VARCHAR(100) NOT NULL, role VARCHAR(20) NOT NULL, is_active BOOLEAN NOT NULL DEFAULT TRUE)"

            Using command As New MySqlCommand(createTable, connection)
                command.ExecuteNonQuery()
            End Using

            DropUsersColumnIfExists(connection, "created_at")
            DropUsersColumnIfExists(connection, "last_login_date")
            Using command As New MySqlCommand("DROP TABLE IF EXISTS user_activity", connection)
                command.ExecuteNonQuery()
            End Using

            Const createSalesTable = "CREATE TABLE IF NOT EXISTS pharmacy_sales (sale_id BIGINT AUTO_INCREMENT PRIMARY KEY, sale_mode VARCHAR(12) NOT NULL, cashier_user_id INT NOT NULL, customer_type VARCHAR(30) NOT NULL, customer_name VARCHAR(100) NULL, customer_id_number VARCHAR(50) NULL, subtotal DECIMAL(12,2) NOT NULL, discount_amount DECIMAL(12,2) NOT NULL, total_amount DECIMAL(12,2) NOT NULL, amount_received DECIMAL(12,2) NOT NULL, change_amount DECIMAL(12,2) NOT NULL, prescription_no VARCHAR(50) NULL, prescribing_practioner VARCHAR(100) NULL, prc_liscense VARCHAR(50) NULL, prescription_date DATE NULL, created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, INDEX idx_pharmacy_sales_date (created_at), INDEX idx_pharmacy_sales_mode (sale_mode), INDEX idx_pharmacy_sales_cashier (cashier_user_id, created_at)) ENGINE=InnoDB"
            Using command As New MySqlCommand(createSalesTable, connection)
                command.ExecuteNonQuery()
            End Using
            AddPharmacySalesColumnIfMissing(connection, "purchase_order_no", "VARCHAR(40) NULL")
            AddPharmacySalesColumnIfMissing(connection, "purchase_order_date", "DATE NULL")
            AddPharmacySalesColumnIfMissing(connection, "payment_method", "VARCHAR(30) NULL")
            AddPharmacySalesColumnIfMissing(connection, "payment_provider", "VARCHAR(30) NULL")
            AddPharmacySalesColumnIfMissing(connection, "payment_account_name", "VARCHAR(150) NULL")
            AddPharmacySalesColumnIfMissing(connection, "payment_reference_number", "VARCHAR(80) NULL")

            RenameWholesaleCustomersTableIfNeeded(connection)
            MigrateLegacyWholesaleClientsTable(connection)
            Const createWholesaleClientsTable = "CREATE TABLE IF NOT EXISTS wholesale_clients (client_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, company_name VARCHAR(150) NOT NULL, fda_lto_no VARCHAR(50) NOT NULL UNIQUE, license_expiry_date DATE NULL, first_name VARCHAR(100) NOT NULL, last_name VARCHAR(100) NOT NULL DEFAULT '', contact_no_1 VARCHAR(30) NOT NULL, email VARCHAR(150) NULL, shipping_address_line VARCHAR(255) NOT NULL, barangay VARCHAR(100) NULL, city_municipality VARCHAR(100) NULL, province VARCHAR(100) NULL, postal_code VARCHAR(10) NULL, preferred_payment_method VARCHAR(50) NULL, is_active TINYINT(1) NOT NULL DEFAULT 1, INDEX idx_wholesale_clients_company (company_name), INDEX idx_wholesale_clients_contact (last_name, first_name)) ENGINE=InnoDB"
            Using command As New MySqlCommand(createWholesaleClientsTable, connection)
                command.ExecuteNonQuery()
            End Using
            AddWholesaleClientColumnIfMissing(connection, "preferred_payment_provider", "VARCHAR(30) NULL")
            AddWholesaleClientColumnIfMissing(connection, "payment_account_number", "VARCHAR(50) NULL")
            AddWholesaleClientColumnIfMissing(connection, "payment_account_name", "VARCHAR(150) NULL")
            NormalizeWholesaleClientPaymentPreferences(connection)
            CopyLegacyWholesaleClientsIfNeeded(connection)
            EnsureWholesaleSalesClientReference(connection)

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

    Private Sub MigrateLegacyWholesaleClientsTable(connection As MySqlConnection)
        If Not TableExists(connection, "wholesale_clients") Then Return
        If ColumnExists(connection, "wholesale_clients", "company_name") Then Return

        'Preserve the earlier, incorrect client/order table and its PO data, then
        'copy the reusable client details into the corrected profile table.
        If Not TableExists(connection, "wholesale_clients_legacy") Then
            Using renameCommand As New MySqlCommand("RENAME TABLE wholesale_clients TO wholesale_clients_legacy", connection)
                renameCommand.ExecuteNonQuery()
            End Using
        Else
            Throw New InvalidOperationException("Both legacy wholesale client tables exist. Resolve the legacy table before continuing database setup.")
        End If
    End Sub

    Private Sub CopyLegacyWholesaleClientsIfNeeded(connection As MySqlConnection)
        If Not TableExists(connection, "wholesale_clients_legacy") Then Return
        If Not ColumnExists(connection, "wholesale_clients_legacy", "license_no") Then Return

        Const copyQuery = "INSERT IGNORE INTO wholesale_clients " &
                          "(client_id, company_name, fda_lto_no, first_name, last_name, contact_no_1, shipping_address_line, is_active) " &
                          "SELECT wholesale_client_id, CONCAT('Legacy Client #', wholesale_client_id), license_no, contact_person, '', contact_no, address, 1 " &
                          "FROM wholesale_clients_legacy"
        Using command As New MySqlCommand(copyQuery, connection)
            command.ExecuteNonQuery()
        End Using

        If Not TableExists(connection, "wholesale_clients_legacy_archive") Then
            Using archiveCommand As New MySqlCommand("RENAME TABLE wholesale_clients_legacy TO wholesale_clients_legacy_archive", connection)
                archiveCommand.ExecuteNonQuery()
            End Using
        End If
    End Sub

    Private Function TableExists(connection As MySqlConnection, tableName As String) As Boolean
        Const query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @tableName"
        Using command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@tableName", tableName)
            Return Convert.ToInt32(command.ExecuteScalar()) > 0
        End Using
    End Function

    Private Function ColumnExists(connection As MySqlConnection, tableName As String, columnName As String) As Boolean
        Const query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @tableName AND COLUMN_NAME = @columnName"
        Using command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@tableName", tableName)
            command.Parameters.AddWithValue("@columnName", columnName)
            Return Convert.ToInt32(command.ExecuteScalar()) > 0
        End Using
    End Function

    Private Sub AddWholesaleClientColumnIfMissing(connection As MySqlConnection, columnName As String, definition As String)
        If ColumnExists(connection, "wholesale_clients", columnName) Then Return
        Using alterCommand As New MySqlCommand("ALTER TABLE wholesale_clients ADD COLUMN " & columnName & " " & definition, connection)
            alterCommand.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub AddPharmacySalesColumnIfMissing(connection As MySqlConnection, columnName As String, definition As String)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'pharmacy_sales' AND COLUMN_NAME = @columnName"
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", columnName)
            If Convert.ToInt32(existsCommand.ExecuteScalar()) > 0 Then Return
        End Using
        Using alterCommand As New MySqlCommand("ALTER TABLE pharmacy_sales ADD COLUMN " & columnName & " " & definition, connection)
            alterCommand.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub EnsureWholesaleSalesClientReference(connection As MySqlConnection)
        AddPharmacySalesColumnIfMissing(connection, "wholesale_client_id", "INT NULL")

        'Link older wholesale sales to saved clients where the stored license number matches.
        Const backfillQuery = "UPDATE pharmacy_sales ps " &
                              "INNER JOIN wholesale_clients wc ON wc.fda_lto_no = ps.customer_id_number " &
                              "SET ps.wholesale_client_id = wc.client_id " &
                              "WHERE ps.wholesale_client_id IS NULL AND ps.sale_mode = 'Wholesale'"
        Using backfillCommand As New MySqlCommand(backfillQuery, connection)
            backfillCommand.ExecuteNonQuery()
        End Using

        Const indexQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS " &
                           "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'pharmacy_sales' " &
                           "AND INDEX_NAME = 'idx_pharmacy_sales_wholesale_client'"
        Using indexCommand As New MySqlCommand(indexQuery, connection)
            If Convert.ToInt32(indexCommand.ExecuteScalar()) = 0 Then
                Using addIndexCommand As New MySqlCommand(
                    "ALTER TABLE pharmacy_sales ADD INDEX idx_pharmacy_sales_wholesale_client (wholesale_client_id)",
                    connection)
                    addIndexCommand.ExecuteNonQuery()
                End Using
            End If
        End Using

        Const foreignKeyQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS " &
                                "WHERE CONSTRAINT_SCHEMA = DATABASE() AND TABLE_NAME = 'pharmacy_sales' " &
                                "AND CONSTRAINT_NAME = 'fk_pharmacy_sales_wholesale_client' " &
                                "AND CONSTRAINT_TYPE = 'FOREIGN KEY'"
        Using foreignKeyCommand As New MySqlCommand(foreignKeyQuery, connection)
            If Convert.ToInt32(foreignKeyCommand.ExecuteScalar()) = 0 Then
                Const addForeignKey = "ALTER TABLE pharmacy_sales " &
                                      "ADD CONSTRAINT fk_pharmacy_sales_wholesale_client " &
                                      "FOREIGN KEY (wholesale_client_id) REFERENCES wholesale_clients(client_id) " &
                                      "ON DELETE SET NULL ON UPDATE CASCADE"
                Using addForeignKeyCommand As New MySqlCommand(addForeignKey, connection)
                    addForeignKeyCommand.ExecuteNonQuery()
                End Using
            End If
        End Using
    End Sub

    Private Sub NormalizeWholesaleClientPaymentPreferences(connection As MySqlConnection)
        Const query = "UPDATE wholesale_clients SET preferred_payment_provider = CASE " &
                      "WHEN LOWER(REPLACE(preferred_payment_method, ' ', '')) IN ('gcash', 'g-cash') THEN 'GCash' " &
                      "WHEN LOWER(preferred_payment_method) = 'maya' THEN 'Maya' " &
                      "WHEN LOWER(REPLACE(preferred_payment_method, ' ', '')) IN ('banktransfer', 'bank-transfer') THEN 'Bank Transfer' " &
                      "ELSE preferred_payment_provider END, " &
                      "preferred_payment_method = CASE " &
                      "WHEN LOWER(preferred_payment_method) IN ('cash') THEN 'Cash' " &
                      "WHEN LOWER(REPLACE(preferred_payment_method, ' ', '')) IN ('epayment', 'e-payment') THEN 'E-payment' " &
                      "WHEN LOWER(REPLACE(preferred_payment_method, ' ', '')) IN ('gcash', 'g-cash', 'maya', 'banktransfer', 'bank-transfer') THEN 'E-payment' " &
                      "WHEN preferred_payment_provider IN ('GCash', 'Maya', 'Bank Transfer') THEN 'E-payment' " &
                      "WHEN preferred_payment_method IS NOT NULL THEN NULL ELSE NULL END " &
                      "WHERE preferred_payment_method IS NOT NULL OR preferred_payment_provider IN ('GCash', 'Maya', 'Bank Transfer')"
        Using command As New MySqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub DropProductCategoryColumnIfExists(connection As MySqlConnection, columnName As String)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS " &
                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'product_category' AND COLUMN_NAME = @columnName"
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", columnName)
            If Convert.ToInt32(existsCommand.ExecuteScalar()) = 0 Then Return
        End Using

        Using alterCommand As New MySqlCommand("ALTER TABLE product_category DROP COLUMN " & columnName, connection)
            alterCommand.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub RenameProductBatchesTableIfNeeded(connection As MySqlConnection)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " &
                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @tableName"

        Dim oldExists As Boolean
        Dim newExists As Boolean
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@tableName", "productbatches")
            oldExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
            existsCommand.Parameters("@tableName").Value = "inventory_batch"
            newExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
        End Using

        If oldExists AndAlso Not newExists Then
            Using renameCommand As New MySqlCommand("RENAME TABLE productbatches TO inventory_batch", connection)
                renameCommand.ExecuteNonQuery()
            End Using
        End If
    End Sub

    Private Sub RenameWholesaleCustomersTableIfNeeded(connection As MySqlConnection)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES " &
                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @tableName"
        Dim oldExists As Boolean
        Dim newExists As Boolean
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@tableName", "wholesale_customers")
            oldExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
            existsCommand.Parameters("@tableName").Value = "wholesale_clients"
            newExists = Convert.ToInt32(existsCommand.ExecuteScalar()) > 0
        End Using

        If oldExists AndAlso Not newExists Then
            Using renameCommand As New MySqlCommand("RENAME TABLE wholesale_customers TO wholesale_clients", connection)
                renameCommand.ExecuteNonQuery()
            End Using
        End If
    End Sub

    Private Sub DropUsersColumnIfExists(connection As MySqlConnection, columnName As String)
        Const existsQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS " &
                            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'users' AND COLUMN_NAME = @columnName"
        Using existsCommand As New MySqlCommand(existsQuery, connection)
            existsCommand.Parameters.AddWithValue("@columnName", columnName)
            If Convert.ToInt32(existsCommand.ExecuteScalar()) = 0 Then Return
        End Using

        Using alterCommand As New MySqlCommand("ALTER TABLE users DROP COLUMN " & columnName, connection)
            alterCommand.ExecuteNonQuery()
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
