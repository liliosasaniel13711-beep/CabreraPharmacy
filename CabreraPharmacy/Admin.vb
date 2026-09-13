Public Class Admin

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        welcomeadmin_label.Text = "Welcome, " & CurrentFullName & " (Admin)"
    End Sub

    Private Sub logoutadmin_button_Click(sender As Object, e As EventArgs) Handles logoutadmin_button.Click
        Close()
    End Sub

End Class