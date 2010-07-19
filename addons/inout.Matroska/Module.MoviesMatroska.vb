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

Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization

Public Class InputMatroska_Module
    Implements Interfaces.EmberMovieInputModule

    Public Shared _AssemblyName As String
    Private fInputSettings As frmMovieInputSettings
    Private _Enabled As Boolean
    Public Shared eSettings As New MySettings


    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.Enabled
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property

    Public Function GetFilesFolderContents(ByRef Movie As Scanner.MovieContainer) As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.GetFilesFolderContents
        Return False
    End Function

    Public Function LoadMovieInfoSheet(ByVal sPath As String, ByVal isSingle As Boolean, ByRef mMovie As MediaContainers.Movie) As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.LoadMovieInfoSheet
        Return False
    End Function

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieInputModule.ModuleName
        Get
            Return "Matroska Input Module"
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieInputModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberMovieInputModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me.fInputSettings = New frmMovieInputSettings
        Me.fInputSettings.chkEnabled.Checked = Me.Enabled
        SPanel.Name = Me.ModuleName
        SPanel.Text = Master.eLang.GetString(91, "Matroska")
        SPanel.Prefix = "MatroskaInputModule_"
        SPanel.Parent = "pnlMovieInput"
        SPanel.Type = Master.eLang.GetString(36, "Movies", True)
        SPanel.ImageIndex = If(Me.Enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fInputSettings.pnlSettings
        'AddHandler Me.fInputSettings.SettingsChanged, AddressOf Handle_ModuleSettingsChanged
        'AddHandler me.fInputSettings.EnabledChanged, AddressOf Handle_SetupChanged
        Return SPanel
        'Return Nothing
    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieInputModule.SaveSetup

    End Sub

    Public Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupChanged
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberMovieInputModule.ModuleSettingsChanged
    Public Sub SetupOrderChanged() Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupOrderChanged

    End Sub

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberMovieInputModule.Init
        _AssemblyName = sAssemblyName
    End Sub
    Sub SaveMySettings()

    End Sub
    Sub LoadMySettings()

    End Sub

End Class

Public Class MySettings
    Sub New()
    End Sub

End Class