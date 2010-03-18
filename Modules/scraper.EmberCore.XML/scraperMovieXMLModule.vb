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
Imports EmberAPI
Imports EmberScraperModule.XMLScraper.ScraperXML

Public Class EmberXMLScraperModule
    Implements Interfaces.EmberMovieScraperModule

    #Region "Fields"

    Private _Name As String = "Ember XML Movie Scrapers"
    Private _PostScraperEnabled As Boolean = False
    Private _ScraperEnabled As Boolean = False
    Private _setup As frmXMLSettingsHolder
    Private XMLManager As New ScraperManager
    Private scraperName As String = String.Empty
    #End Region 'Fields

    #Region "Events"

    Public Event ModuleSettingsChanged() Implements Interfaces.EmberMovieScraperModule.ModuleSettingsChanged

    'Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements Interfaces.EmberMovieScraperModule.MovieScraperEvent
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
            Return "Ember XML Scraper"
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
            If _ScraperEnabled Then
                Enabled()
            Else
                Disabled()
            End If
        End Set
    End Property

    #End Region 'Properties

    #Region "Methods"
    Sub Enabled()
        If XMLManager Is Nothing Then
            Dim tPath As String = Path.Combine(Functions.AppPath, String.Concat("Modules", Path.DirectorySeparatorChar, "XBMC-XML"))
            Dim cPath As String = Path.Combine(Functions.AppPath, String.Concat("Modules", Path.DirectorySeparatorChar, "XBMC-XML", Path.DirectorySeparatorChar, "Cache"))
            If Not Directory.Exists(tPath) Then
                Directory.CreateDirectory(tPath)
            End If
            If Not Directory.Exists(cPath) Then
                Directory.CreateDirectory(cPath)
            End If

            XMLManager = New ScraperManager(tPath, cPath)
            XMLManager.ReloadScrapers()
        End If
    End Sub

    Sub Disabled()

    End Sub
    Public Function PostScraper(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.PostScraper
    End Function

    Function DownloadTrailer(ByRef DBMovie As Structures.DBMovie, ByRef sURL As String) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.DownloadTrailer
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Function GetMovieStudio(ByRef DBMovie As Structures.DBMovie, ByRef sStudio As List(Of String)) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.GetMovieStudio
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberMovieScraperModule.Init
        scraperName = AdvancedSettings.GetSetting("ScraperName", "NFO Scraper")
    End Sub

    Function InjectSetupPostScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupPostScraper
        Dim Spanel As New Containers.SettingsPanel
        Spanel.Name = String.Concat(Me._Name, "PostScraper")
        Spanel.Text = Me._Name
        Spanel.Prefix = "XMLMovieMedia_"
        Spanel.Order = 110
        Spanel.Parent = "pnlMovieMedia"
        Spanel.Type = Master.eLang.GetString(36, "Movies")
        Spanel.ImageIndex = If(Me._PostScraperEnabled, 9, 10)
        Spanel.Panel = New Panel 'Me._setupPost.pnlSettings
        Return Spanel
    End Function

    Function InjectSetupScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupScraper
        Dim Spanel As New Containers.SettingsPanel
        _setup = New frmXMLSettingsHolder
        _setup.cbEnabled.Checked = _ScraperEnabled
        PopulateSettings()
        Spanel.Name = String.Concat(Me._Name, "Scraper")
        Spanel.Text = _Name
        Spanel.Prefix = "XMLMovieInfo_"
        Spanel.Order = 110
        Spanel.Parent = "pnlMovieData"
        Spanel.Type = Master.eLang.GetString(36, "Movies")
        Spanel.ImageIndex = If(_ScraperEnabled, 9, 10)
        Spanel.Panel = _setup.pnlSettings
        AddHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return Spanel
    End Function

    Private Sub Handle_SetupScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent SetupScraperChanged(String.Concat(Me._Name, "Scraper"), state, difforder)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupPostScraper
    End Sub

    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupScraper
        ScraperEnabled = _setup.cbEnabled.Checked
        scraperName = _setup.cbScraper.SelectedItem.ToString
        AdvancedSettings.SetSetting("ScraperName", scraperName)
        ModulesManager.Instance.SaveSettings()
        If DoDispose Then
            RemoveHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
            RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

    Function Scraper(ByRef DBMovie As Structures.DBMovie, ByRef ScrapeType As Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.Scraper
        If scraperName = String.Empty Then
            ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(998, "XML Scraper"), String.Format(Master.eLang.GetString(998, "No Scraper Defined {0}."), vbNewLine), Nothing}))
        Else
            Dim res As New List(Of XMLScraper.ScraperLib.ScrapeResultsEntity)
            res = XMLManager.GetResults(scraperName, DBMovie.Movie.Title, DBMovie.Movie.Year, XMLScraper.ScraperLib.MediaType.movie)
        End If
    End Function

    Function SelectImageOfType(ByRef mMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.SelectImageOfType
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Sub PopulateSettings()
        _setup.cbScraper.Items.Clear()
        For Each s As ScraperInfo In XMLManager.AllScrapers
            _setup.cbScraper.Items.Add(s.ScraperName)
        Next
    End Sub

    #End Region 'Methods

End Class