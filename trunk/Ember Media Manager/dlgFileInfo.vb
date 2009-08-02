﻿Imports System.Windows.Forms

Public Class dlgFileInfo
    Private NeedToRefresh As Boolean = False
    Private _FileInfo As MediaInfo.Fileinfo
    Private SettingDefaults As Boolean = False

    Public Overloads Function ShowDialog(ByVal fi As MediaInfo.Fileinfo) As MediaInfo.Fileinfo
        SettingDefaults = True
        _FileInfo = fi
        If MyBase.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Return _FileInfo
        Else
            Return Nothing
        End If
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        If NeedToRefresh Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        End If
        Me.Close()
    End Sub

    Private Sub dlgFileInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetUp()
        If Not SettingDefaults Then
            _FileInfo = Master.currMovie.Movie.FileInfo
        End If
        LoadInfo()
    End Sub

    Private Sub LoadInfo()
        Dim c As Integer
        Dim g As New ListViewGroup
        Dim i As New ListViewItem
        lvStreams.Groups.Clear()
        lvStreams.Items.Clear()
        Try
            If _FileInfo.StreamDetails.Video.Count > 0 Then
                g = New ListViewGroup
                g.Header = Master.eLang.GetString(595, "Video Stream")
                lvStreams.Groups.Add(g)
                c = 1
                ' Fake Group Header
                i = New ListViewItem
                'i.UseItemStyleForSubItems = False
                i.ForeColor = Color.DarkBlue
                i.Tag = "Header"
                i.Text = ""
                i.SubItems.Add(Master.eLang.GetString(604, "Codec"))
                i.SubItems.Add(Master.eLang.GetString(605, "Scan Type"))
                i.SubItems.Add(Master.eLang.GetString(606, "Width"))
                i.SubItems.Add(Master.eLang.GetString(607, "Height"))
                i.SubItems.Add(Master.eLang.GetString(608, "Aspect"))
                i.SubItems.Add(Master.eLang.GetString(609, "Duration"))
                g.Items.Add(i)
                lvStreams.Items.Add(i)
                Dim v As MediaInfo.Video
                For c = 0 To _FileInfo.StreamDetails.Video.Count - 1
                    v = _FileInfo.StreamDetails.Video(c)
                    If Not v Is Nothing Then
                        i = New ListViewItem
                        i.Tag = Master.eLang.GetString(595, "Video Stream")
                        i.Text = c.ToString
                        i.SubItems.Add(v.Codec)
                        i.SubItems.Add(v.Scantype)
                        i.SubItems.Add(v.Width)
                        i.SubItems.Add(v.Height)
                        i.SubItems.Add(v.Aspect)
                        i.SubItems.Add(v.Duration)
                        g.Items.Add(i)
                        lvStreams.Items.Add(i)
                    End If
                Next
            End If
            If _FileInfo.StreamDetails.Audio.Count > 0 Then
                g = New ListViewGroup
                g.Header = Master.eLang.GetString(596, "Audio Stream")
                lvStreams.Groups.Add(g)
                c = 1
                ' Fake Group Header
                i = New ListViewItem
                'i.UseItemStyleForSubItems = False
                i.ForeColor = Color.DarkBlue
                i.Tag = "Header"
                i.Text = ""
                i.SubItems.Add(Master.eLang.GetString(604, "Codec"))
                i.SubItems.Add(Master.eLang.GetString(610, "Language"))
                i.SubItems.Add(Master.eLang.GetString(611, "Channels"))
                g.Items.Add(i)
                lvStreams.Items.Add(i)
                Dim a As MediaInfo.Audio
                For c = 0 To _FileInfo.StreamDetails.Audio.Count - 1
                    a = _FileInfo.StreamDetails.Audio(c)
                    If Not a Is Nothing Then
                        i = New ListViewItem
                        i.Tag = Master.eLang.GetString(596, "Audio Stream")
                        i.Text = c.ToString
                        i.SubItems.Add(a.Codec)
                        i.SubItems.Add(a.LongLanguage)
                        i.SubItems.Add(a.Channels)
                        g.Items.Add(i)
                        lvStreams.Items.Add(i)
                    End If
                Next
            End If
            If _FileInfo.StreamDetails.Subtitle.Count > 0 Then
                g = New ListViewGroup
                g.Header = Master.eLang.GetString(597, "Subtitle Stream")
                lvStreams.Groups.Add(g)
                c = 1
                ' Fake Group Header
                i = New ListViewItem
                'i.UseItemStyleForSubItems = False
                i.ForeColor = Color.DarkBlue
                i.Tag = "Header"
                i.Text = ""
                i.SubItems.Add(Master.eLang.GetString(610, "Language"))
                i.SubItems.Add(Master.eLang.GetString(612, "Preferred"))
                g.Items.Add(i)
                lvStreams.Items.Add(i)
                Dim s As MediaInfo.Subtitle
                For c = 0 To _FileInfo.StreamDetails.Subtitle.Count - 1
                    s = _FileInfo.StreamDetails.Subtitle(c)
                    If Not s Is Nothing Then
                        i = New ListViewItem
                        i.Tag = Master.eLang.GetString(597, "Subtitle Stream")
                        i.Text = c.ToString
                        i.SubItems.Add(s.LongLanguage)
                        i.SubItems.Add(s.HasPreferred.ToString)
                        g.Items.Add(i)
                        lvStreams.Items.Add(i)
                    End If
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SetUp()
        cbStreamType.Items.Clear()
        cbStreamType.Items.Add(Master.eLang.GetString(595, "Video Stream"))
        cbStreamType.Items.Add(Master.eLang.GetString(596, "Audio Stream"))
        cbStreamType.Items.Add(Master.eLang.GetString(597, "Subtitle Stream"))
        Me.Text = Master.eLang.GetString(594, "Meta Data Editor")
        Me.Label4.Text = Master.eLang.GetString(598, "Stream Type")
        Me.Cancel_Button.Text = Master.eLang.GetString(19, "Close")
    End Sub

    Private Sub lvStreams_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvStreams.DoubleClick
        If lvStreams.SelectedItems.Count > 0 Then
            If lvStreams.SelectedItems.Item(0).Tag.ToString <> "Header" Then
                EditStream()
            End If
        End If
    End Sub

    Private Sub lvStreams_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvStreams.KeyDown
        If e.KeyCode = Keys.Delete Then Me.DeleteStream()
    End Sub

    Private Sub lvStreams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvStreams.SelectedIndexChanged
        If lvStreams.SelectedItems.Count > 0 Then
            If lvStreams.SelectedItems.Item(0).Tag.ToString = "Header" Then
                lvStreams.SelectedItems.Clear()
                btnNewSet.Enabled = True
                btnEditSet.Enabled = False
                btnRemoveSet.Enabled = False
            Else
                btnNewSet.Enabled = False
                btnEditSet.Enabled = True
                btnRemoveSet.Enabled = True
                cbStreamType.SelectedIndex = -1
            End If

        Else
            btnNewSet.Enabled = True
            btnEditSet.Enabled = False
            btnRemoveSet.Enabled = False
        End If
    End Sub

    Private Sub cbStreamType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbStreamType.SelectedIndexChanged
        If cbStreamType.SelectedIndex <> -1 Then
            btnNewSet.Enabled = True
            btnEditSet.Enabled = False
            btnRemoveSet.Enabled = False
            lvStreams.SelectedItems.Clear()
        End If
    End Sub

    Private Sub btnRemoveSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSet.Click
        Me.DeleteStream()
    End Sub

    Private Sub DeleteStream()
        Try
            If lvStreams.SelectedItems.Count > 0 Then
                Dim i As ListViewItem = lvStreams.SelectedItems(0)
                If Not SettingDefaults Then
                    Master.currMovie.Movie.FileInfo = _FileInfo
                End If
                If i.Tag.ToString = Master.eLang.GetString(595, "Video Stream") Then
                    _FileInfo.StreamDetails.Video.RemoveAt(Convert.ToInt16(i.Text))
                End If
                If i.Tag.ToString = Master.eLang.GetString(596, "Audio Stream") Then
                    _FileInfo.StreamDetails.Audio.RemoveAt(Convert.ToInt16(i.Text))
                End If
                If i.Tag.ToString = Master.eLang.GetString(597, "Subtitle Stream") Then
                    _FileInfo.StreamDetails.Subtitle.RemoveAt(Convert.ToInt16(i.Text))
                End If
                If Cancel_Button.Visible = True Then 'Only Save imediatly when running stand alone
                    Master.DB.SaveMovieToDB(Master.currMovie, False, False, True)
                    NeedToRefresh = True
                End If
                LoadInfo()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnEditSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSet.Click
        EditStream()
    End Sub

    Private Sub EditStream()
        Try
            If lvStreams.SelectedItems.Count > 0 Then
                Dim i As ListViewItem = lvStreams.SelectedItems(0)
                Using dEditStream As New dlgFIStreamEditor
                    Dim stream As Object = dEditStream.ShowDialog(i.Tag.ToString, _FileInfo, Convert.ToInt16(i.Text))
                    If Not stream Is Nothing Then
                        If Not SettingDefaults Then
                            Master.currMovie.Movie.FileInfo = _FileInfo
                        End If
                        If i.Tag.ToString = Master.eLang.GetString(595, "Video Stream") Then
                            _FileInfo.StreamDetails.Video(Convert.ToInt16(i.Text)) = DirectCast(stream, MediaInfo.Video)
                        End If
                        If i.Tag.ToString = Master.eLang.GetString(596, "Audio Stream") Then
                            _FileInfo.StreamDetails.Audio(Convert.ToInt16(i.Text)) = DirectCast(stream, MediaInfo.Audio)
                        End If
                        If i.Tag.ToString = Master.eLang.GetString(597, "Subtitle Stream") Then
                            _FileInfo.StreamDetails.Subtitle(Convert.ToInt16(i.Text)) = DirectCast(stream, MediaInfo.Subtitle)
                        End If
                        If Cancel_Button.Visible = True Then 'Only Save imediatly when running stand alone
                            Master.DB.SaveMovieToDB(Master.currMovie, False, False, True)
                            NeedToRefresh = True
                        End If
                        LoadInfo()
                    End If
                End Using
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnNewSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewSet.Click
        Try
            If cbStreamType.SelectedIndex >= 0 Then
                Using dEditStream As New dlgFIStreamEditor
                    Dim stream As New Object
                    stream = dEditStream.ShowDialog(cbStreamType.SelectedItem.ToString, Nothing, 0)
                    If Not stream Is Nothing Then
                        If Not SettingDefaults Then
                            Master.currMovie.Movie.FileInfo = _FileInfo
                        End If
                        If cbStreamType.SelectedItem.ToString = Master.eLang.GetString(595, "Video Stream") Then
                            _FileInfo.StreamDetails.Video.Add(DirectCast(stream, MediaInfo.Video))
                        End If
                        If cbStreamType.SelectedItem.ToString = Master.eLang.GetString(596, "Audio Stream") Then
                            _FileInfo.StreamDetails.Audio.Add(DirectCast(stream, MediaInfo.Audio))
                        End If
                        If cbStreamType.SelectedItem.ToString = Master.eLang.GetString(597, "Subtitle Stream") Then
                            _FileInfo.StreamDetails.Subtitle.Add(DirectCast(stream, MediaInfo.Subtitle))
                        End If
                        If Cancel_Button.Visible = True Then 'Only Save imediatly when running stand alone
                            Master.DB.SaveMovieToDB(Master.currMovie, False, False, True)
                            NeedToRefresh = True
                        End If
                        LoadInfo()
                    End If
                End Using
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgFileInfo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub
End Class