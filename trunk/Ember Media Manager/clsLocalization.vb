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

Option Explicit On

Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class Localization
    Private Shared htStrings As New Hashtable

    Public Sub LoadLanguage(ByVal Language As String)
        htStrings.Clear()
        If Not String.IsNullOrEmpty(Language) Then
            Dim lPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, String.Concat(Language, ".xml"))
            If File.Exists(lPath) Then
                Dim LangXML As XDocument = XDocument.Load(lPath)
                Dim xLanguage = From xLang In LangXML...<strings> Select xLang.<string>.@id, xLang.<string>.Value
                If xLanguage.Count > 0 Then
                    For i As Integer = 0 To xLanguage.Count - 1
                        htStrings.Add(xLanguage(i).id, xLanguage(i).string)
                    Next
                End If
            Else
                MsgBox(String.Concat(String.Format("Cannot find {0}.xml.", Language), vbNewLine, vbNewLine, "Expected path:", vbNewLine, lPath), MsgBoxStyle.Critical, "File Not Found")
            End If
        End If
    End Sub

    Public Function GetString(ByVal ID As String, ByVal strDefault As String) As String
        If htStrings.ContainsKey(ID) Then
            Return htStrings.Item(ID)
        Else
            Return strDefault
        End If
    End Function

End Class
