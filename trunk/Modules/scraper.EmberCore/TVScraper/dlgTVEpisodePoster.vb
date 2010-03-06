Imports System.Windows.Forms

Public Class dlgTVEpisodePoster

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Overloads Function ShowDialog(ByVal Poster As Image) As System.Windows.Forms.DialogResult
        Me.pbPoster.Image = Poster
        Return MyBase.ShowDialog
    End Function

    Private Sub pbPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPoster.Click
        ModulesManager.Instance.RuntimeObjects.InvokeOpenImageViewer(Me.pbPoster.Image)
    End Sub
End Class
