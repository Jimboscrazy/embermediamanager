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
    Private _enabled As Boolean = False
    Private _Name As String = "Media File Manager"

    Dim MyMenuSep As New System.Windows.Forms.ToolStripSeparator
    Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu1 As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu2 As New System.Windows.Forms.ToolStripMenuItem
    Dim FolderSubMenus As New List(Of System.Windows.Forms.ToolStripMenuItem)
    Dim _setup As frmSettingsHolder
    Private MyPath As String
    Private Structure Arguments
        Dim src As String
        Dim dst As String
    End Structure
    Friend WithEvents bwCopyDirectory As New System.ComponentModel.BackgroundWorker

    Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
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
    Function InjectSetup() As Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmSettingsHolder
        Load()
        Dim li As ListViewItem
        _setup.cbEnabled.Checked = _enabled
        For Each e As SettingItem In eSettings.ModuleSettings
            li = New ListViewItem
            li.Text = e.Name
            li.SubItems.Add(e.FolderPath)
            _setup.ListView1.Items.Add(li)
        Next
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(999, "Modules")
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = _setup.pnlSettings
        Return SPanel
    End Function
    Sub SaveSetupScraper() Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        eSettings.ModuleSettings.Clear()
        For Each i As ListViewItem In _setup.ListView1.Items
            eSettings.ModuleSettings.Add(New SettingItem With {.Name = i.SubItems(0).Text, .FolderPath = i.SubItems(1).Text})
        Next
        Save()
        'PopulateFolders()
        PopulateFolders(MySubMenu1)
        PopulateFolders(MySubMenu2)
        _setup.Dispose()
    End Sub

    Sub Enable()
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
    Sub Disable()
        emmRuntimeObjects.MenuMediaList.Items.Remove(MyMenuSep)
        emmRuntimeObjects.MenuMediaList.Items.Remove(MyMenu)
    End Sub
    Sub Init(ByRef emm As ModulesManager.EmberRuntimeObjects) Implements EmberAPI.Interfaces.EmberExternalModule.Init
        emmRuntimeObjects = emm
        'Master.eLang.LoadLanguage(Master.eSettings.Language)
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
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
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
            Dim tMItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)

            For Each sRow As DataGridViewRow In emmRuntimeObjects.MediaList.SelectedRows
                MovieId = Convert.ToInt64(sRow.Cells(0).Value)
                If Not MoviesToWork.Contains(MovieId) Then
                    MoviesToWork.Add(MovieId)
                End If
            Next
            If MoviesToWork.Count > 0 Then
                Dim mMovie As New Structures.DBMovie
                Dim FileDelete As New FileUtils.Delete
                For Each Id As Long In MoviesToWork
                    mMovie = Master.DB.LoadMovieFromDB(Id)
                    ItemsToWork = FileDelete.GetItemsToDelete(False, mMovie)
                    If ItemsToWork.Count = 1 AndAlso Directory.Exists(ItemsToWork(0).ToString) Then
                        Select Case tMItem.OwnerItem.Tag.ToString
                            Case "MOVE"
                                MsgBox("Move from " + ItemsToWork(0).ToString + " To " + Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)), MsgBoxStyle.Information, "Move")
                                'TODO:  need to test it better and move to background worker
                                DirectoryCopy(ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)))
                                Directory.Delete(ItemsToWork(0).ToString, True)
                                Master.DB.DeleteFromDB(MovieId)
                            Case "COPY"
                                MsgBox("Copy from " + ItemsToWork(0).ToString + " To " + Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)), MsgBoxStyle.Information, "Move")
                                'TODO:   need to test it better and move to background worker
                                DirectoryCopy(ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)))
                        End Select
                    End If
                Next
            End If

        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub bwCopyDirectory_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCopyDirectory.DoWork
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        DirectoryCopy(Args.src, Args.dst)
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
        Dim Names As String = String.Empty
        Dim Paths As String = String.Empty
        For Each i As SettingItem In eSettings.ModuleSettings
            Names += String.Concat(If(String.IsNullOrEmpty(Names), String.Empty, "|"), i.Name)
            Paths += String.Concat(If(String.IsNullOrEmpty(Paths), String.Empty, "|"), i.FolderPath)
        Next
        AdvancedSettings.SetSetting("Names", Names)
        AdvancedSettings.SetSetting("Paths", Paths)
    End Sub

    Public Sub Load()
        Dim Names As String() = AdvancedSettings.GetSetting("Names", String.Empty).Split(Convert.ToChar("|"))
        Dim Paths As String() = AdvancedSettings.GetSetting("Paths", String.Empty).Split(Convert.ToChar("|"))
        For n = 0 To Names.Count - 1
            If Not String.IsNullOrEmpty(Names(n)) AndAlso Not String.IsNullOrEmpty(Paths(n)) Then eSettings.ModuleSettings.Add(New SettingItem With {.Name = Names(n), .FolderPath = Paths(n)})
        Next
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

