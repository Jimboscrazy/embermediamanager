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
Imports System.IO.IsolatedStorage


Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class Localization
    Structure Locs
        Dim AssenblyName As String
        Dim htStrings As Hashtable
        Dim FileName As String
    End Structure

    Private Shared htStrings As New Hashtable
    Private Shared htArrayStrings As New List(Of Locs)
    Private _all As String
    Private _none As String
    Private _disabled As String

    Public Property All() As String
        Get
            Return _all
        End Get
        Set(ByVal value As String)
            _all = value
        End Set
    End Property

    Public Property None() As String
        Get
            Return _none
        End Get
        Set(ByVal value As String)
            _none = value
        End Set
    End Property

    Public Property Disabled() As String
        Get
            Return _disabled
        End Get
        Set(ByVal value As String)
            _disabled = value
        End Set
    End Property

    Public Sub New()
        Me.Clear()
    End Sub

    Public Sub Clear()
        _all = "All"
        _none = "[none]"
        _disabled = "[Disabled]"
    End Sub

    Public Sub LoadLanguage(ByVal Language As String)
        Dim _old_all As String = _all
        Me.Clear()
        Try
            If Not String.IsNullOrEmpty(Language) Then
                Dim Assembly As String = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
                Dim lPath As String
                htStrings = New Hashtable
                htStrings.Clear()
                If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                    Assembly = "EmberCORE"
                    lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                Else
                    lPath = String.Concat(Functions.AppPath, "Modules", Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, Assembly, ".", Language, ".xml")
                    If Not File.Exists(lPath) Then 'Fallback, maybe not good idea, but needed for now
                        lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                    End If
                End If
                If File.Exists(lPath) Then
                    Dim LangXML As XDocument = XDocument.Load(lPath)
                    Dim xLanguage = From xLang In LangXML...<strings>...<string> Select xLang.@id, xLang.Value
                    If xLanguage.Count > 0 Then
                        For i As Integer = 0 To xLanguage.Count - 1
                            htStrings.Add(Convert.ToInt32(xLanguage(i).id), xLanguage(i).Value)
                        Next
                        Dim _loc As New Locs With {.AssenblyName = Assembly, .htStrings = htStrings, .FileName = lPath}
                        htArrayStrings.Add(_loc)
                        _all = String.Format("[{0}]", GetString(569, Master.eLang.All))
                        _none = GetString(570, Master.eLang.None)
                        _disabled = GetString(571, Master.eLang.Disabled)

                    End If
                Else
                    MsgBox(String.Concat(String.Format("Cannot find {0}.xml.", Language), vbNewLine, vbNewLine, "Expected path:", vbNewLine, lPath), MsgBoxStyle.Critical, "File Not Found")
                End If
            End If

            ' Need to change Globaly Langs_all
            Master.eSettings.GenreFilter = Master.eSettings.GenreFilter.Replace(_old_all, _all)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Public Function GetString(ByVal ID As Integer, ByVal strDefault As String) As String
        Dim Assembly As String = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
        If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
            Assembly = "EmberCORE"
        End If
        htStrings = htArrayStrings.Where(Function(x) x.AssenblyName = Assembly)(0).htStrings
        If htStrings Is Nothing Then Return strDefault
        If htStrings.ContainsKey(ID) Then
            Return htStrings.Item(ID).ToString
        Else
            '*****************************************************************************************
            ' this will add strings not found *** Dev propose only, should not go to release version
            Try

                Dim lPath As String
                Dim Language As String = Master.eSettings.Language
                If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" OrElse Assembly = "EmberCORE" Then
                    lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                Else
                    lPath = String.Concat(Functions.AppPath, "Modules", Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, Assembly, ".", Language, ".xml")
                End If
                AddNotExist(lPath, ID.ToString, strDefault)
            Catch ex As Exception
            End Try
            '*****************************************************************************************
            Return strDefault
        End If
    End Function
    Sub AddNotExist(ByVal lpath As String, ByVal id As String, ByVal value As String)
        Dim xdoc As New XmlDocument()
        xdoc.Load(lpath)
        Dim elem As XmlElement = xdoc.CreateElement("string")
        Dim attr As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "id", "id", "")
        attr.Value = id
        elem.Attributes.SetNamedItem(attr)
        elem.InnerText = value
        xdoc.DocumentElement.AppendChild(elem)
        xdoc.Save(lpath)
    End Sub

End Class
