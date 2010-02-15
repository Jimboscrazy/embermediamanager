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

Imports EmberAPI

Public Class TestEmberScraperModule
    Implements EmberAPI.Interfaces.EmberScraperModule
    Private Enabled As Boolean = False
    Private _Name As String = "Teste Scraper"
    Private _Version As String = "1.0"

    ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsScraper
        Get
            Return False
        End Get
    End Property
    ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property
    Sub Setup() Implements EmberAPI.Interfaces.EmberScraperModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Function Scraper(ByRef Movie As EmberAPI.MediaContainers.Movie, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.Scraper

        Return True
    End Function
    Function PostScraper(ByRef Movie As EmberAPI.MediaContainers.Movie) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.PostScraper
        Return True
    End Function
    ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property
End Class


