Module SessionModule

    Public CurrentUserID As Integer
    Public CurrentUsername As String
    Public CurrentFullName As String
    Public CurrentStaffInitials As String
    Public CurrentRole As String

    Public Function ReceiptStaffInitials(staffInitials As String) As String
        If Not String.IsNullOrWhiteSpace(staffInitials) Then Return staffInitials.Trim().ToUpperInvariant()
        Return "STAFF"
    End Function

End Module
