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

Public Class TestEmberExternalModule
    Implements EmberAPI.Interfaces.EmberExternalModule
    Dim emmAPI As New EmberModules.ExposedAPI
    Private _Name As String = "Teste Module"
    Private _Version As String = "1.0"
    Sub Setup() Implements EmberAPI.Interfaces.EmberExternalModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Sub Enable() Implements EmberAPI.Interfaces.EmberExternalModule.Enable
    End Sub
    Sub Disable() Implements EmberAPI.Interfaces.EmberExternalModule.Disable
    End Sub
    Sub Init(ByRef emm As EmberModules.ExposedAPI) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        emmAPI = emm
    End Sub

    ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property

    Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu As New System.Windows.Forms.ToolStripMenuItem
    Private Sub MySubMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MySubMenu.Click
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
End Class
