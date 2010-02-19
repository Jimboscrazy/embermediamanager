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
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization



Public Class FileManagerExternalModule
    Implements EmberAPI.Interfaces.EmberExternalModule
    Dim emmRuntimeObjects As New ModulesManager.EmberRuntimeObjects
    Private _Name As String = "Media File Manager"
    Private _Version As String = "0.1"

    Dim MyMenuSep As New System.Windows.Forms.ToolStripSeparator
    Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu1 As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu2 As New System.Windows.Forms.ToolStripMenuItem
    Dim FolderSubMenus As New List(Of System.Windows.Forms.ToolStripMenuItem)
    Private MyPath As String
    Sub Setup() Implements EmberAPI.Interfaces.EmberExternalModule.Setup
        Dim _setup As New frmSetup
        Dim li As ListViewItem
        For Each e As SettingItem In eSettings.ModuleSettings
            li = New ListViewItem
            li.Text = e.Name
            li.SubItems.Add(e.FolderPath)
            _setup.ListView1.Items.Add(li)
        Next
        _setup.ShowDialog()
        eSettings.ModuleSettings.Clear()
        For Each i As ListViewItem In _setup.ListView1.Items
            Dim s As New SettingItem
            s.Name = i.SubItems(0).Text
            s.FolderPath = i.SubItems(1).Text
            eSettings.ModuleSettings.Add(s)
        Next
        Save()
        'PopulateFolders()
        PopulateFolders(MySubMenu1)
        PopulateFolders(MySubMenu2)
    End Sub
    Sub Enable() Implements EmberAPI.Interfaces.EmberExternalModule.Enable
        MyMenu.Text = "Media File Manager"
        MySubMenu1.Text = "Move To"
        MySubMenu1.Tag = "MOVE"
        MySubMenu2.Text = "Copy To"
        MySubMenu2.Tag = "COPY"
        MyMenu.DropDownItems.Add(MySubMenu1)
        MyMenu.DropDownItems.Add(MySubMenu2)
        emmRuntimeObjects.MenuMediaList.Items.Add(MyMenuSep)
        emmRuntimeObjects.MenuMediaList.Items.Add(MyMenu)

        'PopulateFolders()
        PopulateFolders(MySubMenu1)
        PopulateFolders(MySubMenu2)
    End Sub
    Sub Disable() Implements EmberAPI.Interfaces.EmberExternalModule.Disable
        emmRuntimeObjects.MenuMediaList.Items.Remove(MyMenuSep)
        emmRuntimeObjects.MenuMediaList.Items.Remove(MyMenu)
    End Sub
    Sub Init(ByRef emm As ModulesManager.EmberRuntimeObjects) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        emmRuntimeObjects = emm
        MyPath = Path.Combine(Functions.AppPath, "Modules")
        Load()
    End Sub

    ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property

    Private Sub MySubMenuItem1_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MySubMenu1.MouseHover
        'PopulateFolders(sender)
    End Sub
    Private Sub MySubMenuItem2_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MySubMenu2.MouseHover
        'PopulateFolders(sender)
    End Sub
    Private Sub FolderSubMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles FolderSubMenus.Click
        Try
            Dim ItemsToWork As New List(Of IO.FileSystemInfo)
            Dim MoviesToWork As New List(Of Long)
            Dim MovieId As Int64 = -1

            For Each sRow As DataGridViewRow In emmRuntimeObjects.MediaList.SelectedRows
                MovieId = Convert.ToInt64(sRow.Cells(0).Value)
                If Not MoviesToWork.Contains(MovieId) Then
                    MoviesToWork.Add(MovieId)
                End If
            Next
            If MoviesToWork.Count > 0 Then
                Dim mMovie As New Object
                Dim FileDelete As New FileUtils.Delete
                For Each Id As Long In MoviesToWork
                    mMovie = Master.DB.LoadMovieFromDB(Id)
                    ItemsToWork = FileDelete.GetItemsToDelete(False, mMovie)
                    If ItemsToWork.Count = 1 AndAlso Directory.Exists(ItemsToWork(0).ToString) Then
                        Select Case sender.OwnerItem.Tag
                            Case "MOVE"
                                MsgBox("Move from " + ItemsToWork(0).ToString + " To " + Path.Combine(sender.tag, Path.GetFileName(ItemsToWork(0).ToString)), MsgBoxStyle.Information, "Move")
                                'TODO:  Before activate this, need to test it better and move to background worker
                                'DirectoryCopy(ItemsToWork(0).ToString, Path.Combine(sender.tag, Path.GetFileName(ItemsToWork(0).ToString)))
                                'Directory.Delete(ItemsToWork(0).ToString, True)
                            Case "COPY"
                                MsgBox("Copy from " + ItemsToWork(0).ToString + " To " + Path.Combine(sender.tag, Path.GetFileName(ItemsToWork(0).ToString)), MsgBoxStyle.Information, "Move")
                                'TODO:  Before activate this, need to test it better and move to background worker
                                'DirectoryCopy(ItemsToWork(0).ToString, Path.Combine(sender.tag, Path.GetFileName(ItemsToWork(0).ToString)))
                        End Select
                    End If
                Next
            End If

        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Sub PopulateFolders(ByVal mnu As System.Windows.Forms.ToolStripMenuItem)
        FolderSubMenus.RemoveAll(Function(b) True)
        For Each e In eSettings.ModuleSettings
            Dim FolderSubMenuItem As New System.Windows.Forms.ToolStripMenuItem
            FolderSubMenuItem.Text = e.Name
            FolderSubMenuItem.Tag = e.FolderPath
            FolderSubMenus.Add(FolderSubMenuItem)
            AddHandler FolderSubMenuItem.Click, AddressOf Me.FolderSubMenuItem_Click
        Next
        mnu.DropDownItems.Clear()
        For Each i In FolderSubMenus
            mnu.DropDownItems.Add(i)
        Next
    End Sub


    Private eSettings As New Settings
    Public Sub Save()
        Try
            Dim xmlSerial As New XmlSerializer(GetType(Settings))
            Dim xmlWriter As New StreamWriter(Path.Combine(MyPath, "FileManager.xml"))
            xmlSerial.Serialize(xmlWriter, eSettings)
            xmlWriter.Close()
        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Load()
        Try
            Dim xmlSerial As New XmlSerializer(GetType(Settings))
            If File.Exists(Path.Combine(MyPath, "FileManager.xml")) Then
                Dim strmReader As New StreamReader(Path.Combine(MyPath, "FileManager.xml"))
                eSettings = DirectCast(xmlSerial.Deserialize(strmReader), Settings)
                strmReader.Close()
            Else
                eSettings = New Settings
            End If
        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            eSettings = New Settings
        End Try
    End Sub
    Class SettingItem
        Public Name As String
        Public FolderPath As String
    End Class
    Class Settings
        Private _settings As New List(Of SettingItem)
        Public Property ModuleSettings() As List(Of SettingItem)
            Get
                Return Me._settings
            End Get
            Set(ByVal value As List(Of SettingItem))
                Me._settings = value
            End Set
        End Property
    End Class
    Private Sub DirectoryCopy(ByVal sourceDirName As String, ByVal destDirName As String)
        Dim dir As New DirectoryInfo(sourceDirName)
        ' If the source directory does not exist, throw an exception.
        If Not dir.Exists Then
            'Throw New DirectoryNotFoundException(Master.eLang.GetString(364, "Source directory does not exist or could not be found: ") + sourceDirName)
        End If
        ' If the destination directory does not exist, create it.
        If Not Directory.Exists(destDirName) Then
            Directory.CreateDirectory(destDirName)
        End If
        ' Get the file contents of the directory to copy.
        Dim Files As New List(Of FileInfo)

        Try
            Files.AddRange(dir.GetFiles())
        Catch
        End Try

        For Each sFile As FileInfo In Files
            MoveFileWithStream(sFile.FullName, Path.Combine(destDirName, sFile.Name))
        Next

        Files = Nothing
        dir = Nothing
    End Sub
    Public Shared Sub MoveFileWithStream(ByVal sPathFrom As String, ByVal sPathTo As String)

        Try
            Using SourceStream As FileStream = New FileStream(String.Concat("", sPathFrom, ""), FileMode.Open, FileAccess.Read)
                Using DestinationStream As FileStream = New FileStream(String.Concat("", sPathTo, ""), FileMode.Create, FileAccess.Write)
                    Dim StreamBuffer(Convert.ToInt32(SourceStream.Length - 1)) As Byte

                    SourceStream.Read(StreamBuffer, 0, StreamBuffer.Length)
                    DestinationStream.Write(StreamBuffer, 0, StreamBuffer.Length)

                    StreamBuffer = Nothing
                End Using
            End Using
        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
End Class

