Imports System.IO
Imports EmberAPI
''' <summary>
''' Native Scraper
''' </summary>
''' <remarks></remarks>
Public Class EmberBenceScraperModule
    Implements EmberAPI.Interfaces.EmberMovieScraperModule


    Private _Name As String = "Bence Scraper"

    Public Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.DownloadTrailer

    End Function

    Public Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As System.Collections.Generic.List(Of String)) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.GetMovieStudio

    End Function

    Public Sub Init() Implements EmberAPI.Interfaces.EmberMovieScraperModule.Init

    End Sub

    Public Function InjectSetupPostScraper() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberMovieScraperModule.InjectSetupPostScraper
        Return (Nothing)
    End Function

    Public Function InjectSetupScraper() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberMovieScraperModule.InjectSetupScraper
        Return (Nothing)
    End Function

    Public ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsPostScraper
        Get

        End Get
    End Property

    Public ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsScraper
        Get

        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property

    Public Event ModuleSettingsChanged() Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleSettingsChanged

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Event MovieScraperEvent(ByVal eType As EmberAPI.Enums.MovieScraperEventType, ByVal Parameter As Object) Implements EmberAPI.Interfaces.EmberMovieScraperModule.MovieScraperEvent

    Public Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.PostScraper

    End Function

    Public Property PostScraperEnabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.PostScraperEnabled
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieScraperModule.SaveSetupPostScraper

    End Sub

    Public Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieScraperModule.SaveSetupScraper

    End Sub

    Public Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As EmberAPI.Structures.ScrapeOptions) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.Scraper

    End Function

    Public Property ScraperEnabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.ScraperEnabled
        Get

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Function SelectImageOfType(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As EmberAPI.Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.SelectImageOfType

    End Function

    Public Event SetupPostScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupPostScraperChanged

    Public Event SetupScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupScraperChanged
End Class

