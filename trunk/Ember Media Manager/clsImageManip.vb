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

Public Class ImageManip
    Public Shared Sub ResizePB(ByRef pbResize As PictureBox, ByRef pbCache As PictureBox, ByVal maxHeight As Integer, ByVal maxWidth As Integer)

        '//
        ' Resize the picture box based on the dimensions of the image and the dimensions
        ' of the available space... try to use the most screen real estate
        '
        ' Why not use "Zoom" for fanart background? - To keep the image at the top. Zoom centers vertically.
        '\\

        Try
            If Not IsNothing(pbCache.Image) Then
                pbResize.SizeMode = PictureBoxSizeMode.Normal
                Dim sPropPerc As Single = 1.0 'no default scaling

                pbResize.Size = New Size(maxWidth, maxHeight)

                ' Height
                If pbCache.Image.Height > pbResize.Height Then
                    ' Reduce height first
                    sPropPerc = CSng(pbResize.Height / pbCache.Image.Height)
                End If

                ' Width
                If (pbCache.Image.Width * sPropPerc) > pbResize.Width Then
                    ' Scaled width exceeds Box's width, recalculate scale_factor
                    sPropPerc = CSng(pbResize.Width / pbCache.Image.Width)
                End If

                ' Get the source bitmap.
                Dim bmSource As New Bitmap(pbCache.Image)
                ' Make a bitmap for the result.
                Dim bmDest As New Bitmap( _
                Convert.ToInt32(bmSource.Width * sPropPerc), _
                Convert.ToInt32(bmSource.Height * sPropPerc))
                ' Make a Graphics object for the result Bitmap.
                Dim grDest As Graphics = Graphics.FromImage(bmDest)
                ' Copy the source image into the destination bitmap.
                grDest.DrawImage(bmSource, 0, 0, _
                bmDest.Width + 1, _
                bmDest.Height + 1)
                ' Display the result.
                pbResize.Image = bmDest

                'tweak pb after resizing pic
                pbResize.Width = pbResize.Image.Width
                pbResize.Height = pbResize.Image.Height
                'center it

                'Clean up
                bmSource = Nothing
                bmDest = Nothing
                grDest = Nothing
            Else
                pbResize.Left = 0
                pbResize.Size = New Size(maxWidth, maxHeight)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Sub SetOverlay(ByRef pbOverlay As PictureBox)

        '//
        ' Put our crappy glossy overlay over the poster
        '\\

        Try
            Dim bmOverlay As New Bitmap(pbOverlay.Image)
            Dim grOverlay As Graphics = Graphics.FromImage(bmOverlay)
            Dim bmHeight As Integer = pbOverlay.Image.Height * 0.65

            grOverlay.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            grOverlay.DrawImage(My.Resources.overlay, 0, 0, pbOverlay.Image.Width, bmHeight)
            pbOverlay.Image = bmOverlay

            bmOverlay = New Bitmap(pbOverlay.Image)
            grOverlay = Graphics.FromImage(bmOverlay)

            grOverlay.DrawImage(My.Resources.overlay2, 0, 0, pbOverlay.Image.Width, pbOverlay.Image.Height)
            pbOverlay.Image = bmOverlay

            grOverlay.Dispose()
            bmOverlay = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Sub DrawGradEllipse(ByRef graphics As Graphics, ByVal bounds As Rectangle, ByVal color1 As Color, ByVal color2 As Color)
        Try
            Dim rPoints() As Point = { _
            New Point(bounds.X, bounds.Y), _
            New Point(bounds.X + bounds.Width, bounds.Y), _
            New Point(bounds.X + bounds.Width, bounds.Y + bounds.Height), _
            New Point(bounds.X, bounds.Y + bounds.Height) _
        }
            Dim pgBrush As New Drawing2D.PathGradientBrush(rPoints)
            Dim gPath As New Drawing2D.GraphicsPath
            gPath.AddEllipse(bounds.X, bounds.Y, bounds.Width, bounds.Height)
            pgBrush = New Drawing2D.PathGradientBrush(gPath)
            pgBrush.CenterColor = color1
            pgBrush.SurroundColors = New Color() {color2}
            graphics.FillEllipse(pgBrush, bounds.X, bounds.Y, bounds.Width, bounds.Height)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
End Class
