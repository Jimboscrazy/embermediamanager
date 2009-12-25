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

Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class Trailers
    Private _ImdbID As String = String.Empty
    Private _ImdbTrailerPage As String = String.Empty
    Private _TrailerList As New ArrayList
    Private WebPage As New HTTP

    Public Event ProgressUpdated(ByVal iPercent As Integer)

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._TrailerList.Clear()
        Me._ImdbID = String.Empty
        Me._ImdbTrailerPage = String.Empty
    End Sub

    Public Function GetTrailers(ByVal ImdbID As String, Optional ByVal BreakAfterFound As Boolean = True) As ArrayList
        Me._ImdbID = ImdbID

        Me.GetImdbTrailerPage()

        For Each TP As Master.TrailerPages In Master.eSettings.TrailerSites
            If BreakAfterFound AndAlso Me._TrailerList.Count > 0 Then
                Exit For
            End If
            Try
                Select Case TP
                    Case Master.TrailerPages.YouTube
                        Me.GetYouTubeTrailer()
                    Case Master.TrailerPages.Imdb
                        If Not String.IsNullOrEmpty(Me._ImdbTrailerPage) Then Me.GetImdbTrailer()
                End Select
            Catch
            End Try
        Next

        Return Me._TrailerList

    End Function

    Private Sub GetImdbTrailer()
        Dim TrailerNumber As Integer = 0
        Dim Links As MatchCollection
        Dim trailerPage As String
        Dim trailerUrl As String
        Dim Link As Match
        Dim currPage As Integer = 0

        Link = Regex.Match(_ImdbTrailerPage, "of [0-9]{1,3}")

        If Link.Success Then
            TrailerNumber = Convert.ToInt32(Link.Value.Substring(3))

            If TrailerNumber > 0 Then
                currPage = Convert.ToInt32(Math.Ceiling(TrailerNumber / 10))

                For i As Integer = 1 To currPage
                    If Not i = 1 Then
                        _ImdbTrailerPage = WebPage.DownloadData(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", _ImdbID, "/videogallery/content_type-Trailer?page=", i))
                    End If

                    Links = Regex.Matches(_ImdbTrailerPage, "/vi[0-9]+/")

                    For Each m As Match In Links
                        trailerPage = WebPage.DownloadData(String.Concat("http://", Master.eSettings.IMDBURL, "/video/screenplay", m.Value, "player"))

                        trailerUrl = Web.HttpUtility.UrlDecode(Regex.Match(trailerPage, "http.+flv").Value)

                        If Not String.IsNullOrEmpty(trailerUrl) AndAlso WebPage.IsValidURL(trailerUrl) Then
                            Me._TrailerList.Add(trailerUrl)
                        End If
                    Next
                Next
            End If
        End If

    End Sub

    Private Sub GetYouTubeTrailer()
        Dim TMDB As New TMDB.Scraper
        Dim YT As String = TMDB.GetTrailers(_ImdbID)
        Dim T As String = String.Empty
        Dim videoID As String = String.Empty
        Dim L As String = String.Empty
        Dim DLURL As String = String.Empty
        Dim tURL As String = String.Empty

        If Not String.IsNullOrEmpty(YT) Then
            Dim YTPage As String = WebPage.DownloadData(YT)
            If Not String.IsNullOrEmpty(YTPage) Then

                'new YouTube scraper
                Dim scraper As New YouTube.Scraper()
                scraper.GetVideoLinks(YT)

                If scraper.VideoLinks.Count > 0 Then
                    Me._TrailerList.Add(scraper.VideoLinks.Values(0).URL)
                End If
            End If
        End If

        TMDB = Nothing
    End Sub

    Private Function GetImdbTrailerPage() As Boolean
        _ImdbTrailerPage = WebPage.DownloadData(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", _ImdbID, "/videogallery/content_type-Trailer"))
        If _ImdbTrailerPage.ToLower.Contains("page not found") Then
            _ImdbTrailerPage = String.Empty
        End If
    End Function

    Public Function DownloadSingleTrailer(ByVal sPath As String, ByVal ImdbID As String, ByVal isSingle As Boolean, ByVal currNfoTrailer As String) As String
        Dim tURL As String = String.Empty
        Try
            Me._TrailerList.Clear()

            If Not Master.eSettings.UpdaterTrailersNoDownload AndAlso IsAllowedToDownload(sPath, isSingle, currNfoTrailer, True) Then
                Me.GetTrailers(ImdbID, True)

                If Me._TrailerList.Count > 0 Then
                    tURL = WebPage.DownloadFile(Me._TrailerList.Item(0).ToString, sPath, False, "trailer")
                    If Not String.IsNullOrEmpty(tURL) Then
                        'delete any other trailer if enabled in settings and download successful
                        If Master.eSettings.DeleteAllTrailers Then
                            Me.DeleteTrailers(sPath, tURL)
                        End If
                    End If
                End If
            ElseIf Master.eSettings.UpdaterTrailersNoDownload AndAlso IsAllowedToDownload(sPath, isSingle, currNfoTrailer, False) Then
                Me.GetTrailers(ImdbID, True)

                If Me._TrailerList.Count > 0 Then
                    tURL = Me._TrailerList.Item(0).ToString
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return tURL
    End Function

    Public Function DownloadYouTubeTrailer(ByVal sPath As String, ByVal sURL As String) As String

        Dim T As String = String.Empty
        Dim videoID As String = String.Empty
        Dim L As String = String.Empty
        Dim DLURL As String = String.Empty
        Dim tURL As String = String.Empty

        If Not String.IsNullOrEmpty(sURL) Then
            tURL = WebPage.DownloadFile(sURL, sPath, True, "trailer")

            If Not String.IsNullOrEmpty(tURL) Then
                'delete any other trailer if enabled in settings and download successful
                If Master.eSettings.DeleteAllTrailers Then
                    Me.DeleteTrailers(sPath, tURL)
                End If
            End If
        End If

        Return tURL
    End Function

    Public Function DownloadSelectedTrailer(ByVal sPath As String, ByVal sIndex As Integer) As String
        Dim tURL As String = WebPage.DownloadFile(Me._TrailerList.Item(sIndex).ToString, sPath, True, "trailer")

        If Not String.IsNullOrEmpty(tURL) Then
            'delete any other trailer if enabled in settings and download successful
            If Master.eSettings.DeleteAllTrailers Then
                Me.DeleteTrailers(sPath, tURL)
            End If
        End If

        Return tURL
    End Function

    Public Function IsAllowedToDownload(ByVal sPath As String, ByVal isDL As Boolean, ByVal currNfoTrailer As String, Optional ByVal isSS As Boolean = False) As Boolean
        If isDL Then
            If String.IsNullOrEmpty(Master.GetTrailerPath(sPath)) OrElse Master.eSettings.OverwriteTrailer Then
                Return True
            Else
                If isSS AndAlso String.IsNullOrEmpty(Master.GetTrailerPath(sPath)) Then
                    If String.IsNullOrEmpty(currNfoTrailer) OrElse Not Master.eSettings.LockTrailer Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            End If
        Else
            If String.IsNullOrEmpty(currNfoTrailer) OrElse Not Master.eSettings.LockTrailer Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function ShowTDialog(ByVal IMDBID As String, ByVal sPath As String, ByVal currNfoTrailer As String) As String
        If IsAllowedToDownload(sPath, True, currNfoTrailer) Then
            Using dTrailer As New dlgTrailer
                Dim tURL As String = dTrailer.ShowDialog(IMDBID, sPath)
                Return tURL
            End Using
        Else
            Return String.Empty
        End If
    End Function

    Public Sub DeleteTrailers(ByVal sPath As String, ByVal NewTrailer As String)
        Dim parPath As String = Directory.GetParent(sPath).FullName
        Dim tmpName As String = Path.Combine(parPath, StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath)))
        Dim tmpNameNoStack As String = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath))
        For Each t As String In Master.eSettings.ValidExts
            If File.Exists(String.Concat(tmpName, "-trailer", t)) AndAlso Not String.Concat(tmpName, "-trailer", t).ToLower = NewTrailer.ToLower Then
                File.Delete(String.Concat(tmpName, "-trailer", t))
            ElseIf File.Exists(String.Concat(tmpName, "[trailer]", t)) AndAlso Not String.Concat(tmpName, "[trailer]", t).ToLower = NewTrailer.ToLower Then
                File.Delete(String.Concat(tmpName, "[trailer]", t))
            ElseIf File.Exists(String.Concat(tmpNameNoStack, "-trailer", t)) AndAlso Not String.Concat(tmpNameNoStack, "-trailer", t).ToLower = NewTrailer.ToLower Then
                File.Delete(String.Concat(tmpNameNoStack, "-trailer", t))
            ElseIf File.Exists(String.Concat(tmpNameNoStack, "[trailer]", t)) AndAlso Not String.Concat(tmpNameNoStack, "[trailer]", t).ToLower = NewTrailer.ToLower Then
                File.Delete(String.Concat(tmpNameNoStack, "[trailer]", t))
            End If
        Next
    End Sub



End Class
