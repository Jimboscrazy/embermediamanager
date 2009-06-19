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
    Dim tPath As String = String.Empty
    Dim prePath As String = String.Empty
    Friend WithEvents bwDownloadTrailer As New System.ComponentModel.BackgroundWorker

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.OK_Button.Enabled = False
        Me.Cancel_Button.Enabled = False

        Me.lblStatus.Text = "Downloading selected trailer..."
        Me.pnlStatus.Visible = True
        Application.DoEvents()

        If Not String.IsNullOrEmpty(Me.prePath) AndAlso File.Exists(Me.prePath) Then
            tPath = Path.Combine(Directory.GetParent(Me.sPath).FullName, Path.GetFileName(Me.prePath))
            Master.MoveFileWithStream(Me.prePath, tPath)

            File.Delete(Me.prePath)
        Else
            tPath = cTrailer.DownloadSelectedTrailer(Me.sPath, lbTrailers.SelectedIndex)
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub bwDownloadTrailer_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadTrailer.DoWork

        Try
            tArray = cTrailer.GetTrailers(Me.imdbID, False)
        Catch
        End Try

    End Sub

    Private Sub bwDownloadTrailer_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadTrailer.RunWorkerCompleted

        If Me.tArray.Count > 0 Then
            For Each tTrail As String In Me.tArray
                Me.lbTrailers.Items.Add(tTrail)
            Next
        End If

        Me.pnlStatus.Visible = False
        Me.Cancel_Button.Enabled = True
    End Sub

    Private Sub lbTrailers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbTrailers.SelectedIndexChanged
        Me.OK_Button.Enabled = True
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
            Return Me.tPath
        Else
            Return String.Empty
        End If

    End Function

    Private Sub dlgTrailer_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.pnlStatus.Visible = True
        Me.bwDownloadTrailer = New System.ComponentModel.BackgroundWorker
        Me.bwDownloadTrailer.WorkerSupportsCancellation = True
        Me.bwDownloadTrailer.RunWorkerAsync()
    End Sub

    Private Sub btnPlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlayTrailer.Click
        Try
            Me.btnPlayTrailer.Enabled = False
            Me.OK_Button.Enabled = False
            Me.Cancel_Button.Enabled = False

            Me.lblStatus.Text = "Downloading preview..."
            Me.pnlStatus.Visible = True
            Application.DoEvents()

            Dim sHTTP As New HTTP(Me.lbTrailers.SelectedItem.ToString, Path.Combine(Master.TempPath, Path.GetFileName(Me.sPath)))

            Me.pnlStatus.Visible = False
            Me.btnPlayTrailer.Enabled = True
            Me.OK_Button.Enabled = True
            Me.Cancel_Button.Enabled = True

            Me.prePath = sHTTP.SavePath

            If Not String.IsNullOrEmpty(prePath) Then System.Diagnostics.Process.Start(String.Concat("""", prePath, """"))

            sHTTP = Nothing
        Catch
            MsgBox("The trailer could not be played. This could be due to an invalid URI or you do not have the proper player to play the trailer type.", MsgBoxStyle.Critical, "Error Playing Trailer")
        End Try
    End Sub
End Class
