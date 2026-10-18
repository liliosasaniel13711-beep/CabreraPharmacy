Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Drawing.Printing
Imports System.Threading.Tasks

Public Class SuperAdmin

    Private printGrid As DataGridView
    Private printTitle As String
    Private printRowIndex As Integer
    Private printColumns As DataGridViewColumn()

    ' --- 1. INITIALIZATION & NAVIGATION ---
    Private Sub SuperAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
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
        If Not accountsDataGrid.Columns.Contains("accountActivity") Then
            accountsDataGrid.Columns.Add(New DataGridViewButtonColumn With {
                .Name = "accountActivity",
                .HeaderText = "",
                .Text = "Activity",
                .UseColumnTextForButtonValue = True,
                .MinimumWidth = 6
            })
        End If

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
        Dim headerHeight = 100
        Dim toolbarTop = headerHeight + 14
        Dim toolbarHeight = 100
        Dim gridTop = toolbarTop + toolbarHeight + 14
        Dim contentWidth = Math.Max(1, reportPage.ClientSize.Width - horizontalPadding * 2)

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
        Await ShowSalesReport(pnlRetailSalesReport, retailDataGrid, "Retail",
                        "retailTransactionID", "retailDate", "retailCustomer", "retailCashier", "retailType", "retailTotal", "retailStatus", retailGenerated)
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
                                generatedLabel As TextBox) As Task
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
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            grid.DataSource = Await Task.Run(Function() New SalesRepository().GetTransactionReport(saleMode))
            grid.Columns(dateColumn).DefaultCellStyle.Format = "g"
            grid.Columns(totalColumn).DefaultCellStyle.Format = "'P' #,##0.00"
            generatedLabel.Text = "Generated: " & DateTime.Now.ToString("MMM dd, yyyy h:mm tt") & " | By: " & CurrentFullName

            ShowReportPanel(reportPanel)
        Catch ex As Exception
            MessageBox.Show("Could not load sales report. " & ex.Message, "Sales reports", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                Using adapter As New MySqlDataAdapter("SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, IF(is_active, 'Active', 'Inactive') AS Status, last_login_date AS LastLogin FROM users ORDER BY username", connection)
                    adapter.Fill(table)
                End Using

                ' Bind the SQL data to your visual designer columns
                accountsDataGrid.AutoGenerateColumns = False
                accountUserID.DataPropertyName = "UserID"
                accountFullName.DataPropertyName = "FullName"
                accountUsername.DataPropertyName = "Username"
                accountRole.DataPropertyName = "Role"
                accountStatus.DataPropertyName = "Status"
                accountLastLogin.DataPropertyName = "LastLogin"

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
            Case "accountActivity"
                ShowUserActivity(userId.Value)
        End Select
    End Sub

    Private Sub ShowUserActivity(userId As Integer)
        Try
            Dim activityTable As New DataTable()
            Using connection = GetConnection()
                connection.Open()
                Const query = "SELECT user_id AS UserID, username AS Username, full_name AS FullName, role AS Role, activity AS Activity, created_at AS EventTime FROM user_activity WHERE user_id = @userId ORDER BY created_at DESC, activity_id DESC"
                Using adapter As New MySqlDataAdapter(query, connection)
                    adapter.SelectCommand.Parameters.AddWithValue("@userId", userId)
                    adapter.Fill(activityTable)
                End Using
            End Using

            Dim activityGrid As New DataGridView With {
                .Dock = DockStyle.Fill,
                .ReadOnly = True,
                .AutoGenerateColumns = True,
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                .DataSource = activityTable,
                .AllowUserToAddRows = False,
                .AllowUserToDeleteRows = False
            }
            Dim dialog As New Form With {
                .Text = "User Activity - ID " & userId.ToString(),
                .StartPosition = FormStartPosition.CenterParent,
                .Size = New Size(900, 500),
                .MinimumSize = New Size(650, 350),
                .BackColor = Color.White
            }
            dialog.Controls.Add(activityGrid)
            dialog.ShowDialog(Me)
        Catch ex As Exception
            MessageBox.Show("Could not load activity for user " & userId.ToString() & ". " & ex.Message,
                            "User activity", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
        Public LastLoginText As String
        Public CreatedAtText As String
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
                        .IsActive = Convert.ToBoolean(reader("is_active")),
                        .LastLoginText = ReadText(reader, "last_login_date"),
                        .CreatedAtText = ReadText(reader, "created_at")
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
