Imports System.Windows.Forms

Public Class scraperSetup

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub scraperSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.Items.AddRange(EmberAPI.Localization.ISOLangGetLanguages.ToArray)
        If ComboBox1.Items.Count > 0 Then ComboBox1.SelectedIndex = ComboBox1.FindString("English")
    End Sub
End Class
