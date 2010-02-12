Public Class Interfaces
    ' Interfaces for external Modules
    Public Interface EmberExternalModule
        Sub Enable()
        Sub Disable()
        Sub Setup()
        Sub Init(ByRef emm As EmberModules.RuntimeObjects)
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
    End Interface

    Public Interface EmberScraperModule
        Sub Setup()
        'Title or Id must be field in, all movie is past because some scrapper may run to update only some fields (defined in setup)
        Function Scraper(ByVal Movie As EmberAPI.MediaContainers.Movie) As EmberAPI.MediaContainers.Movie
        Function PostScraper(ByVal Movie As EmberAPI.MediaContainers.Movie) As EmberAPI.MediaContainers.Movie ' object is MediaContainers.Movie .. need to wait until all comes here and than change
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property IsPostScraper() As Boolean
    End Interface
End Class
