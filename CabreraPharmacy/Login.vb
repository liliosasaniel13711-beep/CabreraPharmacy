Imports MySql.Data.MySqlClient

Public Class Login

    Private loginDesignScaled As Boolean

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        KeyPreview = True
        Try
            EnsureDatabase()
            ' Password starts hidden; use the existing hidepass PictureBox as the toggle.
            PasswordField.UseSystemPasswordChar = True
            hidepass.Visible = True
            passvisible.Visible = False
            hidepass.BringToFront()
            hidepass.Cursor = Cursors.Hand
            passvisible.Cursor = Cursors.Hand

            ' Old placeholder PictureBoxes are no longer used.
            picHide.Visible = False
            View.Visible = False
        Catch ex As Exception
            MessageBox.Show(
                "Unable to connect to the accounts database. " & ex.Message,
                "Database unavailable",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub Login_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            LoginBtn.PerformClick()
        End If
    End Sub

    Private Sub Login_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If loginDesignScaled Then Return

        'Scale the existing login design to use most of the maximized window.
        Dim widthScale As Single = CSng(ClientSize.Width / 1000.0F)
        Dim heightScale As Single = CSng(ClientSize.Height / 650.0F)
        Dim scale As Single = Math.Min(2.0F, Math.Max(1.0F, Math.Min(widthScale, heightScale)))

        NavBar.Scale(New SizeF(scale, scale))
        loginDesignScaled = True
        CenterLoginDesign()
    End Sub

    Private Sub Login_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If loginDesignScaled Then CenterLoginDesign()
    End Sub

    Private Sub CenterLoginDesign()
        NavBar.Left = Math.Max(0, (ClientSize.Width - NavBar.Width) \ 2)
        NavBar.Top = Math.Max(0, (ClientSize.Height - NavBar.Height) \ 2)
    End Sub

    Private Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        Dim username = UsernameField.Text.Trim()
        Dim password = PasswordField.Text

        If String.IsNullOrWhiteSpace(username) AndAlso String.IsNullOrEmpty(password) Then
            MessageBox.Show(
                "Username and password are required.",
                "Login",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
            Return
        End If

        If String.IsNullOrWhiteSpace(username) Then
            MessageBox.Show("Username is required.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            UsernameField.Focus()
            Return
        End If

        If String.IsNullOrEmpty(password) Then
            MessageBox.Show("Password is required.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            PasswordField.Focus()
            Return
        End If

        Try
            Dim userId As Integer
            Dim fullName As String
            Dim role As String

            Using connection = GetConnection()
                connection.Open()

                Const query = "SELECT user_id AS UserID, " &
                              "username AS Username, " &
                              "full_name AS FullName, " &
                              "role AS Role, " &
                              "password_hash AS PasswordHash, " &
                              "is_active AS IsActive " &
                              "FROM users " &
                              "WHERE username = @username LIMIT 1"

                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@username", username)

                    Using reader = command.ExecuteReader()
                        If Not reader.Read() Then
                            MessageBox.Show("Username was not found.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            UsernameField.Focus()
                            Return
                        End If

                        If Not VerifyPassword(password, reader.GetString("PasswordHash")) Then
                            MessageBox.Show("Incorrect password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            PasswordField.Clear()
                            PasswordField.Focus()
                            Return
                        End If

                        If Not reader.GetBoolean("IsActive") Then
                            MessageBox.Show("This account is inactive. Please contact the Super Admin.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            PasswordField.Clear()
                            PasswordField.Focus()
                            Return
                        End If

                        userId = reader.GetInt32("UserID")
                        fullName = reader.GetString("FullName")
                        role = reader.GetString("Role")
                    End Using
                End Using
            End Using

            Dim normalizedRole = role.Trim().ToLowerInvariant().Replace(" ", "").Replace("_", "").Replace("-", "")

            ' Routes to the correct form based on the user's role in the database
            Select Case normalizedRole
                Case "superadmin", "superadministrator"
                    CompleteLogin(userId, username, fullName, role, New SuperAdmin())

                Case "admin", "administrator"
                    CompleteLogin(userId, username, fullName, role, New Admin())

                Case "staff"
                    CompleteLogin(userId, username, fullName, role, New Staff())

                Case Else
                    MessageBox.Show(
                        "This account has an unrecognized role.",
                        "Login failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

                    PasswordField.Clear()
                    PasswordField.Focus()
            End Select

        Catch ex As Exception
            MessageBox.Show(
                "Login could not be completed. " & ex.Message,
                "Login error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CompleteLogin(userId As Integer,
                          username As String,
                          fullName As String,
                          role As String,
                          page As Form)

        UpdateLastLoginDate(userId)

        CurrentUserID = userId
        CurrentUsername = username
        CurrentFullName = fullName
        CurrentRole = role

        MyBase.Hide()

        page.StartPosition = FormStartPosition.CenterParent
        page.ShowDialog(Me)   'Login is the owner of the user-level form.
        page.Dispose()

        'Clear the previous session.
        CurrentUserID = 0
        CurrentUsername = ""
        CurrentFullName = ""
        CurrentRole = ""

        PasswordField.Clear()
        Me.Show()
        Me.Activate()
        UsernameField.Focus()
    End Sub

    Private Sub UpdateLastLoginDate(userId As Integer)
        Using connection = GetConnection()
            connection.Open()

            Const sql = "UPDATE users SET last_login_date = NOW() WHERE user_id = @id"

            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@id", userId)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' --- PASSWORD VISIBILITY TOGGLES ---
    Private Sub passvisible_Click(sender As Object, e As EventArgs) Handles passvisible.Click
        PasswordField.UseSystemPasswordChar = True
        passvisible.Visible = False
        hidepass.Visible = True
        hidepass.BringToFront()
    End Sub

    Private Sub hidepass_Click(sender As Object, e As EventArgs) Handles hidepass.Click
        PasswordField.UseSystemPasswordChar = False
        hidepass.Visible = False
        passvisible.Visible = True
        passvisible.BringToFront()
    End Sub

    Private Sub imageLogin_Click(sender As Object, e As EventArgs)

    End Sub
End Class
