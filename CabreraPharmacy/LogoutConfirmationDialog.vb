Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public NotInheritable Class LogoutConfirmationDialog
    Private Sub New()
    End Sub

    Public Shared Function Confirm(owner As IWin32Window) As Boolean
        Dim ownerForm = TryCast(owner, Form)
        Using backdrop As New Form()
            backdrop.FormBorderStyle = FormBorderStyle.None
            backdrop.ShowInTaskbar = False
            backdrop.StartPosition = FormStartPosition.Manual
            backdrop.BackColor = Color.FromArgb(28, 34, 42)
            backdrop.Opacity = 0.38R
            If ownerForm IsNot Nothing Then
                backdrop.Bounds = ownerForm.Bounds
                backdrop.TopMost = ownerForm.TopMost
                backdrop.Show(ownerForm)
            Else
                backdrop.Bounds = Screen.PrimaryScreen.WorkingArea
                backdrop.Show()
            End If

            Using prompt As New LogoutPromptForm()
                Dim result = prompt.ShowDialog(backdrop)
                backdrop.Close()
                Return result = DialogResult.Yes
            End Using
        End Using
    End Function

    Private NotInheritable Class LogoutPromptForm
        Inherits Form

        Private ReadOnly cancelActionButton As Button
        Private ReadOnly logoutButton As Button

        Public Sub New()
            Text = "Confirm Logout"
            FormBorderStyle = FormBorderStyle.None
            StartPosition = FormStartPosition.CenterParent
            ShowInTaskbar = False
            MaximizeBox = False
            MinimizeBox = False
            KeyPreview = True
            BackColor = Color.White
            ClientSize = New Size(460, 250)

            Dim header As New Panel With {
                .BackColor = Color.FromArgb(248, 250, 252),
                .Location = New Point(0, 0),
                .Size = New Size(460, 52)
            }
            AddHandler header.Paint,
                Sub(sender, e)
                    Using linePen As New Pen(Color.FromArgb(222, 227, 233))
                        e.Graphics.DrawLine(linePen, 0, header.Height - 1, header.Width, header.Height - 1)
                    End Using
                End Sub

            Dim headerIcon As New Label With {
                .Text = "↪",
                .Font = New Font("Segoe UI Symbol", 17.0F, FontStyle.Regular),
                .ForeColor = Color.FromArgb(100, 111, 125),
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(14, 6),
                .Size = New Size(28, 38)
            }
            Dim headerTitle As New Label With {
                .Text = "Confirm Logout",
                .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(35, 43, 54),
                .TextAlign = ContentAlignment.MiddleLeft,
                .Location = New Point(52, 7),
                .Size = New Size(350, 38)
            }
            header.Controls.AddRange({headerIcon, headerTitle})

            Dim icon As New Panel With {
                .BackColor = Color.FromArgb(255, 246, 232),
                .Location = New Point(28, 86),
                .Size = New Size(62, 62)
            }
            AddHandler icon.Paint, AddressOf LogoutIcon_Paint

            Dim bodyTitle As New Label With {
                .Text = "Confirm Logout",
                .Font = New Font("Segoe UI", 16.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(35, 43, 54),
                .TextAlign = ContentAlignment.MiddleLeft,
                .Location = New Point(106, 80),
                .Size = New Size(330, 32)
            }
            Dim bodyText As New Label With {
                .Text = "You will be securely signed out of Cabrera's" & Environment.NewLine & "Drugstore and Medical Supplies.",
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular),
                .ForeColor = Color.FromArgb(104, 113, 125),
                .TextAlign = ContentAlignment.TopLeft,
                .Location = New Point(106, 116),
                .Size = New Size(328, 46)
            }

            Dim footer As New Panel With {
                .BackColor = Color.FromArgb(248, 250, 252),
                .Location = New Point(0, 182),
                .Size = New Size(460, 68)
            }
            AddHandler footer.Paint,
                Sub(sender, e)
                    Using linePen As New Pen(Color.FromArgb(222, 227, 233))
                        e.Graphics.DrawLine(linePen, 0, 0, footer.Width, 0)
                    End Using
                End Sub

            cancelActionButton = New Button With {
                .Text = "Cancel",
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular),
                .ForeColor = Color.FromArgb(65, 75, 88),
                .BackColor = Color.FromArgb(238, 241, 244),
                .FlatStyle = FlatStyle.Flat,
                .Location = New Point(204, 14),
                .Size = New Size(120, 40),
                .UseVisualStyleBackColor = False,
                .Cursor = Cursors.Hand
            }
            cancelActionButton.FlatAppearance.BorderSize = 0
            cancelActionButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 231, 236)
            AddHandler cancelActionButton.Click, Sub(sender, e) CloseWithResult(DialogResult.Cancel)

            logoutButton = New Button With {
                .Text = "↪  Log Out",
                .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
                .ForeColor = Color.White,
                .BackColor = Color.FromArgb(220, 46, 48),
                .FlatStyle = FlatStyle.Flat,
                .Location = New Point(336, 15),
                .Size = New Size(108, 38),
                .UseVisualStyleBackColor = False,
                .Cursor = Cursors.Hand
            }
            logoutButton.FlatAppearance.BorderSize = 0
            logoutButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(194, 35, 39)
            AddHandler logoutButton.Click, Sub(sender, e) CloseWithResult(DialogResult.Yes)

            footer.Controls.AddRange({cancelActionButton, logoutButton})
            Controls.AddRange({header, icon, bodyTitle, bodyText, footer})
            CancelButton = cancelActionButton

            AddHandler Shown,
                Sub(sender, e)
                    ApplyRoundedRegion(Me, 16)
                    ApplyRoundedRegion(cancelActionButton, 12)
                    ApplyRoundedRegion(logoutButton, 12)
                End Sub
            AddHandler Resize, Sub(sender, e) ApplyRoundedRegion(Me, 16)
            AddHandler icon.Resize, Sub(sender, e) ApplyRoundedRegion(icon, 46)
            AddHandler cancelActionButton.Resize, Sub(sender, e) ApplyRoundedRegion(cancelActionButton, 12)
            AddHandler logoutButton.Resize, Sub(sender, e) ApplyRoundedRegion(logoutButton, 12)
        End Sub

        Private Sub CloseWithResult(result As DialogResult)
            DialogResult = result
            Close()
        End Sub

        Private Sub LogoutIcon_Paint(sender As Object, e As PaintEventArgs)
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            Dim iconPanel = DirectCast(sender, Panel)
            e.Graphics.ScaleTransform(iconPanel.Width / 92.0F, iconPanel.Height / 92.0F)
            Dim circleBounds As New Rectangle(8, 8, 76, 76)
            Using fill As New SolidBrush(Color.FromArgb(255, 239, 215))
                e.Graphics.FillEllipse(fill, circleBounds)
            End Using
            Using border As New Pen(Color.FromArgb(244, 215, 180), 1.5F)
                e.Graphics.DrawEllipse(border, circleBounds)
            End Using
            Using iconPen As New Pen(Color.FromArgb(190, 112, 48), 2.5F)
                iconPen.StartCap = LineCap.Round
                iconPen.EndCap = LineCap.Round
                e.Graphics.DrawLine(iconPen, 38, 29, 38, 61)
                e.Graphics.DrawLine(iconPen, 38, 29, 51, 29)
                e.Graphics.DrawLine(iconPen, 38, 61, 51, 61)
                e.Graphics.DrawLine(iconPen, 48, 45, 66, 45)
                e.Graphics.DrawLine(iconPen, 58, 37, 66, 45)
                e.Graphics.DrawLine(iconPen, 58, 53, 66, 45)
            End Using
        End Sub

        Private Shared Sub ApplyRoundedRegion(control As Control, radius As Integer)
            If control.Width <= 0 OrElse control.Height <= 0 Then Return
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
    End Class
End Class
