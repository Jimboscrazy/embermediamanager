Public Class genericMediaBrowser
    Implements Interfaces.EmberExternalModule


    Private fMediaBrowser As frmMediaBrowser
    Private _enabled As Boolean = False
    Private _name As String = "MediaBrowser compatibility"

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            If _enabled = value Then Return
            _enabled = value
            If _enabled Then
                'Enable()
            Else
                'Disable()
            End If
        End Set
    End Property

    Public Event GenericEvent(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) Implements EmberAPI.Interfaces.EmberExternalModule.GenericEvent

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberExternalModule.Init

    End Sub

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me.fMediaBrowser = New frmMediaBrowser
        Me.fMediaBrowser.chkEnabled.Checked = Me._enabled

        'chkYAMJnfoFields
        SPanel.Name = _name
        SPanel.Text = Master.eLang.GetString(91, "MediaBrowser Compatibility")
        SPanel.Prefix = "MediaBrowser_"
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fMediaBrowser.pnlSettings
        AddHandler Me.fMediaBrowser.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        AddHandler fMediaBrowser.ModuleEnabledChanged, AddressOf Handle_SetupChanged

        'Return SPanel
        Return Nothing
    End Function
    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent ModuleSetupChanged(Me._name, state, difforder)
    End Sub
    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return "MediaBrowser Compatibility"
        End Get
    End Property

    Public Event ModuleSettingsChanged() Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSettingsChanged

    Public Event ModuleSetupChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSetupChanged

    Public ReadOnly Property ModuleType() As System.Collections.Generic.List(Of EmberAPI.Enums.ModuleEventType) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Generic, Enums.ModuleEventType.TVImageNaming, Enums.ModuleEventType.OnMovieNFOSave})
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        'Me.Enabled = Me.fMediaBrowser.chkEnabled.Checked
    End Sub

    Public Function RunGeneric(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object), ByRef _refparam As Object) As EmberAPI.Interfaces.ModuleResult Implements EmberAPI.Interfaces.EmberExternalModule.RunGeneric
        Dim doContinue As Boolean
        Dim mMovie As Structures.DBMovie
        If Enabled Then
            Try
                Select Case mType
                    Case Enums.ModuleEventType.OnMovieNFOSave
                        mMovie = DirectCast(_params(0), Structures.DBMovie)
                        doContinue = DirectCast(_refparam, Boolean)
                        'If AdvancedSettings.GetBooleanSetting("YAMJnfoFields", False) Then
                        'If Not String.IsNullOrEmpty(mMovie.FileSource) Then
                        'mMovie.Movie.VideoSource = mMovie.FileSource
                        'End If
                        'Else
                        'mMovie.Movie.VideoSource = String.Empty
                        'End If
                End Select
                _refparam = doContinue
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Function

End Class
