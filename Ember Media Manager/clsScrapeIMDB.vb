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
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.IO.Compression
Imports System.Globalization

Namespace IMDB
    Public Class MovieSearchResults
        Private _PopularTitles As New List(Of Media.Movie)
        Private _ExactMatches As New List(Of Media.Movie)
        Private _PartialMatches As New List(Of Media.Movie)

        Public Property ExactMatches() As List(Of Media.Movie)
            Get
                Return _ExactMatches
            End Get
            Set(ByVal value As List(Of Media.Movie))
                _ExactMatches = value
            End Set
        End Property

        Public Property PartialMatches() As List(Of Media.Movie)
            Get
                Return _PartialMatches
            End Get
            Set(ByVal value As List(Of Media.Movie))
                _PartialMatches = value
            End Set
        End Property

        Public Property PopularTitles() As List(Of Media.Movie)
            Get
                Return _PopularTitles
            End Get
            Set(ByVal value As List(Of Media.Movie))
                _PopularTitles = value
            End Set
        End Property

    End Class

    Public Class Scraper

        Friend WithEvents bwIMDB As New System.ComponentModel.BackgroundWorker

        Public Event MovieInfoDownloaded(ByVal bSuccess As Boolean)
        Public Event SearchMovieInfoDownloaded(ByVal sPoster As String, ByVal bSuccess As Boolean)
        Public Event SearchResultsDownloaded(ByVal mResults As IMDB.MovieSearchResults)

        Public Event Exception(ByVal ex As Exception)
        Public Event ProgressUpdated(ByVal iPercent As Integer)

        Private Const TABLE_PATTERN As String = "<table.*?>(.*?)</table>"
        Private Const HREF_PATTERN As String = "<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>"
        Private Const HREF_PATTERN_2 As String = "<a\shref=[""""'](?<url>.*?)[""""'].*?>(?<name>.*?)</a>"
        Private Const HREF_PATTERN_3 As String = "<a href=""/List\?certificates=[^""]*"">([^<]*):([^<]*)</a>[^<]*(<i>([^<]*)</i>)?"
        Private Const IMG_PATTERN As String = "<img src=""(?<thumb>.*?)"" width=""\d{1,3}"" height=""\d{1,3}"" border="".{1,3}"">"
        Private Const TR_PATTERN As String = "<tr\sclass="".*?"">(.*?)</tr>"
        Private Const TD_PATTERN_1 As String = "<td\sclass=""nm"">(.*?)</td>"
        Private Const TD_PATTERN_2 As String = "(?<=<td\sclass=""char"">)(.*?)(?=</td>)"
        Private Const TD_PATTERN_3 As String = "<td\sclass=""hs"">(.*?)</td>"
        Private Const MOVIE_TITLE_PATTERN As String = "(?<=<(title)>).*(?=<\/\1>)"
        Private Const IMDB_ID_REGEX As String = "tt\d\d\d\d\d\d\d"

        Private sPoster As String

        Private Enum SearchType
            Movies = 0
            Details = 1
            SearchDetails = 2
        End Enum

        Private Structure Results
            Dim ResultType As SearchType
            Dim Success As Boolean
            Dim Result As Object
        End Structure

        Private Structure Arguments
            Dim Search As SearchType
            Dim IMDBMovie As Media.Movie
            Dim FullCrew As Boolean
            Dim FullCast As Boolean
            Dim Parameter As String
        End Structure

        Private Function GetMovieID(ByVal strObj As String) As String
            Return Regex.Match(strObj, IMDB_ID_REGEX).ToString.Replace("tt", String.Empty)
        End Function

        Public Function GetSearchMovieInfo(ByVal sMovieName As String, ByRef imdbMovie As Media.Movie, ByVal iType As Master.ScrapeType) As Media.Movie
            Dim r As MovieSearchResults = SearchMovie(sMovieName)
            Dim b As Boolean = False

            Try
                Select Case iType
                    Case Master.ScrapeType.FullAsk, Master.ScrapeType.UpdateAsk, Master.ScrapeType.NewAsk, Master.ScrapeType.MarkAsk
                        If r.PopularTitles.Count = 1 Then
                            b = GetMovieInfo(r.PopularTitles.Item(0).IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                        ElseIf r.ExactMatches.Count > 0 Then
                            b = GetMovieInfo(r.ExactMatches.Item(0).IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                        Else
                            Master.tmpMovie = New Media.Movie
                            Using dIMDB As New dlgIMDBSearchResults
                                If dIMDB.ShowDialog(r, sMovieName) = Windows.Forms.DialogResult.OK Then
                                    If String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                        b = False
                                    Else
                                        b = GetMovieInfo(Master.tmpMovie.IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                                    End If
                                Else
                                    b = False
                                End If
                            End Using
                        End If
                    Case Master.ScrapeType.FullAuto, Master.ScrapeType.UpdateAuto, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAuto
                        'it seems "popular matches" is a better result than "exact matches"
                        If r.PopularTitles.Count > 0 Then
                            b = GetMovieInfo(r.PopularTitles.Item(0).IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                            'no popular matches, try exact matches
                        ElseIf r.ExactMatches.Count > 0 Then
                            b = GetMovieInfo(r.ExactMatches.Item(0).IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                            'no populartitles, get partial matches
                        ElseIf r.PartialMatches.Count > 0 Then
                            b = GetMovieInfo(r.PartialMatches.Item(0).IMDBID, imdbMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                        End If
                End Select

                If b Then
                    Return imdbMovie
                Else
                    Return New Media.Movie
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return New Media.Movie
            End Try
        End Function

        Public Sub GetSearchMovieInfoAsync(ByVal imdbID As String, ByVal IMDBMovie As Media.Movie)
            Try
                If Not bwIMDB.IsBusy Then
                    bwIMDB.WorkerReportsProgress = False
                    bwIMDB.RunWorkerAsync(New Arguments With {.Search = SearchType.SearchDetails, _
                                           .Parameter = imdbID, .IMDBMovie = IMDBMovie})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub GetMovieInfoAsync(ByVal imdbID As String, ByRef IMDBMovie As Media.Movie, Optional ByVal FullCrew As Boolean = False, Optional ByVal FullCast As Boolean = False)
            Try
                If Not bwIMDB.IsBusy Then
                    bwIMDB.WorkerReportsProgress = True
                    bwIMDB.RunWorkerAsync(New Arguments With {.Search = SearchType.Details, _
                                           .Parameter = imdbID, .IMDBMovie = IMDBMovie, .FullCrew = FullCrew, .FullCast = FullCast})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub SearchMovieAsync(ByVal sMovie As String)
            Try
                If Not bwIMDB.IsBusy Then
                    bwIMDB.RunWorkerAsync(New Arguments With {.Search = SearchType.Movies, .Parameter = sMovie})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Private Function SearchMovie(ByVal sMovie As String) As MovieSearchResults
            Try

                Dim D, W As Integer
                Dim R As New MovieSearchResults

                Dim sHTTP As New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/find?s=all&q=", Web.HttpUtility.UrlEncode(sMovie, System.Text.Encoding.GetEncoding("ISO-8859-1"))))
                Dim HTML As String = sHTTP.Response
                Dim rUri As String = sHTTP.ResponseUri
                sHTTP = Nothing


                'Check if we've been redirected straight to the movie page
                If Regex.Match(rUri, IMDB_ID_REGEX).Success Then
                    Dim lNewMovie As Media.Movie = New Media.Movie(Regex.Match(rUri, IMDB_ID_REGEX).ToString, _
                                                                CapitalizeAll(sMovie))
                    R.ExactMatches.Add(lNewMovie)
                    Return R
                    Exit Function
                End If

                D = Html.IndexOf("<b>Popular Titles</b>")
                If D <= 0 Then GoTo mPartial
                W = Html.IndexOf("</p>", D)

                Dim Table As String = Regex.Match(Html.Substring(D, W - D), TABLE_PATTERN).ToString

                Dim qPopular = From Mtr As Match In Regex.Matches(Table, HREF_PATTERN_2) _
                               Where Not Mtr.Groups("name").ToString.Contains("<img") _
                               Select New Media.Movie(GetMovieID(Mtr.Groups("url").ToString), _
                                                Web.HttpUtility.HtmlDecode(Mtr.Groups("name").ToString))

                R.PopularTitles = qPopular.ToList
mPartial:


                D = Html.IndexOf("Titles (Partial Matches)")
                If D <= 0 Then GoTo mApprox
                W = Html.IndexOf("</p>", D)

                Table = Regex.Match(Html.Substring(D, W - D), TABLE_PATTERN).ToString
                Dim qpartial = From Mtr As Match In Regex.Matches(Table, HREF_PATTERN_2) _
                    Where Not Mtr.Groups("name").ToString.Contains("<img") _
                    Select New Media.Movie(GetMovieID(Mtr.Groups("url").ToString), _
                                     Web.HttpUtility.HtmlDecode(Mtr.Groups("name").ToString))


                R.PartialMatches = qpartial.ToList
mApprox:

                'Now process "Approx Matches" and merge both Partial and Approx matches
                D = Html.IndexOf("Titles (Approx Matches)")
                If D <= 0 Then GoTo mExact
                W = Html.IndexOf("</p", D)

                Table = Regex.Match(Html.Substring(D, W - D), TABLE_PATTERN).ToString

                Dim qApprox = From Mtr As Match In Regex.Matches(Table, HREF_PATTERN_2) _
                    Where Not Mtr.Groups("name").ToString.Contains("<img") _
                    Select New Media.Movie(GetMovieID(Mtr.Groups("url").ToString), _
                                     Web.HttpUtility.HtmlDecode(Mtr.Groups("name").ToString))

                If R.PartialMatches IsNot Nothing Then
                    R.PartialMatches = R.PartialMatches.Union(qApprox.ToList).ToList
                Else
                    R.PartialMatches = qApprox.ToList
                End If

mExact:


                D = Html.IndexOf("Titles (Exact Matches)")
                If D <= 0 Then GoTo mResult
                W = Html.IndexOf("</p>", D)

                Table = String.Empty
                Table = Regex.Match(Html.Substring(D, W - D), TABLE_PATTERN).ToString

                Dim qExact = From Mtr As Match In Regex.Matches(Table, HREF_PATTERN_2) _
                               Where Not Mtr.Groups("name").ToString.Contains("<img") _
                               Select New Media.Movie(GetMovieID(Mtr.Groups("url").ToString), _
                            Web.HttpUtility.HtmlDecode(Mtr.Groups("name").ToString.ToString))

                R.ExactMatches = qExact.ToList

mResult:
                Return R
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return Nothing
            End Try
        End Function

        Public Function GetMovieInfo(ByVal strID As String, ByRef IMDBMovie As Media.Movie, ByVal FullCrew As Boolean, ByVal FullCast As Boolean, ByVal GetPoster As Boolean) As Boolean
            Try
                Dim ofdbTitle As String = String.Empty
                Dim ofdbOutline As String = String.Empty
                Dim ofdbPlot As String = String.Empty
                Dim ofdbGenre As String = String.Empty

                If Master.eSettings.UseOFDBTitle OrElse Master.eSettings.UseOFDBOutline OrElse Master.eSettings.UseOFDBPlot OrElse Master.eSettings.UseOFDBGenre Then
                    Dim OFDBScrape As New OFDB(strID, IMDBMovie)
                    If Master.eSettings.UseOFDBTitle Then ofdbTitle = OFDBScrape.Title
                    If Master.eSettings.UseOFDBOutline Then ofdbOutline = OFDBScrape.Outline
                    If Master.eSettings.UseOFDBPlot Then ofdbPlot = OFDBScrape.Plot
                    If Master.eSettings.UseOFDBGenre Then ofdbGenre = OFDBScrape.Genre
                End If

                Dim sHTTP As New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", strID, If(FullCrew OrElse FullCast, "/combined", String.Empty)))
                Dim HTML As String = sHTTP.Response
                sHTTP = Nothing

                Dim sPlot As New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", strID, "/plotsummary"))
                Dim PlotHtml As String = sPlot.Response
                sPlot = Nothing

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(1)
                End If
                IMDBMovie.IMDBID = strID

                Dim OriginalTitle As String = Regex.Match(Html, MOVIE_TITLE_PATTERN).ToString
                IMDBMovie.OriginalTitle = CleanTitle(Web.HttpUtility.HtmlDecode(Regex.Match(OriginalTitle, ".*(?=\s\(\d+.*?\))").ToString)).Trim
                If String.IsNullOrEmpty(IMDBMovie.Title) OrElse Not Master.eSettings.LockTitle Then
                    If Not String.IsNullOrEmpty(ofdbTitle) Then
                        IMDBMovie.Title = ofdbTitle.Trim
                    Else
                        IMDBMovie.Title = IMDBMovie.OriginalTitle.Trim
                    End If
                End If
                If GetPoster Then
                    sPoster = Regex.Match(Regex.Match(Html, "(?<=\b(name=""poster"")).*\b[</a>]\b").ToString, "(?<=\b(src=)).*\b(?=[</a>])").ToString.Replace("""", String.Empty).Replace("/></", String.Empty)
                End If

                IMDBMovie.Year = CInt(Regex.Match(OriginalTitle, "(?<=\()\d+(?=.*\))").ToString)

                Dim sRated As String = Regex.Match(Html, "MPAA</a>:</h5>(.[^<]*)", RegexOptions.Singleline Or RegexOptions.IgnoreCase Or RegexOptions.Multiline).Groups(1).Value
                IMDBMovie.MPAA = Web.HttpUtility.HtmlDecode(sRated).Trim()

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(2)
                End If

                Dim D, W As Integer

                'get certifications
                D = Html.IndexOf("<h5>Certification:</h5>")

                If D > 0 Then
                    W = Html.IndexOf("</div>", D)
                    Dim rCert As MatchCollection = Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN_3)

                    Dim Cert = From M As Match In rCert Select N = String.Format("{0}:{1}", M.Groups(1).ToString.Trim, M.Groups(2).ToString.Trim) Where N.Contains(Master.eSettings.CertificationLang)

                    If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then
                        If Cert.Count > 0 Then
                            IMDBMovie.Certification = Cert(0).ToString
                            If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" Then
                                IMDBMovie.MPAA = IMDBMovie.Certification
                            End If
                        End If
                    Else
                        IMDBMovie.Certification = Strings.Join(Cert.ToArray, " / ").Trim
                    End If
                End If

                'Get Release Date ( According to your country )
                Dim RelDate As Date
                Dim sRelDate As String = Regex.Match(Html, "\d+\s\w+\s\d\d\d\d\s").ToString.Trim
                If Not sRelDate = String.Empty Then
                    If Date.TryParse(sRelDate, RelDate) Then
                        IMDBMovie.ReleaseDate = Strings.FormatDateTime(RelDate, DateFormat.ShortDate).ToString
                    End If
                Else
                    IMDBMovie.ReleaseDate = Nothing
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(3)
                End If

                Dim RegexRating As String = Regex.Match(Html, "\b\d\W\d/\d\d").ToString
                If String.IsNullOrEmpty(RegexRating) Then
                    IMDBMovie.Rating = String.Empty
                Else
                    IMDBMovie.Rating = RegexRating.Split("/".ToCharArray)(0).Trim
                End If

                'trailer
                Dim sTrailerUrl As String = Regex.Match(Html, "href=""(.*?/video/imdb/vi.*?)""").Groups(1).Value.Trim
                If Not sTrailerUrl = String.Empty Then
                    Dim sTrailerURL2 As String = String.Empty
                    sTrailerUrl = String.Concat("http://", Master.eSettings.IMDBURL, sTrailerUrl, "player")
                    Dim HTTPTrailer As New HTTP(sTrailerUrl)
                    Dim HtmlTrailer As String = HTTPTrailer.Response
                    HTTPTrailer = Nothing

                    sTrailerUrl = Regex.Match(HtmlTrailer, "so.addVariable\(""id"", ""(.*?)""\);").Groups(1).Value.Trim
                    If sTrailerUrl = String.Empty Then
                        sTrailerURL2 = Regex.Match(HtmlTrailer, "so.addVariable\(""file"", ""(.*?)""\);").Groups(1).Value.Trim
                    Else
                        sTrailerURL2 = String.Concat(Regex.Match(HtmlTrailer, "so.addVariable\(""file"", ""(.*?)""\);").Groups(1).Value.Trim, sTrailerUrl)
                    End If
                    IMDBMovie.Trailer = Web.HttpUtility.UrlDecode(sTrailerURL2)
                End If

                IMDBMovie.Votes = Regex.Match(Html, "class=""tn15more"">([0-9,]+) votes</a>").Groups(1).Value.Trim

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(4)
                End If

                'Find all cast of the movie  
                'Match the table only 1 time
                Dim ActorsTable As String = Regex.Match(Html, TABLE_PATTERN).ToString

                Dim rCast As MatchCollection = Regex.Matches(ActorsTable, TR_PATTERN)

                Dim Cast1 = From M As Match In rCast _
                            Let m1 = Regex.Match(Regex.Match(M.ToString, TD_PATTERN_1).ToString, HREF_PATTERN) _
                            Let m2 = Regex.Match(M.ToString, TD_PATTERN_2).ToString _
                            Let m3 = Regex.Match(Regex.Match(M.ToString, TD_PATTERN_3).ToString, IMG_PATTERN) _
                            Select New Media.Person(Web.HttpUtility.HtmlDecode(m1.Groups("name").ToString.Trim), _
                            Web.HttpUtility.HtmlDecode(m2.ToString.Trim), _
                            If(Strings.InStr(m3.Groups("thumb").ToString, "addtiny") > 0, String.Empty, Strings.Replace(Web.HttpUtility.HtmlDecode(m3.Groups("thumb").ToString.Trim), _
                            "._SY30_SX23_.jpg", "._SY275_SX400_.jpg")))

                If Master.eSettings.CastImagesOnly Then
                    Cast1 = Cast1.Where(Function(p As Media.Person) (Not String.IsNullOrEmpty(p.Thumb)))
                End If

                Dim Cast As List(Of Media.Person) = Cast1.ToList

                'Clean up the actors list
                For Each Ps As Media.Person In Cast
                    Dim a_patterRegex = Regex.Match(Ps.Role, HREF_PATTERN)
                    If a_patterRegex.Success Then Ps.Role = a_patterRegex.Groups("name").ToString.Trim
                Next

                IMDBMovie.Actors = Cast

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(5)
                End If

                D = 0 : W = 0

                'get tagline
                D = Html.IndexOf("<h5>Tagline:</h5>")

                Dim lHtmlIndexOf As Integer = If(D > 0, Html.IndexOf("<a class=""tn15more inline""", D), 0)
                Dim TagLineEnd As Integer = If(lHtmlIndexOf > 0, lHtmlIndexOf, 0)
                If D > 0 Then W = If(TagLineEnd > 0, TagLineEnd, Html.IndexOf("</div>", D))

                IMDBMovie.Tagline = (If(D > 0 AndAlso W > 0, Web.HttpUtility.HtmlDecode(Html.Substring(D, W - D).Replace("<h5>Tagline:</h5>", String.Empty).Split(vbCrLf.ToCharArray)(1)).Trim, String.Empty))

                'Get the directors
                D = If(Html.IndexOf("<h5>Director:</h5>") > 0, Html.IndexOf("<h5>Director:</h5>"), Html.IndexOf("<h5>Directors:</h5>"))
                W = If(D > 0, Html.IndexOf("</div>", D), 0)
                'got any director(s) ?
                If D > 0 AndAlso Not W <= 0 Then
                    'get only the first director's name
                    Dim rDir As MatchCollection = Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN)
                    Dim Dir = From M As Match In rDir Where Not M.Groups("name").ToString.Contains("more") _
                              Select Web.HttpUtility.HtmlDecode(M.Groups("name").ToString)

                    If Dir.Count > 0 Then
                        IMDBMovie.Director = Strings.Join(Dir.ToArray, " / ").Trim
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(6)
                End If


                'Get genres of the movie
                If Not String.IsNullOrEmpty(ofdbGenre) Then
                    IMDBMovie.Genre = ofdbGenre
                Else
                    D = 0 : W = 0
                    D = Html.IndexOf("<h5>Genre:</h5>")
                    'Check if doesnt find genres
                    If D > 0 Then
                        W = Html.IndexOf("</div>", D)

                        If W > 0 Then
                            Dim rGenres As MatchCollection = Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN)

                            Dim Gen = From M As Match In rGenres _
                                      Select N = M.Groups("name").ToString Where Not N.Contains("more")
                            If Gen.Count > 0 Then
                                IMDBMovie.Genre = Strings.Join(Gen.ToArray, " / ").Trim
                            End If
                        End If
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(7)
                End If

                If String.IsNullOrEmpty(IMDBMovie.Outline) OrElse Not Master.eSettings.LockOutline Then

                    If Not String.IsNullOrEmpty(ofdbOutline) Then
                        IMDBMovie.Outline = ofdbOutline
                    Else
                        'Get the Plot Outline
                        D = 0 : W = 0

                        'Check if is a VideoGame
                        Try
                            If IMDBMovie.Title.Contains("(VG)") Then
                                D = If(Html.IndexOf("<h5>Plot Summary:</h5>") > 0, Html.IndexOf("<h5>Plot Summary:</h5>"), Html.IndexOf("<h5>Tagline:</h5>"))
                                If D > 0 Then W = Html.IndexOf("</div>", D)
                            Else
                                D = If(Html.IndexOf("<h5>Plot:</h5>") > 0, Html.IndexOf("<h5>Plot:</h5>"), Html.IndexOf("<h5>Plot Summary:</h5>"))
                                If D <= 0 Then D = Html.IndexOf("<h5>Plot Synopsis:</h5>")
                                If D > 0 Then
                                    W = Html.IndexOf("<a class=", D)
                                    If W > 0 Then
                                        W = Html.IndexOf("</div>", D)
                                    Else
                                        IMDBMovie.Outline = String.Empty
                                        GoTo mplot
                                    End If
                                Else
                                    IMDBMovie.Outline = String.Empty
                                    GoTo mPlot 'This plot synopsis is empty
                                End If
                            End If
                            Dim PlotOutline As String = Html.Substring(D, W - D).Remove(0, "<h5>Plot:</h5> ".Length)

                            PlotOutline = Web.HttpUtility.HtmlDecode(PlotOutline.Replace("|", String.Empty)).Trim
                            IMDBMovie.Outline = Regex.Replace(If(PlotOutline.Contains("is empty") OrElse PlotOutline.Contains("View full synopsis") _
                                               , String.Empty, PlotOutline), HREF_PATTERN, String.Empty).Trim
                        Catch ex As Exception
                            IMDBMovie.Outline = String.Empty
                        End Try
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(8)
                End If

mPlot:
                'Get the full Plot
                If String.IsNullOrEmpty(IMDBMovie.Plot) OrElse Not Master.eSettings.LockPlot Then
                    If Not String.IsNullOrEmpty(ofdbPlot) Then
                        IMDBMovie.Plot = ofdbPlot
                    Else
                        Dim FullPlot As String = Regex.Match(PlotHtml, "<p class=.plotpar.>(.*?)<i>", RegexOptions.Singleline Or RegexOptions.IgnoreCase Or RegexOptions.Multiline).Groups(1).Value
                        IMDBMovie.Plot = Web.HttpUtility.HtmlDecode(FullPlot.Replace("|", String.Empty)).Trim
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(9)
                End If


                'Get the movie duration
                IMDBMovie.Runtime = Regex.Match(Html, "<h5>Runtime:</h5>[^0-9]*([^<]*)").Groups(1).Value.Trim

                'Get Production Studio
                D = 0 : W = 0
                If FullCrew Then
                    D = Html.IndexOf("<b class=""blackcatheader"">Production Companies</b>")
                    If D > 0 Then W = Html.IndexOf("</ul>", D)
                    If D > 0 AndAlso W > 0 Then
                        'only get the first one
                        Dim Ps = From P1 As Match In Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN) _
                                 Where Not P1.Groups("name").ToString = String.Empty _
                                 Select Studio = P1.Groups("name").ToString Take 1
                        IMDBMovie.StudioReal = Ps(0).ToString.Trim
                    End If
                Else
                    D = Html.IndexOf("<h5>Company:</h5>")
                    If D > 0 Then W = Html.IndexOf("</div>", D)
                    If D > 0 AndAlso W > 0 Then
                        IMDBMovie.StudioReal = Regex.Match(Html.Substring(D, W - D), HREF_PATTERN).Groups("name").ToString.Trim
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(10)
                End If

                'Get Writers
                D = 0 : W = 0
                D = Html.IndexOf("<h5>Writer")
                If D > 0 Then W = Html.IndexOf("</div>", D)
                If D > 0 AndAlso W > 0 Then
                    Dim q = From M As Match In Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN) _
                            Where Not M.Groups("name").ToString = "more" _
                            AndAlso Not M.Groups("name").ToString = "(more)" _
                            AndAlso Not M.Groups("name").ToString = "(WGA)" _
                            Select Writer = M.Groups("name").ToString & If(FullCrew, " (writer)", String.Empty)

                    IMDBMovie.Credits = Strings.Join(q.ToArray, " / ").Trim
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(11)
                End If

                'Get All Other Info
                If FullCrew Then

                    D = 0 : W = 0
                    D = Html.IndexOf("Directed by</a></h5>")
                    If D > 0 Then W = Html.IndexOf("</body>", D)
                    If D > 0 AndAlso W > 0 Then
                        Dim qTables As MatchCollection = Regex.Matches(Html.Substring(D, W - D), TABLE_PATTERN)

                        For Each M As Match In qTables
                            'Producers
                            If M.ToString.Contains("Produced by</a></h5>") Then
                                Dim Pr = From Po In Regex.Matches(M.ToString, "<td\svalign=""top"">(.*?)</td>") _
                                Where Not Po.ToString.Contains(String.Concat("http://", Master.eSettings.IMDBURL, "/Glossary/")) _
                                Let P1 = Regex.Match(Po.ToString, HREF_PATTERN_2) _
                                Where Not P1.Groups("name").ToString = String.Empty _
                                Select Producer = P1.Groups("name").ToString & " (producer)"

                                If Pr.Count > 0 Then
                                    IMDBMovie.Credits = IMDBMovie.Credits & " / " & Strings.Join(Pr.ToArray, " / ").Trim
                                End If
                            End If

                            'Music by
                            If M.ToString.Contains("Original Music by</a></h5>") Then
                                Dim Mu = From Mo In Regex.Matches(M.ToString, "<td\svalign=""top"">(.*?)</td>") _
                                Let M1 = Regex.Match(Mo.ToString, HREF_PATTERN) _
                                Where Not M1.Groups("name").ToString = String.Empty _
                                Select Musician = M1.Groups("name").ToString & " (music by)"

                                If Mu.Count > 0 Then
                                    IMDBMovie.Credits = IMDBMovie.Credits & " / " & Strings.Join(Mu.ToArray, " / ").Trim
                                End If
                            End If

                        Next
                    End If

                    If bwIMDB.WorkerReportsProgress Then
                        bwIMDB.ReportProgress(12)
                    End If

                    'Special Effects
                    D = Html.IndexOf("<b class=""blackcatheader"">Special Effects</b>")
                    If D > 0 Then W = Html.IndexOf("</ul>", D)
                    If D > 0 AndAlso W > 0 Then
                        Dim Ps = From P1 As Match In Regex.Matches(Html.Substring(D, W - D), HREF_PATTERN) _
                                 Where Not P1.Groups("name").ToString = String.Empty _
                                 Select Studio = P1.Groups("name").ToString
                        If Ps.Count > 0 Then
                            IMDBMovie.Credits = IMDBMovie.Credits & " / " & Strings.Join(Ps.ToArray, " / ").Trim
                        End If
                    End If
                End If

                If bwIMDB.WorkerReportsProgress Then
                    bwIMDB.ReportProgress(13)
                End If

                Return True
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return False
            End Try
        End Function

        Private Sub BW_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwIMDB.DoWork
            Dim Args As Arguments = e.Argument

            Try
                Select Case Args.Search
                    Case SearchType.Movies
                        Dim r As MovieSearchResults = SearchMovie(Args.Parameter)
                        e.Result = New Results With {.ResultType = SearchType.Movies, .Result = r}
                    Case SearchType.Details
                        Dim s As Boolean = GetMovieInfo(Args.Parameter, Args.IMDBMovie, Args.FullCrew, Args.FullCast, False)
                        e.Result = New Results With {.ResultType = SearchType.Details, .Success = s}
                    Case SearchType.SearchDetails
                        Dim s As Boolean = GetMovieInfo(Args.Parameter, Args.IMDBMovie, False, False, True)
                        e.Result = New Results With {.ResultType = SearchType.SearchDetails, .Success = s}
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub bwIMDB_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwIMDB.ProgressChanged
            RaiseEvent ProgressUpdated(e.ProgressPercentage)
        End Sub

        Private Sub BW_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwIMDB.RunWorkerCompleted
            Dim Res As Results = e.Result

            Try
                Select Case Res.ResultType
                    Case SearchType.Movies
                        RaiseEvent SearchResultsDownloaded(Res.Result)
                    Case SearchType.Details
                        RaiseEvent MovieInfoDownloaded(Res.Success)
                    Case SearchType.SearchDetails
                        RaiseEvent SearchMovieInfoDownloaded(sPoster, Res.Success)
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

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

        Private Function CapitalizeAll(ByVal sString As String) As String
            Try
                Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sString)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return String.Empty
            End Try
        End Function
    End Class

End Namespace