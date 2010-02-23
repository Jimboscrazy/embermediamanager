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
Imports EmberAPI


' TODO update the tooltip and label with all the new settings

Public Class BulkRenamerModule
    Implements EmberAPI.Interfaces.EmberExternalModule
    Dim emmRuntimeObjects As New ModulesManager.EmberRuntimeObjects
    Private enabled As Boolean = False
    Private _Name As String = "Bulk Renamer"
    Sub Setup() Implements EmberAPI.Interfaces.EmberExternalModule.Setup
        'Dim _setup As New frmSetup
        '_setup.ShowDialog()
    End Sub
    Sub Enable() Implements EmberAPI.Interfaces.EmberExternalModule.Enable
        If Not enabled Then
            Dim tmpBulkRenamer As New dlgBulkRenamer
            MyMenu.Image = New Bitmap(tmpBulkRenamer.Icon.ToBitmap)
            MyMenu.Text = Master.eLang.GetString(13, "Bulk &Renamer")
            '.RenamerToolStripMenuItem.Text = Master.eLang.GetString(13, "Bulk &Renamer")
            Dim tsi As ToolStripMenuItem = DirectCast(emmRuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
            tsi.DropDownItems.Add(MyMenu)
            enabled = True
            tmpBulkRenamer.Dispose()
        End If
    End Sub
    Sub Disable() Implements EmberAPI.Interfaces.EmberExternalModule.Disable
        If enabled Then
            Dim tsi As ToolStripMenuItem = DirectCast(emmRuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
            tsi.DropDownItems.Remove(MyMenu)
            enabled = False
        End If
    End Sub
    Sub Init(ByRef emm As ModulesManager.EmberRuntimeObjects) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        emmRuntimeObjects = emm
        Master.eLang.LoadLanguage(Master.eSettings.Language)
    End Sub

    ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    'Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private Sub MyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyMenu.Click
        Using dBulkRename As New dlgBulkRenamer
            Try
                If dBulkRename.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    emmRuntimeObjects.InvokeLoadMedia(New Structures.Scans With {.Movies = True}, String.Empty)
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub
End Class
