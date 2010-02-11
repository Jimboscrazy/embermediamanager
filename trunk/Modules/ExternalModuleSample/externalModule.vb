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

Public Interface EmberExternalModule
    Sub Enable()
    Sub Disable()
    Sub Setup()
    Sub Init(ByRef emm As Object)
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
End Interface

Public Class TestEmberExternalModule
    Implements EmberExternalModule
    Dim emmAPI As New Object
    Private _Name As String = "Teste Module"
    Private _Version As String = "1.0"
    Sub Setup() Implements EmberExternalModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Sub Enable() Implements EmberExternalModule.Enable
        MyMenu.Text = "Master Library"
        MySubMenu.Text = "Move To"
        MyMenu.DropDownItems.Add(MySubMenu)
        emmAPI.MenuMediaList.Items.Add(MyMenu)
    End Sub
    Sub Disable() Implements EmberExternalModule.Disable
    End Sub
    Sub Init(ByRef emm As Object) Implements EmberExternalModule.Init
        emmAPI = emm
    End Sub

    ReadOnly Property ModuleName() As String Implements EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberExternalModule.ModuleVersion
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
