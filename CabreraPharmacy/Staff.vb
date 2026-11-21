Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Diagnostics
Imports MySql.Data.MySqlClient

Public Class Staff

    'FOR ROUNDED BUTTONS SA SIDE NAV INI HEHE
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MakeButtonRounded(navTransaction, 15)
        MakeButtonRounded(navRestock, 15)

    End Sub

    Private Sub MakeButtonRounded(btn As Button, radius As Integer, Optional borderColor As Color = Nothing, Optional borderThickness As Integer = 0)
        ' Disable default rectangular border so it doesn't clash with rounded corners
        btn.FlatAppearance.BorderSize = 0

        AddHandler btn.Paint, Sub(sender As Object, e As PaintEventArgs)
                                  Dim g As Graphics = e.Graphics
                                  g.SmoothingMode = SmoothingMode.AntiAlias
                                  g.PixelOffsetMode = PixelOffsetMode.HighQuality

                                  Dim rect As New Rectangle(0, 0, btn.Width, btn.Height)
                                  Using path As GraphicsPath = GetRoundPath(rect, radius),
                                    penSurface As New Pen(btn.Parent.BackColor, 2)

                                      btn.Region = New Region(path)
                                      g.DrawPath(penSurface, path)

                                      ' Draw optional smooth rounded border
                                      If borderThickness > 0 AndAlso borderColor <> Color.Empty Then
                                          Dim rectBorder As Rectangle = Rectangle.Inflate(rect, -borderThickness, -borderThickness)
                                          Using pathBorder As GraphicsPath = GetRoundPath(rectBorder, radius - borderThickness),
                                            penBorder As New Pen(borderColor, borderThickness)
                                              g.DrawPath(penBorder, pathBorder)
                                          End Using
                                      End If
                                  End Using
                              End Sub

        ' Redraw properly if the form resizes (important for anchored buttons)
        AddHandler btn.Resize, Sub(sender As Object, e As EventArgs)
                                   btn.Invalidate()
                               End Sub
    End Sub

    Private Function GetRoundPath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim curveSize As Single = radius * 2.0F

        path.StartFigure()
        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90)
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90)
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90)
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90)
        path.CloseFigure()

        Return path
    End Function

    'HANGGANG DITO

    Private Class CartItem
        Public ProductID As Integer
        Public BrandName As String
        Public GenericName As String
        Public Dosage As String
        Public Unit As String
        Public Price As Decimal
        Public Quantity As Integer
        Public IsRx As Boolean

        Public ReadOnly Property LineTotal As Decimal
            Get
                Return Price * Quantity
            End Get
        End Property
    End Class

    Private Class WholesaleCustomerRecord
        Public Property CustomerId As Integer
        Public Property CompanyName As String
        Public Property LicenseNumber As String
        Public Property LicenseExpiryDate As Date?
        Public Property FirstName As String
        Public Property LastName As String
        Public Property ContactNumber As String
        Public Property Email As String
        Public Property ShippingAddressLine As String
        Public Property Barangay As String
        Public Property CityMunicipality As String
        Public Property Province As String
        Public Property PostalCode As String
        Public Property PreferredPaymentMethod As String
        Public Property PreferredPaymentProvider As String
        Public Property PaymentAccountNumber As String
        Public Property PaymentAccountName As String
        Public Property IsActive As Boolean
    End Class

    Private ReadOnly darkGreen As Color = Color.FromArgb(20, 100, 60)
    Private ReadOnly cart As New List(Of CartItem)()
    Private transactionFiltersPanel As Panel
    Private inventoryWarningsLabel As Label
    Private products As DataTable
    Private currentProduct As DataRow
    Private isWholesale As Boolean
    Private checkoutModeIsWholesale As Boolean
    Private clearClickedSelection As Boolean
    Private selectedParentCategory As String = "All"
    Private updatingCategoryFilter As Boolean
    Private wholesaleCustomerGrid As DataGridView
    Private wholesaleCustomerSearchBox As TextBox
    Private wholesaleCustomerDeleteButton As Button
    Private selectedWholesaleCustomer As WholesaleCustomerRecord
    Private wholesaleClientReviewPanel As Panel
    Private wholesaleReviewCompanyLabel As Label
    Private wholesaleReviewLicenseLabel As Label
    Private wholesaleReviewContactLabel As Label
    Private wholesaleReviewPhoneLabel As Label
    Private wholesaleReviewAddressLabel As Label
    Private wholesalePurchaseOrderPanel As Panel
    Private wholesalePurchaseOrderNumberLabel As Label
    Private wholesalePurchaseOrderDateLabel As Label
    Private wholesalePaymentModeLabel As Label
    Private wholesalePaymentModeCombo As ComboBox
    Private wholesalePaymentAccountNameLabel As Label
    Private wholesalePaymentAccountNameBox As TextBox
    Private wholesalePaymentReferenceLabel As Label
    Private wholesalePaymentReferenceBox As TextBox
    Private currentWholesalePurchaseOrderNumber As String = String.Empty
    Private currentWholesalePurchaseOrderDate As Date = Date.Today
    Private updatingWholesalePaymentUi As Boolean

    ' Restock View Controls
    Private restockGrid As DataGridView
    Private restockBatchesGrid As DataGridView
    Private restockSearchBox As TextBox
    Private restockAllFilterBtn As Button
    Private restockLowFilterBtn As Button
    Private restockFilterLowStockOnly As Boolean = False
    Private restockSelectedProductId As Integer = 0
    Private restockSelectedTitleLabel As Label
    Private restockSelectedStockLabel As Label
    Private restockPlacementTextBox As TextBox
    Private restockSavePlacementButton As Button
    Private restockBatchNoTextBox As TextBox
    Private restockQuantityNumeric As NumericUpDown
    Private restockExpiryDatePicker As DateTimePicker
    Private restockSupplierCombo As ComboBox
    Private restockSubmitButton As Button

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        ApplyStaffVisualTheme()
        CombineTransactionFilterPanels()
        welcomestaff_label.Text = CurrentFullName
        userRoleLabel.Text = If(String.IsNullOrWhiteSpace(CurrentRole), "Staff", CurrentRole)
        LoadStaffHeaderAvatar()
        searchFilterCombo.SelectedIndex = 0
        searchTextBox.PlaceholderText = "Search by brand, generic name, or placement..."
        AddHandler quantityPanel.Resize, AddressOf QuantityPanel_Resize
        AddHandler modePanel.Resize, AddressOf ModePanel_Resize
        AddHandler parentFilterPanel.Resize, AddressOf ParentFilterPanel_Resize
        AddHandler cartCard.Resize, AddressOf CartCard_Resize
        AddHandler checkoutView.Resize, AddressOf CheckoutView_Resize
        AddHandler transactionView.Resize, AddressOf TransactionView_Resize
        AddHandler transactionLeftPanel.Resize, AddressOf TransactionView_Resize
        AddHandler rxNoTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxPractitionerTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxLicenseTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxDateTextBox.ValueChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler qtyTextBox.TextChanged, AddressOf QuantityInputs_TextChanged
        AddHandler customerNameTextBox.TextChanged, AddressOf CustomerIdentity_TextChanged
        AddHandler idNumberTextBox.TextChanged, AddressOf CustomerIdentity_TextChanged
        InitializeWholesaleCheckoutUi()
        InitializeWholesaleCustomerUi()
        AddHandler cashTextBox.KeyPress, AddressOf cashTextBox_KeyPress
        AddHandler resultsGrid.CellMouseDown, AddressOf resultsGrid_CellMouseDown
        AddHandler resultsGrid.CellClick, AddressOf resultsGrid_CellClick
        InitializeInventoryWarningUi()
        InitializeRestockUi()
        ApplyStaffSurfaceTheme()
        QuantityPanel_Resize(quantityPanel, EventArgs.Empty)
        ModePanel_Resize(modePanel, EventArgs.Empty)
        ParentFilterPanel_Resize(parentFilterPanel, EventArgs.Empty)
        CartCard_Resize(cartCard, EventArgs.Empty)
        CheckoutView_Resize(checkoutView, EventArgs.Empty)
        customerTypeCombo.SelectedIndex = 0
        checkoutView.Visible = False
        restockView.Visible = False
        transactionView.Visible = True
        SetStaffActiveNavigation("Transaction")
        SetTransactionMode(False)
        UpdateTransactionScrollLayout()
        RenderCart()
        LoadProducts()
        UiSelectionStyler.ApplyToForm(Me)
        ApplyStaffGridSelectionColors(Me)
    End Sub

    Private Sub LoadStaffHeaderAvatar()
        If userAvatar Is Nothing Then Return

        Dim photoBytes As Byte() = Nothing
        Dim initials As String = "ST"

        If Not String.IsNullOrWhiteSpace(CurrentFullName) Then
            Dim parts = CurrentFullName.Trim().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            If parts.Length = 1 Then
                initials = parts(0).Substring(0, Math.Min(2, parts(0).Length)).ToUpperInvariant()
            ElseIf parts.Length > 1 Then
                initials = (parts(0)(0).ToString() & parts(parts.Length - 1)(0).ToString()).ToUpperInvariant()
            End If
        End If

        Try
            Using conn = GetConnection()
                conn.Open()
                Try
                    Using ensureCmd As New MySqlCommand("ALTER TABLE users ADD COLUMN IF NOT EXISTS profile_photo LONGBLOB DEFAULT NULL;", conn)
                        ensureCmd.ExecuteNonQuery()
                    End Using
                Catch
                End Try
                Const sql = "SELECT staff_initials, profile_photo FROM users WHERE user_id = @id OR full_name = @fullName LIMIT 1"
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@id", CurrentUserID)
                    cmd.Parameters.AddWithValue("@fullName", If(CurrentFullName, ""))
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            If Not reader.IsDBNull(0) AndAlso Not String.IsNullOrWhiteSpace(reader.GetString(0)) Then
                                initials = reader.GetString(0).Trim()
                            End If
                            If Not reader.IsDBNull(1) Then
                                photoBytes = DirectCast(reader.GetValue(1), Byte())
                            End If
                        End If
                    End Using
                End Using
            End Using
        Catch
        End Try

        For Each child As Control In userAvatar.Controls.Cast(Of Control)().ToArray()
            child.Visible = False
        Next

        Dim diameter As Integer = 40
        userAvatar.Region = Nothing
        userAvatar.Size = New Size(diameter, diameter)
        userAvatar.BackColor = Color.White
        userAvatar.BackgroundImageLayout = ImageLayout.Stretch
        Dim oldImage = userAvatar.BackgroundImage
        userAvatar.BackgroundImage = SuperAdmin.CreateCircularAvatarImage(photoBytes, initials, diameter)
        oldImage?.Dispose()

        Dim parentPanel = userAvatar.Parent
        If parentPanel IsNot Nothing Then
            welcomestaff_label.Text = CurrentFullName
            welcomestaff_label.Location = New Point(12, 8)
            userRoleLabel.Location = New Point(12, 30)

            Dim textRight = Math.Max(welcomestaff_label.Right, userRoleLabel.Right)
            userAvatar.Location = New Point(textRight + 14, Math.Max(4, (parentPanel.Height - diameter) \ 2))
            parentPanel.Width = userAvatar.Right + 12

            If headerPanel IsNot Nothing Then
                parentPanel.Location = New Point(headerPanel.ClientSize.Width - parentPanel.Width - 20,
                                                 Math.Max(0, (headerPanel.ClientSize.Height - parentPanel.Height) \ 2))
            End If
        End If
    End Sub

    Private Sub sidebar_Resize(sender As Object, e As EventArgs) Handles sidebar.Resize
        PositionLogoutButton()
    End Sub

    Private Sub PositionLogoutButton()
        If sidebar Is Nothing OrElse logoutstaff_button Is Nothing Then Return
        logoutstaff_button.Visible = True
        logoutstaff_button.BringToFront()
        logoutstaff_button.SetBounds(19,
                                     Math.Max(20, sidebar.ClientSize.Height - logoutstaff_button.Height - 24),
                                     Math.Max(100, sidebar.ClientSize.Width - 38),
                                     34)
    End Sub

    Private Sub ApplyStaffVisualTheme()
        Dim green = darkGreen
        sidebar.BackColor = green
        mainLayout.ColumnStyles(0).SizeType = SizeType.Absolute
        mainLayout.ColumnStyles(0).Width = 168
        rightLayout.RowStyles(0).Height = 108

        logoBox.SetBounds(12, 23, 62, 50)
        PharName.AutoSize = False
        PharName.SetBounds(80, 16, 80, 68)
        PharName.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        PharName.ForeColor = Color.White

        For Each button As Button In {navTransaction, navRestock}
            button.SetBounds(19, If(button Is navTransaction, 92, 138), 130, 38)
            button.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
            button.Padding = New Padding(10, 0, 0, 0)
            button.TextAlign = ContentAlignment.MiddleLeft
            button.FlatStyle = FlatStyle.Flat
            button.FlatAppearance.BorderSize = 0
            button.UseVisualStyleBackColor = False
        Next

        titleLabel.AutoSize = False
        titleLabel.Font = New Font("Segoe UI", 24.0F, FontStyle.Bold)
        titleLabel.ForeColor = green
        titleLabel.SetBounds(16, 27, 400, 36)
        subtitleLabel.AutoSize = False
        subtitleLabel.Font = New Font("Segoe UI", 9.0F)
        subtitleLabel.ForeColor = Color.Gray
        subtitleLabel.SetBounds(24, 65, 520, 16)
        headerPanel.BackColor = Color.White
        bodyPanel.BackColor = Color.FromArgb(245, 245, 245)
        bodyPanel.Padding = New Padding(20, 12, 20, 12)

        logoutstaff_button.FlatStyle = FlatStyle.Flat
        logoutstaff_button.BackColor = green
        logoutstaff_button.ForeColor = Color.White
        logoutstaff_button.Font = New Font("Segoe UI", 9.0F)
        logoutstaff_button.UseVisualStyleBackColor = False
        logoutstaff_button.FlatAppearance.BorderColor = Color.White
        logoutstaff_button.FlatAppearance.BorderSize = 1
        PositionLogoutButton()
    End Sub

    Private Sub ApplyStaffSurfaceTheme()
        transactionLeftPanel.BackColor = Color.FromArgb(245, 245, 245)
        transactionLeftLayout.BackColor = Color.FromArgb(245, 245, 245)
        For Each card As Panel In {transactionFiltersPanel, resultsPanel,
                                   quantityPanel, transactionDetailsPanel, cartCard,
                                   orderReviewCard, paymentCard, restockCard,
                                   wholesaleDetailsPanel, prescriptionPanel}
            If card Is Nothing Then Continue For
            card.BackColor = Color.White
            card.BorderStyle = BorderStyle.None
            Dim radius = If(card Is cartCard OrElse card Is orderReviewCard OrElse card Is paymentCard OrElse card Is restockCard, 14, 10)
            Dim currentCard = card
            Dim currentRadius = radius
            AddHandler currentCard.Resize, Sub(sender, e) ApplyStaffRoundedCorners(currentCard, currentRadius)
            ApplyStaffRoundedCorners(currentCard, currentRadius)
        Next
        For Each button As Button In {addToCartButton, customerSearchButton, checkoutButton,
                                      backToPosButton, cancelButton, confirmButton, searchButton}
            If button Is Nothing Then Continue For
            button.FlatStyle = FlatStyle.Flat
            button.UseVisualStyleBackColor = False
            button.Cursor = Cursors.Hand
            button.FlatAppearance.BorderSize = If(button.BackColor = darkGreen, 0, 1)
            button.FlatAppearance.BorderColor = Color.FromArgb(215, 224, 219)
            AddHandler button.Resize, Sub(sender, e) ApplyStaffRoundedCorners(DirectCast(sender, Control), 8)
            ApplyStaffRoundedCorners(button, 8)
        Next
        PositionLogoutButton()
    End Sub

    Private Sub CombineTransactionFilterPanels()
        Dim filterLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 3,
            .BackColor = Color.Transparent,
            .Padding = New Padding(8),
            .Margin = Padding.Empty
        }
        filterLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        filterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 55.0F))
        filterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 55.0F))
        filterLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48.0F))

        For Each panel As Panel In {modePanel, parentFilterPanel, searchPanel}
            panel.BackColor = Color.Transparent
            panel.BorderStyle = BorderStyle.None
            panel.Dock = DockStyle.Fill
            panel.Margin = Padding.Empty
        Next
        filterLayout.Controls.Add(modePanel, 0, 0)
        filterLayout.Controls.Add(parentFilterPanel, 0, 1)
        filterLayout.Controls.Add(searchPanel, 0, 2)

        transactionFiltersPanel = New Panel With {
            .Name = "transactionFiltersPanel",
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .Padding = Padding.Empty,
            .Margin = New Padding(3, 4, 3, 4)
        }
        transactionFiltersPanel.Controls.Add(filterLayout)

        transactionLeftLayout.SuspendLayout()
        transactionLeftLayout.Controls.Clear()
        transactionLeftLayout.RowCount = 4
        transactionLeftLayout.RowStyles.Clear()
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 174.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 380.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 76.0F))
        transactionLeftLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 160.0F))
        transactionLeftLayout.Controls.Add(transactionFiltersPanel, 0, 0)
        transactionLeftLayout.Controls.Add(resultsPanel, 0, 1)
        transactionLeftLayout.Controls.Add(quantityPanel, 0, 2)
        transactionLeftLayout.Controls.Add(transactionDetailsPanel, 0, 3)
        transactionLeftLayout.ResumeLayout(True)
    End Sub

    Private Sub ApplyStaffGridSelectionColors(root As Control)
        If root Is Nothing Then Return
        Dim grid = TryCast(root, DataGridView)
        If grid IsNot Nothing Then
            Dim selectionColor = Color.FromArgb(218, 240, 229)
            Dim selectedTextColor = Color.FromArgb(20, 70, 48)
            grid.DefaultCellStyle.SelectionBackColor = selectionColor
            grid.DefaultCellStyle.SelectionForeColor = selectedTextColor
            grid.RowsDefaultCellStyle.SelectionBackColor = selectionColor
            grid.RowsDefaultCellStyle.SelectionForeColor = selectedTextColor
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectionColor
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = selectedTextColor
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor
            grid.RowHeadersDefaultCellStyle.SelectionBackColor = grid.RowHeadersDefaultCellStyle.BackColor
            grid.RowHeadersDefaultCellStyle.SelectionForeColor = grid.RowHeadersDefaultCellStyle.ForeColor
        End If
        AddHandler root.ControlAdded, AddressOf StaffGridSelection_ControlAdded
        For Each child As Control In root.Controls
            ApplyStaffGridSelectionColors(child)
        Next
    End Sub

    Private Sub StaffGridSelection_ControlAdded(sender As Object, e As ControlEventArgs)
        ApplyStaffGridSelectionColors(e.Control)
    End Sub

    Private Sub SetStaffActiveNavigation(activePage As String)
        For Each button As Button In {navTransaction, navRestock}
            Dim isActive = String.Equals(Convert.ToString(button.Tag), activePage, StringComparison.OrdinalIgnoreCase)
            button.BackColor = If(isActive, Color.White, darkGreen)
            button.ForeColor = If(isActive, darkGreen, Color.White)
            button.UseVisualStyleBackColor = False
        Next
    End Sub

    Private Sub ApplyStaffRoundedCorners(control As Control, radius As Integer)
        If control Is Nothing OrElse control.Width <= 0 OrElse control.Height <= 0 Then Return
        Dim diameter = Math.Min(radius * 2, Math.Min(control.Width, control.Height))
        Dim bounds As New Rectangle(0, 0, control.Width, control.Height)
        Using path As New GraphicsPath()
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

    ' ==========================================
    ' RESTOCK VIEW (BATCH ENTRY & PLACEMENT)
    ' ==========================================
    Private Sub InitializeRestockUi()
        If restockView Is Nothing Then Return
        restockView.SuspendLayout()
        restockView.Controls.Clear()
        restockView.BackColor = Color.FromArgb(245, 245, 245)

        Dim rootLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2,
            .BackColor = Color.FromArgb(245, 245, 245),
            .Margin = New Padding(0)
        }
        rootLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 104.0F))
        rootLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        Dim topCard As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(0, 0, 0, 12)}
        AddHandler topCard.Resize, Sub(s, e) ApplyStaffRoundedCorners(topCard, 12)
        topCard.Controls.Add(New Label With {.Text = "Restock & Shelf Placement", .AutoSize = True, .Font = New Font("Segoe UI", 13.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(18, 10)})
        topCard.Controls.Add(New Label With {.Text = "Record incoming supplier batches and manage product shelf placements.", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(19, 36)})

        restockAllFilterBtn = New Button With {
            .Text = "All Products",
            .Size = New Size(112, 32),
            .Location = New Point(18, 58),
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .UseVisualStyleBackColor = False
        }
        restockAllFilterBtn.FlatAppearance.BorderColor = darkGreen

        restockLowFilterBtn = New Button With {
            .Text = "Low Stock Only",
            .Size = New Size(124, 32),
            .Location = New Point(136, 58),
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .UseVisualStyleBackColor = False
        }
        restockLowFilterBtn.FlatAppearance.BorderColor = darkGreen

        restockSearchBox = New TextBox With {
            .Location = New Point(276, 61),
            .Width = 300,
            .Font = New Font("Segoe UI", 9.0F),
            .PlaceholderText = "Search brand, generic name, placement, or supplier..."
        }

        Dim refreshBtn As New Button With {
            .Text = "Refresh",
            .Size = New Size(96, 32),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Right,
            .BackColor = Color.White,
            .ForeColor = darkGreen,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .UseVisualStyleBackColor = False
        }
        refreshBtn.FlatAppearance.BorderColor = darkGreen
        refreshBtn.Location = New Point(topCard.ClientSize.Width - refreshBtn.Width - 18, 58)
        AddHandler topCard.Resize, Sub(s, e) refreshBtn.Left = topCard.ClientSize.Width - refreshBtn.Width - 18

        AddHandler restockAllFilterBtn.Click, Sub(s, e)
                                                  restockFilterLowStockOnly = False
                                                  UpdateRestockFilterButtons()
                                                  BindRestockGrid()
                                              End Sub
        AddHandler restockLowFilterBtn.Click, Sub(s, e)
                                                  restockFilterLowStockOnly = True
                                                  UpdateRestockFilterButtons()
                                                  BindRestockGrid()
                                              End Sub
        AddHandler restockSearchBox.TextChanged, Sub(s, e) BindRestockGrid()
        AddHandler refreshBtn.Click, Sub(s, e) LoadRestockProducts()

        topCard.Controls.Add(restockAllFilterBtn)
        topCard.Controls.Add(restockLowFilterBtn)
        topCard.Controls.Add(restockSearchBox)
        topCard.Controls.Add(refreshBtn)
        UpdateRestockFilterButtons()

        Dim bodyLayout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .BackColor = Color.FromArgb(245, 245, 245),
            .Margin = New Padding(0)
        }
        bodyLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 64.0F))
        bodyLayout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 36.0F))
        bodyLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))

        Dim leftCard As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(12), .Margin = New Padding(0, 0, 12, 0)}
        AddHandler leftCard.Resize, Sub(s, e) ApplyStaffRoundedCorners(leftCard, 14)

        restockGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            .GridColor = Color.FromArgb(225, 235, 230),
            .EnableHeadersVisualStyles = False,
            .ColumnHeadersHeight = 40,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        }
        restockGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        restockGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        restockGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
        restockGrid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        restockGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 240, 229)
        restockGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 70, 48)
        restockGrid.RowTemplate.Height = 36
        AddHandler restockGrid.SelectionChanged, AddressOf RestockGrid_SelectionChanged
        AddHandler restockGrid.CellFormatting, AddressOf RestockGrid_CellFormatting
        leftCard.Controls.Add(restockGrid)

        Dim rightCard As New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(18), .AutoScroll = True, .Margin = New Padding(0)}
        AddHandler rightCard.Resize, Sub(s, e) ApplyStaffRoundedCorners(rightCard, 14)

        restockSelectedTitleLabel = New Label With {
            .Text = "Select a product to restock",
            .AutoSize = False,
            .Size = New Size(360, 26),
            .Font = New Font("Segoe UI", 11.5F, FontStyle.Bold),
            .ForeColor = darkGreen,
            .Location = New Point(18, 16)
        }
        restockSelectedStockLabel = New Label With {
            .Text = "Choose an item from the list on the left.",
            .AutoSize = False,
            .Size = New Size(360, 22),
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Color.Gray,
            .Location = New Point(18, 44)
        }

        rightCard.Controls.Add(New Label With {.Text = "Item Placement (Shelf / Cabinet)", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .ForeColor = Color.FromArgb(55, 65, 60), .Location = New Point(18, 78)})
        restockPlacementTextBox = New TextBox With {.Location = New Point(18, 98), .Width = 220, .Font = New Font("Segoe UI", 9.5F), .PlaceholderText = "e.g., Shelf A - Row 1", .Enabled = False}
        restockSavePlacementButton = New Button With {
            .Text = "Save Placement",
            .Size = New Size(118, 28),
            .Location = New Point(246, 97),
            .BackColor = Color.White,
            .ForeColor = darkGreen,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .Enabled = False,
            .UseVisualStyleBackColor = False
        }
        restockSavePlacementButton.FlatAppearance.BorderColor = darkGreen
        AddHandler restockSavePlacementButton.Click, AddressOf RestockSavePlacement_Click

        rightCard.Controls.Add(New Label With {.Text = "INCOMING BATCH DETAILS", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(18, 140)})

        rightCard.Controls.Add(New Label With {.Text = "Batch Number *", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(18, 166)})
        restockBatchNoTextBox = New TextBox With {.Location = New Point(18, 186), .Width = 346, .Font = New Font("Segoe UI", 9.5F), .CharacterCasing = CharacterCasing.Upper, .PlaceholderText = "e.g., BGC261120", .Enabled = False}

        rightCard.Controls.Add(New Label With {.Text = "Quantity to Add (Units) *", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(18, 222)})
        restockQuantityNumeric = New NumericUpDown With {.Location = New Point(18, 242), .Width = 165, .Minimum = 1, .Maximum = 100000, .Value = 10, .Font = New Font("Segoe UI", 9.5F), .Enabled = False}

        rightCard.Controls.Add(New Label With {.Text = "Expiry Date *", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(199, 222)})
        restockExpiryDatePicker = New DateTimePicker With {
            .Location = New Point(199, 242),
            .Width = 165,
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "MMM dd, yyyy",
            .MinDate = Date.Today.AddDays(1),
            .MaxDate = Date.Today.AddYears(10),
            .Value = Date.Today.AddYears(1),
            .Font = New Font("Segoe UI", 9.5F),
            .Enabled = False
        }

        rightCard.Controls.Add(New Label With {.Text = "Supplier", .AutoSize = True, .Font = New Font("Segoe UI", 8.5F), .ForeColor = Color.Gray, .Location = New Point(18, 280)})
        restockSupplierCombo = New ComboBox With {.Location = New Point(18, 300), .Width = 346, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9.5F), .Enabled = False}

        restockSubmitButton = New Button With {
            .Text = "RECORD RESTOCK",
            .Size = New Size(346, 40),
            .Location = New Point(18, 342),
            .BackColor = darkGreen,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
            .Cursor = Cursors.Hand,
            .Enabled = False,
            .UseVisualStyleBackColor = False
        }
        restockSubmitButton.FlatAppearance.BorderSize = 0
        AddHandler restockSubmitButton.Resize, Sub(s, e) ApplyStaffRoundedCorners(restockSubmitButton, 8)
        AddHandler restockSubmitButton.Click, AddressOf RestockSubmit_Click

        rightCard.Controls.Add(New Label With {.Text = "EXISTING BATCHES FOR SELECTED PRODUCT", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold), .ForeColor = Color.Gray, .Location = New Point(18, 398)})
        restockBatchesGrid = New DataGridView With {
            .Location = New Point(18, 420),
            .Size = New Size(346, 170),
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.FixedSingle,
            .GridColor = Color.FromArgb(230, 234, 231),
            .EnableHeadersVisualStyles = False
        }
        restockBatchesGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(225, 245, 235)
        restockBatchesGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(25, 55, 45)
        restockBatchesGrid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.0F, FontStyle.Bold)
        restockBatchesGrid.DefaultCellStyle.Font = New Font("Segoe UI", 8.5F)
        restockBatchesGrid.RowTemplate.Height = 28

        rightCard.Controls.Add(restockSelectedTitleLabel)
        rightCard.Controls.Add(restockSelectedStockLabel)
        rightCard.Controls.Add(restockPlacementTextBox)
        rightCard.Controls.Add(restockSavePlacementButton)
        rightCard.Controls.Add(restockBatchNoTextBox)
        rightCard.Controls.Add(restockQuantityNumeric)
        rightCard.Controls.Add(restockExpiryDatePicker)
        rightCard.Controls.Add(restockSupplierCombo)
        rightCard.Controls.Add(restockSubmitButton)
        rightCard.Controls.Add(restockBatchesGrid)

        AddHandler rightCard.Resize,
            Sub(s, e)
                Dim fieldWidth = Math.Max(260, rightCard.ClientSize.Width - 36)
                restockSelectedTitleLabel.Width = fieldWidth
                restockSelectedStockLabel.Width = fieldWidth
                restockSavePlacementButton.Left = 18 + fieldWidth - restockSavePlacementButton.Width
                restockPlacementTextBox.Width = Math.Max(120, restockSavePlacementButton.Left - 26)
                restockBatchNoTextBox.Width = fieldWidth
                Dim halfWidth = (fieldWidth - 16) \ 2
                restockQuantityNumeric.Width = halfWidth
                restockExpiryDatePicker.Left = 18 + halfWidth + 16
                restockExpiryDatePicker.Width = fieldWidth - halfWidth - 16
                restockSupplierCombo.Width = fieldWidth
                restockSubmitButton.Width = fieldWidth
                restockBatchesGrid.Width = fieldWidth
                restockBatchesGrid.Height = Math.Max(140, rightCard.ClientSize.Height - 438)
            End Sub

        bodyLayout.Controls.Add(leftCard, 0, 0)
        bodyLayout.Controls.Add(rightCard, 1, 0)
        rootLayout.Controls.Add(topCard, 0, 0)
        rootLayout.Controls.Add(bodyLayout, 0, 1)
        restockView.Controls.Add(rootLayout)
        restockView.ResumeLayout(True)
    End Sub

    Private Sub UpdateRestockFilterButtons()
        If restockAllFilterBtn Is Nothing OrElse restockLowFilterBtn Is Nothing Then Return
        restockAllFilterBtn.BackColor = If(Not restockFilterLowStockOnly, darkGreen, Color.White)
        restockAllFilterBtn.ForeColor = If(Not restockFilterLowStockOnly, Color.White, darkGreen)
        restockLowFilterBtn.BackColor = If(restockFilterLowStockOnly, darkGreen, Color.White)
        restockLowFilterBtn.ForeColor = If(restockFilterLowStockOnly, Color.White, darkGreen)
    End Sub

    Private Sub LoadRestockProducts()
        Try
            Dim pm As New ProductManager()
            products = pm.GetProducts(includeInactive:=False)
            If products Is Nothing Then products = New DataTable()

            Dim suppliers = pm.GetActiveSuppliers()
            restockSupplierCombo.DataSource = suppliers
            restockSupplierCombo.DisplayMember = "company_name"
            restockSupplierCombo.ValueMember = "supplier_ID"

            BindRestockGrid()
        Catch ex As Exception
            MessageBox.Show("Could not load restock inventory: " & ex.Message, "Restock", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BindRestockGrid()
        If restockGrid Is Nothing OrElse products Is Nothing Then Return

        Dim dt As New DataTable()
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("Item Placement", GetType(String))
        dt.Columns.Add("Brand Name", GetType(String))
        dt.Columns.Add("Generic Name", GetType(String))
        dt.Columns.Add("Stock Qty", GetType(Integer))
        dt.Columns.Add("Reorder Point", GetType(Integer))
        dt.Columns.Add("Supplier", GetType(String))
        dt.Columns.Add("Status", GetType(String))

        Dim keyword = If(restockSearchBox IsNot Nothing, restockSearchBox.Text.Trim(), String.Empty)

        For Each row As DataRow In products.Rows
            Dim stockQty = If(row.IsNull("Stock Quantity"), 0, Convert.ToInt32(row("Stock Quantity")))
            Dim reorderPt = If(row.IsNull("Reorder Point"), 0, Convert.ToInt32(row("Reorder Point")))
            Dim isLow = stockQty <= reorderPt
            If restockFilterLowStockOnly AndAlso Not isLow Then Continue For

            Dim placement = If(products.Columns.Contains("Item Placement") AndAlso Not row.IsNull("Item Placement"), Convert.ToString(row("Item Placement")), "Unassigned")
            Dim brand = Convert.ToString(row("Brand Name"))
            Dim genericName = Convert.ToString(row("Generic Name"))
            Dim supplier = Convert.ToString(row("Supplier"))
            Dim searchable = String.Join(" ", {placement, brand, genericName, supplier})
            If keyword.Length > 0 AndAlso searchable.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0 Then Continue For

            dt.Rows.Add(Convert.ToInt32(row("ID")), placement, brand, genericName, stockQty, reorderPt, supplier, If(isLow, "Low Stock", "In Stock"))
        Next

        restockGrid.DataSource = dt
        If restockGrid.Columns.Contains("ID") Then restockGrid.Columns("ID").Visible = False
        If restockGrid.Columns.Contains("Item Placement") Then restockGrid.Columns("Item Placement").FillWeight = 95
        If restockGrid.Columns.Contains("Brand Name") Then restockGrid.Columns("Brand Name").FillWeight = 105
        If restockGrid.Columns.Contains("Generic Name") Then restockGrid.Columns("Generic Name").FillWeight = 125
        If restockGrid.Columns.Contains("Stock Qty") Then restockGrid.Columns("Stock Qty").FillWeight = 65
        If restockGrid.Columns.Contains("Reorder Point") Then restockGrid.Columns("Reorder Point").FillWeight = 75
        If restockGrid.Columns.Contains("Supplier") Then restockGrid.Columns("Supplier").FillWeight = 105
        If restockGrid.Columns.Contains("Status") Then restockGrid.Columns("Status").FillWeight = 70

        restockGrid.ClearSelection()
        restockGrid.CurrentCell = Nothing
        SetRestockFormState(Nothing)
    End Sub

    Private Sub RestockGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse restockGrid Is Nothing OrElse Not restockGrid.Columns.Contains("Status") Then Return
        If restockGrid.Columns(e.ColumnIndex).Name <> "Status" Then Return
        Dim val = Convert.ToString(e.Value)
        If String.Equals(val, "Low Stock", StringComparison.OrdinalIgnoreCase) Then
            e.CellStyle.ForeColor = Color.FromArgb(180, 45, 45)
            e.CellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        Else
            e.CellStyle.ForeColor = darkGreen
        End If
    End Sub

    Private Sub RestockGrid_SelectionChanged(sender As Object, e As EventArgs)
        If restockGrid Is Nothing OrElse restockGrid.SelectedRows.Count = 0 OrElse restockGrid.CurrentRow Is Nothing OrElse restockGrid.CurrentRow.IsNewRow Then
            SetRestockFormState(Nothing)
            Return
        End If
        Dim prodId As Integer
        If Not Integer.TryParse(Convert.ToString(restockGrid.CurrentRow.Cells("ID").Value), prodId) OrElse products Is Nothing Then
            SetRestockFormState(Nothing)
            Return
        End If
        Dim match = products.AsEnumerable().FirstOrDefault(Function(r) Convert.ToInt32(r("ID")) = prodId)
        SetRestockFormState(match)
    End Sub

    Private Sub SetRestockFormState(productRow As DataRow)
        Dim hasSelection = productRow IsNot Nothing
        restockPlacementTextBox.Enabled = hasSelection
        restockSavePlacementButton.Enabled = hasSelection
        restockBatchNoTextBox.Enabled = hasSelection
        restockQuantityNumeric.Enabled = hasSelection
        restockExpiryDatePicker.Enabled = hasSelection
        restockSupplierCombo.Enabled = hasSelection
        restockSubmitButton.Enabled = hasSelection
        restockSubmitButton.BackColor = If(hasSelection, darkGreen, Color.LightGray)
        restockSubmitButton.ForeColor = If(hasSelection, Color.White, Color.DimGray)

        If Not hasSelection Then
            restockSelectedProductId = 0
            restockSelectedTitleLabel.Text = "Select a product to restock"
            restockSelectedStockLabel.Text = "Choose an item from the list on the left."
            restockPlacementTextBox.Clear()
            restockBatchNoTextBox.Clear()
            restockBatchesGrid.DataSource = Nothing
            Return
        End If

        restockSelectedProductId = Convert.ToInt32(productRow("ID"))
        Dim brand = Convert.ToString(productRow("Brand Name"))
        Dim dosage = Convert.ToString(productRow("Dosage"))
        Dim unit = Convert.ToString(productRow("Unit"))
        Dim stockQty = If(productRow.IsNull("Stock Quantity"), 0, Convert.ToInt32(productRow("Stock Quantity")))
        Dim reorderPt = If(productRow.IsNull("Reorder Point"), 0, Convert.ToInt32(productRow("Reorder Point")))
        Dim placement = If(productRow.Table.Columns.Contains("Item Placement") AndAlso Not productRow.IsNull("Item Placement"), Convert.ToString(productRow("Item Placement")), "Unassigned")
        Dim supplierName = Convert.ToString(productRow("Supplier"))

        restockSelectedTitleLabel.Text = brand & " (" & dosage & " " & unit & ")"
        restockSelectedStockLabel.Text = "Current Stock: " & stockQty.ToString() & " units   |   Reorder Point: " & reorderPt.ToString()
        restockPlacementTextBox.Text = placement

        Dim prefix = New String(brand.Where(Function(ch) Char.IsLetter(ch)).Take(3).ToArray()).ToUpperInvariant()
        If prefix.Length < 3 Then prefix = prefix.PadRight(3, "X"c)
        restockBatchNoTextBox.Text = prefix & DateTime.Now.ToString("yyMMdd", CultureInfo.InvariantCulture) & "-" & DateTime.Now.ToString("HHmm", CultureInfo.InvariantCulture)

        For i = 0 To restockSupplierCombo.Items.Count - 1
            Dim itemView = TryCast(restockSupplierCombo.Items(i), DataRowView)
            If itemView IsNot Nothing AndAlso String.Equals(Convert.ToString(itemView("company_name")), supplierName, StringComparison.OrdinalIgnoreCase) Then
                restockSupplierCombo.SelectedIndex = i
                Exit For
            End If
        Next

        Try
            Dim pm As New ProductManager()
            Dim batches = pm.GetProductBatches(restockSelectedProductId)
            restockBatchesGrid.DataSource = batches
            If restockBatchesGrid.Columns.Contains("Batch ID") Then restockBatchesGrid.Columns("Batch ID").Visible = False
            If restockBatchesGrid.Columns.Contains("Expiry Date") Then restockBatchesGrid.Columns("Expiry Date").DefaultCellStyle.Format = "MM/dd/yyyy"
        Catch
            restockBatchesGrid.DataSource = Nothing
        End Try
    End Sub

    Private Sub RestockSavePlacement_Click(sender As Object, e As EventArgs)
        If restockSelectedProductId <= 0 Then Return
        Try
            Dim pm As New ProductManager()
            pm.UpdateItemPlacement(restockSelectedProductId, restockPlacementTextBox.Text)
            MessageBox.Show("Item placement updated successfully.", "Item Placement", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadProducts()
            LoadRestockProducts()
        Catch ex As Exception
            MessageBox.Show("Could not update item placement: " & ex.Message, "Item Placement", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub RestockSubmit_Click(sender As Object, e As EventArgs)
        If restockSelectedProductId <= 0 Then Return
        Dim batchNo = restockBatchNoTextBox.Text.Trim()
        If String.IsNullOrWhiteSpace(batchNo) Then
            MessageBox.Show("Enter a batch number before recording the restock.", "Restock Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            restockBatchNoTextBox.Focus()
            Return
        End If
        If restockExpiryDatePicker.Value.Date <= Date.Today Then
            MessageBox.Show("Expiry date must be in the future.", "Restock Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            restockExpiryDatePicker.Focus()
            Return
        End If

        Dim supplierId As Integer? = Nothing
        If restockSupplierCombo.SelectedValue IsNot Nothing AndAlso Not IsDBNull(restockSupplierCombo.SelectedValue) Then
            Dim parsedId As Integer
            If Integer.TryParse(Convert.ToString(restockSupplierCombo.SelectedValue), parsedId) Then supplierId = parsedId
        End If

        Try
            Dim pm As New ProductManager()
            pm.RestockProductBatch(
                restockSelectedProductId,
                batchNo,
                restockExpiryDatePicker.Value.Date,
                CInt(restockQuantityNumeric.Value),
                supplierId,
                restockPlacementTextBox.Text.Trim())

            MessageBox.Show("Restock recorded! Batch " & batchNo.ToUpperInvariant() & " (" & CInt(restockQuantityNumeric.Value).ToString() & " units) added to inventory.",
                            "Restock Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadProducts()
            LoadRestockProducts()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show("That batch number already exists for this product. Please enter a unique batch number.", "Duplicate Batch Number", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            restockBatchNoTextBox.Focus()
        Catch ex As Exception
            MessageBox.Show("Could not record restock: " & ex.Message, "Restock Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ==========================================
    ' CHECKOUT & ORDER REVIEW PAYMENT UI
    ' ==========================================
    Private Sub InitializeWholesaleCheckoutUi()
        poNoLabel.Visible = False
        poNoTextBox.Visible = False
        poDateLabel.Visible = False
        poDatePicker.Visible = False
        wholesaleClientReviewPanel = New Panel With {.BackColor = Color.FromArgb(248, 250, 248), .BorderStyle = BorderStyle.FixedSingle, .Visible = False}
        wholesaleReviewCompanyLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold), .ForeColor = darkGreen}
        wholesaleReviewLicenseLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 8.5F)}
        wholesaleReviewContactLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 8.5F)}
        wholesaleReviewPhoneLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 8.5F)}
        wholesaleReviewAddressLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 8.5F)}
        wholesaleClientReviewPanel.Controls.Add(wholesaleReviewCompanyLabel)
        wholesaleClientReviewPanel.Controls.Add(wholesaleReviewLicenseLabel)
        wholesaleClientReviewPanel.Controls.Add(wholesaleReviewContactLabel)
        wholesaleClientReviewPanel.Controls.Add(wholesaleReviewPhoneLabel)
        wholesaleClientReviewPanel.Controls.Add(wholesaleReviewAddressLabel)
        orderReviewCard.Controls.Add(wholesaleClientReviewPanel)

        wholesalePurchaseOrderPanel = New Panel With {.BackColor = Color.White, .BorderStyle = BorderStyle.FixedSingle, .Visible = False}
        wholesalePurchaseOrderPanel.Controls.Add(New Label With {.Text = "PURCHASE ORDER", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(10, 8)})
        wholesalePurchaseOrderPanel.Controls.Add(New Label With {.Text = "PO No.", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray, .Location = New Point(10, 34)})
        wholesalePurchaseOrderPanel.Controls.Add(New Label With {.Text = "PO Date", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray, .Location = New Point(360, 34)})
        wholesalePurchaseOrderNumberLabel = New Label With {.AutoEllipsis = True, .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .BackColor = Color.FromArgb(243, 246, 244), .Location = New Point(70, 30), .Size = New Size(270, 24), .TextAlign = ContentAlignment.MiddleLeft}
        wholesalePurchaseOrderDateLabel = New Label With {.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold), .BackColor = Color.FromArgb(243, 246, 244), .Location = New Point(415, 30), .Size = New Size(160, 24), .TextAlign = ContentAlignment.MiddleLeft}
        wholesalePurchaseOrderPanel.Controls.Add(wholesalePurchaseOrderNumberLabel)
        wholesalePurchaseOrderPanel.Controls.Add(wholesalePurchaseOrderDateLabel)
        orderReviewCard.Controls.Add(wholesalePurchaseOrderPanel)

        ' Shared Payment Mode controls (available for BOTH Retail and Wholesale customers)
        wholesalePaymentModeLabel = New Label With {.Text = "MODE OF PAYMENT", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold), .ForeColor = darkGreen, .Location = New Point(20, 65), .Visible = True}
        wholesalePaymentModeCombo = New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Location = New Point(20, 88), .Size = New Size(300, 26), .Visible = True}
        wholesalePaymentModeCombo.Items.AddRange(New Object() {"Cash", "GCash", "Maya", "Bank Transfer"})
        wholesalePaymentModeCombo.SelectedIndex = 0
        wholesalePaymentAccountNameLabel = New Label With {.Text = "Account Name", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray, .Location = New Point(20, 124), .Visible = False}
        wholesalePaymentAccountNameBox = New TextBox With {.Location = New Point(20, 144), .Size = New Size(300, 25), .PlaceholderText = "Sender / account holder name", .Visible = False}
        wholesalePaymentReferenceLabel = New Label With {.Text = "Reference Number *", .AutoSize = True, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray, .Location = New Point(20, 178), .Visible = False}
        wholesalePaymentReferenceBox = New TextBox With {.Location = New Point(20, 198), .Size = New Size(300, 25), .PlaceholderText = "Enter e-payment reference no.", .Visible = False}
        paymentCard.Controls.Add(wholesalePaymentModeLabel)
        paymentCard.Controls.Add(wholesalePaymentModeCombo)
        paymentCard.Controls.Add(wholesalePaymentAccountNameLabel)
        paymentCard.Controls.Add(wholesalePaymentAccountNameBox)
        paymentCard.Controls.Add(wholesalePaymentReferenceLabel)
        paymentCard.Controls.Add(wholesalePaymentReferenceBox)
        AddHandler wholesalePaymentModeCombo.SelectedIndexChanged, Sub(sender, e) UpdateWholesaleCheckoutPaymentUi()
        AddHandler wholesalePaymentReferenceBox.TextChanged, Sub(sender, e) UpdateConfirmButtonAvailability()
    End Sub

    Private Function IsElectronicPaymentSelected() As Boolean
        Return wholesalePaymentModeCombo IsNot Nothing AndAlso
               wholesalePaymentModeCombo.SelectedItem IsNot Nothing AndAlso
               Not String.Equals(Convert.ToString(wholesalePaymentModeCombo.SelectedItem), "Cash", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub UpdateWholesaleCheckoutPaymentUi()
        If wholesalePaymentModeCombo Is Nothing Then Return
        Dim usesElectronicPayment = IsElectronicPaymentSelected()
        wholesalePaymentAccountNameLabel.Visible = usesElectronicPayment
        wholesalePaymentAccountNameBox.Visible = usesElectronicPayment
        wholesalePaymentReferenceLabel.Visible = usesElectronicPayment
        wholesalePaymentReferenceBox.Visible = usesElectronicPayment

        cashLabel.Text = If(usesElectronicPayment, "AMOUNT PAID:", If(checkoutModeIsWholesale, "CASH RECEIVED:", "CASH AMOUNT:"))
        cashTextBox.Visible = Not usesElectronicPayment
        cashValue.Visible = True

        If Not updatingWholesalePaymentUi Then
            UpdateCheckoutTotals()
            CheckoutView_Resize(checkoutView, EventArgs.Empty)
        End If
    End Sub

    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20,
                                       Math.Max(0, (headerPanel.ClientSize.Height - userPanel.Height) \ 2))
    End Sub

    Private Sub searchFilterCombo_DrawItem(sender As Object, e As DrawItemEventArgs) Handles searchFilterCombo.DrawItem
        If e.Index < 0 Then Return

        Dim isSelected = (e.State And DrawItemState.Selected) = DrawItemState.Selected
        Dim background = If(isSelected, Color.FromArgb(218, 240, 229), Color.White)
        Dim foreground = If(isSelected, Color.FromArgb(20, 100, 60), Color.Black)
        Using brush As New SolidBrush(background)
            e.Graphics.FillRectangle(brush, e.Bounds)
        End Using
        TextRenderer.DrawText(e.Graphics,
                              Convert.ToString(searchFilterCombo.Items(e.Index)),
                              e.Font,
                              e.Bounds,
                              foreground,
                              TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
    End Sub

    Private Sub QuantityPanel_Resize(sender As Object, e As EventArgs)
        Dim centerTop = Math.Max(0, (quantityPanel.ClientSize.Height - addToCartButton.Height) \ 2)
        qtyLabel.Top = Math.Max(0, (quantityPanel.ClientSize.Height - qtyLabel.Height) \ 2)
        qtyMinusButton.Top = centerTop
        qtyTextBox.Top = Math.Max(0, (quantityPanel.ClientSize.Height - qtyTextBox.Height) \ 2)
        qtyPlusButton.Top = centerTop
        addToCartButton.Location = New Point(
            Math.Max(qtyPlusButton.Right + 16, quantityPanel.ClientSize.Width - addToCartButton.Width - 12),
            centerTop)
    End Sub

    Private Sub CartCard_Resize(sender As Object, e As EventArgs)
        If cartCard.ClientSize.Width <= 0 OrElse cartCard.ClientSize.Height <= 0 Then Return

        cartItemsPanel.SetBounds(16, 52,
                                 Math.Max(120, cartCard.ClientSize.Width - 32),
                                 Math.Max(80, cartCard.ClientSize.Height - 145))
        cartSubtotalLabel.Location = New Point(16, cartCard.ClientSize.Height - 72)
        cartSubtotalValue.Location = New Point(cartCard.ClientSize.Width - cartSubtotalValue.Width - 16,
                                                cartCard.ClientSize.Height - 72)
        checkoutButton.Location = New Point(cartCard.ClientSize.Width - checkoutButton.Width - 16,
                                             cartCard.ClientSize.Height - checkoutButton.Height - 12)
    End Sub

    Private Sub CheckoutView_Resize(sender As Object, e As EventArgs)
        If checkoutView.ClientSize.Width <= 0 OrElse checkoutView.ClientSize.Height <= 0 Then Return

        Dim availableWidth = checkoutView.ClientSize.Width - 16
        Dim reviewWidth = CInt(availableWidth * 0.62)
        Dim viewHeight = checkoutView.ClientSize.Height
        orderReviewCard.SetBounds(0, 0, reviewWidth, viewHeight)
        paymentCard.SetBounds(reviewWidth + 16, 0, availableWidth - reviewWidth, viewHeight)

        Dim summaryTop As Integer
        If checkoutModeIsWholesale Then
            wholesaleClientReviewPanel.SetBounds(20, 64, Math.Max(120, reviewWidth - 40), 122)
            wholesaleReviewCompanyLabel.SetBounds(10, 8, wholesaleClientReviewPanel.ClientSize.Width - 20, 20)
            wholesaleReviewLicenseLabel.SetBounds(10, 32, wholesaleClientReviewPanel.ClientSize.Width - 20, 18)
            wholesaleReviewContactLabel.SetBounds(10, 53, wholesaleClientReviewPanel.ClientSize.Width - 20, 18)
            wholesaleReviewPhoneLabel.SetBounds(10, 74, wholesaleClientReviewPanel.ClientSize.Width - 20, 18)
            wholesaleReviewAddressLabel.SetBounds(10, 95, wholesaleClientReviewPanel.ClientSize.Width - 20, 18)
            wholesalePurchaseOrderPanel.SetBounds(20, 196, Math.Max(120, reviewWidth - 40), 64)
            wholesalePurchaseOrderDateLabel.Left = Math.Max(415, wholesalePurchaseOrderPanel.ClientSize.Width - 170)
            wholesalePurchaseOrderNumberLabel.Width = Math.Max(120, wholesalePurchaseOrderDateLabel.Left - wholesalePurchaseOrderNumberLabel.Left - 12)
            wholesalePurchaseOrderDateLabel.Width = Math.Max(120, wholesalePurchaseOrderPanel.ClientSize.Width - wholesalePurchaseOrderDateLabel.Left - 10)
            orderSummaryLabel.Top = 278
            summaryTop = 306
            prescriptionLabel.Visible = False
            prescriptionValueLabel.Visible = False
            rxRecordedLabel.Visible = False
        Else
            orderSummaryLabel.Top = 220
            summaryTop = 250
            prescriptionLabel.Top = Math.Max(520, viewHeight - 95)
            prescriptionValueLabel.Top = prescriptionLabel.Top + 25
            prescriptionLabel.Visible = cart.Any(Function(item) item.IsRx)
            prescriptionValueLabel.Visible = prescriptionLabel.Visible
            rxRecordedLabel.Visible = prescriptionLabel.Visible
            rxRecordedLabel.Left = Math.Max(220, reviewWidth - rxRecordedLabel.Width - 20)
            rxRecordedLabel.Top = prescriptionValueLabel.Top
        End If
        orderSummaryPanel.SetBounds(20, summaryTop, Math.Max(120, reviewWidth - 40), Math.Max(120, viewHeight - summaryTop - 24))

        Dim paymentWidth = paymentCard.ClientSize.Width
        Dim usesElectronicPayment = IsElectronicPaymentSelected()

        If wholesalePaymentModeCombo IsNot Nothing Then
            wholesalePaymentModeCombo.Width = Math.Max(150, paymentWidth - 40)
            wholesalePaymentAccountNameBox.Width = Math.Max(150, paymentWidth - 40)
            wholesalePaymentReferenceBox.Width = Math.Max(150, paymentWidth - 40)
        End If

        Dim totalsTop = If(usesElectronicPayment, 238, 136)
        subtotalLabel.Top = totalsTop
        subtotalValue.Top = subtotalLabel.Top
        discountLabel.Top = totalsTop + 28
        discountValue.Top = discountLabel.Top
        totalDueLabel.Top = totalsTop + 56
        totalDueValue.Top = totalDueLabel.Top
        cashLabel.Top = totalsTop + 108
        changeLabel.Top = cashLabel.Top + 46
        changeValue.Top = changeLabel.Top

        subtotalValue.Left = paymentWidth - subtotalValue.Width - 20
        discountValue.Left = paymentWidth - discountValue.Width - 20
        totalDueValue.Left = paymentWidth - totalDueValue.Width - 20
        cashTextBox.Width = 150
        cashTextBox.Left = paymentWidth - cashTextBox.Width - 20
        cashTextBox.Top = cashLabel.Top - 4
        If usesElectronicPayment Then
            cashValue.Left = paymentWidth - cashValue.Width - 20
            cashValue.Top = cashLabel.Top
        Else
            cashValue.Left = cashTextBox.Left - cashValue.Width - 8
            cashValue.Top = cashTextBox.Top + 3
        End If
        changeValue.Left = paymentWidth - changeValue.Width - 20

        confirmButton.Left = paymentWidth - confirmButton.Width - 20
        cancelButton.Left = confirmButton.Left - cancelButton.Width - 10
        backToPosButton.Left = cancelButton.Left - backToPosButton.Width - 10
        backToPosButton.Top = viewHeight - backToPosButton.Height - 16
        cancelButton.Top = viewHeight - cancelButton.Height - 16
        confirmButton.Top = viewHeight - confirmButton.Height - 16
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs) Handles navTransaction.Click, navRestock.Click
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return

        Dim destination = Convert.ToString(button.Tag)
        Dim leavingCheckout = checkoutView.Visible AndAlso
                              (String.Equals(destination, "Transaction", StringComparison.OrdinalIgnoreCase) OrElse
                               String.Equals(destination, "Restock", StringComparison.OrdinalIgnoreCase))
        Dim leavingPosWithCart = transactionView.Visible AndAlso cart.Count > 0 AndAlso
                                 String.Equals(destination, "Restock", StringComparison.OrdinalIgnoreCase)
        If leavingCheckout OrElse leavingPosWithCart Then
            Dim answer = MessageBox.Show("Are you sure? The current transaction will be canceled.",
                                         "Cancel Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If answer <> DialogResult.Yes Then Return
            CancelCurrentTransaction()
        End If

        transactionView.Visible = (CStr(button.Tag) = "Transaction")
        checkoutView.Visible = False
        restockView.Visible = (CStr(button.Tag) = "Restock")
        titleLabel.Text = CStr(button.Tag)
        subtitleLabel.Text = If(restockView.Visible, "Restock inventory and update shelf placements", "Cabrera's Drugstore and Medical Supplies")
        SetStaffActiveNavigation(Convert.ToString(button.Tag))

        If restockView.Visible Then
            LoadRestockProducts()
        ElseIf transactionView.Visible Then
            LoadProducts()
        End If
    End Sub

    Private Sub logoutstaff_button_Click(sender As Object, e As EventArgs) Handles logoutstaff_button.Click
        If Not LogoutConfirmationDialog.Confirm(Me) Then Return
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub retailModeButton_Click(sender As Object, e As EventArgs) Handles retailModeButton.Click
        SetTransactionMode(False)
    End Sub

    Private Sub wholesaleModeButton_Click(sender As Object, e As EventArgs) Handles wholesaleModeButton.Click
        SetTransactionMode(True)
    End Sub

    Private Sub ParentCategoryFilterButton_Click(sender As Object, e As EventArgs) _
        Handles allParentFilterButton.Click, medicineFilterButton.Click, supplementsFilterButton.Click, medicalSupplyFilterButton.Click
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return

        selectedParentCategory = Convert.ToString(button.Tag)
        UpdateParentCategoryFilterButtons()
        PopulateCategoryDropdown()
        If products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub UpdateParentCategoryFilterButtons()
        allParentFilterButton.BackColor = If(selectedParentCategory = "All", darkGreen, Color.White)
        allParentFilterButton.ForeColor = If(selectedParentCategory = "All", Color.White, darkGreen)
        medicineFilterButton.BackColor = If(selectedParentCategory = "Medicine", darkGreen, Color.White)
        medicineFilterButton.ForeColor = If(selectedParentCategory = "Medicine", Color.White, darkGreen)
        supplementsFilterButton.BackColor = If(selectedParentCategory = "Supplements", darkGreen, Color.White)
        supplementsFilterButton.ForeColor = If(selectedParentCategory = "Supplements", Color.White, darkGreen)
        medicalSupplyFilterButton.BackColor = If(selectedParentCategory = "Medical Supply", darkGreen, Color.White)
        medicalSupplyFilterButton.ForeColor = If(selectedParentCategory = "Medical Supply", Color.White, darkGreen)
    End Sub

    Private Sub ParentFilterPanel_Resize(sender As Object, e As EventArgs)
        Dim buttonTop = Math.Max(0, (parentFilterPanel.ClientSize.Height - allParentFilterButton.Height) \ 2)
        allParentFilterButton.Top = buttonTop
        medicineFilterButton.Top = buttonTop
        supplementsFilterButton.Top = buttonTop
        medicalSupplyFilterButton.Top = buttonTop
    End Sub

    Private Sub ModePanel_Resize(sender As Object, e As EventArgs)
        Dim buttonTop = Math.Max(0, (modePanel.ClientSize.Height - retailModeButton.Height) \ 2)
        retailModeButton.SetBounds(11, buttonTop, 137, retailModeButton.Height)
        wholesaleModeButton.SetBounds(retailModeButton.Right + 10, buttonTop, 137, wholesaleModeButton.Height)
    End Sub

    Private Sub SetTransactionMode(wholesale As Boolean)
        If isWholesale = wholesale AndAlso retailModeButton IsNot Nothing Then
            UpdateModeControls()
            Return
        End If

        If cart.Count > 0 Then
            Dim answer = MessageBox.Show(
                "Changing transaction mode clears the current cart because prices differ between Retail and Wholesale. Continue?",
                "Change transaction mode",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)
            If answer <> DialogResult.Yes Then Return
            cart.Clear()
        End If

        isWholesale = wholesale
        currentProduct = Nothing
        resultsGrid.DataSource = Nothing
        qtyTextBox.Text = "0"
        RenderCart()
        UpdateModeControls()
    End Sub

    Private Sub UpdateModeControls()
        retailModeButton.BackColor = If(isWholesale, Color.White, darkGreen)
        retailModeButton.ForeColor = If(isWholesale, darkGreen, Color.White)
        wholesaleModeButton.BackColor = If(isWholesale, darkGreen, Color.White)
        wholesaleModeButton.ForeColor = If(isWholesale, Color.White, darkGreen)
        prescriptionPanel.Visible = Not isWholesale
        wholesaleDetailsPanel.Visible = isWholesale
        UpdatePrescriptionEntryAvailability()
        addToCartButton.Text = "ADD TO CART"
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
        If products IsNot Nothing Then BindSearchResults(products)
        UpdateTransactionScrollLayout()
    End Sub

    Private Sub TransactionView_Resize(sender As Object, e As EventArgs)
        UpdateTransactionScrollLayout()
    End Sub

    Private Sub UpdateTransactionScrollLayout()
        If transactionView Is Nothing OrElse transactionLayout Is Nothing OrElse transactionLeftLayout Is Nothing OrElse transactionLeftPanel Is Nothing Then Return

        transactionLeftLayout.RowStyles(1).SizeType = SizeType.Absolute
        transactionLeftLayout.RowStyles(1).Height = 380.0F
        transactionLeftLayout.RowStyles(3).SizeType = SizeType.Absolute
        transactionLeftLayout.RowStyles(3).Height = If(isWholesale, 380.0F, 160.0F)

        Dim contentHeight = 174 + 380 + 76 + CInt(transactionLeftLayout.RowStyles(3).Height) + 32
        If transactionLeftPanel.ClientSize.Width > 0 Then transactionLeftLayout.Width = transactionLeftPanel.ClientSize.Width
        transactionLeftLayout.Height = contentHeight
        transactionLeftPanel.AutoScrollMinSize = New Size(0, contentHeight)
    End Sub

    Private Sub LoadProducts()
        Try
            Dim manager As New ProductManager()
            products = manager.GetProducts(includeInactive:=False)
            If products Is Nothing Then products = New DataTable()
            PopulateCategoryDropdown()
            BindSearchResults(products)
        Catch ex As Exception
            MessageBox.Show("Unable to load products: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeInventoryWarningUi()
        inventoryWarningsLabel = New Label With {
            .AutoEllipsis = True,
            .BackColor = Color.FromArgb(255, 248, 225),
            .Dock = DockStyle.Bottom,
            .Font = New Font("Segoe UI", 8.5F),
            .ForeColor = Color.FromArgb(120, 75, 0),
            .Height = 76,
            .Padding = New Padding(8, 5, 8, 5),
            .TextAlign = ContentAlignment.MiddleLeft,
            .Visible = False
        }
        resultsPanel.Controls.Add(inventoryWarningsLabel)
        inventoryWarningsLabel.BringToFront()
    End Sub

    Private Sub RefreshSelectedProductInventoryWarnings()
        If inventoryWarningsLabel Is Nothing Then Return
        inventoryWarningsLabel.Text = String.Empty
        inventoryWarningsLabel.Visible = False
        If currentProduct Is Nothing Then Return

        Try
            Dim productId = Convert.ToInt32(currentProduct("ID"))
            Dim summary = New ProductManager().GetStaffInventorySummary(productId, ExpiryMonitorSettings.LoadNearExpiryDays())
            If summary.Rows.Count = 0 Then Return

            Dim first = summary.Rows(0)
            Dim medicineName = Convert.ToString(first("Medicine Name"))
            Dim saleable = Convert.ToInt32(first("Saleable Quantity"))
            Dim reorderPoint = If(first.IsNull("Reorder Point"), 0, Convert.ToInt32(first("Reorder Point")))
            Dim placement = If(currentProduct.Table.Columns.Contains("Item Placement") AndAlso Not currentProduct.IsNull("Item Placement"), Convert.ToString(currentProduct("Item Placement")), "Unassigned")
            Dim messages As New List(Of String)()

            If saleable <= reorderPoint Then
                messages.Add("Low Stock Warning: " & medicineName & " (" & placement & ") has only " & saleable.ToString(CultureInfo.InvariantCulture) & " units available for sale.")
            End If

            For Each row As DataRow In summary.Rows
                If row.IsNull("Batch Number") OrElse row.IsNull("Expiry Date") Then Continue For
                Dim quantity = Convert.ToInt32(row("Near-Expiry Quantity"))
                Dim expiryDate = Convert.ToDateTime(row("Expiry Date"))
                Dim daysRemaining = Convert.ToInt32(row("Days Remaining"))
                messages.Add("Near-Expiry Warning: " & medicineName & " has " & quantity.ToString(CultureInfo.InvariantCulture) &
                             " units expiring on " & expiryDate.ToString("MMM dd, yyyy", CultureInfo.CurrentCulture) &
                             " (Batch " & Convert.ToString(row("Batch Number")) & ", " & daysRemaining.ToString(CultureInfo.InvariantCulture) & " days remaining).")
            Next

            Dim expiredQuantity = Convert.ToInt32(first("Expired Quantity"))
            If expiredQuantity > 0 Then
                messages.Add("Expired Stock Alert: " & medicineName & " has " & expiredQuantity.ToString(CultureInfo.InvariantCulture) &
                             " expired units. These units are excluded from saleable inventory.")
            End If

            Dim hasNearExpiryStock = summary.AsEnumerable().Any(Function(row) Not row.IsNull("Batch Number") AndAlso
                                                                  Not row.IsNull("Expiry Date") AndAlso
                                                                  Convert.ToInt32(row("Near-Expiry Quantity")) > 0)
            If messages.Count = 0 AndAlso saleable > 0 AndAlso Not hasNearExpiryStock AndAlso expiredQuantity = 0 Then
                messages.Add("In Stock: " & medicineName & " (" & placement & ") has " & saleable.ToString(CultureInfo.InvariantCulture) &
                             " eligible units. Stock is not expired or near expiry.")
                inventoryWarningsLabel.BackColor = Color.FromArgb(232, 245, 233)
                inventoryWarningsLabel.ForeColor = Color.FromArgb(27, 94, 32)
            Else
                inventoryWarningsLabel.BackColor = Color.FromArgb(255, 248, 225)
                inventoryWarningsLabel.ForeColor = Color.FromArgb(120, 75, 0)
            End If

            inventoryWarningsLabel.Text = String.Join(Environment.NewLine, messages)
            inventoryWarningsLabel.Visible = messages.Count > 0
        Catch ex As Exception
            inventoryWarningsLabel.Text = ex.Message
            inventoryWarningsLabel.Visible = True
        End Try
    End Sub

    Private Sub PopulateCategoryDropdown()
        If products Is Nothing Then Return

        updatingCategoryFilter = True
        searchFilterCombo.BeginUpdate()
        Try
            searchFilterCombo.Items.Clear()
            searchFilterCombo.Items.Add("All Categories")

            Dim categories = products.AsEnumerable()
            If selectedParentCategory <> "All" AndAlso products.Columns.Contains("Parent Category") Then
                categories = categories.Where(Function(row) String.Equals(
                    Convert.ToString(row("Parent Category")), selectedParentCategory, StringComparison.OrdinalIgnoreCase))
            End If

            Dim categoryNames = categories.Select(Function(row) Convert.ToString(row("Category"))).
                Where(Function(name) Not String.IsNullOrWhiteSpace(name)).
                Distinct(StringComparer.OrdinalIgnoreCase).
                OrderBy(Function(name) name)
            For Each category In categoryNames
                searchFilterCombo.Items.Add(category)
            Next

            searchFilterCombo.SelectedIndex = 0
        Finally
            searchFilterCombo.EndUpdate()
            updatingCategoryFilter = False
        End Try
    End Sub

    Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        If products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    ' Displays "Item Placement" instead of "Product ID" (Product ID stays hidden for selection/checkout)
    Private Sub BindSearchResults(source As DataTable)
        Dim result As New DataTable()
        result.Columns.Add("Product ID", GetType(Integer))
        result.Columns.Add("Item Placement", GetType(String))
        result.Columns.Add("Generic Name", GetType(String))
        result.Columns.Add("Brand Name", GetType(String))
        result.Columns.Add("Form", GetType(String))
        result.Columns.Add("Strength", GetType(String))
        result.Columns.Add("Type", GetType(String))
        result.Columns.Add("Requires Prescription", GetType(Boolean))
        result.Columns.Add("Price", GetType(Decimal))

        Dim keyword = searchTextBox.Text.Trim()
        Dim selectedCategory = If(searchFilterCombo.SelectedItem, "All Categories").ToString()

        For Each row As DataRow In source.Rows
            Dim category = Convert.ToString(row("Category"))
            If selectedParentCategory <> "All" AndAlso source.Columns.Contains("Parent Category") AndAlso
               Not String.Equals(Convert.ToString(row("Parent Category")), selectedParentCategory, StringComparison.OrdinalIgnoreCase) Then Continue For
            If Not String.Equals(selectedCategory, "All Categories", StringComparison.OrdinalIgnoreCase) AndAlso
               Not String.Equals(category, selectedCategory, StringComparison.OrdinalIgnoreCase) Then Continue For

            Dim retailPrice As Decimal
            Dim wholesalePrice As Decimal
            Decimal.TryParse(row("Retail Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, retailPrice)
            Decimal.TryParse(row("Wholesale Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, wholesalePrice)
            Dim price = If(isWholesale, wholesalePrice, retailPrice)
            Dim isRx = IsPrescriptionRequired(row)
            Dim type = If(isRx, "Rx", "OTC")
            Dim placement = If(source.Columns.Contains("Item Placement") AndAlso Not row.IsNull("Item Placement"), Convert.ToString(row("Item Placement")), "Unassigned")
            Dim searchable = String.Join(" ", {
                Convert.ToString(row("ID")),
                placement,
                Convert.ToString(row("Brand Name")),
                Convert.ToString(row("Generic Name"))
            })
            If keyword.Length > 0 AndAlso searchable.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0 Then Continue For

            result.Rows.Add(Convert.ToInt32(row("ID")), placement, row("Generic Name").ToString(), row("Brand Name").ToString(), row("Unit").ToString(), row("Dosage").ToString(), type, isRx, price)
        Next

        resultsGrid.DataSource = result
        If resultsGrid.Columns.Contains("Price") Then
            resultsGrid.Columns("Price").DefaultCellStyle.Format = "'P' #,##0.00"
        End If
        If resultsGrid.Columns.Contains("Requires Prescription") Then
            resultsGrid.Columns("Requires Prescription").Visible = False
        End If
        If resultsGrid.Columns.Contains("Product ID") Then
            resultsGrid.Columns("Product ID").Visible = False
        End If
        If resultsGrid.Columns.Contains("Item Placement") Then
            resultsGrid.Columns("Item Placement").FillWeight = 95
        End If
        If resultsGrid.Columns.Contains("Generic Name") Then
            resultsGrid.Columns("Generic Name").FillWeight = 130
        End If
        If resultsGrid.Columns.Contains("Brand Name") Then
            resultsGrid.Columns("Brand Name").FillWeight = 100
        End If

        resultsGrid.ClearSelection()
        resultsGrid.CurrentCell = Nothing
        currentProduct = Nothing
        UpdatePrescriptionEntryAvailability()
    End Sub

    Private Function IsPrescriptionRequired(product As DataRow) As Boolean
        Return product IsNot Nothing AndAlso
               product.Table.Columns.Contains("Prescription Required") AndAlso
               Not product.IsNull("Prescription Required") AndAlso
               Convert.ToBoolean(product("Prescription Required"))
    End Function

    Private Sub resultsGrid_SelectionChanged(sender As Object, e As EventArgs) Handles resultsGrid.SelectionChanged
        If resultsGrid.SelectedRows.Count = 0 OrElse resultsGrid.CurrentRow Is Nothing OrElse resultsGrid.CurrentRow.IsNewRow Then
            currentProduct = Nothing
            RefreshSelectedProductInventoryWarnings()
            UpdatePrescriptionEntryAvailability()
            Return
        End If

        Dim productId As Integer
        If Not Integer.TryParse(Convert.ToString(resultsGrid.CurrentRow.Cells("Product ID").Value), productId) OrElse products Is Nothing Then
            currentProduct = Nothing
            RefreshSelectedProductInventoryWarnings()
            UpdatePrescriptionEntryAvailability()
            Return
        End If

        currentProduct = products.AsEnumerable().FirstOrDefault(Function(row) Convert.ToInt32(row("ID")) = productId)
        RefreshSelectedProductInventoryWarnings()
        UpdatePrescriptionEntryAvailability()
    End Sub

    Private Sub resultsGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs)
        clearClickedSelection = e.RowIndex >= 0 AndAlso
                                resultsGrid.Rows(e.RowIndex).Selected AndAlso
                                resultsGrid.CurrentRow IsNot Nothing AndAlso
                                resultsGrid.CurrentRow.Index = e.RowIndex
    End Sub

    Private Sub resultsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If Not clearClickedSelection OrElse e.RowIndex < 0 Then Return
        clearClickedSelection = False

        BeginInvoke(New Action(Sub()
                                   If e.RowIndex < resultsGrid.Rows.Count Then
                                       resultsGrid.ClearSelection()
                                       resultsGrid.CurrentCell = Nothing
                                       currentProduct = Nothing
                                       RefreshSelectedProductInventoryWarnings()
                                       UpdatePrescriptionEntryAvailability()
                                   End If
                               End Sub))
    End Sub

    Private Sub searchFilterCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles searchFilterCombo.SelectedIndexChanged
        If Not updatingCategoryFilter AndAlso products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub UpdatePrescriptionEntryAvailability()
        Dim isRxSelected = False
        If Not isWholesale AndAlso resultsGrid IsNot Nothing AndAlso resultsGrid.SelectedRows.Count > 0 AndAlso resultsGrid.CurrentRow IsNot Nothing AndAlso
           resultsGrid.Columns.Contains("Requires Prescription") Then
            Dim value = resultsGrid.CurrentRow.Cells("Requires Prescription").Value
            isRxSelected = value IsNot Nothing AndAlso value IsNot DBNull.Value AndAlso Convert.ToBoolean(value)
        End If
        prescriptionPanel.Enabled = isRxSelected
        UpdatePrescriptionDateLimits()

        Dim prescriptionComplete = Not String.IsNullOrWhiteSpace(rxNoTextBox.Text) AndAlso
                                   Not String.IsNullOrWhiteSpace(rxPractitionerTextBox.Text) AndAlso
                                   Not String.IsNullOrWhiteSpace(rxLicenseTextBox.Text) AndAlso
                                   Not String.IsNullOrWhiteSpace(rxDateTextBox.Text)
        Dim quantity As Integer
        Dim hasPositiveQuantity = Integer.TryParse(qtyTextBox.Text, quantity) AndAlso quantity > 0
        Dim hasSelectedProduct = currentProduct IsNot Nothing AndAlso resultsGrid IsNot Nothing AndAlso resultsGrid.SelectedRows.Count > 0
        Dim canAddToCart = hasSelectedProduct AndAlso hasPositiveQuantity AndAlso
                           (isWholesale OrElse Not isRxSelected OrElse prescriptionComplete)
        addToCartButton.Enabled = canAddToCart
        addToCartButton.BackColor = If(canAddToCart, darkGreen, Color.LightGray)
        addToCartButton.ForeColor = If(canAddToCart, Color.White, Color.DimGray)
    End Sub

    Private Sub UpdatePrescriptionDateLimits()
        If rxDateTextBox Is Nothing Then Return

        Dim today = DateTime.Today
        Dim minDate = today.AddYears(-1)
        If currentProduct IsNot Nothing AndAlso IsPrescriptionRequired(currentProduct) Then
            Dim category = Convert.ToString(currentProduct("Category"))
            If category.IndexOf("antibiotic", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               category.IndexOf("anti-infective", StringComparison.OrdinalIgnoreCase) >= 0 Then
                minDate = today.AddDays(-7)
            End If
        End If

        rxDateTextBox.MaxDate = today
        rxDateTextBox.MinDate = minDate
        If rxDateTextBox.Value.Date < minDate Then rxDateTextBox.Value = minDate
        If rxDateTextBox.Value.Date > today Then rxDateTextBox.Value = today
    End Sub

    Private Function IsPrescriptionDateValid(product As DataRow, prescriptionDate As DateTime) As Boolean
        Dim earliestAllowed = DateTime.Today.AddYears(-1)
        If product IsNot Nothing Then
            Dim category = Convert.ToString(product("Category"))
            If category.IndexOf("antibiotic", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               category.IndexOf("anti-infective", StringComparison.OrdinalIgnoreCase) >= 0 Then
                earliestAllowed = DateTime.Today.AddDays(-7)
            End If
        End If

        Return prescriptionDate.Date >= earliestAllowed AndAlso prescriptionDate.Date <= DateTime.Today
    End Function

    Private Sub PrescriptionInputs_TextChanged(sender As Object, e As EventArgs)
        UpdatePrescriptionEntryAvailability()
    End Sub

    Private Sub QuantityInputs_TextChanged(sender As Object, e As EventArgs)
        UpdatePrescriptionEntryAvailability()
    End Sub

    Private Sub qtyMinusButton_Click(sender As Object, e As EventArgs) Handles qtyMinusButton.Click
        AdjustQuantity(-1)
    End Sub

    Private Sub qtyPlusButton_Click(sender As Object, e As EventArgs) Handles qtyPlusButton.Click
        AdjustQuantity(1)
    End Sub

    Private Sub AdjustQuantity(delta As Integer)
        Dim quantity As Integer
        If Not Integer.TryParse(qtyTextBox.Text, quantity) Then quantity = 0
        quantity = Math.Max(0, quantity + delta)
        qtyTextBox.Text = quantity.ToString(CultureInfo.InvariantCulture)
    End Sub

    Private Sub addToCartButton_Click(sender As Object, e As EventArgs) Handles addToCartButton.Click
        If currentProduct Is Nothing OrElse resultsGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a product from the results table first.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(qtyTextBox.Text, quantity) OrElse quantity < 1 Then
            MessageBox.Show("Enter a quantity of at least 1.", "Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            qtyTextBox.Focus()
            Return
        End If

        Dim isRx = IsPrescriptionRequired(currentProduct)
        If Not isWholesale AndAlso isRx AndAlso
           (String.IsNullOrWhiteSpace(rxNoTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxPractitionerTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxLicenseTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxDateTextBox.Text)) Then
            MessageBox.Show("Complete the prescription information before adding an Rx item.", "Prescription Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not isWholesale AndAlso isRx AndAlso Not IsPrescriptionDateValid(currentProduct, rxDateTextBox.Value) Then
            MessageBox.Show("The prescription date is outside the allowed validity period for this medicine.", "Invalid Prescription Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            rxDateTextBox.Focus()
            Return
        End If

        Try
            Dim physicalQuantity = quantity
            If isWholesale Then
                Dim quantityPerBox As Integer
                If Not Integer.TryParse(Convert.ToString(currentProduct("Qty/Box")), quantityPerBox) OrElse quantityPerBox < 1 Then
                    MessageBox.Show("The medicine has an invalid quantity per box and cannot be added.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
                physicalQuantity *= quantityPerBox
            End If

            Dim inventory = New ProductManager().GetStaffInventorySummary(Convert.ToInt32(currentProduct("ID")), ExpiryMonitorSettings.LoadNearExpiryDays())
            If inventory.Rows.Count = 0 Then Throw New InvalidOperationException("The selected medicine is no longer available.")
            Dim saleableQuantity = Convert.ToInt32(inventory.Rows(0)("Saleable Quantity"))
            Dim existingCartItem = cart.FirstOrDefault(Function(item) item.ProductID = Convert.ToInt32(currentProduct("ID")))
            If existingCartItem IsNot Nothing Then
                Dim existingPhysicalQuantity = existingCartItem.Quantity
                If isWholesale Then existingPhysicalQuantity *= Convert.ToInt32(currentProduct("Qty/Box"))
                physicalQuantity += existingPhysicalQuantity
            End If
            If physicalQuantity > saleableQuantity Then
                MessageBox.Show("Insufficient Saleable Stock: Only " & saleableQuantity.ToString(CultureInfo.InvariantCulture) &
                                " eligible units of " & Convert.ToString(inventory.Rows(0)("Medicine Name")) & " are available.",
                                "Insufficient Saleable Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                RefreshSelectedProductInventoryWarnings()
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Inventory Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try

        Dim price As Decimal
        Dim priceColumn = If(isWholesale, "Wholesale Price", "Retail Price")
        Decimal.TryParse(currentProduct(priceColumn).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, price)
        Dim id = Convert.ToInt32(currentProduct("ID"))
        Dim cartItemToUpdate = cart.FirstOrDefault(Function(item) item.ProductID = id)
        If cartItemToUpdate Is Nothing Then
            cart.Add(New CartItem With {
                .ProductID = id,
                .BrandName = currentProduct("Brand Name").ToString(),
                .GenericName = currentProduct("Generic Name").ToString(),
                .Dosage = currentProduct("Dosage").ToString(),
                .Unit = currentProduct("Unit").ToString(),
                .Price = price,
                .Quantity = quantity,
                .IsRx = isRx
            })
        Else
            cartItemToUpdate.Quantity += quantity
        End If

        RenderCart()
        currentProduct = Nothing
        resultsGrid.CurrentCell = Nothing
        resultsGrid.ClearSelection()
        qtyTextBox.Text = "0"
        UpdatePrescriptionEntryAvailability()
    End Sub

    Private Sub RenderCart()
        cartItemsPanel.SuspendLayout()
        cartItemsPanel.Controls.Clear()

        Dim y As Integer = 4
        y = RenderCartSection("OTC ITEMS", cart.Where(Function(item) Not item.IsRx).ToList(), y)
        y = RenderCartSection("Rx ITEMS", cart.Where(Function(item) item.IsRx).ToList(), y)
        If cart.Count = 0 Then
            Dim emptyLabel = New Label With {.Text = "Cart is empty.", .AutoSize = True, .ForeColor = Color.Gray, .Margin = New Padding(4, 8, 4, 12)}
            cartItemsPanel.Controls.Add(emptyLabel)
        End If

        cartSubtotalValue.Text = "P " & cart.Sum(Function(item) item.LineTotal).ToString("N2")
        cartItemsPanel.ResumeLayout()
    End Sub

    Private Function RenderCartSection(sectionTitle As String, items As List(Of CartItem), startY As Integer) As Integer
        If items.Count = 0 Then Return startY
        Dim header = New Label With {
            .Text = sectionTitle,
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .ForeColor = darkGreen,
            .Margin = New Padding(4, 8, 4, 4)
        }
        cartItemsPanel.Controls.Add(header)

        For Each item In items
            Dim rowWidth = Math.Max(250, cartItemsPanel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 8)
            Dim removeLeft = rowWidth - 28
            Dim totalLeft = removeLeft - 82
            Dim quantityLeft = totalLeft - 98
            Dim rowPanel = New Panel With {.Width = rowWidth, .Height = 58, .Margin = New Padding(2, 2, 2, 4)}
            Dim productLabel = New Label With {
                .Text = item.BrandName & "  " & item.Dosage & " " & item.Unit & Environment.NewLine & item.GenericName,
                .Location = New Point(2, 3),
                .Size = New Size(Math.Max(80, quantityLeft - 6), 48),
                .Font = New Font("Segoe UI", 8.5F)
            }
            Dim quantityLabel = New Label With {
                .Text = "P " & item.Price.ToString("N2") & " x " & item.Quantity.ToString(),
                .Location = New Point(quantityLeft, 15),
                .Size = New Size(95, 24),
                .Font = New Font("Segoe UI", 8.5F)
            }
            Dim totalLabel = New Label With {
                .Text = "P " & item.LineTotal.ToString("N2"),
                .Location = New Point(totalLeft, 15),
                .Size = New Size(78, 24),
                .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)
            }
            Dim removeButton = New Button With {
                .Text = "X",
                .Tag = item.ProductID,
                .Location = New Point(removeLeft, 11),
                .Size = New Size(25, 25),
                .ForeColor = Color.FromArgb(200, 30, 30),
                .FlatStyle = FlatStyle.Flat,
                .BackColor = Color.White
            }
            removeButton.FlatAppearance.BorderColor = Color.FromArgb(200, 30, 30)
            AddHandler removeButton.Click, AddressOf RemoveCartItem_Click
            rowPanel.Controls.Add(productLabel)
            rowPanel.Controls.Add(quantityLabel)
            rowPanel.Controls.Add(totalLabel)
            rowPanel.Controls.Add(removeButton)
            cartItemsPanel.Controls.Add(rowPanel)
        Next

        Return startY + (items.Count * 64) + 24
    End Function

    Private Sub RemoveCartItem_Click(sender As Object, e As EventArgs)
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return
        Dim productId = Convert.ToInt32(button.Tag)
        Dim item = cart.FirstOrDefault(Function(entry) entry.ProductID = productId)
        If item IsNot Nothing Then cart.Remove(item)
        RenderCart()
    End Sub

    Private Sub InitializeWholesaleCustomerUi()
        wholesaleDetailsPanel.SuspendLayout()
        wholesaleDetailsPanel.Controls.Clear()
        wholesaleDetailsPanel.Padding = New Padding(8)

        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 1,
            .RowCount = 2,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 42))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim toolbar As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 4,
            .RowCount = 1,
            .BackColor = Color.White,
            .Margin = New Padding(0)
        }
        toolbar.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 170))
        toolbar.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        toolbar.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 88))
        toolbar.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 88))
        toolbar.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        Dim heading As New Label With {
            .Text = "Wholesale Clients",
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold),
            .ForeColor = darkGreen,
            .Margin = New Padding(4, 0, 4, 0)
        }
        wholesaleCustomerSearchBox = New TextBox With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.0F),
            .PlaceholderText = "Search wholesale clients...",
            .Margin = New Padding(4, 7, 8, 5)
        }
        Dim addButton As New Button With {
            .Text = "Add Client",
            .Dock = DockStyle.Fill,
            .BackColor = darkGreen,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .Margin = New Padding(4, 4, 4, 4),
            .Cursor = Cursors.Hand,
            .UseVisualStyleBackColor = False
        }
        addButton.FlatAppearance.BorderSize = 0
        Dim deleteButton As New Button With {
            .Text = "Delete Client",
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(170, 45, 45),
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
            .Margin = New Padding(4, 4, 4, 4),
            .Cursor = Cursors.Hand,
            .Enabled = False
        }
        deleteButton.FlatAppearance.BorderColor = Color.FromArgb(170, 45, 45)
        wholesaleCustomerDeleteButton = deleteButton
        toolbar.Controls.Add(heading, 0, 0)
        toolbar.Controls.Add(wholesaleCustomerSearchBox, 1, 0)
        toolbar.Controls.Add(addButton, 2, 0)
        toolbar.Controls.Add(deleteButton, 3, 0)

        wholesaleCustomerGrid = New DataGridView With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AllowUserToResizeColumns = False,
            .AllowUserToResizeRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .RowHeadersVisible = False,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .GridColor = Color.FromArgb(225, 232, 227),
            .EnableHeadersVisualStyles = False,
            .ColumnHeadersHeight = 30,
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            .RowTemplate = New DataGridViewRow With {.Height = 30, .MinimumHeight = 30},
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .Font = New Font("Segoe UI", 8.0F),
                .SelectionBackColor = Color.FromArgb(218, 240, 229),
                .SelectionForeColor = Color.Black
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.FromArgb(240, 245, 241),
                .ForeColor = Color.FromArgb(35, 55, 45),
                .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
                .SelectionBackColor = Color.FromArgb(240, 245, 241),
                .SelectionForeColor = Color.FromArgb(35, 55, 45)
            },
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {.BackColor = Color.FromArgb(248, 250, 248)}
        }
        AddHandler wholesaleCustomerSearchBox.TextChanged, AddressOf WholesaleCustomerSearchBox_TextChanged
        AddHandler wholesaleCustomerGrid.SelectionChanged, AddressOf WholesaleCustomerGrid_SelectionChanged
        AddHandler wholesaleCustomerGrid.CellClick, AddressOf WholesaleCustomerGrid_CellClick
        AddHandler wholesaleCustomerGrid.CellDoubleClick, AddressOf WholesaleCustomerGrid_CellDoubleClick
        AddHandler wholesaleCustomerGrid.CellFormatting, AddressOf WholesaleCustomerGrid_CellFormatting
        AddHandler wholesaleCustomerGrid.CellMouseEnter, AddressOf WholesaleCustomerGrid_CellMouseEnter
        AddHandler addButton.Click, AddressOf AddWholesaleCustomer_Click
        AddHandler deleteButton.Click, AddressOf DeleteWholesaleCustomer_Click
        layout.Controls.Add(toolbar, 0, 0)
        layout.Controls.Add(wholesaleCustomerGrid, 0, 1)
        wholesaleDetailsPanel.Controls.Add(layout)
        wholesaleDetailsPanel.ResumeLayout(True)
        LoadWholesaleCustomers()
    End Sub

    Private Sub LoadWholesaleCustomers()
        If wholesaleCustomerGrid Is Nothing Then Return
        Try
            Const query As String = "SELECT client_id AS 'Client ID', company_name AS 'Company', fda_lto_no AS 'FDA LTO No.', CONCAT(first_name, ' ', last_name) AS 'Contact Person', contact_no_1 AS 'Contact No.', CONCAT_WS(', ', NULLIF(shipping_address_line, ''), NULLIF(barangay, ''), NULLIF(city_municipality, ''), NULLIF(province, ''), NULLIF(postal_code, '')) AS 'Shipping Address', CASE WHEN preferred_payment_method = 'E-payment' THEN CONCAT('E-payment: ', preferred_payment_provider) ELSE preferred_payment_method END AS 'Preferred Payment', is_active AS 'Active' " &
                                    "FROM wholesale_clients " &
                                    "WHERE @search = '' OR company_name LIKE @pattern OR fda_lto_no LIKE @pattern OR first_name LIKE @pattern OR last_name LIKE @pattern OR contact_no_1 LIKE @pattern OR email LIKE @pattern OR city_municipality LIKE @pattern OR province LIKE @pattern " &
                                    "ORDER BY company_name, last_name, first_name"
            Dim results As New DataTable()
            Using connection = GetConnection()
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    Dim search = If(wholesaleCustomerSearchBox Is Nothing, String.Empty, wholesaleCustomerSearchBox.Text.Trim())
                    command.Parameters.AddWithValue("@search", search)
                    command.Parameters.AddWithValue("@pattern", "%" & search & "%")
                    Using adapter As New MySqlDataAdapter(command)
                        adapter.Fill(results)
                    End Using
                End Using
            End Using

            wholesaleCustomerGrid.DataSource = results
            If wholesaleCustomerGrid.Columns.Contains("Client ID") Then wholesaleCustomerGrid.Columns("Client ID").Visible = False
            If wholesaleCustomerGrid.Columns.Contains("Active") Then wholesaleCustomerGrid.Columns("Active").Visible = False
            For Each column As DataGridViewColumn In wholesaleCustomerGrid.Columns
                column.Resizable = DataGridViewTriState.False
                column.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            If wholesaleCustomerGrid.Columns.Contains("Company") Then wholesaleCustomerGrid.Columns("Company").FillWeight = 125
            If wholesaleCustomerGrid.Columns.Contains("FDA LTO No.") Then wholesaleCustomerGrid.Columns("FDA LTO No.").FillWeight = 90
            If wholesaleCustomerGrid.Columns.Contains("Contact Person") Then wholesaleCustomerGrid.Columns("Contact Person").FillWeight = 110
            If wholesaleCustomerGrid.Columns.Contains("Contact No.") Then wholesaleCustomerGrid.Columns("Contact No.").FillWeight = 95
            If wholesaleCustomerGrid.Columns.Contains("Shipping Address") Then wholesaleCustomerGrid.Columns("Shipping Address").FillWeight = 160
            If wholesaleCustomerGrid.Columns.Contains("Preferred Payment") Then wholesaleCustomerGrid.Columns("Preferred Payment").FillWeight = 90
            wholesaleCustomerGrid.ClearSelection()
            wholesaleCustomerGrid.CurrentCell = Nothing
        Catch ex As Exception
            MessageBox.Show("Could not load wholesale clients. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub WholesaleCustomerSearchBox_TextChanged(sender As Object, e As EventArgs)
        LoadWholesaleCustomers()
    End Sub

    Private Sub WholesaleCustomerGrid_SelectionChanged(sender As Object, e As EventArgs)
        If wholesaleCustomerGrid Is Nothing OrElse wholesaleCustomerGrid.SelectedRows.Count = 0 Then
            If wholesaleCustomerDeleteButton IsNot Nothing Then wholesaleCustomerDeleteButton.Enabled = False
            Return
        End If
        If wholesaleCustomerDeleteButton IsNot Nothing Then wholesaleCustomerDeleteButton.Enabled = True
    End Sub

    Private Sub WholesaleCustomerGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse wholesaleCustomerGrid Is Nothing Then Return
        Dim row = wholesaleCustomerGrid.Rows(e.RowIndex)
        If Not Convert.ToBoolean(row.Cells("Active").Value) Then
            selectedWholesaleCustomer = Nothing
            customerLicenseTextBox.Clear()
            contactPersonTextBox.Clear()
            contactNoTextBox.Clear()
            addressTextBox.Clear()
            ShowWholesaleCustomerDetails(Convert.ToInt32(row.Cells("Client ID").Value))
            Return
        End If
        ApplyWholesaleCustomerToOrder(row)
    End Sub

    Private Sub WholesaleCustomerGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse wholesaleCustomerGrid Is Nothing Then Return
        Dim row = wholesaleCustomerGrid.Rows(e.RowIndex)
        If Not Convert.ToBoolean(row.Cells("Active").Value) Then Return
        Dim clientId = Convert.ToInt32(row.Cells("Client ID").Value)
        ShowWholesaleCustomerDetails(clientId)
    End Sub

    Private Sub WholesaleCustomerGrid_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse wholesaleCustomerGrid Is Nothing OrElse Not wholesaleCustomerGrid.Columns.Contains("Active") Then Return
        Dim row = wholesaleCustomerGrid.Rows(e.RowIndex)
        If row.IsNewRow OrElse Convert.ToBoolean(row.Cells("Active").Value) Then Return
        e.CellStyle.BackColor = Color.FromArgb(255, 239, 239)
        e.CellStyle.ForeColor = Color.FromArgb(110, 65, 65)
        e.CellStyle.SelectionBackColor = Color.FromArgb(249, 215, 215)
        e.CellStyle.SelectionForeColor = Color.FromArgb(90, 45, 45)
    End Sub

    Private Sub WholesaleCustomerGrid_CellMouseEnter(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse wholesaleCustomerGrid Is Nothing OrElse Not wholesaleCustomerGrid.Columns.Contains("Active") Then Return
        Dim row = wholesaleCustomerGrid.Rows(e.RowIndex)
        If row.IsNewRow Then Return
        row.Cells(e.ColumnIndex).ToolTipText = If(Convert.ToBoolean(row.Cells("Active").Value), String.Empty, "Inactive client — click to view details and reactivate.")
    End Sub

    Private Function GetWholesaleCustomer(clientId As Integer) As WholesaleCustomerRecord
        Const query As String = "SELECT client_id, company_name, fda_lto_no, license_expiry_date, first_name, last_name, contact_no_1, email, shipping_address_line, barangay, city_municipality, province, postal_code, preferred_payment_method, preferred_payment_provider, payment_account_number, payment_account_name, is_active FROM wholesale_clients WHERE client_id = @id"
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@id", clientId)
                Using reader = command.ExecuteReader()
                    If Not reader.Read() Then Return Nothing
                    Return New WholesaleCustomerRecord With {
                        .CustomerId = reader.GetInt32("client_id"),
                        .CompanyName = Convert.ToString(reader("company_name")),
                        .LicenseNumber = Convert.ToString(reader("fda_lto_no")),
                        .LicenseExpiryDate = If(reader.IsDBNull(reader.GetOrdinal("license_expiry_date")), CType(Nothing, Date?), reader.GetDateTime("license_expiry_date")),
                        .FirstName = Convert.ToString(reader("first_name")),
                        .LastName = Convert.ToString(reader("last_name")),
                        .ContactNumber = Convert.ToString(reader("contact_no_1")),
                        .Email = If(reader.IsDBNull(reader.GetOrdinal("email")), String.Empty, Convert.ToString(reader("email"))),
                        .ShippingAddressLine = Convert.ToString(reader("shipping_address_line")),
                        .Barangay = If(reader.IsDBNull(reader.GetOrdinal("barangay")), String.Empty, Convert.ToString(reader("barangay"))),
                        .CityMunicipality = If(reader.IsDBNull(reader.GetOrdinal("city_municipality")), String.Empty, Convert.ToString(reader("city_municipality"))),
                        .Province = If(reader.IsDBNull(reader.GetOrdinal("province")), String.Empty, Convert.ToString(reader("province"))),
                        .PostalCode = If(reader.IsDBNull(reader.GetOrdinal("postal_code")), String.Empty, Convert.ToString(reader("postal_code"))),
                        .PreferredPaymentMethod = If(reader.IsDBNull(reader.GetOrdinal("preferred_payment_method")), String.Empty, Convert.ToString(reader("preferred_payment_method"))),
                        .PreferredPaymentProvider = If(reader.IsDBNull(reader.GetOrdinal("preferred_payment_provider")), String.Empty, Convert.ToString(reader("preferred_payment_provider"))),
                        .PaymentAccountNumber = If(reader.IsDBNull(reader.GetOrdinal("payment_account_number")), String.Empty, Convert.ToString(reader("payment_account_number"))),
                        .PaymentAccountName = If(reader.IsDBNull(reader.GetOrdinal("payment_account_name")), String.Empty, Convert.ToString(reader("payment_account_name"))),
                        .IsActive = Convert.ToBoolean(reader("is_active"))
                    }
                End Using
            End Using
        End Using
    End Function

    Private Sub ShowWholesaleCustomerDetails(clientId As Integer)
        Dim customer As WholesaleCustomerRecord
        Try
            customer = GetWholesaleCustomer(clientId)
        Catch ex As Exception
            MessageBox.Show("Could not load client details. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        If customer Is Nothing Then
            MessageBox.Show("This client could not be found. Refresh the client list and try again.", "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadWholesaleCustomers()
            Return
        End If

        Using dialog As New Form With {
            .Text = "Wholesale Client Details",
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.None,
            .ClientSize = New Size(660, 700),
            .MinimizeBox = False,
            .MaximizeBox = False,
            .ShowInTaskbar = False,
            .BackColor = Color.White,
            .Font = New Font("Segoe UI", 9.0F)
        }
            Dim rootLayout As New TableLayoutPanel With {.Dock = DockStyle.Fill, .Padding = New Padding(18, 12, 18, 10), .ColumnCount = 1, .RowCount = 3, .BackColor = Color.White}
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 62))
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))

            Dim header As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .RowCount = 2, .BackColor = Color.White, .Margin = New Padding(0)}
            header.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
            header.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 78))
            header.RowStyles.Add(New RowStyle(SizeType.Absolute, 32))
            header.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            Dim heading As New Label With {.Text = customer.CompanyName, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = New Font("Segoe UI", 13.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 55, 45), .AutoEllipsis = True}
            Dim clientIdLabel As New Label With {.Text = "Client ID: " & customer.CustomerId.ToString("000") & " · Wholesale Client", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray}
            Dim statusBadge As New Label With {.Text = If(customer.IsActive, "Active", "Inactive"), .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleCenter, .BackColor = If(customer.IsActive, Color.FromArgb(218, 240, 229), Color.FromArgb(245, 230, 230)), .ForeColor = If(customer.IsActive, darkGreen, Color.FromArgb(160, 50, 50)), .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold), .Margin = New Padding(8, 6, 0, 6)}
            header.Controls.Add(heading, 0, 0)
            header.Controls.Add(clientIdLabel, 0, 1)
            header.Controls.Add(statusBadge, 1, 0)
            header.SetRowSpan(statusBadge, 2)

            Dim detailsScroll As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = Color.White, .Margin = New Padding(0), .Padding = New Padding(0, 0, 18, 0)}
            Dim content As New TableLayoutPanel With {.Dock = DockStyle.Top, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink, .ColumnCount = 1, .RowCount = 4, .BackColor = Color.White, .Margin = New Padding(0)}
            content.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

            Dim editors As New Dictionary(Of String, Control)(StringComparer.OrdinalIgnoreCase)
            Dim companyBox = CreateWholesaleDetailEditor(customer.CompanyName)
            Dim licenseBox = CreateWholesaleDetailEditor(customer.LicenseNumber)
            Dim expiryBox = CreateWholesaleDetailEditor(If(customer.LicenseExpiryDate.HasValue, customer.LicenseExpiryDate.Value.ToString("MMMM d, yyyy"), ""))
            Dim firstNameBox = CreateWholesaleDetailEditor(customer.FirstName)
            Dim lastNameBox = CreateWholesaleDetailEditor(customer.LastName)
            Dim phoneBox = CreateWholesaleDetailEditor(customer.ContactNumber)
            Dim emailBox = CreateWholesaleDetailEditor(customer.Email)
            Dim streetBox = CreateWholesaleDetailEditor(customer.ShippingAddressLine)
            Dim barangayBox = CreateWholesaleDetailEditor(customer.Barangay)
            Dim postalBox = CreateWholesaleDetailEditor(customer.PostalCode)
            Dim cityBox = CreateWholesaleDetailEditor(customer.CityMunicipality)
            Dim provinceBox = CreateWholesaleDetailEditor(customer.Province)
            Dim paymentBox As New ComboBox With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Enabled = False, .BackColor = Color.FromArgb(245, 245, 245)}
            paymentBox.Items.AddRange(New Object() {"Cash", "E-payment"})
            If paymentBox.Items.Contains(customer.PreferredPaymentMethod) Then paymentBox.SelectedItem = customer.PreferredPaymentMethod
            Dim providerBox As New ComboBox With {.Dock = DockStyle.Fill, .DropDownStyle = ComboBoxStyle.DropDownList, .Enabled = False, .BackColor = Color.FromArgb(245, 245, 245)}
            providerBox.Items.AddRange(New Object() {"GCash", "Maya", "Bank Transfer"})
            If providerBox.Items.Contains(customer.PreferredPaymentProvider) Then providerBox.SelectedItem = customer.PreferredPaymentProvider
            Dim accountNameBox = CreateWholesaleDetailEditor(customer.PaymentAccountName)
            Dim accountNumberBox = CreateWholesaleDetailEditor(If(String.IsNullOrWhiteSpace(customer.PaymentAccountNumber), "", MaskWholesaleAccountNumber(customer.PaymentAccountNumber)))

            For Each entry In New Dictionary(Of String, Control) From {
                {"Company Name", companyBox}, {"FDA LTO No.", licenseBox}, {"License Expiry", expiryBox},
                {"First Name", firstNameBox}, {"Last Name", lastNameBox}, {"Contact Number", phoneBox}, {"Email Address", emailBox},
                {"Street Address", streetBox}, {"Barangay", barangayBox}, {"Postal Code", postalBox},
                {"City / Municipality", cityBox}, {"Province", provinceBox}, {"Payment Mode", paymentBox},
                {"E-payment Provider", providerBox}, {"Account Name", accountNameBox}, {"Account Number", accountNumberBox}
            }
                editors.Add(entry.Key, entry.Value)
            Next
            For Each editor In editors.Values
                Dim textEditor = TryCast(editor, TextBox)
                If textEditor IsNot Nothing Then
                    textEditor.ReadOnly = True
                    textEditor.TabStop = False
                    textEditor.Enabled = False
                End If
            Next
            paymentBox.Enabled = False
            providerBox.Enabled = False
            paymentBox.TabStop = False
            providerBox.TabStop = False

            Dim businessSection = CreateWholesaleDetailSection("Business & Licensing", 2)
            AddWholesaleDetailField(businessSection, "Company Name", companyBox, 1, 0)
            AddWholesaleDetailField(businessSection, "FDA LTO No.", licenseBox, 1, 1)
            AddWholesaleDetailField(businessSection, "License Expiry", expiryBox, 2, 0, 2)

            Dim contactSection = CreateWholesaleDetailSection("Contact Person", 2)
            AddWholesaleDetailField(contactSection, "First Name", firstNameBox, 1, 0)
            AddWholesaleDetailField(contactSection, "Last Name", lastNameBox, 1, 1)
            AddWholesaleDetailField(contactSection, "Contact Number", phoneBox, 2, 0)
            AddWholesaleDetailField(contactSection, "Email Address", emailBox, 2, 1)

            Dim addressSection = CreateWholesaleDetailSection("Business Address", 3)
            AddWholesaleDetailField(addressSection, "Street Address", streetBox, 1, 0, 2)
            AddWholesaleDetailField(addressSection, "Barangay", barangayBox, 2, 0)
            AddWholesaleDetailField(addressSection, "Postal Code", postalBox, 2, 1)
            AddWholesaleDetailField(addressSection, "City / Municipality", cityBox, 3, 0)
            AddWholesaleDetailField(addressSection, "Province", provinceBox, 3, 1)

            Dim paymentSection = CreateWholesaleDetailSection("Payment Information", 2)
            AddWholesaleDetailField(paymentSection, "Payment Mode", paymentBox, 1, 0)
            Dim providerCard = AddWholesaleDetailField(paymentSection, "E-payment Provider", providerBox, 1, 1)
            Dim accountNameCard = AddWholesaleDetailField(paymentSection, "Account Name", accountNameBox, 2, 0)
            Dim accountNumberCard = AddWholesaleDetailField(paymentSection, "Account Number", accountNumberBox, 2, 1)
            content.Controls.Add(businessSection, 0, 0)
            content.Controls.Add(contactSection, 0, 1)
            content.Controls.Add(addressSection, 0, 2)
            content.Controls.Add(paymentSection, 0, 3)
            For row = 0 To 3
                content.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            Next
            detailsScroll.Controls.Add(content)
            AddHandler detailsScroll.Resize,
                Sub(sender, e)
                    content.Width = Math.Max(200, detailsScroll.ClientSize.Width - 40)
                End Sub

            Dim actions As New FlowLayoutPanel With {.Dock = DockStyle.Fill, .FlowDirection = FlowDirection.RightToLeft, .WrapContents = False, .Padding = New Padding(0, 7, 0, 0), .Margin = New Padding(0)}
            Dim editButton As New Button With {.Text = "Edit Details", .Size = New Size(108, 34), .BackColor = darkGreen, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .UseVisualStyleBackColor = False, .Enabled = True, .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold)}
            editButton.FlatAppearance.BorderSize = 0
            Dim statusActionText = If(customer.IsActive, "Deactivate", "Reactivate")
            Dim statusActionColor = If(customer.IsActive, Color.FromArgb(190, 45, 45), darkGreen)
            Dim deactivateButton As New Button With {.Text = statusActionText, .Size = New Size(90, 34), .BackColor = Color.White, .ForeColor = statusActionColor, .FlatStyle = FlatStyle.Flat, .Enabled = True}
            deactivateButton.FlatAppearance.BorderColor = statusActionColor
            Dim closeButton As New Button With {.Text = "Close", .Size = New Size(62, 34), .BackColor = Color.White, .ForeColor = Color.FromArgb(55, 65, 60), .FlatStyle = FlatStyle.Flat, .DialogResult = DialogResult.Cancel}
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(190, 195, 192)
            actions.Controls.Add(editButton)
            actions.Controls.Add(deactivateButton)
            actions.Controls.Add(closeButton)
            rootLayout.Controls.Add(header, 0, 0)
            rootLayout.Controls.Add(detailsScroll, 0, 1)
            rootLayout.Controls.Add(actions, 0, 2)
            dialog.Controls.Add(rootLayout)
            dialog.CancelButton = closeButton

            Dim isEditing As Boolean = False
            Dim updatePaymentVisibility As Action =
                Sub()
                    Dim ePayment = String.Equals(Convert.ToString(paymentBox.SelectedItem), "E-payment", StringComparison.OrdinalIgnoreCase)
                    providerCard.Visible = ePayment
                    accountNameCard.Visible = ePayment
                    accountNumberCard.Visible = ePayment
                    paymentSection.RowStyles(2).Height = If(ePayment, 54.0F, 0.0F)
                End Sub
            AddHandler paymentBox.SelectedIndexChanged, Sub(sender, e) updatePaymentVisibility()
            updatePaymentVisibility()
            AddHandler editButton.Click,
                Sub(sender, e)
                    If Not isEditing Then
                        isEditing = True
                        For Each editor In editors.Values
                            Dim textEditor = TryCast(editor, TextBox)
                            If textEditor IsNot Nothing Then
                                textEditor.Enabled = True
                                textEditor.ReadOnly = False
                                textEditor.BackColor = Color.White
                                textEditor.TabStop = True
                            End If
                        Next
                        paymentBox.Enabled = True
                        providerBox.Enabled = True
                        paymentBox.TabStop = True
                        providerBox.TabStop = True
                        accountNumberBox.Text = If(IsMaskedWholesaleAccountNumber(customer.PaymentAccountNumber), String.Empty, customer.PaymentAccountNumber)
                        closeButton.Text = "Cancel"
                        closeButton.DialogResult = DialogResult.Cancel
                        deactivateButton.Visible = False
                        editButton.Text = "Save Changes"
                        editButton.Enabled = True
                        editButton.BackColor = darkGreen
                        editButton.ForeColor = Color.White
                        Return
                    End If

                    Dim usesEpayment = String.Equals(Convert.ToString(paymentBox.SelectedItem), "E-payment", StringComparison.OrdinalIgnoreCase)
                    If String.IsNullOrWhiteSpace(companyBox.Text) OrElse String.IsNullOrWhiteSpace(licenseBox.Text) OrElse String.IsNullOrWhiteSpace(firstNameBox.Text) OrElse String.IsNullOrWhiteSpace(lastNameBox.Text) OrElse String.IsNullOrWhiteSpace(phoneBox.Text) OrElse String.IsNullOrWhiteSpace(streetBox.Text) OrElse String.IsNullOrWhiteSpace(cityBox.Text) OrElse String.IsNullOrWhiteSpace(provinceBox.Text) OrElse paymentBox.SelectedIndex < 0 Then
                        MessageBox.Show("Complete all required client details before saving.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    If usesEpayment AndAlso (providerBox.SelectedIndex < 0 OrElse String.IsNullOrWhiteSpace(accountNumberBox.Text) OrElse String.IsNullOrWhiteSpace(accountNameBox.Text)) Then
                        MessageBox.Show("Choose an e-payment provider and enter the account name and number.", "E-payment Details Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If

                    Dim expiry As Date? = Nothing
                    If Not String.IsNullOrWhiteSpace(expiryBox.Text) Then
                        Dim parsedExpiry As Date
                        If Not Date.TryParse(expiryBox.Text, parsedExpiry) Then
                            MessageBox.Show("Enter a valid license expiry date.", "Invalid License Expiry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                        expiry = parsedExpiry.Date
                    End If
                    Dim updatedCustomer As New WholesaleCustomerRecord With {
                        .CustomerId = customer.CustomerId,
                        .CompanyName = companyBox.Text.Trim(),
                        .LicenseNumber = licenseBox.Text.Trim(),
                        .LicenseExpiryDate = expiry,
                        .FirstName = firstNameBox.Text.Trim(),
                        .LastName = lastNameBox.Text.Trim(),
                        .ContactNumber = phoneBox.Text.Trim(),
                        .Email = emailBox.Text.Trim(),
                        .ShippingAddressLine = streetBox.Text.Trim(),
                        .Barangay = barangayBox.Text.Trim(),
                        .CityMunicipality = cityBox.Text.Trim(),
                        .Province = provinceBox.Text.Trim(),
                        .PostalCode = postalBox.Text.Trim(),
                        .PreferredPaymentMethod = Convert.ToString(paymentBox.SelectedItem),
                        .PreferredPaymentProvider = If(usesEpayment, Convert.ToString(providerBox.SelectedItem), String.Empty),
                        .PaymentAccountNumber = If(usesEpayment, accountNumberBox.Text.Trim(), String.Empty),
                        .PaymentAccountName = If(usesEpayment, accountNameBox.Text.Trim(), String.Empty),
                        .IsActive = customer.IsActive
                    }
                    If Not WholesaleClientDetailsChanged(customer, updatedCustomer) Then
                        MessageBox.Show("No changes were made to this client.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    If MessageBox.Show("Save the changes made to " & customer.CompanyName & "?", "Confirm Client Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
                    Try
                        UpdateWholesaleCustomer(updatedCustomer)
                        LoadWholesaleCustomers()
                        dialog.DialogResult = DialogResult.OK
                        dialog.Close()
                    Catch ex As MySqlException When ex.Number = 1062
                        MessageBox.Show("That FDA LTO number is already assigned to another client.", "Duplicate FDA LTO No.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show("Could not update the wholesale client. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Sub
            AddHandler deactivateButton.Click,
                Sub(sender, e)
                    Dim makeActive = Not customer.IsActive
                    Dim actionText = If(makeActive, "Reactivate", "Deactivate")
                    Dim promptText = If(makeActive,
                                        "Reactivate " & customer.CompanyName & "? This client will be available for new wholesale transactions.",
                                        "Deactivate " & customer.CompanyName & "? This client will remain in the system but cannot be selected for new wholesale transactions.")
                    If MessageBox.Show(promptText, actionText & " Wholesale Client", MessageBoxButtons.YesNo, If(makeActive, MessageBoxIcon.Question, MessageBoxIcon.Warning), MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
                    Dim enteredPin = PromptForStaffPin(dialog)
                    If enteredPin Is Nothing Then Return
                    Try
                        If CurrentUserID <= 0 OrElse Not New ProductManager().VerifyUserPin(CurrentUserID, enteredPin) Then
                            MessageBox.Show("The staff PIN is incorrect or no PIN is set for the signed-in account.", "PIN Verification", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                        Using connection = GetConnection()
                            connection.Open()
                            Using command As New MySqlCommand("UPDATE wholesale_clients SET is_active = @active WHERE client_id = @id", connection)
                                command.Parameters.AddWithValue("@active", If(makeActive, 1, 0))
                                command.Parameters.AddWithValue("@id", customer.CustomerId)
                                command.ExecuteNonQuery()
                            End Using
                        End Using
                        If Not makeActive AndAlso String.Equals(customerLicenseTextBox.Text, customer.LicenseNumber, StringComparison.OrdinalIgnoreCase) Then
                            selectedWholesaleCustomer = Nothing
                            customerLicenseTextBox.Clear()
                            contactPersonTextBox.Clear()
                            contactNoTextBox.Clear()
                            addressTextBox.Clear()
                        End If
                        LoadWholesaleCustomers()
                        dialog.Close()
                    Catch ex As Exception
                        MessageBox.Show("Could not " & actionText.ToLowerInvariant() & " the wholesale client. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End Sub

            dialog.ShowDialog(Me)
        End Using
    End Sub

    Private Function PromptForStaffPin(owner As IWin32Window) As String
        Using prompt As New Form With {
            .Text = "Verify Staff PIN",
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .ClientSize = New Size(340, 140),
            .MinimizeBox = False,
            .MaximizeBox = False,
            .ShowInTaskbar = False,
            .BackColor = Color.White
        }
            prompt.Controls.Add(New Label With {.Text = "Enter your staff PIN to confirm:", .AutoSize = True, .Location = New Point(18, 18)})
            Dim pinBox As New TextBox With {.Location = New Point(18, 48), .Width = 300, .UseSystemPasswordChar = True, .MaxLength = 12}
            prompt.Controls.Add(pinBox)
            Dim confirmButton As New Button With {.Text = "Confirm", .DialogResult = DialogResult.OK, .Location = New Point(148, 90), .Size = New Size(80, 30)}
            Dim cancelButton As New Button With {.Text = "Cancel", .DialogResult = DialogResult.Cancel, .Location = New Point(238, 90), .Size = New Size(80, 30)}
            prompt.Controls.Add(confirmButton)
            prompt.Controls.Add(cancelButton)
            prompt.AcceptButton = confirmButton
            prompt.CancelButton = cancelButton
            If prompt.ShowDialog(owner) = DialogResult.OK Then Return pinBox.Text
        End Using
        Return Nothing
    End Function

    Private Function WholesaleClientDetailsChanged(original As WholesaleCustomerRecord, updated As WholesaleCustomerRecord) As Boolean
        Return Not String.Equals(original.CompanyName, updated.CompanyName, StringComparison.Ordinal) OrElse
               Not String.Equals(original.LicenseNumber, updated.LicenseNumber, StringComparison.Ordinal) OrElse
               Not Nullable.Equals(original.LicenseExpiryDate, updated.LicenseExpiryDate) OrElse
               Not String.Equals(original.FirstName, updated.FirstName, StringComparison.Ordinal) OrElse
               Not String.Equals(original.LastName, updated.LastName, StringComparison.Ordinal) OrElse
               Not String.Equals(original.ContactNumber, updated.ContactNumber, StringComparison.Ordinal) OrElse
               Not String.Equals(original.Email, updated.Email, StringComparison.Ordinal) OrElse
               Not String.Equals(original.ShippingAddressLine, updated.ShippingAddressLine, StringComparison.Ordinal) OrElse
               Not String.Equals(original.Barangay, updated.Barangay, StringComparison.Ordinal) OrElse
               Not String.Equals(original.CityMunicipality, updated.CityMunicipality, StringComparison.Ordinal) OrElse
               Not String.Equals(original.Province, updated.Province, StringComparison.Ordinal) OrElse
               Not String.Equals(original.PostalCode, updated.PostalCode, StringComparison.Ordinal) OrElse
               Not String.Equals(original.PreferredPaymentMethod, updated.PreferredPaymentMethod, StringComparison.Ordinal) OrElse
               Not String.Equals(original.PreferredPaymentProvider, updated.PreferredPaymentProvider, StringComparison.Ordinal) OrElse
               Not String.Equals(original.PaymentAccountNumber, updated.PaymentAccountNumber, StringComparison.Ordinal) OrElse
               Not String.Equals(original.PaymentAccountName, updated.PaymentAccountName, StringComparison.Ordinal)
    End Function

    Private Function CreateWholesaleDetailEditor(value As String) As TextBox
        Return New TextBox With {
            .Text = If(value, String.Empty),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.FromArgb(243, 246, 244),
            .ForeColor = Color.FromArgb(35, 45, 40),
            .Font = New Font("Segoe UI", 9.0F),
            .ReadOnly = True,
            .Height = 26
        }
    End Function

    Private Function CreateWholesaleDetailSection(title As String, fieldRowCount As Integer) As TableLayoutPanel
        Dim section As New TableLayoutPanel With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .ColumnCount = 2,
            .RowCount = fieldRowCount + 1,
            .BackColor = Color.White,
            .Margin = New Padding(0, 0, 0, 8),
            .Padding = New Padding(0, 2, 0, 10)
        }
        section.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        section.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        section.RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
        For row = 0 To fieldRowCount - 1
            section.RowStyles.Add(New RowStyle(SizeType.Absolute, 54))
        Next
        Dim sectionHeading As New Label With {
            .Text = "▣  " & title,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .ForeColor = Color.FromArgb(25, 105, 68),
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold),
            .Margin = New Padding(0)
        }
        section.Controls.Add(sectionHeading, 0, 0)
        section.SetColumnSpan(sectionHeading, 2)
        Return section
    End Function

    Private Function AddWholesaleDetailField(section As TableLayoutPanel,
                                             caption As String,
                                             editor As Control,
                                             row As Integer,
                                             column As Integer,
                                             Optional columnSpan As Integer = 1) As Panel
        Dim fieldCard As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Margin = New Padding(0, 2, 12, 3)
        }
        Dim fieldLabel As New Label With {
            .Text = caption,
            .Location = New Point(0, 0),
            .Size = New Size(240, 17),
            .ForeColor = Color.FromArgb(115, 125, 120),
            .Font = New Font("Segoe UI", 7.5F)
        }
        editor.Dock = DockStyle.None
        editor.Location = New Point(0, 18)
        editor.Height = 26
        editor.Width = 225
        editor.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        fieldCard.Controls.Add(fieldLabel)
        fieldCard.Controls.Add(editor)
        section.Controls.Add(fieldCard, column, row)
        If columnSpan > 1 Then section.SetColumnSpan(fieldCard, columnSpan)
        Return fieldCard
    End Function

    Private Function MaskWholesaleAccountNumber(accountNumber As String) As String
        If String.IsNullOrWhiteSpace(accountNumber) Then Return String.Empty
        Return New String("•"c, 6) & accountNumber.Substring(Math.Max(0, accountNumber.Length - 4))
    End Function

    Private Function IsMaskedWholesaleAccountNumber(accountNumber As String) As Boolean
        Return Not String.IsNullOrEmpty(accountNumber) AndAlso
               (accountNumber.Contains("•") OrElse accountNumber.Contains("*") OrElse accountNumber.StartsWith("••", StringComparison.Ordinal))
    End Function

    Private Sub UpdateWholesaleCustomer(customer As WholesaleCustomerRecord)
        Const query As String = "UPDATE wholesale_clients SET company_name = @company, fda_lto_no = @license, license_expiry_date = @expiry, first_name = @first, last_name = @last, contact_no_1 = @phone, email = @email, shipping_address_line = @addressLine, barangay = @barangay, city_municipality = @city, province = @province, postal_code = @postal, preferred_payment_method = @payment, preferred_payment_provider = @provider, payment_account_number = @accountNumber, payment_account_name = @accountName WHERE client_id = @id"
        Using connection = GetConnection()
            connection.Open()
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@company", customer.CompanyName)
                command.Parameters.AddWithValue("@license", customer.LicenseNumber)
                command.Parameters.AddWithValue("@expiry", If(customer.LicenseExpiryDate.HasValue, CType(customer.LicenseExpiryDate.Value.Date, Object), DBNull.Value))
                command.Parameters.AddWithValue("@first", customer.FirstName)
                command.Parameters.AddWithValue("@last", customer.LastName)
                command.Parameters.AddWithValue("@phone", customer.ContactNumber)
                command.Parameters.AddWithValue("@email", If(String.IsNullOrWhiteSpace(customer.Email), CType(DBNull.Value, Object), customer.Email))
                command.Parameters.AddWithValue("@addressLine", customer.ShippingAddressLine)
                command.Parameters.AddWithValue("@barangay", If(String.IsNullOrWhiteSpace(customer.Barangay), CType(DBNull.Value, Object), customer.Barangay))
                command.Parameters.AddWithValue("@city", customer.CityMunicipality)
                command.Parameters.AddWithValue("@province", customer.Province)
                command.Parameters.AddWithValue("@postal", If(String.IsNullOrWhiteSpace(customer.PostalCode), CType(DBNull.Value, Object), customer.PostalCode))
                command.Parameters.AddWithValue("@payment", If(String.IsNullOrWhiteSpace(customer.PreferredPaymentMethod), CType(DBNull.Value, Object), customer.PreferredPaymentMethod))
                command.Parameters.AddWithValue("@provider", If(String.IsNullOrWhiteSpace(customer.PreferredPaymentProvider), CType(DBNull.Value, Object), customer.PreferredPaymentProvider))
                command.Parameters.AddWithValue("@accountNumber", If(String.IsNullOrWhiteSpace(customer.PaymentAccountNumber), CType(DBNull.Value, Object), customer.PaymentAccountNumber))
                command.Parameters.AddWithValue("@accountName", If(String.IsNullOrWhiteSpace(customer.PaymentAccountName), CType(DBNull.Value, Object), customer.PaymentAccountName))
                command.Parameters.AddWithValue("@id", customer.CustomerId)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub ApplyWholesaleCustomerToOrder(row As DataGridViewRow)
        If row Is Nothing OrElse row.DataBoundItem Is Nothing Then Return
        Dim customer = TryCast(row.DataBoundItem, DataRowView)
        If customer Is Nothing Then Return
        If Not Convert.ToBoolean(customer("Active")) Then
            selectedWholesaleCustomer = Nothing
            customerLicenseTextBox.Clear()
            contactPersonTextBox.Clear()
            contactNoTextBox.Clear()
            addressTextBox.Clear()
            MessageBox.Show("This client is deactivated and cannot be selected for a new wholesale transaction.", "Client Deactivated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Try
            selectedWholesaleCustomer = GetWholesaleCustomer(Convert.ToInt32(customer("Client ID")))
            If selectedWholesaleCustomer Is Nothing Then Return
            PopulateWholesaleOrderClientFields(selectedWholesaleCustomer)
        Catch ex As Exception
            MessageBox.Show("Could not load the selected client's saved details. " & ex.Message, "Wholesale Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PopulateWholesaleOrderClientFields(customer As WholesaleCustomerRecord)
        If customer Is Nothing Then Return
        customerLicenseTextBox.Text = customer.LicenseNumber
        contactPersonTextBox.Text = (customer.FirstName & " " & customer.LastName).Trim()
        contactNoTextBox.Text = customer.ContactNumber
        addressTextBox.Text = String.Join(", ", {customer.ShippingAddressLine, customer.Barangay, customer.CityMunicipality, customer.Province, customer.PostalCode}.Where(Function(value) Not String.IsNullOrWhiteSpace(value)))
    End Sub

    Private Sub AddWholesaleCustomer_Click(sender As Object, e As EventArgs)
        Dim customer = ShowWholesaleCustomerDialog()
        If customer Is Nothing Then Return
        Try
            Const query As String = "INSERT INTO wholesale_clients (company_name, fda_lto_no, license_expiry_date, first_name, last_name, contact_no_1, email, shipping_address_line, barangay, city_municipality, province, postal_code, preferred_payment_method, preferred_payment_provider, payment_account_number, payment_account_name, is_active) " &
                                    "VALUES (@company, @license, @expiry, @first, @last, @phone, @email, @addressLine, @barangay, @city, @province, @postal, @payment, @provider, @accountNumber, @accountName, @active)"
            Using connection = GetConnection()
                connection.Open()
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@company", customer.CompanyName)
                    command.Parameters.AddWithValue("@license", customer.LicenseNumber)
                    If customer.LicenseExpiryDate.HasValue Then
                        command.Parameters.AddWithValue("@expiry", customer.LicenseExpiryDate.Value.Date)
                    Else
                        command.Parameters.AddWithValue("@expiry", DBNull.Value)
                    End If
                    command.Parameters.AddWithValue("@first", customer.FirstName)
                    command.Parameters.AddWithValue("@last", customer.LastName)
                    command.Parameters.AddWithValue("@phone", customer.ContactNumber)
                    command.Parameters.AddWithValue("@email", If(String.IsNullOrWhiteSpace(customer.Email), CType(DBNull.Value, Object), customer.Email))
                    command.Parameters.AddWithValue("@addressLine", customer.ShippingAddressLine)
                    command.Parameters.AddWithValue("@barangay", If(String.IsNullOrWhiteSpace(customer.Barangay), CType(DBNull.Value, Object), customer.Barangay))
                    command.Parameters.AddWithValue("@city", customer.CityMunicipality)
                    command.Parameters.AddWithValue("@province", customer.Province)
                    command.Parameters.AddWithValue("@postal", If(String.IsNullOrWhiteSpace(customer.PostalCode), CType(DBNull.Value, Object), customer.PostalCode))
                    command.Parameters.AddWithValue("@payment", If(String.IsNullOrWhiteSpace(customer.PreferredPaymentMethod), CType(DBNull.Value, Object), customer.PreferredPaymentMethod))
                    command.Parameters.AddWithValue("@provider", If(String.IsNullOrWhiteSpace(customer.PreferredPaymentProvider), CType(DBNull.Value, Object), customer.PreferredPaymentProvider))
                    command.Parameters.AddWithValue("@accountNumber", If(String.IsNullOrWhiteSpace(customer.PaymentAccountNumber), CType(DBNull.Value, Object), customer.PaymentAccountNumber))
                    command.Parameters.AddWithValue("@accountName", If(String.IsNullOrWhiteSpace(customer.PaymentAccountName), CType(DBNull.Value, Object), customer.PaymentAccountName))
                    command.Parameters.AddWithValue("@active", customer.IsActive)
                    command.ExecuteNonQuery()
                    customer.CustomerId = CInt(command.LastInsertedId)
                End Using
            End Using
            selectedWholesaleCustomer = customer
            PopulateWholesaleOrderClientFields(customer)
            LoadWholesaleCustomers()
        Catch ex As MySqlException When ex.Number = 1062
            MessageBox.Show("That license number is already saved. Search for the existing client instead.", "Duplicate Client", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Could not save the wholesale client. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DeleteWholesaleCustomer_Click(sender As Object, e As EventArgs)
        If wholesaleCustomerGrid Is Nothing OrElse wholesaleCustomerGrid.SelectedRows.Count = 0 Then
            MessageBox.Show("Select a wholesale client to delete.", "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim selectedRow = wholesaleCustomerGrid.SelectedRows(0)
        Dim customerId = Convert.ToInt32(selectedRow.Cells("Client ID").Value)
        Dim customerName = Convert.ToString(selectedRow.Cells("Company").Value)
        If MessageBox.Show("Are you sure you want to delete " & customerName & "?", "Delete Wholesale Client", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return

        Try
            Using connection = GetConnection()
                connection.Open()
                Using command As New MySqlCommand("DELETE FROM wholesale_clients WHERE client_id = @id", connection)
                    command.Parameters.AddWithValue("@id", customerId)
                    command.ExecuteNonQuery()
                End Using
            End Using
            If String.Equals(customerLicenseTextBox.Text, Convert.ToString(selectedRow.Cells("FDA LTO No.").Value), StringComparison.OrdinalIgnoreCase) Then
                customerLicenseTextBox.Clear()
                contactPersonTextBox.Clear()
                contactNoTextBox.Clear()
                addressTextBox.Clear()
                poNoTextBox.Clear()
            End If
            LoadWholesaleCustomers()
        Catch ex As Exception
            MessageBox.Show("Could not delete the wholesale client. " & ex.Message, "Wholesale Clients", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ShowWholesaleCustomerDialog(Optional existingCustomer As WholesaleCustomerRecord = Nothing) As WholesaleCustomerRecord
        Using dialog As New Form With {
            .Text = If(existingCustomer Is Nothing, "Add Wholesale Client", "Edit Wholesale Client"),
            .StartPosition = FormStartPosition.CenterParent,
            .FormBorderStyle = FormBorderStyle.None,
            .ClientSize = New Size(660, 700),
            .MinimizeBox = False,
            .MaximizeBox = False,
            .ShowInTaskbar = False,
            .BackColor = Color.White,
            .Font = New Font("Segoe UI", 9.0F)
        }
            Dim rootLayout As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .Padding = New Padding(18, 12, 18, 10),
                .ColumnCount = 1,
                .RowCount = 3,
                .BackColor = Color.White
            }
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 54))
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            rootLayout.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))

            Dim header As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 1, .RowCount = 2, .BackColor = Color.White, .Margin = New Padding(0)}
            header.RowStyles.Add(New RowStyle(SizeType.Absolute, 30))
            header.RowStyles.Add(New RowStyle(SizeType.Absolute, 20))
            header.Controls.Add(New Label With {.Text = If(existingCustomer Is Nothing, "Add Wholesale Client", "Edit Wholesale Client"), .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = New Font("Segoe UI", 13.0F, FontStyle.Bold), .ForeColor = Color.FromArgb(35, 55, 45)}, 0, 0)
            header.Controls.Add(New Label With {.Text = "Business, contact, shipping, and payment information", .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft, .Font = New Font("Segoe UI", 8.0F), .ForeColor = Color.Gray}, 0, 1)

            Dim companyBox = CreateWholesaleDetailEditor("")
            Dim licenseBox = CreateWholesaleDetailEditor("")
            Dim expiryPicker As New DateTimePicker With {.Format = DateTimePickerFormat.Long, .ShowCheckBox = True, .Checked = False, .Value = Date.Today, .Font = New Font("Segoe UI", 9.0F), .CalendarMonthBackground = Color.White}
            Dim firstNameBox = CreateWholesaleDetailEditor("")
            Dim lastNameBox = CreateWholesaleDetailEditor("")
            Dim phoneBox = CreateWholesaleDetailEditor("")
            Dim emailBox = CreateWholesaleDetailEditor("")
            Dim addressBox = CreateWholesaleDetailEditor("")
            Dim barangayBox = CreateWholesaleDetailEditor("")
            Dim cityBox = CreateWholesaleDetailEditor("")
            Dim provinceBox = CreateWholesaleDetailEditor("")
            Dim postalBox = CreateWholesaleDetailEditor("")
            Dim paymentBox As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .BackColor = Color.FromArgb(243, 246, 244), .ForeColor = Color.FromArgb(35, 45, 40), .Font = New Font("Segoe UI", 9.0F)}
            paymentBox.Items.AddRange(New Object() {"Cash", "E-payment"})
            paymentBox.SelectedIndex = -1
            Dim providerBox As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .BackColor = Color.FromArgb(243, 246, 244), .ForeColor = Color.FromArgb(35, 45, 40), .Font = New Font("Segoe UI", 9.0F)}
            providerBox.Items.AddRange(New Object() {"GCash", "Maya", "Bank Transfer"})
            Dim accountNumberBox = CreateWholesaleDetailEditor("")
            Dim accountNameBox = CreateWholesaleDetailEditor("")
            For Each editor In New TextBox() {companyBox, licenseBox, firstNameBox, lastNameBox, phoneBox, emailBox, addressBox, barangayBox, cityBox, provinceBox, postalBox, accountNumberBox, accountNameBox}
                editor.ReadOnly = False
                editor.TabStop = True
                editor.Enabled = True
                editor.BackColor = Color.FromArgb(243, 246, 244)
            Next

            Dim businessSection = CreateWholesaleDetailSection("Business & Licensing", 2)
            AddWholesaleDetailField(businessSection, "Company Name", companyBox, 1, 0)
            AddWholesaleDetailField(businessSection, "FDA LTO No.", licenseBox, 1, 1)
            AddWholesaleDetailField(businessSection, "License Expiry", expiryPicker, 2, 0, 2)
            Dim contactSection = CreateWholesaleDetailSection("Contact Person", 2)
            AddWholesaleDetailField(contactSection, "First Name", firstNameBox, 1, 0)
            AddWholesaleDetailField(contactSection, "Last Name", lastNameBox, 1, 1)
            AddWholesaleDetailField(contactSection, "Contact Number", phoneBox, 2, 0)
            AddWholesaleDetailField(contactSection, "Email Address", emailBox, 2, 1)
            Dim addressSection = CreateWholesaleDetailSection("Business Address", 3)
            AddWholesaleDetailField(addressSection, "Street Address", addressBox, 1, 0, 2)
            AddWholesaleDetailField(addressSection, "Barangay", barangayBox, 2, 0)
            AddWholesaleDetailField(addressSection, "Postal Code", postalBox, 2, 1)
            AddWholesaleDetailField(addressSection, "City / Municipality", cityBox, 3, 0)
            AddWholesaleDetailField(addressSection, "Province", provinceBox, 3, 1)
            Dim paymentSection = CreateWholesaleDetailSection("Payment Information", 2)
            AddWholesaleDetailField(paymentSection, "Payment Mode", paymentBox, 1, 0)
            Dim providerCard = AddWholesaleDetailField(paymentSection, "E-payment Provider", providerBox, 1, 1)
            Dim accountNameCard = AddWholesaleDetailField(paymentSection, "Account Name", accountNameBox, 2, 0)
            Dim accountNumberCard = AddWholesaleDetailField(paymentSection, "Account Number", accountNumberBox, 2, 1)

            Dim content As New TableLayoutPanel With {.Dock = DockStyle.Top, .AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink, .ColumnCount = 1, .RowCount = 4, .BackColor = Color.White, .Margin = New Padding(0)}
            content.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
            content.Controls.Add(businessSection, 0, 0)
            content.Controls.Add(contactSection, 0, 1)
            content.Controls.Add(addressSection, 0, 2)
            content.Controls.Add(paymentSection, 0, 3)
            For row = 0 To 3
                content.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            Next
            Dim detailsScroll As New Panel With {.Dock = DockStyle.Fill, .AutoScroll = True, .BackColor = Color.White, .Margin = New Padding(0), .Padding = New Padding(0, 0, 18, 0)}
            content.Width = 560
            detailsScroll.Controls.Add(content)

            Dim paymentDependentCards As Control() = {providerCard, accountNameCard, accountNumberCard}

            If existingCustomer IsNot Nothing Then
                companyBox.Text = existingCustomer.CompanyName
                licenseBox.Text = existingCustomer.LicenseNumber
                expiryPicker.Checked = existingCustomer.LicenseExpiryDate.HasValue
                If existingCustomer.LicenseExpiryDate.HasValue Then expiryPicker.Value = existingCustomer.LicenseExpiryDate.Value
                firstNameBox.Text = existingCustomer.FirstName
                lastNameBox.Text = existingCustomer.LastName
                phoneBox.Text = existingCustomer.ContactNumber
                emailBox.Text = existingCustomer.Email
                addressBox.Text = existingCustomer.ShippingAddressLine
                barangayBox.Text = existingCustomer.Barangay
                cityBox.Text = existingCustomer.CityMunicipality
                provinceBox.Text = existingCustomer.Province
                postalBox.Text = existingCustomer.PostalCode
                If Not String.IsNullOrWhiteSpace(existingCustomer.PreferredPaymentMethod) AndAlso paymentBox.Items.Contains(existingCustomer.PreferredPaymentMethod) Then
                    paymentBox.SelectedItem = existingCustomer.PreferredPaymentMethod
                End If
                If Not String.IsNullOrWhiteSpace(existingCustomer.PreferredPaymentProvider) AndAlso providerBox.Items.Contains(existingCustomer.PreferredPaymentProvider) Then
                    providerBox.SelectedItem = existingCustomer.PreferredPaymentProvider
                End If
                accountNumberBox.Text = existingCustomer.PaymentAccountNumber
                accountNameBox.Text = existingCustomer.PaymentAccountName
            End If

            Dim updatePaymentFields As Action =
                Sub()
                    Dim isElectronicPayment = String.Equals(Convert.ToString(paymentBox.SelectedItem), "E-payment", StringComparison.OrdinalIgnoreCase)
                    For Each control In paymentDependentCards
                        control.Visible = isElectronicPayment
                    Next
                End Sub
            AddHandler paymentBox.SelectedIndexChanged, Sub(sender, e) updatePaymentFields()
            updatePaymentFields()

            Dim actions As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.RightToLeft,
                .WrapContents = False,
                .Padding = New Padding(0, 7, 0, 0),
                .Margin = New Padding(0)
            }
            Dim saveButton As New Button With {
                .Text = If(existingCustomer Is Nothing, "Add Customer", "Save Changes"),
                .Size = New Size(112, 34),
                .BackColor = darkGreen,
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .DialogResult = DialogResult.None
            }
            saveButton.FlatAppearance.BorderSize = 0
            Dim cancelButton As New Button With {.Text = "Cancel", .Size = New Size(84, 34), .DialogResult = DialogResult.Cancel, .BackColor = Color.White, .ForeColor = Color.FromArgb(35, 55, 45), .FlatStyle = FlatStyle.Flat}
            cancelButton.FlatAppearance.BorderColor = Color.FromArgb(205, 215, 210)
            actions.Controls.Add(saveButton)
            actions.Controls.Add(cancelButton)
            rootLayout.Controls.Add(header, 0, 0)
            rootLayout.Controls.Add(detailsScroll, 0, 1)
            rootLayout.Controls.Add(actions, 0, 2)
            dialog.Controls.Add(rootLayout)
            dialog.AcceptButton = saveButton
            dialog.CancelButton = cancelButton

            Dim result As WholesaleCustomerRecord = Nothing
            AddHandler saveButton.Click,
                Sub(sender, e)
                    If {companyBox.Text, licenseBox.Text, firstNameBox.Text, lastNameBox.Text, phoneBox.Text, addressBox.Text, cityBox.Text, provinceBox.Text}.Any(Function(value) String.IsNullOrWhiteSpace(value)) Then
                        MessageBox.Show("Complete the required client and shipping address fields before saving.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    If paymentBox.SelectedIndex < 0 Then
                        MessageBox.Show("Choose Cash or E-payment for the client's preferred payment mode.", "Payment Mode Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    Dim isElectronicPayment = String.Equals(Convert.ToString(paymentBox.SelectedItem), "E-payment", StringComparison.OrdinalIgnoreCase)
                    If isElectronicPayment AndAlso (providerBox.SelectedIndex < 0 OrElse String.IsNullOrWhiteSpace(accountNumberBox.Text) OrElse String.IsNullOrWhiteSpace(accountNameBox.Text)) Then
                        MessageBox.Show("Choose an e-payment type and enter its account number and account name.", "E-payment Details Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                    result = New WholesaleCustomerRecord With {
                        .CustomerId = If(existingCustomer Is Nothing, 0, existingCustomer.CustomerId),
                        .CompanyName = companyBox.Text.Trim(),
                        .LicenseNumber = licenseBox.Text.Trim(),
                        .LicenseExpiryDate = If(expiryPicker.Checked, CType(expiryPicker.Value.Date, Date?), Nothing),
                        .FirstName = firstNameBox.Text.Trim(),
                        .LastName = lastNameBox.Text.Trim(),
                        .ContactNumber = phoneBox.Text.Trim(),
                        .Email = emailBox.Text.Trim(),
                        .ShippingAddressLine = addressBox.Text.Trim(),
                        .Barangay = barangayBox.Text.Trim(),
                        .CityMunicipality = cityBox.Text.Trim(),
                        .Province = provinceBox.Text.Trim(),
                        .PostalCode = postalBox.Text.Trim(),
                        .PreferredPaymentMethod = Convert.ToString(paymentBox.SelectedItem),
                        .PreferredPaymentProvider = If(isElectronicPayment, Convert.ToString(providerBox.SelectedItem), String.Empty),
                        .PaymentAccountNumber = If(isElectronicPayment, accountNumberBox.Text.Trim(), String.Empty),
                        .PaymentAccountName = If(isElectronicPayment, accountNameBox.Text.Trim(), String.Empty),
                        .IsActive = If(existingCustomer Is Nothing, True, existingCustomer.IsActive)
                    }
                    dialog.DialogResult = DialogResult.OK
                    dialog.Close()
                End Sub

            If dialog.ShowDialog(Me) <> DialogResult.OK Then Return Nothing
            Return result
        End Using
    End Function

    Private Sub checkoutButton_Click(sender As Object, e As EventArgs) Handles checkoutButton.Click
        If cart.Count = 0 Then
            MessageBox.Show("Cart is empty. Add items before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If isWholesale AndAlso
            (String.IsNullOrWhiteSpace(customerLicenseTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(contactPersonTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(contactNoTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(addressTextBox.Text) OrElse selectedWholesaleCustomer Is Nothing) Then
            MessageBox.Show("Select a saved wholesale client before checkout.", "Wholesale Client Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        checkoutModeIsWholesale = isWholesale
        If Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex < 0 Then
            customerTypeCombo.SelectedIndex = 0
        End If
        transactionView.Visible = False
        restockView.Visible = False
        checkoutView.Visible = True
        titleLabel.Text = "Order Review"
        subtitleLabel.Text = If(isWholesale, "Wholesale transaction", "Retail transaction")
        CheckoutView_Resize(checkoutView, EventArgs.Empty)
        BuildCheckoutView()
    End Sub

    Private Sub BuildCheckoutView()
        wholesaleClientReviewPanel.Visible = checkoutModeIsWholesale
        wholesalePurchaseOrderPanel.Visible = checkoutModeIsWholesale
        ' Mode of Payment is visible for BOTH Retail (Regular, PWD, Senior Citizen) and Wholesale
        wholesalePaymentModeLabel.Visible = True
        wholesalePaymentModeCombo.Visible = True
        customerTypeLabel.Visible = Not checkoutModeIsWholesale
        customerTypeCombo.Visible = Not checkoutModeIsWholesale
        customerNameLabel.Visible = Not checkoutModeIsWholesale
        customerNameTextBox.Visible = Not checkoutModeIsWholesale
        idNumberLabel.Visible = Not checkoutModeIsWholesale
        idNumberTextBox.Visible = Not checkoutModeIsWholesale

        updatingWholesalePaymentUi = True
        If checkoutModeIsWholesale Then
            currentWholesalePurchaseOrderDate = Date.Today
            currentWholesalePurchaseOrderNumber = "PO-" & DateTime.Now.ToString("yyyyMMdd-HHmmssfff", CultureInfo.InvariantCulture)
            wholesaleReviewCompanyLabel.Text = selectedWholesaleCustomer.CompanyName
            wholesaleReviewLicenseLabel.Text = "FDA LTO No.: " & selectedWholesaleCustomer.LicenseNumber
            wholesaleReviewContactLabel.Text = "Contact Person: " & (selectedWholesaleCustomer.FirstName & " " & selectedWholesaleCustomer.LastName).Trim()
            wholesaleReviewPhoneLabel.Text = "Contact No.: " & selectedWholesaleCustomer.ContactNumber
            wholesaleReviewAddressLabel.Text = "Shipping Address: " & addressTextBox.Text
            wholesalePurchaseOrderNumberLabel.Text = currentWholesalePurchaseOrderNumber
            wholesalePurchaseOrderDateLabel.Text = currentWholesalePurchaseOrderDate.ToString("MMMM d, yyyy", CultureInfo.CurrentCulture)

            Dim preferredMode As String
            If String.Equals(selectedWholesaleCustomer.PreferredPaymentMethod, "Cash", StringComparison.OrdinalIgnoreCase) Then
                preferredMode = "Cash"
            ElseIf String.Equals(selectedWholesaleCustomer.PreferredPaymentMethod, "E-payment", StringComparison.OrdinalIgnoreCase) Then
                preferredMode = selectedWholesaleCustomer.PreferredPaymentProvider
            ElseIf wholesalePaymentModeCombo.Items.Contains(selectedWholesaleCustomer.PreferredPaymentMethod) Then
                preferredMode = selectedWholesaleCustomer.PreferredPaymentMethod
            Else
                preferredMode = "Cash"
            End If
            If Not wholesalePaymentModeCombo.Items.Contains(preferredMode) Then preferredMode = "Cash"
            wholesalePaymentModeCombo.SelectedItem = preferredMode
            wholesalePaymentAccountNameBox.Text = selectedWholesaleCustomer.PaymentAccountName
            wholesalePaymentReferenceBox.Clear()
        Else
            wholesalePaymentModeCombo.SelectedItem = "Cash"
            wholesalePaymentAccountNameBox.Clear()
            wholesalePaymentReferenceBox.Clear()
        End If
        updatingWholesalePaymentUi = False

        orderSummaryPanel.Controls.Clear()
        For Each item In cart
            Dim rowWidth = Math.Max(120, orderSummaryPanel.ClientSize.Width - 12)
            Dim line = New Panel With {
                .Width = rowWidth,
                .Height = 48,
                .Margin = New Padding(4, 4, 4, 4)
            }
            Dim productLabel = New Label With {
                .Text = item.BrandName & "  " & item.Dosage & " " & item.Unit & Environment.NewLine & item.GenericName,
                .Location = New Point(2, 2),
                .Size = New Size(CInt(rowWidth * 0.48), 44),
                .Font = New Font("Segoe UI", 9.0F)
            }
            Dim priceQuantityLabel = New Label With {
                .Text = "P " & item.Price.ToString("N2") & "  x  " & item.Quantity,
                .Location = New Point(CInt(rowWidth * 0.52), 13),
                .Size = New Size(CInt(rowWidth * 0.25), 24),
                .Font = New Font("Segoe UI", 9.0F)
            }
            Dim lineTotalLabel = New Label With {
                .Text = "P " & item.LineTotal.ToString("N2"),
                .Location = New Point(CInt(rowWidth * 0.78), 13),
                .Size = New Size(CInt(rowWidth * 0.2), 24),
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .TextAlign = ContentAlignment.MiddleRight
            }
            line.Controls.Add(productLabel)
            line.Controls.Add(priceQuantityLabel)
            line.Controls.Add(lineTotalLabel)
            orderSummaryPanel.Controls.Add(line)
        Next

        cashTextBox.Text = "0.00"
        cashValue.Text = "P"
        customerTypeCombo.Enabled = Not checkoutModeIsWholesale
        customerNameTextBox.Text = If(checkoutModeIsWholesale, contactPersonTextBox.Text, "")
        idNumberTextBox.Text = If(checkoutModeIsWholesale, customerLicenseTextBox.Text, "")
        UpdateCustomerCheckoutFields()
        UpdateWholesaleCheckoutPaymentUi()
        prescriptionLabel.Visible = Not checkoutModeIsWholesale AndAlso cart.Any(Function(item) item.IsRx)
        prescriptionValueLabel.Visible = prescriptionLabel.Visible
        rxRecordedLabel.Visible = prescriptionLabel.Visible
        prescriptionValueLabel.Text = If(prescriptionLabel.Visible, "RX- " & rxNoTextBox.Text, "")
        CheckoutView_Resize(checkoutView, EventArgs.Empty)
        UpdateCheckoutTotals()
    End Sub

    Private Sub customerTypeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles customerTypeCombo.SelectedIndexChanged
        If checkoutView.Visible AndAlso Not checkoutModeIsWholesale Then
            UpdateCustomerCheckoutFields()
            CheckoutView_Resize(checkoutView, EventArgs.Empty)
            UpdateCheckoutTotals()
        End If
    End Sub

    Private Sub cashTextBox_TextChanged(sender As Object, e As EventArgs) Handles cashTextBox.TextChanged
        UpdateCheckoutTotals()
    End Sub

    Private Sub UpdateCustomerCheckoutFields()
        Dim customerIdentityEnabled = Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex > 0
        customerNameLabel.Visible = Not checkoutModeIsWholesale
        customerNameTextBox.Visible = Not checkoutModeIsWholesale
        idNumberLabel.Visible = Not checkoutModeIsWholesale
        idNumberTextBox.Visible = Not checkoutModeIsWholesale
        customerNameTextBox.Enabled = customerIdentityEnabled
        idNumberTextBox.Enabled = customerIdentityEnabled
        If Not customerIdentityEnabled AndAlso Not checkoutModeIsWholesale Then
            customerNameTextBox.Clear()
            idNumberTextBox.Clear()
        End If
        UpdateConfirmButtonAvailability()
    End Sub

    Private Sub CustomerIdentity_TextChanged(sender As Object, e As EventArgs)
        UpdateConfirmButtonAvailability()
    End Sub

    Private Sub UpdateConfirmButtonAvailability()
        If confirmButton Is Nothing Then Return
        Dim usesElectronicPayment = IsElectronicPaymentSelected()
        Dim paymentDetailsValid = Not usesElectronicPayment OrElse Not String.IsNullOrWhiteSpace(wholesalePaymentReferenceBox.Text)
        Dim identityRequired = Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex > 0
        Dim identityComplete = Not String.IsNullOrWhiteSpace(customerNameTextBox.Text) AndAlso
                               Not String.IsNullOrWhiteSpace(idNumberTextBox.Text)
        Dim cash As Decimal
        Dim cashValid = Decimal.TryParse(cashTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, cash) AndAlso cash >= 0D
        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim totalDue = subtotal - Decimal.Round(subtotal * CheckoutDiscountRate(), 2)
        Dim canConfirm = (Not identityRequired OrElse identityComplete) AndAlso paymentDetailsValid AndAlso
                         (usesElectronicPayment OrElse (cashValid AndAlso cash >= totalDue))
        confirmButton.Enabled = canConfirm
        confirmButton.BackColor = If(canConfirm, darkGreen, Color.LightGray)
        confirmButton.ForeColor = If(canConfirm, Color.White, Color.DimGray)
    End Sub

    Private Function CheckoutDiscountRate() As Decimal
        Return If(Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex > 0, 0.2D, 0D)
    End Function

    Private Sub UpdateCheckoutTotals()
        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim discount = Decimal.Round(subtotal * CheckoutDiscountRate(), 2)
        Dim cash As Decimal
        Decimal.TryParse(cashTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, cash)
        Dim usesElectronicPayment = IsElectronicPaymentSelected()
        Dim receivedAmount = If(usesElectronicPayment, subtotal - discount, cash)
        subtotalValue.Text = "P " & subtotal.ToString("N2")
        discountValue.Text = "P " & discount.ToString("N2")
        totalDueValue.Text = "P " & (subtotal - discount).ToString("N2")
        cashValue.Text = If(usesElectronicPayment, "P " & receivedAmount.ToString("N2"), "P")
        If usesElectronicPayment Then
            cashValue.Left = paymentCard.ClientSize.Width - cashValue.Width - 20
        Else
            cashValue.Left = cashTextBox.Left - cashValue.Width - 8
        End If
        Dim change = receivedAmount - (subtotal - discount)
        changeValue.Text = "P " & change.ToString("N2")
        changeValue.ForeColor = If(change < 0, Color.FromArgb(200, 30, 30), Color.Black)
        UpdateConfirmButtonAvailability()
    End Sub

    Private Sub cashTextBox_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then Return

        Dim decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        If e.KeyChar.ToString() = decimalSeparator AndAlso Not cashTextBox.Text.Contains(decimalSeparator) Then Return

        e.Handled = True
    End Sub

    Private Sub cashTextBox_Enter(sender As Object, e As EventArgs) Handles cashTextBox.Enter
        cashTextBox.SelectAll()
    End Sub

    Private Sub backToPosButton_Click(sender As Object, e As EventArgs) Handles backToPosButton.Click
        checkoutView.Visible = False
        transactionView.Visible = True
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        Dim answer = MessageBox.Show("Are you sure? The current transaction will be canceled.",
                                     "Cancel Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If answer = DialogResult.Yes Then CancelCurrentTransaction()
    End Sub

    Private Sub CancelCurrentTransaction()
        cart.Clear()
        RenderCart()
        customerTypeCombo.SelectedIndex = 0
        customerNameTextBox.Clear()
        idNumberTextBox.Clear()
        rxNoTextBox.Clear()
        rxPractitionerTextBox.Clear()
        rxLicenseTextBox.Clear()
        rxDateTextBox.Value = DateTime.Today
        customerLicenseTextBox.Clear()
        contactPersonTextBox.Clear()
        contactNoTextBox.Clear()
        addressTextBox.Clear()
        poNoTextBox.Clear()
        selectedWholesaleCustomer = Nothing
        If wholesalePaymentModeCombo IsNot Nothing Then wholesalePaymentModeCombo.SelectedItem = "Cash"
        If wholesalePaymentAccountNameBox IsNot Nothing Then wholesalePaymentAccountNameBox.Clear()
        If wholesalePaymentReferenceBox IsNot Nothing Then wholesalePaymentReferenceBox.Clear()
        cashTextBox.Text = "0.00"
        checkoutView.Visible = False
        transactionView.Visible = True
        restockView.Visible = False
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
        SetStaffActiveNavigation("Transaction")
    End Sub

    Private Sub confirmButton_Click(sender As Object, e As EventArgs) Handles confirmButton.Click
        If CurrentUserID <= 0 Then
            MessageBox.Show("No signed-in staff account is associated with this transaction. Sign in again before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim usesElectronicPayment = IsElectronicPaymentSelected()
        If usesElectronicPayment AndAlso String.IsNullOrWhiteSpace(wholesalePaymentReferenceBox.Text) Then
            MessageBox.Show("Enter the e-payment reference number before confirming.", "Payment Reference Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            wholesalePaymentReferenceBox.Focus()
            Return
        End If

        Dim cash As Decimal = 0D
        If Not usesElectronicPayment AndAlso Not Decimal.TryParse(cashTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, cash) Then
            MessageBox.Show("Enter a valid cash amount.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cashTextBox.Focus()
            Return
        End If

        If Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex > 0 AndAlso
           (String.IsNullOrWhiteSpace(customerNameTextBox.Text) OrElse String.IsNullOrWhiteSpace(idNumberTextBox.Text)) Then
            MessageBox.Show("Enter the customer's name and ID number for the selected customer type.", "Customer Details Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If String.IsNullOrWhiteSpace(customerNameTextBox.Text) Then
                customerNameTextBox.Focus()
            Else
                idNumberTextBox.Focus()
            End If
            Return
        End If

        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim discount = Decimal.Round(subtotal * CheckoutDiscountRate(), 2)
        Dim totalDue = subtotal - discount
        If usesElectronicPayment Then cash = totalDue
        If cash < totalDue Then
            MessageBox.Show("Insufficient cash received. Total due is P " & totalDue.ToString("N2"), "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim rxDate As DateTime? = Nothing
        If Not checkoutModeIsWholesale AndAlso cart.Any(Function(item) item.IsRx) Then
            rxDate = rxDateTextBox.Value.Date
        End If

        Dim sale As New SaleRecord With {
            .SaleMode = If(checkoutModeIsWholesale, "Wholesale", "Retail"),
            .WholesaleClientId = If(checkoutModeIsWholesale AndAlso selectedWholesaleCustomer IsNot Nothing,
                                    CType(selectedWholesaleCustomer.CustomerId, Integer?),
                                    Nothing),
            .CustomerType = If(checkoutModeIsWholesale, "Wholesale", customerTypeCombo.Text),
            .CustomerName = If(checkoutModeIsWholesale, contactPersonTextBox.Text.Trim(), customerNameTextBox.Text.Trim()),
            .CustomerIdNumber = If(checkoutModeIsWholesale, customerLicenseTextBox.Text.Trim(), idNumberTextBox.Text.Trim()),
            .Subtotal = subtotal,
            .DiscountAmount = discount,
            .TotalAmount = totalDue,
            .AmountReceived = cash,
            .ChangeAmount = cash - totalDue,
            .PrescriptionNumber = If(checkoutModeIsWholesale, Nothing, rxNoTextBox.Text.Trim()),
            .PrescriberName = If(checkoutModeIsWholesale, Nothing, rxPractitionerTextBox.Text.Trim()),
            .PrescriberLicense = If(checkoutModeIsWholesale, Nothing, rxLicenseTextBox.Text.Trim()),
            .PrescriptionDate = rxDate,
            .PurchaseOrderNumber = If(checkoutModeIsWholesale, currentWholesalePurchaseOrderNumber, Nothing),
            .PurchaseOrderDate = If(checkoutModeIsWholesale, CType(currentWholesalePurchaseOrderDate, DateTime?), Nothing),
            .DeliveryDate = If(checkoutModeIsWholesale, CType(deliveryDatePicker.Value.Date, DateTime?), Nothing),
            .PaymentMethod = If(usesElectronicPayment, "E-payment", "Cash"),
            .PaymentProvider = If(usesElectronicPayment, Convert.ToString(wholesalePaymentModeCombo.SelectedItem), Nothing),
            .PaymentAccountName = If(usesElectronicPayment, wholesalePaymentAccountNameBox.Text.Trim(), Nothing),
            .PaymentReferenceNumber = If(usesElectronicPayment, wholesalePaymentReferenceBox.Text.Trim(), Nothing),
            .Items = cart.Select(Function(item) New SaleLineRecord With {
                .ProductId = item.ProductID,
                .BrandName = item.BrandName,
                .GenericName = item.GenericName,
                .Dosage = item.Dosage,
                .Unit = item.Unit,
                .Quantity = item.Quantity,
                .UnitPrice = item.Price,
                .LineTotal = item.LineTotal,
                .IsPrescription = item.IsRx
            }).ToList()
        }

        Dim saleId As Long
        Try
            saleId = New SalesRepository().SaveSale(sale)
        Catch ex As Exception
            Dim insufficientStock = ex.Message.StartsWith("Insufficient Saleable Stock:", StringComparison.OrdinalIgnoreCase)
            MessageBox.Show("The transaction was not saved, so the cart is still available. " & ex.Message,
                            If(insufficientStock, "Insufficient Saleable Stock", "Checkout failed"),
                            MessageBoxButtons.OK,
                            If(insufficientStock, MessageBoxIcon.Warning, MessageBoxIcon.Error))
            Return
        End Try

        Dim saleMode = If(checkoutModeIsWholesale, "Wholesale", "Retail")
        Dim saleDate = DateTime.Now
        Dim receiptNumber = "RCPT-" & saleId.ToString("D8", CultureInfo.InvariantCulture)
        Dim receiptLines = BuildReceiptLines(receiptNumber, saleId, saleDate, sale)
        Dim receiptPdfPath As String = Nothing
        Try
            receiptPdfPath = CreateReceiptPdf(receiptNumber, saleDate, receiptLines)
        Catch ex As Exception
            MessageBox.Show("The sale was saved, but its PDF receipt could not be created. Use Print Receipt to try again. " & ex.Message,
                            "Receipt PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        ShowPaymentSuccessDialog(saleId, receiptNumber, saleMode, subtotal, discount, totalDue, cash, cash - totalDue, receiptPdfPath, saleDate, receiptLines, usesElectronicPayment)

        cart.Clear()
        RenderCart()
        LoadProducts()
        RefreshSelectedProductInventoryWarnings()
        checkoutView.Visible = False
        transactionView.Visible = True
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
        customerNameTextBox.Clear()
        idNumberTextBox.Clear()
    End Sub

    Private Sub ShowPaymentSuccessDialog(transactionNumber As Long,
                                         receiptNumber As String,
                                         saleMode As String,
                                         subtotal As Decimal,
                                         discount As Decimal,
                                         totalDue As Decimal,
                                         cashReceived As Decimal,
                                         change As Decimal,
                                         receiptPdfPath As String,
                                         saleDate As DateTime,
                                         receiptLines As List(Of String),
                                         Optional isElectronicPayment As Boolean = False)
        Using dialog As New Form()
            dialog.Text = String.Empty
            dialog.FormBorderStyle = FormBorderStyle.None
            dialog.ControlBox = False
            dialog.StartPosition = FormStartPosition.CenterParent
            dialog.ClientSize = New Size(340, 536)
            dialog.MaximizeBox = False
            dialog.MinimizeBox = False
            dialog.ShowInTaskbar = False
            dialog.BackColor = Color.White
            dialog.Font = New Font("Segoe UI", 9.0F)

            Dim successIcon As New Label With {
                .Text = "✓",
                .Font = New Font("Segoe UI", 25.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(20, 135, 75),
                .BackColor = Color.FromArgb(224, 250, 234),
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(140, 12),
                .Size = New Size(60, 60)
            }
            Dim title As New Label With {
                .Text = "Payment Successful",
                .Font = New Font("Segoe UI", 15.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(25, 35, 50),
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(12, 78),
                .Size = New Size(316, 34)
            }
            Dim transactionLabel As New Label With {
                .Text = receiptNumber & "  |  Transaction #" & transactionNumber.ToString(CultureInfo.InvariantCulture) & " · " & saleMode,
                .ForeColor = Color.Gray,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(12, 111),
                .Size = New Size(316, 26)
            }
            dialog.Controls.Add(successIcon)
            dialog.Controls.Add(title)
            dialog.Controls.Add(transactionLabel)
            AddDialogDivider(dialog, 148)
            AddDialogAmountRow(dialog, "Subtotal", "P" & subtotal.ToString("N2"), 158, Color.White, Color.Black, False)
            AddDialogAmountRow(dialog, "Discount", "P" & discount.ToString("N2"), 196, Color.White, Color.Black, False)
            AddDialogDivider(dialog, 234)
            AddDialogAmountRow(dialog, "Total Due", "P" & totalDue.ToString("N2"), 243, Color.White, darkGreen, True)
            AddDialogAmountRow(dialog, If(isElectronicPayment, "Amount Paid", "Cash Received"), "P" & cashReceived.ToString("N2"), 305, Color.FromArgb(248, 249, 250), Color.Black, False)
            AddDialogAmountRow(dialog, "Change", "P" & change.ToString("N2"), 346, Color.FromArgb(248, 249, 250), Color.FromArgb(20, 135, 75), True)

            Dim doneButton As New Button With {
                .Text = "Done",
                .BackColor = darkGreen,
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 11.0F, FontStyle.Bold),
                .Location = New Point(14, 407),
                .Size = New Size(312, 50),
                .DialogResult = DialogResult.OK,
                .UseVisualStyleBackColor = False
            }
            doneButton.FlatAppearance.BorderSize = 0
            Dim printButton As New Button With {
                .Text = "Print Receipt",
                .BackColor = Color.White,
                .ForeColor = Color.FromArgb(35, 45, 60),
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .Location = New Point(14, 466),
                .Size = New Size(312, 48),
                .UseVisualStyleBackColor = False
            }
            printButton.FlatAppearance.BorderColor = Color.LightGray
            AddHandler printButton.Click, Sub(sender, e) OpenReceiptPdf(receiptPdfPath, receiptNumber, saleDate, receiptLines, dialog)
            dialog.Controls.Add(doneButton)
            dialog.Controls.Add(printButton)
            dialog.AcceptButton = doneButton
            dialog.ShowDialog(Me)
        End Using
    End Sub

    Private Sub AddDialogAmountRow(dialog As Form, caption As String, amount As String, top As Integer, backColor As Color, amountColor As Color, bold As Boolean)
        Dim row As New Panel With {
            .BackColor = backColor,
            .Location = New Point(0, top),
            .Size = New Size(dialog.ClientSize.Width, 38)
        }
        Dim style = If(bold, FontStyle.Bold, FontStyle.Regular)
        row.Controls.Add(New Label With {
            .Text = caption,
            .ForeColor = Color.FromArgb(105, 115, 130),
            .Font = New Font("Segoe UI", 9.0F, style),
            .Location = New Point(13, 7),
            .Size = New Size(180, 24),
            .TextAlign = ContentAlignment.MiddleLeft
        })
        row.Controls.Add(New Label With {
            .Text = amount,
            .ForeColor = amountColor,
            .Font = New Font("Segoe UI", 10.0F, style),
            .Location = New Point(193, 7),
            .Size = New Size(134, 24),
            .TextAlign = ContentAlignment.MiddleRight
        })
        dialog.Controls.Add(row)
    End Sub

    Private Sub AddDialogDivider(dialog As Form, top As Integer)
        Dim divider As New Panel With {
            .BackColor = Color.FromArgb(230, 233, 237),
            .Location = New Point(13, top),
            .Size = New Size(dialog.ClientSize.Width - 26, 1)
        }
        dialog.Controls.Add(divider)
    End Sub

    Private Function BuildReceiptLines(receiptNumber As String,
                                       transactionNumber As Long,
                                       saleDate As DateTime,
                                       sale As SaleRecord) As List(Of String)
        Dim lines As New List(Of String) From {
            "CABRERA'S DRUGSTORE AND MEDICAL SUPPLIES",
            "SALES RECEIPT",
            New String("-"c, 76),
            "Receipt No.: " & receiptNumber,
            "Transaction No.: " & transactionNumber.ToString(CultureInfo.InvariantCulture),
            "Date and Time: " & saleDate.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
            "Sale Type: " & sale.SaleMode,
            "Cashier Initials: " & ReceiptStaffInitials(CurrentStaffInitials)
        }

        Dim paymentDescription = If(String.Equals(sale.PaymentMethod, "E-payment", StringComparison.OrdinalIgnoreCase),
                                    "E-payment" & If(String.IsNullOrWhiteSpace(sale.PaymentProvider), String.Empty, " - " & sale.PaymentProvider),
                                    If(String.IsNullOrWhiteSpace(sale.PaymentMethod), "Cash", sale.PaymentMethod))

        If String.Equals(sale.SaleMode, "Wholesale", StringComparison.OrdinalIgnoreCase) Then
            If selectedWholesaleCustomer IsNot Nothing Then
                lines.Add("Company: " & selectedWholesaleCustomer.CompanyName)
                lines.Add("FDA LTO No.: " & selectedWholesaleCustomer.LicenseNumber)
                lines.Add("Contact Person: " & (selectedWholesaleCustomer.FirstName & " " & selectedWholesaleCustomer.LastName).Trim())
                lines.Add("Contact No.: " & selectedWholesaleCustomer.ContactNumber)
                Dim shippingAddress = String.Join(", ", {
                    selectedWholesaleCustomer.ShippingAddressLine,
                    selectedWholesaleCustomer.Barangay,
                    selectedWholesaleCustomer.CityMunicipality,
                    selectedWholesaleCustomer.Province,
                    selectedWholesaleCustomer.PostalCode
                }.Where(Function(part) Not String.IsNullOrWhiteSpace(part)))
                If Not String.IsNullOrWhiteSpace(shippingAddress) Then lines.Add("Shipping Address: " & shippingAddress)
            End If
            If Not String.IsNullOrWhiteSpace(sale.PurchaseOrderNumber) Then lines.Add("Purchase Order No.: " & sale.PurchaseOrderNumber)
            If sale.PurchaseOrderDate.HasValue Then lines.Add("Purchase Order Date: " & sale.PurchaseOrderDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
            lines.Add("Mode of Payment: " & paymentDescription)
            If Not String.IsNullOrWhiteSpace(sale.PaymentAccountName) Then lines.Add("Account Name: " & sale.PaymentAccountName)
            If Not String.IsNullOrWhiteSpace(sale.PaymentReferenceNumber) Then lines.Add("Payment Reference No.: " & sale.PaymentReferenceNumber)
        Else
            lines.Add("Customer Type: " & sale.CustomerType)
            If Not String.IsNullOrWhiteSpace(sale.CustomerName) Then lines.Add("Customer Name: " & sale.CustomerName)
            If Not String.IsNullOrWhiteSpace(sale.CustomerIdNumber) Then lines.Add("Customer ID: " & sale.CustomerIdNumber)
            lines.Add("Mode of Payment: " & paymentDescription)
            If String.Equals(sale.PaymentMethod, "E-payment", StringComparison.OrdinalIgnoreCase) Then
                If Not String.IsNullOrWhiteSpace(sale.PaymentAccountName) Then lines.Add("Account Name: " & sale.PaymentAccountName)
                If Not String.IsNullOrWhiteSpace(sale.PaymentReferenceNumber) Then lines.Add("Payment Reference No.: " & sale.PaymentReferenceNumber)
            End If
        End If

        lines.Add(New String("-"c, 76))
        lines.Add("ITEMS")
        For Each item In sale.Items
            Dim description = item.BrandName
            If Not String.IsNullOrWhiteSpace(item.GenericName) Then description &= " - " & item.GenericName
            If Not String.IsNullOrWhiteSpace(item.Dosage) Then description &= " - " & item.Dosage
            lines.Add(description)
            lines.Add("  " & item.Quantity.ToString(CultureInfo.InvariantCulture) & " " & item.Unit &
                      " x PHP " & item.UnitPrice.ToString("N2", CultureInfo.InvariantCulture) &
                      " = PHP " & item.LineTotal.ToString("N2", CultureInfo.InvariantCulture))
        Next

        If sale.Items.Any(Function(item) item.IsPrescription) Then
            lines.Add(New String("-"c, 76))
            lines.Add("PRESCRIPTION INFORMATION")
            lines.Add("Prescription No.: " & If(sale.PrescriptionNumber, String.Empty))
            lines.Add("Prescribing Practitioner: " & If(sale.PrescriberName, String.Empty))
            lines.Add("PRC License No.: " & If(sale.PrescriberLicense, String.Empty))
            If sale.PrescriptionDate.HasValue Then
                lines.Add("Prescription Date: " & sale.PrescriptionDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
            End If
        End If

        lines.Add(New String("-"c, 76))
        lines.Add("Subtotal: PHP " & sale.Subtotal.ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Discount: PHP " & sale.DiscountAmount.ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("TOTAL DUE: PHP " & sale.TotalAmount.ToString("N2", CultureInfo.InvariantCulture))
        lines.Add(If(String.Equals(sale.PaymentMethod, "E-payment", StringComparison.OrdinalIgnoreCase), "Amount Paid: PHP ", "Cash Received: PHP ") & sale.AmountReceived.ToString("N2", CultureInfo.InvariantCulture))
        lines.Add("Change: PHP " & sale.ChangeAmount.ToString("N2", CultureInfo.InvariantCulture))
        lines.Add(New String("-"c, 76))
        lines.Add("Thank you for your purchase!")
        Return lines
    End Function

    Public Shared Function CreateReceiptPdf(receiptNumber As String, saleDate As DateTime, receiptLines As List(Of String)) As String
        Dim receiptFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "retail_receipts")
        Directory.CreateDirectory(receiptFolder)

        Dim fileName = receiptNumber & "_" & saleDate.ToString("MM-dd-yy", CultureInfo.InvariantCulture) & ".pdf"
        Dim outputPath = Path.Combine(receiptFolder, fileName)
        Dim pageStreams As New List(Of StringBuilder) From {New StringBuilder()}
        Dim pageIndex = 0
        Dim y = 790

        For lineIndex = 0 To receiptLines.Count - 1
            Dim line = ToPdfText(receiptLines(lineIndex))
            For Each wrappedLine In WrapPdfLine(line, 88)
                If y < 48 Then
                    pageStreams.Add(New StringBuilder())
                    pageIndex += 1
                    y = 790
                End If

                Dim isTitle = lineIndex <= 1
                Dim fontName = If(isTitle, "F2", "F1")
                Dim fontSize = If(lineIndex = 0, 15, If(lineIndex = 1, 11, 10))
                pageStreams(pageIndex).Append("BT /").Append(fontName).Append(" ").Append(fontSize.ToString(CultureInfo.InvariantCulture))
                pageStreams(pageIndex).Append(" Tf 50 ").Append(y.ToString(CultureInfo.InvariantCulture)).Append(" Td (")
                pageStreams(pageIndex).Append(EscapePdfText(wrappedLine)).AppendLine(") Tj ET")
                y -= If(isTitle, 22, 15)
            Next
        Next

        Dim objects As New List(Of Byte())
        objects.Add(Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"))
        Dim pageReferences As New List(Of String)
        For index = 0 To pageStreams.Count - 1
            pageReferences.Add((5 + index * 2).ToString(CultureInfo.InvariantCulture) & " 0 R")
        Next
        objects.Add(Encoding.ASCII.GetBytes("<< /Type /Pages /Kids [" & String.Join(" ", pageReferences) & "] /Count " & pageStreams.Count.ToString(CultureInfo.InvariantCulture) & " >>"))
        objects.Add(Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"))
        objects.Add(Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>"))

        For index = 0 To pageStreams.Count - 1
            Dim pageObjectNumber = 5 + index * 2
            Dim contentObjectNumber = pageObjectNumber + 1
            objects.Add(Encoding.ASCII.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 3 0 R /F2 4 0 R >> >> /Contents " & contentObjectNumber.ToString(CultureInfo.InvariantCulture) & " 0 R >>"))
            Dim streamBytes = Encoding.ASCII.GetBytes(pageStreams(index).ToString())
            Using stream As New MemoryStream()
                Dim header = Encoding.ASCII.GetBytes("<< /Length " & streamBytes.Length.ToString(CultureInfo.InvariantCulture) & " >>" & vbLf & "stream" & vbLf)
                stream.Write(header, 0, header.Length)
                stream.Write(streamBytes, 0, streamBytes.Length)
                Dim footer = Encoding.ASCII.GetBytes(vbLf & "endstream")
                stream.Write(footer, 0, footer.Length)
                objects.Add(stream.ToArray())
            End Using
        Next

        Using pdf As New MemoryStream()
            WritePdfAscii(pdf, "%PDF-1.4" & vbLf)
            Dim offsets As New List(Of Long)
            For index = 0 To objects.Count - 1
                offsets.Add(pdf.Position)
                WritePdfAscii(pdf, (index + 1).ToString(CultureInfo.InvariantCulture) & " 0 obj" & vbLf)
                pdf.Write(objects(index), 0, objects(index).Length)
                WritePdfAscii(pdf, vbLf & "endobj" & vbLf)
            Next

            Dim xrefOffset = pdf.Position
            WritePdfAscii(pdf, "xref" & vbLf & "0 " & (objects.Count + 1).ToString(CultureInfo.InvariantCulture) & vbLf)
            WritePdfAscii(pdf, "0000000000 65535 f " & vbLf)
            For Each offset In offsets
                WritePdfAscii(pdf, offset.ToString("D10", CultureInfo.InvariantCulture) & " 00000 n " & vbLf)
            Next
            WritePdfAscii(pdf, "trailer" & vbLf & "<< /Size " & (objects.Count + 1).ToString(CultureInfo.InvariantCulture) & " /Root 1 0 R >>" & vbLf)
            WritePdfAscii(pdf, "startxref" & vbLf & xrefOffset.ToString(CultureInfo.InvariantCulture) & vbLf & "%%EOF")
            File.WriteAllBytes(outputPath, pdf.ToArray())
        End Using

        Return outputPath
    End Function

    Public Shared Sub CreateReportPdf(outputPath As String, reportLines As List(Of String))
        If String.IsNullOrWhiteSpace(outputPath) Then Throw New ArgumentException("A report PDF path is required.", NameOf(outputPath))
        If reportLines Is Nothing Then Throw New ArgumentNullException(NameOf(reportLines))
        Dim outputFolder = Path.GetDirectoryName(outputPath)
        If Not String.IsNullOrWhiteSpace(outputFolder) Then Directory.CreateDirectory(outputFolder)

        Dim pageStreams As New List(Of StringBuilder) From {New StringBuilder()}
        Dim widths As Double() = {55, 98, 95, 90, 55, 170, 50, 75, 60}
        Dim tableX = 47.0R
        Dim summaryRowCount = reportLines.Where(Function(line) line.StartsWith("@SUMMARY|", StringComparison.Ordinal) OrElse line.StartsWith("@MONTHSUMMARY|", StringComparison.Ordinal)).Count()
        Dim summaryTop = 426.0R
        Dim summaryHeight = 38.0R + (summaryRowCount * 19.0R)
        Dim summaryBottom = summaryTop - summaryHeight
        Dim summaryY = summaryTop - 24.0R
        Dim totalY = summaryBottom + 14.0R
        Dim transactionSectionY = summaryBottom - 28.0R
        Dim currentY = transactionSectionY - 27.0R
        Dim tableHeader As String() = Nothing
        Dim currentMonthCaption As String = Nothing
        Dim currentMonthSummary As String = Nothing
        Dim metaLineIndex As Integer = 0

        If reportLines.Count > 0 Then AppendCenteredPdfText(pageStreams(0), "F2", 16, 47, 748, 558, ToPdfText(reportLines(0)))
        If reportLines.Count > 1 Then AppendCenteredPdfText(pageStreams(0), "F2", 13, 47, 748, 532, ToPdfText(reportLines(1)))

        For Each rawLine In reportLines.Skip(2)
            Dim line = ToPdfText(rawLine)
            If line.StartsWith("@META|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                Dim metadataY = If(metaLineIndex = 0, 498.0R, 476.0R)
                metaLineIndex += 1
                If fields.Length >= 2 Then AppendPdfKeyValue(pageStreams(0), 58, metadataY, fields(1))
                If fields.Length >= 3 Then AppendPdfKeyValue(pageStreams(0), 430, metadataY, fields(2))
            ElseIf line.StartsWith("@SECTION|", StringComparison.Ordinal) Then
                Dim sectionName = line.Substring("@SECTION|".Length)
                Dim isSummarySection = sectionName.EndsWith("SUMMARY", StringComparison.OrdinalIgnoreCase)
                Dim sectionY = If(isSummarySection, 448.0R, transactionSectionY)
                AppendCenteredPdfText(pageStreams(0), "F2", 12, 47, 748, sectionY, sectionName)
                If isSummarySection Then DrawPdfSummaryFrame(pageStreams(0), summaryBottom, summaryHeight)
            ElseIf line.StartsWith("@SUMMARY|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                If fields.Length >= 3 Then
                    Dim labelX = If(fields.Length >= 5 OrElse fields(1).StartsWith("Retail", StringComparison.OrdinalIgnoreCase), 135.0R, 455.0R)
                    Dim valueX = If(fields.Length >= 5 OrElse fields(1).StartsWith("Retail", StringComparison.OrdinalIgnoreCase), 270.0R, 590.0R)
                    AppendPdfText(pageStreams(0), "F2", 11, labelX, summaryY, fields(1))
                    AppendPdfText(pageStreams(0), "F1", 11, valueX, summaryY, fields(2))
                    If fields.Length >= 5 Then
                        AppendPdfText(pageStreams(0), "F2", 11, 455, summaryY, fields(3))
                        AppendPdfText(pageStreams(0), "F1", 11, 590, summaryY, fields(4))
                    End If
                    summaryY -= 19
                End If
            ElseIf line.StartsWith("@MONTHSUMMARY|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                If fields.Length >= 3 Then
                    AppendPdfText(pageStreams(0), "F2", 10, 135, summaryY, fields(1))
                    AppendPdfText(pageStreams(0), "F1", 10, 270, summaryY, fields(2))
                    If fields.Length >= 5 Then
                        AppendPdfText(pageStreams(0), "F2", 10, 455, summaryY, fields(3))
                        AppendPdfText(pageStreams(0), "F1", 10, 590, summaryY, fields(4))
                    End If
                    summaryY -= 19
                End If
            ElseIf line.StartsWith("@TOTAL|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                pageStreams(0).Append("q 0.88 0.95 0.91 rg 101 ").Append((totalY - 14).ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 640 28 re f Q")
                pageStreams(0).Append("0.8 w 100 ").Append((totalY - 14).ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 642 29 re S")
                pageStreams(0).AppendLine("0.08 0.39 0.24 rg")
                If fields.Length >= 3 Then
                    AppendPdfText(pageStreams(0), "F2", 11, 120, totalY - 3, fields(1))
                    AppendPdfText(pageStreams(0), "F2", 11, Math.Max(220, 725 - ApproxPdfTextWidth(fields(2), 11)), totalY - 3, fields(2))
                ElseIf fields.Length = 2 Then
                    AppendCenteredPdfText(pageStreams(0), "F2", 10, 105, 632, totalY - 3, fields(1))
                End If
                pageStreams(0).AppendLine("0 g")
            ElseIf line.StartsWith("@MONTH|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                If fields.Length >= 4 Then
                    currentMonthCaption = fields(1)
                    currentMonthSummary = fields(2) & "    " & fields(3)
                    If currentY < 75 Then
                        pageStreams.Add(New StringBuilder())
                        currentY = 548
                    End If
                    DrawPdfMonthBanner(pageStreams(pageStreams.Count - 1), tableX, currentY, widths.Sum(), currentMonthCaption, currentMonthSummary)
                    currentY -= 26
                    tableHeader = Nothing
                End If
            ElseIf line.StartsWith("@NOTE|", StringComparison.Ordinal) Then
                AppendCenteredPdfText(pageStreams(pageStreams.Count - 1), "F1", 11, tableX, widths.Sum(), currentY, line.Substring("@NOTE|".Length))
            ElseIf line.StartsWith("@TABLE|", StringComparison.Ordinal) Then
                Dim cells = line.Split("|"c).Skip(1).ToArray()
                If tableHeader Is Nothing Then
                    tableHeader = cells
                    widths = BuildPdfTableWidths(cells, 748.0R)
                    If currentY - 13 < 38 Then
                        pageStreams.Add(New StringBuilder())
                        currentY = 548
                        If Not String.IsNullOrWhiteSpace(currentMonthCaption) Then
                            DrawPdfMonthBanner(pageStreams(pageStreams.Count - 1), tableX, currentY, widths.Sum(), currentMonthCaption, currentMonthSummary)
                            currentY -= 26
                        End If
                    End If
                    Dim headerHeight = DrawPdfReportTableRow(pageStreams(pageStreams.Count - 1), tableX, currentY, widths, cells, True)
                    currentY -= headerHeight
                Else
                    Dim rowHeight = GetPdfReportRowHeight(widths, cells, False)
                    If currentY - rowHeight / 2 < 38 Then
                        pageStreams.Add(New StringBuilder())
                        Dim nextPage = pageStreams.Count - 1
                        currentY = 548
                        If Not String.IsNullOrWhiteSpace(currentMonthCaption) Then
                            DrawPdfMonthBanner(pageStreams(nextPage), tableX, currentY, widths.Sum(), currentMonthCaption, currentMonthSummary)
                            currentY -= 26
                        End If
                        Dim headerHeight = DrawPdfReportTableRow(pageStreams(nextPage), tableX, currentY, widths, tableHeader, True)
                        currentY -= headerHeight
                    End If
                    rowHeight = DrawPdfReportTableRow(pageStreams(pageStreams.Count - 1), tableX, currentY, widths, cells, False)
                    currentY -= rowHeight
                End If
            End If
        Next

        Dim objects As New List(Of Byte()) From {
            Encoding.ASCII.GetBytes("<< /Type /Catalog /Pages 2 0 R >>"),
            Nothing,
            Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>"),
            Encoding.ASCII.GetBytes("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>")
        }
        Dim pageReferences As New List(Of String)
        For index = 0 To pageStreams.Count - 1
            pageReferences.Add((5 + index * 2).ToString(CultureInfo.InvariantCulture) & " 0 R")
        Next
        objects(1) = Encoding.ASCII.GetBytes("<< /Type /Pages /Kids [" & String.Join(" ", pageReferences) & "] /Count " & pageStreams.Count.ToString(CultureInfo.InvariantCulture) & " >>")
        For index = 0 To pageStreams.Count - 1
            Dim pageObjectNumber = 5 + index * 2
            Dim contentObjectNumber = pageObjectNumber + 1
            objects.Add(Encoding.ASCII.GetBytes("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 842 595] /Resources << /Font << /F1 3 0 R /F2 4 0 R >> >> /Contents " & contentObjectNumber.ToString(CultureInfo.InvariantCulture) & " 0 R >>"))
            Dim streamBytes = Encoding.ASCII.GetBytes(pageStreams(index).ToString())
            Using stream As New MemoryStream()
                Dim header = Encoding.ASCII.GetBytes("<< /Length " & streamBytes.Length.ToString(CultureInfo.InvariantCulture) & " >>" & vbLf & "stream" & vbLf)
                stream.Write(header, 0, header.Length)
                stream.Write(streamBytes, 0, streamBytes.Length)
                Dim footer = Encoding.ASCII.GetBytes(vbLf & "endstream")
                stream.Write(footer, 0, footer.Length)
                objects.Add(stream.ToArray())
            End Using
        Next

        Using pdf As New MemoryStream()
            WritePdfAscii(pdf, "%PDF-1.4" & vbLf)
            Dim offsets As New List(Of Long)
            For index = 0 To objects.Count - 1
                offsets.Add(pdf.Position)
                WritePdfAscii(pdf, (index + 1).ToString(CultureInfo.InvariantCulture) & " 0 obj" & vbLf)
                pdf.Write(objects(index), 0, objects(index).Length)
                WritePdfAscii(pdf, vbLf & "endobj" & vbLf)
            Next
            Dim xrefOffset = pdf.Position
            WritePdfAscii(pdf, "xref" & vbLf & "0 " & (objects.Count + 1).ToString(CultureInfo.InvariantCulture) & vbLf)
            WritePdfAscii(pdf, "0000000000 65535 f " & vbLf)
            For Each offset In offsets
                WritePdfAscii(pdf, offset.ToString("D10", CultureInfo.InvariantCulture) & " 00000 n " & vbLf)
            Next
            WritePdfAscii(pdf, "trailer" & vbLf & "<< /Size " & (objects.Count + 1).ToString(CultureInfo.InvariantCulture) & " /Root 1 0 R >>" & vbLf)
            WritePdfAscii(pdf, "startxref" & vbLf & xrefOffset.ToString(CultureInfo.InvariantCulture) & vbLf & "%%EOF")
            File.WriteAllBytes(outputPath, pdf.ToArray())
        End Using
    End Sub

    Private Shared Function BuildPdfTableWidths(headerCells As String(), totalWidth As Double) As Double()
        If headerCells Is Nothing OrElse headerCells.Length = 0 Then Return {totalWidth}
        ' Keep exact Sales Report column proportions for the 9-column transaction item table
        If headerCells.Length = 9 AndAlso headerCells(0).IndexOf("TRANSACTION", StringComparison.OrdinalIgnoreCase) >= 0 Then
            Return {55, 98, 95, 90, 55, 170, 50, 75, 60}
        End If
        Dim colWidth = totalWidth / headerCells.Length
        Return Enumerable.Repeat(colWidth, headerCells.Length).ToArray()
    End Function
    Private Shared Sub AppendCenteredPdfText(builder As StringBuilder, fontName As String, fontSize As Integer, x As Double, width As Double, y As Double, value As String)
        Dim textWidth = ApproxPdfTextWidth(value, fontSize)
        AppendPdfText(builder, fontName, fontSize, x + Math.Max(0, (width - textWidth) / 2), y, value)
    End Sub

    Private Shared Sub AppendPdfKeyValue(builder As StringBuilder, x As Double, y As Double, value As String)
        Dim colonIndex = value.IndexOf(":"c)
        If colonIndex <= 0 Then
            AppendPdfText(builder, "F1", 11, x, y, value)
            Return
        End If
        Dim key = value.Substring(0, colonIndex + 1)
        Dim detail = value.Substring(colonIndex + 1).Trim()
        AppendPdfText(builder, "F2", 11, x, y, key)
        AppendPdfText(builder, "F1", 11, x + ApproxPdfTextWidth(key & " ", 11), y, detail)
    End Sub

    Private Shared Function DrawPdfReportTableRow(builder As StringBuilder, x As Double, centerY As Double, widths As Double(), cells As String(), isHeader As Boolean) As Double
        Dim wrappedCells As New List(Of List(Of String))
        For index = 0 To widths.Length - 1
            Dim cellText = If(index < cells.Length, ToPdfText(cells(index).Trim()), String.Empty)
            wrappedCells.Add(WrapPdfCellText(cellText, widths(index) - 8, If(isHeader, 9, 8)))
        Next
        Dim fontSize = If(isHeader, 9, 8)
        Dim rowHeight = If(isHeader, 26.0R, Math.Max(24.0R, wrappedCells.Max(Function(cell) cell.Count) * 10.0R + 8))
        Dim bottom = centerY - rowHeight / 2
        Dim cellX = x
        For index = 0 To widths.Length - 1
            Dim width = widths(index)
            If isHeader Then
                builder.Append("q 0.89 0.94 0.91 rg ").Append(cellX.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
                builder.Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ").Append(width.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
                builder.Append(rowHeight.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" re f Q")
            End If
            builder.Append("0.65 w ").Append(cellX.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
            builder.Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ").Append(width.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
            builder.Append(rowHeight.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" re S")
            Dim lines = wrappedCells(index)
            Dim lineY = centerY + ((lines.Count - 1) * 10.0R) / 2 - 3
            For Each cellLine In lines
                Dim textWidth = ApproxPdfTextWidth(cellLine, fontSize)
                Dim textX = cellX + Math.Max(4, (width - textWidth) / 2)
                AppendPdfText(builder, If(isHeader, "F2", "F1"), fontSize, textX, lineY, cellLine)
                lineY -= 10
            Next
            cellX += width
        Next
        Return rowHeight
    End Function

    Private Shared Function GetPdfReportRowHeight(widths As Double(), cells As String(), isHeader As Boolean) As Double
        Dim maxLines = 1
        For index = 0 To widths.Length - 1
            Dim value = If(index < cells.Length, ToPdfText(cells(index).Trim()), String.Empty)
            maxLines = Math.Max(maxLines, WrapPdfCellText(value, widths(index) - 8, If(isHeader, 9, 8)).Count)
        Next
        Return If(isHeader, 26.0R, Math.Max(24.0R, maxLines * 10.0R + 8))
    End Function

    Private Shared Function WrapPdfCellText(value As String, maxWidth As Double, fontSize As Integer) As List(Of String)
        Dim result As New List(Of String)
        Dim words = If(value, String.Empty).Split(" "c, StringSplitOptions.RemoveEmptyEntries)
        Dim current As String = String.Empty
        For Each word In words
            Dim candidate = If(current.Length = 0, word, current & " " & word)
            If ApproxPdfTextWidth(candidate, fontSize) <= maxWidth Then
                current = candidate
                Continue For
            End If
            If current.Length > 0 Then result.Add(current)
            current = String.Empty
            Dim chunk = String.Empty
            For Each character In word
                If chunk.Length > 0 AndAlso ApproxPdfTextWidth(chunk & character, fontSize) > maxWidth Then
                    result.Add(chunk)
                    chunk = String.Empty
                End If
                chunk &= character
            Next
            current = chunk
        Next
        If current.Length > 0 Then result.Add(current)
        If result.Count = 0 Then result.Add(String.Empty)
        Return result
    End Function

    Private Shared Sub DrawPdfSummaryFrame(builder As StringBuilder, bottom As Double, height As Double)
        builder.Append("q 0.97 0.985 0.98 rg 100 ").Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" 642 ").Append(height.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" re f Q")
        builder.Append("0.8 w 100 ").Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" 642 ").Append(height.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" re S")
        builder.Append("0.55 w 100 ").Append((bottom + 29).ToString("0.##", CultureInfo.InvariantCulture)).Append(" m 742 ").Append((bottom + 29).ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" l S")
    End Sub

    Private Shared Sub DrawPdfMonthBanner(builder As StringBuilder, x As Double, centerY As Double, width As Double, monthCaption As String, summary As String)
        Dim bottom = centerY - 10
        builder.Append("q 0.91 0.96 0.93 rg ").Append(x.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
        builder.Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ").Append(width.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 22 re f Q")
        builder.Append("0.65 w ").Append(x.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
        builder.Append(bottom.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ").Append(width.ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 22 re S")
        AppendPdfText(builder, "F2", 10, x + 10, centerY - 3, monthCaption)
        AppendPdfText(builder, "F1", 9, x + width - ApproxPdfTextWidth(summary, 9) - 10, centerY - 3, summary)
    End Sub

    Private Shared Sub AppendPdfText(builder As StringBuilder, fontName As String, fontSize As Integer, x As Double, y As Double, value As String)
        builder.Append("BT /").Append(fontName).Append(" ").Append(fontSize.ToString(CultureInfo.InvariantCulture))
        builder.Append(" Tf ").Append(x.ToString("0.##", CultureInfo.InvariantCulture)).Append(" ")
        builder.Append(y.ToString("0.##", CultureInfo.InvariantCulture)).Append(" Td (")
        builder.Append(EscapePdfText(value)).AppendLine(") Tj ET")
    End Sub

    Private Shared Function ApproxPdfTextWidth(value As String, fontSize As Integer) As Double
        Dim units As Double = 0
        For Each character In value
            If character = " "c Then
                units += 0.28
            ElseIf "ilI.,:;'!|".Contains(character) Then
                units += 0.28
            ElseIf "MW@%".Contains(character) Then
                units += 0.82
            ElseIf Char.IsUpper(character) Then
                units += 0.62
            ElseIf Char.IsDigit(character) Then
                units += 0.56
            Else
                units += 0.52
            End If
        Next
        Return units * fontSize
    End Function

    Private Shared Function WrapPdfLine(text As String, maxLength As Integer) As List(Of String)
        If String.IsNullOrEmpty(text) Then Return New List(Of String) From {String.Empty}
        Dim wrapped As New List(Of String)
        Dim remaining = text.TrimEnd()
        While remaining.Length > maxLength
            Dim splitAt = remaining.LastIndexOf(" "c, Math.Min(maxLength, remaining.Length - 1))
            If splitAt <= 0 Then splitAt = maxLength
            wrapped.Add(remaining.Substring(0, splitAt))
            remaining = remaining.Substring(splitAt).TrimStart()
        End While
        wrapped.Add(remaining)
        Return wrapped
    End Function

    Private Shared Function ToPdfText(value As String) As String
        If value Is Nothing Then Return String.Empty
        Return value.Replace("₱", "PHP ").Replace("–", "-").Replace("—", "-").Replace("·", "-")
    End Function

    Private Shared Function EscapePdfText(value As String) As String
        Dim safeText = New String(value.Select(Function(character) If(AscW(character) >= 32 AndAlso AscW(character) <= 126, character, "?"c)).ToArray())
        Return safeText.Replace("\", "\\").Replace("(", "\(").Replace(")", "\)")
    End Function

    Private Shared Sub WritePdfAscii(stream As Stream, value As String)
        Dim bytes = Encoding.ASCII.GetBytes(value)
        stream.Write(bytes, 0, bytes.Length)
    End Sub

    Private Sub OpenReceiptPdf(receiptPdfPath As String,
                               receiptNumber As String,
                               saleDate As DateTime,
                               receiptLines As List(Of String),
                               owner As IWin32Window)
        Try
            Dim path = receiptPdfPath
            If String.IsNullOrWhiteSpace(path) OrElse Not File.Exists(path) Then
                path = CreateReceiptPdf(receiptNumber, saleDate, receiptLines)
            End If
            Process.Start(New ProcessStartInfo(path) With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to open the PDF receipt: " & ex.Message, "Print Receipt", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub headerPanel_Paint(sender As Object, e As PaintEventArgs) Handles headerPanel.Paint

    End Sub
End Class