' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Xml.Serialization

Public Class NunoScraperModule
    Implements Interfaces.EmberMovieScraperModule

    #Region "Fields"

    Private codLang As String
    Private DoOutline As Boolean
    Private DoPlot As Boolean
    Private DoSubs As Boolean
    Private DoSubsTrans As Boolean
    Private Language As String
    Private MyPath As String
    Private _Name As String = "Nuno's Module"
    Private _PostScraperEnabled As Boolean = False
    Private _ScraperEnabled As Boolean = False
    Private _setup As frmSettingsHolder

    #End Region 'Fields

    #Region "Events"

    Public Event Modulesettingschanged() Implements Interfaces.EmberMovieScraperModule.ModuleSettingsChanged

    Public Event MovieScraperEvent(ByVal eType As Enums.MovieScraperEventType, ByVal Parameter As Object) Implements Interfaces.EmberMovieScraperModule.MovieScraperEvent

    Public Event SetupPostScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberMovieScraperModule.PostScraperSetupChanged

    Public Event SetupScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberMovieScraperModule.ScraperSetupChanged

    #End Region 'Events

    #Region "Properties"

    Public ReadOnly Property IsPostScraper() As Boolean Implements Interfaces.EmberMovieScraperModule.IsPostScraper
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsScraper() As Boolean Implements Interfaces.EmberMovieScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements Interfaces.EmberMovieScraperModule.ModuleName
        Get
            Return "Nuno Scraper"
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberMovieScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Property PostScraperEnabled() As Boolean Implements Interfaces.EmberMovieScraperModule.PostScraperEnabled
        Get
            Return _PostScraperEnabled
        End Get
        Set(ByVal value As Boolean)
            _PostScraperEnabled = value
        End Set
    End Property

    Property ScraperEnabled() As Boolean Implements Interfaces.EmberMovieScraperModule.ScraperEnabled
        Get
            Return _ScraperEnabled
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled = value
        End Set
    End Property

    #End Region 'Properties

    #Region "Methods"

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

    Public Function DownloadTrailer(ByRef DBMovie As Structures.DBMovie, ByRef sURL As String) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.DownloadTrailer
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Public Function GetMovieStudio(ByRef DBMovie As Structures.DBMovie, ByRef sStudio As System.Collections.Generic.List(Of String)) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.GetMovieStudio
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Public Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberMovieScraperModule.Init
        MyPath = Path.Combine(Functions.AppPath, "Modules")
    End Sub

    Public Function PostScraper(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.PostScraper
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Public Function Scraper(ByRef DBMovie As Structures.DBMovie, ByRef ScrapeType As Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.Scraper
        codLang = Localization.ISOLangGetCode2ByLang(Language)
        If DoOutline Then DBMovie.Movie.Outline = Translate(codLang, DBMovie.Movie.Outline)
        If DoPlot Then DBMovie.Movie.Plot = Translate(codLang, DBMovie.Movie.Plot)
        If DoSubs Then
            If Not String.IsNullOrEmpty(DBMovie.SubPath) Then
                'Dim mi As MediaInfo.Fileinfo = DBMovie.Movie.FileInfo
                If DBMovie.Movie.FileInfo.StreamDetails.Subtitle.FirstOrDefault(Function(y) y.SubsType = "External") Is Nothing Then
                    Dim subs As New MediaInfo.Subtitle With {.SubsPath = DBMovie.SubPath, .SubsType = "External"}
                End If
            End If
        End If
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Public Function SelectImageOfType(ByRef DBMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.SelectImageOfType
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Function InjectSetupPostScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupPostScraper
        Dim SPanel As New Containers.SettingsPanel
        SPanel.Name = String.Concat(Me._Name, "PostScraper")
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(698, "Movies")
        SPanel.ImageIndex = If(Me._ScraperEnabled, 9, 10)
        SPanel.Order = 100
        SPanel.Prefix = "Nuno_"
        SPanel.Panel = New System.Windows.Forms.Panel
        SPanel.Parent = "pnlMovieMedia"
        Return SPanel
    End Function

    Function InjectSetupScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupScraper
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmSettingsHolder
        Me._setup.chkEnabled.Checked = Me.ScraperEnabled
        Me._setup.preferedLanguage = AdvancedSettings.GetSetting("Language", "en")
        Me._setup.tOutline.Checked = AdvancedSettings.GetBooleanSetting("Do.Outline", True)
        Me._setup.tPlot.Checked = AdvancedSettings.GetBooleanSetting("Do.Plot", True)
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Prefix = "Nuno_"
        SPanel.Type = Master.eLang.GetString(36, "Movies", True)
        SPanel.ImageIndex = If(Me._ScraperEnabled, 9, 10)
        SPanel.Order = 110
        SPanel.Parent = "pnlMovieData"
        SPanel.Panel = _setup.pnlSettings
        Return SPanel
    End Function

    Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupPostScraper
    End Sub

    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupScraper
        ScraperEnabled = Me._setup.chkEnabled.Checked
        AdvancedSettings.SetSetting("Language", Me._setup.cLanguage.Text)
        AdvancedSettings.SetBooleanSetting("Do.Outline", Me._setup.tOutline.Checked)
        AdvancedSettings.SetBooleanSetting("Do.Plot", Me._setup.tPlot.Checked)
        AdvancedSettings.SetBooleanSetting("Do.Subs", Me._setup.cAddSubMetadata.Checked)
    End Sub

    #End Region 'Methods

End Class