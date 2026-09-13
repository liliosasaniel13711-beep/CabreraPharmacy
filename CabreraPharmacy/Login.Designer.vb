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
        txtUsername = New TextBox()
        txtPassword = New TextBox()
        Username = New Label()
        Password = New Label()
        btnLogin = New Button()
        SuspendLayout()
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(444, 296)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(226, 27)
        txtUsername.TabIndex = 0
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(444, 347)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(226, 27)
        txtPassword.TabIndex = 1
        txtPassword.UseSystemPasswordChar = True
        ' 
        ' Username
        ' 
        Username.AutoSize = True
        Username.Location = New Point(339, 303)
        Username.Name = "Username"
        Username.Size = New Size(75, 20)
        Username.TabIndex = 2
        Username.Text = "Username"
        ' 
        ' Password
        ' 
        Password.AutoSize = True
        Password.Location = New Point(339, 354)
        Password.Name = "Password"
        Password.Size = New Size(70, 20)
        Password.TabIndex = 3
        Password.Text = "Password"
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(473, 409)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(182, 73)
        btnLogin.TabIndex = 4
        btnLogin.Text = "Login"
        btnLogin.UseVisualStyleBackColor = True
        ' 
        ' Login
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1144, 603)
        Controls.Add(btnLogin)
        Controls.Add(Password)
        Controls.Add(Username)
        Controls.Add(txtPassword)
        Controls.Add(txtUsername)
        Name = "Login"
        Text = "Login"
        WindowState = FormWindowState.Maximized
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Username As System.Windows.Forms.Label
    Friend WithEvents Password As System.Windows.Forms.Label
    Friend WithEvents btnLogin As System.Windows.Forms.Button

End Class