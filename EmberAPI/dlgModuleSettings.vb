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
    Public ModulesManager As ModulesManager

    Private Sub lstModules_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstModules.SelectedIndexChanged
        If lstModules.SelectedItems.Count > 0 Then
            btnGenericSetup.Enabled = True
            btnGenericEnable.Enabled = True
            If lstModules.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled") Then
                btnGenericEnable.Enabled = False
                btnGenericDisable.Enabled = True
            Else
                btnGenericEnable.Enabled = True
                btnGenericDisable.Enabled = False
            End If
        Else
            btnGenericDisable.Enabled = False
            btnGenericEnable.Enabled = True
            btnGenericSetup.Enabled = True
        End If
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ModulesManager.SaveSettings()
        Me.Close()
    End Sub


    Private Sub TabPage1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGenreic.Click

    End Sub

    Private Sub btnGenericSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenericSetup.Click
        If lstModules.SelectedItems.Count > 0 Then
            ModulesManager.RunModuleSetup(lstModules.SelectedItems.Item(0).Tag().ToString)
        End If
    End Sub

    Private Sub btnGenericEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenericEnable.Click

        btnGenericEnable.Enabled = False
        btnGenericDisable.Enabled = True
        lstModules.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled")
        ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, True)

    End Sub

    Private Sub btnGenericDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenericDisable.Click
        btnGenericDisable.Enabled = False
        btnGenericEnable.Enabled = True
        lstModules.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(775, "Disabled")
        ModulesManager.SetModuleEnable(lstModules.SelectedItems.Item(0).Tag().ToString, False)
    End Sub

    Private Sub tabScraper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabScraper.Click

    End Sub

    Private Sub btnScraperEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScraperEnable.Click

        btnScraperEnable.Enabled = False
        btnScraperDisable.Enabled = True
        lstScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled")
        ModulesManager.SetScraperEnable(lstScrapers.SelectedItems.Item(0).Tag().ToString, True)
    End Sub

    Private Sub btnScraperDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScraperDisable.Click
        btnScraperDisable.Enabled = False
        btnScraperEnable.Enabled = True
        lstScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(775, "Disabled")
        ModulesManager.SetScraperEnable(lstScrapers.SelectedItems.Item(0).Tag().ToString, False)
    End Sub

    Private Sub btnGenericUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenericUp.Click

    End Sub

    Private Sub btnGenericDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenericDown.Click

    End Sub

    Private Sub btnScraperSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScraperSetup.Click
        If lstScrapers.SelectedItems.Count > 0 Then
            ModulesManager.RunScraperSetup(lstScrapers.SelectedItems.Item(0).Tag().ToString)
        End If
    End Sub

    Private Sub lstScrapers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstScrapers.SelectedIndexChanged
        If lstScrapers.SelectedItems.Count > 0 Then
            btnScraperSetup.Enabled = True
            btnScraperEnable.Enabled = True
            If lstScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled") Then
                btnScraperEnable.Enabled = False
                btnScraperDisable.Enabled = True
            Else
                btnScraperEnable.Enabled = True
                btnScraperDisable.Enabled = False
            End If
            btnScraperUp.Enabled = If(lstScrapers.SelectedItems.Item(0).Index > 0, True, False)
            btnScraperDown.Enabled = If(lstScrapers.SelectedItems.Item(0).Index < lstScrapers.Items.Count - 1, True, False)
        Else
            btnScraperDisable.Enabled = False
            btnScraperEnable.Enabled = True
            btnScraperSetup.Enabled = True
        End If
    End Sub

    Private Sub lstPostScrapers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPostScrapers.SelectedIndexChanged
        If lstPostScrapers.SelectedItems.Count > 0 Then
            btnPostScraperSetup.Enabled = True
            btnPostScraperEnable.Enabled = True
            If lstPostScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled") Then
                btnPostScraperEnable.Enabled = False
                btnPostScraperDisable.Enabled = True
            Else
                btnPostScraperEnable.Enabled = True
                btnPostScraperDisable.Enabled = False
            End If
            btnPostScraperUp.Enabled = If(lstPostScrapers.SelectedItems.Item(0).Index > 0, True, False)
            btnPostScraperDown.Enabled = If(lstPostScrapers.SelectedItems.Item(0).Index < lstPostScrapers.Items.Count - 1, True, False)
        Else
            btnPostScraperDisable.Enabled = False
            btnPostScraperEnable.Enabled = True
            btnPostScraperSetup.Enabled = True
        End If
    End Sub

    Private Sub btnPostScraperEnable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostScraperEnable.Click
        btnPostScraperEnable.Enabled = False
        btnPostScraperDisable.Enabled = True
        lstPostScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(774, "Enabled")
        ModulesManager.SetPostScraperEnable(lstPostScrapers.SelectedItems.Item(0).Tag().ToString, True)
    End Sub

    Private Sub btnPostScraperDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostScraperDisable.Click
        btnPostScraperDisable.Enabled = False
        btnPostScraperEnable.Enabled = True
        lstPostScrapers.SelectedItems.Item(0).SubItems(1).Text = Master.eLang.GetString(775, "Disabled")
        ModulesManager.SetPostScraperEnable(lstPostScrapers.SelectedItems.Item(0).Tag().ToString, False)
    End Sub

    Private Sub btnPostScraperSetup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostScraperSetup.Click
        If lstPostScrapers.SelectedItems.Count > 0 Then
            ModulesManager.RunPostScraperSetup(lstPostScrapers.SelectedItems.Item(0).Tag().ToString)
        End If
    End Sub

    Private Sub btnScraperUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScraperUp.Click
        Dim li As ListViewItem = lstScrapers.SelectedItems.Item(0)
        Dim index As Integer = li.Index - 1
        lstScrapers.Items.Remove(li)
        lstScrapers.Items.Insert(index, li)
        For Each x As ListViewItem In lstScrapers.Items
            ModulesManager.SetScraperOrder(x.Tag().ToString, x.Index)
        Next
    End Sub

    Private Sub btnScraperDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScraperDown.Click
        Dim li As ListViewItem = lstScrapers.SelectedItems.Item(0)
        Dim index As Integer = li.Index + 1
        lstScrapers.Items.Remove(li)
        lstScrapers.Items.Insert(index, li)
        For Each x As ListViewItem In lstScrapers.Items
            ModulesManager.SetScraperOrder(x.Tag().ToString, x.Index)
        Next
    End Sub

    Private Sub btnPostScraperUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostScraperUp.Click
        Dim li As ListViewItem = lstPostScrapers.SelectedItems.Item(0)
        Dim index As Integer = li.Index - 1
        lstPostScrapers.Items.Remove(li)
        lstPostScrapers.Items.Insert(index, li)
        For Each x As ListViewItem In lstPostScrapers.Items
            ModulesManager.SetPostScraperorder(x.Tag().ToString, x.Index)
        Next
    End Sub

    Private Sub btnPostScraperDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostScraperDown.Click
        Dim li As ListViewItem = lstPostScrapers.SelectedItems.Item(0)
        Dim index As Integer = li.Index + 1
        lstPostScrapers.Items.Remove(li)
        lstPostScrapers.Items.Insert(index, li)
        For Each x As ListViewItem In lstPostScrapers.Items
            ModulesManager.SetPostScraperorder(x.Tag().ToString, x.Index)
        Next
    End Sub
    Sub SetUP()

    End Sub
End Class