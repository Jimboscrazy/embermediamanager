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
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class dlgUpdateMedia
    Private bNFO As Boolean
    Private bPoster As Boolean
    Private bFanart As Boolean
    Private bExtrathumbs As Boolean
    Private bMediaInfo As Boolean
    Private bTrailers As Boolean


    Private Sub dlgUpdateMedia_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

    End Sub

    Private Sub dlgUpdateMedia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim newCount As Integer = 0
            Dim markCount As Integer = 0
            For Each drvRow As DataRow In frmMain.GetMediaTable().Rows
                If drvRow.Item(10) Then 'New
                    newCount += 1
                End If
                If drvRow.Item(11) Then 'Marked
                    markCount += 1
                End If
            Next
            If newCount Then
                rbUpdateModifier_New.Enabled = True
                rbUpdateModifier_New.Checked = True
            End If
            If markCount Then
                rbUpdate_Marked.Enabled = True
                rbUpdate_Marked.Checked = True
            End If
            ValidateOptions()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.Close()
    End Sub

    Private Sub cbItems_All_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbItems_All.CheckedChanged
        If sender.Checked Then
            For i As Integer = 0 To cbItems.Items.Count - 1
                cbItems.SetItemCheckState(i, CheckState.Indeterminate)
            Next
            cbItems.Enabled = False
        Else
            For i As Integer = 0 To cbItems.Items.Count - 1
                cbItems.SetItemCheckState(i, CheckState.Unchecked)
            Next
            cbItems.Enabled = True
        End If
        ValidateOptions()
    End Sub
    Sub ValidateOptions()
        If cbItems_All.Checked Then
            bMediaInfo = True
            bPoster = True
            bFanart = True
            bExtrathumbs = True
            bNFO = True
            bTrailers = True
        Else
            For i As Integer = 0 To cbItems.Items.Count - 1
                Select Case cbItems.Items(i)
                    Case "Media Info"
                        bMediaInfo = cbItems.GetItemCheckState(i)
                    Case "Trailers"
                        bTrailers = cbItems.GetItemCheckState(i)
                    Case "Extrathumbs"
                        bExtrathumbs = cbItems.GetItemCheckState(i)
                    Case "Fanart"
                        bFanart = cbItems.GetItemCheckState(i)
                    Case "NFO"
                        bNFO = cbItems.GetItemCheckState(i)
                    Case "Poster"
                        bPoster = cbItems.GetItemCheckState(i)
                End Select
            Next
        End If
        gbMediaInfo.Enabled = bMediaInfo
        lbTrailerSites.Enabled = bTrailers
        chkUseETasFA.Enabled = (bFanart And bExtrathumbs)
        txtAutoThumbs.Enabled = bExtrathumbs

    End Sub

    Private Sub cbItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbItems.SelectedIndexChanged
        ValidateOptions()
    End Sub
End Class
