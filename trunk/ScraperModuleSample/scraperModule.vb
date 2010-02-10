Public Interface EmberScraperModule
    Sub Setup()
    Function ExecFase1(ByVal MovieTitle As String, ByVal Id As String) As ArrayList
    Function ExecFase2(ByVal MovieTitle As String, ByVal Id As String) As Media.Movie
    Function ExecPost(ByVal Movie As Media.Movie) As Media.Movie
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
    ReadOnly Property IsScraper() As Boolean
    ReadOnly Property IsPostScraper() As Boolean
End Interface

Public Class TestEmberScraperModule
    Implements EmberScraperModule
    Dim emmAPI As New Object
    Private Enabled As Boolean = False
    Private _Name As String = "Teste Scraper"
    Private _Version As String = "1.0"
 
    ReadOnly Property IsScraper() As Boolean Implements EmberScraperModule.IsScraper
        Get
            Return False
        End Get
    End Property
    ReadOnly Property IsPostScraper() As Boolean Implements EmberScraperModule.IsPostScraper
        Get
            Return False
        End Get
    End Property
    Sub Setup() Implements EmberScraperModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Function StartFase1(ByVal MovieTitle As String, ByVal Id As String) As ArrayList Implements EmberScraperModule.ExecFase1
        Return Nothing
    End Function

    Function StartFase2(ByVal MovieTitle As String, ByVal Id As String) As Media.Movie Implements EmberScraperModule.ExecFase2
        Return Nothing
    End Function
    Function ExecPost(ByVal Movie As Media.Movie) As Media.Movie Implements EmberScraperModule.ExecPost
        Return Nothing
    End Function
    ReadOnly Property ModuleName() As String Implements EmberScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberScraperModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property
End Class
