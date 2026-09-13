Imports System.Security.Cryptography

Module SecurityModule

    Private Const SaltSize As Integer = 16
    Private Const HashSize As Integer = 32
    Private Const Iterations As Integer = 100000

    Public Function HashPassword(password As String) As String

        'Generate a random salt
        Dim salt(SaltSize - 1) As Byte

        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
            rng.GetBytes(salt)
        End Using

        'Create PBKDF2 hash
        Using pbkdf2 As New Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256)

            Dim hash As Byte() = pbkdf2.GetBytes(HashSize)

            'Store iterations + salt + hash together
            Return Iterations.ToString() & "." &
                   Convert.ToBase64String(salt) & "." &
                   Convert.ToBase64String(hash)

        End Using

    End Function


    Public Function VerifyPassword(
        password As String,
        storedHash As String) As Boolean

        Try

            Dim parts() As String = storedHash.Split("."c)

            If parts.Length <> 3 Then
                Return False
            End If

            Dim iterations As Integer =
                Convert.ToInt32(parts(0))

            Dim salt As Byte() =
                Convert.FromBase64String(parts(1))

            Dim storedPasswordHash As Byte() =
                Convert.FromBase64String(parts(2))

            Using pbkdf2 As New Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256)

                Dim newHash As Byte() =
                    pbkdf2.GetBytes(HashSize)

                Return CryptographicOperations.FixedTimeEquals(
                    newHash,
                    storedPasswordHash)

            End Using

        Catch

            Return False

        End Try

    End Function

End Module