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
        ' 
        ' hidepass
        ' 
        hidepass.Anchor = AnchorStyles.None
        hidepass.ErrorImage = CType(resources.GetObject("hidepass.ErrorImage"), Image)
        hidepass.Image = CType(resources.GetObject("hidepass.Image"), Image)
        hidepass.Location = New Point(558, 251)
        hidepass.Name = "hidepass"
        hidepass.Size = New Size(16, 17)
        hidepass.SizeMode = PictureBoxSizeMode.StretchImage
        hidepass.TabIndex = 23
        hidepass.TabStop = False
        ' 
        ' passvisible
        ' 
        passvisible.Anchor = AnchorStyles.None
        passvisible.ErrorImage = CType(resources.GetObject("passvisible.ErrorImage"), Image)
        passvisible.Image = CType(resources.GetObject("passvisible.Image"), Image)
        passvisible.Location = New Point(558, 251)
        passvisible.Name = "passvisible"
        passvisible.Size = New Size(16, 17)
        passvisible.SizeMode = PictureBoxSizeMode.StretchImage
        passvisible.TabIndex = 22
        passvisible.TabStop = False
        ' 
        ' LoginLabel
        ' 
        LoginLabel.Anchor = AnchorStyles.Right
        LoginLabel.AutoSize = True
        LoginLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginLabel.ForeColor = Color.SeaGreen
        LoginLabel.Location = New Point(234, 151)
        LoginLabel.Name = "LoginLabel"
        LoginLabel.Size = New Size(63, 25)
        LoginLabel.TabIndex = 19
        LoginLabel.Text = "Login"
        ' 
        ' picHide
        ' 
        picHide.Anchor = AnchorStyles.None
        picHide.Location = New Point(551, 254)
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
        View.Location = New Point(551, 229)
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
        LoginIcon.Location = New Point(415, 46)
        LoginIcon.Margin = New Padding(3, 2, 3, 2)
        LoginIcon.Name = "LoginIcon"
        LoginIcon.Size = New Size(91, 76)
        LoginIcon.SizeMode = PictureBoxSizeMode.StretchImage
        LoginIcon.TabIndex = 16
        LoginIcon.TabStop = False
        ' 
        ' LoginBtn
        ' 
        LoginBtn.Anchor = AnchorStyles.None
        LoginBtn.BackColor = Color.DarkOrange
        LoginBtn.FlatAppearance.BorderSize = 0
        LoginBtn.FlatStyle = FlatStyle.Flat
        LoginBtn.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginBtn.ForeColor = Color.White
        LoginBtn.Location = New Point(484, 290)
        LoginBtn.Margin = New Padding(3, 2, 3, 2)
        LoginBtn.Name = "LoginBtn"
        LoginBtn.Size = New Size(97, 28)
        LoginBtn.TabIndex = 15
        LoginBtn.Text = "LOGIN"
        LoginBtn.UseVisualStyleBackColor = False
        ' 
        ' PasswordField
        ' 
        PasswordField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        PasswordField.BackColor = SystemColors.Window
        PasswordField.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        PasswordField.Location = New Point(342, 250)
        PasswordField.Margin = New Padding(3, 2, 3, 2)
        PasswordField.Name = "PasswordField"
        PasswordField.PlaceholderText = "Enter password"
        PasswordField.Size = New Size(239, 29)
        PasswordField.TabIndex = 14
        PasswordField.UseSystemPasswordChar = True
        ' 
        ' UsernameField
        ' 
        UsernameField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        UsernameField.BackColor = SystemColors.Window
        UsernameField.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        UsernameField.Location = New Point(342, 200)
        UsernameField.Margin = New Padding(3, 2, 3, 2)
        UsernameField.Name = "UsernameField"
        UsernameField.PlaceholderText = "Enter username"
        UsernameField.Size = New Size(239, 29)
        UsernameField.TabIndex = 13
        ' 
        ' Password
        ' 
        Password.Anchor = AnchorStyles.Right
        Password.AutoSize = True
        Password.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Password.Location = New Point(342, 232)
        Password.Name = "Password"
        Password.Size = New Size(82, 21)
        Password.TabIndex = 12
        Password.Text = "Password"
        ' 
        ' Username
        ' 
        Username.Anchor = AnchorStyles.Right
        Username.AutoSize = True
        Username.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Username.Location = New Point(342, 182)
        Username.Name = "Username"
        Username.Size = New Size(87, 21)
        Username.TabIndex = 11
        Username.Text = "Username"
        ' 
        ' DrugstoreName
        ' 
        DrugstoreName.Anchor = AnchorStyles.None
        DrugstoreName.AutoSize = True
        DrugstoreName.Font = New Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DrugstoreName.Location = New Point(362, 143)
        DrugstoreName.Name = "DrugstoreName"
        DrugstoreName.Size = New Size(297, 19)
        DrugstoreName.TabIndex = 10
        DrugstoreName.Text = "Cabrera's Drugstore and Medical Supplies "
        DrugstoreName.TextAlign = ContentAlignment.TopCenter
        ' 
        ' imageLogin
        ' 
        imageLogin.Image = My.Resources.Resources.Untitled_design__2_
        imageLogin.Location = New Point(0, 0)
        imageLogin.Margin = New Padding(3, 2, 3, 2)
        imageLogin.Name = "imageLogin"
        imageLogin.Size = New Size(304, 386)
        imageLogin.SizeMode = PictureBoxSizeMode.StretchImage
        imageLogin.TabIndex = 0
        imageLogin.TabStop = False
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
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