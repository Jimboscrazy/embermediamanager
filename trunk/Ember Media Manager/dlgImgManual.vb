﻿' ################################################################################
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

Imports System.Text.RegularExpressions
Imports System.IO

Public Class dlgImgManual

    Dim tImage As New Images
    Dim DLType As New Master.ImageType

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim tmpPathPlus As String = String.Empty

        Try
            If DLType = Master.ImageType.Fanart Then
                tmpPathPlus = Path.Combine(Master.TempPath, "fanart.jpg")
            Else
                tmpPathPlus = Path.Combine(Master.TempPath, "poster.jpg")
            End If

            If IsNothing(tImage.Image) Then
                tImage.FromWeb(Me.txtURL.Text)
            End If

            If Not IsNothing(tImage.Image) Then
                tImage.Save(tmpPathPlus)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtURL_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtURL.TextChanged
        If Not String.IsNullOrEmpty(Me.txtURL.Text) AndAlso Regex.IsMatch(Me.txtURL.Text, "^(https?://)?(([\w!~*'().&=+$%-]+: )?[\w!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([\w!~*'()-]+\.)*([\w^-][\w-]{0,61})?[\w]\.[a-z]{2,6})(:[0-9]{1,4})?((/*)|(/+[\w!~*'().;?:@&=+$,%#-]+)+/*)$", RegexOptions.IgnoreCase) Then
            Me.btnPreview.Enabled = True
            Me.OK_Button.Enabled = True
        Else
            Me.btnPreview.Enabled = False
            Me.OK_Button.Enabled = False
        End If
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Try
            tImage.FromWeb(Me.txtURL.Text)

            If Not IsNothing(tImage.Image) Then

                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(tImage.Image)
                End Using

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgImgManual_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        tImage.Dispose()
        tImage = Nothing
    End Sub

    Public Overloads Function ShowDialog(ByVal _DLType As Master.ImageType) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me.DLType = _DLType

        Return MyBase.ShowDialog()
    End Function

    Private Sub dlgImgManual_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()

        If Me.DLType = Master.ImageType.Fanart Then
            Me.Text = Master.eLang.GetString(182, "Manual Fanart Entry")
        Else
            Me.Text = Master.eLang.GetString(183, "Manual Poster Entry")
        End If
    End Sub

    Private Sub dlgImgManual_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub SetUp()
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.btnPreview.Text = Master.eLang.GetString(180, "Preview")
        Me.Label1.Text = Master.eLang.GetString(181, "Enter URL to Image:")
    End Sub
End Class