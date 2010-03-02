Public Class frmSettingsHolder
    Public preferedLanguage As String = String.Empty
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub scraperSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cLanguage.Items.AddRange(EmberAPI.Localization.ISOLangGetLanguagesList.ToArray)
        If cLanguage.Items.Count > 0 Then cLanguage.SelectedIndex = cLanguage.FindString(preferedLanguage)
    End Sub

    Private Sub cAddSubMetadata_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cAddSubMetadata.CheckedChanged
        cCheckSubsLanguage.Enabled = cAddSubMetadata.Checked
    End Sub
End Class
