Imports System.Windows.Forms

Public Class dlgTrailerFormat
    Private _yturl As String
    Private _selectedformaturl As String

    Private WithEvents YouTube As YouTube.Scraper

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Overloads Function ShowDialog(ByVal YTURL As String) As String

        Me._yturl = YTURL

        If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return _selectedformaturl
        Else
            Return String.Empty
        End If
    End Function

    Private Sub dlgTrailerFormat_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            lstFormats.DataSource = Nothing

            YouTube = New YouTube.Scraper
            YouTube.GetVideoLinksAsync(Me._yturl)

        Catch ex As Exception
            MsgBox(Master.eLang.GetString(647, "The video format links could not be retrieved."), MsgBoxStyle.Critical, Master.eLang.GetString(648, "Error Retrieving Video Format Links"))
        End Try
    End Sub

    Private Sub lstFormats_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFormats.SelectedIndexChanged
        Try
            Me._selectedformaturl = DirectCast(lstFormats.SelectedItem, YouTube.VideoLinkItem).URL

            If Me.lstFormats.SelectedItems.Count > 0 Then
                Me.OK_Button.Enabled = True
            Else
                Me.OK_Button.Enabled = False
            End If
        Catch
        End Try
    End Sub

    Private Sub YouTube_VideoLinksRetrieved(ByVal bSuccess As Boolean) Handles YouTube.VideoLinksRetrieved
        Try

            If bSuccess Then
                lstFormats.DataSource = YouTube.VideoLinks.Values.ToList
                lstFormats.DisplayMember = "Description"
                lstFormats.ValueMember = "URL"

                If YouTube.VideoLinks.ContainsKey(Master.eSettings.PreferredTrailerQuality) Then
                    Me.lstFormats.SelectedIndex = YouTube.VideoLinks.IndexOfKey(Master.eSettings.PreferredTrailerQuality)
                ElseIf Me.lstFormats.Items.Count = 1 Then
                    Me.lstFormats.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        Finally
            Me.pnlStatus.Visible = False
            Me.lstFormats.Enabled = True
        End Try

    End Sub

End Class
