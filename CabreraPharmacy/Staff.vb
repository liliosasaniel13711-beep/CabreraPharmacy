Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
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

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        welcomestaff_label.Text = "Welcome, " & CurrentFullName & " (Staff)"
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
        searchFilterCombo.SelectedIndex = 0
        AddHandler searchPanel.Resize, AddressOf SearchPanel_Resize
        AddHandler quantityPanel.Resize, AddressOf QuantityPanel_Resize
        AddHandler cartCard.Resize, AddressOf CartCard_Resize
        AddHandler checkoutView.Resize, AddressOf CheckoutView_Resize
        SearchPanel_Resize(searchPanel, EventArgs.Empty)
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

    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
    End Sub

    Private Sub SearchPanel_Resize(sender As Object, e As EventArgs)
        Dim buttonLeft = Math.Max(searchTextBox.Left + 100, searchPanel.ClientSize.Width - searchButton.Width - 12)
        searchButton.Location = New Point(buttonLeft, 8)
        searchTextBox.Width = Math.Max(80, buttonLeft - searchTextBox.Left - 10)
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

        orderSummaryPanel.SetBounds(20, 250, Math.Max(120, reviewWidth - 40), Math.Max(120, viewHeight - 365))
        prescriptionLabel.Top = Math.Max(520, viewHeight - 95)
        prescriptionValueLabel.Top = prescriptionLabel.Top + 25
        rxRecordedLabel.Left = Math.Max(220, reviewWidth - rxRecordedLabel.Width - 20)
        rxRecordedLabel.Top = prescriptionValueLabel.Top

        Dim paymentWidth = paymentCard.ClientSize.Width
        subtotalValue.Left = paymentWidth - subtotalValue.Width - 20
        discountValue.Left = paymentWidth - discountValue.Width - 20
        totalDueValue.Left = paymentWidth - totalDueValue.Width - 20
        cashValue.Left = paymentWidth - cashValue.Width - 20
        changeValue.Left = paymentWidth - changeValue.Width - 20
        cancelButton.Left = paymentWidth - confirmButton.Width - cancelButton.Width - 30
        confirmButton.Left = paymentWidth - confirmButton.Width - 20
        cancelButton.Top = viewHeight - cancelButton.Height - 16
        confirmButton.Top = viewHeight - confirmButton.Height - 16
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs) Handles navTransaction.Click, navRestock.Click
        Dim button = TryCast(sender, Button)
        If button Is Nothing Then Return

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
        Close()
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
        qtyTextBox.Text = "1"
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
            products = manager.GetProducts()
            If products Is Nothing Then products = New DataTable()
            BindSearchResults(products)
        Catch ex As Exception
            MessageBox.Show("Unable to load products: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub searchButton_Click(sender As Object, e As EventArgs) Handles searchButton.Click
        LoadProducts()
    End Sub

    Private Sub searchTextBox_TextChanged(sender As Object, e As EventArgs) Handles searchTextBox.TextChanged
        If products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub searchTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles searchTextBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            searchButton.PerformClick()
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
        result.Columns.Add("Price", GetType(Decimal))

        Dim keyword = searchTextBox.Text.Trim()
        Dim selectedFilter = If(searchFilterCombo.SelectedItem, "Generic Name").ToString()

        For Each row As DataRow In source.Rows
            Dim matches As Boolean = True
            If keyword.Length > 0 Then
                Select Case selectedFilter
                    Case "All"
                        matches = row("ID").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                  row("Generic Name").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                  row("Brand Name").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                    Case "Product ID"
                        matches = row("ID").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                    Case "Brand Name"
                        matches = row("Brand Name").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                    Case Else
                        matches = row("Generic Name").ToString().IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
                End Select
            End If

            If Not matches Then Continue For

            Dim retailPrice As Decimal
            Dim wholesalePrice As Decimal
            Decimal.TryParse(row("Retail Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, retailPrice)
            Decimal.TryParse(row("Wholesale Price").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, wholesalePrice)
            Dim price = If(isWholesale, wholesalePrice, retailPrice)
            Dim type = If(IsPrescriptionRequired(Convert.ToInt32(row("ID"))), "Rx", "OTC")
            result.Rows.Add(Convert.ToInt32(row("ID")), row("Generic Name").ToString(), row("Brand Name").ToString(), row("Unit").ToString(), row("Dosage").ToString(), type, price)
        Next

        resultsGrid.DataSource = result
        If resultsGrid.Columns.Contains("Price") Then
            resultsGrid.Columns("Price").DefaultCellStyle.Format = "'P' #,##0.00"
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
    End Sub

    Private Function IsPrescriptionRequired(productId As Integer) As Boolean
        Try
            Using connection = GetConnection()
                connection.Open()
                Const query = "SELECT c.prescription_required FROM product p INNER JOIN product_category c ON p.category_ID = c.category_ID WHERE p.product_ID = @productId LIMIT 1"
                Using command As New MySqlCommand(query, connection)
                    command.Parameters.AddWithValue("@productId", productId)
                    Dim result = command.ExecuteScalar()
                    Return result IsNot Nothing AndAlso Not Convert.IsDBNull(result) AndAlso Convert.ToInt32(result) <> 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Unable to determine prescription status: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub resultsGrid_SelectionChanged(sender As Object, e As EventArgs) Handles resultsGrid.SelectionChanged
        If resultsGrid.CurrentRow Is Nothing OrElse resultsGrid.CurrentRow.IsNewRow Then
            currentProduct = Nothing
            Return
        End If

        Dim productId As Integer
        If Not Integer.TryParse(Convert.ToString(resultsGrid.CurrentRow.Cells("Product ID").Value), productId) OrElse products Is Nothing Then
            currentProduct = Nothing
            Return
        End If

        currentProduct = products.AsEnumerable().FirstOrDefault(Function(row) Convert.ToInt32(row("ID")) = productId)
    End Sub

    Private Sub searchFilterCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles searchFilterCombo.SelectedIndexChanged
        If products IsNot Nothing Then BindSearchResults(products)
    End Sub

    Private Sub qtyMinusButton_Click(sender As Object, e As EventArgs) Handles qtyMinusButton.Click
        AdjustQuantity(-1)
    End Sub

    Private Sub qtyPlusButton_Click(sender As Object, e As EventArgs) Handles qtyPlusButton.Click
        AdjustQuantity(1)
    End Sub

    Private Sub AdjustQuantity(delta As Integer)
        Dim quantity As Integer
        If Not Integer.TryParse(qtyTextBox.Text, quantity) Then quantity = 1
        quantity = Math.Max(1, quantity + delta)
        qtyTextBox.Text = quantity.ToString(CultureInfo.InvariantCulture)
    End Sub

    Private Sub addToCartButton_Click(sender As Object, e As EventArgs) Handles addToCartButton.Click
        If currentProduct Is Nothing Then
            MessageBox.Show("Select a product from the results table first.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim quantity As Integer
        If Not Integer.TryParse(qtyTextBox.Text, quantity) OrElse quantity < 1 Then
            MessageBox.Show("Enter a quantity of at least 1.", "Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            qtyTextBox.Focus()
            Return
        End If

        Dim isRx = IsPrescriptionRequired(Convert.ToInt32(currentProduct("ID")))
        If Not isWholesale AndAlso isRx AndAlso
           (String.IsNullOrWhiteSpace(rxNoTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxPractitionerTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxLicenseTextBox.Text) OrElse
            String.IsNullOrWhiteSpace(rxDateTextBox.Text)) Then
            MessageBox.Show("Complete the prescription information before adding an Rx item.", "Prescription Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
        BuildCheckoutView()
        transactionView.Visible = False
        restockView.Visible = False
        checkoutView.Visible = True
        titleLabel.Text = "Order Review"
        subtitleLabel.Text = If(isWholesale, "Wholesale transaction", "Retail transaction")
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
        Dim discount As Decimal = 0D
        If Not checkoutModeIsWholesale AndAlso (customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2) Then
            discount = Decimal.Round(subtotal * 0.2D, 2)
        End If
        subtotalValue.Text = "P " & subtotal.ToString("N2")
        discountValue.Text = "P " & discount.ToString("N2")
        totalDueValue.Text = "P " & (subtotal - discount).ToString("N2")
        cashTextBox.Clear()
        cashValue.Text = "P 0.00"
        changeValue.Text = "P 0.00"
        customerTypeCombo.Enabled = Not checkoutModeIsWholesale
        customerNameTextBox.Text = If(checkoutModeIsWholesale, contactPersonTextBox.Text, "")
        idNumberTextBox.Text = If(checkoutModeIsWholesale, customerLicenseTextBox.Text, "")
        prescriptionLabel.Visible = Not checkoutModeIsWholesale AndAlso cart.Any(Function(item) item.IsRx)
        prescriptionValueLabel.Visible = prescriptionLabel.Visible
        rxRecordedLabel.Visible = prescriptionLabel.Visible
        prescriptionValueLabel.Text = If(prescriptionLabel.Visible, "RX- " & rxNoTextBox.Text, "")
        CheckoutView_Resize(checkoutView, EventArgs.Empty)
    End Sub

    Private Sub customerTypeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles customerTypeCombo.SelectedIndexChanged
        If checkoutView.Visible AndAlso Not checkoutModeIsWholesale Then BuildCheckoutView()
    End Sub

    Private Sub cashTextBox_TextChanged(sender As Object, e As EventArgs) Handles cashTextBox.TextChanged
        Dim cash As Decimal
        Decimal.TryParse(cashTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, cash)
        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim discount As Decimal = 0D
        If Not checkoutModeIsWholesale AndAlso (customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2) Then
            discount = Decimal.Round(subtotal * 0.2D, 2)
        End If
        cashValue.Text = "P " & cash.ToString("N2")
        Dim change = cash - (subtotal - discount)
        changeValue.Text = "P " & change.ToString("N2")
        changeValue.ForeColor = If(change < 0, Color.FromArgb(200, 30, 30), Color.Black)
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        checkoutView.Visible = False
        transactionView.Visible = True
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
    End Sub

    Private Sub confirmButton_Click(sender As Object, e As EventArgs) Handles confirmButton.Click
        Dim cash As Decimal
        If Not Decimal.TryParse(cashTextBox.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, cash) Then
            MessageBox.Show("Enter a valid cash amount.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cashTextBox.Focus()
            Return
        End If

        Dim subtotal = cart.Sum(Function(item) item.LineTotal)
        Dim discount As Decimal = 0D
        If Not checkoutModeIsWholesale AndAlso (customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2) Then
            discount = Decimal.Round(subtotal * 0.2D, 2)
        End If
        Dim totalDue = subtotal - discount
        If cash < totalDue Then
            MessageBox.Show("Insufficient cash received. Total due is P " & totalDue.ToString("N2"), "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim receipt = "Transaction Complete" & vbCrLf &
                      "Mode: " & If(checkoutModeIsWholesale, "Wholesale", "Retail") & vbCrLf &
                      "Subtotal: P " & subtotal.ToString("N2") & vbCrLf &
                      "Discount: P " & discount.ToString("N2") & vbCrLf &
                      "Total Due: P " & totalDue.ToString("N2") & vbCrLf &
                      "Cash: P " & cash.ToString("N2") & vbCrLf &
                      "Change: P " & (cash - totalDue).ToString("N2")
        If checkoutModeIsWholesale Then
            receipt &= vbCrLf & "Customer: " & contactPersonTextBox.Text & vbCrLf & "PO No.: " & poNoTextBox.Text
        End If
        MessageBox.Show(receipt, "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information)

        cart.Clear()
        RenderCart()
        checkoutView.Visible = False
        transactionView.Visible = True
        titleLabel.Text = "Transaction"
        subtitleLabel.Text = If(isWholesale, "Wholesale Box Price", "Retail Unit Price")
        customerNameTextBox.Clear()
        idNumberTextBox.Clear()
    End Sub

End Class