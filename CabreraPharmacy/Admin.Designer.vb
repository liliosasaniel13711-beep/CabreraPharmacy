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
        mainLayout = New TableLayoutPanel()
        sidebar = New Panel()
        logoBox = New PictureBox()
        PharName = New Label()
        navDashboard = New Button()
        navPriceManager = New Button()
        navInventory = New Button()
        navReports = New Button()
        logoutadmin_button = New Button()
        rightLayout = New TableLayoutPanel()
        headerPanel = New Panel()
        titleLabel = New Label()
        subtitleLabel = New Label()
        userPanel = New Panel()
        userAvatar = New PictureBox()
        welcomeadmin_label = New Label()
        userRoleLabel = New Label()
        bodyPanel = New Panel()
        dashboardView = New Panel()
        dashboardLayout = New TableLayoutPanel()
        summaryArea = New TableLayoutPanel()
        dashboardDateLabel = New Label()
        cardGrid = New TableLayoutPanel()
        card1 = New Panel()
        card1Value = New Label()
        card1Footer = New Label()
        card1Title = New Label()
        card2 = New Panel()
        card2Value = New Label()
        card2Footer = New Label()
        card2Title = New Label()
        card3 = New Panel()
        card3Value = New Label()
        card3Footer = New Label()
        card3Title = New Label()
        card4 = New Panel()
        card4Value = New Label()
        card4Footer = New Label()
        card4Title = New Label()
        card5 = New Panel()
        card5Value = New Label()
        card5Footer = New Label()
        card5Title = New Label()
        card6 = New Panel()
        card6Value = New Label()
        card6Footer = New Label()
        card6Title = New Label()
        card7 = New Panel()
        card7Value = New Label()
        card7Footer = New Label()
        card7Title = New Label()
        card8 = New Panel()
        card8Value = New Label()
        card8Footer = New Label()
        card8Title = New Label()
        middleRow = New TableLayoutPanel()
        salesTrendPanel = New Panel()
        salesTrendTitle = New Label()
        salesTrendLegend = New Label()
        trendFiltersPanel = New FlowLayoutPanel()
        trendThisYearButton = New Button()
        trendThisMonthButton = New Button()
        trend7DaysButton = New Button()
        salesTrendPlot = New Panel()
        revenuePanel = New Panel()
        revenueTitle = New Label()
        revenueNetIncomeLabel = New Label()
        bottomRow = New TableLayoutPanel()
        inventoryAlertsPanel = New Panel()
        inventoryAlertsTitle = New Label()
        inventoryViewAllLink = New LinkLabel()
        inventoryAlertLowStock = New Panel()
        inventoryAlertLowStockIcon = New Label()
        inventoryAlertLowStockCaption = New Label()
        inventoryAlertLowStockCount = New Label()
        inventoryAlertNearExpiry = New Panel()
        inventoryAlertNearExpiryIcon = New Label()
        inventoryAlertNearExpiryCaption = New Label()
        inventoryAlertNearExpiryCount = New Label()
        inventoryAlertExpired = New Panel()
        inventoryAlertExpiredIcon = New Label()
        inventoryAlertExpiredCaption = New Label()
        inventoryAlertExpiredCount = New Label()
        quickOverviewPanel = New Panel()
        quickOverviewTitle = New Label()
        quickOverviewRestockIcon = New Label()
        quickOverviewRestock = New Label()
        quickOverviewRestockValue = New Label()
        quickOverviewSeparator1 = New Panel()
        quickOverviewExpiryIcon = New Label()
        quickOverviewExpiry = New Label()
        quickOverviewExpiryValue = New Label()
        quickOverviewSeparator2 = New Panel()
        quickOverviewTransactionsIcon = New Label()
        quickOverviewTransactions = New Label()
        quickOverviewTransactionsValue = New Label()
        priceManagerView = New Panel()
        priceManagerCard = New Panel()
        priceManagerDesc = New Label()
        priceManagerHead = New Label()
        inventoryView = New Panel()
        dgvInventory = New DataGridView()
        inventoryCard = New Panel()
        inventoryDesc = New Label()
        inventoryHead = New Label()
        reportsView = New Panel()
        reportsCard = New Panel()
        reportsDesc = New Label()
        reportsHead = New Label()
        mainLayout.SuspendLayout()
        sidebar.SuspendLayout()
        CType(logoBox, ComponentModel.ISupportInitialize).BeginInit()
        rightLayout.SuspendLayout()
        headerPanel.SuspendLayout()
        userPanel.SuspendLayout()
        CType(userAvatar, ComponentModel.ISupportInitialize).BeginInit()
        bodyPanel.SuspendLayout()
        dashboardView.SuspendLayout()
        dashboardLayout.SuspendLayout()
        summaryArea.SuspendLayout()
        cardGrid.SuspendLayout()
        card1.SuspendLayout()
        card2.SuspendLayout()
        card3.SuspendLayout()
        card4.SuspendLayout()
        card5.SuspendLayout()
        card6.SuspendLayout()
        card7.SuspendLayout()
        card8.SuspendLayout()
        middleRow.SuspendLayout()
        salesTrendPanel.SuspendLayout()
        trendFiltersPanel.SuspendLayout()
        revenuePanel.SuspendLayout()
        bottomRow.SuspendLayout()
        inventoryAlertsPanel.SuspendLayout()
        inventoryAlertLowStock.SuspendLayout()
        inventoryAlertNearExpiry.SuspendLayout()
        inventoryAlertExpired.SuspendLayout()
        quickOverviewPanel.SuspendLayout()
        priceManagerView.SuspendLayout()
        priceManagerCard.SuspendLayout()
        inventoryView.SuspendLayout()
        CType(dgvInventory, ComponentModel.ISupportInitialize).BeginInit()
        inventoryCard.SuspendLayout()
        reportsView.SuspendLayout()
        reportsCard.SuspendLayout()
        SuspendLayout()
        ' 
        ' mainLayout
        ' 
        mainLayout.ColumnCount = 2
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 168.0F))
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        mainLayout.Controls.Add(sidebar, 0, 0)
        mainLayout.Controls.Add(rightLayout, 1, 0)
        mainLayout.Dock = DockStyle.Fill
        mainLayout.Location = New Point(0, 0)
        mainLayout.Margin = New Padding(0)
        mainLayout.Name = "mainLayout"
        mainLayout.RowCount = 1
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        mainLayout.Size = New Size(1684, 791)
        mainLayout.TabIndex = 0
        ' 
        ' sidebar
        ' 
        sidebar.BackColor = Color.FromArgb(CByte(18), CByte(101), CByte(54))
        sidebar.Controls.Add(logoBox)
        sidebar.Controls.Add(PharName)
        sidebar.Controls.Add(navDashboard)
        sidebar.Controls.Add(navPriceManager)
        sidebar.Controls.Add(navInventory)
        sidebar.Controls.Add(navReports)
        sidebar.Controls.Add(logoutadmin_button)
        sidebar.Dock = DockStyle.Fill
        sidebar.Location = New Point(0, 0)
        sidebar.Margin = New Padding(0)
        sidebar.Name = "sidebar"
        sidebar.Size = New Size(168, 791)
        sidebar.TabIndex = 0
        ' 
        ' logoBox
        ' 
        logoBox.BackColor = Color.FromArgb(CByte(255), CByte(215), CByte(0))
        logoBox.Image = My.Resources.Resources._769082459_1379172074148575_7892593615181140804_n
        logoBox.Location = New Point(12, 23)
        logoBox.Name = "logoBox"
        logoBox.Size = New Size(62, 50)
        logoBox.SizeMode = PictureBoxSizeMode.Zoom
        logoBox.TabIndex = 7
        logoBox.TabStop = False
        ' 
        ' PharName
        ' 
        PharName.AutoSize = False
        PharName.BackColor = Color.Transparent
        PharName.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        PharName.ForeColor = Color.White
        PharName.Location = New Point(80, 16)
        PharName.Name = "PharName"
        PharName.Size = New Size(80, 68)
        PharName.TabIndex = 1
        PharName.Text = "Cabrera's" & vbCrLf & "Drugstore and Medical Supplies"
        ' 
        ' navDashboard
        ' 
        navDashboard.BackColor = Color.White
        navDashboard.Cursor = Cursors.Hand
        navDashboard.FlatAppearance.BorderSize = 0
        navDashboard.FlatStyle = FlatStyle.Flat
        navDashboard.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        navDashboard.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navDashboard.Location = New Point(19, 92)
        navDashboard.Name = "navDashboard"
        navDashboard.Padding = New Padding(10, 0, 0, 0)
        navDashboard.Size = New Size(130, 38)
        navDashboard.TabIndex = 2
        navDashboard.Tag = "Dashboard"
        navDashboard.Text = "Dashboard"
        navDashboard.TextAlign = ContentAlignment.MiddleLeft
        navDashboard.UseVisualStyleBackColor = False
        ' 
        ' navPriceManager
        ' 
        navPriceManager.BackColor = Color.FromArgb(CByte(18), CByte(101), CByte(54))
        navPriceManager.Cursor = Cursors.Hand
        navPriceManager.FlatAppearance.BorderSize = 0
        navPriceManager.FlatStyle = FlatStyle.Flat
        navPriceManager.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        navPriceManager.ForeColor = Color.White
        navPriceManager.Location = New Point(19, 138)
        navPriceManager.Name = "navPriceManager"
        navPriceManager.Padding = New Padding(10, 0, 0, 0)
        navPriceManager.Size = New Size(130, 38)
        navPriceManager.TabIndex = 3
        navPriceManager.Tag = "Price Manager"
        navPriceManager.Text = "Price Manager"
        navPriceManager.TextAlign = ContentAlignment.MiddleLeft
        navPriceManager.UseVisualStyleBackColor = False
        ' 
        ' navInventory
        ' 
        navInventory.BackColor = Color.FromArgb(CByte(18), CByte(101), CByte(54))
        navInventory.Cursor = Cursors.Hand
        navInventory.FlatAppearance.BorderSize = 0
        navInventory.FlatStyle = FlatStyle.Flat
        navInventory.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        navInventory.ForeColor = Color.White
        navInventory.Location = New Point(19, 184)
        navInventory.Name = "navInventory"
        navInventory.Padding = New Padding(10, 0, 0, 0)
        navInventory.Size = New Size(130, 38)
        navInventory.TabIndex = 4
        navInventory.Tag = "Inventory"
        navInventory.Text = "Inventory"
        navInventory.TextAlign = ContentAlignment.MiddleLeft
        navInventory.UseVisualStyleBackColor = False
        ' 
        ' navReports
        ' 
        navReports.BackColor = Color.FromArgb(CByte(18), CByte(101), CByte(54))
        navReports.Cursor = Cursors.Hand
        navReports.FlatAppearance.BorderSize = 0
        navReports.FlatStyle = FlatStyle.Flat
        navReports.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        navReports.ForeColor = Color.White
        navReports.Location = New Point(19, 230)
        navReports.Name = "navReports"
        navReports.Padding = New Padding(10, 0, 0, 0)
        navReports.Size = New Size(130, 38)
        navReports.TabIndex = 5
        navReports.Tag = "Reports"
        navReports.Text = "Reports"
        navReports.TextAlign = ContentAlignment.MiddleLeft
        navReports.UseVisualStyleBackColor = False
        ' 
        ' logoutadmin_button
        ' 
        logoutadmin_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutadmin_button.BackColor = Color.FromArgb(CByte(18), CByte(101), CByte(54))
        logoutadmin_button.FlatAppearance.BorderColor = Color.White
        logoutadmin_button.FlatStyle = FlatStyle.Flat
        logoutadmin_button.ForeColor = Color.White
        logoutadmin_button.Location = New Point(19, 711)
        logoutadmin_button.Name = "logoutadmin_button"
        logoutadmin_button.Size = New Size(130, 34)
        logoutadmin_button.TabIndex = 6
        logoutadmin_button.Text = "Log Out"
        logoutadmin_button.UseVisualStyleBackColor = False
        ' 
        ' rightLayout
        ' 
        rightLayout.ColumnCount = 1
        rightLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        rightLayout.Controls.Add(headerPanel, 0, 0)
        rightLayout.Controls.Add(bodyPanel, 0, 1)
        rightLayout.Dock = DockStyle.Fill
        rightLayout.Location = New Point(168, 0)
        rightLayout.Margin = New Padding(0)
        rightLayout.Name = "rightLayout"
        rightLayout.RowCount = 2
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 72.0F))
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        rightLayout.Size = New Size(1516, 791)
        rightLayout.TabIndex = 1
        ' 
        ' headerPanel
        ' 
        headerPanel.BackColor = Color.White
        headerPanel.Controls.Add(titleLabel)
        headerPanel.Controls.Add(subtitleLabel)
        headerPanel.Controls.Add(userPanel)
        headerPanel.Dock = DockStyle.Fill
        headerPanel.Location = New Point(0, 0)
        headerPanel.Margin = New Padding(0)
        headerPanel.Name = "headerPanel"
        headerPanel.Size = New Size(1516, 72)
        headerPanel.TabIndex = 0
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        titleLabel.Location = New Point(20, 6)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(138, 32)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Dashboard"
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 8.0F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(20, 43)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(287, 13)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Overview of Cabrera's Drugstore and Medical Supplies"
        ' 
        ' userPanel
        ' 
        userPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        userPanel.AutoSize = True
        userPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        userPanel.BackColor = Color.Transparent
        userPanel.Controls.Add(userAvatar)
        userPanel.Controls.Add(welcomeadmin_label)
        userPanel.Controls.Add(userRoleLabel)
        userPanel.Location = New Point(1218, 10)
        userPanel.Name = "userPanel"
        userPanel.Size = New Size(226, 48)
        userPanel.TabIndex = 2
        ' 
        ' userAvatar
        ' 
        userAvatar.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userAvatar.Location = New Point(185, 7)
        userAvatar.Name = "userAvatar"
        userAvatar.Size = New Size(38, 38)
        userAvatar.TabIndex = 0
        userAvatar.TabStop = False
        ' 
        ' welcomeadmin_label
        ' 
        welcomeadmin_label.AutoSize = True
        welcomeadmin_label.BackColor = Color.Transparent
        welcomeadmin_label.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        welcomeadmin_label.ForeColor = Color.Black
        welcomeadmin_label.Location = New Point(0, 7)
        welcomeadmin_label.Name = "welcomeadmin_label"
        welcomeadmin_label.Size = New Size(119, 20)
        welcomeadmin_label.TabIndex = 1
        welcomeadmin_label.Text = "welcome admin"
        ' 
        ' userRoleLabel
        ' 
        userRoleLabel.AutoSize = True
        userRoleLabel.Font = New Font("Segoe UI", 9.0F)
        userRoleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userRoleLabel.Location = New Point(0, 29)
        userRoleLabel.Name = "userRoleLabel"
        userRoleLabel.Size = New Size(160, 15)
        userRoleLabel.TabIndex = 2
        userRoleLabel.Text = "Admin / Pharmacy Assistant "
        ' 
        ' bodyPanel
        ' 
        bodyPanel.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        bodyPanel.Controls.Add(dashboardView)
        bodyPanel.Controls.Add(priceManagerView)
        bodyPanel.Controls.Add(inventoryView)
        bodyPanel.Controls.Add(reportsView)
        bodyPanel.Dock = DockStyle.Fill
        bodyPanel.Location = New Point(0, 72)
        bodyPanel.Margin = New Padding(0)
        bodyPanel.Name = "bodyPanel"
        bodyPanel.Padding = New Padding(20, 12, 20, 12)
        bodyPanel.Size = New Size(1516, 719)
        bodyPanel.TabIndex = 1
        ' 
        ' dashboardView
        ' 
        dashboardView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        dashboardView.Controls.Add(dashboardLayout)
        dashboardView.Dock = DockStyle.Fill
        dashboardView.Location = New Point(20, 12)
        dashboardView.Name = "dashboardView"
        dashboardView.Padding = New Padding(14, 8, 14, 8)
        dashboardView.Size = New Size(1476, 695)
        dashboardView.TabIndex = 0
        ' 
        ' dashboardLayout
        ' 
        dashboardLayout.ColumnCount = 1
        dashboardLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        dashboardLayout.Controls.Add(summaryArea, 0, 0)
        dashboardLayout.Controls.Add(middleRow, 0, 1)
        dashboardLayout.Controls.Add(bottomRow, 0, 2)
        dashboardLayout.Dock = DockStyle.Fill
        dashboardLayout.Location = New Point(14, 8)
        dashboardLayout.Margin = New Padding(0)
        dashboardLayout.Name = "dashboardLayout"
        dashboardLayout.RowCount = 3
        dashboardLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 36.0F))
        dashboardLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 36.0F))
        dashboardLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 28.0F))
        dashboardLayout.Size = New Size(1448, 679)
        dashboardLayout.TabIndex = 1
        ' 
        ' summaryArea
        ' 
        summaryArea.ColumnCount = 1
        summaryArea.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        summaryArea.Controls.Add(dashboardDateLabel, 0, 0)
        summaryArea.Controls.Add(cardGrid, 0, 1)
        summaryArea.Dock = DockStyle.Fill
        summaryArea.Location = New Point(0, 0)
        summaryArea.Margin = New Padding(0, 0, 0, 10)
        summaryArea.Name = "summaryArea"
        summaryArea.RowCount = 2
        summaryArea.RowStyles.Add(New RowStyle(SizeType.Absolute, 26.0F))
        summaryArea.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        summaryArea.Size = New Size(1448, 248)
        summaryArea.TabIndex = 0
        ' 
        ' dashboardDateLabel
        ' 
        dashboardDateLabel.Dock = DockStyle.Fill
        dashboardDateLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        dashboardDateLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        dashboardDateLabel.Location = New Point(3, 0)
        dashboardDateLabel.Name = "dashboardDateLabel"
        dashboardDateLabel.Size = New Size(1442, 26)
        dashboardDateLabel.TabIndex = 0
        dashboardDateLabel.Text = "Today"
        dashboardDateLabel.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' cardGrid
        ' 
        cardGrid.ColumnCount = 4
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
        cardGrid.Controls.Add(card1, 0, 0)
        cardGrid.Controls.Add(card2, 1, 0)
        cardGrid.Controls.Add(card3, 2, 0)
        cardGrid.Controls.Add(card4, 3, 0)
        cardGrid.Controls.Add(card5, 0, 1)
        cardGrid.Controls.Add(card6, 1, 1)
        cardGrid.Controls.Add(card7, 2, 1)
        cardGrid.Controls.Add(card8, 3, 1)
        cardGrid.Dock = DockStyle.Fill
        cardGrid.Location = New Point(3, 29)
        cardGrid.Name = "cardGrid"
        cardGrid.RowCount = 2
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0F))
        cardGrid.Size = New Size(1442, 216)
        cardGrid.TabIndex = 0
        ' 
        ' card1
        ' 
        card1.BackColor = Color.White
        card1.Controls.Add(card1Value)
        card1.Controls.Add(card1Footer)
        card1.Controls.Add(card1Title)
        card1.Dock = DockStyle.Fill
        card1.Location = New Point(0, 0)
        card1.Margin = New Padding(0, 0, 12, 12)
        card1.Name = "card1"
        card1.Padding = New Padding(16, 10, 12, 8)
        card1.Size = New Size(348, 96)
        card1.TabIndex = 0
        ' 
        ' card1Value
        ' 
        card1Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card1Value.Font = New Font("Segoe UI", 17.0F)
        card1Value.ForeColor = Color.Black
        card1Value.Location = New Point(16, 40)
        card1Value.Name = "card1Value"
        card1Value.Size = New Size(314, 32)
        card1Value.TabIndex = 2
        card1Value.Text = "0.00"
        card1Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card1Footer
        ' 
        card1Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card1Footer.Font = New Font("Segoe UI", 8.0F)
        card1Footer.ForeColor = Color.Gray
        card1Footer.Location = New Point(16, 74)
        card1Footer.Name = "card1Footer"
        card1Footer.Size = New Size(314, 18)
        card1Footer.TabIndex = 1
        card1Footer.Text = "August 18, 2026"
        ' 
        ' card1Title
        ' 
        card1Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card1Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card1Title.ForeColor = Color.Black
        card1Title.Location = New Point(16, 12)
        card1Title.Name = "card1Title"
        card1Title.Size = New Size(314, 22)
        card1Title.TabIndex = 0
        card1Title.Text = "Today's Sale"
        ' 
        ' card2
        ' 
        card2.BackColor = Color.White
        card2.Controls.Add(card2Value)
        card2.Controls.Add(card2Footer)
        card2.Controls.Add(card2Title)
        card2.Dock = DockStyle.Fill
        card2.Location = New Point(360, 0)
        card2.Margin = New Padding(0, 0, 12, 12)
        card2.Name = "card2"
        card2.Padding = New Padding(16, 10, 12, 8)
        card2.Size = New Size(348, 96)
        card2.TabIndex = 1
        ' 
        ' card2Value
        ' 
        card2Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card2Value.Font = New Font("Segoe UI", 17.0F)
        card2Value.ForeColor = Color.Black
        card2Value.Location = New Point(16, 40)
        card2Value.Name = "card2Value"
        card2Value.Size = New Size(314, 32)
        card2Value.TabIndex = 2
        card2Value.Text = "0.00"
        card2Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card2Footer
        ' 
        card2Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card2Footer.Font = New Font("Segoe UI", 8.0F)
        card2Footer.ForeColor = Color.Gray
        card2Footer.Location = New Point(16, 74)
        card2Footer.Name = "card2Footer"
        card2Footer.Size = New Size(314, 18)
        card2Footer.TabIndex = 1
        card2Footer.Text = "0 Transactions"
        ' 
        ' card2Title
        ' 
        card2Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card2Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card2Title.ForeColor = Color.Black
        card2Title.Location = New Point(16, 12)
        card2Title.Name = "card2Title"
        card2Title.Size = New Size(314, 22)
        card2Title.TabIndex = 0
        card2Title.Text = "Retail Sales"
        ' 
        ' card3
        ' 
        card3.BackColor = Color.White
        card3.Controls.Add(card3Value)
        card3.Controls.Add(card3Footer)
        card3.Controls.Add(card3Title)
        card3.Dock = DockStyle.Fill
        card3.Location = New Point(720, 0)
        card3.Margin = New Padding(0, 0, 12, 12)
        card3.Name = "card3"
        card3.Padding = New Padding(16, 10, 12, 8)
        card3.Size = New Size(348, 96)
        card3.TabIndex = 2
        ' 
        ' card3Value
        ' 
        card3Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card3Value.Font = New Font("Segoe UI", 17.0F)
        card3Value.ForeColor = Color.Black
        card3Value.Location = New Point(16, 40)
        card3Value.Name = "card3Value"
        card3Value.Size = New Size(314, 32)
        card3Value.TabIndex = 2
        card3Value.Text = "0.00"
        card3Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card3Footer
        ' 
        card3Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card3Footer.Font = New Font("Segoe UI", 8.0F)
        card3Footer.ForeColor = Color.Gray
        card3Footer.Location = New Point(16, 74)
        card3Footer.Name = "card3Footer"
        card3Footer.Size = New Size(314, 18)
        card3Footer.TabIndex = 1
        card3Footer.Text = "0 Transactions"
        ' 
        ' card3Title
        ' 
        card3Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card3Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card3Title.ForeColor = Color.Black
        card3Title.Location = New Point(16, 12)
        card3Title.Name = "card3Title"
        card3Title.Size = New Size(314, 22)
        card3Title.TabIndex = 0
        card3Title.Text = "Wholesale Sales"
        ' 
        ' card4
        ' 
        card4.BackColor = Color.White
        card4.Controls.Add(card4Value)
        card4.Controls.Add(card4Footer)
        card4.Controls.Add(card4Title)
        card4.Dock = DockStyle.Fill
        card4.Location = New Point(1080, 0)
        card4.Margin = New Padding(0, 0, 12, 12)
        card4.Name = "card4"
        card4.Padding = New Padding(16, 10, 12, 8)
        card4.Size = New Size(350, 96)
        card4.TabIndex = 3
        ' 
        ' card4Value
        ' 
        card4Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card4Value.Font = New Font("Segoe UI", 17.0F)
        card4Value.ForeColor = Color.Black
        card4Value.Location = New Point(16, 40)
        card4Value.Name = "card4Value"
        card4Value.Size = New Size(316, 32)
        card4Value.TabIndex = 2
        card4Value.Text = "0"
        card4Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card4Footer
        ' 
        card4Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card4Footer.Font = New Font("Segoe UI", 8.0F)
        card4Footer.ForeColor = Color.Gray
        card4Footer.Location = New Point(16, 74)
        card4Footer.Name = "card4Footer"
        card4Footer.Size = New Size(316, 18)
        card4Footer.TabIndex = 1
        card4Footer.Text = "All Categories"
        ' 
        ' card4Title
        ' 
        card4Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card4Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card4Title.ForeColor = Color.Black
        card4Title.Location = New Point(16, 12)
        card4Title.Name = "card4Title"
        card4Title.Size = New Size(316, 22)
        card4Title.TabIndex = 0
        card4Title.Text = "Total Products"
        ' 
        ' card5
        ' 
        card5.BackColor = Color.White
        card5.Controls.Add(card5Value)
        card5.Controls.Add(card5Footer)
        card5.Controls.Add(card5Title)
        card5.Dock = DockStyle.Fill
        card5.Location = New Point(0, 108)
        card5.Margin = New Padding(0, 0, 12, 12)
        card5.Name = "card5"
        card5.Padding = New Padding(16, 10, 12, 8)
        card5.Size = New Size(348, 96)
        card5.TabIndex = 4
        ' 
        ' card5Value
        ' 
        card5Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card5Value.Font = New Font("Segoe UI", 17.0F)
        card5Value.ForeColor = Color.Black
        card5Value.Location = New Point(16, 40)
        card5Value.Name = "card5Value"
        card5Value.Size = New Size(314, 32)
        card5Value.TabIndex = 2
        card5Value.Text = "0"
        card5Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card5Footer
        ' 
        card5Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card5Footer.Font = New Font("Segoe UI", 8.0F)
        card5Footer.ForeColor = Color.Gray
        card5Footer.Location = New Point(16, 74)
        card5Footer.Name = "card5Footer"
        card5Footer.Size = New Size(314, 18)
        card5Footer.TabIndex = 1
        card5Footer.Text = "Need reordering"
        ' 
        ' card5Title
        ' 
        card5Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card5Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card5Title.ForeColor = Color.Black
        card5Title.Location = New Point(16, 12)
        card5Title.Name = "card5Title"
        card5Title.Size = New Size(314, 22)
        card5Title.TabIndex = 0
        card5Title.Text = "Low Stock"
        ' 
        ' card6
        ' 
        card6.BackColor = Color.White
        card6.Controls.Add(card6Value)
        card6.Controls.Add(card6Footer)
        card6.Controls.Add(card6Title)
        card6.Dock = DockStyle.Fill
        card6.Location = New Point(360, 108)
        card6.Margin = New Padding(0, 0, 12, 12)
        card6.Name = "card6"
        card6.Padding = New Padding(16, 10, 12, 8)
        card6.Size = New Size(348, 96)
        card6.TabIndex = 5
        ' 
        ' card6Value
        ' 
        card6Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card6Value.Font = New Font("Segoe UI", 17.0F)
        card6Value.ForeColor = Color.Black
        card6Value.Location = New Point(16, 40)
        card6Value.Name = "card6Value"
        card6Value.Size = New Size(314, 32)
        card6Value.TabIndex = 2
        card6Value.Text = "0"
        card6Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card6Footer
        ' 
        card6Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card6Footer.Font = New Font("Segoe UI", 8.0F)
        card6Footer.ForeColor = Color.Gray
        card6Footer.Location = New Point(16, 74)
        card6Footer.Name = "card6Footer"
        card6Footer.Size = New Size(314, 18)
        card6Footer.TabIndex = 1
        card6Footer.Text = "Within 0 days"
        ' 
        ' card6Title
        ' 
        card6Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card6Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card6Title.ForeColor = Color.Black
        card6Title.Location = New Point(16, 12)
        card6Title.Name = "card6Title"
        card6Title.Size = New Size(314, 22)
        card6Title.TabIndex = 0
        card6Title.Text = "Expired"
        ' 
        ' card7
        ' 
        card7.BackColor = Color.White
        card7.Controls.Add(card7Value)
        card7.Controls.Add(card7Footer)
        card7.Controls.Add(card7Title)
        card7.Dock = DockStyle.Fill
        card7.Location = New Point(720, 108)
        card7.Margin = New Padding(0, 0, 12, 12)
        card7.Name = "card7"
        card7.Padding = New Padding(16, 10, 12, 8)
        card7.Size = New Size(348, 96)
        card7.TabIndex = 6
        ' 
        ' card7Value
        ' 
        card7Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card7Value.Font = New Font("Segoe UI", 17.0F)
        card7Value.ForeColor = Color.Black
        card7Value.Location = New Point(16, 40)
        card7Value.Name = "card7Value"
        card7Value.Size = New Size(314, 32)
        card7Value.TabIndex = 2
        card7Value.Text = "0"
        card7Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card7Footer
        ' 
        card7Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card7Footer.Font = New Font("Segoe UI", 8.0F)
        card7Footer.ForeColor = Color.Gray
        card7Footer.Location = New Point(16, 74)
        card7Footer.Name = "card7Footer"
        card7Footer.Size = New Size(314, 18)
        card7Footer.TabIndex = 1
        card7Footer.Text = "Requires disposal"
        ' 
        ' card7Title
        ' 
        card7Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card7Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card7Title.ForeColor = Color.Black
        card7Title.Location = New Point(16, 12)
        card7Title.Name = "card7Title"
        card7Title.Size = New Size(314, 22)
        card7Title.TabIndex = 0
        card7Title.Text = "Near Expiry"
        ' 
        ' card8
        ' 
        card8.BackColor = Color.White
        card8.Controls.Add(card8Value)
        card8.Controls.Add(card8Footer)
        card8.Controls.Add(card8Title)
        card8.Dock = DockStyle.Fill
        card8.Location = New Point(1080, 108)
        card8.Margin = New Padding(0)
        card8.Name = "card8"
        card8.Padding = New Padding(16, 10, 12, 8)
        card8.Size = New Size(362, 108)
        card8.TabIndex = 7
        ' 
        ' card8Value
        ' 
        card8Value.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card8Value.Font = New Font("Segoe UI", 17.0F)
        card8Value.ForeColor = Color.Black
        card8Value.Location = New Point(16, 40)
        card8Value.Name = "card8Value"
        card8Value.Size = New Size(328, 32)
        card8Value.TabIndex = 2
        card8Value.Text = "0.00"
        card8Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card8Footer
        ' 
        card8Footer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card8Footer.Font = New Font("Segoe UI", 8.0F)
        card8Footer.ForeColor = Color.Gray
        card8Footer.Location = New Point(16, 74)
        card8Footer.Name = "card8Footer"
        card8Footer.Size = New Size(328, 18)
        card8Footer.TabIndex = 1
        card8Footer.Text = "Expense data is not stored"
        ' 
        ' card8Title
        ' 
        card8Title.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        card8Title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        card8Title.ForeColor = Color.Black
        card8Title.Location = New Point(16, 12)
        card8Title.Name = "card8Title"
        card8Title.Size = New Size(328, 22)
        card8Title.TabIndex = 0
        card8Title.Text = "Today's Expenses"
        ' 
        ' middleRow
        ' 
        middleRow.ColumnCount = 2
        middleRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))
        middleRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        middleRow.Controls.Add(salesTrendPanel, 0, 0)
        middleRow.Controls.Add(revenuePanel, 1, 0)
        middleRow.Dock = DockStyle.Fill
        middleRow.Location = New Point(0, 258)
        middleRow.Margin = New Padding(0, 0, 0, 10)
        middleRow.Name = "middleRow"
        middleRow.RowCount = 1
        middleRow.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        middleRow.Size = New Size(1448, 248)
        middleRow.TabIndex = 1
        ' 
        ' salesTrendPanel
        ' 
        salesTrendPanel.BackColor = Color.White
        salesTrendPanel.BorderStyle = BorderStyle.FixedSingle
        salesTrendPanel.Controls.Add(salesTrendTitle)
        salesTrendPanel.Controls.Add(salesTrendLegend)
        salesTrendPanel.Controls.Add(trendFiltersPanel)
        salesTrendPanel.Controls.Add(salesTrendPlot)
        salesTrendPanel.Dock = DockStyle.Fill
        salesTrendPanel.Location = New Point(0, 0)
        salesTrendPanel.Margin = New Padding(0, 0, 10, 0)
        salesTrendPanel.Name = "salesTrendPanel"
        salesTrendPanel.Padding = New Padding(16)
        salesTrendPanel.Size = New Size(916, 248)
        salesTrendPanel.TabIndex = 0
        ' 
        ' salesTrendTitle
        ' 
        salesTrendTitle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        salesTrendTitle.Location = New Point(16, 10)
        salesTrendTitle.Name = "salesTrendTitle"
        salesTrendTitle.Size = New Size(160, 22)
        salesTrendTitle.TabIndex = 0
        salesTrendTitle.Text = "Sales Trend"
        ' 
        ' salesTrendLegend
        ' 
        salesTrendLegend.Font = New Font("Segoe UI", 8.0F)
        salesTrendLegend.ForeColor = Color.DimGray
        salesTrendLegend.Location = New Point(16, 34)
        salesTrendLegend.Name = "salesTrendLegend"
        salesTrendLegend.Size = New Size(260, 18)
        salesTrendLegend.TabIndex = 1
        salesTrendLegend.Text = "● Sales     Expenses unavailable"
        ' 
        ' trendFiltersPanel
        ' 
        trendFiltersPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        trendFiltersPanel.Controls.Add(trendThisYearButton)
        trendFiltersPanel.Controls.Add(trendThisMonthButton)
        trendFiltersPanel.Controls.Add(trend7DaysButton)
        trendFiltersPanel.FlowDirection = FlowDirection.RightToLeft
        trendFiltersPanel.Location = New Point(610, 8)
        trendFiltersPanel.Name = "trendFiltersPanel"
        trendFiltersPanel.Size = New Size(290, 28)
        trendFiltersPanel.TabIndex = 2
        trendFiltersPanel.WrapContents = False
        ' 
        ' trendThisYearButton
        ' 
        trendThisYearButton.BackColor = Color.White
        trendThisYearButton.Cursor = Cursors.Hand
        trendThisYearButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(225), CByte(225), CByte(225))
        trendThisYearButton.FlatStyle = FlatStyle.Flat
        trendThisYearButton.Font = New Font("Segoe UI", 7.0F)
        trendThisYearButton.Location = New Point(229, 0)
        trendThisYearButton.Margin = New Padding(3, 0, 3, 0)
        trendThisYearButton.Name = "trendThisYearButton"
        trendThisYearButton.Size = New Size(58, 24)
        trendThisYearButton.TabIndex = 2
        trendThisYearButton.Tag = "This Year"
        trendThisYearButton.Text = "This Year"
        trendThisYearButton.UseVisualStyleBackColor = False
        ' 
        ' trendThisMonthButton
        ' 
        trendThisMonthButton.BackColor = Color.White
        trendThisMonthButton.Cursor = Cursors.Hand
        trendThisMonthButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(225), CByte(225), CByte(225))
        trendThisMonthButton.FlatStyle = FlatStyle.Flat
        trendThisMonthButton.Font = New Font("Segoe UI", 7.0F)
        trendThisMonthButton.Location = New Point(153, 0)
        trendThisMonthButton.Margin = New Padding(3, 0, 3, 0)
        trendThisMonthButton.Name = "trendThisMonthButton"
        trendThisMonthButton.Size = New Size(70, 24)
        trendThisMonthButton.TabIndex = 1
        trendThisMonthButton.Tag = "This Month"
        trendThisMonthButton.Text = "This Month"
        trendThisMonthButton.UseVisualStyleBackColor = False
        ' 
        ' trend7DaysButton
        ' 
        trend7DaysButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        trend7DaysButton.Cursor = Cursors.Hand
        trend7DaysButton.FlatAppearance.BorderSize = 0
        trend7DaysButton.FlatStyle = FlatStyle.Flat
        trend7DaysButton.Font = New Font("Segoe UI", 7.0F)
        trend7DaysButton.ForeColor = Color.White
        trend7DaysButton.Location = New Point(89, 0)
        trend7DaysButton.Margin = New Padding(3, 0, 3, 0)
        trend7DaysButton.Name = "trend7DaysButton"
        trend7DaysButton.Size = New Size(58, 24)
        trend7DaysButton.TabIndex = 0
        trend7DaysButton.Tag = "7 Days"
        trend7DaysButton.Text = "7 Days"
        trend7DaysButton.UseVisualStyleBackColor = False
        ' 
        ' salesTrendPlot
        ' 
        salesTrendPlot.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        salesTrendPlot.BackColor = Color.White
        salesTrendPlot.Location = New Point(16, 58)
        salesTrendPlot.Name = "salesTrendPlot"
        salesTrendPlot.Size = New Size(884, 170)
        salesTrendPlot.TabIndex = 3
        ' 
        ' revenuePanel
        ' 
        revenuePanel.BackColor = Color.White
        revenuePanel.BorderStyle = BorderStyle.FixedSingle
        revenuePanel.Controls.Add(revenueNetIncomeLabel)
        revenuePanel.Controls.Add(revenueTitle)
        revenuePanel.Dock = DockStyle.Fill
        revenuePanel.Location = New Point(926, 0)
        revenuePanel.Margin = New Padding(0)
        revenuePanel.Name = "revenuePanel"
        revenuePanel.Padding = New Padding(16)
        revenuePanel.Size = New Size(522, 248)
        revenuePanel.TabIndex = 1
        ' 
        ' revenueTitle
        ' 
        revenueTitle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        revenueTitle.Location = New Point(16, 10)
        revenueTitle.Name = "revenueTitle"
        revenueTitle.Size = New Size(250, 22)
        revenueTitle.TabIndex = 0
        revenueTitle.Text = "Revenue vs. Expenses"
        ' 
        ' revenueNetIncomeLabel
        ' 
        revenueNetIncomeLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        revenueNetIncomeLabel.BackColor = Color.FromArgb(CByte(226), CByte(238), CByte(231))
        revenueNetIncomeLabel.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        revenueNetIncomeLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        revenueNetIncomeLabel.Location = New Point(16, 189)
        revenueNetIncomeLabel.Name = "revenueNetIncomeLabel"
        revenueNetIncomeLabel.Size = New Size(490, 36)
        revenueNetIncomeLabel.TabIndex = 1
        revenueNetIncomeLabel.Text = "Net Income (Today): unavailable without expense records"
        revenueNetIncomeLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' bottomRow
        ' 
        bottomRow.ColumnCount = 2
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60.0F))
        bottomRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0F))
        bottomRow.Controls.Add(inventoryAlertsPanel, 0, 0)
        bottomRow.Controls.Add(quickOverviewPanel, 1, 0)
        bottomRow.Dock = DockStyle.Fill
        bottomRow.Location = New Point(0, 516)
        bottomRow.Margin = New Padding(0)
        bottomRow.Name = "bottomRow"
        bottomRow.RowCount = 1
        bottomRow.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        bottomRow.Size = New Size(1448, 163)
        bottomRow.TabIndex = 2
        ' 
        ' inventoryAlertsPanel
        ' 
        inventoryAlertsPanel.BackColor = Color.White
        inventoryAlertsPanel.BorderStyle = BorderStyle.FixedSingle
        inventoryAlertsPanel.Controls.Add(inventoryAlertsTitle)
        inventoryAlertsPanel.Controls.Add(inventoryViewAllLink)
        inventoryAlertsPanel.Controls.Add(inventoryAlertLowStock)
        inventoryAlertsPanel.Controls.Add(inventoryAlertNearExpiry)
        inventoryAlertsPanel.Controls.Add(inventoryAlertExpired)
        inventoryAlertsPanel.Dock = DockStyle.Fill
        inventoryAlertsPanel.Location = New Point(0, 0)
        inventoryAlertsPanel.Margin = New Padding(0, 0, 10, 0)
        inventoryAlertsPanel.Name = "inventoryAlertsPanel"
        inventoryAlertsPanel.Padding = New Padding(16)
        inventoryAlertsPanel.Size = New Size(800, 163)
        inventoryAlertsPanel.TabIndex = 0
        ' 
        ' inventoryAlertsTitle
        ' 
        inventoryAlertsTitle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        inventoryAlertsTitle.Location = New Point(16, 10)
        inventoryAlertsTitle.Name = "inventoryAlertsTitle"
        inventoryAlertsTitle.Size = New Size(220, 22)
        inventoryAlertsTitle.TabIndex = 0
        inventoryAlertsTitle.Text = "Inventory Alerts"
        ' 
        ' inventoryViewAllLink
        ' 
        inventoryViewAllLink.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        inventoryViewAllLink.AutoSize = True
        inventoryViewAllLink.LinkColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        inventoryViewAllLink.Location = New Point(700, 12)
        inventoryViewAllLink.Name = "inventoryViewAllLink"
        inventoryViewAllLink.Size = New Size(62, 15)
        inventoryViewAllLink.TabIndex = 1
        inventoryViewAllLink.TabStop = True
        inventoryViewAllLink.Text = "View All →"
        ' 
        ' inventoryAlertLowStock
        ' 
        inventoryAlertLowStock.BackColor = Color.FromArgb(CByte(255), CByte(249), CByte(218))
        inventoryAlertLowStock.BorderStyle = BorderStyle.FixedSingle
        inventoryAlertLowStock.Controls.Add(inventoryAlertLowStockCaption)
        inventoryAlertLowStock.Controls.Add(inventoryAlertLowStockCount)
        inventoryAlertLowStock.Controls.Add(inventoryAlertLowStockIcon)
        inventoryAlertLowStock.Location = New Point(16, 42)
        inventoryAlertLowStock.Name = "inventoryAlertLowStock"
        inventoryAlertLowStock.Size = New Size(245, 96)
        inventoryAlertLowStock.TabIndex = 2
        ' 
        ' inventoryAlertLowStockCaption
        ' 
        inventoryAlertLowStockCaption.Dock = DockStyle.Fill
        inventoryAlertLowStockCaption.Font = New Font("Segoe UI", 7.0F)
        inventoryAlertLowStockCaption.ForeColor = Color.FromArgb(CByte(95), CByte(75), CByte(0))
        inventoryAlertLowStockCaption.Location = New Point(0, 28)
        inventoryAlertLowStockCaption.Name = "inventoryAlertLowStockCaption"
        inventoryAlertLowStockCaption.Size = New Size(243, 66)
        inventoryAlertLowStockCaption.TabIndex = 0
        inventoryAlertLowStockCaption.Text = "Low Stock" & vbCrLf & "Need reordering"
        inventoryAlertLowStockCaption.TextAlign = ContentAlignment.MiddleCenter
        inventoryAlertLowStockCaption.Text = "Low Stock" & vbCrLf & "Restock soon"
        ' 
        ' inventoryAlertLowStockCount
        ' 
        inventoryAlertLowStockCount.Dock = DockStyle.Top
        inventoryAlertLowStockCount.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        inventoryAlertLowStockCount.ForeColor = Color.FromArgb(CByte(95), CByte(75), CByte(0))
        inventoryAlertLowStockCount.Location = New Point(0, 0)
        inventoryAlertLowStockCount.Name = "inventoryAlertLowStockCount"
        inventoryAlertLowStockCount.Size = New Size(243, 28)
        inventoryAlertLowStockCount.TabIndex = 1
        inventoryAlertLowStockCount.Text = "0"
        inventoryAlertLowStockCount.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertLowStockIcon
        ' 
        inventoryAlertLowStockIcon.Font = New Font("Segoe UI Symbol", 12.0F, FontStyle.Bold)
        inventoryAlertLowStockIcon.ForeColor = Color.FromArgb(CByte(95), CByte(75), CByte(0))
        inventoryAlertLowStockIcon.Location = New Point(8, 3)
        inventoryAlertLowStockIcon.Name = "inventoryAlertLowStockIcon"
        inventoryAlertLowStockIcon.Size = New Size(28, 25)
        inventoryAlertLowStockIcon.TabIndex = 2
        inventoryAlertLowStockIcon.Text = "⚠"
        inventoryAlertLowStockIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertNearExpiry
        ' 
        inventoryAlertNearExpiry.BackColor = Color.FromArgb(CByte(255), CByte(249), CByte(218))
        inventoryAlertNearExpiry.BorderStyle = BorderStyle.FixedSingle
        inventoryAlertNearExpiry.Controls.Add(inventoryAlertNearExpiryCaption)
        inventoryAlertNearExpiry.Controls.Add(inventoryAlertNearExpiryCount)
        inventoryAlertNearExpiry.Controls.Add(inventoryAlertNearExpiryIcon)
        inventoryAlertNearExpiry.Location = New Point(273, 42)
        inventoryAlertNearExpiry.Name = "inventoryAlertNearExpiry"
        inventoryAlertNearExpiry.Size = New Size(245, 96)
        inventoryAlertNearExpiry.TabIndex = 3
        ' 
        ' inventoryAlertNearExpiryCaption
        ' 
        inventoryAlertNearExpiryCaption.Dock = DockStyle.Fill
        inventoryAlertNearExpiryCaption.Font = New Font("Segoe UI", 7.0F)
        inventoryAlertNearExpiryCaption.ForeColor = Color.FromArgb(CByte(95), CByte(75), CByte(0))
        inventoryAlertNearExpiryCaption.Location = New Point(0, 28)
        inventoryAlertNearExpiryCaption.Name = "inventoryAlertNearExpiryCaption"
        inventoryAlertNearExpiryCaption.Size = New Size(243, 66)
        inventoryAlertNearExpiryCaption.TabIndex = 0
        inventoryAlertNearExpiryCaption.Text = "Near Expiry" & vbCrLf & "Check expiry dates"
        inventoryAlertNearExpiryCaption.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertNearExpiryCount
        ' 
        inventoryAlertNearExpiryCount.Dock = DockStyle.Top
        inventoryAlertNearExpiryCount.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        inventoryAlertNearExpiryCount.ForeColor = Color.FromArgb(CByte(95), CByte(75), CByte(0))
        inventoryAlertNearExpiryCount.Location = New Point(0, 0)
        inventoryAlertNearExpiryCount.Name = "inventoryAlertNearExpiryCount"
        inventoryAlertNearExpiryCount.Size = New Size(243, 28)
        inventoryAlertNearExpiryCount.TabIndex = 1
        inventoryAlertNearExpiryCount.Text = "0"
        inventoryAlertNearExpiryCount.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertNearExpiryIcon
        ' 
        inventoryAlertNearExpiryIcon.Font = New Font("Segoe UI Symbol", 12.0F, FontStyle.Bold)
        inventoryAlertNearExpiryIcon.ForeColor = Color.FromArgb(CByte(140), CByte(85), CByte(20))
        inventoryAlertNearExpiryIcon.Location = New Point(8, 3)
        inventoryAlertNearExpiryIcon.Name = "inventoryAlertNearExpiryIcon"
        inventoryAlertNearExpiryIcon.Size = New Size(28, 25)
        inventoryAlertNearExpiryIcon.TabIndex = 2
        inventoryAlertNearExpiryIcon.Text = "◷"
        inventoryAlertNearExpiryIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertExpired
        ' 
        inventoryAlertExpired.BackColor = Color.FromArgb(CByte(255), CByte(232), CByte(232))
        inventoryAlertExpired.BorderStyle = BorderStyle.FixedSingle
        inventoryAlertExpired.Controls.Add(inventoryAlertExpiredCaption)
        inventoryAlertExpired.Controls.Add(inventoryAlertExpiredCount)
        inventoryAlertExpired.Controls.Add(inventoryAlertExpiredIcon)
        inventoryAlertExpired.Location = New Point(530, 42)
        inventoryAlertExpired.Name = "inventoryAlertExpired"
        inventoryAlertExpired.Size = New Size(245, 96)
        inventoryAlertExpired.TabIndex = 4
        ' 
        ' inventoryAlertExpiredCaption
        ' 
        inventoryAlertExpiredCaption.Dock = DockStyle.Fill
        inventoryAlertExpiredCaption.Font = New Font("Segoe UI", 7.0F)
        inventoryAlertExpiredCaption.ForeColor = Color.FromArgb(CByte(145), CByte(45), CByte(45))
        inventoryAlertExpiredCaption.Location = New Point(0, 28)
        inventoryAlertExpiredCaption.Name = "inventoryAlertExpiredCaption"
        inventoryAlertExpiredCaption.Size = New Size(243, 66)
        inventoryAlertExpiredCaption.TabIndex = 0
        inventoryAlertExpiredCaption.Text = "Expired" & vbCrLf & "Remove from stock"
        inventoryAlertExpiredCaption.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertExpiredCount
        ' 
        inventoryAlertExpiredCount.Dock = DockStyle.Top
        inventoryAlertExpiredCount.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        inventoryAlertExpiredCount.ForeColor = Color.FromArgb(CByte(145), CByte(45), CByte(45))
        inventoryAlertExpiredCount.Location = New Point(0, 0)
        inventoryAlertExpiredCount.Name = "inventoryAlertExpiredCount"
        inventoryAlertExpiredCount.Size = New Size(243, 28)
        inventoryAlertExpiredCount.TabIndex = 1
        inventoryAlertExpiredCount.Text = "0"
        inventoryAlertExpiredCount.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' inventoryAlertExpiredIcon
        ' 
        inventoryAlertExpiredIcon.Font = New Font("Segoe UI Symbol", 12.0F, FontStyle.Bold)
        inventoryAlertExpiredIcon.ForeColor = Color.FromArgb(CByte(145), CByte(45), CByte(45))
        inventoryAlertExpiredIcon.Location = New Point(8, 3)
        inventoryAlertExpiredIcon.Name = "inventoryAlertExpiredIcon"
        inventoryAlertExpiredIcon.Size = New Size(28, 25)
        inventoryAlertExpiredIcon.TabIndex = 2
        inventoryAlertExpiredIcon.Text = "!"
        inventoryAlertExpiredIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' quickOverviewPanel
        ' 
        quickOverviewPanel.BackColor = Color.White
        quickOverviewPanel.BorderStyle = BorderStyle.FixedSingle
        quickOverviewPanel.Controls.Add(quickOverviewTitle)
        quickOverviewPanel.Controls.Add(quickOverviewRestockIcon)
        quickOverviewPanel.Controls.Add(quickOverviewRestock)
        quickOverviewPanel.Controls.Add(quickOverviewRestockValue)
        quickOverviewPanel.Controls.Add(quickOverviewSeparator1)
        quickOverviewPanel.Controls.Add(quickOverviewExpiryIcon)
        quickOverviewPanel.Controls.Add(quickOverviewExpiry)
        quickOverviewPanel.Controls.Add(quickOverviewExpiryValue)
        quickOverviewPanel.Controls.Add(quickOverviewSeparator2)
        quickOverviewPanel.Controls.Add(quickOverviewTransactionsIcon)
        quickOverviewPanel.Controls.Add(quickOverviewTransactions)
        quickOverviewPanel.Controls.Add(quickOverviewTransactionsValue)
        quickOverviewPanel.Dock = DockStyle.Fill
        quickOverviewPanel.Location = New Point(810, 0)
        quickOverviewPanel.Margin = New Padding(0)
        quickOverviewPanel.Name = "quickOverviewPanel"
        quickOverviewPanel.Padding = New Padding(16)
        quickOverviewPanel.Size = New Size(638, 163)
        quickOverviewPanel.TabIndex = 1
        ' 
        ' quickOverviewTitle
        ' 
        quickOverviewTitle.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        quickOverviewTitle.Location = New Point(16, 10)
        quickOverviewTitle.Name = "quickOverviewTitle"
        quickOverviewTitle.Size = New Size(220, 22)
        quickOverviewTitle.TabIndex = 0
        quickOverviewTitle.Text = "Quick Overview"
        ' 
        ' quickOverviewRestockIcon
        ' 
        quickOverviewRestockIcon.Font = New Font("Segoe UI Symbol", 10.0F)
        quickOverviewRestockIcon.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        quickOverviewRestockIcon.Location = New Point(16, 38)
        quickOverviewRestockIcon.Name = "quickOverviewRestockIcon"
        quickOverviewRestockIcon.Size = New Size(24, 24)
        quickOverviewRestockIcon.TabIndex = 7
        quickOverviewRestockIcon.Text = "▣"
        quickOverviewRestockIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' quickOverviewRestock
        ' 
        quickOverviewRestock.Location = New Point(44, 38)
        quickOverviewRestock.Name = "quickOverviewRestock"
        quickOverviewRestock.Size = New Size(360, 24)
        quickOverviewRestock.TabIndex = 1
        quickOverviewRestock.Text = "Products to Restock"
        ' 
        ' quickOverviewRestockValue
        ' 
        quickOverviewRestockValue.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        quickOverviewRestockValue.Location = New Point(452, 38)
        quickOverviewRestockValue.Name = "quickOverviewRestockValue"
        quickOverviewRestockValue.Size = New Size(150, 24)
        quickOverviewRestockValue.TabIndex = 2
        quickOverviewRestockValue.Text = "0 products"
        quickOverviewRestockValue.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' quickOverviewExpiry
        ' 
        quickOverviewExpiry.Location = New Point(44, 66)
        quickOverviewExpiry.Name = "quickOverviewExpiry"
        quickOverviewExpiry.Size = New Size(360, 24)
        quickOverviewExpiry.TabIndex = 3
        quickOverviewExpiry.Text = "Expiring Products"
        ' 
        ' quickOverviewExpiryValue
        ' 
        quickOverviewExpiryValue.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        quickOverviewExpiryValue.Location = New Point(452, 66)
        quickOverviewExpiryValue.Name = "quickOverviewExpiryValue"
        quickOverviewExpiryValue.Size = New Size(150, 24)
        quickOverviewExpiryValue.TabIndex = 4
        quickOverviewExpiryValue.Text = "0 products"
        quickOverviewExpiryValue.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' quickOverviewTransactions
        ' 
        quickOverviewTransactions.Location = New Point(44, 94)
        quickOverviewTransactions.Name = "quickOverviewTransactions"
        quickOverviewTransactions.Size = New Size(360, 24)
        quickOverviewTransactions.TabIndex = 5
        quickOverviewTransactions.Text = "Today's Transactions"
        ' 
        ' quickOverviewTransactionsValue
        ' 
        quickOverviewTransactionsValue.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        quickOverviewTransactionsValue.Location = New Point(452, 94)
        quickOverviewTransactionsValue.Name = "quickOverviewTransactionsValue"
        quickOverviewTransactionsValue.Size = New Size(150, 24)
        quickOverviewTransactionsValue.TabIndex = 6
        quickOverviewTransactionsValue.Text = "0"
        quickOverviewTransactionsValue.TextAlign = ContentAlignment.MiddleRight
        ' 
        ' quickOverviewExpiryIcon
        ' 
        quickOverviewExpiryIcon.Font = New Font("Segoe UI Symbol", 10.0F)
        quickOverviewExpiryIcon.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        quickOverviewExpiryIcon.Location = New Point(16, 66)
        quickOverviewExpiryIcon.Name = "quickOverviewExpiryIcon"
        quickOverviewExpiryIcon.Size = New Size(24, 24)
        quickOverviewExpiryIcon.TabIndex = 8
        quickOverviewExpiryIcon.Text = "◷"
        quickOverviewExpiryIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' quickOverviewTransactionsIcon
        ' 
        quickOverviewTransactionsIcon.Font = New Font("Segoe UI Symbol", 10.0F)
        quickOverviewTransactionsIcon.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        quickOverviewTransactionsIcon.Location = New Point(16, 94)
        quickOverviewTransactionsIcon.Name = "quickOverviewTransactionsIcon"
        quickOverviewTransactionsIcon.Size = New Size(24, 24)
        quickOverviewTransactionsIcon.TabIndex = 9
        quickOverviewTransactionsIcon.Text = "↻"
        quickOverviewTransactionsIcon.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' quickOverviewSeparator1
        ' 
        quickOverviewSeparator1.BackColor = Color.FromArgb(CByte(235), CByte(235), CByte(235))
        quickOverviewSeparator1.Location = New Point(16, 64)
        quickOverviewSeparator1.Name = "quickOverviewSeparator1"
        quickOverviewSeparator1.Size = New Size(570, 1)
        quickOverviewSeparator1.TabIndex = 10
        ' 
        ' quickOverviewSeparator2
        ' 
        quickOverviewSeparator2.BackColor = Color.FromArgb(CByte(235), CByte(235), CByte(235))
        quickOverviewSeparator2.Location = New Point(16, 92)
        quickOverviewSeparator2.Name = "quickOverviewSeparator2"
        quickOverviewSeparator2.Size = New Size(570, 1)
        quickOverviewSeparator2.TabIndex = 11
        ' 
        ' priceManagerView
        ' 
        priceManagerView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        priceManagerView.Controls.Add(priceManagerCard)
        priceManagerView.Dock = DockStyle.Fill
        priceManagerView.Location = New Point(20, 12)
        priceManagerView.Name = "priceManagerView"
        priceManagerView.Size = New Size(1476, 695)
        priceManagerView.TabIndex = 1
        priceManagerView.Visible = False
        ' 
        ' priceManagerCard
        ' 
        priceManagerCard.BackColor = Color.White
        priceManagerCard.Controls.Add(priceManagerDesc)
        priceManagerCard.Controls.Add(priceManagerHead)
        priceManagerCard.Dock = DockStyle.Top
        priceManagerCard.Location = New Point(0, 0)
        priceManagerCard.Name = "priceManagerCard"
        priceManagerCard.Padding = New Padding(20)
        priceManagerCard.Size = New Size(1476, 160)
        priceManagerCard.TabIndex = 0
        ' 
        ' priceManagerDesc
        ' 
        priceManagerDesc.Dock = DockStyle.Bottom
        priceManagerDesc.Font = New Font("Segoe UI", 10.0F)
        priceManagerDesc.ForeColor = Color.Gray
        priceManagerDesc.Location = New Point(20, 100)
        priceManagerDesc.Name = "priceManagerDesc"
        priceManagerDesc.Size = New Size(1436, 40)
        priceManagerDesc.TabIndex = 1
        priceManagerDesc.Text = "Manage retail and wholesale pricing for all products."
        ' 
        ' priceManagerHead
        ' 
        priceManagerHead.Dock = DockStyle.Top
        priceManagerHead.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        priceManagerHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        priceManagerHead.Location = New Point(20, 20)
        priceManagerHead.Name = "priceManagerHead"
        priceManagerHead.Size = New Size(1436, 50)
        priceManagerHead.TabIndex = 0
        priceManagerHead.Text = "Price Manager"
        ' 
        ' inventoryView
        ' 
        inventoryView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        inventoryView.Controls.Add(dgvInventory)
        inventoryView.Controls.Add(inventoryCard)
        inventoryView.Dock = DockStyle.Fill
        inventoryView.Location = New Point(20, 12)
        inventoryView.Name = "inventoryView"
        inventoryView.Size = New Size(1476, 695)
        inventoryView.TabIndex = 2
        inventoryView.Visible = False
        ' 
        ' dgvInventory
        ' 
        dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvInventory.Location = New Point(0, 0)
        dgvInventory.Name = "dgvInventory"
        dgvInventory.RowHeadersWidth = 51
        dgvInventory.Size = New Size(1644, 901)
        dgvInventory.TabIndex = 1
        ' 
        ' inventoryCard
        ' 
        inventoryCard.BackColor = Color.White
        inventoryCard.Controls.Add(inventoryDesc)
        inventoryCard.Controls.Add(inventoryHead)
        inventoryCard.Dock = DockStyle.Top
        inventoryCard.Location = New Point(0, 0)
        inventoryCard.Name = "inventoryCard"
        inventoryCard.Padding = New Padding(20)
        inventoryCard.Size = New Size(1476, 160)
        inventoryCard.TabIndex = 0
        ' 
        ' inventoryDesc
        ' 
        inventoryDesc.Dock = DockStyle.Bottom
        inventoryDesc.Font = New Font("Segoe UI", 10.0F)
        inventoryDesc.ForeColor = Color.Gray
        inventoryDesc.Location = New Point(20, 100)
        inventoryDesc.Name = "inventoryDesc"
        inventoryDesc.Size = New Size(1436, 40)
        inventoryDesc.TabIndex = 1
        inventoryDesc.Text = "Track stock levels, expiring items, and product categories."
        ' 
        ' inventoryHead
        ' 
        inventoryHead.Dock = DockStyle.Top
        inventoryHead.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        inventoryHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        inventoryHead.Location = New Point(20, 20)
        inventoryHead.Name = "inventoryHead"
        inventoryHead.Size = New Size(1436, 50)
        inventoryHead.TabIndex = 0
        inventoryHead.Text = "Inventory"
        ' 
        ' reportsView
        ' 
        reportsView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        reportsView.Controls.Add(reportsCard)
        reportsView.Dock = DockStyle.Fill
        reportsView.Location = New Point(20, 12)
        reportsView.Name = "reportsView"
        reportsView.Size = New Size(1476, 695)
        reportsView.TabIndex = 3
        reportsView.Visible = False
        ' 
        ' reportsCard
        ' 
        reportsCard.BackColor = Color.White
        reportsCard.Controls.Add(reportsDesc)
        reportsCard.Controls.Add(reportsHead)
        reportsCard.Dock = DockStyle.Top
        reportsCard.Location = New Point(0, 0)
        reportsCard.Name = "reportsCard"
        reportsCard.Padding = New Padding(20)
        reportsCard.Size = New Size(1476, 160)
        reportsCard.TabIndex = 0
        ' 
        ' reportsDesc
        ' 
        reportsDesc.Dock = DockStyle.Bottom
        reportsDesc.Font = New Font("Segoe UI", 10.0F)
        reportsDesc.ForeColor = Color.Gray
        reportsDesc.Location = New Point(20, 100)
        reportsDesc.Name = "reportsDesc"
        reportsDesc.Size = New Size(1436, 40)
        reportsDesc.TabIndex = 1
        reportsDesc.Text = "Generate sales, inventory, and expense reports."
        ' 
        ' reportsHead
        ' 
        reportsHead.Dock = DockStyle.Top
        reportsHead.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        reportsHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        reportsHead.Location = New Point(20, 20)
        reportsHead.Name = "reportsHead"
        reportsHead.Size = New Size(1436, 50)
        reportsHead.TabIndex = 0
        reportsHead.Text = "Reports"
        ' 
        ' Admin
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.White
        ClientSize = New Size(1684, 791)
        Controls.Add(mainLayout)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Admin"
        Text = "Cabrera's Drugstore - Dashboard"
        WindowState = FormWindowState.Maximized
        mainLayout.ResumeLayout(False)
        sidebar.ResumeLayout(False)
        CType(logoBox, ComponentModel.ISupportInitialize).EndInit()
        rightLayout.ResumeLayout(False)
        headerPanel.ResumeLayout(False)
        headerPanel.PerformLayout()
        userPanel.ResumeLayout(False)
        userPanel.PerformLayout()
        CType(userAvatar, ComponentModel.ISupportInitialize).EndInit()
        bodyPanel.ResumeLayout(False)
        dashboardView.ResumeLayout(False)
        dashboardLayout.ResumeLayout(False)
        summaryArea.ResumeLayout(False)
        cardGrid.ResumeLayout(False)
        card1.ResumeLayout(False)
        card2.ResumeLayout(False)
        card3.ResumeLayout(False)
        card4.ResumeLayout(False)
        card5.ResumeLayout(False)
        card6.ResumeLayout(False)
        card7.ResumeLayout(False)
        card8.ResumeLayout(False)
        middleRow.ResumeLayout(False)
        salesTrendPanel.ResumeLayout(False)
        trendFiltersPanel.ResumeLayout(False)
        revenuePanel.ResumeLayout(False)
        bottomRow.ResumeLayout(False)
        inventoryAlertsPanel.ResumeLayout(False)
        inventoryAlertsPanel.PerformLayout()
        inventoryAlertLowStock.ResumeLayout(False)
        inventoryAlertNearExpiry.ResumeLayout(False)
        inventoryAlertExpired.ResumeLayout(False)
        quickOverviewPanel.ResumeLayout(False)
        priceManagerView.ResumeLayout(False)
        priceManagerCard.ResumeLayout(False)
        inventoryView.ResumeLayout(False)
        CType(dgvInventory, ComponentModel.ISupportInitialize).EndInit()
        inventoryCard.ResumeLayout(False)
        reportsView.ResumeLayout(False)
        reportsCard.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainLayout As TableLayoutPanel
    Friend WithEvents sidebar As Panel
    Friend WithEvents logoBox As PictureBox
    Friend WithEvents PharName As Label
    Friend WithEvents navDashboard As Button
    Friend WithEvents navPriceManager As Button
    Friend WithEvents navInventory As Button
    Friend WithEvents navReports As Button
    Friend WithEvents logoutadmin_button As Button
    Friend WithEvents rightLayout As TableLayoutPanel
    Friend WithEvents headerPanel As Panel
    Friend WithEvents titleLabel As Label
    Friend WithEvents subtitleLabel As Label
    Friend WithEvents userPanel As Panel
    Friend WithEvents userAvatar As PictureBox
    Friend WithEvents welcomeadmin_label As Label
    Friend WithEvents userRoleLabel As Label
    Friend WithEvents bodyPanel As Panel
    Friend WithEvents dashboardView As Panel
    Friend WithEvents cardGrid As TableLayoutPanel
    Friend WithEvents card1 As Panel
    Friend WithEvents card1Title As Label
    Friend WithEvents card1Value As Label
    Friend WithEvents card1Footer As Label
    Friend WithEvents card2 As Panel
    Friend WithEvents card2Title As Label
    Friend WithEvents card2Value As Label
    Friend WithEvents card2Footer As Label
    Friend WithEvents card3 As Panel
    Friend WithEvents card3Title As Label
    Friend WithEvents card3Value As Label
    Friend WithEvents card3Footer As Label
    Friend WithEvents card4 As Panel
    Friend WithEvents card4Title As Label
    Friend WithEvents card4Value As Label
    Friend WithEvents card4Footer As Label
    Friend WithEvents card5 As Panel
    Friend WithEvents card5Title As Label
    Friend WithEvents card5Value As Label
    Friend WithEvents card5Footer As Label
    Friend WithEvents card6 As Panel
    Friend WithEvents card6Title As Label
    Friend WithEvents card6Value As Label
    Friend WithEvents card6Footer As Label
    Friend WithEvents card7 As Panel
    Friend WithEvents card7Title As Label
    Friend WithEvents card7Value As Label
    Friend WithEvents card7Footer As Label
    Friend WithEvents card8 As Panel
    Friend WithEvents card8Title As Label
    Friend WithEvents dashboardLayout As TableLayoutPanel
    Friend WithEvents summaryArea As TableLayoutPanel
    Friend WithEvents dashboardDateLabel As Label
    Friend WithEvents middleRow As TableLayoutPanel
    Friend WithEvents salesTrendPanel As Panel
    Friend WithEvents salesTrendTitle As Label
    Friend WithEvents salesTrendLegend As Label
    Friend WithEvents trendFiltersPanel As FlowLayoutPanel
    Friend WithEvents trend7DaysButton As Button
    Friend WithEvents trendThisMonthButton As Button
    Friend WithEvents trendThisYearButton As Button
    Friend WithEvents salesTrendPlot As Panel
    Friend WithEvents revenuePanel As Panel
    Friend WithEvents revenueTitle As Label
    Friend WithEvents revenueNetIncomeLabel As Label
    Friend WithEvents bottomRow As TableLayoutPanel
    Friend WithEvents inventoryAlertsPanel As Panel
    Friend WithEvents inventoryAlertsTitle As Label
    Friend WithEvents inventoryViewAllLink As LinkLabel
    Friend WithEvents inventoryAlertLowStock As Panel
    Friend WithEvents inventoryAlertLowStockIcon As Label
    Friend WithEvents inventoryAlertNearExpiry As Panel
    Friend WithEvents inventoryAlertNearExpiryIcon As Label
    Friend WithEvents inventoryAlertExpired As Panel
    Friend WithEvents inventoryAlertExpiredIcon As Label
    Friend WithEvents inventoryAlertLowStockCount As Label
    Friend WithEvents inventoryAlertNearExpiryCount As Label
    Friend WithEvents inventoryAlertExpiredCount As Label
    Friend WithEvents inventoryAlertLowStockCaption As Label
    Friend WithEvents inventoryAlertNearExpiryCaption As Label
    Friend WithEvents inventoryAlertExpiredCaption As Label
    Friend WithEvents quickOverviewPanel As Panel
    Friend WithEvents quickOverviewTitle As Label
    Friend WithEvents quickOverviewRestockIcon As Label
    Friend WithEvents quickOverviewRestock As Label
    Friend WithEvents quickOverviewRestockValue As Label
    Friend WithEvents quickOverviewSeparator1 As Panel
    Friend WithEvents quickOverviewExpiryIcon As Label
    Friend WithEvents quickOverviewExpiry As Label
    Friend WithEvents quickOverviewExpiryValue As Label
    Friend WithEvents quickOverviewSeparator2 As Panel
    Friend WithEvents quickOverviewTransactionsIcon As Label
    Friend WithEvents quickOverviewTransactions As Label
    Friend WithEvents quickOverviewTransactionsValue As Label
    Friend WithEvents card8Value As Label
    Friend WithEvents card8Footer As Label
    Friend WithEvents priceManagerView As Panel
    Friend WithEvents priceManagerCard As Panel
    Friend WithEvents priceManagerHead As Label
    Friend WithEvents priceManagerDesc As Label
    Friend WithEvents inventoryView As Panel
    Friend WithEvents inventoryCard As Panel
    Friend WithEvents inventoryHead As Label
    Friend WithEvents inventoryDesc As Label
    Friend WithEvents reportsView As Panel
    Friend WithEvents reportsCard As Panel
    Friend WithEvents reportsHead As Label
    Friend WithEvents reportsDesc As Label
    Friend WithEvents dgvInventory As DataGridView
End Class
