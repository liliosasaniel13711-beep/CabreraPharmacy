Imports MySql.Data.MySqlClient

Public Class Login

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            EnsureDatabase()
            ' Ensure the password field hides characters by default when loaded
            PasswordField.UseSystemPasswordChar = True
        Catch ex As Exception
            MessageBox.Show(
                "Unable to connect to the accounts database. " & ex.Message,
                "Database unavailable",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
        End Try
    End Sub

    ' Updated to use your new custom LoginBtn
    Private Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        ' Updated to use your new custom UsernameField and PasswordField
        Dim username = UsernameField.Text.Trim()
        Dim password = PasswordField.Text

        If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show(
                "Enter your username and password.",
                "Login",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
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
                        If Not reader.Read() OrElse
                           Not reader.GetBoolean("IsActive") OrElse
                           Not VerifyPassword(password, reader.GetString("PasswordHash")) Then

                            MessageBox.Show(
                                "Invalid username or password, or this account is inactive.",
                                "Login failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)

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

            Dim normalizedRole = role.Trim().
                ToLowerInvariant().
                Replace(" ", "").
                Replace("_", "").
                Replace("-", "")

            Select Case normalizedRole
                Case "superadmin", "superadministrator"
                    CompleteLogin(userId, username, fullName, role, New SuperAdmin())

                Case "admin", "administrator"
                    ' Assuming you have an Admin form created
                    ' CompleteLogin(userId, username, fullName, role, New Admin())

                Case "staff"
                    ' Assuming you have a Staff form created
                    ' CompleteLogin(userId, username, fullName, role, New Staff())

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

    Private Sub CompleteLogin(
        userId As Integer,
        username As String,
        fullName As String,
        role As String,
        page As Form)

        'Save the successful login date and time.
        UpdateLastLoginDate(userId)

        'Store details of the signed-in employee.
        CurrentUserID = userId
        CurrentUsername = username
        CurrentFullName = fullName
        CurrentRole = role

        Hide()

        Using page
            page.ShowDialog()
        End Using

        PasswordField.Clear()
        Show()
        PasswordField.Focus()
    End Sub

    Private Sub UpdateLastLoginDate(userId As Integer)
        Using connection = GetConnection()
            connection.Open()

            Const sql = "UPDATE users " &
                        "SET last_login_date = NOW() " &
                        "WHERE user_id = @id"

            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@id", userId)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ' --- PASSWORD VISIBILITY TOGGLES ---
    Private Sub View_Click(sender As Object, e As EventArgs) Handles View.Click
        PasswordField.UseSystemPasswordChar = False
        View.Visible = False
        picHide.Visible = True
    End Sub

    Private Sub picHide_Click(sender As Object, e As EventArgs) Handles picHide.Click
        PasswordField.UseSystemPasswordChar = True
        picHide.Visible = False
        View.Visible = True
    End Sub

    Private Sub imageLogin_Click(sender As Object, e As EventArgs) Handles imageLogin.Click

    End Sub
End Class