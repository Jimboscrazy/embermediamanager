Public Class frmSettingsHolder
    Public preferedLanguage As String = String.Empty
    Public Event ModuleEnabledChanged(ByVal State As Boolean)
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub scraperSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cLanguage.Items.AddRange(Localization.ISOLangGetLanguagesListAlpha2.ToArray)
        If cLanguage.Items.Count > 0 Then cLanguage.SelectedIndex = cLanguage.FindString(preferedLanguage)
    End Sub

    Private Sub cAddSubMetadata_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cAddSubMetadata.CheckedChanged
        cCheckSubsLanguage.Enabled = cAddSubMetadata.Checked
    End Sub

    Private Sub chkEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(chkEnabled.Checked)
    End Sub
End Class
