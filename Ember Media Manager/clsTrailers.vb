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
Imports System.Collections.Generic
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class Trailers
    Private _ImdbID As String = String.Empty
    Private _ImdbTrailerPage As String = String.Empty
    Private _TrailerList As New ArrayList

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
                    Case Master.TrailerPages.Imdb
                        If Not String.IsNullOrEmpty(Me._ImdbTrailerPage) Then Me.GetImdbTrailer()
                    Case Master.TrailerPages.MattTrailer
                        If Not String.IsNullOrEmpty(Me._ImdbTrailerPage) Then Me.GetMattTrailer()
                    Case Master.TrailerPages.AZMovies
                        If Not String.IsNullOrEmpty(Me._ImdbTrailerPage) Then Me.GetAZMoviesTrailer()
                    Case Master.TrailerPages.YouTube
                        Me.GetYouTubeTrailer()
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
        Dim WebPage As HTTP
        Dim Link As Match
        Dim currPage As Integer

        Link = Regex.Match(_ImdbTrailerPage, "of [0-9]{1,3}")

        If Link.Success Then
            TrailerNumber = Convert.ToInt32(Link.Value.Substring(3))

            If TrailerNumber > 0 Then

                currPage = (TrailerNumber / 10)

                For i As Integer = 1 To currPage
                    If Not i = 1 Then
                        WebPage = New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", _ImdbID, "/trailers?pg=", i))
                        _ImdbTrailerPage = WebPage.Response
                    End If

                    Links = Regex.Matches(_ImdbTrailerPage, "/vi[0-9]+/")

                    For Each m As Match In Links
                        WebPage = New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/video/screenplay", m.Value, "player"))
                        trailerPage = WebPage.Response

                        trailerUrl = Web.HttpUtility.UrlDecode(Regex.Match(trailerPage, "http.+flv").Value)

                        If Not String.IsNullOrEmpty(trailerUrl) AndAlso WebPage.IsValidURL(trailerUrl) Then
                            Me._TrailerList.Add(trailerUrl)
                        End If
                    Next
                Next
            End If
        End If

    End Sub

    Private Sub GetMattTrailer()
        Dim Link As Match = Regex.Match(_ImdbTrailerPage, "http://MattTrailer.com/.+""")

        If Link.Success Then

            Dim WebPage As New HTTP(Link.Value.Substring(0, Link.Value.Length - 1))

            Dim MattPage As String = WebPage.Response

            Link = Regex.Match(MattPage, "trailer=.+flv")

            If Link.Success Then
                Dim TrailerUrl As String = String.Concat("http://mattfind.com/", Link.Value.Substring(8))

                If WebPage.IsValidURL(TrailerUrl) Then
                    Me._TrailerList.Add(TrailerUrl)
                End If
            End If
        End If
    End Sub

    Private Sub GetAZMoviesTrailer()
        Dim Link As Match = Regex.Match(_ImdbTrailerPage, "http://AZmovies.net/.+html")

        If Link.Success Then
            Dim WebPage As New HTTP(Link.Value)
            Dim AZPage As String = WebPage.Response
            Link = Regex.Match(AZPage, "http://www.dailymotion.com/swf/[0-9A-Za-z]+")
            If Link.Success Then
                WebPage = New HTTP(String.Concat("http://keepvid.com/?url=", Link.Value))
                AZPage = WebPage.Response

                Link = Regex.Match(AZPage, "http://proxy.+h264.+[0-9A-Za-z]+")

                If Link.Success Then
                    If WebPage.IsValidURL(Link.Value) Then
                        Me._TrailerList.Add(Link.Value)
                    End If
                End If

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
            Dim WebPage As New HTTP(YT)
            Dim YTPage As String = WebPage.Response

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
        Dim WebPage As New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", _ImdbID, "/trailers"))
        _ImdbTrailerPage = WebPage.Response
        If _ImdbTrailerPage.ToLower.Contains("page not found") Then
            _ImdbTrailerPage = String.Empty
        End If
    End Function

    Public Sub DownloadSingleTrailer(ByVal sPath As String, ByVal ImdbID As String)
        Me.GetTrailers(ImdbID, True)

        If Me._TrailerList.Count > 0 Then
            Dim sHTTP As New HTTP(Me._TrailerList.Item(0), sPath)
            sHTTP = Nothing
        End If
    End Sub

    Public Function DownloadSelectedTrailer(ByVal sPath As String, ByVal sIndex As Integer) As String
        Dim sHTTP As New HTTP(Me._TrailerList.Item(sIndex), sPath)
        Dim tPath As String = sHTTP.SavePath
        sHTTP = Nothing

        Return tPath
    End Function
End Class
