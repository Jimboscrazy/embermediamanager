Imports System.Windows.Forms

Public Class dlgFIStreamEditor
    Private stream_v As New MediaInfo.Video
    Private stream_a As New MediaInfo.Audio
    Private stream_s As New MediaInfo.Subtitle
    Public Overloads Function ShowDialog(ByVal stream_type As String)
        Try

            GroupBox1.Visible = False
            GroupBox2.Visible = False
            GroupBox2.Visible = False
            If stream_type = Master.eLang.GetString(595, "Video Stream") Then
                GroupBox1.Visible = True
                Dim xVTypeFlag = From xVType In XML.FlagsXML...<vtype>...<name> Select xVType.@searchstring
                For Each p() As String In xVTypeFlag.ToArray.Cast(Of String)().Select(Function(AL) AL.Split("|"))
                    cbVideoCodec.Items.AddRange(p)
                Next
            End If
            If stream_type = Master.eLang.GetString(596, "Audio Stream") Then
                GroupBox2.Visible = True
                Dim xATypeFlag = From xAType In XML.FlagsXML...<atype>...<name> Select xAType.@searchstring
                For Each p() As String In xATypeFlag.ToArray.Cast(Of String)().Select(Function(AL) AL.Split("|"))
                    cbAudioCodec.Items.AddRange(p)
                Next
                Dim xShortLang = From xLang In XML.LanguageXML.Descendants("Language") Select xLang.Element("Name").Value
                cbAudioLanguage.Items.AddRange(xShortLang.ToArray)
            End If
            If stream_type = Master.eLang.GetString(597, "Subtitle Stream") Then
                GroupBox3.Visible = True
                Dim xShortLang = From xLang In XML.LanguageXML.Descendants("Language") Select xLang.Element("Name").Value
                cbSubsLanguage.Items.AddRange(xShortLang.ToArray)
            End If


            If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If stream_type = Master.eLang.GetString(595, "Video Stream") Then
                    stream_v.Codec = If(cbVideoCodec.SelectedItem Is Nothing, "", cbVideoCodec.SelectedItem)
                    stream_v.Aspect = txtARatio.Text
                    stream_v.Width = txtWidth.Text
                    stream_v.Height = txtHeight.Text
                    stream_v.Scantype = If(rbProgressive.Checked, "Progressive", "Interlaced")
                    stream_v.Duration = txtDuration.Text
                    Return stream_v
                End If
                If stream_type = Master.eLang.GetString(596, "Audio Stream") Then
                    stream_a.Codec = If(cbAudioCodec.SelectedItem Is Nothing, "", cbAudioCodec.SelectedItem)
                    stream_a.LongLanguage = cbAudioLanguage.SelectedItem
                    stream_a.Language = ConvertL(cbAudioLanguage.SelectedItem)
                    stream_a.Channels = cbAudioChannels.SelectedItem
                    Return stream_a
                End If
                If stream_type = Master.eLang.GetString(597, "Subtitle Stream") Then
                    stream_s.LongLanguage = If(cbSubsLanguage.SelectedItem Is Nothing, "", cbSubsLanguage.SelectedItem)
                    stream_s.Language = ConvertL(cbSubsLanguage.SelectedItem)
                    stream_s.HasPreferred = chbPrefered.Checked.ToString
                    Return stream_s
                End If
                Return Nothing
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ConvertL(ByVal sLang As String) As String

        If XML.LanguageXML.Nodes.Count > 0 Then
            Dim xShortLang = From xLang In XML.LanguageXML.Descendants("Language") Where xLang.Element("Name").Value = sLang Select xLang.Element("Code").Value
            If xShortLang.Count > 0 Then
                Return xShortLang(0)
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgFIStreamEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class
