Imports System.IO

Public Class AddonItem
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker

    Private _enabled As Boolean = True

    Private _addonname As String
    Private _author As String
    Private _summary As String
    Private _screenshot As Image
    Private _version As String
    Private _filelist As Generic.SortedList(Of String, String)

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

    Public Property ScreenShot() As Image
        Get
            Return Me._screenshot
        End Get
        Set(ByVal value As Image)
            Me._screenshot = value
            Me.pbScreenShot.Image = value
        End Set
    End Property

    Public Property Version() As String
        Get
            Return Me._version
        End Get
        Set(ByVal value As String)
            Me._version = value
            Me.lblVersionNumber.Text = value
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

    Public Sub New()
        InitializeComponent()
        Me.Clear()
    End Sub

    Public Sub Clear()
        Me._addonname = String.Empty
        Me._author = String.Empty
        Me._summary = String.Empty
        Me._version = String.Empty
        Me._screenshot = Nothing
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
End Class
