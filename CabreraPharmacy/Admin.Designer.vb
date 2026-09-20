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
        cardGrid.SuspendLayout()
        card1.SuspendLayout()
        card2.SuspendLayout()
        card3.SuspendLayout()
        card4.SuspendLayout()
        card5.SuspendLayout()
        card6.SuspendLayout()
        card7.SuspendLayout()
        card8.SuspendLayout()
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
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 220F))
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        mainLayout.Controls.Add(sidebar, 0, 0)
        mainLayout.Controls.Add(rightLayout, 1, 0)
        mainLayout.Dock = DockStyle.Fill
        mainLayout.Location = New Point(0, 0)
        mainLayout.Margin = New Padding(0)
        mainLayout.Name = "mainLayout"
        mainLayout.RowCount = 1
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        mainLayout.Size = New Size(1904, 1041)
        mainLayout.TabIndex = 0
        ' 
        ' sidebar
        ' 
        sidebar.BackColor = Color.White
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
        sidebar.Size = New Size(220, 1041)
        sidebar.TabIndex = 0
        ' 
        ' logoBox
        ' 
        logoBox.BackColor = Color.FromArgb(CByte(255), CByte(215), CByte(0))
        logoBox.Location = New Point(20, 20)
        logoBox.Name = "logoBox"
        logoBox.Size = New Size(50, 50)
        logoBox.TabIndex = 0
        logoBox.TabStop = False
        ' 
        ' PharName
        ' 
        PharName.AutoSize = True
        PharName.BackColor = Color.Transparent
        PharName.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        PharName.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        PharName.Location = New Point(80, 20)
        PharName.MaximumSize = New Size(130, 0)
        PharName.Name = "PharName"
        PharName.Size = New Size(122, 57)
        PharName.TabIndex = 1
        PharName.Text = "Cabrera's" & vbCrLf & "Drugstore and Medical Supplies"
        ' 
        ' navDashboard
        ' 
        navDashboard.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navDashboard.Cursor = Cursors.Hand
        navDashboard.FlatAppearance.BorderSize = 0
        navDashboard.FlatStyle = FlatStyle.Flat
        navDashboard.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navDashboard.ForeColor = Color.White
        navDashboard.Location = New Point(20, 120)
        navDashboard.Name = "navDashboard"
        navDashboard.Padding = New Padding(10, 0, 0, 0)
        navDashboard.Size = New Size(180, 45)
        navDashboard.TabIndex = 2
        navDashboard.Tag = "Dashboard"
        navDashboard.Text = "Dashboard"
        navDashboard.TextAlign = ContentAlignment.MiddleLeft
        navDashboard.UseVisualStyleBackColor = False
        ' 
        ' navPriceManager
        ' 
        navPriceManager.BackColor = Color.White
        navPriceManager.Cursor = Cursors.Hand
        navPriceManager.FlatAppearance.BorderSize = 0
        navPriceManager.FlatStyle = FlatStyle.Flat
        navPriceManager.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navPriceManager.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navPriceManager.Location = New Point(20, 175)
        navPriceManager.Name = "navPriceManager"
        navPriceManager.Padding = New Padding(10, 0, 0, 0)
        navPriceManager.Size = New Size(180, 45)
        navPriceManager.TabIndex = 3
        navPriceManager.Tag = "Price Manager"
        navPriceManager.Text = "Price Manager"
        navPriceManager.TextAlign = ContentAlignment.MiddleLeft
        navPriceManager.UseVisualStyleBackColor = False
        ' 
        ' navInventory
        ' 
        navInventory.BackColor = Color.White
        navInventory.Cursor = Cursors.Hand
        navInventory.FlatAppearance.BorderSize = 0
        navInventory.FlatStyle = FlatStyle.Flat
        navInventory.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navInventory.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navInventory.Location = New Point(20, 230)
        navInventory.Name = "navInventory"
        navInventory.Padding = New Padding(10, 0, 0, 0)
        navInventory.Size = New Size(180, 45)
        navInventory.TabIndex = 4
        navInventory.Tag = "Inventory"
        navInventory.Text = "Inventory"
        navInventory.TextAlign = ContentAlignment.MiddleLeft
        navInventory.UseVisualStyleBackColor = False
        ' 
        ' navReports
        ' 
        navReports.BackColor = Color.White
        navReports.Cursor = Cursors.Hand
        navReports.FlatAppearance.BorderSize = 0
        navReports.FlatStyle = FlatStyle.Flat
        navReports.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navReports.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navReports.Location = New Point(20, 285)
        navReports.Name = "navReports"
        navReports.Padding = New Padding(10, 0, 0, 0)
        navReports.Size = New Size(180, 45)
        navReports.TabIndex = 5
        navReports.Tag = "Reports"
        navReports.Text = "Reports"
        navReports.TextAlign = ContentAlignment.MiddleLeft
        navReports.UseVisualStyleBackColor = False
        ' 
        ' logoutadmin_button
        ' 
        logoutadmin_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutadmin_button.BackColor = Color.White
        logoutadmin_button.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        logoutadmin_button.FlatStyle = FlatStyle.Flat
        logoutadmin_button.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        logoutadmin_button.Location = New Point(20, 961)
        logoutadmin_button.Name = "logoutadmin_button"
        logoutadmin_button.Size = New Size(180, 45)
        logoutadmin_button.TabIndex = 6
        logoutadmin_button.Text = "Log Out"
        logoutadmin_button.UseVisualStyleBackColor = False
        ' 
        ' rightLayout
        ' 
        rightLayout.ColumnCount = 1
        rightLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        rightLayout.Controls.Add(headerPanel, 0, 0)
        rightLayout.Controls.Add(bodyPanel, 0, 1)
        rightLayout.Dock = DockStyle.Fill
        rightLayout.Location = New Point(220, 0)
        rightLayout.Margin = New Padding(0)
        rightLayout.Name = "rightLayout"
        rightLayout.RowCount = 2
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100F))
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        rightLayout.Size = New Size(1684, 1041)
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
        headerPanel.Size = New Size(1684, 100)
        headerPanel.TabIndex = 0
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 24F, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        titleLabel.Location = New Point(20, 10)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(184, 45)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Dashboard"
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 11F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(20, 55)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(369, 20)
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
        userPanel.Location = New Point(1441, 10)
        userPanel.Name = "userPanel"
        userPanel.Size = New Size(223, 63)
        userPanel.TabIndex = 2
        ' 
        ' userAvatar
        ' 
        userAvatar.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userAvatar.Location = New Point(0, 10)
        userAvatar.Name = "userAvatar"
        userAvatar.Size = New Size(50, 50)
        userAvatar.TabIndex = 0
        userAvatar.TabStop = False
        ' 
        ' welcomeadmin_label
        ' 
        welcomeadmin_label.AutoSize = True
        welcomeadmin_label.BackColor = Color.Transparent
        welcomeadmin_label.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        welcomeadmin_label.ForeColor = Color.Black
        welcomeadmin_label.Location = New Point(60, 10)
        welcomeadmin_label.Name = "welcomeadmin_label"
        welcomeadmin_label.Size = New Size(119, 20)
        welcomeadmin_label.TabIndex = 1
        welcomeadmin_label.Text = "welcome admin"
        ' 
        ' userRoleLabel
        ' 
        userRoleLabel.AutoSize = True
        userRoleLabel.Font = New Font("Segoe UI", 9F)
        userRoleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userRoleLabel.Location = New Point(60, 32)
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
        bodyPanel.Location = New Point(0, 100)
        bodyPanel.Margin = New Padding(0)
        bodyPanel.Name = "bodyPanel"
        bodyPanel.Padding = New Padding(20)
        bodyPanel.Size = New Size(1684, 941)
        bodyPanel.TabIndex = 1
        ' 
        ' dashboardView
        ' 
        dashboardView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        dashboardView.Controls.Add(cardGrid)
        dashboardView.Dock = DockStyle.Fill
        dashboardView.Location = New Point(20, 20)
        dashboardView.Name = "dashboardView"
        dashboardView.Size = New Size(1644, 901)
        dashboardView.TabIndex = 0
        ' 
        ' cardGrid
        ' 
        cardGrid.ColumnCount = 4
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        cardGrid.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25F))
        cardGrid.Controls.Add(card1, 0, 0)
        cardGrid.Controls.Add(card2, 1, 0)
        cardGrid.Controls.Add(card3, 2, 0)
        cardGrid.Controls.Add(card4, 3, 0)
        cardGrid.Controls.Add(card5, 0, 1)
        cardGrid.Controls.Add(card6, 1, 1)
        cardGrid.Controls.Add(card7, 2, 1)
        cardGrid.Controls.Add(card8, 3, 1)
        cardGrid.Dock = DockStyle.Fill
        cardGrid.Location = New Point(0, 0)
        cardGrid.Name = "cardGrid"
        cardGrid.RowCount = 2
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        cardGrid.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        cardGrid.Size = New Size(1644, 901)
        cardGrid.TabIndex = 0
        ' 
        ' card1
        ' 
        card1.BackColor = Color.White
        card1.Controls.Add(card1Value)
        card1.Controls.Add(card1Footer)
        card1.Controls.Add(card1Title)
        card1.Dock = DockStyle.Fill
        card1.Location = New Point(10, 10)
        card1.Margin = New Padding(10)
        card1.Name = "card1"
        card1.Padding = New Padding(20)
        card1.Size = New Size(391, 430)
        card1.TabIndex = 0
        ' 
        ' card1Value
        ' 
        card1Value.Dock = DockStyle.Fill
        card1Value.Font = New Font("Segoe UI", 24F)
        card1Value.ForeColor = Color.Black
        card1Value.Location = New Point(20, 43)
        card1Value.Name = "card1Value"
        card1Value.Size = New Size(351, 344)
        card1Value.TabIndex = 2
        card1Value.Text = "0.00"
        card1Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card1Footer
        ' 
        card1Footer.Dock = DockStyle.Bottom
        card1Footer.Font = New Font("Segoe UI", 9F)
        card1Footer.ForeColor = Color.Gray
        card1Footer.Location = New Point(20, 387)
        card1Footer.Name = "card1Footer"
        card1Footer.Size = New Size(351, 23)
        card1Footer.TabIndex = 1
        card1Footer.Text = "August 18, 2026"
        ' 
        ' card1Title
        ' 
        card1Title.Dock = DockStyle.Top
        card1Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card1Title.ForeColor = Color.Black
        card1Title.Location = New Point(20, 20)
        card1Title.Name = "card1Title"
        card1Title.Size = New Size(351, 23)
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
        card2.Location = New Point(421, 10)
        card2.Margin = New Padding(10)
        card2.Name = "card2"
        card2.Padding = New Padding(20)
        card2.Size = New Size(391, 430)
        card2.TabIndex = 1
        ' 
        ' card2Value
        ' 
        card2Value.Dock = DockStyle.Fill
        card2Value.Font = New Font("Segoe UI", 24F)
        card2Value.ForeColor = Color.Black
        card2Value.Location = New Point(20, 43)
        card2Value.Name = "card2Value"
        card2Value.Size = New Size(351, 344)
        card2Value.TabIndex = 2
        card2Value.Text = "0.00"
        card2Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card2Footer
        ' 
        card2Footer.Dock = DockStyle.Bottom
        card2Footer.Font = New Font("Segoe UI", 9F)
        card2Footer.ForeColor = Color.Gray
        card2Footer.Location = New Point(20, 387)
        card2Footer.Name = "card2Footer"
        card2Footer.Size = New Size(351, 23)
        card2Footer.TabIndex = 1
        card2Footer.Text = "0 Transactions"
        ' 
        ' card2Title
        ' 
        card2Title.Dock = DockStyle.Top
        card2Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card2Title.ForeColor = Color.Black
        card2Title.Location = New Point(20, 20)
        card2Title.Name = "card2Title"
        card2Title.Size = New Size(351, 23)
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
        card3.Location = New Point(832, 10)
        card3.Margin = New Padding(10)
        card3.Name = "card3"
        card3.Padding = New Padding(20)
        card3.Size = New Size(391, 430)
        card3.TabIndex = 2
        ' 
        ' card3Value
        ' 
        card3Value.Dock = DockStyle.Fill
        card3Value.Font = New Font("Segoe UI", 24F)
        card3Value.ForeColor = Color.Black
        card3Value.Location = New Point(20, 43)
        card3Value.Name = "card3Value"
        card3Value.Size = New Size(351, 344)
        card3Value.TabIndex = 2
        card3Value.Text = "0.00"
        card3Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card3Footer
        ' 
        card3Footer.Dock = DockStyle.Bottom
        card3Footer.Font = New Font("Segoe UI", 9F)
        card3Footer.ForeColor = Color.Gray
        card3Footer.Location = New Point(20, 387)
        card3Footer.Name = "card3Footer"
        card3Footer.Size = New Size(351, 23)
        card3Footer.TabIndex = 1
        card3Footer.Text = "0 Transactions"
        ' 
        ' card3Title
        ' 
        card3Title.Dock = DockStyle.Top
        card3Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card3Title.ForeColor = Color.Black
        card3Title.Location = New Point(20, 20)
        card3Title.Name = "card3Title"
        card3Title.Size = New Size(351, 23)
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
        card4.Location = New Point(1243, 10)
        card4.Margin = New Padding(10)
        card4.Name = "card4"
        card4.Padding = New Padding(20)
        card4.Size = New Size(391, 430)
        card4.TabIndex = 3
        ' 
        ' card4Value
        ' 
        card4Value.Dock = DockStyle.Fill
        card4Value.Font = New Font("Segoe UI", 24F)
        card4Value.ForeColor = Color.Black
        card4Value.Location = New Point(20, 43)
        card4Value.Name = "card4Value"
        card4Value.Size = New Size(351, 344)
        card4Value.TabIndex = 2
        card4Value.Text = "0"
        card4Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card4Footer
        ' 
        card4Footer.Dock = DockStyle.Bottom
        card4Footer.Font = New Font("Segoe UI", 9F)
        card4Footer.ForeColor = Color.Gray
        card4Footer.Location = New Point(20, 387)
        card4Footer.Name = "card4Footer"
        card4Footer.Size = New Size(351, 23)
        card4Footer.TabIndex = 1
        card4Footer.Text = "All Categories"
        ' 
        ' card4Title
        ' 
        card4Title.Dock = DockStyle.Top
        card4Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card4Title.ForeColor = Color.Black
        card4Title.Location = New Point(20, 20)
        card4Title.Name = "card4Title"
        card4Title.Size = New Size(351, 23)
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
        card5.Location = New Point(10, 460)
        card5.Margin = New Padding(10)
        card5.Name = "card5"
        card5.Padding = New Padding(20)
        card5.Size = New Size(391, 431)
        card5.TabIndex = 4
        ' 
        ' card5Value
        ' 
        card5Value.Dock = DockStyle.Fill
        card5Value.Font = New Font("Segoe UI", 24F)
        card5Value.ForeColor = Color.Black
        card5Value.Location = New Point(20, 43)
        card5Value.Name = "card5Value"
        card5Value.Size = New Size(351, 345)
        card5Value.TabIndex = 2
        card5Value.Text = "0"
        card5Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card5Footer
        ' 
        card5Footer.Dock = DockStyle.Bottom
        card5Footer.Font = New Font("Segoe UI", 9F)
        card5Footer.ForeColor = Color.Gray
        card5Footer.Location = New Point(20, 388)
        card5Footer.Name = "card5Footer"
        card5Footer.Size = New Size(351, 23)
        card5Footer.TabIndex = 1
        card5Footer.Text = "Need reordering"
        ' 
        ' card5Title
        ' 
        card5Title.Dock = DockStyle.Top
        card5Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card5Title.ForeColor = Color.Black
        card5Title.Location = New Point(20, 20)
        card5Title.Name = "card5Title"
        card5Title.Size = New Size(351, 23)
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
        card6.Location = New Point(421, 460)
        card6.Margin = New Padding(10)
        card6.Name = "card6"
        card6.Padding = New Padding(20)
        card6.Size = New Size(391, 431)
        card6.TabIndex = 5
        ' 
        ' card6Value
        ' 
        card6Value.Dock = DockStyle.Fill
        card6Value.Font = New Font("Segoe UI", 24F)
        card6Value.ForeColor = Color.Black
        card6Value.Location = New Point(20, 43)
        card6Value.Name = "card6Value"
        card6Value.Size = New Size(351, 345)
        card6Value.TabIndex = 2
        card6Value.Text = "0"
        card6Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card6Footer
        ' 
        card6Footer.Dock = DockStyle.Bottom
        card6Footer.Font = New Font("Segoe UI", 9F)
        card6Footer.ForeColor = Color.Gray
        card6Footer.Location = New Point(20, 388)
        card6Footer.Name = "card6Footer"
        card6Footer.Size = New Size(351, 23)
        card6Footer.TabIndex = 1
        card6Footer.Text = "Within 0 days"
        ' 
        ' card6Title
        ' 
        card6Title.Dock = DockStyle.Top
        card6Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card6Title.ForeColor = Color.Black
        card6Title.Location = New Point(20, 20)
        card6Title.Name = "card6Title"
        card6Title.Size = New Size(351, 23)
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
        card7.Location = New Point(832, 460)
        card7.Margin = New Padding(10)
        card7.Name = "card7"
        card7.Padding = New Padding(20)
        card7.Size = New Size(391, 431)
        card7.TabIndex = 6
        ' 
        ' card7Value
        ' 
        card7Value.Dock = DockStyle.Fill
        card7Value.Font = New Font("Segoe UI", 24F)
        card7Value.ForeColor = Color.Black
        card7Value.Location = New Point(20, 43)
        card7Value.Name = "card7Value"
        card7Value.Size = New Size(351, 345)
        card7Value.TabIndex = 2
        card7Value.Text = "0"
        card7Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card7Footer
        ' 
        card7Footer.Dock = DockStyle.Bottom
        card7Footer.Font = New Font("Segoe UI", 9F)
        card7Footer.ForeColor = Color.Gray
        card7Footer.Location = New Point(20, 388)
        card7Footer.Name = "card7Footer"
        card7Footer.Size = New Size(351, 23)
        card7Footer.TabIndex = 1
        card7Footer.Text = "Requires disposal"
        ' 
        ' card7Title
        ' 
        card7Title.Dock = DockStyle.Top
        card7Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card7Title.ForeColor = Color.Black
        card7Title.Location = New Point(20, 20)
        card7Title.Name = "card7Title"
        card7Title.Size = New Size(351, 23)
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
        card8.Location = New Point(1243, 460)
        card8.Margin = New Padding(10)
        card8.Name = "card8"
        card8.Padding = New Padding(20)
        card8.Size = New Size(391, 431)
        card8.TabIndex = 7
        ' 
        ' card8Value
        ' 
        card8Value.Dock = DockStyle.Fill
        card8Value.Font = New Font("Segoe UI", 24F)
        card8Value.ForeColor = Color.Black
        card8Value.Location = New Point(20, 43)
        card8Value.Name = "card8Value"
        card8Value.Size = New Size(351, 345)
        card8Value.TabIndex = 2
        card8Value.Text = "0.00"
        card8Value.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' card8Footer
        ' 
        card8Footer.Dock = DockStyle.Bottom
        card8Footer.Font = New Font("Segoe UI", 9F)
        card8Footer.ForeColor = Color.Gray
        card8Footer.Location = New Point(20, 388)
        card8Footer.Name = "card8Footer"
        card8Footer.Size = New Size(351, 23)
        card8Footer.TabIndex = 1
        card8Footer.Text = "No expenses recorded today"
        ' 
        ' card8Title
        ' 
        card8Title.Dock = DockStyle.Top
        card8Title.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        card8Title.ForeColor = Color.Black
        card8Title.Location = New Point(20, 20)
        card8Title.Name = "card8Title"
        card8Title.Size = New Size(351, 23)
        card8Title.TabIndex = 0
        card8Title.Text = "Today's Expenses"
        ' 
        ' priceManagerView
        ' 
        priceManagerView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        priceManagerView.Controls.Add(priceManagerCard)
        priceManagerView.Dock = DockStyle.Fill
        priceManagerView.Location = New Point(20, 20)
        priceManagerView.Name = "priceManagerView"
        priceManagerView.Size = New Size(1644, 901)
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
        priceManagerCard.Size = New Size(1644, 160)
        priceManagerCard.TabIndex = 0
        ' 
        ' priceManagerDesc
        ' 
        priceManagerDesc.Dock = DockStyle.Bottom
        priceManagerDesc.Font = New Font("Segoe UI", 10F)
        priceManagerDesc.ForeColor = Color.Gray
        priceManagerDesc.Location = New Point(20, 100)
        priceManagerDesc.Name = "priceManagerDesc"
        priceManagerDesc.Size = New Size(1604, 40)
        priceManagerDesc.TabIndex = 1
        priceManagerDesc.Text = "Manage retail and wholesale pricing for all products."
        ' 
        ' priceManagerHead
        ' 
        priceManagerHead.Dock = DockStyle.Top
        priceManagerHead.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        priceManagerHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        priceManagerHead.Location = New Point(20, 20)
        priceManagerHead.Name = "priceManagerHead"
        priceManagerHead.Size = New Size(1604, 50)
        priceManagerHead.TabIndex = 0
        priceManagerHead.Text = "Price Manager"
        ' 
        ' inventoryView
        ' 
        inventoryView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        inventoryView.Controls.Add(dgvInventory)
        inventoryView.Controls.Add(inventoryCard)
        inventoryView.Dock = DockStyle.Fill
        inventoryView.Location = New Point(20, 20)
        inventoryView.Name = "inventoryView"
        inventoryView.Size = New Size(1644, 901)
        inventoryView.TabIndex = 2
        inventoryView.Visible = False
        ' 
        ' dgvInventory
        ' 
        dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvInventory.Location = New Point(0, 0)
        dgvInventory.Name = "dgvInventory"
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
        inventoryCard.Size = New Size(1644, 160)
        inventoryCard.TabIndex = 0
        ' 
        ' inventoryDesc
        ' 
        inventoryDesc.Dock = DockStyle.Bottom
        inventoryDesc.Font = New Font("Segoe UI", 10F)
        inventoryDesc.ForeColor = Color.Gray
        inventoryDesc.Location = New Point(20, 100)
        inventoryDesc.Name = "inventoryDesc"
        inventoryDesc.Size = New Size(1604, 40)
        inventoryDesc.TabIndex = 1
        inventoryDesc.Text = "Track stock levels, expiring items, and product categories."
        ' 
        ' inventoryHead
        ' 
        inventoryHead.Dock = DockStyle.Top
        inventoryHead.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        inventoryHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        inventoryHead.Location = New Point(20, 20)
        inventoryHead.Name = "inventoryHead"
        inventoryHead.Size = New Size(1604, 50)
        inventoryHead.TabIndex = 0
        inventoryHead.Text = "Inventory"
        ' 
        ' reportsView
        ' 
        reportsView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        reportsView.Controls.Add(reportsCard)
        reportsView.Dock = DockStyle.Fill
        reportsView.Location = New Point(20, 20)
        reportsView.Name = "reportsView"
        reportsView.Size = New Size(1644, 901)
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
        reportsCard.Size = New Size(1644, 160)
        reportsCard.TabIndex = 0
        ' 
        ' reportsDesc
        ' 
        reportsDesc.Dock = DockStyle.Bottom
        reportsDesc.Font = New Font("Segoe UI", 10F)
        reportsDesc.ForeColor = Color.Gray
        reportsDesc.Location = New Point(20, 100)
        reportsDesc.Name = "reportsDesc"
        reportsDesc.Size = New Size(1604, 40)
        reportsDesc.TabIndex = 1
        reportsDesc.Text = "Generate sales, inventory, and expense reports."
        ' 
        ' reportsHead
        ' 
        reportsHead.Dock = DockStyle.Top
        reportsHead.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        reportsHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        reportsHead.Location = New Point(20, 20)
        reportsHead.Name = "reportsHead"
        reportsHead.Size = New Size(1604, 50)
        reportsHead.TabIndex = 0
        reportsHead.Text = "Reports"
        ' 
        ' Admin
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.White
        ClientSize = New Size(1904, 1041)
        Controls.Add(mainLayout)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Admin"
        Text = "Cabrera's Drugstore - Dashboard"
        WindowState = FormWindowState.Maximized
        mainLayout.ResumeLayout(False)
        sidebar.ResumeLayout(False)
        sidebar.PerformLayout()
        CType(logoBox, ComponentModel.ISupportInitialize).EndInit()
        rightLayout.ResumeLayout(False)
        headerPanel.ResumeLayout(False)
        headerPanel.PerformLayout()
        userPanel.ResumeLayout(False)
        userPanel.PerformLayout()
        CType(userAvatar, ComponentModel.ISupportInitialize).EndInit()
        bodyPanel.ResumeLayout(False)
        dashboardView.ResumeLayout(False)
        cardGrid.ResumeLayout(False)
        card1.ResumeLayout(False)
        card2.ResumeLayout(False)
        card3.ResumeLayout(False)
        card4.ResumeLayout(False)
        card5.ResumeLayout(False)
        card6.ResumeLayout(False)
        card7.ResumeLayout(False)
        card8.ResumeLayout(False)
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
