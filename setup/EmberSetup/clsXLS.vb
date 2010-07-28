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
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web

Public Class FileOfList

    #Region "Fields"

    Public Filename As String
    Public Hash As String
    Public inCache As Boolean = False
    Public NeedBackup As Boolean = False
    Public NeedInstall As Boolean = True
    Public Path As String
    Public Platform As String

    #End Region 'Fields

End Class

<XmlRoot("UpgradeFile")> _
Public Class FilesList

    #Region "Fields"

    <XmlArray("Files")> _
    <XmlArrayItem("File")> _
    Public Files As List(Of FileOfList)

    #End Region 'Fields

    #Region "Methods"

    Public Sub ConvertToPlatform()
        If Not Files Is Nothing Then
            For Each f As FileOfList In Files
                f.Path = f.Path.Replace("\", Path.DirectorySeparatorChar)
            Next
        End If
    End Sub

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(FilesList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class FileToInstall

    #Region "Fields"

    Public EmberPath As String
    Public Filename As String
    Public Hash As String
    Public OriginalPath As String
    Public Platform As String

    #End Region 'Fields

End Class

Public Class InstallCommand

    #Region "Fields"

    <XmlElement("Description")> _
    Public CommandDescription As String
    <XmlElement("Execute")> _
    Public CommandExecute As String
    <XmlAttribute("Type")> _
    Public CommandType As String

    #End Region 'Fields

End Class

<XmlRoot("CommandFile")> _
Public Class InstallCommands

    #Region "Fields"

    <XmlArray("Commands")> _
    <XmlArrayItem("Command")> _
    Public Command As New List(Of InstallCommand)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(InstallCommands))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

<XmlRoot("VersionsFile")> _
Public Class UpgradeList

    #Region "Fields"

    <XmlArray("Versions")> _
    <XmlArrayItem("Version")> _
    Public VersionList As New List(Of Versions)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(UpgradeList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class Versions
    Implements IComparable(Of Versions)

    #Region "Fields"

    <XmlAttribute("Number")> _
    Public Version As String

    #End Region 'Fields

    #Region "Methods"

    Public Function CompareTo(ByVal other As Versions) As Integer Implements IComparable(Of Versions).CompareTo
        Return Convert.ToInt32(Me.Version).CompareTo(Convert.ToInt32(other.Version))
    End Function

    #End Region 'Methods

End Class

Public Class Langs
    Private Shared htStrings As New Hashtable

    Public lLanguages As New List(Of LanguageList)
    Public Function GetString(ByVal ID As Integer, ByVal strDefault As String) As String
        Try
            If IsNothing(htStrings) Then
                Return strDefault
            End If
            If htStrings.ContainsKey(ID) Then
                Return htStrings.Item(ID).ToString
            Else
                Return strDefault
            End If
        Catch ex As Exception
        End Try
        Return strDefault
    End Function
    Public Function LangExist(ByVal Language As String) As String
        Dim lPath As String = String.Empty
        Try
            lPath = Path.Combine(frmMainSetup.AppPath, String.Format("Setup.{0}.xml", Language))
            If Not File.Exists(lPath) Then
                frmMainSetup.GetURLFile("Setup/" & String.Format("Setup.{0}.xml", System.Web.HttpUtility.HtmlEncode(Language)), lPath)
            End If
            If Not File.Exists(lPath) Then
                lPath = String.Empty
            End If
        Catch ex As Exception
        End Try

        Return lPath
    End Function

    Public Sub LoadLanguage(ByVal Language As String)
        Dim lPath As String = String.Empty
        Dim lhPath As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(Language) Then

                htStrings = New Hashtable
                htStrings.Clear()

                lPath = LangExist(Language)
                If Not String.IsNullOrEmpty(lPath) AndAlso File.Exists(lPath) Then
                    Dim LangXML As XDocument = XDocument.Load(lPath)
                    Dim xLanguage = From xLang In LangXML...<strings>...<string> Select xLang.@id, xLang.Value
                    If xLanguage.Count > 0 Then
                        For i As Integer = 0 To xLanguage.Count - 1
                            htStrings.Add(Convert.ToInt32(xLanguage(i).id), xLanguage(i).Value)
                        Next
                    Else
                    End If
                Else
                    'MsgBox(String.Concat(String.Format("Cannot find {0}.xml.", Language), vbNewLine, vbNewLine, "Expected path:", vbNewLine, lPath), MsgBoxStyle.Critical, "File Not Found")
                End If
            End If

        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
    Public Sub Save()
        Try
            Dim xmlSer As New XmlSerializer(GetType(List(Of LanguageList)))
            Using xmlSW As New StreamWriter(Path.Combine(frmMainSetup.AppPath, "Setup.Languages.xml"))
                xmlSer.Serialize(xmlSW, lLanguages)
            End Using

        Catch ex As Exception
        End Try
    End Sub
    Public Sub Load()
        Try
            If File.Exists(Path.Combine(frmMainSetup.AppPath, "Setup.Languages.xml")) Then
                Dim xmlSer As New XmlSerializer(GetType(List(Of LanguageList)))
                Using xmlSW As New StreamReader(Path.Combine(frmMainSetup.AppPath, "Setup.Languages.xml"))
                    lLanguages = xmlSer.Deserialize(xmlSW)
                End Using
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub GetFromSite()
        frmMainSetup.GetURLFile("Setup/Setup.Languages.xml", Path.Combine(frmMainSetup.AppPath, "Setup.Languages.xml"))
    End Sub
    Public Class LanguageList
        Public Name As String
        Public Filename As String
    End Class
End Class

Class AdvancedSettings
    Private Shared _AdvancedSettings As New List(Of SettingItem)
    Private Shared _ComplexAdvancedSettings As New List(Of ComplexSettingItem)
    ' ******************************************************************************
    Public Class SettingItem
        Public DefaultValue As String
        Public Name As String
        Public Section As String
        Public Value As String
    End Class
    Public Class ComplexSettingItem
        Public Name As String
        Public Section As String
        Public TableItem As New Hashtable
    End Class
    Public Shared Sub Load(ByVal fname As String)
        Try
            If File.Exists(fname) Then
                Dim xdoc As New XDocument
                xdoc = XDocument.Load(fname)
                For Each i As XElement In xdoc...<Setting>
                    Dim ii As XElement = i
                    Dim v = _AdvancedSettings.FirstOrDefault(Function(f) f.Name = ii.@Name AndAlso f.Section = ii.@Section)
                    If v Is Nothing Then
                        _AdvancedSettings.Add(New SettingItem With {.Section = ii.@Section, .Name = ii.@Name, .Value = ii.Value, .DefaultValue = ""})
                    Else
                        _AdvancedSettings.FirstOrDefault(Function(f) f.Name = ii.@Name AndAlso f.Section = ii.@Section).Value = Convert.ToString(i.Value)
                    End If
                Next
                For Each i As XElement In xdoc...<ComplexSettings>...<Table>
                    Dim l As XElement = i
                    Dim dict As New Hashtable
                    For Each t As XElement In l...<Item>
                        dict.Add(t.@Name, t.Value)
                    Next
                    Dim cs As ComplexSettingItem = _ComplexAdvancedSettings.FirstOrDefault(Function(y) y.Section = l.@Section AndAlso y.Name = l.@Name)
                    If cs Is Nothing Then
                        _ComplexAdvancedSettings.Add(New ComplexSettingItem With {.Section = l.@Section, .Name = l.@Name})
                        cs = _ComplexAdvancedSettings.FirstOrDefault(Function(y) y.Section = l.@Section AndAlso y.Name = l.@Name)
                    Else
                        cs.TableItem.Clear()
                    End If
                    cs.TableItem = dict
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Sub Save(ByVal fname As String)
        Try
            If File.Exists(fname) Then
                File.Delete(fname)
            End If
            Dim xdoc As New XmlDocument()
            xdoc.LoadXml("<?xml version=""1.0"" encoding=""utf-8""?><AdvancedSettings xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""></AdvancedSettings>")

            Dim count As Integer = 0
            For Each i As SettingItem In _AdvancedSettings.Where(Function(x) (x.DefaultValue = "" OrElse Not x.DefaultValue = x.Value) AndAlso Not x.Value = "")
                Dim elem As XmlElement = xdoc.CreateElement("Setting")
                Dim attr As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "Section", "Section", "")
                attr.Value = i.Section
                elem.Attributes.SetNamedItem(attr)
                Dim attr2 As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "Name", "Name", "")
                attr2.Value = i.Name
                elem.Attributes.SetNamedItem(attr2)
                elem.InnerText = i.Value
                xdoc.DocumentElement.AppendChild(elem)
                count += 1
            Next
            Dim elemp As XmlElement = xdoc.CreateElement("ComplexSettings")
            For Each i As ComplexSettingItem In _ComplexAdvancedSettings

                If Not i.TableItem Is Nothing Then
                    Dim elem As XmlElement = xdoc.CreateElement("Table")
                    Dim attr As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "Section", "Section", "")
                    attr.Value = i.Section
                    elem.Attributes.SetNamedItem(attr)
                    Dim attr2 As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "Name", "Name", "")
                    attr2.Value = i.Name
                    elem.Attributes.SetNamedItem(attr2)
                    For Each ti In i.TableItem.Keys
                        Dim elemi As XmlElement = xdoc.CreateElement("Item")
                        Dim attr3 As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "Name", "Name", "")
                        attr3.Value = ti.ToString
                        elemi.InnerText = i.TableItem.Item(ti.ToString).ToString
                        elemi.Attributes.SetNamedItem(attr3)
                        elem.AppendChild(elemi)
                    Next
                    elemp.AppendChild(elem)
                    count += 1
                End If
            Next
            xdoc.DocumentElement.AppendChild(elemp)
            If count > 0 Then xdoc.Save(fname)
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetSetting(ByVal key As String, ByVal defvalue As String, ByVal Assembly As String) As String
        Try
            Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
            Return If(v(0) Is Nothing, defvalue, v(0).Value.ToString)

        Catch ex As Exception
            Return defvalue
        End Try
    End Function
    Public Shared Function SetSetting(ByVal key As String, ByVal value As Object, ByVal Assembly As String, Optional ByVal isDefault As Boolean = False) As Boolean
        Try
            Dim v = _AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
            If v Is Nothing Then
                _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = Convert.ToString(value), .DefaultValue = If(isDefault, Convert.ToString(value), "")})
            Else
                _AdvancedSettings.Remove(v)
                _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = Convert.ToString(value), .DefaultValue = If(isDefault, Convert.ToString(value), "")})
                '_AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly).Value = Convert.ToString(value)
            End If

        Catch ex As Exception
        End Try
        Return True
    End Function
End Class