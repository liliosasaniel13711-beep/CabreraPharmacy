<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Staff
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
        welcomestaff_label = New Label()
        logoutstaff_button = New Button()
        SuspendLayout()
        ' 
        ' welcomestaff_label
        ' 
        welcomestaff_label.AutoSize = True
        welcomestaff_label.Location = New Point(544, 267)
        welcomestaff_label.Name = "welcomestaff_label"
        welcomestaff_label.Size = New Size(102, 20)
        welcomestaff_label.TabIndex = 0
        welcomestaff_label.Text = "welcome staff"
        ' 
        ' logoutstaff_button
        ' 
        logoutstaff_button.Location = New Point(498, 301)
        logoutstaff_button.Name = "logoutstaff_button"
        logoutstaff_button.Size = New Size(182, 73)
        logoutstaff_button.TabIndex = 5
        logoutstaff_button.Text = "Logout"
        logoutstaff_button.UseVisualStyleBackColor = True
        ' 
        ' Staff
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1179, 674)
        Controls.Add(logoutstaff_button)
        Controls.Add(welcomestaff_label)
        Name = "Staff"
        Text = "Staff"
        WindowState = FormWindowState.Maximized
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents welcomestaff_label As Label
    Friend WithEvents logoutstaff_button As Button
End Class
