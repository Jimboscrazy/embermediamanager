Public Class NunoScraperModule
    Implements EmberAPI.Interfaces.EmberScraperModule
    Public Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.DownloadTrailer
        Return False
    End Function
    Public Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As System.Collections.Generic.List(Of String)) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.GetMovieStudio
        Return False
    End Function
    Public Sub Init() Implements EmberAPI.Interfaces.EmberScraperModule.Init
    End Sub
    Public ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property
    Public ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsScraper
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property IsTVScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsTVScraper
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleName
        Get
            Return "Nuno Scraper"
        End Get
    End Property
    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleVersion
        Get
            Return "1"
        End Get
    End Property
    Public Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.PostScraper

        Return True
    End Function
    Public Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As EmberAPI.Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.Scraper
        Return True
    End Function
    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberScraperModule.ScraperUpdateMediaList
    Public Function SelectImageOfType(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As EmberAPI.Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.SelectImageOfType
        Return False
    End Function
    Public Sub SetupPostScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupPostScraper
        Using frmSetup As New scraperSetup
            frmSetup.ShowDialog()
        End Using
    End Sub
    Public Sub SetupScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupScraper
    End Sub
    Public Sub SetupTVPostScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupTVPostScraper
    End Sub
    Public Sub SetupTVScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupTVScraper
    End Sub
    Public Function TVPostScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.TVPostScraper
        Return True
    End Function
    Public Function TVScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As EmberAPI.Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.TVScraper
        Return True
    End Function
End Class



