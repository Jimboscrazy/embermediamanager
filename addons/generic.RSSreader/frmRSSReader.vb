Imports System.Windows.Forms

Public Class frmRSSReader

    Private rsslist As New List(Of RSSReader)
    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    Sub AddRSS(ByVal rss As RSSReader)
        rsslist.Add(rss)
    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmRSSReader_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ShowItems()
    End Sub
    Sub ShowItems()
        Me.SuspendLayout()
        ClearItems()
        Dim tTop As Integer = 0
        For Each rss As RSSReader In rsslist
            For Each i As RSSItem In rss.items
                Dim item As New ctlRSSitem With {.CRC = i.crc, .Title = i.title, .URL = i.link, .Description = i.description, .Channel = rss.title, .Top = tTop, .Expanded = i.Gui_Expanded, .MovieTitle = i.name, .MovieYear = i.year, .IMDBID = i.imdb_id}
                i.ref_object = item
                SetStatus(i)
                pnlList.Controls.Add(item)
                AddHandler item.Event_Expand, AddressOf Handle_Expanded
                tTop += If(item.Expanded, 122, 62)
            Next
            AddHandler rss.RSSItemChanged, AddressOf Handle_NewRSSChanged
            AddHandler rss.NewRSSItem, AddressOf Handle_NewRSSItem
        Next
        Me.ResumeLayout()
    End Sub
    Sub ClearItems()
        For Each rss As RSSReader In rsslist
            RemoveHandler rss.NewRSSItem, AddressOf Handle_NewRSSItem
            For Each i As RSSItem In rss.items
                i.ref_object = Nothing
            Next
        Next
        For Each c As Control In pnlList.Controls
            RemoveHandler DirectCast(c, ctlRSSitem).Event_Expand, AddressOf Handle_Expanded
            c = Nothing
        Next
        pnlList.Controls.Clear()
    End Sub
    Delegate Sub DelegateFunction(ByRef rssitem As RSSItem)
    Sub SetStatus(ByRef rssitem As RSSItem)
        Select Case RSSItem.Processed
            Case RSSItem.Processed_State.IMDB
                DirectCast(RSSItem.ref_object, ctlRSSitem).Status = "Searching IMDB"
            Case RSSItem.Processed_State.NotFound
                DirectCast(RSSItem.ref_object, ctlRSSitem).Status = "Not found in Database"
            Case RSSItem.Processed_State.Found
                DirectCast(rssitem.ref_object, ctlRSSitem).Status = "Found in Database"
            Case rssitem.Processed_State.Unknow
                DirectCast(rssitem.ref_object, ctlRSSitem).Status = "Unknow"
        End Select
    End Sub
    Sub Handle_NewRSSChanged(ByRef rssitem As RSSItem)
        Try
            If rssitem.ref_object Is Nothing Then Return
            If Me.InvokeRequired Then
                Me.Invoke(New DelegateFunction(AddressOf Handle_NewRSSChanged), rssitem)
                Return
            End If
            'DirectCast(rssitem.ref_object, ctlRSSitem).Title = rssitem.title
            'DirectCast(rssitem.ref_object, ctlRSSitem).URL = rssitem.link
            'DirectCast(rssitem.ref_object, ctlRSSitem).Description = rssitem.description
            'DirectCast(rssitem.ref_object, ctlRSSitem).Expanded = rssitem.Gui_Expanded
            DirectCast(rssitem.ref_object, ctlRSSitem).MovieTitle = rssitem.name
            DirectCast(rssitem.ref_object, ctlRSSitem).MovieYear = rssitem.year
            DirectCast(rssitem.ref_object, ctlRSSitem).IMDBID = rssitem.imdb_id
            SetStatus(rssitem)
        Catch ex As Exception
        End Try

    End Sub
    Sub Handle_NewRSSItem()
        tsbReload.Enabled = True
    End Sub
    Sub Handle_Expanded(ByVal expanded As Boolean)
        Dim tTop As Integer = If(pnlList.Controls.Count > 0, pnlList.Controls.Item(0).Top, 0)
        Me.SuspendLayout()
        For Each i As ctlRSSitem In pnlList.Controls
            i.Top = tTop
            tTop += If(i.Expanded, 122, 62)
        Next
        Me.ResumeLayout()
    End Sub

    Private Sub tsbReload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbReload.Click
        tsbReload.Enabled = False
        ShowItems()
    End Sub
End Class
