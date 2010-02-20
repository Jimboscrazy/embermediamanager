' ################################################################################
' #                             EMBER ProxyMEdia MANAGER                              #
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
Public Class EmberXMLScraperModule
    Implements EmberAPI.Interfaces.EmberScraperModule
    Sub Init() Implements EmberAPI.Interfaces.EmberScraperModule.Init

    End Sub
    Public ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsPostScraper
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property
    ReadOnly Property IsTVScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsTVScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleName
        Get
            Return "Ember XML Scraper"
        End Get
    End Property
    Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As List(Of String)) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.GetMovieStudio
        Return False
    End Function

    Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.DownloadTrailer

        Return False
    End Function


    Function SelectImageOfType(ByRef mMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.SelectImageOfType
        Return False
    End Function

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.PostScraper

    End Function

    Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.Scraper

    End Function
    Function TVScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.TVScraper
        Return True
    End Function

    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberScraperModule.ScraperUpdateMediaList

    Sub SetupScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupScraper
        Dim _setup As New frmXMLSetup
        _setup.ShowDialog()
    End Sub
    Sub SetupPostScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupPostScraper

    End Sub
    Sub SetupTVScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupTVScraper

    End Sub
    Sub SetupTVPostScraper() Implements EmberAPI.Interfaces.EmberScraperModule.SetupTVPostScraper
    End Sub
    Function TVPostScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.TVPostScraper

    End Function
End Class


