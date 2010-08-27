' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Public Class frmMovieOutputSettings
    Public Event ModuleSettingsChanged()
    Public Event ModuleEnabledChanged(ByVal State As Boolean, ByVal difforder As Integer)

    Private Sub chkMovieNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNFO.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameNFO.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameMultiOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameMultiOnly.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieTBN.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterTBN.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFolderJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameTBN.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFanartJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameFanartJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkMovieNameDotFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameDotFanartJPG.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub rbDashTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDashTrailer.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub rbBracketTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBracketTrailer.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(chkEnabled.Checked, 0)
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Dim order As Integer = ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.AssemblyName = OutputXBMC_Module._AssemblyName).ModuleOrder
        If order < ModulesManager.Instance.externalOutputModules.Count - 1 Then
            ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.ModuleOrder = order + 1).ModuleOrder = order
            ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.AssemblyName = OutputXBMC_Module._AssemblyName).ModuleOrder = order + 1
            RaiseEvent ModuleEnabledChanged(chkEnabled.Checked, 1)
            orderChanged()
        End If
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Dim order As Integer = ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.AssemblyName = OutputXBMC_Module._AssemblyName).ModuleOrder
        If order > 0 Then
            ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.ModuleOrder = order - 1).ModuleOrder = order
            ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.AssemblyName = OutputXBMC_Module._AssemblyName).ModuleOrder = order - 1
            RaiseEvent ModuleEnabledChanged(chkEnabled.Checked, -1)
            orderChanged()
        End If
    End Sub
    Sub orderChanged()
        Dim order As Integer = ModulesManager.Instance.externalOutputModules.FirstOrDefault(Function(p) p.AssemblyName = OutputXBMC_Module._AssemblyName).ModuleOrder
        btnDown.Enabled = (order < ModulesManager.Instance.externalOutputModules.Count - 1)
        btnUp.Enabled = (order > 0)
    End Sub
    Public Sub SetUp()
        Me.chkNoSaveImagesToNfo.Text = Master.eLang.GetString(498, "Do Not Save URLs to Nfo")
    End Sub
End Class