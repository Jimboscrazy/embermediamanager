Imports System.Windows.Forms

Public Class frmAVCodecEditor
    Public Event ModuleSettingsChanged()


    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        For Each sett As AdvancedSettings.SettingItem In AdvancedSettings.GetAllSettings.Where(Function(y) y.Name.StartsWith("AudioFormatConvert:"))
            Dim i As Integer = dgvAudio.Rows.Add(New Object() {sett.Name.Substring(19), sett.Value})
        Next

        For Each sett As AdvancedSettings.SettingItem In AdvancedSettings.GetAllSettings.Where(Function(y) y.Name.StartsWith("VideoFormatConvert:"))
            Dim i As Integer = dgvVideo.Rows.Add(New Object() {sett.Name.Substring(19), sett.Value})
        Next

    End Sub

End Class
