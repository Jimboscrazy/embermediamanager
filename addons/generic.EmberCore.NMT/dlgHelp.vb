Imports System.Windows.Forms
Imports System.IO

Public Class dlgHelp
    Public Overloads Function ShowDialog(ByVal fpath As String) As Windows.Forms.DialogResult
        If File.Exists(Path.Combine(fpath, "help.txt")) Then
            txtHelp.Text = File.ReadAllText(Path.Combine(fpath, "help.txt"))
        End If
        Return MyBase.ShowDialog
    End Function
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

End Class
