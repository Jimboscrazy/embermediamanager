Imports System.Net.Sockets
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Resources
Imports System.Reflection
Public Class frmMainSetup
    Public emberPath As String
    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        emberPath = String.Empty
        Dim Args() As String = Environment.GetCommandLineArgs
        For i As Integer = 1 To Args.Count - 1

        Next
        GetEmberVersion(Application.StartupPath)
        GetEmberPlatform(Application.StartupPath)
    End Sub
    Public Function GetEmberPlatform(ByVal fpath As String) As String
        Dim _Assembly As Assembly = Assembly.ReflectionOnlyLoadFrom(Path.Combine(fpath, "Ember Media Manager.exe"))
        Dim kinds As PortableExecutableKinds
        Dim imgFileMachine As ImageFileMachine
        _Assembly.ManifestModule.GetPEKind(kinds, imgFileMachine)
        If kinds And PortableExecutableKinds.PE32Plus Then
            Return "64"
        End If
        Return "32"
    End Function
    Public Function GetEmberVersion(ByVal fpath As String) As String
        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(fpath, "Ember Media Manager.exe"))
        Return myBuildInfo.ProductPrivatePart
    End Function
    Sub InvalidEmberVersion()
        Using oldVersion As New dlgNotMatchVersion
            If oldVersion.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                End
            End If
        End Using
    End Sub

    Public Function GetURLDataBin(ByVal URL As String, ByVal FName As String, _
        Optional ByRef UserName As String = "", _
        Optional ByRef Password As String = "") As Boolean
        Dim Req As HttpWebRequest
        Dim SourceStream As System.IO.Stream
        Dim Response As HttpWebResponse
        Try
            If System.IO.File.Exists(FName) Then
                System.IO.File.Delete(FName)
            End If
            'Ignore bad https certificates - expired, untrusted, bad name, etc.  
            'ServicePointManager.CertificatePolicy = New MyAcceptCertificatePolicy
            'Dim value As RemoteCertificateValidationCallback
            'value = ServicePointManager.ServerCertificateValidationCallback
            'ServicePointManager.ServerCertificateValidationCallback = New ServicePointManager.ServerCertificateValidationCallback
            'create a web request to the URL  
            Req = HttpWebRequest.Create(URL)
            '---
            Dim noCachePolicy As System.Net.Cache.HttpRequestCachePolicy = New System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
            Req.CachePolicy = noCachePolicy
            'Grrrrr.... HttpWebRequest does not know rfc  
            'you cannot use http://username:password@server:port/uri  
            'Set username and password if required  
            If Len(UserName) > 0 Then
                Req.Credentials = New NetworkCredential(UserName, Password)
            End If
            'get a response from web site  
            Response = Req.GetResponse()
            'Source stream with requested document  
            SourceStream = Response.GetResponseStream()
            Dim readStream As New BinaryReader(SourceStream)
            'SourceStream has no ReadAll, so we must read data block-by-block  
            'Temporary Buffer and block size  
            Dim Buffer(4096) As Byte, BlockSize As Integer
            'Memory stream to store data  
            Dim TempStream As New IO.FileStream(FName, IO.FileMode.Create)
            Do
                BlockSize = readStream.Read(Buffer, 0, 4096)
                If BlockSize > 0 Then TempStream.Write(Buffer, 0, BlockSize)
            Loop While BlockSize > 0
            TempStream.Close()
            readStream.Close()
            SourceStream.Close()
            Response.Close()
            Return True
        Catch ex As Exception
            'Report("Error", ex.Message)
            Return False
        End Try
    End Function

    Private Sub frmMainSetup_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

    End Sub
End Class
