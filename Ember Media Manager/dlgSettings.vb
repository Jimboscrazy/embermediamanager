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

'TODO: Implement settings for TV Season images

Public Class dlgSettings

    Private didApply As Boolean = False
    Private sResult As New Master.SettingsResult
    Private XComs As List(Of emmSettings.XBMCCom)
    Private Meta As List(Of emmSettings.MetadataPerType)
    Private LangChanged As Boolean = False
    Private ShowRegex As List(Of emmSettings.TVShowRegEx)

#Region "Form/Controls"

    ' ########################################
    ' ############ FORMS/CONTROLS ############
    ' ########################################

    Public Overloads Function ShowDialog() As Master.SettingsResult

        MyBase.ShowDialog()
        Return Me.sResult

    End Function

    Private Sub SetApplyButton(ByVal v As Boolean)
        Me.btnApply.Enabled = v
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Try
            Me.SaveSettings()
            If LangChanged Then
                LangChanged = False
                SetUp()
            End If
            Me.SetApplyButton(False)
            If Me.sResult.NeedsUpdate OrElse Me.sResult.NeedsRefresh Then Me.didApply = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMovieAddFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFolder.Click
        Using dSource As New dlgMovieSource
            If dSource.ShowDialog = Windows.Forms.DialogResult.OK Then
                RefreshSources()
                Me.SetApplyButton(True)
                Me.sResult.NeedsUpdate = True
            End If
        End Using
    End Sub

    Private Sub btnMovieRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieRem.Click
        Me.RemoveMovieSource()
    End Sub

    Private Sub btnAddTVSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTVSource.Click
        Using dSource As New dlgTVSource
            If dSource.ShowDialog = Windows.Forms.DialogResult.OK Then
                RefreshTVSources()
                Me.SetApplyButton(True)
                Me.sResult.NeedsUpdate = True
            End If
        End Using
    End Sub

    Private Sub btnRemTVSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemTVSource.Click
        Me.RemoveTVSource()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.SaveSettings()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Not didApply Then sResult.DidCancel = True
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.SetUp()

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
            Me.LoadIntLangs()
            Me.LoadLangs()
            Me.LoadThemes()
            Me.LoadRatingRegions()
            Me.FillSettings()

            Me.SetApplyButton(False)

            Me.sResult.NeedsUpdate = False
            sResult.NeedsRefresh = False
            sResult.DidCancel = False
            Me.didApply = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnAddFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFilter.Click
        If Not String.IsNullOrEmpty(Me.txtFilter.Text) Then
            Me.lstFilters.Items.Add(Me.txtFilter.Text)
            Me.txtFilter.Text = String.Empty
            Me.SetApplyButton(True)
            Me.sResult.NeedsUpdate = True
        End If

        Me.txtFilter.Focus()
    End Sub

    Private Sub btnAddShowFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddShowFilter.Click
        If Not String.IsNullOrEmpty(Me.txtShowFilter.Text) Then
            Me.lstShowFilters.Items.Add(Me.txtShowFilter.Text)
            Me.txtShowFilter.Text = String.Empty
            Me.SetApplyButton(True)
            Me.sResult.NeedsUpdate = True
        End If

        Me.txtShowFilter.Focus()
    End Sub

    Private Sub btnAddEpFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddEpFilter.Click
        If Not String.IsNullOrEmpty(Me.txtEpFilter.Text) Then
            Me.lstEpFilters.Items.Add(Me.txtEpFilter.Text)
            Me.txtEpFilter.Text = String.Empty
            Me.SetApplyButton(True)
            Me.sResult.NeedsUpdate = True
        End If

        Me.txtEpFilter.Focus()
    End Sub

    Private Sub btnRemoveFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFilter.Click
        Me.RemoveFilter()
    End Sub

    Private Sub btnRemoveShowFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveShowFilter.Click
        Me.RemoveShowFilter()
    End Sub

    Private Sub btnRemoveEpFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveEpFilter.Click
        Me.RemoveEpFilter()
    End Sub

    Private Sub chkStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkScanMediaInfo.CheckedChanged
        Me.SetApplyButton(True)
        Me.chkUseMIDuration.Enabled = Me.chkScanMediaInfo.Checked
        Me.cbLanguages.Enabled = Me.chkScanMediaInfo.Checked
        Me.chkIFOScan.Enabled = Me.chkScanMediaInfo.Checked
        If Not Me.chkScanMediaInfo.Checked Then
            Me.chkUseMIDuration.Checked = False
            Me.gbRTFormat.Enabled = False
            Me.rbMins.Checked = True
            Me.chkIFOScan.Checked = False
        End If
    End Sub

    Private Sub cbCert_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCert.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCert.CheckedChanged
        Me.cbCert.SelectedIndex = -1
        Me.cbCert.Enabled = Me.chkCert.Checked
        Me.chkUseCertForMPAA.Enabled = Me.chkCert.Checked
        If Not Me.chkCert.Checked Then
            Me.chkUseCertForMPAA.Checked = False
        End If
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbForce_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbForce.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkForceTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkForceTitle.CheckedChanged
        Me.cbForce.SelectedIndex = -1
        Me.cbForce.Enabled = Me.chkForceTitle.Checked
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkFullCast_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFullCast.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkFullCrew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFullCrew.CheckedChanged
        Me.SetApplyButton(True)

        Me.chkProducers.Enabled = Me.chkFullCrew.Checked
        Me.chkMusicBy.Enabled = Me.chkFullCrew.Checked
        Me.chkCrew.Enabled = Me.chkFullCrew.Checked

        If Not Me.chkFullCrew.Checked Then
            Me.chkProducers.Checked = False
            Me.chkMusicBy.Checked = False
            Me.chkCrew.Checked = False
        End If
    End Sub

    Private Sub chkMoviePosterCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMoviePosterCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieFanartCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieFanartCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieInfoCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieInfoCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieTrailerCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieTrailerCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanFolderJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieTBNb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieTBNb.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNFO.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieNFOb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNFOb.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseTMDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseTMDB.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseIMPA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseIMPA.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbPosterSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPosterSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbFanartSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFanartSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwritePoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwritePoster.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLogErrors_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLogErrors.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkProperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProperCase.CheckedChanged
        Me.SetApplyButton(True)
        Me.sResult.NeedsRefresh = True
    End Sub

    Private Sub chkShowProperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowProperCase.CheckedChanged
        Me.SetApplyButton(True)
        Me.sResult.NeedsRefresh = True
    End Sub

    Private Sub chkEpProperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpProperCase.CheckedChanged
        Me.SetApplyButton(True)
        Me.sResult.NeedsRefresh = True
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            If Me.lstFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstFilters.SelectedItem) AndAlso Me.lstFilters.SelectedIndex > 0 Then
                Dim iIndex As Integer = Me.lstFilters.SelectedIndices(0)
                Me.lstFilters.Items.Insert(iIndex - 1, Me.lstFilters.SelectedItems(0))
                Me.lstFilters.Items.RemoveAt(iIndex + 1)
                Me.lstFilters.SelectedIndex = iIndex - 1
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnShowFilterUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFilterUp.Click
        Try
            If Me.lstShowFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstShowFilters.SelectedItem) AndAlso Me.lstShowFilters.SelectedIndex > 0 Then
                Dim iIndex As Integer = Me.lstShowFilters.SelectedIndices(0)
                Me.lstShowFilters.Items.Insert(iIndex - 1, Me.lstShowFilters.SelectedItems(0))
                Me.lstShowFilters.Items.RemoveAt(iIndex + 1)
                Me.lstShowFilters.SelectedIndex = iIndex - 1
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstShowFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnEpFilterUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEpFilterUp.Click
        Try
            If Me.lstEpFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstEpFilters.SelectedItem) AndAlso Me.lstEpFilters.SelectedIndex > 0 Then
                Dim iIndex As Integer = Me.lstEpFilters.SelectedIndices(0)
                Me.lstEpFilters.Items.Insert(iIndex - 1, Me.lstEpFilters.SelectedItems(0))
                Me.lstEpFilters.Items.RemoveAt(iIndex + 1)
                Me.lstEpFilters.SelectedIndex = iIndex - 1
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstEpFilters.Focus()
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
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnShowFilterDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFilterDown.Click
        Try
            If Me.lstShowFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstShowFilters.SelectedItem) AndAlso Me.lstShowFilters.SelectedIndex < (Me.lstShowFilters.Items.Count - 1) Then
                Dim iIndex As Integer = Me.lstShowFilters.SelectedIndices(0)
                Me.lstShowFilters.Items.Insert(iIndex + 2, Me.lstShowFilters.SelectedItems(0))
                Me.lstShowFilters.Items.RemoveAt(iIndex)
                Me.lstShowFilters.SelectedIndex = iIndex + 1
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstShowFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnEpFilterDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEpFilterDown.Click
        Try
            If Me.lstEpFilters.Items.Count > 0 AndAlso Not IsNothing(Me.lstEpFilters.SelectedItem) AndAlso Me.lstEpFilters.SelectedIndex < (Me.lstEpFilters.Items.Count - 1) Then
                Dim iIndex As Integer = Me.lstEpFilters.SelectedIndices(0)
                Me.lstEpFilters.Items.Insert(iIndex + 2, Me.lstEpFilters.SelectedItems(0))
                Me.lstEpFilters.Items.RemoveAt(iIndex)
                Me.lstEpFilters.SelectedIndex = iIndex + 1
                Me.SetApplyButton(True)
                Me.sResult.NeedsRefresh = True
                Me.lstEpFilters.Focus()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub chkUseMPDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseMPDB.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosterJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFolderJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNFO.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameNFO.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameDotFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameDotFanartJPG.CheckedChanged
        btnApply.Enabled = True
    End Sub

    Private Sub chkLockPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockPlot.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockOutline.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTitle.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSingleScrapeImages_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSingleScrapeImages.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanPosterTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanPosterJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanMovieNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanMovieNameJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanDotFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanDotFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtIP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtIP.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar, True)
    End Sub

    Private Sub txtIP_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIP.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtPort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPort.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPort.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsername.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteNfo.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMarkNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMarkNew.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizeFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeFanart.CheckedChanged
        Me.SetApplyButton(True)

        txtFanartWidth.Enabled = chkResizeFanart.Checked
        txtFanartHeight.Enabled = chkResizeFanart.Checked

        If Not chkResizeFanart.Checked Then
            txtFanartWidth.Text = String.Empty
            txtFanartHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtFanartWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFanartWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtFanartHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFanartHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtFanartWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFanartWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtFanartHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFanartHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizePoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizePoster.CheckedChanged
        Me.SetApplyButton(True)

        txtPosterWidth.Enabled = chkResizePoster.Checked
        txtPosterHeight.Enabled = chkResizePoster.Checked

        If Not chkResizePoster.Checked Then
            txtPosterWidth.Text = String.Empty
            txtPosterHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtPosterWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPosterWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtPosterHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPosterHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtPosterWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPosterWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtPosterHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPosterHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOFDBPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBPlot.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOFDBOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBOutline.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOFDBTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBTitle.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseCertForMPAA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCertForMPAA.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtAutoThumbs_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAutoThumbs.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtAutoThumbs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAutoThumbs.TextChanged
        Me.SetApplyButton(True)
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
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCastWithImg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCastWithImg.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkVideoTSParent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVideoTSParent.CheckedChanged
        Me.SetApplyButton(True)
        Me.sResult.NeedsUpdate = True
    End Sub

    Private Sub tvSettings_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvSettings.AfterSelect
        lblCurrent.Text = tvSettings.SelectedNode.Text

        pnlGeneral.Visible = False
        pnlXBMCCom.Visible = False
        pnlMovies.Visible = False
        pnlSources.Visible = False
        pnlScraper.Visible = False
        pnlTrailers.Visible = False
        pnlExtensions.Visible = False
        pnlImages.Visible = False
        pnlShows.Visible = False
        pnlTVSources.Visible = False
        pnlTVScraper.visible = False
        pnlTVImages.Visible = False

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
            Case "nTrailers"
                pnlTrailers.Visible = True
            Case "nExts"
                pnlExtensions.Visible = True
            Case "nImages"
                pnlImages.Visible = True
            Case "nTV"
                pnlShows.Visible = True
            Case "nTVSources"
                pnlTVSources.Visible = True
            Case "nTVScraper"
                pnlTVScraper.Visible = True
            Case "nTVImages"
                pnlTVImages.Visible = True
        End Select
    End Sub

    Private Sub lbXBMCCom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lbXBMCCom.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveXCom()
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
                    MsgBox(Master.eLang.GetString(559, "The name you are attempting to use for this XBMC installation is already in use. Please choose another."), MsgBoxStyle.Exclamation, Master.eLang.GetString(560, "Each name must be unique"))
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
                    Me.SetApplyButton(True)
                Else
                    MsgBox(Master.eLang.GetString(561, "You must enter a port for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(564, "Please Enter a Port"))
                    txtPort.Focus()
                End If
            Else
                MsgBox(Master.eLang.GetString(562, "You must enter an IP for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(565, "Please Enter an IP"))
                txtIP.Focus()
            End If
        Else
            MsgBox(Master.eLang.GetString(563, "You must enter a name for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(566, "Please Enter a Unique Name"))
            txtName.Focus()
        End If

    End Sub

    Private Sub btnEditCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditCom.Click
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        If Not String.IsNullOrEmpty(txtName.Text) Then

            For i As Integer = 0 To lbXBMCCom.Items.Count - 1
                If Not iSel = i AndAlso lbXBMCCom.Items(i).ToString.ToLower = Me.txtName.Text.ToLower Then
                    MsgBox(Master.eLang.GetString(559, "The name you are attempting to use for this XBMC installation is already in use. Please choose another."), MsgBoxStyle.Exclamation, Master.eLang.GetString(560, "Each name must be unique"))
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

                    Me.SetApplyButton(True)
                Else
                    MsgBox(Master.eLang.GetString(561, "You must enter a port for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(564, "Please Enter a Port"))
                    txtPort.Focus()
                End If
            Else
                MsgBox(Master.eLang.GetString(562, "You must enter an IP for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(565, "Please Enter an IP"))
                txtIP.Focus()
            End If

        Else
            MsgBox(Master.eLang.GetString(563, "You must enter a name for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(566, "Please Enter a Unique Name"))
            txtName.Focus()
        End If

    End Sub

    Private Sub btnRemoveCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveCom.Click
        Me.RemoveXCom()
    End Sub

    Private Sub chkOFDBGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOFDBGenre.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkNoSpoilers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoSpoilers.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtIMDBURL_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIMDBURL.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanExtrathumbs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanExtrathumbs.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnAddMovieExt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMovieExt.Click
        If Not String.IsNullOrEmpty(txtMovieExt.Text) Then
            If Not Strings.Left(txtMovieExt.Text, 1) = "." Then txtMovieExt.Text = String.Concat(".", txtMovieExt.Text)
            If Not lstMovieExts.Items.Contains(txtMovieExt.Text.ToLower) Then
                lstMovieExts.Items.Add(txtMovieExt.Text.ToLower)
                Me.SetApplyButton(True)
                Me.sResult.NeedsUpdate = True
                txtMovieExt.Text = String.Empty
                txtMovieExt.Focus()
            End If
        End If
    End Sub

    Private Sub btnRemMovieExt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemMovieExt.Click
        Me.RemoveMovieExt()
    End Sub

    Private Sub chkUpdates_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUpdates.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkXBMCCompatibility_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockGenre.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockRealStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockRealStudio.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockRating.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkLockTagline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTagline.CheckedChanged
        Me.SetApplyButton(True)
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
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtBDPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBDPath.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseMIDuration_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseMIDuration.CheckedChanged
        Me.SetApplyButton(True)
        Me.gbRTFormat.Enabled = Me.chkUseMIDuration.Checked
        If Not chkUseMIDuration.Checked Then
            Me.rbMins.Checked = True
        End If
    End Sub

    Private Sub chkMovieSubCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieSubCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieExtraCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieExtraCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkPersistImgCache_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPersistImgCache.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseImgCache_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseImgCache.CheckedChanged
        Me.chkPersistImgCache.Enabled = Me.chkUseImgCache.Checked
        Me.chkUseImgCacheUpdaters.Enabled = Me.chkUseImgCache.Checked
        If Not Me.chkUseImgCache.Checked Then
            Me.chkPersistImgCache.Checked = False
            Me.chkUseImgCacheUpdaters.Checked = False
        End If
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUseImgCacheUpdaters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseImgCacheUpdaters.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtSkipLessThan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSkipLessThan.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtSkipLessThan_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSkipLessThan.TextChanged
        Me.SetApplyButton(True)
        Me.sResult.NeedsUpdate = True
    End Sub

    Private Sub chkLockTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLockTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkDownloadTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDownloadTrailer.CheckedChanged
        Me.SetApplyButton(True)
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
            Me.cbTrailerQuality.SelectedIndex = -1
            Me.cbTrailerQuality.Enabled = False
        End If
    End Sub

    Private Sub lbTrailerSites_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lbTrailerSites.ItemCheck
        Me.SetApplyButton(True)
        If e.Index = 0 AndAlso (e.NewValue = CheckState.Checked OrElse Me.lbTrailerSites.GetItemChecked(1)) Then
            Me.cbTrailerQuality.Enabled = True
        ElseIf e.Index = 1 AndAlso (e.NewValue = CheckState.Checked OrElse Me.lbTrailerSites.GetItemChecked(0)) Then
            Me.cbTrailerQuality.Enabled = True
        Else
            If Me.lbTrailerSites.GetItemChecked(0) OrElse Me.lbTrailerSites.GetItemChecked(1) Then
                Me.cbTrailerQuality.Enabled = True
            Else
                Me.cbTrailerQuality.Enabled = False
            End If
        End If
    End Sub

    Private Sub lbGenre_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lbGenre.ItemCheck
        If e.Index = 0 Then
            For i As Integer = 1 To lbGenre.Items.Count - 1
                Me.lbGenre.SetItemChecked(i, False)
            Next
        Else
            Me.lbGenre.SetItemChecked(0, False)
        End If
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkUpdaterTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUpdaterTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtTimeout_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTimeout.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtTimeout_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTimeout.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSingleScrapeTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSingleScrapeTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tbPosterQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbPosterQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblPosterQual.Text = tbPosterQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblPosterQual
            Select Case True
                Case tbPosterQual.Value = 0
                    .ForeColor = Color.Black
                Case tbPosterQual.Value > 95 OrElse tbPosterQual.Value < 20
                    .ForeColor = Color.Red
                Case tbPosterQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbPosterQual.Value, 300 - tbPosterQual.Value, 0)
                Case tbPosterQual.Value >= 80 AndAlso tbPosterQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbPosterQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbPosterQual.Value - 20)), 0)
                Case tbPosterQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbPosterQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub tbFanartQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbFanartQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblFanartQual.Text = tbFanartQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblFanartQual
            Select Case True
                Case tbFanartQual.Value = 0
                    .ForeColor = Color.Black
                Case tbFanartQual.Value > 95 OrElse tbFanartQual.Value < 20
                    .ForeColor = Color.Red
                Case tbFanartQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbFanartQual.Value, 300 - tbFanartQual.Value, 0)
                Case tbFanartQual.Value >= 80 AndAlso tbFanartQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbFanartQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbFanartQual.Value - 20)), 0)
                Case tbFanartQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbFanartQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub chkFanartOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFanartOnly.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkNoDLTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDLTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkNoSaveImagesToNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoSaveImagesToNfo.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tcCleaner_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tcCleaner.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkWhitelistVideo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWhitelistVideo.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnAddWhitelist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWhitelist.Click
        If Not String.IsNullOrEmpty(Me.txtWhitelist.Text) Then
            If Not Strings.Left(txtWhitelist.Text, 1) = "." Then txtWhitelist.Text = String.Concat(".", txtWhitelist.Text)
            If Not lstWhitelist.Items.Contains(txtWhitelist.Text.ToLower) Then
                lstWhitelist.Items.Add(txtWhitelist.Text.ToLower)
                Me.SetApplyButton(True)
                txtWhitelist.Text = String.Empty
                txtWhitelist.Focus()
            End If
        End If
    End Sub

    Private Sub btnRemoveWhitelist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveWhitelist.Click
        If Me.lstWhitelist.Items.Count > 0 AndAlso Me.lstWhitelist.SelectedItems.Count > 0 Then
            While Me.lstWhitelist.SelectedItems.Count > 0
                lstWhitelist.Items.Remove(Me.lstWhitelist.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
        End If
    End Sub

    Private Sub chkInfoPanelAnim_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInfoPanelAnim.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowDims_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowDims.CheckedChanged
        Me.SetApplyButton(True)
    End Sub
    Private Sub chkShowGenresText_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowGenresText.CheckedChanged
        Me.SetApplyButton(True)
    End Sub
    Private Sub chkNoDisplayPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDisplayPoster.CheckedChanged
        Me.SetApplyButton(True)
        If Me.chkNoDisplayFanart.Checked AndAlso Me.chkNoDisplayPoster.Checked Then
            Me.chkShowDims.Enabled = False
            Me.chkShowDims.Checked = False
        Else
            Me.chkShowDims.Enabled = True
        End If
    End Sub

    Private Sub chkNoDisplayFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNoDisplayFanart.CheckedChanged
        Me.SetApplyButton(True)
        If Me.chkNoDisplayFanart.Checked AndAlso Me.chkNoDisplayPoster.Checked Then
            Me.chkShowDims.Enabled = False
            Me.chkShowDims.Checked = False
        Else
            Me.chkShowDims.Enabled = True
        End If
    End Sub

    Private Sub rbDashTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDashTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub rbBracketTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbBracketTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOutlineForPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOutlineForPlot.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkDeleteAllTrailers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeleteAllTrailers.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub rbMins_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMins.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub rbHM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbHM.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtFolderPattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolderPattern.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtFilePattern_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilePattern.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnRemoveNoStack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveNoStack.Click
        Me.RemoveNoStack()
    End Sub

    Private Sub btnAddNoStack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNoStack.Click
        If Not String.IsNullOrEmpty(txtNoStack.Text) Then
            If Not Strings.Left(txtNoStack.Text, 1) = "." Then txtNoStack.Text = String.Concat(".", txtNoStack.Text)
            If Not lstNoStack.Items.Contains(txtNoStack.Text) Then
                lstNoStack.Items.Add(txtNoStack.Text)
                Me.SetApplyButton(True)
                Me.sResult.NeedsUpdate = True
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
                    Me.sResult.NeedsUpdate = True
                    Me.SetApplyButton(True)
                End If
            End Using
        End If
    End Sub

    Private Sub lvTVSources_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvTVSources.DoubleClick
        If lvTVSources.SelectedItems.Count > 0 Then
            Using dTVSource As New dlgTVSource
                If dTVSource.ShowDialog(Convert.ToInt32(lvTVSources.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshTVSources()
                    Me.sResult.NeedsUpdate = True
                    Me.SetApplyButton(True)
                End If
            End Using
        End If
    End Sub

    Private Sub btnEditSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSource.Click
        If lvMovies.SelectedItems.Count > 0 Then
            Using dMovieSource As New dlgMovieSource
                If dMovieSource.ShowDialog(Convert.ToInt32(lvMovies.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshSources()
                    Me.sResult.NeedsUpdate = True
                    Me.SetApplyButton(True)
                End If
            End Using
        End If
    End Sub

    Private Sub btnEditTVSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditTVSource.Click
        If lvTVSources.SelectedItems.Count > 0 Then
            Using dTVSource As New dlgTVSource
                If dTVSource.ShowDialog(Convert.ToInt32(lvTVSources.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshTVSources()
                    Me.sResult.NeedsUpdate = True
                    Me.SetApplyButton(True)
                End If
            End Using
        End If
    End Sub

    Private Sub chkDisplayYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDisplayYear.CheckedChanged
        Me.sResult.NeedsRefresh = True
        Me.SetApplyButton(True)
    End Sub

    Private Sub rbETCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbETCustom.CheckedChanged
        Me.SetApplyButton(False)
        Me.txtETHeight.Enabled = Me.rbETCustom.Checked
        Me.txtETWidth.Enabled = Me.rbETCustom.Checked
        Me.chkETPadding.Enabled = Me.rbETCustom.Checked
    End Sub

    Private Sub rbETNative_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbETNative.CheckedChanged
        Me.SetApplyButton(False)
        If rbETNative.Checked Then
            Me.txtETHeight.Text = String.Empty
            Me.txtETWidth.Text = String.Empty
            Me.chkETPadding.Checked = False
        End If
    End Sub

    Private Sub txtETWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtETWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtETHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtETHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtETWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtETWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtETHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtETHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkETPadding_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkETPadding.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMovieNameMultiOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMovieNameMultiOnly.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnRemoveToken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveToken.Click
        Me.RemoveSortToken()
    End Sub

    Private Sub btnAddToken_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToken.Click
        If Not String.IsNullOrEmpty(txtSortToken.Text) Then
            If Not lstSortTokens.Items.Contains(txtSortToken.Text) Then
                lstSortTokens.Items.Add(txtSortToken.Text)
                Me.sResult.NeedsRefresh = True
                Me.SetApplyButton(True)
                txtSortToken.Text = String.Empty
                txtSortToken.Focus()
            End If
        End If
    End Sub

    Private Sub txtCheckTitleTol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCheckTitleTol.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtCheckTitleTol_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCheckTitleTol.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub chkCheckTitles_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCheckTitles.CheckedChanged
        Me.SetApplyButton(True)
        Me.txtCheckTitleTol.Enabled = Me.chkCheckTitles.Checked
        If Not Me.chkCheckTitles.Checked Then Me.txtCheckTitleTol.Text = String.Empty
    End Sub

    Private Sub chkAutoDetectVTS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoDetectVTS.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCleanDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCleanDB.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbLanguages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLanguages.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbIntLang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIntLang.SelectedIndexChanged
        Me.SetApplyButton(True)
        LangChanged = True
    End Sub

    Private Sub dlgSettings_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub


    Private Sub chkTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTitle.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkYear.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMPAA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMPAA.CheckedChanged
        Me.SetApplyButton(True)

        Me.chkCert.Enabled = Me.chkMPAA.Checked

        If Not Me.chkMPAA.Checked Then
            Me.chkCert.Checked = False
            Me.cbCert.Enabled = False
            Me.cbCert.SelectedIndex = -1
            Me.chkUseCertForMPAA.Enabled = False
            Me.chkUseCertForMPAA.Checked = False
        End If
    End Sub

    Private Sub chkRelease_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRelease.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkRuntime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRuntime.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRating.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkVotes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVotes.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkStudio_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStudio.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGenre.CheckedChanged
        Me.SetApplyButton(True)

        Me.txtGenreLimit.Enabled = Me.chkGenre.Checked

        If Not Me.chkGenre.Checked Then Me.txtGenreLimit.Text = "0"
    End Sub

    Private Sub chkTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkTagline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTagline.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOutline.CheckedChanged
        Me.SetApplyButton(True)

        Me.chkOutlineForPlot.Enabled = Me.chkOutline.Checked
        If Not Me.chkOutline.Checked Then Me.chkOutlineForPlot.Checked = False
    End Sub

    Private Sub chkPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPlot.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCast_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCast.CheckedChanged
        Me.SetApplyButton(True)

        Me.chkFullCast.Enabled = Me.chkCast.Checked
        Me.chkCastWithImg.Enabled = Me.chkCast.Checked
        Me.txtActorLimit.Enabled = Me.chkCast.Checked

        If Not chkCast.Checked Then
            Me.chkFullCast.Checked = False
            Me.chkCastWithImg.Checked = False
            Me.txtActorLimit.Text = "0"
        End If
    End Sub

    Private Sub chkDirector_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDirector.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkWriters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWriters.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkProducers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProducers.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMusicBy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMusicBy.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkCrew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCrew.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkTop250_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTop250.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtActorLimit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtActorLimit.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtGenreLimit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGenreLimit.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtActorLimit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtActorLimit.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtGenreLimit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGenreLimit.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingPoster.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingNFO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingNFO.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingTrailer.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingSubs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingSubs.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkMissingExtra_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMissingExtra.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnDLTrans_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDLTrans.Click
        Using dTranslationDL As New dlgTranslationDL
            If dTranslationDL.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.LoadIntLangs()
                Me.cbIntLang.SelectedItem = Master.eSettings.Language

                Me.LoadThemes()
                Me.cbMovieTheme.SelectedItem = Master.eSettings.MovieTheme
                Me.cbTVShowTheme.SelectedItem = Master.eSettings.TVShowTheme
                Me.cbEpTheme.SelectedItem = Master.eSettings.TVEpTheme
            End If
        End Using
    End Sub

    Private Sub chkRenameMulti_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRenameMulti.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkRenameSingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRenameSingle.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkAutoETSize_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoETSize.CheckedChanged
        Me.SetApplyButton(True)
        Me.cbAutoETSize.Enabled = Me.chkAutoETSize.Checked
    End Sub

    Private Sub cbAutoETSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAutoETSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbMovieTheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMovieTheme.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbTVShowTheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTVShowTheme.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbTVEpTheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEpTheme.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtDefFIExt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDefFIExt.TextChanged
        btnNewMetaDataFT.Enabled = Not String.IsNullOrEmpty(txtDefFIExt.Text) AndAlso Not Me.lstMetaData.Items.Contains(If(txtDefFIExt.Text.StartsWith("."), txtDefFIExt.Text, String.Concat(".", txtDefFIExt.Text)))
        If btnNewMetaDataFT.Enabled Then
            btnEditMetaDataFT.Enabled = False
            btnRemoveMetaDataFT.Enabled = False
        End If
    End Sub

    Private Sub btnNewMetaDataFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewMetaDataFT.Click
        If Not txtDefFIExt.Text.StartsWith(".") Then txtDefFIExt.Text = String.Concat(".", txtDefFIExt.Text)
        Using dEditMeta As New dlgFileInfo
            Dim fi As New MediaInfo.Fileinfo
            fi = dEditMeta.ShowDialog(fi, False)
            If Not fi Is Nothing Then
                Dim m As New emmSettings.MetadataPerType
                m.FileType = txtDefFIExt.Text
                m.MetaData = New MediaInfo.Fileinfo
                m.MetaData = fi
                Meta.Add(m)
                LoadMetadata()
                Me.SetApplyButton(True)
                Me.txtDefFIExt.Text = String.Empty
                Me.txtDefFIExt.Focus()
            End If
        End Using
    End Sub

    Private Sub btnEditMetaDataFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditMetaDataFT.Click
        Using dEditMeta As New dlgFileInfo
            Dim fi As New MediaInfo.Fileinfo
            For Each x As emmSettings.MetadataPerType In Meta
                If x.FileType = lstMetaData.SelectedItems(0).ToString Then
                    fi = dEditMeta.ShowDialog(x.MetaData, False)
                    If Not fi Is Nothing Then
                        Meta.Remove(x)
                        Dim m As New emmSettings.MetadataPerType
                        m.FileType = x.FileType
                        m.MetaData = New MediaInfo.Fileinfo
                        m.MetaData = fi
                        Meta.Add(m)
                        LoadMetadata()
                        Me.SetApplyButton(True)
                    End If
                    Exit For
                End If
            Next
        End Using
    End Sub

    Private Sub btnRemoveMetaDataFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveMetaDataFT.Click
        Me.RemoveMetaData()
    End Sub

    Private Sub lstMetaData_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstMetaData.DoubleClick
        If Me.lstMetaData.SelectedItems.Count > 0 Then
            Using dEditMeta As New dlgFileInfo
                Dim fi As New MediaInfo.Fileinfo
                For Each x As emmSettings.MetadataPerType In Meta
                    If x.FileType = lstMetaData.SelectedItems(0).ToString Then
                        fi = dEditMeta.ShowDialog(x.MetaData, False)
                        If Not fi Is Nothing Then
                            Meta.Remove(x)
                            Dim m As New emmSettings.MetadataPerType
                            m.FileType = x.FileType
                            m.MetaData = New MediaInfo.Fileinfo
                            m.MetaData = fi
                            Meta.Add(m)
                            LoadMetadata()
                            Me.SetApplyButton(True)
                        End If
                        Exit For
                    End If
                Next
            End Using
        End If
    End Sub

    Private Sub lstMetaData_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstMetaData.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveMetaData()
    End Sub

    Private Sub lstMetadata_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstMetaData.SelectedIndexChanged
        If lstMetaData.SelectedItems.Count > 0 Then
            btnEditMetaDataFT.Enabled = True
            btnRemoveMetaDataFT.Enabled = True
            txtDefFIExt.Text = ""
        Else
            btnEditMetaDataFT.Enabled = False
            btnRemoveMetaDataFT.Enabled = False
        End If
    End Sub

    Private Sub chkIFOScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIFOScan.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub lstSortTokens_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstSortTokens.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveSortToken()
    End Sub

    Private Sub lstFilters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstFilters.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveFilter()
    End Sub

    Private Sub lstShowFilters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstShowFilters.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveShowFilter()
    End Sub

    Private Sub lstEpFilters_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstEpFilters.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveEpFilter()
    End Sub

    Private Sub lstMovieExts_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstMovieExts.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveMovieExt()
    End Sub

    Private Sub lstNoStack_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstNoStack.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveNoStack()
    End Sub

    Private Sub lvMovies_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvMovies.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveMovieSource()
    End Sub

    Private Sub lvTVSources_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvTVSources.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveTVSource()
    End Sub

    Private Sub chkIgnoreLastScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnoreLastScan.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkTVCleanDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTVCleanDB.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkTVIgnoreLastScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTVIgnoreLastScan.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkYAMJCompatibleSets_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkYAMJCompatibleSets.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnRemoveShowRegex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveShowRegex.Click
        Me.RemoveRegex()
    End Sub

    Private Sub btnEditShowRegex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditShowRegex.Click
        If Me.lvShowRegex.SelectedItems.Count > 0 Then Me.EditShowRegex(lvShowRegex.SelectedItems(0))
    End Sub

    Private Sub lvShowRegex_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvShowRegex.DoubleClick
        If Me.lvShowRegex.SelectedItems.Count > 0 Then Me.EditShowRegex(lvShowRegex.SelectedItems(0))
    End Sub

    Private Sub lvShowRegex_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvShowRegex.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.btnAddShowRegex.Tag.ToString) Then Me.ClearRegex()
    End Sub

    Private Sub btnAddShowRegex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddShowRegex.Click
        If String.IsNullOrEmpty(Me.btnAddShowRegex.Tag.ToString) Then
            Dim lID = (From lRegex As emmSettings.TVShowRegEx In Me.ShowRegex Select lRegex.ID).Max
            Me.ShowRegex.Add(New emmSettings.TVShowRegEx With {.ID = Convert.ToInt32(lID) + 1, .SeasonRegex = Me.txtSeasonRegex.Text, .SeasonFromDirectory = Convert.ToBoolean(Me.cboSeasonRetrieve.SelectedIndex), .EpisodeRegex = Me.txtEpRegex.Text, .EpisodeRetrieve = DirectCast(Me.cboEpRetrieve.SelectedIndex, emmSettings.EpRetrieve)})
        Else
            Dim selRex = From lRegex As emmSettings.TVShowRegEx In Me.ShowRegex Where lRegex.ID = Convert.ToInt32(Me.btnAddShowRegex.Tag)
            If selRex.Count > 0 Then
                selRex(0).SeasonRegex = Me.txtSeasonRegex.Text
                selRex(0).SeasonFromDirectory = Convert.ToBoolean(Me.cboSeasonRetrieve.SelectedIndex)
                selRex(0).EpisodeRegex = Me.txtEpRegex.Text
                selRex(0).EpisodeRetrieve = DirectCast(Me.cboEpRetrieve.SelectedIndex, emmSettings.EpRetrieve)
            End If
        End If

        Me.ClearRegex()
        Me.SetApplyButton(True)
        Me.LoadShowRegex()
    End Sub

    Private Sub txtSeasonRegex_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSeasonRegex.TextChanged
        Me.ValidateRegex()
    End Sub

    Private Sub txtEpRegex_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEpRegex.TextChanged
        Me.ValidateRegex()
    End Sub

    Private Sub cboSeasonRetrieve_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasonRetrieve.SelectedIndexChanged
        Me.ValidateRegex()
    End Sub

    Private Sub cboEpRetrieve_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEpRetrieve.SelectedIndexChanged
        Me.ValidateRegex()
    End Sub

    Private Sub lvShowRegex_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvShowRegex.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveRegex()
    End Sub

    Private Sub cbTrailerQuality_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTrailerQuality.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tbShowFanartQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbShowFanartQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblShowFanartQual.Text = tbShowFanartQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblShowFanartQual
            Select Case True
                Case tbShowFanartQual.Value = 0
                    .ForeColor = Color.Black
                Case tbShowFanartQual.Value > 95 OrElse tbShowFanartQual.Value < 20
                    .ForeColor = Color.Red
                Case tbShowFanartQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbShowFanartQual.Value, 300 - tbShowFanartQual.Value, 0)
                Case tbShowFanartQual.Value >= 80 AndAlso tbShowFanartQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbShowFanartQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbShowFanartQual.Value - 20)), 0)
                Case tbShowFanartQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbShowFanartQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub txtShowFanartWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtShowFanartWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtShowFanartWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShowFanartWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtShowFanartHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtShowFanartHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtShowFanartHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShowFanartHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizeShowFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeShowFanart.CheckedChanged
        Me.SetApplyButton(True)

        txtShowFanartWidth.Enabled = chkResizeShowFanart.Checked
        txtShowFanartHeight.Enabled = chkResizeShowFanart.Checked

        If Not chkResizeShowFanart.Checked Then
            txtShowFanartWidth.Text = String.Empty
            txtShowFanartHeight.Text = String.Empty
        End If
    End Sub

    Private Sub chkOverwriteShowFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteShowFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowFanartOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowFanartOnly.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub cbShowFanartSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowFanartSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chShowPosterSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowPosterSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteShowPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteShowPoster.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizeShowPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeShowPoster.CheckedChanged
        Me.SetApplyButton(True)

        txtShowPosterWidth.Enabled = chkResizeShowPoster.Checked
        txtShowPosterHeight.Enabled = chkResizeShowPoster.Checked

        If Not chkResizeShowPoster.Checked Then
            txtShowPosterWidth.Text = String.Empty
            txtShowPosterHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtShowPosterWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtShowPosterWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtShowPosterWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShowPosterWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtShowPosterHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtShowPosterHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtShowPosterHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShowPosterHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tbShowPosterQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbShowPosterQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblShowPosterQual.Text = tbShowPosterQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblShowPosterQual
            Select Case True
                Case tbShowPosterQual.Value = 0
                    .ForeColor = Color.Black
                Case tbShowPosterQual.Value > 95 OrElse tbShowPosterQual.Value < 20
                    .ForeColor = Color.Red
                Case tbShowPosterQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbShowPosterQual.Value, 300 - tbShowPosterQual.Value, 0)
                Case tbShowPosterQual.Value >= 80 AndAlso tbShowPosterQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbShowPosterQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbShowPosterQual.Value - 20)), 0)
                Case tbShowPosterQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbShowPosterQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub cbRatingRegion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRatingRegion.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowSeasonAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowSeasonAll.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowFolderJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPosterTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPosterJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowDashFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowDashFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowDotFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowDotFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonXXTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonXXTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonXTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonXTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonPosterTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonPosterTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonPosterJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonPosterJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonNameTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonNameTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonNameJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonNameJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonFanartJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonFanartJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonDashFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonDashFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonDotFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonDotFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeTBN_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeTBN.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeDashFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeDashFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeDotFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeDotFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowPosterCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPosterCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowFanartCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowFanartCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkShowNfoCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowNfoCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonPosterCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonPosterCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSeasonFanartCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonFanartCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodePosterCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodePosterCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeFanartCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeFanartCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEpisodeNfoCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEpisodeNfoCol.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chEpPosterSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEpPosterSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteEpPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteEpPoster.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizeEpPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeEpPoster.CheckedChanged
        Me.SetApplyButton(True)

        txtEpPosterWidth.Enabled = chkResizeEpPoster.Checked
        txtEpPosterHeight.Enabled = chkResizeEpPoster.Checked

        If Not chkResizeEpFanart.Checked Then
            txtEpPosterWidth.Text = String.Empty
            txtEpPosterHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtEpPosterWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEpPosterWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtEpPosterWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEpPosterWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtEpPosterHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEpPosterHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtEpPosterHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEpPosterHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tbEpPosterQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEpPosterQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblEpPosterQual.Text = tbEpPosterQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblEpPosterQual
            Select Case True
                Case tbEpPosterQual.Value = 0
                    .ForeColor = Color.Black
                Case tbEpPosterQual.Value > 95 OrElse tbEpPosterQual.Value < 20
                    .ForeColor = Color.Red
                Case tbEpPosterQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbEpPosterQual.Value, 300 - tbEpPosterQual.Value, 0)
                Case tbEpPosterQual.Value >= 80 AndAlso tbEpPosterQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbEpPosterQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbEpPosterQual.Value - 20)), 0)
                Case tbEpPosterQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbEpPosterQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub chEpFanartSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEpFanartSize.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkOverwriteEpFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverwriteEpFanart.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkResizeEpFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResizeEpFanart.CheckedChanged
        Me.SetApplyButton(True)

        txtEpFanartWidth.Enabled = chkResizeEpFanart.Checked
        txtEpFanartHeight.Enabled = chkResizeEpFanart.Checked

        If Not chkResizeEpFanart.Checked Then
            txtEpFanartWidth.Text = String.Empty
            txtEpFanartHeight.Text = String.Empty
        End If
    End Sub

    Private Sub txtEpFanartWidth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEpFanartWidth.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtEpFanartWidth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEpFanartWidth.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtEpFanartHeight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEpFanartHeight.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtEpFanartHeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEpFanartHeight.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub tbEpFanartQual_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbEpFanartQual.ValueChanged
        Me.SetApplyButton(True)
        Me.lblEpFanartQual.Text = tbEpFanartQual.Value.ToString
        'change text color to indicate recommendations
        With Me.lblEpFanartQual
            Select Case True
                Case tbEpFanartQual.Value = 0
                    .ForeColor = Color.Black
                Case tbEpFanartQual.Value > 95 OrElse tbEpFanartQual.Value < 20
                    .ForeColor = Color.Red
                Case tbEpFanartQual.Value > 85
                    .ForeColor = Color.FromArgb(255, 155 + tbEpFanartQual.Value, 300 - tbEpFanartQual.Value, 0)
                Case tbEpFanartQual.Value >= 80 AndAlso tbEpFanartQual.Value <= 85
                    .ForeColor = Color.Blue
                Case tbEpFanartQual.Value <= 50
                    .ForeColor = Color.FromArgb(255, 255, Convert.ToInt32(8.5 * (tbEpFanartQual.Value - 20)), 0)
                Case tbEpFanartQual.Value < 80
                    .ForeColor = Color.FromArgb(255, Convert.ToInt32(255 - (8.5 * (tbEpFanartQual.Value - 50))), 255, 0)
            End Select
        End With
    End Sub

    Private Sub chkSeasonFolderJPG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSeasonFolderJPG.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkEnableProxy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableProxy.CheckedChanged
        Me.SetApplyButton(True)
        Me.txtProxyURI.Enabled = Me.chkEnableProxy.Checked
        Me.txtProxyPort.Enabled = Me.chkEnableProxy.Checked
        Me.gbCreds.Enabled = Me.chkEnableProxy.Checked

        If Not Me.chkEnableProxy.Checked Then
            Me.txtProxyURI.Text = String.Empty
            Me.txtProxyPort.Text = String.Empty
            Me.chkEnableCredentials.Checked = False
            Me.txtProxyUsername.Text = String.Empty
            Me.txtProxyPassword.Text = String.Empty
            Me.txtProxyDomain.Text = String.Empty
        End If
    End Sub

    Private Sub chkEnableCredentials_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableCredentials.CheckedChanged
        Me.SetApplyButton(True)
        Me.txtProxyUsername.Enabled = Me.chkEnableCredentials.Checked
        Me.txtProxyPassword.Enabled = Me.chkEnableCredentials.Checked
        Me.txtProxyDomain.Enabled = Me.chkEnableCredentials.Checked

        If Not Me.chkEnableCredentials.Checked Then
            Me.txtProxyUsername.Text = String.Empty
            Me.txtProxyPassword.Text = String.Empty
            Me.txtProxyDomain.Text = String.Empty
        End If
    End Sub

    Private Sub txtProxyPort_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtProxyPort.KeyPress
        e.Handled = StringManip.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtProxyPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyPort.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtProxyUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyUsername.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtProxyPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyPassword.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtProxyDomain_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyDomain.TextChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSourceFromFolder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSourceFromFolder.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub chkSortBeforeScan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSortBeforeScan.CheckedChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub btnTVLanguageFetch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTVLanguageFetch.Click
        Dim tvdbLang As New TVDB.Scraper
        Me.cbTVLanguage.DataSource = tvdbLang.GetLangs
        Me.cbTVLanguage.DisplayMember = "LongLang"
        Me.cbTVLanguage.ValueMember = "ShortLang"
    End Sub

    Private Sub cbTVLanguage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTVLanguage.SelectedIndexChanged
        Me.SetApplyButton(True)
    End Sub

    Private Sub txtTVDBMirror_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTVDBMirror.TextChanged
        Me.SetApplyButton(True)
    End Sub
#End Region '*** Form/Controls


#Region "Routines/Functions"

    ' ########################################
    ' ########## ROUTINES/FUNCTIONS ##########
    ' ########################################

    Private Sub SaveSettings()

        Try
            Master.eSettings.FilterCustom.Clear()
            Master.eSettings.FilterCustom.AddRange(Me.lstFilters.Items.OfType(Of String).ToList)
            If Master.eSettings.FilterCustom.Count <= 0 Then Master.eSettings.NoFilters = True
            Master.eSettings.ProperCase = Me.chkProperCase.Checked

            Master.eSettings.ShowFilterCustom.Clear()
            Master.eSettings.ShowFilterCustom.AddRange(Me.lstShowFilters.Items.OfType(Of String).ToList)
            If Master.eSettings.ShowFilterCustom.Count <= 0 Then Master.eSettings.NoShowFilters = True
            Master.eSettings.ShowProperCase = Me.chkShowProperCase.Checked

            Master.eSettings.EpFilterCustom.Clear()
            Master.eSettings.EpFilterCustom.AddRange(Me.lstEpFilters.Items.OfType(Of String).ToList)
            If Master.eSettings.EpFilterCustom.Count <= 0 Then Master.eSettings.NoEpFilters = True
            Master.eSettings.EpProperCase = Me.chkEpProperCase.Checked

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
                Master.eSettings.CleanWhitelistExts.AddRange(Me.lstWhitelist.Items.OfType(Of String).ToList)
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
            Master.eSettings.OverwriteNfo = Me.chkOverwriteNfo.Checked
            Master.eSettings.XBMCComs = Me.XComs
            Master.eSettings.ValidExts.Clear()
            Master.eSettings.ValidExts.AddRange(lstMovieExts.Items.OfType(Of String).ToList)
            Master.eSettings.NoStackExts.Clear()
            Master.eSettings.NoStackExts.AddRange(lstNoStack.Items.OfType(Of String).ToList)
            Master.eSettings.CheckUpdates = chkUpdates.Checked
            Master.eSettings.InfoPanelAnim = chkInfoPanelAnim.Checked
            Master.eSettings.YAMJSetsCompatible = chkYAMJCompatibleSets.Checked
            Master.eSettings.CertificationLang = Me.cbCert.Text
            If Not String.IsNullOrEmpty(Me.cbCert.Text) Then
                Master.eSettings.UseCertForMPAA = Me.chkUseCertForMPAA.Checked
            Else
                Master.eSettings.UseCertForMPAA = False
            End If
            Master.eSettings.ForceTitle = Me.cbForce.Text
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
            Master.eSettings.PreferredPosterSize = DirectCast(Me.cbPosterSize.SelectedIndex, Master.PosterSize)
            Master.eSettings.PreferredFanartSize = DirectCast(Me.cbFanartSize.SelectedIndex, Master.FanartSize)
            Master.eSettings.PreferredShowPosterSize = DirectCast(Me.cbshowPosterSize.SelectedIndex, Master.PosterSize)
            Master.eSettings.PreferredShowFanartSize = DirectCast(Me.cbShowFanartSize.SelectedIndex, Master.FanartSize)
            Master.eSettings.PreferredEpPosterSize = DirectCast(Me.cbEpPosterSize.SelectedIndex, Master.PosterSize)
            Master.eSettings.PreferredEpFanartSize = DirectCast(Me.cbEpFanartSize.SelectedIndex, Master.FanartSize)
            Master.eSettings.AutoET = Me.chkAutoETSize.Checked
            Master.eSettings.AutoETSize = DirectCast(Me.cbAutoETSize.SelectedIndex, Master.FanartSize)
            Master.eSettings.FanartPrefSizeOnly = Me.chkFanartOnly.Checked
            Master.eSettings.ShowFanartPrefSizeOnly = Me.chkShowFanartOnly.Checked
            Master.eSettings.EpFanartPrefSizeOnly = Me.chkEpFanartOnly.Checked
            Master.eSettings.PosterQuality = Me.tbPosterQual.Value
            Master.eSettings.FanartQuality = Me.tbFanartQual.Value
            Master.eSettings.OverwritePoster = Me.chkOverwritePoster.Checked
            Master.eSettings.OverwriteFanart = Me.chkOverwriteFanart.Checked
            Master.eSettings.ShowPosterQuality = Me.tbShowPosterQual.Value
            Master.eSettings.ShowFanartQuality = Me.tbShowFanartQual.Value
            Master.eSettings.OverwriteShowPoster = Me.chkOverwriteShowPoster.Checked
            Master.eSettings.OverwriteShowFanart = Me.chkOverwriteShowFanart.Checked
            Master.eSettings.EpPosterQuality = Me.tbEpPosterQual.Value
            Master.eSettings.EpFanartQuality = Me.tbEpFanartQual.Value
            Master.eSettings.OverwriteEpPoster = Me.chkOverwriteEpPoster.Checked
            Master.eSettings.OverwriteEpFanart = Me.chkOverwriteEpFanart.Checked
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
            Master.eSettings.MovieNameMultiOnly = Me.chkMovieNameMultiOnly.Checked
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
            Master.eSettings.FanartHeight = If(Not String.IsNullOrEmpty(Me.txtFanartHeight.Text), Convert.ToInt32(Me.txtFanartHeight.Text), 0)
            Master.eSettings.FanartWidth = If(Not String.IsNullOrEmpty(Me.txtFanartWidth.Text), Convert.ToInt32(Me.txtFanartWidth.Text), 0)
            Master.eSettings.ResizePoster = Me.chkResizePoster.Checked
            Master.eSettings.PosterHeight = If(Not String.IsNullOrEmpty(Me.txtPosterHeight.Text), Convert.ToInt32(Me.txtPosterHeight.Text), 0)
            Master.eSettings.PosterWidth = If(Not String.IsNullOrEmpty(Me.txtPosterWidth.Text), Convert.ToInt32(Me.txtPosterWidth.Text), 0)
            Master.eSettings.ResizeShowFanart = Me.chkResizeShowFanart.Checked
            Master.eSettings.ShowFanartHeight = If(Not String.IsNullOrEmpty(Me.txtShowFanartHeight.Text), Convert.ToInt32(Me.txtShowFanartHeight.Text), 0)
            Master.eSettings.ShowFanartWidth = If(Not String.IsNullOrEmpty(Me.txtShowFanartWidth.Text), Convert.ToInt32(Me.txtShowFanartWidth.Text), 0)
            Master.eSettings.ResizeShowPoster = Me.chkResizeShowPoster.Checked
            Master.eSettings.ShowPosterHeight = If(Not String.IsNullOrEmpty(Me.txtShowPosterHeight.Text), Convert.ToInt32(Me.txtShowPosterHeight.Text), 0)
            Master.eSettings.ShowPosterWidth = If(Not String.IsNullOrEmpty(Me.txtShowPosterWidth.Text), Convert.ToInt32(Me.txtShowPosterWidth.Text), 0)
            Master.eSettings.ResizeEpFanart = Me.chkResizeEpFanart.Checked
            Master.eSettings.EpFanartHeight = If(Not String.IsNullOrEmpty(Me.txtEpFanartHeight.Text), Convert.ToInt32(Me.txtEpFanartHeight.Text), 0)
            Master.eSettings.EpFanartWidth = If(Not String.IsNullOrEmpty(Me.txtEpFanartWidth.Text), Convert.ToInt32(Me.txtEpFanartWidth.Text), 0)
            Master.eSettings.ResizeEpPoster = Me.chkResizeEpPoster.Checked
            Master.eSettings.EpPosterHeight = If(Not String.IsNullOrEmpty(Me.txtEpPosterHeight.Text), Convert.ToInt32(Me.txtEpPosterHeight.Text), 0)
            Master.eSettings.EpPosterWidth = If(Not String.IsNullOrEmpty(Me.txtEpPosterWidth.Text), Convert.ToInt32(Me.txtEpPosterWidth.Text), 0)
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

            If Me.cbTrailerQuality.SelectedValue IsNot Nothing Then
                Master.eSettings.PreferredTrailerQuality = DirectCast(Me.cbTrailerQuality.SelectedValue, Master.TrailerQuality)
            End If

            Master.eSettings.TrailerSites.Clear()
            If Me.lbTrailerSites.CheckedItems.Count > 0 Then
                For Each iTrailer As Integer In Me.lbTrailerSites.CheckedIndices
                    Master.eSettings.TrailerSites.Add(DirectCast(iTrailer, Master.TrailerPages))
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
                If Me.lbGenre.CheckedItems.Contains(String.Format("[{0}]", Master.eLang.GetString(569, Master.eLang.All))) Then
                    Master.eSettings.GenreFilter = String.Format("[{0}]", Master.eLang.GetString(569, Master.eLang.All))
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
            Master.eSettings.DisplayYear = Me.chkDisplayYear.Checked
            Master.eSettings.ETNative = Me.rbETNative.Checked
            Dim iWidth As Integer = If(Me.txtETWidth.Text.Length > 0, Convert.ToInt32(Me.txtETWidth.Text), 0)
            Dim iHeight As Integer = If(Me.txtETHeight.Text.Length > 0, Convert.ToInt32(Me.txtETHeight.Text), 0)
            If Me.rbETCustom.Checked AndAlso iWidth > 0 AndAlso iHeight > 0 Then
                Master.eSettings.ETWidth = iWidth
                Master.eSettings.ETHeight = iHeight
                Master.eSettings.ETPadding = Me.chkETPadding.Checked
            Else
                Master.eSettings.ETWidth = 0
                Master.eSettings.ETHeight = 0
                Master.eSettings.ETPadding = False
            End If

            Master.eSettings.SortTokens.Clear()
            Master.eSettings.SortTokens.AddRange(lstSortTokens.Items.OfType(Of String).ToList)
            If Master.eSettings.SortTokens.Count <= 0 Then Master.eSettings.NoTokens = True

            Master.eSettings.LevTolerance = If(Not String.IsNullOrEmpty(Me.txtCheckTitleTol.Text), Convert.ToInt32(Me.txtCheckTitleTol.Text), 0)
            Master.eSettings.AutoDetectVTS = Me.chkAutoDetectVTS.Checked
            Master.eSettings.FlagLang = If(Me.cbLanguages.Text = Master.eLang.Disabled, String.Empty, Me.cbLanguages.Text)
            If Not cbIntLang.Text = Master.eSettings.Language Then Master.eLang.LoadLanguage(cbIntLang.Text)
            Master.eSettings.Language = Me.cbIntLang.Text
            Me.lbGenre.Items.Clear()
            LoadGenreLangs()
            FillGenres()
            Master.eSettings.FieldTitle = Me.chkTitle.Checked
            Master.eSettings.FieldYear = Me.chkYear.Checked
            Master.eSettings.FieldMPAA = Me.chkMPAA.Checked
            Master.eSettings.FieldRelease = Me.chkRelease.Checked
            Master.eSettings.FieldRuntime = Me.chkRuntime.Checked
            Master.eSettings.FieldRating = Me.chkRating.Checked
            Master.eSettings.FieldVotes = Me.chkVotes.Checked
            Master.eSettings.FieldStudio = Me.chkStudio.Checked
            Master.eSettings.FieldGenre = Me.chkGenre.Checked
            Master.eSettings.FieldTrailer = Me.chkTrailer.Checked
            Master.eSettings.FieldTagline = Me.chkTagline.Checked
            Master.eSettings.FieldOutline = Me.chkOutline.Checked
            Master.eSettings.FieldPlot = Me.chkPlot.Checked
            Master.eSettings.FieldCast = Me.chkCast.Checked
            Master.eSettings.FieldDirector = Me.chkDirector.Checked
            Master.eSettings.FieldWriters = Me.chkWriters.Checked
            Master.eSettings.FieldProducers = Me.chkProducers.Checked
            Master.eSettings.FieldMusic = Me.chkMusicBy.Checked
            Master.eSettings.FieldCrew = Me.chkCrew.Checked
            Master.eSettings.Field250 = Me.chkTop250.Checked

            If Not String.IsNullOrEmpty(Me.txtActorLimit.Text) Then
                Master.eSettings.ActorLimit = Convert.ToInt32(Me.txtActorLimit.Text)
            Else
                Master.eSettings.ActorLimit = 0
            End If
            If Not String.IsNullOrEmpty(Me.txtGenreLimit.Text) Then
                Master.eSettings.GenreLimit = Convert.ToInt32(Me.txtGenreLimit.Text)
            Else
                Master.eSettings.GenreLimit = 0
            End If

            Master.eSettings.MissingFilterPoster = Me.chkMissingPoster.Checked
            Master.eSettings.MissingFilterFanart = Me.chkMissingFanart.Checked
            Master.eSettings.MissingFilterNFO = Me.chkMissingNFO.Checked
            Master.eSettings.MissingFilterTrailer = Me.chkMissingTrailer.Checked
            Master.eSettings.MissingFilterSubs = Me.chkMissingSubs.Checked
            Master.eSettings.MissingFilterExtras = Me.chkMissingExtra.Checked

            Master.eSettings.AutoRenameMulti = Me.chkRenameMulti.Checked
            Master.eSettings.AutoRenameSingle = Me.chkRenameSingle.Checked
            Master.eSettings.MovieTheme = Me.cbMovieTheme.Text
            Master.eSettings.TVShowTheme = Me.cbTVShowTheme.Text
            Master.eSettings.TVEpTheme = Me.cbEpTheme.Text
            Master.eSettings.EnableIFOScan = Me.chkIFOScan.Checked
            Master.eSettings.CleanDB = Me.chkCleanDB.Checked
            Master.eSettings.IgnoreLastScan = Me.chkIgnoreLastScan.Checked
            Master.eSettings.TVCleanDB = Me.chkTVCleanDB.Checked
            Master.eSettings.TVIgnoreLastScan = Me.chkTVIgnoreLastScan.Checked
            Master.eSettings.TVShowRegexes = Me.ShowRegex
            Master.eSettings.ShowRatingRegion = Me.cbRatingRegion.Text
            Master.eSettings.ShowSeasonAll = Me.chkShowSeasonAll.Checked
            Master.eSettings.ShowFolderJPG = Me.chkShowFolderJPG.Checked
            Master.eSettings.ShowPosterTBN = Me.chkShowPosterTBN.Checked
            Master.eSettings.ShowPosterJPG = Me.chkShowPosterJPG.Checked
            Master.eSettings.ShowFanartJPG = Me.chkShowFanartJPG.Checked
            Master.eSettings.ShowDashFanart = Me.chkShowDashFanart.Checked
            Master.eSettings.ShowDotFanart = Me.chkShowDotFanart.Checked
            Master.eSettings.SeasonXX = Me.chkSeasonXXTBN.Checked
            Master.eSettings.SeasonX = Me.chkSeasonXTBN.Checked
            Master.eSettings.SeasonPosterTBN = Me.chkSeasonPosterTBN.Checked
            Master.eSettings.SeasonPosterJPG = Me.chkSeasonPosterJPG.Checked
            Master.eSettings.SeasonNameTBN = Me.chkSeasonNameTBN.Checked
            Master.eSettings.SeasonNameJPG = Me.chkSeasonNameJPG.Checked
            Master.eSettings.SeasonFolderJPG = Me.chkSeasonFolderJPG.Checked
            Master.eSettings.SeasonFanartJPG = Me.chkSeasonFanartJPG.Checked
            Master.eSettings.SeasonDashFanart = Me.chkSeasonDashFanart.Checked
            Master.eSettings.SeasonDotFanart = Me.chkSeasonDotFanart.Checked
            Master.eSettings.EpisodeTBN = Me.chkEpisodeTBN.Checked
            Master.eSettings.EpisodeJPG = Me.chkEpisodeJPG.Checked
            Master.eSettings.EpisodeDashFanart = Me.chkEpisodeDashFanart.Checked
            Master.eSettings.EpisodeDotFanart = Me.chkEpisodeDotFanart.Checked
            Master.eSettings.ShowPosterCol = Me.chkShowPosterCol.Checked
            Master.eSettings.ShowFanartCol = Me.chkShowFanartCol.Checked
            Master.eSettings.ShowNfoCol = Me.chkShowNfoCol.Checked
            Master.eSettings.SeasonPosterCol = Me.chkSeasonPosterCol.Checked
            Master.eSettings.SeasonFanartCol = Me.chkSeasonFanartCol.Checked
            Master.eSettings.EpisodePosterCol = Me.chkEpisodePosterCol.Checked
            Master.eSettings.EpisodeFanartCol = Me.chkEpisodeFanartCol.Checked
            Master.eSettings.EpisodeNfoCol = Me.chkEpisodeNfoCol.Checked
            Master.eSettings.SourceFromFolder = Me.chkSourceFromFolder.Checked
            Master.eSettings.SortBeforeScan = Me.chkSortBeforeScan.Checked
            Master.eSettings.TVDBLanguage = Me.cbTVLanguage.SelectedValue.ToString
            Master.eSettings.TVDBLanguages = DirectCast(Me.cbTVLanguage.DataSource, List(Of TVDB.TVDBLanguage))
            If Not String.IsNullOrEmpty(Me.txtTVDBMirror.Text) Then
                Master.eSettings.TVDBMirror = Strings.Replace(Me.txtTVDBMirror.Text, "http://", String.Empty)
            Else
                Master.eSettings.TVDBMirror = "thetvdb.com"
            End If

            If Not String.IsNullOrEmpty(Me.txtProxyURI.Text) AndAlso Not String.IsNullOrEmpty(Me.txtProxyPort.Text) Then
                Master.eSettings.ProxyURI = Me.txtProxyURI.Text
                Master.eSettings.ProxyPort = Convert.ToInt32(Me.txtProxyPort.Text)

                If Not String.IsNullOrEmpty(Me.txtProxyUsername.Text) AndAlso Not String.IsNullOrEmpty(Me.txtProxyPassword.Text) Then
                    Master.eSettings.ProxyCreds.UserName = Me.txtProxyUsername.Text
                    Master.eSettings.ProxyCreds.Password = Me.txtProxyPassword.Text
                    Master.eSettings.ProxyCreds.Domain = Me.txtProxyDomain.Text
                Else
                    Master.eSettings.ProxyCreds = New NetworkCredential
                End If
            Else
                Master.eSettings.ProxyURI = String.Empty
                Master.eSettings.ProxyPort = -1
            End If
            Master.eSettings.Save()

            Master.CreateDefaultOptions()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillSettings()

        Try
            Me.lstFilters.Items.AddRange(Master.eSettings.FilterCustom.ToArray)
            Me.chkProperCase.Checked = Master.eSettings.ProperCase
            Me.lstShowFilters.Items.AddRange(Master.eSettings.ShowFilterCustom.ToArray)
            Me.chkShowProperCase.Checked = Master.eSettings.ShowProperCase
            Me.lstEpFilters.Items.AddRange(Master.eSettings.EpFilterCustom.ToArray)
            Me.chkEpProperCase.Checked = Master.eSettings.EpProperCase
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
            Me.chkYAMJCompatibleSets.Checked = Master.eSettings.YAMJSetsCompatible

            Me.XComs = Master.eSettings.XBMCComs
            Me.LoadXComs()

            Me.chkLogErrors.Checked = Master.eSettings.LogErrors
            Me.lstMovieExts.Items.AddRange(Master.eSettings.ValidExts.ToArray)
            Me.lstNoStack.Items.AddRange(Master.eSettings.NoStackExts.ToArray)
            Me.chkUpdates.Checked = Master.eSettings.CheckUpdates
            Me.chkInfoPanelAnim.Checked = Master.eSettings.InfoPanelAnim

            If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then
                Me.chkCert.Checked = True
                Me.cbCert.Enabled = True
                Me.cbCert.Text = Master.eSettings.CertificationLang
                Me.chkUseCertForMPAA.Enabled = True
                Me.chkUseCertForMPAA.Checked = Master.eSettings.UseCertForMPAA
            End If
            If Not String.IsNullOrEmpty(Master.eSettings.ForceTitle) Then
                Me.chkForceTitle.Checked = True
                Me.cbForce.Enabled = True
                Me.cbForce.Text = Master.eSettings.ForceTitle
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
            Me.cbShowPosterSize.SelectedIndex = Master.eSettings.PreferredShowPosterSize
            Me.cbShowFanartSize.SelectedIndex = Master.eSettings.PreferredShowFanartSize
            Me.cbEpPosterSize.SelectedIndex = Master.eSettings.PreferredEpPosterSize
            Me.cbEpFanartSize.SelectedIndex = Master.eSettings.PreferredEpFanartSize
            Me.chkAutoETSize.Checked = Master.eSettings.AutoET
            Me.cbAutoETSize.SelectedIndex = Master.eSettings.AutoETSize
            Me.chkFanartOnly.Checked = Master.eSettings.FanartPrefSizeOnly
            Me.chkShowFanartOnly.Checked = Master.eSettings.ShowFanartPrefSizeOnly
            Me.chkEpFanartOnly.Checked = Master.eSettings.EpFanartPrefSizeOnly
            Me.tbPosterQual.Value = Master.eSettings.PosterQuality
            Me.tbFanartQual.Value = Master.eSettings.FanartQuality
            Me.tbShowPosterQual.Value = Master.eSettings.ShowPosterQuality
            Me.tbShowFanartQual.Value = Master.eSettings.ShowFanartQuality
            Me.tbEpPosterQual.Value = Master.eSettings.EpPosterQuality
            Me.tbEpFanartQual.Value = Master.eSettings.EpFanartQuality
            Me.chkOverwritePoster.Checked = Master.eSettings.OverwritePoster
            Me.chkOverwriteFanart.Checked = Master.eSettings.OverwriteFanart
            Me.chkOverwriteShowPoster.Checked = Master.eSettings.OverwriteShowPoster
            Me.chkOverwriteShowFanart.Checked = Master.eSettings.OverwriteShowFanart
            Me.chkOverwriteEpPoster.Checked = Master.eSettings.OverwriteEpPoster
            Me.chkOverwriteEpFanart.Checked = Master.eSettings.OverwriteEpFanart
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
            Me.chkMovieNameMultiOnly.Checked = Master.eSettings.MovieNameMultiOnly
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
                Me.txtFanartWidth.Text = Master.eSettings.FanartWidth.ToString
                Me.txtFanartHeight.Text = Master.eSettings.FanartHeight.ToString
            End If
            Me.chkResizePoster.Checked = Master.eSettings.ResizePoster
            If Master.eSettings.ResizePoster Then
                Me.txtPosterWidth.Text = Master.eSettings.PosterWidth.ToString
                Me.txtPosterHeight.Text = Master.eSettings.PosterHeight.ToString
            End If
            Me.chkResizeShowFanart.Checked = Master.eSettings.ResizeShowFanart
            If Master.eSettings.ResizeShowFanart Then
                Me.txtShowFanartWidth.Text = Master.eSettings.ShowFanartWidth.ToString
                Me.txtShowFanartHeight.Text = Master.eSettings.ShowFanartHeight.ToString
            End If
            Me.chkResizeShowPoster.Checked = Master.eSettings.ResizeShowPoster
            If Master.eSettings.ResizeShowPoster Then
                Me.txtShowPosterWidth.Text = Master.eSettings.ShowPosterWidth.ToString
                Me.txtShowPosterHeight.Text = Master.eSettings.ShowPosterHeight.ToString
            End If
            Me.chkResizeEpFanart.Checked = Master.eSettings.ResizeEpFanart
            If Master.eSettings.ResizeEpFanart Then
                Me.txtEpFanartWidth.Text = Master.eSettings.EpFanartWidth.ToString
                Me.txtEpFanartHeight.Text = Master.eSettings.EpFanartHeight.ToString
            End If
            Me.chkResizeEpPoster.Checked = Master.eSettings.ResizeEpPoster
            If Master.eSettings.ResizeEpPoster Then
                Me.txtEpPosterWidth.Text = Master.eSettings.EpPosterWidth.ToString
                Me.txtEpPosterHeight.Text = Master.eSettings.EpPosterHeight.ToString
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

            Me.cbTrailerQuality.SelectedValue = Master.eSettings.PreferredTrailerQuality

            If Master.eSettings.TrailerSites.Count > 0 Then
                For Each iTrailer As Integer In Master.eSettings.TrailerSites
                    Me.lbTrailerSites.SetItemChecked(iTrailer, True)
                Next
            End If

            FillGenres()

            Me.chkShowDims.Checked = Master.eSettings.ShowDims
            Me.chkNoDisplayFanart.Checked = Master.eSettings.NoDisplayFanart
            Me.chkNoDisplayPoster.Checked = Master.eSettings.NoDisplayPoster
            Me.chkOutlineForPlot.Checked = Master.eSettings.OutlineForPlot

            Me.txtFolderPattern.Text = Master.eSettings.FoldersPattern
            Me.txtFilePattern.Text = Master.eSettings.FilesPattern
            Me.chkShowGenresText.Checked = Master.eSettings.AllwaysDisplayGenresText
            Me.chkDisplayYear.Checked = Master.eSettings.DisplayYear

            Me.rbETNative.Checked = Master.eSettings.ETNative
            If Not Master.eSettings.ETNative AndAlso Master.eSettings.ETWidth > 0 AndAlso Master.eSettings.ETHeight > 0 Then
                Me.rbETCustom.Checked = True
                Me.txtETHeight.Text = Master.eSettings.ETHeight.ToString
                Me.txtETWidth.Text = Master.eSettings.ETWidth.ToString
                Me.chkETPadding.Checked = Master.eSettings.ETPadding
            Else
                Me.rbETNative.Checked = True
            End If

            Me.lstSortTokens.Items.AddRange(Master.eSettings.SortTokens.ToArray)

            If Master.eSettings.LevTolerance > 0 Then
                Me.chkCheckTitles.Checked = True
                Me.txtCheckTitleTol.Enabled = True
                Me.txtCheckTitleTol.Text = Master.eSettings.LevTolerance.ToString
            End If
            Me.chkAutoDetectVTS.Checked = Master.eSettings.AutoDetectVTS
            Me.cbLanguages.SelectedItem = If(String.IsNullOrEmpty(Master.eSettings.FlagLang), Master.eLang.Disabled, Master.eSettings.FlagLang)
            Me.cbIntLang.SelectedItem = Master.eSettings.Language

            Me.chkTitle.Checked = Master.eSettings.FieldTitle
            Me.chkYear.Checked = Master.eSettings.FieldYear
            Me.chkMPAA.Checked = Master.eSettings.FieldMPAA
            Me.chkRelease.Checked = Master.eSettings.FieldRelease
            Me.chkRuntime.Checked = Master.eSettings.FieldRuntime
            Me.chkRating.Checked = Master.eSettings.FieldRating
            Me.chkVotes.Checked = Master.eSettings.FieldVotes
            Me.chkStudio.Checked = Master.eSettings.FieldStudio
            Me.chkGenre.Checked = Master.eSettings.FieldGenre
            Me.chkTrailer.Checked = Master.eSettings.FieldTrailer
            Me.chkTagline.Checked = Master.eSettings.FieldTagline
            Me.chkOutline.Checked = Master.eSettings.FieldOutline
            Me.chkPlot.Checked = Master.eSettings.FieldPlot
            Me.chkCast.Checked = Master.eSettings.FieldCast
            Me.chkDirector.Checked = Master.eSettings.FieldDirector
            Me.chkWriters.Checked = Master.eSettings.FieldWriters
            If Master.eSettings.FullCrew Then
                Me.chkProducers.Checked = Master.eSettings.FieldProducers
                Me.chkMusicBy.Checked = Master.eSettings.FieldMusic
                Me.chkCrew.Checked = Master.eSettings.FieldCrew
            End If
            Me.chkTop250.Checked = Master.eSettings.Field250
            Me.txtActorLimit.Text = Master.eSettings.ActorLimit.ToString
            Me.txtGenreLimit.Text = Master.eSettings.GenreLimit.ToString

            Me.chkMissingPoster.Checked = Master.eSettings.MissingFilterPoster
            Me.chkMissingFanart.Checked = Master.eSettings.MissingFilterFanart
            Me.chkMissingNFO.Checked = Master.eSettings.MissingFilterNFO
            Me.chkMissingTrailer.Checked = Master.eSettings.MissingFilterTrailer
            Me.chkMissingSubs.Checked = Master.eSettings.MissingFilterSubs
            Me.chkMissingExtra.Checked = Master.eSettings.MissingFilterExtras

            Me.chkRenameMulti.Checked = Master.eSettings.AutoRenameMulti
            Me.chkRenameSingle.Checked = Master.eSettings.AutoRenameSingle
            Me.cbMovieTheme.SelectedItem = Master.eSettings.MovieTheme
            Me.cbTVShowTheme.SelectedItem = Master.eSettings.TVShowTheme
            Me.cbEpTheme.SelectedItem = Master.eSettings.TVEpTheme
            Me.Meta = Master.eSettings.MetadataPerFileType
            Me.LoadMetadata()
            Me.chkIFOScan.Checked = Master.eSettings.EnableIFOScan
            Me.chkCleanDB.Checked = Master.eSettings.CleanDB
            Me.chkIgnoreLastScan.Checked = Master.eSettings.IgnoreLastScan
            Me.chkTVCleanDB.Checked = Master.eSettings.TVCleanDB
            Me.chkTVIgnoreLastScan.Checked = Master.eSettings.TVIgnoreLastScan
            Me.ShowRegex = Master.eSettings.TVShowRegexes
            Me.LoadShowRegex()
            Me.cbRatingRegion.Text = Master.eSettings.ShowRatingRegion
            Me.chkShowSeasonAll.Checked = Master.eSettings.ShowSeasonAll
            Me.chkShowFolderJPG.Checked = Master.eSettings.ShowFolderJPG
            Me.chkShowPosterTBN.Checked = Master.eSettings.ShowPosterTBN
            Me.chkShowPosterJPG.Checked = Master.eSettings.ShowPosterJPG
            Me.chkShowFanartJPG.Checked = Master.eSettings.ShowFanartJPG
            Me.chkShowDashFanart.Checked = Master.eSettings.ShowDashFanart
            Me.chkShowDotFanart.Checked = Master.eSettings.ShowDotFanart
            Me.chkSeasonXXTBN.Checked = Master.eSettings.SeasonXX
            Me.chkSeasonXTBN.Checked = Master.eSettings.SeasonX
            Me.chkSeasonPosterTBN.Checked = Master.eSettings.SeasonPosterTBN
            Me.chkSeasonPosterJPG.Checked = Master.eSettings.SeasonPosterJPG
            Me.chkSeasonNameTBN.Checked = Master.eSettings.SeasonNameTBN
            Me.chkSeasonNameJPG.Checked = Master.eSettings.SeasonNameJPG
            Me.chkSeasonFolderJPG.Checked = Master.eSettings.SeasonFolderJPG
            Me.chkSeasonFanartJPG.Checked = Master.eSettings.SeasonFanartJPG
            Me.chkSeasonDashFanart.Checked = Master.eSettings.SeasonDashFanart
            Me.chkSeasonDotFanart.Checked = Master.eSettings.SeasonDotFanart
            Me.chkEpisodeTBN.Checked = Master.eSettings.EpisodeTBN
            Me.chkEpisodeJPG.Checked = Master.eSettings.EpisodeJPG
            Me.chkEpisodeDashFanart.Checked = Master.eSettings.EpisodeDashFanart
            Me.chkEpisodeDotFanart.Checked = Master.eSettings.EpisodeDotFanart
            Me.chkShowPosterCol.Checked = Master.eSettings.ShowPosterCol
            Me.chkShowFanartCol.Checked = Master.eSettings.ShowFanartCol
            Me.chkShowNfoCol.Checked = Master.eSettings.ShowNfoCol
            Me.chkSeasonPosterCol.Checked = Master.eSettings.SeasonPosterCol
            Me.chkSeasonFanartCol.Checked = Master.eSettings.SeasonFanartCol
            Me.chkEpisodePosterCol.Checked = Master.eSettings.EpisodePosterCol
            Me.chkEpisodeFanartCol.Checked = Master.eSettings.EpisodeFanartCol
            Me.chkEpisodeNfoCol.Checked = Master.eSettings.EpisodeNfoCol
            Me.chkSourceFromFolder.Checked = Master.eSettings.SourceFromFolder
            Me.chkSortBeforeScan.Checked = Master.eSettings.SortBeforeScan
            Me.cbTVLanguage.DataSource = Master.eSettings.TVDBLanguages
            Me.cbTVLanguage.DisplayMember = "LongLang"
            Me.cbTVLanguage.ValueMember = "ShortLang"
            If Me.cbTVLanguage.Items.Count > 0 Then
                Me.cbTVLanguage.SelectedValue = Master.eSettings.TVDBLanguage
            End If
            Me.txtTVDBMirror.Text = Master.eSettings.TVDBMirror

            If Not String.IsNullOrEmpty(Master.eSettings.ProxyURI) AndAlso Master.eSettings.ProxyPort >= 0 Then
                Me.chkEnableProxy.Checked = True
                Me.txtProxyURI.Text = Master.eSettings.ProxyURI
                Me.txtProxyPort.Text = Master.eSettings.ProxyPort.ToString

                If Not String.IsNullOrEmpty(Master.eSettings.ProxyCreds.UserName) Then
                    Me.chkEnableCredentials.Checked = True
                    Me.txtProxyUsername.Text = Master.eSettings.ProxyCreds.UserName
                    Me.txtProxyPassword.Text = Master.eSettings.ProxyCreds.Password
                    Me.txtProxyDomain.Text = Master.eSettings.ProxyCreds.Domain
                End If
            End If

            Me.RefreshSources()
            Me.RefreshTVSources()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillGenres()
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
    End Sub

    Private Sub LoadTrailerQualities()
        Dim items As New Dictionary(Of String, Master.TrailerQuality)
        items.Add("1080p", Master.TrailerQuality.HD1080p)
        items.Add("720p", Master.TrailerQuality.HD720p)
        items.Add("Standard", Master.TrailerQuality.Standard)
        Me.cbTrailerQuality.DataSource = items.ToList
        Me.cbTrailerQuality.DisplayMember = "Key"
        Me.cbTrailerQuality.ValueMember = "Value"
    End Sub

    Private Sub LoadXComs()
        Me.lbXBMCCom.Items.Clear()
        For Each x As emmSettings.XBMCCom In Me.XComs
            Me.lbXBMCCom.Items.Add(x.Name)
        Next
    End Sub

    Private Sub LoadGenreLangs()

        Me.lbGenre.Items.Add(Master.eLang.All)
        Me.lbGenre.Items.AddRange(XML.GetGenreList(True))

    End Sub

    Private Sub LoadLangs()

        Me.cbLanguages.Items.Add(Master.eLang.Disabled)
        Me.cbLanguages.Items.AddRange(XML.GetLanguageList)

    End Sub

    Private Sub LoadIntLangs()

        Me.cbIntLang.Items.Clear()
        If Directory.Exists(Path.Combine(Master.AppPath, "Langs")) Then
            Dim alL As New List(Of String)
            Dim alLangs As New List(Of String)
            Try
                alL.AddRange(Directory.GetFiles(Path.Combine(Master.AppPath, "Langs"), "*).xml"))
            Catch
            End Try
            alLangs.AddRange(alL.Cast(Of String)().Select(Function(AL) Path.GetFileNameWithoutExtension(AL)).ToList)
            Me.cbIntLang.Items.AddRange(alLangs.ToArray)
        End If

    End Sub

    Private Sub LoadThemes()

        Me.cbMovieTheme.Items.Clear()
        Me.cbTVShowTheme.Items.Clear()
        Me.cbEpTheme.Items.Clear()
        If Directory.Exists(Path.Combine(Master.AppPath, "Themes")) Then
            Dim mT As New List(Of String)
            Dim sT As New List(Of String)
            Dim eT As New List(Of String)
            Try
                mT.AddRange(Directory.GetFiles(Path.Combine(Master.AppPath, "Themes"), "movie-*.xml"))
            Catch
            End Try
            Me.cbMovieTheme.Items.AddRange(mT.Cast(Of String)().Select(Function(AL) Path.GetFileNameWithoutExtension(AL).Replace("movie-", String.Empty)).ToArray)
            Try
                sT.AddRange(Directory.GetFiles(Path.Combine(Master.AppPath, "Themes"), "tvshow-*.xml"))
            Catch
            End Try
            Me.cbTVShowTheme.Items.AddRange(sT.Cast(Of String)().Select(Function(AL) Path.GetFileNameWithoutExtension(AL).Replace("tvshow-", String.Empty)).ToArray)
            Try
                eT.AddRange(Directory.GetFiles(Path.Combine(Master.AppPath, "Themes"), "tvep-*.xml"))
            Catch
            End Try
            Me.cbEpTheme.Items.AddRange(eT.Cast(Of String)().Select(Function(AL) Path.GetFileNameWithoutExtension(AL).Replace("tvep-", String.Empty)).ToArray)
        End If

    End Sub

    Private Sub LoadMetadata()
        Me.lstMetaData.Items.Clear()
        For Each x As emmSettings.MetadataPerType In Meta
            Me.lstMetaData.Items.Add(x.FileType)
        Next
    End Sub

    Private Sub RefreshSources()
        Dim lvItem As ListViewItem

        lvMovies.Items.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM sources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    lvItem = New ListViewItem(SQLreader("ID").ToString)
                    lvItem.SubItems.Add(SQLreader("Name").ToString)
                    lvItem.SubItems.Add(SQLreader("Path").ToString)
                    lvItem.SubItems.Add(If(Convert.ToBoolean(SQLreader("Recursive")), "Yes", "No"))
                    lvItem.SubItems.Add(If(Convert.ToBoolean(SQLreader("Foldername")), "Yes", "No"))
                    lvItem.SubItems.Add(If(Convert.ToBoolean(SQLreader("Single")), "Yes", "No"))
                    lvMovies.Items.Add(lvItem)
                End While
            End Using
        End Using

    End Sub

    Private Sub RefreshTVSources()
        Dim lvItem As ListViewItem
        lvTVSources.Items.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM TVSources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    lvItem = New ListViewItem(SQLreader("ID").ToString)
                    lvItem.SubItems.Add(SQLreader("Name").ToString)
                    lvItem.SubItems.Add(SQLreader("Path").ToString)
                    lvTVSources.Items.Add(lvItem)
                End While
            End Using
        End Using

    End Sub

    Private Sub LoadShowRegex()
        Dim lvItem As ListViewItem
        lvShowRegex.Items.Clear()
        For Each rShow As emmSettings.TVShowRegEx In Me.ShowRegex
            lvItem = New ListViewItem(rShow.ID.ToString)
            lvItem.SubItems.Add(rShow.SeasonRegex)
            lvItem.SubItems.Add(If(rShow.SeasonFromDirectory, "Directory", "File"))
            lvItem.SubItems.Add(rShow.EpisodeRegex)
            Select Case rShow.EpisodeRetrieve
                Case emmSettings.EpRetrieve.FromDirectory
                    lvItem.SubItems.Add("Directory")
                Case emmSettings.EpRetrieve.FromFilename
                    lvItem.SubItems.Add("File")
                Case emmSettings.EpRetrieve.FromSeasonResult
                    lvItem.SubItems.Add("Result")
            End Select
            Me.lvShowRegex.Items.Add(lvItem)
        Next
    End Sub

    Private Sub SetUp()
        Me.btnAddShowRegex.Tag = String.Empty
        Me.Text = Master.eLang.GetString(420, "Settings")
        Me.GroupBox11.Text = Master.eLang.GetString(554, "XBMC Communication")
        Me.btnEditCom.Text = Master.eLang.GetString(422, "Commit Edit")
        Me.Label16.Text = Master.eLang.GetString(423, "Name:")
        Me.btnAddCom.Text = Master.eLang.GetString(424, "Add New")
        Me.Label13.Text = Master.eLang.GetString(425, "Username:")
        Me.Label14.Text = Master.eLang.GetString(426, "Password:")
        Me.Label7.Text = Master.eLang.GetString(427, "XBMC IP:")
        Me.Label6.Text = Master.eLang.GetString(428, "XBMC Port:")
        Me.GroupBox4.Text = Master.eLang.GetString(429, "Miscellaneous")
        Me.Label32.Text = Master.eLang.GetString(430, "Interface Language:")
        Me.chkInfoPanelAnim.Text = Master.eLang.GetString(431, "Enable Panel Animation")
        Me.chkUpdates.Text = Master.eLang.GetString(432, "Check for Updates")
        Me.chkOverwriteNfo.Text = Master.eLang.GetString(433, "Overwrite Non-conforming nfos")
        Me.Label5.Text = Master.eLang.GetString(434, "(If unchecked, non-conforming nfos will be renamed to <filename>.info)")
        Me.chkLogErrors.Text = Master.eLang.GetString(435, "Log Errors to File")
        Me.Label31.Text = Master.eLang.GetString(436, "Display Overlay if Video Contains an Audio Stream With the Following Language:")
        Me.GroupBox3.Text = Master.eLang.GetString(437, "Clean Files")
        Me.tpStandard.Text = Master.eLang.GetString(438, "Standard")
        Me.tpExpert.Text = Master.eLang.GetString(439, "Expert")
        Me.chkWhitelistVideo.Text = Master.eLang.GetString(440, "Whitelist Video Extensions")
        Me.Label27.Text = Master.eLang.GetString(441, "Whitelisted Extensions:")
        Me.Label25.Text = Master.eLang.GetString(442, "WARNING: Using the Expert Mode Cleaner could potentially delete wanted files. Take care when using this tool.")
        Me.gbFilters.Text = Master.eLang.GetString(451, "Folder/File Name Filters")
        Me.chkProperCase.Text = Master.eLang.GetString(452, "Convert Names to Proper Case")
        Me.chkShowProperCase.Text = Master.eLang.GetString(452, "Convert Names to Proper Case")
        Me.chkEpProperCase.Text = Master.eLang.GetString(452, "Convert Names to Proper Case")
        Me.GroupBox12.Text = Me.GroupBox4.Text
        Me.chkShowGenresText.Text = Master.eLang.GetString(453, "Always Display Genre Text")
        Me.gbGenreFilter.Text = Master.eLang.GetString(454, "Genre Language Filter:")
        Me.chkNoDisplayFanart.Text = Master.eLang.GetString(455, "Do Not Display Fanart")
        Me.chkNoDisplayPoster.Text = Master.eLang.GetString(456, "Do Not Display Poster")
        Me.chkShowDims.Text = Master.eLang.GetString(457, "Display Image Dimensions")
        Me.Label8.Text = Master.eLang.GetString(458, "(New movies will still display in green if not checked.)")
        Me.chkMarkNew.Text = Master.eLang.GetString(459, "Mark New Movies")
        Me.GroupBox2.Text = Master.eLang.GetString(460, "Media List Options")
        Me.Label30.Text = Master.eLang.GetString(461, "Tolerance:")
        Me.chkCheckTitles.Text = Master.eLang.GetString(462, "Check Title Match Confidence")
        Me.GroupBox25.Text = Master.eLang.GetString(463, "Sort Tokens to Ignore")
        Me.chkDisplayYear.Text = Master.eLang.GetString(464, "Display Year in List Title")
        Me.chkMovieExtraCol.Text = Master.eLang.GetString(465, "Hide Extrathumb Column")
        Me.chkMovieSubCol.Text = Master.eLang.GetString(466, "Hide Sub Column")
        Me.chkMovieTrailerCol.Text = Master.eLang.GetString(467, "Hide Trailer Column")
        Me.chkMovieInfoCol.Text = Master.eLang.GetString(468, "Hide Info Column")
        Me.chkMovieFanartCol.Text = Master.eLang.GetString(469, "Hide Fanart Column")
        Me.chkMoviePosterCol.Text = Master.eLang.GetString(470, "Hide Poster Column")
        Me.GroupBox8.Text = Master.eLang.GetString(471, "File Naming")
        Me.gbTVNaming.Text = Master.eLang.GetString(471, "File Naming")
        Me.chkMovieNameMultiOnly.Text = Master.eLang.GetString(472, "Use <movie> Only for Folders with Multiple Movies")
        Me.GroupBox21.Text = Master.eLang.GetString(151, "Trailer")
        Me.chkVideoTSParent.Text = Master.eLang.GetString(473, "YAMJ Compatible VIDEO_TS File Placement/Naming")
        Me.GroupBox6.Text = Master.eLang.GetString(149, "Fanart")
        Me.GroupBox5.Text = Master.eLang.GetString(148, "Poster")
        Me.colName.Text = Master.eLang.GetString(232, "Name")
        Me.colPath.Text = Master.eLang.GetString(410, "Path")
        Me.colRecur.Text = Master.eLang.GetString(411, "Recursive")
        Me.colFolder.Text = Master.eLang.GetString(412, "Use Folder Name")
        Me.colSingle.Text = Master.eLang.GetString(413, "Single Video")
        Me.btnMovieRem.Text = Master.eLang.GetString(30, "Remove")
        Me.btnRemTVSource.Text = Master.eLang.GetString(30, "Remove")
        Me.btnMovieAddFolder.Text = Master.eLang.GetString(407, "Add Source")
        Me.btnAddTVSource.Text = Master.eLang.GetString(407, "Add Source")
        Me.chkOFDBGenre.Text = Master.eLang.GetString(474, "Use OFDB Genre")
        Me.chkOFDBPlot.Text = Master.eLang.GetString(475, "Use OFDB Plot")
        Me.chkOFDBOutline.Text = Master.eLang.GetString(476, "Use OFDB Outline")
        Me.chkOFDBTitle.Text = Master.eLang.GetString(477, "Use OFDB Title")
        Me.GroupBox14.Text = Master.eLang.GetString(148, "Poster")
        Me.Label24.Text = Master.eLang.GetString(478, "Poster Quality:")
        Me.Label11.Text = Master.eLang.GetString(479, "Max Width:")
        Me.Label12.Text = Master.eLang.GetString(480, "Max Height:")
        Me.chkResizePoster.Text = Master.eLang.GetString(481, "Automatically Resize Poster:")
        Me.lblPosterSize.Text = Master.eLang.GetString(482, "Preferred Poster Size")
        Me.chkOverwritePoster.Text = Master.eLang.GetString(483, "Overwrite Existing Poster")
        Me.GroupBox13.Text = Master.eLang.GetString(149, "Fanart")
        Me.chkFanartOnly.Text = Master.eLang.GetString(145, "Only")
        Me.Label26.Text = Master.eLang.GetString(484, "Fanart Quality:")
        Me.Label9.Text = Me.Label11.Text
        Me.Label10.Text = Me.Label12.Text
        Me.chkResizeFanart.Text = Master.eLang.GetString(485, "Automatically Resize Fanart:")
        Me.lblFanartSize.Text = Master.eLang.GetString(486, "Preferred Fanart Size")
        Me.chkOverwriteFanart.Text = Master.eLang.GetString(487, "Overwrite Existing Fanart")
        Me.GroupBox10.Text = Master.eLang.GetString(488, "Global Locks (Do not allow updates during scraping)")
        Me.chkLockTrailer.Text = Master.eLang.GetString(489, "Lock Trailer")
        Me.chkLockGenre.Text = Master.eLang.GetString(490, "Lock Genre")
        Me.chkLockRealStudio.Text = Master.eLang.GetString(491, "Lock Studio")
        Me.chkLockRating.Text = Master.eLang.GetString(492, "Lock Rating")
        Me.chkLockTagline.Text = Master.eLang.GetString(493, "Lock Tagline")
        Me.chkLockTitle.Text = Master.eLang.GetString(494, "Lock Title")
        Me.chkLockOutline.Text = Master.eLang.GetString(495, "Lock Outline")
        Me.chkLockPlot.Text = Master.eLang.GetString(496, "Lock Plot")
        Me.GroupBox9.Text = Master.eLang.GetString(497, "Images")
        Me.chkNoSaveImagesToNfo.Text = Master.eLang.GetString(498, "Do Not Save Image URLs to Nfo")
        Me.chkSingleScrapeImages.Text = Master.eLang.GetString(499, "Scrape Images on Single Scrape")
        Me.chkUseMPDB.Text = Master.eLang.GetString(500, "Get Images From MoviePostersDB")
        Me.chkUseTMDB.Text = Master.eLang.GetString(501, "Get Images From TMDB")
        Me.chkUseIMPA.Text = Master.eLang.GetString(502, "Get Images From IMPAwards")
        Me.chkUseETasFA.Text = Master.eLang.GetString(503, "Use Extrathumb if no Fanart Found")
        Me.Label17.Text = Master.eLang.GetString(504, "(If checked, Ember will use only the first half of the movie to extract thumbs)")
        Me.chkNoSpoilers.Text = Master.eLang.GetString(505, "No Spoilers")
        Me.Label15.Text = Master.eLang.GetString(506, "Number To Create:")
        Me.chkAutoThumbs.Text = Master.eLang.GetString(507, "Automatically Extract Extrathumbs During Scrapers")
        Me.chkOutlineForPlot.Text = Master.eLang.GetString(508, "Use Outline for Plot if Plot is Empty")
        Me.Label18.Text = Master.eLang.GetString(509, "IMDB Mirror:")
        Me.chkCastWithImg.Text = Master.eLang.GetString(510, "Scrape Only Actors With Images")
        Me.chkUseCertForMPAA.Text = Master.eLang.GetString(511, "Use Certification for MPAA")
        Me.chkFullCast.Text = Master.eLang.GetString(512, "Scrape Full Cast")
        Me.chkFullCrew.Text = Master.eLang.GetString(513, "Scrape Full Crew")
        Me.chkCert.Text = Master.eLang.GetString(514, "Use Certification Language:")
        Me.gbRTFormat.Text = Master.eLang.GetString(515, "Runtime Format")
        Me.chkUseMIDuration.Text = Master.eLang.GetString(516, "Use Duration for Runtime")
        Me.chkScanMediaInfo.Text = Master.eLang.GetString(517, "Scan Meta Data")
        Me.btnOK.Text = Master.eLang.GetString(179, "OK")
        Me.btnApply.Text = Master.eLang.GetString(276, "Apply")
        Me.btnCancel.Text = Master.eLang.GetString(167, "Cancel")
        Me.Label2.Text = Master.eLang.GetString(518, "Configure Ember's appearance and operation.")
        Me.Label4.Text = Me.Text
        Me.btnRemoveCom.Text = Master.eLang.GetString(519, "Remove Selected")
        Me.GroupBox16.Text = Master.eLang.GetString(520, "Backdrops Folder")
        Me.chkAutoBD.Text = Master.eLang.GetString(521, "Automatically Save Fanart To Backdrops Folder")
        Me.GroupBox26.Text = Master.eLang.GetString(59, "Meta Data")
        Me.GroupBox20.Text = Master.eLang.GetString(151, "Trailers")
        Me.chkDeleteAllTrailers.Text = Master.eLang.GetString(522, "Delete All Existing Trailers")
        Me.chkOverwriteTrailer.Text = Master.eLang.GetString(523, "Overwrite Trailer")
        Me.chkNoDLTrailer.Text = Master.eLang.GetString(524, "Only Get URLs During Scrapers")
        Me.chkSingleScrapeTrailer.Text = Master.eLang.GetString(525, "Get Trailers During Single-Scrape")
        Me.Label23.Text = Master.eLang.GetString(526, "Timeout:")
        Me.chkUpdaterTrailer.Text = Master.eLang.GetString(527, "Get Trailers During ""All Items"" Scrapers")
        Me.Label22.Text = Master.eLang.GetString(528, "Supported Trailer Sites:")
        Me.chkDownloadTrailer.Text = Master.eLang.GetString(529, "Enable Trailer Downloading")
        Me.GroupBox22.Text = Master.eLang.GetString(530, "No Stack Extensions")
        Me.gbRenamerPatterns.Text = Master.eLang.GetString(531, "Default Renaming Patterns")
        Me.lblFilePattern.Text = Master.eLang.GetString(532, "Files Pattern")
        Me.lblFolderPattern.Text = Master.eLang.GetString(533, "Folders Pattern")
        Me.GroupBox18.Text = Master.eLang.GetString(534, "Valid Video Extensions")
        Me.btnEditSource.Text = Master.eLang.GetString(535, "Edit Source")
        Me.btnEditTVSource.Text = Master.eLang.GetString(535, "Edit Source")
        Me.GroupBox19.Text = Master.eLang.GetString(536, "Miscellaneous Options")
        Me.gbMiscTVSourceOpts.Text = Master.eLang.GetString(536, "Miscellaneous Options")
        Me.chkAutoDetectVTS.Text = Master.eLang.GetString(537, "Automatically Detect VIDEO_TS Folders Even if They Are Not Named ""VIDEO_TS""")
        Me.chkSkipStackedSizeCheck.Text = Master.eLang.GetString(538, "Skip Size Check of Stacked Files")
        Me.Label21.Text = Master.eLang.GetString(539, "Megabytes")
        Me.Label20.Text = Master.eLang.GetString(540, "Skip files less than:")
        Me.GroupBox23.Text = Master.eLang.GetString(153, "Extrathumbs")
        Me.GroupBox24.Text = Master.eLang.GetString(541, "Sizing (Extracted Frames)")
        Me.chkETPadding.Text = Master.eLang.GetString(542, "Padding")
        Me.Label28.Text = Master.eLang.GetString(543, "Width:")
        Me.Label29.Text = Master.eLang.GetString(544, "Height:")
        Me.rbETCustom.Text = Master.eLang.GetString(545, "Use Custom Size")
        Me.rbETNative.Text = Master.eLang.GetString(546, "Use Native Resolution")
        Me.GroupBox17.Text = Master.eLang.GetString(547, "Caching")
        Me.chkUseImgCacheUpdaters.Text = Master.eLang.GetString(548, "Use Image Cache for Scrapers")
        Me.Label19.Text = Master.eLang.GetString(549, "(When enabled, the cache will be available between sessions)")
        Me.chkPersistImgCache.Text = Master.eLang.GetString(550, "Persistent Image Cache")
        Me.chkUseImgCache.Text = Master.eLang.GetString(551, "Use Image Cache")
        Me.fbdBrowse.Description = Master.eLang.GetString(552, "Select the folder where you wish to store your backdrops.")
        Me.gbOptions.Text = Master.eLang.GetString(577, "Scraper Fields")
        Me.chkCrew.Text = Master.eLang.GetString(391, "Other Crew")
        Me.chkMusicBy.Text = Master.eLang.GetString(392, "Music By")
        Me.chkProducers.Text = Master.eLang.GetString(393, "Producers")
        Me.chkWriters.Text = Master.eLang.GetString(394, "Writers")
        Me.chkStudio.Text = Master.eLang.GetString(395, "Studio")
        Me.chkRuntime.Text = Master.eLang.GetString(396, "Runtime")
        Me.chkPlot.Text = Master.eLang.GetString(65, "Plot")
        Me.chkOutline.Text = Master.eLang.GetString(64, "Plot Outline")
        Me.chkGenre.Text = Master.eLang.GetString(20, "Genres")
        Me.chkDirector.Text = Master.eLang.GetString(62, "Director")
        Me.chkTagline.Text = Master.eLang.GetString(397, "Tagline")
        Me.chkCast.Text = Master.eLang.GetString(398, "Cast")
        Me.chkVotes.Text = Master.eLang.GetString(399, "Votes")
        Me.chkTrailer.Text = Master.eLang.GetString(151, "Trailer")
        Me.chkRating.Text = Master.eLang.GetString(400, "Rating")
        Me.chkRelease.Text = Master.eLang.GetString(57, "Release Date")
        Me.chkMPAA.Text = Master.eLang.GetString(401, "MPAA/Cert")
        Me.chkYear.Text = Master.eLang.GetString(278, "Year")
        Me.chkTitle.Text = Master.eLang.GetString(21, "Title")
        Me.GroupBox1.Text = Master.eLang.GetString(429, "Miscellaneous")
        Me.lblLimit.Text = Master.eLang.GetString(578, "Limit:")
        Me.lblLimit2.Text = Me.lblLimit.Text
        Me.GroupBox27.Text = Master.eLang.GetString(581, "Missing Items Filter")
        Me.chkMissingPoster.Text = Master.eLang.GetString(582, "Check for Poster")
        Me.chkMissingFanart.Text = Master.eLang.GetString(583, "Check for Fanart")
        Me.chkMissingNFO.Text = Master.eLang.GetString(584, "Check for NFO")
        Me.chkMissingTrailer.Text = Master.eLang.GetString(585, "Check for Trailer")
        Me.chkMissingSubs.Text = Master.eLang.GetString(586, "Check for Subs")
        Me.chkMissingExtra.Text = Master.eLang.GetString(587, "Check for Extrathumbs")
        Me.chkTop250.Text = Master.eLang.GetString(591, "Top 250")
        Me.chkRenameMulti.Text = Master.eLang.GetString(592, "Automatically Rename Files During Multi-Scraper")
        Me.chkRenameSingle.Text = Master.eLang.GetString(593, "Automatically Rename Files During Single-Scraper")
        Me.chkAutoETSize.Text = Master.eLang.GetString(599, "Download All Fanart Images of the Following Size as Extrathumbs")
        Me.Label35.Text = String.Concat(Master.eLang.GetString(620, "Movie Theme"), ":")
        Me.Label1.Text = String.Concat(Master.eLang.GetString(666, "TV Show Theme"), ":")
        Me.Label3.Text = String.Concat(Master.eLang.GetString(667, "Episode Theme"), ":")
        Me.btnDLTrans.Text = Master.eLang.GetString(443, "Download Addons")
        Me.Label36.Text = Master.eLang.GetString(621, "You must restart Ember before changes will take effect.")
        Me.GroupBox28.Text = Master.eLang.GetString(625, "Meta Data Defaults by File Type")
        Me.Label34.Text = Master.eLang.GetString(626, "File Type")
        Me.chkIFOScan.Text = Master.eLang.GetString(628, "Enable IFO Parsing")
        Me.GroupBox29.Text = Master.eLang.GetString(629, "Themes")
        Me.chkYAMJCompatibleSets.Text = Master.eLang.GetString(643, "YAMJ Compatible Sets")
        Me.chkCleanDB.Text = Master.eLang.GetString(668, "Clean database after updating library")
        Me.chkTVCleanDB.Text = Master.eLang.GetString(668, "Clean database after updating library")
        Me.chkIgnoreLastScan.Text = Master.eLang.GetString(669, "Ignore last scan time when updating library")
        Me.chkTVIgnoreLastScan.Text = Master.eLang.GetString(669, "Ignore last scan time when updating library")
        Me.gbShowFilter.Text = Master.eLang.GetString(670, "Show Folder/File Name Filters")
        Me.gbEpFilter.Text = Master.eLang.GetString(671, "Episode Folder/File Name Filters")
        Me.gbProxy.Text = Master.eLang.GetString(672, "Proxy")
        Me.chkEnableProxy.Text = Master.eLang.GetString(673, "Enable Proxy")
        Me.lblProxyURI.Text = Master.eLang.GetString(674, "Proxy URL:")
        Me.lblProxyPort.Text = Master.eLang.GetString(675, "Proxy Port:")
        Me.gbCreds.Text = Master.eLang.GetString(676, "Credentials")
        Me.chkEnableCredentials.Text = Master.eLang.GetString(677, "Enable Credentials")
        Me.lblProxyUN.Text = Master.eLang.GetString(425, "Username:")
        Me.lblProxyPW.Text = Master.eLang.GetString(426, "Password:")
        Me.lblProxyDomain.Text = Master.eLang.GetString(678, "Domain:")
        Me.gbTVMisc.Text = Master.eLang.GetString(429, "Miscellaneous")
        Me.lblRatingRegion.Text = Master.eLang.GetString(679, "TV Rating Region")
        Me.gbTVListOptions.Text = Master.eLang.GetString(460, "Media List Options")
        Me.gbShowListOptions.Text = Master.eLang.GetString(680, "Shows")
        Me.gbSeasonListOptions.Text = Master.eLang.GetString(681, "Seasons")
        Me.gbEpisodeListOptions.Text = Master.eLang.GetString(682, "Episodes")
        Me.chkShowPosterCol.Text = Master.eLang.GetString(470, "Hide Poster Column")
        Me.chkSeasonPosterCol.Text = Master.eLang.GetString(470, "Hide Poster Column")
        Me.chkEpisodePosterCol.Text = Master.eLang.GetString(470, "Hide Poster Column")
        Me.chkShowFanartCol.Text = Master.eLang.GetString(469, "Hide Fanart Column")
        Me.chkSeasonFanartCol.Text = Master.eLang.GetString(469, "Hide Fanart Column")
        Me.chkEpisodeFanartCol.Text = Master.eLang.GetString(469, "Hide Fanart Column")
        Me.chkShowNfoCol.Text = Master.eLang.GetString(468, "Hide Info Column")
        Me.chkEpisodeNfoCol.Text = Master.eLang.GetString(468, "Hide Info Column")
        Me.gbShowPosters.Text = Master.eLang.GetString(683, "Show Posters")
        Me.gbShowFanart.Text = Master.eLang.GetString(684, "Show Fanart")
        Me.gbSeasonPosters.Text = Master.eLang.GetString(685, "Season Posters")
        Me.gbSeasonFanart.Text = Master.eLang.GetString(686, "Season Fanart")
        Me.gbEpisodePosters.Text = Master.eLang.GetString(687, "Episode Posters")
        Me.gbEpisodeFanart.Text = Master.eLang.GetString(688, "Episode Fanart")
        Me.lblInsideSeason.Text = Master.eLang.GetString(689, "* Inside Season directory")
        Me.btnEditShowRegex.Text = Master.eLang.GetString(690, "Edit Regex")
        Me.btnRemoveShowRegex.Text = Master.eLang.GetString(30, "Remove")
        Me.gbShowRegex.Text = Master.eLang.GetString(691, "Show Match Regex")
        Me.lblSeasonMatch.Text = Master.eLang.GetString(692, "Season Match Regex:")
        Me.lblEpisodeMatch.Text = Master.eLang.GetString(693, "Episode Match Regex:")
        Me.lblSeasonRetrieve.Text = String.Concat(Master.eLang.GetString(694, "Apply To"), ":")
        Me.lblEpisodeRetrieve.Text = String.Concat(Master.eLang.GetString(694, "Apply To"), ":")
        Me.btnAddShowRegex.Text = Master.eLang.GetString(695, "Edit Regex")
        Me.lvTVSources.Columns(1).Text = Master.eLang.GetString(232, "Name")
        Me.lvTVSources.Columns(2).Text = Master.eLang.GetString(410, "Path")
        Me.lvShowRegex.Columns(0).Text = Master.eLang.GetString(696, "Show Regex")
        Me.lvShowRegex.Columns(1).Text = Master.eLang.GetString(694, "Apply To")
        Me.lvShowRegex.Columns(2).Text = Master.eLang.GetString(697, "Episode Regex")
        Me.lvShowRegex.Columns(3).Text = Master.eLang.GetString(693, "Apply To")
        Me.lvMovies.Columns(1).Text = Master.eLang.GetString(232, "Name")
        Me.lvMovies.Columns(2).Text = Master.eLang.GetString(410, "Path")
        Me.lvMovies.Columns(3).Text = Master.eLang.GetString(411, "Recursive")
        Me.lvMovies.Columns(4).Text = Master.eLang.GetString(412, "Use Folder Name")
        Me.lvMovies.Columns(5).Text = Master.eLang.GetString(413, "Single Video")

        Me.tvSettings.Nodes(0).Text = Master.eLang.GetString(38, "General")
        Me.tvSettings.Nodes(0).Nodes(0).Text = Master.eLang.GetString(553, "File System")
        Me.tvSettings.Nodes(0).Nodes(1).Text = Master.eLang.GetString(421, "Communication")
        Me.tvSettings.Nodes(1).Text = Master.eLang.GetString(36, "Movies")
        Me.tvSettings.Nodes(1).Nodes(0).Text = Master.eLang.GetString(555, "Files and Sources")
        Me.tvSettings.Nodes(1).Nodes(1).Text = Master.eLang.GetString(556, "Scraper - Data")
        Me.tvSettings.Nodes(1).Nodes(2).Text = Master.eLang.GetString(557, "Scraper - Images")
        Me.tvSettings.Nodes(2).Text = Master.eLang.GetString(698, "TV Shows")
        Me.tvSettings.Nodes(2).Nodes(0).Text = Master.eLang.GetString(555, "Files and Sources")
        Me.tvSettings.Nodes(2).Nodes(1).Text = Master.eLang.GetString(556, "Scraper - Data")
        Me.tvSettings.Nodes(2).Nodes(2).Text = Master.eLang.GetString(557, "Scraper - Images")
        Me.gbShowPosterOpts.Text = Master.eLang.GetString(148, "Poster")
        Me.lblShowPosterSize.Text = Master.eLang.GetString(482, "Preferred Poster Size")
        Me.chkOverwriteShowPoster.Text = Master.eLang.GetString(483, "Overwrite Existing Poster")
        Me.chkResizeShowPoster.Text = Master.eLang.GetString(481, "Automatically Resize Poster:")
        Me.lblShowPosterWidth.Text = Master.eLang.GetString(479, "Max Width:")
        Me.lblShowPosterHeight.Text = Master.eLang.GetString(480, "Max Height:")
        Me.lblShowPosterQ.Text = Master.eLang.GetString(478, "Poster Quality:")
        Me.gbShowFanartOpts.Text = Master.eLang.GetString(149, "Fanart")
        Me.lblShowFanartSize.Text = Master.eLang.GetString(486, "Preferred Fanart Size")
        Me.chkOverwriteShowFanart.Text = Master.eLang.GetString(487, "Overwrite Existing Fanart")
        Me.chkResizeShowFanart.Text = Master.eLang.GetString(485, "Automatically Resize Fanart:")
        Me.lblShowFanartWidth.Text = Master.eLang.GetString(479, "Max Width:")
        Me.lblShowFanartHeight.Text = Master.eLang.GetString(480, "Max Height:")
        Me.lblShowFanartQ.Text = Master.eLang.GetString(484, "Fanart Quality:")
        Me.chkShowFanartOnly.Text = Master.eLang.GetString(145, "Only")
        Me.gbEpPosterOpts.Text = Master.eLang.GetString(148, "Poster")
        Me.lblEpPosterSize.Text = Master.eLang.GetString(482, "Preferred Poster Size")
        Me.chkOverwriteEpPoster.Text = Master.eLang.GetString(483, "Overwrite Existing Poster")
        Me.chkResizeEpPoster.Text = Master.eLang.GetString(481, "Automatically Resize Poster:")
        Me.lblEpPosterWidth.Text = Master.eLang.GetString(479, "Max Width:")
        Me.lblEpPosterHeight.Text = Master.eLang.GetString(480, "Max Height:")
        Me.lblEpPosterQ.Text = Master.eLang.GetString(478, "Poster Quality:")
        Me.gbEpFanartOpts.Text = Master.eLang.GetString(149, "Fanart")
        Me.lblEpFanartSize.Text = Master.eLang.GetString(486, "Preferred Fanart Size")
        Me.chkOverwriteEpFanart.Text = Master.eLang.GetString(487, "Overwrite Existing Fanart")
        Me.chkResizeEpFanart.Text = Master.eLang.GetString(485, "Automatically Resize Fanart:")
        Me.lblEpFanartWidth.Text = Master.eLang.GetString(479, "Max Width:")
        Me.lblEpFanartHeight.Text = Master.eLang.GetString(480, "Max Height:")
        Me.lblEpFanartQ.Text = Master.eLang.GetString(484, "Fanart Quality:")
        Me.chkEpFanartOnly.Text = Master.eLang.GetString(145, "Only")
        Me.chkForceTitle.Text = Master.eLang.GetString(710, "Force Title Language:")
        Me.chkSourceFromFolder.Text = Master.eLang.GetString(711, "Include Folder Name in Source Type Check")
        Me.chkSortBeforeScan.Text = Master.eLang.GetString(712, "Sort files into folder before each library update")

        If Not IsNothing(tvSettings.SelectedNode) Then
            Me.lblCurrent.Text = tvSettings.SelectedNode.Text
        Else
            Me.lblCurrent.Text = Master.eLang.GetString(38, "General")
        End If

        Me.TabPage1.Text = Master.eLang.GetString(38, "General")
        Me.TabPage2.Text = Master.eLang.GetString(390, "Options")
        Me.TabPage3.Text = Master.eLang.GetString(38, "General")
        Me.TabPage4.Text = Master.eLang.GetString(699, "Regex")
        Me.TabPage5.Text = Master.eLang.GetString(700, "TV Show")
        Me.TabPage7.Text = Master.eLang.GetString(999, "TV Season")
        Me.TabPage7.Text = Master.eLang.GetString(701, "TV Episode")

        Me.cbPosterSize.Items.AddRange(New Object() {Master.eLang.GetString(322, "X-Large"), Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small"), Master.eLang.GetString(558, "Wide")})
        Me.cbFanartSize.Items.AddRange(New Object() {Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small")})
        Me.cbAutoETSize.Items.AddRange(New Object() {Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small")})
        Me.cbShowPosterSize.Items.AddRange(New Object() {Master.eLang.GetString(322, "X-Large"), Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small"), Master.eLang.GetString(558, "Wide")})
        Me.cbShowFanartSize.Items.AddRange(New Object() {Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small")})
        Me.cbEpPosterSize.Items.AddRange(New Object() {Master.eLang.GetString(322, "X-Large"), Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small"), Master.eLang.GetString(558, "Wide")})
        Me.cbEpFanartSize.Items.AddRange(New Object() {Master.eLang.GetString(323, "Large"), Master.eLang.GetString(324, "Medium"), Master.eLang.GetString(325, "Small")})

        LoadTrailerQualities()
    End Sub

    Private Sub RemoveSortToken()
        If Me.lstSortTokens.Items.Count > 0 AndAlso Me.lstSortTokens.SelectedItems.Count > 0 Then
            While Me.lstSortTokens.SelectedItems.Count > 0
                Me.lstSortTokens.Items.Remove(Me.lstSortTokens.SelectedItems(0))
            End While
            Me.sResult.NeedsRefresh = True
            Me.SetApplyButton(True)
        End If
    End Sub

    Private Sub RemoveXCom()
        If Me.lbXBMCCom.SelectedItems.Count > 0 Then
            Me.XComs.RemoveAt(lbXBMCCom.SelectedIndex)
            Me.LoadXComs()
            Me.SetApplyButton(True)
        End If
    End Sub

    Private Sub RemoveFilter()
        If Me.lstFilters.Items.Count > 0 AndAlso Me.lstFilters.SelectedItems.Count > 0 Then
            While Me.lstFilters.SelectedItems.Count > 0
                Me.lstFilters.Items.Remove(Me.lstFilters.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
            Me.sResult.NeedsRefresh = True
        End If
    End Sub

    Private Sub RemoveShowFilter()
        If Me.lstShowFilters.Items.Count > 0 AndAlso Me.lstShowFilters.SelectedItems.Count > 0 Then
            While Me.lstShowFilters.SelectedItems.Count > 0
                Me.lstShowFilters.Items.Remove(Me.lstShowFilters.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
            Me.sResult.NeedsRefresh = True
        End If
    End Sub

    Private Sub RemoveEpFilter()
        If Me.lstEpFilters.Items.Count > 0 AndAlso Me.lstEpFilters.SelectedItems.Count > 0 Then
            While Me.lstEpFilters.SelectedItems.Count > 0
                Me.lstEpFilters.Items.Remove(Me.lstEpFilters.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
            Me.sResult.NeedsRefresh = True
        End If
    End Sub

    Private Sub RemoveMovieExt()
        If lstMovieExts.Items.Count > 0 AndAlso lstMovieExts.SelectedItems.Count > 0 Then
            While Me.lstMovieExts.SelectedItems.Count > 0
                Me.lstMovieExts.Items.Remove(Me.lstMovieExts.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
            Me.sResult.NeedsUpdate = True
        End If
    End Sub

    Private Sub RemoveNoStack()
        If lstNoStack.Items.Count > 0 AndAlso lstNoStack.SelectedItems.Count > 0 Then
            While Me.lstNoStack.SelectedItems.Count > 0
                Me.lstNoStack.Items.Remove(Me.lstNoStack.SelectedItems(0))
            End While
            Me.SetApplyButton(True)
            Me.sResult.NeedsUpdate = True
        End If
    End Sub

    Private Sub RemoveMetaData()
        If Me.lstMetaData.SelectedItems.Count > 0 Then
            For Each x As emmSettings.MetadataPerType In Meta
                If x.FileType = lstMetaData.SelectedItems(0).ToString Then
                    Meta.Remove(x)
                    LoadMetadata()
                    Me.SetApplyButton(True)
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub RemoveMovieSource()
        Try
            If Me.lvMovies.SelectedItems.Count > 0 Then
                If MsgBox(Master.eLang.GetString(418, "Are you sure you want to remove the selected sources? This will remove the movies from these sources from the Ember database."), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then
                    Me.lvMovies.BeginUpdate()

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                            While Me.lvMovies.SelectedItems.Count > 0
                                parSource.Value = lvMovies.SelectedItems(0).SubItems(1).Text
                                SQLcommand.CommandText = "SELECT Id FROM movies WHERE source = (?);"
                                Using SQLReader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                                    While SQLReader.Read
                                        Master.DB.DeleteFromDB(Convert.ToInt64(SQLReader("ID")), True)
                                    End While
                                End Using
                                SQLcommand.CommandText = String.Concat("DELETE FROM sources WHERE name = (?);")
                                SQLcommand.ExecuteNonQuery()
                                lvMovies.Items.Remove(lvMovies.SelectedItems(0))
                            End While
                        End Using
                        SQLtransaction.Commit()
                    End Using

                    Me.lvMovies.Sort()
                    Me.lvMovies.EndUpdate()
                    Me.lvMovies.Refresh()

                    Master.GetListOfSources()

                    Me.SetApplyButton(True)
                    Me.sResult.NeedsUpdate = True
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RemoveTVSource()
        Try
            If Me.lvTVSources.SelectedItems.Count > 0 Then
                If MsgBox(Master.eLang.GetString(418, "Are you sure you want to remove the selected sources? This will remove the TV Shows from these sources from the Ember database."), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then
                    Me.lvTVSources.BeginUpdate()

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                            While Me.lvTVSources.SelectedItems.Count > 0
                                parSource.Value = lvTVSources.SelectedItems(0).SubItems(1).Text
                                SQLcommand.CommandText = "SELECT Id FROM TVShows WHERE Source = (?);"
                                Using SQLReader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                                    While SQLReader.Read
                                        Master.DB.DeleteTVShowFromDB(Convert.ToInt64(SQLReader("ID")), True)
                                    End While
                                End Using
                                SQLcommand.CommandText = String.Concat("DELETE FROM TVSources WHERE name = (?);")
                                SQLcommand.ExecuteNonQuery()
                                lvTVSources.Items.Remove(lvTVSources.SelectedItems(0))
                            End While
                        End Using
                        SQLtransaction.Commit()
                    End Using

                    Me.lvTVSources.Sort()
                    Me.lvTVSources.EndUpdate()
                    Me.lvTVSources.Refresh()
                    Me.SetApplyButton(True)
                    Me.sResult.NeedsUpdate = True
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub EditShowRegex(ByVal lItem As ListViewItem)
        Me.btnAddShowRegex.Text = "Update Regex"
        Me.btnAddShowRegex.Tag = lItem.Text

        Me.txtSeasonRegex.Text = lItem.SubItems(1).Text

        Select Case lItem.SubItems(2).Text
            Case "Directory"
                Me.cboSeasonRetrieve.SelectedIndex = 0
            Case "File"
                Me.cboSeasonRetrieve.SelectedIndex = 1
        End Select

        Me.txtEpRegex.Text = lItem.SubItems(3).Text

        Select Case lItem.SubItems(4).Text
            Case "Directory"
                Me.cboEpRetrieve.SelectedIndex = 0
            Case "File"
                Me.cboEpRetrieve.SelectedIndex = 1
            Case "Result"
                Me.cboEpRetrieve.SelectedIndex = 2
        End Select
    End Sub

    Private Sub ClearRegex()
        Me.btnAddShowRegex.Text = "Add Regex"
        Me.btnAddShowRegex.Tag = String.Empty
        Me.btnAddShowRegex.Enabled = False
        Me.txtSeasonRegex.Text = String.Empty
        Me.cboSeasonRetrieve.SelectedIndex = -1
        Me.txtEpRegex.Text = String.Empty
        Me.cboEpRetrieve.SelectedIndex = -1
    End Sub

    Private Sub ValidateRegex()
        If Me.txtSeasonRegex.Text.Contains("?<season>") AndAlso Me.txtEpRegex.Text.Contains("?<episode>") AndAlso _
        Me.cboSeasonRetrieve.SelectedIndex > -1 AndAlso Me.cboEpRetrieve.SelectedIndex > -1 Then
            Me.btnAddShowRegex.Enabled = True
        Else
            Me.btnAddShowRegex.Enabled = False
        End If
    End Sub

    Private Sub RemoveRegex()
        Dim ID As Integer
        For Each lItem As ListViewItem In lvShowRegex.SelectedItems
            ID = Convert.ToInt32(lItem.Text)
            Dim selRex = From lRegex As emmSettings.TVShowRegEx In Me.ShowRegex Where lRegex.ID = ID
            If selRex.Count > 0 Then
                Me.ShowRegex.Remove(selRex(0))
                Me.SetApplyButton(True)
            End If
        Next
        Me.LoadShowRegex()
    End Sub

    Private Sub LoadRatingRegions()
        Me.cbRatingRegion.Items.AddRange(XML.GetRatingRegions)
    End Sub
#End Region '*** Routines/Functions

End Class