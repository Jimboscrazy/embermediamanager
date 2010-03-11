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
Imports System.Data

Public Class Scraper
    Public Const APIKey As String = "7B090234F418D074"
    Public Shared TVDBImages As New TVImages
    Public Shared tmpTVDBShow As New TVDBShow
    Public Shared tEpisodes As New List(Of MediaContainers.EpisodeDetails)

    Public Event ScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
    Public Shared WithEvents sObject As New ScraperObject

    <Serializable()> _
    Public Structure TVImages
        Dim ShowPoster As TVDBShowPoster
        Dim ShowFanart As TVDBFanart
        Dim AllSeasonPoster As TVDBShowPoster
        Dim SeasonImageList As List(Of TVDBSeasonImage)

        Public Function Clone() As TVImages
            Dim newTVI As New TVImages
            Try
                Using ms As New IO.MemoryStream()
                    Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter()
                    bf.Serialize(ms, Me)
                    ms.Position = 0
                    newTVI = DirectCast(bf.Deserialize(ms), TVImages)
                    ms.Close()
                End Using
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
            Return newTVI
        End Function
    End Structure

    Public Sub New()
        AddHandler sObject.ScraperEvent, AddressOf InnerEvent
    End Sub

    Public Sub InnerEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        RaiseEvent ScraperEvent(eType, iProgress, Parameter)
    End Sub

    Public Class TVDBShow
        Private _show As Structures.DBTV
        Private _allseason As Structures.DBTV
        Private _episodes As New List(Of Structures.DBTV)
        Private _fanart As New List(Of TVDBFanart)
        Private _showposters As New List(Of TVDBShowPoster)
        Private _seasonposters As New List(Of TVDBSeasonPoster)
        Private _posters As New List(Of TVDBPoster)

        Public Property Show() As Structures.DBTV
            Get
                Return Me._show
            End Get
            Set(ByVal value As Structures.DBTV)
                Me._show = value
            End Set
        End Property

        Public Property AllSeason() As Structures.DBTV
            Get
                Return Me._allseason
            End Get
            Set(ByVal value As Structures.DBTV)
                Me._allseason = value
            End Set
        End Property

        Public Property Episodes() As List(Of Structures.DBTV)
            Get
                Return Me._episodes
            End Get
            Set(ByVal value As List(Of Structures.DBTV))
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
            Me._show = New Structures.DBTV
            Me._allseason = New Structures.DBTV
            Me._episodes = New List(Of Structures.DBTV)
            Me._fanart = New List(Of TVDBFanart)
            Me._showposters = New List(Of TVDBShowPoster)
            Me._seasonposters = New List(Of TVDBSeasonPoster)
            Me._posters = New List(Of TVDBPoster)
        End Sub
    End Class

    Public Class TVSearchResults
        Private _id As Integer
        Private _name As String
        Private _aired As String
        Private _language As Containers.TVLanguage
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

        Public Property Language() As Containers.TVLanguage
            Get
                Return Me._language
            End Get
            Set(ByVal value As Containers.TVLanguage)
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
            Me._language = New Containers.TVLanguage
            Me._overview = String.Empty
            Me._banner = String.Empty
        End Sub
    End Class

    <Serializable()> _
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

    <Serializable()> _
    Public Class TVDBShowPoster
        Private _url As String
        Private _type As Enums.ShowPosterType
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

        Public Property Type() As Enums.ShowPosterType
            Get
                Return Me._type
            End Get
            Set(ByVal value As Enums.ShowPosterType)
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
            Me._type = Enums.ShowPosterType.None
            Me._localfile = String.Empty
            Me._image = New Images
        End Sub
    End Class

    Public Class TVDBSeasonPoster
        Private _url As String
        Private _season As Integer
        Private _type As Enums.SeasonPosterType
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

        Public Property Type() As Enums.SeasonPosterType
            Get
                Return Me._type
            End Get
            Set(ByVal value As Enums.SeasonPosterType)
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
            Me._type = Enums.SeasonPosterType.None
            Me._localfile = String.Empty
            Me._image = New Images
        End Sub
    End Class

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

    <Serializable()> _
    Public Class TVDBSeasonImage
        Private _season As Integer
        Private _poster As Images
        Private _fanart As TVDBFanart

        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        Public Property Poster() As Images
            Get
                Return Me._poster
            End Get
            Set(ByVal value As Images)
                Me._poster = value
            End Set
        End Property

        Public Property Fanart() As TVDBFanart
            Get
                Return Me._fanart
            End Get
            Set(ByVal value As TVDBFanart)
                Me._fanart = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._season = -1
            Me._poster = New Images
            Me._fanart = New TVDBFanart
        End Sub
    End Class

    Public Function GetLangs(ByVal sMirror As String) As List(Of Containers.TVLanguage)
        Dim tvdbLangs As New List(Of Containers.TVLanguage)
        Dim cLang As Containers.TVLanguage
        Dim xmlTVDB As XDocument
        Dim sHTTP As New HTTP

        Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/{1}/languages.xml", sMirror, APIKey))
        sHTTP = Nothing

        If Not String.IsNullOrEmpty(apiXML) Then
            Try
                xmlTVDB = XDocument.Parse(apiXML)
            Catch
                Return tvdbLangs
            End Try

            Dim xLangs = From xLanguages In xmlTVDB.Descendants("Language")

            For Each xL As XElement In xLangs
                cLang = New Containers.TVLanguage
                cLang.LongLang = xL.Element("name").Value
                cLang.ShortLang = xL.Element("abbreviation").Value
                tvdbLangs.Add(cLang)
            Next
        End If
        Return tvdbLangs
    End Function

    Public Sub SingleScrape(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions, ByVal WithCurrent As Boolean)
        sObject.SingleScrape(New Structures.ScrapeInfo With {.ShowID = ShowID, .ShowTitle = ShowTitle, .TVDBID = TVDBID, .SelectedLang = Lang, .Options = Options, .WithCurrent = WithCurrent, .iSeason = -999})
    End Sub

    Public Sub ScrapeEpisode(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iEpisode As Integer, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions)
        sObject.ScrapeEpisode(New Structures.ScrapeInfo With {.ShowID = ShowID, .ShowTitle = ShowTitle, .TVDBID = TVDBID, .iEpisode = iEpisode, .iSeason = iSeason, .SelectedLang = Lang, .Options = Options})
    End Sub

    Public Sub ScrapeSeason(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions)
        sObject.ScrapeSeason(New Structures.ScrapeInfo With {.ShowID = ShowID, .ShowTitle = ShowTitle, .TVDBID = TVDBID, .iSeason = iSeason, .SelectedLang = Lang, .Options = Options})
    End Sub

    Public Function ChangeEpisode(ByVal ShowID As Integer, ByVal TVDBID As String) As MediaContainers.EpisodeDetails
        Return sObject.ChangeEpisode(New Structures.ScrapeInfo With {.ShowID = ShowID, .TVDBID = TVDBID})
    End Function

    Public Function GetSingleEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Season As Integer, ByVal Episode As Integer, ByVal Options As Structures.TVScrapeOptions) As MediaContainers.EpisodeDetails
        Return sObject.GetSingleEpisode(New Structures.ScrapeInfo With {.ShowID = ShowID, .TVDBID = TVDBID, .iSeason = Season, .iEpisode = Episode, .Options = Options})
    End Function

    Public Function GetSingleImage(ByVal Title As String, ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Type As Enums.TVImageType, ByVal Season As Integer, ByVal Episode As Integer, ByVal Lang As String, ByVal CurrentImage As Image) As Image
        Return sObject.GetSingleImage(New Structures.ScrapeInfo With {.ShowTitle = Title, .ShowID = ShowID, .TVDBID = TVDBID, .ImageType = Type, .iSeason = Season, .iEpisode = Episode, .SelectedLang = Lang, .CurrentImage = CurrentImage})
    End Function

    Public Sub CancelAsync()
        sObject.CancelAsync()
    End Sub

    Public Function IsBusy() As Boolean
        Return sObject.IsBusy
    End Function

    Public Sub SaveImages()
        sObject.SaveImages()
    End Sub

    Public Class ScraperObject
        Private sXML As String = String.Empty
        Private bXML As String = String.Empty
        Private aXML As String = String.Empty

        Public Event ScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)

        Friend WithEvents bwTVDB As New System.ComponentModel.BackgroundWorker

        Private Structure Results
            Dim Result As Object
            Dim Type As Integer '0 = search, 1 = show download, 2 = load eps, 3 = save
        End Structure

        Private Structure Arguments
            Dim Parameter As Object
            Dim Type As Integer
        End Structure

        Public Sub SaveImages()
            RaiseEvent ScraperEvent(Enums.TVScraperEventType.SavingStarted, 0, Nothing)
            Me.bwTVDB = New System.ComponentModel.BackgroundWorker
            Me.bwTVDB.WorkerReportsProgress = True
            Me.bwTVDB.WorkerSupportsCancellation = True
            Me.bwTVDB.RunWorkerAsync(New Arguments With {.Type = 3})
        End Sub

        Public Sub PassEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
            RaiseEvent ScraperEvent(eType, iProgress, Parameter)
        End Sub

        Public Sub DownloadSeries(ByVal sInfo As Structures.ScrapeInfo, Optional ByVal ImagesOnly As Boolean = False)
            Try
                Dim fPath As String = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sInfo.TVDBID, Path.DirectorySeparatorChar, sInfo.SelectedLang, ".zip"))
                Dim fExists As Boolean = File.Exists(fPath)
                Dim doDownload As Boolean = False

                Select Case Master.eSettings.TVUpdateTime
                    Case Enums.TVUpdateTime.Always
                        doDownload = True
                    Case Enums.TVUpdateTime.Never
                        doDownload = False
                    Case Enums.TVUpdateTime.Week
                        If fExists AndAlso File.GetCreationTime(fPath).AddDays(7) < Now Then doDownload = True
                    Case Enums.TVUpdateTime.BiWeekly
                        If fExists AndAlso File.GetCreationTime(fPath).AddDays(14) < Now Then doDownload = True
                    Case Enums.TVUpdateTime.Month
                        If fExists AndAlso File.GetCreationTime(fPath).AddMonths(1) < Now Then doDownload = True
                End Select

                If doDownload OrElse Not fExists Then
                    Dim sHTTP As New HTTP
                    Dim xZip As Byte() = sHTTP.DownloadZip(String.Format("http://{0}/api/{1}/series/{2}/all/{3}.zip", Master.eSettings.TVDBMirror, APIKey, sInfo.TVDBID, sInfo.SelectedLang))
                    sHTTP = Nothing

                    If Not IsNothing(xZip) AndAlso xZip.Length > 0 Then
                        'save it to the temp dir
                        Directory.CreateDirectory(Directory.GetParent(fPath).FullName)
                        Using fStream As FileStream = New FileStream(fPath, FileMode.Create, FileAccess.Write)
                            fStream.Write(xZip, 0, xZip.Length)
                        End Using

                        Me.ProcessTVDBZip(xZip, sInfo)
                        Me.ShowFromXML(sInfo, ImagesOnly)
                    End If
                Else
                    Using fStream As FileStream = New FileStream(fPath, FileMode.Open, FileAccess.Read)
                        Dim fZip As Byte() = Functions.ReadStreamToEnd(fStream)

                        Me.ProcessTVDBZip(fZip, sInfo)
                        Me.ShowFromXML(sInfo, ImagesOnly)
                    End Using
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub ProcessTVDBZip(ByVal tvZip As Byte(), ByVal sInfo As Structures.ScrapeInfo)
            sXML = String.Empty
            bXML = String.Empty
            aXML = String.Empty

            Try
                Using zStream As ZipInputStream = New ZipInputStream(New MemoryStream(tvZip))
                    Dim zEntry As ZipEntry = zStream.GetNextEntry

                    While Not IsNothing(zEntry)
                        Dim zBuffer As Byte() = Functions.ReadStreamToEnd(zStream)

                        Select Case True
                            Case zEntry.Name.Equals(String.Concat(sInfo.SelectedLang, ".xml"))
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

        End Sub

        Private Sub ShowFromXML(ByVal sInfo As Structures.ScrapeInfo, ByVal ImagesOnly As Boolean)
            Dim Actors As New List(Of MediaContainers.Person)
            Dim sID As String = String.Empty
            Dim iEp As Integer = -1
            Dim iSeas As Integer = -1
            Dim sTitle As String = String.Empty
            Dim byTitle As Boolean = False
            Dim xE As XElement = Nothing
            Dim tShow As Structures.DBTV = tmpTVDBShow.Show

            If Not ImagesOnly Then
                If Master.eSettings.DisplayMissingEpisodes Then tEpisodes = Me.GetListOfKnownEpisodes(sInfo)

                'get the actors first
                Try
                    If sInfo.Options.bShowActors OrElse sInfo.Options.bEpActors Then
                        If Not String.IsNullOrEmpty(aXML) Then
                            Dim xdActors As XDocument = XDocument.Parse(aXML)
                            For Each Actor As XElement In xdActors.Descendants("Actor")
                                Actors.Add(New MediaContainers.Person With {.Name = Actor.Element("Name").Value, .Role = Actor.Element("Role").Value, .Thumb = If(IsNothing(Actor.Element("Image")) OrElse String.IsNullOrEmpty(Actor.Element("Image").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, Actor.Element("Image").Value))})
                            Next
                        End If
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
                            tShow.ShowLanguage = sInfo.SelectedLang
                            If Not IsNothing(tShow.TVShow) Then
                                With tShow.TVShow
                                    sID = xS(0).Element("id").Value
                                    .ID = sID
                                    If sInfo.Options.bShowTitle AndAlso (String.IsNullOrEmpty(.Title) OrElse Not Master.eSettings.ShowLockTitle) Then .Title = xS(0).Element("SeriesName").Value
                                    If sInfo.Options.bShowEpisodeGuide Then .EpisodeGuideURL = If(Not String.IsNullOrEmpty(Master.eSettings.ExternalTVDBAPIKey), String.Format("http://{0}/api/{1}/series/{2}/all/{3}.zip", Master.eSettings.TVDBMirror, Master.eSettings.ExternalTVDBAPIKey, sID, Master.eSettings.TVDBLanguage), String.Empty)
                                    If sInfo.Options.bShowGenre AndAlso (String.IsNullOrEmpty(.Genre) OrElse Not Master.eSettings.ShowLockGenre) Then .Genre = Strings.Join(xS(0).Element("Genre").Value.Split(Convert.ToChar("|")), " / ")
                                    If sInfo.Options.bShowMPAA Then .MPAA = xS(0).Element("ContentRating").Value
                                    If sInfo.Options.bShowPlot AndAlso (String.IsNullOrEmpty(.Plot) OrElse Not Master.eSettings.ShowLockPlot) Then .Plot = xS(0).Element("Overview").Value
                                    If sInfo.Options.bShowPremiered Then .Premiered = xS(0).Element("FirstAired").Value
                                    If sInfo.Options.bShowRating AndAlso (String.IsNullOrEmpty(.Rating) OrElse Not Master.eSettings.ShowLockRating) Then .Rating = xS(0).Element("Rating").Value
                                    If sInfo.Options.bShowStudio AndAlso (String.IsNullOrEmpty(.Studio) OrElse Not Master.eSettings.ShowLockStudio) Then .Studio = xS(0).Element("Network").Value
                                    If sInfo.Options.bShowActors Then .Actors = Actors
                                End With
                            End If

                            'set it back
                            tmpTVDBShow.Show = tShow

                            For Each Episode As Structures.DBTV In tmpTVDBShow.Episodes

                                Episode.ShowLanguage = sInfo.SelectedLang

                                iEp = Episode.TVEp.Episode
                                iSeas = Episode.TVEp.Season
                                sTitle = Episode.TVEp.Title
                                byTitle = False

                                If Not IsNothing(tShow.TVShow) Then Episode.TVShow = tShow.TVShow

                                xE = xdShow.Descendants("Episode").FirstOrDefault(Function(e) Convert.ToInt32(e.Element("EpisodeNumber").Value) = iEp AndAlso Convert.ToInt32(e.Element("SeasonNumber").Value) = iSeas)

                                If IsNothing(xE) Then
                                    xE = xdShow.Descendants("Episode").FirstOrDefault(Function(e) StringUtils.ComputeLevenshtein(e.Element("EpisodeName").Value, sTitle) < 5)
                                    byTitle = True
                                End If

                                If Not IsNothing(xE) Then
                                    With Episode.TVEp
                                        If sInfo.Options.bEpTitle AndAlso (String.IsNullOrEmpty(.Title) OrElse Not Master.eSettings.EpLockTitle) Then .Title = xE.Element("EpisodeName").Value
                                        If byTitle Then
                                            If sInfo.Options.bEpSeason Then .Season = If(IsNothing(xE.Element("SeasonNumber")) OrElse String.IsNullOrEmpty(xE.Element("SeasonNumber").Value), 0, Convert.ToInt32(xE.Element("SeasonNumber").Value))
                                            If sInfo.Options.bEpEpisode Then .Episode = If(IsNothing(xE.Element("EpisodeNumber")) OrElse String.IsNullOrEmpty(xE.Element("EpisodeNumber").Value), 0, Convert.ToInt32(xE.Element("EpisodeNumber").Value))
                                        End If
                                        If sInfo.Options.bEpAired Then .Aired = xE.Element("FirstAired").Value
                                        If sInfo.Options.bEpRating AndAlso (String.IsNullOrEmpty(.Rating) OrElse Not Master.eSettings.EpLockRating) Then .Rating = xE.Element("Rating").Value
                                        If sInfo.Options.bEpPlot AndAlso (String.IsNullOrEmpty(.Plot) OrElse Not Master.eSettings.EpLockPlot) Then .Plot = xE.Element("Overview").Value
                                        If sInfo.Options.bEpDirector Then .Director = xE.Element("Director").Value
                                        If sInfo.Options.bEpCredits Then .Credits = CreditsString(xE.Element("GuestStars").Value, xE.Element("Writer").Value)
                                        If sInfo.Options.bEpActors Then .Actors = Actors
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
            Else
                sID = sInfo.TVDBID
            End If
            'and finally the images
            Try
                If ImagesOnly OrElse Not IsNothing(tShow.TVShow) Then
                    If Not String.IsNullOrEmpty(bXML) Then
                        Dim xdImage As XDocument = XDocument.Parse(bXML)
                        For Each tImage As XElement In xdImage.Descendants("Banner")
                            If (Not IsNothing(tImage.Element("BannerPath")) AndAlso Not String.IsNullOrEmpty(tImage.Element("BannerPath").Value)) AndAlso _
                               (Not Master.eSettings.OnlyGetTVImagesForSelectedLanguage OrElse ((Not IsNothing(tImage.Element("Language")) AndAlso tImage.Element("Language").Value = Master.eSettings.TVDBLanguage) OrElse _
                               ((IsNothing(tImage.Element("Language")) OrElse tImage.Element("Language").Value = "en") AndAlso Master.eSettings.AlwaysGetEnglishTVImages))) Then
                                Select Case tImage.Element("BannerType").Value
                                    Case "fanart"
                                        tmpTVDBShow.Fanart.Add(New TVDBFanart With { _
                                                             .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                             .ThumbnailURL = If(IsNothing(tImage.Element("ThumbnailPath")) OrElse String.IsNullOrEmpty(tImage.Element("ThumbnailPath").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("ThumbnailPath").Value)), _
                                                             .Size = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), New Size With {.Width = 0, .Height = 0}, StringUtils.StringToSize(tImage.Element("BannerType2").Value)), _
                                                             .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "fanart", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar))), _
                                                             .LocalThumb = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "fanart", Path.DirectorySeparatorChar, tImage.Element("ThumbnailPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                    Case "poster"
                                        tmpTVDBShow.Posters.Add(New TVDBPoster With { _
                                                              .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                              .Size = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), New Size With {.Width = 0, .Height = 0}, StringUtils.StringToSize(tImage.Element("BannerType2").Value)), _
                                                              .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "posters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                    Case "season"
                                        tmpTVDBShow.SeasonPosters.Add(New TVDBSeasonPoster With { _
                                                                .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                                .Season = If(IsNothing(tImage.Element("Season")) OrElse String.IsNullOrEmpty(tImage.Element("Season").Value), 0, Convert.ToInt32(tImage.Element("Season").Value)), _
                                                                .Type = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), Enums.SeasonPosterType.None, StringToSeasonPosterType(tImage.Element("BannerType2").Value)), _
                                                                .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "seasonposters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                    Case "series"
                                        tmpTVDBShow.ShowPosters.Add(New TVDBShowPoster With { _
                                                              .URL = String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, tImage.Element("BannerPath").Value), _
                                                              .Type = If(IsNothing(tImage.Element("BannerType2")) OrElse String.IsNullOrEmpty(tImage.Element("BannerType2").Value), Enums.ShowPosterType.None, StringToShowPosterType(tImage.Element("BannerType2").Value)), _
                                                              .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, "seriesposters", Path.DirectorySeparatorChar, tImage.Element("BannerPath").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))})
                                End Select
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub SaveToCache(ByVal sID As String, ByVal sURL As String, ByVal sPath As String)
            Dim sHTTP As New HTTP
            Dim sImage As New Images

            sHTTP.StartDownloadImage(sURL)

            While sHTTP.IsDownloading
                Application.DoEvents()
            End While

            sImage.Image = sHTTP.Image

            If Not IsNothing(sImage.Image) Then
                sImage.Save(Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sID, Path.DirectorySeparatorChar, sPath.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar))))
            End If

            sImage = Nothing
            sHTTP = Nothing
        End Sub

        Private Function CreditsString(ByVal sGStars As String, ByVal sWriters As String) As String
            Dim cString As New List(Of String)
            Dim gString As String = Master.eLang.GetString(776, "Guest Star")
            Dim wString As String = Master.eLang.GetString(777, "Writer")

            If Not String.IsNullOrEmpty(sGStars) Then
                For Each gStar In sGStars.Split(Convert.ToChar("|"))
                    If Not String.IsNullOrEmpty(gStar) Then cString.Add(String.Concat(gStar, String.Format(" ({0})", gString)))
                Next
            End If

            If Not String.IsNullOrEmpty(sWriters) Then
                For Each Writer In sWriters.Split(Convert.ToChar("|"))
                    If Not String.IsNullOrEmpty(Writer) Then cString.Add(String.Concat(Writer, String.Format(" ({0})", wString)))
                Next
            End If

            Return Strings.Join(cString.ToArray, " / ")
        End Function

        Public Sub DownloadSeriesAsync(ByVal sInfo As Structures.ScrapeInfo)
            Try
                If Not bwTVDB.IsBusy Then
                    RaiseEvent ScraperEvent(Enums.TVScraperEventType.StartingDownload, 0, Nothing)
                    bwTVDB.WorkerReportsProgress = True
                    bwTVDB.WorkerSupportsCancellation = True
                    bwTVDB.RunWorkerAsync(New Arguments With {.Type = 1, .Parameter = sInfo})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub GetSearchResultsAsync(ByVal sInfo As Structures.ScrapeInfo)
            Try
                If Not bwTVDB.IsBusy Then
                    bwTVDB.WorkerReportsProgress = True
                    bwTVDB.WorkerSupportsCancellation = True
                    bwTVDB.RunWorkerAsync(New Arguments With {.Type = 0, .Parameter = sInfo})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Private Function SearchSeries(ByVal sInfo As Structures.ScrapeInfo) As List(Of TVSearchResults)
            Dim tvdbResults As New List(Of TVSearchResults)
            Dim cResult As New TVSearchResults
            Dim xmlTVDB As XDocument
            Dim sHTTP As New HTTP
            Dim sLang As String = String.Empty

            Try
                Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/GetSeries.php?seriesname={1}&language={2}", Master.eSettings.TVDBMirror, sInfo.ShowTitle, Master.eSettings.TVDBLanguage))
                sHTTP = Nothing

                If Not String.IsNullOrEmpty(apiXML) Then
                    Try
                        xmlTVDB = XDocument.Parse(apiXML)
                    Catch
                        Return tvdbResults
                    End Try

                    Dim xSer = From xSeries In xmlTVDB.Descendants("Series") Where xSeries.HasElements

                    For Each xS As XElement In xSer
                        cResult = New TVSearchResults
                        cResult.ID = Convert.ToInt32(xS.Element("seriesid").Value)
                        cResult.Name = If(Not IsNothing(xS.Element("SeriesName")), xS.Element("SeriesName").Value, String.Empty)
                        If Not IsNothing(xS.Element("language")) AndAlso Master.eSettings.TVDBLanguages.Count > 0 Then
                            sLang = xS.Element("language").Value
                            cResult.Language = Master.eSettings.TVDBLanguages.FirstOrDefault(Function(s) s.ShortLang = sLang)
                        ElseIf Not IsNothing(xS.Element("language")) Then
                            sLang = xS.Element("language").Value
                            cResult.Language = New Containers.TVLanguage With {.LongLang = String.Format("Unknown ({0})", sLang), .ShortLang = sLang}
                        Else
                            'no language info available... don't bother adding it
                            Continue For
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

        Public Sub SingleScrape(ByVal sInfo As Structures.ScrapeInfo)
            RaiseEvent ScraperEvent(Enums.TVScraperEventType.LoadingEpisodes, 0, Nothing)
            bwTVDB.WorkerReportsProgress = False
            bwTVDB.WorkerSupportsCancellation = True
            bwTVDB.RunWorkerAsync(New Arguments With {.Type = 2, .Parameter = sInfo})
        End Sub

        Public Sub StartSingleScraper(ByVal sInfo As Structures.ScrapeInfo)

            Dim withCurrent As Boolean = Not String.IsNullOrEmpty(sInfo.TVDBID) AndAlso sInfo.WithCurrent

            Try
                If String.IsNullOrEmpty(sInfo.TVDBID) Then
                    RaiseEvent ScraperEvent(Enums.TVScraperEventType.Searching, 0, Nothing)
                    Using dTVDBSearch As New dlgTVDBSearchResults
                        If dTVDBSearch.ShowDialog(sInfo) = Windows.Forms.DialogResult.OK Then
                            Master.currShow = tmpTVDBShow.Show
                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.SelectImages, 0, Nothing)
                            Using dTVImageSel As New dlgTVImageSelect
                                If dTVImageSel.ShowDialog(sInfo.ShowID, Enums.TVImageType.All, withCurrent) = Windows.Forms.DialogResult.OK Then
                                    If Not IsNothing(sInfo.iSeason) AndAlso sInfo.iSeason >= 0 Then
                                        Me.SaveImages()
                                    Else
                                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 0, Nothing)
                                    End If
                                Else
                                    RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                                End If
                            End Using
                        Else
                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                        End If
                    End Using
                Else
                    DownloadSeries(sInfo)
                    If tmpTVDBShow.Show.TVShow.ID.Length > 0 Then
                        Master.currShow = tmpTVDBShow.Show
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.SelectImages, 0, Nothing)
                        Using dTVImageSel As New dlgTVImageSelect
                            If dTVImageSel.ShowDialog(sInfo.ShowID, Enums.TVImageType.All, withCurrent) = Windows.Forms.DialogResult.OK Then
                                If Not IsNothing(sInfo.iSeason) AndAlso sInfo.iSeason >= 0 Then
                                    Me.SaveImages()
                                Else
                                    RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 0, Nothing)
                                End If
                            Else
                                RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                            End If
                        End Using
                    Else
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.Searching, 0, Nothing)
                        Using dTVDBSearch As New dlgTVDBSearchResults
                            If dTVDBSearch.ShowDialog(sInfo) = Windows.Forms.DialogResult.OK Then
                                Master.currShow = tmpTVDBShow.Show
                                RaiseEvent ScraperEvent(Enums.TVScraperEventType.SelectImages, 0, Nothing)
                                Using dTVImageSel As New dlgTVImageSelect
                                    If dTVImageSel.ShowDialog(sInfo.ShowID, Enums.TVImageType.All, withCurrent) = Windows.Forms.DialogResult.OK Then
                                        If Not IsNothing(sInfo.iSeason) AndAlso sInfo.iSeason >= 0 Then
                                            Me.SaveImages()
                                        Else
                                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 0, Nothing)
                                        End If
                                    Else
                                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                                    End If
                                End Using
                            Else
                                RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                            End If
                        End Using
                    End If
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Sub ScrapeSeason(ByVal sInfo As Structures.ScrapeInfo)
            RaiseEvent ScraperEvent(Enums.TVScraperEventType.LoadingEpisodes, 0, Nothing)
            bwTVDB.WorkerReportsProgress = True
            bwTVDB.WorkerSupportsCancellation = True
            bwTVDB.RunWorkerAsync(New Arguments With {.Type = 4, .Parameter = sInfo})
        End Sub

        Public Sub ScrapeEpisode(ByVal sInfo As Structures.ScrapeInfo)

            Try
                tmpTVDBShow = New TVDBShow
                tmpTVDBShow.Episodes.Add(Master.currShow)

                If String.IsNullOrEmpty(sInfo.TVDBID) Then
                    RaiseEvent ScraperEvent(Enums.TVScraperEventType.Searching, 0, Nothing)
                    Using dTVDBSearch As New dlgTVDBSearchResults
                        If dTVDBSearch.ShowDialog(sInfo) = Windows.Forms.DialogResult.OK Then
                            Master.currShow = tmpTVDBShow.Episodes(0)
                            If Not File.Exists(Master.currShow.TVEp.LocalFile) Then
                                Master.currShow.TVEp.Poster.FromWeb(Master.currShow.TVEp.PosterURL)
                                Directory.CreateDirectory(Directory.GetParent(Master.currShow.TVEp.LocalFile).FullName)
                                Master.currShow.TVEp.Poster.Save(Master.currShow.TVEp.LocalFile)
                            End If
                            Master.currShow.EpPosterPath = Master.currShow.TVEp.LocalFile
                            If String.IsNullOrEmpty(Master.currShow.EpFanartPath) Then Master.currShow.EpFanartPath = Master.currShow.ShowFanartPath

                            If Master.eSettings.ScanTVMediaInfo Then MediaInfo.UpdateTVMediaInfo(Master.currShow)

                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 1, Nothing)
                        Else
                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                        End If
                    End Using
                Else
                    DownloadSeries(sInfo)
                    If tmpTVDBShow.Episodes(0).TVShow.ID.Length > 0 Then
                        Master.currShow = tmpTVDBShow.Episodes(0)
                        If Not File.Exists(Master.currShow.TVEp.LocalFile) Then
                            Master.currShow.TVEp.Poster.FromWeb(Master.currShow.TVEp.PosterURL)
                            Directory.CreateDirectory(Directory.GetParent(Master.currShow.TVEp.LocalFile).FullName)
                            Master.currShow.TVEp.Poster.Save(Master.currShow.TVEp.LocalFile)
                        End If
                        Master.currShow.EpPosterPath = Master.currShow.TVEp.LocalFile
                        If String.IsNullOrEmpty(Master.currShow.EpFanartPath) Then Master.currShow.EpFanartPath = Master.currShow.ShowFanartPath

                        If Master.eSettings.ScanTVMediaInfo Then MediaInfo.UpdateTVMediaInfo(Master.currShow)

                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 1, Nothing)
                    Else
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.Searching, 0, Nothing)
                        Using dTVDBSearch As New dlgTVDBSearchResults
                            If dTVDBSearch.ShowDialog(sInfo) = Windows.Forms.DialogResult.OK Then
                                Master.currShow = tmpTVDBShow.Episodes(0)
                                If Not File.Exists(Master.currShow.TVEp.LocalFile) Then
                                    Master.currShow.TVEp.Poster.FromWeb(Master.currShow.TVEp.PosterURL)
                                    Directory.CreateDirectory(Directory.GetParent(Master.currShow.TVEp.LocalFile).FullName)
                                    Master.currShow.TVEp.Poster.Save(Master.currShow.TVEp.LocalFile)
                                End If
                                Master.currShow.EpPosterPath = Master.currShow.TVEp.LocalFile
                                If String.IsNullOrEmpty(Master.currShow.EpFanartPath) Then Master.currShow.EpFanartPath = Master.currShow.ShowFanartPath

                                If Master.eSettings.ScanTVMediaInfo Then MediaInfo.UpdateTVMediaInfo(Master.currShow)

                                RaiseEvent ScraperEvent(Enums.TVScraperEventType.Verifying, 1, Nothing)
                            Else
                                RaiseEvent ScraperEvent(Enums.TVScraperEventType.Cancelled, 0, Nothing)
                            End If
                        End Using
                    End If
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function ChangeEpisode(ByVal sInfo As Structures.ScrapeInfo) As MediaContainers.EpisodeDetails
            Try
                Dim tEpisodes As List(Of MediaContainers.EpisodeDetails) = Me.GetListOfKnownEpisodes(sInfo)
                If tEpisodes.Count > 0 Then
                    Using dChangeEp As New dlgTVChangeEp
                        Return dChangeEp.ShowDialog(tEpisodes)
                    End Using
                Else
                    MsgBox(Master.eLang.GetString(778, "There are no known episodes for this show. Scrape the show, season, or episode and try again."), MsgBoxStyle.OkOnly, Master.eLang.GetString(779, "No Known Episodes"))
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return Nothing
        End Function

        Public Function GetSingleEpisode(ByVal sInfo As Structures.ScrapeInfo) As MediaContainers.EpisodeDetails
            Dim tEp As New MediaContainers.EpisodeDetails
            Try
                tEp = Me.GetListOfKnownEpisodes(sInfo).FirstOrDefault(Function(e) e.Season = sInfo.iSeason AndAlso e.Episode = sInfo.iEpisode)
                If Not IsNothing(tEp) Then
                    Return tEp
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return New MediaContainers.EpisodeDetails
        End Function

        Public Function GetListOfKnownEpisodes(ByVal sInfo As Structures.ScrapeInfo) As List(Of MediaContainers.EpisodeDetails)
            Dim Actors As New List(Of MediaContainers.Person)
            Dim tEpisodes As New List(Of MediaContainers.EpisodeDetails)
            Dim tEpisode As New MediaContainers.EpisodeDetails
            Dim fPath As String = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sInfo.TVDBID, Path.DirectorySeparatorChar, Master.eSettings.TVDBLanguage, ".zip"))

            Try
                If File.Exists(fPath) Then
                    Using fStream As FileStream = New FileStream(fPath, FileMode.Open, FileAccess.Read)
                        Dim fZip As Byte() = Functions.ReadStreamToEnd(fStream)
                        Me.ProcessTVDBZip(fZip, sInfo)

                        'get the actors first
                        Try
                            If Not String.IsNullOrEmpty(aXML) Then
                                Dim xdActors As XDocument = XDocument.Parse(aXML)
                                For Each Actor As XElement In xdActors.Descendants("Actor")
                                    Actors.Add(New MediaContainers.Person With {.Name = Actor.Element("Name").Value, .Role = Actor.Element("Role").Value, .Thumb = If(IsNothing(Actor.Element("Image")) OrElse String.IsNullOrEmpty(Actor.Element("Image").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, Actor.Element("Image").Value))})
                                Next
                            End If
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try

                        If Not String.IsNullOrEmpty(sXML) Then
                            Dim xdEps As XDocument = XDocument.Parse(sXML)

                            For Each Episode As XElement In xdEps.Descendants("Episode")
                                tEpisode = New MediaContainers.EpisodeDetails

                                With tEpisode
                                    .Title = Episode.Element("EpisodeName").Value
                                    .Season = If(IsNothing(Episode.Element("SeasonNumber")) OrElse String.IsNullOrEmpty(Episode.Element("SeasonNumber").Value), 0, Convert.ToInt32(Episode.Element("SeasonNumber").Value))
                                    .Episode = If(IsNothing(Episode.Element("EpisodeNumber")) OrElse String.IsNullOrEmpty(Episode.Element("EpisodeNumber").Value), 0, Convert.ToInt32(Episode.Element("EpisodeNumber").Value))
                                    .Aired = Episode.Element("FirstAired").Value
                                    .Rating = Episode.Element("Rating").Value
                                    .Plot = Episode.Element("Overview").Value
                                    .Director = Episode.Element("Director").Value
                                    .Credits = CreditsString(Episode.Element("GuestStars").Value, Episode.Element("Writer").Value)
                                    .Actors = Actors
                                    .PosterURL = If(IsNothing(Episode.Element("filename")) OrElse String.IsNullOrEmpty(Episode.Element("filename").Value), String.Empty, String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, Episode.Element("filename").Value))
                                    .LocalFile = Path.Combine(Master.TempPath, String.Concat("Shows", Path.DirectorySeparatorChar, sInfo.TVDBID, Path.DirectorySeparatorChar, "episodeposters", Path.DirectorySeparatorChar, Episode.Element("filename").Value.Replace(Convert.ToChar("/"), Path.DirectorySeparatorChar)))
                                End With

                                tEpisodes.Add(tEpisode)
                            Next

                        End If
                    End Using
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return tEpisodes
        End Function

        Public Function GetSingleImage(ByVal sInfo As Structures.ScrapeInfo) As Image
            tmpTVDBShow = New TVDBShow


            If sInfo.ImageType = Enums.TVImageType.EpisodePoster Then

                If String.IsNullOrEmpty(sInfo.TVDBID) Then
                    Using dTVDBSearch As New dlgTVDBSearchResults
                        sInfo = dTVDBSearch.ShowDialog(sInfo, True)
                        If Not String.IsNullOrEmpty(sInfo.TVDBID) Then
                            Master.currShow.TVShow.ID = sInfo.TVDBID

                            Using tImage As New Images
                                Dim tmpEp As MediaContainers.EpisodeDetails = Me.GetListOfKnownEpisodes(sInfo).FirstOrDefault(Function(e) e.Episode = sInfo.iEpisode AndAlso e.Season = sInfo.iSeason)
                                If Not IsNothing(tmpEp) Then

                                    If File.Exists(tmpEp.LocalFile) Then
                                        tImage.FromFile(tmpEp.LocalFile)
                                    Else
                                        tImage.FromWeb(tmpEp.PosterURL)
                                        If Not IsNothing(tImage.Image) Then
                                            Directory.CreateDirectory(Directory.GetParent(tmpEp.LocalFile).FullName)
                                            tImage.Save(tmpEp.LocalFile)
                                        End If
                                    End If

                                    If Not IsNothing(tImage.Image) Then
                                        Using dPosterConfirm As New dlgTVEpisodePoster
                                            If dPosterConfirm.ShowDialog(tImage.Image) = DialogResult.OK Then
                                                Return tImage.Image
                                            Else
                                                Return Nothing
                                            End If
                                        End Using
                                    Else
                                        MsgBox(Master.eLang.GetString(819, "There is no poster available for this episode."), MsgBoxStyle.OkOnly, Master.eLang.GetString(820, "No Poster Available"))
                                        Return Nothing
                                    End If
                                Else
                                    Return Nothing
                                End If
                            End Using
                        Else
                            Return Nothing
                        End If
                    End Using
                Else
                    Using tImage As New Images
                        Dim tmpEp As MediaContainers.EpisodeDetails = Me.GetListOfKnownEpisodes(sInfo).FirstOrDefault(Function(e) e.Episode = sInfo.iEpisode AndAlso e.Season = sInfo.iSeason)
                        If Not IsNothing(tmpEp) Then

                            If File.Exists(tmpEp.LocalFile) Then
                                tImage.FromFile(tmpEp.LocalFile)
                            Else
                                tImage.FromWeb(tmpEp.PosterURL)
                                If Not IsNothing(tImage.Image) Then
                                    Directory.CreateDirectory(Directory.GetParent(tmpEp.LocalFile).FullName)
                                    tImage.Save(tmpEp.LocalFile)
                                End If
                            End If

                            If Not IsNothing(tImage.Image) Then
                                Using dPosterConfirm As New dlgTVEpisodePoster
                                    If dPosterConfirm.ShowDialog(tImage.Image) = DialogResult.OK Then
                                        Return tImage.Image
                                    Else
                                        Return Nothing
                                    End If
                                End Using
                            Else
                                MsgBox(Master.eLang.GetString(819, "There is no poster available for this episode."), MsgBoxStyle.OkOnly, Master.eLang.GetString(820, "No Poster Available"))
                                Return Nothing
                            End If
                        Else
                            Return Nothing
                        End If
                    End Using
                End If
            Else
                If String.IsNullOrEmpty(sInfo.TVDBID) Then
                    Using dTVDBSearch As New dlgTVDBSearchResults
                        sInfo = dTVDBSearch.ShowDialog(sInfo, True)
                        If Not String.IsNullOrEmpty(sInfo.TVDBID) Then
                            Master.currShow.TVShow.ID = sInfo.TVDBID
                            Me.DownloadSeries(sInfo, True)
                            Using dImageSelect As New dlgTVImageSelect
                                Return dImageSelect.ShowDialog(sInfo.ShowID, sInfo.ImageType, sInfo.iSeason, sInfo.CurrentImage)
                            End Using
                        Else
                            Return Nothing
                        End If
                    End Using
                Else
                    Me.DownloadSeries(sInfo, True)
                    Using dImageSelect As New dlgTVImageSelect
                        Return dImageSelect.ShowDialog(sInfo.ShowID, sInfo.ImageType, sInfo.iSeason, sInfo.CurrentImage)
                    End Using
                End If
            End If
        End Function

        Public Sub CancelAsync()
            If bwTVDB.IsBusy Then bwTVDB.CancelAsync()
        End Sub

        Public Function IsBusy() As Boolean
            Return bwTVDB.IsBusy
        End Function

        Private Sub bwtvDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTVDB.DoWork
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)

            Try
                Select Case Args.Type
                    Case 0 'search
                        e.Result = New Results With {.Type = 0, .Result = SearchSeries(DirectCast(Args.Parameter, Structures.ScrapeInfo))}
                    Case 1 'show download
                        Me.DownloadSeries(DirectCast(Args.Parameter, Structures.ScrapeInfo))
                        e.Result = New Results With {.Type = 1}
                    Case 2 'load episodes
                        LoadAllEpisodes(DirectCast(Args.Parameter, Structures.ScrapeInfo).ShowID, 999)
                        e.Result = New Results With {.Type = 2, .Result = Args.Parameter}
                    Case 3 'save
                        Me.SaveAllTVInfo()
                        e.Result = New Results With {.Type = 3}
                    Case 4
                        Dim sInfo As Structures.ScrapeInfo = DirectCast(Args.Parameter, Structures.ScrapeInfo)
                        LoadAllEpisodes(sInfo.ShowID, sInfo.iSeason)
                        e.Result = New Results With {.Type = 2, .Result = Args.Parameter}
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub bwTVDB_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwTVDB.ProgressChanged
            RaiseEvent ScraperEvent(Enums.TVScraperEventType.Progress, e.ProgressPercentage, e.UserState.ToString)
        End Sub

        Private Sub bwTVDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTVDB.RunWorkerCompleted
            Dim Res As Results = DirectCast(e.Result, Results)

            Try
                Select Case Res.Type
                    Case 0 'search
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.SearchResultsDownloaded, 0, DirectCast(Res.Result, List(Of TVSearchResults)))
                    Case 1 'show download
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.ShowDownloaded, 0, Nothing)
                    Case 2 'load episodes
                        If Not e.Cancelled Then
                            StartSingleScraper(DirectCast(Res.Result, Structures.ScrapeInfo))
                        Else
                            RaiseEvent ScraperEvent(Enums.TVScraperEventType.ScraperDone, 0, Nothing)
                        End If
                    Case 3 'save
                        RaiseEvent ScraperEvent(Enums.TVScraperEventType.ScraperDone, 0, Nothing)
                End Select
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub SaveAllTVInfo()
            Dim iEp As Integer = -1
            Dim iSea As Integer = -1
            Dim iProgress As Integer = 1

            Dim tShow As New Structures.DBTV
            Dim tEpisode As New MediaContainers.EpisodeDetails

            Me.bwTVDB.ReportProgress(tmpTVDBShow.Episodes.Count, "max")

            Using SQLTrans As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Try
                    If Master.eSettings.AllSeasonPosterEnabled AndAlso Not IsNothing(TVDBImages.AllSeasonPoster.Image.Image) Then
                        Dim aSeason As New Structures.DBTV
                        aSeason = tmpTVDBShow.AllSeason
                        aSeason.SeasonPosterPath = TVDBImages.AllSeasonPoster.Image.SaveAsAllSeasonPoster(tmpTVDBShow.Show)
                        Master.DB.SaveTVSeasonToDB(aSeason, False, True)
                    End If

                    For Each Episode As Structures.DBTV In tmpTVDBShow.Episodes

                        Try
                            If Me.bwTVDB.CancellationPending Then Return

                            Episode.ShowID = Master.currShow.ShowID

                            iEp = Episode.TVEp.Episode
                            iSea = Episode.TVEp.Season

                            'remove it from tepisodes since it's a real episode
                            If Master.eSettings.DisplayMissingEpisodes Then
                                tEpisode = tEpisodes.FirstOrDefault(Function(e) e.Episode = iEp AndAlso e.Season = iSea)
                                If Not IsNothing(tEpisode) Then tEpisodes.Remove(tEpisode)
                                tShow = Episode
                            End If

                            If Me.bwTVDB.CancellationPending Then Return

                            If Not IsNothing(Episode.TVEp.Poster.Image) Then Episode.EpPosterPath = Episode.TVEp.Poster.SaveAsEpPoster(Episode)

                            If Me.bwTVDB.CancellationPending Then Return

                            If Master.eSettings.EpisodeFanartEnabled AndAlso Not IsNothing(Episode.TVEp.Fanart.Image) Then Episode.EpFanartPath = Episode.TVEp.Fanart.SaveAsEpFanart(Episode)

                            If Me.bwTVDB.CancellationPending Then Return

                            Dim cSea = From cSeason As TVDBSeasonImage In TVDBImages.SeasonImageList Where cSeason.Season = iSea Take 1
                            If cSea.Count > 0 Then
                                If Not IsNothing(cSea(0).Poster.Image) Then Episode.SeasonPosterPath = cSea(0).Poster.SaveAsSeasonPoster(Episode)

                                If Me.bwTVDB.CancellationPending Then Return

                                If Master.eSettings.SeasonFanartEnabled Then
                                    If Not String.IsNullOrEmpty(cSea(0).Fanart.LocalFile) AndAlso File.Exists(cSea(0).Fanart.LocalFile) Then
                                        cSea(0).Fanart.Image.FromFile(cSea(0).Fanart.LocalFile)
                                        Episode.SeasonFanartPath = cSea(0).Fanart.Image.SaveAsSeasonFanart(Episode)
                                    ElseIf Not String.IsNullOrEmpty(cSea(0).Fanart.URL) AndAlso Not String.IsNullOrEmpty(cSea(0).Fanart.LocalFile) Then
                                        cSea(0).Fanart.Image.FromWeb(cSea(0).Fanart.URL)
                                        If Not IsNothing(cSea(0).Fanart.Image.Image) Then
                                            Directory.CreateDirectory(Directory.GetParent(cSea(0).Fanart.LocalFile).FullName)
                                            cSea(0).Fanart.Image.Save(cSea(0).Fanart.LocalFile)
                                            Episode.SeasonFanartPath = cSea(0).Fanart.Image.SaveAsSeasonFanart(Episode)
                                        End If
                                    End If
                                End If
                            End If

                            If Me.bwTVDB.CancellationPending Then Return

                            If Master.eSettings.ScanTVMediaInfo Then MediaInfo.UpdateTVMediaInfo(Episode)

                            Master.DB.SaveTVEpToDB(Episode, False, True, True, True)

                            If Me.bwTVDB.CancellationPending Then Return

                            Me.bwTVDB.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    'now save all missing episodes
                    If Master.eSettings.DisplayMissingEpisodes Then
                        tShow.Filename = String.Empty
                        tShow.EpFanartPath = String.Empty
                        tShow.EpPosterPath = String.Empty
                        tShow.EpNfoPath = String.Empty
                        tShow.IsLockEp = False
                        tShow.IsMarkEp = False
                        tShow.IsNewEp = False
                        tShow.EpID = -1
                        If tEpisodes.Count > 0 Then
                            For Each Episode As MediaContainers.EpisodeDetails In tEpisodes
                                tShow.TVEp = Episode
                                Master.DB.SaveTVEpToDB(tShow, True, True, True)
                            Next
                        End If
                    End If

                    If Me.bwTVDB.CancellationPending Then Return

                    SQLTrans.Commit()

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

            End Using

        End Sub

        Public Shared Sub LoadAllEpisodes(ByVal _ID As Integer, ByVal OnlySeason As Integer)
            Try

                tmpTVDBShow = New TVDBShow

                tmpTVDBShow.Show = Master.DB.LoadTVFullShowFromDB(_ID)
                tmpTVDBShow.AllSeason = Master.DB.LoadTVAllSeasonFromDB(_ID)

                Using SQLCount As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    If OnlySeason = 999 Then
                        SQLCount.CommandText = String.Concat("SELECT COUNT(ID) AS eCount FROM TVEps WHERE TVShowID = ", _ID, " AND Missing = 0;")
                    Else
                        SQLCount.CommandText = String.Concat("SELECT COUNT(ID) AS eCount FROM TVEps WHERE TVShowID = ", _ID, " AND Season = ", OnlySeason, " AND Missing = 0;")
                    End If
                    Using SQLRCount As SQLite.SQLiteDataReader = SQLCount.ExecuteReader
                        If Convert.ToInt32(SQLRCount("eCount")) > 0 Then
                            Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                                If OnlySeason = 999 Then
                                    SQLCommand.CommandText = String.Concat("SELECT ID, Lock FROM TVEps WHERE TVShowID = ", _ID, " AND Missing = 0;")
                                Else
                                    SQLCommand.CommandText = String.Concat("SELECT ID, Lock FROM TVEps WHERE TVShowID = ", _ID, " AND Season = ", OnlySeason, " AND Missing = 0;")
                                End If
                                Using SQLReader As SQLite.SQLiteDataReader = SQLCommand.ExecuteReader
                                    While SQLReader.Read
                                        If Not Convert.ToBoolean(SQLReader("Lock")) Then tmpTVDBShow.Episodes.Add(Master.DB.LoadTVEpFromDB(Convert.ToInt64(SQLReader("ID")), True))
                                    End While
                                End Using
                            End Using
                        End If
                    End Using
                End Using
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Private Function StringToSeasonPosterType(ByVal sType As String) As Enums.SeasonPosterType
            Select Case sType.ToLower
                Case "season"
                    Return Enums.SeasonPosterType.Poster
                Case "seasonwide"
                    Return Enums.SeasonPosterType.Wide
                Case Else
                    Return Enums.SeasonPosterType.None
            End Select
        End Function

        Private Function StringToShowPosterType(ByVal sType As String) As Enums.ShowPosterType
            Select Case sType.ToLower
                Case "blank"
                    Return Enums.ShowPosterType.Blank
                Case "graphical"
                    Return Enums.ShowPosterType.Graphical
                Case "text"
                    Return Enums.ShowPosterType.Text
                Case Else
                    Return Enums.ShowPosterType.None
            End Select
        End Function
    End Class
End Class
