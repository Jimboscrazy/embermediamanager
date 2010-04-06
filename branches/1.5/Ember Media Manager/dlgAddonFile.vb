Imports System.IO

Public Class dlgAddonFile

    Public Overloads Function ShowDialog(ByVal sPath As String, ByVal sDescription As String) As KeyValuePair(Of String, String)
        If MyBase.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return New KeyValuePair(Of String, String)(Me.txtPath.Text, Me.txtDescription.Text)
        Else
            Return Nothing
        End If
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not String.IsNullOrEmpty(Me.txtPath.Text) AndAlso File.Exists(Me.txtPath.Text) AndAlso Not String.IsNullOrEmpty(Me.txtDescription.Text) Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
