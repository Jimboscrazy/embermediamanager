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

Imports System.IO
Imports ICSharpCode.SharpZipLib.Zip
Imports System.Xml.Serialization

Public Class frmSettingsHolder
    Private confs As New List(Of NMTExporterModule.Config)
    Private conf As NMTExporterModule.Config
    Private sBasePath As String = Path.Combine(Path.Combine(Functions.AppPath, "Modules"), Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly.Location))
#Region "Events"

    Public Event ModuleEnabledChanged(ByVal State As Boolean)

    Public Event ModuleSettingsChanged()

#End Region 'Events

#Region "Methods"

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(cbEnabled.Checked)
    End Sub

    Public Sub New()
        InitializeComponent()
        Me.SetUp()
        LoadTemplates(True)

    End Sub

    Sub LoadTemplates(Optional ByVal withNew As Boolean = False)
        Dim fxml As String
        lstTemplates.Items.Clear()
        lstTemplates.ShowItemToolTips = True
        Dim di As DirectoryInfo = New DirectoryInfo(Path.Combine(sBasePath, "Templates"))
        For Each i As DirectoryInfo In di.GetDirectories
            If Not (i.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                fxml = Path.Combine(sBasePath, String.Concat("Templates", Path.DirectorySeparatorChar, i.Name))
                conf = NMTExporterModule.Config.Load(Path.Combine(fxml, "config.xml"))
                If Not String.IsNullOrEmpty(conf.Name) Then
                    conf.TemplatePath = fxml
                    conf.ReadMe = File.Exists(Path.Combine(conf.TemplatePath, "readme.txt"))
                    confs.Add(conf)
                    Dim li As New ListViewItem(conf.Name)
                    li.SubItems.AddRange(New String() {conf.Version, conf.Author, "Installed"})
                    li.ToolTipText = conf.Description
                    li.Tag = conf
                    lstTemplates.Items.Add(li)
                End If
            End If
        Next
        If withNew Then
            For Each i As FileInfo In di.GetFiles
                If i.Extension = ".zip" Then
                    conf = ScanZip(i.FullName)
                    If Not conf Is Nothing Then
                        fxml = Path.Combine(sBasePath, String.Concat("Templates", Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(i.Name)))
                        conf.TemplatePath = fxml
                        confs.Add(conf)
                        Dim li As New ListViewItem(conf.Name)
                        li.SubItems.AddRange(New String() {conf.Version, conf.Author, "New"})
                        li.ToolTipText = conf.Description
                        li.Tag = conf
                        lstTemplates.Items.Add(li)
                    End If
                End If
            Next
        End If
    End Sub

    Private Function ScanZip(ByVal fname As String) As NMTExporterModule.Config
        Dim conf As NMTExporterModule.Config = Nothing
        Dim haveReadMe As Boolean = False
        Using fStream As FileStream = New FileStream(fname, FileMode.Open, FileAccess.Read)
            Dim fZip As Byte() = Functions.ReadStreamToEnd(fStream)
            Try
                Using zStream As ZipInputStream = New ZipInputStream(New MemoryStream(fZip))
                    Dim zEntry As ZipEntry = zStream.GetNextEntry
                    While Not IsNothing(zEntry)
                        Select zEntry.Name
                            Case "config.xml"
                                Dim xmlSer As XmlSerializer
                                xmlSer = New XmlSerializer(GetType(NMTExporterModule.Config))
                                Try
                                    conf = DirectCast(xmlSer.Deserialize(zStream), NMTExporterModule.Config)
                                Catch ex As Exception
                                End Try
                            Case "readme.txt"
                                haveReadMe = True
                        End Select
                        zEntry = zStream.GetNextEntry
                    End While
                End Using
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Using
        If Not conf Is Nothing Then conf.ReadMe = haveReadMe
        Return conf
    End Function


    Private Sub SetUp()
        Me.cbEnabled.Text = Master.eLang.GetString(774, "Enabled", True)
    End Sub

#End Region 'Methods
    Private Sub lstTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstTemplates.SelectedIndexChanged
        If lstTemplates.SelectedItems.Count > 0 Then
            Select Case lstTemplates.SelectedItems(0).SubItems(3).Text
                Case "Installed"
                    btnRemove.Enabled = True
                    btnInstall.Enabled = False
                Case "New"
                    btnRemove.Enabled = False
                    btnInstall.Enabled = True
                Case Else
                    btnRemove.Enabled = False
                    btnInstall.Enabled = False
            End Select
            Dim conf As NMTExporterModule.Config = DirectCast(lstTemplates.SelectedItems(0).Tag, NMTExporterModule.Config)
            Dim readme As String = String.Empty
            If conf.ReadMe Then
                If lstTemplates.SelectedItems(0).SubItems(3).Text = "Installed" AndAlso _
                    File.Exists(Path.Combine(conf.TemplatePath, "readme.txt")) Then
                    readme = File.ReadAllText(Path.Combine(conf.TemplatePath, "readme.txt"))
                End If
            End If
            lblDetails.Text = readme
        Else
            btnRemove.Enabled = False
            btnInstall.Enabled = False
            lblDetails.Text = String.Empty
        End If
    End Sub
End Class