Imports System.Windows.Forms

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
        If grid.ColumnHeadersDefaultCellStyle IsNot Nothing Then
            Dim headerStyle = grid.ColumnHeadersDefaultCellStyle
            headerStyle.SelectionBackColor = If(headerStyle.BackColor.IsEmpty, SystemColors.Control, headerStyle.BackColor)
            headerStyle.SelectionForeColor = If(headerStyle.ForeColor.IsEmpty, SystemColors.ControlText, headerStyle.ForeColor)
        End If

        If grid.RowHeadersDefaultCellStyle IsNot Nothing Then
            Dim rowHeaderStyle = grid.RowHeadersDefaultCellStyle
            rowHeaderStyle.SelectionBackColor = If(rowHeaderStyle.BackColor.IsEmpty, grid.BackgroundColor, rowHeaderStyle.BackColor)
            rowHeaderStyle.SelectionForeColor = If(rowHeaderStyle.ForeColor.IsEmpty, grid.ForeColor, rowHeaderStyle.ForeColor)
        End If
    End Sub
End Module
