Public Class Interfaces
    ' Interfaces for external Modules
    Public Interface EmberExternalModule
        Sub Enable()
        Sub Disable()
        Sub Setup()
        Sub Init(ByRef emm As ModulesManager.EmberRuntimeObjects)
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
    End Interface

    Public Structure ScraperResult
        Public Cancelled As Boolean
        Public breakChain As Boolean
    End Structure

    Public Interface EmberScraperModule
        Function testSetupScraper(ByRef p As System.Windows.Forms.Panel) As Integer
        Sub SetupScraper()
        Sub SetupPostScraper()
        Sub SetupTVScraper()
        Sub SetupTVPostScraper()
        Sub Init()
        'Movie is byref because some scrapper may run to update only some fields (defined in Scraper Setup)
        'Options is byref to allow field blocking in scraper chain
        Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As ScraperResult
        Function TVScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As ScraperResult
        Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As ScraperResult
        Function SelectImageOfType(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As ScraperResult
        Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As ScraperResult
        Function TVPostScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As ScraperResult
        Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As List(Of String)) As ScraperResult

        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property IsTVScraper() As Boolean
        ReadOnly Property IsPostScraper() As Boolean
        Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean)
    End Interface
End Class
