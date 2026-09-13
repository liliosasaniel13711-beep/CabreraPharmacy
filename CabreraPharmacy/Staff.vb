Public Class Staff

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        welcomestaff_label.Text = "Welcome, " & CurrentFullName & " (Staff)"
    End Sub

    Private Sub logoutstaff_button_Click(sender As Object, e As EventArgs) Handles logoutstaff_button.Click
        Close()
    End Sub

End Class