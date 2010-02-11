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
Imports System.Text
Imports ICSharpCode.SharpZipLib.Zip

Namespace TVDB
    Public Class Scraper
        Private Const APIKey As String = "7B090234F418D074"
        Public Event SearchResultsDownloaded(ByVal mResults As List(Of TVSearchResults))
        Public Event ShowDownloaded()
        Friend WithEvents bwTVDB As New System.ComponentModel.BackgroundWorker

        Private Structure Results
            Dim Result As Object
            Dim Type As Integer '0 = search, 1 = show download
        End Structure

        Private Structure Arguments
            Dim Parameter As String
            Dim Type As Integer
        End Structure

        Public Function GetLangs() As List(Of TVDBLanguage)
            Dim tvdbLangs As New List(Of TVDBLanguage)
            Dim cLang As TVDBLanguage
            Dim xmlTVDB As XDocument
            Dim sHTTP As New HTTP

            Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/{1}/languages.xml", Master.eSettings.TVDBMirror, APIKey))
            sHTTP = Nothing

            If Not String.IsNullOrEmpty(apiXML) Then
                Try
                    xmlTVDB = XDocument.Parse(apiXML)
                Catch
                    Return tvdbLangs
                End Try

                Dim xLangs = From xLanguages In xmlTVDB.Descendants("Language")

                For Each xL As XElement In xLangs
                    cLang = New TVDBLanguage
                    cLang.LongLang = xL.Element("name").Value
                    cLang.ShortLang = xL.Element("abbreviation").Value
                    tvdbLangs.Add(cLang)
                Next
            End If
            Return tvdbLangs
        End Function

        Public Sub DownloadSeries(ByVal sID As String)
            Dim gURL As String = String.Format("http://{0}/api/{1}/series/{2}/all/{3}.zip", Master.eSettings.TVDBMirror, APIKey, sID, Master.eSettings.TVDBLanguage)

            Try
                If Not File.Exists(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, Master.eSettings.TVDBLanguage, ".zip"))) Then
                    Dim sHTTP As New HTTP
                    Dim xZip As Byte() = sHTTP.DownloadZip(gURL)
                    sHTTP = Nothing

                    If Not IsNothing(xZip) AndAlso xZip.Length > 0 Then
                        'save it to the temp dir
                        Directory.CreateDirectory(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID)))
                        Using fStream As FileStream = New FileStream(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, Master.eSettings.TVDBLanguage, ".zip")), FileMode.Create, FileAccess.Write)
                            fStream.Write(xZip, 0, xZip.Length)
                        End Using

                        Me.ProcessTVDBZip(xZip, gURL)
                    End If
                Else
                    Using fStream As FileStream = New FileStream(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, Master.eSettings.TVDBLanguage, ".zip")), FileMode.Open, FileAccess.Read)
                        Dim fZip As Byte() = Master.ReadStreamToEnd(fStream)
                        Me.ProcessTVDBZip(fZip, gURL)
                    End Using
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub ProcessTVDBZip(ByVal tvZip As Byte(), ByVal gURL As String)
            Dim sXML As String = String.Empty
            Dim bXML As String = String.Empty
            Dim aXML As String = String.Empty

            Try
                Using zStream As ZipInputStream = New ZipInputStream(New MemoryStream(tvZip))
                    Dim zEntry As ZipEntry = zStream.GetNextEntry

                    While Not IsNothing(zEntry)
                        Dim zBuffer As Byte() = Master.ReadStreamToEnd(zStream)

                        Select Case True
                            Case zEntry.Name.Equals(String.Concat(Master.eSettings.TVDBLanguage, ".xml"))
                                sXML = System.Text.Encoding.UTF8.GetString(zBuffer)
                            Case zEntry.Name.Equals("banners.xml")
                                bXML = System.Text.Encoding.UTF8.GetString(zBuffer)
                            Case zEntry.Name.Equals("actors.xml")
                                aXML = System.Text.Encoding.UTF8.GetString(zBuffer)
                        End Select

                        zEntry = zStream.GetNextEntry
                    End While
                End Using
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            ShowFromXML(sXML, aXML, bXML, gURL)
        End Sub

        Public Sub ShowFromXML(ByVal sXML As String, ByVal aXML As String, ByVal bXML As String, ByVal gURL As String)
            Dim Actors As New List(Of Media.Person)
            Dim sID As String = String.Empty
            Dim iEp As Integer = -1
            Dim iSeas As Integer = -1

            'get the actors first
            Try
                If Not String.IsNullOrEmpty(aXML) Then
                    Dim xdActors As XDocument = XDocument.Parse(aXML)
                    Dim xA = From xActor In xdActors.Descendants("Actor")
                    For Each Actor As XElement In xA
                        Actors.Add(New Media.Person With {.Name = Actor.Element("Name").Value, .Role = Actor.Element("Role").Value, .Thumb = If(IsNothing(Actor.Element("Image")) OrElse String.IsNullOrEmpty(Actor.Element("Image").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, Actor.Element("Image").Value))})
                    Next
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            'now let's get the show info and all the episodes
            Try
                If Not String.IsNullOrEmpty(sXML) Then
                    Dim xdShow As XDocument = XDocument.Parse(sXML)
                    Dim xS = From xShow In xdShow.Descendants("Series")
                    If xS.Count > 0 Then
                        With Master.tmpTVDBShow.Show.TVShow
                            sID = xS(0).Element("id").Value
                            .ID = sID
                            .Title = xS(0).Element("SeriesName").Value
                            .EpisodeGuideURL = gURL
                            .Genre = Strings.Join(xS(0).Element("Genre").Value.Split(Convert.ToChar("|")), " / ")
                            .MPAA = xS(0).Element("ContentRating").Value
                            .Plot = xS(0).Element("Overview").Value
                            .Premiered = xS(0).Element("FirstAired").Value
                            .Rating = xS(0).Element("Rating").Value
                            .Studio = xS(0).Element("Network").Value
                            .Actors = Actors
                        End With

                        For Each Episode As Master.DBTV In Master.tmpTVDBShow.Episodes

                            iEp = Episode.TVEp.Episode
                            iSeas = Episode.TVEp.Season

                            Episode.TVShow = Master.tmpTVDBShow.Show.TVShow

                            Dim xE As XElement = xdShow.Descendants("Episode").FirstOrDefault(Function(e) Convert.ToInt32(e.Element("EpisodeNumber").Value) = iEp AndAlso Convert.ToInt32(e.Element("SeasonNumber").Value) = iSeas)
                            If Not IsNothing(xE) Then
                                With Episode.TVEp
                                    .Title = xE.Element("EpisodeName").Value
                                    .Season = If(IsNothing(xE.Element("SeasonNumber")) OrElse String.IsNullOrEmpty(xE.Element("SeasonNumber").Value), 0, Convert.ToInt32(xE.Element("SeasonNumber").Value))
                                    .Episode = If(IsNothing(xE.Element("EpisodeNumber")) OrElse String.IsNullOrEmpty(xE.Element("EpisodeNumber").Value), 0, Convert.ToInt32(xE.Element("EpisodeNumber").Value))
                                    .Aired = xE.Element("FirstAired").Value
                                    .Rating = xE.Element("Rating").Value
                                    .Plot = xE.Element("Overview").Value
                                    .Director = xE.Element("Director").Value
                                    .Credits = CreditsString(xE.Element("GuestStars").Value, xE.Element("Writer").Value)
                                    .Actors = Actors
                                    .PosterURL = If(IsNothing(xE.Element("filename")) OrElse String.IsNullOrEmpty(xE.Element("filename").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, xE.Element("filename").Value))
                                    .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "episodeposters", Path.DirectorySeparatorChar, xE.Element("filename").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))
                                End With
                            End If
                        Next

                    End If
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            'and finally the images
            Try
                If Not String.IsNullOrEmpty(bXML) Then
                    Dim xdImage As XDocument = XDocument.Parse(bXML)
                    Dim xI = From xImage In xdImage.Descendants("Banner")
                    For Each tImage As XElement In xI
                        If Not IsNothing(tImage.Element("BannerPath")) AndAlso Not String.IsNullOrEmpty(tImage.Element("BannerPath").Value) Then
                            Select Case tImage.Element("BannerType").Value
                                Case "fanart"
                                    Master.tmpTVDBShow.Fanart.Add(New TVDBFanart With { _
                                                         .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                         .ThumbnailURL = If(IsNothing(tImage.Element("ThumbnailPath")) OrElse String.IsNullOrEmpty(tImage.Element("ThumbnailPath").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("ThumbnailPath").Value)), _
                                                         .Size = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), New Size With {.Width = 0, .Height = 0}, Master.StringToSize(tImage.Element("BannerType2").Value)), _
                                                         .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "fanart", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar))), _
                                                         .LocalThumb = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "fanart", Path.DirectorySeparatorChar, tImage.Element("ThumbnailPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                Case "poster"
                                    Master.tmpTVDBShow.Posters.Add(New TVDBPoster With { _
                                                          .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                          .Size = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), New Size With {.Width = 0, .Height = 0}, Master.StringToSize(tImage.Element("BannerType2").Value)), _
                                                          .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "posters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                Case "season"
                                    Master.tmpTVDBShow.SeasonPosters.Add(New TVDBSeasonPoster With { _
                                                            .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                            .Season = If(IsNothing(tImage.Element("Season")) OrElse String.IsNullOrEmpty(tImage.Element("Season").Value), 0, Convert.ToInt32(tImage.Element("Season").Value)), _
                                                            .Type = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), SeasonPosterType.None, StringToSeasonPosterType(tImage.Element("BannerType2").Value)), _
                                                            .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "seasonposters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                Case "series"
                                    Master.tmpTVDBShow.ShowPosters.Add(New TVDBShowPoster With { _
                                                          .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                          .Type = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), ShowPosterType.None, StringToShowPosterType(tImage.Element("BannerType2").Value)), _
                                                          .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "seriesposters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                            End Select
                        End If
                    Next
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Public Sub SaveToCache(ByVal sID As String, ByVal sURL As String, ByVal sPath As String)
            Dim sHTTP As New HTTP
            Dim sImage As New Images

            sImage.Image = sHTTP.DownloadImage(sURL)

            If Not IsNothing(sImage.Image) Then
                sImage.Save(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, sPath.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar))))
            End If

            sImage = Nothing
            sHTTP = Nothing
        End Sub

        Public Function CreditsString(ByVal sGStars As String, ByVal sWriters As String) As String
            Dim cString As New List(Of String)
            Dim gString As String = Master.eLang.GetString(999, "Guest Star")
            Dim wString As String = Master.eLang.GetString(999, "Writer")

            If Not String.IsNullOrEmpty(sGStars) Then
                For Each gStar In sGStars.Split(Convert.ToChar("|"))
                    cString.Add(String.Concat(gStar, String.Format(" ({0})", gString)))
                Next
            End If

            If Not String.IsNullOrEmpty(sWriters) Then
                For Each Writer In sWriters.Split(Convert.ToChar("|"))
                    cString.Add(String.Concat(Writer, String.Format(" ({0})", wString)))
                Next
            End If

            Return Strings.Join(cString.ToArray, " / ")
        End Function

        Public Sub DownloadSeriesAsync(ByVal iID As Integer)
            Try
                If Not bwTVDB.IsBusy Then
                    bwTVDB.WorkerReportsProgress = False
                    bwTVDB.WorkerSupportsCancellation = True
                    bwTVDB.RunWorkerAsync(New Arguments With {.Type = 1, .Parameter = iID.ToString})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub GetSearchResultsAsync(ByVal sName As String)
            Try
                If Not bwTVDB.IsBusy Then
                    bwTVDB.WorkerReportsProgress = False
                    bwTVDB.WorkerSupportsCancellation = True
                    bwTVDB.RunWorkerAsync(New Arguments With {.Type = 0, .Parameter = sName})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function SearchSeries(ByVal sName As String) As List(Of TVSearchResults)
            Dim tvdbResults As New List(Of TVSearchResults)
            Dim cResult As New TVSearchResults
            Dim xmlTVDB As XDocument
            Dim sHTTP As New HTTP
            Dim sLang As String = String.Empty

            Try
                Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/GetSeries.php?seriesname={1}&language={2}", Master.eSettings.TVDBMirror, sName, Master.eSettings.TVDBLanguage))
                sHTTP = Nothing

                If Not String.IsNullOrEmpty(apiXML) Then
                    Try
                        xmlTVDB = XDocument.Parse(apiXML)
                    Catch
                        Return tvdbResults
                    End Try

                    Dim xSer = From xSeries In xmlTVDB.Descendants("Series") Where xSeries.HasElements

                    For Each xS As XElement In xSer
                        cResult = New TVDB.TVSearchResults
                        cResult.ID = Convert.ToInt32(xS.Element("seriesid").Value)
                        cResult.Name = If(Not IsNothing(xS.Element("SeriesName")), xS.Element("SeriesName").Value, String.Empty)
                        If Not IsNothing(xS.Element("language")) Then
                            sLang = xS.Element("language").Value
                            cResult.Language = Master.eSettings.TVDBLanguages.FirstOrDefault(Function(s) s.ShortLang = sLang)
                        End If
                        cResult.Aired = If(Not IsNothing(xS.Element("FirstAired")), xS.Element("FirstAired").Value, String.Empty)
                        cResult.Overview = If(Not IsNothing(xS.Element("Overview")), xS.Element("Overview").Value, String.Empty)
                        cResult.Banner = If(Not IsNothing(xS.Element("banner")), xS.Element("banner").Value, String.Empty)
                        tvdbResults.Add(cResult)
                    Next
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return tvdbResults
        End Function

        Public Sub CancelAsync()
            If bwTVDB.IsBusy Then bwTVDB.CancelAsync()

            While bwTVDB.IsBusy
                Application.DoEvents()
            End While
        End Sub

        Private Sub bwtvDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTVDB.DoWork
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)

            Try
                Select Case Args.Type
                    Case 0 'search
                        e.Result = New Results With {.Type = Args.Type, .Result = SearchSeries(Args.Parameter)}
                    Case 1 'show download
                        Me.DownloadSeries(Args.Parameter)
                        e.Result = New Results With {.Type = Args.Type}
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub bwTVDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTVDB.RunWorkerCompleted
            Dim Res As Results = DirectCast(e.Result, Results)

            Try
                Select Case Res.Type
                    Case 0 'search
                        RaiseEvent SearchResultsDownloaded(DirectCast(Res.Result, List(Of TVSearchResults)))
                    Case 1 'show download
                        RaiseEvent ShowDownloaded()
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Public Function StringToSeasonPosterType(ByVal sType As String) As SeasonPosterType
            Select Case sType.ToLower
                Case "season"
                    Return SeasonPosterType.Poster
                Case "seasonwide"
                    Return SeasonPosterType.Wide
                Case Else
                    Return SeasonPosterType.None
            End Select
        End Function

        Public Function StringToShowPosterType(ByVal sType As String) As ShowPosterType
            Select Case sType.ToLower
                Case "blank"
                    Return ShowPosterType.Blank
                Case "graphical"
                    Return ShowPosterType.Graphical
                Case "text"
                    Return ShowPosterType.Text
                Case Else
                    Return ShowPosterType.None
            End Select
        End Function
    End Class

    Public Class TVDBShow
        Private _show As Master.DBTV
        Private _episodes As New List(Of Master.DBTV)
        Private _fanart As New List(Of TVDBFanart)
        Private _showposters As New List(Of TVDBShowPoster)
        Private _seasonposters As New List(Of TVDBSeasonPoster)
        Private _posters As New List(Of TVDBPoster)

        Public Property Show() As Master.DBTV
            Get
                Return Me._show
            End Get
            Set(ByVal value As Master.DBTV)
                Me._show = value
            End Set
        End Property

        Public Property Episodes() As List(Of Master.DBTV)
            Get
                Return Me._episodes
            End Get
            Set(ByVal value As List(Of Master.DBTV))
                Me._episodes = value
            End Set
        End Property

        Public Property Fanart() As List(Of TVDBFanart)
            Get
                Return Me._fanart
            End Get
            Set(ByVal value As List(Of TVDBFanart))
                Me._fanart = value
            End Set
        End Property

        Public Property ShowPosters() As List(Of TVDBShowPoster)
            Get
                Return Me._showposters
            End Get
            Set(ByVal value As List(Of TVDBShowPoster))
                Me._showposters = value
            End Set
        End Property

        Public Property SeasonPosters() As List(Of TVDBSeasonPoster)
            Get
                Return Me._seasonposters
            End Get
            Set(ByVal value As List(Of TVDBSeasonPoster))
                Me._seasonposters = value
            End Set
        End Property

        Public Property Posters() As List(Of TVDBPoster)
            Get
                Return Me._posters
            End Get
            Set(ByVal value As List(Of TVDBPoster))
                Me._posters = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._show = New Master.DBTV
            Me._episodes = New List(Of Master.DBTV)
            Me._fanart = New List(Of TVDBFanart)
            Me._showposters = New List(Of TVDBShowPoster)
            Me._seasonposters = New List(Of TVDBSeasonPoster)
            Me._posters = New List(Of TVDBPoster)
        End Sub
    End Class

    Public Class TVDBLanguage
        Private _longlang As String
        Private _shortlang As String

        Public Property LongLang() As String
            Get
                Return Me._longlang
            End Get
            Set(ByVal value As String)
                Me._longlang = value
            End Set
        End Property

        Public Property ShortLang() As String
            Get
                Return Me._shortlang
            End Get
            Set(ByVal value As String)
                Me._shortlang = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._longlang = String.Empty
            Me._shortlang = String.Empty
        End Sub
    End Class

    Public Class TVSearchResults
        Private _id As Integer
        Private _name As String
        Private _aired As String
        Private _language As TVDBLanguage
        Private _overview As String
        Private _banner As String

        Public Property ID() As Integer
            Get
                Return Me._id
            End Get
            Set(ByVal value As Integer)
                Me._id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
            End Set
        End Property

        Public Property Aired() As String
            Get
                Return Me._aired
            End Get
            Set(ByVal value As String)
                Me._aired = value
            End Set
        End Property

        Public Property Language() As TVDBLanguage
            Get
                Return Me._language
            End Get
            Set(ByVal value As TVDBLanguage)
                Me._language = value
            End Set
        End Property

        Public Property Overview() As String
            Get
                Return Me._overview
            End Get
            Set(ByVal value As String)
                Me._overview = value
            End Set
        End Property

        Public Property Banner() As String
            Get
                Return Me._banner
            End Get
            Set(ByVal value As String)
                Me._banner = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._id = 0
            Me._name = String.Empty
            Me._aired = String.Empty
            Me._language = New TVDBLanguage
            Me._overview = String.Empty
            Me._banner = String.Empty
        End Sub
    End Class

    Public Class TVDBFanart
        Private _url As String
        Private _thumbnailurl As String
        Private _size As Size
        Private _localfile As String
        Private _localthumb As String
        Private _image As Images

        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property ThumbnailURL() As String
            Get
                Return Me._thumbnailurl
            End Get
            Set(ByVal value As String)
                Me._thumbnailurl = value
            End Set
        End Property

        Public Property Size() As Size
            Get
                Return Me._size
            End Get
            Set(ByVal value As Size)
                Me._size = value
            End Set
        End Property

        Public Property LocalFile() As String
            Get
                Return Me._localfile
            End Get
            Set(ByVal value As String)
                Me._localfile = value
            End Set
        End Property

        Public Property LocalThumb() As String
            Get
                Return Me._localthumb
            End Get
            Set(ByVal value As String)
                Me._localthumb = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._thumbnailurl = String.Empty
            Me._size = New Size
            Me._localfile = String.Empty
            Me._localthumb = String.Empty
            Me._image = New Images
        End Sub
    End Class

    Public Enum ShowPosterType As Integer
        None = 0
        Blank = 1
        Graphical = 2
        Text = 3
    End Enum

    Public Class TVDBShowPoster
        Private _url As String
        Private _type As ShowPosterType
        Private _localfile As String
        Private _image As Images

        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property Type() As ShowPosterType
            Get
                Return Me._type
            End Get
            Set(ByVal value As ShowPosterType)
                Me._type = value
            End Set
        End Property

        Public Property LocalFile() As String
            Get
                Return Me._localfile
            End Get
            Set(ByVal value As String)
                Me._localfile = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._type = ShowPosterType.None
            Me._localfile = String.Empty
            Me._image = New Images
        End Sub
    End Class

    Public Enum SeasonPosterType As Integer
        None = 0
        Poster = 1
        Wide = 2
    End Enum

    Public Class TVDBSeasonPoster
        Private _url As String
        Private _season As Integer
        Private _type As SeasonPosterType
        Private _localfile As String
        Private _image As Images

        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        Public Property Type() As SeasonPosterType
            Get
                Return Me._type
            End Get
            Set(ByVal value As SeasonPosterType)
                Me._type = value
            End Set
        End Property

        Public Property LocalFile() As String
            Get
                Return Me._localfile
            End Get
            Set(ByVal value As String)
                Me._localfile = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._season = 0
            Me._type = SeasonPosterType.None
            Me._localfile = String.Empty
            Me._image = New Images
        End Sub
    End Class

    Public Enum PosterType As Integer
        None = 0
    End Enum

    Public Class TVDBPoster
        Private _url As String
        Private _size As Size
        Private _localfile As String
        Private _image As Images

        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property Size() As Size
            Get
                Return Me._size
            End Get
            Set(ByVal value As Size)
                Me._size = value
            End Set
        End Property

        Public Property LocalFile() As String
            Get
                Return Me._localfile
            End Get
            Set(ByVal value As String)
                Me._localfile = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._size = New Size
            Me._localfile = String.Empty
            Me._image = New Images
        End Sub
    End Class

End Namespace

