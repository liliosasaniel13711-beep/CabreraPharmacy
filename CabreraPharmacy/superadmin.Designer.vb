<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class superadmin
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
        Me.logoutsuperadmin_button = New Button()
        welcomesuperadmin_label = New Label()
        SuspendLayout()
        ' 
        ' logoutsuperadmin_button
        ' 
        Me.logoutsuperadmin_button.Location = New Point(451, 308)
        Me.logoutsuperadmin_button.Name = "logoutsuperadmin_button"
        Me.logoutsuperadmin_button.Size = New Size(182, 73)
        Me.logoutsuperadmin_button.TabIndex = 9
        Me.logoutsuperadmin_button.Text = "Logout"
        Me.logoutsuperadmin_button.UseVisualStyleBackColor = True
        ' 
        ' welcomesuperadmin_label
        ' 
        welcomesuperadmin_label.AutoSize = True
        welcomesuperadmin_label.Location = New Point(464, 271)
        welcomesuperadmin_label.Name = "welcomesuperadmin_label"
        welcomesuperadmin_label.Size = New Size(155, 20)
        welcomesuperadmin_label.TabIndex = 8
        welcomesuperadmin_label.Text = "welcome  superadmin"
        ' 
        ' superadmin
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1084, 654)
        Controls.Add(Me.logoutsuperadmin_button)
        Controls.Add(welcomesuperadmin_label)
        Name = "superadmin"
        Text = "superadmin"
        WindowState = FormWindowState.Maximized
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents logoutsuperadmin_button As Button
    Friend WithEvents welcomesuperadmin_label As Label
End Class
