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
Imports System.Xml.Serialization

<Serializable()> _
Public Class emmSettings
    Private _version As String
    Private _filterCustom As New ArrayList
    Private _certificationLang As String
    Private _usecertformpaa As Boolean
    Private _scanmediainfo As Boolean
    Private _imdburl As String
    Private _fullcast As Boolean
    Private _fullcrew As Boolean
    Private _castimagesonly As Boolean
    Private _movieposterCol As Boolean
    Private _moviefanartCol As Boolean
    Private _movieinfoCol As Boolean
    Private _movietrailerCol As Boolean
    Private _moviesubCol As Boolean
    Private _movieextraCol As Boolean
    Private _cleanfolderJpg As Boolean
    Private _cleanmovieTbn As Boolean
    Private _cleanmovieTbnB As Boolean
    Private _cleanfanartJpg As Boolean
    Private _cleanmoviefanartJpg As Boolean
    Private _cleanmovieNfo As Boolean
    Private _cleanmovieNfoB As Boolean
    Private _cleanposterTbn As Boolean
    Private _cleanposterJpg As Boolean
    Private _cleanmovieJpg As Boolean
    Private _cleandotfanartJpg As Boolean
    Private _cleanmovienameJpg As Boolean
    Private _cleanextrathumbs As Boolean
    Private _expertcleaner As Boolean
    Private _cleanwhitelistvideo As Boolean
    Private _cleanwhitelistexts As New ArrayList
    Private _useTMDB As Boolean
    Private _useIMPA As Boolean
    Private _useMPDB As Boolean
    Private _postersize As Master.PosterSize
    Private _fanartsize As Master.FanartSize
    Private _autoET As Boolean
    Private _autoETsize As Master.FanartSize
    Private _fanartprefsizeonly As Boolean
    Private _posterQuality As Integer
    Private _fanartQuality As Integer
    Private _overwritePoster As Boolean
    Private _overwriteFanart As Boolean
    Private _logerrors As Boolean
    Private _properCase As Boolean
    Private _overwritenfo As Boolean
    Private _validexts As New ArrayList
    Private _nostackexts As New ArrayList
    Private _movietbn As Boolean
    Private _movienametbn As Boolean
    Private _moviejpg As Boolean
    Private _movienamejpg As Boolean
    Private _postertbn As Boolean
    Private _posterjpg As Boolean
    Private _folderjpg As Boolean
    Private _fanartjpg As Boolean
    Private _movienamefanartjpg As Boolean
    Private _movienamedotfanartjpg As Boolean
    Private _movienfo As Boolean
    Private _movienamenfo As Boolean
    Private _movienamemultionly As Boolean
    Private _dashtrailer As Boolean
    Private _videotsparent As Boolean
    Private _lockplot As Boolean
    Private _lockoutline As Boolean
    Private _locktitle As Boolean
    Private _locktagline As Boolean
    Private _lockrating As Boolean
    Private _lockstudio As Boolean
    Private _lockgenre As Boolean
    Private _locktrailer As Boolean
    Private _singlescrapeimages As Boolean
    Private _marknew As Boolean
    Private _resizefanart As Boolean
    Private _fanartheight As Integer
    Private _fanartwidth As Integer
    Private _resizeposter As Boolean
    Private _posterheight As Integer
    Private _posterwidth As Integer
    Private _useofdbtitle As Boolean
    Private _useofdboutline As Boolean
    Private _useofdbplot As Boolean
    Private _useofdbgenre As Boolean
    Private _autothumbs As Integer
    Private _autothumbnospoilers As Boolean
    Private _windowloc As New Point
    Private _windowsize As New Size
    Private _windowstate As FormWindowState
    Private _infopanelstate As Integer
    Private _filterPanelState As Boolean
    Private _scmainstate As Integer
    Private _infopanelanim As Boolean
    Private _checkupdates As Boolean
    Private _bdpath As String
    Private _autobd As Boolean
    Private _usemiduration As Boolean
    Private _usehmforruntime As Boolean
    Private _genrefilter As String
    Private _useetasfa As Boolean
    Private _sets As New ArrayList
    Private _useimgcache As Boolean
    Private _useimgcacheupdater As Boolean
    Private _persistimagecache As Boolean
    Private _skiplessthan As Integer
    Private _skipstacksizecheck As Boolean
    Private _downloadtrailers As Boolean
    Private _updatertrailers As Boolean
    Private _updatertrailersnodownload As Boolean
    Private _singlescrapetrailer As Boolean
    Private _trailertimeout As Integer
    Private _overwritetrailer As Boolean
    Private _deletealltrailers As Boolean
    Private _trailersites As New List(Of Master.TrailerPages)
    Private _nosaveimagestonfo As Boolean
    Private _showdims As Boolean
    Private _nodisplayposter As Boolean
    Private _nodisplayfanart As Boolean
    Private _outlineforplot As Boolean
    Private _xbmccoms As New List(Of XBMCCom)
    Private _defaultfolderspattern As String
    Private _defaultfilespattern As String
    Private _sortpath As String
    Private _allwaysdisplaygenrestext As Boolean
    Private _displayyear As Boolean
    Private _sorttokens As New ArrayList
    Private _etnative As Boolean
    Private _etwidth As Integer
    Private _etheight As Integer
    Private _etpadding As Boolean
    Private _nofilters As Boolean
    Private _notokens As Boolean
    Private _levtolerance As Integer
    Private _autodetectvts As Boolean
    Private _flaglang As String
    Private _language As String
    Private _fieldtitle As Boolean
    Private _fieldyear As Boolean
    Private _fieldmpaa As Boolean
    Private _fieldrelease As Boolean
    Private _fieldruntime As Boolean
    Private _fieldrating As Boolean
    Private _fieldvotes As Boolean
    Private _fieldstudio As Boolean
    Private _fieldgenre As Boolean
    Private _fieldtrailer As Boolean
    Private _fieldtagline As Boolean
    Private _fieldoutline As Boolean
    Private _fieldplot As Boolean
    Private _fieldcast As Boolean
    Private _fielddirector As Boolean
    Private _fieldwriters As Boolean
    Private _fieldproducers As Boolean
    Private _fieldmusic As Boolean
    Private _fieldcrew As Boolean
    Private _field250 As Boolean
    Private _genrelimit As Integer
    Private _actorlimit As Integer
    Private _missingfilterposter As Boolean
    Private _missingfilterfanart As Boolean
    Private _missingfilternfo As Boolean
    Private _missingfiltertrailer As Boolean
    Private _missingfiltersubs As Boolean
    Private _missingfilterextras As Boolean
    Private _autorenamemulti As Boolean
    Private _autorenamesingle As Boolean
    Private _movietheme As String
    Private _metadatapertype As New List(Of MetadataPerType)
    Private _enableifoscan As Boolean

    Public Property Version() As String
        Get
            Return Me._version
        End Get
        Set(ByVal value As String)
            Me._version = value
        End Set
    End Property

    Public Property FilterCustom() As ArrayList
        Get
            Return Me._filterCustom
        End Get
        Set(ByVal value As ArrayList)
            Me._filterCustom = value
        End Set
    End Property

    Public Property CertificationLang() As String
        Get
            Return Me._certificationLang
        End Get
        Set(ByVal value As String)
            Me._certificationLang = value
        End Set
    End Property

    Public Property UseCertForMPAA() As Boolean
        Get
            Return Me._usecertformpaa
        End Get
        Set(ByVal value As Boolean)
            Me._usecertformpaa = value
        End Set
    End Property

    Public Property ScanMediaInfo() As Boolean
        Get
            Return Me._scanmediainfo
        End Get
        Set(ByVal value As Boolean)
            Me._scanmediainfo = value
        End Set
    End Property

    Public Property IMDBURL() As String
        Get
            Return Me._imdburl
        End Get
        Set(ByVal value As String)
            Me._imdburl = value
        End Set
    End Property

    Public Property FullCast() As Boolean
        Get
            Return Me._fullcast
        End Get
        Set(ByVal value As Boolean)
            Me._fullcast = value
        End Set
    End Property

    Public Property FullCrew() As Boolean
        Get
            Return Me._fullcrew
        End Get
        Set(ByVal value As Boolean)
            Me._fullcrew = value
        End Set
    End Property

    Public Property CastImagesOnly() As Boolean
        Get
            Return Me._castimagesonly
        End Get
        Set(ByVal value As Boolean)
            Me._castimagesonly = value
        End Set
    End Property

    Public Property MoviePosterCol() As Boolean
        Get
            Return Me._movieposterCol
        End Get
        Set(ByVal value As Boolean)
            Me._movieposterCol = value
        End Set
    End Property

    Public Property MovieFanartCol() As Boolean
        Get
            Return Me._moviefanartCol
        End Get
        Set(ByVal value As Boolean)
            Me._moviefanartCol = value
        End Set
    End Property

    Public Property MovieInfoCol() As Boolean
        Get
            Return Me._movieinfoCol
        End Get
        Set(ByVal value As Boolean)
            Me._movieinfoCol = value
        End Set
    End Property

    Public Property MovieTrailerCol() As Boolean
        Get
            Return Me._movietrailerCol
        End Get
        Set(ByVal value As Boolean)
            Me._movietrailerCol = value
        End Set
    End Property

    Public Property MovieSubCol() As Boolean
        Get
            Return Me._moviesubCol
        End Get
        Set(ByVal value As Boolean)
            Me._moviesubCol = value
        End Set
    End Property

    Public Property MovieExtraCol() As Boolean
        Get
            Return Me._movieextraCol
        End Get
        Set(ByVal value As Boolean)
            Me._movieextraCol = value
        End Set
    End Property

    Public Property CleanFolderJPG() As Boolean
        Get
            Return Me._cleanfolderJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanfolderJpg = value
        End Set
    End Property

    Public Property CleanMovieTBN() As Boolean
        Get
            Return _cleanmovieTbn
        End Get
        Set(ByVal value As Boolean)
            _cleanmovieTbn = value
        End Set
    End Property

    Public Property CleanMovieTBNB() As Boolean
        Get
            Return Me._cleanmovieTbnB
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmovieTbnB = value
        End Set
    End Property

    Public Property CleanFanartJPG() As Boolean
        Get
            Return Me._cleanfanartJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanfanartJpg = value
        End Set
    End Property

    Public Property CleanMovieFanartJPG() As Boolean
        Get
            Return Me._cleanmoviefanartJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmoviefanartJpg = value
        End Set
    End Property

    Public Property CleanMovieNFO() As Boolean
        Get
            Return Me._cleanmovieNfo
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmovieNfo = value
        End Set
    End Property

    Public Property CleanMovieNFOB() As Boolean
        Get
            Return Me._cleanmovieNfoB
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmovieNfoB = value
        End Set
    End Property

    Public Property CleanPosterTBN() As Boolean
        Get
            Return Me._cleanposterTbn
        End Get
        Set(ByVal value As Boolean)
            Me._cleanposterTbn = value
        End Set
    End Property

    Public Property CleanPosterJPG() As Boolean
        Get
            Return Me._cleanposterJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanposterJpg = value
        End Set
    End Property

    Public Property CleanMovieNameJPG() As Boolean
        Get
            Return Me._cleanmovienameJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmovienameJpg = value
        End Set
    End Property

    Public Property CleanMovieJPG() As Boolean
        Get
            Return Me._cleanmovieJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleanmovieJpg = value
        End Set
    End Property

    Public Property CleanDotFanartJPG() As Boolean
        Get
            Return Me._cleandotfanartJpg
        End Get
        Set(ByVal value As Boolean)
            Me._cleandotfanartJpg = value
        End Set
    End Property

    Public Property CleanExtraThumbs() As Boolean
        Get
            Return Me._cleanextrathumbs
        End Get
        Set(ByVal value As Boolean)
            Me._cleanextrathumbs = value
        End Set
    End Property

    Public Property ExpertCleaner() As Boolean
        Get
            Return Me._expertcleaner
        End Get
        Set(ByVal value As Boolean)
            Me._expertcleaner = value
        End Set
    End Property

    Public Property CleanWhitelistVideo() As Boolean
        Get
            Return Me._cleanwhitelistvideo
        End Get
        Set(ByVal value As Boolean)
            Me._cleanwhitelistvideo = value
        End Set
    End Property

    Public Property CleanWhitelistExts() As ArrayList
        Get
            Return Me._cleanwhitelistexts
        End Get
        Set(ByVal value As ArrayList)
            Me._cleanwhitelistexts = value
        End Set
    End Property

    Public Property UseTMDB() As Boolean
        Get
            Return Me._useTMDB
        End Get
        Set(ByVal value As Boolean)
            Me._useTMDB = value
        End Set
    End Property

    Public Property UseIMPA() As Boolean
        Get
            Return Me._useIMPA
        End Get
        Set(ByVal value As Boolean)
            Me._useIMPA = value
        End Set
    End Property

    Public Property UseMPDB() As Boolean
        Get
            Return Me._useMPDB
        End Get
        Set(ByVal value As Boolean)
            Me._useMPDB = value
        End Set
    End Property

    Public Property PreferredPosterSize() As Master.PosterSize
        Get
            Return Me._postersize
        End Get
        Set(ByVal value As Master.PosterSize)
            Me._postersize = value
        End Set
    End Property

    Public Property PreferredFanartSize() As Master.FanartSize
        Get
            Return Me._fanartsize
        End Get
        Set(ByVal value As Master.FanartSize)
            Me._fanartsize = value
        End Set
    End Property

    Public Property AutoET() As Boolean
        Get
            Return Me._autoET
        End Get
        Set(ByVal value As Boolean)
            Me._autoET = value
        End Set
    End Property

    Public Property AutoETSize() As Master.FanartSize
        Get
            Return Me._autoETsize
        End Get
        Set(ByVal value As Master.FanartSize)
            Me._autoETsize = value
        End Set
    End Property

    Public Property FanartPrefSizeOnly() As Boolean
        Get
            Return Me._fanartprefsizeonly
        End Get
        Set(ByVal value As Boolean)
            Me._fanartprefsizeonly = value
        End Set
    End Property

    Public Property PosterQuality() As Integer
        Get
            Return Me._posterQuality
        End Get
        Set(ByVal value As Integer)
            Me._posterQuality = value
        End Set
    End Property

    Public Property FanartQuality() As Integer
        Get
            Return Me._fanartQuality
        End Get
        Set(ByVal value As Integer)
            Me._fanartQuality = value
        End Set
    End Property

    Public Property OverwritePoster() As Boolean
        Get
            Return Me._overwritePoster
        End Get
        Set(ByVal value As Boolean)
            Me._overwritePoster = value
        End Set
    End Property

    Public Property OverwriteFanart() As Boolean
        Get
            Return Me._overwriteFanart
        End Get
        Set(ByVal value As Boolean)
            Me._overwriteFanart = value
        End Set
    End Property

    Public Property LogErrors() As Boolean
        Get
            Return Me._logerrors
        End Get
        Set(ByVal value As Boolean)
            Me._logerrors = value
        End Set
    End Property

    Public Property ProperCase() As Boolean
        Get
            Return Me._properCase
        End Get
        Set(ByVal value As Boolean)
            Me._properCase = value
        End Set
    End Property

    Public Property OverwriteNfo() As Boolean
        Get
            Return Me._overwritenfo
        End Get
        Set(ByVal value As Boolean)
            Me._overwritenfo = value
        End Set
    End Property

    Public Property ValidExts() As ArrayList
        Get
            Return Me._validexts
        End Get
        Set(ByVal value As ArrayList)
            Me._validexts = value
        End Set
    End Property

    Public Property NoStackExts() As ArrayList
        Get
            Return Me._nostackexts
        End Get
        Set(ByVal value As ArrayList)
            Me._nostackexts = value
        End Set
    End Property

    Public Property MovieTBN() As Boolean
        Get
            Return Me._movietbn
        End Get
        Set(ByVal value As Boolean)
            Me._movietbn = value
        End Set
    End Property

    Public Property MovieNameTBN() As Boolean
        Get
            Return Me._movienametbn
        End Get
        Set(ByVal value As Boolean)
            Me._movienametbn = value
        End Set
    End Property

    Public Property MovieJPG() As Boolean
        Get
            Return Me._moviejpg
        End Get
        Set(ByVal value As Boolean)
            Me._moviejpg = value
        End Set
    End Property

    Public Property MovieNameJPG() As Boolean
        Get
            Return Me._movienamejpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamejpg = value
        End Set
    End Property

    Public Property PosterTBN() As Boolean
        Get
            Return Me._postertbn
        End Get
        Set(ByVal value As Boolean)
            Me._postertbn = value
        End Set
    End Property

    Public Property PosterJPG() As Boolean
        Get
            Return Me._posterjpg
        End Get
        Set(ByVal value As Boolean)
            Me._posterjpg = value
        End Set
    End Property

    Public Property FolderJPG() As Boolean
        Get
            Return Me._folderjpg
        End Get
        Set(ByVal value As Boolean)
            Me._folderjpg = value
        End Set
    End Property

    Public Property FanartJPG() As Boolean
        Get
            Return Me._fanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._fanartjpg = value
        End Set
    End Property

    Public Property MovieNameFanartJPG() As Boolean
        Get
            Return Me._movienamefanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamefanartjpg = value
        End Set
    End Property

    Public Property MovieNameDotFanartJPG() As Boolean
        Get
            Return Me._movienamedotfanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamedotfanartjpg = value
        End Set
    End Property

    Public Property MovieNFO() As Boolean
        Get
            Return Me._movienfo
        End Get
        Set(ByVal value As Boolean)
            Me._movienfo = value
        End Set
    End Property

    Public Property MovieNameNFO() As Boolean
        Get
            Return Me._movienamenfo
        End Get
        Set(ByVal value As Boolean)
            Me._movienamenfo = value
        End Set
    End Property

    Public Property MovieNameMultiOnly() As Boolean
        Get
            Return Me._movienamemultionly
        End Get
        Set(ByVal value As Boolean)
            Me._movienamemultionly = value
        End Set
    End Property

    Public Property DashTrailer() As Boolean
        Get
            Return Me._dashtrailer
        End Get
        Set(ByVal value As Boolean)
            Me._dashtrailer = value
        End Set
    End Property

    Public Property VideoTSParent() As Boolean
        Get
            Return Me._videotsparent
        End Get
        Set(ByVal value As Boolean)
            Me._videotsparent = value
        End Set
    End Property

    Public Property LockPlot() As Boolean
        Get
            Return Me._lockplot
        End Get
        Set(ByVal value As Boolean)
            Me._lockplot = value
        End Set
    End Property

    Public Property LockOutline() As Boolean
        Get
            Return Me._lockoutline
        End Get
        Set(ByVal value As Boolean)
            Me._lockoutline = value
        End Set
    End Property

    Public Property LockTitle() As Boolean
        Get
            Return Me._locktitle
        End Get
        Set(ByVal value As Boolean)
            Me._locktitle = value
        End Set
    End Property

    Public Property LockTagline() As Boolean
        Get
            Return Me._locktagline
        End Get
        Set(ByVal value As Boolean)
            Me._locktagline = value
        End Set
    End Property

    Public Property LockRating() As Boolean
        Get
            Return Me._lockrating
        End Get
        Set(ByVal value As Boolean)
            Me._lockrating = value
        End Set
    End Property

    Public Property LockStudio() As Boolean
        Get
            Return Me._lockstudio
        End Get
        Set(ByVal value As Boolean)
            Me._lockstudio = value
        End Set
    End Property

    Public Property LockGenre() As Boolean
        Get
            Return Me._lockgenre
        End Get
        Set(ByVal value As Boolean)
            Me._lockgenre = value
        End Set
    End Property

    Public Property LockTrailer() As Boolean
        Get
            Return Me._locktrailer
        End Get
        Set(ByVal value As Boolean)
            Me._locktrailer = value
        End Set
    End Property

    Public Property SingleScrapeImages() As Boolean
        Get
            Return Me._singlescrapeimages
        End Get
        Set(ByVal value As Boolean)
            Me._singlescrapeimages = value
        End Set
    End Property

    Public Property MarkNew() As Boolean
        Get
            Return Me._marknew
        End Get
        Set(ByVal value As Boolean)
            Me._marknew = value
        End Set
    End Property

    Public Property ResizeFanart() As Boolean
        Get
            Return Me._resizefanart
        End Get
        Set(ByVal value As Boolean)
            Me._resizefanart = value
        End Set
    End Property

    Public Property FanartWidth() As Integer
        Get
            Return Me._fanartwidth
        End Get
        Set(ByVal value As Integer)
            Me._fanartwidth = value
        End Set
    End Property

    Public Property FanartHeight() As Integer
        Get
            Return Me._fanartheight
        End Get
        Set(ByVal value As Integer)
            Me._fanartheight = value
        End Set
    End Property

    Public Property ResizePoster() As Boolean
        Get
            Return Me._resizeposter
        End Get
        Set(ByVal value As Boolean)
            Me._resizeposter = value
        End Set
    End Property

    Public Property PosterWidth() As Integer
        Get
            Return Me._posterwidth
        End Get
        Set(ByVal value As Integer)
            Me._posterwidth = value
        End Set
    End Property

    Public Property PosterHeight() As Integer
        Get
            Return Me._posterheight
        End Get
        Set(ByVal value As Integer)
            Me._posterheight = value
        End Set
    End Property

    Public Property UseOFDBTitle() As Boolean
        Get
            Return Me._useofdbtitle
        End Get
        Set(ByVal value As Boolean)
            Me._useofdbtitle = value
        End Set
    End Property

    Public Property UseOFDBOutline() As Boolean
        Get
            Return Me._useofdboutline
        End Get
        Set(ByVal value As Boolean)
            Me._useofdboutline = value
        End Set
    End Property

    Public Property UseOFDBPlot() As Boolean
        Get
            Return Me._useofdbplot
        End Get
        Set(ByVal value As Boolean)
            Me._useofdbplot = value
        End Set
    End Property

    Public Property UseOFDBGenre() As Boolean
        Get
            Return Me._useofdbgenre
        End Get
        Set(ByVal value As Boolean)
            Me._useofdbgenre = value
        End Set
    End Property

    Public Property AutoThumbs() As Integer
        Get
            Return Me._autothumbs
        End Get
        Set(ByVal value As Integer)
            Me._autothumbs = value
        End Set
    End Property

    Public Property AutoThumbsNoSpoilers() As Boolean
        Get
            Return Me._autothumbnospoilers
        End Get
        Set(ByVal value As Boolean)
            Me._autothumbnospoilers = value
        End Set
    End Property

    Public Property WindowLoc() As Point
        Get
            Return Me._windowloc
        End Get
        Set(ByVal value As Point)
            Me._windowloc = value
        End Set
    End Property

    Public Property WindowSize() As Size
        Get
            Return Me._windowsize
        End Get
        Set(ByVal value As Size)
            Me._windowsize = value
        End Set
    End Property

    Public Property WindowState() As FormWindowState
        Get
            Return Me._windowstate
        End Get
        Set(ByVal value As FormWindowState)
            Me._windowstate = value
        End Set
    End Property
    Public Property InfoPanelState() As Integer
        Get
            Return Me._infopanelstate
        End Get
        Set(ByVal value As Integer)
            Me._infopanelstate = value
        End Set
    End Property
    Public Property FilterPanelState() As Boolean
        Get
            Return Me._filterPanelState
        End Get
        Set(ByVal value As Boolean)
            Me._filterPanelState = value
        End Set
    End Property
    Public Property SpliterPanelState() As Integer
        Get
            Return Me._scmainstate
        End Get
        Set(ByVal value As Integer)
            Me._scmainstate = value
        End Set
    End Property
    Public Property InfoPanelAnim() As Boolean
        Get
            Return Me._infopanelanim
        End Get
        Set(ByVal value As Boolean)
            Me._infopanelanim = value
        End Set
    End Property
    Public Property CheckUpdates() As Boolean
        Get
            Return Me._checkupdates
        End Get
        Set(ByVal value As Boolean)
            Me._checkupdates = value
        End Set
    End Property

    Public Property BDPath() As String
        Get
            Return Me._bdpath
        End Get
        Set(ByVal value As String)
            Me._bdpath = value
        End Set
    End Property

    Public Property AutoBD() As Boolean
        Get
            Return Me._autobd
        End Get
        Set(ByVal value As Boolean)
            Me._autobd = value
        End Set
    End Property

    Public Property UseMIDuration() As Boolean
        Get
            Return Me._usemiduration
        End Get
        Set(ByVal value As Boolean)
            Me._usemiduration = value
        End Set
    End Property

    Public Property UseHMForRuntime() As Boolean
        Get
            Return Me._usehmforruntime
        End Get
        Set(ByVal value As Boolean)
            Me._usehmforruntime = value
        End Set
    End Property

    Public Property GenreFilter() As String
        Get
            Return Me._genrefilter
        End Get
        Set(ByVal value As String)
            Me._genrefilter = value
        End Set
    End Property

    Public Property UseETasFA() As Boolean
        Get
            Return Me._useetasfa
        End Get
        Set(ByVal value As Boolean)
            Me._useetasfa = value
        End Set
    End Property

    Public Property Sets() As ArrayList
        Get
            Return Me._sets
        End Get
        Set(ByVal value As ArrayList)
            Me._sets = value
        End Set
    End Property

    Public Property UseImgCache() As Boolean
        Get
            Return Me._useimgcache
        End Get
        Set(ByVal value As Boolean)
            Me._useimgcache = value
        End Set
    End Property

    Public Property UseImgCacheUpdaters() As Boolean
        Get
            Return Me._useimgcacheupdater
        End Get
        Set(ByVal value As Boolean)
            Me._useimgcacheupdater = value
        End Set
    End Property

    Public Property PersistImgCache() As Boolean
        Get
            Return Me._persistimagecache
        End Get
        Set(ByVal value As Boolean)
            Me._persistimagecache = value
        End Set
    End Property

    Public Property SkipLessThan() As Integer
        Get
            Return Me._skiplessthan
        End Get
        Set(ByVal value As Integer)
            Me._skiplessthan = value
        End Set
    End Property

    Public Property SkipStackSizeCheck() As Boolean
        Get
            Return Me._skipstacksizecheck
        End Get
        Set(ByVal value As Boolean)
            Me._skipstacksizecheck = value
        End Set
    End Property

    Public Property DownloadTrailers() As Boolean
        Get
            Return Me._downloadtrailers
        End Get
        Set(ByVal value As Boolean)
            Me._downloadtrailers = value
        End Set
    End Property

    Public Property UpdaterTrailers() As Boolean
        Get
            Return Me._updatertrailers
        End Get
        Set(ByVal value As Boolean)
            Me._updatertrailers = value
        End Set
    End Property

    Public Property UpdaterTrailersNoDownload() As Boolean
        Get
            Return Me._updatertrailersnodownload
        End Get
        Set(ByVal value As Boolean)
            Me._updatertrailersnodownload = value
        End Set
    End Property

    Public Property SingleScrapeTrailer() As Boolean
        Get
            Return Me._singlescrapetrailer
        End Get
        Set(ByVal value As Boolean)
            Me._singlescrapetrailer = value
        End Set
    End Property

    Public Property TrailerTimeout() As Integer
        Get
            Return Me._trailertimeout
        End Get
        Set(ByVal value As Integer)
            Me._trailertimeout = value
        End Set
    End Property

    Public Property OverwriteTrailer() As Boolean
        Get
            Return Me._overwritetrailer
        End Get
        Set(ByVal value As Boolean)
            Me._overwritetrailer = value
        End Set
    End Property

    Public Property DeleteAllTrailers() As Boolean
        Get
            Return Me._deletealltrailers
        End Get
        Set(ByVal value As Boolean)
            Me._deletealltrailers = value
        End Set
    End Property

    Public Property TrailerSites() As List(Of Master.TrailerPages)
        Get
            Return Me._trailersites
        End Get
        Set(ByVal value As List(Of Master.TrailerPages))
            Me._trailersites = value
        End Set
    End Property

    Public Property NoSaveImagesToNfo() As Boolean
        Get
            Return Me._nosaveimagestonfo
        End Get
        Set(ByVal value As Boolean)
            Me._nosaveimagestonfo = value
        End Set
    End Property

    Public Property ShowDims() As Boolean
        Get
            Return Me._showdims
        End Get
        Set(ByVal value As Boolean)
            Me._showdims = value
        End Set
    End Property

    Public Property NoDisplayPoster() As Boolean
        Get
            Return Me._nodisplayposter
        End Get
        Set(ByVal value As Boolean)
            Me._nodisplayposter = value
        End Set
    End Property

    Public Property NoDisplayFanart() As Boolean
        Get
            Return Me._nodisplayfanart
        End Get
        Set(ByVal value As Boolean)
            Me._nodisplayfanart = value
        End Set
    End Property

    Public Property AllwaysDisplayGenresText() As Boolean
        Get
            Return Me._allwaysdisplaygenrestext
        End Get
        Set(ByVal value As Boolean)
            Me._allwaysdisplaygenrestext = value
        End Set
    End Property

    Public Property OutlineForPlot() As Boolean
        Get
            Return Me._outlineforplot
        End Get
        Set(ByVal value As Boolean)
            Me._outlineforplot = value
        End Set
    End Property

    Public Property XBMCComs() As List(Of XBMCCom)
        Get
            Return Me._xbmccoms
        End Get
        Set(ByVal value As List(Of XBMCCom))
            Me._xbmccoms = value
        End Set
    End Property

    Public Property FoldersPattern() As String
        Get
            Return Me._defaultfolderspattern
        End Get
        Set(ByVal value As String)
            Me._defaultfolderspattern = value
        End Set
    End Property

    Public Property FilesPattern() As String
        Get
            Return Me._defaultfilespattern
        End Get
        Set(ByVal value As String)
            Me._defaultfilespattern = value
        End Set
    End Property

    Public Property SortPath() As String
        Get
            Return Me._sortpath
        End Get
        Set(ByVal value As String)
            Me._sortpath = value
        End Set
    End Property

    Public Property DisplayYear() As Boolean
        Get
            Return Me._displayyear
        End Get
        Set(ByVal value As Boolean)
            Me._displayyear = value
        End Set
    End Property

    Public Property SortTokens() As ArrayList
        Get
            Return Me._sorttokens
        End Get
        Set(ByVal value As ArrayList)
            Me._sorttokens = value
        End Set
    End Property

    Public Property ETNative() As Boolean
        Get
            Return Me._etnative
        End Get
        Set(ByVal value As Boolean)
            Me._etnative = value
        End Set
    End Property

    Public Property ETWidth() As Integer
        Get
            Return Me._etwidth
        End Get
        Set(ByVal value As Integer)
            Me._etwidth = value
        End Set
    End Property

    Public Property ETHeight() As Integer
        Get
            Return Me._etheight
        End Get
        Set(ByVal value As Integer)
            Me._etheight = value
        End Set
    End Property

    Public Property ETPadding() As Boolean
        Get
            Return Me._etpadding
        End Get
        Set(ByVal value As Boolean)
            Me._etpadding = value
        End Set
    End Property

    Public Property NoFilters() As Boolean
        Get
            Return Me._nofilters
        End Get
        Set(ByVal value As Boolean)
            Me._nofilters = value
        End Set
    End Property

    Public Property NoTokens() As Boolean
        Get
            Return Me._notokens
        End Get
        Set(ByVal value As Boolean)
            Me._notokens = value
        End Set
    End Property

    Public Property LevTolerance() As Integer
        Get
            Return Me._levtolerance
        End Get
        Set(ByVal value As Integer)
            Me._levtolerance = value
        End Set
    End Property

    Public Property AutoDetectVTS() As Boolean
        Get
            Return Me._autodetectvts
        End Get
        Set(ByVal value As Boolean)
            Me._autodetectvts = value
        End Set
    End Property

    Public Property FlagLang() As String
        Get
            Return Me._flaglang
        End Get
        Set(ByVal value As String)
            Me._flaglang = value
        End Set
    End Property

    Public Property Language() As String
        Get
            Return Me._language
        End Get
        Set(ByVal value As String)
            Me._language = value
        End Set
    End Property

    Public Property FieldTitle() As Boolean
        Get
            Return Me._fieldtitle
        End Get
        Set(ByVal value As Boolean)
            Me._fieldtitle = value
        End Set
    End Property

    Public Property FieldYear() As Boolean
        Get
            Return Me._fieldyear
        End Get
        Set(ByVal value As Boolean)
            Me._fieldyear = value
        End Set
    End Property

    Public Property FieldMPAA() As Boolean
        Get
            Return Me._fieldmpaa
        End Get
        Set(ByVal value As Boolean)
            Me._fieldmpaa = value
        End Set
    End Property

    Public Property FieldRelease() As Boolean
        Get
            Return Me._fieldrelease
        End Get
        Set(ByVal value As Boolean)
            Me._fieldrelease = value
        End Set
    End Property

    Public Property FieldRuntime() As Boolean
        Get
            Return Me._fieldruntime
        End Get
        Set(ByVal value As Boolean)
            Me._fieldruntime = value
        End Set
    End Property

    Public Property FieldRating() As Boolean
        Get
            Return Me._fieldrating
        End Get
        Set(ByVal value As Boolean)
            Me._fieldrating = value
        End Set
    End Property

    Public Property FieldVotes() As Boolean
        Get
            Return Me._fieldvotes
        End Get
        Set(ByVal value As Boolean)
            Me._fieldvotes = value
        End Set
    End Property

    Public Property FieldStudio() As Boolean
        Get
            Return Me._fieldstudio
        End Get
        Set(ByVal value As Boolean)
            Me._fieldstudio = value
        End Set
    End Property

    Public Property FieldGenre() As Boolean
        Get
            Return Me._fieldgenre
        End Get
        Set(ByVal value As Boolean)
            Me._fieldgenre = value
        End Set
    End Property

    Public Property FieldTrailer() As Boolean
        Get
            Return Me._fieldtrailer
        End Get
        Set(ByVal value As Boolean)
            Me._fieldtrailer = value
        End Set
    End Property

    Public Property FieldTagline() As Boolean
        Get
            Return Me._fieldtagline
        End Get
        Set(ByVal value As Boolean)
            Me._fieldtagline = value
        End Set
    End Property

    Public Property FieldOutline() As Boolean
        Get
            Return Me._fieldoutline
        End Get
        Set(ByVal value As Boolean)
            Me._fieldoutline = value
        End Set
    End Property

    Public Property FieldPlot() As Boolean
        Get
            Return Me._fieldplot
        End Get
        Set(ByVal value As Boolean)
            Me._fieldplot = value
        End Set
    End Property

    Public Property FieldCast() As Boolean
        Get
            Return Me._fieldcast
        End Get
        Set(ByVal value As Boolean)
            Me._fieldcast = value
        End Set
    End Property

    Public Property FieldDirector() As Boolean
        Get
            Return Me._fielddirector
        End Get
        Set(ByVal value As Boolean)
            Me._fielddirector = value
        End Set
    End Property

    Public Property FieldWriters() As Boolean
        Get
            Return Me._fieldwriters
        End Get
        Set(ByVal value As Boolean)
            Me._fieldwriters = value
        End Set
    End Property

    Public Property FieldProducers() As Boolean
        Get
            Return Me._fieldproducers
        End Get
        Set(ByVal value As Boolean)
            Me._fieldproducers = value
        End Set
    End Property

    Public Property FieldMusic() As Boolean
        Get
            Return Me._fieldmusic
        End Get
        Set(ByVal value As Boolean)
            Me._fieldmusic = value
        End Set
    End Property

    Public Property FieldCrew() As Boolean
        Get
            Return Me._fieldcrew
        End Get
        Set(ByVal value As Boolean)
            Me._fieldcrew = value
        End Set
    End Property

    Public Property Field250() As Boolean
        Get
            Return Me._field250
        End Get
        Set(ByVal value As Boolean)
            Me._field250 = value
        End Set
    End Property

    Public Property GenreLimit() As Integer
        Get
            Return Me._genrelimit
        End Get
        Set(ByVal value As Integer)
            Me._genrelimit = value
        End Set
    End Property

    Public Property ActorLimit() As Integer
        Get
            Return Me._actorlimit
        End Get
        Set(ByVal value As Integer)
            Me._actorlimit = value
        End Set
    End Property

    Public Property MissingFilterPoster() As Boolean
        Get
            Return Me._missingfilterposter
        End Get
        Set(ByVal value As Boolean)
            Me._missingfilterposter = value
        End Set
    End Property

    Public Property MissingFilterFanart() As Boolean
        Get
            Return Me._missingfilterfanart
        End Get
        Set(ByVal value As Boolean)
            Me._missingfilterfanart = value
        End Set
    End Property

    Public Property MissingFilterNFO() As Boolean
        Get
            Return Me._missingfilternfo
        End Get
        Set(ByVal value As Boolean)
            Me._missingfilternfo = value
        End Set
    End Property

    Public Property MissingFilterTrailer() As Boolean
        Get
            Return Me._missingfiltertrailer
        End Get
        Set(ByVal value As Boolean)
            Me._missingfiltertrailer = value
        End Set
    End Property

    Public Property MissingFilterSubs() As Boolean
        Get
            Return Me._missingfiltersubs
        End Get
        Set(ByVal value As Boolean)
            Me._missingfiltersubs = value
        End Set
    End Property

    Public Property MissingFilterExtras() As Boolean
        Get
            Return Me._missingfilterextras
        End Get
        Set(ByVal value As Boolean)
            Me._missingfilterextras = value
        End Set
    End Property

    Public Property AutoRenameMulti() As Boolean
        Get
            Return Me._autorenamemulti
        End Get
        Set(ByVal value As Boolean)
            Me._autorenamemulti = value
        End Set
    End Property

    Public Property AutoRenameSingle() As Boolean
        Get
            Return Me._autorenamesingle
        End Get
        Set(ByVal value As Boolean)
            Me._autorenamesingle = value
        End Set
    End Property

    Public Property MovieTheme() As String
        Get
            Return Me._movietheme
        End Get
        Set(ByVal value As String)
            Me._movietheme = value
        End Set
    End Property

    Public Property MetadataPerFileType() As List(Of MetadataPerType)
        Get
            Return Me._metadatapertype
        End Get
        Set(ByVal value As List(Of MetadataPerType))
            Me._metadatapertype = value
        End Set
    End Property

    Public Property EnableIFOScan() As Boolean
        Get
            Return Me._enableifoscan
        End Get
        Set(ByVal value As Boolean)
            Me._enableifoscan = value
        End Set
    End Property

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._version = String.Empty
        Me._filterCustom.Clear()
        Me._certificationLang = String.Empty
        Me._usecertformpaa = False
        Me._scanmediainfo = True
        Me._imdburl = "akas.imdb.com"
        Me._fullcast = False
        Me._fullcrew = False
        Me._castimagesonly = False
        Me._movieposterCol = False
        Me._moviefanartCol = False
        Me._movieinfoCol = False
        Me._movietrailerCol = False
        Me._moviesubCol = False
        Me._movieextraCol = False
        Me._cleanfolderJpg = False
        Me._cleanmovieTbn = False
        Me._cleanmovieTbnB = False
        Me._cleanfanartJpg = False
        Me._cleanmoviefanartJpg = False
        Me._cleanmovieNfo = False
        Me._cleanmovieNfoB = False
        Me._cleanposterTbn = False
        Me._cleanposterJpg = False
        Me._cleanmovieJpg = False
        Me._cleandotfanartJpg = False
        Me._cleanmovienameJpg = False
        Me._cleanextrathumbs = False
        Me._expertcleaner = False
        Me._cleanwhitelistvideo = False
        Me._cleanwhitelistexts.Clear()
        Me._useTMDB = True
        Me._useIMPA = False
        Me._useMPDB = False
        Me._postersize = Master.PosterSize.Xlrg
        Me._fanartsize = Master.FanartSize.Lrg
        Me._autoET = False
        Me._autoETsize = Master.FanartSize.Lrg
        Me._fanartprefsizeonly = False
        Me._posterQuality = 85
        Me._fanartQuality = 85
        Me._overwritePoster = False
        Me._overwriteFanart = False
        Me._logerrors = True
        Me._properCase = True
        Me._overwritenfo = False
        Me._validexts.Clear()
        Me._nostackexts.Clear()
        Me._movietbn = True
        Me._movienametbn = True
        Me._moviejpg = False
        Me._movienamejpg = False
        Me._postertbn = False
        Me._posterjpg = False
        Me._folderjpg = False
        Me._fanartjpg = True
        Me._movienamefanartjpg = True
        Me._movienamedotfanartjpg = False
        Me._movienfo = True
        Me._movienamenfo = True
        Me._movienamemultionly = False
        Me._dashtrailer = True
        Me._videotsparent = False
        Me._lockplot = False
        Me._lockoutline = False
        Me._locktitle = False
        Me._locktagline = False
        Me._lockrating = False
        Me._lockstudio = False
        Me._lockrating = False
        Me._locktrailer = False
        Me._singlescrapeimages = True
        Me._marknew = False
        Me._resizefanart = False
        Me._fanartheight = 0
        Me._fanartwidth = 0
        Me._resizeposter = False
        Me._posterheight = 0
        Me._posterwidth = 0
        Me._useofdbtitle = False
        Me._useofdboutline = False
        Me._useofdbplot = False
        Me._useofdbgenre = False
        Me._autothumbs = 0
        Me._autothumbnospoilers = False
        Me._windowloc = New Point(0, 0)
        Me._windowsize = New Size(1024, 768)
        Me._windowstate = FormWindowState.Normal
        Me._infopanelstate = 0
        Me._filterPanelState = False
        Me._scmainstate = 305
        Me._infopanelanim = True
        Me._checkupdates = True
        Me._bdpath = String.Empty
        Me._autobd = False
        Me._usemiduration = False
        Me._usehmforruntime = False
        Me._genrefilter = "[All]"
        Me._useetasfa = False
        Me._useimgcache = True
        Me._useimgcacheupdater = False
        Me._persistimagecache = False
        Me._skiplessthan = 0
        Me._skipstacksizecheck = False
        Me._downloadtrailers = False
        Me._updatertrailers = False
        Me._updatertrailersnodownload = False
        Me._singlescrapetrailer = False
        Me._trailertimeout = 2
        Me._overwritetrailer = False
        Me._deletealltrailers = False
        Me._trailersites.Clear()
        Me._sets.Clear()
        Me._nosaveimagestonfo = False
        Me._showdims = False
        Me._nodisplayposter = False
        Me._nodisplayfanart = False
        Me._outlineforplot = False
        Me._defaultfolderspattern = "$T {($Y)}"
        Me._defaultfilespattern = "$T{.$S}"
        Me._xbmccoms.Clear()
        Me._sortpath = String.Empty
        Me._allwaysdisplaygenrestext = False
        Me._displayyear = False
        Me._sorttokens.Clear()
        Me._etnative = True
        Me._etwidth = 0
        Me._etheight = 0
        Me._etpadding = False
        Me._nofilters = False
        Me._notokens = False
        Me._levtolerance = 0
        Me._autodetectvts = True
        Me._flaglang = String.Empty
        Me._language = String.Empty
        Me._fieldtitle = True
        Me._fieldyear = True
        Me._fieldmpaa = True
        Me._fieldrelease = True
        Me._fieldruntime = True
        Me._fieldrating = True
        Me._fieldvotes = True
        Me._fieldstudio = True
        Me._fieldgenre = True
        Me._fieldtrailer = True
        Me._fieldtagline = True
        Me._fieldoutline = True
        Me._fieldplot = True
        Me._fieldcast = True
        Me._fielddirector = True
        Me._fieldwriters = True
        Me._fieldproducers = True
        Me._fieldmusic = True
        Me._fieldcrew = True
        Me._field250 = True
        Me._genrelimit = 0
        Me._actorlimit = 0
        Me._missingfilterposter = True
        Me._missingfilterfanart = True
        Me._missingfilternfo = True
        Me._missingfiltertrailer = True
        Me._missingfiltersubs = True
        Me._missingfilterextras = True
        Me._autorenamemulti = False
        Me._autorenamesingle = False
        Me._movietheme = String.Empty
        Me._metadatapertype.Clear()
        Me._enableifoscan = True
    End Sub

    Public Sub Save()
        Try
            Dim xmlSerial As New XmlSerializer(GetType(emmSettings))
            Dim xmlWriter As New StreamWriter(Path.Combine(Master.AppPath, "Settings.xml"))
            xmlSerial.Serialize(xmlWriter, Master.eSettings)
            xmlWriter.Close()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Load()
        Try
            Dim xmlSerial As New XmlSerializer(GetType(emmSettings))
            If File.Exists(Path.Combine(Master.AppPath, "Settings.xml")) Then
                Dim strmReader As New StreamReader(Path.Combine(Master.AppPath, "Settings.xml"))
                Master.eSettings = CType(xmlSerial.Deserialize(strmReader), emmSettings)
                strmReader.Close()
            Else
                Master.eSettings = New emmSettings
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Master.eSettings = New emmSettings
        End Try

        If Not Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision) Then
            SetDefaultsForLists()
        End If
    End Sub

    Public Sub SetDefaultsForLists()
        If Master.eSettings.FilterCustom.Count <= 0 AndAlso Not Master.eSettings.NoFilters Then
            Master.eSettings.FilterCustom.Add("[ _.-]\(?\d{4}\)?.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]blu[ _.-]?ray.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]bd[ _.-]?rip.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]dvd.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]720.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]1080.*") 'not really needed because the year title will catch this one, but just in case a user doesn't want the year filter but wants to filter 1080
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]ac3.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]dts.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]divx.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]xvid.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]dc[ _.-]?.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]dir(ector'?s?)?\s?cut.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]extended.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]hd(tv)?.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]unrated.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]uncut.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]([a-z]{3}|multi)[sd]ub.*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]\[offline\].*")
            Master.eSettings.FilterCustom.Add("(?i)[ _.-]ntsc.*")
            Master.eSettings.FilterCustom.Add("[ _.-]PAL[ _.-]?.*")
            Master.eSettings.FilterCustom.Add("\.[->] ")
        End If

        If Master.eSettings.SortTokens.Count <= 0 AndAlso Not Master.eSettings.NoTokens Then
            Master.eSettings.SortTokens.Add("the[ _\.-]")
            Master.eSettings.SortTokens.Add("a[ _\.-]")
            Master.eSettings.SortTokens.Add("an[ _\.-]")
        End If

        If Master.eSettings.ValidExts.Count <= 0 Then
            Master.eSettings.ValidExts.AddRange(Strings.Split(".avi,.divx,.mkv,.iso,.mpg,.mp4,.wmv,.wma,.mov,.mts,.m2t,.img,.dat,.bin,.cue,.vob,.dvb,.evo,.asf,.asx,.avs,.nsv,.ram,.ogg,.ogm,.ogv,.flv,.swf,.nut,.viv,.rar,.m2ts,.dvr-ms,.ts,.m4v,.rmvb", ","))
        End If
    End Sub

    Public Class XBMCCom
        Private _xbmcname As String
        Private _xbmcport As String
        Private _xbmcip As String
        Private _xbmcusername As String
        Private _xbmcpassword As String

        Public Property Name() As String
            Get
                Return Me._xbmcname
            End Get
            Set(ByVal value As String)
                Me._xbmcname = value
            End Set
        End Property

        Public Property IP() As String
            Get
                Return Me._xbmcip
            End Get
            Set(ByVal value As String)
                Me._xbmcip = value
            End Set
        End Property

        Public Property Port() As String
            Get
                Return Me._xbmcport
            End Get
            Set(ByVal value As String)
                Me._xbmcport = value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return Me._xbmcusername
            End Get
            Set(ByVal value As String)
                Me._xbmcusername = value
            End Set
        End Property

        Public Property Password() As String
            Get
                If String.IsNullOrEmpty(Me._xbmcpassword) Then
                    Return String.Empty
                Else
                    Return StringManip.Decode(Me._xbmcpassword)
                End If
            End Get
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then
                    Me._xbmcpassword = value
                Else
                    Me._xbmcpassword = StringManip.Encode(value)
                End If
            End Set
        End Property

        Public Sub New()
            Clear()
        End Sub

        Public Sub Clear()
            Me._xbmcname = String.Empty
            Me._xbmcip = String.Empty
            Me._xbmcport = String.Empty
            Me._xbmcusername = String.Empty
            Me._xbmcpassword = String.Empty
        End Sub
    End Class

    Public Class MetadataPerType
        Private _filetype As String
        Private _metadata As MediaInfo.Fileinfo

        Public Property FileType() As String
            Get
                Return Me._filetype
            End Get
            Set(ByVal value As String)
                Me._filetype = value
            End Set
        End Property

        Public Property MetaData() As MediaInfo.Fileinfo
            Get
                Return Me._metadata
            End Get
            Set(ByVal value As MediaInfo.Fileinfo)
                Me._metadata = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._filetype = String.Empty
            Me._metadata = New MediaInfo.Fileinfo
        End Sub
    End Class
End Class
