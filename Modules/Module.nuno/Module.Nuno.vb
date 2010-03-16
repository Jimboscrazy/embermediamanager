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

Public Class NunoModule
    Implements Interfaces.EmberExternalModule

#Region "Fields"

    Private WithEvents MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private _enabled As Boolean = False
    Private _name As String = "Nuno's Test Bench"
    Private _setup As frmSettingsHolder

#End Region 'Fields

#Region "Events"

    Public Event GenericEvent(ByVal mType As Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) Implements EmberAPI.Interfaces.EmberExternalModule.GenericEvent

    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSetupChanged

    Public Event ModuleSettingsChanged() Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSettingsChanged

#End Region 'Events

#Region "Properties"

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
            SetStatus(_enabled)
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
            SetStatus(_enabled)
        End Set
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property ModuleType() As System.Collections.Generic.List(Of EmberAPI.Enums.ModuleEventType) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Generic})
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

#End Region 'Properties

#Region "Methods"

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        _enabled = True
    End Sub

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me._setup = New frmSettingsHolder
        Me._setup.chkEnabled.Checked = Me._enabled

        SPanel.Name = Me._name
        SPanel.Text = Me._name
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings
        AddHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        Return SPanel
    End Function

    Public Function RunGeneric(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) As EmberAPI.Interfaces.ModuleResult Implements EmberAPI.Interfaces.EmberExternalModule.RunGeneric
    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        Me._enabled = _setup.chkEnabled.Checked
        If DoDispose Then
            RemoveHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
            _setup.Dispose()
        End If
    End Sub

    Private Sub Handle_ModuleEnabledChanged(ByVal State As Boolean)
        RaiseEvent ModuleEnabledChanged(Me._name, State, 0)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub MyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyMenu.Click
        Using dTest As New dlgTest
            Try
                If dTest.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    'ModulesManager.Instance.RuntimeObjects.InvokeLoadMedia(New Structures.Scans With {.Movies = True}, String.Empty)
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Sub SetStatus(ByVal s As Boolean)
        If s Then
            Dim tmpTest As New dlgTest
            MyMenu.Image = New Bitmap(tmpTest.Icon.ToBitmap)
            MyMenu.Text = Master.eLang.GetString(13, "Nuno Test Tool")
            '.RenamerToolStripMenuItem.Text = Master.eLang.GetString(13, "Bulk &Renamer")
            Dim tsi As ToolStripMenuItem = DirectCast(ModulesManager.Instance.RuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
            tsi.DropDownItems.Add(MyMenu)
            _enabled = True
            tmpTest.Dispose()
        Else
            Dim tsi As ToolStripMenuItem = DirectCast(ModulesManager.Instance.RuntimeObjects.TopMenu.Items("ToolsToolStripMenuItem"), ToolStripMenuItem)
            tsi.DropDownItems.Remove(MyMenu)
            _enabled = False
        End If
    End Sub

#End Region 'Methods

End Class