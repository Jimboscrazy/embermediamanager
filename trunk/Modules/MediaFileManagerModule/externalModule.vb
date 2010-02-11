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

Public Interface EmberExternalModule
    Sub Enable()
    Sub Disable()
    Sub Setup()
    Sub Init(ByRef emm As Object)
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
End Interface

Public Class FileManagerExternalModule
    Implements EmberExternalModule
    Dim emmAPI As New Object
    Private _Name As String = "Media File Manager"
    Private _Version As String = "0.1"

    Private MyPath As String
    Sub Setup() Implements EmberExternalModule.Setup
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
        PopulateFolders()
    End Sub
    Sub Enable() Implements EmberExternalModule.Enable
        MyMenu.Text = "Media File Manager"
        MySubMenu.Text = "Move To"
        MyMenu.DropDownItems.Add(MySubMenu)
        emmAPI.MenuMediaList.Items.Add(MyMenuSep)
        emmAPI.MenuMediaList.Items.Add(MyMenu)

        PopulateFolders()
    End Sub
    Sub Disable() Implements EmberExternalModule.Disable
        emmAPI.MenuMediaList.Items.Remove(MyMenuSep)
        emmAPI.MenuMediaList.Items.Remove(MyMenu)
    End Sub
    Sub Init(ByRef emm As Object) Implements EmberExternalModule.Init
        emmAPI = emm
        MyPath = Path.Combine(emmAPI.AppPath, "Modules")
        Load()
    End Sub

    ReadOnly Property ModuleName() As String Implements EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberExternalModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property
    Dim MyMenuSep As New System.Windows.Forms.ToolStripSeparator
    Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim FolderSubMenus As New List(Of System.Windows.Forms.ToolStripMenuItem)
    Private Sub MySubMenuItem_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MySubMenu.MouseHover

    End Sub
    Private Sub FolderSubMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles FolderSubMenus.Click
        Try
            Dim ItemsToWork As New List(Of IO.FileSystemInfo)
            Dim MoviesToWork As New List(Of Long)
            Dim MovieId As Int64 = -1

            For Each sRow As DataGridViewRow In emmAPI.MediaList.SelectedRows
                MovieId = Convert.ToInt64(sRow.Cells(0).Value)
                If Not MoviesToWork.Contains(MovieId) Then
                    MoviesToWork.Add(MovieId)
                End If
            Next
            If MoviesToWork.Count > 0 Then
                Dim mMovie As New Object
                For Each Id As Long In MoviesToWork
                    mMovie = emmAPI.DB.LoadMovieFromDB(Id)
                    ItemsToWork = emmAPI.FileDelete.GetItemsToDelete(False, mMovie)
                    'Dim dPath As String = mMovie.Filename
                    'Dim sPathShort As String = Directory.GetParent(dPath).FullName
                    MsgBox("Move from " + ItemsToWork(0).ToString + " To " + Path.Combine(sender.tag, Path.GetFileName(ItemsToWork(0).ToString)), MsgBoxStyle.Information, "Move")
                Next
            End If

        Catch ex As Exception
            'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Sub PopulateFolders()
        FolderSubMenus.RemoveAll(Function(b) True)
        For Each e In eSettings.ModuleSettings
            Dim FolderSubMenuItem As New System.Windows.Forms.ToolStripMenuItem
            FolderSubMenuItem.Text = e.Name
            FolderSubMenuItem.Tag = e.FolderPath
            FolderSubMenus.Add(FolderSubMenuItem)
            AddHandler FolderSubMenuItem.Click, AddressOf Me.FolderSubMenuItem_Click
        Next
        MySubMenu.DropDownItems.Clear()
        For Each i In FolderSubMenus
            MySubMenu.DropDownItems.Add(i)
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
End Class
