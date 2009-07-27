Imports System.Windows.Forms

Public Class dlgTranslationDL

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DownloadSelected()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DownloadSelected()
        Me.lblStatus.Text = Master.eLang.GetString(447, "Downloading selected translations...")
        Me.pnlStatus.Visible = True
        Application.DoEvents()

        Dim sHTTP As New HTTP
        Try
            For Each lItem As ListViewItem In lvDownload.Items
                If lItem.Checked Then
                    sHTTP.DownloadFile(lItem.Tag, String.Empty, False, "translation")
                End If
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        sHTTP = Nothing
    End Sub

    Private Sub dlgTranslationDL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(443, "Download Translation")
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.lvDownload.Columns(0).Text = Master.eLang.GetString(444, "Language")
        Me.lvDownload.Columns(1).Text = Master.eLang.GetString(445, "Last Update")
        Me.lblStatus.Text = Master.eLang.GetString(446, "Downloading available translations list...")
    End Sub

    Private Sub dlgTranslationDL_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
        Me.DownloadList()
        Me.pnlStatus.Visible = False
    End Sub

    Private Sub DownloadList()
        Dim sHTTP As New HTTP
        Try
            Dim transXML As String = sHTTP.DownloadData("http://www.embermm.com/Updates/Translations/Download.xml")
            sHTTP = Nothing

            Dim xmlTrans As XDocument = XDocument.Parse(transXML)

            Dim xTrans = From xTran In xmlTrans...<translations>...<language>
            If xTrans.Count > 0 Then
                Dim lItem As New ListViewItem
                For Each Trans In xTrans
                    lItem = lvDownload.Items.Add(Trans.@name)
                    lItem.SubItems.Add(Trans.<lastupdate>.Value)
                    lItem.Tag = Trans.<url>.Value
                    lItem = Nothing
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        sHTTP = Nothing
    End Sub
End Class
