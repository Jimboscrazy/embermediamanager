﻿' ################################################################################
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
Imports System.Linq
Imports System.Xml
Imports System.Xml.Linq
Imports System.Xml.Serialization

<Serializable> _
Public Class AdvancedSettings

    #Region "Fields"

    Private Class SettingGroupItem
        Public Section As String
        Public GroupName As String
        Public Items As New List(Of SettingItem)
    End Class
    Private Shared _AdvancedSettings As New List(Of SettingItem)
    Private Shared _ComplexAdvancedSettings As New List(Of ComplexSettingItem)
    Private Shared _DoNotSave As Boolean = False

    #End Region 'Fields

    #Region "Constructors"

    Public Sub New()
        SetDefaults()
        Load(Path.Combine(Functions.AppPath, "AdvancedSettings.xml"))
    End Sub

    #End Region 'Constructors

    #Region "Methods"

    Public Shared Function GetBooleanSetting(ByVal key As String, ByVal defvalue As Boolean, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
        Return If(v(0) Is Nothing, defvalue, Convert.ToBoolean(v(0).Value.ToString))
    End Function

    Public Shared Function GetSetting(ByVal key As String, ByVal defvalue As String, Optional ByVal cAssembly As String = "") As String
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = From e In _AdvancedSettings.Where(Function(f) f.Name = key AndAlso f.Section = Assembly)
        Return If(v(0) Is Nothing, defvalue, v(0).Value.ToString)
    End Function

    Public Shared Sub ClearComplexSetting(ByVal key As String, Optional ByVal cAssembly As String = "")
        Try
            Dim Assembly As String = cAssembly
            If Assembly = "" Then
                Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
                If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                    Assembly = "*EmberAPP"
                End If
            End If
            Dim v = _ComplexAdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
            If Not v Is Nothing Then v.TableItem.Clear()
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function GetComplexSetting(ByVal key As String, Optional ByVal cAssembly As String = "") As Hashtable
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = _ComplexAdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
        Return If(v Is Nothing, Nothing, v.TableItem)
    End Function

    Public Shared Function SetComplexSetting(ByVal key As String, ByVal value As Hashtable, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = _ComplexAdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
        If v Is Nothing Then
            _ComplexAdvancedSettings.Add(New ComplexSettingItem With {.Section = Assembly, .Name = key, .TableItem = value})
        Else
            _ComplexAdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly).TableItem = value
        End If

        If Not _DoNotSave Then Save()
        Return True
    End Function

    Public Shared Sub Load(ByVal fname As String)
        _DoNotSave = True
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        _DoNotSave = False
    End Sub

    Public Shared Sub Save()
        Try
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
            If count > 0 Then xdoc.Save(Path.Combine(Functions.AppPath, "AdvancedSettings.xml"))
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function SetBooleanSetting(ByVal key As String, ByVal value As Boolean, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = _AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
        If v Is Nothing Then
            _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = Convert.ToString(value), .DefaultValue = If(Assembly = "*EmberAPP", Convert.ToString(value), "")})
        Else
            _AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly).Value = Convert.ToString(value)
        End If

        If Not _DoNotSave Then Save()
        Return True
    End Function

    Public Shared Function SetSetting(ByVal key As String, ByVal value As String, Optional ByVal cAssembly As String = "") As Boolean
        Dim Assembly As String = cAssembly
        If Assembly = "" Then
            Assembly = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
            If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                Assembly = "*EmberAPP"
            End If
        End If
        Dim v = _AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly)
        If v Is Nothing Then
            _AdvancedSettings.Add(New SettingItem With {.Section = Assembly, .Name = key, .Value = value, .DefaultValue = If(Assembly = "*EmberAPP", value, "")})
        Else
            _AdvancedSettings.FirstOrDefault(Function(f) f.Name = key AndAlso f.Section = Assembly).Value = value
        End If

        If Not _DoNotSave Then Save()
        Return True
    End Function

    Private Shared Sub SetDefaults()
        _DoNotSave = True
        SetSetting("AudioFormatConvert:ac-3", "ac3")
        SetSetting("AudioFormatConvert:a_ac3", "ac3")
        SetSetting("AudioFormatConvert:a_aac", "aac")
        SetSetting("AudioFormatConvert:wma2", "wmav2")
        SetSetting("AudioFormatConvert:a_dts", "dca")
        SetSetting("AudioFormatConvert:dts", "dca")

        SetSetting("VideoFormatConvert:divx 5", "dx50")
        SetSetting("VideoFormatConvert:mpeg-4 video", "mpeg4")
        SetSetting("VideoFormatConvert:divx 3", "div3")
        SetSetting("VideoFormatConvert:lmp4", "h264")
        SetSetting("VideoFormatConvert:svq3", "h264")
        SetSetting("VideoFormatConvert:v_mpeg4/iso/avc", "h264")
        SetSetting("VideoFormatConvert:x264", "h264")
        SetSetting("VideoFormatConvert:avc", "h264")
        SetSetting("VideoFormatConvert:swf", "flv")
        SetSetting("VideoFormatConvert:3iv0", "3ivx")
        SetSetting("VideoFormatConvert:3iv1", "3ivx")
        SetSetting("VideoFormatConvert:3iv2", "3ivx")
        SetSetting("VideoFormatConvert:3ivd", "3ivx")

        SetSetting("CheckStackMarkers", "[\W_]+(cd|dvd|part|dis[ck])([0-9])")
        SetSetting("DeleteStackMarkers", "[\W_]\s?(cd|dvd|part|dis[ck])([0-9])?")
        SetBooleanSetting("DisableMultiPartMedia", False)

        SetSetting("SubtitleExtension", ".*\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$")
        SetSetting("ToProperCase", "\b(hd|cd|dvd|bc|b\.c\.|ad|a\.d\.|sw|nw|se|sw|ii|iii|iv|vi|vii|viii|ix|x)\b")

        SetSetting("NotValidDirIs", "extrathumbs|video_ts|bdmv|audio_ts|recycler|subs|subtitles|.trashes")
        SetSetting("NotValidDirContains", "-trailer|[trailer|temporary files|(noscan)|$recycle.bin|lost+found|system volume information|sample")

        SetSetting("ForceTitle", "Argentina|Australia|Belgium|Brazil|Canada: English title|Canada: French title|Finland|France|Germany|Hong Kong|Iceland|Ireland|Netherlands|New Zealand|Peru|Portugal|Singapore|South Korea|Spain|Sweden|Switzerland|UK|USA")

        Dim keypair As New Hashtable
        keypair.Add("(b[dr][-\s]?rip|blu[-\s]?ray)", "bluray")
        keypair.Add("hd[-\s]?dvd", "hddvd")
        keypair.Add("hd[-\s]?tv", "hdtv")
        keypair.Add("(sd[-\s]?)?dvd", "dvd")
        keypair.Add("sd[-\s]?tv", "sdtv")
        SetComplexSetting("MovieSources", keypair)
        _DoNotSave = False
    End Sub

    #End Region 'Methods

    #Region "Nested Types"

    ' ******************************************************************************
    Private Class SettingItem
#Region "Fields"
        Public DefaultValue As String
        Public Name As String
        Public Section As String
        Public Value As String
#End Region 'Fields
    End Class
    Public Class ComplexSettingItem
        Public Name As String
        Public Section As String
        Public TableItem As New Hashtable
    End Class


    #End Region 'Nested Types

End Class