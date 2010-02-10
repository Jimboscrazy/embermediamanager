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
            btnEditSet.Enabled = True
            btnNewSet.Enabled = True
            If lstModules.SelectedItems.Item(0).SubItems(1).Text = "Enabled" Then
                btnNewSet.Enabled = False
                btnRemoveSet.Enabled = True
            Else
                btnNewSet.Enabled = True
                btnRemoveSet.Enabled = False
            End If
        Else
            btnRemoveSet.Enabled = False
            btnNewSet.Enabled = True
            btnEditSet.Enabled = True
        End If
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ModulesManager.SaveSettings()
        Me.Close()
    End Sub


    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub btnEditSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSet.Click
        If lstModules.SelectedItems.Count > 0 Then
            ModulesManager.RunModuleSetup(lstModules.SelectedItems.Item(0).Tag().ToString)
        End If
    End Sub

    Private Sub btnNewSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewSet.Click

        btnNewSet.Enabled = False
        btnRemoveSet.Enabled = True
        lstModules.SelectedItems.Item(0).SubItems(1).Text = "Enabled"
        ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, True)

    End Sub

    Private Sub btnRemoveSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSet.Click

        btnRemoveSet.Enabled = False
        btnNewSet.Enabled = True
        lstModules.SelectedItems.Item(0).SubItems(1).Text = "Disabled"
        ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, False)

    End Sub
End Class