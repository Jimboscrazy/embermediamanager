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
    Public Event ModuleSettingsChanged()

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent SetupPostScraperChanged(cbEnabled.Checked, 0)
    End Sub

    Sub SetUp()
        Me.chkTrailerDump.Text = Master.eLang.GetString(829, "Watch for ""Dump"" Folder")
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
        RaiseEvent ModuleSettingsChanged()
        txtDumpPath.Enabled = chkTrailerDump.Checked
        btnBrowse.Enabled = chkTrailerDump.Checked
    End Sub

    Private Sub txtDumpPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDumpPath.TextChanged

    End Sub
End Class
