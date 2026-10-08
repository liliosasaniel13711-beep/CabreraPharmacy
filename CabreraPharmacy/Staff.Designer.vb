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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        mainLayout = New TableLayoutPanel()
        sidebar = New Panel()
        logoBox = New PictureBox()
        PharName = New Label()
        navRetail = New Button()
        navWholesale = New Button()
        navRestock = New Button()
        logoutstaff_button = New Button()
        rightLayout = New TableLayoutPanel()
        headerPanel = New Panel()
        titleLabel = New Label()
        subtitleLabel = New Label()
        userPanel = New Panel()
        userAvatar = New PictureBox()
        welcomestaff_label = New Label()
        userRoleLabel = New Label()
        bodyPanel = New Panel()
        retailView = New Panel()
        searchCard = New Panel()
        searchLabel = New Label()
        searchTextBox = New TextBox()
        searchButton = New Button()
        productCard = New Panel()
        prodNameLabel = New Label()
        prodIdLabel = New Label()
        prodGenericLabel = New Label()
        prodBrandLabel = New Label()
        prodFormLabel = New Label()
        prodStrengthLabel = New Label()
        prodTypeLabel = New Label()
        prodPriceLabel = New Label()
        prodAvailableLabel = New Label()
        prodExpiryLabel = New Label()
        prodBatchLabel = New Label()
        rxSectionLabel = New Label()
        rxNoLabel = New Label()
        rxPractitionerLabel = New Label()
        rxLicenseLabel = New Label()
        rxDateLabel = New Label()
        rxNoTextBox = New TextBox()
        rxPractitionerTextBox = New TextBox()
        rxLicenseTextBox = New TextBox()
        rxDateTextBox = New TextBox()
        qtyLabel = New Label()
        qtyMinusButton = New Button()
        qtyTextBox = New TextBox()
        qtyPlusButton = New Button()
        addToCartButton = New Button()
        cartCard = New Panel()
        cartTitleLabel = New Label()
        cartItemsPanel = New Panel()
        cartSubtotalLabel = New Label()
        cartSubtotalValue = New Label()
        checkoutButton = New Button()
        checkoutView = New Panel()
        orderReviewCard = New Panel()
        orderReviewTitle = New Label()
        customerTypeLabel = New Label()
        customerTypeCombo = New ComboBox()
        customerNameLabel = New Label()
        customerNameTextBox = New TextBox()
        idNumberLabel = New Label()
        idNumberTextBox = New TextBox()
        orderSummaryLabel = New Label()
        orderSummaryPanel = New Panel()
        prescriptionLabel = New Label()
        prescriptionValueLabel = New Label()
        rxRecordedLabel = New Label()
        paymentCard = New Panel()
        paymentTitle = New Label()
        subtotalLabel = New Label()
        subtotalValue = New Label()
        discountLabel = New Label()
        discountValue = New Label()
        totalDueLabel = New Label()
        totalDueValue = New Label()
        cashLabel = New Label()
        cashValue = New Label()
        changeLabel = New Label()
        changeValue = New Label()
        cashTextBox = New TextBox()
        cancelButton = New Button()
        confirmButton = New Button()
        wholesaleView = New Panel()
        wholesaleSearchCard = New Panel()
        wholesaleSearchLabel = New Label()
        wholesaleSearchTextBox = New TextBox()
        wholesaleSearchButton = New Button()
        wholesaleProductCard = New Panel()
        wProdNameLabel = New Label()
        wProdIdLabel = New Label()
        wProdGenericLabel = New Label()
        wProdBrandLabel = New Label()
        wProdFormLabel = New Label()
        wProdStrengthLabel = New Label()
        wProdTypeLabel = New Label()
        wProdPriceLabel = New Label()
        wProdAvailableLabel = New Label()
        wQtyLabel = New Label()
        wQtyMinusButton = New Button()
        wQtyTextBox = New TextBox()
        wQtyPlusButton = New Button()
        wAddToCartButton = New Button()
        wholesaleCartCard = New Panel()
        wholesaleCartTitle = New Label()
        wholesaleCartPanel = New Panel()
        wholesaleSubtotalLabel = New Label()
        wholesaleSubtotalValue = New Label()
        wholesaleCheckoutButton = New Button()
        restockView = New Panel()
        restockCard = New Panel()
        restockDesc = New Label()
        restockHead = New Label()
        mainLayout.SuspendLayout()
        sidebar.SuspendLayout()
        CType(logoBox, ComponentModel.ISupportInitialize).BeginInit()
        rightLayout.SuspendLayout()
        headerPanel.SuspendLayout()
        userPanel.SuspendLayout()
        CType(userAvatar, ComponentModel.ISupportInitialize).BeginInit()
        bodyPanel.SuspendLayout()
        retailView.SuspendLayout()
        searchCard.SuspendLayout()
        productCard.SuspendLayout()
        cartCard.SuspendLayout()
        checkoutView.SuspendLayout()
        orderReviewCard.SuspendLayout()
        paymentCard.SuspendLayout()
        wholesaleView.SuspendLayout()
        wholesaleSearchCard.SuspendLayout()
        wholesaleProductCard.SuspendLayout()
        wholesaleCartCard.SuspendLayout()
        restockView.SuspendLayout()
        restockCard.SuspendLayout()
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
        sidebar.Controls.Add(navRetail)
        sidebar.Controls.Add(navWholesale)
        sidebar.Controls.Add(navRestock)
        sidebar.Controls.Add(logoutstaff_button)
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
        ' navRetail
        ' 
        navRetail.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navRetail.Cursor = Cursors.Hand
        navRetail.FlatAppearance.BorderSize = 0
        navRetail.FlatStyle = FlatStyle.Flat
        navRetail.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navRetail.ForeColor = Color.White
        navRetail.Location = New Point(20, 120)
        navRetail.Name = "navRetail"
        navRetail.Padding = New Padding(10, 0, 0, 0)
        navRetail.Size = New Size(180, 45)
        navRetail.TabIndex = 2
        navRetail.Tag = "Retail"
        navRetail.Text = "Retail"
        navRetail.TextAlign = ContentAlignment.MiddleLeft
        navRetail.UseVisualStyleBackColor = False
        ' 
        ' navWholesale
        ' 
        navWholesale.BackColor = Color.White
        navWholesale.Cursor = Cursors.Hand
        navWholesale.FlatAppearance.BorderSize = 0
        navWholesale.FlatStyle = FlatStyle.Flat
        navWholesale.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navWholesale.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navWholesale.Location = New Point(20, 175)
        navWholesale.Name = "navWholesale"
        navWholesale.Padding = New Padding(10, 0, 0, 0)
        navWholesale.Size = New Size(180, 45)
        navWholesale.TabIndex = 3
        navWholesale.Tag = "Wholesale"
        navWholesale.Text = "Wholesale"
        navWholesale.TextAlign = ContentAlignment.MiddleLeft
        navWholesale.UseVisualStyleBackColor = False
        ' 
        ' navRestock
        ' 
        navRestock.BackColor = Color.White
        navRestock.Cursor = Cursors.Hand
        navRestock.FlatAppearance.BorderSize = 0
        navRestock.FlatStyle = FlatStyle.Flat
        navRestock.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        navRestock.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navRestock.Location = New Point(20, 230)
        navRestock.Name = "navRestock"
        navRestock.Padding = New Padding(10, 0, 0, 0)
        navRestock.Size = New Size(180, 45)
        navRestock.TabIndex = 4
        navRestock.Tag = "Restock"
        navRestock.Text = "Restock"
        navRestock.TextAlign = ContentAlignment.MiddleLeft
        navRestock.UseVisualStyleBackColor = False
        ' 
        ' logoutstaff_button
        ' 
        logoutstaff_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutstaff_button.BackColor = Color.White
        logoutstaff_button.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        logoutstaff_button.FlatStyle = FlatStyle.Flat
        logoutstaff_button.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        logoutstaff_button.Location = New Point(20, 961)
        logoutstaff_button.Name = "logoutstaff_button"
        logoutstaff_button.Size = New Size(180, 45)
        logoutstaff_button.TabIndex = 5
        logoutstaff_button.Text = "Log Out"
        logoutstaff_button.UseVisualStyleBackColor = False
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
        titleLabel.Size = New Size(104, 45)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Retail"
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 11F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(20, 55)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(286, 20)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Cabrera's Drugstore and Medical Supplies"
        ' 
        ' userPanel
        ' 
        userPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        userPanel.AutoSize = True
        userPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
        userPanel.BackColor = Color.Transparent
        userPanel.Controls.Add(userAvatar)
        userPanel.Controls.Add(welcomestaff_label)
        userPanel.Controls.Add(userRoleLabel)
        userPanel.Location = New Point(1476, 10)
        userPanel.Name = "userPanel"
        userPanel.Size = New Size(188, 63)
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
        ' welcomestaff_label
        ' 
        welcomestaff_label.AutoSize = True
        welcomestaff_label.BackColor = Color.Transparent
        welcomestaff_label.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        welcomestaff_label.ForeColor = Color.Black
        welcomestaff_label.Location = New Point(60, 10)
        welcomestaff_label.Name = "welcomestaff_label"
        welcomestaff_label.Size = New Size(108, 20)
        welcomestaff_label.TabIndex = 1
        welcomestaff_label.Text = "welcome staff"
        ' 
        ' userRoleLabel
        ' 
        userRoleLabel.AutoSize = True
        userRoleLabel.Font = New Font("Segoe UI", 9F)
        userRoleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userRoleLabel.Location = New Point(60, 32)
        userRoleLabel.Name = "userRoleLabel"
        userRoleLabel.Size = New Size(125, 15)
        userRoleLabel.TabIndex = 2
        userRoleLabel.Text = "Assistant Pharmacist 1"
        ' 
        ' bodyPanel
        ' 
        bodyPanel.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        bodyPanel.Controls.Add(retailView)
        bodyPanel.Controls.Add(checkoutView)
        bodyPanel.Controls.Add(wholesaleView)
        bodyPanel.Controls.Add(restockView)
        bodyPanel.Dock = DockStyle.Fill
        bodyPanel.Location = New Point(0, 100)
        bodyPanel.Margin = New Padding(0)
        bodyPanel.Name = "bodyPanel"
        bodyPanel.Padding = New Padding(20)
        bodyPanel.Size = New Size(1684, 941)
        bodyPanel.TabIndex = 1
        ' 
        ' retailView
        ' 
        retailView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        retailView.Controls.Add(searchCard)
        retailView.Controls.Add(productCard)
        retailView.Controls.Add(cartCard)
        retailView.Dock = DockStyle.Fill
        retailView.Location = New Point(20, 20)
        retailView.Name = "retailView"
        retailView.Size = New Size(1644, 901)
        retailView.TabIndex = 0
        ' 
        ' searchCard
        ' 
        searchCard.BackColor = Color.White
        searchCard.Controls.Add(searchLabel)
        searchCard.Controls.Add(searchTextBox)
        searchCard.Controls.Add(searchButton)
        searchCard.Location = New Point(0, 0)
        searchCard.Name = "searchCard"
        searchCard.Size = New Size(820, 60)
        searchCard.TabIndex = 0
        ' 
        ' searchLabel
        ' 
        searchLabel.AutoSize = True
        searchLabel.Font = New Font("Segoe UI", 10F)
        searchLabel.ForeColor = Color.Black
        searchLabel.Location = New Point(20, 19)
        searchLabel.Name = "searchLabel"
        searchLabel.Size = New Size(67, 19)
        searchLabel.TabIndex = 0
        searchLabel.Text = "Medicine:"
        ' 
        ' searchTextBox
        ' 
        searchTextBox.Font = New Font("Segoe UI", 10F)
        searchTextBox.Location = New Point(95, 16)
        searchTextBox.Name = "searchTextBox"
        searchTextBox.Size = New Size(380, 25)
        searchTextBox.TabIndex = 1
        ' 
        ' searchButton
        ' 
        searchButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        searchButton.Cursor = Cursors.Hand
        searchButton.FlatAppearance.BorderSize = 0
        searchButton.FlatStyle = FlatStyle.Flat
        searchButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        searchButton.ForeColor = Color.White
        searchButton.Location = New Point(485, 13)
        searchButton.Name = "searchButton"
        searchButton.Size = New Size(100, 32)
        searchButton.TabIndex = 2
        searchButton.Text = "Search"
        searchButton.UseVisualStyleBackColor = False
        ' 
        ' productCard
        ' 
        productCard.BackColor = Color.White
        productCard.Controls.Add(prodNameLabel)
        productCard.Controls.Add(prodIdLabel)
        productCard.Controls.Add(prodGenericLabel)
        productCard.Controls.Add(prodBrandLabel)
        productCard.Controls.Add(prodFormLabel)
        productCard.Controls.Add(prodStrengthLabel)
        productCard.Controls.Add(prodTypeLabel)
        productCard.Controls.Add(prodPriceLabel)
        productCard.Controls.Add(prodAvailableLabel)
        productCard.Controls.Add(prodExpiryLabel)
        productCard.Controls.Add(prodBatchLabel)
        productCard.Controls.Add(rxSectionLabel)
        productCard.Controls.Add(rxNoLabel)
        productCard.Controls.Add(rxPractitionerLabel)
        productCard.Controls.Add(rxLicenseLabel)
        productCard.Controls.Add(rxDateLabel)
        productCard.Controls.Add(rxNoTextBox)
        productCard.Controls.Add(rxPractitionerTextBox)
        productCard.Controls.Add(rxLicenseTextBox)
        productCard.Controls.Add(rxDateTextBox)
        productCard.Controls.Add(qtyLabel)
        productCard.Controls.Add(qtyMinusButton)
        productCard.Controls.Add(qtyTextBox)
        productCard.Controls.Add(qtyPlusButton)
        productCard.Controls.Add(addToCartButton)
        productCard.Location = New Point(0, 70)
        productCard.Name = "productCard"
        productCard.Size = New Size(820, 560)
        productCard.TabIndex = 1
        ' 
        ' prodNameLabel
        ' 
        prodNameLabel.AutoSize = True
        prodNameLabel.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        prodNameLabel.ForeColor = Color.Black
        prodNameLabel.Location = New Point(20, 20)
        prodNameLabel.Name = "prodNameLabel"
        prodNameLabel.Size = New Size(148, 25)
        prodNameLabel.TabIndex = 0
        prodNameLabel.Text = "Product Details"
        ' 
        ' prodIdLabel
        ' 
        prodIdLabel.AutoSize = True
        prodIdLabel.Font = New Font("Segoe UI", 9F)
        prodIdLabel.ForeColor = Color.Gray
        prodIdLabel.Location = New Point(20, 60)
        prodIdLabel.Name = "prodIdLabel"
        prodIdLabel.Size = New Size(66, 15)
        prodIdLabel.TabIndex = 1
        prodIdLabel.Text = "Product ID:"
        ' 
        ' prodGenericLabel
        ' 
        prodGenericLabel.AutoSize = True
        prodGenericLabel.Font = New Font("Segoe UI", 9F)
        prodGenericLabel.ForeColor = Color.Gray
        prodGenericLabel.Location = New Point(20, 85)
        prodGenericLabel.Name = "prodGenericLabel"
        prodGenericLabel.Size = New Size(83, 15)
        prodGenericLabel.TabIndex = 2
        prodGenericLabel.Text = "Generic name:"
        ' 
        ' prodBrandLabel
        ' 
        prodBrandLabel.AutoSize = True
        prodBrandLabel.Font = New Font("Segoe UI", 9F)
        prodBrandLabel.ForeColor = Color.Gray
        prodBrandLabel.Location = New Point(20, 110)
        prodBrandLabel.Name = "prodBrandLabel"
        prodBrandLabel.Size = New Size(74, 15)
        prodBrandLabel.TabIndex = 3
        prodBrandLabel.Text = "Brand name:"
        ' 
        ' prodFormLabel
        ' 
        prodFormLabel.AutoSize = True
        prodFormLabel.Font = New Font("Segoe UI", 9F)
        prodFormLabel.ForeColor = Color.Gray
        prodFormLabel.Location = New Point(20, 135)
        prodFormLabel.Name = "prodFormLabel"
        prodFormLabel.Size = New Size(38, 15)
        prodFormLabel.TabIndex = 4
        prodFormLabel.Text = "Form:"
        ' 
        ' prodStrengthLabel
        ' 
        prodStrengthLabel.AutoSize = True
        prodStrengthLabel.Font = New Font("Segoe UI", 9F)
        prodStrengthLabel.ForeColor = Color.Gray
        prodStrengthLabel.Location = New Point(20, 160)
        prodStrengthLabel.Name = "prodStrengthLabel"
        prodStrengthLabel.Size = New Size(55, 15)
        prodStrengthLabel.TabIndex = 5
        prodStrengthLabel.Text = "Strength:"
        ' 
        ' prodTypeLabel
        ' 
        prodTypeLabel.AutoSize = True
        prodTypeLabel.Font = New Font("Segoe UI", 9F)
        prodTypeLabel.ForeColor = Color.Gray
        prodTypeLabel.Location = New Point(20, 185)
        prodTypeLabel.Name = "prodTypeLabel"
        prodTypeLabel.Size = New Size(35, 15)
        prodTypeLabel.TabIndex = 6
        prodTypeLabel.Text = "Type:"
        ' 
        ' prodPriceLabel
        ' 
        prodPriceLabel.AutoSize = True
        prodPriceLabel.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        prodPriceLabel.ForeColor = Color.Black
        prodPriceLabel.Location = New Point(20, 225)
        prodPriceLabel.Name = "prodPriceLabel"
        prodPriceLabel.Size = New Size(51, 19)
        prodPriceLabel.TabIndex = 7
        prodPriceLabel.Text = "PRICE:"
        ' 
        ' prodAvailableLabel
        ' 
        prodAvailableLabel.AutoSize = True
        prodAvailableLabel.Font = New Font("Segoe UI", 9F)
        prodAvailableLabel.ForeColor = Color.Gray
        prodAvailableLabel.Location = New Point(20, 260)
        prodAvailableLabel.Name = "prodAvailableLabel"
        prodAvailableLabel.Size = New Size(58, 15)
        prodAvailableLabel.TabIndex = 8
        prodAvailableLabel.Text = "Available:"
        ' 
        ' prodExpiryLabel
        ' 
        prodExpiryLabel.AutoSize = True
        prodExpiryLabel.Font = New Font("Segoe UI", 9F)
        prodExpiryLabel.ForeColor = Color.Gray
        prodExpiryLabel.Location = New Point(20, 285)
        prodExpiryLabel.Name = "prodExpiryLabel"
        prodExpiryLabel.Size = New Size(68, 15)
        prodExpiryLabel.TabIndex = 9
        prodExpiryLabel.Text = "Expiry Date:"
        ' 
        ' prodBatchLabel
        ' 
        prodBatchLabel.AutoSize = True
        prodBatchLabel.Font = New Font("Segoe UI", 9F)
        prodBatchLabel.ForeColor = Color.Gray
        prodBatchLabel.Location = New Point(20, 310)
        prodBatchLabel.Name = "prodBatchLabel"
        prodBatchLabel.Size = New Size(62, 15)
        prodBatchLabel.TabIndex = 10
        prodBatchLabel.Text = "Batch No.:"
        ' 
        ' rxSectionLabel
        ' 
        rxSectionLabel.AutoSize = True
        rxSectionLabel.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        rxSectionLabel.ForeColor = Color.Black
        rxSectionLabel.Location = New Point(20, 350)
        rxSectionLabel.Name = "rxSectionLabel"
        rxSectionLabel.Size = New Size(186, 20)
        rxSectionLabel.TabIndex = 11
        rxSectionLabel.Text = "Prescription Information:"
        rxSectionLabel.Visible = False
        ' 
        ' rxNoLabel
        ' 
        rxNoLabel.AutoSize = True
        rxNoLabel.Font = New Font("Segoe UI", 9F)
        rxNoLabel.ForeColor = Color.Gray
        rxNoLabel.Location = New Point(20, 390)
        rxNoLabel.Name = "rxNoLabel"
        rxNoLabel.Size = New Size(95, 15)
        rxNoLabel.TabIndex = 12
        rxNoLabel.Text = "Prescription No.:"
        rxNoLabel.Visible = False
        ' 
        ' rxPractitionerLabel
        ' 
        rxPractitionerLabel.AutoSize = True
        rxPractitionerLabel.Font = New Font("Segoe UI", 9F)
        rxPractitionerLabel.ForeColor = Color.Gray
        rxPractitionerLabel.Location = New Point(20, 420)
        rxPractitionerLabel.Name = "rxPractitionerLabel"
        rxPractitionerLabel.Size = New Size(133, 15)
        rxPractitionerLabel.TabIndex = 13
        rxPractitionerLabel.Text = "Prescribing Practitioner:"
        rxPractitionerLabel.Visible = False
        ' 
        ' rxLicenseLabel
        ' 
        rxLicenseLabel.AutoSize = True
        rxLicenseLabel.Font = New Font("Segoe UI", 9F)
        rxLicenseLabel.ForeColor = Color.Gray
        rxLicenseLabel.Location = New Point(20, 450)
        rxLicenseLabel.Name = "rxLicenseLabel"
        rxLicenseLabel.Size = New Size(96, 15)
        rxLicenseLabel.TabIndex = 14
        rxLicenseLabel.Text = "PRC License No.:"
        rxLicenseLabel.Visible = False
        ' 
        ' rxDateLabel
        ' 
        rxDateLabel.AutoSize = True
        rxDateLabel.Font = New Font("Segoe UI", 9F)
        rxDateLabel.ForeColor = Color.Gray
        rxDateLabel.Location = New Point(20, 480)
        rxDateLabel.Name = "rxDateLabel"
        rxDateLabel.Size = New Size(100, 15)
        rxDateLabel.TabIndex = 15
        rxDateLabel.Text = "Prescription Date:"
        rxDateLabel.Visible = False
        ' 
        ' rxNoTextBox
        ' 
        rxNoTextBox.Font = New Font("Segoe UI", 9F)
        rxNoTextBox.Location = New Point(280, 387)
        rxNoTextBox.Name = "rxNoTextBox"
        rxNoTextBox.Size = New Size(220, 23)
        rxNoTextBox.TabIndex = 16
        rxNoTextBox.Visible = False
        ' 
        ' rxPractitionerTextBox
        ' 
        rxPractitionerTextBox.Font = New Font("Segoe UI", 9F)
        rxPractitionerTextBox.Location = New Point(280, 417)
        rxPractitionerTextBox.Name = "rxPractitionerTextBox"
        rxPractitionerTextBox.Size = New Size(220, 23)
        rxPractitionerTextBox.TabIndex = 17
        rxPractitionerTextBox.Visible = False
        ' 
        ' rxLicenseTextBox
        ' 
        rxLicenseTextBox.Font = New Font("Segoe UI", 9F)
        rxLicenseTextBox.Location = New Point(280, 447)
        rxLicenseTextBox.Name = "rxLicenseTextBox"
        rxLicenseTextBox.Size = New Size(220, 23)
        rxLicenseTextBox.TabIndex = 18
        rxLicenseTextBox.Visible = False
        ' 
        ' rxDateTextBox
        ' 
        rxDateTextBox.Font = New Font("Segoe UI", 9F)
        rxDateTextBox.Location = New Point(280, 477)
        rxDateTextBox.Name = "rxDateTextBox"
        rxDateTextBox.Size = New Size(220, 23)
        rxDateTextBox.TabIndex = 19
        rxDateTextBox.Visible = False
        ' 
        ' qtyLabel
        ' 
        qtyLabel.AutoSize = True
        qtyLabel.Font = New Font("Segoe UI", 10F)
        qtyLabel.ForeColor = Color.Black
        qtyLabel.Location = New Point(20, 525)
        qtyLabel.Name = "qtyLabel"
        qtyLabel.Size = New Size(66, 19)
        qtyLabel.TabIndex = 20
        qtyLabel.Text = "Quantity:"
        ' 
        ' qtyMinusButton
        ' 
        qtyMinusButton.BackColor = Color.White
        qtyMinusButton.Cursor = Cursors.Hand
        qtyMinusButton.FlatAppearance.BorderColor = Color.LightGray
        qtyMinusButton.FlatStyle = FlatStyle.Flat
        qtyMinusButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        qtyMinusButton.ForeColor = Color.Black
        qtyMinusButton.Location = New Point(90, 522)
        qtyMinusButton.Name = "qtyMinusButton"
        qtyMinusButton.Size = New Size(30, 28)
        qtyMinusButton.TabIndex = 21
        qtyMinusButton.Text = "-"
        qtyMinusButton.UseVisualStyleBackColor = False
        ' 
        ' qtyTextBox
        ' 
        qtyTextBox.Font = New Font("Segoe UI", 10F)
        qtyTextBox.Location = New Point(126, 523)
        qtyTextBox.Name = "qtyTextBox"
        qtyTextBox.Size = New Size(50, 25)
        qtyTextBox.TabIndex = 22
        qtyTextBox.Text = "1"
        qtyTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' qtyPlusButton
        ' 
        qtyPlusButton.BackColor = Color.White
        qtyPlusButton.Cursor = Cursors.Hand
        qtyPlusButton.FlatAppearance.BorderColor = Color.LightGray
        qtyPlusButton.FlatStyle = FlatStyle.Flat
        qtyPlusButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        qtyPlusButton.ForeColor = Color.Black
        qtyPlusButton.Location = New Point(182, 522)
        qtyPlusButton.Name = "qtyPlusButton"
        qtyPlusButton.Size = New Size(30, 28)
        qtyPlusButton.TabIndex = 23
        qtyPlusButton.Text = "+"
        qtyPlusButton.UseVisualStyleBackColor = False
        ' 
        ' addToCartButton
        ' 
        addToCartButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        addToCartButton.Cursor = Cursors.Hand
        addToCartButton.FlatAppearance.BorderSize = 0
        addToCartButton.FlatStyle = FlatStyle.Flat
        addToCartButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        addToCartButton.ForeColor = Color.White
        addToCartButton.Location = New Point(660, 515)
        addToCartButton.Name = "addToCartButton"
        addToCartButton.Size = New Size(140, 35)
        addToCartButton.TabIndex = 24
        addToCartButton.Text = "ADD TO CART"
        addToCartButton.UseVisualStyleBackColor = False
        ' 
        ' cartCard
        ' 
        cartCard.BackColor = Color.White
        cartCard.Controls.Add(cartTitleLabel)
        cartCard.Controls.Add(cartItemsPanel)
        cartCard.Controls.Add(cartSubtotalLabel)
        cartCard.Controls.Add(cartSubtotalValue)
        cartCard.Controls.Add(checkoutButton)
        cartCard.Location = New Point(840, 0)
        cartCard.Name = "cartCard"
        cartCard.Size = New Size(804, 630)
        cartCard.TabIndex = 2
        ' 
        ' cartTitleLabel
        ' 
        cartTitleLabel.AutoSize = True
        cartTitleLabel.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        cartTitleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        cartTitleLabel.Location = New Point(20, 20)
        cartTitleLabel.Name = "cartTitleLabel"
        cartTitleLabel.Size = New Size(142, 25)
        cartTitleLabel.TabIndex = 0
        cartTitleLabel.Text = "Shopping Cart"
        ' 
        ' cartItemsPanel
        ' 
        cartItemsPanel.AutoScroll = True
        cartItemsPanel.BackColor = Color.White
        cartItemsPanel.Location = New Point(20, 60)
        cartItemsPanel.Name = "cartItemsPanel"
        cartItemsPanel.Size = New Size(764, 460)
        cartItemsPanel.TabIndex = 1
        ' 
        ' cartSubtotalLabel
        ' 
        cartSubtotalLabel.AutoSize = True
        cartSubtotalLabel.Font = New Font("Segoe UI", 11F)
        cartSubtotalLabel.ForeColor = Color.Gray
        cartSubtotalLabel.Location = New Point(20, 545)
        cartSubtotalLabel.Name = "cartSubtotalLabel"
        cartSubtotalLabel.Size = New Size(68, 20)
        cartSubtotalLabel.TabIndex = 2
        cartSubtotalLabel.Text = "Subtotal:"
        ' 
        ' cartSubtotalValue
        ' 
        cartSubtotalValue.AutoSize = True
        cartSubtotalValue.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        cartSubtotalValue.ForeColor = Color.Black
        cartSubtotalValue.Location = New Point(660, 545)
        cartSubtotalValue.Name = "cartSubtotalValue"
        cartSubtotalValue.Size = New Size(53, 20)
        cartSubtotalValue.TabIndex = 3
        cartSubtotalValue.Text = "P 0.00"
        ' 
        ' checkoutButton
        ' 
        checkoutButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        checkoutButton.Cursor = Cursors.Hand
        checkoutButton.FlatAppearance.BorderSize = 0
        checkoutButton.FlatStyle = FlatStyle.Flat
        checkoutButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        checkoutButton.ForeColor = Color.White
        checkoutButton.Location = New Point(644, 580)
        checkoutButton.Name = "checkoutButton"
        checkoutButton.Size = New Size(140, 35)
        checkoutButton.TabIndex = 4
        checkoutButton.Text = "CHECKOUT"
        checkoutButton.UseVisualStyleBackColor = False
        ' 
        ' checkoutView
        ' 
        checkoutView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        checkoutView.Controls.Add(orderReviewCard)
        checkoutView.Controls.Add(paymentCard)
        checkoutView.Dock = DockStyle.Fill
        checkoutView.Location = New Point(20, 20)
        checkoutView.Name = "checkoutView"
        checkoutView.Size = New Size(1644, 901)
        checkoutView.TabIndex = 1
        checkoutView.Visible = False
        ' 
        ' orderReviewCard
        ' 
        orderReviewCard.BackColor = Color.White
        orderReviewCard.Controls.Add(orderReviewTitle)
        orderReviewCard.Controls.Add(customerTypeLabel)
        orderReviewCard.Controls.Add(customerTypeCombo)
        orderReviewCard.Controls.Add(customerNameLabel)
        orderReviewCard.Controls.Add(customerNameTextBox)
        orderReviewCard.Controls.Add(idNumberLabel)
        orderReviewCard.Controls.Add(idNumberTextBox)
        orderReviewCard.Controls.Add(orderSummaryLabel)
        orderReviewCard.Controls.Add(orderSummaryPanel)
        orderReviewCard.Controls.Add(prescriptionLabel)
        orderReviewCard.Controls.Add(prescriptionValueLabel)
        orderReviewCard.Controls.Add(rxRecordedLabel)
        orderReviewCard.Location = New Point(0, 0)
        orderReviewCard.Name = "orderReviewCard"
        orderReviewCard.Size = New Size(820, 630)
        orderReviewCard.TabIndex = 0
        ' 
        ' orderReviewTitle
        ' 
        orderReviewTitle.AutoSize = True
        orderReviewTitle.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        orderReviewTitle.ForeColor = Color.Black
        orderReviewTitle.Location = New Point(20, 20)
        orderReviewTitle.Name = "orderReviewTitle"
        orderReviewTitle.Size = New Size(131, 25)
        orderReviewTitle.TabIndex = 0
        orderReviewTitle.Text = "Order Review"
        ' 
        ' customerTypeLabel
        ' 
        customerTypeLabel.AutoSize = True
        customerTypeLabel.Font = New Font("Segoe UI", 10F)
        customerTypeLabel.ForeColor = Color.Black
        customerTypeLabel.Location = New Point(20, 65)
        customerTypeLabel.Name = "customerTypeLabel"
        customerTypeLabel.Size = New Size(114, 19)
        customerTypeLabel.TabIndex = 1
        customerTypeLabel.Text = "CUSTOMER TYPE"
        ' 
        ' customerTypeCombo
        ' 
        customerTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList
        customerTypeCombo.Font = New Font("Segoe UI", 10F)
        customerTypeCombo.FormattingEnabled = True
        customerTypeCombo.Items.AddRange(New Object() {"Regular", "PWD", "Senior Citizen"})
        customerTypeCombo.Location = New Point(20, 90)
        customerTypeCombo.Name = "customerTypeCombo"
        customerTypeCombo.Size = New Size(250, 25)
        customerTypeCombo.TabIndex = 2
        ' 
        ' customerNameLabel
        ' 
        customerNameLabel.AutoSize = True
        customerNameLabel.Font = New Font("Segoe UI", 9F)
        customerNameLabel.ForeColor = Color.Gray
        customerNameLabel.Location = New Point(20, 140)
        customerNameLabel.Name = "customerNameLabel"
        customerNameLabel.Size = New Size(97, 15)
        customerNameLabel.TabIndex = 3
        customerNameLabel.Text = "Customer Name:"
        ' 
        ' customerNameTextBox
        ' 
        customerNameTextBox.Font = New Font("Segoe UI", 9F)
        customerNameTextBox.Location = New Point(140, 137)
        customerNameTextBox.Name = "customerNameTextBox"
        customerNameTextBox.Size = New Size(250, 23)
        customerNameTextBox.TabIndex = 4
        ' 
        ' idNumberLabel
        ' 
        idNumberLabel.AutoSize = True
        idNumberLabel.Font = New Font("Segoe UI", 9F)
        idNumberLabel.ForeColor = Color.Gray
        idNumberLabel.Location = New Point(20, 170)
        idNumberLabel.Name = "idNumberLabel"
        idNumberLabel.Size = New Size(68, 15)
        idNumberLabel.TabIndex = 5
        idNumberLabel.Text = "ID Number:"
        ' 
        ' idNumberTextBox
        ' 
        idNumberTextBox.Font = New Font("Segoe UI", 9F)
        idNumberTextBox.Location = New Point(140, 167)
        idNumberTextBox.Name = "idNumberTextBox"
        idNumberTextBox.Size = New Size(250, 23)
        idNumberTextBox.TabIndex = 6
        ' 
        ' orderSummaryLabel
        ' 
        orderSummaryLabel.AutoSize = True
        orderSummaryLabel.Font = New Font("Segoe UI", 10F)
        orderSummaryLabel.ForeColor = Color.Black
        orderSummaryLabel.Location = New Point(20, 220)
        orderSummaryLabel.Name = "orderSummaryLabel"
        orderSummaryLabel.Size = New Size(125, 19)
        orderSummaryLabel.TabIndex = 7
        orderSummaryLabel.Text = "ORDER SUMMARY"
        ' 
        ' orderSummaryPanel
        ' 
        orderSummaryPanel.AutoScroll = True
        orderSummaryPanel.BackColor = Color.White
        orderSummaryPanel.Location = New Point(20, 250)
        orderSummaryPanel.Name = "orderSummaryPanel"
        orderSummaryPanel.Size = New Size(780, 250)
        orderSummaryPanel.TabIndex = 8
        ' 
        ' prescriptionLabel
        ' 
        prescriptionLabel.AutoSize = True
        prescriptionLabel.Font = New Font("Segoe UI", 10F)
        prescriptionLabel.ForeColor = Color.Black
        prescriptionLabel.Location = New Point(20, 520)
        prescriptionLabel.Name = "prescriptionLabel"
        prescriptionLabel.Size = New Size(100, 19)
        prescriptionLabel.TabIndex = 9
        prescriptionLabel.Text = "PRESCRIPTION"
        prescriptionLabel.Visible = False
        ' 
        ' prescriptionValueLabel
        ' 
        prescriptionValueLabel.AutoSize = True
        prescriptionValueLabel.Font = New Font("Segoe UI", 9F)
        prescriptionValueLabel.ForeColor = Color.Gray
        prescriptionValueLabel.Location = New Point(20, 545)
        prescriptionValueLabel.Name = "prescriptionValueLabel"
        prescriptionValueLabel.Size = New Size(69, 15)
        prescriptionValueLabel.TabIndex = 10
        prescriptionValueLabel.Text = "RX- G3200D"
        prescriptionValueLabel.Visible = False
        ' 
        ' rxRecordedLabel
        ' 
        rxRecordedLabel.AutoSize = True
        rxRecordedLabel.Font = New Font("Segoe UI", 9F)
        rxRecordedLabel.ForeColor = Color.Gray
        rxRecordedLabel.Location = New Point(540, 545)
        rxRecordedLabel.Name = "rxRecordedLabel"
        rxRecordedLabel.Size = New Size(149, 15)
        rxRecordedLabel.TabIndex = 11
        rxRecordedLabel.Text = "INFORMATION RECORDED"
        rxRecordedLabel.Visible = False
        ' 
        ' paymentCard
        ' 
        paymentCard.BackColor = Color.White
        paymentCard.Controls.Add(paymentTitle)
        paymentCard.Controls.Add(subtotalLabel)
        paymentCard.Controls.Add(subtotalValue)
        paymentCard.Controls.Add(discountLabel)
        paymentCard.Controls.Add(discountValue)
        paymentCard.Controls.Add(totalDueLabel)
        paymentCard.Controls.Add(totalDueValue)
        paymentCard.Controls.Add(cashLabel)
        paymentCard.Controls.Add(cashValue)
        paymentCard.Controls.Add(changeLabel)
        paymentCard.Controls.Add(changeValue)
        paymentCard.Controls.Add(cashTextBox)
        paymentCard.Controls.Add(cancelButton)
        paymentCard.Controls.Add(confirmButton)
        paymentCard.Location = New Point(840, 0)
        paymentCard.Name = "paymentCard"
        paymentCard.Size = New Size(804, 630)
        paymentCard.TabIndex = 1
        ' 
        ' paymentTitle
        ' 
        paymentTitle.AutoSize = True
        paymentTitle.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        paymentTitle.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        paymentTitle.Location = New Point(20, 20)
        paymentTitle.Name = "paymentTitle"
        paymentTitle.Size = New Size(90, 25)
        paymentTitle.TabIndex = 0
        paymentTitle.Text = "Payment"
        ' 
        ' subtotalLabel
        ' 
        subtotalLabel.AutoSize = True
        subtotalLabel.Font = New Font("Segoe UI", 11F)
        subtotalLabel.ForeColor = Color.Gray
        subtotalLabel.Location = New Point(20, 70)
        subtotalLabel.Name = "subtotalLabel"
        subtotalLabel.Size = New Size(79, 20)
        subtotalLabel.TabIndex = 1
        subtotalLabel.Text = "SUBTOTAL:"
        ' 
        ' subtotalValue
        ' 
        subtotalValue.AutoSize = True
        subtotalValue.Font = New Font("Segoe UI", 11F)
        subtotalValue.ForeColor = Color.Black
        subtotalValue.Location = New Point(660, 70)
        subtotalValue.Name = "subtotalValue"
        subtotalValue.Size = New Size(48, 20)
        subtotalValue.TabIndex = 2
        subtotalValue.Text = "P 0.00"
        ' 
        ' discountLabel
        ' 
        discountLabel.AutoSize = True
        discountLabel.Font = New Font("Segoe UI", 11F)
        discountLabel.ForeColor = Color.Gray
        discountLabel.Location = New Point(20, 100)
        discountLabel.Name = "discountLabel"
        discountLabel.Size = New Size(84, 20)
        discountLabel.TabIndex = 3
        discountLabel.Text = "DISCOUNT:"
        ' 
        ' discountValue
        ' 
        discountValue.AutoSize = True
        discountValue.Font = New Font("Segoe UI", 11F)
        discountValue.ForeColor = Color.Black
        discountValue.Location = New Point(660, 100)
        discountValue.Name = "discountValue"
        discountValue.Size = New Size(48, 20)
        discountValue.TabIndex = 4
        discountValue.Text = "P 0.00"
        ' 
        ' totalDueLabel
        ' 
        totalDueLabel.AutoSize = True
        totalDueLabel.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        totalDueLabel.ForeColor = Color.Black
        totalDueLabel.Location = New Point(20, 130)
        totalDueLabel.Name = "totalDueLabel"
        totalDueLabel.Size = New Size(92, 20)
        totalDueLabel.TabIndex = 5
        totalDueLabel.Text = "TOTAL DUE:"
        ' 
        ' totalDueValue
        ' 
        totalDueValue.AutoSize = True
        totalDueValue.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        totalDueValue.ForeColor = Color.Black
        totalDueValue.Location = New Point(660, 130)
        totalDueValue.Name = "totalDueValue"
        totalDueValue.Size = New Size(53, 20)
        totalDueValue.TabIndex = 6
        totalDueValue.Text = "P 0.00"
        ' 
        ' cashLabel
        ' 
        cashLabel.AutoSize = True
        cashLabel.Font = New Font("Segoe UI", 11F)
        cashLabel.ForeColor = Color.Gray
        cashLabel.Location = New Point(20, 200)
        cashLabel.Name = "cashLabel"
        cashLabel.Size = New Size(120, 20)
        cashLabel.TabIndex = 7
        cashLabel.Text = "CASH RECEIVED:"
        ' 
        ' cashValue
        ' 
        cashValue.AutoSize = True
        cashValue.Font = New Font("Segoe UI", 11F)
        cashValue.ForeColor = Color.Black
        cashValue.Location = New Point(660, 200)
        cashValue.Name = "cashValue"
        cashValue.Size = New Size(48, 20)
        cashValue.TabIndex = 8
        cashValue.Text = "P 0.00"
        ' 
        ' changeLabel
        ' 
        changeLabel.AutoSize = True
        changeLabel.Font = New Font("Segoe UI", 11F)
        changeLabel.ForeColor = Color.Gray
        changeLabel.Location = New Point(20, 230)
        changeLabel.Name = "changeLabel"
        changeLabel.Size = New Size(71, 20)
        changeLabel.TabIndex = 9
        changeLabel.Text = "CHANGE:"
        ' 
        ' changeValue
        ' 
        changeValue.AutoSize = True
        changeValue.Font = New Font("Segoe UI", 11F)
        changeValue.ForeColor = Color.Black
        changeValue.Location = New Point(660, 230)
        changeValue.Name = "changeValue"
        changeValue.Size = New Size(48, 20)
        changeValue.TabIndex = 10
        changeValue.Text = "P 0.00"
        ' 
        ' cashTextBox
        ' 
        cashTextBox.Font = New Font("Segoe UI", 11F)
        cashTextBox.Location = New Point(20, 280)
        cashTextBox.Name = "cashTextBox"
        cashTextBox.Size = New Size(250, 27)
        cashTextBox.TabIndex = 11
        ' 
        ' cancelButton
        ' 
        cancelButton.BackColor = Color.FromArgb(CByte(200), CByte(30), CByte(30))
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Flat
        cancelButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        cancelButton.ForeColor = Color.White
        cancelButton.Location = New Point(560, 580)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(100, 35)
        cancelButton.TabIndex = 12
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = False
        ' 
        ' confirmButton
        ' 
        confirmButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        confirmButton.Cursor = Cursors.Hand
        confirmButton.FlatAppearance.BorderSize = 0
        confirmButton.FlatStyle = FlatStyle.Flat
        confirmButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        confirmButton.ForeColor = Color.White
        confirmButton.Location = New Point(670, 580)
        confirmButton.Name = "confirmButton"
        confirmButton.Size = New Size(114, 35)
        confirmButton.TabIndex = 13
        confirmButton.Text = "CONFIRM"
        confirmButton.UseVisualStyleBackColor = False
        ' 
        ' wholesaleView
        ' 
        wholesaleView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        wholesaleView.Controls.Add(wholesaleSearchCard)
        wholesaleView.Controls.Add(wholesaleProductCard)
        wholesaleView.Controls.Add(wholesaleCartCard)
        wholesaleView.Dock = DockStyle.Fill
        wholesaleView.Location = New Point(20, 20)
        wholesaleView.Name = "wholesaleView"
        wholesaleView.Size = New Size(1644, 901)
        wholesaleView.TabIndex = 2
        wholesaleView.Visible = False
        ' 
        ' wholesaleSearchCard
        ' 
        wholesaleSearchCard.BackColor = Color.White
        wholesaleSearchCard.Controls.Add(wholesaleSearchLabel)
        wholesaleSearchCard.Controls.Add(wholesaleSearchTextBox)
        wholesaleSearchCard.Controls.Add(wholesaleSearchButton)
        wholesaleSearchCard.Location = New Point(0, 0)
        wholesaleSearchCard.Name = "wholesaleSearchCard"
        wholesaleSearchCard.Size = New Size(820, 60)
        wholesaleSearchCard.TabIndex = 0
        ' 
        ' wholesaleSearchLabel
        ' 
        wholesaleSearchLabel.AutoSize = True
        wholesaleSearchLabel.Font = New Font("Segoe UI", 10F)
        wholesaleSearchLabel.ForeColor = Color.Black
        wholesaleSearchLabel.Location = New Point(20, 19)
        wholesaleSearchLabel.Name = "wholesaleSearchLabel"
        wholesaleSearchLabel.Size = New Size(67, 19)
        wholesaleSearchLabel.TabIndex = 0
        wholesaleSearchLabel.Text = "Medicine:"
        ' 
        ' wholesaleSearchTextBox
        ' 
        wholesaleSearchTextBox.Font = New Font("Segoe UI", 10F)
        wholesaleSearchTextBox.Location = New Point(95, 16)
        wholesaleSearchTextBox.Name = "wholesaleSearchTextBox"
        wholesaleSearchTextBox.Size = New Size(380, 25)
        wholesaleSearchTextBox.TabIndex = 1
        ' 
        ' wholesaleSearchButton
        ' 
        wholesaleSearchButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wholesaleSearchButton.Cursor = Cursors.Hand
        wholesaleSearchButton.FlatAppearance.BorderSize = 0
        wholesaleSearchButton.FlatStyle = FlatStyle.Flat
        wholesaleSearchButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wholesaleSearchButton.ForeColor = Color.White
        wholesaleSearchButton.Location = New Point(485, 13)
        wholesaleSearchButton.Name = "wholesaleSearchButton"
        wholesaleSearchButton.Size = New Size(100, 32)
        wholesaleSearchButton.TabIndex = 2
        wholesaleSearchButton.Text = "Search"
        wholesaleSearchButton.UseVisualStyleBackColor = False
        ' 
        ' wholesaleProductCard
        ' 
        wholesaleProductCard.BackColor = Color.White
        wholesaleProductCard.Controls.Add(wProdNameLabel)
        wholesaleProductCard.Controls.Add(wProdIdLabel)
        wholesaleProductCard.Controls.Add(wProdGenericLabel)
        wholesaleProductCard.Controls.Add(wProdBrandLabel)
        wholesaleProductCard.Controls.Add(wProdFormLabel)
        wholesaleProductCard.Controls.Add(wProdStrengthLabel)
        wholesaleProductCard.Controls.Add(wProdTypeLabel)
        wholesaleProductCard.Controls.Add(wProdPriceLabel)
        wholesaleProductCard.Controls.Add(wProdAvailableLabel)
        wholesaleProductCard.Controls.Add(wQtyLabel)
        wholesaleProductCard.Controls.Add(wQtyMinusButton)
        wholesaleProductCard.Controls.Add(wQtyTextBox)
        wholesaleProductCard.Controls.Add(wQtyPlusButton)
        wholesaleProductCard.Controls.Add(wAddToCartButton)
        wholesaleProductCard.Location = New Point(0, 70)
        wholesaleProductCard.Name = "wholesaleProductCard"
        wholesaleProductCard.Size = New Size(820, 560)
        wholesaleProductCard.TabIndex = 1
        ' 
        ' wProdNameLabel
        ' 
        wProdNameLabel.AutoSize = True
        wProdNameLabel.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        wProdNameLabel.ForeColor = Color.Black
        wProdNameLabel.Location = New Point(20, 20)
        wProdNameLabel.Name = "wProdNameLabel"
        wProdNameLabel.Size = New Size(148, 25)
        wProdNameLabel.TabIndex = 0
        wProdNameLabel.Text = "Product Details"
        ' 
        ' wProdIdLabel
        ' 
        wProdIdLabel.AutoSize = True
        wProdIdLabel.Font = New Font("Segoe UI", 9F)
        wProdIdLabel.ForeColor = Color.Gray
        wProdIdLabel.Location = New Point(20, 60)
        wProdIdLabel.Name = "wProdIdLabel"
        wProdIdLabel.Size = New Size(66, 15)
        wProdIdLabel.TabIndex = 1
        wProdIdLabel.Text = "Product ID:"
        ' 
        ' wProdGenericLabel
        ' 
        wProdGenericLabel.AutoSize = True
        wProdGenericLabel.Font = New Font("Segoe UI", 9F)
        wProdGenericLabel.ForeColor = Color.Gray
        wProdGenericLabel.Location = New Point(20, 85)
        wProdGenericLabel.Name = "wProdGenericLabel"
        wProdGenericLabel.Size = New Size(83, 15)
        wProdGenericLabel.TabIndex = 2
        wProdGenericLabel.Text = "Generic name:"
        ' 
        ' wProdBrandLabel
        ' 
        wProdBrandLabel.AutoSize = True
        wProdBrandLabel.Font = New Font("Segoe UI", 9F)
        wProdBrandLabel.ForeColor = Color.Gray
        wProdBrandLabel.Location = New Point(20, 110)
        wProdBrandLabel.Name = "wProdBrandLabel"
        wProdBrandLabel.Size = New Size(74, 15)
        wProdBrandLabel.TabIndex = 3
        wProdBrandLabel.Text = "Brand name:"
        ' 
        ' wProdFormLabel
        ' 
        wProdFormLabel.AutoSize = True
        wProdFormLabel.Font = New Font("Segoe UI", 9F)
        wProdFormLabel.ForeColor = Color.Gray
        wProdFormLabel.Location = New Point(20, 135)
        wProdFormLabel.Name = "wProdFormLabel"
        wProdFormLabel.Size = New Size(38, 15)
        wProdFormLabel.TabIndex = 4
        wProdFormLabel.Text = "Form:"
        ' 
        ' wProdStrengthLabel
        ' 
        wProdStrengthLabel.AutoSize = True
        wProdStrengthLabel.Font = New Font("Segoe UI", 9F)
        wProdStrengthLabel.ForeColor = Color.Gray
        wProdStrengthLabel.Location = New Point(20, 160)
        wProdStrengthLabel.Name = "wProdStrengthLabel"
        wProdStrengthLabel.Size = New Size(55, 15)
        wProdStrengthLabel.TabIndex = 5
        wProdStrengthLabel.Text = "Strength:"
        ' 
        ' wProdTypeLabel
        ' 
        wProdTypeLabel.AutoSize = True
        wProdTypeLabel.Font = New Font("Segoe UI", 9F)
        wProdTypeLabel.ForeColor = Color.Gray
        wProdTypeLabel.Location = New Point(20, 185)
        wProdTypeLabel.Name = "wProdTypeLabel"
        wProdTypeLabel.Size = New Size(35, 15)
        wProdTypeLabel.TabIndex = 6
        wProdTypeLabel.Text = "Type:"
        ' 
        ' wProdPriceLabel
        ' 
        wProdPriceLabel.AutoSize = True
        wProdPriceLabel.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wProdPriceLabel.ForeColor = Color.Black
        wProdPriceLabel.Location = New Point(20, 225)
        wProdPriceLabel.Name = "wProdPriceLabel"
        wProdPriceLabel.Size = New Size(137, 19)
        wProdPriceLabel.TabIndex = 7
        wProdPriceLabel.Text = "WHOLESALE PRICE:"
        ' 
        ' wProdAvailableLabel
        ' 
        wProdAvailableLabel.AutoSize = True
        wProdAvailableLabel.Font = New Font("Segoe UI", 9F)
        wProdAvailableLabel.ForeColor = Color.Gray
        wProdAvailableLabel.Location = New Point(20, 260)
        wProdAvailableLabel.Name = "wProdAvailableLabel"
        wProdAvailableLabel.Size = New Size(58, 15)
        wProdAvailableLabel.TabIndex = 8
        wProdAvailableLabel.Text = "Available:"
        ' 
        ' wQtyLabel
        ' 
        wQtyLabel.AutoSize = True
        wQtyLabel.Font = New Font("Segoe UI", 10F)
        wQtyLabel.ForeColor = Color.Black
        wQtyLabel.Location = New Point(20, 525)
        wQtyLabel.Name = "wQtyLabel"
        wQtyLabel.Size = New Size(66, 19)
        wQtyLabel.TabIndex = 9
        wQtyLabel.Text = "Quantity:"
        ' 
        ' wQtyMinusButton
        ' 
        wQtyMinusButton.BackColor = Color.White
        wQtyMinusButton.Cursor = Cursors.Hand
        wQtyMinusButton.FlatAppearance.BorderColor = Color.LightGray
        wQtyMinusButton.FlatStyle = FlatStyle.Flat
        wQtyMinusButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wQtyMinusButton.ForeColor = Color.Black
        wQtyMinusButton.Location = New Point(90, 522)
        wQtyMinusButton.Name = "wQtyMinusButton"
        wQtyMinusButton.Size = New Size(30, 28)
        wQtyMinusButton.TabIndex = 10
        wQtyMinusButton.Text = "-"
        wQtyMinusButton.UseVisualStyleBackColor = False
        ' 
        ' wQtyTextBox
        ' 
        wQtyTextBox.Font = New Font("Segoe UI", 10F)
        wQtyTextBox.Location = New Point(126, 523)
        wQtyTextBox.Name = "wQtyTextBox"
        wQtyTextBox.Size = New Size(50, 25)
        wQtyTextBox.TabIndex = 11
        wQtyTextBox.Text = "1"
        wQtyTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' wQtyPlusButton
        ' 
        wQtyPlusButton.BackColor = Color.White
        wQtyPlusButton.Cursor = Cursors.Hand
        wQtyPlusButton.FlatAppearance.BorderColor = Color.LightGray
        wQtyPlusButton.FlatStyle = FlatStyle.Flat
        wQtyPlusButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wQtyPlusButton.ForeColor = Color.Black
        wQtyPlusButton.Location = New Point(182, 522)
        wQtyPlusButton.Name = "wQtyPlusButton"
        wQtyPlusButton.Size = New Size(30, 28)
        wQtyPlusButton.TabIndex = 12
        wQtyPlusButton.Text = "+"
        wQtyPlusButton.UseVisualStyleBackColor = False
        ' 
        ' wAddToCartButton
        ' 
        wAddToCartButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wAddToCartButton.Cursor = Cursors.Hand
        wAddToCartButton.FlatAppearance.BorderSize = 0
        wAddToCartButton.FlatStyle = FlatStyle.Flat
        wAddToCartButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wAddToCartButton.ForeColor = Color.White
        wAddToCartButton.Location = New Point(660, 515)
        wAddToCartButton.Name = "wAddToCartButton"
        wAddToCartButton.Size = New Size(140, 35)
        wAddToCartButton.TabIndex = 13
        wAddToCartButton.Text = "ADD TO CART"
        wAddToCartButton.UseVisualStyleBackColor = False
        ' 
        ' wholesaleCartCard
        ' 
        wholesaleCartCard.BackColor = Color.White
        wholesaleCartCard.Controls.Add(wholesaleCartTitle)
        wholesaleCartCard.Controls.Add(wholesaleCartPanel)
        wholesaleCartCard.Controls.Add(wholesaleSubtotalLabel)
        wholesaleCartCard.Controls.Add(wholesaleSubtotalValue)
        wholesaleCartCard.Controls.Add(wholesaleCheckoutButton)
        wholesaleCartCard.Location = New Point(840, 0)
        wholesaleCartCard.Name = "wholesaleCartCard"
        wholesaleCartCard.Size = New Size(804, 630)
        wholesaleCartCard.TabIndex = 2
        ' 
        ' wholesaleCartTitle
        ' 
        wholesaleCartTitle.AutoSize = True
        wholesaleCartTitle.Font = New Font("Segoe UI", 14F, FontStyle.Bold)
        wholesaleCartTitle.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wholesaleCartTitle.Location = New Point(20, 20)
        wholesaleCartTitle.Name = "wholesaleCartTitle"
        wholesaleCartTitle.Size = New Size(142, 25)
        wholesaleCartTitle.TabIndex = 0
        wholesaleCartTitle.Text = "Shopping Cart"
        ' 
        ' wholesaleCartPanel
        ' 
        wholesaleCartPanel.AutoScroll = True
        wholesaleCartPanel.BackColor = Color.White
        wholesaleCartPanel.Location = New Point(20, 60)
        wholesaleCartPanel.Name = "wholesaleCartPanel"
        wholesaleCartPanel.Size = New Size(764, 460)
        wholesaleCartPanel.TabIndex = 1
        ' 
        ' wholesaleSubtotalLabel
        ' 
        wholesaleSubtotalLabel.AutoSize = True
        wholesaleSubtotalLabel.Font = New Font("Segoe UI", 11F)
        wholesaleSubtotalLabel.ForeColor = Color.Gray
        wholesaleSubtotalLabel.Location = New Point(20, 545)
        wholesaleSubtotalLabel.Name = "wholesaleSubtotalLabel"
        wholesaleSubtotalLabel.Size = New Size(68, 20)
        wholesaleSubtotalLabel.TabIndex = 2
        wholesaleSubtotalLabel.Text = "Subtotal:"
        ' 
        ' wholesaleSubtotalValue
        ' 
        wholesaleSubtotalValue.AutoSize = True
        wholesaleSubtotalValue.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        wholesaleSubtotalValue.ForeColor = Color.Black
        wholesaleSubtotalValue.Location = New Point(660, 545)
        wholesaleSubtotalValue.Name = "wholesaleSubtotalValue"
        wholesaleSubtotalValue.Size = New Size(53, 20)
        wholesaleSubtotalValue.TabIndex = 3
        wholesaleSubtotalValue.Text = "P 0.00"
        ' 
        ' wholesaleCheckoutButton
        ' 
        wholesaleCheckoutButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wholesaleCheckoutButton.Cursor = Cursors.Hand
        wholesaleCheckoutButton.FlatAppearance.BorderSize = 0
        wholesaleCheckoutButton.FlatStyle = FlatStyle.Flat
        wholesaleCheckoutButton.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        wholesaleCheckoutButton.ForeColor = Color.White
        wholesaleCheckoutButton.Location = New Point(644, 580)
        wholesaleCheckoutButton.Name = "wholesaleCheckoutButton"
        wholesaleCheckoutButton.Size = New Size(140, 35)
        wholesaleCheckoutButton.TabIndex = 4
        wholesaleCheckoutButton.Text = "CHECKOUT"
        wholesaleCheckoutButton.UseVisualStyleBackColor = False
        ' 
        ' restockView
        ' 
        restockView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        restockView.Controls.Add(restockCard)
        restockView.Dock = DockStyle.Fill
        restockView.Location = New Point(20, 20)
        restockView.Name = "restockView"
        restockView.Size = New Size(1644, 901)
        restockView.TabIndex = 3
        restockView.Visible = False
        ' 
        ' restockCard
        ' 
        restockCard.BackColor = Color.White
        restockCard.Controls.Add(restockDesc)
        restockCard.Controls.Add(restockHead)
        restockCard.Dock = DockStyle.Top
        restockCard.Location = New Point(0, 0)
        restockCard.Name = "restockCard"
        restockCard.Padding = New Padding(20)
        restockCard.Size = New Size(1644, 160)
        restockCard.TabIndex = 0
        ' 
        ' restockDesc
        ' 
        restockDesc.Dock = DockStyle.Bottom
        restockDesc.Font = New Font("Segoe UI", 10F)
        restockDesc.ForeColor = Color.Gray
        restockDesc.Location = New Point(20, 100)
        restockDesc.Name = "restockDesc"
        restockDesc.Size = New Size(1604, 40)
        restockDesc.TabIndex = 1
        restockDesc.Text = "Restock low-inventory products from suppliers."
        ' 
        ' restockHead
        ' 
        restockHead.Dock = DockStyle.Top
        restockHead.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        restockHead.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        restockHead.Location = New Point(20, 20)
        restockHead.Name = "restockHead"
        restockHead.Size = New Size(1604, 50)
        restockHead.TabIndex = 0
        restockHead.Text = "Restock"
        ' 
        ' Staff
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1904, 1041)
        Controls.Add(mainLayout)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Staff"
        Text = "Staff"
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
        retailView.ResumeLayout(False)
        searchCard.ResumeLayout(False)
        searchCard.PerformLayout()
        productCard.ResumeLayout(False)
        productCard.PerformLayout()
        cartCard.ResumeLayout(False)
        cartCard.PerformLayout()
        checkoutView.ResumeLayout(False)
        orderReviewCard.ResumeLayout(False)
        orderReviewCard.PerformLayout()
        paymentCard.ResumeLayout(False)
        paymentCard.PerformLayout()
        wholesaleView.ResumeLayout(False)
        wholesaleSearchCard.ResumeLayout(False)
        wholesaleSearchCard.PerformLayout()
        wholesaleProductCard.ResumeLayout(False)
        wholesaleProductCard.PerformLayout()
        wholesaleCartCard.ResumeLayout(False)
        wholesaleCartCard.PerformLayout()
        restockView.ResumeLayout(False)
        restockCard.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainLayout As TableLayoutPanel
    Friend WithEvents sidebar As Panel
    Friend WithEvents logoBox As PictureBox
    Friend WithEvents PharName As Label
    Friend WithEvents navRetail As Button
    Friend WithEvents navWholesale As Button
    Friend WithEvents navRestock As Button
    Friend WithEvents logoutstaff_button As Button
    Friend WithEvents rightLayout As TableLayoutPanel
    Friend WithEvents headerPanel As Panel
    Friend WithEvents titleLabel As Label
    Friend WithEvents subtitleLabel As Label
    Friend WithEvents userPanel As Panel
    Friend WithEvents userAvatar As PictureBox
    Friend WithEvents welcomestaff_label As Label
    Friend WithEvents userRoleLabel As Label
    Friend WithEvents bodyPanel As Panel
    Friend WithEvents retailView As Panel
    Friend WithEvents searchCard As Panel
    Friend WithEvents searchLabel As Label
    Friend WithEvents searchTextBox As TextBox
    Friend WithEvents searchButton As Button
    Friend WithEvents productCard As Panel
    Friend WithEvents prodNameLabel As Label
    Friend WithEvents prodIdLabel As Label
    Friend WithEvents prodGenericLabel As Label
    Friend WithEvents prodBrandLabel As Label
    Friend WithEvents prodFormLabel As Label
    Friend WithEvents prodStrengthLabel As Label
    Friend WithEvents prodTypeLabel As Label
    Friend WithEvents prodPriceLabel As Label
    Friend WithEvents prodAvailableLabel As Label
    Friend WithEvents prodExpiryLabel As Label
    Friend WithEvents prodBatchLabel As Label
    Friend WithEvents rxSectionLabel As Label
    Friend WithEvents rxNoLabel As Label
    Friend WithEvents rxPractitionerLabel As Label
    Friend WithEvents rxLicenseLabel As Label
    Friend WithEvents rxDateLabel As Label
    Friend WithEvents rxNoTextBox As TextBox
    Friend WithEvents rxPractitionerTextBox As TextBox
    Friend WithEvents rxLicenseTextBox As TextBox
    Friend WithEvents rxDateTextBox As TextBox
    Friend WithEvents qtyLabel As Label
    Friend WithEvents qtyMinusButton As Button
    Friend WithEvents qtyTextBox As TextBox
    Friend WithEvents qtyPlusButton As Button
    Friend WithEvents addToCartButton As Button
    Friend WithEvents cartCard As Panel
    Friend WithEvents cartTitleLabel As Label
    Friend WithEvents cartItemsPanel As Panel
    Friend WithEvents cartSubtotalLabel As Label
    Friend WithEvents cartSubtotalValue As Label
    Friend WithEvents checkoutButton As Button
    Friend WithEvents checkoutView As Panel
    Friend WithEvents orderReviewCard As Panel
    Friend WithEvents orderReviewTitle As Label
    Friend WithEvents customerTypeLabel As Label
    Friend WithEvents customerTypeCombo As ComboBox
    Friend WithEvents customerNameLabel As Label
    Friend WithEvents customerNameTextBox As TextBox
    Friend WithEvents idNumberLabel As Label
    Friend WithEvents idNumberTextBox As TextBox
    Friend WithEvents orderSummaryLabel As Label
    Friend WithEvents orderSummaryPanel As Panel
    Friend WithEvents prescriptionLabel As Label
    Friend WithEvents prescriptionValueLabel As Label
    Friend WithEvents rxRecordedLabel As Label
    Friend WithEvents paymentCard As Panel
    Friend WithEvents paymentTitle As Label
    Friend WithEvents subtotalLabel As Label
    Friend WithEvents subtotalValue As Label
    Friend WithEvents discountLabel As Label
    Friend WithEvents discountValue As Label
    Friend WithEvents totalDueLabel As Label
    Friend WithEvents totalDueValue As Label
    Friend WithEvents cashLabel As Label
    Friend WithEvents cashValue As Label
    Friend WithEvents changeLabel As Label
    Friend WithEvents changeValue As Label
    Friend WithEvents cashTextBox As TextBox
    Friend WithEvents cancelButton As Button
    Friend WithEvents confirmButton As Button
    Friend WithEvents wholesaleView As Panel
    Friend WithEvents wholesaleSearchCard As Panel
    Friend WithEvents wholesaleSearchLabel As Label
    Friend WithEvents wholesaleSearchTextBox As TextBox
    Friend WithEvents wholesaleSearchButton As Button
    Friend WithEvents wholesaleProductCard As Panel
    Friend WithEvents wProdNameLabel As Label
    Friend WithEvents wProdIdLabel As Label
    Friend WithEvents wProdGenericLabel As Label
    Friend WithEvents wProdBrandLabel As Label
    Friend WithEvents wProdFormLabel As Label
    Friend WithEvents wProdStrengthLabel As Label
    Friend WithEvents wProdTypeLabel As Label
    Friend WithEvents wProdPriceLabel As Label
    Friend WithEvents wProdAvailableLabel As Label
    Friend WithEvents wQtyLabel As Label
    Friend WithEvents wQtyMinusButton As Button
    Friend WithEvents wQtyTextBox As TextBox
    Friend WithEvents wQtyPlusButton As Button
    Friend WithEvents wAddToCartButton As Button
    Friend WithEvents wholesaleCartCard As Panel
    Friend WithEvents wholesaleCartTitle As Label
    Friend WithEvents wholesaleCartPanel As Panel
    Friend WithEvents wholesaleSubtotalLabel As Label
    Friend WithEvents wholesaleSubtotalValue As Label
    Friend WithEvents wholesaleCheckoutButton As Button
    Friend WithEvents restockView As Panel
    Friend WithEvents restockCard As Panel
    Friend WithEvents restockDesc As Label
    Friend WithEvents restockHead As Label
End Class
