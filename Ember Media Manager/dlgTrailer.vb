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
Imports System.Text.RegularExpressions

Public Class dlgTrailer

    Private cTrailer As New Trailers
    Private sHTTP As New HTTP
    Dim tArray As New List(Of String)
    Dim imdbID As String = String.Empty
    Dim sPath As String = String.Empty
    Dim tURL As String = String.Empty
    Dim prePath As String = String.Empty
    Friend WithEvents bwCompileList As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwDownloadTrailer As New System.ComponentModel.BackgroundWorker

    Private Structure Arguments
        Dim Parameter As String
        Dim iIndex As Integer
        Dim bType As Boolean
    End Structure

    Private Sub btnSetNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetNfo.Click
        tURL = lbTrailers.SelectedItem.ToString

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.OK_Button.Enabled = False
        Me.btnFetchFormats.Enabled = False
        Me.Cancel_Button.Enabled = False
        Me.btnSetNfo.Enabled = False
        Me.btnPlayTrailer.Enabled = False
        Me.lbTrailers.Enabled = False
        Me.txtYouTube.Enabled = False
        Me.lblStatus.Text = Master.eLang.GetString(380, "Downloading selected trailer...")
        Me.pbStatus.Style = ProgressBarStyle.Continuous
        Me.pbStatus.Value = 0
        Me.pnlStatus.Visible = True
        Application.DoEvents()

        If Me.txtManual.Text.Length > 0 AndAlso Master.eSettings.ValidExts.Contains(Path.GetExtension(Me.txtManual.Text)) AndAlso File.Exists(Me.txtManual.Text) Then
            Me.tURL = Path.Combine(Directory.GetParent(Me.sPath).FullName, String.Concat(Path.GetFileNameWithoutExtension(Me.sPath), If(Master.eSettings.DashTrailer, "-trailer", "[trailer]"), Path.GetExtension(Me.txtManual.Text)))
            FileManip.Common.MoveFileWithStream(Me.txtManual.Text, Me.tURL)

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf Regex.IsMatch(Me.txtYouTube.Text, "http:\/\/.*youtube.*\/watch\?v=(.{11})&?.*") AndAlso lstFormats.SelectedItem IsNot Nothing Then
            Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
            Me.bwDownloadTrailer.WorkerReportsProgress = True
            Me.bwDownloadTrailer.RunWorkerAsync(New Arguments With {.Parameter = DirectCast(lstFormats.SelectedItem, YouTube.VideoLinkItem).URL, .iIndex = -1, .bType = True})
        Else
            If Not String.IsNullOrEmpty(Me.prePath) AndAlso File.Exists(Me.prePath) Then
                Me.tURL = Path.Combine(Directory.GetParent(Me.sPath).FullName, Path.GetFileName(Me.prePath))
                FileManip.Common.MoveFileWithStream(Me.prePath, Me.tURL)

                File.Delete(Me.prePath)

                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
                Me.bwDownloadTrailer.WorkerReportsProgress = True
                Me.bwDownloadTrailer.RunWorkerAsync(New Arguments With {.iIndex = lbTrailers.SelectedIndex, .bType = True})
            End If
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub bwCompileList_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCompileList.DoWork

        Try
            tArray = cTrailer.GetTrailers(Me.imdbID, False)
        Catch
        End Try

    End Sub

    Private Sub bwCompileList_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCompileList.RunWorkerCompleted

        If Me.tArray.Count > 0 Then
            For Each tTrail As String In Me.tArray
                Me.lbTrailers.Items.Add(tTrail)
            Next
        End If

        Me.pnlStatus.Visible = False
        Me.Cancel_Button.Enabled = True
        DoEnableFetchFormats()
    End Sub

    Private Sub bwDownloadTrailer_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadTrailer.DoWork

        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        Try
            If Args.bType AndAlso Args.iIndex >= 0 Then
                tURL = cTrailer.DownloadSelectedTrailer(Me.sPath, Args.iIndex)
            Else
                If Args.bType Then
                    tURL = cTrailer.DownloadYouTubeTrailer(Me.sPath, Args.Parameter)
                Else
                    Me.prePath = sHTTP.DownloadFile(Args.Parameter, Path.Combine(Master.TempPath, Path.GetFileName(Me.sPath)), True, "trailer")
                End If
            End If
        Catch
        End Try

        e.Result = Args.bType
    End Sub

    Private Sub bwDownloadTrailer_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDownloadTrailer.ProgressChanged
        pbStatus.Value = e.ProgressPercentage
    End Sub

    Private Sub bwDownloadTrailer_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadTrailer.RunWorkerCompleted

        If Convert.ToBoolean(e.Result) Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            Me.pnlStatus.Visible = False
            Me.btnPlayTrailer.Enabled = True
            Me.OK_Button.Enabled = True
            Me.btnSetNfo.Enabled = True
            Me.Cancel_Button.Enabled = True

            If Not String.IsNullOrEmpty(prePath) Then System.Diagnostics.Process.Start(String.Concat("""", prePath, """"))
        End If
    End Sub

    Private Sub DownloadProgressUpdated(ByVal iProgress As Integer)
        bwDownloadTrailer.ReportProgress(iProgress)
    End Sub

    Private Sub lbTrailers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbTrailers.SelectedIndexChanged
        Me.OK_Button.Enabled = True
        Me.btnSetNfo.Enabled = True
        Me.btnPlayTrailer.Enabled = True

        If File.Exists(Me.prePath) Then
            File.Delete(Me.prePath)
        End If
        Me.prePath = String.Empty
    End Sub

    Public Overloads Function ShowDialog(ByVal _imdbID As String, ByVal _sPath As String) As String

        '//
        ' Overload to pass data
        '\\

        Me.imdbID = _imdbID
        Me.sPath = _sPath

        If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return Me.tURL
        Else
            Return String.Empty
        End If

    End Function

    Private Sub dlgTrailer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()
        AddHandler cTrailer.ProgressUpdated, AddressOf DownloadProgressUpdated
        AddHandler sHTTP.ProgressUpdated, AddressOf DownloadProgressUpdated
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(372, "Select Trailer")
        Me.OK_Button.Text = Master.eLang.GetString(373, "Download")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.GroupBox1.Text = Master.eLang.GetString(374, "Select Trailer to Download")
        Me.GroupBox2.Text = Master.eLang.GetString(375, "Manual Trailer Entry")
        Me.Label1.Text = Master.eLang.GetString(376, "YouTube URL:")
        Me.lblStatus.Text = Master.eLang.GetString(377, "Compiling trailer list...")
        Me.btnPlayTrailer.Text = Master.eLang.GetString(378, "Preview Trailer")
        Me.btnSetNfo.Text = Master.eLang.GetString(379, "Set To Nfo")
        Me.btnFetchFormats.Text = Master.eLang.GetString(645, "Fetch Formats")
        Me.Label2.Text = Master.eLang.GetString(644, "Local Trailer:")
    End Sub

    Private Sub dlgTrailer_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()

        Me.btnFetchFormats.Enabled = False
        Me.pnlStatus.Visible = True
        Me.bwCompileList = New System.ComponentModel.BackgroundWorker
        Me.bwCompileList.WorkerSupportsCancellation = True
        Me.bwCompileList.RunWorkerAsync()
    End Sub

    Private Sub btnPlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlayTrailer.Click
        Try
            Me.btnPlayTrailer.Enabled = False
            Me.OK_Button.Enabled = False
            Me.btnFetchFormats.Enabled = False
            Me.btnSetNfo.Enabled = False
            Me.Cancel_Button.Enabled = False

            Me.lblStatus.Text = Master.eLang.GetString(381, "Downloading preview...")
            Me.pbStatus.Style = ProgressBarStyle.Continuous
            Me.pbStatus.Value = 0
            Me.pnlStatus.Visible = True
            Application.DoEvents()

            Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
            Me.bwDownloadTrailer.WorkerReportsProgress = True
            Me.bwDownloadTrailer.RunWorkerAsync(New Arguments With {.Parameter = Me.lbTrailers.SelectedItem.ToString, .iIndex = lbTrailers.SelectedIndex, .bType = False})
        Catch
            MsgBox(Master.eLang.GetString(382, "The trailer could not be played. This could be due to an invalid URI or you do not have the proper player to play the trailer type."), MsgBoxStyle.Critical, Master.eLang.GetString(383, "Error Playing Trailer"))
        End Try
    End Sub

#Region " YouTube Stuff "

    Private WithEvents YouTube As YouTube.Scraper

    Private Sub FetchFormatsProgressUpdated(ByVal iProgress As Integer)
        pbStatus.Value = iProgress
    End Sub

    Private Sub txtYouTube_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtYouTube.TextChanged
        Try
            DoEnableFetchFormats()
            lstFormats.DataSource = Nothing
        Catch
        End Try
    End Sub

    Private Sub DoEnableFetchFormats()
        If bwCompileList.IsBusy OrElse bwDownloadTrailer.IsBusy Then
            Me.btnFetchFormats.Enabled = False
        Else
            Me.btnFetchFormats.Enabled = Regex.IsMatch(Me.txtYouTube.Text, "http:\/\/.*youtube.*\/watch\?v=(.{11})&?.*")
        End If
    End Sub

    Private Sub lstFormats_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFormats.SelectedIndexChanged
        Try
            Me.OK_Button.Enabled = True
            Me.btnSetNfo.Enabled = True
            Me.btnPlayTrailer.Enabled = True

            If File.Exists(Me.prePath) Then
                File.Delete(Me.prePath)
            End If
            Me.prePath = String.Empty
        Catch
        End Try
    End Sub

    Private Sub btnFetchFormats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFetchFormats.Click

        Try

            lstFormats.DataSource = Nothing
            Me.btnPlayTrailer.Enabled = False
            Me.Cancel_Button.Enabled = False
            Application.DoEvents()

            YouTube = New YouTube.Scraper
            YouTube.GetVideoLinksAsync(txtYouTube.Text)

            Me.OK_Button.Enabled = False
            Me.btnFetchFormats.Enabled = False
            Me.btnSetNfo.Enabled = False

            Me.lblStatus.Text = Master.eLang.GetString(646, "Retrieving formats...")
            Me.pbStatus.Style = ProgressBarStyle.Marquee
            Me.pbStatus.Value = 0
            Me.pnlStatus.Visible = True

        Catch ex As Exception
            MsgBox(Master.eLang.GetString(647, "The video format links could not be retrieved."), MsgBoxStyle.Critical, Master.eLang.GetString(648, "Error Retrieving Video Format Links"))
        End Try

    End Sub

    Private Sub YouTube_VideoLinksRetrieved(ByVal bSuccess As Boolean) Handles YouTube.VideoLinksRetrieved
        Try

            If bSuccess Then
                lstFormats.DataSource = YouTube.VideoLinks.Values.ToList
                lstFormats.DisplayMember = "Description"
                lstFormats.ValueMember = "URL"

                If YouTube.VideoLinks.ContainsKey(Master.eSettings.PreferredTrailerQuality) Then
                    lstFormats.SelectedIndex = YouTube.VideoLinks.IndexOfKey(Master.eSettings.PreferredTrailerQuality)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        Finally
            Me.pnlStatus.Visible = False
            Me.Cancel_Button.Enabled = True
        End Try

    End Sub

#End Region

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            With ofdTrailer
                .InitialDirectory = Directory.GetParent(Master.currMovie.Filename).FullName
                .Filter = String.Concat("Supported Trailer Formats|*", Master.ListToStringWithSeparator(Master.eSettings.ValidExts.ToArray(), ";*"))
                .FilterIndex = 0
            End With

            If ofdTrailer.ShowDialog() = DialogResult.OK Then
                txtManual.Text = ofdTrailer.FileName
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub txtManual_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtManual.TextChanged
        Me.OK_Button.Enabled = txtManual.Text.Length > 0
    End Sub

    Protected Overrides Sub Finalize()
        cTrailer = Nothing
        sHTTP = Nothing
        MyBase.Finalize()
    End Sub
End Class
