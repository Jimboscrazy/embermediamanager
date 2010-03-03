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

Imports System.Windows.Forms
Imports System.IO
Public Class frmMediaSettingsHolder
    Public Event SetupPostScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent SetupPostScraperChanged(cbEnabled.Checked, 0)
    End Sub

    Private Sub chkDownloadTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDownloadTrailer.CheckedChanged
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
        'Me.SetApplyButton(True)
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
        Me.chkTrailerDump.Text = Master.eLang.GetString(999, "Watch for ""Dump"" Folder")
        Me.chkDownloadTrailer.Text = Master.eLang.GetString(999, "Enable Downloading")
    End Sub

    Private Sub frmMediaSettingsHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetUp()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With Me.fbdBrowse
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not String.IsNullOrEmpty(.SelectedPath.ToString) AndAlso Directory.Exists(.SelectedPath) Then
                    Me.txtDumpPath.Text = .SelectedPath.ToString
                End If
            End If
        End With
    End Sub

    Private Sub chkTrailerDump_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTrailerDump.CheckedChanged
        txtDumpPath.Enabled = chkTrailerDump.Checked
        btnBrowse.Enabled = chkTrailerDump.Checked
    End Sub
End Class
