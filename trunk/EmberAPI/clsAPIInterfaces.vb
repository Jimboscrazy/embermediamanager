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

    Public Interface EmberScraperModule
        Sub Setup(ByVal tScraper As Integer)
        'Title or Id must be field in
        'Movie is byref because some scrapper may run to update only some fields (defined in Scraper Setup)
        'Options is byref to allow field blocking in scraper chain
        Function Scraper(ByRef Movie As EmberAPI.MediaContainers.Movie, ByRef Options As Structures.ScrapeOptions) As Boolean
        Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property IsPostScraper() As Boolean
        Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean)
    End Interface
End Class
