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
Imports System.Text.RegularExpressions

Public Class OFDB
    Private _title As String
    Private _outline As String
    Private _plot As String

    Private OFDBMovie As Media.Movie
    Private imdbID As String

    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Public Property Outline() As String
        Get
            Return _outline
        End Get
        Set(ByVal value As String)
            _outline = value
        End Set
    End Property

    Public Property Plot() As String
        Get
            Return _plot
        End Get
        Set(ByVal value As String)
            _plot = value
        End Set
    End Property

    Private Sub Clear()
        _title = String.Empty
        _outline = String.Empty
        _plot = String.Empty
    End Sub

    Public Sub New(ByVal sID As String, ByRef mMovie As Media.Movie)
        Clear()
        imdbID = sID
        OFDBMovie = mMovie

        GetOFDBDetails()
    End Sub

    Private Function GetOFDBUrlFromIMDBID() As String
        Dim ofdbURL As String = String.Empty
        Dim Html As String = String.Empty
        Dim sURL = String.Concat("http://www.ofdb.de/view.php?SText=", imdbID, "&Kat=IMDb&page=suchergebnis&sourceid=mozilla-search")
        Try
            Dim Wc As New WebClient
            Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
            Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)")

            Dim MS As New MemoryStream(Wc.DownloadData(sURL))

            If Wc.ResponseHeaders(HttpResponseHeader.ContentEncoding) = "gzip" Then
                Html = New StreamReader(New GZipStream(MS, CompressionMode.Decompress)).ReadToEnd
            Else
                Html = New StreamReader(MS).ReadToEnd
            End If

            MS.Close()
            MS = Nothing

            If Not String.IsNullOrEmpty(Html) Then
                Dim mcOFDBURL As MatchCollection = Regex.Matches(Html, "<a href=""film/([^<]+)"" onmouseover")
                If mcOFDBURL.Count > 0 Then
                    'just use the first one if more are found
                    ofdbURL = String.Concat("http://www.ofdb.de/", Regex.Match(mcOFDBURL(0).Value.ToString, """(film/([^<]+))""").Groups(1).Value.ToString)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return ofdbURL
    End Function

    Private Sub GetOFDBDetails()
        Dim Html As String = String.Empty
        Dim sURL As String = GetOFDBUrlFromIMDBID()

        Try
            If Not String.IsNullOrEmpty(sURL) Then
                Dim Wc As New WebClient
                Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)")

                Dim MS As New MemoryStream(Wc.DownloadData(sURL))

                If Wc.ResponseHeaders(HttpResponseHeader.ContentEncoding) = "gzip" Then
                    Html = New StreamReader(New GZipStream(MS, CompressionMode.Decompress)).ReadToEnd
                Else
                    Html = New StreamReader(MS).ReadToEnd
                End If

                MS.Close()
                MS = Nothing

                If Not String.IsNullOrEmpty(Html) Then

                    If String.IsNullOrEmpty(OFDBMovie.Title) OrElse Not Master.eSettings.LockTitle Then
                        Dim OFDBTitle As String = CleanTitle(Web.HttpUtility.HtmlDecode(Regex.Match(Html, "<td width=""99\%""><h2><font face=""Arial,Helvetica,sans-serif"" size=""3""><b>([^<]+)</b></font></h2></td>").Groups(1).Value.ToString))
                        _title = OFDBTitle
                    End If

                    Dim D, W, B As Integer
                    Dim tmpHTML As String

                    'outline
                    If String.IsNullOrEmpty(OFDBMovie.Outline) OrElse Not Master.eSettings.LockOutline Then
                        D = Html.IndexOf("<b>Inhalt:</b>")

                        If D > 0 Then
                            W = Html.IndexOf("<a href=""", D + 14)
                            _outline = Html.Substring(D + 14, W - (D + 14)).Trim
                        End If
                    End If

                    'full plot
                    If String.IsNullOrEmpty(OFDBMovie.Plot) OrElse Not Master.eSettings.LockPlot Then
                        D = Html.IndexOf("<b>Inhalt:</b>")
                        If D > 0 Then
                            Dim L As Integer = Html.Length
                            tmpHTML = Html.Substring(D + 14, L - (D + 14)).Trim
                            W = tmpHTML.IndexOf("<a href=""")
                            If W > 0 Then
                                B = tmpHTML.IndexOf("""><b>[mehr]</b>", W + 9)
                                If B > 0 Then
                                    Dim FullPlot = GetFullPlot(String.Concat("http://www.ofdb.de/", tmpHTML.Substring(W + 9, B - (W + 9))))
                                    If Not String.IsNullOrEmpty(FullPlot) Then
                                        _plot = FullPlot
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Function GetFullPlot(ByVal sURL As String) As String
        Dim Html As String = String.Empty
        Dim FullPlot As String = String.Empty

        Try
            If Not String.IsNullOrEmpty(sURL) Then
                Dim Wc As New WebClient
                Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)")

                Dim MS As New MemoryStream(Wc.DownloadData(sURL))

                If Wc.ResponseHeaders(HttpResponseHeader.ContentEncoding) = "gzip" Then
                    Html = New StreamReader(New GZipStream(MS, CompressionMode.Decompress)).ReadToEnd
                Else
                    Html = New StreamReader(MS).ReadToEnd
                End If

                MS.Close()
                MS = Nothing

                Dim D, W, B As Integer
                Dim tmpHTML As String

                D = Html.IndexOf("Eine Inhaltsangabe von")
                If D > 0 Then
                    Dim L As Integer = Html.Length
                    tmpHTML = Html.Substring(D + 22, L - (D + 22)).Trim
                    W = tmpHTML.IndexOf("</b></b><br><br>")
                    If W > 0 Then
                        B = tmpHTML.IndexOf("</font></p>", W + 16)
                        FullPlot = tmpHTML.Substring(W + 16, B - (W + 16)).Replace("<br />", String.Empty).Replace(vbCrLf, " ").Trim
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return FullPlot
    End Function

    Private Function CleanTitle(ByVal sString As String) As String
        Dim CleanString As String = sString

        Try
            If sString.StartsWith("""") Then CleanString = sString.Remove(0, 1)

            If sString.EndsWith("""") Then CleanString = CleanString.Remove(CleanString.Length - 1, 1)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return CleanString
    End Function

End Class
