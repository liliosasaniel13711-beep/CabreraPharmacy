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
    Private inventoryParentFilterButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private inventorySelectedParentCategory As String = "All"
    Private priceManagerProducts As DataTable
    Private priceManagerCategories As DataTable
    Private priceManagerSearchBox As TextBox
    Private priceManagerCategoryFilter As ComboBox
    Private priceManagerProductList As FlowLayoutPanel
    Private priceManagerSelectedParentCategory As String = "All"
    Private priceManagerUpdatingCategoryFilter As Boolean
    Private priceManagerParentFilterButtons As Dictionary(Of String, Button)
    Private salesReportPeriod As String = "Daily"
    Private salesReportType As String = "All"
    Private salesReportPeriodButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private salesReportTypeButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private salesReportDatePicker As DateTimePicker
    Private salesReportMonthCombo As ComboBox
    Private salesReportYearCombo As ComboBox
    Private salesReportFilterPanel As Panel
    Private salesReportGrid As DataGridView
    ' CHANGE: Holds the no-transactions illustration shown over the report grid.
    Private salesReportEmptyState As Panel
    Private salesReportTotalLabel As Label
    Private salesReportDetailsLabel As Label
    Private empSalesReportPeriod As String = "Daily"
    Private empSalesReportPeriodButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private empSalesReportDatePicker As DateTimePicker
    Private empSalesReportMonthCombo As ComboBox
    Private empSalesReportYearCombo As ComboBox
    Private empSalesReportFilterPanel As Panel
    Private empSalesReportTotalLabel As Label
    Private empSalesReportEmptyState As Panel
    Private WithEvents dgvExpenses As DataGridView
    Private expReportPeriod As String = "Daily"
    Private expReportPeriodButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private expReportDatePicker As DateTimePicker
    Private expReportMonthCombo As ComboBox
    Private expReportYearCombo As ComboBox
    Private expReportFilterPanel As Panel
    Private expReportTotalLabel As Label
    Private expReportEmptyState As Panel
    Private trendButtons As New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
    Private trendLabels As New List(Of String)
    Private trendValues As New List(Of Decimal)
    Private dashboardRevenue As Decimal
    Private dashboardLowStock As Integer
    Private dashboardExpired As Integer
    Private dashboardNearExpiry As Integer
    Private dashboardTodayTransactions As Integer

    Private Sub Admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        ' Welcome logic
        ' Make sure CurrentFullName is declared globally in a module (like SessionModule)
        welcomeadmin_label.Text = CurrentFullName
        userRoleLabel.Text = If(String.IsNullOrWhiteSpace(CurrentRole), "Admin", CurrentRole)
        ApplyRoundedRegion(userAvatar, userAvatar.Width \ 2)
        titleLabel.AutoSize = False
        titleLabel.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
        titleLabel.SetBounds(16, 27, 400, 36)
        subtitleLabel.AutoSize = False
        subtitleLabel.SetBounds(24, 65, 520, 16)

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

        InitializeDashboardUi()
        InitializeInventoryUi()
        InitializePriceManagerUi()
        InitializeReportsUi()
        UiSelectionStyler.ApplyToForm(Me)
        ShowView("Dashboard")
        LoadSalesTrend("7 Days")
    End Sub

    ' Keep the user info panel pinned to the top-right corner
    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20,
                                       Math.Max(0, (headerPanel.ClientSize.Height - userPanel.Height) \ 2))
    End Sub

    Private Sub UserAvatar_Paint(sender As Object, e As PaintEventArgs) Handles userAvatar.Paint
        Dim graphics = e.Graphics
        graphics.SmoothingMode = SmoothingMode.AntiAlias

        Dim scale = Math.Min(userAvatar.ClientSize.Width, userAvatar.ClientSize.Height) / 38.0F
        If scale <= 0 Then Return

        Dim headSize = 10.0F * scale
        Dim centerX = userAvatar.ClientSize.Width / 2.0F
        Using iconBrush As New SolidBrush(Color.FromArgb(20, 100, 60))
            graphics.FillEllipse(iconBrush,
                                 centerX - headSize / 2.0F,
                                 6.0F * scale,
                                 headSize,
                                 headSize)

            Using shoulders As New GraphicsPath()
                Dim bodyLeft = centerX - 12.0F * scale
                Dim bodyTop = 18.0F * scale
                Dim bodyWidth = 24.0F * scale
                Dim bodyHeight = 22.0F * scale
                shoulders.AddArc(bodyLeft, bodyTop, bodyWidth, bodyHeight, 180.0F, 180.0F)
                shoulders.AddLine(bodyLeft + bodyWidth, bodyTop + bodyHeight / 2.0F,
                                  bodyLeft + bodyWidth, userAvatar.ClientSize.Height - 4.0F * scale)
                shoulders.AddLine(bodyLeft + bodyWidth, userAvatar.ClientSize.Height - 4.0F * scale,
                                  bodyLeft, userAvatar.ClientSize.Height - 4.0F * scale)
                shoulders.AddLine(bodyLeft, userAvatar.ClientSize.Height - 4.0F * scale,
                                  bodyLeft, bodyTop + bodyHeight / 2.0F)
                shoulders.CloseFigure()
                graphics.FillPath(iconBrush, shoulders)
            End Using
        End Using

        Using outline As New Pen(Color.FromArgb(176, 222, 198), 1.0F)
            graphics.DrawEllipse(outline, 0.5F, 0.5F,
                                 userAvatar.ClientSize.Width - 1.0F,
                                 userAvatar.ClientSize.Height - 1.0F)
        End Using
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs) Handles navDashboard.Click, navPriceManager.Click, navInventory.Click, navReports.Click
        Dim btn = TryCast(sender, Button)
        If btn IsNot Nothing Then
            ShowView(CStr(btn.Tag))
        End If
    End Sub

    Private Sub UpdateDashboardCounts()
        inventoryAlertLowStockCount.Text = dashboardLowStock.ToString(CultureInfo.InvariantCulture)
        inventoryAlertNearExpiryCount.Text = dashboardNearExpiry.ToString(CultureInfo.InvariantCulture)
        inventoryAlertExpiredCount.Text = dashboardExpired.ToString(CultureInfo.InvariantCulture)
        quickOverviewRestockValue.Text = dashboardLowStock.ToString(CultureInfo.InvariantCulture) & " products"
        quickOverviewExpiryValue.Text = dashboardNearExpiry.ToString(CultureInfo.InvariantCulture) & " products"
        quickOverviewTransactionsValue.Text = dashboardTodayTransactions.ToString(CultureInfo.InvariantCulture)
        salesTrendPanel?.Invalidate()
        salesTrendPlot?.Invalidate()
        revenuePanel?.Invalidate()
    End Sub

    Private Sub logoutadmin_button_Click(sender As Object, e As EventArgs) Handles logoutadmin_button.Click
        If Not LogoutConfirmationDialog.Confirm(Me) Then Return
        Me.Close()
    End Sub

    ' Switch the visible view and highlight the active nav button
    Private Sub ShowView(viewName As String)
        bodyPanel.Padding = If(String.Equals(viewName, "Dashboard", StringComparison.Ordinal),
                               New Padding(20, 0, 20, 12),
                               New Padding(20, 12, 20, 12))
        For Each kvp In contentPanels
            kvp.Value.Visible = (kvp.Key = viewName)
        Next

        For Each btn In navButtons
            Dim isActive As Boolean = (CStr(btn.Tag) = viewName)
            btn.BackColor = If(isActive, Color.White, darkGreen)
            btn.ForeColor = If(isActive, darkGreen, Color.White)
            btn.UseVisualStyleBackColor = False
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
                Return "Overview of Cabrera's Drugstore and Medical Supplies"
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

    Private Sub InitializeDashboardUi()
        trendButtons.Clear()
        trendButtons.Add("7 Days", trend7DaysButton)
        trendButtons.Add("This Month", trendThisMonthButton)
        trendButtons.Add("This Year", trendThisYearButton)
        AddHandler trend7DaysButton.Click, AddressOf SalesTrendFilter_Click
        AddHandler trendThisMonthButton.Click, AddressOf SalesTrendFilter_Click
        AddHandler trendThisYearButton.Click, AddressOf SalesTrendFilter_Click
        AddHandler inventoryViewAllLink.Click, AddressOf InventoryViewAllLink_Click
        AddHandler salesTrendPlot.Paint, AddressOf SalesTrendPlot_Paint
        AddHandler revenuePanel.Paint, AddressOf RevenuePanel_Paint
        AddHandler dashboardView.Resize, AddressOf DashboardView_Resize
        AddHandler inventoryAlertsPanel.Resize, AddressOf DashboardPanels_Resize
        AddHandler quickOverviewPanel.Resize, AddressOf DashboardPanels_Resize
        AddHandler revenuePanel.Resize, AddressOf RevenuePanel_Resize
        dashboardDateLabel.Text = Date.Today.ToString("dddd, MMMM d, yyyy", CultureInfo.CurrentCulture)
        summaryArea.RowStyles(0).SizeType = SizeType.Absolute
        summaryArea.RowStyles(0).Height = 46
        dashboardDateLabel.Dock = DockStyle.None
        dashboardDateLabel.Margin = New Padding(8, 2, 0, 0)
        dashboardDateLabel.BackColor = Color.FromArgb(226, 238, 231)
        dashboardDateLabel.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        dashboardDateLabel.TextAlign = ContentAlignment.MiddleCenter
        dashboardView.Padding = Padding.Empty
        For Each control As Control In {card1, card2, card3, card4, card5, card6, card7, card8,
                                       dashboardDateLabel, salesTrendPanel, revenuePanel,
                                       inventoryAlertsPanel, quickOverviewPanel}
            MoveDashboardControlToSurface(control)
        Next
        dashboardLayout.Visible = False
        quickOverviewRestock.Text = "Products to Restock"
        quickOverviewExpiry.Text = "Expiring Products"
        quickOverviewTransactions.Text = "Today's Transactions"
        LayoutAdminDashboardSurface()
        RefreshDashboardRegions()
    End Sub

    Private Sub MoveDashboardControlToSurface(control As Control)
        If control.Parent IsNot Nothing Then control.Parent.Controls.Remove(control)
        dashboardView.Controls.Add(control)
        control.Dock = DockStyle.None
        control.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        control.Visible = True
    End Sub

    Private Sub RevenuePanel_Resize(sender As Object, e As EventArgs)
        revenueNetIncomeLabel.SetBounds(16, Math.Max(146, revenuePanel.ClientSize.Height - 52), Math.Max(1, revenuePanel.ClientSize.Width - 32), 36)
    End Sub

    Private Sub InventoryViewAllLink_Click(sender As Object, e As EventArgs)
        ShowView("Inventory")
    End Sub

    Private Sub ApplyRoundedRegion(control As Control, radius As Integer)
        If control.Width <= 0 OrElse control.Height <= 0 Then Return
        Using path As New GraphicsPath()
            Dim diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height))
            Dim bounds = New Rectangle(0, 0, control.Width, control.Height)
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90)
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90)
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90)
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90)
            path.CloseFigure()
            Dim newRegion As New Region(path)
            Dim oldRegion = control.Region
            control.Region = newRegion
            oldRegion?.Dispose()
        End Using
    End Sub

    Private Sub StyleReportBackButton(button As Button)
        button.Text = String.Empty
        button.AccessibleName = "Back to Reports"
        button.TextAlign = ContentAlignment.MiddleCenter
        button.BackColor = darkGreen
        button.ForeColor = Color.White
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderSize = 0
        button.FlatAppearance.MouseOverBackColor = activeGreen
        AddHandler button.Paint,
            Sub(sender, e)
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                Dim centerX = button.ClientSize.Width \ 2
                Dim centerY = button.ClientSize.Height \ 2
                Using arrowPen As New Pen(Color.White, 2.0F)
                    arrowPen.StartCap = LineCap.Round
                    arrowPen.EndCap = LineCap.Round
                    e.Graphics.DrawLine(arrowPen, centerX + 7, centerY, centerX - 7, centerY)
                    e.Graphics.DrawLine(arrowPen, centerX - 7, centerY, centerX + 1, centerY - 6)
                    e.Graphics.DrawLine(arrowPen, centerX - 7, centerY, centerX + 1, centerY + 6)
                End Using
            End Sub
        AddHandler button.Resize, Sub(sender, e) ApplyRoundedRegion(button, 8)
        ApplyRoundedRegion(button, 8)
    End Sub

    Private Sub StyleReportActionButton(button As Button)
        AddHandler button.Resize, Sub(sender, e) ApplyRoundedRegion(button, 8)
        ApplyRoundedRegion(button, 8)
    End Sub

    Private Sub FormatSummaryCard(card As Panel)
        Dim title = TryCast(card.Controls.OfType(Of Label)().FirstOrDefault(Function(control) control.Name.EndsWith("Title", StringComparison.Ordinal)), Label)
        Dim value = TryCast(card.Controls.OfType(Of Label)().FirstOrDefault(Function(control) control.Name.EndsWith("Value", StringComparison.Ordinal)), Label)
        Dim footer = TryCast(card.Controls.OfType(Of Label)().FirstOrDefault(Function(control) control.Name.EndsWith("Footer", StringComparison.Ordinal)), Label)
        If title Is Nothing OrElse value Is Nothing OrElse footer Is Nothing Then Return

        Dim width = Math.Max(1, card.ClientSize.Width - 32)
        title.SetBounds(16, 12, width, 22)
        value.SetBounds(16, 40, width, 32)
        footer.SetBounds(16, 74, width, 18)
    End Sub

    Private Sub RefreshDashboardRegions()
        ApplyRoundedRegion(dashboardDateLabel, 10)
        For Each card As Panel In {card1, card2, card3, card4, card5, card6, card7, card8}
            ApplyRoundedRegion(card, 12)
        Next
        For Each panel As Panel In {salesTrendPanel, revenuePanel, inventoryAlertsPanel, quickOverviewPanel}
            ApplyRoundedRegion(panel, 14)
        Next
        For Each alertCard As Panel In {inventoryAlertLowStock, inventoryAlertNearExpiry, inventoryAlertExpired}
            ApplyRoundedRegion(alertCard, 10)
        Next
    End Sub

    Private Sub UpdateQuickOverviewRows()
        Dim overviewRows = {quickOverviewRestock, quickOverviewExpiry, quickOverviewTransactions}
        Dim values = {quickOverviewRestockValue, quickOverviewExpiryValue, quickOverviewTransactionsValue}
        Dim icons = {quickOverviewRestockIcon, quickOverviewExpiryIcon, quickOverviewTransactionsIcon}
        For index = 0 To overviewRows.Length - 1
            Dim availableWidth = Math.Max(1, quickOverviewPanel.ClientSize.Width - 44)
            overviewRows(index).SetBounds(44, 38 + index * 28, Math.Max(1, availableWidth - 126), 24)
            values(index).SetBounds(quickOverviewPanel.ClientSize.Width - 126, 38 + index * 28, 110, 24)
            icons(index).SetBounds(16, 38 + index * 28, 24, 24)
        Next
        quickOverviewSeparator1.SetBounds(16, 64, Math.Max(1, quickOverviewPanel.ClientSize.Width - 32), 1)
        quickOverviewSeparator2.SetBounds(16, 92, Math.Max(1, quickOverviewPanel.ClientSize.Width - 32), 1)
    End Sub


    Private Sub DashboardView_Resize(sender As Object, e As EventArgs)
        LayoutAdminDashboardSurface()
        For Each card As Panel In {card1, card2, card3, card4, card5, card6, card7, card8}
            FormatSummaryCard(card)
        Next
        ResizeDashboardPanels()
        RefreshDashboardRegions()
    End Sub

    Private Sub LayoutAdminDashboardSurface()
        If dashboardView.ClientSize.Width <= 0 OrElse dashboardView.ClientSize.Height <= 0 Then Return

        Dim left = 0
        Dim rightMargin = 0
        Dim cardInset = 8
        Dim dateTop = 10
        Dim firstCardsTop = dateTop + 36
        Dim summaryHeight = 118
        Dim rowGap = 12
        Dim chartHeight = Math.Max(220, CInt(Me.ClientSize.Height * 0.3))
        Dim bottomHeight = Math.Max(155, CInt(Me.ClientSize.Height * 0.18))
        Dim availableWidth = Math.Max(1, dashboardView.ClientSize.Width - left - rightMargin)
        Dim secondCardsTop = firstCardsTop + summaryHeight + rowGap
        Dim chartTop = secondCardsTop + summaryHeight + rowGap
        Dim bottomTop = chartTop + chartHeight + rowGap
        Dim requiredHeight = bottomTop + bottomHeight + 20

        dashboardView.AutoScroll = dashboardView.ClientSize.Height < requiredHeight
        dashboardView.AutoScrollMinSize = If(dashboardView.AutoScroll, New Size(availableWidth + left + rightMargin, requiredHeight), Size.Empty)
        dashboardDateLabel.SetBounds(left + cardInset, dateTop, 244, 28)

        Dim cellWidth = availableWidth / 4.0R
        Dim cards = {card1, card2, card3, card4, card5, card6, card7, card8}
        For index = 0 To cards.Length - 1
            Dim row = index \ 4
            Dim column = index Mod 4
            Dim rowTop = If(row = 0, firstCardsTop, secondCardsTop)
            Dim cardX = left + CInt(Math.Round(column * cellWidth)) + cardInset
            Dim cardWidth = Math.Max(1, CInt(Math.Round(cellWidth)) - cardInset * 2)
            cards(index).Margin = Padding.Empty
            cards(index).SetBounds(cardX, rowTop + cardInset, cardWidth, summaryHeight - cardInset * 2)
        Next

        Dim chartLeftWidth = CInt(availableWidth * 0.6) - 10
        Dim chartRightWidth = Math.Max(1, availableWidth - chartLeftWidth - 10)
        salesTrendPanel.SetBounds(left, chartTop, chartLeftWidth, chartHeight)
        revenuePanel.SetBounds(left + chartLeftWidth + 10, chartTop, chartRightWidth, chartHeight)
        inventoryAlertsPanel.SetBounds(left, bottomTop, chartLeftWidth, bottomHeight)
        quickOverviewPanel.SetBounds(left + chartLeftWidth + 10, bottomTop, chartRightWidth, bottomHeight)

        salesTrendPlot.SetBounds(16, 58, Math.Max(1, salesTrendPanel.ClientSize.Width - 32), Math.Max(1, salesTrendPanel.ClientSize.Height - 78))
        trendFiltersPanel.Top = 8
        trendFiltersPanel.Left = salesTrendPanel.ClientSize.Width - trendFiltersPanel.Width - 16
        RevenuePanel_Resize(revenuePanel, EventArgs.Empty)
        ResizeDashboardPanels()
    End Sub

    Private Sub DashboardPanels_Resize(sender As Object, e As EventArgs)
        ResizeDashboardPanels()
    End Sub

    Private Sub SalesTrendFilter_Click(sender As Object, e As EventArgs)
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return
        LoadSalesTrend(Convert.ToString(button.Tag, CultureInfo.InvariantCulture))
    End Sub

    Private Sub LoadSalesTrend(period As String)
        Dim today = Date.Today
        Dim startDate As DateTime
        Dim endDate = today.AddDays(1)
        Select Case period
            Case "This Month"
                startDate = New DateTime(today.Year, today.Month, 1)
            Case "This Year"
                startDate = New DateTime(today.Year, 1, 1)
            Case Else
                period = "7 Days"
                startDate = today.AddDays(-6)
        End Select

        trendLabels.Clear()
        trendValues.Clear()
        dashboardTodayTransactions = 0
        Try
            Dim transactions = New SalesRepository().GetTransactionReport(Nothing, Nothing, startDate, endDate)
            dashboardTodayTransactions = transactions.AsEnumerable().Count(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")).Date = today)
            UpdateDashboardCounts()
            If period = "This Year" Then
                For monthOffset = 0 To 11
                    Dim monthStart = New DateTime(today.Year, 1, 1).AddMonths(monthOffset)
                    Dim monthEnd = monthStart.AddMonths(1)
                    trendLabels.Add(monthStart.ToString("MMM", CultureInfo.CurrentCulture))
                    trendValues.Add(transactions.AsEnumerable().Where(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")) >= monthStart AndAlso CDate(row("SaleDate")) < monthEnd).Sum(Function(row) Convert.ToDecimal(row("Total"), CultureInfo.InvariantCulture)))
                Next
            Else
                Dim dayCount = If(period = "This Month", Date.DaysInMonth(today.Year, today.Month), 7)
                startDate = If(period = "This Month", New DateTime(today.Year, today.Month, 1), today.AddDays(-6))
                For dayOffset = 0 To dayCount - 1
                    Dim day = startDate.AddDays(dayOffset)
                    trendLabels.Add(If(period = "This Month", day.Day.ToString(CultureInfo.InvariantCulture), day.ToString("ddd", CultureInfo.CurrentCulture)))
                    trendValues.Add(transactions.AsEnumerable().Where(Function(row) Not row.IsNull("SaleDate") AndAlso CDate(row("SaleDate")).Date = day.Date).Sum(Function(row) Convert.ToDecimal(row("Total"), CultureInfo.InvariantCulture)))
                Next
            End If
        Catch ex As Exception
            trendLabels.Clear()
            trendValues.Clear()
            dashboardTodayTransactions = 0
            UpdateDashboardCounts()
        End Try

        For Each item In trendButtons
            Dim active = String.Equals(item.Key, period, StringComparison.OrdinalIgnoreCase)
            item.Value.BackColor = If(active, darkGreen, Color.White)
            item.Value.ForeColor = If(active, Color.White, darkGreen)
            item.Value.FlatAppearance.BorderSize = If(active, 0, 1)
        Next
        salesTrendPanel?.Controls("salesTrendPlot")?.Invalidate()
        revenuePanel?.Invalidate()
    End Sub

    Private Sub SalesTrendPlot_Paint(sender As Object, e As PaintEventArgs)
        Dim plot = TryCast(sender, Panel)
        If plot Is Nothing Then Return
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        Dim bounds = plot.ClientRectangle
        Dim chart = New Rectangle(bounds.Left + 56, bounds.Top + 10, Math.Max(1, bounds.Width - 72), Math.Max(1, bounds.Height - 38))
        Using gridPen As New Pen(Color.FromArgb(235, 235, 235)), axisFont As New Font("Segoe UI", 7.0F), valueFont As New Font("Segoe UI", 6.5F), textBrush As New SolidBrush(Color.Gray), salesBrush As New SolidBrush(darkGreen)
            For index = 0 To 3
                Dim y = chart.Top + CInt(chart.Height * index / 3.0F)
                e.Graphics.DrawLine(gridPen, chart.Left, y, chart.Right, y)
                Dim scaleValue = If(trendValues.Count = 0, 0D, CDec(trendValues.Max()) * (3 - index) / 3D)
                e.Graphics.DrawString("₱" & scaleValue.ToString("N0", CultureInfo.CurrentCulture), axisFont, textBrush, bounds.Left, y - 7)
            Next
            If trendValues.Count > 0 Then
                Dim maximum = Math.Max(1D, CDbl(trendValues.Max()))
                Dim groupWidth = CSng(chart.Width) / trendValues.Count
                Dim barWidth = Math.Max(2.0F, Math.Min(18.0F, groupWidth * 0.28F))
                For index = 0 To trendValues.Count - 1
                    Dim groupCenter = chart.Left + CSng(index + 0.5F) * groupWidth
                    Dim salesHeight = CInt(CDbl(trendValues(index)) / maximum * chart.Height)
                    Dim salesRect As New RectangleF(groupCenter - barWidth - 1, chart.Bottom - salesHeight, barWidth, Math.Max(0, salesHeight))
                    e.Graphics.FillRectangle(salesBrush, salesRect)
                    If salesHeight > 18 AndAlso (trendValues.Count <= 14 OrElse index Mod 5 = 0) Then
                        e.Graphics.DrawString("₱" & trendValues(index).ToString("N0", CultureInfo.CurrentCulture), valueFont, textBrush, salesRect.X - 8, salesRect.Y - 12)
                    End If
                    If index Mod Math.Max(1, trendValues.Count \ 10) = 0 OrElse index = trendValues.Count - 1 Then
                        Dim label = trendLabels(index)
                        Dim labelSize = e.Graphics.MeasureString(label, axisFont)
                        e.Graphics.DrawString(label, axisFont, textBrush, groupCenter - labelSize.Width / 2.0F, chart.Bottom + 5)
                    End If
                Next
            End If
        End Using
    End Sub

    Private Sub RevenuePanel_Paint(sender As Object, e As PaintEventArgs)
        Dim panel = TryCast(sender, Panel)
        If panel Is Nothing Then Return
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
        Dim center = New Point(86, Math.Min(111, panel.ClientSize.Height \ 2 - 9))
        Using unavailablePen As New Pen(Color.FromArgb(225, 230, 226), 15.0F), textFont As New Font("Segoe UI", 7.5F), boldFont As New Font("Segoe UI", 8.0F, FontStyle.Bold), brush As New SolidBrush(Color.FromArgb(25, 25, 25)), grayBrush As New SolidBrush(Color.DimGray)
            Dim ringBounds As New Rectangle(center.X - 34, center.Y - 34, 68, 68)
            Dim sales = dashboardRevenue
            e.Graphics.DrawArc(unavailablePen, ringBounds, -90, 359)
            Dim centerLabel = "N/A"
            Dim centerSize = e.Graphics.MeasureString(centerLabel, boldFont)
            e.Graphics.DrawString(centerLabel, boldFont, grayBrush, center.X - centerSize.Width / 2.0F, center.Y - 6)
            e.Graphics.DrawString("Revenue", textFont, grayBrush, 140, 62)
            e.Graphics.DrawString("₱" & sales.ToString("N2", CultureInfo.CurrentCulture) & " today", boldFont, brush, 140, 77)
            e.Graphics.DrawString("Expenses", textFont, grayBrush, 140, 103)
            e.Graphics.DrawString("Unavailable — no expense records", textFont, grayBrush, 140, 118)
        End Using
    End Sub

    Private Sub ResizeDashboardPanels()
        If dashboardLayout Is Nothing OrElse inventoryAlertsPanel Is Nothing OrElse quickOverviewPanel Is Nothing Then Return
        Dim alertWidth = Math.Max(1, (inventoryAlertsPanel.ClientSize.Width - 52) \ 3)
        Dim alertHeight = Math.Max(54, inventoryAlertsPanel.ClientSize.Height - 54)
        inventoryAlertLowStock.SetBounds(16, 42, alertWidth, alertHeight)
        inventoryAlertNearExpiry.SetBounds(26 + alertWidth, 42, alertWidth, alertHeight)
        inventoryAlertExpired.SetBounds(36 + alertWidth * 2, 42, alertWidth, alertHeight)
        inventoryViewAllLink.Left = inventoryAlertsPanel.ClientSize.Width - inventoryViewAllLink.Width - 16
        UpdateQuickOverviewRows()
        RefreshDashboardRegions()
    End Sub

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
        AddHandler heading.Resize, Sub(sender, e) ApplyRoundedRegion(heading, 12)
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
        ApplyRoundedRegion(heading, 12)
    End Sub

    Private Sub ReportsList_Resize(sender As Object, e As EventArgs)
        Dim reportList = TryCast(sender, FlowLayoutPanel)
        If reportList Is Nothing Then Return
        Dim cardWidth = Math.Max(300, (reportList.ClientSize.Width - 30) \ 2)
        For Each reportCard As Control In reportList.Controls
            reportCard.Width = cardWidth
            If reportCard.Controls.Count > 1 Then reportCard.Controls(1).Width = cardWidth - 32
            ApplyRoundedRegion(reportCard, 12)
            If reportCard.Controls.Count > 2 Then ApplyRoundedRegion(reportCard.Controls(2), 8)
        Next
    End Sub

    Private Function CreateReportCard(title As String, description As String, availableWidth As Integer) As Panel
        Dim cardWidth = If(availableWidth > 0, Math.Max(300, (availableWidth - 30) \ 2), 440)
        Dim card As New Panel With {.Width = cardWidth, .Height = 136, .BackColor = Color.White, .Margin = New Padding(0, 0, 14, 14), .Padding = New Padding(16), .BorderStyle = BorderStyle.None}
        Dim titleLabel As New Label With {.Text = title, .AutoSize = True, .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 45, 40), .Location = New Point(16, 14)}
        Dim descriptionLabel As New Label With {.Text = description, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.Gray, .Location = New Point(16, 44), .Size = New Size(card.Width - 32, 36), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
        Dim viewButton As New Button With {.Text = "View Report", .Size = New Size(132, 34), .Location = New Point(16, 88), .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        viewButton.FlatAppearance.BorderSize = 0
        card.Controls.Add(titleLabel)
        card.Controls.Add(descriptionLabel)
        card.Controls.Add(viewButton)
        AddHandler card.Resize, Sub(sender, e)
                                    ApplyRoundedRegion(card, 12)
                                    ApplyRoundedRegion(viewButton, 8)
                                End Sub
        ApplyRoundedRegion(card, 12)
        ApplyRoundedRegion(viewButton, 8)

        ' --- NEW: Wired up the new reports ---
        If String.Equals(title, "Sales Report", StringComparison.Ordinal) Then
            AddHandler viewButton.Click, Sub(sender, e) ShowSalesReport()
        ElseIf String.Equals(title, "Employee Sales Report", StringComparison.Ordinal) Then
            AddHandler viewButton.Click, Sub(sender, e) ShowEmployeeSalesReport()
        ElseIf String.Equals(title, "Expenses Report", StringComparison.Ordinal) Then
            AddHandler viewButton.Click, Sub(sender, e) ShowExpenseReport()
        End If

        Return card
    End Function

    ' CHANGE: Reworked the sales report banner and body to match the reference image.
    Private Sub ShowSalesReport()
        reportsView.Controls.Clear()
        reportsView.Padding = New Padding(14)
        reportsView.BackColor = lightGray

        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = lightGray, .Margin = New Padding(0)}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 190))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim header As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12)}
        AddHandler header.Resize, Sub(sender, e) ApplyRoundedRegion(header, 16)
        AddHandler header.Paint, Sub(sender, e)
                                     Using brush As New LinearGradientBrush(header.ClientRectangle, Color.White, Color.FromArgb(214, 246, 230), 0.0F)
                                         e.Graphics.FillRectangle(brush, header.ClientRectangle)
                                     End Using
                                 End Sub

        header.Controls.Add(New Label With {.Text = "Sales Report", .AutoSize = True, .Font = New Font("Segoe UI", 17.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 30, 55), .Location = New Point(76, 18)})
        header.Controls.Add(New Label With {.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt", CultureInfo.CurrentCulture) & "  |  By: " & CurrentFullName, .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(90, 105, 125), .Location = New Point(78, 51)})

        Dim backButton As New Button With {.Text = String.Empty, .Size = New Size(42, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Left, .BackColor = Color.White, .ForeColor = darkGreen, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
        backButton.Location = New Point(18, 18)
        StyleReportBackButton(backButton)
        AddHandler backButton.Click, Sub(sender, e) InitializeReportsUi()
        header.Controls.Add(backButton)

        Dim printReportButton As New Button With {.Text = "▣  Print Report", .Size = New Size(150, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        printReportButton.FlatAppearance.BorderSize = 0
        StyleReportActionButton(printReportButton)
        printReportButton.Location = New Point(header.ClientSize.Width - printReportButton.Width - 18, 18)
        AddHandler header.Resize, Sub(sender, e) printReportButton.Left = header.ClientSize.Width - printReportButton.Width - 18
        AddHandler printReportButton.Click, AddressOf PrintSalesReport
        header.Controls.Add(printReportButton)

        ' CHANGE: Period and sale type filters appear in separate rows.
        salesReportPeriodButtons.Clear()
        salesReportTypeButtons.Clear()
        AddSalesReportButtonGroup(header, {"Daily", "Monthly", "Yearly"}, salesReportPeriodButtons, 76, 88, AddressOf SelectSalesReportPeriod, 100, 38)
        AddSalesReportButtonGroup(header, {"All", "Retail", "Wholesale"}, salesReportTypeButtons, 76, 135, AddressOf SelectSalesReportType, 100, 34)
        UpdateSalesReportButtonStyles()

        ' CHANGE: Side-by-side date filter and transaction card.
        Dim body As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .BackColor = lightGray, .Margin = New Padding(0)}
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 270))
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        salesReportFilterPanel = New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(14), .Margin = New Padding(0, 0, 12, 0)}
        AddHandler salesReportFilterPanel.Resize, Sub(sender, e) ApplyRoundedRegion(salesReportFilterPanel, 16)
        salesReportDetailsLabel = New Label With {.Text = "▤  Transaction Details", .Dock = DockStyle.Top, .Height = 42, .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(25, 40, 55), .Padding = New Padding(12, 10, 0, 0)}

        Dim reportPanel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(8), .Margin = New Padding(0)}
        AddHandler reportPanel.Resize, Sub(sender, e) ApplyRoundedRegion(reportPanel, 16)

        ' CHANGE: Mint header row, larger rows, and softer selection colors.
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
            .GridColor = Color.FromArgb(225, 235, 230),
            .EnableHeadersVisualStyles = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .ColumnHeadersHeight = 42,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        }
        salesReportGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        salesReportGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        salesReportGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        salesReportGrid.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        salesReportGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        salesReportGrid.DefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        salesReportGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
        salesReportGrid.DefaultCellStyle.SelectionForeColor = Color.Black
        salesReportGrid.RowTemplate.Height = 40
        AddHandler salesReportGrid.CellClick, AddressOf SalesReportGrid_CellClick

        ' CHANGE: Mint-green sales total strip.
        salesReportTotalLabel = New Label With {.Text = "TOTAL SALES: ₱0.00", .Dock = DockStyle.Bottom, .Height = 46, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .BackColor = Color.FromArgb(225, 245, 235), .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 18, 0)}

        ' CHANGE: Empty state is displayed when the selected date/filter has no sales.
        salesReportEmptyState = New Panel With {.BackColor = Color.White, .Visible = False}
        salesReportEmptyState.Controls.Add(New Label With {.Name = "emptyMessage", .Text = "Try selecting a different date or filter.", .Font = New Font("Segoe UI", 10.0F), .ForeColor = Color.FromArgb(40, 50, 70), .TextAlign = ContentAlignment.MiddleCenter})
        salesReportEmptyState.Controls.Add(New Label With {.Name = "emptyTitle", .Text = "No transactions found", .Font = New Font("Segoe UI", 13.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 30, 55), .TextAlign = ContentAlignment.MiddleCenter})
        salesReportEmptyState.Controls.Add(New Label With {.Name = "emptyIcon", .Text = "▤", .Font = New Font("Segoe UI Symbol", 54.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(57, 165, 125), .TextAlign = ContentAlignment.BottomCenter})
        AddHandler salesReportGrid.SizeChanged, Sub(sender, e) PositionSalesReportEmptyState()
        salesReportGrid.Controls.Add(salesReportEmptyState)

        reportPanel.Controls.Add(salesReportGrid)
        reportPanel.Controls.Add(salesReportTotalLabel)
        reportPanel.Controls.Add(salesReportDetailsLabel)
        body.Controls.Add(salesReportFilterPanel, 0, 0)
        body.Controls.Add(reportPanel, 1, 0)
        layout.Controls.Add(header, 0, 0)
        layout.Controls.Add(body, 0, 1)
        reportsView.Controls.Add(layout)

        ConfigureSalesReportFilters()
        ApplyRoundedRegion(header, 16)
        PositionSalesReportEmptyState()
    End Sub

    ' CHANGE: Centers the empty-state message under the table headers.
    Private Sub PositionSalesReportEmptyState()
        If salesReportGrid Is Nothing OrElse salesReportEmptyState Is Nothing Then Return
        Dim stateHeight = Math.Max(0, salesReportGrid.ClientSize.Height - salesReportGrid.ColumnHeadersHeight)
        salesReportEmptyState.SetBounds(0, salesReportGrid.ColumnHeadersHeight, salesReportGrid.ClientSize.Width, stateHeight)
        Dim contentHeight = 176
        Dim top = Math.Max(8, (stateHeight - contentHeight) \ 2)
        Dim center = salesReportEmptyState.ClientSize.Width \ 2
        salesReportEmptyState.Controls("emptyIcon").SetBounds(center - 60, top, 120, 94)
        salesReportEmptyState.Controls("emptyTitle").SetBounds(0, top + 98, salesReportEmptyState.ClientSize.Width, 36)
        salesReportEmptyState.Controls("emptyMessage").SetBounds(0, top + 138, salesReportEmptyState.ClientSize.Width, 30)
        UpdateSalesReportEmptyState()
    End Sub

    Private Function CreateReportEmptyState(grid As DataGridView, heading As String, description As String) As Panel
        Dim state As New Panel With {.BackColor = Color.White, .Visible = False}
        state.Controls.Add(New Label With {.Name = "emptyIcon", .Text = "▤", .Font = New Font("Segoe UI Symbol", 48.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(57, 165, 125), .TextAlign = ContentAlignment.BottomCenter})
        state.Controls.Add(New Label With {.Name = "emptyTitle", .Text = heading, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 30, 55), .TextAlign = ContentAlignment.MiddleCenter})
        state.Controls.Add(New Label With {.Name = "emptyMessage", .Text = description, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(75, 85, 100), .TextAlign = ContentAlignment.MiddleCenter})
        grid.Controls.Add(state)
        AddHandler grid.SizeChanged, Sub(sender, e) PositionReportEmptyState(grid, state)
        AddHandler grid.DataSourceChanged, Sub(sender, e) UpdateReportEmptyState(grid, state)
        PositionReportEmptyState(grid, state)
        Return state
    End Function

    Private Sub PositionReportEmptyState(grid As DataGridView, state As Panel)
        If grid Is Nothing OrElse state Is Nothing Then Return
        Dim stateHeight = Math.Max(0, grid.ClientSize.Height - grid.ColumnHeadersHeight)
        state.SetBounds(0, grid.ColumnHeadersHeight, grid.ClientSize.Width, stateHeight)
        Dim contentHeight = 146
        Dim top = Math.Max(8, (stateHeight - contentHeight) \ 2)
        Dim center = state.ClientSize.Width \ 2
        state.Controls("emptyIcon").SetBounds(center - 48, top, 96, 76)
        state.Controls("emptyTitle").SetBounds(0, top + 78, state.ClientSize.Width, 30)
        state.Controls("emptyMessage").SetBounds(0, top + 111, state.ClientSize.Width, 26)
        UpdateReportEmptyState(grid, state)
    End Sub

    Private Sub UpdateReportEmptyState(grid As DataGridView, state As Panel)
        If grid Is Nothing OrElse state Is Nothing Then Return
        state.Visible = grid.Rows.Count = 0
        If state.Visible Then state.BringToFront()
    End Sub

    ' CHANGE: Refreshes empty-state visibility after loading report rows.
    Private Sub UpdateSalesReportEmptyState()
        If salesReportGrid Is Nothing OrElse salesReportEmptyState Is Nothing Then Return
        salesReportEmptyState.Visible = salesReportGrid.Rows.Count = 0
    End Sub

    Private Sub AddSalesReportButtonGroup(parent As Control, options As String(), buttons As Dictionary(Of String, Button), left As Integer, top As Integer, onSelect As Action(Of String), Optional buttonWidth As Integer = 94, Optional buttonHeight As Integer = 29)
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
            ' CHANGE: Use the light outline color shown in the reference.
            button.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230)
            button.FlatAppearance.BorderSize = 1

            AddHandler button.Click, Sub(sender, e) onSelect(optionText)
            buttons(optionText) = button
            parent.Controls.Add(button)
            x += button.Width + 8
        Next
    End Sub

    Private Sub SelectSalesReportPeriod(period As String)
        If String.Equals(salesReportPeriod, period, StringComparison.OrdinalIgnoreCase) Then Return
        salesReportPeriod = period
        UpdateSalesReportButtonStyles()
        ConfigureSalesReportFilters()
    End Sub

    Private Sub SelectSalesReportType(saleType As String)
        If String.Equals(salesReportType, saleType, StringComparison.OrdinalIgnoreCase) Then Return
        salesReportType = saleType
        UpdateSalesReportButtonStyles()
        LoadSalesReportRows()
    End Sub

    ' CHANGE: Apply the green active states to the period and sale type filters.
    Private Sub UpdateSalesReportButtonStyles()
        For Each pair In salesReportPeriodButtons
            Dim isSelected = String.Equals(pair.Key, salesReportPeriod, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, darkGreen, Color.White)
            pair.Value.ForeColor = If(isSelected, Color.White, darkGreen)
            pair.Value.FlatAppearance.BorderColor = If(isSelected, darkGreen, Color.FromArgb(210, 220, 230))
        Next
        For Each pair In salesReportTypeButtons
            Dim isSelected = String.Equals(pair.Key, salesReportType, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, Color.FromArgb(218, 240, 229), Color.White)
            pair.Value.ForeColor = darkGreen
            pair.Value.FlatAppearance.BorderColor = If(isSelected, Color.FromArgb(190, 230, 210), Color.FromArgb(210, 220, 230))
        Next
    End Sub

    ' CHANGE: Build a wider, styled date filter panel for the selected report period.
    Private Sub ConfigureSalesReportFilters()
        If salesReportFilterPanel Is Nothing Then Return
        salesReportFilterPanel.Controls.Clear()
        salesReportDatePicker = Nothing
        salesReportMonthCombo = Nothing
        salesReportYearCombo = Nothing
        Dim period = salesReportPeriod
        AddFilterHeader(If(String.Equals(period, "Daily", StringComparison.Ordinal), "Filter by date", "Filter by " & period.ToLowerInvariant()))

        If String.Equals(period, "Daily", StringComparison.Ordinal) Then
            Dim firstAvailableDate As New DateTime(2026, 9, 1)
            Dim lastAvailableDate As New DateTime(2026, 10, 31)
            If Date.Today < lastAvailableDate Then lastAvailableDate = Date.Today
            salesReportFilterPanel.Controls.Add(New Label With {.Text = "Date", .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(75, 90, 115), .Location = New Point(14, 60)})
            salesReportDatePicker = New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "MMMM dd, yyyy",
                .MinDate = firstAvailableDate,
                .MaxDate = lastAvailableDate,
                .Value = lastAvailableDate,
                .Location = New Point(14, 84),
                .Width = Math.Max(190, salesReportFilterPanel.ClientSize.Width - 28),
                .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right,
                .Font = New Font("Segoe UI", 10.0F)
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

    ' CHANGE: Add a small filter icon before the heading.
    Private Sub AddFilterHeader(text As String)
        salesReportFilterPanel.Controls.Add(New Label With {.Text = "▼  " & text, .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(14, 14)})
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
        If salesReportGrid Is Nothing Then Return
        Try
            Dim period = salesReportPeriod
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

            Dim selectedType = salesReportType
            Dim mode = If(String.Equals(selectedType, "All", StringComparison.Ordinal), Nothing, selectedType)
            Dim allRows = New SalesRepository().GetTransactionReport(mode, Nothing, startDate, endDate)
            Dim rows = allRows.Clone()
            For Each row As DataRow In allRows.Rows
                rows.ImportRow(row)
            Next

            ' CHANGE: Format the transaction table columns to match the reference layout.
            salesReportGrid.DataSource = rows
            If salesReportGrid.Columns.Contains("SaleDate") Then
                salesReportGrid.Columns("SaleDate").DefaultCellStyle.Format = "MMMM dd, yyyy h:mm tt"
                salesReportGrid.Columns("SaleDate").HeaderText = "Sale Date"
                salesReportGrid.Columns("SaleDate").FillWeight = 135
            End If
            If salesReportGrid.Columns.Contains("Total") Then
                salesReportGrid.Columns("Total").DefaultCellStyle.Format = "₱#,##0.00"
                salesReportGrid.Columns("Total").FillWeight = 90
            End If
            If salesReportGrid.Columns.Contains("TransactionID") Then
                salesReportGrid.Columns("TransactionID").HeaderText = "Transaction ID"
                salesReportGrid.Columns("TransactionID").FillWeight = 90
            End If
            If salesReportGrid.Columns.Contains("Customer") Then salesReportGrid.Columns("Customer").FillWeight = 120
            If salesReportGrid.Columns.Contains("Cashier") Then salesReportGrid.Columns("Cashier").FillWeight = 120
            If salesReportGrid.Columns.Contains("SaleType") Then
                salesReportGrid.Columns("SaleType").HeaderText = "Sale Type"
                salesReportGrid.Columns("SaleType").FillWeight = 90
            End If
            If salesReportGrid.Columns.Contains("Status") Then salesReportGrid.Columns("Status").FillWeight = 80

            Dim total As Decimal = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total"), 0D, Convert.ToDecimal(row("Total"))))
            salesReportDetailsLabel.Text = "Transaction Details — " & periodCaption
            salesReportTotalLabel.Text = "TOTAL SALES: ₱" & total.ToString("N2", CultureInfo.CurrentCulture)
            UpdateSalesReportEmptyState()
        Catch ex As Exception
            MessageBox.Show("Could not load the sales report. " & ex.Message, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintSalesReport(sender As Object, e As EventArgs)
        Try
            If salesReportGrid Is Nothing Then Return
            Dim rows = TryCast(salesReportGrid.DataSource, DataTable)
            If rows Is Nothing Then Return

            Dim period = salesReportPeriod
            Dim saleType = salesReportType
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
                Dim isWholesaleReceipt = String.Equals(DbText(sale, "sale_mode"), "Wholesale", StringComparison.OrdinalIgnoreCase)
                Dim customerPanel As New Panel With {.Location = New Point(20, 12), .Size = New Size(646, If(isWholesaleReceipt, 340, 224)), .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.White}
                customerPanel.Controls.Add(New Label With {.Text = "SALE INFORMATION", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(16, 13)})
                AddReceiptDetailField(customerPanel, "Sale Type", DbText(sale, "sale_mode"), 16, 48)
                If isWholesaleReceipt Then
                    AddReceiptDetailField(customerPanel, "Wholesale Client", DbText(sale, "wholesale_company_name"), 326, 48)
                    AddReceiptDetailField(customerPanel, "FDA LTO No.", If(String.IsNullOrWhiteSpace(DbText(sale, "wholesale_customer_license")), DbText(sale, "customer_id_number"), DbText(sale, "wholesale_customer_license")), 16, 102)
                    AddReceiptDetailField(customerPanel, "Contact Person", If(String.IsNullOrWhiteSpace(DbText(sale, "contact_person")), DbText(sale, "customer_name"), DbText(sale, "contact_person")), 326, 102)
                    AddReceiptDetailField(customerPanel, "Contact No.", DbText(sale, "contact_number"), 16, 156)
                    AddReceiptDetailField(customerPanel, "Shipping Address", DbText(sale, "customer_address"), 326, 156)
                    AddReceiptDetailField(customerPanel, "Purchase Order No.", DbText(sale, "purchase_order_no"), 16, 210)
                    AddReceiptDetailField(customerPanel, "Purchase Order Date", If(sale.IsNull("purchase_order_date"), String.Empty, Convert.ToDateTime(sale("purchase_order_date")).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)), 326, 210)
                    Dim paymentDescription = If(String.Equals(DbText(sale, "payment_method"), "E-payment", StringComparison.OrdinalIgnoreCase), "E-payment · " & DbText(sale, "payment_provider"), If(String.IsNullOrWhiteSpace(DbText(sale, "payment_method")), "Cash", DbText(sale, "payment_method")))
                    AddReceiptDetailField(customerPanel, "Mode of Payment", paymentDescription, 16, 264)
                    AddReceiptDetailField(customerPanel, "Cashier Initials", ReceiptStaffInitials(DbText(sale, "cashier_initials")), 326, 264)
                Else
                    AddReceiptDetailField(customerPanel, "Customer Type", DbText(sale, "customer_type"), 326, 48)
                    AddReceiptDetailField(customerPanel, "Customer", If(String.IsNullOrWhiteSpace(DbText(sale, "customer_name")), "Walk-in", DbText(sale, "customer_name")), 16, 102)
                    If Not String.IsNullOrWhiteSpace(DbText(sale, "customer_id_number")) Then
                        AddReceiptDetailField(customerPanel, "Customer ID", DbText(sale, "customer_id_number"), 326, 102)
                    End If
                    AddReceiptDetailField(customerPanel, "Cashier Initials", ReceiptStaffInitials(DbText(sale, "cashier_initials")), 16, 160)
                End If

                Dim itemHeadingTop = If(isWholesaleReceipt, 368, 252)
                Dim itemHeading As New Label With {.Text = "ITEMS", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(22, itemHeadingTop)}
                Dim itemGrid As New DataGridView With {
                    .Location = New Point(20, itemHeadingTop + 30),
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

                Dim rxTop = itemHeadingTop + 274
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
            dashboardRevenue = summary.TodaySales
            dashboardTodayTransactions = summary.RetailTransactions + summary.WholesaleTransactions
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
            card6Footer.Text = "Requires disposal"
            card7Value.Text = nearExpiryCount.ToString()
            card7Footer.Text = "Expires within 6 months"
            dashboardLowStock = lowStockCount
            dashboardExpired = expiredCount
            dashboardNearExpiry = nearExpiryCount
        Catch ex As Exception
            MessageBox.Show("Could not load inventory totals. " & ex.Message, "Inventory dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            Dim todayExpenses = New SalesRepository().GetTodayTotalExpenses()
            card8Value.Text = "P " & todayExpenses.ToString("N2")
            card8Footer.Text = "Recorded today"

            ' Optional: If you want to update the Net Income on the Revenue panel
            ' dashboardNetIncome = dashboardRevenue - todayExpenses
        Catch ex As Exception
            card8Value.Text = "P 0.00"
            card8Footer.Text = "Error loading expenses"
        End Try

        UpdateDashboardCounts()
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
            .BackColor = lightGray,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 230.0F))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        Dim toolbar As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(12, 10, 12, 14), .Padding = New Padding(0)}
        AddHandler toolbar.Resize, Sub(sender, e) ApplyRoundedRegion(toolbar, 12)
        Dim filterLabel As New Label With {
            .Text = "Filter by parent category",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.Black,
            .Location = New Point(14, 10)
        }
        toolbar.Controls.Add(filterLabel)

        Dim filterPanel As New FlowLayoutPanel With {
            .Location = New Point(14, 112),
            .Size = New Size(650, 36),
            .WrapContents = False,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        For Each filterName In {"All", "Low Stock", "Near Expiry", "Expired", "In Stock"}
            Dim filterButton As New Button With {
                .Text = filterName,
                .Tag = filterName,
                .AutoSize = False,
                .Size = New Size(104, 34),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.White,
                .ForeColor = Color.FromArgb(20, 100, 60),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Margin = New Padding(0, 0, 8, 0),
                .Padding = New Padding(4, 2, 4, 2),
                .UseVisualStyleBackColor = False,
                .Cursor = Cursors.Hand
            }
            filterButton.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230)
            filterButton.FlatAppearance.BorderSize = 1
            AddHandler filterButton.Click, AddressOf InventoryFilterButton_Click
            inventoryFilterButtons(filterName) = filterButton
            filterPanel.Controls.Add(filterButton)
        Next
        toolbar.Controls.Add(New Label With {
            .Text = "Filter by status",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.Black,
            .Location = New Point(14, 84)
        })
        toolbar.Controls.Add(filterPanel)
        Dim parentFilterPanel As New FlowLayoutPanel With {
            .Location = New Point(14, 38),
            .Size = New Size(520, 36),
            .WrapContents = False,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        For Each parentName In {"All", "Medicine", "Supplements", "Medical Supply"}
            Dim parentButton As New Button With {
                .Text = parentName,
                .Tag = parentName,
                .Size = New Size(120, 34),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = If(parentName = "All", Color.FromArgb(20, 100, 60), Color.White),
                .ForeColor = If(parentName = "All", Color.White, Color.FromArgb(20, 100, 60)),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Margin = New Padding(0, 0, 8, 0),
                .Cursor = Cursors.Hand,
                .UseVisualStyleBackColor = False
            }
            parentButton.FlatAppearance.BorderColor = If(parentName = "All", Color.FromArgb(20, 100, 60), Color.FromArgb(210, 220, 230))
            parentButton.FlatAppearance.BorderSize = 1
            AddHandler parentButton.Click, AddressOf InventoryParentFilterButton_Click
            inventoryParentFilterButtons(parentName) = parentButton
            parentFilterPanel.Controls.Add(parentButton)
        Next
        inventorySearchBox = New TextBox With {
            .Location = New Point(600, 41),
            .Size = New Size(300, 28),
            .Font = New Font("Segoe UI", 10.0F),
            .BackColor = Color.White,
            .ForeColor = Color.Black,
            .BorderStyle = BorderStyle.Fixed3D,
            .PlaceholderText = "Search inventory..."
        }
        AddHandler inventorySearchBox.TextChanged, AddressOf InventorySearchBox_TextChanged

        Dim clearButton As New Button With {
            .Text = "Clear",
            .Location = New Point(600, 112),
            .Size = New Size(82, 32),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(238, 241, 244),
            .ForeColor = Color.FromArgb(40, 40, 40),
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .Margin = New Padding(0),
            .UseVisualStyleBackColor = False,
            .Cursor = Cursors.Hand
        }
        clearButton.FlatAppearance.BorderSize = 0
        clearButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 231, 236)
        Dim expiryMonitorButton As New Button With {
            .Text = "Expiry Monitor",
            .Location = New Point(14, 158),
            .Size = New Size(160, 38),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(20, 100, 60),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .UseVisualStyleBackColor = False,
            .Cursor = Cursors.Hand
        }
        expiryMonitorButton.FlatAppearance.BorderColor = Color.FromArgb(20, 100, 60)
        AddHandler expiryMonitorButton.Click,
            Sub(sender, e)
                Using monitor As New ExpiryManagementForm()
                    monitor.ShowDialog(Me)
                End Using
            End Sub
        AddHandler clearButton.Click,
            Sub(sender, e)
                inventorySearchBox.Clear()
                SetInventoryFilter("All")
                SetInventoryParentFilter("All")
            End Sub
        Dim positionInventorySearchControls As Action =
            Sub()
                inventorySearchBox.Width = Math.Max(180, Math.Min(300, toolbar.ClientSize.Width - parentFilterPanel.Right - 20))
                inventorySearchBox.Left = toolbar.ClientSize.Width - inventorySearchBox.Width - 14
                clearButton.Left = Math.Max(filterPanel.Right + 12, toolbar.ClientSize.Width - clearButton.Width - 14)
            End Sub
        AddHandler toolbar.Resize, Sub(sender, e) positionInventorySearchControls()
        positionInventorySearchControls()
        toolbar.Controls.Add(parentFilterPanel)
        toolbar.Controls.Add(inventorySearchBox)
        toolbar.Controls.Add(clearButton)
        toolbar.Controls.Add(expiryMonitorButton)
        AddHandler clearButton.Resize, Sub(sender, e) ApplyRoundedRegion(clearButton, 8)
        AddHandler expiryMonitorButton.Resize, Sub(sender, e) ApplyRoundedRegion(expiryMonitorButton, 8)
        ApplyRoundedRegion(clearButton, 8)
        ApplyRoundedRegion(expiryMonitorButton, 8)

        Dim gridCard As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(10, 8, 10, 10), .Padding = New Padding(14)}
        AddHandler gridCard.Resize, Sub(sender, e) ApplyRoundedRegion(gridCard, 12)

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
        dgvInventory.ColumnHeadersHeight = 44
        dgvInventory.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvInventory.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvInventory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvInventory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvInventory.ReadOnly = True
        dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        dgvInventory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgvInventory.GridColor = Color.FromArgb(225, 235, 230)
        dgvInventory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        dgvInventory.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        dgvInventory.ColumnHeadersDefaultCellStyle.Padding = New Padding(12, 0, 10, 0)
        dgvInventory.DefaultCellStyle.Padding = New Padding(12, 6, 10, 6)
        AddHandler dgvInventory.DataBindingComplete, AddressOf InventoryGrid_DataBindingComplete
        AddHandler dgvInventory.CellFormatting, AddressOf InventoryGrid_CellFormatting
        AddHandler dgvInventory.CellClick, AddressOf InventoryGrid_CellClick

        layout.Controls.Add(toolbar, 0, 0)
        gridCard.Controls.Add(dgvInventory)
        layout.Controls.Add(gridCard, 0, 1)
        inventoryView.Controls.Clear()
        inventoryView.BackColor = lightGray
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
            batch("Status") = If(expiry <= Date.Today, "Expired", If(expiry <= Date.Today.AddMonths(6), "Near Expiry", "In Stock"))
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
                Dim isExpired = rowView IsNot Nothing AndAlso Not rowView.Row.IsNull("Expiry Date") AndAlso Convert.ToDateTime(rowView.Row("Expiry Date")).Date <= Date.Today
                e.Value = If(isExpired, "Retained", "—")
                e.CellStyle.ForeColor = Color.Gray
                e.CellStyle.BackColor = If(isExpired, Color.FromArgb(248, 248, 248), Color.White)
            End Sub
        AddHandler batchGrid.CellContentClick,
            Sub(sender, e)
                If e.RowIndex < 0 OrElse e.ColumnIndex <> removeColumn.Index Then Return
                Dim rowView = TryCast(batchGrid.Rows(e.RowIndex).DataBoundItem, DataRowView)
                If rowView Is Nothing OrElse Convert.ToDateTime(rowView.Row("Expiry Date")).Date > Date.Today Then Return
                MessageBox.Show("Expired batches remain in inventory for historical tracking. No stock or batch record was changed.",
                                "Batch retained", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Private Sub InventoryParentFilterButton_Click(sender As Object, e As EventArgs)
        Dim button = TryCast(sender, Button)
        If button IsNot Nothing Then SetInventoryParentFilter(Convert.ToString(button.Tag))
    End Sub

    Private Sub SetInventoryParentFilter(parentName As String)
        inventorySelectedParentCategory = If(String.IsNullOrWhiteSpace(parentName), "All", parentName)
        For Each entry In inventoryParentFilterButtons
            Dim active = String.Equals(entry.Key, inventorySelectedParentCategory, StringComparison.OrdinalIgnoreCase)
            entry.Value.BackColor = If(active, Color.FromArgb(20, 100, 60), Color.White)
            entry.Value.ForeColor = If(active, Color.White, Color.FromArgb(20, 100, 60))
            entry.Value.FlatAppearance.BorderColor = If(active, Color.FromArgb(20, 100, 60), Color.FromArgb(210, 220, 230))
        Next
        ApplyInventoryFilters()
    End Sub

    Private Sub InventorySearchBox_TextChanged(sender As Object, e As EventArgs)
        ApplyInventoryFilters()
    End Sub

    Private Sub SetInventoryFilter(filterName As String)
        inventoryActiveFilter = If(String.IsNullOrWhiteSpace(filterName), "All", filterName)
        For Each entry In inventoryFilterButtons
            Dim active = String.Equals(entry.Key, inventoryActiveFilter, StringComparison.OrdinalIgnoreCase)
            entry.Value.BackColor = If(active, Color.FromArgb(218, 240, 229), Color.White)
            entry.Value.ForeColor = Color.FromArgb(20, 100, 60)
            entry.Value.FlatAppearance.BorderColor = If(active, Color.FromArgb(190, 230, 210), Color.FromArgb(210, 220, 230))
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
                Case "In Stock"
                    includeByStatus = status.IsInStock
            End Select

            Dim includeBySearch = search.Length = 0 OrElse
                Convert.ToString(row("ID")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Category")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Brand Name")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Generic Name")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                Convert.ToString(row("Status")).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0

            Dim includeByParent = inventorySelectedParentCategory = "All" OrElse
                (inventoryRows.Columns.Contains("Parent Category") AndAlso
                 String.Equals(Convert.ToString(row("Parent Category")), inventorySelectedParentCategory, StringComparison.OrdinalIgnoreCase))

            If includeByStatus AndAlso includeByParent AndAlso includeBySearch Then visibleRows.ImportRow(row)
        Next
        dgvInventory.DataSource = visibleRows
    End Sub

    Private Function GetInventoryStatus(row As DataRow) As InventoryStatusInfo
        Dim stock = If(row.IsNull("Stock Quantity"), 0, Convert.ToInt32(row("Stock Quantity")))
        Dim reorderPoint = If(row.IsNull("Reorder Point"), 0, Convert.ToInt32(row("Reorder Point")))
        Dim isLowStock = stock <= reorderPoint
        Dim expiry As DateTime? = Nothing
        If Not row.IsNull("Expiry Date") Then expiry = Convert.ToDateTime(row("Expiry Date")).Date
        Dim isExpired = expiry.HasValue AndAlso expiry.Value <= Date.Today
        Dim isNearExpiry = expiry.HasValue AndAlso Not isExpired AndAlso expiry.Value <= Date.Today.AddMonths(6)
        Dim labels As New List(Of String)
        If row.Table.Columns.Contains("Active") AndAlso Not row.IsNull("Active") AndAlso Not Convert.ToBoolean(row("Active")) Then labels.Add("Inactive")
        If isExpired Then labels.Add("Expired")
        If isLowStock Then labels.Add("Low Stock")
        If isNearExpiry Then labels.Add("Near Expiry")
        Dim isInStock = labels.Count = 0
        Dim label = If(isInStock, "In Stock", String.Join(" · ", labels))
        Return New InventoryStatusInfo With {.Label = label, .IsLowStock = isLowStock, .IsNearExpiry = isNearExpiry, .IsExpired = isExpired, .IsInStock = isInStock}
    End Function

    Private Class InventoryStatusInfo
        Public Property Label As String
        Public Property IsLowStock As Boolean
        Public Property IsNearExpiry As Boolean
        Public Property IsExpired As Boolean
        Public Property IsInStock As Boolean
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
        dgvInventory.RowTemplate.Height = 60
        dgvInventory.RowTemplate.MinimumHeight = 60
        For Each row As DataGridViewRow In dgvInventory.Rows
            row.MinimumHeight = 60
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
            ApplyWholesaleDiscountRule()
            priceManagerProducts = New ProductManager().GetProducts(includeInactive:=True)
            If priceManagerProducts Is Nothing Then priceManagerProducts = New DataTable()
            LoadPriceManagerCategories()
            PopulatePriceManagerCategoryFilter()
            RenderPriceManagerProducts()
        Catch ex As Exception
            MessageBox.Show("Could not load product prices. " & ex.Message, "Price Manager", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyWholesaleDiscountRule()
        Using connection = GetConnection()
            connection.Open()
            Const query As String = "UPDATE product SET wholesale_price_per_box = ROUND(retail_price * quantity_per_box * 0.90, 2) WHERE retail_price IS NOT NULL AND quantity_per_box > 0"
            Using command As New MySqlCommand(query, connection)
                command.ExecuteNonQuery()
            End Using
        End Using
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

        Dim listing As New TableLayoutPanel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(12), .Margin = New Padding(4), .ColumnCount = 1, .RowCount = 3}
        AddHandler listing.Resize, Sub(sender, e) ApplyRoundedRegion(listing, 12)
        ApplyRoundedRegion(listing, 12)
        listing.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        listing.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))
        listing.RowStyles.Add(New RowStyle(SizeType.Absolute, 44))
        listing.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        Dim filterSearchRow As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .BackColor = Color.White, .Margin = New Padding(0)}
        filterSearchRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 58))
        filterSearchRow.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 42))
        filterSearchRow.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        Dim parentFilterPanel As New FlowLayoutPanel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False, .Padding = New Padding(0, 7, 0, 0), .Margin = New Padding(0)}
        priceManagerSearchBox = New TextBox With {.Dock = DockStyle.Fill, .Font = New Font("Segoe UI", 10.0F), .PlaceholderText = "Search product ID, category, brand, generic name...", .Margin = New Padding(12, 7, 0, 5)}
        priceManagerParentFilterButtons = New Dictionary(Of String, Button)(StringComparer.OrdinalIgnoreCase)
        For Each category In {"All", "Medicine", "Supplements", "Medical Supply"}
            Dim filterButton As New Button With {
                .Text = category,
                .Tag = category,
                .Size = New Size(120, 32),
                .Margin = New Padding(0, 0, 8, 0),
                .BackColor = If(category = "All", darkGreen, Color.White),
                .ForeColor = If(category = "All", Color.White, darkGreen),
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Cursor = Cursors.Hand,
                .UseVisualStyleBackColor = False
            }
            filterButton.FlatAppearance.BorderColor = darkGreen
            filterButton.FlatAppearance.BorderSize = 1
            priceManagerParentFilterButtons.Add(category, filterButton)
            parentFilterPanel.Controls.Add(filterButton)
            AddHandler filterButton.Click, AddressOf PriceManagerParentFilter_Click
        Next
        priceManagerCategoryFilter = New ComboBox With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .DrawMode = DrawMode.OwnerDrawFixed,
            .BackColor = Color.White,
            .ForeColor = darkGreen,
            .Font = New Font("Segoe UI", 10.0F),
            .Dock = DockStyle.Left,
            .Width = 260,
            .Margin = New Padding(0, 3, 0, 5)
        }
        priceManagerCategoryFilter.Items.Add("All Categories")
        priceManagerCategoryFilter.SelectedIndex = 0
        filterSearchRow.Controls.Add(parentFilterPanel, 0, 0)
        filterSearchRow.Controls.Add(priceManagerSearchBox, 1, 0)
        Dim productArea As New Panel With {.Dock = DockStyle.Fill, .BackColor = lightGray, .Margin = New Padding(0)}
        Dim detailsHeader As New Label With {.Text = "PRODUCT DETAILS", .Dock = DockStyle.Top, .Height = 32, .BackColor = Color.FromArgb(240, 245, 241), .ForeColor = Color.FromArgb(65, 80, 70), .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .TextAlign = ContentAlignment.MiddleLeft, .Padding = New Padding(8, 0, 0, 0)}
        priceManagerProductList = New FlowLayoutPanel With {.Dock = DockStyle.Fill, .AutoScroll = True, .FlowDirection = FlowDirection.TopDown, .WrapContents = False, .BackColor = lightGray, .Padding = New Padding(8, 8, 8, 0)}
        productArea.Controls.Add(priceManagerProductList)
        productArea.Controls.Add(detailsHeader)
        listing.Controls.Add(filterSearchRow, 0, 0)
        listing.Controls.Add(priceManagerCategoryFilter, 0, 1)
        listing.Controls.Add(productArea, 0, 2)
        page.Controls.Add(listing, 0, 0)
        priceManagerView.Controls.Add(page)

        AddHandler priceManagerSearchBox.TextChanged, Sub(sender, e) RenderPriceManagerProducts()
        AddHandler priceManagerCategoryFilter.SelectedIndexChanged,
            Sub(sender, e)
                If Not priceManagerUpdatingCategoryFilter Then RenderPriceManagerProducts()
            End Sub
        AddHandler priceManagerCategoryFilter.DrawItem, AddressOf PriceManagerCategoryFilter_DrawItem
        AddHandler priceManagerProductList.ClientSizeChanged, AddressOf PriceManagerProductList_Resize
    End Sub

    Private Sub PriceManagerParentFilter_Click(sender As Object, e As EventArgs)
        Dim filterButton = TryCast(sender, Button)
        If filterButton Is Nothing Then Return

        priceManagerSelectedParentCategory = Convert.ToString(filterButton.Tag)
        For Each entry In priceManagerParentFilterButtons
            Dim isSelected = String.Equals(entry.Key, priceManagerSelectedParentCategory, StringComparison.OrdinalIgnoreCase)
            entry.Value.BackColor = If(isSelected, darkGreen, Color.White)
            entry.Value.ForeColor = If(isSelected, Color.White, darkGreen)
        Next

        PopulatePriceManagerCategoryFilter()
        RenderPriceManagerProducts()
    End Sub

    Private Sub PopulatePriceManagerCategoryFilter()
        If priceManagerCategoryFilter Is Nothing OrElse priceManagerProducts Is Nothing Then Return

        priceManagerUpdatingCategoryFilter = True
        priceManagerCategoryFilter.BeginUpdate()
        Try
            Dim previousCategory = Convert.ToString(priceManagerCategoryFilter.SelectedItem)
            priceManagerCategoryFilter.Items.Clear()
            priceManagerCategoryFilter.Items.Add("All Categories")
            Dim categories = priceManagerProducts.AsEnumerable()
            If priceManagerSelectedParentCategory <> "All" AndAlso priceManagerProducts.Columns.Contains("Parent Category") Then
                categories = categories.Where(Function(row) String.Equals(
                    Convert.ToString(row("Parent Category")), priceManagerSelectedParentCategory, StringComparison.OrdinalIgnoreCase))
            End If

            Dim categoryNames As IEnumerable(Of String)
            If priceManagerCategories IsNot Nothing AndAlso priceManagerCategories.Columns.Contains("Parent Category") Then
                categoryNames = priceManagerCategories.AsEnumerable().
                    Where(Function(row) priceManagerSelectedParentCategory = "All" OrElse
                        String.Equals(Convert.ToString(row("Parent Category")), priceManagerSelectedParentCategory, StringComparison.OrdinalIgnoreCase)).
                    Select(Function(row) Convert.ToString(row("Category"))).
                    Where(Function(value) Not String.IsNullOrWhiteSpace(value)).
                    Distinct(StringComparer.OrdinalIgnoreCase).
                    OrderBy(Function(value) value)
            Else
                categoryNames = categories.Select(Function(row) Convert.ToString(row("Category"))).
                Where(Function(value) Not String.IsNullOrWhiteSpace(value)).
                Distinct(StringComparer.OrdinalIgnoreCase).
                OrderBy(Function(value) value)
            End If
            For Each category In categoryNames
                priceManagerCategoryFilter.Items.Add(category)
            Next
            If Not String.IsNullOrWhiteSpace(previousCategory) AndAlso priceManagerCategoryFilter.Items.Contains(previousCategory) Then
                priceManagerCategoryFilter.SelectedItem = previousCategory
            Else
                priceManagerCategoryFilter.SelectedIndex = 0
            End If
        Finally
            priceManagerCategoryFilter.EndUpdate()
            priceManagerUpdatingCategoryFilter = False
        End Try
    End Sub

    Private Sub LoadPriceManagerCategories()
        Const query As String = "SELECT child.category_name AS 'Category', parent.category_name AS 'Parent Category' " &
                                "FROM product_category child " &
                                "INNER JOIN product_category parent ON child.parent_category_id = parent.category_ID " &
                                "WHERE child.is_active = 1 AND parent.is_active = 1 " &
                                "ORDER BY parent.category_name, child.category_name"
        priceManagerCategories = New DataTable()
        Using connection = GetConnection()
            connection.Open()
            Using adapter As New MySqlDataAdapter(query, connection)
                adapter.Fill(priceManagerCategories)
            End Using
        End Using
    End Sub

    Private Sub PriceManagerCategoryFilter_DrawItem(sender As Object, e As DrawItemEventArgs)
        If e.Index < 0 Then Return

        Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
        Dim background = If(isSelected, Color.FromArgb(218, 240, 229), Color.White)
        Dim foreground = If(isSelected, darkGreen, Color.Black)
        Using brush As New SolidBrush(background)
            e.Graphics.FillRectangle(brush, e.Bounds)
        End Using
        TextRenderer.DrawText(e.Graphics,
                              Convert.ToString(priceManagerCategoryFilter.Items(e.Index)),
                              e.Font,
                              e.Bounds,
                              foreground,
                              TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
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
            If priceManagerSelectedParentCategory <> "All" AndAlso priceManagerProducts.Columns.Contains("Parent Category") AndAlso
               Not String.Equals(Convert.ToString(product("Parent Category")), priceManagerSelectedParentCategory, StringComparison.OrdinalIgnoreCase) Then Continue For
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
            If TypeOf card Is Panel Then card.Width = GetPriceManagerProductCardWidth()
        Next
    End Sub

    Private Function GetPriceManagerProductCardWidth() As Integer
        If priceManagerProductList Is Nothing Then Return 320
        Return Math.Max(320, priceManagerProductList.ClientSize.Width - priceManagerProductList.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth - 4)
    End Function

    Private Function CreatePriceProductCard(product As DataRow) As Control
        Dim card As New Panel With {.Width = GetPriceManagerProductCardWidth(), .Height = 148, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 10), .BorderStyle = BorderStyle.None}
        Dim brand = Convert.ToString(product("Brand Name"))
        Dim generic = Convert.ToString(product("Generic Name"))
        Dim id = Convert.ToString(product("ID"))
        Dim unit = Convert.ToString(product("Unit"))
        Dim qtyPerBox = If(product.IsNull("Qty/Box"), 0, Convert.ToInt32(product("Qty/Box")))
        Dim edit As New Button With {.Text = "Edit Prices", .Size = New Size(112, 32), .Location = New Point(card.Width - 128, 10), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = activeGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        edit.FlatAppearance.BorderSize = 0
        edit.FlatAppearance.MouseOverBackColor = darkGreen
        AddHandler edit.Resize, Sub(sender, e) ApplyRoundedRegion(edit, 6)
        ApplyRoundedRegion(edit, 6)
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
        AddHandler card.Resize, Sub(sender, e) ApplyRoundedRegion(card, 10)
        ApplyRoundedRegion(card, 10)
        AddHandler edit.Click, Sub(sender, e) EditProductPrices(product)
        AddHandler card.Resize,
            Sub(sender, e)
                priceArea.Width = Math.Max(200, card.ClientSize.Width - 24)
            End Sub
        Return card
    End Function

    Private Function CreatePriceValuePanel(caption As String, amount As Object, unitText As String) As Panel
        Dim panel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(245, 248, 246), .Margin = New Padding(0, 0, 8, 0)}
        AddHandler panel.Resize, Sub(sender, e) ApplyRoundedRegion(panel, 6)
        ApplyRoundedRegion(panel, 6)
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
        Using prompt As New Form With {.Text = "Edit Prices", .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.None, .ControlBox = False, .ClientSize = New Size(420, 250), .MinimizeBox = False, .MaximizeBox = False, .BackColor = Color.White}
            AddBlackDialogBorder(prompt)
            prompt.Controls.Add(New Label With {.Text = Convert.ToString(product("Brand Name")), .AutoSize = True, .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(18, 16)})
            prompt.Controls.Add(New Label With {.Text = "Retail price per " & Convert.ToString(product("Unit")), .AutoSize = True, .Location = New Point(18, 58)})
            Dim retailBox As New TextBox With {.Text = retailText, .Location = New Point(18, 80), .Width = 150}
            prompt.Controls.Add(retailBox)
            prompt.Controls.Add(New Label With {.Text = "Wholesale price per box", .AutoSize = True, .Location = New Point(195, 58)})
            Dim wholesaleBox As New TextBox With {.Text = wholesaleText, .Location = New Point(195, 80), .Width = 200, .ReadOnly = True}
            prompt.Controls.Add(wholesaleBox)
            Dim autoWholesaleCheck As New CheckBox With {.Text = "Auto-calculate wholesale at 10% below box retail value", .AutoSize = True, .Checked = True, .Location = New Point(18, 118), .ForeColor = Color.FromArgb(65, 80, 70)}
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
                        Dim calculated = Decimal.Round(retailPrice * quantityPerBox * 0.9D, 2, MidpointRounding.AwayFromZero)
                        wholesaleBox.Text = calculated.ToString("0.00", CultureInfo.CurrentCulture)
                    End If
                End Sub
            AddHandler retailBox.TextChanged, Sub(sender, e) updateAutoWholesale()
            updateAutoWholesale()
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

    Private Sub PharName_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub card2Value_Click(sender As Object, e As EventArgs) Handles card2Value.Click

    End Sub

    Private Sub ShowEmployeeSalesReport()
        reportsView.Controls.Clear()
        reportsView.Padding = New Padding(14)
        reportsView.BackColor = lightGray

        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = lightGray, .Margin = New Padding(0)}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 190))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim header As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12)}
        AddHandler header.Paint, Sub(sender, e)
                                     Using brush As New LinearGradientBrush(header.ClientRectangle, Color.White, Color.FromArgb(214, 246, 230), 0.0F)
                                         e.Graphics.FillRectangle(brush, header.ClientRectangle)
                                     End Using
                                 End Sub
        header.Controls.Add(New Label With {.Text = "Employee Sales Report", .AutoSize = True, .Font = New Font("Segoe UI", 17.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 30, 55), .Location = New Point(76, 18)})
        header.Controls.Add(New Label With {.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt", CultureInfo.CurrentCulture) & "  |  By: " & CurrentFullName, .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(90, 105, 125), .Location = New Point(78, 51)})

        Dim backButton As New Button With {.Text = String.Empty, .Size = New Size(42, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Left, .BackColor = Color.White, .ForeColor = darkGreen, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
        backButton.Location = New Point(18, 18)
        StyleReportBackButton(backButton)
        AddHandler backButton.Click, Sub(sender, e) InitializeReportsUi()
        header.Controls.Add(backButton)

        Dim printReportButton As New Button With {.Text = "▣  Print Report", .Size = New Size(150, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        printReportButton.FlatAppearance.BorderSize = 0
        StyleReportActionButton(printReportButton)
        printReportButton.Location = New Point(header.ClientSize.Width - printReportButton.Width - 18, 18)
        AddHandler header.Resize, Sub(sender, e) printReportButton.Left = header.ClientSize.Width - printReportButton.Width - 18
        AddHandler printReportButton.Click, AddressOf PrintEmployeeSalesReport
        header.Controls.Add(printReportButton)

        empSalesReportPeriodButtons.Clear()
        AddSalesReportButtonGroup(header, {"Daily", "Monthly", "Yearly"}, empSalesReportPeriodButtons, 76, 88, AddressOf SelectEmpSalesReportPeriod, 100, 38)
        UpdateEmpSalesReportButtonStyles()

        Dim body As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .BackColor = lightGray, .Margin = New Padding(0)}
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 270))
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        empSalesReportFilterPanel = New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(14), .Margin = New Padding(0, 0, 12, 0)}
        AddHandler empSalesReportFilterPanel.Resize, Sub(sender, e) ApplyRoundedRegion(empSalesReportFilterPanel, 16)

        Dim reportPanel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(8), .Margin = New Padding(0)}
        AddHandler reportPanel.Resize, Sub(sender, e) ApplyRoundedRegion(reportPanel, 16)
        salesReportGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .GridColor = Color.FromArgb(225, 235, 230),
            .EnableHeadersVisualStyles = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .ColumnHeadersHeight = 42,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        }
        salesReportGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        salesReportGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        salesReportGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        salesReportGrid.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        salesReportGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        salesReportGrid.DefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        salesReportGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
        salesReportGrid.DefaultCellStyle.SelectionForeColor = Color.Black
        salesReportGrid.RowTemplate.Height = 40

        empSalesReportTotalLabel = New Label With {.Text = "TOTAL SALES: ₱0.00", .Dock = DockStyle.Bottom, .Height = 46, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .BackColor = Color.FromArgb(225, 245, 235), .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 18, 0)}
        Dim detailsLabel As New Label With {.Text = "▤  Employee Sales Details", .Dock = DockStyle.Top, .Height = 42, .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(25, 40, 55), .Padding = New Padding(12, 10, 0, 0)}

        reportPanel.Controls.Add(salesReportGrid)
        reportPanel.Controls.Add(empSalesReportTotalLabel)
        reportPanel.Controls.Add(detailsLabel)
        empSalesReportEmptyState = CreateReportEmptyState(salesReportGrid, "No employee sales found", "Try selecting a different date or period.")

        body.Controls.Add(empSalesReportFilterPanel, 0, 0)
        body.Controls.Add(reportPanel, 1, 0)
        layout.Controls.Add(header, 0, 0)
        layout.Controls.Add(body, 0, 1)
        reportsView.Controls.Add(layout)

        ConfigureEmpSalesReportFilters()
        AddHandler header.Resize, Sub(sender, e) ApplyRoundedRegion(header, 16)
        ApplyRoundedRegion(header, 16)
        PositionReportEmptyState(salesReportGrid, empSalesReportEmptyState)
    End Sub

    Private Sub SelectEmpSalesReportPeriod(period As String)
        If String.Equals(empSalesReportPeriod, period, StringComparison.OrdinalIgnoreCase) Then Return
        empSalesReportPeriod = period
        UpdateEmpSalesReportButtonStyles()
        ConfigureEmpSalesReportFilters()
    End Sub

    Private Sub UpdateEmpSalesReportButtonStyles()
        For Each pair In empSalesReportPeriodButtons
            Dim isSelected = String.Equals(pair.Key, empSalesReportPeriod, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, darkGreen, Color.White)
            pair.Value.ForeColor = If(isSelected, Color.White, darkGreen)
            pair.Value.FlatAppearance.BorderColor = If(isSelected, darkGreen, Color.FromArgb(210, 220, 230))
        Next
    End Sub

    Private Sub ConfigureEmpSalesReportFilters()
        If empSalesReportFilterPanel Is Nothing Then Return
        empSalesReportFilterPanel.Controls.Clear()
        empSalesReportDatePicker = Nothing
        empSalesReportMonthCombo = Nothing
        empSalesReportYearCombo = Nothing
        Dim period = empSalesReportPeriod

        empSalesReportFilterPanel.Controls.Add(New Label With {.Text = "▼  " & If(String.Equals(period, "Daily", StringComparison.Ordinal), "Filter by date", "Filter by " & period.ToLowerInvariant()), .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(14, 14)})

        If String.Equals(period, "Daily", StringComparison.Ordinal) Then
            Dim firstAvailableDate As New DateTime(2026, 9, 1)
            Dim lastAvailableDate As New DateTime(2026, 10, 31)
            If Date.Today < lastAvailableDate Then lastAvailableDate = Date.Today

            empSalesReportFilterPanel.Controls.Add(New Label With {.Text = "Date", .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(75, 90, 115), .Location = New Point(14, 60)})
            empSalesReportDatePicker = New DateTimePicker With {
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "MMMM dd, yyyy",
                .MinDate = firstAvailableDate,
                .MaxDate = lastAvailableDate,
                .Value = lastAvailableDate,
                .Location = New Point(14, 84),
                .Width = Math.Max(190, empSalesReportFilterPanel.ClientSize.Width - 28),
                .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right,
                .Font = New Font("Segoe UI", 10.0F)
            }
            empSalesReportFilterPanel.Controls.Add(empSalesReportDatePicker)
        ElseIf String.Equals(period, "Monthly", StringComparison.Ordinal) Then
            empSalesReportMonthCombo = AddEmpFilterCombo("Month", {"September", "October"}, "October", 60)
            empSalesReportYearCombo = AddEmpFilterCombo("Year", {"2026"}, "2026", 145)
        Else
            empSalesReportYearCombo = AddEmpFilterCombo("Year", {"2026"}, "2026", 60)
        End If

        If empSalesReportDatePicker IsNot Nothing Then AddHandler empSalesReportDatePicker.ValueChanged, Sub(sender, e) LoadEmpSalesReportRows()
        If empSalesReportMonthCombo IsNot Nothing Then AddHandler empSalesReportMonthCombo.SelectedIndexChanged, Sub(sender, e) LoadEmpSalesReportRows()
        If empSalesReportYearCombo IsNot Nothing Then AddHandler empSalesReportYearCombo.SelectedIndexChanged, Sub(sender, e) LoadEmpSalesReportRows()
        LoadEmpSalesReportRows()
    End Sub

    Private Function AddEmpFilterCombo(labelText As String, items As String(), selected As String, top As Integer) As ComboBox
        empSalesReportFilterPanel.Controls.Add(New Label With {.Text = labelText, .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(14, top)})
        Dim combo As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F), .Location = New Point(14, top + 20), .Width = 190}
        combo.Items.AddRange(items.Cast(Of Object)().ToArray())
        combo.SelectedItem = selected
        empSalesReportFilterPanel.Controls.Add(combo)
        Return combo
    End Function

    Private Function AddExpFilterCombo(labelText As String, items As String(), selected As String, top As Integer) As ComboBox
        expReportFilterPanel.Controls.Add(New Label With {.Text = labelText, .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(14, top)})
        Dim combo As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.0F), .Location = New Point(14, top + 20), .Width = 190}
        combo.Items.AddRange(items.Cast(Of Object)().ToArray())
        combo.SelectedItem = selected
        expReportFilterPanel.Controls.Add(combo)
        Return combo
    End Function

    Private Sub LoadEmpSalesReportRows()
        If salesReportGrid Is Nothing Then Return
        Try
            Dim period = empSalesReportPeriod
            Dim year As Integer = 2026
            If Not String.Equals(period, "Daily", StringComparison.Ordinal) Then
                If empSalesReportYearCombo Is Nothing OrElse empSalesReportYearCombo.SelectedItem Is Nothing Then Return
                year = Integer.Parse(Convert.ToString(empSalesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
            End If
            Dim startDate As DateTime
            Dim endDate As DateTime

            If String.Equals(period, "Daily", StringComparison.Ordinal) Then
                If empSalesReportDatePicker Is Nothing Then Return
                startDate = empSalesReportDatePicker.Value.Date
                endDate = startDate.AddDays(1)
            ElseIf String.Equals(period, "Yearly", StringComparison.Ordinal) Then
                startDate = New DateTime(year, 1, 1)
                endDate = startDate.AddYears(1)
            Else
                If empSalesReportMonthCombo Is Nothing OrElse empSalesReportMonthCombo.SelectedItem Is Nothing Then Return
                Dim month = DateTime.ParseExact(Convert.ToString(empSalesReportMonthCombo.SelectedItem), "MMMM", CultureInfo.InvariantCulture).Month
                startDate = New DateTime(year, month, 1)
                endDate = startDate.AddMonths(1)
            End If

            Dim repo As New SalesRepository()
            Dim rows = repo.GetEmployeeSalesReport(startDate, endDate)
            ' CHANGE: Format the transaction table columns to match the reference layout.
            salesReportGrid.DataSource = rows
            UpdateReportEmptyState(salesReportGrid, empSalesReportEmptyState)
            If salesReportGrid.Columns.Contains("Total Sales Revenue") Then
                salesReportGrid.Columns("Total Sales Revenue").DefaultCellStyle.Format = "₱#,##0.00"
            End If

            Dim total As Decimal = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total Sales Revenue"), 0D, Convert.ToDecimal(row("Total Sales Revenue"))))
            empSalesReportTotalLabel.Text = "TOTAL SALES: ₱" & total.ToString("N2", CultureInfo.CurrentCulture)
        Catch ex As Exception
            MessageBox.Show("Could not load the employee sales report. " & ex.Message, "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintEmployeeSalesReport(sender As Object, e As EventArgs)
        Try
            If salesReportGrid Is Nothing Then Return
            Dim rows = TryCast(salesReportGrid.DataSource, DataTable)
            If rows Is Nothing Then Return

            Dim period = empSalesReportPeriod
            Dim title = period & " Employee Sales Report"
            Dim destinationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "employee_sales_report", period.ToLowerInvariant())
            Dim fileSuffix As String
            Dim periodCaption As String

            If String.Equals(period, "Daily", StringComparison.Ordinal) Then
                Dim saleDate = empSalesReportDatePicker.Value.Date
                fileSuffix = saleDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                periodCaption = saleDate.ToString("MMMM dd, yyyy", CultureInfo.CurrentCulture)
            ElseIf String.Equals(period, "Monthly", StringComparison.Ordinal) Then
                Dim year = Integer.Parse(Convert.ToString(empSalesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                Dim month = DateTime.ParseExact(Convert.ToString(empSalesReportMonthCombo.SelectedItem), "MMMM", CultureInfo.InvariantCulture).Month
                Dim saleDate = New DateTime(year, month, 1)
                destinationFolder = Path.Combine(destinationFolder, year.ToString(CultureInfo.InvariantCulture), saleDate.ToString("MMMM", CultureInfo.InvariantCulture))
                fileSuffix = saleDate.ToString("MMMM_yyyy", CultureInfo.InvariantCulture)
                periodCaption = saleDate.ToString("MMMM yyyy", CultureInfo.CurrentCulture)
            Else
                Dim year = Integer.Parse(Convert.ToString(empSalesReportYearCombo.SelectedItem), CultureInfo.InvariantCulture)
                destinationFolder = Path.Combine(destinationFolder, year.ToString(CultureInfo.InvariantCulture))
                fileSuffix = year.ToString(CultureInfo.InvariantCulture)
                periodCaption = year.ToString(CultureInfo.InvariantCulture)
            End If

            Dim total = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total Sales Revenue"), 0D, Convert.ToDecimal(row("Total Sales Revenue"))))
            Dim totalTransactions = rows.AsEnumerable().Sum(Function(row) If(row.IsNull("Total Transactions"), 0, Convert.ToInt32(row("Total Transactions"))))

            Dim lines As New List(Of String) From {
                "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES",
                title,
                "@META|Report Period: " & periodCaption,
                "@META|Generated: " & DateTime.Now.ToString("MMMM dd, yyyy h:mm tt", CultureInfo.CurrentCulture) & "|Prepared by: " & CurrentFullName,
                "@SECTION|EMPLOYEE SALES SUMMARY",
                "@SUMMARY|Active Cashiers|" & rows.Rows.Count.ToString(CultureInfo.InvariantCulture) & "|Total Transactions|" & totalTransactions.ToString(CultureInfo.InvariantCulture),
                "@TOTAL|TOTAL REVENUE|PHP " & total.ToString("N2", CultureInfo.InvariantCulture),
                "@SECTION|EMPLOYEE BREAKDOWN",
                "@TABLE|EMPLOYEE NAME|ROLE|TOTAL TRANSACTIONS|TOTAL SALES REVENUE"
            }

            For Each row As DataRow In rows.Rows
                lines.Add(String.Join("|", {
                    "@TABLE",
                    Convert.ToString(row("Employee Name")),
                    Convert.ToString(row("Role")),
                    Convert.ToString(row("Total Transactions"), CultureInfo.InvariantCulture),
                    "PHP " & Convert.ToDecimal(row("Total Sales Revenue")).ToString("N2", CultureInfo.InvariantCulture)
                }))
            Next

            If rows.Rows.Count = 0 Then lines.Add("@NOTE|No transactions were recorded for this period.")

            Directory.CreateDirectory(destinationFolder)
            Dim outputPath = Path.Combine(destinationFolder, "employee_sales_" & fileSuffix & ".pdf")
            Staff.CreateReportPdf(outputPath, lines)
            Process.Start(New ProcessStartInfo(outputPath) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to create or open the employee sales report PDF. " & ex.Message, "Print Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ShowExpenseReport()
        reportsView.Controls.Clear()
        reportsView.Padding = New Padding(14)
        reportsView.BackColor = lightGray

        Dim layout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = lightGray, .Margin = New Padding(0)}
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 190))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim header As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12)}
        AddHandler header.Paint, Sub(sender, e)
                                     Using brush As New LinearGradientBrush(header.ClientRectangle, Color.White, Color.FromArgb(214, 246, 230), 0.0F)
                                         e.Graphics.FillRectangle(brush, header.ClientRectangle)
                                     End Using
                                 End Sub
        header.Controls.Add(New Label With {.Text = "Financial Expense Report", .AutoSize = True, .Font = New Font("Segoe UI", 17.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(20, 30, 55), .Location = New Point(76, 18)})
        header.Controls.Add(New Label With {.Text = "Generated: " & DateTime.Now.ToString("MMMM dd, yyyy, h:mm tt", Globalization.CultureInfo.CurrentCulture) & "  |  By: " & CurrentFullName, .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(90, 105, 125), .Location = New Point(78, 51)})

        Dim backButton As New Button With {.Text = String.Empty, .Size = New Size(42, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Left, .BackColor = Color.White, .ForeColor = darkGreen, .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand}
        backButton.Location = New Point(18, 18)
        StyleReportBackButton(backButton)
        AddHandler backButton.Click, Sub(sender, e) InitializeReportsUi()

        Dim addExpenseBtn As New Button With {.Text = "+  Add Expense", .Size = New Size(150, 40), .Anchor = AnchorStyles.Top Or AnchorStyles.Right, .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .Cursor = Cursors.Hand}
        addExpenseBtn.FlatAppearance.BorderSize = 0
        StyleReportActionButton(addExpenseBtn)
        addExpenseBtn.Location = New Point(header.ClientSize.Width - addExpenseBtn.Width - 18, 18)
        AddHandler header.Resize, Sub(sender, e) addExpenseBtn.Left = header.ClientSize.Width - addExpenseBtn.Width - 18
        AddHandler addExpenseBtn.Click, AddressOf ShowAddExpenseModal

        header.Controls.Add(backButton)
        header.Controls.Add(addExpenseBtn)

        expReportPeriodButtons.Clear()
        AddSalesReportButtonGroup(header, {"Daily", "Monthly", "Yearly"}, expReportPeriodButtons, 76, 88, AddressOf SelectExpReportPeriod, 100, 38)
        UpdateExpReportButtonStyles()

        Dim body As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 1, .BackColor = lightGray, .Margin = New Padding(0)}
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 270))
        body.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        expReportFilterPanel = New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(14), .Margin = New Padding(0, 0, 12, 0)}
        AddHandler expReportFilterPanel.Resize, Sub(sender, e) ApplyRoundedRegion(expReportFilterPanel, 16)

        Dim reportPanel As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(8), .Margin = New Padding(0)}
        AddHandler reportPanel.Resize, Sub(sender, e) ApplyRoundedRegion(reportPanel, 16)
        dgvExpenses = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .GridColor = Color.FromArgb(225, 235, 230),
            .EnableHeadersVisualStyles = False,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .ColumnHeadersHeight = 42,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        }
        dgvExpenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        dgvExpenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        dgvExpenses.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        dgvExpenses.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        dgvExpenses.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        dgvExpenses.DefaultCellStyle.Padding = New Padding(8, 0, 0, 0)
        dgvExpenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 245, 230)
        dgvExpenses.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvExpenses.RowTemplate.Height = 40

        expReportTotalLabel = New Label With {.Text = "TOTAL EXPENSES: ₱0.00", .Dock = DockStyle.Bottom, .Height = 46, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .BackColor = Color.FromArgb(225, 245, 235), .TextAlign = ContentAlignment.MiddleRight, .Padding = New Padding(0, 0, 18, 0)}
        Dim detailsLabel As New Label With {.Text = "▤  Expense Details", .Dock = DockStyle.Top, .Height = 42, .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(25, 40, 55), .Padding = New Padding(12, 10, 0, 0)}

        reportPanel.Controls.Add(dgvExpenses)
        reportPanel.Controls.Add(expReportTotalLabel)
        reportPanel.Controls.Add(detailsLabel)
        expReportEmptyState = CreateReportEmptyState(dgvExpenses, "No expenses found", "Try selecting a different date or period.")

        body.Controls.Add(expReportFilterPanel, 0, 0)
        body.Controls.Add(reportPanel, 1, 0)
        layout.Controls.Add(header, 0, 0)
        layout.Controls.Add(body, 0, 1)
        reportsView.Controls.Add(layout)

        ConfigureExpReportFilters()
        AddHandler header.Resize, Sub(sender, e) ApplyRoundedRegion(header, 16)
        ApplyRoundedRegion(header, 16)
        PositionReportEmptyState(dgvExpenses, expReportEmptyState)
    End Sub

    ' --- Expense Report Filter Logic ---
    Private Sub SelectExpReportPeriod(period As String)
        If String.Equals(expReportPeriod, period, StringComparison.OrdinalIgnoreCase) Then Return
        expReportPeriod = period
        UpdateExpReportButtonStyles()
        ConfigureExpReportFilters()
    End Sub

    Private Sub UpdateExpReportButtonStyles()
        For Each pair In expReportPeriodButtons
            Dim isSelected = String.Equals(pair.Key, expReportPeriod, StringComparison.OrdinalIgnoreCase)
            pair.Value.BackColor = If(isSelected, darkGreen, Color.White)
            pair.Value.ForeColor = If(isSelected, Color.White, darkGreen)
            pair.Value.FlatAppearance.BorderColor = If(isSelected, darkGreen, Color.FromArgb(210, 220, 230))
        Next
    End Sub

    Private Sub ConfigureExpReportFilters()
        If expReportFilterPanel Is Nothing Then Return
        expReportFilterPanel.Controls.Clear()
        expReportDatePicker = Nothing
        expReportMonthCombo = Nothing
        expReportYearCombo = Nothing
        Dim period = expReportPeriod

        expReportFilterPanel.Controls.Add(New Label With {.Text = "▼  " & If(String.Equals(period, "Daily", StringComparison.Ordinal), "Filter by date", "Filter by " & period.ToLowerInvariant()), .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(14, 14)})

        If String.Equals(period, "Daily", StringComparison.Ordinal) Then
            Dim firstAvailableDate As New DateTime(2026, 9, 1)
            Dim lastAvailableDate As New DateTime(2026, 10, 31)
            If Date.Today < lastAvailableDate Then lastAvailableDate = Date.Today

            expReportFilterPanel.Controls.Add(New Label With {.Text = "Date", .AutoSize = True, .Font = New Font("Segoe UI", 9.0F), .ForeColor = Color.FromArgb(75, 90, 115), .Location = New Point(14, 60)})
            expReportDatePicker = New DateTimePicker With {.Format = DateTimePickerFormat.Custom, .CustomFormat = "MMMM dd, yyyy", .Value = lastAvailableDate, .Location = New Point(14, 84), .Width = Math.Max(190, expReportFilterPanel.ClientSize.Width - 28), .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right, .Font = New Font("Segoe UI", 10.0F)}
            expReportFilterPanel.Controls.Add(expReportDatePicker)
            AddHandler expReportDatePicker.ValueChanged, Sub(sender, e) LoadExpenseReportRows()
        ElseIf String.Equals(period, "Monthly", StringComparison.Ordinal) Then
            ' Updated to use AddExpFilterCombo
            expReportMonthCombo = AddExpFilterCombo("Month", {"September", "October"}, "October", 60)
            expReportYearCombo = AddExpFilterCombo("Year", {"2026"}, "2026", 145)
            AddHandler expReportMonthCombo.SelectedIndexChanged, Sub(sender, e) LoadExpenseReportRows()
            AddHandler expReportYearCombo.SelectedIndexChanged, Sub(sender, e) LoadExpenseReportRows()
        Else
            ' Updated to use AddExpFilterCombo
            expReportYearCombo = AddExpFilterCombo("Year", {"2026"}, "2026", 60)
            AddHandler expReportYearCombo.SelectedIndexChanged, Sub(sender, e) LoadExpenseReportRows()
        End If

        LoadExpenseReportRows()
    End Sub

    Private Sub LoadExpenseReportRows()
        If dgvExpenses Is Nothing Then Return
        Try
            Dim repo As New SalesRepository()
            Dim rows = repo.GetExpenseReport() ' Future optimization: pass dates to repo like we did for sales

            Dim filteredRows = rows.Clone()
            For Each row As DataRow In rows.Rows
                Dim expDate = Convert.ToDateTime(row("Date"))
                Dim include = False

                If expReportPeriod = "Daily" AndAlso expReportDatePicker IsNot Nothing Then
                    include = (expDate.Date = expReportDatePicker.Value.Date)
                ElseIf expReportPeriod = "Monthly" AndAlso expReportMonthCombo IsNot Nothing AndAlso expReportYearCombo IsNot Nothing Then
                    Dim selectedMonth = DateTime.ParseExact(expReportMonthCombo.Text, "MMMM", Globalization.CultureInfo.InvariantCulture).Month
                    Dim selectedYear = Integer.Parse(expReportYearCombo.Text)
                    include = (expDate.Month = selectedMonth AndAlso expDate.Year = selectedYear)
                ElseIf expReportPeriod = "Yearly" AndAlso expReportYearCombo IsNot Nothing Then
                    include = (expDate.Year = Integer.Parse(expReportYearCombo.Text))
                End If

                If include Then filteredRows.ImportRow(row)
            Next

            dgvExpenses.DataSource = filteredRows
            UpdateReportEmptyState(dgvExpenses, expReportEmptyState)
            If dgvExpenses.Columns.Contains("Amount") Then dgvExpenses.Columns("Amount").DefaultCellStyle.Format = "₱#,##0.00"
            If dgvExpenses.Columns.Contains("Date") Then dgvExpenses.Columns("Date").DefaultCellStyle.Format = "MM/dd/yyyy"

            Dim total As Decimal = filteredRows.AsEnumerable().Sum(Function(row) If(row.IsNull("Amount"), 0D, Convert.ToDecimal(row("Amount"))))
            expReportTotalLabel.Text = "TOTAL EXPENSES: ₱" & total.ToString("N2", Globalization.CultureInfo.CurrentCulture)
        Catch ex As Exception
            MessageBox.Show("Could not load expenses. " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' --- Add Expense Modal ---
    Private Sub ShowAddExpenseModal()
        Using modal As New Form()
            modal.Text = "Log Financial Expense"
            modal.Size = New Size(400, 360)
            modal.StartPosition = FormStartPosition.CenterParent
            modal.FormBorderStyle = FormBorderStyle.FixedDialog
            modal.MaximizeBox = False
            modal.MinimizeBox = False
            modal.BackColor = Color.White

            modal.Controls.Add(New Label With {.Text = "Expense Category:", .Location = New Point(25, 23), .Size = New Size(120, 22), .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)})
            Dim cboCategory As New ComboBox() With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(150, 20), .Width = 200, .Font = New Font("Segoe UI", 9.5F)}
            cboCategory.Items.AddRange({"Utilities", "Payroll", "Store Supplies", "Maintenance", "Marketing", "Logistics", "Other"})
            cboCategory.SelectedIndex = 0
            modal.Controls.Add(cboCategory)

            modal.Controls.Add(New Label With {.Text = "Amount (₱):", .Location = New Point(25, 63), .Size = New Size(120, 22), .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)})
            Dim numAmount As New NumericUpDown() With {.DecimalPlaces = 2, .Maximum = 1000000, .Location = New Point(150, 60), .Width = 200, .Font = New Font("Segoe UI", 9.5F)}
            modal.Controls.Add(numAmount)

            modal.Controls.Add(New Label With {.Text = "Description:", .Location = New Point(25, 103), .Size = New Size(120, 22), .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)})
            Dim txtDesc As New TextBox() With {.Multiline = True, .Location = New Point(150, 100), .Size = New Size(200, 80), .Font = New Font("Segoe UI", 9.5F)}
            modal.Controls.Add(txtDesc)

            Dim btnSave As New Button() With {.Text = "Save Expense", .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .Size = New Size(130, 38), .Location = New Point(220, 200)}
            AddHandler btnSave.Click, Sub(s, ev)
                                          If numAmount.Value <= 0 Then
                                              MessageBox.Show("Please enter a valid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                              Return
                                          End If
                                          Dim repo As New SalesRepository()
                                          repo.AddExpense(cboCategory.Text, txtDesc.Text, numAmount.Value)
                                          modal.DialogResult = DialogResult.OK
                                          modal.Close()
                                      End Sub
            modal.Controls.Add(btnSave)

            If modal.ShowDialog(Me) = DialogResult.OK Then
                LoadExpenseReportRows()
                LoadDashboardStats() ' Refresh dashboard total
            End If
        End Using
    End Sub

End Class






