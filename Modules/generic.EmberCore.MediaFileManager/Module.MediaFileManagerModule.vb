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
    Implements Interfaces.EmberExternalModule
    Private _enabled As Boolean = False
    Private _Name As String = Master.eLang.GetString(824, "Media File Manager")

    Private MyMenuSep As New System.Windows.Forms.ToolStripSeparator
    Private MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MySubMenu1 As New System.Windows.Forms.ToolStripMenuItem
    Private WithEvents MySubMenu2 As New System.Windows.Forms.ToolStripMenuItem
    Private FolderSubMenus As New List(Of System.Windows.Forms.ToolStripMenuItem)
    Private _setup As frmSettingsHolder
    Private MyPath As String
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberExternalModule.ModuleSettingsChanged
    Public Event ModuleEnabledChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements Interfaces.EmberExternalModule.ModuleSetupChanged
    Public Event GenericEvent(ByVal _params As List(Of Object)) Implements Interfaces.EmberExternalModule.GenericEvent

    Private Structure Arguments
        Dim src As String
        Dim dst As String
    End Structure
    Friend WithEvents bwCopyDirectory As New System.ComponentModel.BackgroundWorker

    Public ReadOnly Property ModuleType() As List(Of Enums.ModuleEventType) Implements Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Generic})
        End Get
    End Property

    Property Enabled() As Boolean Implements Interfaces.EmberExternalModule.Enabled
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
    Function InjectSetup() As Containers.SettingsPanel Implements Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        _setup = New frmSettingsHolder
        Load()
        Dim li As ListViewItem
        _setup.ListView1.Items.Clear()
        _setup.cbEnabled.Checked = _enabled
        For Each e As SettingItem In eSettings.ModuleSettings
            li = New ListViewItem
            li.Text = e.Name
            li.SubItems.Add(e.FolderPath)
            _setup.ListView1.Items.Add(li)
        Next
        SPanel.Name = Me._Name
        SPanel.Text = Me._Name
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = _setup.pnlSettings
        AddHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
        AddHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        Return SPanel
    End Function

    Private Sub Handle_ModuleEnabledChanged(ByVal State As Boolean)
        RaiseEvent ModuleEnabledChanged(Me._Name, State, 0)
    End Sub

    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub

    Sub SaveSetupScraper(ByVal DoDispose As Boolean) Implements Interfaces.EmberExternalModule.SaveSetup
        Me._enabled = Me._setup.cbEnabled.Checked
        eSettings.ModuleSettings.Clear()
        For Each i As ListViewItem In _setup.ListView1.Items
            eSettings.ModuleSettings.Add(New SettingItem With {.Name = i.SubItems(0).Text, .FolderPath = i.SubItems(1).Text})
        Next
        Save()
        'PopulateFolders()
        PopulateFolders(MySubMenu1)
        PopulateFolders(MySubMenu2)
        If DoDispose Then
            RemoveHandler Me._setup.ModuleEnabledChanged, AddressOf Handle_ModuleEnabledChanged
            RemoveHandler Me._setup.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
            _setup.Dispose()
        End If
    End Sub

    Sub Enable()
        MyMenu.Text = Master.eLang.GetString(824, "Media File Manager")

        MySubMenu1.Text = Master.eLang.GetString(825, "Move To")
        MySubMenu1.Tag = "MOVE"
        MySubMenu2.Text = Master.eLang.GetString(826, "Copy To")
        MySubMenu2.Tag = "COPY"
        MyMenu.DropDownItems.Add(MySubMenu1)
        MyMenu.DropDownItems.Add(MySubMenu2)

        ModulesManager.Instance.RuntimeObjects.MenuMediaList.Items.Add(MyMenuSep)
        ModulesManager.Instance.RuntimeObjects.MenuMediaList.Items.Add(MyMenu)
        'PopulateFolders()
        PopulateFolders(MySubMenu1)
        PopulateFolders(MySubMenu2)
        MyMenuSep.Visible = (eSettings.ModuleSettings.Count > 0)
        MyMenu.Visible = (eSettings.ModuleSettings.Count > 0)

    End Sub
    Sub Disable()
        ModulesManager.Instance.RuntimeObjects.MenuMediaList.Items.Remove(MyMenuSep)
        ModulesManager.Instance.RuntimeObjects.MenuMediaList.Items.Remove(MyMenu)
    End Sub
    Sub Init(ByVal sAssemblyName As String) Implements Interfaces.EmberExternalModule.Init
        'Master.eLang.LoadLanguage(Master.eSettings.Language)
        MyPath = Path.Combine(Functions.AppPath, "Modules")
        Master.eLang.LoadLanguage(Master.eSettings.Language)
        Load()
    End Sub

    ReadOnly Property ModuleName() As String Implements Interfaces.EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements Interfaces.EmberExternalModule.ModuleVersion
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

            For Each sRow As DataGridViewRow In ModulesManager.Instance.RuntimeObjects.MediaList.SelectedRows
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
                                If MsgBox(String.Format(Master.eLang.GetString(827, "Move from {0} To {1}"), ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString))), MsgBoxStyle.YesNo, "Move") = MsgBoxResult.Yes Then
                                    'TODO:  need to test it better
                                    DirectoryMove(ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)), Master.eLang.GetString(756, "Moving Movie"))
                                    Master.DB.DeleteFromDB(MovieId)
                                End If

                            Case "COPY"
                                If MsgBox(String.Format(Master.eLang.GetString(828, "Copy from {0} To {1}"), ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString))), MsgBoxStyle.YesNo, "Copy") = MsgBoxResult.Yes Then
                                    'TODO:   need to test it better 
                                    DirectoryCopy(ItemsToWork(0).ToString, Path.Combine(tMItem.Tag.ToString, Path.GetFileName(ItemsToWork(0).ToString)), Master.eLang.GetString(756, "Copying Movie"))
                                End If
                        End Select
                    End If
                Next
            End If

        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Sub DirectoryCopy(ByVal src As String, ByVal dst As String, Optional ByVal title As String = "")
        Using dCopy As New dlgCopyFiles
            dCopy.Show()
            dCopy.ProgressBar1.Style = ProgressBarStyle.Marquee
            dCopy.Text = title
            dCopy.Label1.Text = Path.GetFileNameWithoutExtension(src)
            bwCopyDirectory.WorkerReportsProgress = True
            bwCopyDirectory.WorkerSupportsCancellation = True
            bwCopyDirectory.RunWorkerAsync(New Arguments With {.src = src, .dst = dst})
            While bwCopyDirectory.IsBusy
                Application.DoEvents()
            End While
        End Using
    End Sub
    Sub DirectoryMove(ByVal src As String, ByVal dst As String, Optional ByVal title As String = "")
        Using dCopy As New dlgCopyFiles
            dCopy.Show()
            dCopy.ProgressBar1.Style = ProgressBarStyle.Marquee
            dCopy.Text = title
            dCopy.Label1.Text = Path.GetFileNameWithoutExtension(src)
            bwCopyDirectory.WorkerReportsProgress = True
            bwCopyDirectory.WorkerSupportsCancellation = True
            bwCopyDirectory.RunWorkerAsync(New Arguments With {.src = src, .dst = dst})
            While bwCopyDirectory.IsBusy
                Application.DoEvents()
            End While
            Directory.Delete(src, True)
        End Using
    End Sub

    Private Sub bwCopyDirectory_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCopyDirectory.DoWork
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        _DirectoryCopy(Args.src, Args.dst)
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

        MyMenuSep.Visible = (eSettings.ModuleSettings.Count > 0)
        MyMenu.Visible = (eSettings.ModuleSettings.Count > 0)
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
        eSettings.ModuleSettings.Clear()
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
    Private Sub _DirectoryCopy(ByVal sourceDirName As String, ByVal destDirName As String)
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

    Public Function RunGeneric(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object)) As Interfaces.ModuleResult Implements Interfaces.EmberExternalModule.RunGeneric
        Return New Interfaces.ModuleResult With {.breakChain = False}
    End Function
End Class

