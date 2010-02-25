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

<XmlRoot("VersionsFile")> _
Public Class UpgradeList
    <XmlArray("Versions")> _
    <XmlArrayItem("Version")> _
    Public VersionList As New List(Of Versions)

    <XmlArray("APIVersions")> _
    <XmlArrayItem("Version")> _
    Public APIVersionList As New List(Of Versions)

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(UpgradeList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class Versions : Implements IComparable(Of Versions)
    <XmlAttribute("Number")> _
    Public Version As String

    Public Function CompareTo(ByVal other As Versions) As Integer Implements IComparable(Of Versions).CompareTo
        Return Convert.ToInt32(Me.Version).CompareTo(Convert.ToInt32(other.Version))
    End Function
End Class

Public Class FileToInstall
    Public Filename As String
    Public OriginalPath As String
    Public EmberPath As String
    Public Hash As String
    Public Platform As String
End Class

<XmlRoot("UpgradeFile")> _
Public Class FilesList
    <XmlArray("Files")> _
    <XmlArrayItem("File")> _
    Public Files As List(Of FileOfList)
    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(FilesList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class FileOfList
    Public Path As String
    Public Filename As String
    Public Platform As String
    Public Hash As String
    Public NeedBackup As Boolean = False
    Public NeedInstall As Boolean = True
    Public inCache As Boolean = False
End Class

<XmlRoot("CommandFile")> _
Public Class InstallCommands
    <XmlArray("Commands")> _
    <XmlArrayItem("Command")> _
    Public Command As List(Of InstallCommand)

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(InstallCommands))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class InstallCommand
    <XmlElement("Description")> _
    Public CommandDescription As String
    <XmlAttribute("Type")> _
    Public CommandType As String
    <XmlElement("Execute")> _
    Public CommandExecute As String
End Class