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

Imports System.Windows.Forms

Public Class frmInfoSettingsHolder

    Private Sub frmInfoSettingsHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.chkOFDBGenre.Text = Master.eLang.GetString(474, "Use OFDB Genre")
        Me.chkOFDBPlot.Text = Master.eLang.GetString(475, "Use OFDB Plot")
        Me.chkOFDBOutline.Text = Master.eLang.GetString(476, "Use OFDB Outline")
        Me.chkOFDBTitle.Text = Master.eLang.GetString(477, "Use OFDB Title")
        Me.Label18.Text = Master.eLang.GetString(509, "IMDB Mirror:")
        Me.gbOptions.Text = Master.eLang.GetString(577, "Scraper Fields")
        Me.chkCrew.Text = Master.eLang.GetString(391, "Other Crew**")
        Me.chkMusicBy.Text = Master.eLang.GetString(392, "Music By**")
        Me.chkProducers.Text = Master.eLang.GetString(393, "Producers**")
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
        Me.chkMPAA.Text = Master.eLang.GetString(401, "MPAA")
        Me.chkYear.Text = Master.eLang.GetString(278, "Year")
        Me.chkTitle.Text = Master.eLang.GetString(21, "Title")
        Me.Label46.Text = Master.eLang.GetString(797, "*Scrape Full Crew Must Be Enabled")
        Me.Label1.Text = Master.eLang.GetString(9999, "This are Scraper specific Settings, and act as a filter.\nYou should check Ember Global Setting also.").Replace("\n", vbCrLf)
    End Sub

End Class
