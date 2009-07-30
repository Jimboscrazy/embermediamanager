Imports System.Windows.Forms

Public Class dlgFileInfo
    Private _movie As Master.DBMovie

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgFileInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetUp()
        _movie = Master.currMovie
        LoadInfo()
    End Sub
    Sub LoadInfo()
        Dim c As Integer
        Dim g As ListViewGroup
        Dim i As ListViewItem
        lvStreams.Groups.Clear()
        lvStreams.Items.Clear()
        If _movie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
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
            i.SubItems.Add("Codec")
            i.SubItems.Add("Scan Type")
            i.SubItems.Add("Width")
            i.SubItems.Add("Height")
            i.SubItems.Add("Aspect")
            i.SubItems.Add("Duration")
            g.Items.Add(i)
            lvStreams.Items.Add(i)

            For Each v As MediaInfo.Video In _movie.Movie.FileInfo.StreamDetails.Video
                i = New ListViewItem
                i.Tag = ""
                i.Text = c
                i.SubItems.Add(v.Codec)
                i.SubItems.Add(v.Scantype)
                i.SubItems.Add(v.Width)
                i.SubItems.Add(v.Height)
                i.SubItems.Add(v.Aspect)
                i.SubItems.Add(v.Duration)
                g.Items.Add(i)
                lvStreams.Items.Add(i)
                c += 1
            Next
        End If
        If _movie.Movie.FileInfo.StreamDetails.Audio.Count > 0 Then
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
            i.SubItems.Add("Codec")
            i.SubItems.Add("Language")
            i.SubItems.Add("Channels")
            g.Items.Add(i)
            lvStreams.Items.Add(i)

            For Each v As MediaInfo.Audio In _movie.Movie.FileInfo.StreamDetails.Audio
                i = New ListViewItem
                i.Tag = ""
                i.Text = c
                i.SubItems.Add(v.Codec)
                i.SubItems.Add(v.LongLanguage)
                i.SubItems.Add(v.Channels)
                g.Items.Add(i)
                lvStreams.Items.Add(i)
                c += 1
            Next
        End If
        If _movie.Movie.FileInfo.StreamDetails.Subtitle.Count > 0 Then
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
            i.SubItems.Add("Language")
            i.SubItems.Add("Prefered")
            g.Items.Add(i)
            lvStreams.Items.Add(i)

            For Each v As MediaInfo.Subtitle In _movie.Movie.FileInfo.StreamDetails.Subtitle
                i = New ListViewItem
                i.Tag = ""
                i.Text = c
                i.SubItems.Add(v.LongLanguage)
                i.SubItems.Add(v.HasPreferred)

                g.Items.Add(i)
                lvStreams.Items.Add(i)
                c += 1
            Next
        End If
    End Sub
    Private Sub SetUp()
        cbStreamType.Items.Clear()
        cbStreamType.Items.Add(Master.eLang.GetString(599, "Video"))
        cbStreamType.Items.Add(Master.eLang.GetString(600, "Audio"))
        cbStreamType.Items.Add(Master.eLang.GetString(601, "Subtilte"))
        Me.Text = Master.eLang.GetString(594, "Metadata Editor")
        Me.Label4.Text = Master.eLang.GetString(598, "Stream Type")
    End Sub

    Private Sub lvStreams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvStreams.SelectedIndexChanged
        If lvStreams.SelectedItems.Count > 0 Then
            If lvStreams.SelectedItems.Item(0).Tag = "Header" Then
                lvStreams.SelectedItems.Clear()
                btnNewSet.Enabled = True
                btnEditSet.Enabled = False
                btnRemoveSet.Enabled = False
                cbStreamType.Enabled = True
            Else
                btnNewSet.Enabled = False
                btnEditSet.Enabled = True
                btnRemoveSet.Enabled = True
                cbStreamType.Enabled = False
            End If

        Else
            btnNewSet.Enabled = True
            btnEditSet.Enabled = False
            btnRemoveSet.Enabled = False
            cbStreamType.Enabled = False
        End If
    End Sub
End Class
