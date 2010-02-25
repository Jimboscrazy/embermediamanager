﻿' ################################################################################
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

Imports System.IO

Public Class dlgNewVersion
    Private WithEvents bwDownloadSetup As New System.ComponentModel.BackgroundWorker
    Private Sub llClick_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClick.LinkClicked

        If Master.isWindows Then
            Process.Start("http://www.embermm.com/tab/show/embermm")
        Else
            Using Explorer As New Process
                Explorer.StartInfo.FileName = "xdg-open"
                Explorer.StartInfo.Arguments = "http://www.embermm.com/tab/show/embermm"
                Explorer.Start()
            End Using
        End If

    End Sub

    Public Overloads Function ShowDialog(ByVal iNew As Integer) As Windows.Forms.DialogResult

        Me.lblNew.Text = String.Format(Master.eLang.GetString(210, "Version r{0} is now available."), iNew)
        Me.txtChangelog.Text = Functions.GetChangelog.Replace("\n", vbNewLine)

        Return MyBase.ShowDialog()
    End Function

    Private Sub dlgNewVersion_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub dlgNewVersion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(209, "A New Version Is Available")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.llClick.Text = Master.eLang.GetString(211, "Click Here")
        Me.Label2.Text = Master.eLang.GetString(212, "to visit embermm.com.")
        Me.lblStart.Text = Master.eLang.GetString(999, "Preparing for upgrade ...")
        Me.lblUpgrade.Text = Master.eLang.GetString(999, "We are now ready to upgrade. Ember will now close so the Upgrade can start.\n\nDo you want to continue?").Replace("\n", vbCrLf)
        Me.btnYes.Text = Master.eLang.GetString(999, "YES")
        Me.btnNo.Text = Master.eLang.GetString(999, "NO")
        Me.btnUpgrade.Text = Master.eLang.GetString(999, "Upgrade")

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpgrade.Click
        btnUpgrade.Enabled = False
        pnlUpgrade.Visible = True
        bwDownloadSetup.RunWorkerAsync()
        While bwDownloadSetup.IsBusy
            Application.DoEvents()
        End While
        lblStart.Visible = False
        pbUpgrade.Visible = False
        lblUpgrade.Visible = True
        btnNo.Visible = True
        btnYes.Visible = True
    End Sub

    Private Sub bwbwDownloadSetup_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadSetup.DoWork
        If File.Exists(Path.Combine(EmberAPI.Functions.AppPath, "EmberSetup.exe")) Then
            File.Delete(Path.Combine(EmberAPI.Functions.AppPath, "EmberSetup.exe"))
        End If
        Dim lhttp As New EmberAPI.HTTP
        lhttp.DownloadFile("http://www.embermm.com/Updates/EmberSetup.exe", Path.Combine(EmberAPI.Functions.AppPath, "EmberSetup.exe"), False, "other")
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        If Master.isWindows Then
            Process.Start(Path.Combine(EmberAPI.Functions.AppPath, "EmberSetup.exe"))
        Else
            Using Explorer As New Process
                Explorer.StartInfo.FileName = "xdg-open"
                Explorer.StartInfo.Arguments = Path.Combine(EmberAPI.Functions.AppPath, "EmberSetup.exe")
                Explorer.Start()
            End Using
        End If
        Application.Exit()
    End Sub
End Class
