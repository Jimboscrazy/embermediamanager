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
    Implements EmberAPI.Interfaces.EmberMovieScraperModule

    Function testSetupScraper(ByRef p As System.Windows.Forms.Panel) As Integer Implements EmberAPI.Interfaces.EmberMovieScraperModule.testSetupScraper
        Dim _setup As New frmXMLSetup
        _setup.TopLevel = False
        _setup.FormBorderStyle = FormBorderStyle.None
        p.Controls.Add(_setup)
        _setup.Top = 30
        _setup.Show()
        Return _setup.Height
    End Function

    Sub Init() Implements EmberAPI.Interfaces.EmberMovieScraperModule.Init

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
    ReadOnly Property IsTVScraper() As Boolean Implements EmberAPI.Interfaces.EmberMovieScraperModule.IsTVScraper
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleName
        Get
            Return "Ember XML Scraper"
        End Get
    End Property
    Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sStudio As List(Of String)) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.GetMovieStudio
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function

    Function DownloadTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef sURL As String) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.DownloadTrailer

        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function


    Function SelectImageOfType(ByRef mMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.SelectImageOfType
        Return New EmberAPI.Interfaces.ScraperResult With {.breakChain = False}
    End Function

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.PostScraper

    End Function

    Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef ScrapeType As EmberAPI.Enums.ScrapeType, ByRef Options As Structures.ScrapeOptions) As EmberAPI.Interfaces.ScraperResult Implements EmberAPI.Interfaces.EmberMovieScraperModule.Scraper

    End Function

    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberMovieScraperModule.ScraperUpdateMediaList

    Sub SetupScraper() Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupScraper
        Dim _setup As New frmXMLSetup
        _setup.ShowDialog()
    End Sub
    Sub SetupPostScraper() Implements EmberAPI.Interfaces.EmberMovieScraperModule.SetupPostScraper

    End Sub

End Class


