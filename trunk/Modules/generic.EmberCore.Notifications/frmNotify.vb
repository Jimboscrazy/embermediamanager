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

Public Class frmNotify

    Private Shared DisplayedForms As New List(Of frmNotify)

    Protected AnimationTimer As New Timer()
    Protected AnimationType As AnimationTypes = AnimationTypes.Show

    Public Enum AnimationTypes
        Show = 0
        Wait = 1
        Close = 2
    End Enum

    Protected Overrides ReadOnly Property ShowWithoutActivation() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overloads Sub Show(ByVal _icon As Integer, ByVal _title As String, ByVal _message As String, ByVal _customicon As Image)
        If Not IsNothing(_customicon) Then
            Me.pbIcon.Image = _customicon
        Else
            Select Case _icon
                Case 1
                    Me.pbIcon.Image = My.Resources._error
                Case 2
                    Me.pbIcon.Image = My.Resources._comment
                Case 3
                    Me.pbIcon.Image = My.Resources._movie
                Case 4
                    Me.pbIcon.Image = My.Resources._tv
                Case Else
                    Me.pbIcon.Image = My.Resources._info
            End Select
        End If
        Me.lblTitle.Text = _title
        Me.lblMessage.Text = _message

        MyBase.Show()
    End Sub

    Private Sub frmNotify_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        frmNotify.DisplayedForms.Remove(Me)
    End Sub

    Private Sub frmNotify_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        AddHandler AnimationTimer.Tick, AddressOf OnTimer

        Me.SetBounds(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 5, Screen.PrimaryScreen.WorkingArea.Height - 5, 315, 0)

        For Each DisplayedForm As frmNotify In frmNotify.DisplayedForms
            DisplayedForm.Top -= 5
        Next

        frmNotify.DisplayedForms.Add(Me)

    End Sub

    Protected Sub OnTimer(ByVal sender As Object, ByVal e As EventArgs)

        Select Case Me.AnimationType

            Case AnimationTypes.Show

                If Me.Height < 80 Then
                    Me.SetBounds(Me.Left, Me.Top - 2, Me.Width, Me.Height + 2)

                    For Each DisplayedForm As frmNotify In frmNotify.DisplayedForms.Where(Function(s) Not s Is Me)
                        DisplayedForm.Top -= 2
                    Next
                Else
                    Me.AnimationTimer.Stop()
                    Me.Height = 80
                    Me.AnimationTimer.Interval = 3000
                    Me.AnimationType = AnimationTypes.Wait
                    Me.AnimationTimer.Start()
                End If

            Case AnimationTypes.Wait

                Me.AnimationTimer.Stop()
                Me.AnimationTimer.Interval = 5
                Me.AnimationType = AnimationTypes.Close
                Me.AnimationTimer.Start()

            Case AnimationTypes.Close

                If Me.Height > 0 Then
                    Me.SetBounds(Me.Left, Me.Top + 2, Me.Width, Me.Height - 2)
                Else
                    Me.Close()
                End If

        End Select
        Me.Refresh()
    End Sub

    Private Sub frmNotify_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.AnimationTimer.Stop()
        Me.AnimationTimer.Interval = 5
        Me.AnimationType = AnimationTypes.Show
        Me.AnimationTimer.Start()
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub
End Class
