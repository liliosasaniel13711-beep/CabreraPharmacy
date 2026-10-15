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
        transactionDetailsPanel = New Panel()
        modePanel = New Panel()
        retailModeButton = New Button()
        wholesaleModeButton = New Button()
        searchPanel = New Panel()
        searchLabel = New Label()
        searchFilterCombo = New ComboBox()
        searchTextBox = New TextBox()
        searchButton = New Button()
        resultsGrid = New DataGridView()
        quantityPanel = New Panel()
        qtyLabel = New Label()
        qtyMinusButton = New Button()
        qtyTextBox = New TextBox()
        qtyPlusButton = New Button()
        addToCartButton = New Button()
        prescriptionPanel = New Panel()
        prescriptionTitle = New Label()
        rxNoLabel = New Label()
        rxNoTextBox = New TextBox()
        rxPractitionerLabel = New Label()
        rxPractitionerTextBox = New TextBox()
        rxLicenseLabel = New Label()
        rxLicenseTextBox = New TextBox()
        rxDateLabel = New Label()
        rxDateTextBox = New TextBox()
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
        cancelButton = New Button()
        confirmButton = New Button()
        restockView = New Panel()
        restockCard = New Panel()
        restockHead = New Label()
        restockDesc = New Label()
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
        transactionDetailsPanel.SuspendLayout()
        modePanel.SuspendLayout()
        searchPanel.SuspendLayout()
        CType(resultsGrid, ComponentModel.ISupportInitialize).BeginInit()
        quantityPanel.SuspendLayout()
        prescriptionPanel.SuspendLayout()
        wholesaleDetailsPanel.SuspendLayout()
        cartCard.SuspendLayout()
        checkoutView.SuspendLayout()
        orderReviewCard.SuspendLayout()
        paymentCard.SuspendLayout()
        restockView.SuspendLayout()
        restockCard.SuspendLayout()
        SuspendLayout()

        mainLayout.ColumnCount = 2
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 220.0F))
        mainLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        mainLayout.Controls.Add(sidebar, 0, 0)
        mainLayout.Controls.Add(rightLayout, 1, 0)
        mainLayout.Dock = DockStyle.Fill
        mainLayout.RowCount = 1
        mainLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        sidebar.BackColor = Color.White
        sidebar.Controls.Add(logoBox)
        sidebar.Controls.Add(PharName)
        sidebar.Controls.Add(navTransaction)
        sidebar.Controls.Add(navRestock)
        sidebar.Controls.Add(logoutstaff_button)
        sidebar.Dock = DockStyle.Fill
        logoBox.BackColor = Color.FromArgb(255, 215, 0)
        logoBox.Location = New Point(20, 20)
        logoBox.Size = New Size(50, 50)
        PharName.AutoSize = True
        PharName.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        PharName.ForeColor = Color.FromArgb(20, 100, 60)
        PharName.Location = New Point(80, 20)
        PharName.MaximumSize = New Size(130, 0)
        PharName.Text = "Cabrera's" & vbCrLf & "Drugstore and Medical Supplies"
        navTransaction = ConfigureNavButton(navTransaction, "Transaction", 120, True)
        navRestock = ConfigureNavButton(navRestock, "Restock", 175, False)
        logoutstaff_button.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        logoutstaff_button.BackColor = Color.White
        logoutstaff_button.FlatStyle = FlatStyle.Flat
        logoutstaff_button.ForeColor = Color.FromArgb(20, 100, 60)
        logoutstaff_button.Location = New Point(20, 961)
        logoutstaff_button.Size = New Size(180, 45)
        logoutstaff_button.Text = "Log Out"
        logoutstaff_button.UseVisualStyleBackColor = False

        rightLayout.ColumnCount = 1
        rightLayout.RowCount = 2
        rightLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100.0F))
        rightLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        rightLayout.Controls.Add(headerPanel, 0, 0)
        rightLayout.Controls.Add(bodyPanel, 0, 1)
        rightLayout.Dock = DockStyle.Fill
        headerPanel.BackColor = Color.White
        headerPanel.Controls.Add(titleLabel)
        headerPanel.Controls.Add(subtitleLabel)
        headerPanel.Controls.Add(userPanel)
        headerPanel.Dock = DockStyle.Fill
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
        titleLabel.ForeColor = Color.FromArgb(20, 100, 60)
        titleLabel.Location = New Point(20, 10)
        titleLabel.Text = "Transaction"
        subtitleLabel.AutoSize = True
        subtitleLabel.Font = New Font("Segoe UI", 11.0F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.Location = New Point(20, 55)
        subtitleLabel.Text = "Cabrera's Drugstore and Medical Supplies"
        userPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        userPanel.Controls.Add(userAvatar)
        userPanel.Controls.Add(welcomestaff_label)
        userPanel.Controls.Add(userRoleLabel)
        userPanel.Size = New Size(250, 65)
        userAvatar.BackColor = Color.FromArgb(20, 100, 60)
        userAvatar.Location = New Point(0, 10)
        userAvatar.Size = New Size(50, 50)
        welcomestaff_label.AutoSize = True
        welcomestaff_label.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        welcomestaff_label.Location = New Point(60, 10)
        welcomestaff_label.Text = "Welcome, Staff"
        userRoleLabel.AutoSize = True
        userRoleLabel.ForeColor = Color.FromArgb(20, 100, 60)
        userRoleLabel.Location = New Point(60, 34)
        userRoleLabel.Text = "Assistant Pharmacist"

        bodyPanel.BackColor = Color.FromArgb(245, 245, 245)
        bodyPanel.Controls.Add(transactionView)
        bodyPanel.Controls.Add(checkoutView)
        bodyPanel.Controls.Add(restockView)
        bodyPanel.Dock = DockStyle.Fill
        bodyPanel.Padding = New Padding(20)

        transactionView.Controls.Add(transactionLayout)
        transactionView.Dock = DockStyle.Fill
        transactionLayout.ColumnCount = 2
        transactionLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70.0F))
        transactionLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
        transactionLayout.Controls.Add(transactionLeftPanel, 0, 0)
        transactionLayout.Controls.Add(cartCard, 1, 0)
        transactionLayout.Dock = DockStyle.Fill
        transactionLayout.RowCount = 1
        transactionLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        transactionLeftPanel.Dock = DockStyle.Fill
        transactionLeftPanel.Padding = New Padding(0, 0, 10, 0)
        transactionLeftLayout.ColumnCount = 1
        transactionLeftLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        transactionLeftLayout.Dock = DockStyle.Fill
        transactionLeftLayout.RowCount = 5
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 46.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 160.0F))
        transactionLeftPanel.Controls.Add(transactionLeftLayout)

        modePanel.BackColor = Color.White
        modePanel.Controls.Add(retailModeButton)
        modePanel.Controls.Add(wholesaleModeButton)
        modePanel.Dock = DockStyle.Fill
        retailModeButton = ConfigureModeButton(retailModeButton, "Retail", 10, True)
        wholesaleModeButton = ConfigureModeButton(wholesaleModeButton, "Wholesale", 132, False)
        transactionLeftLayout.Controls.Add(modePanel, 0, 0)

        searchPanel.BackColor = Color.White
        searchPanel.Controls.Add(searchLabel)
        searchPanel.Controls.Add(searchFilterCombo)
        searchPanel.Controls.Add(searchTextBox)
        searchPanel.Controls.Add(searchButton)
        searchPanel.Dock = DockStyle.Fill
        searchLabel.AutoSize = True
        searchLabel.Location = New Point(12, 15)
        searchLabel.Text = "Medicine:"
        searchFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList
        searchFilterCombo.Items.AddRange(New Object() {"All", "Product ID", "Generic Name", "Brand Name"})
        searchFilterCombo.Location = New Point(90, 11)
        searchFilterCombo.Size = New Size(145, 25)
        searchTextBox.Location = New Point(245, 11)
        searchTextBox.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        searchTextBox.Size = New Size(470, 25)
        searchButton = ConfigureActionButton(searchButton, "Search", 725, 8, 100, 32)
        searchButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        transactionLeftLayout.Controls.Add(searchPanel, 0, 1)

        resultsGrid.AllowUserToAddRows = False
        resultsGrid.AllowUserToDeleteRows = False
        resultsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        resultsGrid.BackgroundColor = Color.White
        resultsGrid.BorderStyle = BorderStyle.None
        resultsGrid.ColumnHeadersHeight = 34
        resultsGrid.MultiSelect = False
        resultsGrid.ReadOnly = True
        resultsGrid.RowHeadersVisible = False
        resultsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        resultsGrid.Dock = DockStyle.Fill
        resultsGrid.Margin = New Padding(0, 0, 0, 8)
        transactionLeftLayout.Controls.Add(resultsGrid, 0, 2)

        quantityPanel.BackColor = Color.White
        quantityPanel.Controls.Add(qtyLabel)
        quantityPanel.Controls.Add(qtyMinusButton)
        quantityPanel.Controls.Add(qtyTextBox)
        quantityPanel.Controls.Add(qtyPlusButton)
        quantityPanel.Controls.Add(addToCartButton)
        quantityPanel.Dock = DockStyle.Fill
        qtyLabel.AutoSize = True
        qtyLabel.Location = New Point(12, 15)
        qtyLabel.Text = "Quantity:"
        qtyMinusButton = ConfigureSmallButton(qtyMinusButton, "-", 90)
        qtyTextBox.Location = New Point(126, 11)
        qtyTextBox.Size = New Size(50, 25)
        qtyTextBox.Text = "1"
        qtyTextBox.TextAlign = HorizontalAlignment.Center
        qtyPlusButton = ConfigureSmallButton(qtyPlusButton, "+", 182)
        addToCartButton = ConfigureActionButton(addToCartButton, "ADD TO CART", 725, 7, 150, 34)
        addToCartButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        quantityPanel.Controls.Add(addToCartButton)
        transactionLeftLayout.Controls.Add(quantityPanel, 0, 3)

        transactionDetailsPanel.Dock = DockStyle.Fill
        transactionDetailsPanel.Controls.Add(prescriptionPanel)
        transactionDetailsPanel.Controls.Add(wholesaleDetailsPanel)
        transactionLeftLayout.Controls.Add(transactionDetailsPanel, 0, 4)
        prescriptionPanel.BackColor = Color.White
        prescriptionPanel.Controls.Add(prescriptionTitle)
        AddField(prescriptionPanel, rxNoLabel, "Prescription No.", rxNoTextBox, 12, 43, 290)
        AddField(prescriptionPanel, rxPractitionerLabel, "Prescribing Practitioner", rxPractitionerTextBox, 320, 43, 290)
        AddField(prescriptionPanel, rxLicenseLabel, "PRC License No.", rxLicenseTextBox, 628, 43, 230)
        AddField(prescriptionPanel, rxDateLabel, "Prescription Date", rxDateTextBox, 12, 99, 290)
        prescriptionPanel.Dock = DockStyle.Fill
        prescriptionPanel.Size = New Size(900, 145)
        prescriptionTitle = ConfigureSectionLabel(prescriptionTitle, "Prescription Information", 12, 12)

        wholesaleDetailsPanel.BackColor = Color.White
        wholesaleDetailsPanel.Controls.Add(wholesaleDetailsTitle)
        wholesaleDetailsPanel.Controls.Add(customerSearchButton)
        AddField(wholesaleDetailsPanel, customerLicenseLabel, "License No.", customerLicenseTextBox, 12, 48, 190)
        AddField(wholesaleDetailsPanel, contactPersonLabel, "Contact Person", contactPersonTextBox, 215, 48, 190)
        AddField(wholesaleDetailsPanel, contactNoLabel, "Contact No.", contactNoTextBox, 418, 48, 190)
        AddField(wholesaleDetailsPanel, addressLabel, "Address", addressTextBox, 621, 48, 250)
        AddField(wholesaleDetailsPanel, poNoLabel, "PO No.", poNoTextBox, 12, 105, 190)
        AddDateField(wholesaleDetailsPanel, poDateLabel, "PO Date", poDatePicker, 215, 105, 190)
        AddDateField(wholesaleDetailsPanel, deliveryDateLabel, "Delivery Date", deliveryDatePicker, 418, 105, 190)
        wholesaleDetailsPanel.Dock = DockStyle.Fill
        wholesaleDetailsPanel.Size = New Size(900, 160)
        wholesaleDetailsTitle = ConfigureSectionLabel(wholesaleDetailsTitle, "Wholesale Customer and Purchase Order", 12, 12)
        customerSearchButton = ConfigureActionButton(customerSearchButton, "Find Customer", 735, 8, 140, 30)
        wholesaleDetailsPanel.Controls.Add(customerSearchButton)

        cartCard.BackColor = Color.White
        cartCard.Controls.Add(cartTitleLabel)
        cartCard.Controls.Add(cartItemsPanel)
        cartCard.Controls.Add(cartSubtotalLabel)
        cartCard.Controls.Add(cartSubtotalValue)
        cartCard.Controls.Add(checkoutButton)
        cartCard.Dock = DockStyle.Fill
        cartCard.Padding = New Padding(16)
        cartTitleLabel = ConfigureSectionLabel(cartTitleLabel, "Shopping Cart", 16, 14)
        cartTitleLabel.ForeColor = Color.FromArgb(20, 100, 60)
        cartItemsPanel.AutoScroll = True
        cartItemsPanel.FlowDirection = FlowDirection.TopDown
        cartItemsPanel.Location = New Point(16, 52)
        cartItemsPanel.WrapContents = False
        cartItemsPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        cartItemsPanel.Size = New Size(440, 560)
        cartSubtotalLabel.AutoSize = True
        cartSubtotalLabel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        cartSubtotalLabel.Location = New Point(16, 630)
        cartSubtotalLabel.Text = "Subtotal:"
        cartSubtotalValue.AutoSize = True
        cartSubtotalValue.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        cartSubtotalValue.Location = New Point(330, 630)
        cartSubtotalValue.Text = "P 0.00"
        checkoutButton = ConfigureActionButton(checkoutButton, "CHECKOUT", 300, 670, 140, 36)
        checkoutButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right

        checkoutView.BackColor = Color.FromArgb(245, 245, 245)
        checkoutView.Controls.Add(orderReviewCard)
        checkoutView.Controls.Add(paymentCard)
        checkoutView.Dock = DockStyle.Fill
        checkoutView.Visible = False
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
        orderReviewCard.Size = New Size(820, 700)
        orderReviewTitle = ConfigureSectionLabel(orderReviewTitle, "Order Review", 20, 20)
        customerTypeLabel = ConfigureFieldLabel(customerTypeLabel, "CUSTOMER TYPE", 20, 65)
        customerTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList
        customerTypeCombo.Items.AddRange(New Object() {"Regular", "PWD", "Senior Citizen"})
        customerTypeCombo.Location = New Point(20, 90)
        customerTypeCombo.Size = New Size(250, 25)
        customerNameLabel = ConfigureFieldLabel(customerNameLabel, "Customer Name:", 20, 140)
        customerNameTextBox.Location = New Point(140, 137)
        customerNameTextBox.Size = New Size(250, 23)
        idNumberLabel = ConfigureFieldLabel(idNumberLabel, "ID Number:", 20, 170)
        idNumberTextBox.Location = New Point(140, 167)
        idNumberTextBox.Size = New Size(250, 23)
        orderSummaryLabel = ConfigureFieldLabel(orderSummaryLabel, "ORDER SUMMARY", 20, 220)
        orderSummaryPanel.AutoScroll = True
        orderSummaryPanel.FlowDirection = FlowDirection.TopDown
        orderSummaryPanel.Location = New Point(20, 250)
        orderSummaryPanel.Size = New Size(780, 350)
        orderSummaryPanel.WrapContents = False
        prescriptionLabel = ConfigureFieldLabel(prescriptionLabel, "PRESCRIPTION", 20, 620)
        prescriptionValueLabel = ConfigureFieldLabel(prescriptionValueLabel, "", 20, 648)
        rxRecordedLabel = ConfigureFieldLabel(rxRecordedLabel, "INFORMATION RECORDED", 540, 648)
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
        paymentCard.Size = New Size(804, 700)
        paymentTitle = ConfigureSectionLabel(paymentTitle, "Payment", 20, 20)
        paymentTitle.ForeColor = Color.FromArgb(20, 100, 60)
        subtotalLabel = ConfigureFieldLabel(subtotalLabel, "SUBTOTAL:", 20, 70)
        subtotalValue = ConfigureFieldLabel(subtotalValue, "P 0.00", 660, 70)
        discountLabel = ConfigureFieldLabel(discountLabel, "DISCOUNT:", 20, 100)
        discountValue = ConfigureFieldLabel(discountValue, "P 0.00", 660, 100)
        totalDueLabel = ConfigureFieldLabel(totalDueLabel, "TOTAL DUE:", 20, 130)
        totalDueValue = ConfigureFieldLabel(totalDueValue, "P 0.00", 660, 130)
        cashLabel = ConfigureFieldLabel(cashLabel, "CASH RECEIVED:", 20, 200)
        cashValue = ConfigureFieldLabel(cashValue, "P 0.00", 660, 200)
        changeLabel = ConfigureFieldLabel(changeLabel, "CHANGE:", 20, 230)
        changeValue = ConfigureFieldLabel(changeValue, "P 0.00", 660, 230)
        cashTextBox.Location = New Point(20, 280)
        cashTextBox.Size = New Size(250, 27)
        cancelButton = ConfigureDangerButton(cancelButton, "CANCEL", 560, 640, 100, 35)
        confirmButton = ConfigureActionButton(confirmButton, "CONFIRM", 670, 640, 114, 35)

        restockView.BackColor = Color.FromArgb(245, 245, 245)
        restockView.Controls.Add(restockCard)
        restockView.Dock = DockStyle.Fill
        restockView.Visible = False
        restockCard.BackColor = Color.White
        restockCard.Controls.Add(restockHead)
        restockCard.Controls.Add(restockDesc)
        restockCard.Dock = DockStyle.Top
        restockCard.Padding = New Padding(20)
        restockCard.Size = New Size(1644, 160)
        restockHead = ConfigureSectionLabel(restockHead, "Restock", 20, 20)
        restockDesc = ConfigureFieldLabel(restockDesc, "Restock low-inventory products from suppliers.", 20, 80)

        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1904, 1041)
        Controls.Add(mainLayout)
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
        transactionDetailsPanel.ResumeLayout(False)
        modePanel.ResumeLayout(False)
        searchPanel.ResumeLayout(False)
        searchPanel.PerformLayout()
        CType(resultsGrid, ComponentModel.ISupportInitialize).EndInit()
        quantityPanel.ResumeLayout(False)
        quantityPanel.PerformLayout()
        prescriptionPanel.ResumeLayout(False)
        prescriptionPanel.PerformLayout()
        wholesaleDetailsPanel.ResumeLayout(False)
        wholesaleDetailsPanel.PerformLayout()
        cartCard.ResumeLayout(False)
        cartCard.PerformLayout()
        checkoutView.ResumeLayout(False)
        orderReviewCard.ResumeLayout(False)
        orderReviewCard.PerformLayout()
        paymentCard.ResumeLayout(False)
        paymentCard.PerformLayout()
        restockView.ResumeLayout(False)
        restockCard.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Private Function ConfigureNavButton(button As Button, text As String, top As Integer, active As Boolean) As Button
        button.Cursor = Cursors.Hand
        button.FlatAppearance.BorderSize = 0
        button.FlatStyle = FlatStyle.Flat
        button.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        button.BackColor = If(active, Color.FromArgb(20, 100, 60), Color.White)
        button.ForeColor = If(active, Color.White, Color.FromArgb(20, 100, 60))
        button.Location = New Point(20, top)
        button.Padding = New Padding(10, 0, 0, 0)
        button.Size = New Size(180, 42)
        button.Tag = text
        button.Text = text
        button.TextAlign = ContentAlignment.MiddleLeft
        button.UseVisualStyleBackColor = False
        Return button
    End Function

    Private Function ConfigureModeButton(button As Button, text As String, left As Integer, active As Boolean) As Button
        button.BackColor = If(active, Color.FromArgb(20, 100, 60), Color.White)
        button.Cursor = Cursors.Hand
        button.FlatAppearance.BorderColor = Color.FromArgb(20, 100, 60)
        button.FlatStyle = FlatStyle.Flat
        button.ForeColor = If(active, Color.White, Color.FromArgb(20, 100, 60))
        button.Location = New Point(left, 7)
        button.Size = New Size(120, 32)
        button.Tag = text
        button.Text = text
        button.UseVisualStyleBackColor = False
        Return button
    End Function

    Private Function ConfigureActionButton(button As Button, text As String, left As Integer, top As Integer, width As Integer, height As Integer) As Button
        button.BackColor = Color.FromArgb(20, 100, 60)
        button.Cursor = Cursors.Hand
        button.FlatAppearance.BorderSize = 0
        button.FlatStyle = FlatStyle.Flat
        button.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        button.ForeColor = Color.White
        button.Location = New Point(left, top)
        button.Size = New Size(width, height)
        button.Text = text
        button.UseVisualStyleBackColor = False
        Return button
    End Function

    Private Function ConfigureDangerButton(button As Button, text As String, left As Integer, top As Integer, width As Integer, height As Integer) As Button
        button = ConfigureActionButton(button, text, left, top, width, height)
        button.BackColor = Color.FromArgb(200, 30, 30)
        Return button
    End Function

    Private Function ConfigureSmallButton(button As Button, text As String, left As Integer) As Button
        button.BackColor = Color.White
        button.Cursor = Cursors.Hand
        button.FlatAppearance.BorderColor = Color.LightGray
        button.FlatStyle = FlatStyle.Flat
        button.Location = New Point(left, 9)
        button.Size = New Size(30, 28)
        button.Text = text
        Return button
    End Function

    Private Function ConfigureSectionLabel(label As Label, text As String, left As Integer, top As Integer) As Label
        label.AutoSize = True
        label.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        label.ForeColor = Color.Black
        label.Location = New Point(left, top)
        label.Text = text
        Return label
    End Function

    Private Function ConfigureFieldLabel(label As Label, text As String, left As Integer, top As Integer) As Label
        label.AutoSize = True
        label.Font = New Font("Segoe UI", 9.0F)
        label.ForeColor = Color.Gray
        label.Location = New Point(left, top)
        label.Text = text
        Return label
    End Function

    Private Sub AddField(parent As Panel, caption As Label, text As String, input As TextBox, left As Integer, top As Integer, width As Integer)
        caption = ConfigureFieldLabel(caption, text, left, top)
        input.Location = New Point(left, top + 20)
        input.Size = New Size(width, 24)
        parent.Controls.Add(caption)
        parent.Controls.Add(input)
    End Sub

    Private Sub AddDateField(parent As Panel, caption As Label, text As String, input As DateTimePicker, left As Integer, top As Integer, width As Integer)
        caption = ConfigureFieldLabel(caption, text, left, top)
        input.Format = DateTimePickerFormat.Short
        input.Location = New Point(left, top + 20)
        input.Size = New Size(width, 24)
        parent.Controls.Add(caption)
        parent.Controls.Add(input)
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
    Friend WithEvents searchPanel As Panel
    Friend WithEvents searchLabel As Label
    Friend WithEvents searchFilterCombo As ComboBox
    Friend WithEvents searchTextBox As TextBox
    Friend WithEvents searchButton As Button
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
    Friend WithEvents rxDateTextBox As TextBox
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
    Friend WithEvents cancelButton As Button
    Friend WithEvents confirmButton As Button
    Friend WithEvents restockView As Panel
    Friend WithEvents restockCard As Panel
    Friend WithEvents restockHead As Label
    Friend WithEvents restockDesc As Label
End Class
