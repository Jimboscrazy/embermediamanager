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

Public Class frmXMLSettingsHolder
    Public Event SetupScraperChanged(ByVal state As Boolean, ByVal difforder As Integer)
    Public Event ModuleSettingsChanged()
    Public Event PopulateScrapers()
    Public parentRunning As Boolean = False

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent SetupScraperChanged(cbEnabled.Checked, 0)
    End Sub

    Private Sub cbScraper_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbScraper.SelectedIndexChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub pnlSettings_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlSettings.Paint

    End Sub

    Private Sub btnPopulate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopulate.Click
        parentRunning = True
        btnPopulate.Enabled = False
        cbScraper.Enabled = False
        pnlLoading.Visible = True
        RaiseEvent PopulateScrapers()
        While parentRunning
            Application.DoEvents()
        End While
        btnPopulate.Enabled = True
        cbScraper.Enabled = True
        pnlLoading.Visible = False

    End Sub
End Class