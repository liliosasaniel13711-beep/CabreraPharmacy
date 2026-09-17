Imports System.Drawing
Imports System.Windows.Forms

Public Class Admin

    ' ColorDef
    Private darkGreen As Color = Color.FromArgb(20, 100, 60)
    Private activeGreen As Color = Color.FromArgb(25, 120, 75)
    Private lightGray As Color = Color.FromArgb(245, 245, 245)
    Private yellow As Color = Color.FromArgb(255, 215, 0)

    ' Navigation state
    Private navButtons As New List(Of Button)()
    Private contentPanels As New Dictionary(Of String, Panel)()
    Private titleLabel As Label
    Private subtitleLabel As Label
    Private bodyPanel As Panel

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDashboard()
        ShowView("Dashboard")
        'wekcome logiccc
        welcomeadmin_label.Text = "Welcome, " & CurrentFullName & " (Admin)"
    End Sub

    Private Sub logoutadmin_button_Click(sender As Object, e As EventArgs) Handles logoutadmin_button.Click
        Close()
    End Sub

    Private Sub welcomeadmin_label_Click(sender As Object, e As EventArgs) Handles welcomeadmin_label.Click

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles PharName.Click

    End Sub

    Private Sub SetupDashboard()
        Me.BackColor = Color.White
        Me.Text = "Cabrera's Drugstore - Dashboard"

        ' Main Layout
        Dim mainLayout As New TableLayoutPanel()
        mainLayout.Dock = DockStyle.Fill
        mainLayout.ColumnCount = 2
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 220))
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        mainLayout.Margin = New Padding(0)
        Me.Controls.Add(mainLayout)

        ' Sidebar Nav
        Dim sidebar As New Panel()
        sidebar.BackColor = Color.White
        sidebar.Dock = DockStyle.Fill
        sidebar.Margin = New Padding(0)
        mainLayout.Controls.Add(sidebar, 0, 0)

        Dim logoBox As New PictureBox()
        logoBox.BackColor = yellow
        logoBox.Size = New Size(50, 50)
        logoBox.Location = New Point(20, 20)
        sidebar.Controls.Add(logoBox)

        sidebar.Controls.Add(PharName)
        PharName.Text = "Cabrera’s" & vbCrLf & "Drugstore and Medical Supplies"
        PharName.ForeColor = darkGreen
        PharName.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        PharName.AutoSize = True
        PharName.MaximumSize = New Size(130, 0)
        PharName.Location = New Point(80, 20)
        PharName.BackColor = Color.Transparent
        PharName.BringToFront()

        AddHandler sidebar.Layout, Sub(s, ev) logoBox.Location = New Point(20, PharName.Top + (PharName.Height - logoBox.Height) \ 2)

        Dim navItems As String() = {"Dashboard", "Price Manager", "Inventory", "Reports"}
        Dim yPos As Integer = 120
        For Each item As String In navItems
            Dim navButton As New Button()
            navButton.Text = GetNavIcon(item) & "   " & item
            navButton.Tag = item
            navButton.FlatStyle = FlatStyle.Flat
            navButton.FlatAppearance.BorderSize = 0
            navButton.Font = New Font("Segoe UI", 11, FontStyle.Bold)
            navButton.Size = New Size(180, 45)
            navButton.Location = New Point(20, yPos)
            navButton.TextAlign = ContentAlignment.MiddleLeft
            navButton.Padding = New Padding(10, 0, 0, 0)
            navButton.Cursor = Cursors.Hand
            AddHandler navButton.Click, AddressOf NavButton_Click

            sidebar.Controls.Add(navButton)
            navButtons.Add(navButton)
            yPos += 55
        Next

        'Log Out Button
        sidebar.Controls.Add(logoutadmin_button)
        logoutadmin_button.FlatStyle = FlatStyle.Flat
        logoutadmin_button.FlatAppearance.BorderSize = 1
        logoutadmin_button.FlatAppearance.BorderColor = darkGreen
        logoutadmin_button.ForeColor = darkGreen
        logoutadmin_button.BackColor = Color.White
        logoutadmin_button.Size = New Size(180, 45)
        logoutadmin_button.Location = New Point(20, Me.ClientSize.Height - 80)
        logoutadmin_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutadmin_button.BringToFront()

        ' Main Content Area
        Dim rightLayout As New TableLayoutPanel()
        rightLayout.Dock = DockStyle.Fill
        rightLayout.RowCount = 2
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100)) ' Header Row
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))  ' Cards Row
        rightLayout.Margin = New Padding(0)
        mainLayout.Controls.Add(rightLayout, 1, 0)

        ' Header Area
        Dim headerPanel As New Panel()
        headerPanel.BackColor = Color.White ' Seamless white top
        headerPanel.Dock = DockStyle.Fill
        headerPanel.Margin = New Padding(0)
        rightLayout.Controls.Add(headerPanel, 0, 0)

        titleLabel = New Label()
        titleLabel.Text = "Dashboard"
        titleLabel.Font = New Font("Segoe UI", 24, FontStyle.Bold)
        titleLabel.ForeColor = darkGreen
        titleLabel.Location = New Point(20, 10)
        titleLabel.AutoSize = True
        headerPanel.Controls.Add(titleLabel)

        subtitleLabel = New Label()
        subtitleLabel.Text = "Overview of Cabrera’s Drugstore and Medical Supplies"
        subtitleLabel.Font = New Font("Segoe UI", 11)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(20, 55)
        subtitleLabel.AutoSize = True
        headerPanel.Controls.Add(subtitleLabel)

        ' User Info sa righy
        Dim userPanel As New Panel()
        userPanel.AutoSize = True
        userPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        userPanel.BackColor = Color.Transparent
        userPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        headerPanel.Controls.Add(userPanel)
        AddHandler headerPanel.Layout, Sub(s, ev) userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)

        Dim userAvatar As New PictureBox()
        userAvatar.BackColor = darkGreen
        userAvatar.Size = New Size(50, 50)
        userAvatar.Location = New Point(0, 10)
        userPanel.Controls.Add(userAvatar)

        ' Welcome Label
        userPanel.Controls.Add(welcomeadmin_label)
        welcomeadmin_label.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        welcomeadmin_label.ForeColor = Color.Black
        welcomeadmin_label.Location = New Point(60, 10)
        welcomeadmin_label.BackColor = Color.Transparent
        welcomeadmin_label.AutoSize = True
        welcomeadmin_label.BringToFront()

        Dim userRoleLabel As New Label()
        userRoleLabel.Text = "Staff / Pharmacy Assistant"
        userRoleLabel.Font = New Font("Segoe UI", 9)
        userRoleLabel.ForeColor = darkGreen
        userRoleLabel.Location = New Point(60, 32)
        userRoleLabel.AutoSize = True
        userPanel.Controls.Add(userRoleLabel)

        bodyPanel = New Panel()
        bodyPanel.Dock = DockStyle.Fill
        bodyPanel.BackColor = lightGray
        bodyPanel.Padding = New Padding(20)
        bodyPanel.Margin = New Padding(0)
        rightLayout.Controls.Add(bodyPanel, 0, 1)

        ' Dashboard view (gridss)
        Dim dashboardView As New Panel()
        dashboardView.Dock = DockStyle.Fill
        dashboardView.BackColor = lightGray

        Dim cardGrid As New TableLayoutPanel()
        cardGrid.Dock = DockStyle.Fill
        cardGrid.ColumnCount = 4
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25))
        cardGrid.RowCount = 2
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 15))
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        dashboardView.Controls.Add(cardGrid)

        Dim todayDate As String = DateTime.Now.ToString("MMMM dd, yyyy")

        cardGrid.Controls.Add(CreateKpiCard("Today’s Sale", "0.00", todayDate), 0, 0)
        cardGrid.Controls.Add(CreateKpiCard("Retail Sales", "0.00", "0 Transactions"), 1, 0)
        cardGrid.Controls.Add(CreateKpiCard("Wholesale Sales", "0.00", "0 Transactions"), 2, 0)
        cardGrid.Controls.Add(CreateKpiCard("Total Products", "0", "All Categories"), 3, 0)
        cardGrid.Controls.Add(CreateKpiCard("Low Stock", "0", "Need reordering"), 0, 1)
        cardGrid.Controls.Add(CreateKpiCard("Expired", "0", "Within 0 days"), 1, 1)
        cardGrid.Controls.Add(CreateKpiCard("Near Expiry", "0", "Requires disposal"), 2, 1)
        cardGrid.Controls.Add(CreateKpiCard("Today’s Expenses", "0.00", "No expenses recorded today"), 3, 1)

        ' Register all switchable views
        RegisterView("Dashboard", dashboardView)
        RegisterView("Price Manager", CreatePlaceholderView("Price Manager", "Manage retail and wholesale pricing for all products."))
        RegisterView("Inventory", CreatePlaceholderView("Inventory", "Track stock levels, expiring items, and product categories."))
        RegisterView("Reports", CreatePlaceholderView("Reports", "Generate sales, inventory, and expense reports."))
    End Sub

    ' Navigation Helpers
    Private Sub RegisterView(name As String, view As Panel)
        view.Visible = False
        contentPanels.Add(name, view)
        bodyPanel.Controls.Add(view)
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs)
        Dim btn = TryCast(sender, Button)
        If btn IsNot Nothing Then
            ShowView(CStr(btn.Tag))
        End If
    End Sub

    Private Sub ShowView(viewName As String)
        For Each kvp In contentPanels
            kvp.Value.Visible = (kvp.Key = viewName)
        Next

        For Each btn In navButtons
            Dim isActive As Boolean = (CStr(btn.Tag) = viewName)
            btn.BackColor = If(isActive, darkGreen, Color.White)
            btn.ForeColor = If(isActive, Color.White, darkGreen)
        Next

        titleLabel.Text = viewName
        subtitleLabel.Text = GetSubtitle(viewName)
    End Sub

    Private Function GetSubtitle(viewName As String) As String
        Select Case viewName
            Case "Dashboard"
                Return "Overview of Cabrera’s Drugstore and Medical Supplies"
            Case "Price Manager"
                Return "Manage retail and wholesale product prices"
            Case "Inventory"
                Return "Track stock levels and product expiry"
            Case "Reports"
                Return "Sales and inventory reports"
            Case Else
                Return String.Empty
        End Select
    End Function

    Private Function GetNavIcon(item As String) As String
        Select Case item
            Case "Dashboard"
                Return "☰"
            Case "Price Manager"
                Return "◉"
            Case "Inventory"
                Return "▤"
            Case "Reports"
                Return "▦"
            Case Else
                Return "•"
        End Select
    End Function

    Private Function CreatePlaceholderView(title As String, description As String) As Panel
        Dim view As New Panel()
        view.Dock = DockStyle.Fill
        view.BackColor = lightGray

        Dim card As New Panel()
        card.BackColor = Color.White
        card.Dock = DockStyle.Top
        card.Height = 160
        card.Padding = New Padding(20)

        Dim descLabel As New Label()
        descLabel.Text = description
        descLabel.Font = New Font("Segoe UI", 10)
        descLabel.ForeColor = Color.Gray
        descLabel.Dock = DockStyle.Bottom
        descLabel.Height = 40

        Dim headLabel As New Label()
        headLabel.Text = title
        headLabel.Font = New Font("Segoe UI", 16, FontStyle.Bold)
        headLabel.ForeColor = darkGreen
        headLabel.Dock = DockStyle.Top
        headLabel.Height = 50

        card.Controls.Add(descLabel)
        card.Controls.Add(headLabel)
        view.Controls.Add(card)

        Return view
    End Function

    ' KPI Card Helper Function (Updated for Left-Alignment)
    Private Function CreateKpiCard(title As String, value As String, footer As String) As Panel
        Dim card As New Panel()
        card.BackColor = Color.White
        card.Padding = New Padding(20)
        card.Margin = New Padding(10)
        card.Dock = DockStyle.Fill

        ' Footer
        Dim footerLabel As New Label()
        footerLabel.Text = footer
        footerLabel.Font = New Font("Segoe UI", 9)
        footerLabel.Dock = DockStyle.Bottom
        footerLabel.ForeColor = Color.Gray
        card.Controls.Add(footerLabel)

        ' Add Value
        Dim valueLabel As New Label()
        valueLabel.Text = value
        valueLabel.Font = New Font("Segoe UI", 24)
        valueLabel.Dock = DockStyle.Fill
        valueLabel.TextAlign = ContentAlignment.MiddleLeft
        valueLabel.ForeColor = Color.Black
        card.Controls.Add(valueLabel)

        ' Add Title
        Dim titleLabel As New Label()
        titleLabel.Text = title
        titleLabel.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        titleLabel.Dock = DockStyle.Top
        titleLabel.ForeColor = Color.Black
        card.Controls.Add(titleLabel)

        Return card
    End Function

End Class