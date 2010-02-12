Public Class Containers
    Public Class TVLanguage
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
End Class

' Interfaces for external Modules
Public Interface EmberExternalModule
    Sub Enable()
    Sub Disable()
    Sub Setup()
    Sub Init(ByRef emm As Object)
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
End Interface

Public Interface EmberScraperModule
    Sub Setup()
    'Title or Id must be field in, all movie is past because some scrapper may run to update only some fields (defined in setup)
    Function Scraper(ByVal Movie As Object) As Object
    Function PostScraper(ByVal Movie As Object) As Object ' object is Media.Movie .. need to wait until all comes here and than change
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
    ReadOnly Property IsScraper() As Boolean
    ReadOnly Property IsPostScraper() As Boolean
End Interface

