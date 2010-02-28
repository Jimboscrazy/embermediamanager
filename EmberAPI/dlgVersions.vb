Imports System.Text

Public Class dlgVersions

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim sVersions As New StringBuilder
        For Each lItem As ListViewItem In lstVersions.Items
            sVersions.AppendLine(String.Format("{0} (Version: {1})", lItem.Text, lItem.SubItems(1).Text))
        Next
        Clipboard.SetText(sVersions.ToString)
        sVersions = Nothing
    End Sub
End Class
