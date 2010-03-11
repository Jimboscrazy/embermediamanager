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

Public Class MovieExporterModule
    Implements Interfaces.EmberExternalModule

    Private _enabled As Boolean = False
    Private _Name As String = "Movie List Exporter"
    Private _setup As frmSettingsHolder

    Private WithEvents MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MyTrayMenu As New System.Windows.Forms.ToolStripMenuItem

    Public Event ModuleSettingsChanged() Implements Interfaces.EmberExternalModule.ModuleSettingsChanged
    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements Interfaces.EmberExternalModule.ModuleSetupChanged
    Public Event GenericEvent(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object)) Implements Interfaces.EmberExternalModule.GenericEvent

    Public ReadOnly Property ModuleType() As List(Of Enums.ModuleEventType) Implements Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Generic})
        End Get
    End Property

    Function InjectSetup() As Containers.SettingsPanel Implements Interfaces.EmberExternalModule.InjectSetup
        Me._setup = New frmSettingsHolder
        Me._setup.cbEnabled.Checked = Me._enabled
        Dim SPanel As New Containers.SettingsPanel
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings
        AddHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        AddHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Private Sub Handle_ModuleEnabledChanged(ByVal State As Boolean)
        RaiseEvent ModuleEnabledChanged(Me._Name, State, 0)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SaveSetup(ByVal DoDispose As Boolean) Implements Interfaces.EmberExternalModule.SaveSetup
        Me._enabled = Me._setup.cbEnabled.Checked
    End Sub

    Property Enabled() As Boolean Implements Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
            If _enabled Then
                Enable()
            Else
                disable()
            End If
        End Set
    End Property

    Sub Enable()
        Dim tmpExportMovies As New dlgExportMovies
        Dim tsi As New ToolStripMenuItem
        MyMenu.Image = New Bitmap(tmpExportMovies.Icon.ToBitmap)
        MyMenu.Text = "Export Movie List"
        tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
        tsi.DropDownItems.Add(MyMenu)
        MyTrayMenu.Image = New Bitmap(tmpExportMovies.Icon.ToBitmap)
        MyTrayMenu.Text = "Export Movie List"
        tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TrayMenu.Items("cmnuTrayIconTools"), ToolStripMenuItem)
        tsi.DropDownItems.Add(MyTrayMenu)
        tmpExportMovies.Dispose()

    End Sub
    Sub Disable()
        Dim tsi As New ToolStripMenuItem
        tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
        tsi.DropDownItems.Remove(MyMenu)
        tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TopMenu.Items("cmnuTrayIconTools"), ToolStripMenuItem)
        tsi.DropDownItems.Remove(MyTrayMenu)
    End Sub
    Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberExternalModule.Init
        'Master.eLang.LoadLanguage(Master.eSettings.Language)
    End Sub

    ReadOnly Property ModuleName() As String Implements Interfaces.EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Private Sub MyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyMenu.Click
        Using dExportMovies As New dlgExportMovies
            dExportMovies.ShowDialog()
        End Using
    End Sub

    Private Sub MyTrayMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyTrayMenu.Click
        Using dExportMovies As New dlgExportMovies
            dExportMovies.ShowDialog()
        End Using
    End Sub

    Public Function RunGeneric(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object)) As Interfaces.ModuleResult Implements Interfaces.EmberExternalModule.RunGeneric
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function
End Class
