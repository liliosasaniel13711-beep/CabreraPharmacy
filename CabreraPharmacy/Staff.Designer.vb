<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Staff
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        mainLayout = New TableLayoutPanel()
        sidebar = New Panel()
        logoBox = New PictureBox()
        PharName = New Label()
        navTransaction = New Button()
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
        transactionView = New Panel()
        transactionLayout = New TableLayoutPanel()
        transactionLeftPanel = New Panel()
        transactionLeftLayout = New TableLayoutPanel()
        modePanel = New Panel()
        retailModeButton = New Button()
        wholesaleModeButton = New Button()
        parentFilterPanel = New Panel()
        allParentFilterButton = New Button()
        medicineFilterButton = New Button()
        supplementsFilterButton = New Button()
        medicalSupplyFilterButton = New Button()
        searchPanel = New Panel()
        searchLabel = New Label()
        searchFilterCombo = New ComboBox()
        searchTextBox = New TextBox()
        resultsPanel = New Panel()
        resultsGrid = New DataGridView()
        quantityPanel = New Panel()
        qtyLabel = New Label()
        qtyMinusButton = New Button()
        qtyTextBox = New TextBox()
        qtyPlusButton = New Button()
        addToCartButton = New Button()
        transactionDetailsPanel = New Panel()
        wholesaleDetailsPanel = New Panel()
        wholesaleDetailsTitle = New Label()
        customerSearchButton = New Button()
        customerLicenseLabel = New Label()
        customerLicenseTextBox = New TextBox()
        contactPersonLabel = New Label()
        contactPersonTextBox = New TextBox()
        contactNoLabel = New Label()
        contactNoTextBox = New TextBox()
        addressLabel = New Label()
        addressTextBox = New TextBox()
        poNoLabel = New Label()
        poNoTextBox = New TextBox()
        poDateLabel = New Label()
        poDatePicker = New DateTimePicker()
        deliveryDateLabel = New Label()
        deliveryDatePicker = New DateTimePicker()
        prescriptionPanel = New Panel()
        prescriptionTitle = New Label()
        rxNoLabel = New Label()
        rxNoTextBox = New TextBox()
        rxPractitionerLabel = New Label()
        rxPractitionerTextBox = New TextBox()
        rxLicenseLabel = New Label()
        rxLicenseTextBox = New TextBox()
        rxDateLabel = New Label()
        rxDateTextBox = New DateTimePicker()
        cartCard = New Panel()
        cartTitleLabel = New Label()
        cartItemsPanel = New FlowLayoutPanel()
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
        orderSummaryPanel = New FlowLayoutPanel()
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
        backToPosButton = New Button()
        cancelButton = New Button()
        confirmButton = New Button()
        restockView = New Panel()
        restockCard = New Panel()
        restockHead = New Label()
        restockDesc = New Label()
        searchButton = New Button()
        mainLayout.SuspendLayout()
        sidebar.SuspendLayout()
        CType(logoBox, ComponentModel.ISupportInitialize).BeginInit()
        rightLayout.SuspendLayout()
        headerPanel.SuspendLayout()
        userPanel.SuspendLayout()
        CType(userAvatar, ComponentModel.ISupportInitialize).BeginInit()
        bodyPanel.SuspendLayout()
        transactionView.SuspendLayout()
        transactionLayout.SuspendLayout()
        transactionLeftPanel.SuspendLayout()
        transactionLeftLayout.SuspendLayout()
        modePanel.SuspendLayout()
        parentFilterPanel.SuspendLayout()
        searchPanel.SuspendLayout()
        resultsPanel.SuspendLayout()
        CType(resultsGrid, ComponentModel.ISupportInitialize).BeginInit()
        quantityPanel.SuspendLayout()
        transactionDetailsPanel.SuspendLayout()
        wholesaleDetailsPanel.SuspendLayout()
        prescriptionPanel.SuspendLayout()
        cartCard.SuspendLayout()
        checkoutView.SuspendLayout()
        orderReviewCard.SuspendLayout()
        paymentCard.SuspendLayout()
        restockView.SuspendLayout()
        restockCard.SuspendLayout()
        SuspendLayout()
        ' 
        ' mainLayout
        ' 
        mainLayout.ColumnCount = 2
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 251F))
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        mainLayout.Controls.Add(sidebar, 0, 0)
        mainLayout.Controls.Add(rightLayout, 1, 0)
        mainLayout.Dock = DockStyle.Fill
        mainLayout.Location = New Point(0, 0)
        mainLayout.Margin = New Padding(3, 4, 3, 4)
        mainLayout.Name = "mainLayout"
        mainLayout.RowCount = 1
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        mainLayout.Size = New Size(2176, 1325)
        mainLayout.TabIndex = 0
        ' 
        ' sidebar
        ' 
        sidebar.BackColor = Color.White
        sidebar.Controls.Add(logoBox)
        sidebar.Controls.Add(PharName)
        sidebar.Controls.Add(navTransaction)
        sidebar.Controls.Add(navRestock)
        sidebar.Controls.Add(logoutstaff_button)
        sidebar.Dock = DockStyle.Fill
        sidebar.Location = New Point(3, 4)
        sidebar.Margin = New Padding(3, 4, 3, 4)
        sidebar.Name = "sidebar"
        sidebar.Size = New Size(245, 1317)
        sidebar.TabIndex = 0
        ' 
        ' logoBox
        ' 
        logoBox.BackColor = Color.FromArgb(CByte(255), CByte(215), CByte(0))
        logoBox.Image = My.Resources.Resources._769082459_1379172074148575_7892593615181140804_n
        logoBox.Location = New Point(23, 27)
        logoBox.Margin = New Padding(3, 4, 3, 4)
        logoBox.Name = "logoBox"
        logoBox.Size = New Size(57, 67)
        logoBox.SizeMode = PictureBoxSizeMode.Zoom
        logoBox.TabIndex = 0
        logoBox.TabStop = False
        ' 
        ' PharName
        ' 
        PharName.AutoSize = True
        PharName.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        PharName.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        PharName.Location = New Point(91, 27)
        PharName.MaximumSize = New Size(149, 0)
        PharName.Name = "PharName"
        PharName.Size = New Size(146, 69)
        PharName.TabIndex = 1
        PharName.Text = "Cabrera's" & vbCrLf & "Drugstore and Medical Supplies"
        ' 
        ' navTransaction
        ' 
        navTransaction.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navTransaction.Cursor = Cursors.Hand
        navTransaction.FlatAppearance.BorderSize = 0
        navTransaction.FlatStyle = FlatStyle.Flat
        navTransaction.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        navTransaction.ForeColor = Color.White
        navTransaction.Location = New Point(23, 160)
        navTransaction.Margin = New Padding(3, 4, 3, 4)
        navTransaction.Name = "navTransaction"
        navTransaction.Padding = New Padding(11, 0, 0, 0)
        navTransaction.Size = New Size(206, 56)
        navTransaction.TabIndex = 2
        navTransaction.Tag = "Transaction"
        navTransaction.Text = "Transaction"
        navTransaction.TextAlign = ContentAlignment.MiddleLeft
        navTransaction.UseVisualStyleBackColor = False
        ' 
        ' navRestock
        ' 
        navRestock.BackColor = Color.White
        navRestock.Cursor = Cursors.Hand
        navRestock.FlatAppearance.BorderSize = 0
        navRestock.FlatStyle = FlatStyle.Flat
        navRestock.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        navRestock.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        navRestock.Location = New Point(23, 233)
        navRestock.Margin = New Padding(3, 4, 3, 4)
        navRestock.Name = "navRestock"
        navRestock.Padding = New Padding(11, 0, 0, 0)
        navRestock.Size = New Size(206, 56)
        navRestock.TabIndex = 3
        navRestock.Tag = "Restock"
        navRestock.Text = "Restock"
        navRestock.TextAlign = ContentAlignment.MiddleLeft
        navRestock.UseVisualStyleBackColor = False
        ' 
        ' logoutstaff_button
        ' 
        logoutstaff_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutstaff_button.BackColor = Color.White
        logoutstaff_button.FlatStyle = FlatStyle.Flat
        logoutstaff_button.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        logoutstaff_button.Location = New Point(23, 2465)
        logoutstaff_button.Margin = New Padding(3, 4, 3, 4)
        logoutstaff_button.Name = "logoutstaff_button"
        logoutstaff_button.Size = New Size(206, 60)
        logoutstaff_button.TabIndex = 4
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
        rightLayout.Location = New Point(254, 4)
        rightLayout.Margin = New Padding(3, 4, 3, 4)
        rightLayout.Name = "rightLayout"
        rightLayout.RowCount = 2
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 116F))
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        rightLayout.Size = New Size(1919, 1317)
        rightLayout.TabIndex = 1
        ' 
        ' headerPanel
        ' 
        headerPanel.BackColor = Color.White
        headerPanel.Controls.Add(titleLabel)
        headerPanel.Controls.Add(subtitleLabel)
        headerPanel.Controls.Add(userPanel)
        headerPanel.Dock = DockStyle.Fill
        headerPanel.Location = New Point(3, 4)
        headerPanel.Margin = New Padding(3, 4, 3, 4)
        headerPanel.Name = "headerPanel"
        headerPanel.Size = New Size(1913, 108)
        headerPanel.TabIndex = 0
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 24F, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        titleLabel.Location = New Point(23, 27)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(239, 54)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Transaction"
        ' 
        ' subtitleLabel
        ' 
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 11F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(23, 65)
        subtitleLabel.Name = "subtitleLabel"
        subtitleLabel.Size = New Size(365, 25)
        subtitleLabel.TabIndex = 1
        subtitleLabel.Text = "Cabrera's Drugstore and Medical Supplies"
        ' 
        ' userPanel
        ' 
        userPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        userPanel.Controls.Add(userAvatar)
        userPanel.Controls.Add(welcomestaff_label)
        userPanel.Controls.Add(userRoleLabel)
        userPanel.Location = New Point(1685, 0)
        userPanel.Margin = New Padding(3, 4, 3, 4)
        userPanel.Name = "userPanel"
        userPanel.Size = New Size(330, 56)
        userPanel.TabIndex = 2
        ' 
        ' userAvatar
        ' 
        userAvatar.BackColor = Color.FromArgb(CByte(224), CByte(247), CByte(236))
        userAvatar.Location = New Point(0, 5)
        userAvatar.Margin = New Padding(3, 4, 3, 4)
        userAvatar.Name = "userAvatar"
        userAvatar.Size = New Size(46, 46)
        userAvatar.TabIndex = 0
        userAvatar.TabStop = False
        ' 
        ' welcomestaff_label
        ' 
        welcomestaff_label.AutoSize = True
        welcomestaff_label.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        welcomestaff_label.Location = New Point(60, 6)
        welcomestaff_label.Name = "welcomestaff_label"
        welcomestaff_label.Size = New Size(134, 23)
        welcomestaff_label.TabIndex = 1
        welcomestaff_label.Text = "Welcome, Staff"
        ' 
        ' userRoleLabel
        ' 
        userRoleLabel.AutoSize = True
        userRoleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        userRoleLabel.Location = New Point(60, 29)
        userRoleLabel.Name = "userRoleLabel"
        userRoleLabel.Size = New Size(143, 20)
        userRoleLabel.TabIndex = 2
        userRoleLabel.Text = "Assistant Pharmacist"
        ' 
        ' bodyPanel
        ' 
        bodyPanel.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        bodyPanel.Controls.Add(transactionView)
        bodyPanel.Controls.Add(checkoutView)
        bodyPanel.Controls.Add(restockView)
        bodyPanel.Dock = DockStyle.Fill
        bodyPanel.Location = New Point(3, 120)
        bodyPanel.Margin = New Padding(3, 4, 3, 4)
        bodyPanel.Name = "bodyPanel"
        bodyPanel.Padding = New Padding(23, 27, 23, 27)
        bodyPanel.Size = New Size(1913, 1193)
        bodyPanel.TabIndex = 1
        ' 
        ' transactionView
        ' 
        transactionView.Controls.Add(transactionLayout)
        transactionView.Dock = DockStyle.Fill
        transactionView.Location = New Point(23, 27)
        transactionView.Margin = New Padding(3, 4, 3, 4)
        transactionView.Name = "transactionView"
        transactionView.Size = New Size(1867, 1122)
        transactionView.TabIndex = 0
        ' 
        ' transactionLayout
        ' 
        transactionLayout.ColumnCount = 2
        transactionLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70F))
        transactionLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30F))
        transactionLayout.Controls.Add(transactionLeftPanel, 0, 0)
        transactionLayout.Controls.Add(cartCard, 1, 0)
        transactionLayout.Dock = DockStyle.Fill
        transactionLayout.Location = New Point(0, 0)
        transactionLayout.Margin = New Padding(3, 4, 3, 4)
        transactionLayout.Name = "transactionLayout"
        transactionLayout.RowCount = 1
        transactionLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        transactionLayout.Size = New Size(1867, 1122)
        transactionLayout.TabIndex = 0
        ' 
        ' transactionLeftPanel
        ' 
        transactionLeftPanel.AutoScroll = True
        transactionLeftPanel.Controls.Add(transactionLeftLayout)
        transactionLeftPanel.Dock = DockStyle.Fill
        transactionLeftPanel.Location = New Point(3, 4)
        transactionLeftPanel.Margin = New Padding(3, 4, 3, 4)
        transactionLeftPanel.Name = "transactionLeftPanel"
        transactionLeftPanel.Padding = New Padding(0, 0, 11, 0)
        transactionLeftPanel.Size = New Size(1300, 1114)
        transactionLeftPanel.TabIndex = 0
        ' 
        ' transactionLeftLayout
        ' 
        transactionLeftLayout.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        transactionLeftLayout.ColumnCount = 1
        transactionLeftLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        transactionLeftLayout.Controls.Add(modePanel, 0, 0)
        transactionLeftLayout.Controls.Add(parentFilterPanel, 0, 1)
        transactionLeftLayout.Controls.Add(searchPanel, 0, 2)
        transactionLeftLayout.Controls.Add(resultsPanel, 0, 3)
        transactionLeftLayout.Controls.Add(quantityPanel, 0, 4)
        transactionLeftLayout.Controls.Add(transactionDetailsPanel, 0, 5)
        transactionLeftLayout.Location = New Point(0, 0)
        transactionLeftLayout.Margin = New Padding(0)
        transactionLeftLayout.Name = "transactionLeftLayout"
        transactionLeftLayout.RowCount = 6
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 75F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 75F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 64F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 507F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 101F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 213F))
        transactionLeftLayout.Size = New Size(2100, 1083)
        transactionLeftLayout.TabIndex = 0
        ' 
        ' modePanel
        ' 
        modePanel.BackColor = Color.White
        modePanel.Controls.Add(retailModeButton)
        modePanel.Controls.Add(wholesaleModeButton)
        modePanel.Dock = DockStyle.Fill
        modePanel.Location = New Point(3, 4)
        modePanel.Margin = New Padding(3, 4, 3, 4)
        modePanel.Name = "modePanel"
        modePanel.Size = New Size(2094, 67)
        modePanel.TabIndex = 0
        ' 
        ' retailModeButton
        ' 
        retailModeButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        retailModeButton.Cursor = Cursors.Hand
        retailModeButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        retailModeButton.FlatStyle = FlatStyle.Flat
        retailModeButton.ForeColor = Color.White
        retailModeButton.Location = New Point(11, 9)
        retailModeButton.Margin = New Padding(3, 4, 3, 4)
        retailModeButton.Name = "retailModeButton"
        retailModeButton.Size = New Size(137, 43)
        retailModeButton.TabIndex = 0
        retailModeButton.Tag = "Retail"
        retailModeButton.Text = "Retail"
        retailModeButton.UseVisualStyleBackColor = False
        ' 
        ' wholesaleModeButton
        ' 
        wholesaleModeButton.BackColor = Color.White
        wholesaleModeButton.Cursor = Cursors.Hand
        wholesaleModeButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wholesaleModeButton.FlatStyle = FlatStyle.Flat
        wholesaleModeButton.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        wholesaleModeButton.Location = New Point(151, 9)
        wholesaleModeButton.Margin = New Padding(3, 4, 3, 4)
        wholesaleModeButton.Name = "wholesaleModeButton"
        wholesaleModeButton.Size = New Size(137, 43)
        wholesaleModeButton.TabIndex = 1
        wholesaleModeButton.Tag = "Wholesale"
        wholesaleModeButton.Text = "Wholesale"
        wholesaleModeButton.UseVisualStyleBackColor = False
        ' 
        ' parentFilterPanel
        ' 
        parentFilterPanel.BackColor = Color.White
        parentFilterPanel.Controls.Add(allParentFilterButton)
        parentFilterPanel.Controls.Add(medicineFilterButton)
        parentFilterPanel.Controls.Add(supplementsFilterButton)
        parentFilterPanel.Controls.Add(medicalSupplyFilterButton)
        parentFilterPanel.Dock = DockStyle.Fill
        parentFilterPanel.Location = New Point(3, 79)
        parentFilterPanel.Margin = New Padding(3, 4, 3, 4)
        parentFilterPanel.Name = "parentFilterPanel"
        parentFilterPanel.Size = New Size(2094, 67)
        parentFilterPanel.TabIndex = 1
        ' 
        ' allParentFilterButton
        ' 
        allParentFilterButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        allParentFilterButton.Cursor = Cursors.Hand
        allParentFilterButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        allParentFilterButton.FlatStyle = FlatStyle.Flat
        allParentFilterButton.ForeColor = Color.White
        allParentFilterButton.Location = New Point(11, 9)
        allParentFilterButton.Margin = New Padding(3, 4, 3, 4)
        allParentFilterButton.Name = "allParentFilterButton"
        allParentFilterButton.Size = New Size(137, 40)
        allParentFilterButton.TabIndex = 0
        allParentFilterButton.Tag = "All"
        allParentFilterButton.Text = "All"
        allParentFilterButton.UseVisualStyleBackColor = False
        ' 
        ' medicineFilterButton
        ' 
        medicineFilterButton.BackColor = Color.White
        medicineFilterButton.Cursor = Cursors.Hand
        medicineFilterButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        medicineFilterButton.FlatStyle = FlatStyle.Flat
        medicineFilterButton.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        medicineFilterButton.Location = New Point(158, 9)
        medicineFilterButton.Margin = New Padding(3, 4, 3, 4)
        medicineFilterButton.Name = "medicineFilterButton"
        medicineFilterButton.Size = New Size(137, 40)
        medicineFilterButton.TabIndex = 1
        medicineFilterButton.Tag = "Medicine"
        medicineFilterButton.Text = "Medicine"
        medicineFilterButton.UseVisualStyleBackColor = False
        ' 
        ' supplementsFilterButton
        ' 
        supplementsFilterButton.BackColor = Color.White
        supplementsFilterButton.Cursor = Cursors.Hand
        supplementsFilterButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        supplementsFilterButton.FlatStyle = FlatStyle.Flat
        supplementsFilterButton.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        supplementsFilterButton.Location = New Point(304, 9)
        supplementsFilterButton.Margin = New Padding(3, 4, 3, 4)
        supplementsFilterButton.Name = "supplementsFilterButton"
        supplementsFilterButton.Size = New Size(137, 40)
        supplementsFilterButton.TabIndex = 2
        supplementsFilterButton.Tag = "Supplements"
        supplementsFilterButton.Text = "Supplements"
        supplementsFilterButton.UseVisualStyleBackColor = False
        ' 
        ' medicalSupplyFilterButton
        ' 
        medicalSupplyFilterButton.BackColor = Color.White
        medicalSupplyFilterButton.Cursor = Cursors.Hand
        medicalSupplyFilterButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        medicalSupplyFilterButton.FlatStyle = FlatStyle.Flat
        medicalSupplyFilterButton.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        medicalSupplyFilterButton.Location = New Point(450, 9)
        medicalSupplyFilterButton.Margin = New Padding(3, 4, 3, 4)
        medicalSupplyFilterButton.Name = "medicalSupplyFilterButton"
        medicalSupplyFilterButton.Size = New Size(137, 40)
        medicalSupplyFilterButton.TabIndex = 3
        medicalSupplyFilterButton.Tag = "Medical Supply"
        medicalSupplyFilterButton.Text = "Medical Supply"
        medicalSupplyFilterButton.UseVisualStyleBackColor = False
        ' 
        ' searchPanel
        ' 
        searchPanel.BackColor = Color.White
        searchPanel.Controls.Add(searchLabel)
        searchPanel.Controls.Add(searchFilterCombo)
        searchPanel.Controls.Add(searchTextBox)
        searchPanel.Dock = DockStyle.Fill
        searchPanel.Location = New Point(3, 154)
        searchPanel.Margin = New Padding(3, 4, 3, 4)
        searchPanel.Name = "searchPanel"
        searchPanel.Size = New Size(2094, 56)
        searchPanel.TabIndex = 2
        ' 
        ' searchLabel
        ' 
        searchLabel.AutoSize = True
        searchLabel.Location = New Point(14, 20)
        searchLabel.Name = "searchLabel"
        searchLabel.Size = New Size(72, 20)
        searchLabel.TabIndex = 0
        searchLabel.Text = "Category:"
        ' 
        ' searchFilterCombo
        ' 
        searchFilterCombo.BackColor = Color.White
        searchFilterCombo.DrawMode = DrawMode.OwnerDrawFixed
        searchFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList
        searchFilterCombo.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        searchFilterCombo.Items.AddRange(New Object() {"All Categories"})
        searchFilterCombo.Location = New Point(103, 15)
        searchFilterCombo.Margin = New Padding(3, 4, 3, 4)
        searchFilterCombo.Name = "searchFilterCombo"
        searchFilterCombo.Size = New Size(205, 28)
        searchFilterCombo.TabIndex = 1
        ' 
        ' searchTextBox
        ' 
        searchTextBox.Location = New Point(326, 15)
        searchTextBox.Margin = New Padding(3, 4, 3, 4)
        searchTextBox.Name = "searchTextBox"
        searchTextBox.PlaceholderText = "Search by product name or brand..."
        searchTextBox.Size = New Size(411, 27)
        searchTextBox.TabIndex = 2
        ' 
        ' resultsPanel
        ' 
        resultsPanel.BackColor = Color.White
        resultsPanel.Controls.Add(resultsGrid)
        resultsPanel.Dock = DockStyle.Fill
        resultsPanel.Location = New Point(3, 218)
        resultsPanel.Margin = New Padding(3, 4, 3, 4)
        resultsPanel.Name = "resultsPanel"
        resultsPanel.Padding = New Padding(9, 11, 9, 11)
        resultsPanel.Size = New Size(2094, 499)
        resultsPanel.TabIndex = 3
        ' 
        ' resultsGrid
        ' 
        resultsGrid.AllowUserToAddRows = False
        resultsGrid.AllowUserToDeleteRows = False
        resultsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        resultsGrid.BackgroundColor = Color.White
        resultsGrid.BorderStyle = BorderStyle.None
        resultsGrid.ColumnHeadersHeight = 34
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Window
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle1.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(CByte(218), CByte(240), CByte(229))
        DataGridViewCellStyle1.SelectionForeColor = Color.Black
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.False
        resultsGrid.DefaultCellStyle = DataGridViewCellStyle1
        resultsGrid.Dock = DockStyle.Fill
        resultsGrid.Location = New Point(9, 11)
        resultsGrid.Margin = New Padding(0)
        resultsGrid.MultiSelect = False
        resultsGrid.Name = "resultsGrid"
        resultsGrid.ReadOnly = True
        resultsGrid.RowHeadersVisible = False
        resultsGrid.RowHeadersWidth = 51
        resultsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        resultsGrid.Size = New Size(2076, 477)
        resultsGrid.TabIndex = 0
        ' 
        ' quantityPanel
        ' 
        quantityPanel.BackColor = Color.White
        quantityPanel.Controls.Add(qtyLabel)
        quantityPanel.Controls.Add(qtyMinusButton)
        quantityPanel.Controls.Add(qtyTextBox)
        quantityPanel.Controls.Add(qtyPlusButton)
        quantityPanel.Controls.Add(addToCartButton)
        quantityPanel.Dock = DockStyle.Fill
        quantityPanel.Location = New Point(3, 725)
        quantityPanel.Margin = New Padding(3, 4, 3, 4)
        quantityPanel.Name = "quantityPanel"
        quantityPanel.Size = New Size(2094, 93)
        quantityPanel.TabIndex = 4
        ' 
        ' qtyLabel
        ' 
        qtyLabel.AutoSize = True
        qtyLabel.Location = New Point(14, 20)
        qtyLabel.Name = "qtyLabel"
        qtyLabel.Size = New Size(68, 20)
        qtyLabel.TabIndex = 0
        qtyLabel.Text = "Quantity:"
        ' 
        ' qtyMinusButton
        ' 
        qtyMinusButton.BackColor = Color.White
        qtyMinusButton.Cursor = Cursors.Hand
        qtyMinusButton.FlatAppearance.BorderColor = Color.LightGray
        qtyMinusButton.FlatStyle = FlatStyle.Flat
        qtyMinusButton.Location = New Point(103, 12)
        qtyMinusButton.Margin = New Padding(3, 4, 3, 4)
        qtyMinusButton.Name = "qtyMinusButton"
        qtyMinusButton.Size = New Size(34, 37)
        qtyMinusButton.TabIndex = 1
        qtyMinusButton.Text = "-"
        qtyMinusButton.UseVisualStyleBackColor = False
        ' 
        ' qtyTextBox
        ' 
        qtyTextBox.Location = New Point(144, 15)
        qtyTextBox.Margin = New Padding(3, 4, 3, 4)
        qtyTextBox.Name = "qtyTextBox"
        qtyTextBox.Size = New Size(57, 27)
        qtyTextBox.TabIndex = 2
        qtyTextBox.Text = "0"
        qtyTextBox.TextAlign = HorizontalAlignment.Center
        ' 
        ' qtyPlusButton
        ' 
        qtyPlusButton.BackColor = Color.White
        qtyPlusButton.Cursor = Cursors.Hand
        qtyPlusButton.FlatAppearance.BorderColor = Color.LightGray
        qtyPlusButton.FlatStyle = FlatStyle.Flat
        qtyPlusButton.Location = New Point(208, 12)
        qtyPlusButton.Margin = New Padding(3, 4, 3, 4)
        qtyPlusButton.Name = "qtyPlusButton"
        qtyPlusButton.Size = New Size(34, 37)
        qtyPlusButton.TabIndex = 3
        qtyPlusButton.Text = "+"
        qtyPlusButton.UseVisualStyleBackColor = False
        ' 
        ' addToCartButton
        ' 
        addToCartButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        addToCartButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        addToCartButton.Cursor = Cursors.Hand
        addToCartButton.FlatAppearance.BorderSize = 0
        addToCartButton.FlatStyle = FlatStyle.Flat
        addToCartButton.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        addToCartButton.ForeColor = Color.White
        addToCartButton.Location = New Point(2694, 9)
        addToCartButton.Margin = New Padding(3, 4, 3, 4)
        addToCartButton.Name = "addToCartButton"
        addToCartButton.Size = New Size(171, 45)
        addToCartButton.TabIndex = 4
        addToCartButton.Text = "ADD TO CART"
        addToCartButton.UseVisualStyleBackColor = False
        ' 
        ' transactionDetailsPanel
        ' 
        transactionDetailsPanel.Controls.Add(wholesaleDetailsPanel)
        transactionDetailsPanel.Controls.Add(prescriptionPanel)
        transactionDetailsPanel.Dock = DockStyle.Fill
        transactionDetailsPanel.Location = New Point(3, 826)
        transactionDetailsPanel.Margin = New Padding(3, 4, 3, 4)
        transactionDetailsPanel.Name = "transactionDetailsPanel"
        transactionDetailsPanel.Size = New Size(2094, 253)
        transactionDetailsPanel.TabIndex = 5
        ' 
        ' wholesaleDetailsPanel
        ' 
        wholesaleDetailsPanel.BackColor = Color.White
        wholesaleDetailsPanel.Controls.Add(wholesaleDetailsTitle)
        wholesaleDetailsPanel.Controls.Add(customerLicenseLabel)
        wholesaleDetailsPanel.Controls.Add(customerLicenseTextBox)
        wholesaleDetailsPanel.Controls.Add(contactPersonLabel)
        wholesaleDetailsPanel.Controls.Add(contactPersonTextBox)
        wholesaleDetailsPanel.Controls.Add(contactNoLabel)
        wholesaleDetailsPanel.Controls.Add(contactNoTextBox)
        wholesaleDetailsPanel.Controls.Add(addressLabel)
        wholesaleDetailsPanel.Controls.Add(addressTextBox)
        wholesaleDetailsPanel.Controls.Add(poNoLabel)
        wholesaleDetailsPanel.Controls.Add(poNoTextBox)
        wholesaleDetailsPanel.Controls.Add(poDateLabel)
        wholesaleDetailsPanel.Controls.Add(poDatePicker)
        wholesaleDetailsPanel.Controls.Add(deliveryDateLabel)
        wholesaleDetailsPanel.Controls.Add(deliveryDatePicker)
        wholesaleDetailsPanel.Controls.Add(customerSearchButton)
        wholesaleDetailsPanel.Dock = DockStyle.Fill
        wholesaleDetailsPanel.Location = New Point(0, 0)
        wholesaleDetailsPanel.Margin = New Padding(0)
        wholesaleDetailsPanel.Name = "wholesaleDetailsPanel"
        wholesaleDetailsPanel.Size = New Size(2094, 253)
        wholesaleDetailsPanel.TabIndex = 0
        wholesaleDetailsPanel.Visible = False
        ' 
        ' wholesaleDetailsTitle
        ' 
        wholesaleDetailsTitle.AutoSize = True
        wholesaleDetailsTitle.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        wholesaleDetailsTitle.ForeColor = Color.Black
        wholesaleDetailsTitle.Location = New Point(14, 16)
        wholesaleDetailsTitle.Name = "wholesaleDetailsTitle"
        wholesaleDetailsTitle.Size = New Size(374, 25)
        wholesaleDetailsTitle.TabIndex = 0
        wholesaleDetailsTitle.Text = "Wholesale Customer and Purchase Order"
        ' 
        ' customerSearchButton
        ' 
        customerSearchButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        customerSearchButton.Cursor = Cursors.Hand
        customerSearchButton.FlatAppearance.BorderSize = 0
        customerSearchButton.FlatStyle = FlatStyle.Flat
        customerSearchButton.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        customerSearchButton.ForeColor = Color.White
        customerSearchButton.Location = New Point(840, 11)
        customerSearchButton.Margin = New Padding(3, 4, 3, 4)
        customerSearchButton.Name = "customerSearchButton"
        customerSearchButton.Size = New Size(160, 40)
        customerSearchButton.TabIndex = 1
        customerSearchButton.Text = "Find Customer"
        customerSearchButton.UseVisualStyleBackColor = False
        ' 
        ' customerLicenseLabel
        ' 
        customerLicenseLabel.AutoSize = True
        customerLicenseLabel.Font = New Font("Segoe UI", 9F)
        customerLicenseLabel.ForeColor = Color.Gray
        customerLicenseLabel.Location = New Point(14, 64)
        customerLicenseLabel.Name = "customerLicenseLabel"
        customerLicenseLabel.Size = New Size(84, 20)
        customerLicenseLabel.TabIndex = 2
        customerLicenseLabel.Text = "License No."
        ' 
        ' customerLicenseTextBox
        ' 
        customerLicenseTextBox.Location = New Point(14, 91)
        customerLicenseTextBox.Margin = New Padding(3, 4, 3, 4)
        customerLicenseTextBox.Name = "customerLicenseTextBox"
        customerLicenseTextBox.Size = New Size(217, 27)
        customerLicenseTextBox.TabIndex = 3
        ' 
        ' contactPersonLabel
        ' 
        contactPersonLabel.AutoSize = True
        contactPersonLabel.Font = New Font("Segoe UI", 9F)
        contactPersonLabel.ForeColor = Color.Gray
        contactPersonLabel.Location = New Point(246, 64)
        contactPersonLabel.Name = "contactPersonLabel"
        contactPersonLabel.Size = New Size(107, 20)
        contactPersonLabel.TabIndex = 4
        contactPersonLabel.Text = "Contact Person"
        ' 
        ' contactPersonTextBox
        ' 
        contactPersonTextBox.Location = New Point(246, 91)
        contactPersonTextBox.Margin = New Padding(3, 4, 3, 4)
        contactPersonTextBox.Name = "contactPersonTextBox"
        contactPersonTextBox.Size = New Size(217, 27)
        contactPersonTextBox.TabIndex = 5
        ' 
        ' contactNoLabel
        ' 
        contactNoLabel.AutoSize = True
        contactNoLabel.Font = New Font("Segoe UI", 9F)
        contactNoLabel.ForeColor = Color.Gray
        contactNoLabel.Location = New Point(478, 64)
        contactNoLabel.Name = "contactNoLabel"
        contactNoLabel.Size = New Size(87, 20)
        contactNoLabel.TabIndex = 6
        contactNoLabel.Text = "Contact No."
        ' 
        ' contactNoTextBox
        ' 
        contactNoTextBox.Location = New Point(478, 91)
        contactNoTextBox.Margin = New Padding(3, 4, 3, 4)
        contactNoTextBox.Name = "contactNoTextBox"
        contactNoTextBox.Size = New Size(217, 27)
        contactNoTextBox.TabIndex = 7
        ' 
        ' addressLabel
        ' 
        addressLabel.AutoSize = True
        addressLabel.Font = New Font("Segoe UI", 9F)
        addressLabel.ForeColor = Color.Gray
        addressLabel.Location = New Point(710, 64)
        addressLabel.Name = "addressLabel"
        addressLabel.Size = New Size(62, 20)
        addressLabel.TabIndex = 8
        addressLabel.Text = "Address"
        ' 
        ' addressTextBox
        ' 
        addressTextBox.Location = New Point(710, 91)
        addressTextBox.Margin = New Padding(3, 4, 3, 4)
        addressTextBox.Name = "addressTextBox"
        addressTextBox.Size = New Size(285, 27)
        addressTextBox.TabIndex = 9
        ' 
        ' poNoLabel
        ' 
        poNoLabel.AutoSize = True
        poNoLabel.Font = New Font("Segoe UI", 9F)
        poNoLabel.ForeColor = Color.Gray
        poNoLabel.Location = New Point(14, 140)
        poNoLabel.Name = "poNoLabel"
        poNoLabel.Size = New Size(55, 20)
        poNoLabel.TabIndex = 10
        poNoLabel.Text = "PO No."
        ' 
        ' poNoTextBox
        ' 
        poNoTextBox.Location = New Point(14, 167)
        poNoTextBox.Margin = New Padding(3, 4, 3, 4)
        poNoTextBox.Name = "poNoTextBox"
        poNoTextBox.Size = New Size(217, 27)
        poNoTextBox.TabIndex = 11
        ' 
        ' poDateLabel
        ' 
        poDateLabel.AutoSize = True
        poDateLabel.Font = New Font("Segoe UI", 9F)
        poDateLabel.ForeColor = Color.Gray
        poDateLabel.Location = New Point(246, 140)
        poDateLabel.Name = "poDateLabel"
        poDateLabel.Size = New Size(64, 20)
        poDateLabel.TabIndex = 12
        poDateLabel.Text = "PO Date"
        ' 
        ' poDatePicker
        ' 
        poDatePicker.Format = DateTimePickerFormat.Short
        poDatePicker.Location = New Point(246, 167)
        poDatePicker.Margin = New Padding(3, 4, 3, 4)
        poDatePicker.Name = "poDatePicker"
        poDatePicker.Size = New Size(217, 27)
        poDatePicker.TabIndex = 13
        ' 
        ' deliveryDateLabel
        ' 
        deliveryDateLabel.AutoSize = True
        deliveryDateLabel.Font = New Font("Segoe UI", 9F)
        deliveryDateLabel.ForeColor = Color.Gray
        deliveryDateLabel.Location = New Point(478, 140)
        deliveryDateLabel.Name = "deliveryDateLabel"
        deliveryDateLabel.Size = New Size(99, 20)
        deliveryDateLabel.TabIndex = 14
        deliveryDateLabel.Text = "Delivery Date"
        ' 
        ' deliveryDatePicker
        ' 
        deliveryDatePicker.Format = DateTimePickerFormat.Short
        deliveryDatePicker.Location = New Point(478, 167)
        deliveryDatePicker.Margin = New Padding(3, 4, 3, 4)
        deliveryDatePicker.Name = "deliveryDatePicker"
        deliveryDatePicker.Size = New Size(217, 27)
        deliveryDatePicker.TabIndex = 15
        ' 
        ' prescriptionPanel
        ' 
        prescriptionPanel.BackColor = Color.White
        prescriptionPanel.Controls.Add(prescriptionTitle)
        prescriptionPanel.Controls.Add(rxNoLabel)
        prescriptionPanel.Controls.Add(rxNoTextBox)
        prescriptionPanel.Controls.Add(rxPractitionerLabel)
        prescriptionPanel.Controls.Add(rxPractitionerTextBox)
        prescriptionPanel.Controls.Add(rxLicenseLabel)
        prescriptionPanel.Controls.Add(rxLicenseTextBox)
        prescriptionPanel.Controls.Add(rxDateLabel)
        prescriptionPanel.Controls.Add(rxDateTextBox)
        prescriptionPanel.Dock = DockStyle.Fill
        prescriptionPanel.Location = New Point(0, 0)
        prescriptionPanel.Margin = New Padding(0)
        prescriptionPanel.Name = "prescriptionPanel"
        prescriptionPanel.Size = New Size(2094, 253)
        prescriptionPanel.TabIndex = 1
        ' 
        ' prescriptionTitle
        ' 
        prescriptionTitle.AutoSize = True
        prescriptionTitle.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        prescriptionTitle.ForeColor = Color.Black
        prescriptionTitle.Location = New Point(14, 16)
        prescriptionTitle.Name = "prescriptionTitle"
        prescriptionTitle.Size = New Size(233, 25)
        prescriptionTitle.TabIndex = 0
        prescriptionTitle.Text = "Prescription Information"
        ' 
        ' rxNoLabel
        ' 
        rxNoLabel.AutoSize = True
        rxNoLabel.Font = New Font("Segoe UI", 9F)
        rxNoLabel.ForeColor = Color.Gray
        rxNoLabel.Location = New Point(14, 57)
        rxNoLabel.Name = "rxNoLabel"
        rxNoLabel.Size = New Size(114, 20)
        rxNoLabel.TabIndex = 1
        rxNoLabel.Text = "Prescription No."
        ' 
        ' rxNoTextBox
        ' 
        rxNoTextBox.CharacterCasing = CharacterCasing.Upper
        rxNoTextBox.Location = New Point(14, 84)
        rxNoTextBox.Margin = New Padding(3, 4, 3, 4)
        rxNoTextBox.Name = "rxNoTextBox"
        rxNoTextBox.Size = New Size(331, 27)
        rxNoTextBox.TabIndex = 2
        ' 
        ' rxPractitionerLabel
        ' 
        rxPractitionerLabel.AutoSize = True
        rxPractitionerLabel.Font = New Font("Segoe UI", 9F)
        rxPractitionerLabel.ForeColor = Color.Gray
        rxPractitionerLabel.Location = New Point(366, 57)
        rxPractitionerLabel.Name = "rxPractitionerLabel"
        rxPractitionerLabel.Size = New Size(162, 20)
        rxPractitionerLabel.TabIndex = 3
        rxPractitionerLabel.Text = "Prescribing Practitioner"
        ' 
        ' rxPractitionerTextBox
        ' 
        rxPractitionerTextBox.CharacterCasing = CharacterCasing.Upper
        rxPractitionerTextBox.Location = New Point(366, 84)
        rxPractitionerTextBox.Margin = New Padding(3, 4, 3, 4)
        rxPractitionerTextBox.Name = "rxPractitionerTextBox"
        rxPractitionerTextBox.Size = New Size(331, 27)
        rxPractitionerTextBox.TabIndex = 4
        ' 
        ' rxLicenseLabel
        ' 
        rxLicenseLabel.AutoSize = True
        rxLicenseLabel.Font = New Font("Segoe UI", 9F)
        rxLicenseLabel.ForeColor = Color.Gray
        rxLicenseLabel.Location = New Point(718, 57)
        rxLicenseLabel.Name = "rxLicenseLabel"
        rxLicenseLabel.Size = New Size(114, 20)
        rxLicenseLabel.TabIndex = 5
        rxLicenseLabel.Text = "PRC License No."
        ' 
        ' rxLicenseTextBox
        ' 
        rxLicenseTextBox.CharacterCasing = CharacterCasing.Upper
        rxLicenseTextBox.Location = New Point(718, 84)
        rxLicenseTextBox.Margin = New Padding(3, 4, 3, 4)
        rxLicenseTextBox.Name = "rxLicenseTextBox"
        rxLicenseTextBox.Size = New Size(262, 27)
        rxLicenseTextBox.TabIndex = 6
        ' 
        ' rxDateLabel
        ' 
        rxDateLabel.AutoSize = True
        rxDateLabel.Font = New Font("Segoe UI", 9F)
        rxDateLabel.ForeColor = Color.Gray
        rxDateLabel.Location = New Point(14, 132)
        rxDateLabel.Name = "rxDateLabel"
        rxDateLabel.Size = New Size(123, 20)
        rxDateLabel.TabIndex = 7
        rxDateLabel.Text = "Prescription Date"
        ' 
        ' rxDateTextBox
        ' 
        rxDateTextBox.CustomFormat = "MM/dd/yy"
        rxDateTextBox.Format = DateTimePickerFormat.Custom
        rxDateTextBox.Location = New Point(14, 159)
        rxDateTextBox.Margin = New Padding(3, 4, 3, 4)
        rxDateTextBox.Name = "rxDateTextBox"
        rxDateTextBox.Size = New Size(331, 27)
        rxDateTextBox.TabIndex = 8
        ' 
        ' cartCard
        ' 
        cartCard.BackColor = Color.White
        cartCard.Controls.Add(cartTitleLabel)
        cartCard.Controls.Add(cartItemsPanel)
        cartCard.Controls.Add(cartSubtotalLabel)
        cartCard.Controls.Add(cartSubtotalValue)
        cartCard.Controls.Add(checkoutButton)
        cartCard.Dock = DockStyle.Fill
        cartCard.Location = New Point(1309, 4)
        cartCard.Margin = New Padding(3, 4, 3, 4)
        cartCard.Name = "cartCard"
        cartCard.Padding = New Padding(18, 21, 18, 21)
        cartCard.Size = New Size(555, 1114)
        cartCard.TabIndex = 1
        ' 
        ' cartTitleLabel
        ' 
        cartTitleLabel.AutoSize = True
        cartTitleLabel.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        cartTitleLabel.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        cartTitleLabel.Location = New Point(18, 19)
        cartTitleLabel.Name = "cartTitleLabel"
        cartTitleLabel.Size = New Size(142, 25)
        cartTitleLabel.TabIndex = 0
        cartTitleLabel.Text = "Shopping Cart"
        ' 
        ' cartItemsPanel
        ' 
        cartItemsPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        cartItemsPanel.AutoScroll = True
        cartItemsPanel.FlowDirection = FlowDirection.TopDown
        cartItemsPanel.Location = New Point(18, 69)
        cartItemsPanel.Margin = New Padding(3, 4, 3, 4)
        cartItemsPanel.Name = "cartItemsPanel"
        cartItemsPanel.Size = New Size(829, 1728)
        cartItemsPanel.TabIndex = 1
        cartItemsPanel.WrapContents = False
        ' 
        ' cartSubtotalLabel
        ' 
        cartSubtotalLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        cartSubtotalLabel.AutoSize = True
        cartSubtotalLabel.Location = New Point(18, 1821)
        cartSubtotalLabel.Name = "cartSubtotalLabel"
        cartSubtotalLabel.Size = New Size(68, 20)
        cartSubtotalLabel.TabIndex = 2
        cartSubtotalLabel.Text = "Subtotal:"
        ' 
        ' cartSubtotalValue
        ' 
        cartSubtotalValue.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        cartSubtotalValue.AutoSize = True
        cartSubtotalValue.Location = New Point(704, 1821)
        cartSubtotalValue.Name = "cartSubtotalValue"
        cartSubtotalValue.Size = New Size(48, 20)
        cartSubtotalValue.TabIndex = 3
        cartSubtotalValue.Text = "P 0.00"
        ' 
        ' checkoutButton
        ' 
        checkoutButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        checkoutButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        checkoutButton.Cursor = Cursors.Hand
        checkoutButton.FlatAppearance.BorderSize = 0
        checkoutButton.FlatStyle = FlatStyle.Flat
        checkoutButton.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        checkoutButton.ForeColor = Color.White
        checkoutButton.Location = New Point(669, 1874)
        checkoutButton.Margin = New Padding(3, 4, 3, 4)
        checkoutButton.Name = "checkoutButton"
        checkoutButton.Size = New Size(160, 48)
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
        checkoutView.Location = New Point(23, 27)
        checkoutView.Margin = New Padding(3, 4, 3, 4)
        checkoutView.Name = "checkoutView"
        checkoutView.Size = New Size(1867, 1122)
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
        orderReviewCard.Margin = New Padding(3, 4, 3, 4)
        orderReviewCard.Name = "orderReviewCard"
        orderReviewCard.Size = New Size(937, 933)
        orderReviewCard.TabIndex = 0
        ' 
        ' orderReviewTitle
        ' 
        orderReviewTitle.AutoSize = True
        orderReviewTitle.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        orderReviewTitle.ForeColor = Color.Black
        orderReviewTitle.Location = New Point(23, 27)
        orderReviewTitle.Name = "orderReviewTitle"
        orderReviewTitle.Size = New Size(131, 25)
        orderReviewTitle.TabIndex = 0
        orderReviewTitle.Text = "Order Review"
        ' 
        ' customerTypeLabel
        ' 
        customerTypeLabel.AutoSize = True
        customerTypeLabel.Font = New Font("Segoe UI", 9F)
        customerTypeLabel.ForeColor = Color.Gray
        customerTypeLabel.Location = New Point(23, 87)
        customerTypeLabel.Name = "customerTypeLabel"
        customerTypeLabel.Size = New Size(120, 20)
        customerTypeLabel.TabIndex = 1
        customerTypeLabel.Text = "CUSTOMER TYPE"
        ' 
        ' customerTypeCombo
        ' 
        customerTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList
        customerTypeCombo.Items.AddRange(New Object() {"Regular", "PWD", "Senior Citizen"})
        customerTypeCombo.Location = New Point(23, 120)
        customerTypeCombo.Margin = New Padding(3, 4, 3, 4)
        customerTypeCombo.Name = "customerTypeCombo"
        customerTypeCombo.Size = New Size(285, 28)
        customerTypeCombo.TabIndex = 2
        ' 
        ' customerNameLabel
        ' 
        customerNameLabel.AutoSize = True
        customerNameLabel.Font = New Font("Segoe UI", 9F)
        customerNameLabel.ForeColor = Color.Gray
        customerNameLabel.Location = New Point(23, 187)
        customerNameLabel.Name = "customerNameLabel"
        customerNameLabel.Size = New Size(119, 20)
        customerNameLabel.TabIndex = 3
        customerNameLabel.Text = "Customer Name:"
        ' 
        ' customerNameTextBox
        ' 
        customerNameTextBox.CharacterCasing = CharacterCasing.Upper
        customerNameTextBox.Location = New Point(160, 183)
        customerNameTextBox.Margin = New Padding(3, 4, 3, 4)
        customerNameTextBox.Name = "customerNameTextBox"
        customerNameTextBox.Size = New Size(285, 27)
        customerNameTextBox.TabIndex = 4
        ' 
        ' idNumberLabel
        ' 
        idNumberLabel.AutoSize = True
        idNumberLabel.Font = New Font("Segoe UI", 9F)
        idNumberLabel.ForeColor = Color.Gray
        idNumberLabel.Location = New Point(23, 227)
        idNumberLabel.Name = "idNumberLabel"
        idNumberLabel.Size = New Size(85, 20)
        idNumberLabel.TabIndex = 5
        idNumberLabel.Text = "ID Number:"
        ' 
        ' idNumberTextBox
        ' 
        idNumberTextBox.CharacterCasing = CharacterCasing.Upper
        idNumberTextBox.Location = New Point(160, 223)
        idNumberTextBox.Margin = New Padding(3, 4, 3, 4)
        idNumberTextBox.Name = "idNumberTextBox"
        idNumberTextBox.Size = New Size(285, 27)
        idNumberTextBox.TabIndex = 6
        ' 
        ' orderSummaryLabel
        ' 
        orderSummaryLabel.AutoSize = True
        orderSummaryLabel.Font = New Font("Segoe UI", 9F)
        orderSummaryLabel.ForeColor = Color.Gray
        orderSummaryLabel.Location = New Point(23, 293)
        orderSummaryLabel.Name = "orderSummaryLabel"
        orderSummaryLabel.Size = New Size(132, 20)
        orderSummaryLabel.TabIndex = 7
        orderSummaryLabel.Text = "ORDER SUMMARY"
        ' 
        ' orderSummaryPanel
        ' 
        orderSummaryPanel.AutoScroll = True
        orderSummaryPanel.FlowDirection = FlowDirection.TopDown
        orderSummaryPanel.Location = New Point(23, 333)
        orderSummaryPanel.Margin = New Padding(3, 4, 3, 4)
        orderSummaryPanel.Name = "orderSummaryPanel"
        orderSummaryPanel.Size = New Size(891, 467)
        orderSummaryPanel.TabIndex = 8
        orderSummaryPanel.WrapContents = False
        ' 
        ' prescriptionLabel
        ' 
        prescriptionLabel.AutoSize = True
        prescriptionLabel.Font = New Font("Segoe UI", 9F)
        prescriptionLabel.ForeColor = Color.Gray
        prescriptionLabel.Location = New Point(23, 827)
        prescriptionLabel.Name = "prescriptionLabel"
        prescriptionLabel.Size = New Size(106, 20)
        prescriptionLabel.TabIndex = 9
        prescriptionLabel.Text = "PRESCRIPTION"
        ' 
        ' prescriptionValueLabel
        ' 
        prescriptionValueLabel.AutoSize = True
        prescriptionValueLabel.Font = New Font("Segoe UI", 9F)
        prescriptionValueLabel.ForeColor = Color.Gray
        prescriptionValueLabel.Location = New Point(23, 864)
        prescriptionValueLabel.Name = "prescriptionValueLabel"
        prescriptionValueLabel.Size = New Size(0, 20)
        prescriptionValueLabel.TabIndex = 10
        ' 
        ' rxRecordedLabel
        ' 
        rxRecordedLabel.AutoSize = True
        rxRecordedLabel.Font = New Font("Segoe UI", 9F)
        rxRecordedLabel.ForeColor = Color.Gray
        rxRecordedLabel.Location = New Point(617, 864)
        rxRecordedLabel.Name = "rxRecordedLabel"
        rxRecordedLabel.Size = New Size(187, 20)
        rxRecordedLabel.TabIndex = 11
        rxRecordedLabel.Text = "INFORMATION RECORDED"
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
        paymentCard.Controls.Add(backToPosButton)
        paymentCard.Controls.Add(cancelButton)
        paymentCard.Controls.Add(confirmButton)
        paymentCard.Location = New Point(960, 0)
        paymentCard.Margin = New Padding(3, 4, 3, 4)
        paymentCard.Name = "paymentCard"
        paymentCard.Size = New Size(919, 933)
        paymentCard.TabIndex = 1
        ' 
        ' paymentTitle
        ' 
        paymentTitle.AutoSize = True
        paymentTitle.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        paymentTitle.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        paymentTitle.Location = New Point(23, 27)
        paymentTitle.Name = "paymentTitle"
        paymentTitle.Size = New Size(90, 25)
        paymentTitle.TabIndex = 0
        paymentTitle.Text = "Payment"
        ' 
        ' subtotalLabel
        ' 
        subtotalLabel.AutoSize = True
        subtotalLabel.Font = New Font("Segoe UI", 9F)
        subtotalLabel.ForeColor = Color.Gray
        subtotalLabel.Location = New Point(23, 93)
        subtotalLabel.Name = "subtotalLabel"
        subtotalLabel.Size = New Size(79, 20)
        subtotalLabel.TabIndex = 1
        subtotalLabel.Text = "SUBTOTAL:"
        ' 
        ' subtotalValue
        ' 
        subtotalValue.AutoSize = True
        subtotalValue.Font = New Font("Segoe UI", 9F)
        subtotalValue.ForeColor = Color.Gray
        subtotalValue.Location = New Point(754, 93)
        subtotalValue.Name = "subtotalValue"
        subtotalValue.Size = New Size(48, 20)
        subtotalValue.TabIndex = 2
        subtotalValue.Text = "P 0.00"
        ' 
        ' discountLabel
        ' 
        discountLabel.AutoSize = True
        discountLabel.Font = New Font("Segoe UI", 9F)
        discountLabel.ForeColor = Color.Gray
        discountLabel.Location = New Point(23, 133)
        discountLabel.Name = "discountLabel"
        discountLabel.Size = New Size(84, 20)
        discountLabel.TabIndex = 3
        discountLabel.Text = "DISCOUNT:"
        ' 
        ' discountValue
        ' 
        discountValue.AutoSize = True
        discountValue.Font = New Font("Segoe UI", 9F)
        discountValue.ForeColor = Color.Gray
        discountValue.Location = New Point(754, 133)
        discountValue.Name = "discountValue"
        discountValue.Size = New Size(48, 20)
        discountValue.TabIndex = 4
        discountValue.Text = "P 0.00"
        ' 
        ' totalDueLabel
        ' 
        totalDueLabel.AutoSize = True
        totalDueLabel.Font = New Font("Segoe UI", 9F)
        totalDueLabel.ForeColor = Color.Gray
        totalDueLabel.Location = New Point(23, 173)
        totalDueLabel.Name = "totalDueLabel"
        totalDueLabel.Size = New Size(86, 20)
        totalDueLabel.TabIndex = 5
        totalDueLabel.Text = "TOTAL DUE:"
        ' 
        ' totalDueValue
        ' 
        totalDueValue.AutoSize = True
        totalDueValue.Font = New Font("Segoe UI", 9F)
        totalDueValue.ForeColor = Color.Gray
        totalDueValue.Location = New Point(754, 173)
        totalDueValue.Name = "totalDueValue"
        totalDueValue.Size = New Size(48, 20)
        totalDueValue.TabIndex = 6
        totalDueValue.Text = "P 0.00"
        ' 
        ' cashLabel
        ' 
        cashLabel.AutoSize = True
        cashLabel.Font = New Font("Segoe UI", 9F)
        cashLabel.ForeColor = Color.Gray
        cashLabel.Location = New Point(23, 267)
        cashLabel.Name = "cashLabel"
        cashLabel.Size = New Size(117, 20)
        cashLabel.TabIndex = 7
        cashLabel.Text = "CASH AMOUNT:"
        ' 
        ' cashValue
        ' 
        cashValue.AutoSize = True
        cashValue.Font = New Font("Segoe UI", 9F)
        cashValue.ForeColor = Color.Gray
        cashValue.Location = New Point(754, 267)
        cashValue.Name = "cashValue"
        cashValue.Size = New Size(17, 20)
        cashValue.TabIndex = 8
        cashValue.Text = "P"
        ' 
        ' changeLabel
        ' 
        changeLabel.AutoSize = True
        changeLabel.Font = New Font("Segoe UI", 9F)
        changeLabel.ForeColor = Color.Gray
        changeLabel.Location = New Point(23, 307)
        changeLabel.Name = "changeLabel"
        changeLabel.Size = New Size(71, 20)
        changeLabel.TabIndex = 9
        changeLabel.Text = "CHANGE:"
        ' 
        ' changeValue
        ' 
        changeValue.AutoSize = True
        changeValue.Font = New Font("Segoe UI", 9F)
        changeValue.ForeColor = Color.Gray
        changeValue.Location = New Point(754, 307)
        changeValue.Name = "changeValue"
        changeValue.Size = New Size(48, 20)
        changeValue.TabIndex = 10
        changeValue.Text = "P 0.00"
        ' 
        ' cashTextBox
        ' 
        cashTextBox.Location = New Point(23, 373)
        cashTextBox.Margin = New Padding(3, 4, 3, 4)
        cashTextBox.MaxLength = 12
        cashTextBox.Name = "cashTextBox"
        cashTextBox.Size = New Size(285, 27)
        cashTextBox.TabIndex = 11
        cashTextBox.Text = "0.00"
        cashTextBox.TextAlign = HorizontalAlignment.Right
        ' 
        ' backToPosButton
        ' 
        backToPosButton.BackColor = Color.White
        backToPosButton.Cursor = Cursors.Hand
        backToPosButton.FlatAppearance.BorderColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        backToPosButton.FlatStyle = FlatStyle.Flat
        backToPosButton.ForeColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        backToPosButton.Location = New Point(377, 853)
        backToPosButton.Margin = New Padding(3, 4, 3, 4)
        backToPosButton.Name = "backToPosButton"
        backToPosButton.Size = New Size(135, 47)
        backToPosButton.TabIndex = 12
        backToPosButton.Text = "BACK TO POS"
        backToPosButton.UseVisualStyleBackColor = False
        ' 
        ' cancelButton
        ' 
        cancelButton.BackColor = Color.FromArgb(CByte(200), CByte(30), CByte(30))
        cancelButton.Cursor = Cursors.Hand
        cancelButton.FlatAppearance.BorderSize = 0
        cancelButton.FlatStyle = FlatStyle.Flat
        cancelButton.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        cancelButton.ForeColor = Color.White
        cancelButton.Location = New Point(640, 853)
        cancelButton.Margin = New Padding(3, 4, 3, 4)
        cancelButton.Name = "cancelButton"
        cancelButton.Size = New Size(114, 47)
        cancelButton.TabIndex = 13
        cancelButton.Text = "CANCEL"
        cancelButton.UseVisualStyleBackColor = False
        ' 
        ' confirmButton
        ' 
        confirmButton.BackColor = Color.FromArgb(CByte(20), CByte(100), CByte(60))
        confirmButton.Cursor = Cursors.Hand
        confirmButton.FlatAppearance.BorderSize = 0
        confirmButton.FlatStyle = FlatStyle.Flat
        confirmButton.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        confirmButton.ForeColor = Color.White
        confirmButton.Location = New Point(766, 853)
        confirmButton.Margin = New Padding(3, 4, 3, 4)
        confirmButton.Name = "confirmButton"
        confirmButton.Size = New Size(130, 47)
        confirmButton.TabIndex = 14
        confirmButton.Text = "CONFIRM"
        confirmButton.UseVisualStyleBackColor = False
        ' 
        ' restockView
        ' 
        restockView.BackColor = Color.FromArgb(CByte(245), CByte(245), CByte(245))
        restockView.Controls.Add(restockCard)
        restockView.Dock = DockStyle.Fill
        restockView.Location = New Point(23, 27)
        restockView.Margin = New Padding(3, 4, 3, 4)
        restockView.Name = "restockView"
        restockView.Size = New Size(1867, 1122)
        restockView.TabIndex = 2
        restockView.Visible = False
        ' 
        ' restockCard
        ' 
        restockCard.BackColor = Color.White
        restockCard.Controls.Add(restockHead)
        restockCard.Controls.Add(restockDesc)
        restockCard.Dock = DockStyle.Top
        restockCard.Location = New Point(0, 0)
        restockCard.Margin = New Padding(3, 4, 3, 4)
        restockCard.Name = "restockCard"
        restockCard.Padding = New Padding(23, 27, 23, 27)
        restockCard.Size = New Size(1867, 213)
        restockCard.TabIndex = 0
        ' 
        ' restockHead
        ' 
        restockHead.AutoSize = True
        restockHead.Font = New Font("Segoe UI", 11F, FontStyle.Bold)
        restockHead.ForeColor = Color.Black
        restockHead.Location = New Point(23, 27)
        restockHead.Name = "restockHead"
        restockHead.Size = New Size(81, 25)
        restockHead.TabIndex = 0
        restockHead.Text = "Restock"
        ' 
        ' restockDesc
        ' 
        restockDesc.AutoSize = True
        restockDesc.Font = New Font("Segoe UI", 9F)
        restockDesc.ForeColor = Color.Gray
        restockDesc.Location = New Point(23, 107)
        restockDesc.Name = "restockDesc"
        restockDesc.Size = New Size(319, 20)
        restockDesc.TabIndex = 1
        restockDesc.Text = "Restock low-inventory products from suppliers."
        ' 
        ' searchButton
        ' 
        searchButton.Location = New Point(0, 0)
        searchButton.Name = "searchButton"
        searchButton.Size = New Size(75, 23)
        searchButton.TabIndex = 0
        ' 
        ' Staff
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(2176, 1325)
        Controls.Add(mainLayout)
        Margin = New Padding(3, 4, 3, 4)
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
        transactionView.ResumeLayout(False)
        transactionLayout.ResumeLayout(False)
        transactionLeftPanel.ResumeLayout(False)
        transactionLeftLayout.ResumeLayout(False)
        modePanel.ResumeLayout(False)
        parentFilterPanel.ResumeLayout(False)
        searchPanel.ResumeLayout(False)
        searchPanel.PerformLayout()
        resultsPanel.ResumeLayout(False)
        CType(resultsGrid, ComponentModel.ISupportInitialize).EndInit()
        quantityPanel.ResumeLayout(False)
        quantityPanel.PerformLayout()
        transactionDetailsPanel.ResumeLayout(False)
        wholesaleDetailsPanel.ResumeLayout(False)
        wholesaleDetailsPanel.PerformLayout()
        prescriptionPanel.ResumeLayout(False)
        prescriptionPanel.PerformLayout()
        cartCard.ResumeLayout(False)
        cartCard.PerformLayout()
        checkoutView.ResumeLayout(False)
        orderReviewCard.ResumeLayout(False)
        orderReviewCard.PerformLayout()
        paymentCard.ResumeLayout(False)
        paymentCard.PerformLayout()
        restockView.ResumeLayout(False)
        restockCard.ResumeLayout(False)
        restockCard.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents mainLayout As TableLayoutPanel
    Friend WithEvents sidebar As Panel
    Friend WithEvents logoBox As PictureBox
    Friend WithEvents PharName As Label
    Friend WithEvents navTransaction As Button
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
    Friend WithEvents transactionView As Panel
    Friend WithEvents transactionLayout As TableLayoutPanel
    Friend WithEvents transactionLeftPanel As Panel
    Friend WithEvents transactionLeftLayout As TableLayoutPanel
    Friend WithEvents transactionDetailsPanel As Panel
    Friend WithEvents modePanel As Panel
    Friend WithEvents retailModeButton As Button
    Friend WithEvents wholesaleModeButton As Button
    Friend WithEvents parentFilterPanel As Panel
    Friend WithEvents allParentFilterButton As Button
    Friend WithEvents medicineFilterButton As Button
    Friend WithEvents supplementsFilterButton As Button
    Friend WithEvents medicalSupplyFilterButton As Button
    Friend WithEvents searchPanel As Panel
    Friend WithEvents searchLabel As Label
    Friend WithEvents searchFilterCombo As ComboBox
    Friend WithEvents searchTextBox As TextBox
    Friend WithEvents searchButton As Button
    Friend WithEvents resultsPanel As Panel
    Friend WithEvents resultsGrid As DataGridView
    Friend WithEvents quantityPanel As Panel
    Friend WithEvents qtyLabel As Label
    Friend WithEvents qtyMinusButton As Button
    Friend WithEvents qtyTextBox As TextBox
    Friend WithEvents qtyPlusButton As Button
    Friend WithEvents addToCartButton As Button
    Friend WithEvents prescriptionPanel As Panel
    Friend WithEvents prescriptionTitle As Label
    Friend WithEvents rxNoLabel As Label
    Friend WithEvents rxNoTextBox As TextBox
    Friend WithEvents rxPractitionerLabel As Label
    Friend WithEvents rxPractitionerTextBox As TextBox
    Friend WithEvents rxLicenseLabel As Label
    Friend WithEvents rxLicenseTextBox As TextBox
    Friend WithEvents rxDateLabel As Label
    Friend WithEvents rxDateTextBox As DateTimePicker
    Friend WithEvents wholesaleDetailsPanel As Panel
    Friend WithEvents wholesaleDetailsTitle As Label
    Friend WithEvents customerSearchButton As Button
    Friend WithEvents customerLicenseLabel As Label
    Friend WithEvents customerLicenseTextBox As TextBox
    Friend WithEvents contactPersonLabel As Label
    Friend WithEvents contactPersonTextBox As TextBox
    Friend WithEvents contactNoLabel As Label
    Friend WithEvents contactNoTextBox As TextBox
    Friend WithEvents addressLabel As Label
    Friend WithEvents addressTextBox As TextBox
    Friend WithEvents poNoLabel As Label
    Friend WithEvents poNoTextBox As TextBox
    Friend WithEvents poDateLabel As Label
    Friend WithEvents poDatePicker As DateTimePicker
    Friend WithEvents deliveryDateLabel As Label
    Friend WithEvents deliveryDatePicker As DateTimePicker
    Friend WithEvents cartCard As Panel
    Friend WithEvents cartTitleLabel As Label
    Friend WithEvents cartItemsPanel As FlowLayoutPanel
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
    Friend WithEvents orderSummaryPanel As FlowLayoutPanel
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
    Friend WithEvents backToPosButton As Button
    Friend WithEvents cancelButton As Button
    Friend WithEvents confirmButton As Button
    Friend WithEvents restockView As Panel
    Friend WithEvents restockCard As Panel
    Friend WithEvents restockHead As Label
    Friend WithEvents restockDesc As Label
End Class
