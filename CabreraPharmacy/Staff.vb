Imports System.Drawing
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

    Private darkGreen As Color = Color.FromArgb(20, 100, 60)

    Private navButtons As Button()
    Private contentPanels As Dictionary(Of String, Panel)

    ' Retail state
    Private currentProduct As DataRow = Nothing
    Private cart As New List(Of CartItem)()

    ' Wholesale state
    Private currentWholesaleProduct As DataRow = Nothing
    Private wholesaleCart As New List(Of CartItem)()

    ' Checkout state
    Private checkoutIsWholesale As Boolean = False

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        welcomestaff_label.Text = "Welcome, " & CurrentFullName & " (Staff)"

        navButtons = {navRetail, navWholesale, navRestock}
        contentPanels = New Dictionary(Of String, Panel) From {
            {"Retail", retailView},
            {"Checkout", checkoutView},
            {"Wholesale", wholesaleView},
            {"Restock", restockView}
        }

        customerTypeCombo.SelectedIndex = 0

        ShowView("Retail")
    End Sub

    ' Keep the user info panel pinned to the top-right corner
    Private Sub HeaderPanel_Layout(sender As Object, e As LayoutEventArgs) Handles headerPanel.Layout
        userPanel.Location = New Point(headerPanel.ClientSize.Width - userPanel.Width - 20, 10)
    End Sub

    ' ===================== NAVIGATION =====================

    Private Sub NavButton_Click(sender As Object, e As EventArgs) Handles navRetail.Click, navWholesale.Click, navRestock.Click
        Dim btn = TryCast(sender, Button)
        If btn IsNot Nothing Then
            ShowView(CStr(btn.Tag))
        End If
    End Sub

    Private Sub logoutstaff_button_Click(sender As Object, e As EventArgs) Handles logoutstaff_button.Click
        Close()
    End Sub

    Private Sub ShowView(viewName As String)
        For Each kvp In contentPanels
            kvp.Value.Visible = (kvp.Key = viewName)
        Next

        ' Highlight nav buttons only for the three primary views
        For Each btn In navButtons
            Dim isActive As Boolean = (CStr(btn.Tag) = viewName)
            btn.BackColor = If(isActive, darkGreen, Color.White)
            btn.ForeColor = If(isActive, Color.White, darkGreen)
        Next

        Select Case viewName
            Case "Retail"
                titleLabel.Text = "Retail"
            Case "Wholesale"
                titleLabel.Text = "Wholesale"
            Case "Restock"
                titleLabel.Text = "Restock"
            Case "Checkout"
                titleLabel.Text = If(checkoutIsWholesale, "Wholesale", "Retail")
        End Select
        subtitleLabel.Text = "Cabrera's Drugstore and Medical Supplies"
    End Sub

    ' ===================== SEARCH & PRODUCT DETAILS =====================

    Private Sub searchButton_Click(sender As Object, e As EventArgs) Handles searchButton.Click
        Dim keyword = searchTextBox.Text.Trim()
        If String.IsNullOrEmpty(keyword) Then
            MessageBox.Show("Enter a medicine name to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Dim pm As New ProductManager()
            Dim dt = pm.GetProducts(keyword)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("No product found matching """ & keyword & """.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            currentProduct = dt.Rows(0)
            BindProductDetails(currentProduct, prodNameLabel, prodIdLabel, prodGenericLabel, prodBrandLabel, prodFormLabel, prodStrengthLabel, prodTypeLabel, prodPriceLabel, prodAvailableLabel, prodExpiryLabel, prodBatchLabel, rxSectionLabel, rxNoTextBox, rxPractitionerTextBox, rxLicenseTextBox, rxDateTextBox, False)
        Catch ex As Exception
            MessageBox.Show("Search failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub wholesaleSearchButton_Click(sender As Object, e As EventArgs) Handles wholesaleSearchButton.Click
        Dim keyword = wholesaleSearchTextBox.Text.Trim()
        If String.IsNullOrEmpty(keyword) Then
            MessageBox.Show("Enter a medicine name to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Dim pm As New ProductManager()
            Dim dt = pm.GetProducts(keyword)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("No product found matching """ & keyword & """.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            currentWholesaleProduct = dt.Rows(0)
            BindProductDetails(currentWholesaleProduct, wProdNameLabel, wProdIdLabel, wProdGenericLabel, wProdBrandLabel, wProdFormLabel, wProdStrengthLabel, wProdTypeLabel, wProdPriceLabel, wProdAvailableLabel, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True)
        Catch ex As Exception
            MessageBox.Show("Search failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BindProductDetails(row As DataRow,
                                   nameLabel As Label,
                                   idLabel As Label,
                                   genericLabel As Label,
                                   brandLabel As Label,
                                   formLabel As Label,
                                   strengthLabel As Label,
                                   typeLabel As Label,
                                   priceLabel As Label,
                                   availableLabel As Label,
                                   expiryLabel As Label,
                                   batchLabel As Label,
                                   rxSection As Label,
                                   rxNo As TextBox,
                                   rxPractitioner As TextBox,
                                   rxLicense As TextBox,
                                   rxDate As TextBox,
                                   isWholesale As Boolean)

        Dim brand = row("Brand Name").ToString()
        Dim generic = row("Generic Name").ToString()
        Dim dosage = row("Dosage").ToString()
        Dim unit = row("Unit").ToString()
        Dim qtyPerBox As Integer = 0
        Integer.TryParse(row("Qty/Box").ToString(), qtyPerBox)

        ' Determine Rx vs OTC from the category's prescription flag
        Dim isRx = IsPrescriptionRequired(Convert.ToInt32(row("ID")))

        nameLabel.Text = brand & " " & dosage & " " & unit
        idLabel.Text = "Product ID: " & row("ID").ToString()
        genericLabel.Text = "Generic name: " & generic
        brandLabel.Text = "Brand name: " & brand
        formLabel.Text = "Form: " & unit
        strengthLabel.Text = "Strength: " & dosage
        typeLabel.Text = "Type: " & If(isRx, "Rx", "OTC")

        Dim price As Decimal = 0D
        If isWholesale Then
            Decimal.TryParse(row("Wholesale Price").ToString(), price)
            priceLabel.Text = "WHOLESALE PRICE: P " & price.ToString("N2")
        Else
            Decimal.TryParse(row("Retail Price").ToString(), price)
            priceLabel.Text = "PRICE: P " & price.ToString("N2")
        End If

        availableLabel.Text = "Available: " & qtyPerBox.ToString() & " " & unit.ToLower() & If(qtyPerBox <> 1, "s", "")

        If expiryLabel IsNot Nothing Then expiryLabel.Text = "Expiry Date: N/A"
        If batchLabel IsNot Nothing Then batchLabel.Text = "Batch No.: N/A"

        ' Show/hide prescription section
        Dim showRx As Boolean = isRx AndAlso Not isWholesale
        If rxSection IsNot Nothing Then rxSection.Visible = showRx
        If rxNo IsNot Nothing Then rxNo.Visible = showRx
        If rxPractitioner IsNot Nothing Then rxPractitioner.Visible = showRx
        If rxLicense IsNot Nothing Then rxLicense.Visible = showRx
        If rxDate IsNot Nothing Then rxDate.Visible = showRx
    End Sub

    Private Function IsPrescriptionRequired(productId As Integer) As Boolean
        Try
            Using conn = GetConnection()
                conn.Open()
                Const query = "SELECT c.prescription_required FROM product p INNER JOIN product_category c ON p.category_ID = c.category_ID WHERE p.product_ID = @pid LIMIT 1"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@pid", productId)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not Convert.IsDBNull(result) Then
                        Return Convert.ToInt32(result) <> 0
                    End If
                End Using
            End Using
        Catch
            ' Default to OTC on lookup failure
        End Try
        Return False
    End Function

    ' ===================== QUANTITY =====================

    Private Sub qtyMinusButton_Click(sender As Object, e As EventArgs) Handles qtyMinusButton.Click
        AdjustQuantity(qtyTextBox, -1)
    End Sub

    Private Sub qtyPlusButton_Click(sender As Object, e As EventArgs) Handles qtyPlusButton.Click
        AdjustQuantity(qtyTextBox, 1)
    End Sub

    Private Sub wQtyMinusButton_Click(sender As Object, e As EventArgs) Handles wQtyMinusButton.Click
        AdjustQuantity(wQtyTextBox, -1)
    End Sub

    Private Sub wQtyPlusButton_Click(sender As Object, e As EventArgs) Handles wQtyPlusButton.Click
        AdjustQuantity(wQtyTextBox, 1)
    End Sub

    Private Sub AdjustQuantity(txt As TextBox, delta As Integer)
        Dim q As Integer = 1
        Integer.TryParse(txt.Text, q)
        q += delta
        If q < 1 Then q = 1
        txt.Text = q.ToString()
    End Sub

    ' ===================== ADD TO CART =====================

    Private Sub addToCartButton_Click(sender As Object, e As EventArgs) Handles addToCartButton.Click
        If currentProduct Is Nothing Then
            MessageBox.Show("Search and select a product first.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim qty As Integer = 1
        Integer.TryParse(qtyTextBox.Text, qty)
        If qty < 1 Then qty = 1

        Dim price As Decimal = 0D
        Decimal.TryParse(currentProduct("Retail Price").ToString(), price)
        Dim isRx = IsPrescriptionRequired(Convert.ToInt32(currentProduct("ID")))

        ' Merge if already in cart
        Dim existing = cart.Find(Function(c) c.ProductID = Convert.ToInt32(currentProduct("ID")))
        If existing IsNot Nothing Then
            existing.Quantity += qty
        Else
            cart.Add(New CartItem With {
                .ProductID = Convert.ToInt32(currentProduct("ID")),
                .BrandName = currentProduct("Brand Name").ToString(),
                .GenericName = currentProduct("Generic Name").ToString(),
                .Dosage = currentProduct("Dosage").ToString(),
                .Unit = currentProduct("Unit").ToString(),
                .Price = price,
                .Quantity = qty,
                .IsRx = isRx
            })
        End If

        RenderCart()
        MessageBox.Show(qtyTextBox.Text & " x " & currentProduct("Brand Name").ToString() & " added to cart.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub wAddToCartButton_Click(sender As Object, e As EventArgs) Handles wAddToCartButton.Click
        If currentWholesaleProduct Is Nothing Then
            MessageBox.Show("Search and select a product first.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim qty As Integer = 1
        Integer.TryParse(wQtyTextBox.Text, qty)
        If qty < 1 Then qty = 1

        Dim price As Decimal = 0D
        Decimal.TryParse(currentWholesaleProduct("Wholesale Price").ToString(), price)

        Dim existing = wholesaleCart.Find(Function(c) c.ProductID = Convert.ToInt32(currentWholesaleProduct("ID")))
        If existing IsNot Nothing Then
            existing.Quantity += qty
        Else
            wholesaleCart.Add(New CartItem With {
                .ProductID = Convert.ToInt32(currentWholesaleProduct("ID")),
                .BrandName = currentWholesaleProduct("Brand Name").ToString(),
                .GenericName = currentWholesaleProduct("Generic Name").ToString(),
                .Dosage = currentWholesaleProduct("Dosage").ToString(),
                .Unit = currentWholesaleProduct("Unit").ToString(),
                .Price = price,
                .Quantity = qty,
                .IsRx = False
            })
        End If

        RenderWholesaleCart()
        MessageBox.Show(wQtyTextBox.Text & " x " & currentWholesaleProduct("Brand Name").ToString() & " added to cart.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ===================== CART RENDERING =====================

    Private Sub RenderCart()
        cartItemsPanel.Controls.Clear()

        Dim otcItems = cart.Where(Function(c) Not c.IsRx).ToList()
        Dim rxItems = cart.Where(Function(c) c.IsRx).ToList()

        Dim y As Integer = 10
        If otcItems.Count > 0 Then
            y = RenderCartSection(cartItemsPanel, "OTC ITEMS", otcItems, y)
        End If
        If rxItems.Count > 0 Then
            y = RenderCartSection(cartItemsPanel, "Rx ITEMS", rxItems, y)
        End If
        If cart.Count = 0 Then
            Dim empty As New Label() With {
                .Text = "Cart is empty.",
                .Font = New Font("Segoe UI", 10.0F),
                .ForeColor = Color.Gray,
                .AutoSize = True,
                .Location = New Point(10, y)
            }
            cartItemsPanel.Controls.Add(empty)
        End If

        Dim subtotal As Decimal = cart.Sum(Function(c) c.LineTotal)
        cartSubtotalValue.Text = "P " & subtotal.ToString("N2")
    End Sub

    Private Sub RenderWholesaleCart()
        wholesaleCartPanel.Controls.Clear()

        Dim y As Integer = 10
        If wholesaleCart.Count > 0 Then
            y = RenderCartSection(wholesaleCartPanel, "WHOLESALE ITEMS", wholesaleCart, y)
        Else
            Dim empty As New Label() With {
                .Text = "Cart is empty.",
                .Font = New Font("Segoe UI", 10.0F),
                .ForeColor = Color.Gray,
                .AutoSize = True,
                .Location = New Point(10, y)
            }
            wholesaleCartPanel.Controls.Add(empty)
        End If

        Dim subtotal As Decimal = wholesaleCart.Sum(Function(c) c.LineTotal)
        wholesaleSubtotalValue.Text = "P " & subtotal.ToString("N2")
    End Sub

    Private Function RenderCartSection(parent As Panel, sectionTitle As String, items As List(Of CartItem), startY As Integer) As Integer
        Dim header As New Label() With {
            .Text = sectionTitle,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(20, 100, 60),
            .AutoSize = True,
            .Location = New Point(10, startY)
        }
        parent.Controls.Add(header)

        Dim y As Integer = startY + 30
        For Each item In items
            Dim line1 As New Label() With {
                .Text = item.BrandName & "  " & item.Dosage & "  " & item.Unit,
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(10, y)
            }
            parent.Controls.Add(line1)

            Dim line2 As New Label() With {
                .Text = item.GenericName,
                .Font = New Font("Segoe UI", 8.0F),
                .ForeColor = Color.Gray,
                .AutoSize = True,
                .Location = New Point(10, y + 20)
            }
            parent.Controls.Add(line2)

            Dim line3 As New Label() With {
                .Text = "P " & item.Price.ToString("N2") & "  x  " & item.Quantity.ToString(),
                .Font = New Font("Segoe UI", 10.0F),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(400, y + 10)
            }
            parent.Controls.Add(line3)

            Dim line4 As New Label() With {
                .Text = "P " & item.LineTotal.ToString("N2"),
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(620, y + 10)
            }
            parent.Controls.Add(line4)

            ' Remove button
            Dim removeBtn As New Button() With {
                .Text = "X",
                .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(200, 30, 30),
                .BackColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Size = New Size(26, 26),
                .Location = New Point(720, y + 5),
                .Tag = item.ProductID
            }
            removeBtn.FlatAppearance.BorderColor = Color.FromArgb(200, 30, 30)
            AddHandler removeBtn.Click, AddressOf RemoveCartItem_Click
            parent.Controls.Add(removeBtn)

            y += 60
        Next

        Return y + 10
    End Function

    Private Sub RemoveCartItem_Click(sender As Object, e As EventArgs)
        Dim btn = TryCast(sender, Button)
        If btn Is Nothing Then Return
        Dim pid As Integer = CInt(btn.Tag)

        ' Try both carts
        Dim rItem = cart.Find(Function(c) c.ProductID = pid)
        If rItem IsNot Nothing Then
            cart.Remove(rItem)
            RenderCart()
            Return
        End If

        Dim wItem = wholesaleCart.Find(Function(c) c.ProductID = pid)
        If wItem IsNot Nothing Then
            wholesaleCart.Remove(wItem)
            RenderWholesaleCart()
        End If
    End Sub

    ' ===================== CHECKOUT =====================

    Private Sub checkoutButton_Click(sender As Object, e As EventArgs) Handles checkoutButton.Click
        If cart.Count = 0 Then
            MessageBox.Show("Cart is empty. Add items before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        checkoutIsWholesale = False
        BuildCheckoutView(cart)
        ShowView("Checkout")
    End Sub

    Private Sub wholesaleCheckoutButton_Click(sender As Object, e As EventArgs) Handles wholesaleCheckoutButton.Click
        If wholesaleCart.Count = 0 Then
            MessageBox.Show("Cart is empty. Add items before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        checkoutIsWholesale = True
        BuildCheckoutView(wholesaleCart)
        ShowView("Checkout")
    End Sub

    Private Sub BuildCheckoutView(items As List(Of CartItem))
        ' Order summary
        orderSummaryPanel.Controls.Clear()
        Dim y As Integer = 10
        For Each item In items
            Dim nameLabel As New Label() With {
                .Text = item.BrandName & "  " & item.Dosage & "  " & item.Unit,
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(10, y)
            }
            orderSummaryPanel.Controls.Add(nameLabel)

            Dim genericLabel As New Label() With {
                .Text = item.GenericName,
                .Font = New Font("Segoe UI", 8.0F),
                .ForeColor = Color.Gray,
                .AutoSize = True,
                .Location = New Point(10, y + 20)
            }
            orderSummaryPanel.Controls.Add(genericLabel)

            Dim priceQty As New Label() With {
                .Text = "P " & item.Price.ToString("N2") & "  x  " & item.Quantity.ToString(),
                .Font = New Font("Segoe UI", 10.0F),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(400, y + 10)
            }
            orderSummaryPanel.Controls.Add(priceQty)

            Dim totalLabel As New Label() With {
                .Text = "P " & item.LineTotal.ToString("N2"),
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.Black,
                .AutoSize = True,
                .Location = New Point(620, y + 10)
            }
            orderSummaryPanel.Controls.Add(totalLabel)

            y += 60
        Next

        ' Prescription info
        Dim hasRx As Boolean = items.Any(Function(c) c.IsRx)
        prescriptionLabel.Visible = hasRx
        prescriptionValueLabel.Visible = hasRx
        rxRecordedLabel.Visible = hasRx
        If hasRx Then
            prescriptionValueLabel.Text = "RX- " & rxNoTextBox.Text.Trim()
        End If

        ' Payment summary
        Dim subtotal As Decimal = items.Sum(Function(c) c.LineTotal)
        Dim discount As Decimal = 0D
        If customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2 Then
            discount = subtotal * 0.2D
        End If
        Dim total As Decimal = subtotal - discount

        subtotalValue.Text = "P " & subtotal.ToString("N2")
        discountValue.Text = "P " & discount.ToString("N2")
        totalDueValue.Text = "P " & total.ToString("N2")
        cashValue.Text = "P 0.00"
        changeValue.Text = "P 0.00"
        cashTextBox.Text = ""
    End Sub

    ' ===================== PAYMENT =====================

    Private Sub customerTypeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles customerTypeCombo.SelectedIndexChanged
        ' Recalculate discount when customer type changes
        Dim items = If(checkoutIsWholesale, wholesaleCart, cart)
        If items.Count > 0 AndAlso checkoutView.Visible Then
            BuildCheckoutView(items)
        End If
    End Sub

    Private Sub cashTextBox_TextChanged(sender As Object, e As EventArgs) Handles cashTextBox.TextChanged
        Dim cash As Decimal = 0D
        Decimal.TryParse(cashTextBox.Text, cash)

        Dim items = If(checkoutIsWholesale, wholesaleCart, cart)
        Dim subtotal As Decimal = items.Sum(Function(c) c.LineTotal)
        Dim discount As Decimal = 0D
        If customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2 Then
            discount = subtotal * 0.2D
        End If
        Dim total As Decimal = subtotal - discount

        cashValue.Text = "P " & cash.ToString("N2")
        Dim change As Decimal = cash - total
        changeValue.Text = "P " & change.ToString("N2")
        If change < 0 Then
            changeValue.ForeColor = Color.FromArgb(200, 30, 30)
        Else
            changeValue.ForeColor = Color.Black
        End If
    End Sub

    Private Sub cancelButton_Click(sender As Object, e As EventArgs) Handles cancelButton.Click
        If checkoutIsWholesale Then
            ShowView("Wholesale")
        Else
            ShowView("Retail")
        End If
    End Sub

    Private Sub confirmButton_Click(sender As Object, e As EventArgs) Handles confirmButton.Click
        Dim items = If(checkoutIsWholesale, wholesaleCart, cart)
        If items.Count = 0 Then
            MessageBox.Show("No items to checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim cash As Decimal = 0D
        Decimal.TryParse(cashTextBox.Text, cash)

        Dim subtotal As Decimal = items.Sum(Function(c) c.LineTotal)
        Dim discount As Decimal = 0D
        If customerTypeCombo.SelectedIndex = 1 OrElse customerTypeCombo.SelectedIndex = 2 Then
            discount = subtotal * 0.2D
        End If
        Dim total As Decimal = subtotal - discount

        If cash < total Then
            MessageBox.Show("Insufficient cash received. Total due is P " & total.ToString("N2"), "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim change As Decimal = cash - total
        Dim receipt As String =
            "Transaction Complete!" & vbCrLf & vbCrLf &
            "Subtotal: P " & subtotal.ToString("N2") & vbCrLf &
            "Discount: P " & discount.ToString("N2") & vbCrLf &
            "Total Due: P " & total.ToString("N2") & vbCrLf &
            "Cash: P " & cash.ToString("N2") & vbCrLf &
            "Change: P " & change.ToString("N2")

        MessageBox.Show(receipt, "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ' Clear cart and return
        If checkoutIsWholesale Then
            wholesaleCart.Clear()
            RenderWholesaleCart()
            ShowView("Wholesale")
        Else
            cart.Clear()
            RenderCart()
            ShowView("Retail")
        End If
    End Sub

End Class