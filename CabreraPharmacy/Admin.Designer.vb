<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Admin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        logoutadmin_button = New Button()
        welcomeadmin_label = New Label()
        SuspendLayout()
        ' 
        ' logoutadmin_button
        ' 
        logoutadmin_button.Location = New Point(458, 288)
        logoutadmin_button.Name = "logoutadmin_button"
        logoutadmin_button.Size = New Size(182, 73)
        logoutadmin_button.TabIndex = 7
        logoutadmin_button.Text = "Logout"
        logoutadmin_button.UseVisualStyleBackColor = True
        ' 
        ' welcomeadmin_label
        ' 
        welcomeadmin_label.AutoSize = True
        welcomeadmin_label.Location = New Point(504, 254)
        welcomeadmin_label.Name = "welcomeadmin_label"
        welcomeadmin_label.Size = New Size(115, 20)
        welcomeadmin_label.TabIndex = 6
        welcomeadmin_label.Text = "welcome admin"
        ' 
        ' Admin
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1098, 615)
        Controls.Add(logoutadmin_button)
        Controls.Add(welcomeadmin_label)
        Name = "Admin"
        Text = "Admin"
        WindowState = FormWindowState.Maximized
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents logoutadmin_button As Button
    Friend WithEvents welcomeadmin_label As Label
End Class
