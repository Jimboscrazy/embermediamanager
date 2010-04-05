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
Imports System.Xml.Serialization

Imports EmberAPI

Public Class XBMCxCom
    Implements Interfaces.EmberExternalModule

    #Region "Fields"

    Private  WithEvents MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private  WithEvents MyTrayMenu As New System.Windows.Forms.ToolStripMenuItem
    Private _enabled As Boolean = False
    Private _MySettings As New MySettings
    Private _name As String = "XBMC Controller"
    Private _setup As frmSettingsHolder

    #End Region 'Fields

    #Region "Events"

    Public Event GenericEvent(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) Implements EmberAPI.Interfaces.EmberExternalModule.GenericEvent

    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements Interfaces.EmberExternalModule.ModuleSetupChanged

    Public Event ModuleSettingsChanged() Implements Interfaces.EmberExternalModule.ModuleSettingsChanged

    #End Region 'Events

    #Region "Properties"

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            If _enabled = value Then Return
            _enabled = value
            If _enabled Then
                Enable()
            Else
                Disable()
            End If
        End Set
    End Property

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

    #End Region 'Properties

    #Region "Methods"

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
        SPanel.Text = Master.eLang.GetString(0, "XBMC Controller")
        SPanel.Prefix = "XBMCCom_"
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me._setup.pnlSettings()
        AddHandler _setup.ModuleEnabledChanged, AddressOf Handle_SetupChanged
        AddHandler _setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Public Function RunGeneric(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object), ByRef _refparam As Object) As EmberAPI.Interfaces.ModuleResult Implements EmberAPI.Interfaces.EmberExternalModule.RunGeneric
    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        'Master.eSettings.XBMCComs.AddRange(_MySettings.XComs)
        Me.Enabled = _setup.cbEnabled.Checked
        _MySettings.XComs = _setup.XComs
        MySettings.Save(_MySettings)

        If Me._enabled Then
            Me.Disable()
            Me.Enable()
        End If
    End Sub

    Sub Disable()
        Try
            Dim tsb As New ToolStripSplitButton
            tsb = DirectCast(ModulesManager.Instance.RuntimeObjects.MainTool.Items("tsbMediaCenters"), ToolStripSplitButton)
            tsb.DropDownItems.Remove(MyMenu)
            MyMenu.DropDownItems.Clear()
            tsb.Visible = (tsb.DropDownItems.Count > 0)
            Dim tsi As New ToolStripMenuItem
            tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TrayMenu.Items("cmnuTrayIconMediaCenters"), ToolStripMenuItem)
            tsi.DropDownItems.Remove(MyTrayMenu)
            tsi.Visible = (tsi.DropDownItems.Count > 0)
        Catch ex As Exception
        End Try
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
                    ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(16, "Unable to Start XBMC Update"), String.Format(Master.eLang.GetString(17, "There was a problem communicating with {0}{1}."), xCom.Name, vbNewLine), Nothing}))
                End If
            End Using
            Wr = Nothing
        Catch
            ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"info", 5, Master.eLang.GetString(16, "Unable to Start XBMC Update"), String.Format(Master.eLang.GetString(17, "There was a problem communicating with {0}{1}."), xCom.Name, vbNewLine), Nothing}))
        End Try
    End Sub

    Sub Enable()
        Try
            _MySettings = MySettings.Load
            Dim tSettingsHolder As New frmSettingsHolder
            Dim tsb As New ToolStripSplitButton
            MyMenu.Image = New Bitmap(tSettingsHolder.Icon.ToBitmap)
            MyMenu.Text = Master.eLang.GetString(18, "XBMC")
            tsb = DirectCast(ModulesManager.Instance.RuntimeObjects.MainTool.Items("tsbMediaCenters"), ToolStripSplitButton)
            Dim tsi As New ToolStripMenuItem
            MyTrayMenu.Image = New Bitmap(tSettingsHolder.Icon.ToBitmap)
            MyTrayMenu.Text = Master.eLang.GetString(19, "XBMC Controller")
            tsi = DirectCast(ModulesManager.Instance.RuntimeObjects.TrayMenu.Items("cmnuTrayIconMediaCenters"), ToolStripMenuItem)
            tSettingsHolder.Dispose()
            MyMenu.DropDownItems.Clear()
            MyTrayMenu.DropDownItems.Clear()
            If _MySettings.XComs.Count > 0 Then
                Dim tMenu As New System.Windows.Forms.ToolStripMenuItem With {.Text = Master.eLang.GetString(649, "Update All", True), .Tag = Nothing}
                AddHandler tMenu.Click, AddressOf xCom_Click
                MyMenu.DropDownItems.Add(tMenu)
                Dim tTrayMenu As New System.Windows.Forms.ToolStripMenuItem With {.Text = Master.eLang.GetString(649, "Update All", True), .Tag = Nothing}
                AddHandler tTrayMenu.Click, AddressOf xCom_Click
                MyTrayMenu.DropDownItems.Add(tTrayMenu)

                For Each xCom As XBMCCom In _MySettings.XComs
                    tMenu = New System.Windows.Forms.ToolStripMenuItem With {.Text = String.Format(Master.eLang.GetString(143, "Update {0} Only", True), xCom.Name), .Tag = xCom, .DropDownDirection = ToolStripDropDownDirection.Left}
                    AddHandler tMenu.Click, AddressOf xCom_Click
                    MyMenu.DropDownItems.Add(tMenu)
                    tTrayMenu = New System.Windows.Forms.ToolStripMenuItem With {.Text = String.Format(Master.eLang.GetString(143, "Update {0} Only", True), xCom.Name), .Tag = xCom, .DropDownDirection = ToolStripDropDownDirection.Left}
                    AddHandler tTrayMenu.Click, AddressOf xCom_Click
                    MyTrayMenu.DropDownItems.Add(tTrayMenu)
                Next
                tsb.DropDownItems.Add(MyMenu)
                tsi.DropDownItems.Add(MyTrayMenu)
            End If
            tsb.Visible = (tsb.DropDownItems.Count > 0)
            tsi.Visible = (tsi.DropDownItems.Count > 0)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent ModuleEnabledChanged(Me._Name, state, difforder)
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

    Public Shared Function SendCmd(ByVal xCom As XBMCCom, ByVal str As String) As String
        Dim Wr As HttpWebRequest = DirectCast(HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?{2}", xCom.IP, xCom.Port, str)), HttpWebRequest)
        Wr.Timeout = 2500
        Dim Sr As String
        If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
            Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
        End If
        Using Wres As HttpWebResponse = DirectCast(Wr.GetResponse, HttpWebResponse)
            Sr = New StreamReader(Wres.GetResponseStream()).ReadToEnd
        End Using
        Wr = Nothing
        If Sr.StartsWith("<html>") Then Sr = Sr.Remove(0, 6)
        If Sr.EndsWith("</html>") Then Sr = Sr.Remove(Sr.Length - 7, 7)
        Return Sr
    End Function

    #End Region 'Methods

    #Region "Nested Types"

    Public Class XBMCCom

        #Region "Fields"

        Private _xbmcip As String
        Private _xbmcname As String
        Private _xbmcpassword As String
        Private _xbmcport As String
        Private _xbmcusername As String

        #End Region 'Fields

        #Region "Constructors"

        Public Sub New()
            Clear()
        End Sub

        #End Region 'Constructors

        #Region "Properties"

        Public Property IP() As String
            Get
                Return Me._xbmcip
            End Get
            Set(ByVal value As String)
                Me._xbmcip = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._xbmcname
            End Get
            Set(ByVal value As String)
                Me._xbmcname = value
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

        #End Region 'Properties

        #Region "Methods"

        Public Sub Clear()
            Me._xbmcname = String.Empty
            Me._xbmcip = String.Empty
            Me._xbmcport = String.Empty
            Me._xbmcusername = String.Empty
            Me._xbmcpassword = String.Empty
        End Sub

        #End Region 'Methods

    End Class

    Class MySettings

        #Region "Fields"

        Public XComs As New List(Of XBMCxCom.XBMCCom)

        #End Region 'Fields

        #Region "Methods"

        Public Shared Function Load() As MySettings
            Dim tmp As New MySettings
            Try
                Dim Asett As New List(Of Hashtable)
                Asett = AdvancedSettings.GetComplexSetting("XBMCHosts")
                If Not Asett Is Nothing Then
                    Dim t As XBMCxCom.XBMCCom
                    For Each i In Asett
                        t = New XBMCxCom.XBMCCom
                        For Each k In i.Keys
                            Select Case k.ToString
                                Case "Name"
                                    t.Name = i.Item("Name").ToString
                                Case "IP"
                                    t.IP = i.Item("IP").ToString
                                Case "Port"
                                    t.Port = i.Item("Port").ToString
                                Case "Username"
                                    t.Username = i.Item("Username").ToString
                                Case "Password"
                                    t.Password = i.Item("Password").ToString
                            End Select
                        Next
                        tmp.XComs.Add(t)
                    Next
                End If
                'Dim tmp As New MySettings
                'Try
                'Dim xmlSerial As New XmlSerializer(GetType(MySettings))
                'If File.Exists(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml")) Then
                ' Dim strmReader As New StreamReader(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml"))
                'tmp = DirectCast(xmlSerial.Deserialize(strmReader), MySettings)
                'strmReader.Close()
                'Else
                'tmp = New MySettings
                'End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                tmp = New MySettings
            End Try
            Return tmp
        End Function

        Public Shared Sub Save(ByVal tmp As MySettings)
            Try

                Dim Asett As New List(Of Hashtable)
                For Each t As XBMCxCom.XBMCCom In tmp.XComs
                    Dim h As New Hashtable
                    h.Add("Name", t.Name)
                    h.Add("IP", t.IP)
                    h.Add("Port", t.Port)
                    h.Add("Username", t.Username)
                    h.Add("Password", t.Password)
                    Asett.Add(h)
                Next
                AdvancedSettings.ClearComplexSetting("XBMCHosts")
                AdvancedSettings.SetComplexSetting("XBMCHosts", Asett)

                'Dim xmlSerial As New XmlSerializer(GetType(MySettings))
                'Dim xmlWriter As New StreamWriter(Path.Combine(Path.Combine(Functions.AppPath, "Modules"), "XBMCxCom.xml"))
                'xmlSerial.Serialize(xmlWriter, tmp)
                'xmlWriter.Close()
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        #End Region 'Methods

    End Class

    #End Region 'Nested Types

End Class