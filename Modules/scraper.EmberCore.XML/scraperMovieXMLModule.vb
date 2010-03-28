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
Imports System.Reflection
Imports System.Drawing.Imaging

Public Class EmberXMLScraperModule
    Implements Interfaces.EmberMovieScraperModule

#Region "Fields"

    Private _Name As String = "Ember XML Movie Scrapers"
    Private _PostScraperEnabled As Boolean = False
    Private _ScraperEnabled As Boolean = False
    Private _setup As frmXMLSettingsHolder
    Private XMLManager As ScraperManager = Nothing
    Private scraperName As String = String.Empty
    Private scraperFileName As String = String.Empty
    Private ScrapersLoaded As Boolean = False
    Private lMediaTag As XMLScraper.MediaTags.MediaTag
    Private LastDBMovieID As String = String.Empty
    Friend WithEvents bwPopulate As New System.ComponentModel.BackgroundWorker

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
            Return True
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
        'PrepareScraper()
    End Sub

    Sub Disabled()

    End Sub
    Function QueryPostScraperCapabilities(ByVal cap As Enums.PostScraperCapabilities) As Boolean Implements Interfaces.EmberMovieScraperModule.QueryPostScraperCapabilities

    End Function
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
        scraperFileName = AdvancedSettings.GetSetting("ScraperFileName", "")
        PrepareScraper()
        XMLManager.LoadScrapers(scraperFileName)
        LoadScraperSettings()
    End Sub

    Function InjectSetupPostScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupPostScraper
        'PrepareScraper()
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
        'PrepareScraper()
        Dim Spanel As New Containers.SettingsPanel
        _setup = New frmXMLSettingsHolder
        _setup.cbEnabled.Checked = _ScraperEnabled
        If _setup.cbScraper.Items.Count = 0 Then
            _setup.cbScraper.Items.Add(scraperName)
            _setup.cbScraper.SelectedIndex = 0
        Else
            _setup.cbScraper.SelectedIndex = _setup.cbScraper.Items.IndexOf(scraperName)
        End If
        PopulateScraperSettings()
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
        AddHandler _setup.PopulateScrapers, AddressOf PopulateSettings
        Return Spanel
    End Function

    Private Sub Handle_SetupScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
        ScraperEnabled = state
        RaiseEvent SetupScraperChanged(String.Concat(Me._Name, "Scraper"), state, difforder)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        PopulateScraperSettings()
        scraperFileName = XMLManager.AllScrapers.FirstOrDefault(Function(y) y.ScraperName = _setup.cbScraper.SelectedItem.ToString).FileName
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupPostScraper
    End Sub

    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupScraper
        ScraperEnabled = _setup.cbEnabled.Checked
        If Not _setup.cbScraper.SelectedItem Is Nothing Then
            scraperName = _setup.cbScraper.SelectedItem.ToString
            Dim s As ScraperInfo = XMLManager.AllScrapers.FirstOrDefault(Function(y) y.ScraperName = scraperName)
            If Not IsNothing(s) Then
                scraperFileName = s.FileName
                AdvancedSettings.SetSetting("ScraperName", scraperName)
                AdvancedSettings.SetSetting("ScraperFileName", scraperFileName)
                For Each ss As XMLScraper.ScraperLib.ScraperSetting In s.Settings.Where(Function(u) Not u.Hidden)
                    If ss.ID IsNot Nothing Then
                        For Each r As DataGridViewRow In _setup.dgvSettings.Rows
                            If r.Cells(1).Tag.ToString = ss.ID.ToString Then
                                AdvancedSettings.SetSetting(String.Concat(scraperFileName, ".", ss.ID.ToString), r.Cells(1).Value.ToString)
                                ss.Parameter = r.Cells(1).Value.ToString
                                Exit For
                            End If
                        Next
                    End If
                Next
            End If
        End If
        'ModulesManager.Instance.SaveSettings()
        If DoDispose Then
            RemoveHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
            RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            RemoveHandler _setup.PopulateScrapers, AddressOf PopulateSettings
            _setup.Dispose()
        End If
    End Sub
    Sub LoadScraperSettings()
        Dim s As ScraperInfo = XMLManager.AllScrapers.FirstOrDefault(Function(y) y.ScraperName = scraperName)
        If Not s Is Nothing Then
            For Each ss As XMLScraper.ScraperLib.ScraperSetting In s.Settings.Where(Function(u) Not u.Hidden)
                ss.Parameter = AdvancedSettings.GetSetting(String.Concat(scraperFileName, ".", ss.ID.ToString), ss.Default)
            Next
        End If
    End Sub
    Function Scraper(ByRef DBMovie As Structures.DBMovie, ByRef ScrapeType As Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.Scraper
        Try
            'PrepareScraper()
            If Not ScrapersLoaded AndAlso Not String.IsNullOrEmpty(scraperFileName) Then
                'XMLManager.ReloadScrapers()
                XMLManager.LoadScrapers(scraperFileName)
                LoadScraperSettings()
                PoupulateForm()
                ScrapersLoaded = True
            End If
            If scraperName = String.Empty Then
                ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(998, "XML Scraper"), String.Format(Master.eLang.GetString(998, "No XML Scraper Defined {0}."), vbNewLine), Nothing}))
                Return New Interfaces.ModuleResult With {.breakChain = False}
            End If
            Dim res As New List(Of XMLScraper.ScraperLib.ScrapeResultsEntity)

            If Master.GlobalScrapeMod.NFO Then

                If ScrapeType = Enums.ScrapeType.SingleScrape AndAlso Master.GlobalScrapeMod.DoSearch Then
                    DBMovie.ClearExtras = True
                    DBMovie.PosterPath = String.Empty
                    DBMovie.FanartPath = String.Empty
                    DBMovie.TrailerPath = String.Empty
                    DBMovie.ExtraPath = String.Empty
                    DBMovie.SubPath = String.Empty
                    DBMovie.NfoPath = String.Empty
                    DBMovie.Movie.Clear()
                End If
                Select Case ScrapeType
                    Case Enums.ScrapeType.FilterAuto, Enums.ScrapeType.FullAuto, Enums.ScrapeType.MarkAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.UpdateAuto
                        If res.Count > 0 Then
                            ' Get first and go
                            lMediaTag = XMLManager.GetDetails(res(0))
                            MapFields(DBMovie, DirectCast(lMediaTag, XMLScraper.MediaTags.MovieTag), Options)
                        End If
                    Case Else
                        res = XMLManager.GetResults(scraperName, DBMovie.Movie.Title, DBMovie.Movie.Year, XMLScraper.ScraperLib.MediaType.movie)
                        If res.Count > 0 Then
                            ' search Dialog
                            Using dlg As New dlgSearchResults
                                dlg.XMLManager = XMLManager
                                If dlg.ShowDialog(res, DBMovie.Movie.Title) = Windows.Forms.DialogResult.OK Then
                                    lMediaTag = XMLManager.GetDetails(res(dlg.SelectIdx))
                                    MapFields(DBMovie, DirectCast(lMediaTag, XMLScraper.MediaTags.MovieTag), Options)
                                Else
                                    Return New Interfaces.ModuleResult With {.breakChain = False, .Cancelled = True}
                                End If
                            End Using
                        End If
                End Select
            End If
        Catch ex As Exception
        End Try
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function
    Sub MapFields(ByRef DBMovie As Structures.DBMovie, ByVal lMediaTag As XMLScraper.MediaTags.MovieTag, ByVal Options As Structures.ScrapeOptions)
        If Options.bCert Then
            If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then DBMovie.Movie.Certification = (lMediaTag.Certifications.FirstOrDefault(Function(y) y.StartsWith(Master.eSettings.CertificationLang)))
            If Not DBMovie.Movie.Certification Is Nothing AndAlso DBMovie.Movie.Certification.IndexOf("(") >= 0 Then DBMovie.Movie.Certification = DBMovie.Movie.Certification.Substring(0, DBMovie.Movie.Certification.IndexOf("("))
        End If
        If Options.bDirector Then DBMovie.Movie.Director = Strings.Join(lMediaTag.Directors.ToArray(), " / ")
        If Options.bGenre Then DBMovie.Movie.Genre = Strings.Join(lMediaTag.Genres.ToArray(), " / ")
        If Options.bMPAA Then DBMovie.Movie.MPAA = lMediaTag.MPAA
        If Options.bPlot Then DBMovie.Movie.Plot = lMediaTag.Plot
        If Options.bOutline Then DBMovie.Movie.Outline = lMediaTag.Outline
        DBMovie.Movie.PlayCount = lMediaTag.PlayCount.ToString
        If Options.bRelease Then DBMovie.Movie.ReleaseDate = lMediaTag.Premiered
        If Options.bRating Then DBMovie.Movie.Rating = lMediaTag.Rating.ToString
        If Options.bRuntime Then DBMovie.Movie.Runtime = lMediaTag.Runtime
        'DBMovie.Movie.Sets = lMediaTag.Sets
        If Options.bStudio Then DBMovie.Movie.Studio = lMediaTag.Studio
        If Options.bTagline Then DBMovie.Movie.Tagline = lMediaTag.Tagline
        If Options.bTitle Then DBMovie.Movie.Title = lMediaTag.Title
        For Each t As XMLScraper.MediaTags.Thumbnail In lMediaTag.Thumbs
            DBMovie.Movie.Thumb.Add(t.Thumb)
        Next
        If Options.bTop250 Then DBMovie.Movie.Top250 = lMediaTag.Top250.ToString
        'DBMovie.Movie.Trailer = lMediaTag.Trailers
        If Options.bVotes Then DBMovie.Movie.Votes = lMediaTag.Votes.ToString
        If Options.bWriters Then DBMovie.Movie.Credits = Strings.Join(lMediaTag.Writers.ToArray, " / ")
        If Options.bYear Then DBMovie.Movie.Year = lMediaTag.Year.ToString
        DBMovie.Movie.ID = If(lMediaTag.ID.StartsWith("tt"), lMediaTag.ID, String.Empty)
        If Options.bCast Then
            For Each p As XMLScraper.MediaTags.PersonTag In lMediaTag.Actors
                Dim person As New MediaContainers.Person
                person.Name = p.Name
                person.Role = p.Role
                person.Thumb = p.Thumb.Thumb
                DBMovie.Movie.Actors.Add(person)
            Next
        End If
    End Sub
    Sub PopulateScraperSettings()
        Try
            If Not _setup.cbScraper.SelectedItem Is Nothing Then
                scraperName = _setup.cbScraper.SelectedItem.ToString
                scraperFileName = XMLManager.AllScrapers.FirstOrDefault(Function(y) y.ScraperName = _setup.cbScraper.SelectedItem.ToString).FileName
                Dim s As ScraperInfo = XMLManager.AllScrapers.FirstOrDefault(Function(y) y.ScraperName = scraperName)
                If Not s Is Nothing Then
                    _setup.dgvSettings.Rows.Clear()
                    For Each ss As XMLScraper.ScraperLib.ScraperSetting In s.Settings.Where(Function(u) Not u.Hidden)
                        If Not ss.Label Is Nothing Then

                            If ss.Values.Count > 0 Then
                                Dim i As Integer = _setup.dgvSettings.Rows.Add(ss.Label.ToString)
                                Dim dcb As DataGridViewComboBoxCell = DirectCast(_setup.dgvSettings.Rows(i).Cells(1), DataGridViewComboBoxCell)
                                dcb.DataSource = ss.Values.ToArray
                                dcb.Value = ss.Parameter.ToString
                                dcb.Tag = ss.ID.ToString
                            Else
                                Select Case ss.Type
                                    Case XMLScraper.ScraperLib.ScraperSetting.ScraperSettingType.bool
                                        Dim i As Integer = _setup.dgvSettings.Rows.Add(ss.Label.ToString)
                                        Dim dcb As DataGridViewComboBoxCell = DirectCast(_setup.dgvSettings.Rows(i).Cells(1), DataGridViewComboBoxCell)
                                        dcb.DataSource = New String() {"true", "false"}
                                        dcb.Value = ss.Parameter.ToString
                                        dcb.Tag = ss.ID.ToString
                                    Case Else
                                        Dim i As Integer = _setup.dgvSettings.Rows.Add(ss.Label.ToString)
                                End Select
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Function SelectImageOfType(ByRef mMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.SelectImageOfType
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Sub PopulateSettings()
        bwPopulate.WorkerReportsProgress = True
        bwPopulate.RunWorkerAsync()
    End Sub
    Private Sub bwPopulate_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPopulate.DoWork
        XMLManager.ReloadScrapers()
        ScrapersLoaded = True
    End Sub
    Private Sub bwPopulate_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPopulate.RunWorkerCompleted
        PoupulateForm()
        _setup.parentRunning = False
    End Sub
    Delegate Sub DelegateFunction()
    Sub PoupulateForm()
        If _setup Is Nothing Then Return
        If _setup.InvokeRequired Then
            _setup.Invoke(New DelegateFunction(AddressOf PoupulateForm))
            Return
        End If
        PopulateScraperSettings()
        _setup.cbScraper.Items.Clear()
        For Each s As ScraperInfo In XMLManager.AllScrapers
            _setup.cbScraper.Items.Add(s.ScraperName)
        Next
        If _setup.cbScraper.Items.Count = 0 Then
            _setup.cbScraper.Items.Add(scraperName)
            _setup.cbScraper.SelectedIndex = 0
        Else
            _setup.cbScraper.SelectedIndex = _setup.cbScraper.Items.IndexOf(scraperName)
        End If
    End Sub
    Sub PrepareScraper()
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

        End If
    End Sub

#End Region 'Methods

End Class