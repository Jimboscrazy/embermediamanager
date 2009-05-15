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

Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

<Serializable()> _
Public Class ummSettings
    Private _movieFolders As New ArrayList
    Private _filterCustom As New ArrayList
    Private _headerColor As String
    Private _backgroundColor As String
    Private _infopanelColor As String
    Private _toppanelColor As String
    Private _toppaneltextColor As String
    Private _headertextColor As String
    Private _paneltextColor As String
    Private _certificationLang As String
    Private _studiotags As Boolean
    Private _fullcast As Boolean
    Private _fullcrew As Boolean
    Private _moviemediaCol As Boolean
    Private _movieposterCol As Boolean
    Private _moviefanartCol As Boolean
    Private _movieinfoCol As Boolean
    Private _movietrailerCol As Boolean
    Private _cleanfolderJpg As Boolean
    Private _cleanmovieTbn As Boolean
    Private _cleanmovieTbnB As Boolean
    Private _cleanfanartJpg As Boolean
    Private _cleanmoviefanartJpg As Boolean
    Private _cleanmovieNfo As Boolean
    Private _cleanmovieNfoB As Boolean
    Private _useTMDB As Boolean
    Private _useIMPA As Boolean
    Private _useMPDB As Boolean
    Private _postersize As Master.PosterSize
    Private _fanartsize As Master.FanartSize
    Private _overwritePoster As Boolean
    Private _overwriteFanart As Boolean
    Private _logerrors As Boolean
    Private _usefolderName As Boolean
    Private _properCase As Boolean
    Private _movieext As Boolean
    Private _overwritenfo As Boolean
    Private _usenamefromNfo As Boolean
    Private _movielist As New ArrayList

    Public Property FilterCustom() As ArrayList
        Get
            Return Me._filterCustom
        End Get
        Set(ByVal value As ArrayList)
            Me._filterCustom = value
        End Set
    End Property

    Public Property MovieFolders() As ArrayList
        Get
            Return Me._movieFolders
        End Get
        Set(ByVal value As ArrayList)
            Me._movieFolders = value
        End Set
    End Property

    Public Property HeaderColor() As String
        Get
            Return Me._headerColor
        End Get
        Set(ByVal value As String)
            Me._headerColor = value
        End Set
    End Property

    Public Property BackgroundColor() As String
        Get
            Return Me._backgroundColor
        End Get
        Set(ByVal value As String)
            Me._backgroundColor = value
        End Set
    End Property

    Public Property InfoPanelColor() As String
        Get
            Return Me._infopanelColor
        End Get
        Set(ByVal value As String)
            Me._infopanelColor = value
        End Set
    End Property

    Public Property TopPanelColor() As String
        Get
            Return Me._toppanelColor
        End Get
        Set(ByVal value As String)
            Me._toppanelColor = value
        End Set
    End Property

    Public Property TopPanelTextColor() As String
        Get
            Return Me._toppaneltextColor
        End Get
        Set(ByVal value As String)
            Me._toppaneltextColor = value
        End Set
    End Property

    Public Property PanelTextColor() As String
        Get
            Return Me._paneltextColor
        End Get
        Set(ByVal value As String)
            Me._paneltextColor = value
        End Set
    End Property

    Public Property HeaderTextColor() As String
        Get
            Return Me._headertextColor
        End Get
        Set(ByVal value As String)
            Me._headertextColor = value
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

    Public Property UseStudioTags() As String
        Get
            Return Me._studiotags
        End Get
        Set(ByVal value As String)
            Me._studiotags = value
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

    Public Property MovieMediaCol() As Boolean
        Get
            Return Me._moviemediaCol
        End Get
        Set(ByVal value As Boolean)
            Me._moviemediaCol = value
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

    Public Property UseFolderName() As Boolean
        Get
            Return Me._usefolderName
        End Get
        Set(ByVal value As Boolean)
            Me._usefolderName = value
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

    Public Property MovieExt() As Boolean
        Get
            Return Me._movieext
        End Get
        Set(ByVal value As Boolean)
            Me._movieext = value
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

    Public Property UseNameFromNfo() As Boolean
        Get
            Return Me._usenamefromNfo
        End Get
        Set(ByVal value As Boolean)
            Me._usenamefromNfo = value
        End Set
    End Property

    Public Property MovieList() As ArrayList
        Get
            Return Me._movielist
        End Get
        Set(ByVal value As ArrayList)
            Me._movielist = value
        End Set
    End Property

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._movieFolders.Clear()
        Me._filterCustom.Clear()
        Me._headerColor = Color.DimGray.ToArgb
        Me._backgroundColor = Color.DimGray.ToArgb
        Me._infopanelColor = Color.Gainsboro.ToArgb
        Me._toppanelColor = Color.Gainsboro.ToArgb
        Me._toppaneltextColor = Color.Black.ToArgb
        Me._headertextColor = Color.White.ToArgb
        Me._paneltextColor = Color.Black.ToArgb
        Me._certificationLang = String.Empty
        Me._studiotags = False
        Me._fullcast = False
        Me._fullcrew = False
        Me._moviemediaCol = False
        Me._movieposterCol = False
        Me._moviefanartCol = False
        Me._movieinfoCol = False
        Me._movietrailerCol = False
        Me._cleanfolderJpg = False
        Me._cleanmovieTbn = False
        Me._cleanmovieTbnB = False
        Me._cleanfanartJpg = False
        Me._cleanmoviefanartJpg = False
        Me._cleanmovieNfo = False
        Me._cleanmovieNfoB = False
        Me._useTMDB = False
        Me._useIMPA = False
        Me._useMPDB = False
        Me._postersize = Master.PosterSize.Xlrg
        Me._fanartsize = Master.FanartSize.Lrg
        Me._overwritePoster = False
        Me._overwriteFanart = False
        Me._logerrors = False
        Me._usefolderName = True
        Me._properCase = False
        Me._movieext = True
        Me._overwritenfo = False
        Me._usenamefromNfo = False
        Me._movielist.Clear()
    End Sub

    Public Sub Save()
        Try
            Dim xmlSerial As New XmlSerializer(GetType(ummSettings))
            Dim xmlWriter As New StreamWriter(Application.StartupPath & "\settings.xml")
            xmlSerial.Serialize(xmlWriter, Master.uSettings)
            xmlWriter.Close()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Load()
        Dim xmlSerial As New XmlSerializer(GetType(ummSettings))
        Try
            Dim strmReader As New StreamReader(Application.StartupPath & "\settings.xml")
            Master.uSettings = CType(xmlSerial.Deserialize(strmReader), ummSettings)
            strmReader.Close()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Master.uSettings = New ummSettings
        End Try
    End Sub
End Class

