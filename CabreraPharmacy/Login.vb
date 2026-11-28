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
        LayoutPasswordVisibilityIcons()
        CenterLoginDesign()
    End Sub

    Private Sub Login_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If loginDesignScaled Then CenterLoginDesign()
    End Sub

    Private Sub CenterLoginDesign()
        NavBar.Left = Math.Max(0, (ClientSize.Width - NavBar.Width) \ 2)
        NavBar.Top = Math.Max(0, (ClientSize.Height - NavBar.Height) \ 2)
    End Sub

    Private Sub LayoutPasswordVisibilityIcons()
        If PasswordField Is Nothing OrElse hidepass Is Nothing OrElse passvisible Is Nothing Then Return
        ' Keep the password field at its full width and overlay the eye icons inside its right edge.
        Dim iconLeft = PasswordField.Right - hidepass.Width - 6
        Dim iconTop = PasswordField.Top + (PasswordField.Height - hidepass.Height) \ 2
        hidepass.SetBounds(iconLeft, iconTop, hidepass.Width, hidepass.Height)
        passvisible.SetBounds(iconLeft, iconTop, passvisible.Width, passvisible.Height)
        If hidepass.Visible Then hidepass.BringToFront()
        If passvisible.Visible Then passvisible.BringToFront()
    End Sub

    Private Sub NavBar_SizeChanged(sender As Object, e As EventArgs)
        ApplyRoundedRegion(NavBar, Math.Min(24, Math.Min(NavBar.Width, NavBar.Height) \ 4))
    End Sub

    Private Sub LoginIcon_SizeChanged(sender As Object, e As EventArgs)
        ApplyRoundedRegion(LoginIcon, Math.Min(34, Math.Min(LoginIcon.Width, LoginIcon.Height) \ 2))
    End Sub

    Private Sub LoginBtn_SizeChanged(sender As Object, e As EventArgs)
        ApplyRoundedRegion(LoginBtn, Math.Min(18, Math.Max(1, LoginBtn.Height \ 2)))
    End Sub

    Private Sub ApplyRoundedRegion(control As Control, corner As Integer)
        If control.Width <= 0 OrElse control.Height <= 0 Then Return

        corner = Math.Max(1, Math.Min(corner, Math.Min(control.Width, control.Height)))
        Using path As New Drawing2D.GraphicsPath()
            path.AddArc(0, 0, corner, corner, 180, 90)
            path.AddArc(control.Width - corner, 0, corner, corner, 270, 90)
            path.AddArc(control.Width - corner, control.Height - corner, corner, corner, 0, 90)
            path.AddArc(0, control.Height - corner, corner, corner, 90, 90)
            path.CloseFigure()

            Dim previousRegion = control.Region
            control.Region = New Region(path)
            If previousRegion IsNot Nothing Then previousRegion.Dispose()
        End Using
    End Sub

    Private Sub imageLogin_Paint(sender As Object, e As PaintEventArgs)
        Dim bounds = imageLogin.ClientRectangle
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Using gradient As New Drawing2D.LinearGradientBrush(bounds,
                Color.FromArgb(53, 158, 108), Color.FromArgb(20, 100, 60),
                Drawing2D.LinearGradientMode.ForwardDiagonal)
            e.Graphics.FillRectangle(gradient, bounds)
        End Using
        Using lightShape As New SolidBrush(Color.FromArgb(48, Color.White))
            e.Graphics.FillPolygon(lightShape, {
                New Point(0, 0), New Point(bounds.Width, 0),
                New Point(bounds.Width \ 2, bounds.Height \ 3), New Point(0, bounds.Height \ 4)})
        End Using
        Using warmShape As New SolidBrush(Color.FromArgb(35, 8, 68, 42))
            e.Graphics.FillPolygon(warmShape, {
                New Point(0, CInt(bounds.Height * 0.65)),
                New Point(CInt(bounds.Width * 0.64), CInt(bounds.Height * 0.42)),
                New Point(bounds.Width, CInt(bounds.Height * 0.64)),
                New Point(bounds.Width, bounds.Height), New Point(0, bounds.Height)})
        End Using
        Using detailPen As New Pen(Color.FromArgb(54, Color.White), 1.0F)
            e.Graphics.DrawArc(detailPen, CInt(bounds.Width * 0.45), -40,
                               CInt(bounds.Width * 0.7), 170, 15, 145)
            e.Graphics.DrawLine(detailPen, 0, CInt(bounds.Height * 0.25), bounds.Width, 0)
        End Using
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
            Dim staffInitials As String
            Dim role As String

            Using connection = GetConnection()
                connection.Open()

                Const query = "SELECT user_id AS UserID, " &
                              "username AS Username, " &
                              "full_name AS FullName, " &
                              "staff_initials AS StaffInitials, " &
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
                        staffInitials = If(reader.IsDBNull(reader.GetOrdinal("StaffInitials")), String.Empty, reader.GetString("StaffInitials"))
                        role = reader.GetString("Role")
                    End Using
                End Using
            End Using

            Dim normalizedRole = role.Trim().ToLowerInvariant().Replace(" ", "").Replace("_", "").Replace("-", "")

            ' Routes to the correct form based on the user's role in the database
            Select Case normalizedRole
                Case "superadmin", "superadministrator"
                    CompleteLogin(userId, username, fullName, staffInitials, role, New SuperAdmin())

                Case "admin", "administrator"
                    CompleteLogin(userId, username, fullName, staffInitials, role, New Admin())

                Case "staff"
                    CompleteLogin(userId, username, fullName, staffInitials, role, New Staff())

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
                          staffInitials As String,
                          role As String,
                          page As Form)

        CurrentUserID = userId
        CurrentUsername = username
        CurrentFullName = fullName
        CurrentStaffInitials = staffInitials
        CurrentRole = role

        MyBase.Hide()

        page.StartPosition = FormStartPosition.CenterParent
        page.ShowDialog(Me)   'Login is the owner of the user-level form.
        page.Dispose()

        'Clear the previous session.
        CurrentUserID = 0
        CurrentUsername = ""
        CurrentFullName = ""
        CurrentStaffInitials = ""
        CurrentRole = ""

        PasswordField.Clear()
        Me.Show()
        Me.Activate()
        UsernameField.Focus()
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
