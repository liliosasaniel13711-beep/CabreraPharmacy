Imports System.Globalization
Imports System.IO

Public NotInheritable Class ExpiryMonitorSettings
    Private Const DefaultNearExpiryDays As Integer = 90
    Private Const MinimumNearExpiryDays As Integer = 1
    Private Const MaximumNearExpiryDays As Integer = 3650

    Private Sub New()
    End Sub

    Public Shared Function LoadNearExpiryDays() As Integer
        Try
            Dim settingsPath = GetSettingsPath()
            If Not File.Exists(settingsPath) Then Return DefaultNearExpiryDays

            Dim days As Integer
            If Integer.TryParse(File.ReadAllText(settingsPath), NumberStyles.Integer, CultureInfo.InvariantCulture, days) AndAlso
               days >= MinimumNearExpiryDays AndAlso days <= MaximumNearExpiryDays Then
                Return days
            End If
        Catch
        End Try

        Return DefaultNearExpiryDays
    End Function

    Public Shared Sub SaveNearExpiryDays(days As Integer)
        If days < MinimumNearExpiryDays OrElse days > MaximumNearExpiryDays Then
            Throw New ArgumentOutOfRangeException(NameOf(days), "The near-expiry threshold must be between 1 and 3650 days.")
        End If

        Dim settingsPath = GetSettingsPath()
        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath))
        File.WriteAllText(settingsPath, days.ToString(CultureInfo.InvariantCulture))
    End Sub

    Private Shared Function GetSettingsPath() As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "CabreraPharmacy",
                            "near-expiry-days.txt")
    End Function
End Class
