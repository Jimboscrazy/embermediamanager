Imports System.Windows.Forms

Public Class dlgDeleteAddon

    Private _id As Integer = -1

    Public Overloads Function ShowDialog(ByVal ID As Integer) As System.Windows.Forms.DialogResult
        Me._id = ID

        Return MyBase.ShowDialog
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim sHTTP As New HTTP
        Dim postData As New List(Of String())
        postData.Add((New String() {"username", Me.txtUsername.Text}))
        postData.Add((New String() {"password", Me.txtPassword.Text}))
        postData.Add((New String() {"id", Me._id.ToString}))
        postData.Add((New String() {"func", "delete"}))

        Dim Result As String = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)
        sHTTP = Nothing

        If String.IsNullOrEmpty(Result) AndAlso Result.Contains("OK") Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
