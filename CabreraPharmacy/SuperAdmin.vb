Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing.Printing
Imports System.Globalization
Imports System.IO
Imports System.Threading.Tasks

Public Class SuperAdmin

    Private printGrid As DataGridView
    Private printTitle As String
    Private printRowIndex As Integer
    Private printColumns As DataGridViewColumn()
    Private WithEvents salesTransactionFilter As ComboBox
    Private WithEvents salesTransactionDate As DateTimePicker
    Private salesBackToReports As Button
    Private transactionDetailsTitle As Label
    Private salesDateFilterLabel As Label
    Private salesDateCaption As Label
    Private salesMonthCaption As Label
    Private salesYearCaption As Label
    Private salesReportMonthCombo As ComboBox
    Private salesReportYearCombo As ComboBox
    Private salesFiltersInitialized As Boolean
    Private salesReportFilterPanel As Panel
    Private salesReportDetailsPanel As Panel
    Private salesReportTotalLabel As Label
    Private salesReportPageLayout As TableLayoutPanel
    Private salesReportBody As TableLayoutPanel
    Private ReadOnly reportGridCards As New Dictionary(Of DataGridView, Panel)()
    Private ReadOnly reportStyledToolbars As New HashSet(Of Panel)()
    Private salesReportPeriodButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private salesReportModeButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private dashboardTrendLabels As New List(Of String)
    Private dashboardTrendValues As New List(Of Decimal)
    Private dashboardTodaySales As Decimal
    Private dashboardTodayTransactions As Integer
    Private dashboardLowStock As Integer
    Private dashboardNearExpiry As Integer
    Private dashboardExpired As Integer
    Private dashboardTrendPeriod As String = "7 Days"
    Private accountDataView As DataView
    Private accountListCard As Panel
    Private accountSearchBox As TextBox
    Private accountRoleFilter As ComboBox

    ' --- 1. INITIALIZATION & NAVIGATION ---
    Private Sub SuperAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        InitializeSalesReportFilters()
        AddHandler Me.Resize, AddressOf SuperAdmin_Resize
        motherPanel.Dock = DockStyle.None
        motherPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnDashboard.Tag = "Dashboard"
        btnAccounts.Tag = "Accounts"
        btnReports.Tag = "Reports"

        ' Setup DataGridView Button Columns
        usrName.Text = CurrentFullName
        TextBox23.Text = usrName.Text
        TextBox24.Text = usrName.Text
        usrName.ReadOnly = True
        accountEdit.Text = "Edit"
        accountEdit.UseColumnTextForButtonValue = True
        accountResetPassword.Text = "Reset"
        accountResetPassword.UseColumnTextForButtonValue = True
        AddHandler pnlAccounts.Resize, AddressOf LayoutAccounts_Resize
        AddHandler pnlHeaderAccounts.Resize, AddressOf LayoutAccounts_Resize
        accountsDataGrid.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        accountsDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        InitializeAccountsPresentation()
        ApplySuperAdminVisualTheme()

        ' Set initial screen
        AddHandler pnlDashboard.Resize, AddressOf LayoutDashboardCards
        AddHandler pnlReports.Resize, AddressOf LayoutReports
        AddHandler trend7DaysButton.Click, AddressOf DashboardTrendFilter_Click
        AddHandler trendThisMonthButton.Click, AddressOf DashboardTrendFilter_Click
        AddHandler trendThisYearButton.Click, AddressOf DashboardTrendFilter_Click
        AddHandler salesTrendPlot.Paint, AddressOf DashboardSalesTrend_Paint
        AddHandler Panel12.Paint, AddressOf DashboardRevenue_Paint
        AddHandler inventoryViewAllLink.LinkClicked,
            AddressOf ExpiryAlert_Click
        For Each expiryAlertControl As Control In {
            inventoryAlertNearExpiry, inventoryAlertNearExpiryCount, inventoryAlertNearExpiryCaption,
            inventoryAlertExpired, inventoryAlertExpiredCount, inventoryAlertExpiredCaption
        }
            expiryAlertControl.Cursor = Cursors.Hand
            AddHandler expiryAlertControl.Click, AddressOf ExpiryAlert_Click
        Next
        inventoryAlertNearExpiryCaption.Text = "Near Expiry" & vbCrLf & "Open batch monitor"
        inventoryAlertExpiredCaption.Text = "Expired" & vbCrLf & "Retained for tracking"
        EnsureDashboardIcons()
        For Each countLabel In {inventoryAlertLowStockCount, inventoryAlertNearExpiryCount, inventoryAlertExpiredCount}
            countLabel.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
        Next
        For Each captionLabel In {inventoryAlertLowStockCaption, inventoryAlertNearExpiryCaption, inventoryAlertExpiredCaption}
            captionLabel.Font = New Font("Segoe UI", 7.0F)
        Next
        AddHandler Panel9.Resize, AddressOf DashboardLowerPanels_Resize
        AddHandler Panel10.Resize, AddressOf DashboardLowerPanels_Resize
        dashboardDateLabel.Text = Date.Today.ToString("dddd, MMMM d, yyyy", CultureInfo.CurrentCulture)
        LoadDashboardTrend("7 Days")
        LayoutRootContent()
        LayoutPrimaryHeaders()
        LayoutAccounts()
        ShowPrimaryView(pnlDashboard, "Dashboard")
        LayoutDashboardCards(pnlDashboard, EventArgs.Empty)
        LayoutReports(pnlReports, EventArgs.Empty)

        ' Load the database data into the Accounts grid
        LoadAccounts()
        LoadSalesDashboardStats()
        UiSelectionStyler.ApplyToForm(Me)

        expensesViewReport.Enabled = False
        purchasesViewReport.Enabled = False
        btnAddExpenses.Enabled = False
        Button15.Enabled = False
        Button14.Enabled = False
        expensesDescriptions.Text = "Unavailable: expense data is not stored by this application."
        purchasesDescriptions.Text = "Unavailable: no purchase data source exists in this application."
    End Sub

    Private Sub SuperAdmin_Resize(sender As Object, e As EventArgs)
        LayoutRootContent()
        LayoutPrimaryHeaders()
        LayoutDashboardCards(pnlDashboard, EventArgs.Empty)
        LayoutAccounts()
        LayoutReports(pnlReports, EventArgs.Empty)
        LayoutAllReportDetails()
    End Sub

    Private Sub ApplySuperAdminVisualTheme()
        Dim green = Color.FromArgb(20, 100, 60)
        Dim pageGray = Color.FromArgb(245, 245, 245)
        sideBarSuperAdmin.BackColor = green
        sideBarSuperAdmin.Width = 168
        motherPanel.BackColor = pageGray

        For Each button As Button In {btnDashboard, btnAccounts, btnReports}
            button.FlatStyle = FlatStyle.Flat
            button.FlatAppearance.BorderSize = 0
            button.Font = New Font("Segoe UI", 9.0F, If(button Is btnDashboard, FontStyle.Bold, FontStyle.Regular))
            button.SetBounds(19, If(button Is btnDashboard, 92, If(button Is btnAccounts, 138, 184)), 130, 38)
            button.Padding = New Padding(10, 0, 0, 0)
            button.TextAlign = ContentAlignment.MiddleLeft
            button.UseVisualStyleBackColor = False
            button.Cursor = Cursors.Hand
        Next
        btnLogin.SetBounds(19, sideBarSuperAdmin.ClientSize.Height - btnLogin.Height - 16, 130, 34)
        btnLogin.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        btnLogin.BackColor = green
        btnLogin.ForeColor = Color.White
        btnLogin.FlatStyle = FlatStyle.Flat
        btnLogin.FlatAppearance.BorderColor = Color.White
        btnLogin.FlatAppearance.BorderSize = 1
        btnLogin.UseVisualStyleBackColor = False

        For Each header As Panel In {pnlHeaderDashboard, pnlHeaderAccounts, pnlHeaderReports}
            header.BackColor = Color.White
        Next
        For Each page As Panel In {pnlDashboard, pnlAccounts, pnlReports}
            page.BackColor = pageGray
        Next
        For Each title As Label In {hdrDashboard, hdrAccounts, Label6}
            title.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
            title.ForeColor = green
        Next
        For Each subtitle As Label In {subtitleLabel, Label3, Label5}
            subtitle.Font = New Font("Segoe UI", 9.0F)
            subtitle.ForeColor = Color.Gray
        Next
        For Each userName As TextBox In {usrName, TextBox23, TextBox24}
            userName.ReadOnly = True
            userName.TabStop = False
            userName.BorderStyle = BorderStyle.None
            userName.BackColor = Color.White
            userName.ForeColor = Color.FromArgb(25, 35, 45)
            userName.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        Next
        For Each role As Label In {usrSuperAdmin, Label2, Label4}
            role.Font = New Font("Segoe UI", 9.0F)
            role.ForeColor = green
        Next
    End Sub

    Private Sub ReportPage_Resize(sender As Object, e As EventArgs)
        LayoutAllReportDetails()
    End Sub

    Private Sub LayoutRootContent()
        If ClientSize.Width <= sideBarSuperAdmin.Width OrElse ClientSize.Height <= 0 Then Return

        motherPanel.SetBounds(sideBarSuperAdmin.Width,
                              0,
                              ClientSize.Width - sideBarSuperAdmin.Width,
                              ClientSize.Height)
        motherPanel.BringToFront()
        sideBarSuperAdmin.BringToFront()
    End Sub

    Private Sub LayoutPrimaryHeaders()
        If motherPanel.ClientSize.Width <= 0 Then Return

        Dim headerWidth = motherPanel.ClientSize.Width
        Dim headerHeight = 108
        pnlHeaderDashboard.SetBounds(0, 0, headerWidth, headerHeight)
        pnlHeaderAccounts.SetBounds(0, 0, headerWidth, headerHeight)
        pnlHeaderReports.SetBounds(0, 0, headerWidth, headerHeight)

        ' Compensate for the title font's built-in left bearing so both lines align visually.
        hdrDashboard.SetBounds(16, 27, Math.Max(1, headerWidth - 280), 32)
        subtitleLabel.SetBounds(24, 65, Math.Max(1, headerWidth - 280), 16)
        hdrDashboard.Visible = True

        hdrAccounts.SetBounds(16, 27, Math.Max(1, headerWidth - 280), 32)
        Label3.SetBounds(24, 65, Math.Max(1, headerWidth - 280), 16)
        Label6.SetBounds(16, 27, Math.Max(1, headerWidth - 280), 32)
        Label5.SetBounds(24, 65, Math.Max(1, headerWidth - 280), 16)

        LayoutHeaderUser(usrName, usrSuperAdmin, headerWidth, pnlHeaderDashboard)
        LayoutHeaderUser(TextBox23, Label2, headerWidth, pnlHeaderAccounts)
        LayoutHeaderUser(TextBox24, Label4, headerWidth, pnlHeaderReports)
    End Sub

    Private Sub LayoutHeaderUser(userName As TextBox, role As Label, headerWidth As Integer, profilePanel As Control)
        Dim userWidth = Math.Min(300, Math.Max(180, headerWidth - 40))
        Dim textLeft = headerWidth - userWidth - 24
        userName.SetBounds(textLeft, 34, userWidth, 20)
        userName.TextAlign = HorizontalAlignment.Left
        role.Location = New Point(textLeft, 57)
        role.AutoSize = False
        role.Size = New Size(userWidth, 16)
        role.TextAlign = ContentAlignment.MiddleLeft
        EnsureProfileAvatar(profilePanel, New Point(textLeft - 60, 31))
    End Sub

    Private Sub EnsureProfileAvatar(profilePanel As Control, location As Point)
        If profilePanel Is Nothing Then Return

        Dim avatar = TryCast(profilePanel.Controls("profileAvatar"), Panel)
        If avatar Is Nothing Then
            avatar = New Panel With {.Name = "profileAvatar", .Size = New Size(46, 46)}
            profilePanel.Controls.Add(avatar)
        End If

        avatar.SetBounds(location.X, location.Y, 46, 46)
        ProfileAvatarStyler.Apply(avatar)
        avatar.BringToFront()
    End Sub

    Private Sub HideAllPanels()
        pnlDashboard.Visible = False
        pnlAccounts.Visible = False
        pnlReports.Visible = False
        pnlInventoryStockReport.Visible = False
        pnlFinancialSalesReport.Visible = False
        pnlRetailSalesReport.Visible = False
        pnlExpensesReport.Visible = False
        pnlPurchasesReport.Visible = False
        pnlWholesaleSalesReport.Visible = False
        pnlEmployeesalesReport.Visible = False
        pnlLogin.Visible = False
    End Sub

    Private Sub PrintReport_Click(sender As Object, e As EventArgs) _
        Handles inventoryPrintReport.Click,
                financialPrintReport.Click,
                retailPrintReport.Click,
                wholesalePrintReport.Click,
                employeePrintReport.Click
        Dim printButton = DirectCast(sender, Button)
        If printButton Is retailPrintReport Then
            PrintSalesReportPdf()
            Return
        End If

        Dim reportGrid As DataGridView
        Dim reportTitle As String

        Select Case printButton.Name
            Case NameOf(inventoryPrintReport)
                reportGrid = inventoryDataGrid
                reportTitle = titleInventoryStockReport.Text
            Case NameOf(financialPrintReport)
                reportGrid = DataGridView4
                reportTitle = titleFinancialSalesReport.Text
            Case NameOf(retailPrintReport)
                reportGrid = retailDataGrid
                reportTitle = titleRetailSalesReport.Text
            Case NameOf(wholesalePrintReport)
                reportGrid = wholesaleDataGrid
                reportTitle = titleWholesaleSalesReport.Text
            Case Else
                reportGrid = employeDataGrid
                reportTitle = titleEmployeeSalesReport.Text
        End Select

        If reportGrid.DataSource Is Nothing Then
            MessageBox.Show("Load the report before printing.", "Print report", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        printGrid = reportGrid
        printTitle = reportTitle
        printRowIndex = 0
        printColumns = reportGrid.Columns.Cast(Of DataGridViewColumn)().
            Where(Function(column) column.Visible).
            OrderBy(Function(column) column.DisplayIndex).
            ToArray()

        Using document As New PrintDocument()
            document.DocumentName = reportTitle
            document.DefaultPageSettings.Landscape = True
            AddHandler document.PrintPage, AddressOf ReportPrintDocument_PrintPage

            Using preview As New PrintPreviewDialog()
                preview.Document = document
                preview.Width = 1100
                preview.Height = 800
                preview.StartPosition = FormStartPosition.CenterParent
                preview.ShowDialog(Me)
            End Using
        End Using
    End Sub

    Private Sub ReportPrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        If printGrid Is Nothing OrElse printColumns Is Nothing Then
            e.HasMorePages = False
            Return
        End If

        Dim bounds = e.MarginBounds
        Dim y = bounds.Top
        Dim headerHeight = 34
        Dim rowHeight = 26
        Dim widths(printColumns.Length - 1) As Single
        Dim totalGridWidth = printColumns.Sum(Function(column) Math.Max(column.Width, 1))
        Dim scale = CSng(bounds.Width) / Math.Max(totalGridWidth, 1)

        Using titleFont As New Font("Segoe UI", 14.0F, FontStyle.Bold),
              headerFont As New Font("Segoe UI", 8.0F, FontStyle.Bold),
              cellFont As New Font("Segoe UI", 8.0F),
              borderPen As New Pen(Color.LightGray),
              headerBrush As New SolidBrush(Color.FromArgb(230, 238, 233)),
              textBrush As New SolidBrush(Color.Black),
              format As New StringFormat With {.Trimming = StringTrimming.EllipsisCharacter, .FormatFlags = StringFormatFlags.NoWrap}
            e.Graphics.DrawString(printTitle, titleFont, textBrush, bounds.Left, y)
            y += titleFont.Height + 12
            e.Graphics.DrawString("Generated: " & DateTime.Now.ToString("MMM dd, yyyy h:mm tt") & " | By: " & CurrentFullName,
                                  cellFont, textBrush, bounds.Left, y)
            y += cellFont.Height + 12

            For index = 0 To printColumns.Length - 1
                widths(index) = Math.Max(1.0F, printColumns(index).Width * scale)
            Next

            Dim x = bounds.Left
            For index = 0 To printColumns.Length - 1
                Dim cellBounds As New RectangleF(x, y, widths(index), headerHeight)
                e.Graphics.FillRectangle(headerBrush, cellBounds)
                e.Graphics.DrawRectangle(borderPen, Rectangle.Round(cellBounds))
                e.Graphics.DrawString(printColumns(index).HeaderText, headerFont, textBrush,
                                      New RectangleF(cellBounds.X + 4, cellBounds.Y + 4, cellBounds.Width - 8, cellBounds.Height - 8), format)
                x += widths(index)
            Next
            y += headerHeight

            While printRowIndex < printGrid.Rows.Count AndAlso y + rowHeight <= bounds.Bottom
                Dim row = printGrid.Rows(printRowIndex)
                If Not row.IsNewRow Then
                    x = bounds.Left
                    For index = 0 To printColumns.Length - 1
                        Dim cellBounds As New RectangleF(x, y, widths(index), rowHeight)
                        e.Graphics.DrawRectangle(borderPen, Rectangle.Round(cellBounds))
                        Dim cellValue = Convert.ToString(row.Cells(printColumns(index).Index).FormattedValue)
                        e.Graphics.DrawString(cellValue, cellFont, textBrush,
                                              New RectangleF(cellBounds.X + 4, cellBounds.Y + 4, cellBounds.Width - 8, cellBounds.Height - 8), format)
                        x += widths(index)
                    Next
                    y += rowHeight
                End If
                printRowIndex += 1
            End While

            e.HasMorePages = printRowIndex < printGrid.Rows.Count
        End Using
    End Sub

    Private Sub PrintSalesReportPdf()
        Try
            Dim rows = TryCast(retailDataGrid.DataSource, DataTable)
            If rows Is Nothing Then
                MessageBox.Show("Load the report before printing.", "Print report", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim period = Convert.ToString(ComboBox4.SelectedItem, CultureInfo.InvariantCulture)
            Dim saleType = If(salesTransactionFilter.SelectedIndex <= 0, "All", Convert.ToString(salesTransactionFilter.SelectedItem, CultureInfo.InvariantCulture))
            Dim fromDate As DateTime?
            Dim throughDateExclusive As DateTime?
            GetSalesReportDateRange(fromDate, throughDateExclusive)
            If Not fromDate.HasValue OrElse Not throughDateExclusive.HasValue Then Return

            Dim startDate = fromDate.Value
            Dim endDate = throughDateExclusive.Value
            Dim periodCaption = transactionDetailsTitle.Text.Replace("Transaction Details — ", String.Empty)
            Dim destinationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "sales_report", period.ToLowerInvariant())
            Dim fileSuffix As String
            If String.Equals(period, "Daily", StringComparison.OrdinalIgnoreCase) Then
                fileSuffix = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            ElseIf String.Equals(period, "Monthly", StringComparison.OrdinalIgnoreCase) Then
                destinationFolder = Path.Combine(destinationFolder, startDate.Year.ToString(CultureInfo.InvariantCulture), startDate.ToString("MMMM", CultureInfo.InvariantCulture))
                fileSuffix = startDate.ToString("MMMM_yyyy", CultureInfo.InvariantCulture)
            Else
                destinationFolder = Path.Combine(destinationFolder, startDate.Year.ToString(CultureInfo.InvariantCulture))
                fileSuffix = startDate.Year.ToString(CultureInfo.InvariantCulture)
            End If

            Dim mode = If(String.Equals(saleType, "All", StringComparison.OrdinalIgnoreCase), Nothing, saleType)
            Dim itemDetails = New SalesRepository().GetSalesReportItemDetails(mode, startDate, endDate)
            Dim total = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
            Dim retailRows = rows.AsEnumerable().Where(Function(row) String.Equals(Convert.ToString(row("SaleType")), "Retail", StringComparison.OrdinalIgnoreCase)).ToList()
            Dim wholesaleRows = rows.AsEnumerable().Where(Function(row) String.Equals(Convert.ToString(row("SaleType")), "Wholesale", StringComparison.OrdinalIgnoreCase)).ToList()
            Dim itemCount = itemDetails.AsEnumerable().Sum(Function(row) If(row.IsNull("Quantity"), 0, Convert.ToInt32(row("Quantity"))))
            Dim lines As New List(Of String) From {
                "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES",
                period & " Sales Report",
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
            Else
                lines.Insert(6, "@SUMMARY|Wholesale Sales|PHP " & wholesaleRows.Sum(Function(row) Convert.ToDecimal(row("Total"))).ToString("N2", CultureInfo.InvariantCulture))
            End If

            If String.Equals(period, "Yearly", StringComparison.OrdinalIgnoreCase) Then
                Dim monthlyTotals = Enumerable.Range(1, 12).Select(Function(monthNumber) New With {
                    .MonthLabel = New DateTime(startDate.Year, monthNumber, 1).ToString("MMMM", CultureInfo.CurrentCulture),
                    .Sales = rows.AsEnumerable().Where(Function(row) Convert.ToDateTime(row("SaleDate")).Month = monthNumber).Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
                }).ToList()
                Dim insertAt = lines.FindIndex(Function(line) line.StartsWith("@TOTAL|", StringComparison.Ordinal))
                For monthIndex = 0 To monthlyTotals.Count - 1 Step 2
                    Dim leftMonth = monthlyTotals(monthIndex)
                    If monthIndex + 1 < monthlyTotals.Count Then
                        Dim rightMonth = monthlyTotals(monthIndex + 1)
                        lines.Insert(insertAt, "@MONTHSUMMARY|" & leftMonth.MonthLabel & "|PHP " & leftMonth.Sales.ToString("N2", CultureInfo.InvariantCulture) & "|" & rightMonth.MonthLabel & "|PHP " & rightMonth.Sales.ToString("N2", CultureInfo.InvariantCulture))
                    Else
                        lines.Insert(insertAt, "@MONTHSUMMARY|" & leftMonth.MonthLabel & "|PHP " & leftMonth.Sales.ToString("N2", CultureInfo.InvariantCulture))
                    End If
                    insertAt += 1
                Next
            End If

            Const tableHeader = "@TABLE|TRANSACTION ID|SALE DATE / TIME|CUSTOMER|CASHIER|TYPE|MEDICINE / DETAILS|QTY|UNIT PRICE|AMOUNT TOTAL"
            If String.Equals(period, "Yearly", StringComparison.OrdinalIgnoreCase) Then
                For Each monthGroup In itemDetails.AsEnumerable().GroupBy(Function(row) Convert.ToDateTime(row("SaleDate")).Month).OrderBy(Function(group) group.Key)
                    Dim monthRows = rows.AsEnumerable().Where(Function(row) Convert.ToDateTime(row("SaleDate")).Month = monthGroup.Key).ToList()
                    Dim monthDate = New DateTime(startDate.Year, monthGroup.Key, 1)
                    Dim monthSales = monthRows.Sum(Function(row) Convert.ToDecimal(row("Total")))
                    lines.Add("@MONTH|" & monthDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture) & "|Transactions: " & monthRows.Count.ToString(CultureInfo.InvariantCulture) & "|Sales: PHP " & monthSales.ToString("N2", CultureInfo.InvariantCulture))
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
            Dim reportPrefix = If(String.Equals(saleType, "Retail", StringComparison.OrdinalIgnoreCase), "retail_sales_report", If(String.Equals(saleType, "Wholesale", StringComparison.OrdinalIgnoreCase), "wholesale_sales_report", "sales_summary"))
            Dim outputPath = Path.Combine(destinationFolder, reportPrefix & "_" & fileSuffix & ".pdf")
            Staff.CreateReportPdf(outputPath, lines)
            Process.Start(New ProcessStartInfo(outputPath) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to create or open the sales report PDF. " & ex.Message, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AddSalesReportItemLine(lines As List(Of String), row As DataRow)
        Dim medicineName = Convert.ToString(row("BrandName"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("GenericName"))) Then medicineName &= " / " & Convert.ToString(row("GenericName"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("Dosage"))) Then medicineName &= " / " & Convert.ToString(row("Dosage"))
        If Not String.IsNullOrWhiteSpace(Convert.ToString(row("Unit"))) Then medicineName &= " (" & Convert.ToString(row("Unit")) & ")"
        lines.Add(String.Join("|", {
            "@TABLE",
            Convert.ToString(row("TransactionID"), CultureInfo.InvariantCulture),
            Convert.ToDateTime(row("SaleDate")).ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
            Convert.ToString(row("Customer")), Convert.ToString(row("Cashier")), Convert.ToString(row("SaleType")),
            medicineName, Convert.ToString(row("Quantity"), CultureInfo.InvariantCulture),
            "PHP " & Convert.ToDecimal(row("UnitPrice")).ToString("N2", CultureInfo.InvariantCulture),
            "PHP " & Convert.ToDecimal(row("LineTotal")).ToString("N2", CultureInfo.InvariantCulture)
        }))
    End Sub

    Private Sub ShowPrimaryView(view As Panel, pageTitle As String)
        HideAllPanels()
        view.Visible = True
        view.BringToFront()

        hdrDashboard.Text = pageTitle
        hdrAccounts.Text = pageTitle
        Label6.Text = pageTitle
        LayoutPrimaryHeaders()

        Select Case pageTitle
            Case "Dashboard"
                LayoutDashboardCards(pnlDashboard, EventArgs.Empty)
            Case "Accounts"
                LayoutAccounts()
            Case "Reports"
                LayoutReports(pnlReports, EventArgs.Empty)
        End Select

        SetActiveNavigation(pageTitle)
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        ShowPrimaryView(pnlDashboard, "Dashboard")
        LoadSalesDashboardStats()
    End Sub

    Private Sub LayoutDashboardCards(sender As Object, e As EventArgs)
        If pnlDashboard.ClientSize.Width <= 0 OrElse pnlDashboard.ClientSize.Height <= 0 Then Return

        TableLayoutPanel2.Visible = True
        TableLayoutPanel3.Visible = True
        TableLayoutPanel4.Visible = True
        For Each cardLayout As TableLayoutPanel In {TableLayoutPanel1, TableLayoutPanel2}
            cardLayout.ColumnCount = 4
            cardLayout.ColumnStyles.Clear()
            For index = 0 To cardLayout.ColumnCount - 1
                cardLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0F))
            Next
            cardLayout.RowCount = 1
            cardLayout.RowStyles.Clear()
            cardLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            cardLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            cardLayout.Margin = Padding.Empty
            cardLayout.Padding = Padding.Empty
        Next

        For Each card In {Panel1, Panel2, Panel3, Panel4, Panel5, Panel6, Panel7, Panel8}
            card.Margin = New Padding(8)
        Next

        Dim left = 20
        Dim cardInset = 8
        Dim dateTop = pnlHeaderDashboard.Height + 10
        Dim firstCardsTop = dateTop + 36
        Dim rightMargin = 20
        Dim summaryHeight = 118
        Dim rowGap = 12
        Dim chartHeight = Math.Max(220, CInt(pnlDashboard.ClientSize.Height * 0.3))
        Dim bottomHeight = Math.Max(155, CInt(pnlDashboard.ClientSize.Height * 0.18))
        Dim availableWidth = Math.Max(1, pnlDashboard.ClientSize.Width - left - rightMargin)
        Dim secondCardsTop = firstCardsTop + summaryHeight + rowGap
        Dim chartTop = secondCardsTop + summaryHeight + 12
        Dim bottomTop = chartTop + chartHeight + rowGap
        Dim requiredHeight = bottomTop + bottomHeight + 20
        pnlDashboard.AutoScroll = pnlDashboard.ClientSize.Height < requiredHeight
        TableLayoutPanel1.Visible = True
        TableLayoutPanel2.Visible = True
        pnlDashboard.AutoScrollMinSize = If(pnlDashboard.AutoScroll, New Size(availableWidth, requiredHeight), Size.Empty)
        dashboardDateLabel.SetBounds(left + cardInset, dateTop, 244, 28)
        ApplyDashboardRoundedCorners(dashboardDateLabel, 10)
        TableLayoutPanel1.SetBounds(left, firstCardsTop, availableWidth, summaryHeight)
        TableLayoutPanel2.SetBounds(left, secondCardsTop, availableWidth, summaryHeight)
        TableLayoutPanel4.SetBounds(left, chartTop, availableWidth, chartHeight)
        TableLayoutPanel3.SetBounds(left, bottomTop, availableWidth, bottomHeight)
        salesTrendTitleLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        salesTrendLegendLabel.Font = New Font("Segoe UI", 8.0F)
        revenueTitleLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        inventoryAlertsTitleLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        quickOverviewTitleLabel.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        salesTrendPlot.SetBounds(16, 58, Math.Max(1, Panel11.ClientSize.Width - 32), Math.Max(1, Panel11.ClientSize.Height - 78))
        trendFiltersPanel.Top = 8
        trendFiltersPanel.Left = Panel11.ClientSize.Width - trendFiltersPanel.Width - 16
        revenueNetIncomeLabel.SetBounds(16, Math.Max(120, Panel12.ClientSize.Height - 52), Math.Max(1, Panel12.ClientSize.Width - 32), 36)
        LayoutDashboardAlertCards()
        LayoutDashboardQuickOverview()

        FormatDashboardCard(Panel1, todaysSale, userIDText, monthDateYear)
        FormatDashboardCard(Panel2, TextBox2, TextBox3, transactionRetail)
        FormatDashboardCard(Panel3, TextBox5, TextBox6, transactionsWholesale)
        FormatDashboardCard(Panel4, TextBox14, TextBox15, allCategories)
        FormatDashboardCard(Panel5, TextBox8, TextBox9, needOrdering)
        FormatDashboardCard(Panel6, TextBox11, TextBox12, days)
        FormatDashboardCard(Panel7, TextBox17, TextBox19, requiresDisposal)
        FormatDashboardCard(Panel8, TextBox18, TextBox20, expensesRecorded)

        For Each card In {Panel1, Panel2, Panel3, Panel4, Panel5, Panel6, Panel7, Panel8}
            ApplyDashboardRoundedCorners(card, 12)
        Next
        For Each card In {Panel9, Panel10, Panel11, Panel12}
            ApplyDashboardRoundedCorners(card, 14)
        Next
        For Each alertCard In {inventoryAlertLowStock, inventoryAlertNearExpiry, inventoryAlertExpired}
            ApplyDashboardRoundedCorners(alertCard, 10)
        Next
    End Sub

    Private Sub LayoutDashboardAlertCards()
        Dim inset = 16
        Dim gap = 10
        Dim width = Math.Max(1, (Panel9.ClientSize.Width - inset * 2 - gap * 2) \ 3)
        Dim height = Math.Max(60, Panel9.ClientSize.Height - 54)
        inventoryAlertLowStock.SetBounds(inset, 42, width, height)
        inventoryAlertNearExpiry.SetBounds(inset + width + gap, 42, width, height)
        inventoryAlertExpired.SetBounds(inset + (width + gap) * 2, 42, width, height)
        For Each alertCard In {inventoryAlertLowStock, inventoryAlertNearExpiry, inventoryAlertExpired}
            ApplyDashboardRoundedCorners(alertCard, 10)
            Dim icon = alertCard.Controls("dashboardAlertIcon")
            If icon IsNot Nothing Then icon.SetBounds(8, 3, 28, 25)
        Next
        inventoryViewAllLink.Left = Panel9.ClientSize.Width - inventoryViewAllLink.Width - 16
    End Sub

    Private Sub LayoutDashboardQuickOverview()
        Dim labels = {quickOverviewRestockLabel, quickOverviewExpiryLabel, quickOverviewTransactionsLabel}
        Dim values = {quickOverviewRestockValue, quickOverviewExpiryValue, quickOverviewTransactionsValue}
        For index = 0 To labels.Length - 1
            Dim y = 38 + index * 28
            Dim icon = Panel10.Controls("dashboardQuickIcon" & index.ToString(CultureInfo.InvariantCulture))
            If icon IsNot Nothing Then icon.SetBounds(16, y, 24, 24)
            labels(index).SetBounds(44, y, Math.Max(1, Panel10.ClientSize.Width - 170), 24)
            values(index).SetBounds(Panel10.ClientSize.Width - 126, y, 110, 24)
        Next
        quickOverviewSeparator1.SetBounds(16, 64, Math.Max(1, Panel10.ClientSize.Width - 32), 1)
        quickOverviewSeparator2.SetBounds(16, 92, Math.Max(1, Panel10.ClientSize.Width - 32), 1)
    End Sub

    Private Sub EnsureDashboardIcons()
        Dim alertPanels = {inventoryAlertLowStock, inventoryAlertNearExpiry, inventoryAlertExpired}
        Dim alertGlyphs = {"⚠", "◷", "!"}
        Dim alertColors = {Color.FromArgb(95, 75, 0), Color.FromArgb(140, 85, 20), Color.FromArgb(145, 45, 45)}
        For index = 0 To alertPanels.Length - 1
            EnsureDashboardIcon(alertPanels(index), "dashboardAlertIcon", alertGlyphs(index), alertColors(index), "Segoe UI Symbol", 12.0F)
        Next

        Dim quickGlyphs = {"▣", "◷", "↻"}
        For index = 0 To quickGlyphs.Length - 1
            EnsureDashboardIcon(Panel10, "dashboardQuickIcon" & index.ToString(CultureInfo.InvariantCulture), quickGlyphs(index), Color.FromArgb(20, 100, 60), "Segoe UI Symbol", 10.0F)
        Next
    End Sub

    Private Sub EnsureDashboardIcon(host As Control, name As String, glyph As String, color As Color, fontFamily As String, fontSize As Single)
        If host.Controls(name) IsNot Nothing Then Return
        Dim icon As New Label With {
            .Name = name,
            .Text = glyph,
            .BackColor = Color.Transparent,
            .ForeColor = color,
            .Font = New Font(fontFamily, fontSize, FontStyle.Bold),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Size = New Size(28, 25)
        }
        host.Controls.Add(icon)
        icon.BringToFront()
    End Sub

    Private Sub DashboardChartPanel_Resize(sender As Object, e As EventArgs)
        salesTrendPlot.SetBounds(16, 58, Math.Max(1, Panel11.ClientSize.Width - 32), Math.Max(1, Panel11.ClientSize.Height - 78))
        trendFiltersPanel.Top = 8
        trendFiltersPanel.Left = Panel11.ClientSize.Width - trendFiltersPanel.Width - 16
        revenueNetIncomeLabel.SetBounds(16, Math.Max(120, Panel12.ClientSize.Height - 52), Math.Max(1, Panel12.ClientSize.Width - 32), 36)
    End Sub

    Private Sub DashboardLowerPanels_Resize(sender As Object, e As EventArgs)
        LayoutDashboardAlertCards()
        LayoutDashboardQuickOverview()
        For Each card In {Panel9, Panel10}
            ApplyDashboardRoundedCorners(card, 14)
        Next
        For Each alertCard In {inventoryAlertLowStock, inventoryAlertNearExpiry, inventoryAlertExpired}
            ApplyDashboardRoundedCorners(alertCard, 10)
        Next
    End Sub

    Private Sub ApplyDashboardRoundedCorners(control As Control, radius As Integer)
        If control Is Nothing OrElse control.Width <= 0 OrElse control.Height <= 0 Then Return

        Dim diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height))
        Dim bounds As New Rectangle(0, 0, control.Width, control.Height)
        Using path As New Drawing.Drawing2D.GraphicsPath()
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90)
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90)
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90)
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90)
            path.CloseFigure()

            Dim previousRegion = control.Region
            control.Region = New Region(path)
            If previousRegion IsNot Nothing Then previousRegion.Dispose()
        End Using
    End Sub

    Private Sub DashboardTrendFilter_Click(sender As Object, e As EventArgs)
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return
        LoadDashboardTrend(Convert.ToString(button.Tag, CultureInfo.InvariantCulture))
    End Sub

    Private Sub LoadDashboardTrend(period As String)
        Dim today = Date.Today
        Dim startDate = today.AddDays(-6)
        Dim endDate = today.AddDays(1)
        Select Case period
            Case "This Month"
                startDate = New DateTime(today.Year, today.Month, 1)
            Case "This Year"
                startDate = New DateTime(today.Year, 1, 1)
            Case Else
                period = "7 Days"
        End Select

        dashboardTrendPeriod = period
        dashboardTrendLabels.Clear()
        dashboardTrendValues.Clear()
        Try
            Dim transactions = New SalesRepository().GetTransactionReport(Nothing, Nothing, startDate, endDate)
            dashboardTodayTransactions = transactions.AsEnumerable().Count(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")).Date = today)
            If period = "This Year" Then
                For monthOffset = 0 To 11
                    Dim monthStart = New DateTime(today.Year, 1, 1).AddMonths(monthOffset)
                    Dim monthEnd = monthStart.AddMonths(1)
                    dashboardTrendLabels.Add(monthStart.ToString("MMM", CultureInfo.CurrentCulture))
                    dashboardTrendValues.Add(transactions.AsEnumerable().Where(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")) >= monthStart AndAlso CDate(row("SaleDate")) < monthEnd).Sum(Function(row) Convert.ToDecimal(row("Total"), CultureInfo.InvariantCulture)))
                Next
            Else
                Dim dayCount = If(period = "This Month", Date.DaysInMonth(today.Year, today.Month), 7)
                For dayOffset = 0 To dayCount - 1
                    Dim currentDay = startDate.AddDays(dayOffset)
                    dashboardTrendLabels.Add(If(period = "This Month", currentDay.Day.ToString(CultureInfo.InvariantCulture), currentDay.ToString("ddd", CultureInfo.CurrentCulture)))
                    dashboardTrendValues.Add(transactions.AsEnumerable().Where(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")).Date = currentDay.Date).Sum(Function(row) Convert.ToDecimal(row("Total"), CultureInfo.InvariantCulture)))
                Next
            End If
        Catch ex As Exception
            dashboardTrendLabels.Clear()
            dashboardTrendValues.Clear()
            dashboardTodayTransactions = 0
        End Try

        For Each filterButton In {trend7DaysButton, trendThisMonthButton, trendThisYearButton}
            Dim active = String.Equals(Convert.ToString(filterButton.Tag), dashboardTrendPeriod, StringComparison.OrdinalIgnoreCase)
            filterButton.BackColor = If(active, Color.FromArgb(20, 100, 60), Color.White)
            filterButton.ForeColor = If(active, Color.White, Color.FromArgb(20, 100, 60))
            filterButton.FlatAppearance.BorderSize = If(active, 0, 1)
        Next
        quickOverviewTransactionsValue.Text = dashboardTodayTransactions.ToString(CultureInfo.CurrentCulture)
        salesTrendPlot.Invalidate()
        Panel12.Invalidate()
    End Sub

    Private Sub DashboardSalesTrend_Paint(sender As Object, e As PaintEventArgs)
        Dim plot = TryCast(sender, Panel)
        If plot Is Nothing Then Return
        e.Graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
        Dim bounds = plot.ClientRectangle
        Dim chart = New Rectangle(bounds.Left + 58, bounds.Top + 8, Math.Max(1, bounds.Width - 70), Math.Max(1, bounds.Height - 36))
        Using gridPen As New Pen(Color.FromArgb(235, 235, 235)), axisFont As New Font("Segoe UI", 7.0F), labelFont As New Font("Segoe UI", 7.0F), textBrush As New SolidBrush(Color.Gray), salesBrush As New SolidBrush(Color.FromArgb(20, 145, 95))
            Dim maxValue = If(dashboardTrendValues.Count = 0, 0D, dashboardTrendValues.Max())
            For index = 0 To 3
                Dim y = chart.Top + CInt(chart.Height * index / 3.0F)
                e.Graphics.DrawLine(gridPen, chart.Left, y, chart.Right, y)
                Dim tick = maxValue * (3 - index) / 3D
                e.Graphics.DrawString("₱" & tick.ToString("N0", CultureInfo.CurrentCulture), axisFont, textBrush, bounds.Left, y - 7)
            Next
            If dashboardTrendValues.Count = 0 Then Return
            Dim maximum = Math.Max(1D, CDbl(maxValue))
            Dim groupWidth = CSng(chart.Width) / dashboardTrendValues.Count
            Dim barWidth = Math.Max(2.0F, Math.Min(18.0F, groupWidth * 0.55F))
            For index = 0 To dashboardTrendValues.Count - 1
                Dim center = chart.Left + CSng(index + 0.5F) * groupWidth
                Dim barHeight = CInt(CDbl(dashboardTrendValues(index)) / maximum * chart.Height)
                Dim bar As New RectangleF(center - barWidth / 2.0F, chart.Bottom - barHeight, barWidth, Math.Max(0, barHeight))
                e.Graphics.FillRectangle(salesBrush, bar)
                If index Mod Math.Max(1, dashboardTrendValues.Count \ 10) = 0 OrElse index = dashboardTrendValues.Count - 1 Then
                    Dim label = dashboardTrendLabels(index)
                    Dim size = e.Graphics.MeasureString(label, labelFont)
                    e.Graphics.DrawString(label, labelFont, textBrush, center - size.Width / 2.0F, chart.Bottom + 4)
                End If
            Next
        End Using
    End Sub

    Private Sub DashboardRevenue_Paint(sender As Object, e As PaintEventArgs)
        Dim panel = TryCast(sender, Panel)
        If panel Is Nothing Then Return
        e.Graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
        Dim center = New Point(86, Math.Min(111, panel.ClientSize.Height \ 2 - 9))
        Using ringPen As New Pen(Color.FromArgb(225, 230, 226), 15.0F), font As New Font("Segoe UI", 8.0F, FontStyle.Bold), smallFont As New Font("Segoe UI", 7.5F), textBrush As New SolidBrush(Color.FromArgb(25, 25, 25)), grayBrush As New SolidBrush(Color.DimGray)
            Dim ring As New Rectangle(center.X - 34, center.Y - 34, 68, 68)
            e.Graphics.DrawArc(ringPen, ring, -90, 359)
            Dim centerLabel = "N/A"
            Dim size = e.Graphics.MeasureString(centerLabel, font)
            e.Graphics.DrawString(centerLabel, font, grayBrush, center.X - size.Width / 2.0F, center.Y - 7)
            e.Graphics.DrawString("Revenue", smallFont, grayBrush, 140, 62)
            e.Graphics.DrawString("₱" & dashboardTodaySales.ToString("N2", CultureInfo.CurrentCulture) & " today", font, textBrush, 140, 77)
            e.Graphics.DrawString("Expenses", smallFont, grayBrush, 140, 103)
            e.Graphics.DrawString("Unavailable — no expense records", smallFont, grayBrush, 140, 118)
        End Using
    End Sub

    Private Sub FormatDashboardCard(card As Panel, title As TextBox, value As TextBox, footer As TextBox)
        If card.ClientSize.Width <= 0 OrElse card.ClientSize.Height <= 0 Then Return

        card.BackColor = Color.White
        title.ReadOnly = True
        title.TabStop = False
        title.BorderStyle = BorderStyle.None
        title.BackColor = Color.White
        If title.Font.Size <> 10.0F OrElse title.Font.Style <> FontStyle.Bold Then title.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        title.Location = New Point(16, 12)
        title.Size = New Size(Math.Max(80, card.ClientSize.Width - 32), 22)

        value.ReadOnly = True
        value.TabStop = False
        value.BorderStyle = BorderStyle.None
        value.BackColor = Color.White
        If value.Font.Size <> 17.0F OrElse value.Font.Style <> FontStyle.Regular Then value.Font = New Font("Segoe UI", 17.0F)
        value.Location = New Point(16, 40)
        value.Size = New Size(Math.Max(80, card.ClientSize.Width - 32), 36)

        footer.ReadOnly = True
        footer.TabStop = False
        footer.BorderStyle = BorderStyle.None
        footer.BackColor = Color.White
        If footer.Font.Size <> 8.0F OrElse footer.Font.Style <> FontStyle.Regular Then footer.Font = New Font("Segoe UI", 8.0F)
        footer.ForeColor = Color.Gray
        footer.Location = New Point(16, 74)
        footer.Size = New Size(Math.Max(80, card.ClientSize.Width - 32), 18)
    End Sub

    Private Sub btnAccounts_Click(sender As Object, e As EventArgs) Handles btnAccounts.Click
        ShowPrimaryView(pnlAccounts, "Accounts")
        LoadAccounts() ' Refresh data when opened
    End Sub

    Private Sub InitializeAccountsPresentation()
        accountListCard = New Panel With {.Name = "accountListCard", .BackColor = Color.White, .BorderStyle = BorderStyle.None}

        accountSearchBox = New TextBox With {
            .Name = "accountSearchBox",
            .PlaceholderText = "Search users...",
            .Font = New Font("Segoe UI", 9.0F),
            .BorderStyle = BorderStyle.None,
            .BackColor = Color.FromArgb(247, 250, 248)
        }
        accountRoleFilter = New ComboBox With {
            .Name = "accountRoleFilter",
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F),
            .FlatStyle = FlatStyle.Flat
        }
        accountRoleFilter.Items.AddRange(New Object() {"All Roles", "Super Admin", "Admin", "Staff"})
        accountRoleFilter.SelectedIndex = 0
        AddHandler accountSearchBox.TextChanged, AddressOf AccountFilters_Changed
        AddHandler accountRoleFilter.SelectedIndexChanged, AddressOf AccountFilters_Changed

        numberTotalUsersRegistered.ReadOnly = True
        numberTotalUsersRegistered.TabStop = False
        numberTotalUsersRegistered.BackColor = Color.White
        numberTotalUsersRegistered.ForeColor = Color.FromArgb(95, 105, 100)
        numberTotalUsersRegistered.Font = New Font("Segoe UI", 9.0F)
        btnAddAccount.Text = "+  Add Account"
        btnAddAccount.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        btnAddAccount.Size = New Size(132, 34)
        btnAddAccount.BackColor = Color.FromArgb(18, 101, 54)
        btnAddAccount.FlatAppearance.BorderSize = 0
        AddHandler btnAddAccount.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(btnAddAccount, 8)
        ApplyDashboardRoundedCorners(btnAddAccount, 8)

        pnlAccounts.Controls.Remove(accountsDataGrid)
        pnlAccounts.Controls.Remove(numberTotalUsersRegistered)
        pnlAccounts.Controls.Remove(btnAddAccount)
        accountListCard.Controls.Add(numberTotalUsersRegistered)
        accountListCard.Controls.Add(accountSearchBox)
        accountListCard.Controls.Add(accountRoleFilter)
        accountListCard.Controls.Add(btnAddAccount)
        accountListCard.Controls.Add(accountsDataGrid)
        pnlAccounts.Controls.Add(accountListCard)

        accountsDataGrid.BackgroundColor = Color.White
        accountsDataGrid.BorderStyle = BorderStyle.None
        accountsDataGrid.AllowUserToAddRows = False
        accountsDataGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        accountsDataGrid.GridColor = Color.FromArgb(229, 234, 231)
        accountsDataGrid.RowHeadersVisible = False
        accountsDataGrid.EnableHeadersVisualStyles = False
        accountsDataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        accountsDataGrid.ColumnHeadersHeight = 38
        accountsDataGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 247, 243)
        accountsDataGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(65, 78, 70)
        accountsDataGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        accountsDataGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        accountsDataGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        accountsDataGrid.DefaultCellStyle.ForeColor = Color.FromArgb(35, 45, 40)
        accountsDataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 247, 240)
        accountsDataGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(35, 45, 40)
        accountsDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 252, 251)
        accountsDataGrid.RowTemplate.Height = 44
        accountsDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        accountsDataGrid.MultiSelect = False
        accountUserID.FillWeight = 8
        accountFullName.FillWeight = 22
        accountUsername.FillWeight = 15
        accountRole.FillWeight = 14
        accountStatus.FillWeight = 12
        accountEdit.FillWeight = 10
        accountResetPassword.FillWeight = 12
        accountEnableDisable.FillWeight = 10
        accountEdit.HeaderText = "ACTIONS"
        accountEdit.FlatStyle = FlatStyle.Flat
        accountResetPassword.HeaderText = String.Empty
        accountResetPassword.FlatStyle = FlatStyle.Flat
        accountEnableDisable.HeaderText = String.Empty
        accountEdit.DefaultCellStyle.BackColor = Color.White
        accountEdit.DefaultCellStyle.ForeColor = Color.FromArgb(25, 95, 65)
        accountResetPassword.DefaultCellStyle.BackColor = Color.White
        accountResetPassword.DefaultCellStyle.ForeColor = Color.FromArgb(45, 55, 50)
        AddHandler accountsDataGrid.CellFormatting, AddressOf AccountsGrid_CellFormatting

        StyleAccountSummaryCard(panelSuperAdmin, titleSuperAdmin, numberSuperAdmin, "Administrative privileges", "shield")
        StyleAccountSummaryCard(panelAdmin, titleAdmin, numberAdmin, "Pharmacist accounts", "user")
        StyleAccountSummaryCard(panelAssistant, titleAssistant, numberAssistant, "Assistant accounts", "users")
    End Sub

    Private Sub StyleAccountSummaryCard(card As Panel, title As TextBox, value As TextBox, footerText As String, iconKind As String)
        card.BackColor = Color.White
        card.BorderStyle = BorderStyle.None
        card.Margin = New Padding(7)
        card.Controls.Add(New Label With {
            .Name = "accountSummaryFooter",
            .Text = footerText,
            .AutoSize = False,
            .Font = New Font("Segoe UI", 8.0F),
            .ForeColor = Color.FromArgb(105, 115, 110),
            .Location = New Point(18, 91),
            .Size = New Size(240, 18)
        })
        Dim divider As New Panel With {.Name = "accountSummaryDivider", .BackColor = Color.FromArgb(225, 231, 228), .Location = New Point(18, 82), .Size = New Size(Math.Max(1, card.ClientSize.Width - 36), 1), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
        card.Controls.Add(divider)

        Dim icon As New Panel With {.Name = "accountSummaryIcon", .BackColor = Color.FromArgb(236, 248, 242), .Size = New Size(32, 32), .Tag = iconKind}
        AddHandler icon.Paint, AddressOf AccountRoleIcon_Paint
        card.Controls.Add(icon)
        icon.BringToFront()

        title.ReadOnly = True
        title.TabStop = False
        title.BorderStyle = BorderStyle.None
        title.BackColor = Color.White
        title.ForeColor = Color.FromArgb(45, 55, 50)
        title.Font = New Font("Segoe UI", 10.0F)
        value.ReadOnly = True
        value.TabStop = False
        value.BorderStyle = BorderStyle.None
        value.BackColor = Color.White
        value.ForeColor = Color.FromArgb(18, 101, 54)
        value.Font = New Font("Segoe UI", 21.0F, FontStyle.Bold)
    End Sub

    Private Sub AccountRoleIcon_Paint(sender As Object, e As PaintEventArgs)
        Dim icon = TryCast(sender, Panel)
        If icon Is Nothing Then Return
        e.Graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
        Using pen As New Pen(Color.FromArgb(18, 101, 54), 1.6F)
            Dim centerX = icon.ClientSize.Width / 2.0F
            e.Graphics.DrawEllipse(pen, centerX - 3.2F, 6.0F, 6.4F, 6.4F)
            e.Graphics.DrawArc(pen, centerX - 7.0F, 14.0F, 14.0F, 12.0F, 180.0F, 180.0F)
            If String.Equals(Convert.ToString(icon.Tag), "users", StringComparison.Ordinal) Then
                e.Graphics.DrawArc(pen, centerX - 12.0F, 16.0F, 10.0F, 9.0F, 185.0F, 170.0F)
                e.Graphics.DrawEllipse(pen, centerX - 10.0F, 7.0F, 4.4F, 4.4F)
            ElseIf String.Equals(Convert.ToString(icon.Tag), "shield", StringComparison.Ordinal) Then
                Dim shield = {New PointF(centerX - 8.0F, 4.0F), New PointF(centerX + 8.0F, 4.0F), New PointF(centerX + 7.0F, 14.0F), New PointF(centerX, 20.0F), New PointF(centerX - 7.0F, 14.0F)}
                e.Graphics.DrawPolygon(pen, shield)
            End If
        End Using
    End Sub

    Private Sub AccountFilters_Changed(sender As Object, e As EventArgs)
        If accountDataView Is Nothing Then Return
        Dim query = accountSearchBox.Text.Trim().Replace("'", "''").Replace("[", "[[]").Replace("]", "[]]").Replace("%", "[%]").Replace("*", "[*]")
        Dim filters As New List(Of String)
        If query.Length > 0 Then
            filters.Add("([FullName] LIKE '%" & query & "%' OR [Username] LIKE '%" & query & "%')")
        End If
        If accountRoleFilter.SelectedIndex > 0 Then
            Dim role = Convert.ToString(accountRoleFilter.SelectedItem, CultureInfo.InvariantCulture).Replace("'", "''")
            filters.Add("[Role] = '" & role & "'")
        End If
        accountDataView.RowFilter = String.Join(" AND ", filters)
    End Sub

    Private Sub AccountsGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Select Case accountsDataGrid.Columns(e.ColumnIndex).Name
            Case "accountRole"
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                e.CellStyle.ForeColor = Color.FromArgb(55, 70, 62)
                e.CellStyle.BackColor = Color.FromArgb(244, 247, 245)
            Case "accountStatus"
                Dim active = String.Equals(Convert.ToString(e.Value), "Active", StringComparison.OrdinalIgnoreCase)
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                e.CellStyle.ForeColor = If(active, Color.FromArgb(24, 126, 76), Color.FromArgb(170, 65, 65))
                e.CellStyle.BackColor = If(active, Color.FromArgb(232, 248, 240), Color.FromArgb(253, 238, 238))
                e.CellStyle.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        End Select
    End Sub

    Private Sub LayoutAccounts_Resize(sender As Object, e As EventArgs)
        LayoutAccounts()
    End Sub

    Private Sub LayoutAccounts()
        If pnlAccounts.ClientSize.Width <= 0 OrElse pnlAccounts.ClientSize.Height <= 0 Then Return

        Dim horizontalPadding = 34
        Dim top = pnlHeaderAccounts.Height + 16
        Dim availableWidth = Math.Max(1, pnlAccounts.ClientSize.Width - horizontalPadding * 2)
        Dim summaryHeight = 132
        Dim gap = 14
        Dim listTop = top + summaryHeight + gap

        accountTableLayout.SetBounds(horizontalPadding, top, availableWidth, summaryHeight)
        accountTableLayout.Margin = Padding.Empty
        accountTableLayout.Padding = Padding.Empty
        accountTableLayout.ColumnCount = 3
        accountTableLayout.ColumnStyles.Clear()
        For index = 0 To 2
            accountTableLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333F))
        Next

        For Each card In {panelSuperAdmin, panelAdmin, panelAssistant}
            card.Margin = New Padding(7)
            ApplyDashboardRoundedCorners(card, 14)
            Dim title = TryCast(card.Controls.OfType(Of TextBox)().FirstOrDefault(Function(control) control.Name.StartsWith("title", StringComparison.Ordinal)), TextBox)
            Dim value = TryCast(card.Controls.OfType(Of TextBox)().FirstOrDefault(Function(control) control.Name.StartsWith("number", StringComparison.Ordinal)), TextBox)
            If title IsNot Nothing Then title.SetBounds(18, 16, Math.Max(120, card.ClientSize.Width - 68), 22)
            If value IsNot Nothing Then value.SetBounds(18, 43, 120, 30)
            Dim divider = card.Controls("accountSummaryDivider")
            If divider IsNot Nothing Then divider.SetBounds(18, 80, Math.Max(1, card.ClientSize.Width - 36), 1)
            Dim footerLabel = card.Controls("accountSummaryFooter")
            If footerLabel IsNot Nothing Then footerLabel.SetBounds(18, 88, Math.Max(120, card.ClientSize.Width - 36), 18)
            Dim icon = card.Controls("accountSummaryIcon")
            If icon IsNot Nothing Then
                icon.SetBounds(card.ClientSize.Width - 50, 14, 32, 32)
                ApplyDashboardRoundedCorners(icon, 8)
            End If
        Next

        accountListCard.SetBounds(horizontalPadding,
                                  listTop,
                                  availableWidth,
                                  Math.Max(220, pnlAccounts.ClientSize.Height - listTop - horizontalPadding))
        ApplyDashboardRoundedCorners(accountListCard, 14)
        numberTotalUsersRegistered.SetBounds(18, 14, Math.Max(150, availableWidth - 36), 20)
        accountSearchBox.SetBounds(18, 44, 240, 32)
        accountRoleFilter.SetBounds(270, 44, 142, 32)
        btnAddAccount.SetBounds(accountListCard.ClientSize.Width - btnAddAccount.Width - 18, 43, btnAddAccount.Width, btnAddAccount.Height)
        ApplyDashboardRoundedCorners(btnAddAccount, 8)
        accountsDataGrid.SetBounds(18,
                                   90,
                                   Math.Max(1, accountListCard.ClientSize.Width - 36),
                                   Math.Max(1, accountListCard.ClientSize.Height - 108))
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        LayoutReports(pnlReports, EventArgs.Empty)
        ShowPrimaryView(pnlReports, "Reports")
    End Sub

    Private Sub LayoutReports(sender As Object, e As EventArgs)
        If pnlReports.ClientSize.Width <= 0 OrElse pnlReports.ClientSize.Height <= 0 Then Return

        Dim horizontalPadding = 34
        Dim rowGap = 12
        ' The Admin report cards are 136px tall with a 14px bottom gap.
        ' Give each table row that full card height plus its visual gap.
        Dim cardHeight = 150
        Dim availableWidth = Math.Max(1, pnlReports.ClientSize.Width - horizontalPadding * 2)
        Panel16.SetBounds(horizontalPadding, pnlHeaderReports.Height + rowGap, availableWidth, 64)
        Panel16.BackColor = Color.White
        Panel16.BorderStyle = BorderStyle.None
        titleBusinessReports.SetBounds(18, 10, Math.Max(180, availableWidth - 36), 25)
        SetReportFont(titleBusinessReports, 14.0F, FontStyle.Bold)
        titleBusinessReports.ForeColor = Color.FromArgb(35, 45, 40)
        reportsDescription.SetBounds(18, 37, Math.Max(240, availableWidth - 36), 18)
        SetReportFont(reportsDescription, 9.0F, FontStyle.Regular)
        reportsDescription.ForeColor = Color.Gray
        Dim top = Panel16.Bottom + rowGap

        For Each cardLayout As TableLayoutPanel In {TableLayoutPanel8, TableLayoutPanel6, TableLayoutPanel7, TableLayoutPanel9}
            cardLayout.ColumnCount = 2
            cardLayout.ColumnStyles.Clear()
            cardLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
            cardLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
            cardLayout.RowCount = 1
            cardLayout.RowStyles.Clear()
            cardLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
            cardLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize
            cardLayout.Margin = Padding.Empty
            cardLayout.Padding = Padding.Empty
            cardLayout.SetBounds(horizontalPadding, top, availableWidth, cardHeight)
            top += cardHeight + rowGap
        Next

        For Each card As Panel In {Panel20, Panel21, Panel17, Panel18, Panel19, Panel22, Panel23}
            card.Dock = DockStyle.Fill
            card.Margin = New Padding(0, 0, 14, 14)
            card.BackColor = Color.White
            card.BorderStyle = BorderStyle.None
        Next

        LayoutReportCard(Panel20, inventoryTitle, inventoryDescriptions, inventoryViewReport)
        LayoutReportCard(Panel21, retailTitle, retailDescriptions, retailViewReport)
        LayoutReportCard(Panel17, wholesaleTitle, wholesaleDescriptions, wholesaleViewReport)
        LayoutReportCard(Panel18, purchasesTitle, purchasesDescriptions, purchasesViewReport)
        LayoutReportCard(Panel19, expensesTitle, expensesDescriptions, expensesViewReport)
        LayoutReportCard(Panel22, financialTitle, financialDescriptions, financialViewReport)
        LayoutReportCard(Panel23, employeeTitle, employeeDescription, employeeViewReport)
        ApplyDashboardRoundedCorners(Panel16, 12)
        For Each card In {Panel20, Panel21, Panel17, Panel18, Panel19, Panel22, Panel23}
            ApplyDashboardRoundedCorners(card, 12)
        Next
        For Each reportButton In {inventoryViewReport, retailViewReport, wholesaleViewReport, purchasesViewReport, expensesViewReport, financialViewReport, employeeViewReport}
            ApplyDashboardRoundedCorners(reportButton, 8)
        Next

        pnlReports.AutoScrollMinSize = New Size(0, top + horizontalPadding)
    End Sub

    Private Sub LayoutReportCard(card As Panel, title As TextBox, description As TextBox, viewButton As Button)
        If card.ClientSize.Width <= 0 OrElse card.ClientSize.Height <= 0 Then Return

        Dim inset = 16
        Dim textWidth = Math.Max(1, card.ClientSize.Width - inset * 2)
        title.SetBounds(inset, 14, textWidth, 24)
        SetReportFont(title, 11.0F, FontStyle.Bold)
        title.ForeColor = Color.FromArgb(35, 45, 40)
        description.SetBounds(inset, 44, textWidth, 36)
        SetReportFont(description, 9.0F, FontStyle.Regular)
        description.ForeColor = Color.Gray
        viewButton.SetBounds(inset, 88, Math.Min(132, textWidth), 34)
        SetReportFont(viewButton, 9.0F, FontStyle.Bold)
        viewButton.BackColor = Color.FromArgb(25, 120, 75)
        viewButton.ForeColor = Color.White
        viewButton.FlatStyle = FlatStyle.Flat
        viewButton.FlatAppearance.BorderSize = 0
    End Sub

    Private Sub SetReportFont(control As Control, size As Single, style As FontStyle)
        If control.Font IsNot Nothing AndAlso
           String.Equals(control.Font.Name, "Segoe UI", StringComparison.OrdinalIgnoreCase) AndAlso
           Math.Abs(control.Font.Size - size) < 0.1F AndAlso
           control.Font.Style = style Then Return

        control.Font = New Font("Segoe UI", size, style)
    End Sub

    Private Sub LayoutAllReportDetails()
        LayoutReportPage(pnlInventoryStockReport, Panel25, panelnvenotryStockReport, inventoryDataGrid, Label9, Label8, TextBox50, Label7)
        LayoutReportPage(pnlFinancialSalesReport, Panel31, panelFinancialSalesReport, DataGridView4, Label18, Label17)
        LayoutReportPage(pnlRetailSalesReport, Panel24, panelRetailSalesReport, retailDataGrid, Label12, Label11, TextBox48, Label10)
        LayoutReportPage(pnlExpensesReport, Panel35, panelExpenseReport, expenseDataGrid, Label26, Label25, TextBox66, Label27)
        LayoutReportPage(pnlPurchasesReport, Panel33, panelPurchasesReport, purchasesDataGrid, Label21, Label20, TextBox61, Label22)
        LayoutReportPage(pnlWholesaleSalesReport, Panel29, panelWholesale, wholesaleDataGrid, Label15, Label14, TextBox25, Label32)
        LayoutReportPage(pnlEmployeesalesReport, Panel37, panelEmployeeSalesReport, employeDataGrid, Label30, Label29, TextBox68, Label31, panelEmployeeDataGrid)
        TextBox64.Visible = False
        Label23.Visible = False
        TextBox65.Visible = False
        Label24.Visible = False
        TextBox60.Visible = False
        Label19.Visible = False
        TextBox69.Visible = False
        Label28.Visible = False
        Button18.Visible = False
    End Sub

    Private Sub LayoutReportPage(reportPage As Panel,
                                 pageHeader As Panel,
                                 toolbar As Panel,
                                 grid As DataGridView,
                                 headerTitle As Label,
                                 headerStore As Label,
                                 Optional headerUserName As TextBox = Nothing,
                                 Optional headerRole As Label = Nothing,
                                 Optional gridContainer As Panel = Nothing)
        If reportPage.ClientSize.Width <= 0 OrElse reportPage.ClientSize.Height <= 0 Then Return

        Dim horizontalPadding = 14
        Dim isSalesReport = reportPage Is pnlRetailSalesReport
        Dim contentWidth = Math.Max(1, reportPage.ClientSize.Width - horizontalPadding * 2)

        If isSalesReport Then
            pageHeader.Visible = True
            pageHeader.Dock = DockStyle.Fill
            headerTitle.AutoSize = False
            headerTitle.Text = "Reports"
            headerTitle.SetBounds(24, 27, Math.Max(1, reportPage.ClientSize.Width - 380), 36)
            headerTitle.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
            headerTitle.ForeColor = Color.FromArgb(20, 100, 60)
            headerStore.AutoSize = False
            headerStore.Text = "Sales and inventory reports"
            headerStore.SetBounds(24, 65, Math.Max(1, reportPage.ClientSize.Width - 380), 18)
            headerStore.Font = New Font("Segoe UI", 9.0F)
            headerStore.ForeColor = Color.Gray

            If headerUserName IsNot Nothing Then
                headerUserName.Text = CurrentFullName
            End If
            If headerRole IsNot Nothing Then
                headerRole.Text = "Super Admin / Owner"
            End If

            LayoutHeaderUser(headerUserName, headerRole, reportPage.ClientSize.Width, pageHeader)
            If headerUserName IsNot Nothing Then headerUserName.BringToFront()
            If headerRole IsNot Nothing Then headerRole.BringToFront()

            If salesReportPageLayout IsNot Nothing Then salesReportPageLayout.Dock = DockStyle.Fill
            toolbar.Dock = DockStyle.Fill
            StyleSuperAdminReportToolbar(toolbar)
            LayoutReportToolbar(reportPage, toolbar)
            salesReportFilterPanel.Dock = DockStyle.Fill
            salesReportDetailsPanel.Dock = DockStyle.Fill
            salesReportFilterPanel.BackColor = Color.White
            salesReportDetailsPanel.BackColor = Color.White
            salesReportDetailsPanel.Padding = New Padding(8)
            salesReportFilterPanel.Margin = New Padding(0, 0, 12, 0)
            salesReportDetailsPanel.Margin = Padding.Empty
            If Not String.Equals(Convert.ToString(salesReportFilterPanel.Tag), "ReportCard", StringComparison.Ordinal) Then
                AddHandler salesReportFilterPanel.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(DirectCast(sender, Control), 16)
                salesReportFilterPanel.Tag = "ReportCard"
            End If
            If Not String.Equals(Convert.ToString(salesReportDetailsPanel.Tag), "ReportCard", StringComparison.Ordinal) Then
                AddHandler salesReportDetailsPanel.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(DirectCast(sender, Control), 16)
                salesReportDetailsPanel.Tag = "ReportCard"
            End If
            ApplyDashboardRoundedCorners(salesReportFilterPanel, 16)
            ApplyDashboardRoundedCorners(salesReportDetailsPanel, 16)
            salesDateFilterLabel.SetBounds(14, 14, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 24)
            salesDateCaption.SetBounds(14, 60, 80, 18)
            salesTransactionDate.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            salesMonthCaption.SetBounds(14, 60, 80, 18)
            salesReportMonthCombo.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            salesYearCaption.SetBounds(14, If(String.Equals(Convert.ToString(ComboBox4.SelectedItem), "Monthly", StringComparison.Ordinal), 145, 60), 80, 18)
            salesReportYearCombo.SetBounds(14, If(String.Equals(Convert.ToString(ComboBox4.SelectedItem), "Monthly", StringComparison.Ordinal), 167, 82), Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            transactionDetailsTitle.Dock = DockStyle.Top
            transactionDetailsTitle.Height = 42
            transactionDetailsTitle.Padding = New Padding(14, 8, 0, 0)
            retailDataGrid.Dock = DockStyle.Fill
            StyleSuperAdminReportGrid(retailDataGrid)
            salesReportTotalLabel.Dock = DockStyle.Bottom
            salesReportTotalLabel.Height = 44
            Return
        End If

        Dim headerHeight = 100
        Dim toolbarTop = headerHeight + 26
        Dim toolbarHeight = 178
        Dim gridTop = toolbarTop + toolbarHeight + 12

        pageHeader.SetBounds(0, 0, reportPage.ClientSize.Width, headerHeight)
        toolbar.SetBounds(horizontalPadding, toolbarTop, contentWidth, toolbarHeight)
        LayoutReportHeader(pageHeader, headerTitle, headerStore, headerUserName, headerRole)
        StyleSuperAdminReportToolbar(toolbar)
        LayoutReportToolbar(reportPage, toolbar)

        If gridContainer Is Nothing Then
            Dim card = GetReportGridCard(reportPage, grid)
            card.SetBounds(horizontalPadding,
                           gridTop,
                           contentWidth,
                           Math.Max(1, reportPage.ClientSize.Height - gridTop - horizontalPadding))
            ApplyDashboardRoundedCorners(card, 14)
        Else
            gridContainer.SetBounds(horizontalPadding,
                                     gridTop,
                                     contentWidth,
                                     Math.Max(1, reportPage.ClientSize.Height - gridTop - horizontalPadding))
            gridContainer.BackColor = Color.White
            gridContainer.BorderStyle = BorderStyle.None
            ApplyDashboardRoundedCorners(gridContainer, 14)
            employeeName.SetBounds(14, 14, Math.Max(1, gridContainer.ClientSize.Width - 28), 26)
            grid.SetBounds(14,
                           employeeName.Bottom + 10,
                           Math.Max(1, gridContainer.ClientSize.Width - 28),
                           Math.Max(1, gridContainer.ClientSize.Height - employeeName.Bottom - 24))
        End If
        StyleSuperAdminReportGrid(grid)
        pageHeader.BringToFront()
        toolbar.BringToFront()
    End Sub

    Private Function GetReportGridCard(reportPage As Panel, grid As DataGridView) As Panel
        Dim card As Panel = Nothing
        If reportGridCards.TryGetValue(grid, card) Then Return card
        If grid.Parent IsNot Nothing Then grid.Parent.Controls.Remove(grid)
        card = New Panel With {.Name = grid.Name & "Card", .BackColor = Color.White, .BorderStyle = BorderStyle.None, .Padding = New Padding(8)}
        AddHandler card.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(DirectCast(sender, Control), 14)
        reportPage.Controls.Add(card)
        card.Controls.Add(grid)
        grid.Dock = DockStyle.Fill
        reportGridCards(grid) = card
        Return card
    End Function

    Private Sub StyleSuperAdminReportToolbar(toolbar As Panel)
        toolbar.BackColor = Color.FromArgb(235, 249, 242)
        If reportStyledToolbars.Add(toolbar) Then
            AddHandler toolbar.Paint,
                Sub(sender, e)
                    Dim panel = DirectCast(sender, Panel)
                    Using brush As New Drawing.Drawing2D.LinearGradientBrush(panel.ClientRectangle, Color.White, Color.FromArgb(214, 246, 230), 0.0F)
                        e.Graphics.FillRectangle(brush, panel.ClientRectangle)
                    End Using
                End Sub
            AddHandler toolbar.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(DirectCast(sender, Control), 16)
        End If

        Dim backButton As Button
        If toolbar Is panelRetailSalesReport Then
            backButton = salesBackToReports
            backButton.Text = ""
            backButton.AccessibleName = "Back to Reports"
            RemoveHandler backButton.Paint, AddressOf SuperAdminReportBackButton_Paint
            AddHandler backButton.Paint, AddressOf SuperAdminReportBackButton_Paint
        Else
            backButton = TryCast(toolbar.Controls("reportBackButton"), Button)
            If backButton Is Nothing Then
                backButton = New Button With {.Name = "reportBackButton", .Text = "", .AccessibleName = "Back to Reports", .Size = New Size(42, 40), .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .UseVisualStyleBackColor = False}
                AddHandler backButton.Click, Sub(sender, e) ShowPrimaryView(pnlReports, "Reports")
                AddHandler backButton.Paint, AddressOf SuperAdminReportBackButton_Paint
                AddHandler backButton.Resize, Sub(sender, e) ApplyDashboardRoundedCorners(DirectCast(sender, Control), 8)
                toolbar.Controls.Add(backButton)
            End If
        End If
        backButton.SetBounds(18, 18, 42, 40)
        backButton.BackColor = Color.FromArgb(20, 100, 60)
        backButton.ForeColor = Color.White
        backButton.FlatStyle = FlatStyle.Flat
        backButton.FlatAppearance.BorderSize = 0
        backButton.UseVisualStyleBackColor = False
        ApplyDashboardRoundedCorners(backButton, 8)
        ApplyDashboardRoundedCorners(toolbar, 16)
    End Sub

    Private Sub SuperAdminReportBackButton_Paint(sender As Object, e As PaintEventArgs)
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return
        e.Graphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
        Dim cx = button.ClientSize.Width \ 2
        Dim cy = button.ClientSize.Height \ 2
        Using pen As New Pen(Color.White, 2.2F)
            pen.StartCap = Drawing.Drawing2D.LineCap.Round
            pen.EndCap = Drawing.Drawing2D.LineCap.Round
            e.Graphics.DrawLine(pen, cx + 7, cy, cx - 7, cy)
            e.Graphics.DrawLine(pen, cx - 7, cy, cx + 1, cy - 6)
            e.Graphics.DrawLine(pen, cx - 7, cy, cx + 1, cy + 6)
        End Using
    End Sub

    Private Sub StyleSuperAdminReportGrid(grid As DataGridView)
        If grid Is Nothing Then Return
        grid.BackgroundColor = Color.White
        grid.BorderStyle = BorderStyle.None
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        grid.GridColor = Color.FromArgb(225, 235, 230)
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.ColumnHeadersHeight = 42
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 235)
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(25, 55, 45)
        grid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(35, 45, 40)
        grid.DefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
        grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(25, 45, 35)
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 249)
        grid.RowTemplate.Height = 40
        grid.RowTemplate.MinimumHeight = 40
    End Sub

    Private Sub LayoutReportHeader(pageHeader As Panel,
                                   headerTitle As Label,
                                   headerStore As Label,
                                   headerUserName As TextBox,
                                   headerRole As Label)
        Dim width = pageHeader.ClientSize.Width
        headerTitle.AutoSize = False
        headerTitle.SetBounds(24, 27, Math.Max(1, width - 380), 36)
        If headerTitle.Font.Size <> 24.0F OrElse headerTitle.Font.Style <> FontStyle.Bold Then
            headerTitle.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
        End If
        headerTitle.ForeColor = Color.FromArgb(20, 100, 60)
        headerStore.AutoSize = False
        headerStore.SetBounds(24, 65, Math.Max(1, width - 380), 18)
        If headerStore.Font.Size <> 9.0F OrElse headerStore.Font.Style <> FontStyle.Regular Then
            headerStore.Font = New Font("Segoe UI", 9.0F)
        End If
        headerStore.ForeColor = Color.Gray

        If headerUserName Is Nothing Then
            headerUserName = TryCast(pageHeader.Controls("reportHeaderUserName"), TextBox)
            If headerUserName Is Nothing Then
                headerUserName = New TextBox With {
                    .Name = "reportHeaderUserName",
                    .ReadOnly = True,
                    .BorderStyle = BorderStyle.None,
                    .BackColor = Color.White,
                    .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                    .TextAlign = HorizontalAlignment.Right
                }
                pageHeader.Controls.Add(headerUserName)
            End If
        End If

        If headerRole Is Nothing Then
            headerRole = TryCast(pageHeader.Controls("reportHeaderRole"), Label)
            If headerRole Is Nothing Then
                headerRole = New Label With {
                    .Name = "reportHeaderRole",
                    .AutoSize = False,
                    .Font = New Font("Segoe UI", 7.0F),
                    .ForeColor = Color.SeaGreen,
                    .Text = "Super Admin / Owner",
                    .TextAlign = ContentAlignment.MiddleRight
                }
                pageHeader.Controls.Add(headerRole)
            End If
        End If

        headerUserName.Text = CurrentFullName
        LayoutHeaderUser(headerUserName, headerRole, width, pageHeader)
    End Sub

    Private Sub LayoutReportToolbar(reportPage As Panel, toolbar As Panel)
        Dim inset = 18
        Dim buttonWidth = 150
        Dim right = toolbar.ClientSize.Width - inset
        Dim printButton = TryCast(toolbar.Controls("inventoryPrintReport"), Button)
        If printButton Is Nothing Then printButton = TryCast(toolbar.Controls("financialPrintReport"), Button)
        If printButton Is Nothing Then printButton = TryCast(toolbar.Controls("retailPrintReport"), Button)
        If printButton Is Nothing Then printButton = TryCast(toolbar.Controls("wholesalePrintReport"), Button)
        If printButton Is Nothing Then printButton = TryCast(toolbar.Controls("employeePrintReport"), Button)

        If printButton IsNot Nothing Then
            printButton.Text = "▣  Print Report"
            printButton.BackColor = Color.FromArgb(20, 100, 60)
            printButton.ForeColor = Color.White
            printButton.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
            printButton.FlatStyle = FlatStyle.Flat
            printButton.FlatAppearance.BorderSize = 0
            printButton.UseVisualStyleBackColor = False
            printButton.SetBounds(right - buttonWidth, 18, buttonWidth, 40)
            ApplyDashboardRoundedCorners(printButton, 8)
        End If

        If toolbar Is panelRetailSalesReport Then
            titleRetailSalesReport.Font = New Font("Segoe UI", 17.0F, FontStyle.Bold)
            titleRetailSalesReport.BorderStyle = BorderStyle.None
            titleRetailSalesReport.BackColor = Color.FromArgb(239, 250, 244)
            titleRetailSalesReport.ForeColor = Color.FromArgb(20, 30, 55)
            Dim retailTitleWidth = TextRenderer.MeasureText(titleRetailSalesReport.Text, titleRetailSalesReport.Font).Width + 12
            titleRetailSalesReport.SetBounds(82, 18, retailTitleWidth, 26)
            retailGenerated.Font = New Font("Segoe UI", 8.5F)
            retailGenerated.BorderStyle = BorderStyle.None
            retailGenerated.BackColor = Color.FromArgb(239, 250, 244)
            retailGenerated.ForeColor = Color.FromArgb(90, 105, 125)
            Dim retailGeneratedWidth = TextRenderer.MeasureText(retailGenerated.Text, retailGenerated.Font).Width + 12
            retailGenerated.SetBounds(82, 51, Math.Min(retailGeneratedWidth, Math.Max(1, toolbar.ClientSize.Width - 260)), 18)
            retailGenerated.Visible = True
            retailPrintReport.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
            retailPrintReport.Text = "▣  Print Report"
            retailPrintReport.BackColor = Color.FromArgb(20, 100, 60)
            retailPrintReport.ForeColor = Color.White
            retailPrintReport.FlatStyle = FlatStyle.Flat
            retailPrintReport.FlatAppearance.BorderSize = 0
            retailPrintReport.UseVisualStyleBackColor = False
            retailPrintReport.Cursor = Cursors.Hand
            retailPrintReport.SetBounds(right - 150, 18, 150, 40)
            ApplyDashboardRoundedCorners(retailPrintReport, 8)
            salesBackToReports.Font = New Font("Segoe UI", 9.0F)
            salesBackToReports.SetBounds(18, 18, 42, 40)
            ComboBox4.Visible = False
            salesTransactionFilter.Visible = False
            LayoutSalesReportFilterButtons(toolbar)
            Return
        End If

        Dim generated = toolbar.Controls.OfType(Of TextBox)().FirstOrDefault(Function(textBox) textBox.Name.ToLowerInvariant().Contains("generated"))
        If toolbar Is panelEmployeeSalesReport Then generated = TextBox67
        If generated IsNot Nothing Then
            generated.BorderStyle = BorderStyle.None
            generated.BackColor = Color.FromArgb(239, 250, 244)
            generated.ForeColor = Color.FromArgb(90, 105, 125)
            generated.Font = New Font("Segoe UI", 9.0F)
            Dim generatedWidth = TextRenderer.MeasureText(generated.Text, generated.Font).Width + 12
            generated.SetBounds(82, 51, Math.Min(generatedWidth, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - 110)), 20)
        End If

        Dim toolbarTitle = toolbar.Controls.OfType(Of TextBox)().FirstOrDefault(Function(textBox) textBox.Name.StartsWith("title", StringComparison.OrdinalIgnoreCase))
        If toolbarTitle IsNot Nothing Then
            toolbarTitle.Font = New Font("Segoe UI", 17.0F, FontStyle.Bold)
            toolbarTitle.ForeColor = Color.FromArgb(20, 30, 55)
            toolbarTitle.BorderStyle = BorderStyle.None
            toolbarTitle.BackColor = Color.FromArgb(239, 250, 244)
            Dim titleWidth = TextRenderer.MeasureText(toolbarTitle.Text, toolbarTitle.Font).Width + 12
            toolbarTitle.SetBounds(82, 18, Math.Min(titleWidth, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - 110)), 28)
        End If

        Dim selector = toolbar.Controls.OfType(Of ComboBox)().FirstOrDefault()
        If selector IsNot Nothing AndAlso selector.Visible Then
            selector.SetBounds(right - selector.Width, 88, selector.Width, selector.Height)
            selector.Font = New Font("Segoe UI", 10.0F)
            If generated IsNot Nothing Then
                generated.Width = Math.Min(TextRenderer.MeasureText(generated.Text, generated.Font).Width + 12,
                                           Math.Max(1, selector.Left - generated.Left - inset))
            End If
        End If

        If toolbar Is panelEmployeeSalesReport Then
            employeeNamesDropdown.SetBounds(right - employeeNamesDropdown.Width, 88, employeeNamesDropdown.Width, employeeNamesDropdown.Height)
            employeeScheduleDropdown.SetBounds(76, 88, employeeScheduleDropdown.Width, employeeScheduleDropdown.Height)
        ElseIf toolbar Is panelExpenseReport Then
            titleExpensesReport.SetBounds(82, 18, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - 110), 28)
            Dim periodWidth = expensesPeriodDropdown.Width
            Dim dateWidth = expensesDateTime.Width
            expensesPeriodDropdown.SetBounds(right - periodWidth, 135, periodWidth, expensesPeriodDropdown.Height)
            expensesDateTime.SetBounds(right - periodWidth - 10 - dateWidth, 135, dateWidth, expensesDateTime.Height)
            expensesSearchBar.SetBounds(76, 88, Math.Max(1, expensesDateTime.Left - 88), 32)
            btnAddExpenses.SetBounds(right - buttonWidth, 18, buttonWidth, 40)
            btnAddExpenses.BackColor = Color.FromArgb(20, 100, 60)
            btnAddExpenses.ForeColor = Color.White
            btnAddExpenses.FlatStyle = FlatStyle.Flat
            btnAddExpenses.FlatAppearance.BorderSize = 0
            btnAddExpenses.UseVisualStyleBackColor = False
            ApplyDashboardRoundedCorners(btnAddExpenses, 8)
            ComboBox6.Visible = False
            Button15.Visible = False
        ElseIf toolbar Is panelPurchasesReport Then
            titlePurchasesReport.SetBounds(82, 18, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - 110), 28)
            Dim periodWidth = ComboBox5.Width
            Dim dateWidth = purchasesDateTime.Width
            ComboBox5.SetBounds(right - periodWidth, 135, periodWidth, ComboBox5.Height)
            purchasesDateTime.SetBounds(right - periodWidth - 10 - dateWidth, 135, dateWidth, purchasesDateTime.Height)
            purchasesSearchBar.SetBounds(76, 88, Math.Max(1, purchasesDateTime.Left - 88), 32)
            Button14.Visible = False
        End If
    End Sub

    Private Async Sub financialViewReport_Click(sender As Object, e As EventArgs) Handles financialViewReport.Click
        Await ShowSalesReport(pnlFinancialSalesReport, DataGridView4, Nothing,
                        "financialTransactionID", "financialDate", "financialCustomer", "financialCashier", "financialType", "financialTotal", "financialStatus", financialGenerated)
    End Sub

    Private Async Sub retailViewReport_Click(sender As Object, e As EventArgs) Handles retailViewReport.Click
        Await ShowSalesReport(pnlRetailSalesReport, retailDataGrid, Nothing,
                        "retailTransactionID", "retailDate", "retailCustomer", "retailCashier", "retailType", "retailTotal", "retailStatus", retailGenerated, True)
    End Sub

    Private Async Sub wholesaleViewReport_Click(sender As Object, e As EventArgs) Handles wholesaleViewReport.Click
        Await ShowSalesReport(pnlWholesaleSalesReport, wholesaleDataGrid, "Wholesale",
                        "DataGridViewTextBoxColumn2", "DataGridViewTextBoxColumn3", "DataGridViewTextBoxColumn4", "DataGridViewTextBoxColumn5", "DataGridViewTextBoxColumn6", "DataGridViewTextBoxColumn7", "DataGridViewTextBoxColumn8", employeeGenerated)
    End Sub

    Private Async Sub employeeViewReport_Click(sender As Object, e As EventArgs) Handles employeeViewReport.Click
        Await ShowSalesReport(pnlEmployeesalesReport, employeDataGrid, Nothing,
                        "employeeTransactionID", "employeeDate", "employeeCustomer", "employeeCashier", "employeeType", "employeeTotal", "employeeStatus", TextBox67)
    End Sub

    Private Async Function ShowSalesReport(reportPanel As Panel,
                                grid As DataGridView,
                                saleMode As String,
                                idColumn As String,
                                dateColumn As String,
                                customerColumn As String,
                                cashierColumn As String,
                                typeColumn As String,
                                totalColumn As String,
                                statusColumn As String,
                                generatedLabel As TextBox,
                                Optional useSalesReportFilters As Boolean = False) As Task
        Try
            Dim columnNames = {idColumn, dateColumn, customerColumn, cashierColumn, typeColumn, totalColumn, statusColumn}
            Dim propertyNames = {"TransactionID", "SaleDate", "Customer", "Cashier", "SaleType", "Total", "Status"}
            For index = 0 To columnNames.Length - 1
                grid.Columns(columnNames(index)).DataPropertyName = propertyNames(index)
            Next

            grid.AutoGenerateColumns = False
            grid.AllowUserToAddRows = False
            grid.ReadOnly = True
            grid.RowHeadersVisible = False
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            grid.MultiSelect = False
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            grid.BackgroundColor = Color.White
            grid.BorderStyle = BorderStyle.None
            grid.GridColor = Color.FromArgb(230, 234, 231)
            grid.EnableHeadersVisualStyles = False
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 246)
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray
            grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
            grid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
            grid.DefaultCellStyle.SelectionForeColor = Color.Black
            grid.RowTemplate.Height = 36
            Dim fromDate As DateTime? = Nothing
            Dim throughDateExclusive As DateTime? = Nothing
            If useSalesReportFilters Then
                GetSalesReportDateRange(fromDate, throughDateExclusive)
                saleMode = If(salesTransactionFilter.SelectedIndex <= 0, Nothing, CStr(salesTransactionFilter.SelectedItem))
            End If
            grid.DataSource = Await Task.Run(Function() New SalesRepository().GetTransactionReport(saleMode,
                                                                                                  fromDate:=fromDate,
                                                                                                  throughDateExclusive:=throughDateExclusive))
            grid.Columns(dateColumn).DefaultCellStyle.Format = "g"
            grid.Columns(totalColumn).DefaultCellStyle.Format = "₱#,##0.00"
            grid.Columns(idColumn).HeaderText = "Transaction ID"
            grid.Columns(dateColumn).HeaderText = "Sale Date"
            grid.Columns(customerColumn).HeaderText = "Customer"
            grid.Columns(cashierColumn).HeaderText = "Cashier"
            grid.Columns(typeColumn).HeaderText = "Sale Type"
            grid.Columns(totalColumn).HeaderText = "Total"
            grid.Columns(statusColumn).HeaderText = "Status"
            generatedLabel.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt") & " | By: " & CurrentFullName
            StyleSuperAdminReportGrid(grid)

            If useSalesReportFilters Then
                Dim startDate As DateTime
                Dim endDate As DateTime
                GetSalesReportDateRange(fromDate, throughDateExclusive)
                startDate = fromDate.Value
                endDate = throughDateExclusive.Value
                Dim periodCaption = If(ComboBox4.SelectedIndex = 0,
                                       startDate.ToString("MMMM dd, yyyy"),
                                       If(ComboBox4.SelectedIndex = 1,
                                          startDate.ToString("MMMM yyyy"),
                                          startDate.ToString("yyyy")))
                transactionDetailsTitle.Text = "Transaction Details — " & periodCaption
                Dim reportRows = TryCast(grid.DataSource, DataTable)
                Dim total As Decimal = 0D
                If reportRows IsNot Nothing AndAlso reportRows.Columns.Contains("Total") Then
                    total = reportRows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
                End If
                salesReportTotalLabel.Text = "TOTAL SALES: ₱" & total.ToString("N2")
                salesDateFilterLabel.Text = "Filter by " & CStr(ComboBox4.SelectedItem).ToLowerInvariant()
            End If

            ShowReportPanel(reportPanel)
        Catch ex As Exception
            MessageBox.Show("Could not load sales report. " & ex.Message, "Sales reports", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub InitializeSalesReportFilters()
        If salesFiltersInitialized Then Return
        salesFiltersInitialized = True

        ComboBox4.Items.Clear()
        ComboBox4.Items.AddRange(New Object() {"Daily", "Monthly", "Yearly"})
        ComboBox4.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox4.SelectedIndex = 0

        salesTransactionFilter = New ComboBox With {
            .Name = "salesTransactionFilter",
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Font = New Font("Segoe UI", 9.0F)
        }
        salesTransactionFilter.Items.AddRange(New Object() {"All", "Retail", "Wholesale"})
        salesTransactionFilter.SelectedIndex = 0
        salesTransactionDate = New DateTimePicker With {
            .Name = "salesTransactionDate",
            .Font = New Font("Segoe UI", 9.0F),
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "MMMM dd, yyyy",
            .Value = Date.Today
        }
        salesBackToReports = New Button With {
            .Name = "salesBackToReports",
            .Text = "Back to Reports",
            .BackColor = Color.FromArgb(20, 100, 60),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 9.0F)
        }
        salesBackToReports.FlatAppearance.BorderSize = 0
        transactionDetailsTitle = New Label With {
            .Name = "transactionDetailsTitle",
            .Text = "Transaction details",
            .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold),
            .AutoSize = True
        }
        salesDateCaption = New Label With {
            .Name = "salesDateCaption",
            .Text = "Date",
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Color.Gray,
            .AutoSize = True
        }
        salesMonthCaption = New Label With {
            .Name = "salesMonthCaption",
            .Text = "Month",
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Color.Gray,
            .AutoSize = True
        }
        salesYearCaption = New Label With {
            .Name = "salesYearCaption",
            .Text = "Year",
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Color.Gray,
            .AutoSize = True
        }
        salesDateFilterLabel = New Label With {
            .Name = "salesDateFilterLabel",
            .Text = "Filter by daily",
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(20, 100, 60),
            .AutoSize = True
        }
        salesReportFilterPanel = New Panel With {
            .Name = "salesReportFilterPanel",
            .BackColor = Color.White
        }
        salesReportDetailsPanel = New Panel With {
            .Name = "salesReportDetailsPanel",
            .BackColor = Color.White
        }
        salesReportTotalLabel = New Label With {
            .Name = "salesReportTotalLabel",
            .Text = "TOTAL SALES: ₱0.00",
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(20, 100, 60),
            .BackColor = Color.FromArgb(225, 245, 235),
            .TextAlign = ContentAlignment.MiddleRight,
            .Padding = New Padding(0, 0, 18, 0)
        }
        salesReportMonthCombo = New ComboBox With {.Name = "salesReportMonthCombo", .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F)}
        salesReportMonthCombo.Items.AddRange(New Object() {"September", "October"})
        salesReportMonthCombo.SelectedItem = "October"
        salesReportYearCombo = New ComboBox With {.Name = "salesReportYearCombo", .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F)}
        salesReportYearCombo.Items.Add("2026")
        salesReportYearCombo.SelectedItem = "2026"

        Dim firstAvailableDate As New DateTime(2026, 9, 1)
        Dim lastAvailableDate As New DateTime(2026, 10, 31)
        If Date.Today < lastAvailableDate Then lastAvailableDate = Date.Today
        salesTransactionDate.MinDate = firstAvailableDate
        salesTransactionDate.MaxDate = lastAvailableDate
        salesTransactionDate.Value = lastAvailableDate

        salesReportFilterPanel.Dock = DockStyle.Fill
        salesReportFilterPanel.Padding = New Padding(14)
        salesReportFilterPanel.Margin = New Padding(0, 0, 12, 0)
        salesReportDetailsPanel.Dock = DockStyle.Fill
        salesReportDetailsPanel.Padding = New Padding(8)
        salesReportDetailsPanel.Margin = Padding.Empty
        transactionDetailsTitle.Dock = DockStyle.Top
        transactionDetailsTitle.AutoSize = False
        transactionDetailsTitle.Height = 42
        transactionDetailsTitle.Padding = New Padding(14, 8, 0, 0)
        retailDataGrid.Dock = DockStyle.Fill
        retailDataGrid.RowTemplate.Height = 36
        salesReportTotalLabel.Dock = DockStyle.Bottom
        salesReportTotalLabel.Height = 44

        salesReportBody = New TableLayoutPanel With {
            .Name = "salesReportBody",
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .BackColor = Color.FromArgb(245, 245, 245),
            .Margin = New Padding(34, 0, 24, 14),
            .Padding = Padding.Empty
        }
        salesReportBody.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 270))
        salesReportBody.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        salesReportBody.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        salesReportPageLayout = New TableLayoutPanel With {
            .Name = "salesReportPageLayout",
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 5,
            .BackColor = Color.FromArgb(245, 245, 245),
            .Margin = Padding.Empty,
            .Padding = Padding.Empty
        }
        salesReportPageLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 100))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 26))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 178))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 12))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        ComboBox4.Visible = False
        salesTransactionFilter.Visible = False
        AddSalesReportFilterButtonGroup(panelRetailSalesReport, {"Daily", "Monthly", "Yearly"}, salesReportPeriodButtons, 76, 88, AddressOf SelectSalesReportPeriod, 100, 38)
        AddSalesReportFilterButtonGroup(panelRetailSalesReport, {"All", "Retail", "Wholesale"}, salesReportModeButtons, 76, 135, AddressOf SelectSalesReportMode, 100, 34)
        UpdateSalesReportFilterButtonStyles()
        panelRetailSalesReport.Controls.Add(salesBackToReports)
        salesReportFilterPanel.Controls.Add(salesDateFilterLabel)
        salesReportFilterPanel.Controls.Add(salesDateCaption)
        salesReportFilterPanel.Controls.Add(salesTransactionDate)
        salesReportFilterPanel.Controls.Add(salesMonthCaption)
        salesReportFilterPanel.Controls.Add(salesReportMonthCombo)
        salesReportFilterPanel.Controls.Add(salesYearCaption)
        salesReportFilterPanel.Controls.Add(salesReportYearCombo)
        salesReportDetailsPanel.Controls.Add(retailDataGrid)
        salesReportDetailsPanel.Controls.Add(salesReportTotalLabel)
        salesReportDetailsPanel.Controls.Add(transactionDetailsTitle)
        salesReportBody.Controls.Add(salesReportFilterPanel, 0, 0)
        salesReportBody.Controls.Add(salesReportDetailsPanel, 1, 0)

        Panel24.Dock = DockStyle.Fill
        Panel24.Margin = Padding.Empty
        panelRetailSalesReport.Dock = DockStyle.Fill
        panelRetailSalesReport.Margin = New Padding(34, 0, 24, 0)
        pnlRetailSalesReport.Controls.Clear()
        salesReportPageLayout.Controls.Add(Panel24, 0, 0)
        salesReportPageLayout.Controls.Add(panelRetailSalesReport, 0, 2)
        salesReportPageLayout.Controls.Add(salesReportBody, 0, 4)
        pnlRetailSalesReport.Controls.Add(salesReportPageLayout)

        AddHandler retailDataGrid.CellClick, AddressOf SalesReportGrid_CellClick
        AddHandler ComboBox4.SelectedIndexChanged, AddressOf SalesReportPeriod_Changed
        AddHandler salesTransactionFilter.SelectedIndexChanged, AddressOf SalesReportFilter_Changed
        AddHandler salesTransactionDate.ValueChanged, AddressOf SalesReportFilter_Changed
        AddHandler salesReportMonthCombo.SelectedIndexChanged, AddressOf SalesReportFilter_Changed
        AddHandler salesReportYearCombo.SelectedIndexChanged, AddressOf SalesReportFilter_Changed
        AddHandler salesBackToReports.Click, AddressOf SalesBackToReports_Click
        ConfigureSuperAdminSalesReportFilters()
    End Sub

    Private Sub AddSalesReportFilterButtonGroup(parent As Control,
                                                options As String(),
                                                buttons As Dictionary(Of String, Button),
                                                left As Integer,
                                                top As Integer,
                                                onSelect As Action(Of String),
                                                buttonWidth As Integer,
                                                buttonHeight As Integer)
        Dim x = left
        For Each optionText In options
            Dim button As New Button With {
                .Text = optionText,
                .Size = New Size(buttonWidth, buttonHeight),
                .Location = New Point(x, top),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .FlatStyle = FlatStyle.Flat,
                .Cursor = Cursors.Hand,
                .UseVisualStyleBackColor = False
            }
            button.FlatAppearance.BorderColor = Color.FromArgb(25, 120, 75)
            button.FlatAppearance.BorderSize = 1
            AddHandler button.Click, Sub(sender, e) onSelect(optionText)
            buttons(optionText) = button
            parent.Controls.Add(button)
            x += button.Width + 8
        Next
    End Sub

    Private Sub LayoutSalesReportFilterButtons(toolbar As Control)
        Dim periodX = 76
        For Each pair In salesReportPeriodButtons
            pair.Value.SetBounds(periodX, 88, 100, 38)
            periodX += pair.Value.Width + 8
        Next

        Dim modeX = 76
        For Each pair In salesReportModeButtons
            pair.Value.SetBounds(modeX, 135, 100, 34)
            modeX += pair.Value.Width + 8
        Next
    End Sub

    Private Sub SelectSalesReportPeriod(period As String)
        If String.Equals(Convert.ToString(ComboBox4.SelectedItem), period, StringComparison.OrdinalIgnoreCase) Then Return
        ComboBox4.SelectedItem = period
        UpdateSalesReportFilterButtonStyles()
    End Sub

    Private Sub SelectSalesReportMode(saleMode As String)
        If String.Equals(Convert.ToString(salesTransactionFilter.SelectedItem), saleMode, StringComparison.OrdinalIgnoreCase) Then Return
        salesTransactionFilter.SelectedItem = saleMode
        UpdateSalesReportFilterButtonStyles()
    End Sub

    Private Sub UpdateSalesReportFilterButtonStyles()
        Dim selectedPeriod = Convert.ToString(ComboBox4.SelectedItem)
        For Each pair In salesReportPeriodButtons
            Dim isSelected = String.Equals(pair.Key, selectedPeriod, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, Color.FromArgb(20, 100, 60), Color.White)
            pair.Value.ForeColor = If(isSelected, Color.White, Color.FromArgb(20, 100, 60))
        Next

        Dim selectedMode = Convert.ToString(salesTransactionFilter.SelectedItem)
        For Each pair In salesReportModeButtons
            Dim isSelected = String.Equals(pair.Key, selectedMode, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, Color.FromArgb(218, 240, 229), Color.White)
            pair.Value.ForeColor = Color.FromArgb(20, 100, 60)
        Next
    End Sub

    Private Sub GetSalesReportDateRange(ByRef fromDate As DateTime?, ByRef throughDateExclusive As DateTime?)
        Select Case CStr(ComboBox4.SelectedItem)
            Case "Monthly"
                Dim selectedYear = Integer.Parse(Convert.ToString(salesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                Dim selectedMonth = DateTime.ParseExact(Convert.ToString(salesReportMonthCombo.SelectedItem), "MMMM", CultureInfo.InvariantCulture).Month
                fromDate = New DateTime(selectedYear, selectedMonth, 1)
                throughDateExclusive = fromDate.Value.AddMonths(1)
            Case "Yearly"
                Dim selectedYear = Integer.Parse(Convert.ToString(salesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                fromDate = New DateTime(selectedYear, 1, 1)
                throughDateExclusive = fromDate.Value.AddYears(1)
            Case Else
                Dim selectedDate = salesTransactionDate.Value.Date
                fromDate = selectedDate
                throughDateExclusive = selectedDate.AddDays(1)
        End Select
    End Sub

    Private Sub ConfigureSuperAdminSalesReportFilters()
        If salesReportFilterPanel Is Nothing Then Return
        Dim period = Convert.ToString(ComboBox4.SelectedItem)
        salesDateFilterLabel.Text = "Filter by " & period.ToLowerInvariant()
        salesDateCaption.Visible = String.Equals(period, "Daily", StringComparison.Ordinal)
        salesTransactionDate.Visible = salesDateCaption.Visible
        salesMonthCaption.Visible = String.Equals(period, "Monthly", StringComparison.Ordinal)
        salesReportMonthCombo.Visible = salesMonthCaption.Visible
        salesYearCaption.Visible = Not String.Equals(period, "Daily", StringComparison.Ordinal)
        salesReportYearCombo.Visible = salesYearCaption.Visible
        salesMonthCaption.SetBounds(14, 60, 80, 18)
        salesReportMonthCombo.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
        If String.Equals(period, "Monthly", StringComparison.Ordinal) Then
            salesYearCaption.SetBounds(14, 145, 80, 18)
            salesReportYearCombo.SetBounds(14, 167, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
        Else
            salesYearCaption.SetBounds(14, 60, 80, 18)
            salesReportYearCombo.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
        End If
        UpdateSalesReportFilterButtonStyles()
    End Sub

    Private Async Sub SalesReportPeriod_Changed(sender As Object, e As EventArgs)
        Select Case Convert.ToString(ComboBox4.SelectedItem)
            Case "Daily"
                salesTransactionDate.Value = salesTransactionDate.MaxDate
            Case "Monthly"
                salesReportMonthCombo.SelectedItem = "October"
                salesReportYearCombo.SelectedItem = "2026"
            Case "Yearly"
                salesReportYearCombo.SelectedItem = "2026"
        End Select
        ConfigureSuperAdminSalesReportFilters()
        Await ReloadCurrentSalesReport()
    End Sub

    Private Async Sub SalesReportFilter_Changed(sender As Object, e As EventArgs)
        Await ReloadCurrentSalesReport()
    End Sub

    Private Async Function ReloadCurrentSalesReport() As Task
        If Not salesFiltersInitialized OrElse Not pnlRetailSalesReport.Visible OrElse retailDataGrid.DataSource Is Nothing Then Return
        Await ShowSalesReport(pnlRetailSalesReport, retailDataGrid, Nothing,
                              "retailTransactionID", "retailDate", "retailCustomer", "retailCashier", "retailType", "retailTotal", "retailStatus", retailGenerated, True)
    End Function

    Private Sub SalesBackToReports_Click(sender As Object, e As EventArgs)
        ShowPrimaryView(pnlReports, "Reports")
    End Sub

    Private Sub SalesReportGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse Not retailDataGrid.Columns.Contains("retailTransactionID") Then Return
        Dim value = retailDataGrid.Rows(e.RowIndex).Cells("retailTransactionID").Value
        If value Is Nothing OrElse value Is DBNull.Value Then Return
        ShowTransactionDetailsDialog(Convert.ToInt64(value, CultureInfo.InvariantCulture))
    End Sub

    Private Sub ShowTransactionDetailsDialog(saleId As Long)
        Try
            Dim details = New SalesRepository().GetSaleReceiptDetails(saleId)
            If details.Rows.Count = 0 Then
                MessageBox.Show("The transaction details could not be found.", "Transaction Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim sale = details.Rows(0)
            Dim saleDate = Convert.ToDateTime(sale("created_at"), CultureInfo.InvariantCulture)
            Dim receiptNumber = "RCPT-" & saleId.ToString("D8", CultureInfo.InvariantCulture)
            Dim receiptLines = BuildSavedReceiptLines(details, receiptNumber, saleId, saleDate)
            Using dialog As New Form()
                dialog.Text = "Transaction Details"
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.ClientSize = New Size(700, 620)
                dialog.BackColor = Color.White
                dialog.Font = New Font("Segoe UI", 9.0F)
                dialog.ShowInTaskbar = False

                Dim header As New Panel With {.Dock = DockStyle.Top, .Height = 82, .BackColor = Color.White}
                header.Controls.Add(New Label With {.Text = "Transaction Details", .AutoSize = True, .Font = New Font("Segoe UI", 16.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 100, 60), .Location = New Point(22, 12)})
                header.Controls.Add(New Label With {.Text = receiptNumber & "  |  " & saleDate.ToString("MMMM dd, yyyy  h:mm tt", CultureInfo.CurrentCulture), .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.Gray, .Location = New Point(24, 47)})

                Dim content As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = Color.White}
                Dim isWholesaleReceipt = String.Equals(DbText(sale, "sale_mode"), "Wholesale", StringComparison.OrdinalIgnoreCase)
                Dim salePanel As New Panel With {.Location = New Point(20, 12), .Size = New Size(646, If(isWholesaleReceipt, 340, 224)), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.White}
                salePanel.Controls.Add(New Label With {.Text = "SALE INFORMATION", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 100, 60), .Location = New Point(16, 13)})
                AddReceiptDetailField(salePanel, "Sale Type", DbText(sale, "sale_mode"), 16, 48)
                If isWholesaleReceipt Then
                    AddReceiptDetailField(salePanel, "Wholesale Client", DbText(sale, "wholesale_company_name"), 326, 48)
                    AddReceiptDetailField(salePanel, "FDA LTO No.", If(String.IsNullOrWhiteSpace(DbText(sale, "wholesale_customer_license")), DbText(sale, "customer_id_number"), DbText(sale, "wholesale_customer_license")), 16, 102)
                    AddReceiptDetailField(salePanel, "Contact Person", If(String.IsNullOrWhiteSpace(DbText(sale, "contact_person")), DbText(sale, "customer_name"), DbText(sale, "contact_person")), 326, 102)
                    AddReceiptDetailField(salePanel, "Contact No.", DbText(sale, "contact_number"), 16, 156)
                    AddReceiptDetailField(salePanel, "Shipping Address", DbText(sale, "customer_address"), 326, 156)
                    AddReceiptDetailField(salePanel, "Purchase Order No.", DbText(sale, "purchase_order_no"), 16, 210)
                    AddReceiptDetailField(salePanel, "Purchase Order Date", If(sale.IsNull("purchase_order_date"), String.Empty, Convert.ToDateTime(sale("purchase_order_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)), 326, 210)
                    Dim paymentDescription = If(String.Equals(DbText(sale, "payment_method"), "E-payment", StringComparison.OrdinalIgnoreCase), "E-payment · " & DbText(sale, "payment_provider"), If(String.IsNullOrWhiteSpace(DbText(sale, "payment_method")), "Cash", DbText(sale, "payment_method")))
                    AddReceiptDetailField(salePanel, "Mode of Payment", paymentDescription, 16, 264)
                    AddReceiptDetailField(salePanel, "Cashier Initials", ReceiptStaffInitials(DbText(sale, "cashier_initials")), 326, 264)
                Else
                    AddReceiptDetailField(salePanel, "Customer Type", DbText(sale, "customer_type"), 326, 48)
                    AddReceiptDetailField(salePanel, "Customer", If(String.IsNullOrWhiteSpace(DbText(sale, "customer_name")), "Walk-in", DbText(sale, "customer_name")), 16, 102)
                    If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_id_number")) Then AddReceiptDetailField(salePanel, "Customer ID", DbText(sale, "customer_id_number"), 326, 102)
                    AddReceiptDetailField(salePanel, "Cashier Initials", ReceiptStaffInitials(DbText(sale, "cashier_initials")), 16, 160)
                End If

                Dim itemHeadingTop = If(isWholesaleReceipt, 368, 252)
                Dim itemHeading As New Label With {.Text = "ITEMS", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 100, 60), .Location = New Point(22, itemHeadingTop)}
                Dim itemGrid As New DataGridView With {
                    .Location = New Point(20, itemHeadingTop + 30), .Size = New Size(646, 224), .ReadOnly = True,
                    .AllowUserToAddRows = False, .AllowUserToDeleteRows = False, .AllowUserToResizeRows = False,
                    .AutoGenerateColumns = False, .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect, .MultiSelect = False,
                    .RowHeadersVisible = False, .BackgroundColor = Color.White, .BorderStyle = BorderStyle.FixedSingle,
                    .GridColor = Color.FromArgb(230, 234, 231), .EnableHeadersVisualStyles = False
                }
                itemGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 239)
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

                Dim totalTop = itemHeadingTop + 274
                Dim hasPrescription = details.AsEnumerable().Any(Function(row) Not row.IsNull("is_prescription") AndAlso Convert.ToBoolean(row("is_prescription")))
                If hasPrescription Then
                    Dim prescriptionPanel As New Panel With {.Location = New Point(20, totalTop), .Size = New Size(646, 186), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.White}
                    prescriptionPanel.Controls.Add(New Label With {.Text = "PRESCRIPTION INFORMATION", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 100, 60), .Location = New Point(16, 12)})
                    AddReceiptDetailField(prescriptionPanel, "Prescription No.", DbText(sale, "prescription_no"), 16, 48)
                    AddReceiptDetailField(prescriptionPanel, "Prescribing Practitioner", DbText(sale, "prescribing_practioner"), 326, 48)
                    AddReceiptDetailField(prescriptionPanel, "PRC License No.", DbText(sale, "prc_liscense"), 16, 104)
                    AddReceiptDetailField(prescriptionPanel, "Prescription Date", If(sale.IsNull("prescription_date"), String.Empty, Convert.ToDateTime(sale("prescription_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)), 326, 104)
                    content.Controls.Add(prescriptionPanel)
                    totalTop += 202
                End If
                Dim totalPanel As New Panel With {.Location = New Point(20, totalTop), .Size = New Size(646, 188), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.FromArgb(248, 250, 248)}
                AddReceiptDetailField(totalPanel, "Subtotal", "₱" & Convert.ToDecimal(sale("subtotal")).ToString("N2", CultureInfo.CurrentCulture), 16, 18)
                AddReceiptDetailField(totalPanel, "Discount", "₱" & Convert.ToDecimal(sale("discount_amount")).ToString("N2", CultureInfo.CurrentCulture), 326, 18)
                AddReceiptDetailField(totalPanel, "Total Due", "₱" & Convert.ToDecimal(sale("total_amount")).ToString("N2", CultureInfo.CurrentCulture), 16, 78, True)
                AddReceiptDetailField(totalPanel, "Cash Received", "₱" & Convert.ToDecimal(sale("amount_received")).ToString("N2", CultureInfo.CurrentCulture), 326, 78)
                AddReceiptDetailField(totalPanel, "Change", "₱" & Convert.ToDecimal(sale("change_amount")).ToString("N2", CultureInfo.CurrentCulture), 16, 138)
                content.Controls.Add(salePanel)
                content.Controls.Add(itemHeading)
                content.Controls.Add(itemGrid)
                content.Controls.Add(totalPanel)

                Dim footer As New Panel With {.Dock = DockStyle.Bottom, .Height = 72, .BackColor = Color.White}
                Dim closeButton As New Button With {.Text = "Close", .Size = New Size(110, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = Color.White, .ForeColor = Color.FromArgb(20, 100, 60), .FlatStyle = FlatStyle.Flat, .DialogResult = DialogResult.OK}
                closeButton.Location = New Point(footer.ClientSize.Width - closeButton.Width - 12, 12)
                AddHandler footer.Resize, Sub(sender, e) closeButton.Left = footer.ClientSize.Width - closeButton.Width - 12
                Dim printButton As New Button With {.Text = "Print Receipt", .Size = New Size(150, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = Color.FromArgb(20, 100, 60), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
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
            "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES", "SALES RECEIPT", New String("-"c, 76),
            "Receipt No.: " & receiptNumber,
            "Transaction No.: " & saleId.ToString(CultureInfo.InvariantCulture),
            "Date and Time: " & saleDate.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
            "Sale Type: " & DbText(sale, "sale_mode"),
            "Cashier Initials: " & ReceiptStaffInitials(DbText(sale, "cashier_initials"))
        }
        If String.Equals(DbText(sale, "sale_mode"), "Wholesale", StringComparison.OrdinalIgnoreCase) Then
            If Not String.IsNullOrWhiteSpace(DbText(sale, "wholesale_company_name")) Then lines.Add("Company: " & DbText(sale, "wholesale_company_name"))
            Dim license = If(String.IsNullOrWhiteSpace(DbText(sale, "wholesale_customer_license")), DbText(sale, "customer_id_number"), DbText(sale, "wholesale_customer_license"))
            If Not String.IsNullOrWhiteSpace(license) Then lines.Add("FDA LTO No.: " & license)
            Dim contactPerson = If(String.IsNullOrWhiteSpace(DbText(sale, "contact_person")), DbText(sale, "customer_name"), DbText(sale, "contact_person"))
            If Not String.IsNullOrWhiteSpace(contactPerson) Then lines.Add("Contact Person: " & contactPerson)
            If Not String.IsNullOrWhiteSpace(DbText(sale, "contact_number")) Then lines.Add("Contact No.: " & DbText(sale, "contact_number"))
            If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_address")) Then lines.Add("Shipping Address: " & DbText(sale, "customer_address"))
            If Not String.IsNullOrWhiteSpace(DbText(sale, "purchase_order_no")) Then lines.Add("Purchase Order No.: " & DbText(sale, "purchase_order_no"))
            If Not sale.IsNull("purchase_order_date") Then lines.Add("Purchase Order Date: " & Convert.ToDateTime(sale("purchase_order_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
            Dim paymentDescription = If(String.Equals(DbText(sale, "payment_method"), "E-payment", StringComparison.OrdinalIgnoreCase),
                                        "E-payment" & If(String.IsNullOrWhiteSpace(DbText(sale, "payment_provider")), String.Empty, " - " & DbText(sale, "payment_provider")),
                                        If(String.IsNullOrWhiteSpace(DbText(sale, "payment_method")), "Cash", DbText(sale, "payment_method")))
            lines.Add("Mode of Payment: " & paymentDescription)
            If Not String.IsNullOrWhiteSpace(DbText(sale, "payment_account_name")) Then lines.Add("Account Name: " & DbText(sale, "payment_account_name"))
            If Not String.IsNullOrWhiteSpace(DbText(sale, "payment_reference_number")) Then lines.Add("Payment Reference No.: " & DbText(sale, "payment_reference_number"))
        Else
            lines.Add("Customer Type: " & DbText(sale, "customer_type"))
            If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_name")) Then lines.Add("Customer Name: " & DbText(sale, "customer_name"))
            If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_id_number")) Then lines.Add("Customer ID: " & DbText(sale, "customer_id_number"))
        End If
        lines.Add(New String("-"c, 76))
        lines.Add("ITEMS")
        For Each row As DataRow In details.Rows
            If row.IsNull("sale_item_id") Then Continue For
            Dim productName = DbText(row, "brand_name")
            If Not String.IsNullOrWhiteSpace(DbText(row, "generic_name")) Then productName &= " - " & DbText(row, "generic_name")
            If Not String.IsNullOrWhiteSpace(DbText(row, "dosage")) Then productName &= " - " & DbText(row, "dosage")
            lines.Add(productName)
            lines.Add("  " & DbText(row, "quantity") & " " & DbText(row, "unit") & " x PHP " &
                      Convert.ToDecimal(row("unit_price")).ToString("N2", CultureInfo.InvariantCulture) & " = PHP " &
                      Convert.ToDecimal(row("line_total")).ToString("N2", CultureInfo.InvariantCulture))
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
        panel.Controls.Add(New Label With {.Text = caption, .AutoSize = True, .MaximumSize = New Size(fieldWidth, 0), .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(x, y)})
        panel.Controls.Add(New Label With {.Text = value, .AutoSize = True, .MaximumSize = New Size(fieldWidth, 0), .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold), .ForeColor = If(emphasized, Color.FromArgb(20, 100, 60), Color.FromArgb(35, 45, 40)), .Location = New Point(x, y + 22)})
    End Sub

    Private Function DbText(row As DataRow, columnName As String) As String
        If row Is Nothing OrElse Not row.Table.Columns.Contains(columnName) OrElse row.IsNull(columnName) Then Return String.Empty
        Return Convert.ToString(row(columnName), CultureInfo.CurrentCulture)
    End Function

    Private Async Sub inventoryViewReport_Click(sender As Object, e As EventArgs) Handles inventoryViewReport.Click
        Try
            Dim products = Await Task.Run(Function() New ProductManager().GetProducts(includeInactive:=True))
            If products Is Nothing Then products = New DataTable()
            If Not products.Columns.Contains("Status") Then products.Columns.Add("Status", GetType(String))

            For Each row As DataRow In products.Rows
                Dim stock = If(row.IsNull("Stock Quantity"), 0, Convert.ToInt32(row("Stock Quantity")))
                Dim reorderPoint = If(row.IsNull("Reorder Point"), 0, Convert.ToInt32(row("Reorder Point")))
                Dim status = If(stock <= reorderPoint, "Low Stock", "In Stock")
                If Not row.IsNull("Expiry Date") Then
                    Dim expiry = Convert.ToDateTime(row("Expiry Date")).Date
                    If expiry < Date.Today Then
                        status = "Expired"
                    ElseIf expiry <= Date.Today.AddMonths(6) Then
                        status = "Near Expiry"
                    End If
                End If
                row("Status") = status
            Next

            inventoryDataGrid.AutoGenerateColumns = False
            inventoryDataGrid.AllowUserToAddRows = False
            inventoryDataGrid.ReadOnly = True
            inventoryDataGrid.RowHeadersVisible = False
            inventoryDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            inventoryDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            SetInventoryColumn("DataGridViewTextBoxColumn1", "Brand Name")
            SetInventoryColumn("inventoryCategory", "Category")
            SetInventoryColumn("inventoryUnit", "Unit")
            SetInventoryColumn("inventoryStock", "Stock Quantity")
            SetInventoryColumn("inventoryPackage", "Qty/Box")
            SetInventoryColumn("inventoryBatch", "CPR Number")
            SetInventoryColumn("inventoryExpiry", "Expiry Date")
            SetInventoryColumn("inventorySupplier", "Supplier")
            SetInventoryColumn("inventoryStatus", "Status")
            inventoryDataGrid.Columns("inventoryExpiry").DefaultCellStyle.Format = "d"
            inventoryDataGrid.DataSource = products
            inventoryGenerated.Text = "Generated: " & DateTime.Now.ToString("MMM dd, yyyy h:mm tt") & " | By: " & CurrentFullName

            ShowReportPanel(pnlInventoryStockReport)
        Catch ex As Exception
            MessageBox.Show("Could not load the inventory report. " & ex.Message, "Inventory report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetInventoryColumn(columnName As String, propertyName As String)
        inventoryDataGrid.Columns(columnName).DataPropertyName = propertyName
    End Sub

    Private Sub ExpiryAlert_Click(sender As Object, e As EventArgs)
        Using monitor As New ExpiryManagementForm()
            monitor.ShowDialog(Me)
        End Using
    End Sub

    Private Sub ShowReportPanel(reportPanel As Panel)
        LayoutAllReportDetails()
        HideAllPanels()
        reportPanel.Visible = True
        reportPanel.BringToFront()
        hdrDashboard.Text = "Reports"
        hdrAccounts.Text = "Reports"
        Label6.Text = "Reports"
        LayoutPrimaryHeaders()
        LayoutReportPageFor(reportPanel)
        SetActiveNavigation("Reports")
    End Sub

    Private Sub LayoutReportPageFor(reportPage As Panel)
        Select Case reportPage.Name
            Case NameOf(pnlInventoryStockReport)
                LayoutReportPage(pnlInventoryStockReport, Panel25, panelnvenotryStockReport, inventoryDataGrid, Label9, Label8, TextBox50, Label7)
            Case NameOf(pnlFinancialSalesReport)
                LayoutReportPage(pnlFinancialSalesReport, Panel31, panelFinancialSalesReport, DataGridView4, Label18, Label17)
            Case NameOf(pnlRetailSalesReport)
                LayoutReportPage(pnlRetailSalesReport, Panel24, panelRetailSalesReport, retailDataGrid, Label12, Label11, TextBox48, Label10)
            Case NameOf(pnlExpensesReport)
                LayoutReportPage(pnlExpensesReport, Panel35, panelExpenseReport, expenseDataGrid, Label26, Label25, TextBox66, Label27)
            Case NameOf(pnlPurchasesReport)
                LayoutReportPage(pnlPurchasesReport, Panel33, panelPurchasesReport, purchasesDataGrid, Label21, Label20, TextBox61, Label22)
            Case NameOf(pnlWholesaleSalesReport)
                LayoutReportPage(pnlWholesaleSalesReport, Panel29, panelWholesale, wholesaleDataGrid, Label15, Label14, TextBox25, Label32)
            Case NameOf(pnlEmployeesalesReport)
                LayoutReportPage(pnlEmployeesalesReport, Panel37, panelEmployeeSalesReport, employeDataGrid, Label30, Label29, TextBox68, Label31, panelEmployeeDataGrid)
        End Select
    End Sub

    Private Sub SetActiveNavigation(activePage As String)
        For Each button As Button In {btnDashboard, btnAccounts, btnReports}
            Dim isActive = String.Equals(Convert.ToString(button.Tag), activePage, StringComparison.OrdinalIgnoreCase)
            button.BackColor = If(isActive, Color.White, Color.FromArgb(18, 101, 54))
            button.ForeColor = If(isActive, Color.FromArgb(20, 100, 60), Color.White)
            button.UseVisualStyleBackColor = False
        Next
    End Sub

    Private Sub LoadSalesDashboardStats()
        Try
            Dim summary = New SalesRepository().GetSummary()
            dashboardTodaySales = summary.TodaySales
            dashboardTodayTransactions = summary.RetailTransactions + summary.WholesaleTransactions
            userIDText.Text = "P " & summary.TodaySales.ToString("N2")
            monthDateYear.Text = DateTime.Today.ToString("MMMM dd, yyyy")
            TextBox3.Text = "P " & summary.RetailSales.ToString("N2")
            transactionRetail.Text = summary.RetailTransactions.ToString() & " Transactions"
            TextBox6.Text = "P " & summary.WholesaleSales.ToString("N2")
            transactionsWholesale.Text = summary.WholesaleTransactions.ToString() & " Transactions"
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
                Dim stock = If(row.IsNull("Stock Quantity"), 0, Convert.ToInt32(row("Stock Quantity")))
                Dim reorderPoint = If(row.IsNull("Reorder Point"), 0, Convert.ToInt32(row("Reorder Point")))
                If stock <= reorderPoint Then lowStockCount += 1

                If Not row.IsNull("Expiry Date") Then
                    Dim expiry = Convert.ToDateTime(row("Expiry Date")).Date
                    If expiry <= Date.Today Then
                        expiredCount += 1
                    ElseIf expiry <= Date.Today.AddMonths(6) Then
                        nearExpiryCount += 1
                    End If
                End If
            Next

            TextBox15.Text = products.Rows.Count.ToString()
            allCategories.Text = "All Categories"
            TextBox9.Text = lowStockCount.ToString()
            needOrdering.Text = "Need reordering"
            TextBox12.Text = expiredCount.ToString()
            days.Text = "Currently expired"
            TextBox19.Text = nearExpiryCount.ToString()
            requiresDisposal.Text = "Expires within 6 months"
            dashboardLowStock = lowStockCount
            dashboardNearExpiry = nearExpiryCount
            dashboardExpired = expiredCount
            TextBox20.Text = "Unavailable"
            expensesRecorded.Text = "Expense data is not stored"
            inventoryAlertLowStockCount.Text = lowStockCount.ToString(CultureInfo.CurrentCulture)
            inventoryAlertNearExpiryCount.Text = nearExpiryCount.ToString(CultureInfo.CurrentCulture)
            inventoryAlertExpiredCount.Text = expiredCount.ToString(CultureInfo.CurrentCulture)
            quickOverviewRestockValue.Text = lowStockCount.ToString(CultureInfo.CurrentCulture) & " products"
            quickOverviewExpiryValue.Text = nearExpiryCount.ToString(CultureInfo.CurrentCulture) & " products"
            quickOverviewTransactionsValue.Text = dashboardTodayTransactions.ToString(CultureInfo.CurrentCulture)
            revenueNetIncomeLabel.Text = "Net income unavailable without expense records"
        Catch ex As Exception
            MessageBox.Show("Could not load inventory totals. " & ex.Message, "Inventory dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Panel12.Invalidate()
    End Sub

    Private Sub LogoutBtn_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If Not LogoutConfirmationDialog.Confirm(Me) Then Return
        Me.Close()
    End Sub

    ' --- 2. ACCOUNTS DATABASE BINDING ---
    Private Sub LoadAccounts()
        Try
            Using connection = GetConnection()
                connection.Open()
                Dim table As New DataTable()
                Using adapter As New MySqlDataAdapter("SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, IF(is_active, 'Active', 'Inactive') AS Status FROM users ORDER BY username", connection)
                    adapter.Fill(table)
                End Using

                ' Bind the SQL data to your visual designer columns
                accountsDataGrid.AutoGenerateColumns = False
                accountUserID.DataPropertyName = "UserID"
                accountFullName.DataPropertyName = "FullName"
                accountUsername.DataPropertyName = "Username"
                accountRole.DataPropertyName = "Role"
                accountStatus.DataPropertyName = "Status"

                accountDataView = table.DefaultView
                accountsDataGrid.DataSource = accountDataView
                AccountFilters_Changed(accountSearchBox, EventArgs.Empty)
                accountsDataGrid.ClearSelection()

                ' Update Summary Cards
                UpdateAccountSummaries()
            End Using
        Catch ex As Exception
            MessageBox.Show("Could not load accounts. " & ex.Message, "Account management", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateAccountSummaries()
        numberSuperAdmin.Text = GetRoleCount("Super Admin").ToString()
        numberAdmin.Text = GetRoleCount("Admin").ToString()
        numberAssistant.Text = GetRoleCount("Staff").ToString()

        Dim total As Integer = GetRoleCount("Super Admin") + GetRoleCount("Admin") + GetRoleCount("Staff")
        numberTotalUsersRegistered.Text = total.ToString() & " users registered"
    End Sub

    Private Function GetRoleCount(roleName As String) As Integer
        Try
            Using connection = GetConnection()
                connection.Open()
                Using command As New MySqlCommand("SELECT COUNT(*) FROM users WHERE role = @role", connection)
                    command.Parameters.AddWithValue("@role", roleName)
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Using
        Catch
            Return 0
        End Try
    End Function

    ' --- 3. ACCOUNTS GRID ACTIONS ---
    Private Sub accountsDataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles accountsDataGrid.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim userId As Integer? = SelectedUserId(e.RowIndex)
        If Not userId.HasValue Then Return

        Select Case accountsDataGrid.Columns(e.ColumnIndex).Name
            Case "accountEdit"
                EditAccount(userId.Value)
            Case "accountResetPassword"
                ResetPassword(userId.Value)
            Case "accountEnableDisable"
                ToggleAccount(userId.Value, e.RowIndex)
        End Select
    End Sub

    Private Function SelectedUserId(rowIndex As Integer) As Integer?
        If rowIndex < 0 OrElse accountsDataGrid.Rows(rowIndex).Cells("accountUserID").Value Is Nothing Then
            Return Nothing
        End If
        Return Convert.ToInt32(accountsDataGrid.Rows(rowIndex).Cells("accountUserID").Value)
    End Function

    Private Sub btnAddAccount_Click(sender As Object, e As EventArgs) Handles btnAddAccount.Click
        ShowAccountDialog(Nothing)
    End Sub

    Private Sub EditAccount(userId As Integer)
        ShowAccountDialog(GetAccountDetails(userId))
    End Sub

    Private Sub ToggleAccount(userId As Integer, rowIndex As Integer)
        Dim active As Boolean = accountsDataGrid.Rows(rowIndex).Cells("accountStatus").Value.ToString() = "Active"
        Execute("UPDATE users SET is_active=@active WHERE user_id=@id", Not active, userId)
        LoadAccounts()
    End Sub

    ' Custom drawing for the toggle switch in the grid
    Private Sub accountsDataGrid_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles accountsDataGrid.CellPainting
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim columnName = accountsDataGrid.Columns(e.ColumnIndex).Name
        If columnName = "accountRole" OrElse columnName = "accountStatus" Then
            e.PaintBackground(e.CellBounds, True)
            Dim text = Convert.ToString(e.FormattedValue, CultureInfo.CurrentCulture)
            Dim isStatus = columnName = "accountStatus"
            Dim active = String.Equals(text, "Active", StringComparison.OrdinalIgnoreCase)
            Dim fillColor = If(isStatus, If(active, Color.FromArgb(232, 248, 240), Color.FromArgb(253, 238, 238)), Color.FromArgb(244, 247, 245))
            Dim textColor = If(isStatus, If(active, Color.FromArgb(24, 126, 76), Color.FromArgb(170, 65, 65)), Color.FromArgb(55, 70, 62))
            Dim contentWidth = TextRenderer.MeasureText(text, accountsDataGrid.Font).Width + If(isStatus, 26, 18)
            Dim pillWidth = Math.Min(e.CellBounds.Width - 12, Math.Max(54, contentWidth))
            Dim pillBounds As New Rectangle(e.CellBounds.Left + (e.CellBounds.Width - pillWidth) \ 2, e.CellBounds.Top + 10, pillWidth, e.CellBounds.Height - 20)
            DrawAccountPill(e.Graphics, pillBounds, fillColor, Color.Transparent, 10)
            If isStatus Then
                Using dotBrush As New SolidBrush(textColor)
                    e.Graphics.FillEllipse(dotBrush, pillBounds.Left + 9, pillBounds.Top + (pillBounds.Height - 6) \ 2, 6, 6)
                End Using
                TextRenderer.DrawText(e.Graphics, text, accountsDataGrid.Font, New Rectangle(pillBounds.Left + 19, pillBounds.Top, pillBounds.Width - 22, pillBounds.Height), textColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.Left)
            Else
                TextRenderer.DrawText(e.Graphics, text, accountsDataGrid.Font, pillBounds, textColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            End If
            e.Handled = True
            Return
        End If
        If columnName = "accountEdit" OrElse columnName = "accountResetPassword" Then
            e.PaintBackground(e.CellBounds, True)
            Dim buttonText = Convert.ToString(e.FormattedValue, CultureInfo.CurrentCulture)
            Dim buttonWidth = Math.Min(e.CellBounds.Width - 12, If(columnName = "accountEdit", 76, 92))
            Dim buttonBounds As New Rectangle(e.CellBounds.Left + (e.CellBounds.Width - buttonWidth) \ 2, e.CellBounds.Top + 8, buttonWidth, e.CellBounds.Height - 16)
            DrawAccountPill(e.Graphics, buttonBounds, Color.White, Color.FromArgb(210, 218, 214), 7)
            Dim glyph = If(columnName = "accountEdit", "✎", "↻")
            TextRenderer.DrawText(e.Graphics, glyph, accountsDataGrid.Font, New Rectangle(buttonBounds.Left + 6, buttonBounds.Top, 16, buttonBounds.Height), Color.FromArgb(55, 70, 62), TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            TextRenderer.DrawText(e.Graphics, buttonText, accountsDataGrid.Font, New Rectangle(buttonBounds.Left + 22, buttonBounds.Top, buttonBounds.Width - 24, buttonBounds.Height), Color.FromArgb(35, 45, 40), TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            e.Handled = True
            Return
        End If
        If columnName <> "accountEnableDisable" Then Return
        If accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value Is Nothing Then Return

        Dim statusValue = accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value
        Dim isActive = statusValue.ToString() = "Active"

        e.PaintBackground(e.CellBounds, True)

        Dim switchWidth = 44.0F
        Dim switchHeight = 22
        Dim switchLeft = e.CellBounds.Left + (e.CellBounds.Width - switchWidth) \ 2
        Dim switchTop = e.CellBounds.Top + (e.CellBounds.Height - switchHeight) \ 2
        Dim knobSize = 16
        Dim knobLeft As Integer = If(isActive, switchLeft + switchWidth - knobSize - 3, switchLeft + 3)
        Dim knobTop = switchTop + 3
        Dim trackColor = If(isActive, Color.ForestGreen, Color.Firebrick)

        Using trackBrush As New SolidBrush(trackColor), knobBrush As New SolidBrush(Color.White), borderPen As New Pen(Color.DimGray)
            e.Graphics.FillEllipse(trackBrush, switchLeft, switchTop, switchHeight, switchHeight)
            e.Graphics.FillEllipse(trackBrush, switchLeft + switchWidth - switchHeight, switchTop, switchHeight, switchHeight)
            e.Graphics.FillRectangle(trackBrush, switchLeft + switchHeight \ 2, switchTop, switchWidth - switchHeight, switchHeight)
            e.Graphics.FillEllipse(knobBrush, knobLeft, knobTop, knobSize, knobSize)
            e.Graphics.DrawEllipse(borderPen, knobLeft, knobTop, knobSize, knobSize)
        End Using

        e.Handled = True
    End Sub

    Private Sub DrawAccountPill(graphics As Graphics, bounds As Rectangle, fillColor As Color, borderColor As Color, radius As Integer)
        If bounds.Width <= 0 OrElse bounds.Height <= 0 Then Return
        Using path As New Drawing.Drawing2D.GraphicsPath()
            Dim diameter = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height))
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90)
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90)
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90)
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90)
            path.CloseFigure()
            Using fill As New SolidBrush(fillColor)
                graphics.FillPath(fill, path)
            End Using
            If borderColor <> Color.Transparent Then
                Using border As New Pen(borderColor)
                    graphics.DrawPath(border, path)
                End Using
            End If
        End Using
    End Sub

    ' --- 4. DATA MODELS & QUERIES ---
    Private Class AccountDetails
        Public UserId As Integer
        Public Username As String
        Public FullName As String
        Public FirstName As String
        Public LastName As String
        Public StaffInitials As String
        Public Role As String
        Public WorkShift As String
        Public EmailAddress As String
        Public ContactNumber As String
        Public HireDate As DateTime?
        Public BirthDate As DateTime?
        Public IsActive As Boolean
    End Class

    Private Function GetAccountDetails(id As Integer) As AccountDetails
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand("SELECT * FROM users WHERE user_id = @id", connection)
                command.Parameters.AddWithValue("@id", id)
                Using reader = command.ExecuteReader()
                    If Not reader.Read() Then Return Nothing
                    Return New AccountDetails With {
                        .UserId = reader.GetInt32("user_id"),
                        .Username = ReadText(reader, "username"),
                        .FullName = ReadText(reader, "full_name"),
                        .FirstName = ReadText(reader, "first_name"),
                        .LastName = ReadText(reader, "last_name"),
                        .StaffInitials = ReadText(reader, "staff_initials"),
                        .Role = ReadText(reader, "role"),
                        .WorkShift = ReadText(reader, "work_shift"),
                        .EmailAddress = ReadText(reader, "email_address"),
                        .ContactNumber = ReadText(reader, "contact_number"),
                        .HireDate = ReadDate(reader, "hire_date"),
                        .BirthDate = ReadDate(reader, "birth_date"),
                        .IsActive = Convert.ToBoolean(reader("is_active"))
                    }
                End Using
            End Using
        End Using
    End Function

    Private Sub SaveAccount(id As Integer, isNew As Boolean, username As String, fullName As String, firstName As String, lastName As String, staffInitials As String, role As String, workShift As String, email As String, contact As String, hireDate As Object, birthDate As Object, pin As String, password As String, isActive As Boolean)
        Using connection = GetConnection()
            connection.Open()
            Dim sql As String
            If isNew Then
                sql = "INSERT INTO users (username, password_hash, full_name, first_name, last_name, staff_initials, role, work_shift, email_address, contact_number, hire_date, birth_date, pin_code, is_active) VALUES (@username, @passwordHash, @fullName, @firstName, @lastName, @staffInitials, @role, @workShift, @email, @contact, @hireDate, @birthDate, IF(@changePin, @pinHash, NULL), @isActive)"
            Else
                sql = "UPDATE users SET username=@username, full_name=@fullName, first_name=@firstName, last_name=@lastName, staff_initials=@staffInitials, role=@role, work_shift=@workShift, email_address=@email, contact_number=@contact, hire_date=@hireDate, birth_date=@birthDate, pin_code=IF(@changePin, @pinHash, pin_code), is_active=@isActive, password_hash=IF(@changePassword, @passwordHash, password_hash) WHERE user_id=@id"
            End If
            Using command As New MySqlCommand(sql, connection)
                command.Parameters.AddWithValue("@username", username)
                command.Parameters.AddWithValue("@fullName", fullName)
                command.Parameters.AddWithValue("@firstName", firstName)
                command.Parameters.AddWithValue("@lastName", lastName)
                command.Parameters.AddWithValue("@staffInitials", staffInitials)
                command.Parameters.AddWithValue("@role", role)
                command.Parameters.AddWithValue("@workShift", workShift)
                command.Parameters.AddWithValue("@email", If(email = "", DBNull.Value, email))
                command.Parameters.AddWithValue("@contact", If(contact = "", DBNull.Value, contact))
                command.Parameters.AddWithValue("@hireDate", hireDate)
                command.Parameters.AddWithValue("@birthDate", birthDate)
                command.Parameters.AddWithValue("@isActive", isActive)
                command.Parameters.AddWithValue("@changePassword", password.Length > 0)
                command.Parameters.AddWithValue("@changePin", pin.Length > 0)

                ' We only hash the password if it's new or being changed, using your existing DatabaseModule HashPassword function
                command.Parameters.AddWithValue("@passwordHash", HashPassword(If(password = "", "unused", password)))
                command.Parameters.AddWithValue("@pinHash", HashPassword(If(pin = "", "0000", pin)))

                If Not isNew Then command.Parameters.AddWithValue("@id", id)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub Execute(sql As String, ParamArray values() As Object)
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand(sql, connection)
                If sql.Contains("@passwordHash") Then command.Parameters.AddWithValue("@passwordHash", values(0))
                If sql.Contains("@active") Then command.Parameters.AddWithValue("@active", values(0))
                If sql.Contains("@id") Then command.Parameters.AddWithValue("@id", values(values.Length - 1))
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Function ReadText(reader As MySqlDataReader, field As String) As String
        Return If(reader.IsDBNull(reader.GetOrdinal(field)), "", reader(field).ToString())
    End Function

    Private Function ReadDate(reader As MySqlDataReader, field As String) As DateTime?
        Try
            If reader.IsDBNull(reader.GetOrdinal(field)) Then Return Nothing
            Dim value = Convert.ToDateTime(reader(field))
            If value < New DateTime(1900, 1, 1) Then Return Nothing
            Return value
        Catch
            Return Nothing
        End Try
    End Function

    ' --- 5. DYNAMIC POPUP DIALOGS ---
    Private Sub ShowAccountDialog(account As AccountDetails)
        Dim isNew = account Is Nothing
        If isNew Then account = New AccountDetails With {.Role = "Staff", .IsActive = True}

        Dim dialog As New Form With {.Text = If(isNew, "Add Employee", "Edit Employee"), .Size = New Size(680, 720), .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False, .MinimizeBox = False, .BackColor = Color.White}
        Dim fields As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .AutoScroll = True, .Padding = New Padding(24, 20, 24, 10)}
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 68.0F))

        Dim firstNameBox = New TextBox With {.Text = account.FirstName, .Dock = DockStyle.Fill}
        Dim lastNameBox = New TextBox With {.Text = account.LastName, .Dock = DockStyle.Fill}
        Dim initialsBox = New TextBox With {.Text = account.StaffInitials, .Dock = DockStyle.Fill}
        Dim usernameBox = New TextBox With {.Text = account.Username, .Dock = DockStyle.Fill}
        Dim roleBox As New ComboBox With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList}
        roleBox.Items.AddRange({"Staff", "Admin", "Super Admin"})
        roleBox.SelectedItem = If(roleBox.Items.Contains(account.Role), account.Role, "Staff")
        Dim workShiftBox = New TextBox With {.Text = account.WorkShift, .Dock = DockStyle.Fill}
        Dim emailBox = New TextBox With {.Text = account.EmailAddress, .Dock = DockStyle.Fill}
        Dim contactBox = New TextBox With {.Text = account.ContactNumber, .Dock = DockStyle.Fill}
        Dim hireDateBox = CreateDatePicker(account.HireDate)
        Dim birthDateBox = CreateDatePicker(account.BirthDate)
        Dim passwordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim confirmPasswordBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim showPasswordsBox As New CheckBox With {.Text = "Show passwords", .AutoSize = True}

        Dim lengthRule As New Label With {.AutoSize = True}
        Dim capitalRule As New Label With {.AutoSize = True}
        Dim numberRule As New Label With {.AutoSize = True}
        Dim symbolRule As New Label With {.AutoSize = True}
        Dim passwordRules As New FlowLayoutPanel With {.FlowDirection = FlowDirection.TopDown, .WrapContents = False, .AutoSize = True}
        passwordRules.Controls.AddRange({lengthRule, capitalRule, numberRule, symbolRule})

        UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)
        AddHandler passwordBox.TextChanged, Sub() UpdatePasswordRules(passwordBox.Text, lengthRule, capitalRule, numberRule, symbolRule)
        AddHandler showPasswordsBox.CheckedChanged, Sub()
                                                        passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                        confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                    End Sub

        Dim pinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim confirmPinBox = New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True, .MaxLength = 4}
        Dim activeBox = New CheckBox With {.Text = "Employee is active", .Checked = account.IsActive, .AutoSize = True}

        AddSection(fields, "PERSONAL INFORMATION")
        AddField(fields, "First name *", firstNameBox)
        AddField(fields, "Last name *", lastNameBox)
        AddField(fields, "Birth date *", birthDateBox)
        AddField(fields, "Email address", emailBox)
        AddField(fields, "Contact number", contactBox)
        AddSection(fields, "EMPLOYMENT INFORMATION")
        AddField(fields, "Staff initials *", initialsBox)
        AddField(fields, "Work shift *", workShiftBox)
        AddField(fields, "Role *", roleBox)
        AddField(fields, "Hire date *", hireDateBox)
        AddField(fields, "Status", activeBox)
        AddSection(fields, "LOGIN CREDENTIALS")
        AddField(fields, "Username *", usernameBox)

        If isNew Then
            AddField(fields, "Password *", passwordBox)
            AddField(fields, "Confirm password *", confirmPasswordBox)
            AddField(fields, "Show password", showPasswordsBox)
            AddField(fields, "Password requirements", passwordRules)
        End If

        Dim buttons As New FlowLayoutPanel With {.Dock = DockStyle.Bottom, .Height = 56, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(12)}
        Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Width = 90}
        Dim saveButton As New Button With {.Text = "Save Employee", .Width = 110, .BackColor = Color.SeaGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

        AddHandler saveButton.Click, Sub()
                                         ' Validation Logic
                                         If String.IsNullOrWhiteSpace(firstNameBox.Text) OrElse String.IsNullOrWhiteSpace(lastNameBox.Text) OrElse String.IsNullOrWhiteSpace(usernameBox.Text) Then
                                             MessageBox.Show("Required fields missing.", "Employee management", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                             Return
                                         End If
                                         Try
                                             SaveAccount(account.UserId, isNew, usernameBox.Text.Trim(), firstNameBox.Text & " " & lastNameBox.Text, firstNameBox.Text.Trim(), lastNameBox.Text.Trim(), initialsBox.Text.Trim(), roleBox.Text, workShiftBox.Text.Trim(), emailBox.Text.Trim(), contactBox.Text.Trim(), DateFromPicker(hireDateBox), DateFromPicker(birthDateBox), pinBox.Text, passwordBox.Text, activeBox.Checked)
                                             dialog.DialogResult = DialogResult.OK
                                             dialog.Close()
                                             LoadAccounts()
                                         Catch ex As MySqlException
                                             MessageBox.Show("Could not save the employee. " & ex.Message, "Employee management", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End Try
                                     End Sub

        buttons.Controls.Add(saveButton)
        buttons.Controls.Add(cancelButton)
        dialog.AcceptButton = saveButton
        dialog.CancelButton = cancelButton
        dialog.Controls.Add(fields)
        dialog.Controls.Add(buttons)
        dialog.ShowDialog(Me)
    End Sub

    Private Sub ResetPassword(userId As Integer)
        ShowResetPasswordDialog(userId)
    End Sub

    Private Sub ShowResetPasswordDialog(userId As Integer)
        Dim dialog As New Form With {.Text = "Reset Password", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .ClientSize = New Size(440, 390)}

        Dim fields As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .AutoScroll = True, .Padding = New Padding(20)}
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 36.0F))
        fields.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 64.0F))

        Dim passwordBox As New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim confirmPasswordBox As New TextBox With {.Dock = DockStyle.Fill, .UseSystemPasswordChar = True}
        Dim showPasswordsBox As New CheckBox With {.Text = "Show passwords", .AutoSize = True}

        AddHandler showPasswordsBox.CheckedChanged, Sub()
                                                        passwordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                        confirmPasswordBox.UseSystemPasswordChar = Not showPasswordsBox.Checked
                                                    End Sub

        AddField(fields, "New password *", passwordBox)
        AddField(fields, "Confirm password *", confirmPasswordBox)
        AddField(fields, "Password visibility", showPasswordsBox)

        Dim buttons As New FlowLayoutPanel With {.Dock = DockStyle.Bottom, .Height = 60, .FlowDirection = FlowDirection.RightToLeft, .Padding = New Padding(12)}
        Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Width = 90}
        Dim saveButton As New Button With {.Text = "Save Password", .Width = 115, .BackColor = Color.SeaGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

        AddHandler saveButton.Click, Sub()
                                         If passwordBox.Text <> confirmPasswordBox.Text Then
                                             MessageBox.Show("Passwords do not match.", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                             Return
                                         End If
                                         Try
                                             Execute("UPDATE users SET password_hash=@passwordHash WHERE user_id=@id", HashPassword(passwordBox.Text), userId)
                                             MessageBox.Show("Password reset successfully.", "Employee Management", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                             dialog.Close()
                                         Catch ex As Exception
                                             MessageBox.Show("Error resetting password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                         End Try
                                     End Sub

        buttons.Controls.Add(saveButton)
        buttons.Controls.Add(cancelButton)
        dialog.AcceptButton = saveButton
        dialog.CancelButton = cancelButton
        dialog.Controls.Add(fields)
        dialog.Controls.Add(buttons)
        dialog.ShowDialog(Me)
    End Sub

    ' Password Validation Helpers
    Private Sub UpdatePasswordRules(password As String, lengthRule As Label, capitalRule As Label, numberRule As Label, symbolRule As Label)
        SetPasswordRule(lengthRule, password.Length >= 8, "At least 8 characters")
        SetPasswordRule(capitalRule, Regex.IsMatch(password, "[A-Z]"), "At least 1 uppercase letter")
        SetPasswordRule(numberRule, Regex.IsMatch(password, "\d"), "At least 1 number")
        SetPasswordRule(symbolRule, Regex.IsMatch(password, "[^a-zA-Z0-9]"), "At least 1 special character")
    End Sub

    Private Sub SetPasswordRule(ruleLabel As Label, passed As Boolean, ruleText As String)
        ruleLabel.Text = If(passed, "✓ ", "✗ ") & ruleText
        ruleLabel.ForeColor = If(passed, Color.ForestGreen, Color.Firebrick)
    End Sub

    Private Function CreateDatePicker(value As DateTime?) As DateTimePicker
        Dim picker As New DateTimePicker With {.Dock = DockStyle.Fill, .Format = DateTimePickerFormat.Short, .ShowCheckBox = True}
        If value.HasValue AndAlso value.Value >= picker.MinDate AndAlso value.Value <= picker.MaxDate Then
            picker.Value = value.Value
            picker.Checked = True
        Else
            picker.Checked = False
        End If
        Return picker
    End Function

    Private Function DateFromPicker(picker As DateTimePicker) As Object
        Return If(picker.Checked, picker.Value.Date, DBNull.Value)
    End Function

    Private Sub AddSection(table As TableLayoutPanel, title As String)
        Dim row = table.RowCount
        table.RowCount += 1
        table.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Dim label As New Label With {.Text = title, .Font = New Font("Segoe UI", 9, FontStyle.Bold), .ForeColor = Color.SeaGreen, .AutoSize = True, .Margin = New Padding(0, 16, 0, 5)}
        table.Controls.Add(label, 0, row)
        table.SetColumnSpan(label, 2)
    End Sub

    Private Sub AddField(table As TableLayoutPanel, caption As String, control As Control)
        Dim row = table.RowCount
        table.RowCount += 1
        table.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        table.Controls.Add(New Label With {.Text = caption, .AutoSize = True, .Anchor = AnchorStyles.Left, .Margin = New Padding(0, 8, 12, 8)}, 0, row)
        control.Margin = New Padding(0, 5, 0, 5)
        table.Controls.Add(control, 1, row)
    End Sub

    Private Sub pnlDashboard_Paint(sender As Object, e As PaintEventArgs) Handles pnlDashboard.Paint

    End Sub


    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub storeName1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub storeName_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub logoBox_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub logoBox_Click_1(sender As Object, e As EventArgs) Handles logoBox.Click

    End Sub

    Private Sub PharName_Click(sender As Object, e As EventArgs) Handles PharName.Click

    End Sub

    Private Sub navDashboard_Click(sender As Object, e As EventArgs)

    End Sub
End Class
