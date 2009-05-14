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

Public NotInheritable Class dlgAbout

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If Not String.IsNullOrEmpty(My.Application.Info.Title) Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        Me.lblProductName.Text = My.Application.Info.ProductName
        Me.lblVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.lblCopyright.Text = My.Application.Info.Copyright
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub pbTMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbTMDB.Click
        System.Diagnostics.Process.Start("http://www.themoviedb.org/")
    End Sub

    Private Sub pbIMPA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbIMPA.Click
        System.Diagnostics.Process.Start("http://www.impawards.com/")
    End Sub

    Private Sub pbIMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbIMDB.Click
        System.Diagnostics.Process.Start("http://www.imdb.com/")
    End Sub

    Private Sub pbMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbMI.Click
        System.Diagnostics.Process.Start("http://mediainfo.sourceforge.net")
    End Sub

    Private Sub pbFFMPEG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbFFMPEG.Click
        System.Diagnostics.Process.Start("http://www.ffmpeg.org/")
    End Sub
End Class
