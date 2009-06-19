Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Public Class clsTrailers
    Private _ImdbID As String = String.Empty
    Private _ImdbTrailerPage As String = String.Empty
    Private _TrailerList As ArrayList

    ''' <summary>
    ''' Supported trailer pages
    ''' </summary>
    Public Enum TrailerPages
        Imdb
        MattTrailer
        AZMovies
        YouTube
    End Enum

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._TrailerList.Clear()
        Me._ImdbID = String.Empty
        Me._ImdbTrailerPage = String.Empty
    End Sub

    Public Function GetTrailers(ByVal ImdbID As String, ByVal TrailerPageSelection As List(Of TrailerPages), Optional ByVal BreakAfterFound As Boolean = True) As ArrayList
        Me._ImdbID = ImdbID

        If Me.GetImdbTrailerPage() Then
            For Each TP As TrailerPages In TrailerPageSelection
                If BreakAfterFound AndAlso Me._TrailerList.Count > 0 Then
                    Exit For
                End If
                Select Case TP
                    Case TrailerPages.Imdb
                        Me.GetImdbTrailer()
                    Case TrailerPages.MattTrailer
                        Me.GetMattTrailer()
                    Case TrailerPages.AZMovies
                        Me.GetAZMoviesTrailer()
                    Case TrailerPages.YouTube
                        Me.GetYouTubeTrailer()
                End Select
            Next

            Return Me._TrailerList
        End If

        Return Nothing
    End Function

    Private Sub GetImdbTrailer()
        Dim TrailerNumber As Integer = 0
        Dim Links As MatchCollection
        Dim trailerPage As String
        Dim trailerUrl As String
        Dim WebPage As HTTP
        Dim Link As Match

        Link = Regex.Match(_ImdbTrailerPage, "of [0-9]{1,3}")

        If Link.Success Then
            TrailerNumber = Convert.ToInt32(Link.Value.Substring(3))

            If TrailerNumber > 0 Then
                For i As Integer = 1 To TrailerNumber
                    If Not i = 1 Then
                        WebPage = New HTTP(String.Concat("http://www.imdb.com/title/tt", _ImdbID, "/trailers?pg=", i))
                        _ImdbTrailerPage = WebPage.Response
                    End If

                    Links = Regex.Matches(_ImdbTrailerPage, "/vi[0-9]+/")

                    For Each m As Match In Links
                        WebPage = New HTTP(String.Concat("http://www.imdb.com/video/screenplay", m.Value, "player"))
                        trailerPage = WebPage.Response

                        trailerUrl = Web.HttpUtility.UrlDecode(Regex.Match(trailerPage, "http.+flv").Value)

                        If Not String.IsNullOrEmpty(trailerUrl) Then
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
                Dim TrailerUrl As String = Link.Value.Substring(8)

                Me._TrailerList.Add(String.Concat("http://mattfind.com/", TrailerUrl))
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
                    Me._TrailerList.Add(Link.Value)
                End If

            End If
        End If
    End Sub

    Private Sub GetYouTubeTrailer()
        Dim TMDB As New TMDB.Scraper
        Dim YT As String = TMDB.GetTrailers(_ImdbID)

        If Not String.IsNullOrEmpty(YT) Then
            Me._TrailerList.Add(YT)
        End If

        TMDB = Nothing
    End Sub

    Private Function GetImdbTrailerPage() As Boolean
        Dim WebPage As New HTTP(String.Concat("http://www.imdb.com/title/tt", _ImdbID, "/trailers"))
        _ImdbTrailerPage = WebPage.Response
        If Not String.IsNullOrEmpty(_ImdbTrailerPage) AndAlso Not _ImdbTrailerPage.ToLower.Contains("page not found") AndAlso _
        Not _ImdbTrailerPage.ToLower.Contains("there are no related videos available") Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
