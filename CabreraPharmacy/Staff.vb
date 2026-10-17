Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports System.Diagnostics
Imports MySql.Data.MySqlClient

Public Class Staff

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

    Private ReadOnly darkGreen As Color = Color.FromArgb(20, 100, 60)
    Private ReadOnly cart As New List(Of CartItem)()
    Private products As DataTable
    Private currentProduct As DataRow
    Private isWholesale As Boolean
    Private checkoutModeIsWholesale As Boolean
    Private clearClickedSelection As Boolean

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        PositionLogoutButton()
        welcomestaff_label.Text = "Welcome, " & CurrentFullName & " (Staff)"
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
        searchFilterCombo.SelectedIndex = 0
        AddHandler quantityPanel.Resize, AddressOf QuantityPanel_Resize
        AddHandler cartCard.Resize, AddressOf CartCard_Resize
        AddHandler checkoutView.Resize, AddressOf CheckoutView_Resize
        AddHandler rxNoTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxPractitionerTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxLicenseTextBox.TextChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler rxDateTextBox.ValueChanged, AddressOf PrescriptionInputs_TextChanged
        AddHandler qtyTextBox.TextChanged, AddressOf QuantityInputs_TextChanged
        AddHandler customerNameTextBox.TextChanged, AddressOf CustomerIdentity_TextChanged
        AddHandler idNumberTextBox.TextChanged, AddressOf CustomerIdentity_TextChanged
        AddHandler cashTextBox.KeyPress, AddressOf cashTextBox_KeyPress
        AddHandler resultsGrid.CellMouseDown, AddressOf resultsGrid_CellMouseDown
        AddHandler resultsGrid.CellClick, AddressOf resultsGrid_CellClick
        QuantityPanel_Resize(quantityPanel, EventArgs.Empty)
        CartCard_Resize(cartCard, EventArgs.Empty)
        CheckoutView_Resize(checkoutView, EventArgs.Empty)
        customerTypeCombo.SelectedIndex = 0
        checkoutView.Visible = False
        restockView.Visible = False
        transactionView.Visible = True
        SetTransactionMode(False)
        RenderCart()
        LoadProducts()
    End Sub

    Private Sub sidebar_Resize(sender As Object, e As EventArgs) Handles sidebar.Resize
        PositionLogoutButton()
    End Sub

    Private Sub PositionLogoutButton()
        If sidebar Is Nothing OrElse logoutstaff_button Is Nothing Then Return
        logoutstaff_button.Visible = True
        logoutstaff_button.BringToFront()
        logoutstaff_button.Location = New Point(20, Math.Max(20, sidebar.ClientSize.Height - logoutstaff_button.Height - 30))
        logoutstaff_button.Width = Math.Max(100, sidebar.ClientSize.Width - 40)
    End Sub

    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
    End Sub

    Private Sub QuantityPanel_Resize(sender As Object, e As EventArgs)
        addToCartButton.Location = New Point(Math.Max(qtyPlusButton.Right + 16, quantityPanel.ClientSize.Width - addToCartButton.Width - 12), 7)
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

        orderSummaryLabel.Top = 220
        Dim summaryTop = 250
        prescriptionLabel.Top = Math.Max(520, viewHeight - 95)
        prescriptionValueLabel.Top = prescriptionLabel.Top + 25
        orderSummaryPanel.SetBounds(20, summaryTop, Math.Max(120, reviewWidth - 40), Math.Max(120, prescriptionLabel.Top - summaryTop - 55))
        rxRecordedLabel.Left = Math.Max(220, reviewWidth - rxRecordedLabel.Width - 20)
        rxRecordedLabel.Top = prescriptionValueLabel.Top

        Dim paymentWidth = paymentCard.ClientSize.Width
        subtotalValue.Left = paymentWidth - subtotalValue.Width - 20
        discountValue.Left = paymentWidth - discountValue.Width - 20
        totalDueValue.Left = paymentWidth - totalDueValue.Width - 20
        cashTextBox.Width = 150
        cashTextBox.Left = paymentWidth - cashTextBox.Width - 20
        cashTextBox.Top = cashLabel.Top - 4
        cashValue.Left = cashTextBox.Left - cashValue.Width - 8
        cashValue.Top = cashTextBox.Top + 3
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
        subtitleLabel.Text = "Cabrera's Drugstore and Medical Supplies"
        navTransaction.BackColor = If(transactionView.Visible, darkGreen, Color.White)
        navTransaction.ForeColor = If(transactionView.Visible, Color.White, darkGreen)
        navRestock.BackColor = If(restockView.Visible, darkGreen, Color.White)
        navRestock.ForeColor = If(restockView.Visible, Color.White, darkGreen)
    End Sub

    Private Sub logoutstaff_button_Click(sender As Object, e As EventArgs) Handles logoutstaff_button.Click
        If MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Try
            RecordUserActivity("Logout")
        Catch ex As Exception
            MessageBox.Show("Your session will close, but the logout activity could not be recorded. " & ex.Message,
                            "Activity log", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub retailModeButton_Click(sender As Object, e As EventArgs) Handles retailModeButton.Click
        SetTransactionMode(False)
    End Sub

    Private Sub wholesaleModeButton_Click(sender As Object, e As EventArgs) Handles wholesaleModeButton.Click
        SetTransactionMode(True)
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
        addToCartButton.Text = If(isWholesale, "ADD TO CART", "ADD TO CART")
        titleLabel.Text = "Transaction"
        If isWholesale Then
            subtitleLabel.Text = "Wholesale Box Price"
        Else
            subtitleLabel.Text = "Retail Unit Price"
        End If
        If products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub LoadProducts()
        Try
            Dim manager As New ProductManager()
            products = manager.GetProducts(includeInactive:=False)
            If products Is Nothing Then products = New DataTable()
            searchFilterCombo.BeginUpdate()
            searchFilterCombo.Items.Clear()
            searchFilterCombo.Items.Add("All Categories")
            For Each category In products.AsEnumerable().Select(Function(row) Convert.ToString(row("Category"))).Where(Function(name) Not String.IsNullOrWhiteSpace(name)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(Function(name) name)
                searchFilterCombo.Items.Add(category)
            Next
            searchFilterCombo.SelectedIndex = 0
            searchFilterCombo.EndUpdate()
            BindSearchResults(products)
        Catch ex As Exception
            MessageBox.Show("Unable to load products: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub BindSearchResults(source As DataTable)
        Dim result As New DataTable()
        result.Columns.Add("Product ID", GetType(Integer))
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
            If Not String.Equals(selectedCategory, "All Categories", StringComparison.OrdinalIgnoreCase) AndAlso
               Not String.Equals(category, selectedCategory, StringComparison.OrdinalIgnoreCase) Then Continue For

            Dim retailPrice As Decimal
            Dim wholesalePrice As Decimal
            Decimal.TryParse(row("Retail Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, retailPrice)
            Decimal.TryParse(row("Wholesale Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, wholesalePrice)
            Dim price = If(isWholesale, wholesalePrice, retailPrice)
            Dim isRx = IsPrescriptionRequired(row)
            Dim type = If(isRx, "Rx", "OTC")
            Dim searchable = String.Join(" ", {
                Convert.ToString(row("ID")),
                Convert.ToString(row("Brand Name")),
                Convert.ToString(row("Generic Name"))
            })
            If keyword.Length > 0 AndAlso searchable.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0 Then Continue For

            result.Rows.Add(Convert.ToInt32(row("ID")), row("Generic Name").ToString(), row("Brand Name").ToString(), row("Unit").ToString(), row("Dosage").ToString(), type, isRx, price)
        Next

        resultsGrid.DataSource = result
        If resultsGrid.Columns.Contains("Price") Then
            resultsGrid.Columns("Price").DefaultCellStyle.Format = "'P' #,##0.00"
        End If
        If resultsGrid.Columns.Contains("Requires Prescription") Then
            resultsGrid.Columns("Requires Prescription").Visible = False
        End If
        If resultsGrid.Columns.Contains("Product ID") Then
            resultsGrid.Columns("Product ID").FillWeight = 60
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
            UpdatePrescriptionEntryAvailability()
            Return
        End If

        Dim productId As Integer
        If Not Integer.TryParse(Convert.ToString(resultsGrid.CurrentRow.Cells("Product ID").Value), productId) OrElse products Is Nothing Then
            currentProduct = Nothing
            UpdatePrescriptionEntryAvailability()
            Return
        End If

        currentProduct = products.AsEnumerable().FirstOrDefault(Function(row) Convert.ToInt32(row("ID")) = productId)
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
                                       UpdatePrescriptionEntryAvailability()
                                   End If
                               End Sub))
    End Sub

    Private Sub searchFilterCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles searchFilterCombo.SelectedIndexChanged
        If products IsNot Nothing Then BindSearchResults(products)
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

        Dim price As Decimal
        Dim priceColumn = If(isWholesale, "Wholesale Price", "Retail Price")
        Decimal.TryParse(currentProduct(priceColumn).ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, price)
        Dim id = Convert.ToInt32(currentProduct("ID"))
        Dim existing = cart.FirstOrDefault(Function(item) item.ProductID = id)
        If existing Is Nothing Then
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
            existing.Quantity += quantity
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

    Private Sub customerSearchButton_Click(sender As Object, e As EventArgs) Handles customerSearchButton.Click
        MessageBox.Show("Customer records are not available in the current database. Enter the wholesale customer details manually.", "Customer Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub checkoutButton_Click(sender As Object, e As EventArgs) Handles checkoutButton.Click
        If cart.Count = 0 Then
            MessageBox.Show("Cart is empty. Add items before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If isWholesale AndAlso
           (String.IsNullOrWhiteSpace(customerLicenseTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(contactPersonTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(contactNoTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(addressTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(poNoTextBox.Text)) Then
            MessageBox.Show("Complete the wholesale customer and purchase order details before checkout.", "Wholesale Details Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        cashTextBox.Text = "0.00"
        cashValue.Text = "P"
        customerTypeCombo.Enabled = Not checkoutModeIsWholesale
        customerNameTextBox.Text = If(checkoutModeIsWholesale, contactPersonTextBox.Text, "")
        idNumberTextBox.Text = If(checkoutModeIsWholesale, customerLicenseTextBox.Text, "")
        UpdateCustomerCheckoutFields()
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
        customerNameLabel.Visible = True
        customerNameTextBox.Visible = True
        idNumberLabel.Visible = True
        idNumberTextBox.Visible = True
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
        Dim identityRequired = Not checkoutModeIsWholesale AndAlso customerTypeCombo.SelectedIndex > 0
        Dim identityComplete = Not String.IsNullOrWhiteSpace(customerNameTextBox.Text) AndAlso
                               Not String.IsNullOrWhiteSpace(idNumberTextBox.Text)
        Dim cash As Decimal
        Dim cashValid = Decimal.TryParse(cashTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, cash) AndAlso cash >= 0D
        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim totalDue = subtotal - Decimal.Round(subtotal * CheckoutDiscountRate(), 2)
        Dim canConfirm = (Not identityRequired OrElse identityComplete) AndAlso cashValid AndAlso cash >= totalDue
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
        subtotalValue.Text = "P " & subtotal.ToString("N2")
        discountValue.Text = "P " & discount.ToString("N2")
        totalDueValue.Text = "P " & (subtotal - discount).ToString("N2")
        cashValue.Text = "P"
        Dim change = cash - (subtotal - discount)
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
        cashTextBox.Text = "0.00"
        checkoutView.Visible = False
        transactionView.Visible = True
        restockView.Visible = False
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
        navTransaction.BackColor = darkGreen
        navTransaction.ForeColor = Color.White
        navRestock.BackColor = Color.White
        navRestock.ForeColor = darkGreen
    End Sub

    Private Sub confirmButton_Click(sender As Object, e As EventArgs) Handles confirmButton.Click
        If CurrentUserID <= 0 Then
            MessageBox.Show("No signed-in staff account is associated with this transaction. Sign in again before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim cash As Decimal
        If Not Decimal.TryParse(cashTextBox.Text, NumberStyles.Number, CultureInfo.CurrentCulture, cash) Then
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
            .WholesaleCustomerLicense = If(checkoutModeIsWholesale, customerLicenseTextBox.Text.Trim(), Nothing),
            .ContactPerson = If(checkoutModeIsWholesale, contactPersonTextBox.Text.Trim(), Nothing),
            .ContactNumber = If(checkoutModeIsWholesale, contactNoTextBox.Text.Trim(), Nothing),
            .CustomerAddress = If(checkoutModeIsWholesale, addressTextBox.Text.Trim(), Nothing),
            .PurchaseOrderNumber = If(checkoutModeIsWholesale, poNoTextBox.Text.Trim(), Nothing),
            .PurchaseOrderDate = If(checkoutModeIsWholesale, CType(poDatePicker.Value.Date, DateTime?), Nothing),
            .DeliveryDate = If(checkoutModeIsWholesale, CType(deliveryDatePicker.Value.Date, DateTime?), Nothing),
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
            MessageBox.Show("The transaction was not saved, so the cart is still available. " & ex.Message,
                            "Checkout failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        ShowPaymentSuccessDialog(saleId, receiptNumber, saleMode, subtotal, discount, totalDue, cash, cash - totalDue, receiptPdfPath, saleDate, receiptLines)

        cart.Clear()
        RenderCart()
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
                                         receiptLines As List(Of String))
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
            AddDialogAmountRow(dialog, "Cash Received", "P" & cashReceived.ToString("N2"), 305, Color.FromArgb(248, 249, 250), Color.Black, False)
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
            "Cashier: " & If(String.IsNullOrWhiteSpace(CurrentFullName), CurrentUsername, CurrentFullName),
            "Customer Type: " & sale.CustomerType
        }

        If Not String.IsNullOrWhiteSpace(sale.CustomerName) Then lines.Add("Customer Name: " & sale.CustomerName)
        If Not String.IsNullOrWhiteSpace(sale.CustomerIdNumber) Then lines.Add("Customer ID: " & sale.CustomerIdNumber)

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
        lines.Add("Cash Received: PHP " & sale.AmountReceived.ToString("N2", CultureInfo.InvariantCulture))
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

        If reportLines.Count > 0 Then AppendCenteredPdfText(pageStreams(0), "F2", 16, 47, 748, 558, ToPdfText(reportLines(0)))
        If reportLines.Count > 1 Then AppendCenteredPdfText(pageStreams(0), "F2", 13, 47, 748, 532, ToPdfText(reportLines(1)))

        For Each rawLine In reportLines.Skip(2)
            Dim line = ToPdfText(rawLine)
            If line.StartsWith("@META|", StringComparison.Ordinal) Then
                Dim fields = line.Split("|"c)
                If fields.Length >= 3 Then
                    Dim metadataY = If(fields(1).StartsWith("Report Period:", StringComparison.Ordinal), 498.0R, 476.0R)
                    AppendPdfKeyValue(pageStreams(0), 58, metadataY, fields(1))
                    AppendPdfKeyValue(pageStreams(0), 430, metadataY, fields(2))
                End If
            ElseIf line.StartsWith("@SECTION|", StringComparison.Ordinal) Then
                Dim sectionName = line.Substring("@SECTION|".Length)
                Dim sectionY = If(sectionName = "SALES SUMMARY", 448.0R, transactionSectionY)
                AppendCenteredPdfText(pageStreams(0), "F2", 12, 47, 748, sectionY, sectionName)
                If sectionName = "SALES SUMMARY" Then DrawPdfSummaryFrame(pageStreams(0), summaryBottom, summaryHeight)
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
                If fields.Length >= 3 Then
                    pageStreams(0).Append("q 0.88 0.95 0.91 rg 101 ").Append((totalY - 14).ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 640 28 re f Q")
                    pageStreams(0).Append("0.8 w 100 ").Append((totalY - 14).ToString("0.##", CultureInfo.InvariantCulture)).AppendLine(" 642 29 re S")
                    pageStreams(0).AppendLine("0.08 0.39 0.24 rg")
                    AppendPdfText(pageStreams(0), "F2", 12, 155, totalY, fields(1))
                    AppendPdfText(pageStreams(0), "F2", 12, 520, totalY, fields(2))
                    pageStreams(0).AppendLine("0 g")
                End If
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
