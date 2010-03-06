' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################



Imports System.IO
Imports System.IO.Compression

<Serializable()> _
Public Class HTTP

    Private _responseuri As String
    Private _image As Image
    Private _cancel As Boolean
    Private _URL As String = String.Empty
    Private dThread As New Threading.Thread(AddressOf DownloadImage)
    Private wrRequest As HttpWebRequest

    Public Event ProgressUpdated(ByVal iPercent As Integer)

    Public Property ResponseUri() As String
        Get
            Return Me._responseuri
        End Get
        Set(ByVal value As String)
            Me._responseuri = value
        End Set
    End Property

    Public ReadOnly Property Image() As Image
        Get
            Return Me._image
        End Get
    End Property

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._responseuri = String.Empty
        Me._image = Nothing
        Me._cancel = False
    End Sub

    Public Function DownloadZip(ByVal URL As String) As Byte()
        Dim wrRequest As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
        wrRequest.Timeout = 10000

        If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
            Dim wProxy As New WebProxy(Master.eSettings.ProxyURI, Master.eSettings.ProxyPort)
            wProxy.BypassProxyOnLocal = True
            If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) Then
                wProxy.Credentials = Master.eSettings.ProxyCreds
            Else
                wProxy.Credentials = CredentialCache.DefaultCredentials
            End If
            wrRequest.Proxy = wProxy
        End If

        Using wrResponse As HttpWebResponse = DirectCast(wrRequest.GetResponse(), HttpWebResponse)
            Return Functions.ReadStreamToEnd(wrResponse.GetResponseStream)
        End Using

    End Function

    Public Function DownloadData(ByVal URL As String) As String
        Dim sResponse As String = String.Empty
        Dim cEncoding As System.Text.Encoding

        Me.Clear()

        Try

            Dim wrRequest As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
            wrRequest.Timeout = 10000
            wrRequest.Headers.Add("Accept-Encoding", "gzip,deflate")

            If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
                Dim wProxy As New WebProxy(Master.eSettings.ProxyURI, Master.eSettings.ProxyPort)
                wProxy.BypassProxyOnLocal = True
                If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) Then
                    wProxy.Credentials = Master.eSettings.ProxyCreds
                Else
                    wProxy.Credentials = CredentialCache.DefaultCredentials
                End If
                wrRequest.Proxy = wProxy
            End If

            Using wrResponse As HttpWebResponse = DirectCast(wrRequest.GetResponse(), HttpWebResponse)
                Select Case True
                    'for our purposes I think it's safe to assume that all xmls we will be dealing with will be UTF-8 encoded
                    Case wrResponse.ContentType.ToLower.Contains("application/xml") OrElse wrResponse.ContentType.ToLower.Contains("charset=utf-8")
                        cEncoding = System.Text.Encoding.UTF8
                    Case Else
                        cEncoding = System.Text.Encoding.GetEncoding(28591)
                End Select
                Using Ms As Stream = wrResponse.GetResponseStream
                    If wrResponse.ContentEncoding.ToLower = "gzip" Then
                        sResponse = New StreamReader(New GZipStream(Ms, CompressionMode.Decompress), cEncoding, True).ReadToEnd
                    ElseIf wrResponse.ContentEncoding.ToLower = "deflate" Then
                        sResponse = New StreamReader(New DeflateStream(Ms, CompressionMode.Decompress), cEncoding, True).ReadToEnd
                    Else
                        sResponse = New StreamReader(Ms, cEncoding, True).ReadToEnd
                    End If
                End Using
                Me._responseuri = wrResponse.ResponseUri.ToString
            End Using
            wrRequest = Nothing
        Catch ex As Exception
        End Try

        Return sResponse
    End Function

    Public Function IsValidURL(ByVal URL As String) As Boolean
        Dim wrRequest As WebRequest
        Dim wrResponse As WebResponse
        Try
            wrRequest = HttpWebRequest.Create(URL)

            If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
                Dim wProxy As New WebProxy(Master.eSettings.ProxyURI, Master.eSettings.ProxyPort)
                wProxy.BypassProxyOnLocal = True
                If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) Then
                    wProxy.Credentials = Master.eSettings.ProxyCreds
                Else
                    wProxy.Credentials = CredentialCache.DefaultCredentials
                End If
                wrRequest.Proxy = wProxy
            End If

            Dim noCachePolicy As System.Net.Cache.HttpRequestCachePolicy = New System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore)
            wrRequest.CachePolicy = noCachePolicy
            wrRequest.Timeout = Master.eSettings.TrailerTimeout * 1000
            wrResponse = wrRequest.GetResponse()
        Catch ex As Exception
            Return False
        End Try
        wrResponse.Close()
        wrResponse = Nothing
        wrRequest = Nothing
        Return True
    End Function

    Public Function DownloadFile(ByVal URL As String, ByVal LocalFile As String, ByVal ReportUpdate As Boolean, ByVal Type As String) As String
        Dim outFile As String = String.Empty

        Try
            Dim wrRequest As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
            wrRequest.Timeout = 5000

            If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
                Dim wProxy As New WebProxy(Master.eSettings.ProxyURI, Master.eSettings.ProxyPort)
                wProxy.BypassProxyOnLocal = True
                If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) Then
                    wProxy.Credentials = Master.eSettings.ProxyCreds
                Else
                    wProxy.Credentials = CredentialCache.DefaultCredentials
                End If
                wrRequest.Proxy = wProxy
            End If

            Using wrResponse As HttpWebResponse = DirectCast(wrRequest.GetResponse(), HttpWebResponse)

                Select Case True
                    Case Type = "trailer" AndAlso wrResponse.ContentType.Contains("mp4")
                        outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.mp4", "[trailer].mp4")))
                    Case Type = "trailer" AndAlso (wrResponse.ContentType.Contains("flv") OrElse (URL.ToLower.Contains("mattfind.com") AndAlso wrResponse.ContentType.Contains("plain"))) 'matttrailer reports "text/plain" for flv files
                        outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.flv", "[trailer].flv")))
                    Case Type = "trailer" AndAlso (wrResponse.ContentType.Contains("shockwave") OrElse wrResponse.ContentType.Contains("flash"))
                        outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.swf", "[trailer].swf")))
                    Case Type = "translation"
                        outFile = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, URL.Substring(URL.LastIndexOf("/") + 1))
                    Case Type = "template"
                        Dim basePath As String = Path.Combine(Functions.AppPath, "Langs")
                        Dim folders() As String = URL.Replace("http://www.embermm.com/Updates/Translations/", String.Empty).Trim.Split(Convert.ToChar("/"))
                        For i As Integer = 0 To folders.Count - 2
                            If Not Directory.Exists(Path.Combine(basePath, folders(i))) Then Directory.CreateDirectory(Path.Combine(basePath, folders(i)))
                            basePath = Path.Combine(basePath, folders(i))
                        Next
                        outFile = Path.Combine(basePath, URL.Substring(URL.LastIndexOf("/") + 1))
                    Case Type = "movietheme"
                        outFile = String.Concat(Functions.AppPath, "Themes", Path.DirectorySeparatorChar, URL.Substring(URL.LastIndexOf("/") + 1))
                    Case Type = "other"
                        outFile = LocalFile
                End Select

                If Not String.IsNullOrEmpty(outFile) AndAlso wrResponse.ContentLength > 0 Then

                    If File.Exists(outFile) Then File.Delete(outFile)

                    Using Ms As Stream = wrResponse.GetResponseStream
                        Using mStream As New FileStream(outFile, FileMode.Create, FileAccess.Write)
                            Dim StreamBuffer(4096) As Byte
                            Dim BlockSize As Integer
                            Dim iProgress As Integer
                            Dim iCurrent As Integer
                            Do
                                BlockSize = Ms.Read(StreamBuffer, 0, 4096)
                                iCurrent += BlockSize
                                If BlockSize > 0 Then
                                    mStream.Write(StreamBuffer, 0, BlockSize)
                                    If ReportUpdate Then
                                        iProgress = Convert.ToInt32((iCurrent / wrResponse.ContentLength) * 100)
                                        RaiseEvent ProgressUpdated(iProgress)
                                    End If
                                End If
                            Loop While BlockSize > 0
                            StreamBuffer = Nothing
                        End Using
                    End Using
                End If

            End Using
            wrRequest = Nothing
        Catch
        End Try

        Return outFile
    End Function

    Public Sub StartDownloadImage(ByVal sURL As String)
        Me.Clear()
        Me._URL = sURL
        Me.dThread = New Threading.Thread(AddressOf DownloadImage)
        Me.dThread.IsBackground = True
        Me.dThread.Start()
    End Sub

    Public Function IsDownloading() As Boolean
        Return Me.dThread.IsAlive
    End Function

    Public Sub Cancel()
        Me._cancel = True
        Me.wrRequest.Abort()
    End Sub

    Public Sub DownloadImage()
        Try
            If StringUtils.isValidURL(Me._URL) Then
                wrRequest = DirectCast(HttpWebRequest.Create(Me._URL), HttpWebRequest)
                wrRequest.Timeout = 5000

                If Me._cancel Then Return

                If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
                    Dim wProxy As New WebProxy(Master.eSettings.ProxyURI, Master.eSettings.ProxyPort)
                    wProxy.BypassProxyOnLocal = True
                    If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) AndAlso _
                    Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.Password) Then
                        wProxy.Credentials = Master.eSettings.ProxyCreds
                    Else
                        wProxy.Credentials = CredentialCache.DefaultCredentials
                    End If
                    wrRequest.Proxy = wProxy
                End If

                If Me._cancel Then Return

                Using wrResponse As WebResponse = wrRequest.GetResponse()
                    If Me._cancel Then Return
                    Dim temp As String = wrResponse.ContentType.ToString
                    If wrResponse.ContentType.ToLower.Contains("image") Then
                        If Me._cancel Then Return
                        Me._image = ReadImageStreamToEnd(wrResponse.GetResponseStream)
                    End If
                End Using

                wrRequest = Nothing
            End If
        Catch
        End Try
    End Sub

    Public Function ReadImageStreamToEnd(ByVal rStream As Stream) As Image
        Try
            Dim StreamBuffer(4096) As Byte
            Dim BlockSize As Integer = 0
            Using mStream As MemoryStream = New MemoryStream()
                Do
                    BlockSize = rStream.Read(StreamBuffer, 0, StreamBuffer.Length)
                    If Me._cancel Then Return Nothing
                    If BlockSize > 0 Then mStream.Write(StreamBuffer, 0, BlockSize)
                    If Me._cancel Then Return Nothing

                Loop While BlockSize > 0

                Return Image.FromStream(mStream)
            End Using
        Catch
        End Try

        Return Nothing
    End Function
End Class
