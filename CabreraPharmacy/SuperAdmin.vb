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
    Private salesReportPeriodButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private salesReportModeButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)

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
        usrName.Text = "Welcome, " & CurrentFullName & " (Super Admin)"
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

        ' Set initial screen
        AddHandler pnlDashboard.Resize, AddressOf LayoutDashboardCards
        AddHandler pnlReports.Resize, AddressOf LayoutReports
        LayoutRootContent()
        ShowPrimaryView(pnlDashboard, "Dashboard")
        LayoutDashboardCards(pnlDashboard, EventArgs.Empty)
        LayoutReports(pnlReports, EventArgs.Empty)

        ' Load the database data into the Accounts grid
        LoadAccounts()
        LoadSalesDashboardStats()

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
        Dim headerHeight = 100
        pnlHeaderDashboard.SetBounds(0, 0, headerWidth, headerHeight)
        pnlHeaderAccounts.SetBounds(0, 0, headerWidth, headerHeight)
        pnlHeaderReports.SetBounds(0, 0, headerWidth, headerHeight)

        titleLabel.SetBounds(20, 10, Math.Max(1, headerWidth - 280), 45)
        titleLabel.Text = hdrDashboard.Text
        subtitleLabel.SetBounds(20, 58, Math.Max(1, headerWidth - 280), 20)
        hdrDashboard.Visible = False

        hdrAccounts.SetBounds(20, 10, Math.Max(1, headerWidth - 280), 45)
        Label3.SetBounds(20, 58, Math.Max(1, headerWidth - 280), 20)
        Label6.SetBounds(20, 10, Math.Max(1, headerWidth - 280), 45)
        Label5.SetBounds(20, 58, Math.Max(1, headerWidth - 280), 20)

        LayoutHeaderUser(usrName, usrSuperAdmin, headerWidth)
        LayoutHeaderUser(TextBox23, Label2, headerWidth)
        LayoutHeaderUser(TextBox24, Label4, headerWidth)
    End Sub

    Private Sub LayoutHeaderUser(userName As TextBox, role As Label, headerWidth As Integer)
        Dim userWidth = Math.Min(300, Math.Max(180, headerWidth - 40))
        userName.SetBounds(headerWidth - userWidth - 20, 24, userWidth, 20)
        userName.TextAlign = HorizontalAlignment.Right
        role.Location = New Point(headerWidth - userWidth - 20, 50)
        role.AutoSize = False
        role.Size = New Size(userWidth, 16)
        role.TextAlign = ContentAlignment.MiddleRight
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
        titleLabel.Text = pageTitle
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

        TableLayoutPanel3.Visible = False
        TableLayoutPanel4.Visible = False
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

        Dim left = 14
        Dim top = pnlHeaderDashboard.Height + 14
        Dim rightMargin = 14
        Dim rowGap = 16
        Dim availableWidth = Math.Max(1, pnlDashboard.ClientSize.Width - left - rightMargin)
        Dim availableHeight = pnlDashboard.ClientSize.Height - top - 14 - rowGap
        If availableHeight < 220 Then
            TableLayoutPanel1.Visible = False
            TableLayoutPanel2.Visible = False
            pnlDashboard.AutoScroll = True
            pnlDashboard.AutoScrollMinSize = New Size(availableWidth, top + 220)
            Return
        End If

        pnlDashboard.AutoScroll = False
        TableLayoutPanel1.Visible = True
        TableLayoutPanel2.Visible = True
        pnlDashboard.AutoScrollMinSize = Size.Empty

        Dim firstRowHeight = availableHeight \ 2
        TableLayoutPanel1.SetBounds(left, top, availableWidth, firstRowHeight)
        TableLayoutPanel2.SetBounds(left, top + firstRowHeight + rowGap, availableWidth, availableHeight - firstRowHeight)

        FormatDashboardCard(Panel1, todaysSale, userIDText, monthDateYear)
        FormatDashboardCard(Panel2, TextBox2, TextBox3, transactionRetail)
        FormatDashboardCard(Panel3, TextBox5, TextBox6, transactionsWholesale)
        FormatDashboardCard(Panel4, TextBox14, TextBox15, allCategories)
        FormatDashboardCard(Panel5, TextBox8, TextBox9, needOrdering)
        FormatDashboardCard(Panel6, TextBox11, TextBox12, days)
        FormatDashboardCard(Panel7, TextBox17, TextBox19, requiresDisposal)
        FormatDashboardCard(Panel8, TextBox18, TextBox20, expensesRecorded)
    End Sub

    Private Sub FormatDashboardCard(card As Panel, title As TextBox, value As TextBox, footer As TextBox)
        If card.ClientSize.Width <= 0 OrElse card.ClientSize.Height <= 0 Then Return

        card.BackColor = Color.White
        title.ReadOnly = True
        title.TabStop = False
        title.BorderStyle = BorderStyle.None
        title.BackColor = Color.White
        If title.Font.Size <> 11.0F OrElse title.Font.Style <> FontStyle.Bold Then title.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold)
        title.Location = New Point(20, 20)
        title.Size = New Size(Math.Max(80, card.ClientSize.Width - 40), 24)

        value.ReadOnly = True
        value.TabStop = False
        value.BorderStyle = BorderStyle.None
        value.BackColor = Color.White
        If value.Font.Size <> 24.0F OrElse value.Font.Style <> FontStyle.Regular Then value.Font = New Font("Segoe UI", 24.0F)
        value.Location = New Point(20, Math.Max(55, card.ClientSize.Height \ 2 - 16))
        value.Size = New Size(Math.Max(80, card.ClientSize.Width - 40), 42)

        footer.ReadOnly = True
        footer.TabStop = False
        footer.BorderStyle = BorderStyle.None
        footer.BackColor = Color.White
        If footer.Font.Size <> 9.0F OrElse footer.Font.Style <> FontStyle.Regular Then footer.Font = New Font("Segoe UI", 9.0F)
        footer.ForeColor = Color.Gray
        footer.Location = New Point(20, card.ClientSize.Height - 40)
        footer.Size = New Size(Math.Max(80, card.ClientSize.Width - 40), 22)
    End Sub

    Private Sub btnAccounts_Click(sender As Object, e As EventArgs) Handles btnAccounts.Click
        ShowPrimaryView(pnlAccounts, "Accounts")
        LoadAccounts() ' Refresh data when opened
    End Sub

    Private Sub LayoutAccounts_Resize(sender As Object, e As EventArgs)
        LayoutAccounts()
    End Sub

    Private Sub LayoutAccounts()
        If pnlAccounts.ClientSize.Width <= 0 OrElse pnlAccounts.ClientSize.Height <= 0 Then Return

        Dim horizontalPadding = 14
        Dim top = pnlHeaderAccounts.Height + 14
        Dim availableWidth = Math.Max(1, pnlAccounts.ClientSize.Width - horizontalPadding * 2)
        Dim summaryHeight = 112
        Dim gap = 12
        Dim summaryRowTop = top + summaryHeight + gap
        Dim gridTop = summaryRowTop + btnAddAccount.Height + 8

        accountTableLayout.SetBounds(horizontalPadding, top, availableWidth, summaryHeight)
        numberTotalUsersRegistered.SetBounds(horizontalPadding, summaryRowTop + (btnAddAccount.Height - numberTotalUsersRegistered.Height) \ 2, Math.Max(1, availableWidth - btnAddAccount.Width - gap), numberTotalUsersRegistered.Height)
        btnAddAccount.SetBounds(pnlAccounts.ClientSize.Width - horizontalPadding - btnAddAccount.Width,
                                summaryRowTop,
                                btnAddAccount.Width,
                                btnAddAccount.Height)
        accountsDataGrid.SetBounds(horizontalPadding,
                                   gridTop,
                                   availableWidth,
                                   Math.Max(1, pnlAccounts.ClientSize.Height - gridTop - horizontalPadding))
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        LayoutReports(pnlReports, EventArgs.Empty)
        ShowPrimaryView(pnlReports, "Reports")
    End Sub

    Private Sub LayoutReports(sender As Object, e As EventArgs)
        If pnlReports.ClientSize.Width <= 0 OrElse pnlReports.ClientSize.Height <= 0 Then Return

        Dim horizontalPadding = 14
        Dim rowGap = 12
        Dim cardHeight = 110
        Dim availableWidth = Math.Max(1, pnlReports.ClientSize.Width - horizontalPadding * 2)
        Panel16.SetBounds(horizontalPadding, pnlHeaderReports.Height + rowGap, availableWidth, 76)
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
            card.Margin = New Padding(6)
        Next

        LayoutReportCard(Panel20, inventoryTitle, inventoryDescriptions, inventoryViewReport)
        LayoutReportCard(Panel21, retailTitle, retailDescriptions, retailViewReport)
        LayoutReportCard(Panel17, wholesaleTitle, wholesaleDescriptions, wholesaleViewReport)
        LayoutReportCard(Panel18, purchasesTitle, purchasesDescriptions, purchasesViewReport)
        LayoutReportCard(Panel19, expensesTitle, expensesDescriptions, expensesViewReport)
        LayoutReportCard(Panel22, financialTitle, financialDescriptions, financialViewReport)
        LayoutReportCard(Panel23, employeeTitle, employeeDescription, employeeViewReport)

        pnlReports.AutoScrollMinSize = New Size(0, top + horizontalPadding)
    End Sub

    Private Sub LayoutReportCard(card As Panel, title As TextBox, description As TextBox, viewButton As Button)
        If card.ClientSize.Width <= 0 OrElse card.ClientSize.Height <= 0 Then Return

        Dim inset = 6
        Dim textWidth = Math.Max(1, card.ClientSize.Width - inset * 2)
        title.SetBounds(inset, 10, textWidth, 26)
        description.SetBounds(inset, 38, textWidth, 22)
        viewButton.SetBounds(inset, 68, Math.Min(150, textWidth), 30)
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
            headerTitle.SetBounds(20, 6, Math.Max(1, reportPage.ClientSize.Width - 360), 30)
            headerTitle.Font = New Font("Segoe UI", 21.0F, FontStyle.Bold)
            headerTitle.ForeColor = Color.FromArgb(20, 100, 60)
            headerStore.AutoSize = False
            headerStore.Text = "Sales and inventory reports"
            headerStore.SetBounds(20, 37, Math.Max(1, reportPage.ClientSize.Width - 360), 18)
            headerStore.Font = New Font("Segoe UI", 9.0F)
            headerStore.ForeColor = Color.Gray

            If headerUserName IsNot Nothing Then
                headerUserName.SetBounds(reportPage.ClientSize.Width - 310, 10, 280, 20)
                headerUserName.TextAlign = HorizontalAlignment.Right
                headerUserName.Text = "Welcome, " & CurrentFullName & " (Super Admin)"
            End If
            If headerRole IsNot Nothing Then
                headerRole.SetBounds(reportPage.ClientSize.Width - 310, 32, 280, 16)
                headerRole.Text = "Super Admin / Owner"
                headerRole.TextAlign = ContentAlignment.MiddleRight
            End If

            Dim avatar = TryCast(pageHeader.Controls("reportHeaderAvatar"), Panel)
            If avatar Is Nothing Then
                avatar = New Panel With {.Name = "reportHeaderAvatar", .BackColor = Color.FromArgb(20, 100, 60)}
                pageHeader.Controls.Add(avatar)
            End If
            avatar.SetBounds(reportPage.ClientSize.Width - 350, 8, 36, 36)
            avatar.BringToFront()
            If headerUserName IsNot Nothing Then headerUserName.BringToFront()
            If headerRole IsNot Nothing Then headerRole.BringToFront()

            If salesReportPageLayout IsNot Nothing Then salesReportPageLayout.Dock = DockStyle.Fill
            toolbar.Dock = DockStyle.Fill
            LayoutReportToolbar(reportPage, toolbar)
            salesReportFilterPanel.Dock = DockStyle.Fill
            salesReportDetailsPanel.Dock = DockStyle.Fill
            salesDateFilterLabel.SetBounds(14, 14, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 24)
            salesDateCaption.SetBounds(14, 60, 80, 18)
            salesTransactionDate.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            salesMonthCaption.SetBounds(14, 60, 80, 18)
            salesReportMonthCombo.SetBounds(14, 82, Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            salesYearCaption.SetBounds(14, If(String.Equals(Convert.ToString(ComboBox4.SelectedItem), "Monthly", StringComparison.Ordinal), 145, 60), 80, 18)
            salesReportYearCombo.SetBounds(14, If(String.Equals(Convert.ToString(ComboBox4.SelectedItem), "Monthly", StringComparison.Ordinal), 167, 82), Math.Max(1, salesReportFilterPanel.ClientSize.Width - 28), 28)
            transactionDetailsTitle.Dock = DockStyle.Top
            transactionDetailsTitle.Height = 34
            transactionDetailsTitle.Padding = New Padding(14, 8, 0, 0)
            retailDataGrid.Dock = DockStyle.Fill
            salesReportTotalLabel.Dock = DockStyle.Bottom
            salesReportTotalLabel.Height = 44
            Return
        End If

        Dim headerHeight = 100
        Dim toolbarTop = headerHeight + 14
        Dim toolbarHeight = 100
        Dim gridTop = toolbarTop + toolbarHeight + 14

        pageHeader.SetBounds(0, 0, reportPage.ClientSize.Width, headerHeight)
        toolbar.SetBounds(horizontalPadding, toolbarTop, contentWidth, toolbarHeight)
        LayoutReportHeader(pageHeader, headerTitle, headerStore, headerUserName, headerRole)
        LayoutReportToolbar(reportPage, toolbar)

        If gridContainer Is Nothing Then
            grid.SetBounds(horizontalPadding,
                           gridTop,
                           contentWidth,
                           Math.Max(1, reportPage.ClientSize.Height - gridTop - horizontalPadding))
        Else
            gridContainer.SetBounds(horizontalPadding,
                                     gridTop,
                                     contentWidth,
                                     Math.Max(1, reportPage.ClientSize.Height - gridTop - horizontalPadding))
            employeeName.SetBounds(horizontalPadding, 14, Math.Max(1, gridContainer.ClientSize.Width - horizontalPadding * 2), 26)
            grid.SetBounds(horizontalPadding,
                           employeeName.Bottom + 10,
                           Math.Max(1, gridContainer.ClientSize.Width - horizontalPadding * 2),
                           Math.Max(1, gridContainer.ClientSize.Height - employeeName.Bottom - 24))
        End If
    End Sub

    Private Sub LayoutReportHeader(pageHeader As Panel,
                                   headerTitle As Label,
                                   headerStore As Label,
                                   headerUserName As TextBox,
                                   headerRole As Label)
        Dim width = pageHeader.ClientSize.Width
        headerTitle.AutoSize = False
        headerTitle.SetBounds(20, 8, Math.Max(1, width - 340), 42)
        If headerTitle.Font.Size <> 22.0F OrElse headerTitle.Font.Style <> FontStyle.Bold Then
            headerTitle.Font = New Font("Segoe UI", 22.0F, FontStyle.Bold)
        End If
        headerTitle.ForeColor = Color.FromArgb(20, 100, 60)
        headerStore.AutoSize = False
        headerStore.SetBounds(20, 54, Math.Max(1, width - 340), 22)
        If headerStore.Font.Size <> 10.0F OrElse headerStore.Font.Style <> FontStyle.Regular Then
            headerStore.Font = New Font("Segoe UI", 10.0F)
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

        LayoutHeaderUser(headerUserName, headerRole, width)
        headerUserName.Text = "Welcome, " & CurrentFullName & " (Super Admin)"
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
            printButton.SetBounds(right - buttonWidth, 12, buttonWidth, 34)
        End If

        If toolbar Is panelRetailSalesReport Then
            titleRetailSalesReport.Font = New Font("Segoe UI", 14.0F, FontStyle.Bold)
            titleRetailSalesReport.SetBounds(inset, 7, Math.Max(1, toolbar.ClientSize.Width - 390), 24)
            retailGenerated.Font = New Font("Segoe UI", 8.5F)
            retailGenerated.SetBounds(inset, 31, Math.Max(1, toolbar.ClientSize.Width - 390), 18)
            retailGenerated.Visible = True
            retailPrintReport.Font = New Font("Segoe UI", 9.0F)
            retailPrintReport.SetBounds(right - 126 - 10 - 112, 7, 112, 32)
            salesBackToReports.Font = New Font("Segoe UI", 9.0F)
            salesBackToReports.BackColor = Color.White
            salesBackToReports.ForeColor = Color.FromArgb(20, 100, 60)
            salesBackToReports.FlatAppearance.BorderSize = 1
            salesBackToReports.SetBounds(right - 126, 7, 126, 32)
            ComboBox4.Visible = False
            salesTransactionFilter.Visible = False
            LayoutSalesReportFilterButtons(toolbar)
            Return
        End If

        Dim generated = toolbar.Controls.OfType(Of TextBox)().FirstOrDefault(Function(textBox) textBox.Name.ToLowerInvariant().Contains("generated"))
        If toolbar Is panelEmployeeSalesReport Then generated = TextBox67
        If generated IsNot Nothing Then
            generated.SetBounds(inset, 56, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - inset * 3), 22)
        End If

        Dim toolbarTitle = toolbar.Controls.OfType(Of TextBox)().FirstOrDefault(Function(textBox) textBox.Name.StartsWith("title", StringComparison.OrdinalIgnoreCase))
        If toolbarTitle IsNot Nothing Then
            toolbarTitle.SetBounds(inset, 12, Math.Max(1, toolbar.ClientSize.Width - buttonWidth - inset * 3), 28)
        End If

        Dim selector = toolbar.Controls.OfType(Of ComboBox)().FirstOrDefault()
        If selector IsNot Nothing AndAlso selector.Visible Then
            selector.SetBounds(right - selector.Width, 54, selector.Width, selector.Height)
            If generated IsNot Nothing Then
                generated.Width = Math.Max(1, selector.Left - inset * 2)
            End If
        End If

        If toolbar Is panelEmployeeSalesReport Then
            employeeNamesDropdown.SetBounds(right - employeeNamesDropdown.Width, 54, employeeNamesDropdown.Width, employeeNamesDropdown.Height)
            employeeScheduleDropdown.SetBounds(inset, 54, employeeScheduleDropdown.Width, employeeScheduleDropdown.Height)
        ElseIf toolbar Is panelExpenseReport Then
            titleExpensesReport.SetBounds(inset, 12, Math.Max(1, toolbar.ClientSize.Width - inset * 2), 28)
            Dim periodWidth = expensesPeriodDropdown.Width
            Dim dateWidth = expensesDateTime.Width
            expensesPeriodDropdown.SetBounds(right - periodWidth, 54, periodWidth, expensesPeriodDropdown.Height)
            expensesDateTime.SetBounds(right - periodWidth - 10 - dateWidth, 54, dateWidth, expensesDateTime.Height)
            expensesSearchBar.SetBounds(inset, 52, Math.Max(1, expensesDateTime.Left - inset * 2), 28)
            btnAddExpenses.SetBounds(right - buttonWidth, 10, buttonWidth, 34)
            ComboBox6.Visible = False
            Button15.Visible = False
        ElseIf toolbar Is panelPurchasesReport Then
            titlePurchasesReport.SetBounds(inset, 12, Math.Max(1, toolbar.ClientSize.Width - inset * 2), 28)
            Dim periodWidth = ComboBox5.Width
            Dim dateWidth = purchasesDateTime.Width
            ComboBox5.SetBounds(right - periodWidth, 54, periodWidth, ComboBox5.Height)
            purchasesDateTime.SetBounds(right - periodWidth - 10 - dateWidth, 54, dateWidth, purchasesDateTime.Height)
            purchasesSearchBar.SetBounds(inset, 52, Math.Max(1, purchasesDateTime.Left - inset * 2), 28)
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
            grid.Columns(idColumn).HeaderText = "TransactionID"
            grid.Columns(dateColumn).HeaderText = "SaleDate"
            grid.Columns(customerColumn).HeaderText = "Customer"
            grid.Columns(cashierColumn).HeaderText = "Cashier"
            grid.Columns(typeColumn).HeaderText = "SaleType"
            grid.Columns(totalColumn).HeaderText = "Total"
            grid.Columns(statusColumn).HeaderText = "Status"
            generatedLabel.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt") & " | By: " & CurrentFullName

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
        salesReportDetailsPanel.Margin = Padding.Empty
        transactionDetailsTitle.Dock = DockStyle.Top
        transactionDetailsTitle.AutoSize = False
        transactionDetailsTitle.Height = 34
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
            .Margin = New Padding(14, 0, 14, 14),
            .Padding = Padding.Empty
        }
        salesReportBody.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 225))
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
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 66))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 23))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 142))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 9))
        salesReportPageLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        ComboBox4.Visible = False
        salesTransactionFilter.Visible = False
        AddSalesReportFilterButtonGroup(panelRetailSalesReport, {"Daily", "Monthly", "Yearly"}, salesReportPeriodButtons, 18, 66, AddressOf SelectSalesReportPeriod, 94, 29)
        AddSalesReportFilterButtonGroup(panelRetailSalesReport, {"All", "Retail", "Wholesale"}, salesReportModeButtons, 18, 101, AddressOf SelectSalesReportMode, 82, 25)
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
        panelRetailSalesReport.Margin = New Padding(14, 0, 14, 0)
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
        Dim periodX = 18
        For Each pair In salesReportPeriodButtons
            pair.Value.SetBounds(periodX, 66, 94, 29)
            periodX += pair.Value.Width + 8
        Next

        Dim modeX = 18
        For Each pair In salesReportModeButtons
            pair.Value.SetBounds(modeX, 101, 82, 25)
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

    Private Sub ShowReportPanel(reportPanel As Panel)
        LayoutAllReportDetails()
        HideAllPanels()
        reportPanel.Visible = True
        reportPanel.BringToFront()
        hdrDashboard.Text = "Reports"
        hdrAccounts.Text = "Reports"
        Label6.Text = "Reports"
        titleLabel.Text = "Reports"
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
            button.BackColor = If(isActive, Color.FromArgb(20, 100, 60), Color.White)
            button.ForeColor = If(isActive, Color.White, Color.FromArgb(20, 100, 60))
        Next
    End Sub

    Private Sub LoadSalesDashboardStats()
        Try
            Dim summary = New SalesRepository().GetSummary()
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
                    If expiry < Date.Today Then
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
            TextBox20.Text = "P 0.00"
            expensesRecorded.Text = "Expense data is not stored yet"
        Catch ex As Exception
            MessageBox.Show("Could not load inventory totals. " & ex.Message, "Inventory dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LogoutBtn_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
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

                accountsDataGrid.DataSource = table
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
        If accountsDataGrid.Columns(e.ColumnIndex).Name <> "accountEnableDisable" Then Return
        If accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value Is Nothing Then Return

        Dim statusValue = accountsDataGrid.Rows(e.RowIndex).Cells("accountStatus").Value
        Dim isActive = statusValue.ToString() = "Active"

        e.PaintBackground(e.CellBounds, True)

        Dim switchWidth = 44.0F
        Dim switchHeight = 22
        Dim switchLeft = e.CellBounds.Left + (e.CellBounds.Width - switchWidth) \ 2 - 20
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

        Dim statusText = If(isActive, "Active", "Disabled")
        Dim textColor = If(isActive, Color.ForestGreen, Color.Firebrick)
        TextRenderer.DrawText(e.Graphics, statusText, accountsDataGrid.Font, New Point(switchLeft + switchWidth + 7, switchTop + 2), textColor)

        e.Handled = True
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
