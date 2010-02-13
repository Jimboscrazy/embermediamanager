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
        Sub Setup()
        'Title or Id must be field in, all movie is past because some scrapper may run to update only some fields (defined in setup)
        Function Scraper(ByRef Movie As EmberAPI.MediaContainers.Movie, ByRef Options As Structures.ScrapeOptions) As Boolean
        Function PostScraper(ByRef Movie As EmberAPI.MediaContainers.Movie) As Boolean ' object is MediaContainers.Movie .. need to wait until all comes here and than change
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property IsPostScraper() As Boolean
    End Interface
End Class
