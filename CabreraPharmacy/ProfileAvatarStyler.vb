Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Module ProfileAvatarStyler
    Private ReadOnly AvatarBackColor As Color = Color.FromArgb(224, 247, 236)
    Private ReadOnly AvatarIconColor As Color = Color.FromArgb(20, 100, 60)
    Private ReadOnly AvatarBorderColor As Color = Color.FromArgb(176, 222, 198)

    Public Sub Apply(avatar As Control)
        If avatar Is Nothing Then Return

        avatar.BackColor = AvatarBackColor
        UpdateRegion(avatar)
        RemoveHandler avatar.SizeChanged, AddressOf Avatar_SizeChanged
        RemoveHandler avatar.Paint, AddressOf Avatar_Paint
        AddHandler avatar.SizeChanged, AddressOf Avatar_SizeChanged
        AddHandler avatar.Paint, AddressOf Avatar_Paint
        avatar.Invalidate()
    End Sub

    Private Sub Avatar_SizeChanged(sender As Object, e As EventArgs)
        Dim avatar = TryCast(sender, Control)
        If avatar IsNot Nothing Then UpdateRegion(avatar)
    End Sub

    Private Sub UpdateRegion(avatar As Control)
        If avatar.Width <= 0 OrElse avatar.Height <= 0 Then Return

        Dim previousRegion = avatar.Region
        Using path As New GraphicsPath()
            path.AddEllipse(0, 0, avatar.Width, avatar.Height)
            avatar.Region = New Region(path)
        End Using
        If previousRegion IsNot Nothing Then previousRegion.Dispose()
    End Sub

    Private Sub Avatar_Paint(sender As Object, e As PaintEventArgs)
        Dim avatar = TryCast(sender, Control)
        If avatar Is Nothing OrElse avatar.ClientSize.Width <= 0 OrElse avatar.ClientSize.Height <= 0 Then Return

        Dim graphics = e.Graphics
        graphics.SmoothingMode = SmoothingMode.AntiAlias
        Dim scale = Math.Min(avatar.ClientSize.Width, avatar.ClientSize.Height) / 46.0F
        Dim centerX = avatar.ClientSize.Width / 2.0F
        Dim headSize = 12.0F * scale

        Using iconBrush As New SolidBrush(AvatarIconColor)
            graphics.FillEllipse(iconBrush,
                                 centerX - headSize / 2.0F,
                                 7.0F * scale,
                                 headSize,
                                 headSize)

            Using shoulders As New GraphicsPath()
                Dim bodyLeft = centerX - 15.0F * scale
                Dim bodyTop = 21.0F * scale
                Dim bodyWidth = 30.0F * scale
                Dim bodyHeight = 28.0F * scale
                Dim bottom = avatar.ClientSize.Height - 5.0F * scale
                shoulders.AddArc(bodyLeft, bodyTop, bodyWidth, bodyHeight, 180.0F, 180.0F)
                shoulders.AddLine(bodyLeft + bodyWidth, bodyTop + bodyHeight / 2.0F,
                                  bodyLeft + bodyWidth, bottom)
                shoulders.AddLine(bodyLeft + bodyWidth, bottom, bodyLeft, bottom)
                shoulders.AddLine(bodyLeft, bottom, bodyLeft, bodyTop + bodyHeight / 2.0F)
                shoulders.CloseFigure()
                graphics.FillPath(iconBrush, shoulders)
            End Using
        End Using

        Using outline As New Pen(AvatarBorderColor, Math.Max(1.0F, scale))
            graphics.DrawEllipse(outline,
                                 scale / 2.0F,
                                 scale / 2.0F,
                                 avatar.ClientSize.Width - scale,
                                 avatar.ClientSize.Height - scale)
        End Using
    End Sub
End Module
