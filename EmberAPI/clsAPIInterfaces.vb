﻿' ################################################################################
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

Public Class Interfaces
    ' Interfaces for external Modules
    Public Interface EmberMovieInputModule
        Property Enabled() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Function InjectSetup() As Containers.SettingsPanel
        Sub SaveSetup(ByVal DoDispose As Boolean)
        Sub SetupOrderChanged()
        Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Sub Init(ByVal sAssemblyName As String)
        Event ModuleSettingsChanged()
        '********************************************************************************************
        Function GetFilesFolderContents(ByRef Movie As Scanner.MovieContainer) As Boolean
        Function LoadMovieInfoSheet(ByVal sPath As String, ByVal isSingle As Boolean, ByRef mMovie As MediaContainers.Movie) As Boolean
    End Interface

    Public Interface EmberMovieOutputModule
        Property Enabled() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Function InjectSetup() As Containers.SettingsPanel
        Sub SaveSetup(ByVal DoDispose As Boolean)
        Sub SetupOrderChanged()
        Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event ModuleSettingsChanged()
        Sub Init(ByVal sAssemblyName As String)
        '********************************************************************************************
        Function SaveMovieInfoSheet(ByRef movieToSave As Structures.DBMovie) As Boolean
        Function SaveImageAs(ByVal imageType As Enums.ImageType, ByRef mMovie As Structures.DBMovie) As List(Of String)
        Function IsAllowedToDownload(ByVal mMovie As Structures.DBMovie, ByVal fType As Enums.ImageType, Optional ByVal isChange As Boolean = False) As Boolean
        Function CopyBackDrops(ByVal sPath As String, ByVal sFile As String) As Boolean
    End Interface

    Public Interface EmberTVInputModule
        Property Enabled() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Function InjectSetup() As Containers.SettingsPanel
        Sub SaveSetup(ByVal DoDispose As Boolean)
        Sub SetupOrderChanged()
        Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event ModuleSettingsChanged()
        Sub Init(ByVal sAssemblyName As String)
        '********************************************************************************************
        Function LoadShowInfoSheet(ByVal sPath As String, ByRef mShow As MediaContainers.TVShow) As Boolean
        Function LoadEpisodeInfoSheet(ByVal sPath As String, ByRef mEpisode As MediaContainers.EpisodeDetails) As Boolean
    End Interface

    Public Interface EmberTVOutputModule
        Property Enabled() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Function InjectSetup() As Containers.SettingsPanel
        Sub SaveSetup(ByVal DoDispose As Boolean)
        Sub SetupOrderChanged()
        Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event ModuleSettingsChanged()
        Sub Init(ByVal sAssemblyName As String)
        '********************************************************************************************
        Function SaveShowInfoSheet(ByRef showToSave As Structures.DBTV) As Boolean
        Function SaveImageAs(ByVal imageType As Enums.ImageType, ByRef mShow As Structures.DBTV) As List(Of String)
    End Interface

    Public Interface EmberExternalModule
        Event GenericEvent(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object))
        Event ModuleSettingsChanged()
        Event ModuleSetupChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer)
        Property Enabled() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleType() As List(Of Enums.ModuleEventType)
        ReadOnly Property ModuleVersion() As String
        Sub Init(ByVal sAssemblyName As String)
        Function InjectSetup() As Containers.SettingsPanel
        Function RunGeneric(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object), ByRef _refparam As Object) As ModuleResult
        Sub SaveSetup(ByVal DoDispose As Boolean)
    End Interface

    Public Interface EmberMovieScraperModule
        Event ModuleSettingsChanged()
        Event MovieScraperEvent(ByVal eType As Enums.MovieScraperEventType, ByVal Parameter As Object)
        Event PostScraperSetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event ScraperSetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Sub ScraperOrderChanged()
        Sub PostScraperOrderChanged()
        ReadOnly Property IsPostScraper() As Boolean
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Property PostScraperEnabled() As Boolean
        Property ScraperEnabled() As Boolean
        Function DownloadTrailer(ByRef DBMovie As Structures.DBMovie, ByRef sURL As String) As ModuleResult
        Function GetMovieStudio(ByRef DBMovie As Structures.DBMovie, ByRef sStudio As List(Of String)) As ModuleResult
        Sub Init(ByVal sAssemblyName As String)
        Function InjectSetupPostScraper() As Containers.SettingsPanel
        Function QueryPostScraperCapabilities(ByVal cap As Enums.PostScraperCapabilities) As Boolean
        Function InjectSetupScraper() As Containers.SettingsPanel
        Function PostScraper(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType) As ModuleResult
        Sub SaveSetupPostScraper(ByVal DoDispose As Boolean)
        Sub SaveSetupScraper(ByVal DoDispose As Boolean)
        'Options is byref to allow field blocking in scraper chain
        Function Scraper(ByRef DBMovie As Structures.DBMovie, ByRef ScrapeType As Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As ModuleResult
        Function SelectImageOfType(ByRef DBMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As ModuleResult
    End Interface

    Public Interface EmberTVScraperModule
        Event ModuleSettingsChanged()
        Event SetupPostScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event SetupScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer)
        Event TVScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        ReadOnly Property IsBusy() As Boolean
        ReadOnly Property IsPostScraper() As Boolean
        ReadOnly Property IsScraper() As Boolean
        ReadOnly Property ModuleName() As String
        ReadOnly Property ModuleVersion() As String
        Property PostScraperEnabled() As Boolean
        Property ScraperEnabled() As Boolean
        Sub CancelAsync()
        Function ChangeEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Lang As String, ByRef epDet As MediaContainers.EpisodeDetails) As ModuleResult
        Function GetLangs(ByVal sMirror As String, ByRef Langs As List(Of Containers.TVLanguage)) As ModuleResult
        Function GetSingleEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Season As Integer, ByVal Episode As Integer, ByVal Lang As String, ByVal Ordering As Enums.Ordering, ByVal Options As Structures.TVScrapeOptions, ByRef epDetails As MediaContainers.EpisodeDetails) As ModuleResult
        Function GetSingleImage(ByVal Title As String, ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Type As Enums.TVImageType, ByVal Season As Integer, ByVal Episode As Integer, ByVal Lang As String, ByVal Ordering As Enums.Ordering, ByVal CurrentImage As Image, ByRef Image As Image) As ModuleResult
        Sub Init(ByVal sAssemblyName As String)
        Function InjectSetupPostScraper() As Containers.SettingsPanel
        Function InjectSetupScraper() As Containers.SettingsPanel
        Function PostScraper(ByRef DBTV As Structures.DBTV, ByVal ScrapeType As Enums.ScrapeType) As ModuleResult
        Function SaveImages() As ModuleResult
        Sub SaveSetupPostScraper(ByVal DoDispose As Boolean)
        Sub SaveSetupScraper(ByVal DoDispose As Boolean)
        Function ScrapeEpisode(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iEpisode As Integer, ByVal iSeason As Integer, ByVal Lang As String, ByVal Ordering As Enums.Ordering, ByVal Options As Structures.TVScrapeOptions) As ModuleResult
        Function Scraper(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal Lang As String, ByVal Ordering As Enums.Ordering, ByVal Options As Structures.TVScrapeOptions, ByVal ScrapeType As Enums.ScrapeType, ByVal WithCurrent As Boolean) As ModuleResult
        Function ScrapeSeason(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iSeason As Integer, ByVal Lang As String, ByVal Ordering As Enums.Ordering, ByVal Options As Structures.TVScrapeOptions) As ModuleResult
    End Interface

    Public Structure ModuleResult
        Public breakChain As Boolean
        Public Cancelled As Boolean
        Public BoolProperty As Boolean
    End Structure
End Class