Imports System.Windows.Forms

Public Class dlgDeleteAddon

    Private _id As Integer = -1

    Public Overloads Function ShowDialog(ByVal ID As Integer) As System.Windows.Forms.DialogResult
        Me._id = ID

        Return MyBase.ShowDialog
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim sHTTP As New HTTP
        Dim Result As String = sHTTP.DownloadData(String.Format("http://www.embermm.com/addons/addons.php?func=delete&id={0}&username={1}&password={2}", Me._id, Me.txtUsername.Text, Me.txtPassword.Text))
        sHTTP = Nothing

        If String.IsNullOrEmpty(Result) Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
