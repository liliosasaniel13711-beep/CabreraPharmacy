<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Login))
        NavBar = New Panel()
        hidepass = New PictureBox()
        passvisible = New PictureBox()
        LoginLabel = New Label()
        picHide = New PictureBox()
        View = New PictureBox()
        LoginIcon = New PictureBox()
        LoginBtn = New Button()
        PasswordField = New TextBox()
        UsernameField = New TextBox()
        Password = New Label()
        Username = New Label()
        DrugstoreName = New Label()
        imageLogin = New PictureBox()
        NavBar.SuspendLayout()
        CType(hidepass, ComponentModel.ISupportInitialize).BeginInit()
        CType(passvisible, ComponentModel.ISupportInitialize).BeginInit()
        CType(picHide, ComponentModel.ISupportInitialize).BeginInit()
        CType(View, ComponentModel.ISupportInitialize).BeginInit()
        CType(LoginIcon, ComponentModel.ISupportInitialize).BeginInit()
        CType(imageLogin, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' NavBar
        ' 
        NavBar.Anchor = AnchorStyles.None
        NavBar.BackColor = Color.White
        NavBar.Controls.Add(hidepass)
        NavBar.Controls.Add(passvisible)
        NavBar.Controls.Add(LoginLabel)
        NavBar.Controls.Add(picHide)
        NavBar.Controls.Add(View)
        NavBar.Controls.Add(LoginIcon)
        NavBar.Controls.Add(LoginBtn)
        NavBar.Controls.Add(PasswordField)
        NavBar.Controls.Add(UsernameField)
        NavBar.Controls.Add(Password)
        NavBar.Controls.Add(Username)
        NavBar.Controls.Add(DrugstoreName)
        NavBar.Controls.Add(imageLogin)
        NavBar.Location = New Point(418, 179)
        NavBar.Margin = New Padding(3, 2, 3, 2)
        NavBar.Name = "NavBar"
        NavBar.Size = New Size(607, 386)
        NavBar.TabIndex = 1
        Dim cardPath As New Drawing2D.GraphicsPath()
        cardPath.AddArc(0, 0, 24, 24, 180, 90)
        cardPath.AddArc(NavBar.Width - 24, 0, 24, 24, 270, 90)
        cardPath.AddArc(NavBar.Width - 24, NavBar.Height - 24, 24, 24, 0, 90)
        cardPath.AddArc(0, NavBar.Height - 24, 24, 24, 90, 90)
        cardPath.CloseFigure()
        NavBar.Region = New Region(cardPath)
        cardPath.Dispose()
        AddHandler NavBar.SizeChanged,
            Sub(sender As Object, e As EventArgs)
                Dim path As New Drawing2D.GraphicsPath()
                Dim corner As Integer = Math.Min(24, Math.Min(NavBar.Width, NavBar.Height) \ 4)
                path.AddArc(0, 0, corner, corner, 180, 90)
                path.AddArc(NavBar.Width - corner, 0, corner, corner, 270, 90)
                path.AddArc(NavBar.Width - corner, NavBar.Height - corner, corner, corner, 0, 90)
                path.AddArc(0, NavBar.Height - corner, corner, corner, 90, 90)
                path.CloseFigure()
                Dim previousRegion = NavBar.Region
                NavBar.Region = New Region(path)
                previousRegion.Dispose()
                path.Dispose()
            End Sub
        ' 
        ' hidepass
        ' 
        hidepass.Anchor = AnchorStyles.None
        hidepass.ErrorImage = CType(resources.GetObject("hidepass.ErrorImage"), Image)
        hidepass.Image = CType(resources.GetObject("hidepass.Image"), Image)
        hidepass.Location = New Point(552, 259)
        hidepass.Name = "hidepass"
        hidepass.Size = New Size(18, 18)
        hidepass.SizeMode = PictureBoxSizeMode.Zoom
        hidepass.TabIndex = 23
        hidepass.TabStop = False
        ' 
        ' passvisible
        ' 
        passvisible.Anchor = AnchorStyles.None
        passvisible.ErrorImage = CType(resources.GetObject("passvisible.ErrorImage"), Image)
        passvisible.Image = CType(resources.GetObject("passvisible.Image"), Image)
        passvisible.Location = New Point(552, 259)
        passvisible.Name = "passvisible"
        passvisible.Size = New Size(18, 18)
        passvisible.SizeMode = PictureBoxSizeMode.Zoom
        passvisible.TabIndex = 22
        passvisible.TabStop = False
        ' 
        ' LoginLabel
        ' 
        LoginLabel.Anchor = AnchorStyles.Right
        LoginLabel.AutoSize = True
        LoginLabel.Font = New Font("Segoe UI", 15.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginLabel.ForeColor = Color.FromArgb(38, 126, 83)
        LoginLabel.Visible = False
        LoginLabel.Location = New Point(316, 164)
        LoginLabel.Name = "LoginLabel"
        LoginLabel.Size = New Size(63, 25)
        LoginLabel.TabIndex = 19
        LoginLabel.Text = "Login"
        ' 
        ' picHide
        ' 
        picHide.Anchor = AnchorStyles.None
        picHide.Location = New Point(551, 266)
        picHide.Margin = New Padding(3, 2, 3, 2)
        picHide.Name = "picHide"
        picHide.Size = New Size(30, 17)
        picHide.SizeMode = PictureBoxSizeMode.StretchImage
        picHide.TabIndex = 18
        picHide.TabStop = False
        ' 
        ' View
        ' 
        View.Anchor = AnchorStyles.None
        View.Location = New Point(551, 241)
        View.Margin = New Padding(3, 2, 3, 2)
        View.Name = "View"
        View.Size = New Size(30, 17)
        View.SizeMode = PictureBoxSizeMode.StretchImage
        View.TabIndex = 17
        View.TabStop = False
        ' 
        ' LoginIcon
        ' 
        LoginIcon.Anchor = AnchorStyles.None
        LoginIcon.Image = My.Resources.Resources.ed8bb44f_0265_4bb2_a677_771c15e67760
        LoginIcon.Location = New Point(414, 30)
        LoginIcon.Margin = New Padding(3, 2, 3, 2)
        LoginIcon.Name = "LoginIcon"
        LoginIcon.Size = New Size(82, 72)
        LoginIcon.SizeMode = PictureBoxSizeMode.Zoom
        LoginIcon.TabIndex = 16
        LoginIcon.TabStop = False
        Dim logoPath As New Drawing2D.GraphicsPath()
        logoPath.AddArc(0, 0, 34, 34, 180, 90)
        logoPath.AddArc(LoginIcon.Width - 34, 0, 34, 34, 270, 90)
        logoPath.AddArc(LoginIcon.Width - 34, LoginIcon.Height - 34, 34, 34, 0, 90)
        logoPath.AddArc(0, LoginIcon.Height - 34, 34, 34, 90, 90)
        logoPath.CloseFigure()
        LoginIcon.Region = New Region(logoPath)
        logoPath.Dispose()
        AddHandler LoginIcon.SizeChanged,
            Sub(sender As Object, e As EventArgs)
                Dim path As New Drawing2D.GraphicsPath()
                Dim corner As Integer = Math.Min(34, Math.Min(LoginIcon.Width, LoginIcon.Height) \ 2)
                path.AddArc(0, 0, corner, corner, 180, 90)
                path.AddArc(LoginIcon.Width - corner, 0, corner, corner, 270, 90)
                path.AddArc(LoginIcon.Width - corner, LoginIcon.Height - corner, corner, corner, 0, 90)
                path.AddArc(0, LoginIcon.Height - corner, corner, corner, 90, 90)
                path.CloseFigure()
                Dim previousRegion = LoginIcon.Region
                LoginIcon.Region = New Region(path)
                If previousRegion IsNot Nothing Then previousRegion.Dispose()
                path.Dispose()
            End Sub
        ' 
        ' LoginBtn
        ' 
        LoginBtn.Anchor = AnchorStyles.None
        LoginBtn.BackColor = Color.FromArgb(245, 126, 0)
        LoginBtn.FlatAppearance.BorderSize = 0
        LoginBtn.FlatStyle = FlatStyle.Flat
        LoginBtn.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginBtn.ForeColor = Color.White
        LoginBtn.Location = New Point(330, 324)
        LoginBtn.Margin = New Padding(3, 2, 3, 2)
        LoginBtn.Name = "LoginBtn"
        LoginBtn.Size = New Size(250, 32)
        LoginBtn.TabIndex = 15
        LoginBtn.Text = "LOGIN"
        LoginBtn.UseVisualStyleBackColor = False
        LoginBtn.Cursor = Cursors.Hand
        LoginBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 112, 0)
        Dim buttonPath As New Drawing2D.GraphicsPath()
        buttonPath.AddArc(0, 0, 16, 16, 180, 90)
        buttonPath.AddArc(LoginBtn.Width - 16, 0, 16, 16, 270, 90)
        buttonPath.AddArc(LoginBtn.Width - 16, LoginBtn.Height - 16, 16, 16, 0, 90)
        buttonPath.AddArc(0, LoginBtn.Height - 16, 16, 16, 90, 90)
        buttonPath.CloseFigure()
        LoginBtn.Region = New Region(buttonPath)
        buttonPath.Dispose()
        AddHandler LoginBtn.SizeChanged,
            Sub(sender As Object, e As EventArgs)
                Dim path As New Drawing2D.GraphicsPath()
                Dim corner As Integer = Math.Min(18, Math.Max(1, LoginBtn.Height \ 2))
                path.AddArc(0, 0, corner, corner, 180, 90)
                path.AddArc(LoginBtn.Width - corner, 0, corner, corner, 270, 90)
                path.AddArc(LoginBtn.Width - corner, LoginBtn.Height - corner, corner, corner, 0, 90)
                path.AddArc(0, LoginBtn.Height - corner, corner, corner, 90, 90)
                path.CloseFigure()
                Dim previousRegion = LoginBtn.Region
                LoginBtn.Region = New Region(path)
                If previousRegion IsNot Nothing Then previousRegion.Dispose()
                path.Dispose()
            End Sub
        ' 
        ' PasswordField
        ' 
        PasswordField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        PasswordField.BackColor = SystemColors.Window
        PasswordField.Font = New Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        PasswordField.Location = New Point(330, 258)
        PasswordField.Margin = New Padding(3, 2, 3, 2)
        PasswordField.Name = "PasswordField"
        PasswordField.PlaceholderText = "Enter password"
        PasswordField.Size = New Size(250, 26)
        PasswordField.TabIndex = 14
        PasswordField.UseSystemPasswordChar = True
        PasswordField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        PasswordField.BackColor = SystemColors.Window
        PasswordField.Location = New Point(330, 258)
        PasswordField.Size = New Size(250, 26)
        ' 
        ' UsernameField
        ' 
        UsernameField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        UsernameField.BackColor = SystemColors.Window
        UsernameField.Font = New Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        UsernameField.Location = New Point(330, 208)
        UsernameField.Margin = New Padding(3, 2, 3, 2)
        UsernameField.Name = "UsernameField"
        UsernameField.PlaceholderText = "Enter username"
        UsernameField.Size = New Size(250, 26)
        UsernameField.TabIndex = 13
        ' 
        ' Password
        ' 
        Password.Anchor = AnchorStyles.Right
        Password.AutoSize = True
        Password.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Password.Location = New Point(330, 239)
        Password.Name = "Password"
        Password.Size = New Size(82, 21)
        Password.TabIndex = 12
        Password.Text = "Password"
        ' 
        ' Username
        ' 
        Username.Anchor = AnchorStyles.Right
        Username.AutoSize = True
        Username.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Username.Location = New Point(330, 189)
        Username.Name = "Username"
        Username.Size = New Size(87, 21)
        Username.TabIndex = 11
        Username.Text = "Username"
        ' 
        ' DrugstoreName
        ' 
        DrugstoreName.Anchor = AnchorStyles.None
        DrugstoreName.AutoSize = False
        DrugstoreName.Font = New Font("Segoe UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DrugstoreName.ForeColor = Color.FromArgb(45, 58, 51)
        DrugstoreName.Location = New Point(313, 119)
        DrugstoreName.Name = "DrugstoreName"
        DrugstoreName.Size = New Size(284, 28)
        DrugstoreName.TabIndex = 10
        DrugstoreName.Text = "Cabrera's Drugstore and Medical Supplies"
        DrugstoreName.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' imageLogin
        ' 
        imageLogin.BackColor = Color.FromArgb(246, 157, 65)
        imageLogin.Image = Nothing
        imageLogin.Location = New Point(0, 0)
        imageLogin.Margin = New Padding(3, 2, 3, 2)
        imageLogin.Name = "imageLogin"
        imageLogin.Size = New Size(304, 386)
        imageLogin.SizeMode = PictureBoxSizeMode.Normal
        imageLogin.TabIndex = 0
        imageLogin.TabStop = False
        AddHandler imageLogin.Paint,
            Sub(sender As Object, e As PaintEventArgs)
                Dim bounds = imageLogin.ClientRectangle
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Using gradient As New Drawing2D.LinearGradientBrush(bounds,
                        Color.FromArgb(255, 192, 88), Color.FromArgb(239, 126, 66),
                        Drawing2D.LinearGradientMode.ForwardDiagonal)
                    e.Graphics.FillRectangle(gradient, bounds)
                End Using
                Using lightShape As New SolidBrush(Color.FromArgb(48, Color.White))
                    e.Graphics.FillPolygon(lightShape, {
                        New Point(0, 0), New Point(bounds.Width, 0),
                        New Point(bounds.Width \ 2, bounds.Height \ 3), New Point(0, bounds.Height \ 4)})
                End Using
                Using warmShape As New SolidBrush(Color.FromArgb(35, 192, 82, 43))
                    e.Graphics.FillPolygon(warmShape, {
                        New Point(0, CInt(bounds.Height * 0.65)),
                        New Point(CInt(bounds.Width * 0.64), CInt(bounds.Height * 0.42)),
                        New Point(bounds.Width, CInt(bounds.Height * 0.64)),
                        New Point(bounds.Width, bounds.Height), New Point(0, bounds.Height)})
                End Using
                Using detailPen As New Pen(Color.FromArgb(54, Color.White), 1.0F)
                    e.Graphics.DrawArc(detailPen, CInt(bounds.Width * 0.45), -40,
                                       CInt(bounds.Width * 0.70), 170, 15, 145)
                    e.Graphics.DrawLine(detailPen, 0, CInt(bounds.Height * 0.25), bounds.Width, 0)
                End Using
            End Sub
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(243, 245, 247)
        Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ClientSize = New Size(1442, 745)
        Controls.Add(NavBar)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Login"
        Text = "Login"
        WindowState = FormWindowState.Maximized
        NavBar.ResumeLayout(False)
        NavBar.PerformLayout()
        CType(hidepass, ComponentModel.ISupportInitialize).EndInit()
        CType(passvisible, ComponentModel.ISupportInitialize).EndInit()
        CType(picHide, ComponentModel.ISupportInitialize).EndInit()
        CType(View, ComponentModel.ISupportInitialize).EndInit()
        CType(LoginIcon, ComponentModel.ISupportInitialize).EndInit()
        CType(imageLogin, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents NavBar As Panel
    Friend WithEvents LoginLabel As Label
    Friend WithEvents picHide As PictureBox
    Friend WithEvents View As PictureBox
    Friend WithEvents LoginIcon As PictureBox
    Friend WithEvents LoginBtn As Button
    Friend WithEvents PasswordField As TextBox
    Friend WithEvents UsernameField As TextBox
    Friend WithEvents Password As Label
    Friend WithEvents Username As Label
    Friend WithEvents DrugstoreName As Label
    Friend WithEvents imageLogin As PictureBox
    Friend WithEvents passvisible As PictureBox
    Friend WithEvents hidepass As PictureBox

End Class
