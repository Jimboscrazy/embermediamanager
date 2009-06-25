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

Option Explicit On

Imports System.IO
Imports System.IO.Compression

Public Class HTTP

    Dim _responseuri As String

    Public Event ProgressUpdated(ByVal iPercent As Integer)

    Public Property ResponseUri() As String
        Get
            Return Me._responseuri
        End Get
        Set(ByVal value As String)
            Me._responseuri = value
        End Set
    End Property

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._responseuri = String.Empty
    End Sub

    Public Function DownloadData(ByVal URL As String) As String
        Dim sResponse As String = String.Empty
        Me.Clear()

        Try

            Dim wrRequest As HttpWebRequest = HttpWebRequest.Create(URL)
            wrRequest.Method = "GET"
            wrRequest.Timeout = 10000
            wrRequest.Headers.Add("Accept-Encoding", "gzip,deflate")
            Dim wrResponse As HttpWebResponse = wrRequest.GetResponse()
            Dim contentEncoding As String = String.Empty
            For Each resKey As String In wrResponse.Headers.Keys
                If Not IsNothing(resKey) Then
                    If resKey.ToLower = "content-encoding" Then
                        contentEncoding = wrResponse.Headers.Item(resKey)
                        Exit For
                    End If
                End If
            Next
            Using Ms As Stream = wrResponse.GetResponseStream
                If contentEncoding.ToLower = "gzip" Then
                    sResponse = New StreamReader(New GZipStream(Ms, CompressionMode.Decompress)).ReadToEnd
                ElseIf contentEncoding.ToLower = "deflate" Then
                    sResponse = New StreamReader(New DeflateStream(Ms, CompressionMode.Decompress)).ReadToEnd
                Else
                    sResponse = New StreamReader(Ms).ReadToEnd
                End If
            End Using
            Me._responseuri = wrResponse.ResponseUri.ToString
            wrResponse.Close()
            wrResponse = Nothing
            wrRequest = Nothing
        Catch ex As Exception
        End Try

        Return sResponse
    End Function

    Public Function IsValidURL(ByVal URL As String) As Boolean
        Dim wrRequest As HttpWebRequest
        Dim wrResponse As HttpWebResponse
        Try
            wrRequest = HttpWebRequest.Create(URL)
            wrRequest.Method = "GET"
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

    Public Function DownloadFile(ByVal URL As String, ByVal LocalFile As String, ByVal ReportUpdate As Boolean) As String
        Dim outFile As String = String.Empty

        Try
            Dim wrRequest As HttpWebRequest = HttpWebRequest.Create(URL)
            wrRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)"
            wrRequest.Method = "GET"
            wrRequest.Timeout = 10000
            Dim wrResponse As HttpWebResponse = wrRequest.GetResponse()

            Select Case True
                Case wrResponse.ContentType.Contains("mp4")
                    outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.mp4", "[trailer].mp4")))
                Case wrResponse.ContentType.Contains("flv"), URL.ToLower.Contains("mattfind.com") AndAlso wrResponse.ContentType.Contains("plain") 'matttrailer reports "text/plain" for flv files
                    outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.flv", "[trailer].flv")))
                Case wrResponse.ContentType.Contains("shockwave"), wrResponse.ContentType.Contains("flash")
                    outFile = Path.Combine(Directory.GetParent(LocalFile).FullName, String.Concat(Path.GetFileNameWithoutExtension(LocalFile), If(Master.eSettings.DashTrailer, "-trailer.swf", "[trailer].swf")))
            End Select

            If Not String.IsNullOrEmpty(outFile) AndAlso wrResponse.ContentLength > 0 Then

                If File.Exists(outFile) Then File.Delete(outFile)

                Using Ms As Stream = wrResponse.GetResponseStream
                    Using mStream As New FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write)
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
                                    iProgress = (iCurrent / wrResponse.ContentLength) * 100
                                    RaiseEvent ProgressUpdated(iProgress)
                                End If
                            End If
                        Loop While BlockSize > 0
                        StreamBuffer = Nothing
                    End Using
                End Using
            End If

            wrResponse.Close()
            wrResponse = Nothing
            wrRequest = Nothing
        Catch
        End Try

        Return outFile
    End Function
End Class
