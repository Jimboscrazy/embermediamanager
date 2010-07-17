Imports System.Text.RegularExpressions
Imports System.IO

Class RSS_IMDB
    ' Duplicate from IMDB class in Native Scraper, but better not to use them ATM
    Private Const TITLE_PATTERN As String = "<a\shref=[""""'](?<url>.*?)[""""'].*?>(?<name>.*?)</a>((\s)+?(\((?<year>\d{4})(\/.*?)?\)))?((\s)+?(\((?<type>.*?)\)))?"
    Private Const IMDBURL As String = "akas.imdb.com"
    Private Const IMDB_ID_REGEX As String = "tt\d\d\d\d\d\d\d"
    Private Const MOVIE_TITLE_PATTERN As String = "(?<=<(title)>).*(?=<\/\1>)"
    Private Const TR_PATTERN As String = "<tr\sclass="".*?"">(.*?)</tr>"
    Private Const TABLE_PATTERN As String = "<table.*?>(.*?)</table>"
    Public Function SearchImdbLink(ByVal url As String) As String
        Dim sHTTP As New HTTP
        Dim HTML As String = sHTTP.DownloadData(url)
        Return Regex.Match(HTML, IMDB_ID_REGEX).ToString
    End Function
    Public Function SearchMovie(ByVal sMovie As String, Optional ByVal imdbid As String = "") As MovieSearchResults
        Try
            Dim D, W As Integer
            Dim R As New MovieSearchResults
            Dim sHTTP As New HTTP
            Dim HTML As String
            If String.IsNullOrEmpty(imdbid) Then
                HTML = sHTTP.DownloadData(String.Concat("http://", IMDBURL, "/find?s=all&q=", Web.HttpUtility.UrlEncode(sMovie, System.Text.Encoding.GetEncoding("ISO-8859-1")), "&x=0&y=0"))
            Else
                HTML = sHTTP.DownloadData(String.Concat("http://", IMDBURL, "/title/tt", imdbid))
            End If

            Dim rUri As String = sHTTP.ResponseUri
            sHTTP = Nothing
            'Check if we've been redirected straight to the movie page
            If Regex.IsMatch(rUri, IMDB_ID_REGEX) Then
                Dim lNewMovie As MediaContainers.Movie = New MediaContainers.Movie(Regex.Match(rUri, IMDB_ID_REGEX).ToString, _
                    StringUtils.ProperCase(sMovie), Regex.Match(Regex.Match(HTML, MOVIE_TITLE_PATTERN).ToString, "(?<=\()\d+(?=.*\))").ToString, 0)
                R.ExactMatches.Add(lNewMovie)
                Return R
            End If

            D = HTML.IndexOf("<b>Popular Titles</b>")
            If D <= 0 Then GoTo mPartial
            W = HTML.IndexOf("</table>", D) + 8

            Dim Table As String = Regex.Match(HTML.Substring(D, W - D), TABLE_PATTERN).ToString

            Dim qPopular = From Mtr In Regex.Matches(Table, TITLE_PATTERN) _
                           Where Not DirectCast(Mtr, Match).Groups("name").ToString.Contains("<img") AndAlso Not DirectCast(Mtr, Match).Groups("type").ToString.Contains("VG") _
                           Select New MediaContainers.Movie(GetMovieID(DirectCast(Mtr, Match).Groups("url").ToString), _
                                            Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString), Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("year").ToString), StringUtils.ComputeLevenshtein(StringUtils.FilterYear(sMovie).ToLower, StringUtils.FilterYear(Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString)).ToLower))

            R.PopularTitles = qPopular.ToList
mPartial:

            D = HTML.IndexOf("Titles (Partial Matches)")
            If D <= 0 Then GoTo mApprox
            W = HTML.IndexOf("</table>", D) + 8

            Table = Regex.Match(HTML.Substring(D, W - D), TABLE_PATTERN).ToString
            Dim qpartial = From Mtr In Regex.Matches(Table, TITLE_PATTERN) _
                Where Not DirectCast(Mtr, Match).Groups("name").ToString.Contains("<img") AndAlso Not DirectCast(Mtr, Match).Groups("type").ToString.Contains("VG") _
                Select New MediaContainers.Movie(GetMovieID(DirectCast(Mtr, Match).Groups("url").ToString), _
                                 Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString), Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("year").ToString), StringUtils.ComputeLevenshtein(StringUtils.FilterYear(sMovie).ToLower, StringUtils.FilterYear(Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString)).ToLower))

            R.PartialMatches = qpartial.ToList
mApprox:

            'Now process "Approx Matches" and merge both Partial and Approx matches
            D = HTML.IndexOf("Titles (Approx Matches)")
            If D <= 0 Then GoTo mExact
            W = HTML.IndexOf("</table>", D) + 8

            Table = Regex.Match(HTML.Substring(D, W - D), TABLE_PATTERN).ToString

            Dim qApprox = From Mtr In Regex.Matches(Table, TITLE_PATTERN) _
                Where Not DirectCast(Mtr, Match).Groups("name").ToString.Contains("<img") AndAlso Not DirectCast(Mtr, Match).Groups("type").ToString.Contains("VG") _
                Select New MediaContainers.Movie(GetMovieID(DirectCast(Mtr, Match).Groups("url").ToString), _
                                 Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString), Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("year").ToString), StringUtils.ComputeLevenshtein(StringUtils.FilterYear(sMovie).ToLower, StringUtils.FilterYear(Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString)).ToLower))

            If Not IsNothing(R.PartialMatches) Then
                R.PartialMatches = R.PartialMatches.Union(qApprox.ToList).ToList
            Else
                R.PartialMatches = qApprox.ToList
            End If

mExact:

            D = HTML.IndexOf("Titles (Exact Matches)")
            If D <= 0 Then GoTo mResult
            W = HTML.IndexOf("</table>", D) + 8

            Table = String.Empty
            Table = Regex.Match(HTML.Substring(D, W - D), TABLE_PATTERN).ToString

            Dim qExact = From Mtr In Regex.Matches(Table, TITLE_PATTERN) _
                           Where Not DirectCast(Mtr, Match).Groups("name").ToString.Contains("<img") AndAlso Not DirectCast(Mtr, Match).Groups("type").ToString.Contains("VG") _
                           Select New MediaContainers.Movie(GetMovieID(DirectCast(Mtr, Match).Groups("url").ToString), _
                        Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString.ToString), Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("year").ToString), StringUtils.ComputeLevenshtein(StringUtils.FilterYear(sMovie).ToLower, StringUtils.FilterYear(Web.HttpUtility.HtmlDecode(DirectCast(Mtr, Match).Groups("name").ToString)).ToLower))

            R.ExactMatches = qExact.ToList

mResult:
            Return R
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return Nothing
        End Try
    End Function
    Private Function GetMovieID(ByVal strObj As String) As String
        Return Regex.Match(strObj, IMDB_ID_REGEX).ToString.Replace("tt", String.Empty)
    End Function

    Public Shared Function IMDB_FindYear(ByVal tmpyear As String, ByVal lst As List(Of MediaContainers.Movie)) As Integer
        Dim ret As Integer = -1
        If IsNumeric(tmpyear) AndAlso Convert.ToInt32(tmpyear) > 1950 Then 'let's assume there are no movies older then 1950
            For c = 0 To lst.Count - 1
                If lst(c).Year = tmpyear Then
                    ret = c
                    Exit For
                End If
            Next
        End If

        Return ret
    End Function


    Public Class MovieSearchResults

#Region "Fields"

        Private _ExactMatches As New List(Of MediaContainers.Movie)
        Private _PartialMatches As New List(Of MediaContainers.Movie)
        Private _PopularTitles As New List(Of MediaContainers.Movie)

#End Region 'Fields

#Region "Properties"

        Public Property ExactMatches() As List(Of MediaContainers.Movie)
            Get
                Return _ExactMatches
            End Get
            Set(ByVal value As List(Of MediaContainers.Movie))
                _ExactMatches = value
            End Set
        End Property

        Public Property PartialMatches() As List(Of MediaContainers.Movie)
            Get
                Return _PartialMatches
            End Get
            Set(ByVal value As List(Of MediaContainers.Movie))
                _PartialMatches = value
            End Set
        End Property

        Public Property PopularTitles() As List(Of MediaContainers.Movie)
            Get
                Return _PopularTitles
            End Get
            Set(ByVal value As List(Of MediaContainers.Movie))
                _PopularTitles = value
            End Set
        End Property

#End Region 'Properties

    End Class
End Class