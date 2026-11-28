Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Module SidebarBranding
    Public Sub Apply(sidebar As Panel, logo As PictureBox, brandName As Label, logoutButton As Button)
        If sidebar Is Nothing Then Return

        If logo IsNot Nothing Then
            logo.Image = My.Resources.Resources.ed8bb44f_0265_4bb2_a677_771c15e67760
            logo.BackColor = Color.White
            logo.SizeMode = PictureBoxSizeMode.Zoom
            logo.SetBounds(Math.Max(0, (sidebar.ClientSize.Width - 76) \ 2), 22, 76, 76)
            SetRoundedRegion(logo, 10)
        End If

        If brandName IsNot Nothing Then
            brandName.Text = "Cabrera's"
            brandName.AutoSize = False
            brandName.SetBounds(10, 101, sidebar.ClientSize.Width - 20, 24)
            brandName.TextAlign = ContentAlignment.MiddleCenter
            brandName.Font = New Font("Segoe UI", 13.0F, FontStyle.Bold)
            brandName.ForeColor = Color.White
            brandName.BackColor = Color.Transparent
        End If

        Dim subtitle = TryCast(sidebar.Controls("sidebarBrandSubtitle"), Label)
        If subtitle Is Nothing Then
            subtitle = New Label With {.Name = "sidebarBrandSubtitle", .AutoSize = False}
            sidebar.Controls.Add(subtitle)
        End If
        subtitle.Text = "Drugstore and Medical Supplies"
        subtitle.SetBounds(10, 125, sidebar.ClientSize.Width - 20, 30)
        subtitle.TextAlign = ContentAlignment.TopCenter
        subtitle.Font = New Font("Segoe UI", 8.5F, FontStyle.Regular)
        subtitle.ForeColor = Color.FromArgb(218, 237, 225)
        subtitle.BackColor = Color.Transparent
        subtitle.BringToFront()

        If logoutButton Is Nothing Then Return
        logoutButton.Text = "↪   Log Out"
        logoutButton.Size = New Size(sidebar.ClientSize.Width - 28, 40)
        logoutButton.Location = New Point(14, Math.Max(12, sidebar.ClientSize.Height - 56))
        logoutButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        logoutButton.BackColor = Color.FromArgb(16, 122, 72)
        logoutButton.ForeColor = Color.White
        logoutButton.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        logoutButton.TextAlign = ContentAlignment.MiddleCenter
        logoutButton.FlatStyle = FlatStyle.Flat
        logoutButton.FlatAppearance.BorderSize = 0
        logoutButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(24, 140, 84)
        logoutButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(12, 96, 56)
        logoutButton.UseVisualStyleBackColor = False
        logoutButton.Cursor = Cursors.Hand
        SetRoundedRegion(logoutButton, 12)
        If logoutButton.AccessibleDescription <> "RoundedSidebarLogout" Then
            logoutButton.AccessibleDescription = "RoundedSidebarLogout"
            AddHandler logoutButton.Resize, Sub(sender, e) SetRoundedRegion(logoutButton, 12)
        End If
    End Sub

    Private Sub SetRoundedRegion(control As Control, radius As Integer)
        If control Is Nothing OrElse control.Width <= 0 OrElse control.Height <= 0 Then Return
        Dim diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height))
        Dim bounds As New Rectangle(0, 0, control.Width, control.Height)
        Using path As New GraphicsPath()
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90)
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90)
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90)
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90)
            path.CloseFigure()
            Dim oldRegion = control.Region
            control.Region = New Region(path)
            If oldRegion IsNot Nothing Then oldRegion.Dispose()
        End Using
    End Sub
End Module
