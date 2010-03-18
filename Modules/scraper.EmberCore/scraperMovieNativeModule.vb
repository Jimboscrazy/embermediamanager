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

''' <summary>
''' Native Scraper
''' </summary>
''' <remarks></remarks>
Public Class EmberNativeScraperModule
    Implements Interfaces.EmberMovieScraperModule

    #Region "Fields"

    Public Shared ConfigOptions As New Structures.ScrapeOptions
    Public Shared ConfigScrapeModifier As New Structures.ScrapeModifier
    Public Shared _AssemblyName As String

    Private dFImgSelect As dlgImgSelect = Nothing

    ''' <summary>
    ''' Scraping Here
    ''' </summary>
    ''' <remarks></remarks>
    Private IMDB As New IMDB.Scraper
    Private MySettings As New _MySettings
    Private _Name As String = "Ember Native Movie Scrapers"
    Private _PostScraperEnabled As Boolean = False
    Private _ScraperEnabled As Boolean = False
    Private _setup As frmInfoSettingsHolder
    Private _setupPost As frmMediaSettingsHolder

    #End Region 'Fields

    #Region "Events"

    Public Event ModuleSettingsChanged() Implements Interfaces.EmberMovieScraperModule.ModuleSettingsChanged

    'Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements Interfaces.EmberMovieScraperModule.MovieScraperEvent
    Public Event MovieScraperEvent(ByVal eType As Enums.MovieScraperEventType, ByVal Parameter As Object) Implements Interfaces.EmberMovieScraperModule.MovieScraperEvent

    Public Event SetupPostScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberMovieScraperModule.PostScraperSetupChanged

    Public Event SetupScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberMovieScraperModule.ScraperSetupChanged

    #End Region 'Events

    #Region "Properties"

    ReadOnly Property IsPostScraper() As Boolean Implements Interfaces.EmberMovieScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property

    ReadOnly Property IsScraper() As Boolean Implements Interfaces.EmberMovieScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property

    ReadOnly Property ModuleName() As String Implements Interfaces.EmberMovieScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property

    ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberMovieScraperModule.ModuleVersion
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

    Function DownloadTrailer(ByRef DBMovie As Structures.DBMovie, ByRef sURL As String) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.DownloadTrailer
        Using dTrailer As New dlgTrailer
            sURL = dTrailer.ShowDialog(DBMovie.Movie.IMDBID, DBMovie.Filename)
        End Using
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Function GetMovieStudio(ByRef DBMovie As Structures.DBMovie, ByRef studio As List(Of String)) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.GetMovieStudio
        Dim IMDB As New IMDB.Scraper
        IMDB.UseOFDBTitle = MySettings.UseOFDBTitle
        IMDB.UseOFDBOutline = MySettings.UseOFDBOutline
        IMDB.UseOFDBPlot = MySettings.UseOFDBPlot
        IMDB.UseOFDBGenre = MySettings.UseOFDBGenre
        IMDB.IMDBURL = MySettings.IMDBURL
        studio = IMDB.GetMovieStudios(DBMovie.Movie.IMDBID)
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub Handle_PostModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub Handle_SetupPostScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
        '_PostScraperEnabled = _setup.cbEnabled.Checked
        RaiseEvent SetupPostScraperChanged(String.Concat(Me._Name, "PostScraper"), state, difforder)
    End Sub

    Private Sub Handle_SetupScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent SetupScraperChanged(String.Concat(Me._Name, "Scraper"), state, difforder)
    End Sub

    Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberMovieScraperModule.Init
        _AssemblyName = sAssemblyName
        LoadSettings()
    End Sub

    Function InjectSetupPostScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupPostScraper
        Dim Spanel As New Containers.SettingsPanel
        _setupPost = New frmMediaSettingsHolder
        LoadSettings()
        _setupPost.cbEnabled.Checked = _PostScraperEnabled
        If Master.eSettings.TrailerSites.Count > 0 Then
            For Each iTrailer As Integer In Master.eSettings.TrailerSites
                _setupPost.lbTrailerSites.SetItemChecked(iTrailer, True)
            Next
        End If
        _setupPost.chkScrapePoster.Checked = ConfigScrapeModifier.Poster
        _setupPost.chkScrapeFanart.Checked = ConfigScrapeModifier.Fanart
        _setupPost.chkUseTMDB.Checked = Master.eSettings.UseTMDB
        _setupPost.chkUseIMPA.Checked = Master.eSettings.UseIMPA
        _setupPost.chkUseMPDB.Checked = Master.eSettings.UseMPDB
        _setupPost.txtTimeout.Text = Master.eSettings.TrailerTimeout.ToString
        _setupPost.chkDownloadTrailer.Checked = MySettings.DownloadTrailers
        _setupPost.CheckTrailer()
        _setupPost.orderChanged()
        Spanel.Name = String.Concat(Me._Name, "PostScraper")
        Spanel.Text = Me._Name
        Spanel.Prefix = "NativeMovieMedia_"
        Spanel.Order = 110
        Spanel.Parent = "pnlMovieMedia"
        Spanel.Type = Master.eLang.GetString(36, "Movies")
        Spanel.ImageIndex = If(Me._PostScraperEnabled, 9, 10)
        Spanel.Panel = Me._setupPost.pnlSettings

        AddHandler _setupPost.SetupPostScraperChanged, AddressOf Handle_SetupPostScraperChanged
        AddHandler _setupPost.ModuleSettingsChanged, AddressOf Handle_PostModuleSettingsChanged
        Return SPanel
    End Function

    Function InjectSetupScraper() As Containers.SettingsPanel Implements Interfaces.EmberMovieScraperModule.InjectSetupScraper
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmInfoSettingsHolder
        LoadSettings()
        _setup.cbEnabled.Checked = _ScraperEnabled
        _setup.chkTitle.Checked = ConfigOptions.bTitle
        _setup.chkYear.Checked = ConfigOptions.bYear
        _setup.chkMPAA.Checked = ConfigOptions.bMPAA
        _setup.chkRelease.Checked = ConfigOptions.bRelease
        _setup.chkRuntime.Checked = ConfigOptions.bRuntime
        _setup.chkRating.Checked = ConfigOptions.bRating
        _setup.chkVotes.Checked = ConfigOptions.bVotes
        _setup.chkStudio.Checked = ConfigOptions.bStudio
        _setup.chkTagline.Checked = ConfigOptions.bTagline
        _setup.chkOutline.Checked = ConfigOptions.bOutline
        _setup.chkPlot.Checked = ConfigOptions.bPlot
        _setup.chkCast.Checked = ConfigOptions.bCast
        _setup.chkDirector.Checked = ConfigOptions.bDirector
        _setup.chkWriters.Checked = ConfigOptions.bWriters
        _setup.chkProducers.Checked = ConfigOptions.bProducers
        _setup.chkGenre.Checked = ConfigOptions.bGenre
        _setup.chkTrailer.Checked = ConfigOptions.bTrailer
        _setup.chkMusicBy.Checked = ConfigOptions.bMusicBy
        _setup.chkCrew.Checked = ConfigOptions.bOtherCrew
        _setup.chkTop250.Checked = ConfigOptions.bTop250
        _setup.chkCertification.Checked = ConfigOptions.bCert
        _setup.chkOFDBTitle.Checked = MySettings.UseOFDBTitle
        _setup.chkOFDBOutline.Checked = MySettings.UseOFDBOutline
        _setup.chkOFDBPlot.Checked = MySettings.UseOFDBPlot
        _setup.chkOFDBGenre.Checked = MySettings.UseOFDBGenre

        If String.IsNullOrEmpty(MySettings.IMDBURL) Then
            MySettings.IMDBURL = "akas.imdb.com"
        End If
        _setup.txtIMDBURL.Text = MySettings.IMDBURL
        _setup.orderChanged()
        SPanel.Name = String.Concat(Me._Name, "Scraper")
        SPanel.Text = _Name
        SPanel.Prefix = "NativeMovieInfo_"
        SPanel.Order = 110
        SPanel.Parent = "pnlMovieData"
        SPanel.Type = Master.eLang.GetString(36, "Movies")
        SPanel.ImageIndex = If(_ScraperEnabled, 9, 10)
        SPanel.Panel = _setup.pnlSettings
        AddHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Sub LoadSettings()
        ConfigOptions.bTitle = AdvancedSettings.GetBooleanSetting("DoTitle", True)
        ConfigOptions.bYear = AdvancedSettings.GetBooleanSetting("DoYear", True)
        ConfigOptions.bMPAA = AdvancedSettings.GetBooleanSetting("DoMPAA", True)
        ConfigOptions.bRelease = AdvancedSettings.GetBooleanSetting("DoRelease", True)
        ConfigOptions.bRuntime = AdvancedSettings.GetBooleanSetting("DoRuntime", True)
        ConfigOptions.bRating = AdvancedSettings.GetBooleanSetting("DoRating", True)
        ConfigOptions.bVotes = AdvancedSettings.GetBooleanSetting("DoVotes", True)
        ConfigOptions.bStudio = AdvancedSettings.GetBooleanSetting("DoStudio", True)
        ConfigOptions.bTagline = AdvancedSettings.GetBooleanSetting("DoTagline", True)
        ConfigOptions.bOutline = AdvancedSettings.GetBooleanSetting("DoOutline", True)
        ConfigOptions.bPlot = AdvancedSettings.GetBooleanSetting("DoPlot", True)
        ConfigOptions.bCast = AdvancedSettings.GetBooleanSetting("DoCast", True)
        ConfigOptions.bDirector = AdvancedSettings.GetBooleanSetting("DoDirector", True)
        ConfigOptions.bWriters = AdvancedSettings.GetBooleanSetting("DoWriters", True)
        ConfigOptions.bProducers = AdvancedSettings.GetBooleanSetting("DoProducers", True)
        ConfigOptions.bGenre = AdvancedSettings.GetBooleanSetting("DoGenres", True)
        ConfigOptions.bTrailer = AdvancedSettings.GetBooleanSetting("DoTrailer", True)
        ConfigOptions.bMusicBy = AdvancedSettings.GetBooleanSetting("DoMusic", True)
        ConfigOptions.bOtherCrew = AdvancedSettings.GetBooleanSetting("DoOtherCrews", True)
        ConfigOptions.bTop250 = AdvancedSettings.GetBooleanSetting("DoTop250", True)
        ConfigOptions.bCert = AdvancedSettings.GetBooleanSetting("DoCert", True)
        MySettings.IMDBURL = AdvancedSettings.GetSetting("IMDBURL", "akas.imdb.com")
        MySettings.UseOFDBTitle = AdvancedSettings.GetBooleanSetting("UseOFDBTitle", False)
        MySettings.UseOFDBOutline = AdvancedSettings.GetBooleanSetting("UseOFDBOutline", False)
        MySettings.UseOFDBPlot = AdvancedSettings.GetBooleanSetting("UseOFDBPlot", False)
        MySettings.UseOFDBGenre = AdvancedSettings.GetBooleanSetting("UseOFDBGenre", False)
        MySettings.DownloadTrailers = AdvancedSettings.GetBooleanSetting("DownloadTraliers", False)
        ConfigScrapeModifier.DoSearch = True
        ConfigScrapeModifier.Meta = True
        ConfigScrapeModifier.NFO = True
        ConfigScrapeModifier.Extra = True

        ConfigScrapeModifier.Poster = AdvancedSettings.GetBooleanSetting("DoPoster", True)
        ConfigScrapeModifier.Fanart = AdvancedSettings.GetBooleanSetting("DoFanart", True)
        ConfigScrapeModifier.Trailer = AdvancedSettings.GetBooleanSetting("DoTrailer", True)
    End Sub

    Function PostScraper(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.PostScraper
        'LoadSettings()
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim pResults As Containers.ImgResult
        Dim fResults As Containers.ImgResult
        Dim tURL As String = String.Empty
        Dim Trailer As New Trailers
        LoadSettings()
        Dim saveModifier As Structures.ScrapeModifier = Master.GlobalScrapeMod
        Master.GlobalScrapeMod = Functions.ScrapeModifierAndAlso(Master.GlobalScrapeMod, ConfigScrapeModifier)

        Trailer.IMDBURL = MySettings.IMDBURL
        Dim doSave As Boolean = False
        If Master.GlobalScrapeMod.Poster Then
            Poster.Clear()
            If Poster.IsAllowedToDownload(DBMovie, Enums.ImageType.Posters) Then
                pResults = New Containers.ImgResult
                If ScrapeImages.GetPreferredImage(Poster, DBMovie.Movie.IMDBID, Enums.ImageType.Posters, pResults, DBMovie.Filename, False, If(ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk OrElse ScrapeType = Enums.ScrapeType.UpdateAsk, True, False)) Then
                    If Not IsNothing(Poster.Image) Then
                        pResults.ImagePath = Poster.SaveAsPoster(DBMovie)
                        If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                            DBMovie.PosterPath = pResults.ImagePath
                            RaiseEvent MovieScraperEvent(Enums.MovieScraperEventType.PosterItem, True) '4, True)
                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                DBMovie.Movie.Thumb = pResults.Posters
                            End If
                        End If
                    ElseIf ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk OrElse ScrapeType = Enums.ScrapeType.UpdateAsk Then
                        MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.IMDBURL = MySettings.IMDBURL
                            pResults = dImgSelect.ShowDialog(DBMovie, Enums.ImageType.Posters)
                            If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                DBMovie.PosterPath = pResults.ImagePath
                                RaiseEvent MovieScraperEvent(Enums.MovieScraperEventType.PosterItem, True) '4, True)
                                If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                    DBMovie.Movie.Thumb = pResults.Posters
                                End If
                            End If
                        End Using
                    End If
                End If
            End If
        End If
        Dim didEts As Boolean
        If Master.GlobalScrapeMod.Fanart Then
            Fanart.Clear()
            If Fanart.IsAllowedToDownload(DBMovie, Enums.ImageType.Fanart) Then
                fResults = New Containers.ImgResult
                didEts = True
                If ScrapeImages.GetPreferredImage(Fanart, DBMovie.Movie.IMDBID, Enums.ImageType.Fanart, fResults, DBMovie.Filename, Master.GlobalScrapeMod.Extra, If(ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk OrElse ScrapeType = Enums.ScrapeType.UpdateAsk, True, False)) Then
                    If Not IsNothing(Fanart.Image) Then
                        fResults.ImagePath = Fanart.SaveAsFanart(DBMovie)
                        If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                            DBMovie.FanartPath = fResults.ImagePath
                            RaiseEvent MovieScraperEvent(Enums.MovieScraperEventType.FanartItem, True) '
                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                DBMovie.Movie.Fanart = fResults.Fanart
                            End If
                        End If
                    ElseIf ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk OrElse ScrapeType = Enums.ScrapeType.UpdateAsk Then
                        MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))

                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.IMDBURL = MySettings.IMDBURL
                            fResults = dImgSelect.ShowDialog(DBMovie, Enums.ImageType.Fanart)
                            If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                DBMovie.FanartPath = fResults.ImagePath
                                RaiseEvent MovieScraperEvent(Enums.MovieScraperEventType.FanartItem, True)
                                If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                    DBMovie.Movie.Fanart = fResults.Fanart
                                End If
                            End If
                        End Using
                    End If
                End If
            End If
        End If
        If Master.GlobalScrapeMod.Trailer Then
            tURL = Trailer.DownloadSingleTrailer(DBMovie.Filename, DBMovie.Movie.IMDBID, DBMovie.isSingle, DBMovie.Movie.Trailer)
            If Not String.IsNullOrEmpty(tURL) Then
                If tURL.Substring(0, 7) = "http://" Then
                    DBMovie.Movie.Trailer = tURL
                    'doSave = True
                Else
                    DBMovie.TrailerPath = tURL
                    RaiseEvent MovieScraperEvent(Enums.MovieScraperEventType.TrailerItem, True)
                End If
            End If
        End If
        Master.GlobalScrapeMod = saveModifier
        Return New Interfaces.ModuleResult With {.breakChain = False, .BoolProperty = didEts}
    End Function

    Sub SaveSettings()
        AdvancedSettings.SetBooleanSetting("DoTitle", ConfigOptions.bTitle)
        AdvancedSettings.SetBooleanSetting("DoYear", ConfigOptions.bYear)
        AdvancedSettings.SetBooleanSetting("DoMPAA", ConfigOptions.bMPAA)
        AdvancedSettings.SetBooleanSetting("DoRelease", ConfigOptions.bRelease)
        AdvancedSettings.SetBooleanSetting("DoRuntime", ConfigOptions.bRuntime)
        AdvancedSettings.SetBooleanSetting("DoRating", ConfigOptions.bRating)
        AdvancedSettings.SetBooleanSetting("DoVotes", ConfigOptions.bVotes)
        AdvancedSettings.SetBooleanSetting("DoStudio", ConfigOptions.bStudio)
        AdvancedSettings.SetBooleanSetting("DoTagline", ConfigOptions.bTagline)
        AdvancedSettings.SetBooleanSetting("DoOutline", ConfigOptions.bOutline)
        AdvancedSettings.SetBooleanSetting("DoPlot", ConfigOptions.bPlot)
        AdvancedSettings.SetBooleanSetting("DoCast", ConfigOptions.bCast)
        AdvancedSettings.SetBooleanSetting("DoDirector", ConfigOptions.bDirector)
        AdvancedSettings.SetBooleanSetting("DoWriters", ConfigOptions.bWriters)
        AdvancedSettings.SetBooleanSetting("DoProducers", ConfigOptions.bProducers)
        AdvancedSettings.SetBooleanSetting("DoGenres", ConfigOptions.bGenre)
        AdvancedSettings.SetBooleanSetting("DoTrailer", ConfigOptions.bTrailer)
        AdvancedSettings.SetBooleanSetting("DoMusic", ConfigOptions.bMusicBy)
        AdvancedSettings.SetBooleanSetting("DoOtherCrews", ConfigOptions.bOtherCrew)
        AdvancedSettings.SetBooleanSetting("DoTop250", ConfigOptions.bTop250)
        AdvancedSettings.SetBooleanSetting("DoCert", ConfigOptions.bCert)
        AdvancedSettings.SetSetting("IMDBURL", MySettings.IMDBURL)
        AdvancedSettings.SetBooleanSetting("UseOFDBTitle", MySettings.UseOFDBTitle)
        AdvancedSettings.SetBooleanSetting("UseOFDBOutline", MySettings.UseOFDBOutline)
        AdvancedSettings.SetBooleanSetting("UseOFDBPlot", MySettings.UseOFDBPlot)
        AdvancedSettings.SetBooleanSetting("UseOFDBGenre", MySettings.UseOFDBGenre)

        AdvancedSettings.SetBooleanSetting("DownloadTraliers", MySettings.DownloadTrailers)
        AdvancedSettings.SetBooleanSetting("DoPoster", ConfigScrapeModifier.Poster)
        AdvancedSettings.SetBooleanSetting("DoFanart", ConfigScrapeModifier.Fanart)
        AdvancedSettings.SetBooleanSetting("DoTrailer", ConfigScrapeModifier.Trailer)
    End Sub

    Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupPostScraper
        MySettings.DownloadTrailers = _setupPost.chkDownloadTrailer.Checked
        ' TODO: this should move to Advanced Serttings (Modules) OR SPECIFIC settings file
        For Each iTrailer As Integer In _setupPost.lbTrailerSites.CheckedIndices
            Master.eSettings.TrailerSites.Add(DirectCast(iTrailer, Enums.TrailerPages))
        Next
        Master.eSettings.TrailerTimeout = Convert.ToInt32(_setupPost.txtTimeout.Text)
        Master.eSettings.UseTMDB = _setupPost.chkUseTMDB.Checked
        Master.eSettings.UseIMPA = _setupPost.chkUseIMPA.Checked
        Master.eSettings.UseMPDB = _setupPost.chkUseMPDB.Checked
        ConfigScrapeModifier.Poster = _setupPost.chkScrapePoster.Checked
        ConfigScrapeModifier.Fanart = _setupPost.chkScrapeFanart.Checked
        SaveSettings()
        If DoDispose Then
            RemoveHandler _setupPost.SetupPostScraperChanged, AddressOf Handle_SetupPostScraperChanged
            RemoveHandler _setupPost.ModuleSettingsChanged, AddressOf Handle_PostModuleSettingsChanged
            _setupPost.Dispose()
        End If
    End Sub

    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberMovieScraperModule.SaveSetupScraper
        If Not String.IsNullOrEmpty(_setup.txtIMDBURL.Text) Then
            MySettings.IMDBURL = Strings.Replace(_setup.txtIMDBURL.Text, "http://", String.Empty)
        Else
            MySettings.IMDBURL = "akas.imdb.com"
        End If
        MySettings.UseOFDBTitle = _setup.chkOFDBTitle.Checked
        MySettings.UseOFDBOutline = _setup.chkOFDBOutline.Checked
        MySettings.UseOFDBPlot = _setup.chkOFDBPlot.Checked
        MySettings.UseOFDBGenre = _setup.chkOFDBGenre.Checked
        ConfigOptions.bTitle = _setup.chkTitle.Checked
        ConfigOptions.bYear = _setup.chkYear.Checked
        ConfigOptions.bMPAA = _setup.chkMPAA.Checked
        ConfigOptions.bRelease = _setup.chkRelease.Checked
        ConfigOptions.bRuntime = _setup.chkRuntime.Checked
        ConfigOptions.bRating = _setup.chkRating.Checked
        ConfigOptions.bVotes = _setup.chkVotes.Checked
        ConfigOptions.bStudio = _setup.chkStudio.Checked
        ConfigOptions.bTagline = _setup.chkTagline.Checked
        ConfigOptions.bOutline = _setup.chkOutline.Checked
        ConfigOptions.bPlot = _setup.chkPlot.Checked
        ConfigOptions.bCast = _setup.chkCast.Checked
        ConfigOptions.bDirector = _setup.chkDirector.Checked
        ConfigOptions.bWriters = _setup.chkWriters.Checked
        ConfigOptions.bProducers = _setup.chkProducers.Checked
        ConfigOptions.bGenre = _setup.chkGenre.Checked
        ConfigOptions.bTrailer = _setup.chkTrailer.Checked
        ConfigOptions.bMusicBy = _setup.chkMusicBy.Checked
        ConfigOptions.bOtherCrew = _setup.chkCrew.Checked
        ConfigOptions.bTop250 = _setup.chkTop250.Checked
        ConfigOptions.bCert = _setup.chkCertification.Checked

        SaveSettings()
        ModulesManager.Instance.SaveSettings()
        If DoDispose Then
            RemoveHandler _setup.SetupScraperChanged, AddressOf Handle_SetupScraperChanged
            RemoveHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

    Function Scraper(ByRef DBMovie As Structures.DBMovie, ByRef ScrapeType As Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.Scraper
        'LoadSettings()
        IMDB.IMDBURL = MySettings.IMDBURL
        IMDB.UseOFDBTitle = MySettings.UseOFDBTitle
        IMDB.UseOFDBOutline = MySettings.UseOFDBOutline
        IMDB.UseOFDBPlot = MySettings.UseOFDBPlot
        IMDB.UseOFDBGenre = MySettings.UseOFDBGenre
        Dim tTitle As String = String.Empty
        Dim OldTitle As String = String.Empty
        If Master.GlobalScrapeMod.NFO AndAlso Not Master.GlobalScrapeMod.DoSearch Then
            If Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID) Then
                IMDB.GetMovieInfo(DBMovie.Movie.IMDBID, DBMovie.Movie, Options.bFullCrew, Options.bFullCast, False, Options)
            Else
                'Movie = IMDB.GetSearchMovieInfo(Movie.Title, Movie, Enums.ScrapeType.SingleScrape, Options)
                DBMovie.Movie = IMDB.GetSearchMovieInfo(DBMovie.Movie.Title, DBMovie.Movie, ScrapeType, Options)
            End If
        End If
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
        If String.IsNullOrEmpty(DBMovie.Movie.IMDBID) Then
            Select Case ScrapeType
                Case Enums.ScrapeType.FilterAuto, Enums.ScrapeType.FullAuto, Enums.ScrapeType.MarkAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.UpdateAuto
                    Return New Interfaces.ModuleResult With {.breakChain = False}
            End Select
            Using dSearch As New dlgIMDBSearchResults
                dSearch.IMDBURL = MySettings.IMDBURL
                Dim tmpTitle As String = DBMovie.Movie.Title
                If String.IsNullOrEmpty(tmpTitle) Then
                    tmpTitle = StringUtils.FilterName(If(DBMovie.isSingle, Directory.GetParent(DBMovie.Filename).Name, Path.GetFileNameWithoutExtension(DBMovie.Filename)))
                End If

                If dSearch.ShowDialog(tmpTitle) = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                        DBMovie.Movie.IMDBID = Master.tmpMovie.IMDBID
                    End If
                    If Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID) Then

                        Master.currMovie.ClearExtras = True
                        Master.currMovie.PosterPath = String.Empty
                        Master.currMovie.FanartPath = String.Empty
                        Master.currMovie.TrailerPath = String.Empty
                        Master.currMovie.ExtraPath = String.Empty
                        Master.currMovie.SubPath = String.Empty
                        Master.currMovie.NfoPath = String.Empty
                        Dim filterOptions As Structures.ScrapeOptions = Functions.ScrapeOptionsAndAlso(Options, ConfigOptions)

                        IMDB.GetMovieInfoAsync(DBMovie.Movie.IMDBID, DBMovie.Movie, filterOptions)
                    End If
                Else
                    Return New Interfaces.ModuleResult With {.breakChain = False, .Cancelled = True}
                End If
            End Using
        End If

        If Not String.IsNullOrEmpty(DBMovie.Movie.Title) Then
            tTitle = StringUtils.FilterTokens(DBMovie.Movie.Title)
            If Not OldTitle = DBMovie.Movie.Title OrElse String.IsNullOrEmpty(DBMovie.Movie.SortTitle) Then DBMovie.Movie.SortTitle = tTitle
            If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(DBMovie.Movie.Year) Then
                DBMovie.ListTitle = String.Format("{0} ({1})", tTitle, DBMovie.Movie.Year)
            Else
                DBMovie.ListTitle = tTitle
            End If
        Else
            If FileUtils.Common.isVideoTS(DBMovie.Filename) Then
                DBMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(DBMovie.Filename).FullName).Name)
            ElseIf FileUtils.Common.isBDRip(DBMovie.Filename) Then
                DBMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(DBMovie.Filename).FullName).FullName).Name)
            Else
                'NOTE.NOTE.NOTE what is Row 46 ??????????
                'If Convert.ToBoolean(drvRow.Item(46)) AndAlso DBMovie.isSingle Then
                If True AndAlso DBMovie.isSingle Then
                    DBMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(DBMovie.Filename).Name)
                Else
                    DBMovie.ListTitle = StringUtils.FilterName(Path.GetFileNameWithoutExtension(DBMovie.Filename))
                End If
            End If
            If Not OldTitle = DBMovie.Movie.Title OrElse String.IsNullOrEmpty(DBMovie.Movie.SortTitle) Then DBMovie.Movie.SortTitle = DBMovie.ListTitle
        End If

        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Function SelectImageOfType(ByRef mMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As Interfaces.ModuleResult Implements Interfaces.EmberMovieScraperModule.SelectImageOfType
        If preload AndAlso _DLType = Enums.ImageType.Fanart AndAlso Not IsNothing(dFImgSelect) Then
            pResults = dFImgSelect.ShowDialog()
            dFImgSelect = Nothing
        Else
            Using dImgSelect As New dlgImgSelect
                If preload Then
                    dFImgSelect = New dlgImgSelect
                    dFImgSelect.PreLoad(mMovie, Enums.ImageType.Fanart, _isEdit)
                End If
                dImgSelect.IMDBURL = MySettings.IMDBURL
                pResults = dImgSelect.ShowDialog(mMovie, _DLType, _isEdit)
            End Using
        End If
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    #End Region 'Methods

    #Region "Nested Types"

    Structure _MySettings

        #Region "Fields"

        Dim DownloadTrailers As Boolean
        Dim IMDBURL As String
        Dim UseOFDBGenre As Boolean
        Dim UseOFDBOutline As Boolean
        Dim UseOFDBPlot As Boolean
        Dim UseOFDBTitle As Boolean

        #End Region 'Fields

    End Structure

    #End Region 'Nested Types

End Class