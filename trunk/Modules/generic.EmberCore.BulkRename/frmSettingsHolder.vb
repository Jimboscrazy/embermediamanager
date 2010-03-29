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

Public Class frmSettingsHolder

    #Region "Events"

    Public Event ModuleEnabledChanged(ByVal State As Boolean, ByVal difforder As Integer)

    Public Event ModuleSettingsChanged()

    #End Region 'Events

    #Region "Methods"

    Private Sub chkBulRenamer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBulkRenamer.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(chkEnabled.Checked, 0)
    End Sub

    Private Sub chkGenericModule_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGenericModule.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkOnError_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameMulti_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRenameMulti.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub chkRenameSingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRenameSingle.CheckedChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SetUp()
        Me.chkRenameMulti.Text = Master.eLang.GetString(21, "Automatically Rename Files During Multi-Scraper")
        Me.chkRenameSingle.Text = Master.eLang.GetString(22, "Automatically Rename Files During Single-Scraper")
        Me.gbRenamerPatterns.Text = Master.eLang.GetString(23, "Default Renaming Patterns")
        Me.lblFilePattern.Text = Master.eLang.GetString(24, "Files Pattern")
        Me.lblFolderPattern.Text = Master.eLang.GetString(25, "Folders Pattern")
        Me.cbEnabled.Text = Master.eLang.GetString(774, "Enabled", True)
    End Sub

    Private Sub txtFilePattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilePattern.TextChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub txtFolderPattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolderPattern.TextChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub

    #End Region 'Methods

End Class