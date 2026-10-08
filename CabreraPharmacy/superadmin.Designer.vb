<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SuperAdmin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        sideBarSuperAdmin = New Panel()
        btnDashboard = New Button()
        btnAccounts = New Button()
        btnReports = New Button()
        btnLogin = New Button()
        LogoutBtn = New Button()
        PictureBox1 = New PictureBox()
        storeName1 = New Label()
        storeName = New Label()
        motherPanel = New Panel()
        pnlDashboard = New Panel()
        pnlHeaderDashboard = New Panel()
        usrName = New TextBox()
        usrSuperAdmin = New Label()
        hdrStore = New Label()
        hdrDashboard = New Label()
        TableLayoutPanel4 = New TableLayoutPanel()
        Panel11 = New Panel()
        Panel12 = New Panel()
        TableLayoutPanel1 = New TableLayoutPanel()
        Panel1 = New Panel()
        todaysSale = New TextBox()
        monthDateYear = New TextBox()
        userIDText = New TextBox()
        Panel2 = New Panel()
        transactionRetail = New TextBox()
        TextBox3 = New TextBox()
        TextBox2 = New TextBox()
        Panel3 = New Panel()
        transactionsWholesale = New TextBox()
        TextBox6 = New TextBox()
        TextBox5 = New TextBox()
        Panel4 = New Panel()
        allCategories = New TextBox()
        TextBox15 = New TextBox()
        TextBox14 = New TextBox()
        TableLayoutPanel2 = New TableLayoutPanel()
        Panel5 = New Panel()
        needOrdering = New TextBox()
        TextBox9 = New TextBox()
        TextBox8 = New TextBox()
        Panel6 = New Panel()
        days = New TextBox()
        TextBox12 = New TextBox()
        TextBox11 = New TextBox()
        Panel7 = New Panel()
        requiresDisposal = New TextBox()
        TextBox19 = New TextBox()
        TextBox17 = New TextBox()
        Panel8 = New Panel()
        expensesRecorded = New TextBox()
        TextBox20 = New TextBox()
        TextBox18 = New TextBox()
        TableLayoutPanel3 = New TableLayoutPanel()
        Panel9 = New Panel()
        Panel10 = New Panel()
        pnlReports = New Panel()
        TableLayoutPanel9 = New TableLayoutPanel()
        Panel23 = New Panel()
        employeeViewReport = New Button()
        employeeDescription = New TextBox()
        employeeTitle = New TextBox()
        TableLayoutPanel7 = New TableLayoutPanel()
        Panel19 = New Panel()
        expensesViewReport = New Button()
        expensesDescriptions = New TextBox()
        expensesTitle = New TextBox()
        Panel22 = New Panel()
        financialViewReport = New Button()
        financialDescriptions = New TextBox()
        financialTitle = New TextBox()
        pnlHeaderReports = New Panel()
        TextBox24 = New TextBox()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Panel16 = New Panel()
        reportsDescription = New TextBox()
        titleBusinessReports = New TextBox()
        TableLayoutPanel8 = New TableLayoutPanel()
        Panel20 = New Panel()
        inventoryViewReport = New Button()
        inventoryDescriptions = New TextBox()
        inventoryTitle = New TextBox()
        Panel21 = New Panel()
        retailViewReport = New Button()
        retailDescriptions = New TextBox()
        retailTitle = New TextBox()
        TableLayoutPanel6 = New TableLayoutPanel()
        Panel17 = New Panel()
        wholesaleViewReport = New Button()
        wholesaleDescriptions = New TextBox()
        wholesaleTitle = New TextBox()
        Panel18 = New Panel()
        purchasesViewReport = New Button()
        purchasesDescriptions = New TextBox()
        purchasesTitle = New TextBox()
        pnlInventoryStockReport = New Panel()
        inventoryDataGrid = New DataGridView()
        DataGridViewTextBoxColumn1 = New DataGridViewTextBoxColumn()
        inventoryCategory = New DataGridViewTextBoxColumn()
        inventoryUnit = New DataGridViewTextBoxColumn()
        inventoryStock = New DataGridViewTextBoxColumn()
        inventoryPackage = New DataGridViewTextBoxColumn()
        inventoryBatch = New DataGridViewTextBoxColumn()
        inventoryExpiry = New DataGridViewTextBoxColumn()
        inventorySupplier = New DataGridViewTextBoxColumn()
        inventoryStatus = New DataGridViewTextBoxColumn()
        panelnvenotryStockReport = New Panel()
        inventoryPeriodDropdown = New ComboBox()
        inventoryPrintReport = New Button()
        inventoryGenerated = New TextBox()
        titleInventoryStockReport = New TextBox()
        Panel25 = New Panel()
        Label7 = New Label()
        TextBox50 = New TextBox()
        Label8 = New Label()
        Label9 = New Label()
        pnlFinancialSalesReport = New Panel()
        DataGridView4 = New DataGridView()
        financialTransactionID = New DataGridViewTextBoxColumn()
        financialDate = New DataGridViewTextBoxColumn()
        financialCustomer = New DataGridViewTextBoxColumn()
        financialCashier = New DataGridViewTextBoxColumn()
        financialType = New DataGridViewTextBoxColumn()
        financialTotal = New DataGridViewTextBoxColumn()
        financialStatus = New DataGridViewTextBoxColumn()
        panelFinancialSalesReport = New Panel()
        financialPeriodDropdown = New ComboBox()
        financialPrintReport = New Button()
        financialGenerated = New TextBox()
        titleFinancialSalesReport = New TextBox()
        Panel31 = New Panel()
        Label17 = New Label()
        Label18 = New Label()
        pnlRetailSalesReport = New Panel()
        retailDataGrid = New DataGridView()
        retailTransactionID = New DataGridViewTextBoxColumn()
        retailDate = New DataGridViewTextBoxColumn()
        retailCustomer = New DataGridViewTextBoxColumn()
        retailCashier = New DataGridViewTextBoxColumn()
        retailType = New DataGridViewTextBoxColumn()
        retailTotal = New DataGridViewTextBoxColumn()
        retailStatus = New DataGridViewTextBoxColumn()
        panelRetailSalesReport = New Panel()
        ComboBox4 = New ComboBox()
        retailPrintReport = New Button()
        retailGenerated = New TextBox()
        titleRetailSalesReport = New TextBox()
        Panel24 = New Panel()
        Label10 = New Label()
        TextBox48 = New TextBox()
        Label11 = New Label()
        Label12 = New Label()
        pnlExpensesReport = New Panel()
        expenseDataGrid = New DataGridView()
        expensesExpenseID = New DataGridViewTextBoxColumn()
        expensesDate = New DataGridViewTextBoxColumn()
        expensesCategory = New DataGridViewTextBoxColumn()
        expensesDescription = New DataGridViewTextBoxColumn()
        expensesAmount = New DataGridViewTextBoxColumn()
        expensesRecordedBy = New DataGridViewTextBoxColumn()
        panelExpenseReport = New Panel()
        expensesPeriodDropdown = New ComboBox()
        btnAddExpenses = New Button()
        expensesDateTime = New DateTimePicker()
        ComboBox6 = New ComboBox()
        Button15 = New Button()
        expensesSearchBar = New TextBox()
        titleExpensesReport = New TextBox()
        Panel35 = New Panel()
        TextBox66 = New TextBox()
        Label27 = New Label()
        TextBox64 = New TextBox()
        Label23 = New Label()
        Label24 = New Label()
        TextBox65 = New TextBox()
        Label25 = New Label()
        Label26 = New Label()
        pnlPurchasesReport = New Panel()
        purchasesDataGrid = New DataGridView()
        DataGridViewTextBoxColumn9 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn10 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn11 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn12 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn13 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn14 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn15 = New DataGridViewTextBoxColumn()
        panelPurchasesReport = New Panel()
        purchasesDateTime = New DateTimePicker()
        ComboBox5 = New ComboBox()
        Button14 = New Button()
        purchasesSearchBar = New TextBox()
        titlePurchasesReport = New TextBox()
        Panel33 = New Panel()
        TextBox61 = New TextBox()
        Label22 = New Label()
        Label19 = New Label()
        TextBox60 = New TextBox()
        Label20 = New Label()
        Label21 = New Label()
        pnlWholesaleSalesReport = New Panel()
        wholesaleDataGrid = New DataGridView()
        DataGridViewTextBoxColumn2 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn3 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn4 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn5 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn6 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn7 = New DataGridViewTextBoxColumn()
        DataGridViewTextBoxColumn8 = New DataGridViewTextBoxColumn()
        panelWholesale = New Panel()
        wholesaleScheduleDropdown = New ComboBox()
        wholesalePrintReport = New Button()
        employeeGenerated = New TextBox()
        titleWholesaleSalesReport = New TextBox()
        Panel29 = New Panel()
        TextBox25 = New TextBox()
        Label32 = New Label()
        Label14 = New Label()
        Label15 = New Label()
        pnlEmployeesalesReport = New Panel()
        panelEmployeeDataGrid = New Panel()
        employeeName = New TextBox()
        employeDataGrid = New DataGridView()
        employeeTransactionID = New DataGridViewTextBoxColumn()
        employeeDate = New DataGridViewTextBoxColumn()
        employeeCustomer = New DataGridViewTextBoxColumn()
        employeeCashier = New DataGridViewTextBoxColumn()
        employeeType = New DataGridViewTextBoxColumn()
        employeeTotal = New DataGridViewTextBoxColumn()
        employeeStatus = New DataGridViewTextBoxColumn()
        panelEmployeeSalesReport = New Panel()
        employeeScheduleDropdown = New ComboBox()
        employeeNamesDropdown = New ComboBox()
        employeePrintReport = New Button()
        Button18 = New Button()
        TextBox67 = New TextBox()
        titleEmployeeSalesReport = New TextBox()
        Panel37 = New Panel()
        TextBox68 = New TextBox()
        Label31 = New Label()
        Label28 = New Label()
        TextBox69 = New TextBox()
        Label29 = New Label()
        Label30 = New Label()
        pnlAccounts = New Panel()
        accountsDataGrid = New DataGridView()
        accountUserID = New DataGridViewTextBoxColumn()
        accountFullName = New DataGridViewTextBoxColumn()
        accountUsername = New DataGridViewTextBoxColumn()
        accountRole = New DataGridViewTextBoxColumn()
        accountStatus = New DataGridViewTextBoxColumn()
        accountLastLogin = New DataGridViewTextBoxColumn()
        accountEdit = New DataGridViewButtonColumn()
        accountResetPassword = New DataGridViewButtonColumn()
        accountEnableDisable = New DataGridViewButtonColumn()
        numberTotalUsersRegistered = New TextBox()
        btnAddAccount = New Button()
        accountTableLayout = New TableLayoutPanel()
        panelSuperAdmin = New Panel()
        titleSuperAdmin = New TextBox()
        numberSuperAdmin = New TextBox()
        panelAdmin = New Panel()
        numberAdmin = New TextBox()
        titleAdmin = New TextBox()
        panelAssistant = New Panel()
        numberAssistant = New TextBox()
        titleAssistant = New TextBox()
        pnlHeaderAccounts = New Panel()
        TextBox23 = New TextBox()
        Label2 = New Label()
        Label3 = New Label()
        hdrAccounts = New Label()
        pnlLogin = New Panel()
        NavBar = New Panel()
        LoginLabel = New Label()
        picHide = New PictureBox()
        View = New PictureBox()
        LoginIcon = New PictureBox()
        LoginBtn = New Button()
        PasswordField = New TextBox()
        UsernameField = New TextBox()
        Password = New Label()
        Username = New Label()
        DrugstoreName = New Label()
        imageLogin = New PictureBox()
        sideBarSuperAdmin.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        motherPanel.SuspendLayout()
        pnlDashboard.SuspendLayout()
        pnlHeaderDashboard.SuspendLayout()
        TableLayoutPanel4.SuspendLayout()
        TableLayoutPanel1.SuspendLayout()
        Panel1.SuspendLayout()
        Panel2.SuspendLayout()
        Panel3.SuspendLayout()
        Panel4.SuspendLayout()
        TableLayoutPanel2.SuspendLayout()
        Panel5.SuspendLayout()
        Panel6.SuspendLayout()
        Panel7.SuspendLayout()
        Panel8.SuspendLayout()
        TableLayoutPanel3.SuspendLayout()
        pnlReports.SuspendLayout()
        TableLayoutPanel9.SuspendLayout()
        Panel23.SuspendLayout()
        TableLayoutPanel7.SuspendLayout()
        Panel19.SuspendLayout()
        Panel22.SuspendLayout()
        pnlHeaderReports.SuspendLayout()
        Panel16.SuspendLayout()
        TableLayoutPanel8.SuspendLayout()
        Panel20.SuspendLayout()
        Panel21.SuspendLayout()
        TableLayoutPanel6.SuspendLayout()
        Panel17.SuspendLayout()
        Panel18.SuspendLayout()
        pnlInventoryStockReport.SuspendLayout()
        CType(inventoryDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelnvenotryStockReport.SuspendLayout()
        Panel25.SuspendLayout()
        pnlFinancialSalesReport.SuspendLayout()
        CType(DataGridView4, ComponentModel.ISupportInitialize).BeginInit()
        panelFinancialSalesReport.SuspendLayout()
        Panel31.SuspendLayout()
        pnlRetailSalesReport.SuspendLayout()
        CType(retailDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelRetailSalesReport.SuspendLayout()
        Panel24.SuspendLayout()
        pnlExpensesReport.SuspendLayout()
        CType(expenseDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelExpenseReport.SuspendLayout()
        Panel35.SuspendLayout()
        pnlPurchasesReport.SuspendLayout()
        CType(purchasesDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelPurchasesReport.SuspendLayout()
        Panel33.SuspendLayout()
        pnlWholesaleSalesReport.SuspendLayout()
        CType(wholesaleDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelWholesale.SuspendLayout()
        Panel29.SuspendLayout()
        pnlEmployeesalesReport.SuspendLayout()
        panelEmployeeDataGrid.SuspendLayout()
        CType(employeDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        panelEmployeeSalesReport.SuspendLayout()
        Panel37.SuspendLayout()
        pnlAccounts.SuspendLayout()
        CType(accountsDataGrid, ComponentModel.ISupportInitialize).BeginInit()
        accountTableLayout.SuspendLayout()
        panelSuperAdmin.SuspendLayout()
        panelAdmin.SuspendLayout()
        panelAssistant.SuspendLayout()
        pnlHeaderAccounts.SuspendLayout()
        pnlLogin.SuspendLayout()
        NavBar.SuspendLayout()
        CType(picHide, ComponentModel.ISupportInitialize).BeginInit()
        CType(View, ComponentModel.ISupportInitialize).BeginInit()
        CType(LoginIcon, ComponentModel.ISupportInitialize).BeginInit()
        CType(imageLogin, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' sideBarSuperAdmin
        ' 
        sideBarSuperAdmin.BackColor = SystemColors.ControlLightLight
        sideBarSuperAdmin.Controls.Add(btnDashboard)
        sideBarSuperAdmin.Controls.Add(btnAccounts)
        sideBarSuperAdmin.Controls.Add(btnReports)
        sideBarSuperAdmin.Controls.Add(btnLogin)
        sideBarSuperAdmin.Controls.Add(LogoutBtn)
        sideBarSuperAdmin.Controls.Add(PictureBox1)
        sideBarSuperAdmin.Controls.Add(storeName1)
        sideBarSuperAdmin.Controls.Add(storeName)
        sideBarSuperAdmin.Dock = DockStyle.Left
        sideBarSuperAdmin.Location = New Point(0, 0)
        sideBarSuperAdmin.Name = "sideBarSuperAdmin"
        sideBarSuperAdmin.Size = New Size(219, 830)
        sideBarSuperAdmin.TabIndex = 1
        ' 
        ' btnDashboard
        ' 
        btnDashboard.BackColor = SystemColors.ControlLightLight
        btnDashboard.FlatAppearance.BorderSize = 0
        btnDashboard.FlatStyle = FlatStyle.Flat
        btnDashboard.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDashboard.ForeColor = Color.SeaGreen
        btnDashboard.Location = New Point(20, 122)
        btnDashboard.Name = "btnDashboard"
        btnDashboard.Size = New Size(175, 35)
        btnDashboard.TabIndex = 11
        btnDashboard.Text = "     Dashboard"
        btnDashboard.UseVisualStyleBackColor = False
        ' 
        ' btnAccounts
        ' 
        btnAccounts.BackColor = SystemColors.ControlLightLight
        btnAccounts.FlatAppearance.BorderSize = 0
        btnAccounts.FlatStyle = FlatStyle.Flat
        btnAccounts.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAccounts.ForeColor = Color.SeaGreen
        btnAccounts.Location = New Point(20, 163)
        btnAccounts.Name = "btnAccounts"
        btnAccounts.Size = New Size(175, 35)
        btnAccounts.TabIndex = 12
        btnAccounts.Text = "   Accounts"
        btnAccounts.UseVisualStyleBackColor = False
        ' 
        ' btnReports
        ' 
        btnReports.BackColor = SystemColors.ControlLightLight
        btnReports.FlatAppearance.BorderSize = 0
        btnReports.FlatStyle = FlatStyle.Flat
        btnReports.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnReports.ForeColor = Color.SeaGreen
        btnReports.Location = New Point(20, 204)
        btnReports.Name = "btnReports"
        btnReports.Size = New Size(175, 35)
        btnReports.TabIndex = 13
        btnReports.Text = "Reports"
        btnReports.UseVisualStyleBackColor = False
        ' 
        ' btnLogin
        ' 
        btnLogin.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnLogin.BackColor = SystemColors.ControlLightLight
        btnLogin.FlatAppearance.BorderSize = 0
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnLogin.ForeColor = Color.SeaGreen
        btnLogin.Location = New Point(20, 782)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(175, 36)
        btnLogin.TabIndex = 17
        btnLogin.Text = "Log Out"
        btnLogin.UseVisualStyleBackColor = False
        ' 
        ' LogoutBtn
        ' 
        LogoutBtn.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        LogoutBtn.BackColor = SystemColors.Control
        LogoutBtn.FlatAppearance.BorderSize = 2
        LogoutBtn.FlatStyle = FlatStyle.Flat
        LogoutBtn.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LogoutBtn.ForeColor = Color.SeaGreen
        LogoutBtn.Location = New Point(20, 4084)
        LogoutBtn.Name = "LogoutBtn"
        LogoutBtn.Size = New Size(175, 35)
        LogoutBtn.TabIndex = 14
        LogoutBtn.Text = "Log Out"
        LogoutBtn.UseVisualStyleBackColor = False
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = My.Resources.Resources.ed8bb44f_0265_4bb2_a677_771c15e67760
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(65, 76)
        PictureBox1.TabIndex = 18
        PictureBox1.TabStop = False
        ' 
        ' storeName1
        ' 
        storeName1.AutoSize = True
        storeName1.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        storeName1.ForeColor = Color.SeaGreen
        storeName1.Location = New Point(71, 50)
        storeName1.Name = "storeName1"
        storeName1.Size = New Size(129, 12)
        storeName1.TabIndex = 3
        storeName1.Text = "Drugstore and Medical Supplies"
        ' 
        ' storeName
        ' 
        storeName.AutoSize = True
        storeName.Font = New Font("Segoe UI", 18.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        storeName.ForeColor = Color.SeaGreen
        storeName.Location = New Point(71, 12)
        storeName.Name = "storeName"
        storeName.Size = New Size(124, 35)
        storeName.TabIndex = 2
        storeName.Text = "Cabrera's"
        ' 
        ' motherPanel
        ' 
        motherPanel.BackColor = SystemColors.Control
        motherPanel.Controls.Add(pnlDashboard)
        motherPanel.Controls.Add(pnlReports)
        motherPanel.Controls.Add(pnlInventoryStockReport)
        motherPanel.Controls.Add(pnlFinancialSalesReport)
        motherPanel.Controls.Add(pnlRetailSalesReport)
        motherPanel.Controls.Add(pnlExpensesReport)
        motherPanel.Controls.Add(pnlPurchasesReport)
        motherPanel.Controls.Add(pnlWholesaleSalesReport)
        motherPanel.Controls.Add(pnlEmployeesalesReport)
        motherPanel.Controls.Add(pnlAccounts)
        motherPanel.Dock = DockStyle.Fill
        motherPanel.Location = New Point(0, 0)
        motherPanel.Name = "motherPanel"
        motherPanel.Size = New Size(1264, 830)
        motherPanel.TabIndex = 2
        ' 
        ' pnlDashboard
        ' 
        pnlDashboard.Controls.Add(pnlHeaderDashboard)
        pnlDashboard.Controls.Add(TableLayoutPanel4)
        pnlDashboard.Controls.Add(TableLayoutPanel1)
        pnlDashboard.Controls.Add(TableLayoutPanel2)
        pnlDashboard.Controls.Add(TableLayoutPanel3)
        pnlDashboard.Dock = DockStyle.Fill
        pnlDashboard.Location = New Point(0, 0)
        pnlDashboard.Name = "pnlDashboard"
        pnlDashboard.Size = New Size(1264, 830)
        pnlDashboard.TabIndex = 1
        ' 
        ' pnlHeaderDashboard
        ' 
        pnlHeaderDashboard.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pnlHeaderDashboard.BackColor = SystemColors.ControlLightLight
        pnlHeaderDashboard.Controls.Add(usrName)
        pnlHeaderDashboard.Controls.Add(usrSuperAdmin)
        pnlHeaderDashboard.Controls.Add(hdrStore)
        pnlHeaderDashboard.Controls.Add(hdrDashboard)
        pnlHeaderDashboard.Location = New Point(219, 0)
        pnlHeaderDashboard.Name = "pnlHeaderDashboard"
        pnlHeaderDashboard.Size = New Size(1045, 76)
        pnlHeaderDashboard.TabIndex = 2
        ' 
        ' usrName
        ' 
        usrName.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        usrName.BackColor = SystemColors.ControlLightLight
        usrName.BorderStyle = BorderStyle.None
        usrName.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        usrName.Location = New Point(874, 26)
        usrName.Name = "usrName"
        usrName.Size = New Size(145, 16)
        usrName.TabIndex = 7
        usrName.Text = "Super Admin Name"
        usrName.TextAlign = HorizontalAlignment.Center
        ' 
        ' usrSuperAdmin
        ' 
        usrSuperAdmin.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        usrSuperAdmin.AutoSize = True
        usrSuperAdmin.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        usrSuperAdmin.ForeColor = Color.SeaGreen
        usrSuperAdmin.Location = New Point(901, 45)
        usrSuperAdmin.Name = "usrSuperAdmin"
        usrSuperAdmin.Size = New Size(91, 12)
        usrSuperAdmin.TabIndex = 5
        usrSuperAdmin.Text = "Super Admin / Owner"
        ' 
        ' hdrStore
        ' 
        hdrStore.AutoSize = True
        hdrStore.BackColor = Color.Transparent
        hdrStore.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        hdrStore.Location = New Point(51, 45)
        hdrStore.Name = "hdrStore"
        hdrStore.Size = New Size(241, 17)
        hdrStore.TabIndex = 1
        hdrStore.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' hdrDashboard
        ' 
        hdrDashboard.AutoSize = True
        hdrDashboard.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        hdrDashboard.ForeColor = Color.SeaGreen
        hdrDashboard.Location = New Point(51, 10)
        hdrDashboard.Name = "hdrDashboard"
        hdrDashboard.Size = New Size(157, 37)
        hdrDashboard.TabIndex = 0
        hdrDashboard.Text = "Dashboard"
        ' 
        ' TableLayoutPanel4
        ' 
        TableLayoutPanel4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel4.ColumnCount = 2
        TableLayoutPanel4.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel4.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0000076F))
        TableLayoutPanel4.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel4.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel4.Controls.Add(Panel11, 0, 0)
        TableLayoutPanel4.Controls.Add(Panel12, 1, 0)
        TableLayoutPanel4.Location = New Point(270, 698)
        TableLayoutPanel4.Name = "TableLayoutPanel4"
        TableLayoutPanel4.RowCount = 1
        TableLayoutPanel4.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel4.Size = New Size(940, 333)
        TableLayoutPanel4.TabIndex = 8
        ' 
        ' Panel11
        ' 
        Panel11.BackColor = SystemColors.ControlLightLight
        Panel11.Dock = DockStyle.Fill
        Panel11.Location = New Point(3, 3)
        Panel11.Name = "Panel11"
        Panel11.Size = New Size(463, 327)
        Panel11.TabIndex = 0
        ' 
        ' Panel12
        ' 
        Panel12.BackColor = SystemColors.ControlLightLight
        Panel12.Dock = DockStyle.Fill
        Panel12.Location = New Point(472, 3)
        Panel12.Name = "Panel12"
        Panel12.Size = New Size(465, 327)
        Panel12.TabIndex = 1
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel1.ColumnCount = 4
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006218F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006275F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006275F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 24.9981251F))
        TableLayoutPanel1.Controls.Add(Panel1, 0, 0)
        TableLayoutPanel1.Controls.Add(Panel2, 1, 0)
        TableLayoutPanel1.Controls.Add(Panel3, 2, 0)
        TableLayoutPanel1.Controls.Add(Panel4, 3, 0)
        TableLayoutPanel1.Location = New Point(270, 122)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.RowCount = 1
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(940, 117)
        TableLayoutPanel1.TabIndex = 3
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = SystemColors.ControlLightLight
        Panel1.Controls.Add(todaysSale)
        Panel1.Controls.Add(monthDateYear)
        Panel1.Controls.Add(userIDText)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(3, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(229, 111)
        Panel1.TabIndex = 0
        ' 
        ' todaysSale
        ' 
        todaysSale.BackColor = SystemColors.ControlLightLight
        todaysSale.BorderStyle = BorderStyle.None
        todaysSale.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        todaysSale.Location = New Point(67, 13)
        todaysSale.Name = "todaysSale"
        todaysSale.Size = New Size(112, 26)
        todaysSale.TabIndex = 4
        todaysSale.Text = "Today's Sale"
        ' 
        ' monthDateYear
        ' 
        monthDateYear.BackColor = SystemColors.ControlLightLight
        monthDateYear.BorderStyle = BorderStyle.None
        monthDateYear.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        monthDateYear.Location = New Point(67, 79)
        monthDateYear.Name = "monthDateYear"
        monthDateYear.Size = New Size(137, 18)
        monthDateYear.TabIndex = 3
        monthDateYear.Text = "Month, Date, Year"
        ' 
        ' userIDText
        ' 
        userIDText.BackColor = SystemColors.ControlLightLight
        userIDText.BorderStyle = BorderStyle.None
        userIDText.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        userIDText.Location = New Point(67, 45)
        userIDText.Name = "userIDText"
        userIDText.Size = New Size(100, 26)
        userIDText.TabIndex = 2
        userIDText.Text = "00"
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = SystemColors.ControlLightLight
        Panel2.Controls.Add(transactionRetail)
        Panel2.Controls.Add(TextBox3)
        Panel2.Controls.Add(TextBox2)
        Panel2.Dock = DockStyle.Fill
        Panel2.Location = New Point(238, 3)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(229, 111)
        Panel2.TabIndex = 1
        ' 
        ' transactionRetail
        ' 
        transactionRetail.BackColor = SystemColors.ControlLightLight
        transactionRetail.BorderStyle = BorderStyle.None
        transactionRetail.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        transactionRetail.Location = New Point(59, 79)
        transactionRetail.Name = "transactionRetail"
        transactionRetail.Size = New Size(137, 18)
        transactionRetail.TabIndex = 7
        transactionRetail.Text = "0 Transactions"
        ' 
        ' TextBox3
        ' 
        TextBox3.BackColor = SystemColors.ControlLightLight
        TextBox3.BorderStyle = BorderStyle.None
        TextBox3.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox3.Location = New Point(59, 45)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(100, 26)
        TextBox3.TabIndex = 5
        TextBox3.Text = "00"
        ' 
        ' TextBox2
        ' 
        TextBox2.BackColor = SystemColors.ControlLightLight
        TextBox2.BorderStyle = BorderStyle.None
        TextBox2.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox2.Location = New Point(59, 13)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(112, 26)
        TextBox2.TabIndex = 6
        TextBox2.Text = "Retail Sale"
        ' 
        ' Panel3
        ' 
        Panel3.BackColor = SystemColors.ControlLightLight
        Panel3.Controls.Add(transactionsWholesale)
        Panel3.Controls.Add(TextBox6)
        Panel3.Controls.Add(TextBox5)
        Panel3.Dock = DockStyle.Fill
        Panel3.Location = New Point(473, 3)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(229, 111)
        Panel3.TabIndex = 2
        ' 
        ' transactionsWholesale
        ' 
        transactionsWholesale.BackColor = SystemColors.ControlLightLight
        transactionsWholesale.BorderStyle = BorderStyle.None
        transactionsWholesale.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        transactionsWholesale.Location = New Point(58, 79)
        transactionsWholesale.Name = "transactionsWholesale"
        transactionsWholesale.Size = New Size(137, 18)
        transactionsWholesale.TabIndex = 8
        transactionsWholesale.Text = "0 Transactions"
        ' 
        ' TextBox6
        ' 
        TextBox6.BackColor = SystemColors.ControlLightLight
        TextBox6.BorderStyle = BorderStyle.None
        TextBox6.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox6.Location = New Point(58, 47)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(100, 26)
        TextBox6.TabIndex = 8
        TextBox6.Text = "00"
        ' 
        ' TextBox5
        ' 
        TextBox5.BackColor = SystemColors.ControlLightLight
        TextBox5.BorderStyle = BorderStyle.None
        TextBox5.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox5.Location = New Point(58, 13)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(133, 26)
        TextBox5.TabIndex = 7
        TextBox5.Text = "Wholesale Sale"
        ' 
        ' Panel4
        ' 
        Panel4.BackColor = SystemColors.ControlLightLight
        Panel4.Controls.Add(allCategories)
        Panel4.Controls.Add(TextBox15)
        Panel4.Controls.Add(TextBox14)
        Panel4.Dock = DockStyle.Fill
        Panel4.Location = New Point(708, 3)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(229, 111)
        Panel4.TabIndex = 3
        ' 
        ' allCategories
        ' 
        allCategories.BackColor = SystemColors.ControlLightLight
        allCategories.BorderStyle = BorderStyle.None
        allCategories.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        allCategories.Location = New Point(47, 79)
        allCategories.Name = "allCategories"
        allCategories.Size = New Size(137, 18)
        allCategories.TabIndex = 10
        allCategories.Text = "All categories"
        ' 
        ' TextBox15
        ' 
        TextBox15.BackColor = SystemColors.ControlLightLight
        TextBox15.BorderStyle = BorderStyle.None
        TextBox15.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox15.Location = New Point(47, 47)
        TextBox15.Name = "TextBox15"
        TextBox15.Size = New Size(100, 26)
        TextBox15.TabIndex = 9
        TextBox15.Text = "00"
        ' 
        ' TextBox14
        ' 
        TextBox14.BackColor = SystemColors.ControlLightLight
        TextBox14.BorderStyle = BorderStyle.None
        TextBox14.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox14.Location = New Point(47, 13)
        TextBox14.Name = "TextBox14"
        TextBox14.Size = New Size(133, 26)
        TextBox14.TabIndex = 8
        TextBox14.Text = "Wholesale Sale"
        ' 
        ' TableLayoutPanel2
        ' 
        TableLayoutPanel2.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel2.ColumnCount = 4
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006218F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006275F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0006275F))
        TableLayoutPanel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 24.9981251F))
        TableLayoutPanel2.Controls.Add(Panel5, 0, 0)
        TableLayoutPanel2.Controls.Add(Panel6, 1, 0)
        TableLayoutPanel2.Controls.Add(Panel7, 2, 0)
        TableLayoutPanel2.Controls.Add(Panel8, 3, 0)
        TableLayoutPanel2.Location = New Point(270, 242)
        TableLayoutPanel2.Name = "TableLayoutPanel2"
        TableLayoutPanel2.RowCount = 1
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel2.Size = New Size(940, 117)
        TableLayoutPanel2.TabIndex = 7
        ' 
        ' Panel5
        ' 
        Panel5.BackColor = SystemColors.ControlLightLight
        Panel5.Controls.Add(needOrdering)
        Panel5.Controls.Add(TextBox9)
        Panel5.Controls.Add(TextBox8)
        Panel5.Dock = DockStyle.Fill
        Panel5.Location = New Point(3, 3)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(229, 111)
        Panel5.TabIndex = 0
        ' 
        ' needOrdering
        ' 
        needOrdering.BackColor = SystemColors.ControlLightLight
        needOrdering.BorderStyle = BorderStyle.None
        needOrdering.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        needOrdering.Location = New Point(67, 81)
        needOrdering.Name = "needOrdering"
        needOrdering.Size = New Size(137, 18)
        needOrdering.TabIndex = 6
        needOrdering.Text = "Need reordering"
        ' 
        ' TextBox9
        ' 
        TextBox9.BackColor = SystemColors.ControlLightLight
        TextBox9.BorderStyle = BorderStyle.None
        TextBox9.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox9.Location = New Point(67, 49)
        TextBox9.Name = "TextBox9"
        TextBox9.Size = New Size(100, 26)
        TextBox9.TabIndex = 5
        TextBox9.Text = "00"
        ' 
        ' TextBox8
        ' 
        TextBox8.BackColor = SystemColors.ControlLightLight
        TextBox8.BorderStyle = BorderStyle.None
        TextBox8.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox8.Location = New Point(67, 17)
        TextBox8.Name = "TextBox8"
        TextBox8.Size = New Size(112, 26)
        TextBox8.TabIndex = 5
        TextBox8.Text = "Low Stock"
        ' 
        ' Panel6
        ' 
        Panel6.BackColor = SystemColors.ControlLightLight
        Panel6.Controls.Add(days)
        Panel6.Controls.Add(TextBox12)
        Panel6.Controls.Add(TextBox11)
        Panel6.Dock = DockStyle.Fill
        Panel6.Location = New Point(238, 3)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(229, 111)
        Panel6.TabIndex = 1
        ' 
        ' days
        ' 
        days.BackColor = SystemColors.ControlLightLight
        days.BorderStyle = BorderStyle.None
        days.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        days.Location = New Point(59, 81)
        days.Name = "days"
        days.Size = New Size(137, 18)
        days.TabIndex = 7
        days.Text = "Within 0 days"
        ' 
        ' TextBox12
        ' 
        TextBox12.BackColor = SystemColors.ControlLightLight
        TextBox12.BorderStyle = BorderStyle.None
        TextBox12.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox12.Location = New Point(59, 49)
        TextBox12.Name = "TextBox12"
        TextBox12.Size = New Size(100, 26)
        TextBox12.TabIndex = 6
        TextBox12.Text = "00"
        ' 
        ' TextBox11
        ' 
        TextBox11.BackColor = SystemColors.ControlLightLight
        TextBox11.BorderStyle = BorderStyle.None
        TextBox11.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox11.Location = New Point(59, 17)
        TextBox11.Name = "TextBox11"
        TextBox11.Size = New Size(112, 26)
        TextBox11.TabIndex = 6
        TextBox11.Text = "Expired"
        ' 
        ' Panel7
        ' 
        Panel7.BackColor = SystemColors.ControlLightLight
        Panel7.Controls.Add(requiresDisposal)
        Panel7.Controls.Add(TextBox19)
        Panel7.Controls.Add(TextBox17)
        Panel7.Dock = DockStyle.Fill
        Panel7.Location = New Point(473, 3)
        Panel7.Name = "Panel7"
        Panel7.Size = New Size(229, 111)
        Panel7.TabIndex = 2
        ' 
        ' requiresDisposal
        ' 
        requiresDisposal.BackColor = SystemColors.ControlLightLight
        requiresDisposal.BorderStyle = BorderStyle.None
        requiresDisposal.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        requiresDisposal.Location = New Point(58, 81)
        requiresDisposal.Name = "requiresDisposal"
        requiresDisposal.Size = New Size(137, 18)
        requiresDisposal.TabIndex = 10
        requiresDisposal.Text = "Requires disposal"
        ' 
        ' TextBox19
        ' 
        TextBox19.BackColor = SystemColors.ControlLightLight
        TextBox19.BorderStyle = BorderStyle.None
        TextBox19.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox19.Location = New Point(58, 49)
        TextBox19.Name = "TextBox19"
        TextBox19.Size = New Size(100, 26)
        TextBox19.TabIndex = 9
        TextBox19.Text = "00"
        ' 
        ' TextBox17
        ' 
        TextBox17.BackColor = SystemColors.ControlLightLight
        TextBox17.BorderStyle = BorderStyle.None
        TextBox17.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox17.Location = New Point(58, 17)
        TextBox17.Name = "TextBox17"
        TextBox17.Size = New Size(133, 26)
        TextBox17.TabIndex = 8
        TextBox17.Text = "Near Expiry"
        ' 
        ' Panel8
        ' 
        Panel8.BackColor = SystemColors.ControlLightLight
        Panel8.Controls.Add(expensesRecorded)
        Panel8.Controls.Add(TextBox20)
        Panel8.Controls.Add(TextBox18)
        Panel8.Dock = DockStyle.Fill
        Panel8.Location = New Point(708, 3)
        Panel8.Name = "Panel8"
        Panel8.Size = New Size(229, 111)
        Panel8.TabIndex = 3
        ' 
        ' expensesRecorded
        ' 
        expensesRecorded.BackColor = SystemColors.ControlLightLight
        expensesRecorded.BorderStyle = BorderStyle.None
        expensesRecorded.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        expensesRecorded.Location = New Point(47, 81)
        expensesRecorded.Name = "expensesRecorded"
        expensesRecorded.Size = New Size(166, 18)
        expensesRecorded.TabIndex = 11
        expensesRecorded.Text = "0 expenses recorded today"
        ' 
        ' TextBox20
        ' 
        TextBox20.BackColor = SystemColors.ControlLightLight
        TextBox20.BorderStyle = BorderStyle.None
        TextBox20.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox20.Location = New Point(47, 49)
        TextBox20.Name = "TextBox20"
        TextBox20.Size = New Size(100, 26)
        TextBox20.TabIndex = 10
        TextBox20.Text = "00"
        ' 
        ' TextBox18
        ' 
        TextBox18.BackColor = SystemColors.ControlLightLight
        TextBox18.BorderStyle = BorderStyle.None
        TextBox18.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox18.Location = New Point(47, 17)
        TextBox18.Name = "TextBox18"
        TextBox18.Size = New Size(144, 26)
        TextBox18.TabIndex = 9
        TextBox18.Text = "Today's Expenses"
        ' 
        ' TableLayoutPanel3
        ' 
        TableLayoutPanel3.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel3.ColumnCount = 2
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0000076F))
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel3.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel3.Controls.Add(Panel9, 0, 0)
        TableLayoutPanel3.Controls.Add(Panel10, 1, 0)
        TableLayoutPanel3.Location = New Point(270, 362)
        TableLayoutPanel3.Name = "TableLayoutPanel3"
        TableLayoutPanel3.RowCount = 1
        TableLayoutPanel3.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel3.Size = New Size(940, 333)
        TableLayoutPanel3.TabIndex = 5
        ' 
        ' Panel9
        ' 
        Panel9.BackColor = SystemColors.ControlLightLight
        Panel9.Dock = DockStyle.Fill
        Panel9.Location = New Point(3, 3)
        Panel9.Name = "Panel9"
        Panel9.Size = New Size(463, 327)
        Panel9.TabIndex = 0
        ' 
        ' Panel10
        ' 
        Panel10.BackColor = SystemColors.ControlLightLight
        Panel10.Dock = DockStyle.Fill
        Panel10.Location = New Point(472, 3)
        Panel10.Name = "Panel10"
        Panel10.Size = New Size(465, 327)
        Panel10.TabIndex = 1
        ' 
        ' pnlReports
        ' 
        pnlReports.Controls.Add(TableLayoutPanel9)
        pnlReports.Controls.Add(TableLayoutPanel7)
        pnlReports.Controls.Add(pnlHeaderReports)
        pnlReports.Controls.Add(Panel16)
        pnlReports.Controls.Add(TableLayoutPanel8)
        pnlReports.Controls.Add(TableLayoutPanel6)
        pnlReports.Dock = DockStyle.Fill
        pnlReports.Location = New Point(0, 0)
        pnlReports.Name = "pnlReports"
        pnlReports.Size = New Size(1264, 830)
        pnlReports.TabIndex = 10
        ' 
        ' TableLayoutPanel9
        ' 
        TableLayoutPanel9.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel9.ColumnCount = 2
        TableLayoutPanel9.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel9.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.00002F))
        TableLayoutPanel9.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel9.Controls.Add(Panel23, 0, 0)
        TableLayoutPanel9.Location = New Point(271, 599)
        TableLayoutPanel9.Name = "TableLayoutPanel9"
        TableLayoutPanel9.RowCount = 1
        TableLayoutPanel9.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel9.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel9.Size = New Size(940, 117)
        TableLayoutPanel9.TabIndex = 9
        ' 
        ' Panel23
        ' 
        Panel23.BackColor = SystemColors.ControlLightLight
        Panel23.Controls.Add(employeeViewReport)
        Panel23.Controls.Add(employeeDescription)
        Panel23.Controls.Add(employeeTitle)
        Panel23.Dock = DockStyle.Fill
        Panel23.Location = New Point(3, 3)
        Panel23.Name = "Panel23"
        Panel23.Size = New Size(463, 111)
        Panel23.TabIndex = 0
        ' 
        ' employeeViewReport
        ' 
        employeeViewReport.BackColor = Color.SeaGreen
        employeeViewReport.FlatAppearance.BorderSize = 0
        employeeViewReport.FlatStyle = FlatStyle.Flat
        employeeViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        employeeViewReport.ForeColor = SystemColors.ControlLightLight
        employeeViewReport.Location = New Point(66, 64)
        employeeViewReport.Name = "employeeViewReport"
        employeeViewReport.Size = New Size(175, 35)
        employeeViewReport.TabIndex = 14
        employeeViewReport.Text = "View Report"
        employeeViewReport.UseVisualStyleBackColor = False
        ' 
        ' employeeDescription
        ' 
        employeeDescription.BackColor = SystemColors.ControlLightLight
        employeeDescription.BorderStyle = BorderStyle.None
        employeeDescription.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        employeeDescription.ForeColor = SystemColors.ControlDarkDark
        employeeDescription.Location = New Point(66, 39)
        employeeDescription.Name = "employeeDescription"
        employeeDescription.Size = New Size(609, 22)
        employeeDescription.TabIndex = 7
        employeeDescription.Text = "Sales performance breakdown by cashier for accountability and monitoring."
        ' 
        ' employeeTitle
        ' 
        employeeTitle.BackColor = SystemColors.ControlLightLight
        employeeTitle.BorderStyle = BorderStyle.None
        employeeTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        employeeTitle.ForeColor = SystemColors.ActiveCaptionText
        employeeTitle.Location = New Point(67, 13)
        employeeTitle.Name = "employeeTitle"
        employeeTitle.Size = New Size(208, 26)
        employeeTitle.TabIndex = 4
        employeeTitle.Text = "Employee Sales Report"
        ' 
        ' TableLayoutPanel7
        ' 
        TableLayoutPanel7.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel7.ColumnCount = 2
        TableLayoutPanel7.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel7.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.00002F))
        TableLayoutPanel7.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel7.Controls.Add(Panel19, 0, 0)
        TableLayoutPanel7.Controls.Add(Panel22, 1, 0)
        TableLayoutPanel7.Location = New Point(270, 479)
        TableLayoutPanel7.Name = "TableLayoutPanel7"
        TableLayoutPanel7.RowCount = 1
        TableLayoutPanel7.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel7.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel7.Size = New Size(940, 117)
        TableLayoutPanel7.TabIndex = 8
        ' 
        ' Panel19
        ' 
        Panel19.BackColor = SystemColors.ControlLightLight
        Panel19.Controls.Add(expensesViewReport)
        Panel19.Controls.Add(expensesDescriptions)
        Panel19.Controls.Add(expensesTitle)
        Panel19.Dock = DockStyle.Fill
        Panel19.Location = New Point(3, 3)
        Panel19.Name = "Panel19"
        Panel19.Size = New Size(463, 111)
        Panel19.TabIndex = 0
        ' 
        ' expensesViewReport
        ' 
        expensesViewReport.BackColor = Color.SeaGreen
        expensesViewReport.FlatAppearance.BorderSize = 0
        expensesViewReport.FlatStyle = FlatStyle.Flat
        expensesViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        expensesViewReport.ForeColor = SystemColors.ControlLightLight
        expensesViewReport.Location = New Point(66, 64)
        expensesViewReport.Name = "expensesViewReport"
        expensesViewReport.Size = New Size(175, 35)
        expensesViewReport.TabIndex = 14
        expensesViewReport.Text = "View Report"
        expensesViewReport.UseVisualStyleBackColor = False
        ' 
        ' expensesDescriptions
        ' 
        expensesDescriptions.BackColor = SystemColors.ControlLightLight
        expensesDescriptions.BorderStyle = BorderStyle.None
        expensesDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        expensesDescriptions.ForeColor = SystemColors.ControlDarkDark
        expensesDescriptions.Location = New Point(0, 0)
        expensesDescriptions.Name = "expensesDescriptions"
        expensesDescriptions.Size = New Size(609, 22)
        expensesDescriptions.TabIndex = 7
        expensesDescriptions.Text = "All recorded business expenses categorized by type and date range."
        ' 
        ' expensesTitle
        ' 
        expensesTitle.BackColor = SystemColors.ControlLightLight
        expensesTitle.BorderStyle = BorderStyle.None
        expensesTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        expensesTitle.ForeColor = SystemColors.ActiveCaptionText
        expensesTitle.Location = New Point(67, 13)
        expensesTitle.Name = "expensesTitle"
        expensesTitle.Size = New Size(208, 26)
        expensesTitle.TabIndex = 4
        expensesTitle.Text = "Expenses Report"
        ' 
        ' Panel22
        ' 
        Panel22.BackColor = SystemColors.ControlLightLight
        Panel22.Controls.Add(financialViewReport)
        Panel22.Controls.Add(financialDescriptions)
        Panel22.Controls.Add(financialTitle)
        Panel22.Dock = DockStyle.Fill
        Panel22.Location = New Point(472, 3)
        Panel22.Name = "Panel22"
        Panel22.Size = New Size(465, 111)
        Panel22.TabIndex = 1
        ' 
        ' financialViewReport
        ' 
        financialViewReport.BackColor = Color.SeaGreen
        financialViewReport.FlatAppearance.BorderSize = 0
        financialViewReport.FlatStyle = FlatStyle.Flat
        financialViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        financialViewReport.ForeColor = SystemColors.ControlLightLight
        financialViewReport.Location = New Point(59, 64)
        financialViewReport.Name = "financialViewReport"
        financialViewReport.Size = New Size(175, 35)
        financialViewReport.TabIndex = 15
        financialViewReport.Text = "View Report"
        financialViewReport.UseVisualStyleBackColor = False
        ' 
        ' financialDescriptions
        ' 
        financialDescriptions.BackColor = SystemColors.ControlLightLight
        financialDescriptions.BorderStyle = BorderStyle.None
        financialDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        financialDescriptions.ForeColor = SystemColors.ControlDarkDark
        financialDescriptions.Location = New Point(58, 39)
        financialDescriptions.Name = "financialDescriptions"
        financialDescriptions.Size = New Size(609, 22)
        financialDescriptions.TabIndex = 15
        financialDescriptions.Text = "Revenue, cost of goods, expenses, and net profit summary."
        ' 
        ' financialTitle
        ' 
        financialTitle.BackColor = SystemColors.ControlLightLight
        financialTitle.BorderStyle = BorderStyle.None
        financialTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        financialTitle.ForeColor = SystemColors.ActiveCaptionText
        financialTitle.Location = New Point(59, 13)
        financialTitle.Name = "financialTitle"
        financialTitle.Size = New Size(177, 26)
        financialTitle.TabIndex = 6
        financialTitle.Text = "Financial Report"
        ' 
        ' pnlHeaderReports
        ' 
        pnlHeaderReports.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pnlHeaderReports.BackColor = SystemColors.ControlLightLight
        pnlHeaderReports.Controls.Add(TextBox24)
        pnlHeaderReports.Controls.Add(Label4)
        pnlHeaderReports.Controls.Add(Label5)
        pnlHeaderReports.Controls.Add(Label6)
        pnlHeaderReports.Location = New Point(219, 0)
        pnlHeaderReports.Name = "pnlHeaderReports"
        pnlHeaderReports.Size = New Size(1045, 76)
        pnlHeaderReports.TabIndex = 4
        ' 
        ' TextBox24
        ' 
        TextBox24.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox24.BackColor = SystemColors.ControlLightLight
        TextBox24.BorderStyle = BorderStyle.None
        TextBox24.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox24.Location = New Point(874, 26)
        TextBox24.Name = "TextBox24"
        TextBox24.Size = New Size(145, 16)
        TextBox24.TabIndex = 8
        TextBox24.Text = "Super Admin Name"
        TextBox24.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label4
        ' 
        Label4.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label4.ForeColor = Color.SeaGreen
        Label4.Location = New Point(901, 45)
        Label4.Name = "Label4"
        Label4.Size = New Size(91, 12)
        Label4.TabIndex = 9
        Label4.Text = "Super Admin / Owner"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = Color.Transparent
        Label5.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label5.Location = New Point(51, 45)
        Label5.Name = "Label5"
        Label5.Size = New Size(241, 17)
        Label5.TabIndex = 1
        Label5.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label6.ForeColor = Color.SeaGreen
        Label6.Location = New Point(51, 10)
        Label6.Name = "Label6"
        Label6.Size = New Size(134, 37)
        Label6.TabIndex = 0
        Label6.Text = "Accounts"
        ' 
        ' Panel16
        ' 
        Panel16.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel16.BackColor = SystemColors.ControlLightLight
        Panel16.Controls.Add(reportsDescription)
        Panel16.Controls.Add(titleBusinessReports)
        Panel16.Location = New Point(274, 122)
        Panel16.Name = "Panel16"
        Panel16.Size = New Size(934, 100)
        Panel16.TabIndex = 7
        ' 
        ' reportsDescription
        ' 
        reportsDescription.BackColor = SystemColors.ControlLightLight
        reportsDescription.BorderStyle = BorderStyle.None
        reportsDescription.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        reportsDescription.ForeColor = SystemColors.ControlDarkDark
        reportsDescription.Location = New Point(18, 53)
        reportsDescription.Name = "reportsDescription"
        reportsDescription.Size = New Size(391, 22)
        reportsDescription.TabIndex = 6
        reportsDescription.Text = "Select reports to view, generate or print."
        ' 
        ' titleBusinessReports
        ' 
        titleBusinessReports.BackColor = SystemColors.ControlLightLight
        titleBusinessReports.BorderStyle = BorderStyle.None
        titleBusinessReports.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleBusinessReports.ForeColor = SystemColors.ActiveCaptionText
        titleBusinessReports.Location = New Point(18, 22)
        titleBusinessReports.Name = "titleBusinessReports"
        titleBusinessReports.Size = New Size(194, 26)
        titleBusinessReports.TabIndex = 5
        titleBusinessReports.Text = "Business Reports"
        ' 
        ' TableLayoutPanel8
        ' 
        TableLayoutPanel8.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel8.ColumnCount = 2
        TableLayoutPanel8.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel8.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.00002F))
        TableLayoutPanel8.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel8.Controls.Add(Panel20, 0, 0)
        TableLayoutPanel8.Controls.Add(Panel21, 1, 0)
        TableLayoutPanel8.Location = New Point(271, 239)
        TableLayoutPanel8.Name = "TableLayoutPanel8"
        TableLayoutPanel8.RowCount = 1
        TableLayoutPanel8.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel8.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel8.Size = New Size(940, 117)
        TableLayoutPanel8.TabIndex = 6
        ' 
        ' Panel20
        ' 
        Panel20.BackColor = SystemColors.ControlLightLight
        Panel20.Controls.Add(inventoryViewReport)
        Panel20.Controls.Add(inventoryDescriptions)
        Panel20.Controls.Add(inventoryTitle)
        Panel20.Dock = DockStyle.Fill
        Panel20.Location = New Point(3, 3)
        Panel20.Name = "Panel20"
        Panel20.Size = New Size(463, 111)
        Panel20.TabIndex = 0
        ' 
        ' inventoryViewReport
        ' 
        inventoryViewReport.BackColor = Color.SeaGreen
        inventoryViewReport.FlatAppearance.BorderSize = 0
        inventoryViewReport.FlatStyle = FlatStyle.Flat
        inventoryViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        inventoryViewReport.ForeColor = SystemColors.ControlLightLight
        inventoryViewReport.Location = New Point(66, 64)
        inventoryViewReport.Name = "inventoryViewReport"
        inventoryViewReport.Size = New Size(175, 35)
        inventoryViewReport.TabIndex = 14
        inventoryViewReport.Text = "View Report"
        inventoryViewReport.UseVisualStyleBackColor = False
        ' 
        ' inventoryDescriptions
        ' 
        inventoryDescriptions.BackColor = SystemColors.ControlLightLight
        inventoryDescriptions.BorderStyle = BorderStyle.None
        inventoryDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        inventoryDescriptions.ForeColor = SystemColors.ControlDarkDark
        inventoryDescriptions.Location = New Point(66, 39)
        inventoryDescriptions.Name = "inventoryDescriptions"
        inventoryDescriptions.Size = New Size(609, 22)
        inventoryDescriptions.TabIndex = 7
        inventoryDescriptions.Text = "Complete list of all products with current stock levels, batch, and expiry information."
        ' 
        ' inventoryTitle
        ' 
        inventoryTitle.BackColor = SystemColors.ControlLightLight
        inventoryTitle.BorderStyle = BorderStyle.None
        inventoryTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        inventoryTitle.ForeColor = SystemColors.ActiveCaptionText
        inventoryTitle.Location = New Point(67, 13)
        inventoryTitle.Name = "inventoryTitle"
        inventoryTitle.Size = New Size(208, 26)
        inventoryTitle.TabIndex = 4
        inventoryTitle.Text = "Inventory Stock Report"
        ' 
        ' Panel21
        ' 
        Panel21.BackColor = SystemColors.ControlLightLight
        Panel21.Controls.Add(retailViewReport)
        Panel21.Controls.Add(retailDescriptions)
        Panel21.Controls.Add(retailTitle)
        Panel21.Dock = DockStyle.Fill
        Panel21.Location = New Point(472, 3)
        Panel21.Name = "Panel21"
        Panel21.Size = New Size(465, 111)
        Panel21.TabIndex = 1
        ' 
        ' retailViewReport
        ' 
        retailViewReport.BackColor = Color.SeaGreen
        retailViewReport.FlatAppearance.BorderSize = 0
        retailViewReport.FlatStyle = FlatStyle.Flat
        retailViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        retailViewReport.ForeColor = SystemColors.ControlLightLight
        retailViewReport.Location = New Point(59, 64)
        retailViewReport.Name = "retailViewReport"
        retailViewReport.Size = New Size(175, 35)
        retailViewReport.TabIndex = 15
        retailViewReport.Text = "View Report"
        retailViewReport.UseVisualStyleBackColor = False
        ' 
        ' retailDescriptions
        ' 
        retailDescriptions.BackColor = SystemColors.ControlLightLight
        retailDescriptions.BorderStyle = BorderStyle.None
        retailDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        retailDescriptions.ForeColor = SystemColors.ControlDarkDark
        retailDescriptions.Location = New Point(58, 39)
        retailDescriptions.Name = "retailDescriptions"
        retailDescriptions.Size = New Size(609, 22)
        retailDescriptions.TabIndex = 15
        retailDescriptions.Text = "Detailed breakdown of all retail transactions within a date range."
        ' 
        ' retailTitle
        ' 
        retailTitle.BackColor = SystemColors.ControlLightLight
        retailTitle.BorderStyle = BorderStyle.None
        retailTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        retailTitle.ForeColor = SystemColors.ActiveCaptionText
        retailTitle.Location = New Point(59, 13)
        retailTitle.Name = "retailTitle"
        retailTitle.Size = New Size(177, 26)
        retailTitle.TabIndex = 6
        retailTitle.Text = "Retail Sales Report"
        ' 
        ' TableLayoutPanel6
        ' 
        TableLayoutPanel6.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        TableLayoutPanel6.ColumnCount = 2
        TableLayoutPanel6.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 49.99999F))
        TableLayoutPanel6.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.00002F))
        TableLayoutPanel6.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        TableLayoutPanel6.Controls.Add(Panel17, 0, 0)
        TableLayoutPanel6.Controls.Add(Panel18, 1, 0)
        TableLayoutPanel6.Location = New Point(271, 359)
        TableLayoutPanel6.Name = "TableLayoutPanel6"
        TableLayoutPanel6.RowCount = 1
        TableLayoutPanel6.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        TableLayoutPanel6.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
        TableLayoutPanel6.Size = New Size(940, 117)
        TableLayoutPanel6.TabIndex = 7
        ' 
        ' Panel17
        ' 
        Panel17.BackColor = SystemColors.ControlLightLight
        Panel17.Controls.Add(wholesaleViewReport)
        Panel17.Controls.Add(wholesaleDescriptions)
        Panel17.Controls.Add(wholesaleTitle)
        Panel17.Dock = DockStyle.Fill
        Panel17.Location = New Point(3, 3)
        Panel17.Name = "Panel17"
        Panel17.Size = New Size(463, 111)
        Panel17.TabIndex = 0
        ' 
        ' wholesaleViewReport
        ' 
        wholesaleViewReport.BackColor = Color.SeaGreen
        wholesaleViewReport.FlatAppearance.BorderSize = 0
        wholesaleViewReport.FlatStyle = FlatStyle.Flat
        wholesaleViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        wholesaleViewReport.ForeColor = SystemColors.ControlLightLight
        wholesaleViewReport.Location = New Point(66, 64)
        wholesaleViewReport.Name = "wholesaleViewReport"
        wholesaleViewReport.Size = New Size(175, 35)
        wholesaleViewReport.TabIndex = 14
        wholesaleViewReport.Text = "View Report"
        wholesaleViewReport.UseVisualStyleBackColor = False
        ' 
        ' wholesaleDescriptions
        ' 
        wholesaleDescriptions.BackColor = SystemColors.ControlLightLight
        wholesaleDescriptions.BorderStyle = BorderStyle.None
        wholesaleDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wholesaleDescriptions.ForeColor = SystemColors.ControlDarkDark
        wholesaleDescriptions.Location = New Point(66, 39)
        wholesaleDescriptions.Name = "wholesaleDescriptions"
        wholesaleDescriptions.Size = New Size(609, 22)
        wholesaleDescriptions.TabIndex = 7
        wholesaleDescriptions.Text = "All wholesale transactions with customer, product, and total information."
        ' 
        ' wholesaleTitle
        ' 
        wholesaleTitle.BackColor = SystemColors.ControlLightLight
        wholesaleTitle.BorderStyle = BorderStyle.None
        wholesaleTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        wholesaleTitle.ForeColor = SystemColors.ActiveCaptionText
        wholesaleTitle.Location = New Point(67, 13)
        wholesaleTitle.Name = "wholesaleTitle"
        wholesaleTitle.Size = New Size(208, 26)
        wholesaleTitle.TabIndex = 4
        wholesaleTitle.Text = "Wholesales Report"
        ' 
        ' Panel18
        ' 
        Panel18.BackColor = SystemColors.ControlLightLight
        Panel18.Controls.Add(purchasesViewReport)
        Panel18.Controls.Add(purchasesDescriptions)
        Panel18.Controls.Add(purchasesTitle)
        Panel18.Dock = DockStyle.Fill
        Panel18.Location = New Point(472, 3)
        Panel18.Name = "Panel18"
        Panel18.Size = New Size(465, 111)
        Panel18.TabIndex = 1
        ' 
        ' purchasesViewReport
        ' 
        purchasesViewReport.BackColor = Color.SeaGreen
        purchasesViewReport.FlatAppearance.BorderSize = 0
        purchasesViewReport.FlatStyle = FlatStyle.Flat
        purchasesViewReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        purchasesViewReport.ForeColor = SystemColors.ControlLightLight
        purchasesViewReport.Location = New Point(59, 64)
        purchasesViewReport.Name = "purchasesViewReport"
        purchasesViewReport.Size = New Size(175, 35)
        purchasesViewReport.TabIndex = 15
        purchasesViewReport.Text = "View Report"
        purchasesViewReport.UseVisualStyleBackColor = False
        ' 
        ' purchasesDescriptions
        ' 
        purchasesDescriptions.BackColor = SystemColors.ControlLightLight
        purchasesDescriptions.BorderStyle = BorderStyle.None
        purchasesDescriptions.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        purchasesDescriptions.ForeColor = SystemColors.ControlDarkDark
        purchasesDescriptions.Location = New Point(58, 39)
        purchasesDescriptions.Name = "purchasesDescriptions"
        purchasesDescriptions.Size = New Size(609, 22)
        purchasesDescriptions.TabIndex = 15
        purchasesDescriptions.Text = "All product deliveries and restocking events with supplier and cost details."
        ' 
        ' purchasesTitle
        ' 
        purchasesTitle.BackColor = SystemColors.ControlLightLight
        purchasesTitle.BorderStyle = BorderStyle.None
        purchasesTitle.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        purchasesTitle.ForeColor = SystemColors.ActiveCaptionText
        purchasesTitle.Location = New Point(59, 13)
        purchasesTitle.Name = "purchasesTitle"
        purchasesTitle.Size = New Size(177, 26)
        purchasesTitle.TabIndex = 6
        purchasesTitle.Text = "Purchases Report"
        ' 
        ' pnlInventoryStockReport
        ' 
        pnlInventoryStockReport.Controls.Add(inventoryDataGrid)
        pnlInventoryStockReport.Controls.Add(panelnvenotryStockReport)
        pnlInventoryStockReport.Controls.Add(Panel25)
        pnlInventoryStockReport.Dock = DockStyle.Fill
        pnlInventoryStockReport.Location = New Point(0, 0)
        pnlInventoryStockReport.Name = "pnlInventoryStockReport"
        pnlInventoryStockReport.Size = New Size(1264, 830)
        pnlInventoryStockReport.TabIndex = 10
        ' 
        ' inventoryDataGrid
        ' 
        inventoryDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        inventoryDataGrid.BackgroundColor = SystemColors.ControlLightLight
        inventoryDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        inventoryDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        inventoryDataGrid.Columns.AddRange(New DataGridViewColumn() {DataGridViewTextBoxColumn1, inventoryCategory, inventoryUnit, inventoryStock, inventoryPackage, inventoryBatch, inventoryExpiry, inventorySupplier, inventoryStatus})
        inventoryDataGrid.Location = New Point(273, 248)
        inventoryDataGrid.Name = "inventoryDataGrid"
        inventoryDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        inventoryDataGrid.Size = New Size(940, 89)
        inventoryDataGrid.TabIndex = 15
        ' 
        ' DataGridViewTextBoxColumn1
        ' 
        DataGridViewTextBoxColumn1.HeaderText = "PRODUCT"
        DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        ' 
        ' inventoryCategory
        ' 
        inventoryCategory.HeaderText = "CATEGORY"
        inventoryCategory.Name = "inventoryCategory"
        ' 
        ' inventoryUnit
        ' 
        inventoryUnit.HeaderText = "UNIT"
        inventoryUnit.Name = "inventoryUnit"
        ' 
        ' inventoryStock
        ' 
        inventoryStock.HeaderText = "STOCK"
        inventoryStock.Name = "inventoryStock"
        ' 
        ' inventoryPackage
        ' 
        inventoryPackage.HeaderText = "PACKAGE"
        inventoryPackage.Name = "inventoryPackage"
        ' 
        ' inventoryBatch
        ' 
        inventoryBatch.HeaderText = "BATCH"
        inventoryBatch.Name = "inventoryBatch"
        ' 
        ' inventoryExpiry
        ' 
        inventoryExpiry.HeaderText = "EXPIRY"
        inventoryExpiry.Name = "inventoryExpiry"
        ' 
        ' inventorySupplier
        ' 
        inventorySupplier.HeaderText = "SUPPLIER"
        inventorySupplier.Name = "inventorySupplier"
        ' 
        ' inventoryStatus
        ' 
        inventoryStatus.HeaderText = "STATUS"
        inventoryStatus.Name = "inventoryStatus"
        ' 
        ' panelnvenotryStockReport
        ' 
        panelnvenotryStockReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelnvenotryStockReport.BackColor = SystemColors.ControlLightLight
        panelnvenotryStockReport.Controls.Add(inventoryPeriodDropdown)
        panelnvenotryStockReport.Controls.Add(inventoryPrintReport)
        panelnvenotryStockReport.Controls.Add(inventoryGenerated)
        panelnvenotryStockReport.Controls.Add(titleInventoryStockReport)
        panelnvenotryStockReport.Location = New Point(274, 122)
        panelnvenotryStockReport.Name = "panelnvenotryStockReport"
        panelnvenotryStockReport.Size = New Size(934, 100)
        panelnvenotryStockReport.TabIndex = 8
        ' 
        ' inventoryPeriodDropdown
        ' 
        inventoryPeriodDropdown.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        inventoryPeriodDropdown.FormattingEnabled = True
        inventoryPeriodDropdown.Location = New Point(742, 53)
        inventoryPeriodDropdown.Name = "inventoryPeriodDropdown"
        inventoryPeriodDropdown.Size = New Size(175, 36)
        inventoryPeriodDropdown.TabIndex = 19
        ' 
        ' inventoryPrintReport
        ' 
        inventoryPrintReport.BackColor = Color.SeaGreen
        inventoryPrintReport.FlatAppearance.BorderSize = 0
        inventoryPrintReport.FlatStyle = FlatStyle.Flat
        inventoryPrintReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        inventoryPrintReport.ForeColor = SystemColors.ControlLightLight
        inventoryPrintReport.Location = New Point(742, 12)
        inventoryPrintReport.Name = "inventoryPrintReport"
        inventoryPrintReport.Size = New Size(175, 35)
        inventoryPrintReport.TabIndex = 18
        inventoryPrintReport.Text = "Print Report"
        inventoryPrintReport.UseVisualStyleBackColor = False
        ' 
        ' inventoryGenerated
        ' 
        inventoryGenerated.BackColor = SystemColors.ControlLightLight
        inventoryGenerated.BorderStyle = BorderStyle.None
        inventoryGenerated.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        inventoryGenerated.ForeColor = SystemColors.ControlDarkDark
        inventoryGenerated.Location = New Point(18, 53)
        inventoryGenerated.Name = "inventoryGenerated"
        inventoryGenerated.Size = New Size(391, 22)
        inventoryGenerated.TabIndex = 6
        inventoryGenerated.Text = "Generated: Month, Date, Year, Time | By: Name" & vbCrLf & vbCrLf
        ' 
        ' titleInventoryStockReport
        ' 
        titleInventoryStockReport.BackColor = SystemColors.ControlLightLight
        titleInventoryStockReport.BorderStyle = BorderStyle.None
        titleInventoryStockReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleInventoryStockReport.ForeColor = SystemColors.ActiveCaptionText
        titleInventoryStockReport.Location = New Point(18, 22)
        titleInventoryStockReport.Name = "titleInventoryStockReport"
        titleInventoryStockReport.Size = New Size(223, 26)
        titleInventoryStockReport.TabIndex = 5
        titleInventoryStockReport.Text = "Inventory Stock Report"
        ' 
        ' Panel25
        ' 
        Panel25.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel25.BackColor = SystemColors.ControlLightLight
        Panel25.Controls.Add(Label7)
        Panel25.Controls.Add(TextBox50)
        Panel25.Controls.Add(Label8)
        Panel25.Controls.Add(Label9)
        Panel25.Location = New Point(219, 0)
        Panel25.Name = "Panel25"
        Panel25.Size = New Size(1045, 76)
        Panel25.TabIndex = 5
        ' 
        ' Label7
        ' 
        Label7.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label7.ForeColor = Color.SeaGreen
        Label7.Location = New Point(900, 45)
        Label7.Name = "Label7"
        Label7.Size = New Size(91, 12)
        Label7.TabIndex = 10
        Label7.Text = "Super Admin / Owner"
        ' 
        ' TextBox50
        ' 
        TextBox50.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox50.BackColor = SystemColors.ControlLightLight
        TextBox50.BorderStyle = BorderStyle.None
        TextBox50.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox50.Location = New Point(874, 26)
        TextBox50.Name = "TextBox50"
        TextBox50.Size = New Size(145, 16)
        TextBox50.TabIndex = 9
        TextBox50.Text = "Super Admin Name"
        TextBox50.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.BackColor = Color.Transparent
        Label8.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label8.Location = New Point(51, 45)
        Label8.Name = "Label8"
        Label8.Size = New Size(241, 17)
        Label8.TabIndex = 1
        Label8.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label9.ForeColor = Color.SeaGreen
        Label9.Location = New Point(51, 10)
        Label9.Name = "Label9"
        Label9.Size = New Size(118, 37)
        Label9.TabIndex = 0
        Label9.Text = "Reports"
        ' 
        ' pnlFinancialSalesReport
        ' 
        pnlFinancialSalesReport.Controls.Add(DataGridView4)
        pnlFinancialSalesReport.Controls.Add(panelFinancialSalesReport)
        pnlFinancialSalesReport.Controls.Add(Panel31)
        pnlFinancialSalesReport.Dock = DockStyle.Fill
        pnlFinancialSalesReport.Location = New Point(0, 0)
        pnlFinancialSalesReport.Name = "pnlFinancialSalesReport"
        pnlFinancialSalesReport.Size = New Size(1264, 830)
        pnlFinancialSalesReport.TabIndex = 18
        ' 
        ' DataGridView4
        ' 
        DataGridView4.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        DataGridView4.BackgroundColor = SystemColors.ControlLightLight
        DataGridView4.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        DataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView4.Columns.AddRange(New DataGridViewColumn() {financialTransactionID, financialDate, financialCustomer, financialCashier, financialType, financialTotal, financialStatus})
        DataGridView4.Location = New Point(271, 248)
        DataGridView4.Name = "DataGridView4"
        DataGridView4.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        DataGridView4.Size = New Size(934, 100)
        DataGridView4.TabIndex = 16
        ' 
        ' financialTransactionID
        ' 
        financialTransactionID.HeaderText = "TRANSACTION ID"
        financialTransactionID.Name = "financialTransactionID"
        ' 
        ' financialDate
        ' 
        financialDate.HeaderText = "DATE"
        financialDate.Name = "financialDate"
        ' 
        ' financialCustomer
        ' 
        financialCustomer.HeaderText = "CUSTOMER"
        financialCustomer.Name = "financialCustomer"
        ' 
        ' financialCashier
        ' 
        financialCashier.HeaderText = "CASHIER"
        financialCashier.Name = "financialCashier"
        ' 
        ' financialType
        ' 
        financialType.HeaderText = "TYPE"
        financialType.Name = "financialType"
        ' 
        ' financialTotal
        ' 
        financialTotal.HeaderText = "TOTAL"
        financialTotal.Name = "financialTotal"
        ' 
        ' financialStatus
        ' 
        financialStatus.HeaderText = "STATUS"
        financialStatus.Name = "financialStatus"
        ' 
        ' panelFinancialSalesReport
        ' 
        panelFinancialSalesReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelFinancialSalesReport.BackColor = SystemColors.ControlLightLight
        panelFinancialSalesReport.Controls.Add(financialPeriodDropdown)
        panelFinancialSalesReport.Controls.Add(financialPrintReport)
        panelFinancialSalesReport.Controls.Add(financialGenerated)
        panelFinancialSalesReport.Controls.Add(titleFinancialSalesReport)
        panelFinancialSalesReport.Location = New Point(274, 122)
        panelFinancialSalesReport.Name = "panelFinancialSalesReport"
        panelFinancialSalesReport.Size = New Size(934, 100)
        panelFinancialSalesReport.TabIndex = 9
        ' 
        ' financialPeriodDropdown
        ' 
        financialPeriodDropdown.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        financialPeriodDropdown.FormattingEnabled = True
        financialPeriodDropdown.Location = New Point(742, 53)
        financialPeriodDropdown.Name = "financialPeriodDropdown"
        financialPeriodDropdown.Size = New Size(175, 36)
        financialPeriodDropdown.TabIndex = 17
        ' 
        ' financialPrintReport
        ' 
        financialPrintReport.BackColor = Color.SeaGreen
        financialPrintReport.FlatAppearance.BorderSize = 0
        financialPrintReport.FlatStyle = FlatStyle.Flat
        financialPrintReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        financialPrintReport.ForeColor = SystemColors.ControlLightLight
        financialPrintReport.Location = New Point(742, 12)
        financialPrintReport.Name = "financialPrintReport"
        financialPrintReport.Size = New Size(175, 35)
        financialPrintReport.TabIndex = 16
        financialPrintReport.Text = "Print Report"
        financialPrintReport.UseVisualStyleBackColor = False
        ' 
        ' financialGenerated
        ' 
        financialGenerated.BackColor = SystemColors.ControlLightLight
        financialGenerated.BorderStyle = BorderStyle.None
        financialGenerated.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        financialGenerated.ForeColor = SystemColors.ControlDarkDark
        financialGenerated.Location = New Point(18, 53)
        financialGenerated.Name = "financialGenerated"
        financialGenerated.Size = New Size(391, 22)
        financialGenerated.TabIndex = 6
        financialGenerated.Text = "Generated: Month, Date, Year, Time | By: Name" & vbCrLf & vbCrLf
        ' 
        ' titleFinancialSalesReport
        ' 
        titleFinancialSalesReport.BackColor = SystemColors.ControlLightLight
        titleFinancialSalesReport.BorderStyle = BorderStyle.None
        titleFinancialSalesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleFinancialSalesReport.ForeColor = SystemColors.ActiveCaptionText
        titleFinancialSalesReport.Location = New Point(18, 22)
        titleFinancialSalesReport.Name = "titleFinancialSalesReport"
        titleFinancialSalesReport.Size = New Size(223, 26)
        titleFinancialSalesReport.TabIndex = 5
        titleFinancialSalesReport.Text = "Financial Sales Report"
        ' 
        ' Panel31
        ' 
        Panel31.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel31.BackColor = SystemColors.ControlLightLight
        Panel31.Controls.Add(Label17)
        Panel31.Controls.Add(Label18)
        Panel31.Location = New Point(219, 0)
        Panel31.Name = "Panel31"
        Panel31.Size = New Size(1045, 76)
        Panel31.TabIndex = 4
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.BackColor = Color.Transparent
        Label17.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label17.Location = New Point(51, 45)
        Label17.Name = "Label17"
        Label17.Size = New Size(241, 17)
        Label17.TabIndex = 1
        Label17.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label18.ForeColor = Color.SeaGreen
        Label18.Location = New Point(51, 10)
        Label18.Name = "Label18"
        Label18.Size = New Size(118, 37)
        Label18.TabIndex = 0
        Label18.Text = "Reports"
        ' 
        ' pnlRetailSalesReport
        ' 
        pnlRetailSalesReport.Controls.Add(retailDataGrid)
        pnlRetailSalesReport.Controls.Add(panelRetailSalesReport)
        pnlRetailSalesReport.Controls.Add(Panel24)
        pnlRetailSalesReport.Dock = DockStyle.Fill
        pnlRetailSalesReport.Location = New Point(0, 0)
        pnlRetailSalesReport.Name = "pnlRetailSalesReport"
        pnlRetailSalesReport.Size = New Size(1264, 830)
        pnlRetailSalesReport.TabIndex = 16
        ' 
        ' retailDataGrid
        ' 
        retailDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        retailDataGrid.BackgroundColor = SystemColors.ControlLightLight
        retailDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        retailDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        retailDataGrid.Columns.AddRange(New DataGridViewColumn() {retailTransactionID, retailDate, retailCustomer, retailCashier, retailType, retailTotal, retailStatus})
        retailDataGrid.Location = New Point(271, 248)
        retailDataGrid.Name = "retailDataGrid"
        retailDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        retailDataGrid.Size = New Size(940, 89)
        retailDataGrid.TabIndex = 16
        ' 
        ' retailTransactionID
        ' 
        retailTransactionID.HeaderText = "TRANSACTION ID"
        retailTransactionID.Name = "retailTransactionID"
        ' 
        ' retailDate
        ' 
        retailDate.HeaderText = "DATE"
        retailDate.Name = "retailDate"
        ' 
        ' retailCustomer
        ' 
        retailCustomer.HeaderText = "CUSTOMER"
        retailCustomer.Name = "retailCustomer"
        ' 
        ' retailCashier
        ' 
        retailCashier.HeaderText = "CASHIER"
        retailCashier.Name = "retailCashier"
        ' 
        ' retailType
        ' 
        retailType.HeaderText = "TYPE"
        retailType.Name = "retailType"
        ' 
        ' retailTotal
        ' 
        retailTotal.HeaderText = "TOTAL"
        retailTotal.Name = "retailTotal"
        ' 
        ' retailStatus
        ' 
        retailStatus.HeaderText = "STATUS"
        retailStatus.Name = "retailStatus"
        ' 
        ' panelRetailSalesReport
        ' 
        panelRetailSalesReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelRetailSalesReport.BackColor = SystemColors.ControlLightLight
        panelRetailSalesReport.Controls.Add(ComboBox4)
        panelRetailSalesReport.Controls.Add(retailPrintReport)
        panelRetailSalesReport.Controls.Add(retailGenerated)
        panelRetailSalesReport.Controls.Add(titleRetailSalesReport)
        panelRetailSalesReport.Location = New Point(274, 122)
        panelRetailSalesReport.Name = "panelRetailSalesReport"
        panelRetailSalesReport.Size = New Size(934, 100)
        panelRetailSalesReport.TabIndex = 9
        ' 
        ' ComboBox4
        ' 
        ComboBox4.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ComboBox4.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ComboBox4.FormattingEnabled = True
        ComboBox4.Location = New Point(742, 54)
        ComboBox4.Name = "ComboBox4"
        ComboBox4.Size = New Size(175, 36)
        ComboBox4.TabIndex = 19
        ' 
        ' retailPrintReport
        ' 
        retailPrintReport.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        retailPrintReport.BackColor = Color.SeaGreen
        retailPrintReport.FlatAppearance.BorderSize = 0
        retailPrintReport.FlatStyle = FlatStyle.Flat
        retailPrintReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        retailPrintReport.ForeColor = SystemColors.ControlLightLight
        retailPrintReport.Location = New Point(742, 13)
        retailPrintReport.Name = "retailPrintReport"
        retailPrintReport.Size = New Size(175, 35)
        retailPrintReport.TabIndex = 18
        retailPrintReport.Text = "Print Report"
        retailPrintReport.UseVisualStyleBackColor = False
        ' 
        ' retailGenerated
        ' 
        retailGenerated.BackColor = SystemColors.ControlLightLight
        retailGenerated.BorderStyle = BorderStyle.None
        retailGenerated.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        retailGenerated.ForeColor = SystemColors.ControlDarkDark
        retailGenerated.Location = New Point(18, 53)
        retailGenerated.Name = "retailGenerated"
        retailGenerated.Size = New Size(391, 22)
        retailGenerated.TabIndex = 6
        retailGenerated.Text = "Generated: Month, Date, Year, Time | By: Name" & vbCrLf & vbCrLf
        ' 
        ' titleRetailSalesReport
        ' 
        titleRetailSalesReport.BackColor = SystemColors.ControlLightLight
        titleRetailSalesReport.BorderStyle = BorderStyle.None
        titleRetailSalesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleRetailSalesReport.ForeColor = SystemColors.ActiveCaptionText
        titleRetailSalesReport.Location = New Point(18, 22)
        titleRetailSalesReport.Name = "titleRetailSalesReport"
        titleRetailSalesReport.Size = New Size(177, 26)
        titleRetailSalesReport.TabIndex = 5
        titleRetailSalesReport.Text = "Retail Sales Report"
        ' 
        ' Panel24
        ' 
        Panel24.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel24.BackColor = SystemColors.ControlLightLight
        Panel24.Controls.Add(Label10)
        Panel24.Controls.Add(TextBox48)
        Panel24.Controls.Add(Label11)
        Panel24.Controls.Add(Label12)
        Panel24.Location = New Point(219, 0)
        Panel24.Name = "Panel24"
        Panel24.Size = New Size(1045, 76)
        Panel24.TabIndex = 4
        ' 
        ' Label10
        ' 
        Label10.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label10.AutoSize = True
        Label10.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label10.ForeColor = Color.SeaGreen
        Label10.Location = New Point(900, 45)
        Label10.Name = "Label10"
        Label10.Size = New Size(91, 12)
        Label10.TabIndex = 12
        Label10.Text = "Super Admin / Owner"
        ' 
        ' TextBox48
        ' 
        TextBox48.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox48.BackColor = SystemColors.ControlLightLight
        TextBox48.BorderStyle = BorderStyle.None
        TextBox48.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox48.Location = New Point(874, 26)
        TextBox48.Name = "TextBox48"
        TextBox48.Size = New Size(145, 16)
        TextBox48.TabIndex = 11
        TextBox48.Text = "Super Admin Name"
        TextBox48.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.BackColor = Color.Transparent
        Label11.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label11.Location = New Point(51, 45)
        Label11.Name = "Label11"
        Label11.Size = New Size(241, 17)
        Label11.TabIndex = 1
        Label11.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label12.ForeColor = Color.SeaGreen
        Label12.Location = New Point(51, 10)
        Label12.Name = "Label12"
        Label12.Size = New Size(118, 37)
        Label12.TabIndex = 0
        Label12.Text = "Reports"
        ' 
        ' pnlExpensesReport
        ' 
        pnlExpensesReport.Controls.Add(expenseDataGrid)
        pnlExpensesReport.Controls.Add(panelExpenseReport)
        pnlExpensesReport.Controls.Add(Panel35)
        pnlExpensesReport.Dock = DockStyle.Fill
        pnlExpensesReport.Location = New Point(0, 0)
        pnlExpensesReport.Name = "pnlExpensesReport"
        pnlExpensesReport.Size = New Size(1264, 830)
        pnlExpensesReport.TabIndex = 20
        ' 
        ' expenseDataGrid
        ' 
        expenseDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        expenseDataGrid.BackgroundColor = SystemColors.ControlLightLight
        expenseDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        expenseDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        expenseDataGrid.Columns.AddRange(New DataGridViewColumn() {expensesExpenseID, expensesDate, expensesCategory, expensesDescription, expensesAmount, expensesRecordedBy})
        expenseDataGrid.Location = New Point(271, 248)
        expenseDataGrid.Name = "expenseDataGrid"
        expenseDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        expenseDataGrid.Size = New Size(934, 100)
        expenseDataGrid.TabIndex = 16
        ' 
        ' expensesExpenseID
        ' 
        expensesExpenseID.HeaderText = "EXPENSE ID"
        expensesExpenseID.Name = "expensesExpenseID"
        ' 
        ' expensesDate
        ' 
        expensesDate.HeaderText = "DATE"
        expensesDate.Name = "expensesDate"
        ' 
        ' expensesCategory
        ' 
        expensesCategory.HeaderText = "CATEGORY"
        expensesCategory.Name = "expensesCategory"
        ' 
        ' expensesDescription
        ' 
        expensesDescription.HeaderText = "DESCRIPTION"
        expensesDescription.Name = "expensesDescription"
        ' 
        ' expensesAmount
        ' 
        expensesAmount.HeaderText = "AMOUNT"
        expensesAmount.Name = "expensesAmount"
        ' 
        ' expensesRecordedBy
        ' 
        expensesRecordedBy.HeaderText = "RECORDED BY"
        expensesRecordedBy.Name = "expensesRecordedBy"
        ' 
        ' panelExpenseReport
        ' 
        panelExpenseReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelExpenseReport.BackColor = SystemColors.ControlLightLight
        panelExpenseReport.Controls.Add(expensesPeriodDropdown)
        panelExpenseReport.Controls.Add(btnAddExpenses)
        panelExpenseReport.Controls.Add(expensesDateTime)
        panelExpenseReport.Controls.Add(ComboBox6)
        panelExpenseReport.Controls.Add(Button15)
        panelExpenseReport.Controls.Add(expensesSearchBar)
        panelExpenseReport.Controls.Add(titleExpensesReport)
        panelExpenseReport.Location = New Point(274, 122)
        panelExpenseReport.Name = "panelExpenseReport"
        panelExpenseReport.Size = New Size(934, 100)
        panelExpenseReport.TabIndex = 9
        ' 
        ' expensesPeriodDropdown
        ' 
        expensesPeriodDropdown.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        expensesPeriodDropdown.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        expensesPeriodDropdown.FormattingEnabled = True
        expensesPeriodDropdown.Location = New Point(515, 52)
        expensesPeriodDropdown.Name = "expensesPeriodDropdown"
        expensesPeriodDropdown.Size = New Size(121, 28)
        expensesPeriodDropdown.TabIndex = 22
        ' 
        ' btnAddExpenses
        ' 
        btnAddExpenses.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAddExpenses.BackColor = Color.SeaGreen
        btnAddExpenses.FlatAppearance.BorderSize = 0
        btnAddExpenses.FlatStyle = FlatStyle.Flat
        btnAddExpenses.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAddExpenses.ForeColor = SystemColors.ControlLightLight
        btnAddExpenses.Location = New Point(742, 12)
        btnAddExpenses.Name = "btnAddExpenses"
        btnAddExpenses.Size = New Size(175, 35)
        btnAddExpenses.TabIndex = 21
        btnAddExpenses.Text = "     Add Expenses"
        btnAddExpenses.UseVisualStyleBackColor = False
        ' 
        ' expensesDateTime
        ' 
        expensesDateTime.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        expensesDateTime.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        expensesDateTime.Location = New Point(654, 53)
        expensesDateTime.Name = "expensesDateTime"
        expensesDateTime.Size = New Size(263, 27)
        expensesDateTime.TabIndex = 20
        ' 
        ' ComboBox6
        ' 
        ComboBox6.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ComboBox6.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ComboBox6.FormattingEnabled = True
        ComboBox6.Location = New Point(2210, 54)
        ComboBox6.Name = "ComboBox6"
        ComboBox6.Size = New Size(175, 36)
        ComboBox6.TabIndex = 19
        ' 
        ' Button15
        ' 
        Button15.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button15.BackColor = Color.SeaGreen
        Button15.FlatAppearance.BorderSize = 0
        Button15.FlatStyle = FlatStyle.Flat
        Button15.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button15.ForeColor = SystemColors.ControlLightLight
        Button15.Location = New Point(2210, 13)
        Button15.Name = "Button15"
        Button15.Size = New Size(175, 35)
        Button15.TabIndex = 18
        Button15.Text = "Print Report"
        Button15.UseVisualStyleBackColor = False
        ' 
        ' expensesSearchBar
        ' 
        expensesSearchBar.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        expensesSearchBar.BackColor = SystemColors.ControlLight
        expensesSearchBar.BorderStyle = BorderStyle.None
        expensesSearchBar.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        expensesSearchBar.ForeColor = SystemColors.ControlDarkDark
        expensesSearchBar.Location = New Point(18, 53)
        expensesSearchBar.Name = "expensesSearchBar"
        expensesSearchBar.Size = New Size(477, 26)
        expensesSearchBar.TabIndex = 6
        expensesSearchBar.Text = "   Search by delivery ID or supplier..."
        ' 
        ' titleExpensesReport
        ' 
        titleExpensesReport.BackColor = SystemColors.ControlLightLight
        titleExpensesReport.BorderStyle = BorderStyle.None
        titleExpensesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleExpensesReport.ForeColor = SystemColors.ActiveCaptionText
        titleExpensesReport.Location = New Point(18, 22)
        titleExpensesReport.Name = "titleExpensesReport"
        titleExpensesReport.Size = New Size(223, 26)
        titleExpensesReport.TabIndex = 5
        titleExpensesReport.Text = "Expenses Report"
        ' 
        ' Panel35
        ' 
        Panel35.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel35.BackColor = SystemColors.ControlLightLight
        Panel35.Controls.Add(TextBox66)
        Panel35.Controls.Add(Label27)
        Panel35.Controls.Add(TextBox64)
        Panel35.Controls.Add(Label23)
        Panel35.Controls.Add(Label24)
        Panel35.Controls.Add(TextBox65)
        Panel35.Controls.Add(Label25)
        Panel35.Controls.Add(Label26)
        Panel35.Location = New Point(219, 0)
        Panel35.Name = "Panel35"
        Panel35.Size = New Size(1045, 76)
        Panel35.TabIndex = 4
        ' 
        ' TextBox66
        ' 
        TextBox66.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox66.BackColor = SystemColors.ControlLightLight
        TextBox66.BorderStyle = BorderStyle.None
        TextBox66.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox66.Location = New Point(874, 26)
        TextBox66.Name = "TextBox66"
        TextBox66.Size = New Size(145, 16)
        TextBox66.TabIndex = 16
        TextBox66.Text = "Super Admin Name"
        TextBox66.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label27
        ' 
        Label27.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label27.AutoSize = True
        Label27.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label27.ForeColor = Color.SeaGreen
        Label27.Location = New Point(901, 45)
        Label27.Name = "Label27"
        Label27.Size = New Size(91, 12)
        Label27.TabIndex = 15
        Label27.Text = "Super Admin / Owner"
        ' 
        ' TextBox64
        ' 
        TextBox64.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox64.BackColor = SystemColors.ControlLightLight
        TextBox64.BorderStyle = BorderStyle.None
        TextBox64.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox64.Location = New Point(1719, 26)
        TextBox64.Name = "TextBox64"
        TextBox64.Size = New Size(145, 16)
        TextBox64.TabIndex = 14
        TextBox64.Text = "Super Admin Name"
        TextBox64.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label23
        ' 
        Label23.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label23.AutoSize = True
        Label23.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label23.ForeColor = Color.SeaGreen
        Label23.Location = New Point(1746, 45)
        Label23.Name = "Label23"
        Label23.Size = New Size(91, 12)
        Label23.TabIndex = 13
        Label23.Text = "Super Admin / Owner"
        ' 
        ' Label24
        ' 
        Label24.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label24.AutoSize = True
        Label24.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label24.ForeColor = Color.SeaGreen
        Label24.Location = New Point(2590, 45)
        Label24.Name = "Label24"
        Label24.Size = New Size(91, 12)
        Label24.TabIndex = 12
        Label24.Text = "Super Admin / Owner"
        ' 
        ' TextBox65
        ' 
        TextBox65.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox65.BackColor = SystemColors.ControlLightLight
        TextBox65.BorderStyle = BorderStyle.None
        TextBox65.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox65.Location = New Point(2564, 26)
        TextBox65.Name = "TextBox65"
        TextBox65.Size = New Size(145, 16)
        TextBox65.TabIndex = 11
        TextBox65.Text = "Super Admin Name"
        TextBox65.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.BackColor = Color.Transparent
        Label25.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label25.Location = New Point(51, 45)
        Label25.Name = "Label25"
        Label25.Size = New Size(241, 17)
        Label25.TabIndex = 1
        Label25.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label26.ForeColor = Color.SeaGreen
        Label26.Location = New Point(51, 10)
        Label26.Name = "Label26"
        Label26.Size = New Size(118, 37)
        Label26.TabIndex = 0
        Label26.Text = "Reports"
        ' 
        ' pnlPurchasesReport
        ' 
        pnlPurchasesReport.Controls.Add(purchasesDataGrid)
        pnlPurchasesReport.Controls.Add(panelPurchasesReport)
        pnlPurchasesReport.Controls.Add(Panel33)
        pnlPurchasesReport.Dock = DockStyle.Fill
        pnlPurchasesReport.Location = New Point(0, 0)
        pnlPurchasesReport.Name = "pnlPurchasesReport"
        pnlPurchasesReport.Size = New Size(1264, 830)
        pnlPurchasesReport.TabIndex = 19
        ' 
        ' purchasesDataGrid
        ' 
        purchasesDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        purchasesDataGrid.BackgroundColor = SystemColors.ControlLightLight
        purchasesDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        purchasesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        purchasesDataGrid.Columns.AddRange(New DataGridViewColumn() {DataGridViewTextBoxColumn9, DataGridViewTextBoxColumn10, DataGridViewTextBoxColumn11, DataGridViewTextBoxColumn12, DataGridViewTextBoxColumn13, DataGridViewTextBoxColumn14, DataGridViewTextBoxColumn15})
        purchasesDataGrid.Location = New Point(271, 248)
        purchasesDataGrid.Name = "purchasesDataGrid"
        purchasesDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        purchasesDataGrid.Size = New Size(934, 100)
        purchasesDataGrid.TabIndex = 16
        ' 
        ' DataGridViewTextBoxColumn9
        ' 
        DataGridViewTextBoxColumn9.HeaderText = "TRANSACTION ID"
        DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        ' 
        ' DataGridViewTextBoxColumn10
        ' 
        DataGridViewTextBoxColumn10.HeaderText = "DATE"
        DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        ' 
        ' DataGridViewTextBoxColumn11
        ' 
        DataGridViewTextBoxColumn11.HeaderText = "CUSTOMER"
        DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        ' 
        ' DataGridViewTextBoxColumn12
        ' 
        DataGridViewTextBoxColumn12.HeaderText = "CASHIER"
        DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        ' 
        ' DataGridViewTextBoxColumn13
        ' 
        DataGridViewTextBoxColumn13.HeaderText = "TYPE"
        DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        ' 
        ' DataGridViewTextBoxColumn14
        ' 
        DataGridViewTextBoxColumn14.HeaderText = "TOTAL"
        DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        ' 
        ' DataGridViewTextBoxColumn15
        ' 
        DataGridViewTextBoxColumn15.HeaderText = "STATUS"
        DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        ' 
        ' panelPurchasesReport
        ' 
        panelPurchasesReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelPurchasesReport.BackColor = SystemColors.ControlLightLight
        panelPurchasesReport.Controls.Add(purchasesDateTime)
        panelPurchasesReport.Controls.Add(ComboBox5)
        panelPurchasesReport.Controls.Add(Button14)
        panelPurchasesReport.Controls.Add(purchasesSearchBar)
        panelPurchasesReport.Controls.Add(titlePurchasesReport)
        panelPurchasesReport.Location = New Point(274, 122)
        panelPurchasesReport.Name = "panelPurchasesReport"
        panelPurchasesReport.Size = New Size(934, 100)
        panelPurchasesReport.TabIndex = 9
        ' 
        ' purchasesDateTime
        ' 
        purchasesDateTime.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        purchasesDateTime.Location = New Point(654, 53)
        purchasesDateTime.Name = "purchasesDateTime"
        purchasesDateTime.Size = New Size(263, 27)
        purchasesDateTime.TabIndex = 20
        ' 
        ' ComboBox5
        ' 
        ComboBox5.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        ComboBox5.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        ComboBox5.FormattingEnabled = True
        ComboBox5.Location = New Point(1476, 54)
        ComboBox5.Name = "ComboBox5"
        ComboBox5.Size = New Size(175, 36)
        ComboBox5.TabIndex = 19
        ' 
        ' Button14
        ' 
        Button14.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button14.BackColor = Color.SeaGreen
        Button14.FlatAppearance.BorderSize = 0
        Button14.FlatStyle = FlatStyle.Flat
        Button14.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button14.ForeColor = SystemColors.ControlLightLight
        Button14.Location = New Point(1476, 13)
        Button14.Name = "Button14"
        Button14.Size = New Size(175, 35)
        Button14.TabIndex = 18
        Button14.Text = "Print Report"
        Button14.UseVisualStyleBackColor = False
        ' 
        ' purchasesSearchBar
        ' 
        purchasesSearchBar.BackColor = SystemColors.ControlLight
        purchasesSearchBar.BorderStyle = BorderStyle.None
        purchasesSearchBar.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        purchasesSearchBar.ForeColor = SystemColors.ControlDarkDark
        purchasesSearchBar.Location = New Point(18, 53)
        purchasesSearchBar.Name = "purchasesSearchBar"
        purchasesSearchBar.Size = New Size(618, 26)
        purchasesSearchBar.TabIndex = 6
        purchasesSearchBar.Text = "   Search by delivery ID or supplier..."
        ' 
        ' titlePurchasesReport
        ' 
        titlePurchasesReport.BackColor = SystemColors.ControlLightLight
        titlePurchasesReport.BorderStyle = BorderStyle.None
        titlePurchasesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titlePurchasesReport.ForeColor = SystemColors.ActiveCaptionText
        titlePurchasesReport.Location = New Point(18, 22)
        titlePurchasesReport.Name = "titlePurchasesReport"
        titlePurchasesReport.Size = New Size(223, 26)
        titlePurchasesReport.TabIndex = 5
        titlePurchasesReport.Text = "Purchases Report"
        ' 
        ' Panel33
        ' 
        Panel33.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel33.BackColor = SystemColors.ControlLightLight
        Panel33.Controls.Add(TextBox61)
        Panel33.Controls.Add(Label22)
        Panel33.Controls.Add(Label19)
        Panel33.Controls.Add(TextBox60)
        Panel33.Controls.Add(Label20)
        Panel33.Controls.Add(Label21)
        Panel33.Location = New Point(219, 0)
        Panel33.Name = "Panel33"
        Panel33.Size = New Size(1045, 76)
        Panel33.TabIndex = 4
        ' 
        ' TextBox61
        ' 
        TextBox61.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox61.BackColor = SystemColors.ControlLightLight
        TextBox61.BorderStyle = BorderStyle.None
        TextBox61.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox61.Location = New Point(874, 26)
        TextBox61.Name = "TextBox61"
        TextBox61.Size = New Size(145, 16)
        TextBox61.TabIndex = 14
        TextBox61.Text = "Super Admin Name"
        TextBox61.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label22
        ' 
        Label22.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label22.AutoSize = True
        Label22.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label22.ForeColor = Color.SeaGreen
        Label22.Location = New Point(901, 45)
        Label22.Name = "Label22"
        Label22.Size = New Size(91, 12)
        Label22.TabIndex = 13
        Label22.Text = "Super Admin / Owner"
        ' 
        ' Label19
        ' 
        Label19.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label19.AutoSize = True
        Label19.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label19.ForeColor = Color.SeaGreen
        Label19.Location = New Point(1745, 45)
        Label19.Name = "Label19"
        Label19.Size = New Size(91, 12)
        Label19.TabIndex = 12
        Label19.Text = "Super Admin / Owner"
        ' 
        ' TextBox60
        ' 
        TextBox60.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox60.BackColor = SystemColors.ControlLightLight
        TextBox60.BorderStyle = BorderStyle.None
        TextBox60.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox60.Location = New Point(1719, 26)
        TextBox60.Name = "TextBox60"
        TextBox60.Size = New Size(145, 16)
        TextBox60.TabIndex = 11
        TextBox60.Text = "Super Admin Name"
        TextBox60.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.BackColor = Color.Transparent
        Label20.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label20.Location = New Point(51, 45)
        Label20.Name = "Label20"
        Label20.Size = New Size(241, 17)
        Label20.TabIndex = 1
        Label20.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label21.ForeColor = Color.SeaGreen
        Label21.Location = New Point(51, 10)
        Label21.Name = "Label21"
        Label21.Size = New Size(118, 37)
        Label21.TabIndex = 0
        Label21.Text = "Reports"
        ' 
        ' pnlWholesaleSalesReport
        ' 
        pnlWholesaleSalesReport.Controls.Add(wholesaleDataGrid)
        pnlWholesaleSalesReport.Controls.Add(panelWholesale)
        pnlWholesaleSalesReport.Controls.Add(Panel29)
        pnlWholesaleSalesReport.Dock = DockStyle.Fill
        pnlWholesaleSalesReport.Location = New Point(0, 0)
        pnlWholesaleSalesReport.Name = "pnlWholesaleSalesReport"
        pnlWholesaleSalesReport.Size = New Size(1264, 830)
        pnlWholesaleSalesReport.TabIndex = 17
        ' 
        ' wholesaleDataGrid
        ' 
        wholesaleDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        wholesaleDataGrid.BackgroundColor = SystemColors.ControlLightLight
        wholesaleDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        wholesaleDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        wholesaleDataGrid.Columns.AddRange(New DataGridViewColumn() {DataGridViewTextBoxColumn2, DataGridViewTextBoxColumn3, DataGridViewTextBoxColumn4, DataGridViewTextBoxColumn5, DataGridViewTextBoxColumn6, DataGridViewTextBoxColumn7, DataGridViewTextBoxColumn8})
        wholesaleDataGrid.Location = New Point(271, 248)
        wholesaleDataGrid.Name = "wholesaleDataGrid"
        wholesaleDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        wholesaleDataGrid.Size = New Size(934, 100)
        wholesaleDataGrid.TabIndex = 16
        ' 
        ' DataGridViewTextBoxColumn2
        ' 
        DataGridViewTextBoxColumn2.HeaderText = "TRANSACTION ID"
        DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        ' 
        ' DataGridViewTextBoxColumn3
        ' 
        DataGridViewTextBoxColumn3.HeaderText = "DATE"
        DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        ' 
        ' DataGridViewTextBoxColumn4
        ' 
        DataGridViewTextBoxColumn4.HeaderText = "CUSTOMER"
        DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        ' 
        ' DataGridViewTextBoxColumn5
        ' 
        DataGridViewTextBoxColumn5.HeaderText = "CASHIER"
        DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        ' 
        ' DataGridViewTextBoxColumn6
        ' 
        DataGridViewTextBoxColumn6.HeaderText = "TYPE"
        DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        ' 
        ' DataGridViewTextBoxColumn7
        ' 
        DataGridViewTextBoxColumn7.HeaderText = "TOTAL"
        DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        ' 
        ' DataGridViewTextBoxColumn8
        ' 
        DataGridViewTextBoxColumn8.HeaderText = "STATUS"
        DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        ' 
        ' panelWholesale
        ' 
        panelWholesale.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelWholesale.BackColor = SystemColors.ControlLightLight
        panelWholesale.Controls.Add(wholesaleScheduleDropdown)
        panelWholesale.Controls.Add(wholesalePrintReport)
        panelWholesale.Controls.Add(employeeGenerated)
        panelWholesale.Controls.Add(titleWholesaleSalesReport)
        panelWholesale.Location = New Point(274, 122)
        panelWholesale.Name = "panelWholesale"
        panelWholesale.Size = New Size(934, 100)
        panelWholesale.TabIndex = 9
        ' 
        ' wholesaleScheduleDropdown
        ' 
        wholesaleScheduleDropdown.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wholesaleScheduleDropdown.FormattingEnabled = True
        wholesaleScheduleDropdown.Location = New Point(742, 53)
        wholesaleScheduleDropdown.Name = "wholesaleScheduleDropdown"
        wholesaleScheduleDropdown.Size = New Size(175, 36)
        wholesaleScheduleDropdown.TabIndex = 19
        ' 
        ' wholesalePrintReport
        ' 
        wholesalePrintReport.BackColor = Color.SeaGreen
        wholesalePrintReport.FlatAppearance.BorderSize = 0
        wholesalePrintReport.FlatStyle = FlatStyle.Flat
        wholesalePrintReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        wholesalePrintReport.ForeColor = SystemColors.ControlLightLight
        wholesalePrintReport.Location = New Point(742, 12)
        wholesalePrintReport.Name = "wholesalePrintReport"
        wholesalePrintReport.Size = New Size(175, 35)
        wholesalePrintReport.TabIndex = 18
        wholesalePrintReport.Text = "Print Report"
        wholesalePrintReport.UseVisualStyleBackColor = False
        ' 
        ' employeeGenerated
        ' 
        employeeGenerated.BackColor = SystemColors.ControlLightLight
        employeeGenerated.BorderStyle = BorderStyle.None
        employeeGenerated.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        employeeGenerated.ForeColor = SystemColors.ControlDarkDark
        employeeGenerated.Location = New Point(18, 53)
        employeeGenerated.Name = "employeeGenerated"
        employeeGenerated.Size = New Size(391, 22)
        employeeGenerated.TabIndex = 6
        employeeGenerated.Text = "Generated: Month, Date, Year, Time | By: Name" & vbCrLf & vbCrLf
        ' 
        ' titleWholesaleSalesReport
        ' 
        titleWholesaleSalesReport.BackColor = SystemColors.ControlLightLight
        titleWholesaleSalesReport.BorderStyle = BorderStyle.None
        titleWholesaleSalesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleWholesaleSalesReport.ForeColor = SystemColors.ActiveCaptionText
        titleWholesaleSalesReport.Location = New Point(18, 22)
        titleWholesaleSalesReport.Name = "titleWholesaleSalesReport"
        titleWholesaleSalesReport.Size = New Size(223, 26)
        titleWholesaleSalesReport.TabIndex = 5
        titleWholesaleSalesReport.Text = "Wholesale Sales Report"
        ' 
        ' Panel29
        ' 
        Panel29.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel29.BackColor = SystemColors.ControlLightLight
        Panel29.Controls.Add(TextBox25)
        Panel29.Controls.Add(Label32)
        Panel29.Controls.Add(Label14)
        Panel29.Controls.Add(Label15)
        Panel29.Location = New Point(219, 0)
        Panel29.Name = "Panel29"
        Panel29.Size = New Size(1045, 76)
        Panel29.TabIndex = 4
        ' 
        ' TextBox25
        ' 
        TextBox25.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox25.BackColor = SystemColors.ControlLightLight
        TextBox25.BorderStyle = BorderStyle.None
        TextBox25.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox25.Location = New Point(874, 26)
        TextBox25.Name = "TextBox25"
        TextBox25.Size = New Size(145, 16)
        TextBox25.TabIndex = 16
        TextBox25.Text = "Super Admin Name"
        TextBox25.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label32
        ' 
        Label32.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label32.AutoSize = True
        Label32.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label32.ForeColor = Color.SeaGreen
        Label32.Location = New Point(901, 45)
        Label32.Name = "Label32"
        Label32.Size = New Size(91, 12)
        Label32.TabIndex = 15
        Label32.Text = "Super Admin / Owner"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.BackColor = Color.Transparent
        Label14.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label14.Location = New Point(51, 45)
        Label14.Name = "Label14"
        Label14.Size = New Size(241, 17)
        Label14.TabIndex = 1
        Label14.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label15.ForeColor = Color.SeaGreen
        Label15.Location = New Point(51, 10)
        Label15.Name = "Label15"
        Label15.Size = New Size(118, 37)
        Label15.TabIndex = 0
        Label15.Text = "Reports"
        ' 
        ' pnlEmployeesalesReport
        ' 
        pnlEmployeesalesReport.Controls.Add(panelEmployeeDataGrid)
        pnlEmployeesalesReport.Controls.Add(panelEmployeeSalesReport)
        pnlEmployeesalesReport.Controls.Add(Panel37)
        pnlEmployeesalesReport.Dock = DockStyle.Fill
        pnlEmployeesalesReport.Location = New Point(0, 0)
        pnlEmployeesalesReport.Name = "pnlEmployeesalesReport"
        pnlEmployeesalesReport.Size = New Size(1264, 830)
        pnlEmployeesalesReport.TabIndex = 21
        ' 
        ' panelEmployeeDataGrid
        ' 
        panelEmployeeDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelEmployeeDataGrid.BackColor = SystemColors.ControlLightLight
        panelEmployeeDataGrid.Controls.Add(employeeName)
        panelEmployeeDataGrid.Controls.Add(employeDataGrid)
        panelEmployeeDataGrid.Location = New Point(270, 248)
        panelEmployeeDataGrid.Name = "panelEmployeeDataGrid"
        panelEmployeeDataGrid.Size = New Size(937, 528)
        panelEmployeeDataGrid.TabIndex = 17
        ' 
        ' employeeName
        ' 
        employeeName.BackColor = SystemColors.ControlLightLight
        employeeName.BorderStyle = BorderStyle.None
        employeeName.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        employeeName.ForeColor = SystemColors.ActiveCaptionText
        employeeName.Location = New Point(22, 21)
        employeeName.Name = "employeeName"
        employeeName.Size = New Size(223, 26)
        employeeName.TabIndex = 18
        employeeName.Text = "Name of Employee"
        ' 
        ' employeDataGrid
        ' 
        employeDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        employeDataGrid.BackgroundColor = SystemColors.ControlLightLight
        employeDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        employeDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        employeDataGrid.Columns.AddRange(New DataGridViewColumn() {employeeTransactionID, employeeDate, employeeCustomer, employeeCashier, employeeType, employeeTotal, employeeStatus})
        employeDataGrid.Location = New Point(22, 53)
        employeDataGrid.Name = "employeDataGrid"
        employeDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        employeDataGrid.Size = New Size(888, 100)
        employeDataGrid.TabIndex = 16
        ' 
        ' employeeTransactionID
        ' 
        employeeTransactionID.HeaderText = "TRANSACTION ID"
        employeeTransactionID.Name = "employeeTransactionID"
        ' 
        ' employeeDate
        ' 
        employeeDate.HeaderText = "DATE"
        employeeDate.Name = "employeeDate"
        ' 
        ' employeeCustomer
        ' 
        employeeCustomer.HeaderText = "CUSTOMER"
        employeeCustomer.Name = "employeeCustomer"
        ' 
        ' employeeCashier
        ' 
        employeeCashier.HeaderText = "CASHIER"
        employeeCashier.Name = "employeeCashier"
        ' 
        ' employeeType
        ' 
        employeeType.HeaderText = "TYPE"
        employeeType.Name = "employeeType"
        ' 
        ' employeeTotal
        ' 
        employeeTotal.HeaderText = "TOTAL"
        employeeTotal.Name = "employeeTotal"
        ' 
        ' employeeStatus
        ' 
        employeeStatus.HeaderText = "STATUS"
        employeeStatus.Name = "employeeStatus"
        ' 
        ' panelEmployeeSalesReport
        ' 
        panelEmployeeSalesReport.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        panelEmployeeSalesReport.BackColor = SystemColors.ControlLightLight
        panelEmployeeSalesReport.Controls.Add(employeeScheduleDropdown)
        panelEmployeeSalesReport.Controls.Add(employeeNamesDropdown)
        panelEmployeeSalesReport.Controls.Add(employeePrintReport)
        panelEmployeeSalesReport.Controls.Add(Button18)
        panelEmployeeSalesReport.Controls.Add(TextBox67)
        panelEmployeeSalesReport.Controls.Add(titleEmployeeSalesReport)
        panelEmployeeSalesReport.Location = New Point(274, 122)
        panelEmployeeSalesReport.Name = "panelEmployeeSalesReport"
        panelEmployeeSalesReport.Size = New Size(934, 100)
        panelEmployeeSalesReport.TabIndex = 9
        ' 
        ' employeeScheduleDropdown
        ' 
        employeeScheduleDropdown.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        employeeScheduleDropdown.FormattingEnabled = True
        employeeScheduleDropdown.Location = New Point(18, 54)
        employeeScheduleDropdown.Name = "employeeScheduleDropdown"
        employeeScheduleDropdown.Size = New Size(175, 36)
        employeeScheduleDropdown.TabIndex = 20
        ' 
        ' employeeNamesDropdown
        ' 
        employeeNamesDropdown.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        employeeNamesDropdown.Font = New Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        employeeNamesDropdown.FormattingEnabled = True
        employeeNamesDropdown.Location = New Point(742, 53)
        employeeNamesDropdown.Name = "employeeNamesDropdown"
        employeeNamesDropdown.Size = New Size(175, 36)
        employeeNamesDropdown.TabIndex = 19
        ' 
        ' employeePrintReport
        ' 
        employeePrintReport.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        employeePrintReport.BackColor = Color.SeaGreen
        employeePrintReport.FlatAppearance.BorderSize = 0
        employeePrintReport.FlatStyle = FlatStyle.Flat
        employeePrintReport.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        employeePrintReport.ForeColor = SystemColors.ControlLightLight
        employeePrintReport.Location = New Point(742, 12)
        employeePrintReport.Name = "employeePrintReport"
        employeePrintReport.Size = New Size(175, 35)
        employeePrintReport.TabIndex = 18
        employeePrintReport.Text = "Print Report"
        employeePrintReport.UseVisualStyleBackColor = False
        ' 
        ' Button18
        ' 
        Button18.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button18.BackColor = Color.SeaGreen
        Button18.FlatAppearance.BorderSize = 0
        Button18.FlatStyle = FlatStyle.Flat
        Button18.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button18.ForeColor = SystemColors.ControlLightLight
        Button18.Location = New Point(2210, 36)
        Button18.Name = "Button18"
        Button18.Size = New Size(175, 35)
        Button18.TabIndex = 15
        Button18.Text = "Print Report"
        Button18.UseVisualStyleBackColor = False
        ' 
        ' TextBox67
        ' 
        TextBox67.BackColor = SystemColors.ControlLightLight
        TextBox67.BorderStyle = BorderStyle.None
        TextBox67.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox67.ForeColor = SystemColors.ControlDarkDark
        TextBox67.Location = New Point(18, 53)
        TextBox67.Name = "TextBox67"
        TextBox67.Size = New Size(0, 22)
        TextBox67.TabIndex = 6
        TextBox67.Text = "Generated: Month, Date, Year, Time | By: Name" & vbCrLf & vbCrLf
        ' 
        ' titleEmployeeSalesReport
        ' 
        titleEmployeeSalesReport.BackColor = SystemColors.ControlLightLight
        titleEmployeeSalesReport.BorderStyle = BorderStyle.None
        titleEmployeeSalesReport.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleEmployeeSalesReport.ForeColor = SystemColors.ActiveCaptionText
        titleEmployeeSalesReport.Location = New Point(18, 22)
        titleEmployeeSalesReport.Name = "titleEmployeeSalesReport"
        titleEmployeeSalesReport.Size = New Size(223, 26)
        titleEmployeeSalesReport.TabIndex = 5
        titleEmployeeSalesReport.Text = "Employee Sales Report"
        ' 
        ' Panel37
        ' 
        Panel37.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Panel37.BackColor = SystemColors.ControlLightLight
        Panel37.Controls.Add(TextBox68)
        Panel37.Controls.Add(Label31)
        Panel37.Controls.Add(Label28)
        Panel37.Controls.Add(TextBox69)
        Panel37.Controls.Add(Label29)
        Panel37.Controls.Add(Label30)
        Panel37.Location = New Point(219, 0)
        Panel37.Name = "Panel37"
        Panel37.Size = New Size(1045, 76)
        Panel37.TabIndex = 4
        ' 
        ' TextBox68
        ' 
        TextBox68.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox68.BackColor = SystemColors.ControlLightLight
        TextBox68.BorderStyle = BorderStyle.None
        TextBox68.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox68.Location = New Point(874, 26)
        TextBox68.Name = "TextBox68"
        TextBox68.Size = New Size(145, 16)
        TextBox68.TabIndex = 14
        TextBox68.Text = "Super Admin Name"
        TextBox68.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label31
        ' 
        Label31.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label31.AutoSize = True
        Label31.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label31.ForeColor = Color.SeaGreen
        Label31.Location = New Point(901, 45)
        Label31.Name = "Label31"
        Label31.Size = New Size(91, 12)
        Label31.TabIndex = 13
        Label31.Text = "Super Admin / Owner"
        ' 
        ' Label28
        ' 
        Label28.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label28.AutoSize = True
        Label28.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label28.ForeColor = Color.SeaGreen
        Label28.Location = New Point(3654, 45)
        Label28.Name = "Label28"
        Label28.Size = New Size(91, 12)
        Label28.TabIndex = 12
        Label28.Text = "Super Admin / Owner"
        ' 
        ' TextBox69
        ' 
        TextBox69.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox69.BackColor = SystemColors.ControlLightLight
        TextBox69.BorderStyle = BorderStyle.None
        TextBox69.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox69.Location = New Point(3628, 26)
        TextBox69.Name = "TextBox69"
        TextBox69.Size = New Size(145, 16)
        TextBox69.TabIndex = 11
        TextBox69.Text = "Super Admin Name"
        TextBox69.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label29
        ' 
        Label29.AutoSize = True
        Label29.BackColor = Color.Transparent
        Label29.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label29.Location = New Point(51, 45)
        Label29.Name = "Label29"
        Label29.Size = New Size(241, 17)
        Label29.TabIndex = 1
        Label29.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' Label30
        ' 
        Label30.AutoSize = True
        Label30.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label30.ForeColor = Color.SeaGreen
        Label30.Location = New Point(51, 10)
        Label30.Name = "Label30"
        Label30.Size = New Size(118, 37)
        Label30.TabIndex = 0
        Label30.Text = "Reports"
        ' 
        ' pnlAccounts
        ' 
        pnlAccounts.Controls.Add(accountsDataGrid)
        pnlAccounts.Controls.Add(numberTotalUsersRegistered)
        pnlAccounts.Controls.Add(btnAddAccount)
        pnlAccounts.Controls.Add(accountTableLayout)
        pnlAccounts.Controls.Add(pnlHeaderAccounts)
        pnlAccounts.Dock = DockStyle.Fill
        pnlAccounts.Location = New Point(0, 0)
        pnlAccounts.Name = "pnlAccounts"
        pnlAccounts.Size = New Size(1264, 830)
        pnlAccounts.TabIndex = 9
        ' 
        ' accountsDataGrid
        ' 
        accountsDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        accountsDataGrid.BackgroundColor = SystemColors.ControlLightLight
        accountsDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        accountsDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        accountsDataGrid.Columns.AddRange(New DataGridViewColumn() {accountUserID, accountFullName, accountUsername, accountRole, accountStatus, accountLastLogin, accountEdit, accountResetPassword, accountEnableDisable})
        accountsDataGrid.Location = New Point(273, 309)
        accountsDataGrid.Name = "accountsDataGrid"
        accountsDataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
        accountsDataGrid.Size = New Size(940, 89)
        accountsDataGrid.TabIndex = 14
        ' 
        ' accountUserID
        ' 
        accountUserID.HeaderText = "USER ID"
        accountUserID.Name = "accountUserID"
        ' 
        ' accountFullName
        ' 
        accountFullName.HeaderText = "FULL NAME"
        accountFullName.Name = "accountFullName"
        ' 
        ' accountUsername
        ' 
        accountUsername.HeaderText = "USERNAME"
        accountUsername.Name = "accountUsername"
        ' 
        ' accountRole
        ' 
        accountRole.HeaderText = "ROLE"
        accountRole.Name = "accountRole"
        ' 
        ' accountStatus
        ' 
        accountStatus.HeaderText = "STATUS"
        accountStatus.Name = "accountStatus"
        ' 
        ' accountLastLogin
        ' 
        accountLastLogin.HeaderText = "LAST LOGIN"
        accountLastLogin.Name = "accountLastLogin"
        ' 
        ' accountEdit
        ' 
        accountEdit.HeaderText = ""
        accountEdit.Name = "accountEdit"
        ' 
        ' accountResetPassword
        ' 
        accountResetPassword.HeaderText = ""
        accountResetPassword.Name = "accountResetPassword"
        ' 
        ' accountEnableDisable
        ' 
        accountEnableDisable.HeaderText = ""
        accountEnableDisable.Name = "accountEnableDisable"
        ' 
        ' numberTotalUsersRegistered
        ' 
        numberTotalUsersRegistered.BackColor = SystemColors.Control
        numberTotalUsersRegistered.BorderStyle = BorderStyle.None
        numberTotalUsersRegistered.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        numberTotalUsersRegistered.ForeColor = SystemColors.ControlDarkDark
        numberTotalUsersRegistered.Location = New Point(276, 122)
        numberTotalUsersRegistered.Name = "numberTotalUsersRegistered"
        numberTotalUsersRegistered.Size = New Size(145, 16)
        numberTotalUsersRegistered.TabIndex = 8
        numberTotalUsersRegistered.Text = "0 users registered "
        ' 
        ' btnAddAccount
        ' 
        btnAddAccount.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnAddAccount.BackColor = Color.SeaGreen
        btnAddAccount.FlatAppearance.BorderSize = 0
        btnAddAccount.FlatStyle = FlatStyle.Flat
        btnAddAccount.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAddAccount.ForeColor = SystemColors.ControlLightLight
        btnAddAccount.Location = New Point(1038, 122)
        btnAddAccount.Name = "btnAddAccount"
        btnAddAccount.Size = New Size(175, 35)
        btnAddAccount.TabIndex = 13
        btnAddAccount.Text = "     Add Account"
        btnAddAccount.UseVisualStyleBackColor = False
        ' 
        ' accountTableLayout
        ' 
        accountTableLayout.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        accountTableLayout.ColumnCount = 3
        accountTableLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333244F))
        accountTableLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333359F))
        accountTableLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33334F))
        accountTableLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 20F))
        accountTableLayout.Controls.Add(panelSuperAdmin, 0, 0)
        accountTableLayout.Controls.Add(panelAdmin, 1, 0)
        accountTableLayout.Controls.Add(panelAssistant, 2, 0)
        accountTableLayout.Location = New Point(273, 172)
        accountTableLayout.Name = "accountTableLayout"
        accountTableLayout.RowCount = 1
        accountTableLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        accountTableLayout.Size = New Size(940, 117)
        accountTableLayout.TabIndex = 4
        ' 
        ' panelSuperAdmin
        ' 
        panelSuperAdmin.BackColor = SystemColors.ControlLightLight
        panelSuperAdmin.Controls.Add(titleSuperAdmin)
        panelSuperAdmin.Controls.Add(numberSuperAdmin)
        panelSuperAdmin.Dock = DockStyle.Fill
        panelSuperAdmin.Location = New Point(3, 3)
        panelSuperAdmin.Name = "panelSuperAdmin"
        panelSuperAdmin.Size = New Size(307, 111)
        panelSuperAdmin.TabIndex = 0
        ' 
        ' titleSuperAdmin
        ' 
        titleSuperAdmin.BackColor = SystemColors.ControlLightLight
        titleSuperAdmin.BorderStyle = BorderStyle.None
        titleSuperAdmin.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleSuperAdmin.ForeColor = SystemColors.ControlDarkDark
        titleSuperAdmin.Location = New Point(67, 13)
        titleSuperAdmin.Name = "titleSuperAdmin"
        titleSuperAdmin.Size = New Size(194, 26)
        titleSuperAdmin.TabIndex = 4
        titleSuperAdmin.Text = "Super Admin / Owner"
        ' 
        ' numberSuperAdmin
        ' 
        numberSuperAdmin.BackColor = SystemColors.ControlLightLight
        numberSuperAdmin.BorderStyle = BorderStyle.None
        numberSuperAdmin.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        numberSuperAdmin.Location = New Point(67, 45)
        numberSuperAdmin.Name = "numberSuperAdmin"
        numberSuperAdmin.Size = New Size(100, 26)
        numberSuperAdmin.TabIndex = 2
        numberSuperAdmin.Text = "00"
        ' 
        ' panelAdmin
        ' 
        panelAdmin.BackColor = SystemColors.ControlLightLight
        panelAdmin.Controls.Add(numberAdmin)
        panelAdmin.Controls.Add(titleAdmin)
        panelAdmin.Dock = DockStyle.Fill
        panelAdmin.Location = New Point(316, 3)
        panelAdmin.Name = "panelAdmin"
        panelAdmin.Size = New Size(307, 111)
        panelAdmin.TabIndex = 1
        ' 
        ' numberAdmin
        ' 
        numberAdmin.BackColor = SystemColors.ControlLightLight
        numberAdmin.BorderStyle = BorderStyle.None
        numberAdmin.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        numberAdmin.Location = New Point(59, 45)
        numberAdmin.Name = "numberAdmin"
        numberAdmin.Size = New Size(100, 26)
        numberAdmin.TabIndex = 5
        numberAdmin.Text = "00"
        ' 
        ' titleAdmin
        ' 
        titleAdmin.BackColor = SystemColors.ControlLightLight
        titleAdmin.BorderStyle = BorderStyle.None
        titleAdmin.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleAdmin.ForeColor = SystemColors.ControlDarkDark
        titleAdmin.Location = New Point(59, 13)
        titleAdmin.Name = "titleAdmin"
        titleAdmin.Size = New Size(177, 26)
        titleAdmin.TabIndex = 6
        titleAdmin.Text = "Admin / Pharmacist"
        ' 
        ' panelAssistant
        ' 
        panelAssistant.BackColor = SystemColors.ControlLightLight
        panelAssistant.Controls.Add(numberAssistant)
        panelAssistant.Controls.Add(titleAssistant)
        panelAssistant.Dock = DockStyle.Fill
        panelAssistant.Location = New Point(629, 3)
        panelAssistant.Name = "panelAssistant"
        panelAssistant.Size = New Size(308, 111)
        panelAssistant.TabIndex = 2
        ' 
        ' numberAssistant
        ' 
        numberAssistant.BackColor = SystemColors.ControlLightLight
        numberAssistant.BorderStyle = BorderStyle.None
        numberAssistant.Font = New Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        numberAssistant.Location = New Point(58, 47)
        numberAssistant.Name = "numberAssistant"
        numberAssistant.Size = New Size(100, 26)
        numberAssistant.TabIndex = 8
        numberAssistant.Text = "00"
        ' 
        ' titleAssistant
        ' 
        titleAssistant.BackColor = SystemColors.ControlLightLight
        titleAssistant.BorderStyle = BorderStyle.None
        titleAssistant.Font = New Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        titleAssistant.ForeColor = SystemColors.ControlDarkDark
        titleAssistant.Location = New Point(58, 13)
        titleAssistant.Name = "titleAssistant"
        titleAssistant.Size = New Size(178, 26)
        titleAssistant.TabIndex = 7
        titleAssistant.Text = "Pharmacy Assistant"
        ' 
        ' pnlHeaderAccounts
        ' 
        pnlHeaderAccounts.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pnlHeaderAccounts.BackColor = SystemColors.ControlLightLight
        pnlHeaderAccounts.Controls.Add(TextBox23)
        pnlHeaderAccounts.Controls.Add(Label2)
        pnlHeaderAccounts.Controls.Add(Label3)
        pnlHeaderAccounts.Controls.Add(hdrAccounts)
        pnlHeaderAccounts.Location = New Point(219, 0)
        pnlHeaderAccounts.Name = "pnlHeaderAccounts"
        pnlHeaderAccounts.Size = New Size(1045, 76)
        pnlHeaderAccounts.TabIndex = 3
        ' 
        ' TextBox23
        ' 
        TextBox23.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        TextBox23.BackColor = SystemColors.ControlLightLight
        TextBox23.BorderStyle = BorderStyle.None
        TextBox23.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox23.Location = New Point(874, 26)
        TextBox23.Name = "TextBox23"
        TextBox23.Size = New Size(145, 16)
        TextBox23.TabIndex = 7
        TextBox23.Text = "Super Admin Name"
        TextBox23.TextAlign = HorizontalAlignment.Center
        ' 
        ' Label2
        ' 
        Label2.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.ForeColor = Color.SeaGreen
        Label2.Location = New Point(901, 45)
        Label2.Name = "Label2"
        Label2.Size = New Size(91, 12)
        Label2.TabIndex = 5
        Label2.Text = "Super Admin / Owner"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.Transparent
        Label3.Font = New Font("Segoe UI Semilight", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(51, 45)
        Label3.Name = "Label3"
        Label3.Size = New Size(241, 17)
        Label3.TabIndex = 1
        Label3.Text = "Cabrera’s Drugstore and Medical Supplies"
        ' 
        ' hdrAccounts
        ' 
        hdrAccounts.AutoSize = True
        hdrAccounts.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        hdrAccounts.ForeColor = Color.SeaGreen
        hdrAccounts.Location = New Point(51, 10)
        hdrAccounts.Name = "hdrAccounts"
        hdrAccounts.Size = New Size(134, 37)
        hdrAccounts.TabIndex = 0
        hdrAccounts.Text = "Accounts"
        ' 
        ' pnlLogin
        ' 
        pnlLogin.Controls.Add(NavBar)
        pnlLogin.Location = New Point(0, 0)
        pnlLogin.Name = "pnlLogin"
        pnlLogin.Size = New Size(1264, 830)
        pnlLogin.TabIndex = 18
        ' 
        ' NavBar
        ' 
        NavBar.BackColor = Color.White
        NavBar.Controls.Add(LoginLabel)
        NavBar.Controls.Add(picHide)
        NavBar.Controls.Add(View)
        NavBar.Controls.Add(LoginIcon)
        NavBar.Controls.Add(LoginBtn)
        NavBar.Controls.Add(PasswordField)
        NavBar.Controls.Add(UsernameField)
        NavBar.Controls.Add(Password)
        NavBar.Controls.Add(Username)
        NavBar.Controls.Add(DrugstoreName)
        NavBar.Controls.Add(imageLogin)
        NavBar.Location = New Point(285, 158)
        NavBar.Name = "NavBar"
        NavBar.Size = New Size(694, 514)
        NavBar.TabIndex = 1
        ' 
        ' LoginLabel
        ' 
        LoginLabel.Anchor = AnchorStyles.Right
        LoginLabel.AutoSize = True
        LoginLabel.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginLabel.ForeColor = Color.SeaGreen
        LoginLabel.Location = New Point(268, 201)
        LoginLabel.Name = "LoginLabel"
        LoginLabel.Size = New Size(63, 25)
        LoginLabel.TabIndex = 19
        LoginLabel.Text = "Login"
        ' 
        ' picHide
        ' 
        picHide.Anchor = AnchorStyles.None
        picHide.Location = New Point(630, 339)
        picHide.Name = "picHide"
        picHide.Size = New Size(34, 23)
        picHide.SizeMode = PictureBoxSizeMode.StretchImage
        picHide.TabIndex = 18
        picHide.TabStop = False
        ' 
        ' View
        ' 
        View.Anchor = AnchorStyles.None
        View.Location = New Point(630, 305)
        View.Name = "View"
        View.Size = New Size(34, 23)
        View.SizeMode = PictureBoxSizeMode.StretchImage
        View.TabIndex = 17
        View.TabStop = False
        ' 
        ' LoginIcon
        ' 
        LoginIcon.Anchor = AnchorStyles.None
        LoginIcon.Location = New Point(474, 61)
        LoginIcon.Margin = New Padding(3, 2, 3, 2)
        LoginIcon.Name = "LoginIcon"
        LoginIcon.Size = New Size(104, 101)
        LoginIcon.SizeMode = PictureBoxSizeMode.StretchImage
        LoginIcon.TabIndex = 16
        LoginIcon.TabStop = False
        ' 
        ' LoginBtn
        ' 
        LoginBtn.Anchor = AnchorStyles.None
        LoginBtn.BackColor = Color.DarkOrange
        LoginBtn.FlatAppearance.BorderSize = 0
        LoginBtn.FlatStyle = FlatStyle.Flat
        LoginBtn.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LoginBtn.ForeColor = Color.White
        LoginBtn.Location = New Point(553, 387)
        LoginBtn.Margin = New Padding(3, 2, 3, 2)
        LoginBtn.Name = "LoginBtn"
        LoginBtn.Size = New Size(111, 29)
        LoginBtn.TabIndex = 15
        LoginBtn.Text = "LOGIN"
        LoginBtn.UseVisualStyleBackColor = False
        ' 
        ' PasswordField
        ' 
        PasswordField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        PasswordField.BackColor = SystemColors.Window
        PasswordField.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        PasswordField.Location = New Point(391, 333)
        PasswordField.Margin = New Padding(3, 2, 3, 2)
        PasswordField.Name = "PasswordField"
        PasswordField.PlaceholderText = "Enter password"
        PasswordField.Size = New Size(273, 29)
        PasswordField.TabIndex = 14
        ' 
        ' UsernameField
        ' 
        UsernameField.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        UsernameField.BackColor = SystemColors.Window
        UsernameField.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        UsernameField.Location = New Point(391, 266)
        UsernameField.Margin = New Padding(3, 2, 3, 2)
        UsernameField.Name = "UsernameField"
        UsernameField.PlaceholderText = "Enter username"
        UsernameField.Size = New Size(273, 29)
        UsernameField.TabIndex = 13
        ' 
        ' Password
        ' 
        Password.Anchor = AnchorStyles.Right
        Password.AutoSize = True
        Password.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Password.Location = New Point(391, 310)
        Password.Name = "Password"
        Password.Size = New Size(82, 21)
        Password.TabIndex = 12
        Password.Text = "Password"
        ' 
        ' Username
        ' 
        Username.Anchor = AnchorStyles.Right
        Username.AutoSize = True
        Username.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Username.Location = New Point(391, 243)
        Username.Name = "Username"
        Username.Size = New Size(87, 21)
        Username.TabIndex = 11
        Username.Text = "Username"
        ' 
        ' DrugstoreName
        ' 
        DrugstoreName.Anchor = AnchorStyles.None
        DrugstoreName.AutoSize = True
        DrugstoreName.Font = New Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DrugstoreName.Location = New Point(376, 164)
        DrugstoreName.Name = "DrugstoreName"
        DrugstoreName.Size = New Size(297, 19)
        DrugstoreName.TabIndex = 10
        DrugstoreName.Text = "Cabrera's Drugstore and Medical Supplies "
        DrugstoreName.TextAlign = ContentAlignment.TopCenter
        ' 
        ' imageLogin
        ' 
        imageLogin.Location = New Point(0, 0)
        imageLogin.Name = "imageLogin"
        imageLogin.Size = New Size(348, 514)
        imageLogin.SizeMode = PictureBoxSizeMode.StretchImage
        imageLogin.TabIndex = 0
        imageLogin.TabStop = False
        ' 
        ' SuperAdmin
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1264, 830)
        Controls.Add(sideBarSuperAdmin)
        Controls.Add(motherPanel)
        Controls.Add(pnlLogin)
        Name = "SuperAdmin"
        Text = "Form1"
        WindowState = FormWindowState.Maximized
        sideBarSuperAdmin.ResumeLayout(False)
        sideBarSuperAdmin.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        motherPanel.ResumeLayout(False)
        pnlDashboard.ResumeLayout(False)
        pnlHeaderDashboard.ResumeLayout(False)
        pnlHeaderDashboard.PerformLayout()
        TableLayoutPanel4.ResumeLayout(False)
        TableLayoutPanel1.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        Panel2.ResumeLayout(False)
        Panel2.PerformLayout()
        Panel3.ResumeLayout(False)
        Panel3.PerformLayout()
        Panel4.ResumeLayout(False)
        Panel4.PerformLayout()
        TableLayoutPanel2.ResumeLayout(False)
        Panel5.ResumeLayout(False)
        Panel5.PerformLayout()
        Panel6.ResumeLayout(False)
        Panel6.PerformLayout()
        Panel7.ResumeLayout(False)
        Panel7.PerformLayout()
        Panel8.ResumeLayout(False)
        Panel8.PerformLayout()
        TableLayoutPanel3.ResumeLayout(False)
        pnlReports.ResumeLayout(False)
        TableLayoutPanel9.ResumeLayout(False)
        Panel23.ResumeLayout(False)
        Panel23.PerformLayout()
        TableLayoutPanel7.ResumeLayout(False)
        Panel19.ResumeLayout(False)
        Panel19.PerformLayout()
        Panel22.ResumeLayout(False)
        Panel22.PerformLayout()
        pnlHeaderReports.ResumeLayout(False)
        pnlHeaderReports.PerformLayout()
        Panel16.ResumeLayout(False)
        Panel16.PerformLayout()
        TableLayoutPanel8.ResumeLayout(False)
        Panel20.ResumeLayout(False)
        Panel20.PerformLayout()
        Panel21.ResumeLayout(False)
        Panel21.PerformLayout()
        TableLayoutPanel6.ResumeLayout(False)
        Panel17.ResumeLayout(False)
        Panel17.PerformLayout()
        Panel18.ResumeLayout(False)
        Panel18.PerformLayout()
        pnlInventoryStockReport.ResumeLayout(False)
        CType(inventoryDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelnvenotryStockReport.ResumeLayout(False)
        panelnvenotryStockReport.PerformLayout()
        Panel25.ResumeLayout(False)
        Panel25.PerformLayout()
        pnlFinancialSalesReport.ResumeLayout(False)
        CType(DataGridView4, ComponentModel.ISupportInitialize).EndInit()
        panelFinancialSalesReport.ResumeLayout(False)
        panelFinancialSalesReport.PerformLayout()
        Panel31.ResumeLayout(False)
        Panel31.PerformLayout()
        pnlRetailSalesReport.ResumeLayout(False)
        CType(retailDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelRetailSalesReport.ResumeLayout(False)
        panelRetailSalesReport.PerformLayout()
        Panel24.ResumeLayout(False)
        Panel24.PerformLayout()
        pnlExpensesReport.ResumeLayout(False)
        CType(expenseDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelExpenseReport.ResumeLayout(False)
        panelExpenseReport.PerformLayout()
        Panel35.ResumeLayout(False)
        Panel35.PerformLayout()
        pnlPurchasesReport.ResumeLayout(False)
        CType(purchasesDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelPurchasesReport.ResumeLayout(False)
        panelPurchasesReport.PerformLayout()
        Panel33.ResumeLayout(False)
        Panel33.PerformLayout()
        pnlWholesaleSalesReport.ResumeLayout(False)
        CType(wholesaleDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelWholesale.ResumeLayout(False)
        panelWholesale.PerformLayout()
        Panel29.ResumeLayout(False)
        Panel29.PerformLayout()
        pnlEmployeesalesReport.ResumeLayout(False)
        panelEmployeeDataGrid.ResumeLayout(False)
        panelEmployeeDataGrid.PerformLayout()
        CType(employeDataGrid, ComponentModel.ISupportInitialize).EndInit()
        panelEmployeeSalesReport.ResumeLayout(False)
        panelEmployeeSalesReport.PerformLayout()
        Panel37.ResumeLayout(False)
        Panel37.PerformLayout()
        pnlAccounts.ResumeLayout(False)
        pnlAccounts.PerformLayout()
        CType(accountsDataGrid, ComponentModel.ISupportInitialize).EndInit()
        accountTableLayout.ResumeLayout(False)
        panelSuperAdmin.ResumeLayout(False)
        panelSuperAdmin.PerformLayout()
        panelAdmin.ResumeLayout(False)
        panelAdmin.PerformLayout()
        panelAssistant.ResumeLayout(False)
        panelAssistant.PerformLayout()
        pnlHeaderAccounts.ResumeLayout(False)
        pnlHeaderAccounts.PerformLayout()
        pnlLogin.ResumeLayout(False)
        NavBar.ResumeLayout(False)
        NavBar.PerformLayout()
        CType(picHide, ComponentModel.ISupportInitialize).EndInit()
        CType(View, ComponentModel.ISupportInitialize).EndInit()
        CType(LoginIcon, ComponentModel.ISupportInitialize).EndInit()
        CType(imageLogin, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents sideBarSuperAdmin As Panel
    Friend WithEvents LogoutBtn As Button
    Friend WithEvents btnReports As Button
    Friend WithEvents btnAccounts As Button
    Friend WithEvents btnDashboard As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents storeName1 As Label
    Friend WithEvents storeName As Label
    Friend WithEvents btnLogin As Button
    Friend WithEvents motherPanel As Panel
    Friend WithEvents pnlDashboard As Panel
    Friend WithEvents pnlHeaderDashboard As Panel
    Friend WithEvents usrSuperAdmin As Label
    Friend WithEvents hdrStore As Label
    Friend WithEvents hdrDashboard As Label
    Friend WithEvents usrName As TextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Panel10 As Panel
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Panel12 As Panel
    Friend WithEvents userIDText As TextBox
    Friend WithEvents monthDateYear As TextBox
    Friend WithEvents todaysSale As TextBox
    Friend WithEvents transactionRetail As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents transactionsWholesale As TextBox
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents needOrdering As TextBox
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents days As TextBox
    Friend WithEvents TextBox12 As TextBox
    Friend WithEvents TextBox11 As TextBox
    Friend WithEvents allCategories As TextBox
    Friend WithEvents TextBox15 As TextBox
    Friend WithEvents TextBox14 As TextBox
    Friend WithEvents requiresDisposal As TextBox
    Friend WithEvents TextBox19 As TextBox
    Friend WithEvents TextBox17 As TextBox
    Friend WithEvents expensesRecorded As TextBox
    Friend WithEvents TextBox20 As TextBox
    Friend WithEvents TextBox18 As TextBox
    Friend WithEvents pnlLogin As Panel
    Friend WithEvents NavBar As Panel
    Friend WithEvents picHide As PictureBox
    Friend WithEvents View As PictureBox
    Friend WithEvents LoginIcon As PictureBox
    Friend WithEvents LoginBtn As Button
    Friend WithEvents PasswordField As TextBox
    Friend WithEvents UsernameField As TextBox
    Friend WithEvents Password As Label
    Friend WithEvents Username As Label
    Friend WithEvents DrugstoreName As Label
    Friend WithEvents imageLogin As PictureBox
    Friend WithEvents LoginLabel As Label
    Friend WithEvents pnlAccounts As Panel
    Friend WithEvents pnlHeaderAccounts As Panel
    Friend WithEvents TextBox23 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents hdrAccounts As Label
    Friend WithEvents pnlReports As Panel
    Friend WithEvents pnlHeaderReports As Panel
    Friend WithEvents TextBox24 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents btnAddAccount As Button
    Friend WithEvents accountTableLayout As TableLayoutPanel
    Friend WithEvents panelSuperAdmin As Panel
    Friend WithEvents titleSuperAdmin As TextBox
    Friend WithEvents numberSuperAdmin As TextBox
    Friend WithEvents panelAdmin As Panel
    Friend WithEvents numberAdmin As TextBox
    Friend WithEvents titleAdmin As TextBox
    Friend WithEvents panelAssistant As Panel
    Friend WithEvents numberAssistant As TextBox
    Friend WithEvents titleAssistant As TextBox
    Friend WithEvents numberTotalUsersRegistered As TextBox
    Friend WithEvents accountsDataGrid As DataGridView
    Friend WithEvents accountUserID As DataGridViewTextBoxColumn
    Friend WithEvents accountFullName As DataGridViewTextBoxColumn
    Friend WithEvents accountUsername As DataGridViewTextBoxColumn
    Friend WithEvents accountRole As DataGridViewTextBoxColumn
    Friend WithEvents accountStatus As DataGridViewTextBoxColumn
    Friend WithEvents accountLastLogin As DataGridViewTextBoxColumn
    Friend WithEvents accountEdit As DataGridViewButtonColumn
    Friend WithEvents accountResetPassword As DataGridViewButtonColumn
    Friend WithEvents accountEnableDisable As DataGridViewButtonColumn
    Friend WithEvents TableLayoutPanel8 As TableLayoutPanel
    Friend WithEvents Panel20 As Panel
    Friend WithEvents inventoryTitle As TextBox
    Friend WithEvents Panel21 As Panel
    Friend WithEvents retailTitle As TextBox
    Friend WithEvents Panel16 As Panel
    Friend WithEvents reportsDescription As TextBox
    Friend WithEvents titleBusinessReports As TextBox
    Friend WithEvents inventoryViewReport As Button
    Friend WithEvents inventoryDescriptions As TextBox
    Friend WithEvents retailViewReport As Button
    Friend WithEvents retailDescriptions As TextBox
    Friend WithEvents TableLayoutPanel9 As TableLayoutPanel
    Friend WithEvents Panel23 As Panel
    Friend WithEvents employeeViewReport As Button
    Friend WithEvents employeeDescription As TextBox
    Friend WithEvents employeeTitle As TextBox
    Friend WithEvents TableLayoutPanel7 As TableLayoutPanel
    Friend WithEvents Panel19 As Panel
    Friend WithEvents expensesViewReport As Button
    Friend WithEvents expensesDescriptions As TextBox
    Friend WithEvents expensesTitle As TextBox
    Friend WithEvents Panel22 As Panel
    Friend WithEvents financialViewReport As Button
    Friend WithEvents financialDescriptions As TextBox
    Friend WithEvents financialTitle As TextBox
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents Panel17 As Panel
    Friend WithEvents wholesaleViewReport As Button
    Friend WithEvents wholesaleDescriptions As TextBox
    Friend WithEvents wholesaleTitle As TextBox
    Friend WithEvents Panel18 As Panel
    Friend WithEvents purchasesViewReport As Button
    Friend WithEvents purchasesDescriptions As TextBox
    Friend WithEvents purchasesTitle As TextBox
    Friend WithEvents pnlInventoryStockReport As Panel
    Friend WithEvents Panel25 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents panelnvenotryStockReport As Panel
    Friend WithEvents inventoryGenerated As TextBox
    Friend WithEvents titleInventoryStockReport As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents TextBox50 As TextBox
    Friend WithEvents inventoryDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents inventoryCategory As DataGridViewTextBoxColumn
    Friend WithEvents inventoryUnit As DataGridViewTextBoxColumn
    Friend WithEvents inventoryStock As DataGridViewTextBoxColumn
    Friend WithEvents inventoryPackage As DataGridViewTextBoxColumn
    Friend WithEvents inventoryBatch As DataGridViewTextBoxColumn
    Friend WithEvents inventoryExpiry As DataGridViewTextBoxColumn
    Friend WithEvents inventorySupplier As DataGridViewTextBoxColumn
    Friend WithEvents inventoryStatus As DataGridViewTextBoxColumn
    Friend WithEvents pnlRetailSalesReport As Panel
    Friend WithEvents Panel24 As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents TextBox48 As TextBox
    Friend WithEvents panelRetailSalesReport As Panel
    Friend WithEvents retailGenerated As TextBox
    Friend WithEvents titleRetailSalesReport As TextBox
    Friend WithEvents retailDataGrid As DataGridView
    Friend WithEvents retailTransactionID As DataGridViewTextBoxColumn
    Friend WithEvents retailDate As DataGridViewTextBoxColumn
    Friend WithEvents retailCustomer As DataGridViewTextBoxColumn
    Friend WithEvents retailCashier As DataGridViewTextBoxColumn
    Friend WithEvents retailType As DataGridViewTextBoxColumn
    Friend WithEvents retailTotal As DataGridViewTextBoxColumn
    Friend WithEvents retailStatus As DataGridViewTextBoxColumn
    Friend WithEvents pnlWholesaleSalesReport As Panel
    Friend WithEvents wholesaleDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents panelWholesale As Panel
    Friend WithEvents Button9 As Button
    Friend WithEvents employeeGenerated As TextBox
    Friend WithEvents titleWholesaleSalesReport As TextBox
    Friend WithEvents Panel29 As Panel
    Friend WithEvents Label13 As Label
    Friend WithEvents TextBox54 As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents pnlFinancialSalesReport As Panel
    Friend WithEvents DataGridView4 As DataGridView
    Friend WithEvents panelFinancialSalesReport As Panel
    Friend WithEvents Button10 As Button
    Friend WithEvents financialGenerated As TextBox
    Friend WithEvents titleFinancialSalesReport As TextBox
    Friend WithEvents Panel31 As Panel
    Friend WithEvents Label16 As Label
    Friend WithEvents TextBox57 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents financialTransactionID As DataGridViewTextBoxColumn
    Friend WithEvents financialDate As DataGridViewTextBoxColumn
    Friend WithEvents financialCustomer As DataGridViewTextBoxColumn
    Friend WithEvents financialCashier As DataGridViewTextBoxColumn
    Friend WithEvents financialType As DataGridViewTextBoxColumn
    Friend WithEvents financialTotal As DataGridViewTextBoxColumn
    Friend WithEvents financialStatus As DataGridViewTextBoxColumn
    Friend WithEvents financialPeriodDropdown As ComboBox
    Friend WithEvents financialPrintReport As Button
    Friend WithEvents wholesaleScheduleDropdown As ComboBox
    Friend WithEvents wholesalePrintReport As Button
    Friend WithEvents inventoryPeriodDropdown As ComboBox
    Friend WithEvents inventoryPrintReport As Button
    Friend WithEvents ComboBox4 As ComboBox
    Friend WithEvents retailPrintReport As Button
    Friend WithEvents pnlPurchasesReport As Panel
    Friend WithEvents purchasesDataGrid As DataGridView
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As DataGridViewTextBoxColumn
    Friend WithEvents panelPurchasesReport As Panel
    Friend WithEvents ComboBox5 As ComboBox
    Friend WithEvents Button14 As Button
    Friend WithEvents purchasesSearchBar As TextBox
    Friend WithEvents titlePurchasesReport As TextBox
    Friend WithEvents Panel33 As Panel
    Friend WithEvents Label19 As Label
    Friend WithEvents TextBox60 As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents purchasesDateTime As DateTimePicker
    Friend WithEvents TextBox61 As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents pnlExpensesReport As Panel
    Friend WithEvents expenseDataGrid As DataGridView
    Friend WithEvents panelExpenseReport As Panel
    Friend WithEvents expensesPeriodDropdown As ComboBox
    Friend WithEvents btnAddExpenses As Button
    Friend WithEvents expensesDateTime As DateTimePicker
    Friend WithEvents ComboBox6 As ComboBox
    Friend WithEvents Button15 As Button
    Friend WithEvents expensesSearchBar As TextBox
    Friend WithEvents titleExpensesReport As TextBox
    Friend WithEvents Panel35 As Panel
    Friend WithEvents TextBox66 As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents TextBox64 As TextBox
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents TextBox65 As TextBox
    Friend WithEvents Label25 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents expensesExpenseID As DataGridViewTextBoxColumn
    Friend WithEvents expensesDate As DataGridViewTextBoxColumn
    Friend WithEvents expensesCategory As DataGridViewTextBoxColumn
    Friend WithEvents expensesDescription As DataGridViewTextBoxColumn
    Friend WithEvents expensesAmount As DataGridViewTextBoxColumn
    Friend WithEvents expensesRecordedBy As DataGridViewTextBoxColumn
    Friend WithEvents pnlEmployeesalesReport As Panel
    Friend WithEvents employeDataGrid As DataGridView
    Friend WithEvents panelEmployeeSalesReport As Panel
    Friend WithEvents employeeNamesDropdown As ComboBox
    Friend WithEvents employeePrintReport As Button
    Friend WithEvents Button18 As Button
    Friend WithEvents TextBox67 As TextBox
    Friend WithEvents titleEmployeeSalesReport As TextBox
    Friend WithEvents Panel37 As Panel
    Friend WithEvents Label28 As Label
    Friend WithEvents TextBox69 As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents Label30 As Label
    Friend WithEvents panelEmployeeDataGrid As Panel
    Friend WithEvents employeeScheduleDropdown As ComboBox
    Friend WithEvents employeeName As TextBox
    Friend WithEvents employeeTransactionID As DataGridViewTextBoxColumn
    Friend WithEvents employeeDate As DataGridViewTextBoxColumn
    Friend WithEvents employeeCustomer As DataGridViewTextBoxColumn
    Friend WithEvents employeeCashier As DataGridViewTextBoxColumn
    Friend WithEvents employeeType As DataGridViewTextBoxColumn
    Friend WithEvents employeeTotal As DataGridViewTextBoxColumn
    Friend WithEvents employeeStatus As DataGridViewTextBoxColumn
    Friend WithEvents TextBox68 As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents TextBox25 As TextBox
    Friend WithEvents Label32 As Label

End Class