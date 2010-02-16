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

Public Class EmberScraperModule
    Implements EmberAPI.Interfaces.EmberScraperModule
    Private Enabled As Boolean = False
    Private _Name As String = "Ember Native Scraper"
    Private _Version As String = "1.0"
    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberScraperModule.ScraperUpdateMediaList
    ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property
    ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property
    Sub Setup(ByVal tScraper As Integer) Implements EmberAPI.Interfaces.EmberScraperModule.Setup
        Dim _setup As New frmSetup
        _setup.TabControl1.SelectTab(tScraper)
        _setup.ShowDialog()
    End Sub
    ''' <summary>
    ''' Scraping Here
    ''' </summary>
    ''' <remarks></remarks>
    Private IMDB As New IMDB.Scraper
    Function Scraper(ByRef Movie As EmberAPI.MediaContainers.Movie, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.Scraper
        If Not String.IsNullOrEmpty(Movie.IMDBID) Then
            IMDB.GetMovieInfo(Movie.IMDBID, Movie, Options.bFullCrew, Options.bFullCast, False, Options)
        Else
            'Movie = IMDB.GetSearchMovieInfo(Movie.Title, Movie, Enums.ScrapeType.SingleScrape, Options)
            Movie = IMDB.GetSearchMovieInfo(Movie.Title, Movie, Enums.ScrapeType.FullAsk, Options)
        End If

        Return True
    End Function
    Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.PostScraper
        Dim Poster As New EmberAPI.Images
        Dim Fanart As New EmberAPI.Images
        Dim pResults As EmberAPI.Containers.ImgResult
        Dim fResults As EmberAPI.Containers.ImgResult
        If Master.GlobalScrapeMod.Poster Then
            Poster.Clear()
            If Poster.IsAllowedToDownload(DBMovie, Enums.ImageType.Posters) Then
                pResults = New Containers.ImgResult
                If Poster.GetPreferredImage(DBMovie.Movie.IMDBID, Enums.ImageType.Posters, pResults, DBMovie.Filename, False, If(ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk, True, False)) Then
                    If Not IsNothing(Poster.Image) Then
                        pResults.ImagePath = Poster.SaveAsPoster(DBMovie)
                        If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                            DBMovie.PosterPath = pResults.ImagePath
                            'Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                            RaiseEvent ScraperUpdateMediaList(4, True)
                            Application.DoEvents() 'for debug
                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                DBMovie.Movie.Thumb = pResults.Posters
                            End If
                        End If
                    ElseIf ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk Then
                        MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                        Using dImgSelect As New dlgImgSelect
                            pResults = dImgSelect.ShowDialog(DBMovie, Enums.ImageType.Posters)
                            If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                DBMovie.PosterPath = pResults.ImagePath
                                'Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                RaiseEvent ScraperUpdateMediaList(4, True)
                                Application.DoEvents() 'for debug
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
                If Fanart.GetPreferredImage(DBMovie.Movie.IMDBID, Enums.ImageType.Fanart, fResults, DBMovie.Filename, Master.GlobalScrapeMod.Extra, If(ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk, True, False)) Then
                    If Not IsNothing(Fanart.Image) Then
                        fResults.ImagePath = Fanart.SaveAsFanart(DBMovie)
                        If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                            DBMovie.FanartPath = fResults.ImagePath
                            'Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                DBMovie.Movie.Fanart = fResults.Fanart
                            End If
                        End If
                    ElseIf ScrapeType = Enums.ScrapeType.FullAsk OrElse ScrapeType = Enums.ScrapeType.NewAsk OrElse ScrapeType = Enums.ScrapeType.MarkAsk Then
                        MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))

                        Using dImgSelect As New dlgImgSelect
                            fResults = dImgSelect.ShowDialog(DBMovie, Enums.ImageType.Fanart)
                            If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                DBMovie.FanartPath = fResults.ImagePath
                                'Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                    DBMovie.Movie.Fanart = fResults.Fanart
                                End If
                            End If
                        End Using
                    End If
                End If
            End If
        End If
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


