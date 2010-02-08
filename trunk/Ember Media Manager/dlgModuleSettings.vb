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


Imports System
Imports System.IO

Public Class dlgModuleSettings
    Public ModulesManager As EmberModules

    Private Sub lstModules_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstModules.SelectedIndexChanged
        If lstModules.SelectedItems.Count > 0 Then
            btnSetup.Enabled = True
            btnEnable.Enabled = True
            If lstModules.SelectedItems.Item(0).SubItems(1).Text = "Enabled" Then
                btnEnable.Text = "Disable"
            Else
                btnEnable.Text = "Enable"
            End If
        Else
            btnSetup.Enabled = False
            btnEnable.Enabled = False
        End If
    End Sub

    Private Sub btnSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetup.Click
        If lstModules.SelectedItems.Count > 0 Then
            ModulesManager.RunModuleSetup(lstModules.SelectedItems.Item(0).Tag().ToString)
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ModulesManager.SaveSettings()
        Me.Close()
    End Sub

    Private Sub btnEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnable.Click
        If btnEnable.Text = "Enable" Then
            btnEnable.Text = "Disable"
            lstModules.SelectedItems.Item(0).SubItems(1).Text = "Enabled"
            ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, True)
        Else
            btnEnable.Text = "Enable"
            lstModules.SelectedItems.Item(0).SubItems(1).Text = "Disabled"
            ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, False)
        End If

    End Sub
End Class