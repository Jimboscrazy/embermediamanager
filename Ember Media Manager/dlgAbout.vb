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




Public NotInheritable Class dlgAbout

    Dim CredList As New List(Of CredLine)
    Dim PicY As Single

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim iBackground As New Bitmap(Me.picDisplay.Width, Me.picDisplay.Height)
        Dim iLogo As New Bitmap(My.Resources.Logo)
        For xPix As Integer = 0 To iLogo.Width - 1
            For yPix As Integer = 0 To iLogo.Height - 1
                Dim clr As Color = iLogo.GetPixel(xPix, yPix)
                If clr.A > 128 Then
                    clr = Color.FromArgb(128, clr.R, clr.G, clr.B)
                    iLogo.SetPixel(xPix, yPix, clr)
                End If
            Next
        Next

        Using g As Graphics = Graphics.FromImage(iBackground)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            Dim DrawRect As New Rectangle(0, 0, picDisplay.ClientSize.Width, Convert.ToInt32(picDisplay.ClientSize.Height * 0.735))
            g.FillRectangle(New Drawing2D.LinearGradientBrush(DrawRect, Color.FromArgb(255, 200, 200, 255), Color.FromArgb(255, 250, 250, 250), Drawing2D.LinearGradientMode.Vertical), DrawRect)
            DrawRect = New Rectangle(0, Convert.ToInt32(picDisplay.ClientSize.Height * 0.735), picDisplay.ClientSize.Width, Convert.ToInt32(picDisplay.ClientSize.Height * 0.265))
            g.FillRectangle(New Drawing2D.LinearGradientBrush(DrawRect, Color.White, Color.FromArgb(255, 230, 230, 230), Drawing2D.LinearGradientMode.Vertical), DrawRect)
            Dim x As Integer = Convert.ToInt32((picDisplay.Width - My.Resources.Logo.Width) / 2)
            Dim y As Integer = Convert.ToInt32((picDisplay.Height - My.Resources.Logo.Height) / 2)
            g.DrawImage(iLogo, x, y, My.Resources.Logo.Width, My.Resources.Logo.Height)
            Me.picDisplay.BackgroundImage = iBackground
        End Using

        Me.Text = "About Ember Media Manager"

        ' Optimize Painting.
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)

        CredList.Add(New CredLine With {.Text = "Ember Media Manager", .Font = New Font("Microsoft Sans Serif", 18, FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Format("Version r{0}", My.Application.Info.Version.Revision), .Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = My.Application.Info.Description, .Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = My.Application.Info.Copyright, .Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Created By: Jason Schnitzler", .Font = New Font("Microsoft Sans Serif", 8, FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "__________Project Coders__________", .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Underline Or FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Jason ""nul7"" Schnitzler"})
        CredList.Add(New CredLine With {.Text = "Nuno ""Zordor"" Novais"})
        CredList.Add(New CredLine With {.Text = """RogueDazza"""})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "__________Project Manager__________", .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Underline Or FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Bence ""olympia"" Nádas"})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "_________Special Thanks_________", .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Underline Or FontStyle.Bold)})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Carlos ""asphinx"" Nabb - Genre Images"})
        CredList.Add(New CredLine With {.Text = "Krzysztof ""Halibutt"" Machocki - Studio Icon Pack"})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Ember Media Manager is free software:"})
        CredList.Add(New CredLine With {.Text = "you can redistribute it and/or modify"})
        CredList.Add(New CredLine With {.Text = "it under the terms of the GNU General"})
        CredList.Add(New CredLine With {.Text = "Public License as published by the Free"})
        CredList.Add(New CredLine With {.Text = "Software Foundation, either version 3"})
        CredList.Add(New CredLine With {.Text = "of the License, or (at your option) any"})
        CredList.Add(New CredLine With {.Text = "later version."})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "Ember Media Manager is distributed in"})
        CredList.Add(New CredLine With {.Text = "the hope that it will be useful, but"})
        CredList.Add(New CredLine With {.Text = "WITHOUT ANY WARRANTY; without even the"})
        CredList.Add(New CredLine With {.Text = "implied warranty of MERCHANTABILITY or"})
        CredList.Add(New CredLine With {.Text = "FITNESS FOR A PARTICULAR PURPOSE."})
        CredList.Add(New CredLine With {.Text = String.Empty})
        CredList.Add(New CredLine With {.Text = "See the GNU General Public License for more details."})

        PicY = Me.picDisplay.ClientSize.Height

    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub pbTMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbTMDB.Click
        Process.Start("http://www.themoviedb.org/")
    End Sub

    Private Sub pbIMPA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbIMPA.Click
        Process.Start("http://www.impawards.com/")
    End Sub

    Private Sub pbIMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbIMDB.Click
        Process.Start("http://www.imdb.com/")
    End Sub

    Private Sub pbMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbMI.Click
        Process.Start("http://mediainfo.sourceforge.net")
    End Sub

    Private Sub pbFFMPEG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbFFMPEG.Click
        Process.Start("http://www.ffmpeg.org/")
    End Sub

    Private Sub pbMPDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbMPDB.Click
        Process.Start("http://www.moviepostersdb.com/")
    End Sub

    Private Sub pbXBMC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbXBMC.Click
        Process.Start("http://www.xbmc.org/")
    End Sub

    Private Sub pbYouTube_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbYouTube.Click
        Process.Start("http://www.youtube.com/")
    End Sub

    Private Sub picDisplay_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles picDisplay.Paint
        Dim CurrentX As Single, CurrentY As Single, FontMod As Single = 0

        For i As Integer = 0 To CredList.Count - 1

            CurrentY = PicY + FontMod
            FontMod += CredList(i).Font.Size + 5

            CurrentX = (Me.picDisplay.ClientSize.Width - e.Graphics.MeasureString(CredList(i).Text, CredList(i).Font).Width) / 2

            If i = CredList.Count - 1 And CurrentY < -25 Then PicY = Me.picDisplay.ClientSize.Height

            e.Graphics.DrawString(CredList(i).Text, CredList(i).Font, Brushes.Black, CurrentX, CurrentY)

        Next

        PicY -= 1

        System.Threading.Thread.Sleep(30)

        Me.picDisplay.Invalidate()

    End Sub

    Friend Class CredLine
        Private _text As String
        Private _font As Font

        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal value As String)
                _text = value
            End Set
        End Property

        Public Property Font() As Font
            Get
                Return _font
            End Get
            Set(ByVal value As Font)
                _font = value
            End Set
        End Property

        Public Sub New()
            clear()
        End Sub

        Public Sub Clear()
            _text = String.Empty
            _font = New Font("Microsoft Sans Serif", 11, FontStyle.Bold)
        End Sub
    End Class

    Private Sub dlgAbout_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
        Me.Refresh()
    End Sub

    Private Sub picDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picDisplay.Click
        Process.Start("http://code.google.com/p/embermediamanager/")
    End Sub
End Class
