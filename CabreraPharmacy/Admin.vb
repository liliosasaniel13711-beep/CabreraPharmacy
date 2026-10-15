Imports System.Drawing
Imports System.Windows.Forms

Public Class Admin

    ' ColorDef
    Private darkGreen As Color = Color.FromArgb(20, 100, 60)
    Private activeGreen As Color = Color.FromArgb(25, 120, 75)
    Private lightGray As Color = Color.FromArgb(245, 245, 245)
    Private yellow As Color = Color.FromArgb(255, 215, 0)

    ' Navigation state
    Private navButtons As Button()
    Private contentPanels As Dictionary(Of String, Panel)

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
                Return "Overview of Cabrera’s Drugstore and Medical Supplies"
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

    Private Sub userRoleLabel_Click(sender As Object, e As EventArgs) Handles userRoleLabel.Click
        ' Optional click logic for user role label
    End Sub

    ' =================================================================
    ' BACKEND DATA RETRIEVAL METHODS
    ' =================================================================

    Private Sub LoadDashboardStats()
        ' Future implementation: Add SQL COUNT() logic here to update the dashboard summary cards
        ' Example: lblTotalProducts.Text = GetProductCount()
    End Sub

    Private Sub LoadInventoryData()
        Try
            Dim prodManager As New ProductManager()
            dgvInventory.DataSource = prodManager.GetProducts()

            ' --- NEW LAYOUT FIXES ---
            ' 1. Stretch columns to fill all the gray space
            dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            ' 2. Remove the blank asterisk (*) row at the bottom
            dgvInventory.AllowUserToAddRows = False

            ' 3. Remove the empty margin on the far left
            dgvInventory.RowHeadersVisible = False

            ' 4. Make clicking select the whole row instead of one cell
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            ' ------------------------

            If dgvInventory.Columns.Contains("Retail Price") Then
                dgvInventory.Columns("Retail Price").DefaultCellStyle.Format = "C2"
            End If
            If dgvInventory.Columns.Contains("Wholesale Price") Then
                dgvInventory.Columns("Wholesale Price").DefaultCellStyle.Format = "C2"
            End If

        Catch ex As Exception
            MessageBox.Show("Failed to load inventory: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPriceManagerData()
        ' Future implementation: Load data specific to the Price Manager panel
    End Sub

    Private Sub bodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles bodyPanel.Paint

    End Sub
End Class
