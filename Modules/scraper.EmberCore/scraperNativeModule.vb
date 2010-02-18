Imports System.IO
Imports EmberAPI
''' <summary>
''' Native Scraper
''' </summary>
''' <remarks></remarks>
Public Class EmberNativeScraperModule
    Implements EmberAPI.Interfaces.EmberScraperModule
    Private Enabled As Boolean = False
    Private _Name As String = "Ember Native Scraper"
    Public Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean) Implements EmberAPI.Interfaces.EmberScraperModule.ScraperUpdateMediaList

    ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberScraperModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property
    ReadOnly Property IsScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsScraper
        Get
            Return True
        End Get
    End Property
    ReadOnly Property IsTVScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsTVScraper
        Get
            Return False
        End Get
    End Property
    ReadOnly Property IsPostScraper() As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.IsPostScraper
        Get
            Return True
        End Get
    End Property
    Function TVScraper(ByRef DBTV As EmberAPI.Structures.DBTV, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.TVScraper
        Return True
    End Function
    Sub Setup(ByVal tScraper As Integer) Implements EmberAPI.Interfaces.EmberScraperModule.Setup
        Dim _setup As New frmNativeSetup
        _setup.lblVersion.Text = ModuleVersion
        _setup.TabControl1.SelectTab(tScraper)
        _setup.ShowDialog()
    End Sub
    ''' <summary>
    ''' Scraping Here
    ''' </summary>
    ''' <remarks></remarks>
    Private IMDB As New IMDB.Scraper
    Function Scraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByRef Options As Structures.ScrapeOptions) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.Scraper
        Dim tTitle As String = String.Empty
        Dim OldTitle As String = String.Empty
        If Master.GlobalScrapeMod.NFO Then
            If Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID) Then
                IMDB.GetMovieInfo(DBMovie.Movie.IMDBID, DBMovie.Movie, Options.bFullCrew, Options.bFullCast, False, Options)
            Else
                'Movie = IMDB.GetSearchMovieInfo(Movie.Title, Movie, Enums.ScrapeType.SingleScrape, Options)
                DBMovie.Movie = IMDB.GetSearchMovieInfo(DBMovie.Movie.Title, DBMovie.Movie, Enums.ScrapeType.FullAsk, Options)
            End If
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
        If Master.eSettings.ScanMediaInfo AndAlso Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Meta Then
            EmberAPI.MediaInfo.UpdateMediaInfo(DBMovie)
        End If
        RaiseEvent ScraperUpdateMediaList(6, True)
        ' I removed it to main form .. scraper should NOT save db or rename files!!! ???
        'If Master.eSettings.AutoRenameMulti AndAlso Master.GlobalScrapeMod.NFO Then
        'FileFolderRenamer.RenameSingle(DBMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, True, Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID), False)
        'Else
        'Master.DB.SaveMovieToDB(DBMovie, False, True, Not String.IsNullOrEmpty(DBMovie.Movie.IMDBID))
        'End If
        Return True
    End Function
    Function PostScraper(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean Implements EmberAPI.Interfaces.EmberScraperModule.PostScraper
        Dim Poster As New EmberAPI.Images
        Dim Fanart As New EmberAPI.Images
        Dim pResults As EmberAPI.Containers.ImgResult
        Dim fResults As EmberAPI.Containers.ImgResult
        Dim tURL As String = String.Empty
        Dim Trailer As New Trailers
        Dim doSave As Boolean = False
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
                            RaiseEvent ScraperUpdateMediaList(5, True)
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
                                RaiseEvent ScraperUpdateMediaList(5, True)
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
                    'Me.Invoke(myDelegate, New Object() {drvRow, 7, True})
                    RaiseEvent ScraperUpdateMediaList(7, True)
                End If
            End If
        End If
        If Master.GlobalScrapeMod.Extra Then
            If Master.eSettings.AutoET AndAlso Not didEts Then
                Fanart.GetPreferredFAasET(DBMovie.Movie.IMDBID, DBMovie.Filename)
            End If
            If Master.eSettings.AutoThumbs > 0 AndAlso DBMovie.isSingle Then
                Dim ETasFA As String = ThumbGenerator.CreateRandomThumbs(DBMovie, Master.eSettings.AutoThumbs, False)
                If Not String.IsNullOrEmpty(ETasFA) Then
                    'Me.Invoke(myDelegate, New Object() {drvRow, 9, True})
                    RaiseEvent ScraperUpdateMediaList(9, True)
                    DBMovie.ExtraPath = "TRUE"
                    If Not ETasFA = "TRUE" Then
                        'Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                        RaiseEvent ScraperUpdateMediaList(5, True)
                        DBMovie.FanartPath = ETasFA
                    End If
                End If
            End If
        End If
        If doSave Then
            'NOTE to Nuno: Why this way... no other way to do this
            'Me.Invoke(myDelegate, New Object() {drvRow, 6, True})
        End If
        ' need event for this or better move it out of here
        'Me.Invoke(myDelegate, New Object() {drvRow, 3, scrapeMovie.ListTitle})
        'Me.Invoke(myDelegate, New Object() {drvRow, 50, scrapeMovie.Movie.SortTitle})

        Return True
    End Function

End Class


