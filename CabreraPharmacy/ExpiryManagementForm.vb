Imports System.Data
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class ExpiryManagementForm
    Inherits Form

    Private ReadOnly thresholdInput As New NumericUpDown()
    Private ReadOnly batchGrid As New DataGridView()
    Private ReadOnly quarantineGrid As New DataGridView()
    Private ReadOnly actionGrid As New DataGridView()
    Private ReadOnly statusLabel As New Label()
    Private ReadOnly migrationStatusLabel As New Label()
    Private ReadOnly quarantineButton As New Button()
    Private ReadOnly returnButton As New Button()
    Private ReadOnly disposalButton As New Button()
    Private ReadOnly releaseQuarantineButton As New Button()
    Private ReadOnly confirmActionButton As New Button()
    Private ReadOnly cancelActionButton As New Button()
    Private migrationApplied As Boolean

    Public Sub New()
        Text = "Near-Expiry and Expired Batches"
        StartPosition = FormStartPosition.CenterParent
        Size = New Size(1180, 680)
        MinimumSize = New Size(900, 520)
        BackColor = Color.White
        Font = New Font("Segoe UI", 9.0F)

        Dim toolbar As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 48,
            .Padding = New Padding(12, 8, 12, 4),
            .WrapContents = False,
            .BackColor = Color.White
        }
        toolbar.Controls.Add(New Label With {
            .Text = "Near-expiry threshold (days):",
            .AutoSize = True,
            .Margin = New Padding(0, 7, 8, 0),
            .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        })

        thresholdInput.Minimum = 1
        thresholdInput.Maximum = 3650
        If LicenseManager.UsageMode <> LicenseUsageMode.Designtime Then
            thresholdInput.Value = ExpiryMonitorSettings.LoadNearExpiryDays()
        End If
        thresholdInput.Width = 82
        thresholdInput.Margin = New Padding(0, 2, 8, 0)
        toolbar.Controls.Add(thresholdInput)

        Dim saveButton As New Button With {
            .Text = "Save Threshold",
            .AutoSize = True,
            .Height = 30,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(20, 100, 60),
            .ForeColor = Color.White,
            .UseVisualStyleBackColor = False,
            .Margin = New Padding(0, 0, 8, 0)
        }
        AddHandler saveButton.Click, AddressOf SaveThreshold_Click
        toolbar.Controls.Add(saveButton)

        Dim refreshButton As New Button With {
            .Text = "Refresh",
            .AutoSize = True,
            .Height = 30,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(20, 100, 60),
            .Margin = New Padding(0, 0, 8, 0)
        }
        AddHandler refreshButton.Click, Sub(sender, e) LoadBatches()
        toolbar.Controls.Add(refreshButton)

        statusLabel.AutoSize = True
        statusLabel.Margin = New Padding(12, 7, 0, 0)
        toolbar.Controls.Add(statusLabel)
        migrationStatusLabel.AutoSize = True
        migrationStatusLabel.Margin = New Padding(14, 7, 0, 0)
        migrationStatusLabel.ForeColor = Color.FromArgb(95, 75, 0)
        toolbar.Controls.Add(migrationStatusLabel)

        ConfigureGrid(batchGrid)
        ConfigureGrid(quarantineGrid)
        ConfigureGrid(actionGrid)

        Dim monitorPage As New TabPage("Expiry Batches")
        Dim monitorToolbar As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 42,
            .Padding = New Padding(8, 5, 8, 4),
            .WrapContents = False
        }
        ConfigureActionButton(quarantineButton, "Quarantine Selected")
        ConfigureActionButton(returnButton, "Request Supplier Return")
        ConfigureActionButton(disposalButton, "Request Disposal")
        AddHandler quarantineButton.Click, AddressOf QuarantineSelectedBatch_Click
        AddHandler returnButton.Click, AddressOf RequestSupplierReturn_Click
        AddHandler disposalButton.Click, AddressOf RequestDisposal_Click
        monitorToolbar.Controls.AddRange({quarantineButton, returnButton, disposalButton})
        monitorPage.Controls.Add(batchGrid)
        monitorPage.Controls.Add(monitorToolbar)

        Dim quarantinePage As New TabPage("Quarantine History")
        Dim quarantineToolbar As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 42,
            .Padding = New Padding(8, 5, 8, 4),
            .WrapContents = False
        }
        ConfigureActionButton(releaseQuarantineButton, "Release Selected")
        AddHandler releaseQuarantineButton.Click, AddressOf ReleaseSelectedQuarantine_Click
        quarantineToolbar.Controls.Add(releaseQuarantineButton)
        quarantinePage.Controls.Add(quarantineGrid)
        quarantinePage.Controls.Add(quarantineToolbar)

        Dim actionsPage As New TabPage("Return and Disposal Requests")
        Dim actionsToolbar As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .Height = 42,
            .Padding = New Padding(8, 5, 8, 4),
            .WrapContents = False
        }
        ConfigureActionButton(confirmActionButton, "Confirm Selected")
        ConfigureActionButton(cancelActionButton, "Cancel Selected")
        AddHandler confirmActionButton.Click, AddressOf ConfirmSelectedAction_Click
        AddHandler cancelActionButton.Click, AddressOf CancelSelectedAction_Click
        actionsToolbar.Controls.AddRange({confirmActionButton, cancelActionButton})
        actionsPage.Controls.Add(actionGrid)
        actionsPage.Controls.Add(actionsToolbar)

        Dim pages As New TabControl With {.Dock = DockStyle.Fill}
        pages.TabPages.Add(monitorPage)
        pages.TabPages.Add(quarantinePage)
        pages.TabPages.Add(actionsPage)

        Dim note As New Label With {
            .Text = "Expired batches remain in physical inventory. Saleable quantity excludes expired, inactive, quarantined, and pending-action stock. Pending actions change stock only after explicit confirmation.",
            .Dock = DockStyle.Bottom,
            .Height = 42,
            .Padding = New Padding(12, 8, 12, 4),
            .ForeColor = Color.FromArgb(75, 75, 75),
            .BackColor = Color.FromArgb(248, 250, 248)
        }

        Controls.Add(pages)
        Controls.Add(note)
        Controls.Add(toolbar)
        If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
            statusLabel.Text = "Batch data loads when the form runs"
            migrationStatusLabel.Text = "Runtime database status"
        Else
            RefreshMigrationState()
            UiSelectionStyler.ApplyToForm(Me)
            LoadBatches()
        End If
    End Sub

    Private Sub SaveThreshold_Click(sender As Object, e As EventArgs)
        Try
            ExpiryMonitorSettings.SaveNearExpiryDays(CInt(thresholdInput.Value))
            LoadBatches()
        Catch ex As Exception
            MessageBox.Show("Could not save the expiry threshold. " & ex.Message,
                            "Expiry monitor settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadBatches()
        Try
            Dim batches = New ExpiryBatchRepository().GetExpiryBatches(CInt(thresholdInput.Value), nearExpiryOnly:=True)
            batchGrid.DataSource = batches
            If batchGrid.Columns.Contains("Product ID") Then batchGrid.Columns("Product ID").Visible = False
            If batchGrid.Columns.Contains("Batch ID") Then batchGrid.Columns("Batch ID").Visible = False
            If batchGrid.Columns.Contains("Expiry Date") Then batchGrid.Columns("Expiry Date").DefaultCellStyle.Format = "MMM dd, yyyy"
            statusLabel.Text = batches.Rows.Count.ToString() & " batches"
        Catch ex As Exception
            batchGrid.DataSource = Nothing
            statusLabel.Text = "Load failed"
            MessageBox.Show("Could not load expiry batches. " & ex.Message,
                            "Expiry monitor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Shared Sub ConfigureGrid(grid As DataGridView)
        grid.Dock = DockStyle.Fill
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.RowHeadersVisible = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.BackgroundColor = Color.White
        grid.BorderStyle = BorderStyle.None
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 239)
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 240, 230)
        grid.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Shared Sub ConfigureActionButton(button As Button, caption As String)
        button.Text = caption
        button.AutoSize = True
        button.Height = 30
        button.FlatStyle = FlatStyle.Flat
        button.BackColor = Color.White
        button.ForeColor = Color.FromArgb(20, 100, 60)
        button.Margin = New Padding(0, 0, 8, 0)
    End Sub

    Private Sub RefreshMigrationState()
        Try
            migrationApplied = BatchControlRepository.IsMigrationApplied()
        Catch
            migrationApplied = False
        End Try

        Dim authorized = BatchControlRepository.IsAuthorizedUser()
        For Each button In {quarantineButton, returnButton, disposalButton, releaseQuarantineButton, confirmActionButton, cancelActionButton}
            button.Enabled = migrationApplied AndAlso authorized
        Next
        migrationStatusLabel.Text = If(migrationApplied,
                                       "V001 active; batch controls enabled",
                                       "V001 not applied; batch controls are read-only")
        If migrationApplied AndAlso authorized Then
            LoadQuarantines()
            LoadBatchActions()
        End If
    End Sub

    Private Sub LoadQuarantines()
        quarantineGrid.DataSource = New BatchControlRepository().GetQuarantineRecords()
        If quarantineGrid.Columns.Contains("Quarantine ID") Then quarantineGrid.Columns("Quarantine ID").Visible = False
        If quarantineGrid.Columns.Contains("Expiry Date") Then quarantineGrid.Columns("Expiry Date").DefaultCellStyle.Format = "MMM dd, yyyy"
        If quarantineGrid.Columns.Contains("Quarantined At") Then quarantineGrid.Columns("Quarantined At").DefaultCellStyle.Format = "g"
        If quarantineGrid.Columns.Contains("Released At") Then quarantineGrid.Columns("Released At").DefaultCellStyle.Format = "g"
    End Sub

    Private Sub LoadBatchActions()
        actionGrid.DataSource = New BatchControlRepository().GetBatchActions()
        If actionGrid.Columns.Contains("Action ID") Then actionGrid.Columns("Action ID").Visible = False
        If actionGrid.Columns.Contains("Expiry Date") Then actionGrid.Columns("Expiry Date").DefaultCellStyle.Format = "MMM dd, yyyy"
        For Each columnName In {"Requested At", "Confirmed At", "Cancelled At"}
            If actionGrid.Columns.Contains(columnName) Then actionGrid.Columns(columnName).DefaultCellStyle.Format = "g"
        Next
    End Sub

    Private Function SelectedBatchId() As Integer?
        If batchGrid.CurrentRow Is Nothing Then Return Nothing
        Dim view = TryCast(batchGrid.CurrentRow.DataBoundItem, DataRowView)
        If view Is Nothing OrElse view.Row.IsNull("Batch ID") Then Return Nothing
        Return Convert.ToInt32(view.Row("Batch ID"))
    End Function

    Private Function SelectedRowId(grid As DataGridView, keyColumn As String) As Long?
        If grid.CurrentRow Is Nothing Then Return Nothing
        Dim view = TryCast(grid.CurrentRow.DataBoundItem, DataRowView)
        If view Is Nothing OrElse view.Row.IsNull(keyColumn) Then Return Nothing
        Return Convert.ToInt64(view.Row(keyColumn))
    End Function

    Private Function PromptQuantityAndReason(actionName As String, ByRef quantity As Integer, ByRef reason As String) As Boolean
        Dim quantityText = Microsoft.VisualBasic.Interaction.InputBox("Enter the quantity for " & actionName & ".", actionName)
        If Not Integer.TryParse(quantityText, quantity) OrElse quantity < 1 Then Return False
        reason = Microsoft.VisualBasic.Interaction.InputBox("Enter the reason for " & actionName & ".", actionName).Trim()
        Return Not String.IsNullOrWhiteSpace(reason)
    End Function

    Private Sub QuarantineSelectedBatch_Click(sender As Object, e As EventArgs)
        Dim batchId = SelectedBatchId()
        If Not batchId.HasValue Then Return
        Dim quantity As Integer
        Dim reason As String = Nothing
        If Not PromptQuantityAndReason("quarantine", quantity, reason) Then Return
        Try
            Dim repository As New BatchControlRepository()
            repository.CreateQuarantine(batchId.Value, quantity, reason)
            LoadQuarantines()
        Catch ex As Exception
            ShowWorkflowError(ex)
        End Try
    End Sub

    Private Sub RequestSupplierReturn_Click(sender As Object, e As EventArgs)
        RequestBatchAction("Supplier Return")
    End Sub

    Private Sub RequestDisposal_Click(sender As Object, e As EventArgs)
        RequestBatchAction("Disposal")
    End Sub

    Private Sub RequestBatchAction(actionType As String)
        Dim batchId = SelectedBatchId()
        If Not batchId.HasValue Then Return
        Dim quantity As Integer
        Dim reason As String = Nothing
        If Not PromptQuantityAndReason(actionType.ToLowerInvariant(), quantity, reason) Then Return
        Try
            Dim repository As New BatchControlRepository()
            repository.RequestBatchAction(batchId.Value, actionType, quantity, reason)
            LoadBatchActions()
        Catch ex As Exception
            ShowWorkflowError(ex)
        End Try
    End Sub

    Private Sub ConfirmSelectedAction_Click(sender As Object, e As EventArgs)
        Dim actionId = SelectedRowId(actionGrid, "Action ID")
        If Not actionId.HasValue Then Return
        Dim view = TryCast(actionGrid.CurrentRow.DataBoundItem, DataRowView)
        If view Is Nothing OrElse Not String.Equals(Convert.ToString(view.Row("Status")), "Pending", StringComparison.OrdinalIgnoreCase) Then Return
        Dim actionType = Convert.ToString(view.Row("Action"))
        Dim quantity = Convert.ToInt32(view.Row("Quantity"))
        If MessageBox.Show("Confirm this " & actionType & " for " & quantity.ToString() & " units? The confirmed quantity will be deducted from the batch and recorded in its history.",
                           "Confirm batch action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Dim note = Microsoft.VisualBasic.Interaction.InputBox("Optional confirmation note.", "Confirm batch action")
        Try
            Dim repository As New BatchControlRepository()
            repository.ConfirmBatchAction(actionId.Value, note)
            LoadBatchActions()
            LoadBatches()
        Catch ex As Exception
            ShowWorkflowError(ex)
        End Try
    End Sub

    Private Sub CancelSelectedAction_Click(sender As Object, e As EventArgs)
        Dim actionId = SelectedRowId(actionGrid, "Action ID")
        If Not actionId.HasValue Then Return
        Dim reason = Microsoft.VisualBasic.Interaction.InputBox("Enter the reason for cancelling this pending action.", "Cancel batch action").Trim()
        If String.IsNullOrWhiteSpace(reason) Then Return
        Try
            Dim repository As New BatchControlRepository()
            repository.CancelBatchAction(actionId.Value, reason)
            LoadBatchActions()
        Catch ex As Exception
            ShowWorkflowError(ex)
        End Try
    End Sub

    Private Sub ReleaseSelectedQuarantine_Click(sender As Object, e As EventArgs)
        Dim quarantineId = SelectedRowId(quarantineGrid, "Quarantine ID")
        If Not quarantineId.HasValue Then Return
        Dim view = TryCast(quarantineGrid.CurrentRow.DataBoundItem, DataRowView)
        If view Is Nothing OrElse Not String.Equals(Convert.ToString(view.Row("Status")), "Active", StringComparison.OrdinalIgnoreCase) Then Return
        Dim reason = Microsoft.VisualBasic.Interaction.InputBox("Enter the reason for releasing this quarantine.", "Release quarantine").Trim()
        If String.IsNullOrWhiteSpace(reason) Then Return
        Try
            Dim repository As New BatchControlRepository()
            repository.ReleaseQuarantine(quarantineId.Value, reason)
            LoadQuarantines()
        Catch ex As Exception
            ShowWorkflowError(ex)
        End Try
    End Sub

    Private Sub ShowWorkflowError(ex As Exception)
        MessageBox.Show("The batch action was not completed. " & ex.Message,
                        "Batch controls", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class
