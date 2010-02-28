Imports System.IO
Imports EmberAPI
''' <summary>
''' Native Scraper
''' </summary>
''' <remarks></remarks>
Public Class EmberNativeTVScraperModule
    Implements EmberAPI.Interfaces.EmberTVScraperModule

    Private _Name As String = "Ember Native TV Scraper"
    Public Shared TVScraper As New TVDB.Scraper
    Public Sub Init() Implements EmberAPI.Interfaces.EmberTVScraperModule.Init

    End Sub

    Public ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberTVScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberTVScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberTVScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberTVScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function PostScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.PostScraper

    End Function
    Function GetLangs(ByVal sMirror As String, ByRef Langs As List(Of Containers.TVLanguage)) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.GetLangs
        Langs = TVScraper.GetLangs(sMirror)
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function
    Function SaveImages() As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.SaveImages
        AddHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        TVScraper.SaveImages()
        RemoveHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function
    Function Scraper(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.Scraper
        AddHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        TVScraper.SingleScrape(ShowID, ShowTitle, TVDBID, Lang, Options)
        RemoveHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function
    Function ScrapeEpisode(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iEpisode As Integer, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.ScrapeEpisode
        AddHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        TVScraper.ScrapeEpisode(ShowID, ShowTitle, TVDBID, iEpisode, iSeason, Lang, Options)
        RemoveHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function
    Public Sub Handler_ScraperEvent(ByVal eType As EmberAPI.Enums.ScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        RaiseEvent TVScraperEvent(eType, iProgress, Parameter)
    End Sub

    Function GetSingleEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Season As Integer, ByVal Episode As Integer, ByVal Options As Structures.TVScrapeOptions, ByRef epDetails As MediaContainers.EpisodeDetails) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.GetSingleEpisode
        AddHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        epDetails = TVScraper.GetSingleEpisode(ShowID, TVDBID, Season, Episode, Options)
        RemoveHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function

    Public Event TVScraperEvent(ByVal eType As EmberAPI.Enums.ScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object) Implements EmberAPI.Interfaces.EmberTVScraperModule.TVScraperEvent
    'Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberTVScraperModule.ScraperUpdateMediaList

    Public Sub SetupPostScraper() Implements EmberAPI.Interfaces.EmberTVScraperModule.SetupPostScraper

    End Sub

    Public Sub SetupScraper() Implements EmberAPI.Interfaces.EmberTVScraperModule.SetupScraper

    End Sub

    Public Function testSetupScraper(ByRef p As System.Windows.Forms.Panel) As Integer Implements EmberAPI.Interfaces.EmberTVScraperModule.testSetupScraper

    End Function

    Function ChangeEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByRef epDet As MediaContainers.EpisodeDetails) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberTVScraperModule.ChangeEpisode
        epDet = TVScraper.ChangeEpisode(ShowID, TVDBID)
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = True}
    End Function
End Class


