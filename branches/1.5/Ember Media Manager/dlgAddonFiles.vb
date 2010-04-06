Imports System.Windows.Forms

Public Class dlgAddonFiles

    Public Overloads Function ShowDialog(ByVal FileList As Generic.SortedList(Of String, String)) As DialogResult

        Dim lvItem As New ListViewItem
        For Each _file As KeyValuePair(Of String, String) In FileList
            lvItem = lvFiles.Items.Add(_file.Key)
            lvItem.SubItems.Add(_file.Value)
        Next

        Return MyBase.ShowDialog
    End Function

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

End Class
