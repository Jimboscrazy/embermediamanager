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
' Highly modified version of original code by Maximilian "mafis90" Fischer

Option Explicit On

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

        AddHandler WebPage.ProgressUpdated, AddressOf DownloadProgressUpdated

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
                currPage = (TrailerNumber / 10)

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
        Dim StartIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim tLink As String = String.Empty
        Dim T As String = String.Empty
        Dim videoID As String = String.Empty
        Dim L As String = String.Empty

        If Not String.IsNullOrEmpty(YT) Then
            Dim YTPage As String = WebPage.DownloadData(YT)

            If Not String.IsNullOrEmpty(YTPage) Then
                StartIndex = YTPage.IndexOf("/watch_fullscreen?") + 18
                If StartIndex > 0 Then
                    EndIndex = YTPage.IndexOf("title=", StartIndex)
                    If EndIndex > 0 Then
                        tLink = YTPage.Substring(StartIndex, (EndIndex - StartIndex)).Trim

                        If Not String.IsNullOrEmpty(tLink) Then
                            StartIndex = tLink.IndexOf("&t=") + 3
                            If StartIndex > 0 Then
                                EndIndex = tLink.IndexOf("&", StartIndex + 1)
                                If EndIndex > 0 Then
                                    T = tLink.Substring(StartIndex, (EndIndex - StartIndex))

                                    If Not String.IsNullOrEmpty(T) Then
                                        StartIndex = tLink.IndexOf("&video_id=") + 10
                                        If StartIndex > 0 Then
                                            EndIndex = tLink.IndexOf("&", StartIndex + 1)
                                            If EndIndex > 0 Then
                                                videoID = tLink.Substring(StartIndex, (EndIndex - StartIndex))

                                                If Not String.IsNullOrEmpty(videoID) Then
                                                    StartIndex = tLink.IndexOf("&l=") + 3
                                                    If StartIndex > 0 Then
                                                        EndIndex = tLink.IndexOf("&", StartIndex + 1)
                                                        If EndIndex > 0 Then
                                                            L = tLink.Substring(StartIndex, EndIndex - StartIndex)

                                                            If Not String.IsNullOrEmpty(L) Then
                                                                Dim YTURL As String = String.Format("http://www.youtube.com/get_video?video_id={0}&l={1}&t={2}&fmt=18", videoID, L, T)
                                                                If WebPage.IsValidURL(YTURL) Then
                                                                    Me._TrailerList.Add(YTURL)
                                                                ElseIf WebPage.IsValidURL(YTURL.Replace("&fmt=18", String.Empty)) Then
                                                                    'try the flv version
                                                                    Me._TrailerList.Add(YTURL.Replace("&fmt=18", String.Empty))
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
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

    Public Function DownloadSingleTrailer(ByVal sPath As String, ByVal ImdbID As String, ByVal isFile As Boolean, ByVal currNfoTrailer As String) As String
        Dim tURL As String = String.Empty

        Me._TrailerList.Clear()

        If Not Master.eSettings.UpdaterTrailersNoDownload AndAlso IsAllowedToDownload(sPath, isFile, True, currNfoTrailer) Then
            Me.GetTrailers(ImdbID, True)

            If Me._TrailerList.Count > 0 Then
                tURL = WebPage.DownloadFile(Me._TrailerList.Item(0), sPath, False)
                If Not String.IsNullOrEmpty(tURL) Then
                    'delete any other trailer if enabled in settings and download successful
                    If Master.eSettings.DeleteAllTrailers Then
                        Me.DeleteTrailers(sPath, isFile, tURL)
                    End If
                End If
            End If
        ElseIf Master.eSettings.UpdaterTrailersNoDownload AndAlso IsAllowedToDownload(sPath, isFile, False, currNfoTrailer) Then
            Me.GetTrailers(ImdbID, True)

            If Me._TrailerList.Count > 0 Then
                tURL = Me._TrailerList.Item(0)
            End If
        End If

        Return tURL
    End Function

    Public Function DownloadSelectedTrailer(ByVal sPath As String, ByVal sIndex As Integer, ByVal isFile As Boolean) As String
        Dim tURL As String = WebPage.DownloadFile(Me._TrailerList.Item(sIndex), sPath, True)

        If Not String.IsNullOrEmpty(tURL) Then
            'delete any other trailer if enabled in settings and download successful
            If Master.eSettings.DeleteAllTrailers Then
                Me.DeleteTrailers(sPath, isFile, tURL)
            End If
        End If

        Return tURL
    End Function

    Public Sub DownloadProgressUpdated(ByVal iProgress As Integer)
        RaiseEvent ProgressUpdated(iProgress)
    End Sub

    Public Function IsAllowedToDownload(ByVal sPath As String, ByVal isFile As Boolean, ByVal isDL As Boolean, ByVal currNfoTrailer As String, Optional ByVal isSingle As Boolean = False) As Boolean
        If isDL Then
            If String.IsNullOrEmpty(Master.GetTrailerPath(sPath, isFile)) OrElse Master.eSettings.OverwriteTrailer Then
                Return True
            Else
                If isSingle AndAlso String.IsNullOrEmpty(Master.GetTrailerPath(sPath, isFile)) Then
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

    Public Function ShowTDialog(ByVal IMDBID As String, ByVal sPath As String, ByVal isFile As String, ByVal currNfoTrailer As String) As String
        If IsAllowedToDownload(sPath, isFile, True, currNfoTrailer) Then
            Using dTrailer As New dlgTrailer
                Dim tURL As String = dTrailer.ShowDialog(IMDBID, sPath, isFile)
                Return tURL
            End Using
        Else
            Return String.Empty
        End If
    End Function

    Public Sub DeleteTrailers(ByVal sPath As String, ByVal isFile As Boolean, ByVal NewTrailer As String)
        If isFile Then
            Dim parPath As String = Directory.GetParent(sPath).FullName
            Dim tmpName As String = Path.Combine(parPath, Master.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath)))
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
        Else
            Dim di As New DirectoryInfo(Directory.GetParent(sPath).FullName)
            Dim lFi As New List(Of FileInfo)()

            Try
                lFi.AddRange(di.GetFiles())
            Catch
            End Try

            For Each sFile As FileInfo In lFi
                If Master.eSettings.ValidExts.Contains(sFile.Extension.ToLower) AndAlso Not sFile.Name.ToLower.Contains("sample") AndAlso _
                    (sFile.Name.ToLower.Contains("-trailer") OrElse sFile.Name.ToLower.Contains("[trailer")) AndAlso _
                    Not sFile.FullName.ToLower = NewTrailer.ToLower Then
                    File.Delete(sFile.FullName)
                End If
            Next
        End If
    End Sub
End Class
