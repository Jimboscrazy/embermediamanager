Imports EmberAPI
Imports System
Imports System.IO
Imports System.Xml.Serialization
Public Class XBMCxCom
    Implements Interfaces.EmberExternalModule

    Private _name As String = "XBMC Controller"
    Private _enabled As Boolean = False
    Private _setup As frmSettingsHolder
    Private WithEvents MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MyTrayMenu As New System.Windows.Forms.ToolStripMenuItem
    Public Event GenericEvent(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) Implements EmberAPI.Interfaces.EmberExternalModule.GenericEvent
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberExternalModule.ModuleSettingsChanged
    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements Interfaces.EmberExternalModule.ModuleSetupChanged

    Private _MySettings As New MySettings


    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
            If _enabled Then
                Enable()
            Else
                Disable()
            End If
        End Set
    End Property
    Sub Enable()
        Try
            Dim tSettingsHolder As New frmSettingsHolder
            Dim tsi As New ToolStripSplitButton
            MyMenu.Image = New Bitmap(tSettingsHolder.Icon.ToBitmap)
            MyMenu.Text = Master.eLang.GetString(13, "XBMC")
            tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.MainTool.Items("tsbMediaCenters"), ToolStripSplitButton)
            tsi.DropDownItems.Add(MyMenu)
            'MyTrayMenu.Image = New Bitmap(tSettingsHolder.Icon.ToBitmap)
            'MyTrayMenu.Text = Master.eLang.GetString(13, "XBMC Controller")
            'tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TrayMenu.Items("cmnuTrayIconTools"), ToolStripMenuItem)
            'tsi.DropDownItems.Add(MyTrayMenu)
            tSettingsHolder.Dispose()
            MyMenu.DropDownItems.Clear()
            Dim tMenu As New System.Windows.Forms.ToolStripMenuItem With {.Text = "Update All", .Tag = Nothing}
            AddHandler tMenu.Click, AddressOf xCom_Click
            MyMenu.DropDownItems.Add(tMenu)
            For Each xCom As XBMCCom In _MySettings.XComs
                tMenu = New System.Windows.Forms.ToolStripMenuItem With {.Text = xCom.Name, .Tag = xCom, .DropDownDirection = ToolStripDropDownDirection.Left}
                AddHandler tMenu.Click, AddressOf xCom_Click
                MyMenu.DropDownItems.Add(tMenu)
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub xCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim tMenu As New System.Windows.Forms.ToolStripMenuItem
        tMenu = DirectCast(sender, System.Windows.Forms.ToolStripMenuItem)
        If tMenu.Tag Is Nothing Then
            Try
                For Each tCom As XBMCCom In _MySettings.XComs
                    Me.DoXCom(tCom)
                Next
            Catch
            End Try
        Else
            Dim xCom As XBMCCom = DirectCast(tMenu.Tag, XBMCCom)
            DoXCom(xCom)
        End If
    End Sub
    Sub Disable()
        Try
            Dim tsi As New ToolStripSplitButton
            tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.MainTool.Items("tsbMediaCenters"), ToolStripSplitButton)
            tsi.DropDownItems.Remove(MyMenu)
            MyMenu.DropDownItems.Clear()
            'tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TrayMenu.Items("cmnuTrayIconTools"), ToolStripMenuItem)
            'tsi.DropDownItems.Remove(MyTrayMenu)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        '_MySettings.XComs.AddRange(Master.eSettings.XBMCComs)
        _MySettings = MySettings.Load
    End Sub

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me._setup = New frmSettingsHolder
        Me._setup.cbEnabled.Checked = Me._enabled
        _setup.XComs = _MySettings.XComs
        _setup.LoadXComs()
        SPanel.Name = Me._name
        SPanel.Text = Me._name
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings()
        AddHandler _setup.ModuleEnabledChanged, AddressOf Handle_SetupChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function
    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent ModuleEnabledChanged(Me._Name, state, difforder)
    End Sub

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property ModuleType() As System.Collections.Generic.List(Of EmberAPI.Enums.ModuleEventType) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType)
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function RunGeneric(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) As EmberAPI.Interfaces.ModuleResult Implements EmberAPI.Interfaces.EmberExternalModule.RunGeneric

    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        'Master.eSettings.XBMCComs.AddRange(_MySettings.XComs)
        Me.Enabled = _setup.cbEnabled.Checked
        _MySettings.XComs = _setup.XComs
        MySettings.Save(_MySettings)
    End Sub
    Private Sub DoXCom(ByVal xCom As XBMCCom)
        Try
            Dim Wr As HttpWebRequest = DirectCast(HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", xCom.IP, xCom.Port)), HttpWebRequest)
            Wr.Timeout = 2500

            If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
                Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
            End If

            Using Wres As HttpWebResponse = DirectCast(Wr.GetResponse, HttpWebResponse)
                Dim Sr As String = New StreamReader(Wres.GetResponseStream()).ReadToEnd
                If Not Sr.Contains("OK") Then
                    ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(147, "Unable to Start XBMC Update"), String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}."), xCom.Name, vbNewLine), Nothing}))
                    'MsgBox(String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}. Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), xCom.Name, vbNewLine), MsgBoxStyle.Exclamation, String.Format(Master.eLang.GetString(147, "Unable to Start XBMC Update for {0}"), xCom.Name))
                End If
            End Using
            Wr = Nothing
        Catch
            ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(147, "Unable to Start XBMC Update"), String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}."), xCom.Name, vbNewLine), Nothing}))
            'MsgBox(String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}. Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), xCom.Name, vbNewLine), MsgBoxStyle.Exclamation, String.Format(Master.eLang.GetString(147, "Unable to Start XBMC Update for {0}"), xCom.Name))
        End Try
    End Sub
    Class MySettings
        Public XComs As New List(Of XBMCxCom.XBMCCom)
        Public Shared Function Load() As MySettings
            Dim tmp As New MySettings
            Try
                Dim xmlSerial As New XmlSerializer(GetType(MySettings))
                If File.Exists(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml")) Then
                    Dim strmReader As New StreamReader(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml"))
                    tmp = DirectCast(xmlSerial.Deserialize(strmReader), MySettings)
                    strmReader.Close()
                Else
                    tmp = New MySettings
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                tmp = New MySettings
            End Try
            Return tmp
        End Function
        Public Shared Sub Save(ByVal tmp As MySettings)
            Try
                Dim xmlSerial As New XmlSerializer(GetType(MySettings))
                Dim xmlWriter As New StreamWriter(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml"))

                xmlSerial.Serialize(xmlWriter, tmp)
                xmlWriter.Close()
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub
    End Class

    Public Class XBMCCom
        Private _xbmcname As String
        Private _xbmcport As String
        Private _xbmcip As String
        Private _xbmcusername As String
        Private _xbmcpassword As String

        Public Property Name() As String
            Get
                Return Me._xbmcname
            End Get
            Set(ByVal value As String)
                Me._xbmcname = value
            End Set
        End Property

        Public Property IP() As String
            Get
                Return Me._xbmcip
            End Get
            Set(ByVal value As String)
                Me._xbmcip = value
            End Set
        End Property

        Public Property Port() As String
            Get
                Return Me._xbmcport
            End Get
            Set(ByVal value As String)
                Me._xbmcport = value
            End Set
        End Property

        Public Property Username() As String
            Get
                Return Me._xbmcusername
            End Get
            Set(ByVal value As String)
                Me._xbmcusername = value
            End Set
        End Property

        Public Property Password() As String
            Get
                If String.IsNullOrEmpty(Me._xbmcpassword) Then
                    Return String.Empty
                Else
                    Return StringUtils.Decode(Me._xbmcpassword)
                End If
            End Get
            Set(ByVal value As String)
                If String.IsNullOrEmpty(value) Then
                    Me._xbmcpassword = value
                Else
                    Me._xbmcpassword = StringUtils.Encode(value)
                End If
            End Set
        End Property

        Public Sub New()
            Clear()
        End Sub

        Public Sub Clear()
            Me._xbmcname = String.Empty
            Me._xbmcip = String.Empty
            Me._xbmcport = String.Empty
            Me._xbmcusername = String.Empty
            Me._xbmcpassword = String.Empty
        End Sub
    End Class

End Class
