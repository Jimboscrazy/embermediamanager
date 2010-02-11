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

Public Interface EmberScraperModule
    Sub Setup()
    'Title or Id must be field in, all movie is past because some scrapper may run to update only some fields (defined in setup)
    Function Scraper(ByVal Movie As Media.Movie) As Media.Movie
    Function PostScraper(ByVal Movie As Media.Movie) As Media.Movie
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
    ReadOnly Property IsScraper() As Boolean
    ReadOnly Property IsPostScraper() As Boolean
End Interface

Public Class TestEmberScraperModule
    Implements EmberScraperModule
    Private Enabled As Boolean = False
    Private _Name As String = "Teste Scraper"
    Private _Version As String = "1.0"
 
    ReadOnly Property IsScraper() As Boolean Implements EmberScraperModule.IsScraper
        Get
            Return False
        End Get
    End Property
    ReadOnly Property IsPostScraper() As Boolean Implements EmberScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property
    Sub Setup() Implements EmberScraperModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Function Scraper(ByVal Movie As Media.Movie) As Media.Movie Implements EmberScraperModule.Scraper
        Return Nothing
    End Function
    Function PostScraper(ByVal Movie As Media.Movie) As Media.Movie Implements EmberScraperModule.PostScraper
        Return Nothing
    End Function
    ReadOnly Property ModuleName() As String Implements EmberScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberScraperModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property
End Class
