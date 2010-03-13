﻿Imports System.Threading

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
    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements Interfaces.EmberExternalModule.ModuleSetupChanged
    Public Event GenericEvent(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object)) Implements Interfaces.EmberExternalModule.GenericEvent
    Private dNotify As frmNotify

    Public ReadOnly Property ModuleType() As List(Of Enums.ModuleEventType) Implements Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Notification}) 'Enums.ModuleType.Notification
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

    Public Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberExternalModule.Init
        LoadSettings()
    End Sub

    Sub SaveSetup(ByVal DoDispose As Boolean) Implements Interfaces.EmberExternalModule.SaveSetup
        Me._enabled = _setup.chkEnabled.Checked
        eSettings.OnError = _setup.chkOnError.Checked
        eSettings.OnNewMovie = _setup.chkonnewmovie.checked
        eSettings.OnMovieScraped = _setup.chkOnMovieScraped.Checked
        eSettings.OnNewEp = _setup.chkOnNewEp.Checked
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
        Me._setup.chkOnNewMovie.Checked = eSettings.OnNewMovie
        Me._setup.chkOnMovieScraped.Checked = eSettings.OnMovieScraped
        Me._setup.chkOnNewEp.Checked = eSettings.OnNewEp
        SPanel.Name = Me._name
        SPanel.Text = Me._name
        SPanel.Prefix = "Notify_"
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings
        AddHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        AddHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Private Sub Handle_ModuleEnabledChanged(ByVal State As Boolean)
        RaiseEvent ModuleEnabledChanged(Me._name, State, 0)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub Handle_NotifierClicked(ByVal _type As String)
        RaiseEvent GenericEvent(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {_type}))
    End Sub

    Private Sub Handle_NotifierClosed()
        RemoveHandler Me.dNotify.NotifierClicked, AddressOf Me.Handle_NotifierClicked
        RemoveHandler Me.dNotify.NotifierClosed, AddressOf Me.Handle_NotifierClosed
    End Sub

    Public Function RunGeneric(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object)) As Interfaces.ModuleResult Implements Interfaces.EmberExternalModule.RunGeneric
        Try
            If mType = Enums.ModuleEventType.Notification Then
                Dim ShowIt As Boolean = False

                Select Case True
                    Case _params(0).ToString = "error" AndAlso eSettings.OnError
                        ShowIt = True
                    Case _params(0).ToString = "newmovie" AndAlso eSettings.OnNewMovie
                        ShowIt = True
                    Case _params(0).ToString = "moviescraped" AndAlso eSettings.OnMovieScraped
                        ShowIt = True
                    Case _params(0).ToString = "newep" AndAlso eSettings.OnNewEp
                        ShowIt = True
                    Case _params(0).ToString = "info"
                        ShowIt = True
                End Select

                If ShowIt Then
                    dNotify = New frmNotify
                    AddHandler dNotify.NotifierClicked, AddressOf Me.Handle_NotifierClicked
                    AddHandler dNotify.NotifierClosed, AddressOf Me.Handle_NotifierClosed
                    dNotify.Show(_params(0).ToString, Convert.ToInt32(_params(1)), _params(2).ToString, _params(3).ToString, If(Not IsNothing(_params(4)), DirectCast(_params(4), Image), Nothing))
                End If
            End If
        Catch ex As Exception
        End Try
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function

    Private Sub SaveSettings()
        AdvancedSettings.SetBooleanSetting("NotifyOnError", eSettings.OnError)
        AdvancedSettings.SetBooleanSetting("NotifyOnNewMovie", eSettings.OnNewMovie)
        AdvancedSettings.SetBooleanSetting("NotifyOnMovieScraped", eSettings.OnMovieScraped)
        AdvancedSettings.SetBooleanSetting("NotifyOnNewEp", eSettings.OnNewEp)
    End Sub

    Private Sub LoadSettings()
        eSettings.OnError = AdvancedSettings.GetBooleanSetting("NotifyOnError", True)
        eSettings.OnNewMovie = AdvancedSettings.GetBooleanSetting("NotifyOnNewMovie", False)
        eSettings.OnMovieScraped = AdvancedSettings.GetBooleanSetting("NotifyOnMovieScraped", True)
        eSettings.OnNewEp = AdvancedSettings.GetBooleanSetting("NotifyOnNewEp", False)
    End Sub

    Class NotifySettings
        Private _onerror As Boolean
        Private _onnewmovie As Boolean
        Private _onmoviescraped As Boolean
        Private _onnewep As Boolean

        Public Property OnError() As Boolean
            Get
                Return Me._onerror
            End Get
            Set(ByVal value As Boolean)
                Me._onerror = value
            End Set
        End Property

        Public Property OnNewMovie() As Boolean
            Get
                Return Me._onnewmovie
            End Get
            Set(ByVal value As Boolean)
                Me._onnewmovie = value
            End Set
        End Property

        Public Property OnMovieScraped() As Boolean
            Get
                Return Me._onmoviescraped
            End Get
            Set(ByVal value As Boolean)
                Me._onmoviescraped = value
            End Set
        End Property

        Public Property OnNewEp() As Boolean
            Get
                Return Me._onnewep
            End Get
            Set(ByVal value As Boolean)
                Me._onnewep = value
            End Set
        End Property
    End Class
End Class
