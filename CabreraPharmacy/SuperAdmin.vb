Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Public Class superadmin
    Private ReadOnly contentHost As New Panel()
    Private ReadOnly accountsGrid As New DataGridView()
    Private ReadOnly green As Color = Color.FromArgb(21, 120, 67)

    Private Sub superadmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Cabrera Pharmacy - Super Admin"
        WindowState = FormWindowState.Maximized
        BackColor = Color.White
        Controls.Clear()
        BuildShell()
        ShowDashboard()
    End Sub

    Private Sub BuildShell()
        Dim topBar As New Panel With {.Dock = DockStyle.Top, .Height = 82, .BackColor = Color.White}
        Dim brand As New Label With {.Text = "Cabrera's", .Font = New Font("Segoe UI", 18, FontStyle.Bold), .ForeColor = green, .AutoSize = True, .Location = New Point(28, 18)}
        Dim subBrand As New Label With {.Text = "Drugstore and Medical Supplies", .AutoSize = True, .ForeColor = Color.DimGray, .Location = New Point(30, 51)}
        Dim userInfo As New Label With {.Text = CurrentFullName & Environment.NewLine & "Super Admin / Owner", .Font = New Font("Segoe UI", 9), .AutoSize = True, .TextAlign = ContentAlignment.MiddleRight, .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .Location = New Point(1000, 20)}
        AddHandler topBar.Resize, Sub() userInfo.Left = topBar.ClientSize.Width - userInfo.Width - 34
        topBar.Controls.AddRange({brand, subBrand, userInfo})

        Dim sidebar As New Panel With {.Dock = DockStyle.Left, .Width = 220, .BackColor = Color.White, .Padding = New Padding(20, 24, 20, 20)}
        Dim navigation As New FlowLayoutPanel With {.Dock = DockStyle.Top, .FlowDirection = FlowDirection.TopDown, .WrapContents = False, .AutoSize = True}
        navigation.Controls.Add(CreateNavigationButton("Dashboard", Sub() ShowDashboard()))
        navigation.Controls.Add(CreateNavigationButton("Accounts", Sub() ShowAccounts()))
        navigation.Controls.Add(CreateNavigationButton("Reports", Sub() ShowPlaceholder("Reports")))

        Dim logout As New Button With {.Text = "Log Out", .Dock = DockStyle.Bottom, .Height = 40, .FlatStyle = FlatStyle.Flat, .ForeColor = green, .BackColor = Color.White}
        logout.FlatAppearance.BorderColor = green
        AddHandler logout.Click, Sub() Close()
        sidebar.Controls.Add(logout)
        sidebar.Controls.Add(navigation)

        contentHost.Dock = DockStyle.Fill
        contentHost.BackColor = Color.FromArgb(247, 248, 247)
        contentHost.Padding = New Padding(34, 26, 34, 26)

        Controls.Add(contentHost)
        Controls.Add(sidebar)
        Controls.Add(topBar)
    End Sub

    Private Function CreateNavigationButton(caption As String, clickAction As EventHandler) As Button
        Dim button As New Button With {.Text = caption, .Width = 178, .Height = 42, .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(16, 0, 0, 0), .FlatStyle = FlatStyle.Flat, .BackColor = Color.White, .ForeColor = green, .Margin = New Padding(0, 0, 0, 8)}
        button.FlatAppearance.BorderSize = 0
        AddHandler button.Click, clickAction
        Return button
    End Function

    Private Sub ShowDashboard()
        contentHost.Controls.Clear()
        Dim title As New Label With {.Text = "Dashboard", .Font = New Font("Segoe UI", 22, FontStyle.Bold), .ForeColor = green, .AutoSize = True, .Dock = DockStyle.Top}
        Dim subtitle As New Label With {.Text = "Overview of Cabrera's Drugstore and Medical Supplies", .ForeColor = Color.DimGray, .AutoSize = True, .Dock = DockStyle.Top, .Padding = New Padding(0, 3, 0, 20)}
        Dim cards As New TableLayoutPanel With {.Dock = DockStyle.Top, .Height = 240, .ColumnCount = 4, .RowCount = 2, .Padding = New Padding(0), .BackColor = contentHost.BackColor}
        For index = 0 To 3
            cards.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        Next
        cards.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        cards.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))

        Dim items = {("Today's Sale", "0.00", "No sales today"), ("Retail Sales", "0.00", "0 transactions"), ("Wholesale Sales", "0.00", "0 transactions"), ("Total Products", "0", "All categories"), ("Low Stock", "0", "Need reordering"), ("Expired", "0", "Within 0 days"), ("Near Expiry", "0", "Requires disposal"), ("Today's Expenses", "0.00", "No expenses recorded today")}
        For index = 0 To items.Length - 1
            cards.Controls.Add(CreateCard(items(index).Item1, items(index).Item2, items(index).Item3), index Mod 4, index \ 4)
        Next

        contentHost.Controls.Add(cards)
        contentHost.Controls.Add(subtitle)
        contentHost.Controls.Add(title)
    End Sub

    Private Function CreateCard(caption As String, value As String, note As String) As Panel
        Dim card As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(8), .Padding = New Padding(18)}
        card.BorderStyle = BorderStyle.FixedSingle
        Dim heading As New Label With {.Text = caption, .Font = New Font("Segoe UI", 10, FontStyle.Bold), .AutoSize = True, .Dock = DockStyle.Top}
        Dim detail As New Label With {.Text = note, .ForeColor = Color.DimGray, .AutoSize = True, .Dock = DockStyle.Bottom}
        Dim amount As New Label With {.Text = value, .Font = New Font("Segoe UI", 16), .ForeColor = Color.DimGray, .AutoSize = True, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}
        card.Controls.Add(amount)
        card.Controls.Add(detail)
        card.Controls.Add(heading)
        Return card
    End Function

    Private Sub ShowAccounts()
        contentHost.Controls.Clear()
        Dim title As New Label With {.Text = "Accounts", .Font = New Font("Segoe UI", 22, FontStyle.Bold), .ForeColor = green, .AutoSize = True, .Dock = DockStyle.Top}
        Dim topLine As New Panel With {.Dock = DockStyle.Top, .Height = 48}
        Dim subtitle As New Label With {.Text = "Registered user accounts", .ForeColor = Color.DimGray, .AutoSize = True, .Location = New Point(0, 12)}
        Dim addButton = CreateActionButton("+  Add Account", AddressOf AddAccount)
        addButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        addButton.Location = New Point(800, 6)
        AddHandler topLine.Resize, Sub() addButton.Left = topLine.ClientSize.Width - addButton.Width
        topLine.Controls.Add(subtitle)
        topLine.Controls.Add(addButton)

        Dim summaries As New TableLayoutPanel With {.Dock = DockStyle.Top, .Height = 112, .ColumnCount = 3, .Padding = New Padding(0, 0, 0, 12)}
        summaries.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
        summaries.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
        summaries.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.34F))
        summaries.Controls.Add(CreateAccountSummaryCard("SUPER ADMIN / OWNER", "Super Admin"), 0, 0)
        summaries.Controls.Add(CreateAccountSummaryCard("ADMIN / PHARMACIST", "Admin"), 1, 0)
        summaries.Controls.Add(CreateAccountSummaryCard("PHARMACY ASSISTANT", "Staff"), 2, 0)

        accountsGrid.Dock = DockStyle.Fill
        accountsGrid.BackgroundColor = Color.White
        accountsGrid.ReadOnly = True
        accountsGrid.AllowUserToAddRows = False
        accountsGrid.AllowUserToDeleteRows = False
        accountsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        accountsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        accountsGrid.MultiSelect = False
        ConfigureAccountsGrid()
        RemoveHandler accountsGrid.CellContentClick, AddressOf AccountGrid_CellContentClick
        AddHandler accountsGrid.CellContentClick, AddressOf AccountGrid_CellContentClick
        AddHandler accountsGrid.CellPainting, AddressOf AccountsGrid_CellPainting

        contentHost.Controls.Add(accountsGrid)
        contentHost.Controls.Add(summaries)
        contentHost.Controls.Add(topLine)
        contentHost.Controls.Add(title)
        LoadAccounts()
    End Sub

    Private Function CreateAccountSummaryCard(titleText As String, roleName As String) As Panel
        Dim card As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .BorderStyle = BorderStyle.FixedSingle, .Margin = New Padding(6)}
        Dim heading As New Label With {.Text = titleText, .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = Color.DimGray, .AutoSize = True, .Location = New Point(18, 18)}
        Dim count As New Label With {.Text = GetRoleCount(roleName).ToString() & " account(s)", .Font = New Font("Segoe UI", 15, FontStyle.Bold), .ForeColor = green, .AutoSize = True, .Location = New Point(18, 48)}
        card.Controls.Add(heading)
        card.Controls.Add(count)
        Return card
    End Function

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

    Private Sub ConfigureAccountsGrid()
        accountsGrid.AutoGenerateColumns = False
        accountsGrid.Columns.Clear()
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "UserID", .Name = "UserID", .HeaderText = "USER ID", .Width = 70})
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FullName", .Name = "FullName", .HeaderText = "FULL NAME", .Width = 200})
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Username", .Name = "Username", .HeaderText = "USERNAME", .Width = 120})
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Role", .Name = "Role", .HeaderText = "ROLE", .Width = 130})
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Status", .Name = "Status", .HeaderText = "STATUS", .Width = 90})
        accountsGrid.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "LastLogin", .Name = "LastLogin", .HeaderText = "LAST LOGIN", .Width = 135})
        accountsGrid.Columns.Add(New DataGridViewButtonColumn With {.Name = "EditAction", .HeaderText = "ACTIONS", .Text = "Edit", .UseColumnTextForButtonValue = True, .Width = 65})
        accountsGrid.Columns.Add(New DataGridViewButtonColumn With {.Name = "ResetAction", .HeaderText = "", .Text = "Reset", .UseColumnTextForButtonValue = True, .Width = 70})
        accountsGrid.Columns.Add(New DataGridViewButtonColumn With {
    .Name = "ToggleAction",
    .HeaderText = "ACCESS",
    .Text = "",
    .UseColumnTextForButtonValue = True,
    .Width = 125,
    .FlatStyle = FlatStyle.Flat
})
    End Sub

    Private Sub AccountGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        accountsGrid.CurrentCell = accountsGrid.Rows(e.RowIndex).Cells("Username")
        Select Case accountsGrid.Columns(e.ColumnIndex).Name
            Case "EditAction"
                EditAccount(Nothing, EventArgs.Empty)
            Case "ResetAction"
                ResetPassword(Nothing, EventArgs.Empty)
            Case "ToggleAction"
                ToggleAccount(Nothing, EventArgs.Empty)
        End Select
    End Sub

    Private Sub AccountsGrid_CellPainting(
    sender As Object,
    e As DataGridViewCellPaintingEventArgs)

        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then
            Return
        End If

        If e.RowIndex >= accountsGrid.Rows.Count OrElse
       e.ColumnIndex >= accountsGrid.Columns.Count Then

            Return
        End If

        If accountsGrid.Columns(e.ColumnIndex).Name <> "ToggleAction" Then
            Return
        End If

        If Not accountsGrid.Columns.Contains("Status") Then
            Return
        End If

        Dim statusColumnIndex = accountsGrid.Columns("Status").Index

        If statusColumnIndex < 0 OrElse
       statusColumnIndex >= accountsGrid.Rows(e.RowIndex).Cells.Count Then

            Return
        End If

        Dim statusValue =
        accountsGrid.Rows(e.RowIndex).
        Cells(statusColumnIndex).Value

        Dim isActive =
        statusValue IsNot Nothing AndAlso
        statusValue.ToString() = "Active"

        e.PaintBackground(e.CellBounds, True)

        Dim switchWidth = 44
        Dim switchHeight = 22

        Dim switchLeft =
        e.CellBounds.Left +
        (e.CellBounds.Width - switchWidth) \ 2

        Dim switchTop =
        e.CellBounds.Top +
        (e.CellBounds.Height - switchHeight) \ 2

        Dim knobSize = 16

        Dim knobLeft As Integer =
        If(isActive,
           switchLeft + switchWidth - knobSize - 3,
           switchLeft + 3)

        Dim knobTop = switchTop + 3

        Dim trackColor =
        If(isActive, Color.ForestGreen, Color.Firebrick)

        Using trackBrush As New SolidBrush(trackColor),
          knobBrush As New SolidBrush(Color.White),
          borderPen As New Pen(Color.DimGray)

            'Left rounded end
            e.Graphics.FillEllipse(
            trackBrush,
            switchLeft,
            switchTop,
            switchHeight,
            switchHeight)

            'Right rounded end
            e.Graphics.FillEllipse(
            trackBrush,
            switchLeft + switchWidth - switchHeight,
            switchTop,
            switchHeight,
            switchHeight)

            'Middle of the switch
            e.Graphics.FillRectangle(
            trackBrush,
            switchLeft + switchHeight \ 2,
            switchTop,
            switchWidth - switchHeight,
            switchHeight)

            'White switch indicator
            e.Graphics.FillEllipse(
            knobBrush,
            knobLeft,
            knobTop,
            knobSize,
            knobSize)

            e.Graphics.DrawEllipse(
            borderPen,
            knobLeft,
            knobTop,
            knobSize,
            knobSize)
        End Using

        Dim statusText = If(isActive, "Active", "Disabled")

        Dim textColor =
        If(isActive, Color.ForestGreen, Color.Firebrick)

        TextRenderer.DrawText(
        e.Graphics,
        statusText,
        accountsGrid.Font,
        New Point(switchLeft + switchWidth + 7, switchTop + 2),
        textColor)

        e.Handled = True
    End Sub

    Private Function CreateActionButton(caption As String, clickAction As EventHandler) As Button
        Dim button As New Button With {.Text = caption, .AutoSize = True, .Height = 32, .FlatStyle = FlatStyle.Flat, .BackColor = green, .ForeColor = Color.White, .Margin = New Padding(0, 0, 8, 0)}
        button.FlatAppearance.BorderSize = 0
        AddHandler button.Click, clickAction
        Return button
    End Function

    Private Sub ShowPlaceholder(titleText As String)
        contentHost.Controls.Clear()
        contentHost.Controls.Add(New Label With {.Text = titleText & Environment.NewLine & Environment.NewLine & "This section is ready for future development.", .Font = New Font("Segoe UI", 16), .AutoSize = True, .Location = New Point(34, 26)})
    End Sub

    Private Sub LoadAccounts()
        Try
            Using connection = GetConnection()
                Dim table As New DataTable()
                Using adapter As New MySqlDataAdapter("SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, IF(is_active, 'Active', 'Inactive') AS Status, last_login_date AS LastLogin FROM users ORDER BY username", connection)
                    adapter.Fill(table)
                End Using
                accountsGrid.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("Could not load accounts. " & ex.Message, "Account management", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function SelectedUserId() As Integer?
        If accountsGrid.CurrentRow Is Nothing Then
            MessageBox.Show("Select an account first.", "Account management", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        End If
        Return Convert.ToInt32(accountsGrid.CurrentRow.Cells("UserID").Value)
    End Function

    Private Function ChooseRole(initialRole As String) As String
        Dim role = InputBox("Role: Staff, Admin, or Super Admin", "Account role", initialRole).Trim()
        Dim normalizedRole = role.Replace(" ", "").Replace("_", "").Replace("-", "")
        If Not {"Staff", "Admin", "SuperAdmin"}.Contains(normalizedRole, StringComparer.OrdinalIgnoreCase) Then
            MessageBox.Show("Role must be Staff, Admin, or Super Admin.", "Account management", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If
        Return If(normalizedRole.Equals("superadmin", StringComparison.OrdinalIgnoreCase), "Super Admin", If(normalizedRole.Equals("admin", StringComparison.OrdinalIgnoreCase), "Admin", "Staff"))
    End Function

    Private Sub AddAccount(sender As Object, e As EventArgs)
        ShowAccountDialog(Nothing)
    End Sub

    Private Sub EditAccount(sender As Object, e As EventArgs)
        Dim id = SelectedUserId()
        If Not id.HasValue Then Return
        ShowAccountDialog(GetAccountDetails(id.Value))
    End Sub

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

            Using command As New MySqlCommand(
                "SELECT * FROM users WHERE user_id = @id", connection)

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

    Private Function ReadText(reader As MySqlDataReader, field As String) As String
        Return If(reader.IsDBNull(reader.GetOrdinal(field)), "", reader(field).ToString())
    End Function

    Private Function ReadDate(reader As MySqlDataReader, field As String) As DateTime?
        Try
            If reader.IsDBNull(reader.GetOrdinal(field)) Then Return Nothing

            Dim value = Convert.ToDateTime(reader(field))

            If value < New DateTime(1900, 1, 1) Then
                Return Nothing
            End If

            Return value
        Catch
            Return Nothing
        End Try
    End Function

    Private Sub ShowAccountDialog(account As AccountDetails)
        Dim isNew = account Is Nothing

        If isNew Then
            account = New AccountDetails With {
                .Role = "Staff",
                .IsActive = True
            }
        End If

        Dim dialog As New Form With {
            .Text = If(isNew, "Add Employee", "Edit Employee"),
            .Size = New Size(680, 720),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .MaximizeBox = False,
            .MinimizeBox = False,
            .BackColor = Color.White
        }

        Dim fields As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .AutoScroll = True,
            .Padding = New Padding(24, 20, 24, 10)
        }

        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 68.0F))

        Dim firstNameBox = New TextBox With {.Text = account.FirstName, .Dock = DockStyle.Fill}
        Dim lastNameBox = New TextBox With {.Text = account.LastName, .Dock = DockStyle.Fill}
        Dim initialsBox = New TextBox With {.Text = account.StaffInitials, .Dock = DockStyle.Fill}
        Dim usernameBox = New TextBox With {.Text = account.Username, .Dock = DockStyle.Fill}

        Dim roleBox As New ComboBox With {
            .Dock = DockStyle.Fill,
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        roleBox.Items.AddRange({"Staff", "Admin", "Super Admin"})
        roleBox.SelectedItem = If(roleBox.Items.Contains(account.Role), account.Role, "Staff")

        Dim workShiftBox = New TextBox With {.Text = account.WorkShift, .Dock = DockStyle.Fill}
        Dim emailBox = New TextBox With {.Text = account.EmailAddress, .Dock = DockStyle.Fill}
        Dim contactBox = New TextBox With {.Text = account.ContactNumber, .Dock = DockStyle.Fill}
        Dim hireDateBox = CreateDatePicker(account.HireDate)
        Dim birthDateBox = CreateDatePicker(account.BirthDate)

        Dim passwordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim confirmPasswordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim showPasswordsBox As New CheckBox With {
                .Text = "Show passwords",
                .AutoSize = True
            }

        Dim lengthRule As New Label With {.AutoSize = True}
        Dim capitalRule As New Label With {.AutoSize = True}
        Dim numberRule As New Label With {.AutoSize = True}
        Dim symbolRule As New Label With {.AutoSize = True}

        Dim passwordRules As New FlowLayoutPanel With {
                        .FlowDirection = FlowDirection.TopDown,
                        .WrapContents = False,
                        .AutoSize = True
                    }

        passwordRules.Controls.Add(lengthRule)
        passwordRules.Controls.Add(capitalRule)
        passwordRules.Controls.Add(numberRule)
        passwordRules.Controls.Add(symbolRule)

        UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)

        AddHandler passwordBox.TextChanged,
                        Sub()
                            UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)
                        End Sub

        AddHandler showPasswordsBox.CheckedChanged,
                        Sub()
                            passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                            confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                        End Sub
        Dim pinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim confirmPinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim activeBox = New CheckBox With {
            .Text = "Employee is active",
            .Checked = account.IsActive,
            .AutoSize = True
        }

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

        AddSection(fields, "TRANSACTION SECURITY")
        AddField(fields, "Set PIN (4 digits)", pinBox)
        AddField(fields, "Confirm PIN", confirmPinBox)

        If Not isNew Then
            AddSection(fields, "ACCOUNT HISTORY")
            AddField(fields, "Last login", New Label With {.Text = account.LastLoginText, .AutoSize = True})
            AddField(fields, "Created at", New Label With {.Text = account.CreatedAtText, .AutoSize = True})
        End If

        Dim buttons As New FlowLayoutPanel With {
            .Dock = DockStyle.Bottom,
            .Height = 56,
            .FlowDirection = FlowDirection.RightToLeft,
            .Padding = New Padding(12)
        }

        Dim cancelButton As New Button With {
            .Text = "Cancel",
            .DialogResult = DialogResult.Cancel,
            .Width = 90
        }

        Dim saveButton As New Button With {
            .Text = "Save Employee",
            .Width = 110,
            .BackColor = green,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }

        saveButton.FlatAppearance.BorderSize = 0

        AddHandler saveButton.Click,
            Sub()
                Dim firstName = firstNameBox.Text.Trim()
                Dim lastName = lastNameBox.Text.Trim()
                Dim initials = initialsBox.Text.Trim().ToUpper()
                Dim username = usernameBox.Text.Trim()
                Dim fullName = firstName & " " & lastName
                Dim password = passwordBox.Text
                Dim confirmPassword = confirmPasswordBox.Text
                Dim pin = pinBox.Text
                Dim confirmPin = confirmPinBox.Text

                If String.IsNullOrWhiteSpace(firstName) OrElse
                   String.IsNullOrWhiteSpace(lastName) OrElse
                   String.IsNullOrWhiteSpace(initials) OrElse
                   String.IsNullOrWhiteSpace(username) OrElse
                   String.IsNullOrWhiteSpace(workShiftBox.Text) Then

                    MessageBox.Show(
                        "First name, last name, staff initials, username, and work shift are required.",
                        "Employee management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
                    Return
                End If

                If Not hireDateBox.Checked OrElse Not birthDateBox.Checked Then
                    MessageBox.Show(
                        "Hire date and birth date are required.",
                        "Employee management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)
                    Return
                End If

                If isNew OrElse password.Length > 0 OrElse confirmPassword.Length > 0 Then
                    If password <> confirmPassword Then
                        MessageBox.Show(
                            "The password and confirmation do not match.",
                            "Employee management",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                        Return
                    End If

                    Dim passwordError = ValidatePassword(password)

                    If passwordError <> "" Then
                        MessageBox.Show(
                            passwordError,
                            "Employee management",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                        Return
                    End If

                    If PasswordUsedByAnotherAccount(password, account.UserId) Then
                        MessageBox.Show(
                            "This password is already used by another account. Choose a unique password.",
                            "Employee management",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                        Return
                    End If
                End If

                If pin.Length > 0 OrElse confirmPin.Length > 0 Then
                    If pin <> confirmPin Then
                        MessageBox.Show(
                            "The PIN and confirmation do not match.",
                            "Employee management",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                        Return
                    End If

                    If Not Regex.IsMatch(pin, "^\d{4}$") Then
                        MessageBox.Show(
                            "PIN must contain exactly four numbers.",
                            "Employee management",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                        Return
                    End If
                End If

                Try
                    SaveAccount(
                        account.UserId,
                        isNew,
                        username,
                        fullName,
                        firstName,
                        lastName,
                        initials,
                        roleBox.Text,
                        workShiftBox.Text.Trim(),
                        emailBox.Text.Trim(),
                        contactBox.Text.Trim(),
                        DateFromPicker(hireDateBox),
                        DateFromPicker(birthDateBox),
                        pin,
                        password,
                        activeBox.Checked)

                    dialog.DialogResult = DialogResult.OK
                    dialog.Close()
                    LoadAccounts()

                Catch ex As MySqlException
                    MessageBox.Show(
                        "Could not save the employee. " & ex.Message,
                        "Employee management",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
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

    Private Function ValidatePassword(password As String) As String
        If password.Length < 8 Then
            Return "Password must be at least 8 characters long."
        End If

        If Not Regex.IsMatch(password, "[A-Z]") Then
            Return "Password must contain at least one uppercase letter."
        End If

        If Not Regex.IsMatch(password, "\d") Then
            Return "Password must contain at least one number."
        End If

        If Not Regex.IsMatch(password, "[^a-zA-Z0-9]") Then
            Return "Password must contain at least one special character."
        End If

        Return ""
    End Function
    Private Sub UpdatePasswordRules(
    password As String,
    lengthRule As Label,
    capitalRule As Label,
    numberRule As Label,
    symbolRule As Label)

        SetPasswordRule(lengthRule, password.Length >= 8, "At least 8 characters")
        SetPasswordRule(capitalRule, Regex.IsMatch(password, "[A-Z]"), "At least 1 uppercase letter")
        SetPasswordRule(numberRule, Regex.IsMatch(password, "\d"), "At least 1 number")
        SetPasswordRule(symbolRule, Regex.IsMatch(password, "[^a-zA-Z0-9]"), "At least 1 special character")
    End Sub

    Private Sub SetPasswordRule(ruleLabel As Label, passed As Boolean, ruleText As String)
        ruleLabel.Text = If(passed, "✓ ", "✗ ") & ruleText
        ruleLabel.ForeColor = If(passed, Color.ForestGreen, Color.Firebrick)
    End Sub

    Private Function PasswordUsedByAnotherAccount(password As String, currentUserId As Integer) As Boolean
        Using connection = GetConnection()
            connection.Open()

            Using command As New MySqlCommand(
                "SELECT user_id, password_hash FROM users WHERE user_id <> @id",
                connection)

                command.Parameters.AddWithValue("@id", currentUserId)

                Using reader = command.ExecuteReader()
                    While reader.Read()
                        If VerifyPassword(password, reader.GetString("password_hash")) Then
                            Return True
                        End If
                    End While
                End Using
            End Using
        End Using

        Return False
    End Function

    Private Function CreateDatePicker(value As DateTime?) As DateTimePicker
        Dim picker As New DateTimePicker With {
            .Dock = DockStyle.Fill,
            .Format = DateTimePickerFormat.Short,
            .ShowCheckBox = True
        }

        If value.HasValue AndAlso
           value.Value >= picker.MinDate AndAlso
           value.Value <= picker.MaxDate Then

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

        Dim label As New Label With {
            .Text = title,
            .Font = New Font("Segoe UI", 9, FontStyle.Bold),
            .ForeColor = green,
            .AutoSize = True,
            .Margin = New Padding(0, 16, 0, 5)
        }

        table.Controls.Add(label, 0, row)
        table.SetColumnSpan(label, 2)
    End Sub

    Private Sub AddField(table As TableLayoutPanel, caption As String, control As Control)
        Dim row = table.RowCount
        table.RowCount += 1
        table.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        table.Controls.Add(
            New Label With {
                .Text = caption,
                .AutoSize = True,
                .Anchor = AnchorStyles.Left,
                .Margin = New Padding(0, 8, 12, 8)
            },
            0,
            row)

        control.Margin = New Padding(0, 5, 0, 5)
        table.Controls.Add(control, 1, row)
    End Sub

    Private Sub SaveAccount(
        id As Integer,
        isNew As Boolean,
        username As String,
        fullName As String,
        firstName As String,
        lastName As String,
        staffInitials As String,
        role As String,
        workShift As String,
        email As String,
        contact As String,
        hireDate As Object,
        birthDate As Object,
        pin As String,
        password As String,
        isActive As Boolean)

        Using connection = GetConnection()
            connection.Open()

            Dim sql As String

            If isNew Then
                sql =
                    "INSERT INTO users " &
                    "(username, password_hash, full_name, first_name, last_name, staff_initials, role, work_shift, email_address, contact_number, hire_date, birth_date, pin_code, is_active) " &
                    "VALUES (@username, @passwordHash, @fullName, @firstName, @lastName, @staffInitials, @role, @workShift, @email, @contact, @hireDate, @birthDate, IF(@changePin, @pinHash, NULL), @isActive)"
            Else
                sql =
                    "UPDATE users SET " &
                    "username=@username, full_name=@fullName, first_name=@firstName, last_name=@lastName, staff_initials=@staffInitials, " &
                    "role=@role, work_shift=@workShift, email_address=@email, contact_number=@contact, hire_date=@hireDate, birth_date=@birthDate, " &
                    "pin_code=IF(@changePin, @pinHash, pin_code), is_active=@isActive, " &
                    "password_hash=IF(@changePassword, @passwordHash, password_hash) " &
                    "WHERE user_id=@id"
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
                command.Parameters.AddWithValue("@passwordHash", HashPassword(If(password = "", "unused", password)))
                command.Parameters.AddWithValue("@pinHash", HashPassword(If(pin = "", "0000", pin)))

                If Not isNew Then
                    command.Parameters.AddWithValue("@id", id)
                End If

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub ResetPassword(sender As Object, e As EventArgs)
        Dim id = SelectedUserId()
        If Not id.HasValue Then Return

        'The signed-in Super Admin must approve the reset with their own PIN.
        If Not VerifySuperAdminPin() Then Return

        ShowResetPasswordDialog(id.Value)
    End Sub

    Private Function VerifySuperAdminPin() As Boolean
        Dim enteredPin = InputBox(
        "Enter your Super Admin PIN to approve this password reset.",
        "Super Admin Authorization").Trim()

        If String.IsNullOrWhiteSpace(enteredPin) Then
            Return False
        End If

        If Not Regex.IsMatch(enteredPin, "^\d{4}$") Then
            MessageBox.Show(
            "PIN must contain exactly four numbers.",
            "Super Admin Authorization",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)
            Return False
        End If

        Try
            Using connection = GetConnection()
                connection.Open()

                Const sql = "SELECT pin_code " &
                        "FROM users " &
                        "WHERE user_id = @id AND is_active = TRUE"

                Using command As New MySqlCommand(sql, connection)
                    command.Parameters.AddWithValue("@id", CurrentUserID)

                    Using reader = command.ExecuteReader()
                        If Not reader.Read() OrElse reader.IsDBNull(reader.GetOrdinal("pin_code")) Then
                            MessageBox.Show(
                            "Your Super Admin account does not have a PIN. Set one before resetting passwords.",
                            "Super Admin Authorization",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                            Return False
                        End If

                        If Not VerifyPassword(enteredPin, reader.GetString("pin_code")) Then
                            MessageBox.Show(
                            "Incorrect Super Admin PIN.",
                            "Super Admin Authorization",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                            Return False
                        End If
                    End Using
                End Using
            End Using

            Return True

        Catch ex As Exception
            MessageBox.Show(
            "Could not verify the Super Admin PIN. " & ex.Message,
            "Super Admin Authorization",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error)

            Return False
        End Try
    End Function

    Private Sub ShowResetPasswordDialog(userId As Integer)
        Dim dialog As New Form With {
        .Text = "Reset Password",
        .StartPosition = FormStartPosition.CenterParent,
        .FormBorderStyle = FormBorderStyle.FixedDialog,
        .MaximizeBox = False,
        .MinimizeBox = False,
        .ShowInTaskbar = False,
        .ClientSize = New Size(440, 390)
    }

        Dim fields As New TableLayoutPanel With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .AutoScroll = True,
        .Padding = New Padding(20)
    }

        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 36.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 64.0F))

        Dim passwordBox As New TextBox With {
        .Dock = DockStyle.Fill,
        .UseSystemPasswordChar = True
    }

        Dim confirmPasswordBox As New TextBox With {
        .Dock = DockStyle.Fill,
        .UseSystemPasswordChar = True
    }

        Dim showPasswordsBox As New CheckBox With {
        .Text = "Show passwords",
        .AutoSize = True
    }

        Dim lengthRule As New Label With {.AutoSize = True}
        Dim capitalRule As New Label With {.AutoSize = True}
        Dim numberRule As New Label With {.AutoSize = True}
        Dim symbolRule As New Label With {.AutoSize = True}

        Dim passwordRules As New FlowLayoutPanel With {
        .FlowDirection = FlowDirection.TopDown,
        .WrapContents = False,
        .AutoSize = True
    }

        passwordRules.Controls.Add(lengthRule)
        passwordRules.Controls.Add(capitalRule)
        passwordRules.Controls.Add(numberRule)
        passwordRules.Controls.Add(symbolRule)

        UpdatePasswordRules(
        passwordBox.Text,
        lengthRule,
        capitalRule,
        numberRule,
        symbolRule)

        AddHandler passwordBox.TextChanged,
        Sub()
            UpdatePasswordRules(
                passwordBox.Text,
                lengthRule,
                capitalRule,
                numberRule,
                symbolRule)
        End Sub

        AddHandler showPasswordsBox.CheckedChanged,
        Sub()
            passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
            confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
        End Sub

        AddField(fields, "New password *", passwordBox)
        AddField(fields, "Confirm password *", confirmPasswordBox)
        AddField(fields, "Password visibility", showPasswordsBox)
        AddField(fields, "Requirements", passwordRules)

        Dim buttons As New FlowLayoutPanel With {
        .Dock = DockStyle.Bottom,
        .Height = 60,
        .FlowDirection = FlowDirection.RightToLeft,
        .Padding = New Padding(12)
    }

        Dim cancelButton As New Button With {
        .Text = "Cancel",
        .DialogResult = DialogResult.Cancel,
        .Width = 90
    }

        Dim saveButton As New Button With {
        .Text = "Save Password",
        .Width = 115,
        .BackColor = green,
        .ForeColor = Color.White,
        .FlatStyle = FlatStyle.Flat
    }

        saveButton.FlatAppearance.BorderSize = 0

        AddHandler saveButton.Click,
        Sub()
            Dim newPassword = passwordBox.Text
            Dim confirmedPassword = confirmPasswordBox.Text

            If newPassword <> confirmedPassword Then
                MessageBox.Show(
                    "The password and confirmation do not match.",
                    "Reset Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return
            End If

            Dim passwordError = ValidatePassword(newPassword)

            If passwordError <> "" Then
                MessageBox.Show(
                    passwordError,
                    "Reset Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return
            End If

            If PasswordUsedByAnotherAccount(newPassword, userId) Then
                MessageBox.Show(
                    "This password is already used by another account.",
                    "Reset Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning)
                Return
            End If

            Try
                Execute(
                    "UPDATE users SET password_hash=@passwordHash WHERE user_id=@id",
                    HashPassword(newPassword),
                    userId)

                MessageBox.Show(
                    "Password reset successfully.",
                    "Employee Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)

                dialog.DialogResult = DialogResult.OK
                dialog.Close()

            Catch ex As Exception
                MessageBox.Show(
                    "Could not reset the password. " & ex.Message,
                    "Reset Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
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

    Private Sub ToggleAccount(sender As Object, e As EventArgs)
        Dim id = SelectedUserId()
        If Not id.HasValue Then Return

        If id.Value = CurrentUserID Then
            MessageBox.Show(
                "You cannot deactivate the account currently signed in.",
                "Employee management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Dim active = accountsGrid.CurrentRow.Cells("Status").Value.ToString() = "Active"

        Execute(
            "UPDATE users SET is_active=@active WHERE user_id=@id",
            Not active,
            id.Value)

        LoadAccounts()
    End Sub

    Private Sub Execute(sql As String, ParamArray values() As Object)
        Using connection = GetConnection()
            connection.Open()

            Using command As New MySqlCommand(sql, connection)
                If sql.Contains("@passwordHash") Then
                    command.Parameters.AddWithValue("@passwordHash", values(0))
                End If

                If sql.Contains("@active") Then
                    command.Parameters.AddWithValue("@active", values(0))
                End If

                If sql.Contains("@id") Then
                    command.Parameters.AddWithValue("@id", values(values.Length - 1))
                End If

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub
End Class
