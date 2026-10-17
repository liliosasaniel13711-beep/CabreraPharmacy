Imports System.Drawing
Imports System.Windows.Forms
Imports System.Data
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Diagnostics
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class Admin

    ' ColorDef
    Private darkGreen As Color = Color.FromArgb(20, 100, 60)
    Private activeGreen As Color = Color.FromArgb(25, 120, 75)
    Private lightGray As Color = Color.FromArgb(245, 245, 245)
    Private yellow As Color = Color.FromArgb(255, 215, 0)

    ' Navigation state
    Private navButtons As Button()
    Private contentPanels As Dictionary(Of String, Panel)
    Private inventoryRows As DataTable
    Private inventorySearchBox As TextBox
    Private inventoryFilterButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private inventoryActiveFilter As String = "All"
    Private priceManagerProducts As DataTable
    Private priceManagerSearchBox As TextBox
    Private priceManagerCategoryFilter As ComboBox
    Private priceManagerProductList As FlowLayoutPanel
    Private salesReportPeriodCombo As ComboBox
    Private salesReportTypeCombo As ComboBox
    Private salesReportDatePicker As DateTimePicker
    Private salesReportMonthCombo As ComboBox
    Private salesReportYearCombo As ComboBox
    Private salesReportFilterPanel As Panel
    Private salesReportGrid As DataGridView
    Private salesReportTotalLabel As Label
    Private salesReportDetailsLabel As Label

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        ' Welcome logic
        ' Make sure CurrentFullName is declared globally in a module (like SessionModule)
        welcomeadmin_label.Text = "Welcome, " & CurrentFullName & " (Admin)"

        ' Map nav buttons and views
        navButtons = {navDashboard, navPriceManager, navInventory, navReports}
        contentPanels = New Dictionary(Of String, Panel) From {
            {"Dashboard", dashboardView},
            {"Price Manager", priceManagerView},
            {"Inventory", inventoryView},
            {"Reports", reportsView}
        }

        ' Dynamic values that the Designer can't serialize
        card1Footer.Text = DateTime.Now.ToString("MMMM dd, yyyy")

        InitializeInventoryUi()
        InitializePriceManagerUi()
        InitializeReportsUi()
        ShowView("Dashboard")
    End Sub

    ' Keep the yellow logo vertically centered against the brand text
    Private Sub Sidebar_Layout(sender As Object, e As LayoutEventArgs) Handles sidebar.Layout
        logoBox.Location = New Point(20, PharName.Top + (PharName.Height - logoBox.Height) \ 2)
    End Sub

    ' Keep the user info panel pinned to the top-right corner
    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs) Handles navDashboard.Click, navPriceManager.Click, navInventory.Click, navReports.Click
        Dim btn = TryCast(sender, Button)
        If btn IsNot Nothing Then
            ShowView(CStr(btn.Tag))
        End If
    End Sub

    Private Sub logoutadmin_button_Click(sender As Object, e As EventArgs) Handles logoutadmin_button.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Me.Close()
    End Sub

    ' Switch the visible view and highlight the active nav button
    Private Sub ShowView(viewName As String)
        For Each kvp In contentPanels
            kvp.Value.Visible = (kvp.Key = viewName)
        Next

        For Each btn In navButtons
            Dim isActive As Boolean = (CStr(btn.Tag) = viewName)
            btn.BackColor = If(isActive, darkGreen, Color.White)
            btn.ForeColor = If(isActive, Color.White, darkGreen)
        Next

        ' Trigger backend data retrieval based on the active view
        Select Case viewName
            Case "Dashboard"
                LoadDashboardStats()
            Case "Inventory"
                LoadInventoryData()
            Case "Price Manager"
                LoadPriceManagerData()
        End Select

        titleLabel.Text = viewName
        subtitleLabel.Text = GetSubtitle(viewName)
    End Sub

    Private Function GetSubtitle(viewName As String) As String
        Select Case viewName
            Case "Dashboard"
                Return Date.Today.ToString("MMMM yyyy", CultureInfo.CurrentCulture) & " · Overview of Cabrera’s Drugstore and Medical Supplies"
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

    Private Sub InitializeReportsUi()
        For Each existing As Control In reportsView.Controls.Cast(Of Control)().ToArray()
            existing.Dispose()
        Next
        reportsView.Controls.Clear()
        reportsView.BackColor = lightGray
        reportsView.Padding = New Padding(14)

        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = lightGray, .Margin = New Padding(0)}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 76))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim heading As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12), .Padding = New Padding(18, 12, 18, 10)}
        heading.Controls.Add(New Label With {.Text = "Select reports to view.", .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.Gray, .Location = New Point(18, 42)})
        heading.Controls.Add(New Label With {.Text = "Business Reports", .AutoSize = True, .Font = New Font("Segoe UI", 14.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 55, 45), .Location = New Point(18, 12)})

        Dim reportList As New FlowLayoutPanel With {.Dock = DockStyle.Fill, .AutoScroll = True, .WrapContents = True, .FlowDirection = FlowDirection.LeftToRight, .BackColor = lightGray, .Padding = New Padding(0), .Margin = New Padding(0)}
        AddHandler reportList.ClientSizeChanged, AddressOf ReportsList_Resize
        Dim reportDefinitions As New List(Of Tuple(Of String, String)) From {
            Tuple.Create("Inventory Stock Report", "Complete list of products with current stock levels, batches, and expiry information."),
            Tuple.Create("Sales Report", "Detailed breakdown of retail and wholesale transactions within a selected period."),
            Tuple.Create("Purchases Report", "Product deliveries and restocking events with supplier and cost details."),
            Tuple.Create("Expenses Report", "Recorded business expenses categorized by type and date range."),
            Tuple.Create("Financial Report", "Revenue, cost of goods, expenses, and net profit summary."),
            Tuple.Create("Employee Sales Report", "Sales performance by cashier for accountability and monitoring.")
        }
        For Each definition In reportDefinitions
            reportList.Controls.Add(CreateReportCard(definition.Item1, definition.Item2, reportList.ClientSize.Width))
        Next

        layout.Controls.Add(heading, 0, 0)
        layout.Controls.Add(reportList, 0, 1)
        reportsView.Controls.Add(layout)
    End Sub

    Private Sub ReportsList_Resize(sender As Object, e As EventArgs)
        Dim reportList = TryCast(sender, FlowLayoutPanel)
        If reportList Is Nothing Then Return
        Dim cardWidth = Math.Max(300, (reportList.ClientSize.Width - 30) \ 2)
        For Each reportCard As Control In reportList.Controls
            reportCard.Width = cardWidth
            If reportCard.Controls.Count > 1 Then reportCard.Controls(1).Width = cardWidth - 32
        Next
    End Sub

    Private Function CreateReportCard(title As String, description As String, availableWidth As Integer) As Panel
        Dim cardWidth = If(availableWidth > 0, Math.Max(300, (availableWidth - 30) \ 2), 440)
        Dim card As New Panel With {.Width = cardWidth, .Height = 124, .BackColor = Color.White, .Margin = New Padding(0, 0, 14, 14), .Padding = New Padding(16), .BorderStyle = BorderStyle.FixedSingle}
        Dim titleLabel As New Label With {.Text = title, .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 45, 40), .Location = New Point(16, 12)}
        Dim descriptionLabel As New Label With {.Text = description, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(16, 39), .Size = New Size(card.Width - 32, 34), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
        Dim viewButton As New Button With {.Text = "View Report", .Size = New Size(116, 34), .Location = New Point(16, 80), .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        viewButton.FlatAppearance.BorderSize = 0
        card.Controls.Add(titleLabel)
        card.Controls.Add(descriptionLabel)
        card.Controls.Add(viewButton)
        If String.Equals(title, "Sales Report", StringComparison.Ordinal) Then
            AddHandler viewButton.Click, Sub(sender, e) ShowSalesReport()
        End If
        Return card
    End Function

    Private Sub ShowSalesReport()
        reportsView.Controls.Clear()
        reportsView.Padding = New Padding(14)
        reportsView.BackColor = lightGray

        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = lightGray, .Margin = New Padding(0)}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 104))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim header As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12)}
        header.Controls.Add(New Label With {.Text = "Sales Report", .AutoSize = True, .Font = New Font("Segoe UI", 14.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 45, 40), .Location = New Point(18, 12)})
        header.Controls.Add(New Label With {.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt", CultureInfo.CurrentCulture) & " | By: " & CurrentFullName, .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(20, 42)})
        Dim backButton As New Button With {.Text = "Back to Reports", .Size = New Size(126, 32), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = Color.White, .ForeColor = darkGreen, .FlatStyle = FlatStyle.Flat}
        backButton.Location = New Point(header.ClientSize.Width - backButton.Width - 16, 12)
        AddHandler header.Resize, Sub(sender, e) backButton.Left = header.ClientSize.Width - backButton.Width - 16
        AddHandler backButton.Click, Sub(sender, e) InitializeReportsUi()
        header.Controls.Add(backButton)
        Dim printReportButton As New Button With {.Text = "Print Report", .Size = New Size(112, 32), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        printReportButton.FlatAppearance.BorderSize = 0
        printReportButton.Location = New Point(backButton.Left - printReportButton.Width - 10, 12)
        AddHandler header.Resize, Sub(sender, e) printReportButton.Left = backButton.Left - printReportButton.Width - 10
        AddHandler printReportButton.Click, AddressOf PrintSalesReport
        header.Controls.Add(printReportButton)

        salesReportPeriodCombo = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F), .Size = New Size(150, 30), .Location = New Point(18, 66)}
        salesReportPeriodCombo.Items.AddRange(New Object() {"Daily", "Monthly", "Yearly"})
        salesReportPeriodCombo.SelectedItem = "Daily"
        salesReportTypeCombo = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F), .Size = New Size(150, 30), .Anchor = AnchorStyles.Top Or AnchorStyles.Right}
        salesReportTypeCombo.Items.AddRange(New Object() {"All", "Retail", "Wholesale"})
        salesReportTypeCombo.SelectedItem = "All"
        salesReportTypeCombo.Location = New Point(header.ClientSize.Width - salesReportTypeCombo.Width - 16, 66)
        AddHandler header.Resize, Sub(sender, e) salesReportTypeCombo.Left = header.ClientSize.Width - salesReportTypeCombo.Width - 16
        header.Controls.Add(salesReportPeriodCombo)
        header.Controls.Add(salesReportTypeCombo)

        Dim body As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .BackColor = lightGray, .Margin = New Padding(0)}
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 225))
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        salesReportFilterPanel = New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(14), .Margin = New Padding(0, 0, 12, 0)}
        salesReportDetailsLabel = New Label With {.Text = "Transaction Details", .Dock = DockStyle.Top, .Height = 42, .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 45, 40), .Padding = New Padding(16, 10, 0, 0)}
        Dim reportPanel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(0), .Margin = New Padding(0)}
        salesReportGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AllowUserToResizeRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .GridColor = Color.FromArgb(230, 234, 231),
            .EnableHeadersVisualStyles = False
        }
        salesReportGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 246)
        salesReportGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray
        salesReportGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        salesReportGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        salesReportGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
        salesReportGrid.DefaultCellStyle.SelectionForeColor = Color.Black
        salesReportGrid.RowTemplate.Height = 36
        AddHandler salesReportGrid.CellClick, AddressOf SalesReportGrid_CellClick
        salesReportTotalLabel = New Label With {.Text = "TOTAL SALES: ₱0.00", .Dock = DockStyle.Bottom, .Height = 44, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 18, 0)}
        reportPanel.Controls.Add(salesReportGrid)
        reportPanel.Controls.Add(salesReportTotalLabel)
        reportPanel.Controls.Add(salesReportDetailsLabel)

        body.Controls.Add(salesReportFilterPanel, 0, 0)
        body.Controls.Add(reportPanel, 1, 0)
        layout.Controls.Add(header, 0, 0)
        layout.Controls.Add(body, 0, 1)
        reportsView.Controls.Add(layout)

        AddHandler salesReportPeriodCombo.SelectedIndexChanged, Sub(sender, e) ConfigureSalesReportFilters()
        AddHandler salesReportTypeCombo.SelectedIndexChanged, Sub(sender, e) LoadSalesReportRows()
        ConfigureSalesReportFilters()
    End Sub

    Private Sub ConfigureSalesReportFilters()
        If salesReportFilterPanel Is Nothing Then Return
        salesReportFilterPanel.Controls.Clear()
        salesReportDatePicker = Nothing
        salesReportMonthCombo = Nothing
        salesReportYearCombo = Nothing
        Dim period = Convert.ToString(salesReportPeriodCombo.SelectedItem)
        AddFilterHeader("Filter by " & period.ToLowerInvariant())

        If String.Equals(period, "Daily", StringComparison.Ordinal) Then
            Dim firstAvailableDate As New DateTime(2026, 9, 1)
            Dim lastAvailableDate As New DateTime(2026, 10, 31)
            If Date.Today < lastAvailableDate Then lastAvailableDate = Date.Today
            salesReportFilterPanel.Controls.Add(New Label With {.Text = "Date", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(14, 60)})
            salesReportDatePicker = New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "MMMM dd, yyyy",
                .MinDate = firstAvailableDate,
                .MaxDate = lastAvailableDate,
                .Value = lastAvailableDate,
                .Location = New Point(14, 82),
                .Width = 190,
                .Font = New Font("Segoe UI", 9.0F)
            }
            salesReportFilterPanel.Controls.Add(salesReportDatePicker)
        ElseIf String.Equals(period, "Monthly", StringComparison.Ordinal) Then
            salesReportMonthCombo = AddFilterCombo("Month", {"September", "October"}, "October", 60)
            salesReportYearCombo = AddFilterCombo("Year", {"2026"}, "2026", 145)
        Else
            salesReportYearCombo = AddFilterCombo("Year", {"2026"}, "2026", 60)
        End If

        If salesReportDatePicker IsNot Nothing Then AddHandler salesReportDatePicker.ValueChanged, Sub(sender, e) LoadSalesReportRows()
        If salesReportMonthCombo IsNot Nothing Then AddHandler salesReportMonthCombo.SelectedIndexChanged, Sub(sender, e) LoadSalesReportRows()
        If salesReportYearCombo IsNot Nothing Then AddHandler salesReportYearCombo.SelectedIndexChanged, Sub(sender, e) LoadSalesReportRows()
        LoadSalesReportRows()
    End Sub

    Private Sub AddFilterHeader(text As String)
        salesReportFilterPanel.Controls.Add(New Label With {.Text = text, .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(14, 14)})
    End Sub

    Private Function AddFilterCombo(labelText As String, items As String(), selected As String, top As Integer) As ComboBox
        salesReportFilterPanel.Controls.Add(New Label With {.Text = labelText, .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(14, top)})
        Dim combo As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F), .Location = New Point(14, top + 20), .Width = 190}
        combo.Items.AddRange(items.Cast(Of Object)().ToArray())
        combo.SelectedItem = selected
        salesReportFilterPanel.Controls.Add(combo)
        Return combo
    End Function

    Private Sub LoadSalesReportRows()
        If salesReportGrid Is Nothing OrElse salesReportPeriodCombo Is Nothing Then Return
        Try
            Dim period = Convert.ToString(salesReportPeriodCombo.SelectedItem)
            Dim year As Integer = 2026
            If Not String.Equals(period, "Daily", StringComparison.Ordinal) Then
                If salesReportYearCombo Is Nothing OrElse salesReportYearCombo.SelectedItem Is Nothing Then Return
                year = Integer.Parse(Convert.ToString(salesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
            End If
            Dim startDate As DateTime
            Dim endDate As DateTime
            Dim periodCaption As String

            If String.Equals(period, "Daily", StringComparison.Ordinal) Then
                If salesReportDatePicker Is Nothing Then Return
                startDate = salesReportDatePicker.Value.Date
                endDate = startDate.AddDays(1)
                periodCaption = startDate.ToString("MMMM dd, yyyy", CultureInfo.CurrentCulture)
            ElseIf String.Equals(period, "Yearly", StringComparison.Ordinal) Then
                startDate = New DateTime(year, 1, 1)
                endDate = startDate.AddYears(1)
                periodCaption = year.ToString(CultureInfo.InvariantCulture)
            Else
                If salesReportMonthCombo Is Nothing OrElse salesReportMonthCombo.SelectedItem Is Nothing Then Return
                Dim month = DateTime.ParseExact(Convert.ToString(salesReportMonthCombo.SelectedItem), "MMMM", CultureInfo.InvariantCulture).Month
                startDate = New DateTime(year, month, 1)
                endDate = startDate.AddMonths(1)
                periodCaption = startDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture)
            End If

            Dim selectedType = Convert.ToString(salesReportTypeCombo.SelectedItem)
            Dim mode = If(String.Equals(selectedType, "All", StringComparison.Ordinal), Nothing, selectedType)
            Dim allRows = New SalesRepository().GetTransactionReport(mode, Nothing, startDate, endDate)
            Dim rows = allRows.Clone()
            For Each row As DataRow In allRows.Rows
                rows.ImportRow(row)
            Next

            salesReportGrid.DataSource = rows
            If salesReportGrid.Columns.Contains("SaleDate") Then salesReportGrid.Columns("SaleDate").DefaultCellStyle.Format = "yyyy-MM-dd HH:mm"
            If salesReportGrid.Columns.Contains("Total") Then salesReportGrid.Columns("Total").DefaultCellStyle.Format = "₱#,##0.00"
            Dim total As Decimal = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
            salesReportDetailsLabel.Text = "Transaction Details — " & periodCaption
            salesReportTotalLabel.Text = "TOTAL SALES: ₱" & total.ToString("N2", CultureInfo.CurrentCulture)
        Catch ex As Exception
            MessageBox.Show("Could not load the sales report. " & ex.Message, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintSalesReport(sender As Object, e As EventArgs)
        Try
            If salesReportGrid Is Nothing OrElse salesReportPeriodCombo Is Nothing Then Return
            Dim rows = TryCast(salesReportGrid.DataSource, DataTable)
            If rows Is Nothing Then Return

            Dim period = Convert.ToString(salesReportPeriodCombo.SelectedItem)
            Dim saleType = Convert.ToString(salesReportTypeCombo.SelectedItem)
            Dim periodCaption = salesReportDetailsLabel.Text.Replace("Transaction Details — ", String.Empty)
            Dim title = period & " Sales Report"
            Dim saleDate As DateTime
            Dim reportStart As DateTime
            Dim reportEnd As DateTime
            Dim destinationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "sales_report", period.ToLowerInvariant())
            Dim fileSuffix As String

            If String.Equals(period, "Daily", StringComparison.Ordinal) Then
                saleDate = salesReportDatePicker.Value.Date
                reportStart = saleDate
                reportEnd = saleDate.AddDays(1)
                fileSuffix = saleDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            ElseIf String.Equals(period, "Monthly", StringComparison.Ordinal) Then
                Dim year = Integer.Parse(Convert.ToString(salesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                Dim month = DateTime.ParseExact(Convert.ToString(salesReportMonthCombo.SelectedItem), "MMMM", CultureInfo.InvariantCulture).Month
                saleDate = New DateTime(year, month, 1)
                reportStart = saleDate
                reportEnd = saleDate.AddMonths(1)
                destinationFolder = Path.Combine(destinationFolder, year.ToString(CultureInfo.InvariantCulture), saleDate.ToString("MMMM", CultureInfo.InvariantCulture))
                fileSuffix = saleDate.ToString("MMMM_yyyy", CultureInfo.InvariantCulture)
            Else
                Dim year = Integer.Parse(Convert.ToString(salesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                saleDate = New DateTime(year, 1, 1)
                reportStart = saleDate
                reportEnd = saleDate.AddYears(1)
                destinationFolder = Path.Combine(destinationFolder, year.ToString(CultureInfo.InvariantCulture))
                fileSuffix = year.ToString(CultureInfo.InvariantCulture)
            End If

            Dim mode = If(String.Equals(saleType, "All", StringComparison.OrdinalIgnoreCase), Nothing, saleType)
            Dim itemDetails = New SalesRepository().GetSalesReportItemDetails(mode, reportStart, reportEnd)
            Dim total = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
            Dim retailRows = rows.AsEnumerable().Where(Function(row) String.Equals(Convert.ToString(row("SaleType")), "Retail", StringComparison.OrdinalIgnoreCase)).ToList()
            Dim wholesaleRows = rows.AsEnumerable().Where(Function(row) String.Equals(Convert.ToString(row("SaleType")), "Wholesale", StringComparison.OrdinalIgnoreCase)).ToList()
            Dim itemCount = itemDetails.AsEnumerable().Sum(Function(row) If(row.IsNull("Quantity"), 0, Convert.ToInt32(row("Quantity"))))
            Dim lines As New List(Of String) From {
                "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES",
                title,
                "@META|Report Period: " & periodCaption & "|Sale Type: " & saleType,
                "@META|Generated: " & DateTime.Now.ToString("MMMM dd, yyyy h:mm tt", CultureInfo.CurrentCulture) & "|Prepared by: " & CurrentFullName,
                "@SECTION|SALES SUMMARY",
                "@SUMMARY|Transactions|" & rows.Rows.Count.ToString(CultureInfo.InvariantCulture) & "|Items Sold|" & itemCount.ToString(CultureInfo.InvariantCulture),
                "@TOTAL|TOTAL SALES|PHP " & total.ToString("N2", CultureInfo.InvariantCulture),
                "@SECTION|TRANSACTION DETAILS"
            }

            If String.Equals(saleType, "All", StringComparison.OrdinalIgnoreCase) Then
                lines.Insert(6, "@SUMMARY|Retail Sales|PHP " & retailRows.Sum(Function(row) Convert.ToDecimal(row("Total"))).ToString("N2", CultureInfo.InvariantCulture) & "|Wholesale Sales|PHP " & wholesaleRows.Sum(Function(row) Convert.ToDecimal(row("Total"))).ToString("N2", CultureInfo.InvariantCulture))
            ElseIf String.Equals(saleType, "Retail", StringComparison.OrdinalIgnoreCase) Then
                lines.Insert(6, "@SUMMARY|Retail Sales|PHP " & retailRows.Sum(Function(row) Convert.ToDecimal(row("Total"))).ToString("N2", CultureInfo.InvariantCulture))
            ElseIf String.Equals(saleType, "Wholesale", StringComparison.OrdinalIgnoreCase) Then
                lines.Insert(6, "@SUMMARY|Wholesale Sales|PHP " & wholesaleRows.Sum(Function(row) Convert.ToDecimal(row("Total"))).ToString("N2", CultureInfo.InvariantCulture))
            End If

            If String.Equals(period, "Yearly", StringComparison.OrdinalIgnoreCase) Then
                Dim monthlyTotals = Enumerable.Range(1, 12).Select(Function(monthNumber) New With {
                    .MonthLabel = New DateTime(saleDate.Year, monthNumber, 1).ToString("MMMM", CultureInfo.CurrentCulture),
                    .Sales = rows.AsEnumerable().Where(Function(row) Convert.ToDateTime(row("SaleDate")).Month = monthNumber).Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
                }).ToList()
                Dim insertAt = lines.FindIndex(Function(line) line.StartsWith("@TOTAL|", StringComparison.Ordinal))
                For monthIndex = 0 To monthlyTotals.Count - 1 Step 2
                    Dim leftMonth = monthlyTotals(monthIndex)
                    If monthIndex + 1 < monthlyTotals.Count Then
                        Dim rightMonth = monthlyTotals(monthIndex + 1)
                        lines.Insert(insertAt, "@MONTHSUMMARY|" & leftMonth.MonthLabel & "|PHP " & leftMonth.Sales.ToString("N2", CultureInfo.InvariantCulture) &
                                     "|" & rightMonth.MonthLabel & "|PHP " & rightMonth.Sales.ToString("N2", CultureInfo.InvariantCulture))
                    Else
                        lines.Insert(insertAt, "@MONTHSUMMARY|" & leftMonth.MonthLabel & "|PHP " & leftMonth.Sales.ToString("N2", CultureInfo.InvariantCulture))
                    End If
                    insertAt += 1
                Next
            End If

            Const tableHeader = "@TABLE|TRANSACTION ID|SALE DATE / TIME|CUSTOMER|CASHIER|TYPE|MEDICINE / DETAILS|QTY|UNIT PRICE|AMOUNT TOTAL"
            If String.Equals(period, "Yearly", StringComparison.OrdinalIgnoreCase) Then
                Dim monthGroups = itemDetails.AsEnumerable().GroupBy(Function(row) Convert.ToDateTime(row("SaleDate")).Month).OrderBy(Function(group) group.Key)
                For Each monthGroup In monthGroups
                    Dim monthRows = rows.AsEnumerable().Where(Function(row) Convert.ToDateTime(row("SaleDate")).Month = monthGroup.Key).ToList()
                    Dim monthDate = New DateTime(saleDate.Year, monthGroup.Key, 1)
                    Dim monthSales = monthRows.Sum(Function(row) Convert.ToDecimal(row("Total")))
                    lines.Add("@MONTH|" & monthDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture) &
                              "|Transactions: " & monthRows.Count.ToString(CultureInfo.InvariantCulture) &
                              "|Sales: PHP " & monthSales.ToString("N2", CultureInfo.InvariantCulture))
                    lines.Add(tableHeader)
                    For Each row As DataRow In monthGroup
                        AddSalesReportItemLine(lines, row)
                    Next
                Next
            Else
                lines.Add(tableHeader)
                For Each row As DataRow In itemDetails.Rows
                    AddSalesReportItemLine(lines, row)
                Next
            End If

            If itemDetails.Rows.Count = 0 Then lines.Add("@NOTE|No transaction items were recorded for this period.")

            Directory.CreateDirectory(destinationFolder)
            Dim reportFilePrefix = If(String.Equals(saleType, "Retail", StringComparison.OrdinalIgnoreCase),
                                      "retail_sales_report",
                                      If(String.Equals(saleType, "Wholesale", StringComparison.OrdinalIgnoreCase), "wholesale_sales_report", "sales_summary"))
            Dim outputPath = Path.Combine(destinationFolder, reportFilePrefix & "_" & fileSuffix & ".pdf")
            Staff.CreateReportPdf(outputPath, lines)
            Process.Start(New ProcessStartInfo(outputPath) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to create or open the sales report PDF. " & ex.Message, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AddSalesReportItemLine(lines As List(Of String), row As DataRow)
        Dim saleAt = Convert.ToDateTime(row("SaleDate"))
        Dim medicineName = Convert.ToString(row("BrandName"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("GenericName"))) Then medicineName &= " / " & Convert.ToString(row("GenericName"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("Dosage"))) Then medicineName &= " / " & Convert.ToString(row("Dosage"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("Unit"))) Then medicineName &= " (" & Convert.ToString(row("Unit")) & ")"
        lines.Add(String.Join("|", {
            "@TABLE",
            Convert.ToString(row("TransactionID"), CultureInfo.InvariantCulture),
            saleAt.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
            Convert.ToString(row("Customer")),
            Convert.ToString(row("Cashier")),
            Convert.ToString(row("SaleType")),
            medicineName,
            Convert.ToString(row("Quantity"), CultureInfo.InvariantCulture),
            "PHP " & Convert.ToDecimal(row("UnitPrice")).ToString("N2", CultureInfo.InvariantCulture),
            "PHP " & Convert.ToDecimal(row("LineTotal")).ToString("N2", CultureInfo.InvariantCulture)
        }))
    End Sub

    Private Sub SalesReportGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse salesReportGrid Is Nothing OrElse Not salesReportGrid.Columns.Contains("TransactionID") Then Return
        Dim value = salesReportGrid.Rows(e.RowIndex).Cells("TransactionID").Value
        If value Is Nothing OrElse value Is DBNull.Value Then Return
        ShowTransactionDetailsDialog(Convert.ToInt64(value))
    End Sub

    Private Sub ShowTransactionDetailsDialog(saleId As Long)
        Try
            Dim details = New SalesRepository().GetSaleReceiptDetails(saleId)
            If details.Rows.Count = 0 Then
                MessageBox.Show("The transaction details could not be found.", "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim sale = details.Rows(0)
            Dim saleDate = Convert.ToDateTime(sale("created_at"))
            Dim receiptNumber = "RCPT-" & saleId.ToString("D8", CultureInfo.InvariantCulture)
            Dim receiptLines = BuildSavedReceiptLines(details, receiptNumber, saleId, saleDate)

            Using dialog As New Form()
                dialog.FormBorderStyle = FormBorderStyle.None
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.ClientSize = New Size(700, 620)
                dialog.BackColor = Color.White
                dialog.Font = New Font("Segoe UI", 9.0F)
                dialog.ShowInTaskbar = False
                AddHandler dialog.Paint,
                    Sub(sender, e)
                        Using borderPen As New Pen(Color.Black, 2.0F)
                            e.Graphics.DrawRectangle(borderPen, 1, 1, dialog.ClientSize.Width - 3, dialog.ClientSize.Height - 3)
                        End Using
                    End Sub

                Dim header As New Panel With {.Dock = DockStyle.Top, .Height = 82, .BackColor = Color.White, .Padding = New Padding(22, 12, 22, 8)}
                header.Controls.Add(New Label With {.Text = "Transaction Details", .AutoSize = True, .Font = New Font("Segoe UI", 16.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(20, 12)})
                header.Controls.Add(New Label With {.Text = receiptNumber & "  |  " & saleDate.ToString("MMMM dd, yyyy  h:mm tt", CultureInfo.CurrentCulture), .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.Gray, .Location = New Point(22, 47)})

                Dim content As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = Color.White, .Padding = New Padding(22)}
                Dim customerPanel As New Panel With {.Location = New Point(20, 12), .Size = New Size(646, 224), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.White}
                customerPanel.Controls.Add(New Label With {.Text = "SALE INFORMATION", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(16, 13)})
                AddReceiptDetailField(customerPanel, "Sale Type", DbText(sale, "sale_mode"), 16, 48)
                AddReceiptDetailField(customerPanel, "Customer Type", DbText(sale, "customer_type"), 326, 48)
                AddReceiptDetailField(customerPanel, "Customer", If(String.IsNullOrWhiteSpace(DbText(sale, "customer_name")), "Walk-in", DbText(sale, "customer_name")), 16, 102)
                If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_id_number")) Then
                    AddReceiptDetailField(customerPanel, "Customer ID", DbText(sale, "customer_id_number"), 326, 102)
                End If
                AddReceiptDetailField(customerPanel, "Cashier", DbText(sale, "cashier_name"), 16, 160)

                Dim itemHeading As New Label With {.Text = "ITEMS", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(22, 252)}
                Dim itemGrid As New DataGridView With {
                    .Location = New Point(20, 282),
                    .Size = New Size(646, 224),
                    .ReadOnly = True,
                    .AllowUserToAddRows = False,
                    .AllowUserToDeleteRows = False,
                    .AllowUserToResizeRows = False,
                    .AutoGenerateColumns = False,
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .MultiSelect = False,
                    .RowHeadersVisible = False,
                    .BackgroundColor = Color.White,
                    .BorderStyle = BorderStyle.FixedSingle,
                    .GridColor = Color.FromArgb(230, 234, 231),
                    .EnableHeadersVisualStyles = False
                }
                itemGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 239)
                itemGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
                itemGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
                itemGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
                itemGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
                itemGrid.DefaultCellStyle.SelectionForeColor = Color.Black
                itemGrid.RowTemplate.Height = 30
                itemGrid.Columns.AddRange(New DataGridViewColumn() {
                    New DataGridViewTextBoxColumn With {.Name = "Product", .DataPropertyName = "Product", .HeaderText = "Product", .FillWeight = 240, .SortMode = DataGridViewColumnSortMode.NotSortable},
                    New DataGridViewTextBoxColumn With {.Name = "ProductID", .DataPropertyName = "ProductID", .HeaderText = "Product ID", .FillWeight = 55, .SortMode = DataGridViewColumnSortMode.NotSortable},
                    New DataGridViewTextBoxColumn With {.Name = "Quantity", .DataPropertyName = "Quantity", .HeaderText = "Quantity", .FillWeight = 55, .SortMode = DataGridViewColumnSortMode.NotSortable},
                    New DataGridViewTextBoxColumn With {.Name = "UnitPrice", .DataPropertyName = "UnitPrice", .HeaderText = "Unit Price", .FillWeight = 70, .SortMode = DataGridViewColumnSortMode.NotSortable, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "₱#,##0.00"}},
                    New DataGridViewTextBoxColumn With {.Name = "LineTotal", .DataPropertyName = "LineTotal", .HeaderText = "Line Total", .FillWeight = 75, .SortMode = DataGridViewColumnSortMode.NotSortable, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "₱#,##0.00"}}
                })
                Dim itemRows As New DataTable()
                itemRows.Columns.Add("Product", GetType(String))
                itemRows.Columns.Add("ProductID", GetType(String))
                itemRows.Columns.Add("Quantity", GetType(Integer))
                itemRows.Columns.Add("UnitPrice", GetType(Decimal))
                itemRows.Columns.Add("LineTotal", GetType(Decimal))
                For Each row As DataRow In details.Rows
                    If row.IsNull("sale_item_id") Then Continue For
                    Dim productName = DbText(row, "brand_name")
                    If Not String.IsNullOrWhiteSpace(DbText(row, "generic_name")) Then productName &= " - " & DbText(row, "generic_name")
                    If Not String.IsNullOrWhiteSpace(DbText(row, "dosage")) Then productName &= " - " & DbText(row, "dosage")
                    If Not String.IsNullOrWhiteSpace(DbText(row, "unit")) Then productName &= " (" & DbText(row, "unit") & ")"
                    itemRows.Rows.Add(productName, DbText(row, "product_id"), Convert.ToInt32(row("quantity")), Convert.ToDecimal(row("unit_price")), Convert.ToDecimal(row("line_total")))
                Next
                itemGrid.DataSource = itemRows

                Dim rxTop = 526
                Dim hasPrescription = details.AsEnumerable().Any(Function(row) Not row.IsNull("is_prescription") AndAlso Convert.ToBoolean(row("is_prescription")))
                If hasPrescription Then
                    Dim rxPanel As New Panel With {.Location = New Point(20, rxTop), .Size = New Size(646, 186), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.White}
                    rxPanel.Controls.Add(New Label With {.Text = "PRESCRIPTION INFORMATION", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(16, 12)})
                    AddReceiptDetailField(rxPanel, "Prescription No.", DbText(sale, "prescription_no"), 16, 48)
                    AddReceiptDetailField(rxPanel, "Prescribing Practitioner", DbText(sale, "prescribing_practioner"), 326, 48)
                    AddReceiptDetailField(rxPanel, "PRC License No.", DbText(sale, "prc_liscense"), 16, 104)
                    AddReceiptDetailField(rxPanel, "Prescription Date", If(sale.IsNull("prescription_date"), String.Empty, Convert.ToDateTime(sale("prescription_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)), 326, 104)
                    content.Controls.Add(rxPanel)
                    rxTop += 202
                End If

                Dim totalPanel As New Panel With {.Location = New Point(20, rxTop), .Size = New Size(646, 188), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.FromArgb(248, 250, 248)}
                AddReceiptDetailField(totalPanel, "Subtotal", "₱" & Convert.ToDecimal(sale("subtotal")).ToString("N2", CultureInfo.CurrentCulture), 16, 18)
                AddReceiptDetailField(totalPanel, "Discount", "₱" & Convert.ToDecimal(sale("discount_amount")).ToString("N2", CultureInfo.CurrentCulture), 326, 18)
                AddReceiptDetailField(totalPanel, "Total Due", "₱" & Convert.ToDecimal(sale("total_amount")).ToString("N2", CultureInfo.CurrentCulture), 16, 78, True)
                AddReceiptDetailField(totalPanel, "Cash Received", "₱" & Convert.ToDecimal(sale("amount_received")).ToString("N2", CultureInfo.CurrentCulture), 326, 78)
                AddReceiptDetailField(totalPanel, "Change", "₱" & Convert.ToDecimal(sale("change_amount")).ToString("N2", CultureInfo.CurrentCulture), 16, 138)

                content.Controls.Add(customerPanel)
                content.Controls.Add(itemHeading)
                content.Controls.Add(itemGrid)
                content.Controls.Add(totalPanel)

                Dim footer As New Panel With {.Dock = DockStyle.Bottom, .Height = 72, .BackColor = Color.White, .Padding = New Padding(16, 12, 16, 12)}
                Dim closeButton As New Button With {.Text = "Close", .Size = New Size(110, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = Color.White, .ForeColor = darkGreen, .FlatStyle = FlatStyle.Flat, .DialogResult = DialogResult.OK, .UseVisualStyleBackColor = False}
                closeButton.FlatAppearance.BorderColor = darkGreen
                closeButton.Location = New Point(footer.ClientSize.Width - closeButton.Width - 12, 12)
                AddHandler footer.Resize, Sub(sender, e) closeButton.Left = footer.ClientSize.Width - closeButton.Width - 12
                Dim printButton As New Button With {.Text = "Print Receipt", .Size = New Size(150, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False}
                printButton.FlatAppearance.BorderSize = 0
                printButton.Location = New Point(closeButton.Left - printButton.Width - 10, 12)
                AddHandler footer.Resize, Sub(sender, e) printButton.Left = closeButton.Left - printButton.Width - 10
                AddHandler printButton.Click,
                    Sub(sender, e)
                        Try
                            Dim path = Staff.CreateReceiptPdf(receiptNumber, saleDate, receiptLines)
                            Process.Start(New ProcessStartInfo(path) With {.UseShellExecute = True})
                        Catch ex As Exception
                            MessageBox.Show("Unable to create or open the receipt PDF. " & ex.Message, "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    End Sub
                footer.Controls.Add(printButton)
                footer.Controls.Add(closeButton)
                dialog.Controls.Add(content)
                dialog.Controls.Add(footer)
                dialog.Controls.Add(header)
                dialog.AcceptButton = closeButton
                dialog.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show("Could not load transaction details. " & ex.Message, "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function BuildSavedReceiptLines(details As DataTable,
                                            receiptNumber As String,
                                            saleId As Long,
                                            saleDate As DateTime) As List(Of String)
        Dim sale = details.Rows(0)
        Dim lines As New List(Of String) From {
            "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES",
            "SALES RECEIPT",
            New String("-"c, 76),
            "Receipt No.: " & receiptNumber,
            "Transaction No.: " & saleId.ToString(CultureInfo.InvariantCulture),
            "Date and Time: " & saleDate.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
            "Sale Type: " & DbText(sale, "sale_mode"),
            "Cashier: " & DbText(sale, "cashier_name"),
            "Customer Type: " & DbText(sale, "customer_type")
        }
        If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_name")) Then lines.Add("Customer Name: " & DbText(sale, "customer_name"))
        If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_id_number")) Then lines.Add("Customer ID: " & DbText(sale, "customer_id_number"))
        lines.Add(New String("-"c, 76))
        lines.Add("ITEMS")
        For Each row As DataRow In details.Rows
            If row.IsNull("sale_item_id") Then Continue For
            Dim productName = DbText(row, "brand_name")
            If Not String.IsNullOrWhiteSpace(DbText(row, "generic_name")) Then productName &= " - " & DbText(row, "generic_name")
            If Not String.IsNullOrWhiteSpace(DbText(row, "dosage")) Then productName &= " - " & DbText(row, "dosage")
            lines.Add(productName)
            lines.Add("  " & DbText(row, "quantity") & " " & DbText(row, "unit") &
                      " x PHP " & Convert.ToDecimal(row("unit_price")).ToString("N2", CultureInfo.InvariantCulture) &
                      " = PHP " & Convert.ToDecimal(row("line_total")).ToString("N2", CultureInfo.InvariantCulture))
        Next
        If details.AsEnumerable().Any(Function(row) Not row.IsNull("is_prescription") AndAlso Convert.ToBoolean(row("is_prescription"))) Then
            lines.Add(New String("-"c, 76))
            lines.Add("PRESCRIPTION INFORMATION")
            lines.Add("Prescription No.: " & DbText(sale, "prescription_no"))
            lines.Add("Prescribing Practitioner: " & DbText(sale, "prescribing_practioner"))
            lines.Add("PRC License No.: " & DbText(sale, "prc_liscense"))
            If Not sale.IsNull("prescription_date") Then lines.Add("Prescription Date: " & Convert.ToDateTime(sale("prescription_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
        End If
        lines.Add(New String("-"c, 76))
        lines.Add("Subtotal: PHP " & Convert.ToDecimal(sale("subtotal")).ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Discount: PHP " & Convert.ToDecimal(sale("discount_amount")).ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("TOTAL DUE: PHP " & Convert.ToDecimal(sale("total_amount")).ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Cash Received: PHP " & Convert.ToDecimal(sale("amount_received")).ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Change: PHP " & Convert.ToDecimal(sale("change_amount")).ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Thank you for your purchase!")
        Return lines
    End Function

    Private Sub AddReceiptDetailField(panel As Panel, caption As String, value As String, x As Integer, y As Integer, Optional emphasized As Boolean = False)
        Dim fieldWidth = Math.Max(120, panel.ClientSize.Width \ 2 - 24)
        Dim valueLabel As New Label With {
            .Text = value,
            .AutoSize = True,
            .MaximumSize = New Size(fieldWidth, 0),
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
            .ForeColor = If(emphasized, darkGreen, Color.FromArgb(35, 45, 40)),
            .Location = New Point(x, y + 22)
        }
        panel.Controls.Add(New Label With {.Text = caption, .AutoSize = True, .MaximumSize = New Size(fieldWidth, 0), .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(x, y)})
        panel.Controls.Add(valueLabel)
    End Sub

    Private Function DbText(row As DataRow, columnName As String) As String
        If Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then Return String.Empty
        Return Convert.ToString(row(columnName), CultureInfo.CurrentCulture)
    End Function

    Private Sub userRoleLabel_Click(sender As Object, e As EventArgs) Handles userRoleLabel.Click
        ' Optional click logic for user role label
    End Sub


    ' BACKEND DATA RETRIEVAL METHODS


    Private Sub LoadDashboardStats()
        Try
            Dim summary = New SalesRepository().GetSummary()
            card1Value.Text = "P " & summary.TodaySales.ToString("N2")
            card1Footer.Text = DateTime.Today.ToString("MMMM dd, yyyy")
            card2Value.Text = "P " & summary.RetailSales.ToString("N2")
            card2Footer.Text = summary.RetailTransactions.ToString() & " Transactions"
            card3Value.Text = "P " & summary.WholesaleSales.ToString("N2")
            card3Footer.Text = summary.WholesaleTransactions.ToString() & " Transactions"
        Catch ex As Exception
            MessageBox.Show("Could not load sales totals. " & ex.Message, "Sales dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            Dim products = New ProductManager().GetProducts(includeInactive:=True)
            If products Is Nothing Then products = New DataTable()

            Dim lowStockCount As Integer = 0
            Dim expiredCount As Integer = 0
            Dim nearExpiryCount As Integer = 0

            For Each row As DataRow In products.Rows
                Dim status = GetInventoryStatus(row)
                If status.IsLowStock Then lowStockCount += 1
                If status.IsExpired Then expiredCount += 1
                If status.IsNearExpiry Then nearExpiryCount += 1
            Next

            card4Value.Text = products.Rows.Count.ToString()
            card4Footer.Text = "All Categories"
            card5Value.Text = lowStockCount.ToString()
            card5Footer.Text = "Need reordering"
            card6Value.Text = expiredCount.ToString()
            card6Footer.Text = "Within 0 days"
            card7Value.Text = nearExpiryCount.ToString()
            card7Footer.Text = "Expires within 6 months"
        Catch ex As Exception
            MessageBox.Show("Could not load inventory totals. " & ex.Message, "Inventory dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' The connected cabreradb schema currently has no expense records/table or purchase cost field.
        ' Keep the card explicit instead of presenting an unsupported expense total as a real figure.
        card8Value.Text = "P 0.00"
        card8Footer.Text = "Expense data is not stored yet"
    End Sub

    Private Sub LoadInventoryData()
        Try
            Dim prodManager As New ProductManager()
            inventoryRows = prodManager.GetProducts()
            If inventoryRows Is Nothing Then inventoryRows = New DataTable()
            If Not inventoryRows.Columns.Contains("Status") Then inventoryRows.Columns.Add("Status", GetType(String))
            For Each row As DataRow In inventoryRows.Rows
                row("Status") = GetInventoryStatus(row).Label
            Next
            ApplyInventoryFilters()

        Catch ex As Exception
            MessageBox.Show("Failed to load inventory: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeInventoryUi()
        Dim layout As New TableLayoutPanel With {
            .Name = "inventoryLayout",
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 118.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        Dim toolbar As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White}
        Dim filterLabel As New Label With {
            .Text = "Filter by status",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.Black,
            .Location = New Point(8, 2)
        }
        toolbar.Controls.Add(filterLabel)

        Dim filterPanel As New FlowLayoutPanel With {
            .Location = New Point(8, 28),
            .Size = New Size(650, 34),
            .WrapContents = False,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        For Each filterName In {"All", "Low Stock", "Near Expiry", "Expired"}
            Dim filterButton As New Button With {
                .Text = filterName,
                .Tag = filterName,
                .AutoSize = True,
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.FromArgb(240, 240, 240),
                .ForeColor = Color.FromArgb(40, 40, 40),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Margin = New Padding(0, 0, 6, 0),
                .Padding = New Padding(6, 2, 6, 2),
                .Height = 28
            }
            filterButton.FlatAppearance.BorderSize = 0
            AddHandler filterButton.Click, AddressOf InventoryFilterButton_Click
            inventoryFilterButtons(filterName) = filterButton
            filterPanel.Controls.Add(filterButton)
        Next
        toolbar.Controls.Add(filterPanel)

        inventorySearchBox = New TextBox With {
            .Location = New Point(8, 69),
            .Size = New Size(620, 30),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right,
            .Font = New Font("Segoe UI", 10.0F),
            .BackColor = Color.White,
            .ForeColor = Color.Black,
            .BorderStyle = BorderStyle.FixedSingle,
            .PlaceholderText = "Search product ID, category, brand, generic name, or status..."
        }
        AddHandler inventorySearchBox.TextChanged, AddressOf InventorySearchBox_TextChanged
        toolbar.Controls.Add(inventorySearchBox)

        Dim clearButton As New Button With {
            .Text = "Clear",
            .Location = New Point(638, 68),
            .Size = New Size(78, 31),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(240, 240, 240),
            .ForeColor = Color.FromArgb(40, 40, 40),
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        }
        clearButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80)
        AddHandler clearButton.Click,
            Sub(sender, e)
                inventorySearchBox.Clear()
                SetInventoryFilter("All")
            End Sub
        toolbar.Controls.Add(clearButton)

        dgvInventory.Dock = DockStyle.Fill
        dgvInventory.BackgroundColor = Color.White
        dgvInventory.BorderStyle = BorderStyle.None
        dgvInventory.GridColor = Color.FromArgb(225, 225, 225)
        dgvInventory.EnableHeadersVisualStyles = False
        dgvInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.White
        dgvInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgvInventory.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 240, 230)
        dgvInventory.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black
        dgvInventory.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        dgvInventory.DefaultCellStyle.BackColor = Color.White
        dgvInventory.DefaultCellStyle.ForeColor = Color.Black
        dgvInventory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 240, 230)
        dgvInventory.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvInventory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248)
        dgvInventory.RowHeadersVisible = False
        dgvInventory.AllowUserToAddRows = False
        dgvInventory.AllowUserToDeleteRows = False
        dgvInventory.AllowUserToResizeColumns = True
        dgvInventory.AllowUserToResizeRows = True
        dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgvInventory.ColumnHeadersHeight = 38
        dgvInventory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvInventory.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvInventory.ReadOnly = True
        dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        AddHandler dgvInventory.DataBindingComplete, AddressOf InventoryGrid_DataBindingComplete
        AddHandler dgvInventory.CellFormatting, AddressOf InventoryGrid_CellFormatting
        AddHandler dgvInventory.CellClick, AddressOf InventoryGrid_CellClick

        layout.Controls.Add(toolbar, 0, 0)
        layout.Controls.Add(dgvInventory, 0, 1)
        inventoryView.Controls.Clear()
        inventoryView.BackColor = Color.White
        inventoryView.Controls.Add(layout)
        SetInventoryFilter("All")
    End Sub

    Private Sub InventoryGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse e.RowIndex >= dgvInventory.Rows.Count Then Return
        Dim view = TryCast(dgvInventory.Rows(e.RowIndex).DataBoundItem, DataRowView)
        If view Is Nothing Then Return
        ShowProductDetails(view.Row)
    End Sub

    Private Sub ShowProductDetails(product As DataRow)
        Dim productId = Convert.ToInt32(product("ID"))
        Dim manager As New ProductManager()
        Dim batches As DataTable
        Try
            batches = manager.GetProductBatches(productId)
        Catch ex As Exception
            MessageBox.Show("Could not load batch details. " & ex.Message, "Product details", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try


        Dim details As New ProductDetailsWindow With {
            .Text = "Product Details",
            .StartPosition = FormStartPosition.CenterParent,
            .Size = New Size(620, 650),
            .BackColor = Color.White,
            .Font = New Font("Segoe UI", 9.0F),
            .FormBorderStyle = FormBorderStyle.None,
            .ControlBox = False,
            .MinimizeBox = False,
            .MaximizeBox = False,
            .ShowInTaskbar = False,
            .Padding = New Padding(1)
        }


        Dim content As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .BackColor = Color.White,
            .Padding = New Padding(18),
            .Margin = New Padding(0)
        }

        Dim footer As New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 62,
            .BackColor = Color.White,
            .Padding = New Padding(18, 8, 18, 8),
            .Margin = New Padding(0)
        }
        Dim closeButton As New Button With {.Text = "Close", .Size = New Size(96, 34), .Anchor = AnchorStyles.Right Or AnchorStyles.Top, .FlatStyle = FlatStyle.Flat, .BackColor = Color.White, .ForeColor = darkGreen}
        closeButton.Location = New Point(footer.ClientSize.Width - closeButton.Width - 18, 6)
        AddHandler closeButton.Click, Sub(sender, e) details.Close()
        footer.Controls.Add(closeButton)
        AddHandler footer.Resize, Sub(sender, e) closeButton.Left = footer.ClientSize.Width - closeButton.Width - 18

        Dim active = Not product.IsNull("Active") AndAlso Convert.ToBoolean(product("Active"))
        Dim sectionWidth As Integer = Math.Max(540, details.ClientSize.Width - content.Padding.Horizontal - 8)
        Dim header As New Panel With {.Width = sectionWidth, .Height = 62, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 10)}
        Dim title As New Label With {
            .Text = Convert.ToString(product("Brand Name")),
            .Font = New Font("Segoe UI", 18.0F, FontStyle.Bold),
            .ForeColor = darkGreen,
            .AutoSize = True,
            .Location = New Point(0, 0)
        }
        Dim productIdLabel As New Label With {
            .Text = "Product ID: " & productId.ToString(),
            .AutoSize = True,
            .ForeColor = Color.Gray,
            .Font = New Font("Segoe UI", 9.0F),
            .Location = New Point(2, 36)
        }
        Dim state As New Label With {
            .Text = If(active, "Active", "Inactive"),
            .AutoSize = True,
            .ForeColor = If(active, Color.DarkGreen, Color.DarkRed),
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right
        }
        state.Location = New Point(sectionWidth - state.PreferredWidth, 12)
        header.Controls.Add(title)
        header.Controls.Add(productIdLabel)
        header.Controls.Add(state)
        content.Controls.Add(header)
        AddHandler content.ClientSizeChanged,
            Sub(sender, e)
                Dim scrollbarSpace = If(content.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
                sectionWidth = Math.Max(540, content.ClientSize.Width - content.Padding.Horizontal - 4 + scrollbarSpace)
                For Each control As Control In content.Controls
                    If TypeOf control Is Panel Then control.Width = sectionWidth
                Next
                state.Left = sectionWidth - state.Width
            End Sub

        AddProductDetailSection(content, "1. Product Information", {
            {"Brand Name", "Brand Name"}, {"Generic Name", "Generic Name"}, {"Category", "Category"},
            {"Dosage Strength", "Dosage"}, {"Manufacturer", "Manufacturer"}, {"CPR Number", "CPR Number"}
        }, product)
        AddProductDetailSection(content, "2. Inventory Information", {
            {"Total Stock", "Stock Quantity"}, {"Reorder Point", "Reorder Point"},
            {"Unit of Measure", "Unit"}, {"Quantity per Box", "Qty/Box"}
        }, product)

        Dim batchSection As New Panel With {.Width = sectionWidth, .Height = 184, .BackColor = Color.White, .Margin = New Padding(0, 10, 0, 14)}
        Dim batchHeading As New Label With {.Text = "3. Batch Information", .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .AutoSize = True, .Location = New Point(0, 0)}
        batchSection.Controls.Add(batchHeading)
        Dim batchGrid As New DataGridView With {
            .Location = New Point(0, 38), .Size = New Size(sectionWidth - 12, 140), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right,
            .ReadOnly = True, .AllowUserToAddRows = False, .AllowUserToDeleteRows = False,
            .AllowUserToResizeRows = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect, .RowHeadersVisible = False,
            .BackgroundColor = Color.White, .BorderStyle = BorderStyle.None,
            .GridColor = Color.FromArgb(225, 232, 227),
            .EnableHeadersVisualStyles = False,
            .ColumnHeadersHeight = 32,
            .RowTemplate = New DataGridViewRow With {.Height = 30}
        }
        batchGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 239)
        batchGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        batchGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 240, 230)
        batchGrid.DefaultCellStyle.SelectionForeColor = Color.Black
        batchGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        batchGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        If Not batches.Columns.Contains("Status") Then batches.Columns.Add("Status", GetType(String))
        For Each batch As DataRow In batches.Rows
            Dim expiry = Convert.ToDateTime(batch("Expiry Date")).Date
            batch("Status") = If(expiry < Date.Today, "Expired", If(expiry <= Date.Today.AddMonths(6), "Near Expiry", "In Stock"))
        Next
        batchGrid.DataSource = batches
        If batchGrid.Columns.Contains("Batch ID") Then batchGrid.Columns("Batch ID").Visible = False
        If batchGrid.Columns.Contains("Expiry Date") Then batchGrid.Columns("Expiry Date").DefaultCellStyle.Format = "MMM dd, yyyy"
        Dim removeColumn As New DataGridViewButtonColumn With {
            .Name = "RemoveBatch",
            .HeaderText = "Action",
            .Text = "Remove",
            .UseColumnTextForButtonValue = True,
            .Width = 82,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .FlatStyle = FlatStyle.Flat
        }
        batchGrid.Columns.Add(removeColumn)
        AddHandler batchGrid.CellFormatting,
            Sub(sender, e)
                If e.RowIndex < 0 OrElse e.ColumnIndex <> removeColumn.Index Then Return
                Dim rowView = TryCast(batchGrid.Rows(e.RowIndex).DataBoundItem, DataRowView)
                Dim isExpired = rowView IsNot Nothing AndAlso Not rowView.Row.IsNull("Expiry Date") AndAlso Convert.ToDateTime(rowView.Row("Expiry Date")).Date < Date.Today
                e.Value = If(isExpired, "Remove", "—")
                e.CellStyle.ForeColor = If(isExpired, Color.DarkRed, Color.Gray)
                e.CellStyle.BackColor = If(isExpired, Color.FromArgb(255, 235, 235), Color.White)
            End Sub
        AddHandler batchGrid.CellContentClick,
            Sub(sender, e)
                If e.RowIndex < 0 OrElse e.ColumnIndex <> removeColumn.Index Then Return
                Dim rowView = TryCast(batchGrid.Rows(e.RowIndex).DataBoundItem, DataRowView)
                If rowView Is Nothing OrElse Convert.ToDateTime(rowView.Row("Expiry Date")).Date >= Date.Today Then Return
                Dim batchId = Convert.ToInt64(rowView.Row("Batch ID"))
                Dim batchNumber = Convert.ToString(rowView.Row("Batch No"))
                Dim remaining = Convert.ToString(rowView.Row("Remaining"))
                Dim prompt = "Remove expired batch " & batchNumber & " (" & remaining & " units) from inventory? This will permanently remove its remaining stock."
                If MessageBox.Show(prompt, "Remove expired batch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then Return
                Try
                    If manager.RemoveExpiredBatch(batchId) Then
                        MessageBox.Show("The expired batch was removed from inventory.", "Batch removed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        details.Close()
                        LoadInventoryData()
                    Else
                        MessageBox.Show("This batch could not be found. Refresh inventory and try again.", "Batch not found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Could not remove the expired batch. " & ex.Message, "Remove failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub
        batchSection.Controls.Add(batchGrid)
        content.Controls.Add(batchSection)

        AddProductDetailSection(content, "4. Pricing Information", {
            {"Retail Price / Unit", "Retail Price"}, {"Wholesale Price / Box", "Wholesale Price"}
        }, product)

        Dim activitySection As New Panel With {.Width = sectionWidth, .Height = 68, .BackColor = Color.FromArgb(248, 250, 248), .Margin = New Padding(0, 6, 0, 6), .BorderStyle = BorderStyle.FixedSingle, .Padding = New Padding(10)}
        Dim activityTitle As New Label With {.Text = "5. Product Activity", .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(10, 6)}
        Dim activityText As New Label With {.Text = If(active, "Status: Product is Active", "Status: Product is Inactive"), .AutoSize = True, .Location = New Point(10, 34)}
        activitySection.Controls.Add(activityTitle)
        activitySection.Controls.Add(activityText)
        content.Controls.Add(activitySection)

        Dim productActivityButton As New Button With {
            .Text = If(active, "Deactivate Product", "Reactivate Product"),
            .Enabled = True,
            .Size = New Size(170, 34),
            .BackColor = If(active, Color.FromArgb(170, 45, 45), darkGreen),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Margin = New Padding(0, 2, 0, 15)
        }
        productActivityButton.FlatAppearance.BorderSize = 0
        productActivityButton.Location = New Point(18, 13)
        AddHandler productActivityButton.Click,
            Sub(sender, e)
                Dim makeActive = Not active
                Dim confirmation = If(makeActive,
                    "Reactivate this product? Staff will be able to sell it again.",
                    "Deactivate this product? It will remain in inventory but staff will no longer be able to sell it.")
                Dim dialogTitle = If(makeActive, "Confirm reactivation", "Confirm deactivation")
                Dim dialogIcon = If(makeActive, MessageBoxIcon.Question, MessageBoxIcon.Warning)
                If MessageBox.Show(confirmation, dialogTitle, MessageBoxButtons.YesNo, dialogIcon) <> DialogResult.Yes Then Return
                Dim enteredPin = PromptForPin(details)
                If enteredPin Is Nothing Then Return
                Try
                    If Not manager.VerifyUserPin(CurrentUserID, enteredPin) Then
                        MessageBox.Show("The PIN is incorrect or no PIN is set for your account.", "PIN verification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    If manager.SetProductActive(productId, makeActive) Then
                        Dim successMessage = If(makeActive,
                            "Product reactivated and available for staff to sell.",
                            "Product deactivated. It remains visible in inventory and is now unavailable to staff.")
                        Dim successTitle = If(makeActive, "Product reactivated", "Product deactivated")
                        MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        details.Close()
                        LoadInventoryData()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Could not deactivate this product. " & ex.Message, "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Sub
        footer.Controls.Add(productActivityButton)

        details.Controls.Add(content)
        details.Controls.Add(footer)
        productActivityButton.Location = New Point(18, 14)
        productActivityButton.Anchor = AnchorStyles.Left Or AnchorStyles.Top
        details.ShowDialog(Me)
    End Sub

    Private Sub AddProductDetailSection(parent As FlowLayoutPanel, heading As String, fields As String(,), product As DataRow)
        Dim scrollbarSpace = If(parent.VerticalScroll.Visible, SystemInformation.VerticalScrollBarWidth, 0)
        Dim sectionWidth = Math.Max(540, parent.ClientSize.Width - parent.Padding.Horizontal - 4 + scrollbarSpace)
        Dim rowCount = CInt(Math.Ceiling(fields.GetLength(0) / 2.0))
        Dim rowSpacing As Integer = 66
        Dim section As New Panel With {.Width = sectionWidth, .Height = rowCount * rowSpacing + 58, .BackColor = Color.White, .BorderStyle = BorderStyle.FixedSingle, .Margin = New Padding(0, 0, 0, 12)}
        section.Controls.Add(New Label With {.Text = heading, .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(12, 10)})
        Dim fieldLabels As New List(Of Label)
        Dim fieldValues As New List(Of Label)
        For index = 0 To fields.GetLength(0) - 1
            Dim col = index Mod 2
            Dim row = index \ 2
            Dim colWidth = (sectionWidth - 24) \ 2
            Dim x = 12 + col * colWidth
            Dim y = 44 + row * rowSpacing
            Dim value As String = "—"
            Dim columnName = fields(index, 1)
            If product.Table.Columns.Contains(columnName) AndAlso Not product.IsNull(columnName) Then value = Convert.ToString(product(columnName))
            If columnName = "Stock Quantity" AndAlso product.Table.Columns.Contains("Unit") AndAlso Not product.IsNull("Unit") Then value &= " " & Convert.ToString(product("Unit"))
            If columnName = "Qty/Box" AndAlso product.Table.Columns.Contains("Unit") AndAlso Not product.IsNull("Unit") Then value &= " " & Convert.ToString(product("Unit"))
            If columnName = "Retail Price" OrElse columnName = "Wholesale Price" Then
                Dim price As Decimal
                If Decimal.TryParse(value, price) Then value = "₱" & price.ToString("N2")
            End If
            Dim fieldLabel As New Label With {.Text = fields(index, 0), .AutoSize = True, .ForeColor = Color.FromArgb(115, 115, 115), .Location = New Point(x, y), .Tag = col}
            Dim fieldValue As New Label With {.Text = value, .AutoSize = True, .MaximumSize = New Size(colWidth - 24, 0), .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 35, 35), .Location = New Point(x, y + 18), .Tag = col}
            fieldLabels.Add(fieldLabel)
            fieldValues.Add(fieldValue)
            section.Controls.Add(fieldLabel)
            section.Controls.Add(fieldValue)
        Next
        AddHandler section.Resize,
            Sub(sender, e)
                Dim colWidth = (section.ClientSize.Width - 24) \ 2
                For Each label In fieldLabels
                    label.Left = 12 + CInt(label.Tag) * colWidth
                Next
                For Each valueLabel In fieldValues
                    valueLabel.Left = 12 + CInt(valueLabel.Tag) * colWidth
                    valueLabel.MaximumSize = New Size(colWidth - 18, 0)
                Next
            End Sub
        parent.Controls.Add(section)
    End Sub

    Private Function PromptForPin(owner As IWin32Window) As String
        Using prompt As New Form With {
            .Text = "Verify PIN",
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .ClientSize = New Size(340, 140),
            .MinimizeBox = False,
            .MaximizeBox = False,
            .BackColor = Color.White
        }
            prompt.Controls.Add(New Label With {.Text = "Enter your account PIN to confirm:", .AutoSize = True, .Location = New Point(18, 18)})
            Dim pinBox As New TextBox With {.Location = New Point(18, 48), .Width = 300, .UseSystemPasswordChar = True, .MaxLength = 12}
            prompt.Controls.Add(pinBox)
            Dim okButton As New Button With {.Text = "Verify", .DialogResult = DialogResult.OK, .Location = New Point(150, 90), .Size = New Size(80, 30)}
            Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Location = New Point(238, 90), .Size = New Size(80, 30)}
            prompt.Controls.Add(okButton)
            prompt.Controls.Add(cancelButton)
            prompt.AcceptButton = okButton
            prompt.CancelButton = cancelButton
            If prompt.ShowDialog(owner) = DialogResult.OK Then Return pinBox.Text
        End Using
        Return Nothing
    End Function

    Private Sub InventoryFilterButton_Click(sender As Object, e As EventArgs)
        Dim button = TryCast(sender, Button)
        If button IsNot Nothing Then SetInventoryFilter(Convert.ToString(button.Tag))
    End Sub

    Private Sub InventorySearchBox_TextChanged(sender As Object, e As EventArgs)
        ApplyInventoryFilters()
    End Sub

    Private Sub SetInventoryFilter(filterName As String)
        inventoryActiveFilter = If(String.IsNullOrWhiteSpace(filterName), "All", filterName)
        For Each entry In inventoryFilterButtons
            Dim active = String.Equals(entry.Key, inventoryActiveFilter, StringComparison.OrdinalIgnoreCase)
            entry.Value.BackColor = If(active, Color.FromArgb(20, 100, 60), Color.FromArgb(240, 240, 240))
            entry.Value.ForeColor = If(active, Color.White, Color.FromArgb(40, 40, 40))
        Next
        ApplyInventoryFilters()
    End Sub

    Private Sub ApplyInventoryFilters()
        If inventoryRows Is Nothing OrElse Not inventoryRows.Columns.Contains("Status") Then Return
        Dim visibleRows = inventoryRows.Clone()
        Dim search = If(inventorySearchBox Is Nothing, String.Empty, inventorySearchBox.Text.Trim())
        For Each row As DataRow In inventoryRows.Rows
            Dim status = GetInventoryStatus(row)
            Dim includeByStatus = True
            Select Case inventoryActiveFilter
                Case "Low Stock"
                    includeByStatus = status.IsLowStock
                Case "Near Expiry"
                    includeByStatus = status.IsNearExpiry
                Case "Expired"
                    includeByStatus = status.IsExpired
            End Select

            Dim includeBySearch = search.Length = 0 OrElse
                Convert.ToString(row("ID")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Category")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Brand Name")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Generic Name")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Status")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0

            If includeByStatus AndAlso includeBySearch Then visibleRows.ImportRow(row)
        Next
        dgvInventory.DataSource = visibleRows
    End Sub

    Private Function GetInventoryStatus(row As DataRow) As InventoryStatusInfo
        Dim stock = If(row.IsNull("Stock Quantity"), 0, Convert.ToInt32(row("Stock Quantity")))
        Dim reorderPoint = If(row.IsNull("Reorder Point"), 0, Convert.ToInt32(row("Reorder Point")))
        Dim isLowStock = stock <= reorderPoint
        Dim expiry As DateTime? = Nothing
        If Not row.IsNull("Expiry Date") Then expiry = Convert.ToDateTime(row("Expiry Date")).Date
        Dim isExpired = expiry.HasValue AndAlso expiry.Value < Date.Today
        Dim isNearExpiry = expiry.HasValue AndAlso Not isExpired AndAlso expiry.Value <= Date.Today.AddMonths(6)
        Dim labels As New List(Of String)
        If row.Table.Columns.Contains("Active") AndAlso Not row.IsNull("Active") AndAlso Not Convert.ToBoolean(row("Active")) Then labels.Add("Inactive")
        If isExpired Then labels.Add("Expired")
        If isLowStock Then labels.Add("Low Stock")
        If isNearExpiry Then labels.Add("Near Expiry")
        Dim label = If(labels.Count = 0, "In Stock", String.Join(" · ", labels))
        Return New InventoryStatusInfo With {.Label = label, .IsLowStock = isLowStock, .IsNearExpiry = isNearExpiry, .IsExpired = isExpired}
    End Function

    Private Class InventoryStatusInfo
        Public Property Label As String
        Public Property IsLowStock As Boolean
        Public Property IsNearExpiry As Boolean
        Public Property IsExpired As Boolean
    End Class

    Private Sub InventoryGrid_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        For Each column As DataGridViewColumn In dgvInventory.Columns
            column.Visible = {"ID", "Category", "Brand Name", "Generic Name", "Stock Quantity", "Expiry Date", "Status"}.Contains(column.Name)
            column.ReadOnly = True
            Dim allowResize = {"Category", "Brand Name", "Generic Name"}.Contains(column.Name)
            column.Resizable = If(allowResize, DataGridViewTriState.True, DataGridViewTriState.False)
            column.AutoSizeMode = If(allowResize, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.None)
        Next
        Dim captions As New Dictionary(Of String, String) From {{"ID", "Product ID"}, {"Stock Quantity", "Stock Quantity"}, {"Expiry Date", "Expiry Date"}}
        For Each caption In captions
            If dgvInventory.Columns.Contains(caption.Key) Then dgvInventory.Columns(caption.Key).HeaderText = caption.Value
        Next
        If dgvInventory.Columns.Contains("Expiry Date") Then dgvInventory.Columns("Expiry Date").DefaultCellStyle.Format = "MM/dd/yyyy"
        If dgvInventory.Columns.Contains("ID") Then dgvInventory.Columns("ID").Width = 90
        If dgvInventory.Columns.Contains("Category") Then dgvInventory.Columns("Category").FillWeight = 120
        If dgvInventory.Columns.Contains("Brand Name") Then dgvInventory.Columns("Brand Name").FillWeight = 105
        If dgvInventory.Columns.Contains("Generic Name") Then dgvInventory.Columns("Generic Name").FillWeight = 170
        If dgvInventory.Columns.Contains("Stock Quantity") Then dgvInventory.Columns("Stock Quantity").Width = 170
        If dgvInventory.Columns.Contains("Expiry Date") Then dgvInventory.Columns("Expiry Date").Width = 145
        If dgvInventory.Columns.Contains("Status") Then dgvInventory.Columns("Status").Width = 145
        dgvInventory.RowTemplate.MinimumHeight = 48
        For Each row As DataGridViewRow In dgvInventory.Rows
            row.MinimumHeight = 48
        Next
    End Sub

    Private Sub InventoryGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim columnName = dgvInventory.Columns(e.ColumnIndex).Name
        If columnName = "Expiry Date" AndAlso e.Value IsNot Nothing AndAlso Not Convert.IsDBNull(e.Value) Then
            e.Value = Convert.ToDateTime(e.Value).ToString("MM/dd/yyyy")
            e.FormattingApplied = True
        ElseIf columnName = "Stock Quantity" AndAlso dgvInventory.Columns.Contains("Unit") Then
            Dim quantity = Convert.ToInt32(e.Value)
            Dim unitValue = Convert.ToString(dgvInventory.Rows(e.RowIndex).Cells("Unit").Value)
            e.Value = quantity.ToString() & " " & FormatInventoryUnit(unitValue, quantity)
            e.FormattingApplied = True
        ElseIf columnName = "Status" Then
            Dim status = Convert.ToString(e.Value)
            Dim hasExpired = status.Contains("Expired")
            Dim hasNearExpiry = status.Contains("Near Expiry")
            Dim hasLowStock = status.Contains("Low Stock")
            Dim isInactive = status.Contains("Inactive")
            e.CellStyle.BackColor = If(isInactive, Color.FromArgb(235, 235, 235), If(hasExpired, Color.FromArgb(255, 230, 230), If(hasNearExpiry, Color.FromArgb(255, 241, 220), If(hasLowStock, Color.FromArgb(255, 239, 220), Color.FromArgb(225, 245, 230)))))
            e.CellStyle.ForeColor = If(isInactive, Color.DimGray, If(hasExpired, Color.DarkRed, If(hasNearExpiry, Color.DarkOrange, If(hasLowStock, Color.SaddleBrown, Color.DarkGreen))))
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
        End If
    End Sub

    Private Function FormatInventoryUnit(unitValue As String, quantity As Integer) As String
        Dim unit = If(unitValue, String.Empty).Trim()
        If quantity = 1 OrElse unit.Length = 0 Then Return unit.ToLowerInvariant()
        If unit.StartsWith("Bottle", StringComparison.OrdinalIgnoreCase) Then
            Return "Bottles" & unit.Substring("Bottle".Length).ToLowerInvariant()
        End If
        If unit.EndsWith("Tablet", StringComparison.OrdinalIgnoreCase) OrElse
           unit.EndsWith("Capsule", StringComparison.OrdinalIgnoreCase) Then
            Return unit.ToLowerInvariant() & "s"
        End If
        Return If(unit.EndsWith("s", StringComparison.OrdinalIgnoreCase), unit.ToLowerInvariant(), unit.ToLowerInvariant() & "s")
    End Function

    Private Sub LoadPriceManagerData()
        Try
            priceManagerProducts = New ProductManager().GetProducts(includeInactive:=True)
            If priceManagerProducts Is Nothing Then priceManagerProducts = New DataTable()

            Dim selectedCategory = If(priceManagerCategoryFilter Is Nothing, "All Categories", Convert.ToString(priceManagerCategoryFilter.SelectedItem))
            If priceManagerCategoryFilter IsNot Nothing Then
                priceManagerCategoryFilter.BeginUpdate()
                priceManagerCategoryFilter.Items.Clear()
                priceManagerCategoryFilter.Items.Add("All Categories")
                If priceManagerProducts.Columns.Contains("Category") Then
                    For Each category In priceManagerProducts.AsEnumerable().Select(Function(row) Convert.ToString(row("Category"))).Where(Function(value) Not String.IsNullOrWhiteSpace(value)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(Function(value) value)
                        priceManagerCategoryFilter.Items.Add(category)
                    Next
                End If
                If priceManagerCategoryFilter.Items.Contains(selectedCategory) Then
                    priceManagerCategoryFilter.SelectedItem = selectedCategory
                Else
                    priceManagerCategoryFilter.SelectedIndex = 0
                End If
                priceManagerCategoryFilter.EndUpdate()
            End If
            RenderPriceManagerProducts()
        Catch ex As Exception
            MessageBox.Show("Could not load product prices. " & ex.Message, "Price Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializePriceManagerUi()
        priceManagerView.Controls.Clear()
        priceManagerView.Padding = New Padding(8)
        priceManagerView.BackColor = lightGray

        Dim page As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 1,
            .BackColor = lightGray,
            .Margin = New Padding(0)
        }
        page.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        page.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim listing As New TableLayoutPanel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(12), .Margin = New Padding(0), .ColumnCount = 1, .RowCount = 3}
        listing.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        listing.RowStyles.Add(New RowStyle(SizeType.Absolute, 46))
        listing.RowStyles.Add(New RowStyle(SizeType.Absolute, 44))
        listing.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        priceManagerSearchBox = New TextBox With {.Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 10.0F), .PlaceholderText = "Search product ID, category, brand, generic name...", .Margin = New Padding(0, 3, 0, 5)}
        priceManagerCategoryFilter = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 10.0F), .Dock = DockStyle.Left, .Width = 260, .Margin = New Padding(0, 3, 0, 5)}
        priceManagerCategoryFilter.Items.Add("All Categories")
        priceManagerCategoryFilter.SelectedIndex = 0
        Dim productArea As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0)}
        Dim detailsHeader As New Label With {.Text = "PRODUCT DETAILS", .Dock = DockStyle.Top, .Height = 32, .BackColor = Color.FromArgb(240, 245, 241), .ForeColor = Color.FromArgb(65, 80, 70), .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(8, 0, 0, 0)}
        priceManagerProductList = New FlowLayoutPanel With {.Dock = DockStyle.Fill, .AutoScroll = True, .FlowDirection = FlowDirection.TopDown, .WrapContents = False, .BackColor = Color.White, .Padding = New Padding(0, 8, 0, 0)}
        productArea.Controls.Add(priceManagerProductList)
        productArea.Controls.Add(detailsHeader)
        listing.Controls.Add(priceManagerSearchBox, 0, 0)
        listing.Controls.Add(priceManagerCategoryFilter, 0, 1)
        listing.Controls.Add(productArea, 0, 2)
        page.Controls.Add(listing, 0, 0)
        priceManagerView.Controls.Add(page)

        AddHandler priceManagerSearchBox.TextChanged, Sub(sender, e) RenderPriceManagerProducts()
        AddHandler priceManagerCategoryFilter.SelectedIndexChanged, Sub(sender, e) RenderPriceManagerProducts()
        AddHandler priceManagerProductList.ClientSizeChanged, AddressOf PriceManagerProductList_Resize
    End Sub

    Private Sub RenderPriceManagerProducts()
        If priceManagerProductList Is Nothing OrElse priceManagerProducts Is Nothing Then Return
        priceManagerProductList.SuspendLayout()
        For Each existing As Control In priceManagerProductList.Controls.Cast(Of Control)().ToArray()
            existing.Dispose()
        Next
        priceManagerProductList.Controls.Clear()
        Dim search = If(priceManagerSearchBox Is Nothing, String.Empty, priceManagerSearchBox.Text.Trim())
        Dim selectedCategory = If(priceManagerCategoryFilter Is Nothing, "All Categories", Convert.ToString(priceManagerCategoryFilter.SelectedItem))
        Dim visibleCount = 0

        For Each product As DataRow In priceManagerProducts.Rows
            Dim category = Convert.ToString(product("Category"))
            If Not String.Equals(selectedCategory, "All Categories", StringComparison.OrdinalIgnoreCase) AndAlso Not String.Equals(category, selectedCategory, StringComparison.OrdinalIgnoreCase) Then Continue For

            Dim searchable = String.Join(" ", {Convert.ToString(product("ID")), category, Convert.ToString(product("Brand Name")), Convert.ToString(product("Generic Name"))})
            If search.Length > 0 AndAlso searchable.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0 Then Continue For
            priceManagerProductList.Controls.Add(CreatePriceProductCard(product))
            visibleCount += 1
        Next

        If visibleCount = 0 Then
            priceManagerProductList.Controls.Add(New Label With {.Text = "No products match your search.", .AutoSize = True, .ForeColor = Color.Gray, .Margin = New Padding(8, 20, 8, 8)})
        End If
        priceManagerProductList.ResumeLayout()
    End Sub

    Private Sub PriceManagerProductList_Resize(sender As Object, e As EventArgs)
        If priceManagerProductList Is Nothing Then Return
        For Each card As Control In priceManagerProductList.Controls
            If TypeOf card Is Panel Then card.Width = Math.Max(320, priceManagerProductList.ClientSize.Width - 28)
        Next
    End Sub

    Private Function CreatePriceProductCard(product As DataRow) As Control
        Dim card As New Panel With {.Width = Math.Max(320, priceManagerProductList.ClientSize.Width - 28), .Height = 148, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 8), .BorderStyle = BorderStyle.FixedSingle}
        Dim brand = Convert.ToString(product("Brand Name"))
        Dim generic = Convert.ToString(product("Generic Name"))
        Dim id = Convert.ToString(product("ID"))
        Dim unit = Convert.ToString(product("Unit"))
        Dim qtyPerBox = If(product.IsNull("Qty/Box"), 0, Convert.ToInt32(product("Qty/Box")))
        Dim edit As New Button With {.Text = "Edit Prices", .Size = New Size(112, 32), .Location = New Point(card.Width - 128, 10), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        edit.FlatAppearance.BorderSize = 0
        Dim title As New Label With {.Text = brand, .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(30, 55, 43), .Location = New Point(16, 12)}
        Dim description As New Label With {.Text = "P" & id & " · " & generic & " " & Convert.ToString(product("Dosage")), .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.Gray, .Location = New Point(16, 40)}
        Dim priceArea As New TableLayoutPanel With {.Location = New Point(12, 72), .Size = New Size(card.Width - 24, 62), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .ColumnCount = 2, .RowCount = 1, .BackColor = Color.White}
        priceArea.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        priceArea.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        Dim retail = CreatePriceValuePanel("Retail Price", product("Retail Price"), "Per " & unit)
        Dim wholesale = CreatePriceValuePanel("Wholesale Price", product("Wholesale Price"), "Per box (" & qtyPerBox.ToString() & " " & unit & ")")
        priceArea.Controls.Add(retail, 0, 0)
        priceArea.Controls.Add(wholesale, 1, 0)
        card.Controls.Add(priceArea)
        card.Controls.Add(description)
        card.Controls.Add(title)
        card.Controls.Add(edit)
        AddHandler edit.Click, Sub(sender, e) EditProductPrices(product)
        AddHandler card.Resize,
            Sub(sender, e)
                priceArea.Width = Math.Max(200, card.ClientSize.Width - 24)
            End Sub
        Return card
    End Function

    Private Function CreatePriceValuePanel(caption As String, amount As Object, unitText As String) As Panel
        Dim panel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(245, 248, 246), .Margin = New Padding(0, 0, 8, 0)}
        Dim price As Decimal = If(amount Is Nothing OrElse Convert.IsDBNull(amount), 0D, Convert.ToDecimal(amount))
        panel.Controls.Add(New Label With {.Text = unitText, .AutoSize = True, .ForeColor = Color.Gray, .Font = New Font("Segoe UI", 8.5F), .Location = New Point(8, 40)})
        panel.Controls.Add(New Label With {.Text = "₱" & price.ToString("N2", CultureInfo.CurrentCulture), .AutoSize = True, .ForeColor = Color.FromArgb(30, 55, 43), .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .Location = New Point(8, 20)})
        panel.Controls.Add(New Label With {.Text = caption, .AutoSize = True, .ForeColor = Color.Gray, .Font = New Font("Segoe UI", 8.5F), .Location = New Point(8, 3)})
        Return panel
    End Function

    Private Sub EditProductPrices(product As DataRow)
        Dim retailText = Convert.ToDecimal(product("Retail Price")).ToString("0.00", CultureInfo.CurrentCulture)
        Dim wholesaleText = Convert.ToDecimal(product("Wholesale Price")).ToString("0.00", CultureInfo.CurrentCulture)
        Dim originalRetail = Convert.ToDecimal(product("Retail Price"))
        Dim originalWholesale = Convert.ToDecimal(product("Wholesale Price"))
        Dim quantityPerBox = If(product.IsNull("Qty/Box"), 0D, Convert.ToDecimal(product("Qty/Box")))
        Dim originalBoxRetailTotal = originalRetail * quantityPerBox
        Dim wholesaleMultiplier = If(originalBoxRetailTotal > 0D, originalWholesale / originalBoxRetailTotal, 1D)
        Using prompt As New Form With {.Text = "Edit Prices", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.None, .ControlBox = False, .ClientSize = New Size(420, 250), .MinimizeBox = False, .MaximizeBox = False, .BackColor = Color.White}
            AddBlackDialogBorder(prompt)
            prompt.Controls.Add(New Label With {.Text = Convert.ToString(product("Brand Name")), .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(18, 16)})
            prompt.Controls.Add(New Label With {.Text = "Retail price per " & Convert.ToString(product("Unit")), .AutoSize = True, .Location = New Point(18, 58)})
            Dim retailBox As New TextBox With {.Text = retailText, .Location = New Point(18, 80), .Width = 150}
            prompt.Controls.Add(retailBox)
            prompt.Controls.Add(New Label With {.Text = "Wholesale price per box", .AutoSize = True, .Location = New Point(195, 58)})
            Dim wholesaleBox As New TextBox With {.Text = wholesaleText, .Location = New Point(195, 80), .Width = 200, .ReadOnly = True}
            prompt.Controls.Add(wholesaleBox)
            Dim autoWholesaleCheck As New CheckBox With {.Text = "Auto-calculate wholesale using the current discount", .AutoSize = True, .Checked = True, .Location = New Point(18, 118), .ForeColor = Color.FromArgb(65, 80, 70)}
            prompt.Controls.Add(autoWholesaleCheck)
            Dim boxQuantityNote As New Label With {.Text = "Box quantity: " & quantityPerBox.ToString("0.##", CultureInfo.CurrentCulture) & " " & Convert.ToString(product("Unit")), .AutoSize = True, .ForeColor = Color.Gray, .Location = New Point(18, 146)}
            prompt.Controls.Add(boxQuantityNote)
            Dim saveButton As New Button With {.Text = "Save", .Location = New Point(230, 194), .Size = New Size(78, 34), .DialogResult = DialogResult.None, .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            Dim cancelButton As New Button With {.Text = "Cancel", .Location = New Point(318, 194), .Size = New Size(78, 34), .DialogResult = DialogResult.Cancel}
            prompt.Controls.Add(saveButton)
            prompt.Controls.Add(cancelButton)
            prompt.CancelButton = cancelButton
            Dim updateAutoWholesale As Action =
                Sub()
                    If Not autoWholesaleCheck.Checked Then Return
                    Dim retailPrice As Decimal
                    If Decimal.TryParse(retailBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, retailPrice) AndAlso retailPrice >= 0D Then
                        Dim calculated = Decimal.Round(retailPrice * quantityPerBox * wholesaleMultiplier, 2, MidpointRounding.AwayFromZero)
                        wholesaleBox.Text = calculated.ToString("0.00", CultureInfo.CurrentCulture)
                    End If
                End Sub
            AddHandler retailBox.TextChanged, Sub(sender, e) updateAutoWholesale()
            AddHandler autoWholesaleCheck.CheckedChanged,
                Sub(sender, e)
                    wholesaleBox.ReadOnly = autoWholesaleCheck.Checked
                    If autoWholesaleCheck.Checked Then
                        updateAutoWholesale()
                    Else
                        ' Turning auto-calculate off restores the saved wholesale price as the manual starting point.
                        wholesaleBox.Text = originalWholesale.ToString("0.00", CultureInfo.CurrentCulture)
                    End If
                End Sub
            AddHandler saveButton.Click,
                Sub(sender, e)
                    Dim retailPrice As Decimal
                    Dim wholesalePrice As Decimal
                    If Not Decimal.TryParse(retailBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, retailPrice) OrElse retailPrice < 0D OrElse
                       Not Decimal.TryParse(wholesaleBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, wholesalePrice) OrElse wholesalePrice < 0D Then
                        MessageBox.Show("Enter valid non-negative prices.", "Invalid price", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    Dim pricesChanged = retailPrice <> originalRetail OrElse wholesalePrice <> originalWholesale
                    If Not pricesChanged Then
                        prompt.DialogResult = DialogResult.OK
                        prompt.Close()
                        Return
                    End If

                    If Not ConfirmPriceChange(prompt,
                                              Convert.ToString(product("Brand Name")),
                                              Convert.ToString(product("Unit")),
                                              originalRetail, retailPrice,
                                              originalWholesale, wholesalePrice) Then Return

                    Try
                        Using connection = GetConnection()
                            connection.Open()
                            Const query As String = "UPDATE product SET retail_price = @retail, wholesale_price_per_box = @wholesale WHERE product_ID = @id"
                            Using command As New MySqlCommand(query, connection)
                                command.Parameters.AddWithValue("@retail", retailPrice)
                                command.Parameters.AddWithValue("@wholesale", wholesalePrice)
                                command.Parameters.AddWithValue("@id", Convert.ToInt32(product("ID")))
                                command.ExecuteNonQuery()
                            End Using

                            Const verifyQuery As String = "SELECT retail_price, wholesale_price_per_box FROM product WHERE product_ID = @id"
                            Using verifyCommand As New MySqlCommand(verifyQuery, connection)
                                verifyCommand.Parameters.AddWithValue("@id", Convert.ToInt32(product("ID")))
                                Using reader = verifyCommand.ExecuteReader()
                                    If Not reader.Read() Then Throw New InvalidOperationException("The product was not found in the connected database.")
                                    Dim savedRetail = reader.GetDecimal("retail_price")
                                    Dim savedWholesale = reader.GetDecimal("wholesale_price_per_box")
                                    If savedRetail <> retailPrice OrElse savedWholesale <> wholesalePrice Then
                                        Throw New InvalidOperationException("The database values did not match the prices you entered.")
                                    End If
                                    retailPrice = savedRetail
                                    wholesalePrice = savedWholesale
                                End Using
                            End Using
                        End Using
                        product("Retail Price") = retailPrice
                        product("Wholesale Price") = wholesalePrice
                        prompt.DialogResult = DialogResult.OK
                        prompt.Close()
                    Catch ex As Exception
                        MessageBox.Show("Could not update prices. " & ex.Message, "Price update failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Sub
            If prompt.ShowDialog(Me) = DialogResult.OK Then RenderPriceManagerProducts()
        End Using
    End Sub

    Private Function ConfirmPriceChange(owner As IWin32Window,
                                        brandName As String,
                                        unit As String,
                                        currentRetail As Decimal,
                                        newRetail As Decimal,
                                        currentWholesale As Decimal,
                                        newWholesale As Decimal) As Boolean
        Using confirmation As New Form With {
            .Text = "Confirm Price Change",
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.None,
            .ControlBox = False,
            .ClientSize = New Size(440, 250),
            .MinimizeBox = False,
            .MaximizeBox = False,
            .ShowInTaskbar = False,
            .BackColor = Color.White
        }
            AddBlackDialogBorder(confirmation)
            confirmation.Controls.Add(New Label With {.Text = "Save these prices for " & brandName & "?", .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(20, 18)})
            confirmation.Controls.Add(New Label With {.Text = "Retail price per " & unit, .AutoSize = True, .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold), .Location = New Point(20, 64)})
            confirmation.Controls.Add(New Label With {.Text = "Current: ₱" & currentRetail.ToString("N2", CultureInfo.CurrentCulture) & "     New: ₱" & newRetail.ToString("N2", CultureInfo.CurrentCulture), .AutoSize = True, .Location = New Point(20, 88)})
            confirmation.Controls.Add(New Label With {.Text = "Wholesale price per box", .AutoSize = True, .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold), .Location = New Point(20, 124)})
            confirmation.Controls.Add(New Label With {.Text = "Current: ₱" & currentWholesale.ToString("N2", CultureInfo.CurrentCulture) & "     New: ₱" & newWholesale.ToString("N2", CultureInfo.CurrentCulture), .AutoSize = True, .Location = New Point(20, 148)})
            Dim confirmButton As New Button With {.Text = "Confirm", .Location = New Point(244, 196), .Size = New Size(82, 34), .DialogResult = DialogResult.Yes, .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
            Dim cancelButton As New Button With {.Text = "Cancel", .Location = New Point(336, 196), .Size = New Size(82, 34), .DialogResult = DialogResult.Cancel}
            confirmation.Controls.Add(confirmButton)
            confirmation.Controls.Add(cancelButton)
            confirmation.AcceptButton = confirmButton
            confirmation.CancelButton = cancelButton
            Return confirmation.ShowDialog(owner) = DialogResult.Yes
        End Using
    End Function

    Private Sub AddBlackDialogBorder(dialog As Form)
        AddHandler dialog.Paint,
            Sub(sender, e)
                If dialog.ClientSize.Width < 2 OrElse dialog.ClientSize.Height < 2 Then Return
                Using borderPen As New Pen(Color.Black, 1.0F)
                    e.Graphics.DrawRectangle(borderPen, 0, 0, dialog.ClientSize.Width - 1, dialog.ClientSize.Height - 1)
                End Using
            End Sub
    End Sub

    Private Sub bodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles bodyPanel.Paint

    End Sub


    Private Class ProductDetailsWindow
        Inherits Form

        Public Sub New()
            Me.DoubleBuffered = True
            Me.SetStyle(ControlStyles.ResizeRedraw, True)
            Me.UpdateStyles()
        End Sub

        Protected Overrides ReadOnly Property CreateParams As CreateParams
            Get
                Dim parameters = MyBase.CreateParams
                parameters.ClassStyle = parameters.ClassStyle And Not &H20000
                Return parameters
            End Get
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

            Using borderPen As New Pen(Color.FromArgb(90, 100, 95), 1.0F)
                e.Graphics.DrawRectangle(
                    borderPen,
                    0, 0,
                    Me.ClientSize.Width - 1,
                    Me.ClientSize.Height - 1
                )
            End Using
        End Sub
    End Class
End Class
