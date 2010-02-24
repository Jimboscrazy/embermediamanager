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


Imports System.Xml.Linq
Imports System.Xml
Imports System
Imports System.IO
Imports System.Xml.Serialization
Imports System.Linq

<Serializable()> _
Public Class AdvancedSettings
    Private Shared Sub SetDefaults()
        _DoNotSave = True
        SetBooleanSetting("Renamer.UseDTSInAudioChannel", True)
        _DoNotSave = False
    End Sub
    ' ******************************************************************************
    Private Class SettingItem
        Public Section As String
        Public Name As String
        Public Value As String
        Public DefaultValue As String
    End Class
    Private Shared _AdvancedSettings As New List(Of SettingItem)
    Private Shared _DoNotSave As Boolean = False
    Public Sub New()
        SetDefaults()
        Load()
    End Sub
    Public Shared Function GetSetting(ByVal key As String, Optional ByVal cAssembly As String = "") As String
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberCORE"
            End If
        End If
        Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
        Return If(v(0) Is Nothing, "", v(0).Value.ToString)
    End Function
    Public Shared Function GetBooleanSetting(ByVal key As String, Optional ByVal defvalue As Boolean = False, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberCORE"
            End If
        End If
        Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
        Return If(v(0) Is Nothing, defvalue, Convert.ToBoolean(v(0).Value.ToString))
    End Function
    Public Shared Function SetSetting(ByVal key As String, ByVal value As String, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberCORE"
            End If
            Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
            If v(0) Is Nothing Then
                _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = value, .DefaultValue = If(Assembly = "*EmberCORE", value, "")})
            Else
                _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)(0).Value = value
            End If
        End If
        If Not _DoNotSave Then Save()
        Return True
    End Function
    Public Shared Function SetBooleanSetting(ByVal key As String, ByVal value As Boolean, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberCORE"
            End If
            Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
            If v(0) Is Nothing Then
                _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = Convert.ToString(value), .DefaultValue = If(Assembly = "*EmberCORE", Convert.ToString(value), "")})
            Else
                _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)(0).Value = Convert.ToString(value)
            End If
        End If
        If Not _DoNotSave Then Save()
        Return True
    End Function
    Public Shared Sub Save()
        If File.Exists(Path.Combine(Functions.AppPath, "AdvancedSettings.xml")) Then
            File.Delete(Path.Combine(Functions.AppPath, "AdvancedSettings.xml"))
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
        If count > 0 Then xdoc.Save(Path.Combine(Functions.AppPath, "AdvancedSettings.xml"))
    End Sub
    Public Shared Sub Load()
        _DoNotSave = True
        If File.Exists(Path.Combine(Functions.AppPath, "AdvancedSettings.xml")) Then
            Dim xdoc As New XDocument
            xdoc = XDocument.Load(Path.Combine(Functions.AppPath, "AdvancedSettings.xml"))
            For Each i As XElement In xdoc...<Setting>
                _AdvancedSettings.Add(New SettingItem With {.Section = i.@Section, .Name = i.@Name, .Value = i.Value, .DefaultValue = ""})
            Next
        End If
        _DoNotSave = False
    End Sub
End Class
