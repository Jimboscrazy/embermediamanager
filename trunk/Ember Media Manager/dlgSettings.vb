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
Imports System
Imports System.IO

Public Class dlgSettings

    Private doRefresh As Boolean = False
    Private didApply As Boolean = False

#Region "Form/Controls"

    Private XComs As List(Of emmSettings.XBMCCom)

    ' ########################################
    ' ############ FORMS/CONTROLS ############
    ' ########################################

    Private Sub btnInfoPanelText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfoPanelText.Click

        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnInfoPanelText.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub btnHeaderText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHeaderText.Click

        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnHeaderText.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnTopPanelText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopPanelText.Click
        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnTopPanelText.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnHeaders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHeaders.Click
        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnHeaders.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnBackground_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackground.Click
        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnBackground.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnInfoPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInfoPanel.Click
        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnInfoPanel.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnTopPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTopPanel.Click
        Try
            With Me.cdColor
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not .Color = Nothing Then
                        Me.btnTopPanel.BackColor = .Color
                        Me.btnApply.Enabled = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            Me.SaveSettings()
            Me.btnApply.Enabled = False
            If doRefresh Then didApply = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMovieAddFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFolder.Click
        Using dSource As New dlgMovieSource
            If dSource.ShowDialog = Windows.Forms.DialogResult.OK Then
                RefreshSources()
                Me.btnApply.Enabled = True
                Me.doRefresh = True
            End If
        End Using
    End Sub

    Private Sub btnMovieRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieRem.Click
        Try
            If Me.lvMovies.SelectedItems.Count > 0 Then
                If MsgBox("Are you sure you want to remove the selected sources? This will remove the movies from these sources from the Ember database.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Are You Sure?") = MsgBoxResult.Yes Then
                    Me.lvMovies.BeginUpdate()

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                            For i As Integer = lvMovies.SelectedItems.Count - 1 To 0 Step -1
                                parSource.Value = lvMovies.SelectedItems(i).SubItems(1).Text
                                SQLcommand.CommandText = String.Concat("DELETE FROM movies WHERE source = (?);")
                                SQLcommand.ExecuteNonQuery()
                                SQLcommand.CommandText = String.Concat("DELETE FROM sources WHERE name = (?);")
                                SQLcommand.ExecuteNonQuery()
                                lvMovies.Items.RemoveAt(lvMovies.SelectedItems(i).Index)
                            Next
                        End Using
                        SQLtransaction.Commit()
                    End Using

                    Me.lvMovies.Sort()
                    Me.lvMovies.EndUpdate()
                    Me.lvMovies.Refresh()
                    Me.btnApply.Enabled = True
                    Me.doRefresh = True
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = If(doRefresh, Windows.Forms.DialogResult.Retry, Windows.Forms.DialogResult.OK)
        Me.SaveSettings()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = If(didApply, Windows.Forms.DialogResult.Retry, Windows.Forms.DialogResult.Cancel)
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Activate()

            tvSettings.ExpandAll()
            tvSettings.SelectedNode = tvSettings.Nodes(0)

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            iBackground = New Bitmap(Me.pnlCurrent.Width, Me.pnlCurrent.Height)
            Using b As Graphics = Graphics.FromImage(iBackground)
                b.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlCurrent.ClientRectangle, Color.SteelBlue, Color.FromKnownColor(KnownColor.Control), Drawing2D.LinearGradientMode.Horizontal), pnlCurrent.ClientRectangle)
                Me.pnlCurrent.BackgroundImage = iBackground
            End Using

            Me.LoadGenreLangs()
            Me.FillSettings()

            Me.btnApply.Enabled = False
            Me.doRefresh = False
            Me.didApply = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnAddFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFilter.Click
        If Not String.IsNullOrEmpty(Me.txtFilter.Text) Then
            Me.lstFilters.Items.Add(Me.txtFilter.Text)
            Me.txtFilter.Text = String.Empty
            Me.btnApply.Enabled = True
            Me.doRefresh = True
        End If

        Me.txtFilter.Focus()
    End Sub

    Private Sub btnRemoveFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFilter.Click
        If Me.lstFilters.Items.Count > 0 AndAlso Me.lstFilters.SelectedItems.Count > 0 Then
            For Each i As Integer In lstFilters.SelectedIndices
                lstFilters.Items.RemoveAt(i)
            Next
            Me.btnApply.Enabled = True
            Me.doRefresh = True
        End If
    End Sub

    Private Sub chkStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScanMediaInfo.CheckedChanged
        Me.btnApply.Enabled = True
        Me.chkUseMIDuration.Enabled = Me.chkScanMediaInfo.Checked
        If Not Me.chkScanMediaInfo.Checked Then
            Me.chkUseMIDuration.Checked = False
            Me.gbRTFormat.Enabled = False
            Me.rbMins.Checked = True
        End If
    End Sub

    Private Sub cbCert_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCert.SelectedIndexChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCert.CheckedChanged
        Me.cbCert.SelectedIndex = -1
        Me.cbCert.Enabled = Me.chkCert.Checked
        Me.chkUseCertForMPAA.Enabled = Me.chkCert.Checked
        If Not Me.chkCert.Checked Then
            Me.chkUseCertForMPAA.Checked = False
        End If
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkFullCast_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFullCast.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkFullCrew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFullCrew.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieMediaCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMoviePosterCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMoviePosterCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieFanartCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieFanartCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieInfoCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieInfoCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieTrailerCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieTrailerCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanFolderJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieTBN.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieTBNb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieTBNb.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanFanartJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieFanartJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNFO.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieNFOb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNFOb.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseTMDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseTMDB.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseIMPA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseIMPA.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub cbPosterSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPosterSize.SelectedIndexChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub cbFanartSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFanartSize.SelectedIndexChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOverwritePoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwritePoster.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOverwriteFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteFanart.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLogErrors_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLogErrors.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseFolderNames_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub chkProperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProperCase.CheckedChanged
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            If Me.lstFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstFilters.SelectedItem) AndAlso Me.lstFilters.SelectedIndex > 0 Then
                Dim iIndex As Integer = Me.lstFilters.SelectedIndices(0)
                Me.lstFilters.Items.Insert(iIndex - 1, Me.lstFilters.SelectedItems(0))
                Me.lstFilters.Items.RemoveAt(iIndex + 1)
                Me.lstFilters.SelectedIndex = iIndex - 1
                Me.btnApply.Enabled = True
                Me.doRefresh = True
                Me.lstFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            If Me.lstFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstFilters.SelectedItem) AndAlso Me.lstFilters.SelectedIndex < (Me.lstFilters.Items.Count - 1) Then
                Dim iIndex As Integer = Me.lstFilters.SelectedIndices(0)
                Me.lstFilters.Items.Insert(iIndex + 2, Me.lstFilters.SelectedItems(0))
                Me.lstFilters.Items.RemoveAt(iIndex)
                Me.lstFilters.SelectedIndex = iIndex + 1
                Me.btnApply.Enabled = True
                Me.doRefresh = True
                Me.lstFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub chkTitleFromNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub chkUseMPDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseMPDB.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieTBN.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNameTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameTBN.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterTBN.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFolderJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFanartJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNameFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameFanartJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNFO.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNameNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameNFO.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieNameDotFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameDotFanartJPG.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub chkLockPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockPlot.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockOutline.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTitle.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkSingleScrapeImages_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSingleScrapeImages.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanPosterTBN.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanPosterJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanMovieNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNameJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanDotFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanDotFanartJPG.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtIP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar), True)
    End Sub

    Private Sub txtIP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtPort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPort.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPort.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOverwriteNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteNfo.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMarkNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMarkNew.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkResizeFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeFanart.CheckedChanged
        Me.btnApply.Enabled = True

        txtFanartWidth.Enabled = chkResizeFanart.Checked
        txtFanartHeight.Enabled = chkResizeFanart.Checked

        If Not chkResizeFanart.Checked Then
            txtFanartWidth.Text = String.Empty
            txtFanartHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtFanartWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFanartWidth.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtFanartHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFanartHeight.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtFanartWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFanartWidth.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtFanartHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFanartHeight.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkResizePoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizePoster.CheckedChanged
        Me.btnApply.Enabled = True

        txtPosterWidth.Enabled = chkResizePoster.Checked
        txtPosterHeight.Enabled = chkResizePoster.Checked

        If Not chkResizePoster.Checked Then
            txtPosterWidth.Text = String.Empty
            txtPosterHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtPosterWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPosterWidth.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtPosterHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPosterHeight.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtPosterWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPosterWidth.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtPosterHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPosterHeight.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOFDBPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBPlot.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOFDBOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBOutline.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOFDBTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBTitle.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseCertForMPAA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCertForMPAA.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtAutoThumbs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAutoThumbs.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtAutoThumbs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAutoThumbs.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkAutoThumbs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoThumbs.CheckedChanged
        Me.txtAutoThumbs.Enabled = Me.chkAutoThumbs.Checked
        Me.chkNoSpoilers.Enabled = Me.chkAutoThumbs.Checked
        Me.chkUseETasFA.Enabled = Me.chkAutoThumbs.Checked
        If Not chkAutoThumbs.Checked Then
            Me.txtAutoThumbs.Text = String.Empty
            Me.chkNoSpoilers.Checked = False
            Me.chkUseETasFA.Checked = False
        End If
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkScanRecursive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub chkCastWithImg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCastWithImg.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkVideoTSParent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVideoTSParent.CheckedChanged
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub tvSettings_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvSettings.AfterSelect
        lblCurrent.Text = tvSettings.SelectedNode.Text

        pnlGeneral.Visible = False
        pnlXBMCCom.Visible = False
        pnlMovies.Visible = False
        pnlSources.Visible = False
        pnlScraper.Visible = False
        pnlExtensions.Visible = False
        pnlImages.Visible = False

        Select Case tvSettings.SelectedNode.Name
            Case "nGeneral"
                pnlGeneral.Visible = True
            Case "nXBMCCom"
                pnlXBMCCom.Visible = True
            Case "nMovies"
                pnlMovies.Visible = True
            Case "nSources"
                pnlSources.Visible = True
            Case "nScraper"
                pnlScraper.Visible = True
            Case "nExts"
                pnlExtensions.Visible = True
            Case "nImages"
                pnlImages.Visible = True
        End Select
    End Sub

    Private Sub lbXBMCCom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbXBMCCom.SelectedIndexChanged
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        Me.txtName.Text = Me.XComs.Item(iSel).Name
        Me.txtIP.Text = Me.XComs.Item(iSel).IP
        Me.txtPort.Text = Me.XComs.Item(iSel).Port
        Me.txtUsername.Text = Me.XComs.Item(iSel).Username
        Me.txtPassword.Text = Me.XComs.Item(iSel).Password

        btnEditCom.Enabled = True
    End Sub

    Private Sub btnAddCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCom.Click
        If Not String.IsNullOrEmpty(txtName.Text) Then

            'have to iterate the list instead of using .comtains so we can convert each to lower case
            For i As Integer = 0 To lbXBMCCom.Items.Count - 1
                If lbXBMCCom.Items(i).ToString.ToLower = Me.txtName.Text.ToLower Then
                    MsgBox("The name you are attempting to use for this XBMC installation is already in use. Please choose another.", MsgBoxStyle.Exclamation, "Each name must be unique")
                    txtName.Focus()
                    Exit Sub
                End If
            Next

            If Not String.IsNullOrEmpty(txtIP.Text) Then
                If Not String.IsNullOrEmpty(txtPort.Text) Then
                    XComs.Add(New emmSettings.XBMCCom With {.Name = txtName.Text, .IP = txtIP.Text, .Port = txtPort.Text, .Username = txtUsername.Text, .Password = txtPassword.Text})
                    Me.LoadXComs()

                    Me.txtName.Text = String.Empty
                    Me.txtIP.Text = String.Empty
                    Me.txtPort.Text = String.Empty
                    Me.txtUsername.Text = String.Empty
                    Me.txtPassword.Text = String.Empty

                    Me.btnEditCom.Enabled = False
                    Me.btnApply.Enabled = True
                Else
                    MsgBox("You must enter a port for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter a Port")
                    txtPort.Focus()
                End If
            Else
                MsgBox("You must enter an IP for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter an IP")
                txtIP.Focus()
            End If
        Else
            MsgBox("You must enter a name for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter a Unique Name")
            txtName.Focus()
        End If

    End Sub

    Private Sub btnEditCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditCom.Click
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        If Not String.IsNullOrEmpty(txtName.Text) Then

            For i As Integer = 0 To lbXBMCCom.Items.Count - 1
                If Not iSel = i AndAlso lbXBMCCom.Items(i).ToString.ToLower = Me.txtName.Text.ToLower Then
                    MsgBox("The name you are attempting to use for this XBMC installation is already in use. Please choose another.", MsgBoxStyle.Exclamation, "Each name must be unique")
                    txtName.Focus()
                    Exit Sub
                End If
            Next

            If Not String.IsNullOrEmpty(txtIP.Text) Then
                If Not String.IsNullOrEmpty(txtPort.Text) Then

                    Me.XComs.Item(iSel).Name = Me.txtName.Text
                    Me.XComs.Item(iSel).IP = Me.txtIP.Text
                    Me.XComs.Item(iSel).Port = Me.txtPort.Text
                    Me.XComs.Item(iSel).Username = Me.txtUsername.Text
                    Me.XComs.Item(iSel).Password = Me.txtPassword.Text

                    btnEditCom.Enabled = False

                    Me.txtName.Text = String.Empty
                    Me.txtIP.Text = String.Empty
                    Me.txtPort.Text = String.Empty
                    Me.txtUsername.Text = String.Empty
                    Me.txtPassword.Text = String.Empty

                    Me.btnApply.Enabled = True
                Else
                    MsgBox("You must enter a port for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter a Port")
                    txtPort.Focus()
                End If
            Else
                MsgBox("You must enter an IP for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter an IP")
                txtIP.Focus()
            End If

        Else
            MsgBox("You must enter a name for this XBMC installation.", MsgBoxStyle.Exclamation, "Please Enter a Unique Name")
            txtName.Focus()
        End If

    End Sub

    Private Sub btnRemoveCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveCom.Click
        Me.XComs.RemoveAt(lbXBMCCom.SelectedIndex)
        Me.LoadXComs()
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOFDBGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBGenre.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkNoSpoilers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoSpoilers.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtIMDBURL_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIMDBURL.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCleanExtrathumbs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanExtrathumbs.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub btnAddMovieExt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMovieExt.Click
        If Not String.IsNullOrEmpty(txtMovieExt.Text) Then
            If Not Strings.Left(txtMovieExt.Text, 1) = "." Then txtMovieExt.Text = String.Concat(".", txtMovieExt.Text)
            If Not lstMovieExts.Items.Contains(txtMovieExt.Text.ToLower) Then
                lstMovieExts.Items.Add(txtMovieExt.Text.ToLower)
                Me.btnApply.Enabled = True
                Me.doRefresh = True
                txtMovieExt.Text = String.Empty
                txtMovieExt.Focus()
            End If
        End If
    End Sub

    Private Sub btnRemMovieExt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemMovieExt.Click
        If lstMovieExts.Items.Count > 0 And lstMovieExts.SelectedItems.Count > 0 Then
            For Each i As Integer In lstMovieExts.SelectedIndices
                lstMovieExts.Items.RemoveAt(i)
            Next
            Me.btnApply.Enabled = True
            Me.doRefresh = True
        End If
    End Sub

    Private Sub chkUpdates_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUpdates.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockGenre.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockRealStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockRealStudio.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockRating.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkLockTagline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTagline.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With Me.fbdBrowse
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not String.IsNullOrEmpty(.SelectedPath.ToString) AndAlso Directory.Exists(.SelectedPath) Then
                    Me.txtBDPath.Text = .SelectedPath.ToString
                End If
            End If
        End With
    End Sub

    Private Sub chkAutoBD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoBD.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtBDPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBDPath.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseMIDuration_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseMIDuration.CheckedChanged
        Me.btnApply.Enabled = True
        Me.gbRTFormat.Enabled = Me.chkUseMIDuration.Checked
        If Not chkUseMIDuration.Checked Then
            Me.rbMins.Checked = True
        End If
    End Sub

    Private Sub chkMovieSubCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieSubCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkMovieExtraCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieExtraCol.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkPersistImgCache_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPersistImgCache.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseImgCache_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseImgCache.CheckedChanged
        Me.chkPersistImgCache.Enabled = Me.chkUseImgCache.Checked
        Me.chkUseImgCacheUpdaters.Enabled = Me.chkUseImgCache.Checked
        If Not Me.chkUseImgCache.Checked Then
            Me.chkPersistImgCache.Checked = False
            Me.chkUseImgCacheUpdaters.Checked = False
        End If
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseImgCacheUpdaters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseImgCacheUpdaters.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtSkipLessThan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSkipLessThan.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtSkipLessThan_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSkipLessThan.TextChanged
        Me.btnApply.Enabled = True
        Me.doRefresh = True
    End Sub

    Private Sub chkLockTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkDownloadTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDownloadTrailer.CheckedChanged
        Me.btnApply.Enabled = True
        Me.chkUpdaterTrailer.Enabled = Me.chkDownloadTrailer.Checked
        Me.txtTimeout.Enabled = Me.chkDownloadTrailer.Checked
        Me.lbTrailerSites.Enabled = Me.chkDownloadTrailer.Checked
        Me.chkSingleScrapeTrailer.Enabled = Me.chkDownloadTrailer.Checked
        Me.chkOverwriteTrailer.Enabled = Me.chkDownloadTrailer.Checked
        Me.chkNoDLTrailer.Enabled = Me.chkDownloadTrailer.Checked
        Me.chkDeleteAllTrailers.Enabled = Me.chkDownloadTrailer.Checked

        If Not Me.chkDownloadTrailer.Checked Then
            Me.chkUpdaterTrailer.Checked = False
            Me.chkSingleScrapeTrailer.Checked = False
            Me.chkNoDLTrailer.Checked = False
            Me.chkOverwriteTrailer.Checked = False
            Me.chkDeleteAllTrailers.Checked = False
            Me.txtTimeout.Text = "2"
            For i As Integer = 0 To lbTrailerSites.Items.Count - 1
                lbTrailerSites.SetItemChecked(i, False)
            Next
        End If
    End Sub

    Private Sub lbTrailerSites_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lbTrailerSites.ItemCheck
        Me.btnApply.Enabled = True
    End Sub

    Private Sub lbGenre_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lbGenre.ItemCheck
        If e.Index = 0 Then
            For i As Integer = 1 To lbGenre.Items.Count - 1
                Me.lbGenre.SetItemChecked(i, False)
            Next
        Else
            Me.lbGenre.SetItemChecked(0, False)
        End If
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUpdaterTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUpdaterTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtTimeout_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTimeout.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtTimeout_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTimeout.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkSingleScrapeTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSingleScrapeTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub tbPosterQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPosterQual.ValueChanged
        Me.btnApply.Enabled = True
        Me.lblPosterQual.Text = tbPosterQual.Value.ToString
    End Sub

    Private Sub tbFanartQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFanartQual.ValueChanged
        Me.btnApply.Enabled = True
        Me.lblFanartQual.Text = tbFanartQual.Value.ToString
    End Sub

    Private Sub chkFanartOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFanartOnly.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkNoDLTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDLTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkNoSaveImagesToNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoSaveImagesToNfo.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub tcCleaner_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcCleaner.SelectedIndexChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkWhitelistVideo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWhitelistVideo.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub btnAddWhitelist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWhitelist.Click
        If Not String.IsNullOrEmpty(Me.txtWhitelist.Text) Then
            If Not Strings.Left(txtWhitelist.Text, 1) = "." Then txtWhitelist.Text = String.Concat(".", txtWhitelist.Text)
            If Not lstWhitelist.Items.Contains(txtWhitelist.Text.ToLower) Then
                lstWhitelist.Items.Add(txtWhitelist.Text.ToLower)
                Me.btnApply.Enabled = True
                txtWhitelist.Text = String.Empty
                txtWhitelist.Focus()
            End If
        End If
    End Sub

    Private Sub btnRemoveWhitelist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveWhitelist.Click
        If lstWhitelist.Items.Count > 0 And lstWhitelist.SelectedItems.Count > 0 Then
            For Each i As Integer In lstWhitelist.SelectedIndices
                lstWhitelist.Items.RemoveAt(i)
            Next
            Me.btnApply.Enabled = True
        End If
    End Sub

    Private Sub chkInfoPanelAnim_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInfoPanelAnim.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOverwriteTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkShowDims_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowDims.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub
    Private Sub chkShowGenresText_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowGenresText.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub
    Private Sub chkNoDisplayPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDisplayPoster.CheckedChanged
        Me.btnApply.Enabled = True
        If Me.chkNoDisplayFanart.Checked AndAlso Me.chkNoDisplayPoster.Checked Then
            Me.chkShowDims.Enabled = False
            Me.chkShowDims.Checked = False
        Else
            Me.chkShowDims.Enabled = True
        End If
    End Sub

    Private Sub chkNoDisplayFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDisplayFanart.CheckedChanged
        Me.btnApply.Enabled = True
        If Me.chkNoDisplayFanart.Checked AndAlso Me.chkNoDisplayPoster.Checked Then
            Me.chkShowDims.Enabled = False
            Me.chkShowDims.Checked = False
        Else
            Me.chkShowDims.Enabled = True
        End If
    End Sub

    Private Sub rbDashTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDashTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub rbBracketTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBracketTrailer.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkOutlineForPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOutlineForPlot.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkDeleteAllTrailers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeleteAllTrailers.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub rbMins_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMins.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub rbHM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbHM.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtFolderPattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolderPattern.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub txtFilePattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilePattern.TextChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub btnRemoveNoStack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveNoStack.Click
        If lstNoStack.Items.Count > 0 And lstNoStack.SelectedItems.Count > 0 Then
            For Each i As Integer In lstNoStack.SelectedIndices
                lstNoStack.Items.RemoveAt(i)
            Next
            Me.btnApply.Enabled = True
            Me.doRefresh = True
        End If
    End Sub

    Private Sub btnAddNoStack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNoStack.Click
        If Not String.IsNullOrEmpty(txtNoStack.Text) Then
            If Not Strings.Left(txtNoStack.Text, 1) = "." Then txtNoStack.Text = String.Concat(".", txtNoStack.Text)
            If Not lstNoStack.Items.Contains(txtNoStack.Text) Then
                lstNoStack.Items.Add(txtNoStack.Text)
                Me.btnApply.Enabled = True
                Me.doRefresh = True
                txtNoStack.Text = String.Empty
                txtNoStack.Focus()
            End If
        End If
    End Sub

    Private Sub lvMovies_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMovies.DoubleClick
        If lvMovies.SelectedItems.Count > 0 Then
            Using dMovieSource As New dlgMovieSource
                If dMovieSource.ShowDialog(Convert.ToInt32(lvMovies.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshSources()
                    Me.doRefresh = True
                    Me.btnApply.Enabled = True
                End If
            End Using
        End If
    End Sub

    Private Sub btnEditSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSource.Click
        If lvMovies.SelectedItems.Count > 0 Then
            Using dMovieSource As New dlgMovieSource
                If dMovieSource.ShowDialog(Convert.ToInt32(lvMovies.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshSources()
                    Me.doRefresh = True
                    Me.btnApply.Enabled = True
                End If
            End Using
        End If
    End Sub
#End Region '*** Form/Controls


#Region "Routines/Functions"

    ' ########################################
    ' ########## ROUTINES/FUNCTIONS ##########
    ' ########################################

    Private Sub SaveSettings()

        Try
            '######## GENERAL TAB ########
            Master.eSettings.FilterCustom.Clear()
            Master.eSettings.FilterCustom.AddRange(Me.lstFilters.Items)
            Master.eSettings.HeaderColor = Me.btnHeaders.BackColor.ToArgb
            Master.eSettings.BackgroundColor = Me.btnBackground.BackColor.ToArgb
            Master.eSettings.InfoPanelColor = Me.btnInfoPanel.BackColor.ToArgb
            Master.eSettings.TopPanelColor = Me.btnTopPanel.BackColor.ToArgb
            Master.eSettings.PanelTextColor = Me.btnInfoPanelText.BackColor.ToArgb()
            Master.eSettings.TopPanelTextColor = Me.btnTopPanelText.BackColor.ToArgb
            Master.eSettings.HeaderTextColor = Me.btnHeaderText.BackColor.ToArgb
            If Me.tcCleaner.SelectedTab.Name = "tpExpert" Then
                Master.eSettings.ExpertCleaner = True
                Master.eSettings.CleanFolderJPG = False
                Master.eSettings.CleanMovieTBN = False
                Master.eSettings.CleanMovieTBNB = False
                Master.eSettings.CleanFanartJPG = False
                Master.eSettings.CleanMovieFanartJPG = False
                Master.eSettings.CleanMovieNFO = False
                Master.eSettings.CleanMovieNFOB = False
                Master.eSettings.CleanPosterTBN = False
                Master.eSettings.CleanPosterJPG = False
                Master.eSettings.CleanMovieJPG = False
                Master.eSettings.CleanMovieNameJPG = False
                Master.eSettings.CleanDotFanartJPG = False
                Master.eSettings.CleanExtraThumbs = False
                Master.eSettings.CleanWhitelistVideo = Me.chkWhitelistVideo.Checked
                Master.eSettings.CleanWhitelistExts.Clear()
                Master.eSettings.CleanWhitelistExts.AddRange(Me.lstWhitelist.Items)
            Else
                Master.eSettings.ExpertCleaner = False
                Master.eSettings.CleanFolderJPG = Me.chkCleanFolderJPG.Checked
                Master.eSettings.CleanMovieTBN = Me.chkCleanMovieTBN.Checked
                Master.eSettings.CleanMovieTBNB = Me.chkCleanMovieTBNb.Checked
                Master.eSettings.CleanFanartJPG = Me.chkCleanFanartJPG.Checked
                Master.eSettings.CleanMovieFanartJPG = Me.chkCleanMovieFanartJPG.Checked
                Master.eSettings.CleanMovieNFO = Me.chkCleanMovieNFO.Checked
                Master.eSettings.CleanMovieNFOB = Me.chkCleanMovieNFOb.Checked
                Master.eSettings.CleanPosterTBN = Me.chkCleanPosterTBN.Checked
                Master.eSettings.CleanPosterJPG = Me.chkCleanPosterJPG.Checked
                Master.eSettings.CleanMovieJPG = Me.chkCleanMovieJPG.Checked
                Master.eSettings.CleanMovieNameJPG = Me.chkCleanMovieNameJPG.Checked
                Master.eSettings.CleanDotFanartJPG = Me.chkCleanDotFanartJPG.Checked
                Master.eSettings.CleanExtraThumbs = Me.chkCleanExtrathumbs.Checked
                Master.eSettings.CleanWhitelistVideo = False
                Master.eSettings.CleanWhitelistExts.Clear()
            End If
            Master.eSettings.LogErrors = Me.chkLogErrors.Checked
            Master.eSettings.ProperCase = Me.chkProperCase.Checked
            Master.eSettings.OverwriteNfo = Me.chkOverwriteNfo.Checked
            Master.eSettings.XBMCComs = Me.XComs
            Master.eSettings.ValidExts.Clear()
            Master.eSettings.ValidExts.AddRange(lstMovieExts.Items)
            Master.eSettings.NoStackExts.Clear()
            Master.eSettings.NoStackExts.AddRange(lstNoStack.Items)
            Master.eSettings.CheckUpdates = chkUpdates.Checked
            Master.eSettings.InfoPanelAnim = chkInfoPanelAnim.Checked

            '######## MOVIES TAB ########

            Master.eSettings.CertificationLang = Me.cbCert.Text
            If Not String.IsNullOrEmpty(Me.cbCert.Text) Then
                Master.eSettings.UseCertForMPAA = Me.chkUseCertForMPAA.Checked
            Else
                Master.eSettings.UseCertForMPAA = False
            End If
            Master.eSettings.ScanMediaInfo = Me.chkScanMediaInfo.Checked
            Master.eSettings.FullCast = Me.chkFullCast.Checked
            Master.eSettings.FullCrew = Me.chkFullCrew.Checked
            Master.eSettings.CastImagesOnly = Me.chkCastWithImg.Checked
            Master.eSettings.MoviePosterCol = Me.chkMoviePosterCol.Checked
            Master.eSettings.MovieFanartCol = Me.chkMovieFanartCol.Checked
            Master.eSettings.MovieInfoCol = Me.chkMovieInfoCol.Checked
            Master.eSettings.MovieTrailerCol = Me.chkMovieTrailerCol.Checked
            Master.eSettings.MovieSubCol = Me.chkMovieSubCol.Checked
            Master.eSettings.MovieExtraCol = Me.chkMovieExtraCol.Checked
            Master.eSettings.UseTMDB = Me.chkUseTMDB.Checked
            Master.eSettings.UseIMPA = Me.chkUseIMPA.Checked
            Master.eSettings.UseMPDB = Me.chkUseMPDB.Checked
            Master.eSettings.PreferredPosterSize = Me.cbPosterSize.SelectedIndex
            Master.eSettings.PreferredFanartSize = Me.cbFanartSize.SelectedIndex
            Master.eSettings.FanartPrefSizeOnly = Me.chkFanartOnly.Checked
            Master.eSettings.PosterQuality = Me.tbPosterQual.Value
            Master.eSettings.FanartQuality = Me.tbFanartQual.Value
            Master.eSettings.OverwritePoster = Me.chkOverwritePoster.Checked
            Master.eSettings.OverwriteFanart = Me.chkOverwriteFanart.Checked
            Master.eSettings.MovieTBN = Me.chkMovieTBN.Checked
            Master.eSettings.MovieNameTBN = Me.chkMovieNameTBN.Checked
            Master.eSettings.MovieJPG = Me.chkMovieJPG.Checked
            Master.eSettings.MovieNameJPG = Me.chkMovieNameJPG.Checked
            Master.eSettings.PosterTBN = Me.chkPosterTBN.Checked
            Master.eSettings.PosterJPG = Me.chkPosterJPG.Checked
            Master.eSettings.FolderJPG = Me.chkFolderJPG.Checked
            Master.eSettings.FanartJPG = Me.chkFanartJPG.Checked
            Master.eSettings.MovieNameFanartJPG = Me.chkMovieNameFanartJPG.Checked
            Master.eSettings.MovieNameDotFanartJPG = Me.chkMovieNameDotFanartJPG.Checked
            Master.eSettings.MovieNFO = Me.chkMovieNFO.Checked
            Master.eSettings.MovieNameNFO = Me.chkMovieNameNFO.Checked
            Master.eSettings.DashTrailer = Me.rbDashTrailer.Checked
            Master.eSettings.VideoTSParent = Me.chkVideoTSParent.Checked
            Master.eSettings.LockPlot = Me.chkLockPlot.Checked
            Master.eSettings.LockOutline = Me.chkLockOutline.Checked
            Master.eSettings.LockTitle = Me.chkLockTitle.Checked
            Master.eSettings.LockTagline = Me.chkLockTagline.Checked
            Master.eSettings.LockRating = Me.chkLockRating.Checked
            Master.eSettings.LockStudio = Me.chkLockRealStudio.Checked
            Master.eSettings.LockGenre = Me.chkLockGenre.Checked
            Master.eSettings.LockTrailer = Me.chkLockTrailer.Checked
            Master.eSettings.SingleScrapeImages = Me.chkSingleScrapeImages.Checked
            Master.eSettings.MarkNew = Me.chkMarkNew.Checked
            Master.eSettings.ResizeFanart = Me.chkResizeFanart.Checked
            Master.eSettings.FanartHeight = If(Not String.IsNullOrEmpty(Me.txtFanartHeight.Text), CLng(Me.txtFanartHeight.Text), 0)
            Master.eSettings.FanartWidth = If(Not String.IsNullOrEmpty(Me.txtFanartWidth.Text), CLng(Me.txtFanartWidth.Text), 0)
            Master.eSettings.ResizePoster = Me.chkResizePoster.Checked
            Master.eSettings.PosterHeight = If(Not String.IsNullOrEmpty(Me.txtPosterHeight.Text), CLng(Me.txtPosterHeight.Text), 0)
            Master.eSettings.PosterWidth = If(Not String.IsNullOrEmpty(Me.txtPosterWidth.Text), CLng(Me.txtPosterWidth.Text), 0)
            Master.eSettings.UseOFDBTitle = Me.chkOFDBTitle.Checked
            Master.eSettings.UseOFDBOutline = Me.chkOFDBOutline.Checked
            Master.eSettings.UseOFDBPlot = Me.chkOFDBPlot.Checked
            Master.eSettings.UseOFDBGenre = Me.chkOFDBGenre.Checked
            If Not String.IsNullOrEmpty(txtAutoThumbs.Text) AndAlso Convert.ToInt32(txtAutoThumbs.Text) > 0 Then
                Master.eSettings.AutoThumbs = Convert.ToInt32(txtAutoThumbs.Text)
                Master.eSettings.AutoThumbsNoSpoilers = Me.chkNoSpoilers.Checked
                Master.eSettings.UseETasFA = Me.chkUseETasFA.Checked
            Else
                Master.eSettings.AutoThumbs = 0
                Master.eSettings.AutoThumbsNoSpoilers = False
                Master.eSettings.UseETasFA = False
            End If
            If Not String.IsNullOrEmpty(Me.txtIMDBURL.Text) Then
                Master.eSettings.IMDBURL = Strings.Replace(Me.txtIMDBURL.Text, "http://", String.Empty)
            Else
                Master.eSettings.IMDBURL = "akas.imdb.com"
            End If
            Master.eSettings.BDPath = Me.txtBDPath.Text
            Master.eSettings.AutoBD = Me.chkAutoBD.Checked
            Master.eSettings.UseMIDuration = Me.chkUseMIDuration.Checked
            Master.eSettings.UseHMForRuntime = Me.rbHM.Checked
            Master.eSettings.UseImgCache = Me.chkUseImgCache.Checked
            Master.eSettings.UseImgCacheUpdaters = Me.chkUseImgCacheUpdaters.Checked
            Master.eSettings.PersistImgCache = Me.chkPersistImgCache.Checked
            Master.eSettings.SkipLessThan = Convert.ToInt32(Me.txtSkipLessThan.Text)
            Master.eSettings.SkipStackSizeCheck = Me.chkSkipStackedSizeCheck.Checked
            Master.eSettings.NoSaveImagesToNfo = Me.chkNoSaveImagesToNfo.Checked

            Master.eSettings.TrailerSites.Clear()
            If Me.lbTrailerSites.CheckedItems.Count > 0 Then
                For Each iTrailer As Integer In Me.lbTrailerSites.CheckedIndices
                    Master.eSettings.TrailerSites.Add(iTrailer)
                Next
                Master.eSettings.DownloadTrailers = Me.chkDownloadTrailer.Checked
                Master.eSettings.UpdaterTrailers = Me.chkUpdaterTrailer.Checked
                Master.eSettings.TrailerTimeout = Convert.ToInt32(Me.txtTimeout.Text)
                Master.eSettings.SingleScrapeTrailer = Me.chkSingleScrapeTrailer.Checked
                Master.eSettings.UpdaterTrailersNoDownload = Me.chkNoDLTrailer.Checked
                Master.eSettings.OverwriteTrailer = Me.chkOverwriteTrailer.Checked
                Master.eSettings.DeleteAllTrailers = Me.chkDeleteAllTrailers.Checked
            Else
                Master.eSettings.DownloadTrailers = False
                Master.eSettings.UpdaterTrailers = False
                Master.eSettings.SingleScrapeTrailer = False
                Master.eSettings.TrailerTimeout = 2
                Master.eSettings.UpdaterTrailersNoDownload = False
                Master.eSettings.OverwriteTrailer = False
                Master.eSettings.DeleteAllTrailers = False
            End If

            If Me.lbGenre.CheckedItems.Count > 0 Then

                If Me.lbGenre.CheckedIndices.Contains(0) Then
                    Master.eSettings.GenreFilter = "[All]"
                Else
                    Dim strGenre As String = String.Empty
                    Dim iChecked = From iCheck In Me.lbGenre.CheckedItems
                    strGenre = Strings.Join(iChecked.ToArray, ",")
                    Master.eSettings.GenreFilter = strGenre.Trim
                End If
            End If

            Master.eSettings.ShowDims = Me.chkShowDims.Checked
            Master.eSettings.NoDisplayFanart = Me.chkNoDisplayFanart.Checked
            Master.eSettings.NoDisplayPoster = Me.chkNoDisplayPoster.Checked
            Master.eSettings.OutlineForPlot = Me.chkOutlineForPlot.Checked

            Master.eSettings.FoldersPattern = Me.txtFolderPattern.Text
            Master.eSettings.FilesPattern = Me.txtFilePattern.Text
            Master.eSettings.AllwaysDisplayGenresText = Me.chkShowGenresText.Checked
            Master.eSettings.Save()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillSettings()

        Try
            '######## GENERAL TAB ########
            Me.lstFilters.Items.AddRange(Master.eSettings.FilterCustom.ToArray)
            Me.btnHeaders.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
            Me.btnBackground.BackColor = Color.FromArgb(Master.eSettings.BackgroundColor)
            Me.btnInfoPanel.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
            Me.btnTopPanel.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
            Me.btnInfoPanelText.BackColor = Color.FromArgb(Master.eSettings.PanelTextColor)
            Me.btnTopPanelText.BackColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
            Me.btnHeaderText.BackColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
            Me.chkCleanFolderJPG.Checked = Master.eSettings.CleanFolderJPG
            Me.chkCleanMovieTBN.Checked = Master.eSettings.CleanMovieTBN
            Me.chkCleanMovieTBNb.Checked = Master.eSettings.CleanMovieTBNB
            Me.chkCleanFanartJPG.Checked = Master.eSettings.CleanFanartJPG
            Me.chkCleanMovieFanartJPG.Checked = Master.eSettings.CleanMovieFanartJPG
            Me.chkCleanMovieNFO.Checked = Master.eSettings.CleanMovieNFO
            Me.chkCleanMovieNFOb.Checked = Master.eSettings.CleanMovieNFOB
            Me.chkCleanPosterTBN.Checked = Master.eSettings.CleanPosterTBN
            Me.chkCleanPosterJPG.Checked = Master.eSettings.CleanPosterJPG
            Me.chkCleanMovieJPG.Checked = Master.eSettings.CleanMovieJPG
            Me.chkCleanMovieNameJPG.Checked = Master.eSettings.CleanMovieNameJPG
            Me.chkCleanDotFanartJPG.Checked = Master.eSettings.CleanDotFanartJPG
            Me.chkCleanExtrathumbs.Checked = Master.eSettings.CleanExtraThumbs
            tcCleaner.SelectedTab = If(Master.eSettings.ExpertCleaner, tpExpert, tpStandard)
            Me.chkWhitelistVideo.Checked = Master.eSettings.CleanWhitelistVideo
            Me.lstWhitelist.Items.AddRange(Master.eSettings.CleanWhitelistExts.ToArray)
            Me.chkOverwriteNfo.Checked = Master.eSettings.OverwriteNfo

            Me.XComs = Master.eSettings.XBMCComs
            Me.LoadXComs()

            Me.chkLogErrors.Checked = Master.eSettings.LogErrors
            Me.chkProperCase.Checked = Master.eSettings.ProperCase
            Me.lstMovieExts.Items.AddRange(Master.eSettings.ValidExts.ToArray)
            Me.lstNoStack.Items.AddRange(Master.eSettings.NoStackExts.ToArray)
            Me.chkUpdates.Checked = Master.eSettings.CheckUpdates
            Me.chkInfoPanelAnim.Checked = Master.eSettings.InfoPanelAnim
            '######## MOVIES TAB ########

            If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then
                Me.chkCert.Checked = True
                Me.cbCert.Enabled = True
                Me.cbCert.Text = Master.eSettings.CertificationLang
                Me.chkUseCertForMPAA.Enabled = True
                Me.chkUseCertForMPAA.Checked = Master.eSettings.UseCertForMPAA
            End If
            Me.chkScanMediaInfo.Checked = Master.eSettings.ScanMediaInfo
            Me.chkFullCast.Checked = Master.eSettings.FullCast
            Me.chkFullCrew.Checked = Master.eSettings.FullCrew
            Me.chkCastWithImg.Checked = Master.eSettings.CastImagesOnly
            Me.chkMoviePosterCol.Checked = Master.eSettings.MoviePosterCol
            Me.chkMovieFanartCol.Checked = Master.eSettings.MovieFanartCol
            Me.chkMovieInfoCol.Checked = Master.eSettings.MovieInfoCol
            Me.chkMovieTrailerCol.Checked = Master.eSettings.MovieTrailerCol
            Me.chkMovieSubCol.Checked = Master.eSettings.MovieSubCol
            Me.chkMovieExtraCol.Checked = Master.eSettings.MovieExtraCol
            Me.chkUseTMDB.Checked = Master.eSettings.UseTMDB
            Me.chkUseIMPA.Checked = Master.eSettings.UseIMPA
            Me.chkUseMPDB.Checked = Master.eSettings.UseMPDB
            Me.cbPosterSize.SelectedIndex = Master.eSettings.PreferredPosterSize
            Me.cbFanartSize.SelectedIndex = Master.eSettings.PreferredFanartSize
            Me.chkFanartOnly.Checked = Master.eSettings.FanartPrefSizeOnly
            Me.tbPosterQual.Value = Master.eSettings.PosterQuality
            Me.tbFanartQual.Value = Master.eSettings.FanartQuality
            Me.chkOverwritePoster.Checked = Master.eSettings.OverwritePoster
            Me.chkOverwriteFanart.Checked = Master.eSettings.OverwriteFanart
            Me.chkMovieTBN.Checked = Master.eSettings.MovieTBN
            Me.chkMovieNameTBN.Checked = Master.eSettings.MovieNameTBN
            Me.chkMovieJPG.Checked = Master.eSettings.MovieJPG
            Me.chkMovieNameJPG.Checked = Master.eSettings.MovieNameJPG
            Me.chkPosterTBN.Checked = Master.eSettings.PosterTBN
            Me.chkPosterJPG.Checked = Master.eSettings.PosterJPG
            Me.chkFolderJPG.Checked = Master.eSettings.FolderJPG
            Me.chkFanartJPG.Checked = Master.eSettings.FanartJPG
            Me.chkMovieNameFanartJPG.Checked = Master.eSettings.MovieNameFanartJPG
            Me.chkMovieNameDotFanartJPG.Checked = Master.eSettings.MovieNameDotFanartJPG
            Me.chkMovieNFO.Checked = Master.eSettings.MovieNFO
            Me.chkMovieNameNFO.Checked = Master.eSettings.MovieNameNFO
            Me.rbDashTrailer.Checked = Master.eSettings.DashTrailer
            Me.rbBracketTrailer.Checked = Not Master.eSettings.DashTrailer
            Me.chkVideoTSParent.Checked = Master.eSettings.VideoTSParent
            Me.chkLockPlot.Checked = Master.eSettings.LockPlot
            Me.chkLockOutline.Checked = Master.eSettings.LockOutline
            Me.chkLockTitle.Checked = Master.eSettings.LockTitle
            Me.chkLockTagline.Checked = Master.eSettings.LockTagline
            Me.chkLockRating.Checked = Master.eSettings.LockRating
            Me.chkLockRealStudio.Checked = Master.eSettings.LockStudio
            Me.chkLockGenre.Checked = Master.eSettings.LockGenre
            Me.chkLockTrailer.Checked = Master.eSettings.LockTrailer
            Me.chkSingleScrapeImages.Checked = Master.eSettings.SingleScrapeImages
            Me.chkMarkNew.Checked = Master.eSettings.MarkNew
            Me.chkResizeFanart.Checked = Master.eSettings.ResizeFanart
            If Master.eSettings.ResizeFanart Then
                Me.txtFanartWidth.Text = Master.eSettings.FanartWidth
                Me.txtFanartHeight.Text = Master.eSettings.FanartHeight
            End If
            Me.chkResizePoster.Checked = Master.eSettings.ResizePoster
            If Master.eSettings.ResizePoster Then
                Me.txtPosterWidth.Text = Master.eSettings.PosterWidth
                Me.txtPosterHeight.Text = Master.eSettings.PosterHeight
            End If
            Me.chkOFDBTitle.Checked = Master.eSettings.UseOFDBTitle
            Me.chkOFDBOutline.Checked = Master.eSettings.UseOFDBOutline
            Me.chkOFDBPlot.Checked = Master.eSettings.UseOFDBPlot
            Me.chkOFDBGenre.Checked = Master.eSettings.UseOFDBGenre
            If Master.eSettings.AutoThumbs > 0 Then
                Me.chkAutoThumbs.Checked = True
                Me.txtAutoThumbs.Enabled = True
                Me.txtAutoThumbs.Text = Master.eSettings.AutoThumbs.ToString
                Me.chkNoSpoilers.Enabled = True
                Me.chkNoSpoilers.Checked = Master.eSettings.AutoThumbsNoSpoilers
                Me.chkUseETasFA.Enabled = True
                Me.chkUseETasFA.Checked = Master.eSettings.UseETasFA
            End If
            Me.txtIMDBURL.Text = Master.eSettings.IMDBURL
            Me.txtBDPath.Text = Master.eSettings.BDPath
            Me.chkAutoBD.Checked = Master.eSettings.AutoBD
            Me.chkUseMIDuration.Checked = Master.eSettings.UseMIDuration
            Me.rbHM.Checked = Master.eSettings.UseHMForRuntime
            Me.chkUseImgCache.Checked = Master.eSettings.UseImgCache
            Me.chkUseImgCacheUpdaters.Checked = Master.eSettings.UseImgCacheUpdaters
            Me.chkPersistImgCache.Checked = Master.eSettings.PersistImgCache
            Me.txtSkipLessThan.Text = Master.eSettings.SkipLessThan.ToString
            Me.chkSkipStackedSizeCheck.Checked = Master.eSettings.SkipStackSizeCheck
            Me.chkNoSaveImagesToNfo.Checked = Master.eSettings.NoSaveImagesToNfo
            Me.chkDownloadTrailer.Checked = Master.eSettings.DownloadTrailers
            Me.chkUpdaterTrailer.Checked = Master.eSettings.UpdaterTrailers
            Me.chkNoDLTrailer.Checked = Master.eSettings.UpdaterTrailersNoDownload
            Me.chkSingleScrapeTrailer.Checked = Master.eSettings.SingleScrapeTrailer
            Me.txtTimeout.Text = Master.eSettings.TrailerTimeout.ToString
            Me.chkOverwriteTrailer.Checked = Master.eSettings.OverwriteTrailer
            Me.chkDeleteAllTrailers.Checked = Master.eSettings.DeleteAllTrailers

            If Master.eSettings.TrailerSites.Count > 0 Then
                For Each iTrailer As Integer In Master.eSettings.TrailerSites
                    Me.lbTrailerSites.SetItemChecked(iTrailer, True)
                Next
            End If

            If Not String.IsNullOrEmpty(Master.eSettings.GenreFilter) Then
                Dim genreArray() As String
                genreArray = Strings.Split(Master.eSettings.GenreFilter, ",")
                For g As Integer = 0 To UBound(genreArray)
                    If Me.lbGenre.FindString(Strings.Trim(genreArray(g))) > 0 Then Me.lbGenre.SetItemChecked(Me.lbGenre.FindString(Strings.Trim(genreArray(g))), True)
                Next

                If Me.lbGenre.CheckedItems.Count = 0 Then
                    Me.lbGenre.SetItemChecked(0, True)
                End If
            Else
                Me.lbGenre.SetItemChecked(0, True)
            End If

            Me.chkShowDims.Checked = Master.eSettings.ShowDims
            Me.chkNoDisplayFanart.Checked = Master.eSettings.NoDisplayFanart
            Me.chkNoDisplayPoster.Checked = Master.eSettings.NoDisplayPoster
            Me.chkOutlineForPlot.Checked = Master.eSettings.OutlineForPlot

            Me.txtFolderPattern.Text = Master.eSettings.FoldersPattern
            Me.txtFilePattern.Text = Master.eSettings.FilesPattern
            Me.chkShowGenresText.Checked = Master.eSettings.AllwaysDisplayGenresText

            Me.RefreshSources()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub LoadXComs()
        Me.lbXBMCCom.Items.Clear()
        For Each x As emmSettings.XBMCCom In Me.XComs
            Me.lbXBMCCom.Items.Add(x.Name)
        Next
    End Sub

    Private Sub LoadGenreLangs()

        '//
        ' Read all the genre languages from the xml and load into the list
        '\\

        Me.lbGenre.Items.Add("[All]")

        Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Genres")

        If File.Exists(Path.Combine(mePath, "Genres.xml")) Then
            Try
                Dim xmlGenre As XDocument = XDocument.Load(Path.Combine(mePath, "Genres.xml"))

                Dim xGenre = From xGen In xmlGenre...<supported>.Descendants Select xGen.Value
                If xGenre.Count > 0 Then
                    For Each sGenre As String In xGenre
                        Me.lbGenre.Items.Add(sGenre)
                    Next
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox(String.Concat("Cannot find Genres.xml.", vbNewLine, vbNewLine, "Expected path:", vbNewLine, Path.Combine(mePath, "Genres.xml")), MsgBoxStyle.Critical, "File Not Found")
        End If

    End Sub

    Private Sub RefreshSources()
        lvMovies.Items.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM sources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    Dim lvItem As New ListViewItem(SQLreader("ID").ToString)
                    lvItem.SubItems.Add(SQLreader("Name").ToString)
                    lvItem.SubItems.Add(SQLreader("Path").ToString)
                    lvItem.SubItems.Add(If(SQLreader("Recursive"), "Yes", "No"))
                    lvItem.SubItems.Add(If(SQLreader("Foldername"), "Yes", "No"))
                    lvItem.SubItems.Add(If(SQLreader("Single"), "Yes", "No"))
                    lvMovies.Items.Add(lvItem)
                End While
            End Using
        End Using

    End Sub

#End Region '*** Routines/Functions


End Class