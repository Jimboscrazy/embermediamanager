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

Imports System.Windows.Forms
Imports System.IO
Public Class frmMediaSettingsHolder
    Public Event SetupPostScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
    Public Event ModuleSettingsChanged()

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent SetupPostScraperChanged(cbEnabled.Checked, 0)
    End Sub

    Private Sub chkDownloadTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDownloadTrailer.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
        CheckTrailer()
    End Sub
    Sub CheckTrailer()
        Me.txtTimeout.Enabled = Me.chkDownloadTrailer.Checked
        Me.lbTrailerSites.Enabled = Me.chkDownloadTrailer.Checked
        If Not Me.chkDownloadTrailer.Checked Then
            Me.txtTimeout.Text = "2"
            For i As Integer = 0 To lbTrailerSites.Items.Count - 1
                lbTrailerSites.SetItemChecked(i, False)
            Next
        End If
    End Sub
    Private Sub lbTrailerSites_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)
        RaiseEvent ModuleSettingsChanged()
        If e.Index = 0 AndAlso (e.NewValue = CheckState.Checked OrElse Me.lbTrailerSites.GetItemChecked(1)) Then
            'Me.cbTrailerQuality.Enabled = True
        ElseIf e.Index = 1 AndAlso (e.NewValue = CheckState.Checked OrElse Me.lbTrailerSites.GetItemChecked(0)) Then
            'Me.cbTrailerQuality.Enabled = True
        Else
            If Me.lbTrailerSites.GetItemChecked(0) OrElse Me.lbTrailerSites.GetItemChecked(1) Then
                'Me.cbTrailerQuality.Enabled = True
            Else
                'Me.cbTrailerQuality.Enabled = False
            End If
        End If
    End Sub
    Sub SetUp()
        Me.txtTimeout.Text = Master.eSettings.TrailerTimeout.ToString
        'Me.GroupBox20.Text = Master.eLang.GetString(151, "Trailers")
        Me.Label23.Text = Master.eLang.GetString(526, "Timeout:")
        Me.GroupBox2.Text = Master.eLang.GetString(528, "Supported Trailer Sites:")
        Me.chkUseMPDB.Text = Master.eLang.GetString(500, "MoviePosterDB.com")
        Me.chkUseTMDB.Text = Master.eLang.GetString(501, "TheMovieDB.org")
        Me.chkUseIMPA.Text = Master.eLang.GetString(502, "IMPAwards.com")
        Me.GroupBox9.Text = Master.eLang.GetString(798, "Get Images From:")
        Me.chkDownloadTrailer.Text = Master.eLang.GetString(529, "Enable Downloading")
    End Sub

    Private Sub frmMediaSettingsHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetUp()
    End Sub

    Private Sub chkScrapePoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScrapePoster.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkScrapeFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScrapeFanart.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub txtTimeout_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTimeout.TextChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkUseTMDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseTMDB.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkUseIMPA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseIMPA.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkUseMPDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseMPDB.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkAutoThumbs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoThumbs.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Dim order As Integer = ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.AssemblyName = EmberNativeScraperModule._AssemblyName).PostScraperOrder
        If order < ModulesManager.Instance.externalScrapersModules.Where(Function(y) y.ProcessorModule.IsPostScraper).Count - 1 Then
            ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.PostScraperOrder = order + 1).PostScraperOrder = order
            ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.AssemblyName = EmberNativeScraperModule._AssemblyName).PostScraperOrder = order + 1
            RaiseEvent SetupPostScraperChanged(cbEnabled.Checked, 1)
            orderChanged()
        End If
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Dim order As Integer = ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.AssemblyName = EmberNativeScraperModule._AssemblyName).PostScraperOrder
        If order > 0 Then
            ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.PostScraperOrder = order - 1).PostScraperOrder = order
            ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.AssemblyName = EmberNativeScraperModule._AssemblyName).PostScraperOrder = order - 1
            RaiseEvent SetupPostScraperChanged(cbEnabled.Checked, -1)
            orderChanged()
        End If

    End Sub
    Sub orderChanged()
        Dim order As Integer = ModulesManager.Instance.externalScrapersModules.FirstOrDefault(Function(p) p.AssemblyName = EmberNativeScraperModule._AssemblyName).PostScraperOrder
        btnDown.Enabled = (order < ModulesManager.Instance.externalScrapersModules.Where(Function(y) y.ProcessorModule.IsPostScraper).Count - 1)
        btnUp.Enabled = (order > 0)
    End Sub
End Class
