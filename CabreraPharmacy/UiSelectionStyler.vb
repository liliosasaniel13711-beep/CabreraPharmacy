Imports System.Windows.Forms
Imports System.Drawing

Public Module UiSelectionStyler
    Public Sub ApplyToForm(root As Control)
        If root Is Nothing Then Return
        ApplyToControlTree(root)
    End Sub

    Private Sub ApplyToControlTree(parent As Control)
        If parent Is Nothing Then Return

        Dim grid = TryCast(parent, DataGridView)
        If grid IsNot Nothing Then ApplyToGrid(grid)

        AddHandler parent.ControlAdded, AddressOf Parent_ControlAdded
        For Each child As Control In parent.Controls
            ApplyToControlTree(child)
        Next
    End Sub

    Private Sub Parent_ControlAdded(sender As Object, e As ControlEventArgs)
        ApplyToControlTree(e.Control)
    End Sub

    Private Sub ApplyToGrid(grid As DataGridView)
        If grid Is Nothing Then Return

        Dim headerGreen = Color.FromArgb(20, 125, 77)
        Dim selectedGreen = Color.FromArgb(229, 244, 236)
        Dim textGreen = Color.FromArgb(24, 54, 40)

        grid.BackgroundColor = Color.White
        grid.BorderStyle = BorderStyle.None
        grid.GridColor = Color.FromArgb(223, 230, 237)
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = headerGreen
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerGreen
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        grid.ColumnHeadersDefaultCellStyle.Padding = New Padding(12, 0, 8, 0)
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        grid.ColumnHeadersHeight = 40
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single

        grid.DefaultCellStyle.BackColor = Color.White
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(30, 36, 42)
        grid.DefaultCellStyle.SelectionBackColor = selectedGreen
        grid.DefaultCellStyle.SelectionForeColor = textGreen
        grid.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        grid.DefaultCellStyle.Padding = New Padding(12, 0, 8, 0)
        grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        grid.DefaultCellStyle.WrapMode = DataGridViewTriState.False
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)
        grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(30, 36, 42)
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectedGreen
        grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = textGreen
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        grid.RowHeadersVisible = False
        grid.RowTemplate.Height = 42
        grid.RowTemplate.MinimumHeight = 42
        For Each row As DataGridViewRow In grid.Rows
            If row.IsNewRow Then Continue For
            row.Height = 42
            row.MinimumHeight = 42
        Next

        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerGreen
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White
        If grid.RowHeadersDefaultCellStyle IsNot Nothing Then
            grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White
            grid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 36, 42)
        End If

        If grid.AccessibleDescription <> "ConsistentGreenTable" Then
            grid.AccessibleDescription = "ConsistentGreenTable"
            AddHandler grid.DataBindingComplete,
                Sub(sender, e)
                    Dim currentGrid = DirectCast(sender, DataGridView)
                    For Each row As DataGridViewRow In currentGrid.Rows
                        If row.IsNewRow Then Continue For
                        row.Height = 42
                        row.MinimumHeight = 42
                    Next
                End Sub
        End If
    End Sub
End Module
