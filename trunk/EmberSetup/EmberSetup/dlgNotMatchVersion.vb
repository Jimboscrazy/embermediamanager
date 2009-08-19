Imports System.Windows.Forms

Public Class dlgNotMatchVersion

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub dlgNotMatchVersion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub dlgNotMatchVersion_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        RichTextBox1.Rtf = My.Resources.NotMatchVersion
    End Sub
End Class
