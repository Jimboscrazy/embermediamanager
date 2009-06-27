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

Public Class dlgTrailer

    Private cTrailer As New Trailers
    Dim tArray As New ArrayList
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
        Me.Cancel_Button.Enabled = False
        Me.btnSetNfo.Enabled = False
        Me.btnPlayTrailer.Enabled = False

        Me.lblStatus.Text = "Downloading selected trailer..."
        Me.pbStatus.Style = ProgressBarStyle.Continuous
        Me.pbStatus.Value = 0
        Me.pnlStatus.Visible = True
        Application.DoEvents()

        If Not String.IsNullOrEmpty(Me.prePath) AndAlso File.Exists(Me.prePath) Then
            tURL = Path.Combine(Directory.GetParent(Me.sPath).FullName, Path.GetFileName(Me.prePath))
            Master.MoveFileWithStream(Me.prePath, tURL)

            File.Delete(Me.prePath)
        Else
            Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
            Me.bwDownloadTrailer.WorkerReportsProgress = True
            Me.bwDownloadTrailer.RunWorkerAsync(New Arguments With {.iIndex = lbTrailers.SelectedIndex, .bType = True})
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
    End Sub

    Private Sub bwDownloadTrailer_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadTrailer.DoWork

        Dim Args As Arguments = e.Argument
        Try
            If Args.bType Then
                tURL = cTrailer.DownloadSelectedTrailer(Me.sPath, Args.iIndex)
            Else
                Dim sHTTP As New HTTP
                Me.prePath = sHTTP.DownloadFile(Args.Parameter, Path.Combine(Master.TempPath, Path.GetFileName(Me.sPath)), True)
                sHTTP = Nothing
            End If
        Catch
        End Try

        e.Result = Args.bType
    End Sub

    Private Sub bwDownloadTrailer_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDownloadTrailer.ProgressChanged
        pbStatus.Value = e.ProgressPercentage
    End Sub

    Private Sub bwDownloadTrailer_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadTrailer.RunWorkerCompleted

        If e.Result Then
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
        AddHandler cTrailer.ProgressUpdated, AddressOf DownloadProgressUpdated
    End Sub

    Private Sub dlgTrailer_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()

        Me.pnlStatus.Visible = True
        Me.bwCompileList = New System.ComponentModel.BackgroundWorker
        Me.bwCompileList.WorkerSupportsCancellation = True
        Me.bwCompileList.RunWorkerAsync()
    End Sub

    Private Sub btnPlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlayTrailer.Click
        Try
            Me.btnPlayTrailer.Enabled = False
            Me.OK_Button.Enabled = False
            Me.btnSetNfo.Enabled = False
            Me.Cancel_Button.Enabled = False

            Me.lblStatus.Text = "Downloading preview..."
            Me.pbStatus.Style = ProgressBarStyle.Continuous
            Me.pbStatus.Value = 0
            Me.pnlStatus.Visible = True
            Application.DoEvents()

            Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
            Me.bwDownloadTrailer.WorkerReportsProgress = True
            Me.bwDownloadTrailer.RunWorkerAsync(New Arguments With {.Parameter = Me.lbTrailers.SelectedItem.ToString, .iIndex = lbTrailers.SelectedIndex, .bType = False})
        Catch
            MsgBox("The trailer could not be played. This could be due to an invalid URI or you do not have the proper player to play the trailer type.", MsgBoxStyle.Critical, "Error Playing Trailer")
        End Try
    End Sub

End Class
