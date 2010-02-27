Imports System.IO
Imports System.Xml.Serialization
Imports System.Net
Imports System.Web


Public Class NunoScraperModule
    Implements EmberAPI.Interfaces.EmberMovieScraperModule
    Function testSetupScraper(ByRef p As System.Windows.Forms.Panel) As Integer Implements EmberAPI.Interfaces.EmberMovieScraperModule.testSetupScraper
        Return 0
    End Function

    Private MyPath As String
    Private codLang As String
    Private DoOutline As Boolean
    Private DoPlot As Boolean
    Private Language As String
    Public Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.DownloadTrailer

        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function
    Public Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As System.Collections.Generic.List(Of String)) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.GetMovieStudio
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function
    Public Sub Init() Implements EmberAPI.Interfaces.EmberMovieScraperModule.Init
        'Master.eLang.LoadLanguage(Master.eSettings.Language)
        MyPath = Path.Combine(Functions.AppPath, "Modules")
    End Sub
    Public ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsPostScraper
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property
    Public ReadOnly Property IsTVScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsTVScraper
        Get
            Return False
        End Get
    End Property
    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleName
        Get
            Return "Nuno Scraper"
        End Get
    End Property
    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property
    Public Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.PostScraper

        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function
    Public Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As EmberAPI.Structures.ScrapeOptions) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.Scraper
        codLang = Localization.ISOLangGetCode2ByLang(Language)
        If DoOutline Then DBMovie.Movie.Outline = Translate(codLang, DBMovie.Movie.Outline)
        If DoPlot Then DBMovie.Movie.Plot = Translate(codLang, DBMovie.Movie.Plot)
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function
    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberMovieScraperModule.ScraperUpdateMediaList
    Public Function SelectImageOfType(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As EmberAPI.Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.SelectImageOfType
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function
    Public Sub SetupPostScraper() Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupPostScraper
    End Sub
    Public Sub SetupScraper() Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupScraper
        Using frmSetup As New scraperSetup
            frmSetup.preferedLanguage = AdvancedSettings.GetSetting("Language")
            frmSetup.tOutline.Checked = AdvancedSettings.GetBooleanSetting("Do.Outline")
            frmSetup.tPlot.Checked = AdvancedSettings.GetBooleanSetting("Do.Plot")
            If frmSetup.ShowDialog() = Windows.Forms.DialogResult.OK Then
                AdvancedSettings.SetSetting("Language", frmSetup.cLanguage.Text)
                AdvancedSettings.SetBooleanSetting("Do.Outline", frmSetup.tOutline.Checked)
                AdvancedSettings.SetBooleanSetting("Do.Plot", frmSetup.tPlot.Checked)
            End If
        End Using
    End Sub
    Public Shared Function Translate(ByVal codLang As String, ByVal txt As String) As String
        Dim ret As String = String.Empty
        Try
            txt = Web.HttpUtility.UrlEncode(txt)
            'txt = txt.Replace(" ", "%20")
            Dim url As String = String.Concat("http://ajax.googleapis.com/ajax/services/language/translate?langpair=%7C", codLang, "&v=1.0&q=", txt)
            Dim wrRequest As WebRequest = HttpWebRequest.Create(url)
            Dim sResponse As String = String.Empty
            wrRequest.Method = WebRequestMethods.Http.Get
            wrRequest.Timeout = 10000
            Dim wrResponse As WebResponse = wrRequest.GetResponse()
            Dim contentEncoding As String = String.Empty
            'wrResponse = wrRequest.GetResponse()
            Using Ms As Stream = wrResponse.GetResponseStream
                sResponse = New StreamReader(Ms).ReadToEnd '.Replace("""", "")
                'sResponse = sResponse.Replace(" ", "")
            End Using
            wrResponse.Close()
            Dim t As String()
            t = sResponse.Split(New Char() {Convert.ToChar("{"), Convert.ToChar("}")})
            If t.Count >= 2 Then
                t = t(2).Split(Convert.ToChar(":"))
                If t.Count >= 2 Then
                    If t(0).Replace("""", "") = "translatedText" Then
                        ret = t(1).Replace(",""detectedSourceLanguage""", "")
                        If ret.Length > 2 Then ret = ret.Substring(1, ret.Length - 2)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try

        Return ret
    End Function


End Class



