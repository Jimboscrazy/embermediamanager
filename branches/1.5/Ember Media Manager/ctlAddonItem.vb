﻿Imports System.IO

Public Class AddonItem
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker

    Public Event NeedsRefresh()

    Private _enabled As Boolean = True

    Private _id As Integer
    Private _addonname As String
    Private _author As String
    Private _summary As String
    Private _category As String
    Private _screenshot As Image
    Private _version As Single
    Private _mineversion As Single
    Private _maxeversion As Single
    Private _filelist As Generic.SortedList(Of String, String)
    Private _owned As Boolean

    Public Property ID() As Integer
        Get
            Return Me._id
        End Get
        Set(ByVal value As Integer)
            Me._id = value
        End Set
    End Property

    Public Property AddonName() As String
        Get
            Return Me._addonname
        End Get
        Set(ByVal value As String)
            Me._addonname = value
            Me.lblName.Text = value
        End Set
    End Property

    Public Property Author() As String
        Get
            Return Me._author
        End Get
        Set(ByVal value As String)
            Me._author = value
            Me.lblAuthor.Text = value
        End Set
    End Property

    Public Property Summary() As String
        Get
            Return Me._summary
        End Get
        Set(ByVal value As String)
            Me._summary = value
            Me.lblSummary.Text = value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return Me._category
        End Get
        Set(ByVal value As String)
            Me._category = value
        End Set
    End Property

    Public Property ScreenShot() As Image
        Get
            Return Me._screenshot
        End Get
        Set(ByVal value As Image)
            Me._screenshot = value
            Me.pbScreenShot.Image = value
        End Set
    End Property

    Public Property Version() As Single
        Get
            Return Me._version
        End Get
        Set(ByVal value As Single)
            Me._version = value
            Me.lblVersionNumber.Text = value.ToString
        End Set
    End Property

    Public Property MinEVersion() As Single
        Get
            Return Me._mineversion
        End Get
        Set(ByVal value As Single)
            Me._mineversion = value
        End Set
    End Property

    Public Property MaxEVersion() As Single
        Get
            Return Me._maxeversion
        End Get
        Set(ByVal value As Single)
            Me._maxeversion = value
        End Set
    End Property

    Public Property FileList() As Generic.SortedList(Of String, String)
        Get
            Return Me._filelist
        End Get
        Set(ByVal value As Generic.SortedList(Of String, String))
            Me._filelist = value
        End Set
    End Property

    Public Property Owned() As Boolean
        Get
            Return Me._owned
        End Get
        Set(ByVal value As Boolean)
            Me._owned = value
            If value Then
                Me.btnDelete.Visible = True
                Me.btnEdit.Visible = True
            End If
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._id = -1
        Me._addonname = String.Empty
        Me._author = String.Empty
        Me._summary = String.Empty
        Me._category = String.Empty
        Me._version = -1
        Me._mineversion = -1
        Me._maxeversion = -1
        Me._screenshot = Nothing
        Me._owned = False
        Me._filelist = New Generic.SortedList(Of String, String)
    End Sub

    Private Sub ViewFileListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewFileListToolStripMenuItem.Click
        Using dFileList As New dlgAddonFiles
            dFileList.ShowDialog(Me._filelist)
        End Using
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Me.ControlsEnabled(False)

        Me.pbStatus.Maximum = Me._filelist.Count
        Me.pbStatus.Value = 0
        Me.pnlStatus.Visible = True

        Me.bwDownload = New System.ComponentModel.BackgroundWorker
        Me.bwDownload.WorkerReportsProgress = True
        Me.bwDownload.RunWorkerAsync()
    End Sub

    Private Sub bwDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownload.DoWork
        Dim sHTTP As New HTTP

        For Each _file As KeyValuePair(Of String, String) In Me._filelist
            sHTTP.DownloadFile(String.Format("http://www.embermm.com/Addons/Files/{0}/{1}", Me._addonname.Replace(" ", "_"), _file.Key), Path.Combine(Master.TempPath, _file.Key), False, "other")
            Me.bwDownload.ReportProgress(1)
        Next

        sHTTP = Nothing
    End Sub

    Private Sub bwDownload_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDownload.ProgressChanged
        Me.pbStatus.Value += 1
    End Sub

    Private Sub bwDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownload.RunWorkerCompleted
        Me.pnlStatus.Visible = False
        Me.ControlsEnabled(True)
    End Sub

    Private Sub ControlsEnabled(ByVal isEnabled As Boolean)
        Me._enabled = isEnabled
        Me.btnDelete.Enabled = isEnabled
        Me.btnDownload.Enabled = isEnabled
        Me.btnEdit.Enabled = isEnabled
    End Sub

    Private Sub AddonItem_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then Me.ContextMenuStrip = If(Not Me._enabled, Nothing, Me.cMenu)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Using dDelAddon As New dlgDeleteAddon
            If dDelAddon.ShowDialog(Me._id) = DialogResult.OK Then
                RaiseEvent NeedsRefresh()
            End If
        End Using
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim tAddon As New Containers.Addon
        tAddon.ID = Me._id
        tAddon.Name = Me._addonname
        tAddon.Author = Me._author
        tAddon.Description = Me._summary
        tAddon.Version = Me._version
        tAddon.MinEVersion = Me._mineversion
        tAddon.MaxEVersion = Me._maxeversion
        tAddon.Category = Me._category
        tAddon.Files = Me._filelist
        tAddon.ScreenShotImage = Me._screenshot

        Using dEditAddon As New dlgAddEditAddon
            Dim eAddon As Containers.Addon = dEditAddon.ShowDialog(tAddon)
            If Not IsNothing(eAddon) Then
                ''do upload
            End If
        End Using
    End Sub
End Class