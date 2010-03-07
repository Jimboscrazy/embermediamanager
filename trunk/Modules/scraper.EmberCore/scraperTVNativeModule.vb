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

Imports System.IO
''' <summary>
''' Native Scraper
''' </summary>
''' <remarks></remarks>
Public Class EmberNativeTVScraperModule
    Implements Interfaces.EmberTVScraperModule

    Private _Name As String = "Ember Native TV Scraper"
    Public Shared TVScraper As New Scraper
    Private _ScraperEnabled As Boolean = False
    Private _PostScraperEnabled As Boolean = False
    Public Event SetupScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberTVScraperModule.SetupScraperChanged
    Public Event SetupPostScraperChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements Interfaces.EmberTVScraperModule.SetupPostScraperChanged
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberTVScraperModule.ModuleSettingsChanged

    Property ScraperEnabled() As Boolean Implements Interfaces.EmberTVScraperModule.ScraperEnabled
        Get
            Return _ScraperEnabled
        End Get
        Set(ByVal value As Boolean)
            _ScraperEnabled = value
        End Set
    End Property
    Property PostScraperEnabled() As Boolean Implements Interfaces.EmberTVScraperModule.PostScraperEnabled
        Get
            Return _PostScraperEnabled
        End Get
        Set(ByVal value As Boolean)
            _PostScraperEnabled = value
        End Set
    End Property

    Public Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberTVScraperModule.Init
        AddHandler TVScraper.ScraperEvent, AddressOf Handler_ScraperEvent
    End Sub

    Public ReadOnly Property IsPostScraper() As Boolean Implements Interfaces.EmberTVScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property IsScraper() As Boolean Implements Interfaces.EmberTVScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements Interfaces.EmberTVScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberTVScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function PostScraper(ByRef DBTV As Structures.DBTV, ByVal ScrapeType As Enums.ScrapeType) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.PostScraper

    End Function
    Function GetLangs(ByVal sMirror As String, ByRef Langs As List(Of Containers.TVLanguage)) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.GetLangs
        Langs = TVScraper.GetLangs(sMirror)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
    Function SaveImages() As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.SaveImages
        TVScraper.SaveImages()
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
    Function Scraper(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.Scraper
        TVScraper.SingleScrape(ShowID, ShowTitle, TVDBID, Lang, Options)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
    Function ScrapeEpisode(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iEpisode As Integer, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.ScrapeEpisode
        TVScraper.ScrapeEpisode(ShowID, ShowTitle, TVDBID, iEpisode, iSeason, Lang, Options)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
    Public Sub Handler_ScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        RaiseEvent TVScraperEvent(eType, iProgress, Parameter)
    End Sub

    Function GetSingleEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Season As Integer, ByVal Episode As Integer, ByVal Options As Structures.TVScrapeOptions, ByRef epDetails As MediaContainers.EpisodeDetails) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.GetSingleEpisode
        epDetails = TVScraper.GetSingleEpisode(ShowID, TVDBID, Season, Episode, Options)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function

    Public Function GetSingleImage(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Type As Enums.TVImageType, ByVal Season As Integer, ByVal Episode As Integer, ByVal Lang As String, ByVal CurrentImage As Image, ByRef Image As Image) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.GetSingleImage
        Image = TVScraper.GetSingleImage(ShowID, TVDBID, Type, Season, Episode, Lang, CurrentImage)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
    Public Event TVScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object) Implements Interfaces.EmberTVScraperModule.TVScraperEvent
    'Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements Interfaces.EmberTVScraperModule.ScraperUpdateMediaList

    Function InjectSetupScraper() As Containers.SettingsPanel Implements Interfaces.EmberTVScraperModule.InjectSetupScraper
        Dim SPanel As New Containers.SettingsPanel
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(698, "TV Shows")
        SPanel.ImageIndex = If(Me._ScraperEnabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = New Panel
        SPanel.Parent = "pnlTVData"
        Return SPanel
    End Function
    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberTVScraperModule.SaveSetupScraper
    End Sub
    Sub SaveSetupPostScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberTVScraperModule.SaveSetupPostScraper
    End Sub

    Function InjectSetupPostScraper() As Containers.SettingsPanel Implements Interfaces.EmberTVScraperModule.InjectSetupPostScraper
        Dim SPanel As New Containers.SettingsPanel
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(698, "TV Shows")
        SPanel.ImageIndex = If(Me._ScraperEnabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = New Panel
        SPanel.Parent = "pnlTVMedia"
        Return SPanel
    End Function


    Function ChangeEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByRef epDet As MediaContainers.EpisodeDetails) As Interfaces.ModuleResult Implements Interfaces.EmberTVScraperModule.ChangeEpisode
        epDet = TVScraper.ChangeEpisode(ShowID, TVDBID)
        Return New Interfaces.ModuleResult With {.breakChain = True}
    End Function
End Class


