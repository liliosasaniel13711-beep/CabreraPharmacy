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
        PharName = New Label()
        SuspendLayout()
        ' 
        ' logoutadmin_button
        ' 
        logoutadmin_button.Location = New Point(12, 997)
        logoutadmin_button.Margin = New Padding(3, 2, 3, 2)
        logoutadmin_button.Name = "logoutadmin_button"
        logoutadmin_button.Size = New Size(159, 33)
        logoutadmin_button.TabIndex = 7
        logoutadmin_button.Text = "Logout"
        logoutadmin_button.UseVisualStyleBackColor = True
        ' 
        ' welcomeadmin_label
        ' 
        welcomeadmin_label.AutoSize = True
        welcomeadmin_label.Location = New Point(1435, 48)
        welcomeadmin_label.Name = "welcomeadmin_label"
        welcomeadmin_label.Size = New Size(92, 15)
        welcomeadmin_label.TabIndex = 6
        welcomeadmin_label.Text = "welcome admin"
        ' 
        ' PharName
        ' 
        PharName.AutoSize = True
        PharName.Location = New Point(35, 21)
        PharName.Name = "PharName"
        PharName.Size = New Size(41, 15)
        PharName.TabIndex = 8
        PharName.Text = "Cabrera's"
        ' 
        ' Admin
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1904, 1041)
        Controls.Add(PharName)
        Controls.Add(logoutadmin_button)
        Controls.Add(welcomeadmin_label)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Admin"
        Text = "Admin"
        WindowState = FormWindowState.Maximized
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents logoutadmin_button As Button
    Friend WithEvents welcomeadmin_label As Label
    Friend WithEvents PharName As Label
End Class
