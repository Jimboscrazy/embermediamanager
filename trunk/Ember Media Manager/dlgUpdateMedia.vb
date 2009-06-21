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

Public Class dlgUpdateMedia

    Private CustomUpdater As New Master.CustomUpdaterStruct

    Private Sub dlgUpdateMedia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            'set defaults
            CustomUpdater.ScrapeType = Master.ScrapeType.FullAuto
            CustomUpdater.Modifier = Master.ScrapeModifier.All

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub rbAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbAll.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.All
        Me.gbOptions.Enabled = True
    End Sub

    Private Sub rbNfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbNfo.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.NFO
        Me.gbOptions.Enabled = True
    End Sub

    Private Sub rbPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbPoster.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.Poster
        Me.gbOptions.Enabled = False
    End Sub

    Private Sub rbFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFanart.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.Fanart
        Me.gbOptions.Enabled = False
    End Sub

    Private Sub rbExtra_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbExtra.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.Extra
        Me.gbOptions.Enabled = False
    End Sub

    Private Sub rbTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTrailer.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.Trailer
        Me.gbOptions.Enabled = False
    End Sub

    Private Sub rbMediaInfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMediaInfo.CheckedChanged
        Me.CustomUpdater.Modifier = Master.ScrapeModifier.MI
        Me.gbOptions.Enabled = False
    End Sub

    Private Sub rbUpdateModifier_All_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdateModifier_All.CheckedChanged
        If Me.rbUpdate_Auto.Checked Then
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.FullAuto
        Else
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.FullAsk
        End If
    End Sub

    Private Sub rbUpdateModifier_Missing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdateModifier_Missing.CheckedChanged
        If Me.rbUpdate_Auto.Checked Then
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.UpdateAuto
        Else
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.UpdateAsk
        End If
    End Sub

    Private Sub rbUpdateModifier_New_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdateModifier_New.CheckedChanged
        If Me.rbUpdate_Auto.Checked Then
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.NewAuto
        Else
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.NewAsk
        End If
    End Sub

    Private Sub rbUpdateModifier_Marked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdateModifier_Marked.CheckedChanged
        If Me.rbUpdate_Auto.Checked Then
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.MarkAuto
        Else
            Me.CustomUpdater.ScrapeType = Master.ScrapeType.MarkAsk
        End If
    End Sub

    Private Sub rbUpdate_Auto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdate_Auto.CheckedChanged
        Select Case True
            Case Me.rbUpdateModifier_All.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.FullAuto
            Case Me.rbUpdateModifier_Missing.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.UpdateAuto
            Case Me.rbUpdateModifier_New.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.NewAuto
            Case Me.rbUpdateModifier_Marked.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.MarkAuto
        End Select
    End Sub

    Private Sub rbUpdate_Ask_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbUpdate_Ask.CheckedChanged
        Select Case True
            Case Me.rbUpdateModifier_All.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.FullAsk
            Case Me.rbUpdateModifier_Missing.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.UpdateAsk
            Case Me.rbUpdateModifier_New.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.NewAsk
            Case rbUpdateModifier_Marked.Checked
                Me.CustomUpdater.ScrapeType = Master.ScrapeType.MarkAsk
        End Select
    End Sub

    Private Sub chkTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTitle.CheckedChanged
        CustomUpdater.Options.bTitle = chkTitle.Checked
    End Sub

    Private Sub chkYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkYear.CheckedChanged
        CustomUpdater.Options.bYear = chkYear.Checked
    End Sub

    Private Sub chkMPAA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMPAA.CheckedChanged
        CustomUpdater.Options.bMPAA = chkMPAA.Checked
    End Sub

    Private Sub chkRelease_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRelease.CheckedChanged
        CustomUpdater.Options.bRelease = chkRelease.Checked
    End Sub

    Private Sub chkRuntime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRuntime.CheckedChanged
        CustomUpdater.Options.bRuntime = chkRuntime.Checked
    End Sub

    Private Sub chkRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRating.CheckedChanged
        CustomUpdater.Options.bRating = chkRating.Checked
    End Sub

    Private Sub chkVotes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkVotes.CheckedChanged
        CustomUpdater.Options.bVotes = chkVotes.Checked
    End Sub

    Private Sub chkStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStudio.CheckedChanged
        CustomUpdater.Options.bStudio = chkStudio.Checked
    End Sub

    Private Sub chkGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGenre.CheckedChanged
        CustomUpdater.Options.bGenre = chkGenre.Checked
    End Sub

    Private Sub chkTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTrailer.CheckedChanged
        CustomUpdater.Options.bTrailer = chkTrailer.Checked
    End Sub

    Private Sub chkTagline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTagline.CheckedChanged
        CustomUpdater.Options.bTagline = chkTagline.Checked
    End Sub

    Private Sub chkOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOutline.CheckedChanged
        CustomUpdater.Options.bOutline = chkOutline.Checked
    End Sub

    Private Sub chkPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPlot.CheckedChanged
        CustomUpdater.Options.bPlot = chkPlot.Checked
    End Sub

    Private Sub chkCast_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCast.CheckedChanged
        CustomUpdater.Options.bCast = chkCast.Checked
    End Sub

    Private Sub chkDirector_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDirector.CheckedChanged
        CustomUpdater.Options.bDirector = chkDirector.Checked
    End Sub

    Private Sub chkWriters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWriters.CheckedChanged
        CustomUpdater.Options.bWriters = chkWriters.Checked
    End Sub

    Private Sub chkProducers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProducers.CheckedChanged
        CustomUpdater.Options.bProducers = chkProducers.Checked
    End Sub

    Private Sub chkMusicBy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMusicBy.CheckedChanged
        CustomUpdater.Options.bMusicBy = chkMusicBy.Checked
    End Sub

    Private Sub chkCrew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCrew.CheckedChanged
        CustomUpdater.Options.bOtherCrew = chkCrew.Checked
    End Sub

    Public Overloads Function ShowDialog() As Master.CustomUpdaterStruct

        '//
        ' Overload to pass data
        '\\

        If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return Me.CustomUpdater
        Else
            Return Nothing
        End If

    End Function

    Private Sub Update_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Update_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class
