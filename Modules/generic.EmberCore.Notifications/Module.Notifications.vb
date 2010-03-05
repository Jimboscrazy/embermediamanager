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

Public Class NotificationsModule
    Implements Interfaces.EmberExternalModule
    Private _enabled As Boolean = False
    Private _name As String = "Notifications"
    Private _setup As frmSettingsHolder
    Private eSettings As New NotifySettings
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberExternalModule.ModuleSettingsChanged
    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean) Implements Interfaces.EmberExternalModule.ModuleEnabledChanged

    Public ReadOnly Property ModuleType() As Enums.ModuleType Implements Interfaces.EmberExternalModule.ModuleType
        Get
            Return Enums.ModuleType.Notification
        End Get
    End Property

    Public Property Enabled() As Boolean Implements Interfaces.EmberExternalModule.Enabled
        Get
            Return Me._enabled
        End Get
        Set(ByVal value As Boolean)
            Me._enabled = value
        End Set
    End Property

    Public ReadOnly Property ModuleName() As String Implements Interfaces.EmberExternalModule.ModuleName
        Get
            Return Me._name
        End Get
    End Property

    ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Sub Init() Implements EmberAPI.Interfaces.EmberExternalModule.Init
        LoadSettings()
    End Sub

    Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        Me._enabled = _setup.chkEnabled.Checked
        eSettings.OnError = _setup.chkOnError.Checked
        SaveSettings()
        If DoDispose Then
            RemoveHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
            RemoveHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

    Public Function InjectSetup() As Containers.SettingsPanel Implements Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me._setup = New frmSettingsHolder
        Me._setup.chkEnabled.Checked = Me._enabled
        Me._setup.chkOnError.Checked = eSettings.OnError
        SPanel.Name = Me._name
        SPanel.Text = Me._name
        SPanel.Type = Master.eLang.GetString(999, "Modules")
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings
        AddHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        AddHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Private Sub Handle_ModuleEnabledChanged(ByVal State As Boolean)
        RaiseEvent ModuleEnabledChanged(Me._name, State)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Public Sub RunGeneric(ByVal _params As List(Of Object)) Implements Interfaces.EmberExternalModule.RunGeneric
        Try
            Dim ShowIt As Boolean = False

            Select Case True
                Case _params(0).ToString = "error" AndAlso eSettings.OnError
                    ShowIt = True
            End Select

            If ShowIt Then
                Dim dNotify As New frmNotify
                dNotify.Show(Convert.ToInt32(_params(1)), _params(2).ToString, _params(3).ToString, If(Not IsNothing(_params(4)), DirectCast(_params(4), Image), Nothing))
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SaveSettings()
        AdvancedSettings.SetBooleanSetting("NotifyOnError", eSettings.OnError)
    End Sub

    Private Sub LoadSettings()
        eSettings.OnError = AdvancedSettings.GetBooleanSetting("NotifyOnError", True)
    End Sub

    Class NotifySettings
        Private _onerror As Boolean
        Public Property OnError() As Boolean
            Get
                Return Me._onerror
            End Get
            Set(ByVal value As Boolean)
                Me._onerror = value
            End Set
        End Property
    End Class
End Class
