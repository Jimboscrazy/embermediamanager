Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports generic.RSSreader.RSS_IMDB

Public Class RSSItem
    Private _title As String
    Public Property title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _link As String
    Public Property link() As String
        Get
            Return _link
        End Get
        Set(ByVal value As String)
            _link = value
        End Set
    End Property

    Private _description As String
    Public Property description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property

    Private _name As String
    Public Property name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Private _year As String
    Public Property year() As String
        Get
            Return _year
        End Get
        Set(ByVal value As String)
            _year = value
        End Set
    End Property
    Enum Processed_State
        None = 0
        IMDB = 1
        Database = 2
        Found = 3
        NotFound = 4
        Unknow = 5
    End Enum
    Private _Processed As Processed_State = 0
    Public Property Processed() As Processed_State
        Get
            Return _Processed
        End Get
        Set(ByVal value As Processed_State)
            _Processed = value
        End Set
    End Property
    Private _imdbid As String
    Public Property imdb_id() As String
        Get
            Return _imdbid
        End Get
        Set(ByVal value As String)
            _imdbid = value
        End Set
    End Property
    Private _crc As String
    Public ReadOnly Property crc() As String
        Get
            Return _crc
        End Get
    End Property
    Function compute() As String
        _crc = RSSReader.Gethash(Me)
        Return _crc
    End Function
    Private _gui_expanded As Boolean = False
    Public Property Gui_Expanded() As Boolean
        Get
            Return _gui_expanded
        End Get
        Set(ByVal value As Boolean)
            _gui_expanded = value
        End Set
    End Property
    Public ref_object As Object
End Class
Public Class RSSReader
    Private Shared dtMovieMedia As DataTable
    Public Event NewRSSItem()
    Public Event RSSItemChanged(ByRef rssitem As RSSItem)
    Private haveNew As Boolean = False
    Friend WithEvents bwCheckFeeds As New System.ComponentModel.BackgroundWorker
    Public title As String
    Public link As String
    Public description As String
    Public items As New List(Of RSSItem)
    Private _url As String
    Private FileMarkers As String = AdvancedSettings.GetSetting("CleanMarkers", "720p|720i|1080p|1080i|divx|xvid|x264|dvdrip|brrip|bluray|blu-ray|h264|[")
    Public Sub New(ByVal url As String)
        _url = url
        StartCheck()
    End Sub

    Sub StartCheck()
        bwCheckFeeds.WorkerSupportsCancellation = True
        bwCheckFeeds.WorkerReportsProgress = True
        bwCheckFeeds.RunWorkerAsync()
    End Sub
    Sub StopCheck()
        If bwCheckFeeds.IsBusy Then bwCheckFeeds.CancelAsync()
    End Sub

    Private Sub bwCheckFeeds_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCheckFeeds.DoWork
        Try

            Dim imdb As New RSS_IMDB
            Dim sresult As MovieSearchResults
            Dim loopCount As Integer = 60
            While Not bwCheckFeeds.CancellationPending
                loopCount = 60
                Read()
                bwCheckFeeds.ReportProgress(1)

                For Each i As RSSItem In items.Where(Function(o) o.Processed = RSSItem.Processed_State.None)
                    i.Processed = RSSItem.Processed_State.IMDB
                    RaiseEvent RSSItemChanged(i)
                    sresult = imdb.SearchMovie(i.name)
                    Dim exactHaveYear As Integer = IMDB_FindYear(i.year, sresult.ExactMatches)
                    Dim popularHaveYear As Integer = IMDB_FindYear(i.name, sresult.PopularTitles)
                    'it seems "popular matches" is a better result than "exact matches"
                    If sresult.ExactMatches.Count = 1 AndAlso sresult.PopularTitles.Count = 0 AndAlso sresult.PartialMatches.Count = 0 Then 'redirected to imdb info page
                        i.imdb_id = sresult.ExactMatches.Item(0).IMDBID
                        i.name = sresult.ExactMatches.Item(0).Title
                    ElseIf (popularHaveYear >= 0 OrElse exactHaveYear = -1) AndAlso sresult.PopularTitles.Count > 0 AndAlso sresult.PopularTitles(If(popularHaveYear >= 0, popularHaveYear, 0)).Lev <= 5 Then
                        i.imdb_id = sresult.PopularTitles.Item(If(popularHaveYear >= 0, popularHaveYear, 0)).IMDBID
                        i.name = sresult.PopularTitles.Item(If(popularHaveYear >= 0, popularHaveYear, 0)).Title
                    ElseIf sresult.ExactMatches.Count > 0 AndAlso sresult.ExactMatches(If(exactHaveYear >= 0, exactHaveYear, 0)).Lev <= 5 Then
                        i.imdb_id = sresult.ExactMatches.Item(If(exactHaveYear >= 0, exactHaveYear, 0)).IMDBID
                        i.name = sresult.ExactMatches.Item(If(exactHaveYear >= 0, exactHaveYear, 0)).Title
                    ElseIf sresult.PartialMatches.Count > 0 Then
                        'i.imdb_id = sresult.PartialMatches.Item(0).IMDBID
                        'i.name = sresult.PartialMatches.Item(0).Title
                        i.imdb_id = String.Empty
                        i.Processed = RSSItem.Processed_State.Unknow
                    Else
                        i.imdb_id = String.Empty
                        i.Processed = RSSItem.Processed_State.Unknow
                    End If
                    RaiseEvent RSSItemChanged(i)
                Next
                bwCheckFeeds.ReportProgress(1)
                If dtMovieMedia Is Nothing AndAlso Master.DB.SQLcn.State = ConnectionState.Open Then
                    dtMovieMedia = New DataTable
                    Master.DB.FillDataTable(dtMovieMedia, "SELECT * FROM movies ORDER BY ListTitle COLLATE NOCASE;")
                End If
                If Not dtMovieMedia Is Nothing Then
                    For Each i As RSSItem In items.Where(Function(o) o.Processed = RSSItem.Processed_State.IMDB AndAlso Not o.imdb_id = String.Empty)
                        i.Processed = RSSItem.Processed_State.Database
                        Dim _rows() As DataRow = dtMovieMedia.Select(String.Format("IMDB = '{0}'", i.imdb_id))
                        If _rows.Count > 0 Then
                            i.Processed = RSSItem.Processed_State.Found
                            RaiseEvent RSSItemChanged(i)
                        Else
                            i.Processed = RSSItem.Processed_State.NotFound
                            RaiseEvent RSSItemChanged(i)
                        End If
                    Next
                Else
                    loopCount = 5
                End If
                bwCheckFeeds.ReportProgress(1)
                For seconds As Integer = 1 To loopCount
                    System.Threading.Thread.Sleep(1000)
                    If bwCheckFeeds.CancellationPending Then Exit For
                Next
            End While
        Catch ex As Exception
        End Try
    End Sub
    Private Sub bwCheckFeeds_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwCheckFeeds.ProgressChanged
        If haveNew Then
            RaiseEvent NewRSSItem()
            haveNew = False
        End If
    End Sub
    Private Sub bwCheckFeeds_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCheckFeeds.RunWorkerCompleted

    End Sub
    Public Sub Read()
        Try
            Dim _items As List(Of RSSItem)
            Dim rssFeed As XElement = XElement.Load(_url).Element("channel")
            Me.title = rssFeed.Element("title").Value
            Me.link = rssFeed.Element("link").Value
            Me.description = rssFeed.Element("description").Value

            _items = (From Item In rssFeed.Elements("item") _
                     Select New RSSItem With { _
                        .title = Item.Element("title").Value, _
                        .description = Item.Element("description").Value, _
                        .link = Item.Element("link").Value}).ToList()
            For a = _items.Count - 1 To 0 Step -1 'Keep the order of the items, even on seconds calls
                Dim c As String = _items(a).compute()
                If items.Where(Function(e) e.crc = c).Count = 0 Then
                    _items(a).name = CleanFileName(_items(a).title)
                    _items(a).year = FindYear(_items(a).name)
                    items.Insert(0, _items(a))
                    haveNew = True
                End If
            Next
            If haveNew Then RaiseEvent NewRSSItem()
        Catch ex As Exception
        End Try
    End Sub

    Function CleanFileName(ByVal fname As String) As String
        ' TODO: Move this to AdvancedSettings

        Dim Markers() As String = FileMarkers.Split(New String() {"|"}, StringSplitOptions.RemoveEmptyEntries)
        fname = StringUtils.CleanStackingMarkers(fname)
        Dim i As Integer
        For Each s In Markers
            i = fname.ToLower.IndexOf(s)
            If i >= 0 Then
                fname = fname.Substring(0, i)
            End If
        Next
        fname = fname.Replace(".", " ")
        Return fname.Trim
    End Function
    Function FindYear(ByRef movie As String) As String
        Dim year As String = String.Empty
        Dim s As String = movie.Replace("(", String.Empty).Replace(")", String.Empty)
        Dim i As Integer = s.LastIndexOf(" ")
        If i >= 0 AndAlso IsNumeric(s.Substring(i)) Then
            year = s.Substring(i)
            movie = s.Substring(0, i)
        End If
        Return year
    End Function

    Shared Function Gethash(ByVal item As RSSItem) As String
        Dim sString As String = String.Concat(item.title, item.link, item.description)
        Dim KeyValue As Byte() = (New System.Text.UnicodeEncoding).GetBytes("HashingKey")
        Using HMA As New HMACSHA1(KeyValue, True)
            Dim UE As New UnicodeEncoding()
            Dim MessageBytes As Byte() = UE.GetBytes(sString)
            Dim hash() As Byte
            hash = HMA.ComputeHash(MessageBytes)
            Dim sb As New StringBuilder(hash.Length * 2)
            For i As Integer = 0 To hash.Length - 1
                sb.Append(hash(i).ToString("X2"))
            Next
            Return sb.ToString().ToLower
        End Using
    End Function
End Class
