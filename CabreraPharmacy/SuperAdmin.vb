Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.Data

Public Class SuperAdmin

    ' --- 1. INITIALIZATION & NAVIGATION ---
    Private Sub SuperAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        ' Setup DataGridView Button Columns
        usrName.Text = "Welcome, " & CurrentFullName & " (Super Admin)"
        usrName.ReadOnly = True
        accountEdit.Text = "Edit"
        accountEdit.UseColumnTextForButtonValue = True
        accountResetPassword.Text = "Reset"
        accountResetPassword.UseColumnTextForButtonValue = True
        If Not accountsDataGrid.Columns.Contains("accountActivity") Then
            accountsDataGrid.Columns.Add(New DataGridViewButtonColumn With {
                .Name = "accountActivity",
                .HeaderText = "",
                .Text = "Activity",
                .UseColumnTextForButtonValue = True,
                .MinimumWidth = 6
            })
        End If

        accountsDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        accountsDataGrid.Height = pnlAccounts.Height - accountsDataGrid.Top - 40
        accountsDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' Set initial screen
        HideAllPanels()
        pnlDashboard.Visible = True
        pnlDashboard.BringToFront()

        ' Load the database data into the Accounts grid
        LoadAccounts()
        LoadSalesDashboardStats()
    End Sub

    Private Sub HideAllPanels()
        pnlDashboard.Visible = False
        pnlAccounts.Visible = False
        pnlReports.Visible = False
        pnlInventoryStockReport.Visible = False
        pnlFinancialSalesReport.Visible = False
        pnlRetailSalesReport.Visible = False
        pnlExpensesReport.Visible = False
        pnlPurchasesReport.Visible = False
        pnlWholesaleSalesReport.Visible = False
        pnlEmployeesalesReport.Visible = False
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        HideAllPanels()
        pnlDashboard.Visible = True
        pnlDashboard.BringToFront()
    End Sub

    Private Sub btnAccounts_Click(sender As Object, e As EventArgs) Handles btnAccounts.Click
        HideAllPanels()
        pnlAccounts.Visible = True
        pnlAccounts.BringToFront()
        LoadAccounts() ' Refresh data when opened
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        HideAllPanels()
        pnlReports.Visible = True
        pnlReports.BringToFront()
    End Sub

    Private Sub financialViewReport_Click(sender As Object, e As EventArgs) Handles financialViewReport.Click
        ShowSalesReport(pnlFinancialSalesReport, DataGridView4, Nothing,
                        "financialTransactionID", "financialDate", "financialCustomer", "financialCashier", "financialType", "financialTotal", "financialStatus", financialGenerated)
    End Sub

    Private Sub retailViewReport_Click(sender As Object, e As EventArgs) Handles retailViewReport.Click
        ShowSalesReport(pnlRetailSalesReport, retailDataGrid, "Retail",
                        "retailTransactionID", "retailDate", "retailCustomer", "retailCashier", "retailType", "retailTotal", "retailStatus", retailGenerated)
    End Sub

    Private Sub wholesaleViewReport_Click(sender As Object, e As EventArgs) Handles wholesaleViewReport.Click
        ShowSalesReport(pnlWholesaleSalesReport, wholesaleDataGrid, "Wholesale",
                        "DataGridViewTextBoxColumn2", "DataGridViewTextBoxColumn3", "DataGridViewTextBoxColumn4", "DataGridViewTextBoxColumn5", "DataGridViewTextBoxColumn6", "DataGridViewTextBoxColumn7", "DataGridViewTextBoxColumn8", employeeGenerated)
    End Sub

    Private Sub employeeViewReport_Click(sender As Object, e As EventArgs) Handles employeeViewReport.Click
        ShowSalesReport(pnlEmployeesalesReport, employeDataGrid, Nothing,
                        "employeeTransactionID", "employeeDate", "employeeCustomer", "employeeCashier", "employeeType", "employeeTotal", "employeeStatus", TextBox67)
    End Sub

    Private Sub ShowSalesReport(reportPanel As Panel,
                                grid As DataGridView,
                                saleMode As String,
                                idColumn As String,
                                dateColumn As String,
                                customerColumn As String,
                                cashierColumn As String,
                                typeColumn As String,
                                totalColumn As String,
                                statusColumn As String,
                                generatedLabel As TextBox)
        Try
            Dim columnNames = {idColumn, dateColumn, customerColumn, cashierColumn, typeColumn, totalColumn, statusColumn}
            Dim propertyNames = {"TransactionID", "SaleDate", "Customer", "Cashier", "SaleType", "Total", "Status"}
            For index = 0 To columnNames.Length - 1
                grid.Columns(columnNames(index)).DataPropertyName = propertyNames(index)
            Next

            grid.AutoGenerateColumns = False
            grid.AllowUserToAddRows = False
            grid.ReadOnly = True
            grid.RowHeadersVisible = False
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            grid.DataSource = New SalesRepository().GetTransactionReport(saleMode)
            grid.Columns(dateColumn).DefaultCellStyle.Format = "g"
            grid.Columns(totalColumn).DefaultCellStyle.Format = "'P' #,##0.00"
            generatedLabel.Text = "Generated: " & DateTime.Now.ToString("MMM dd, yyyy h:mm tt") & " | By: " & CurrentFullName

            HideAllPanels()
            reportPanel.Visible = True
            reportPanel.BringToFront()
        Catch ex As Exception
            MessageBox.Show("Could not load sales report. " & ex.Message, "Sales reports", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadSalesDashboardStats()
        Try
            Dim summary = New SalesRepository().GetSummary()
            userIDText.Text = "P " & summary.TodaySales.ToString("N2")
            monthDateYear.Text = DateTime.Today.ToString("MMMM dd, yyyy")
            TextBox3.Text = "P " & summary.RetailSales.ToString("N2")
            transactionRetail.Text = summary.RetailTransactions.ToString() & " Transactions"
            TextBox6.Text = "P " & summary.WholesaleSales.ToString("N2")
            transactionsWholesale.Text = summary.WholesaleTransactions.ToString() & " Transactions"
        Catch ex As Exception
            MessageBox.Show("Could not load sales totals. " & ex.Message, "Sales dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LogoutBtn_Click(sender As Object, e As EventArgs) Handles LogoutBtn.Click, btnLogin.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Me.Close()
    End Sub

    ' --- 2. ACCOUNTS DATABASE BINDING ---
    Private Sub LoadAccounts()
        Try
            Using connection = GetConnection()
                connection.Open()
                Dim table As New DataTable()
                Using adapter As New MySqlDataAdapter("SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, IF(is_active, 'Active', 'Inactive') AS Status, last_login_date AS LastLogin FROM users ORDER BY username", connection)
                    adapter.Fill(table)
                End Using

                ' Bind the SQL data to your visual designer columns
                accountsDataGrid.AutoGenerateColumns = False
                accountUserID.DataPropertyName = "UserID"
                accountFullName.DataPropertyName = "FullName"
                accountUsername.DataPropertyName = "Username"
                accountRole.DataPropertyName = "Role"
                accountStatus.DataPropertyName = "Status"
                accountLastLogin.DataPropertyName = "LastLogin"

                accountsDataGrid.DataSource = table
                accountsDataGrid.ClearSelection()

                ' Update Summary Cards
                UpdateAccountSummaries()
            End Using
        Catch ex As Exception
            MessageBox.Show("Could not load accounts. " & ex.Message, "Account management", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateAccountSummaries()
        numberSuperAdmin.Text = GetRoleCount("Super Admin").ToString()
        numberAdmin.Text = GetRoleCount("Admin").ToString()
        numberAssistant.Text = GetRoleCount("Staff").ToString()

        Dim total As Integer = GetRoleCount("Super Admin") + GetRoleCount("Admin") + GetRoleCount("Staff")
        numberTotalUsersRegistered.Text = total.ToString() & " users registered"
    End Sub

    Private Function GetRoleCount(roleName As String) As Integer
        Try
            Using connection = GetConnection()
                connection.Open()
                Using command As New MySqlCommand("SELECT COUNT(*) FROM users WHERE role = @role", connection)
                    command.Parameters.AddWithValue("@role", roleName)
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch
            Return 0
        End Try
    End Function

    ' --- 3. ACCOUNTS GRID ACTIONS ---
    Private Sub accountsDataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles accountsDataGrid.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim userId As Integer? = SelectedUserId(e.RowIndex)
        If Not userId.HasValue Then Return

        Select Case accountsDataGrid.Columns(e.ColumnIndex).Name
            Case "accountEdit"
                EditAccount(userId.Value)
            Case "accountResetPassword"
                ResetPassword(userId.Value)
            Case "accountEnableDisable"
                ToggleAccount(userId.Value, e.RowIndex)
            Case "accountActivity"
                ShowUserActivity(userId.Value)
        End Select
    End Sub

    Private Sub ShowUserActivity(userId As Integer)
        Try
            Dim activityTable As New DataTable()
            Using connection = GetConnection()
                connection.Open()
                Const query = "SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, activity AS Activity, created_at AS EventTime FROM user_activity WHERE user_id = @userId ORDER BY created_at DESC, activity_id DESC"
                Using adapter As New MySqlDataAdapter(query, connection)
                    adapter.SelectCommand.Parameters.AddWithValue("@userId", userId)
                    adapter.Fill(activityTable)
                End Using
            End Using

            Dim activityGrid As New DataGridView With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .DataSource = activityTable,
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False
            }
            Dim dialog As New Form With {
                .Text = "User Activity - ID " & userId.ToString(),
                .StartPosition = FormStartPosition.CenterParent,
                .Size = New Size(900, 500),
                .MinimumSize = New Size(650, 350),
                .BackColor = Color.White
            }
            dialog.Controls.Add(activityGrid)
            dialog.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show("Could not load activity for user " & userId.ToString() & ". " & ex.Message,
                            "User activity", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function SelectedUserId(rowIndex As Integer) As Integer?
        If rowIndex < 0 OrElse accountsDataGrid.Rows(rowIndex).Cells("accountUserID").Value Is Nothing Then
            Return Nothing
        End If
        Return Convert.ToInt32(accountsDataGrid.Rows(rowIndex).Cells("accountUserID").Value)
    End Function

    Private Sub btnAddAccount_Click(sender As Object, e As EventArgs) Handles btnAddAccount.Click
        ShowAccountDialog(Nothing)
    End Sub

    Private Sub EditAccount(userId As Integer)
        ShowAccountDialog(GetAccountDetails(userId))
    End Sub

    Private Sub ToggleAccount(userId As Integer, rowIndex As Integer)
        Dim active As Boolean = accountsDataGrid.Rows(rowIndex).Cells("accountStatus").Value.ToString() = "Active"
        Execute("UPDATE users SET is_active=@active WHERE user_id=@id", Not active, userId)
        LoadAccounts()
    End Sub

    ' Custom drawing for the toggle switch in the grid
    Private Sub accountsDataGrid_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles accountsDataGrid.CellPainting
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If accountsDataGrid.Columns(e.ColumnIndex).Name <> "accountEnableDisable" Then Return
        If accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value Is Nothing Then Return

        Dim statusValue = accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value
        Dim isActive = statusValue.ToString() = "Active"

        e.PaintBackground(e.CellBounds, True)

        Dim switchWidth = 44.0F
        Dim switchHeight = 22
        Dim switchLeft = e.CellBounds.Left + (e.CellBounds.Width - switchWidth) \ 2 - 20
        Dim switchTop = e.CellBounds.Top + (e.CellBounds.Height - switchHeight) \ 2
        Dim knobSize = 16
        Dim knobLeft As Integer = If(isActive, switchLeft + switchWidth - knobSize - 3, switchLeft + 3)
        Dim knobTop = switchTop + 3
        Dim trackColor = If(isActive, Color.ForestGreen, Color.Firebrick)

        Using trackBrush As New SolidBrush(trackColor), knobBrush As New SolidBrush(Color.White), borderPen As New Pen(Color.DimGray)
            e.Graphics.FillEllipse(trackBrush, switchLeft, switchTop, switchHeight, switchHeight)
            e.Graphics.FillEllipse(trackBrush, switchLeft + switchWidth - switchHeight, switchTop, switchHeight, switchHeight)
            e.Graphics.FillRectangle(trackBrush, switchLeft + switchHeight \ 2, switchTop, switchWidth - switchHeight, switchHeight)
            e.Graphics.FillEllipse(knobBrush, knobLeft, knobTop, knobSize, knobSize)
            e.Graphics.DrawEllipse(borderPen, knobLeft, knobTop, knobSize, knobSize)
        End Using

        Dim statusText = If(isActive, "Active", "Disabled")
        Dim textColor = If(isActive, Color.ForestGreen, Color.Firebrick)
        TextRenderer.DrawText(e.Graphics, statusText, accountsDataGrid.Font, New Point(switchLeft + switchWidth + 7, switchTop + 2), textColor)

        e.Handled = True
    End Sub

    ' --- 4. DATA MODELS & QUERIES ---
    Private Class AccountDetails
        Public UserId As Integer
        Public Username As String
        Public FullName As String
        Public FirstName As String
        Public LastName As String
        Public StaffInitials As String
        Public Role As String
        Public WorkShift As String
        Public EmailAddress As String
        Public ContactNumber As String
        Public HireDate As DateTime?
        Public BirthDate As DateTime?
        Public IsActive As Boolean
        Public LastLoginText As String
        Public CreatedAtText As String
    End Class

    Private Function GetAccountDetails(id As Integer) As AccountDetails
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand("SELECT * FROM users WHERE user_id = @id", connection)
                command.Parameters.AddWithValue("@id", id)
                Using reader = command.ExecuteReader()
                    If Not reader.Read() Then Return Nothing
                    Return New AccountDetails With {
                        .UserId = reader.GetInt32("user_id"),
                        .Username = ReadText(reader, "username"),
                        .FullName = ReadText(reader, "full_name"),
                        .FirstName = ReadText(reader, "first_name"),
                        .LastName = ReadText(reader, "last_name"),
                        .StaffInitials = ReadText(reader, "staff_initials"),
                        .Role = ReadText(reader, "role"),
                        .WorkShift = ReadText(reader, "work_shift"),
                        .EmailAddress = ReadText(reader, "email_address"),
                        .ContactNumber = ReadText(reader, "contact_number"),
                        .HireDate = ReadDate(reader, "hire_date"),
                        .BirthDate = ReadDate(reader, "birth_date"),
                        .IsActive = Convert.ToBoolean(reader("is_active")),
                        .LastLoginText = ReadText(reader, "last_login_date"),
                        .CreatedAtText = ReadText(reader, "created_at")
                    }
                End Using
            End Using
        End Using
    End Function

    Private Sub SaveAccount(id As Integer, isNew As Boolean, username As String, fullName As String, firstName As String, lastName As String, staffInitials As String, role As String, workShift As String, email As String, contact As String, hireDate As Object, birthDate As Object, pin As String, password As String, isActive As Boolean)
        Using connection = GetConnection()
            connection.Open()
            Dim sql As String
            If isNew Then
                sql = "INSERT INTO users (username, password_hash, full_name, first_name, last_name, staff_initials, role, work_shift, email_address, contact_number, hire_date, birth_date, pin_code, is_active) VALUES (@username, @passwordHash, @fullName, @firstName, @lastName, @staffInitials, @role, @workShift, @email, @contact, @hireDate, @birthDate, IF(@changePin, @pinHash, NULL), @isActive)"
            Else
                sql = "UPDATE users SET username=@username, full_name=@fullName, first_name=@firstName, last_name=@lastName, staff_initials=@staffInitials, role=@role, work_shift=@workShift, email_address=@email, contact_number=@contact, hire_date=@hireDate, birth_date=@birthDate, pin_code=IF(@changePin, @pinHash, pin_code), is_active=@isActive, password_hash=IF(@changePassword, @passwordHash, password_hash) WHERE user_id=@id"
            End If
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@username", username)
                command.Parameters.AddWithValue("@fullName", fullName)
                command.Parameters.AddWithValue("@firstName", firstName)
                command.Parameters.AddWithValue("@lastName", lastName)
                command.Parameters.AddWithValue("@staffInitials", staffInitials)
                command.Parameters.AddWithValue("@role", role)
                command.Parameters.AddWithValue("@workShift", workShift)
                command.Parameters.AddWithValue("@email", If(email = "", DBNull.Value, email))
                command.Parameters.AddWithValue("@contact", If(contact = "", DBNull.Value, contact))
                command.Parameters.AddWithValue("@hireDate", hireDate)
                command.Parameters.AddWithValue("@birthDate", birthDate)
                command.Parameters.AddWithValue("@isActive", isActive)
                command.Parameters.AddWithValue("@changePassword", password.Length > 0)
                command.Parameters.AddWithValue("@changePin", pin.Length > 0)

                ' We only hash the password if it's new or being changed, using your existing DatabaseModule HashPassword function
                command.Parameters.AddWithValue("@passwordHash", HashPassword(If(password = "", "unused", password)))
                command.Parameters.AddWithValue("@pinHash", HashPassword(If(pin = "", "0000", pin)))

                If Not isNew Then command.Parameters.AddWithValue("@id", id)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub Execute(sql As String, ParamArray values() As Object)
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand(sql, connection)
                If sql.Contains("@passwordHash") Then command.Parameters.AddWithValue("@passwordHash", values(0))
                If sql.Contains("@active") Then command.Parameters.AddWithValue("@active", values(0))
                If sql.Contains("@id") Then command.Parameters.AddWithValue("@id", values(values.Length - 1))
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Function ReadText(reader As MySqlDataReader, field As String) As String
        Return If(reader.IsDBNull(reader.GetOrdinal(field)), "", reader(field).ToString())
    End Function

    Private Function ReadDate(reader As MySqlDataReader, field As String) As DateTime?
        Try
            If reader.IsDBNull(reader.GetOrdinal(field)) Then Return Nothing
            Dim value = Convert.ToDateTime(reader(field))
            If value < New DateTime(1900, 1, 1) Then Return Nothing
            Return value
        Catch
            Return Nothing
        End Try
    End Function

    ' --- 5. DYNAMIC POPUP DIALOGS ---
    Private Sub ShowAccountDialog(account As AccountDetails)
        Dim isNew = account Is Nothing
        If isNew Then account = New AccountDetails With {.Role = "Staff", .IsActive = True}

        Dim dialog As New Form With {.Text = If(isNew, "Add Employee", "Edit Employee"), .Size = New Size(680, 720), .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False, .MinimizeBox = False, .BackColor = Color.White}
        Dim fields As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .AutoScroll = True, .Padding = New Padding(24, 20, 24, 10)}
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 68.0F))

        Dim firstNameBox = New TextBox With {.Text = account.FirstName, .Dock = DockStyle.Fill}
        Dim lastNameBox = New TextBox With {.Text = account.LastName, .Dock = DockStyle.Fill}
        Dim initialsBox = New TextBox With {.Text = account.StaffInitials, .Dock = DockStyle.Fill}
        Dim usernameBox = New TextBox With {.Text = account.Username, .Dock = DockStyle.Fill}
        Dim roleBox As New ComboBox With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList}
        roleBox.Items.AddRange({"Staff", "Admin", "Super Admin"})
        roleBox.SelectedItem = If(roleBox.Items.Contains(account.Role), account.Role, "Staff")
        Dim workShiftBox = New TextBox With {.Text = account.WorkShift, .Dock = DockStyle.Fill}
        Dim emailBox = New TextBox With {.Text = account.EmailAddress, .Dock = DockStyle.Fill}
        Dim contactBox = New TextBox With {.Text = account.ContactNumber, .Dock = DockStyle.Fill}
        Dim hireDateBox = CreateDatePicker(account.HireDate)
        Dim birthDateBox = CreateDatePicker(account.BirthDate)
        Dim passwordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim confirmPasswordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim showPasswordsBox As New CheckBox With {.Text = "Show passwords", .AutoSize = True}

        Dim lengthRule As New Label With {.AutoSize = True}
        Dim capitalRule As New Label With {.AutoSize = True}
        Dim numberRule As New Label With {.AutoSize = True}
        Dim symbolRule As New Label With {.AutoSize = True}
        Dim passwordRules As New FlowLayoutPanel With {.FlowDirection = FlowDirection.TopDown, .WrapContents = False, .AutoSize = True}
        passwordRules.Controls.AddRange({lengthRule, capitalRule, numberRule, symbolRule})

        UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)
        AddHandler passwordBox.TextChanged, Sub() UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)
        AddHandler showPasswordsBox.CheckedChanged, Sub()
                                                        passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                        confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                    End Sub

        Dim pinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim confirmPinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim activeBox = New CheckBox With {.Text = "Employee is active", .Checked = account.IsActive, .AutoSize = True}

        AddSection(fields, "PERSONAL INFORMATION")
        AddField(fields, "First name *", firstNameBox)
        AddField(fields, "Last name *", lastNameBox)
        AddField(fields, "Birth date *", birthDateBox)
        AddField(fields, "Email address", emailBox)
        AddField(fields, "Contact number", contactBox)
        AddSection(fields, "EMPLOYMENT INFORMATION")
        AddField(fields, "Staff initials *", initialsBox)
        AddField(fields, "Work shift *", workShiftBox)
        AddField(fields, "Role *", roleBox)
        AddField(fields, "Hire date *", hireDateBox)
        AddField(fields, "Status", activeBox)
        AddSection(fields, "LOGIN CREDENTIALS")
        AddField(fields, "Username *", usernameBox)

        If isNew Then
            AddField(fields, "Password *", passwordBox)
            AddField(fields, "Confirm password *", confirmPasswordBox)
            AddField(fields, "Show password", showPasswordsBox)
            AddField(fields, "Password requirements", passwordRules)
        End If

        Dim buttons As New FlowLayoutPanel With {.Dock = DockStyle.Bottom, .Height = 56, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(12)}
        Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Width = 90}
        Dim saveButton As New Button With {.Text = "Save Employee", .Width = 110, .BackColor = Color.SeaGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

        AddHandler saveButton.Click, Sub()
                                         ' Validation Logic
                                         If String.IsNullOrWhiteSpace(firstNameBox.Text) OrElse String.IsNullOrWhiteSpace(lastNameBox.Text) OrElse String.IsNullOrWhiteSpace(usernameBox.Text) Then
                                             MessageBox.Show("Required fields missing.", "Employee management", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                             Return
                                         End If
                                         Try
                                             SaveAccount(account.UserId, isNew, usernameBox.Text.Trim(), firstNameBox.Text & " " & lastNameBox.Text, firstNameBox.Text.Trim(), lastNameBox.Text.Trim(), initialsBox.Text.Trim(), roleBox.Text, workShiftBox.Text.Trim(), emailBox.Text.Trim(), contactBox.Text.Trim(), DateFromPicker(hireDateBox), DateFromPicker(birthDateBox), pinBox.Text, passwordBox.Text, activeBox.Checked)
                                             dialog.DialogResult = DialogResult.OK
                                             dialog.Close()
                                             LoadAccounts()
                                         Catch ex As MySqlException
                                             MessageBox.Show("Could not save the employee. " & ex.Message, "Employee management", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End Try
                                     End Sub

        buttons.Controls.Add(saveButton)
        buttons.Controls.Add(cancelButton)
        dialog.AcceptButton = saveButton
        dialog.CancelButton = cancelButton
        dialog.Controls.Add(fields)
        dialog.Controls.Add(buttons)
        dialog.ShowDialog(Me)
    End Sub

    Private Sub ResetPassword(userId As Integer)
        ShowResetPasswordDialog(userId)
    End Sub

    Private Sub ShowResetPasswordDialog(userId As Integer)
        Dim dialog As New Form With {.Text = "Reset Password", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .ClientSize = New Size(440, 390)}

        Dim fields As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .AutoScroll = True, .Padding = New Padding(20)}
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 36.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 64.0F))

        Dim passwordBox As New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim confirmPasswordBox As New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim showPasswordsBox As New CheckBox With {.Text = "Show passwords", .AutoSize = True}

        AddHandler showPasswordsBox.CheckedChanged, Sub()
                                                        passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                        confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                    End Sub

        AddField(fields, "New password *", passwordBox)
        AddField(fields, "Confirm password *", confirmPasswordBox)
        AddField(fields, "Password visibility", showPasswordsBox)

        Dim buttons As New FlowLayoutPanel With {.Dock = DockStyle.Bottom, .Height = 60, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(12)}
        Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Width = 90}
        Dim saveButton As New Button With {.Text = "Save Password", .Width = 115, .BackColor = Color.SeaGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

        AddHandler saveButton.Click, Sub()
                                         If passwordBox.Text <> confirmPasswordBox.Text Then
                                             MessageBox.Show("Passwords do not match.", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                             Return
                                         End If
                                         Try
                                             Execute("UPDATE users SET password_hash=@passwordHash WHERE user_id=@id", HashPassword(passwordBox.Text), userId)
                                             MessageBox.Show("Password reset successfully.", "Employee Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                             dialog.Close()
                                         Catch ex As Exception
                                             MessageBox.Show("Error resetting password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End Try
                                     End Sub

        buttons.Controls.Add(saveButton)
        buttons.Controls.Add(cancelButton)
        dialog.AcceptButton = saveButton
        dialog.CancelButton = cancelButton
        dialog.Controls.Add(fields)
        dialog.Controls.Add(buttons)
        dialog.ShowDialog(Me)
    End Sub

    ' Password Validation Helpers
    Private Sub UpdatePasswordRules(password As String, lengthRule As Label, capitalRule As Label, numberRule As Label, symbolRule As Label)
        SetPasswordRule(lengthRule, password.Length >= 8, "At least 8 characters")
        SetPasswordRule(capitalRule, Regex.IsMatch(password, "[A-Z]"), "At least 1 uppercase letter")
        SetPasswordRule(numberRule, Regex.IsMatch(password, "\d"), "At least 1 number")
        SetPasswordRule(symbolRule, Regex.IsMatch(password, "[^a-zA-Z0-9]"), "At least 1 special character")
    End Sub

    Private Sub SetPasswordRule(ruleLabel As Label, passed As Boolean, ruleText As String)
        ruleLabel.Text = If(passed, "✓ ", "✗ ") & ruleText
        ruleLabel.ForeColor = If(passed, Color.ForestGreen, Color.Firebrick)
    End Sub

    Private Function CreateDatePicker(value As DateTime?) As DateTimePicker
        Dim picker As New DateTimePicker With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short, .ShowCheckBox = True}
        If value.HasValue AndAlso value.Value >= picker.MinDate AndAlso value.Value <= picker.MaxDate Then
            picker.Value = value.Value
            picker.Checked = True
        Else
            picker.Checked = False
        End If
        Return picker
    End Function

    Private Function DateFromPicker(picker As DateTimePicker) As Object
        Return If(picker.Checked, picker.Value.Date, DBNull.Value)
    End Function

    Private Sub AddSection(table As TableLayoutPanel, title As String)
        Dim row = table.RowCount
        table.RowCount += 1
        table.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Dim label As New Label With {.Text = title, .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = Color.SeaGreen, .AutoSize = True, .Margin = New Padding(0, 16, 0, 5)}
        table.Controls.Add(label, 0, row)
        table.SetColumnSpan(label, 2)
    End Sub

    Private Sub AddField(table As TableLayoutPanel, caption As String, control As Control)
        Dim row = table.RowCount
        table.RowCount += 1
        table.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        table.Controls.Add(New Label With {.Text = caption, .AutoSize = True, .Anchor = AnchorStyles.Left, .Margin = New Padding(0, 8, 12, 8)}, 0, row)
        control.Margin = New Padding(0, 5, 0, 5)
        table.Controls.Add(control, 1, row)
    End Sub

    Private Sub pnlDashboard_Paint(sender As Object, e As PaintEventArgs) Handles pnlDashboard.Paint

    End Sub


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub storeName1_Click(sender As Object, e As EventArgs) Handles storeName1.Click

    End Sub
End Class
