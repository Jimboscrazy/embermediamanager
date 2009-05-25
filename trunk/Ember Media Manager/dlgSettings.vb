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

#Region "Form/Controls"

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
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMovieAddFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFolder.Click
        Me.AddItem(True)
    End Sub

    Private Sub btnMovieAddFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFiles.Click
        Me.AddItem(False)
    End Sub

    Private Sub btnMovieRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieRem.Click
        Try
            If Me.lvMovies.SelectedItems.Count > 0 Then
                Me.lvMovies.BeginUpdate()
                For Each lvItem As ListViewItem In Me.lvMovies.SelectedItems
                    lvItem.Remove()
                Next
                Me.lvMovies.Sort()
                Me.lvMovies.EndUpdate()
                Me.lvMovies.Refresh()
                Me.btnApply.Enabled = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.SaveSettings()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Dim g As Graphics = Graphics.FromImage(iBackground)
            g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, 20), pnlTop.ClientRectangle)
            Me.pnlTop.BackgroundImage = iBackground
            g.Dispose()

            Me.FillSettings()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnAddFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFilter.Click
        If Not String.IsNullOrEmpty(Me.txtFilter.Text) Then
            Me.lstFilters.Items.Add(Me.txtFilter.Text)
            Me.txtFilter.Text = String.Empty
            Me.btnApply.Enabled = True
        End If

        Me.txtFilter.Focus()
    End Sub

    Private Sub btnRemoveFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFilter.Click
        If Me.lstFilters.Items.Count > 0 Then
            For i As Integer = Me.lstFilters.SelectedItems.Count - 1 To 0 Step -1
                Me.lstFilters.Items.Remove(Me.lstFilters.SelectedItems(i))
                Me.btnApply.Enabled = True
            Next
        End If
    End Sub

    Private Sub chkStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStudio.CheckedChanged
        Me.btnApply.Enabled = True
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

    Private Sub chkUseFolderNames_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseFolderNames.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkProperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProperCase.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            If lstFilters.Items.Count > 0 AndAlso Not IsNothing(lstFilters.SelectedItem) AndAlso lstFilters.SelectedIndex > 0 Then
                Dim iIndex As Integer = lstFilters.SelectedIndices(0)
                lstFilters.Items.Insert(iIndex - 1, lstFilters.SelectedItems(0))
                lstFilters.Items.RemoveAt(iIndex + 1)
                btnApply.Enabled = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            If lstFilters.Items.Count > 0 AndAlso Not IsNothing(lstFilters.SelectedItem) AndAlso lstFilters.SelectedIndex < (lstFilters.Items.Count - 1) Then
                Dim iIndex As Integer = lstFilters.SelectedIndices(0)
                lstFilters.Items.Insert(iIndex + 2, lstFilters.SelectedItems(0))
                lstFilters.Items.RemoveAt(iIndex)
                btnApply.Enabled = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub rbMovieName_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
    End Sub

    Private Sub rbMovie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkTitleFromNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkUseMPDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
        If Not chkAutoThumbs.Checked Then
            Me.txtAutoThumbs.Text = String.Empty
        End If
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkScanRecursive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScanRecursive.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub

    Private Sub chkCastWithImg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCastWithImg.CheckedChanged
        Me.btnApply.Enabled = True
    End Sub
#End Region '*** Form/Controls


#Region "Routines/Functions"

    ' ########################################
    ' ########## ROUTINES/FUNCTIONS ##########
    ' ########################################

    Private Sub AddItem(ByVal blnIsFolder As Boolean)

        '//
        ' Add folder to folder list. Check to make sure it exists before adding it
        '\\


        Try
            Dim iFound As Integer = -1
            Dim lvItem As ListViewItem

            With Me.fbdBrowse
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(.SelectedPath.ToString) AndAlso Directory.Exists(.SelectedPath) Then

                        If Me.lvMovies.Items.Count > 0 Then
                            For i As Integer = 0 To Me.lvMovies.Items.Count - 1
                                If Me.lvMovies.Items(i).ToString.ToLower = .SelectedPath.ToString.ToLower Then
                                    iFound = i
                                    Exit For
                                End If
                            Next
                        Else
                            iFound = -1
                        End If


                        If iFound >= 0 Then
                            Me.lvMovies.Items(iFound).Selected = True
                            Me.lvMovies.Items(iFound).Focused = True
                            Me.lvMovies.Focus()
                        Else
                            Me.lvMovies.BeginUpdate()
                            lvItem = Me.lvMovies.Items.Add(.SelectedPath)
                            If blnIsFolder = True Then
                                lvItem.SubItems.Add("Folders")
                            Else
                                lvItem.SubItems.Add("Files")
                            End If
                            lvItem = Nothing
                            Me.lvMovies.Sort()
                            Me.lvMovies.EndUpdate()
                            Me.lvMovies.Columns(0).Width = 388
                            Me.lvMovies.Columns(1).Width = 74
                            Me.lvMovies.Refresh()
                            Me.btnApply.Enabled = True
                        End If

                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SaveSettings()

        Try
            '######## GENERAL TAB ########
            Master.eSettings.FilterCustom.Clear()
            For Each str As String In Me.lstFilters.Items
                Master.eSettings.FilterCustom.Add(str)
            Next

            Master.eSettings.HeaderColor = Me.btnHeaders.BackColor.ToArgb
            Master.eSettings.BackgroundColor = Me.btnBackground.BackColor.ToArgb
            Master.eSettings.InfoPanelColor = Me.btnInfoPanel.BackColor.ToArgb
            Master.eSettings.TopPanelColor = Me.btnTopPanel.BackColor.ToArgb
            Master.eSettings.PanelTextColor = Me.btnInfoPanelText.BackColor.ToArgb()
            Master.eSettings.TopPanelTextColor = Me.btnTopPanelText.BackColor.ToArgb
            Master.eSettings.HeaderTextColor = Me.btnHeaderText.BackColor.ToArgb
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
            Master.eSettings.LogErrors = Me.chkLogErrors.Checked
            Master.eSettings.ProperCase = Me.chkProperCase.Checked
            Master.eSettings.OverwriteNfo = Me.chkOverwriteNfo.Checked
            Master.eSettings.XBMCIP = Me.txtIP.Text
            Master.eSettings.XBMCPort = Me.txtPort.Text
            Master.eSettings.XBMCUsername = Me.txtUsername.Text
            Master.eSettings.XBMCPassword = Me.txtPassword.Text

            '######## MOVIES TAB ########
            Master.eSettings.MovieFolders.Clear()
            For Each lvItem As ListViewItem In Me.lvMovies.Items
                Master.eSettings.MovieFolders.Add(lvItem.Text & "|" & lvItem.SubItems(1).Text)
            Next

            Master.eSettings.CertificationLang = Me.cbCert.Text
            If Not String.IsNullOrEmpty(Me.cbCert.Text) Then
                Master.eSettings.UseCertForMPAA = Me.chkUseCertForMPAA.Checked
            Else
                Master.eSettings.UseCertForMPAA = False
            End If
            Master.eSettings.UseStudioTags = Me.chkStudio.Checked
            Master.eSettings.FullCast = Me.chkFullCast.Checked
            Master.eSettings.FullCrew = Me.chkFullCrew.Checked
            Master.eSettings.CastImagesOnly = Me.chkCastWithImg.Checked
            Master.eSettings.MoviePosterCol = Me.chkMoviePosterCol.Checked
            Master.eSettings.MovieFanartCol = Me.chkMovieFanartCol.Checked
            Master.eSettings.MovieInfoCol = Me.chkMovieInfoCol.Checked
            Master.eSettings.MovieTrailerCol = Me.chkMovieTrailerCol.Checked
            Master.eSettings.UseTMDB = Me.chkUseTMDB.Checked
            Master.eSettings.UseIMPA = Me.chkUseIMPA.Checked
            Master.eSettings.UseMPDB = Me.chkUseMPDB.Checked
            Master.eSettings.PreferredPosterSize = Me.cbPosterSize.SelectedIndex
            Master.eSettings.PreferredFanartSize = Me.cbFanartSize.SelectedIndex
            Master.eSettings.OverwritePoster = Me.chkOverwritePoster.Checked
            Master.eSettings.OverwriteFanart = Me.chkOverwriteFanart.Checked
            Master.eSettings.UseFolderName = Me.chkUseFolderNames.Checked
            Master.eSettings.UseNameFromNfo = Me.chkTitleFromNfo.Checked
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
            Master.eSettings.LockPlot = Me.chkLockPlot.Checked
            Master.eSettings.LockOutline = Me.chkLockOutline.Checked
            Master.eSettings.LockTitle = Me.chkLockTitle.Checked
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
            If Not String.IsNullOrEmpty(txtAutoThumbs.Text) AndAlso CInt(txtAutoThumbs.Text) > 0 Then
                Master.eSettings.AutoThumbs = CInt(txtAutoThumbs.Text)
            Else
                Master.eSettings.AutoThumbs = 0
            End If
            Master.eSettings.ScanRecursive = Me.chkScanRecursive.Checked

            Master.eSettings.Save()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillSettings()

        Dim dirArray() As String
        Dim lvItem As ListViewItem

        Try
            '######## GENERAL TAB ########
            For Each strFilter As String In Master.eSettings.FilterCustom
                Me.lstFilters.Items.Add(strFilter)
            Next

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
            Me.chkOverwriteNfo.Checked = Master.eSettings.OverwriteNfo

            Me.txtIP.Text = Master.eSettings.XBMCIP
            Me.txtPort.Text = Master.eSettings.XBMCPort
            Me.txtUsername.Text = Master.eSettings.XBMCUsername
            Me.txtPassword.Text = Master.eSettings.XBMCPassword

            Me.chkLogErrors.Checked = Master.eSettings.LogErrors
            Me.chkProperCase.Checked = Master.eSettings.ProperCase

            '######## MOVIES TAB ########
            For Each strFolders As String In Master.eSettings.MovieFolders
                dirArray = Split(strFolders, "|")
                lvItem = Me.lvMovies.Items.Add(dirArray(0).ToString)
                lvItem.SubItems.Add(dirArray(1).ToString)
            Next

            If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then
                Me.chkCert.Checked = True
                Me.cbCert.Enabled = True
                Me.cbCert.Text = Master.eSettings.CertificationLang
                Me.chkUseCertForMPAA.Enabled = True
                Me.chkUseCertForMPAA.Checked = Master.eSettings.UseCertForMPAA
            End If
            Me.chkStudio.Checked = Master.eSettings.UseStudioTags
            Me.chkFullCast.Checked = Master.eSettings.FullCast
            Me.chkFullCrew.Checked = Master.eSettings.FullCrew
            Me.chkCastWithImg.Checked = Master.eSettings.CastImagesOnly
            Me.chkMoviePosterCol.Checked = Master.eSettings.MoviePosterCol
            Me.chkMovieFanartCol.Checked = Master.eSettings.MovieFanartCol
            Me.chkMovieInfoCol.Checked = Master.eSettings.MovieInfoCol
            Me.chkMovieTrailerCol.Checked = Master.eSettings.MovieTrailerCol
            Me.chkUseTMDB.Checked = Master.eSettings.UseTMDB
            Me.chkUseIMPA.Checked = Master.eSettings.UseIMPA
            Me.chkUseMPDB.Checked = Master.eSettings.UseMPDB
            Me.cbPosterSize.SelectedIndex = Master.eSettings.PreferredPosterSize
            Me.cbFanartSize.SelectedIndex = Master.eSettings.PreferredFanartSize
            Me.chkOverwritePoster.Checked = Master.eSettings.OverwritePoster
            Me.chkOverwriteFanart.Checked = Master.eSettings.OverwriteFanart
            Me.chkUseFolderNames.Checked = Master.eSettings.UseFolderName
            Me.chkTitleFromNfo.Checked = Master.eSettings.UseNameFromNfo
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
            Me.chkLockPlot.Checked = Master.eSettings.LockPlot
            Me.chkLockOutline.Checked = Master.eSettings.LockOutline
            Me.chkLockTitle.Checked = Master.eSettings.LockTitle
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
            If Master.eSettings.AutoThumbs > 0 Then
                Me.chkAutoThumbs.Checked = True
                Me.txtAutoThumbs.Text = Master.eSettings.AutoThumbs.ToString
                Me.txtAutoThumbs.Enabled = True
            End If
            Me.chkScanRecursive.Checked = Master.eSettings.ScanRecursive

            Me.lvMovies.Columns(0).Width = 388
            Me.lvMovies.Columns(1).Width = 74
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

#End Region '*** Routines/Functions

End Class