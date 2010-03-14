Imports System.Windows.Forms

Public Class dlgLoading

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgLoading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtStage.Text = ""
    End Sub
    Public Sub SetStage(ByVal txt As String)
        txtStage.Text = txt
        Me.Refresh()
        Application.DoEvents()
    End Sub
End Class
