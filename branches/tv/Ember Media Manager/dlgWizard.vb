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



Imports System.IO

Public Class dlgWizard

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.SaveSettings()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        Select Case True
            Case Me.Panel1.Visible
                Me.btnBack.Enabled = True
                Me.Panel1.Visible = False
                Me.Panel2.Visible = True
            Case Me.Panel2.Visible
                Me.Panel2.Visible = False
                Me.Panel3.Visible = True
            Case Me.Panel3.Visible
                Me.Panel3.Visible = False
                Me.Panel4.Visible = True
                Me.btnNext.Enabled = False
                Me.OK_Button.Enabled = True
        End Select
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Select Case True
            Case Me.Panel2.Visible
                Me.btnBack.Enabled = False
                Me.Panel2.Visible = False
                Me.Panel1.Visible = True
            Case Me.Panel3.Visible
                Me.Panel3.Visible = False
                Me.Panel2.Visible = True
            Case Me.Panel4.Visible
                Me.Panel4.Visible = False
                Me.Panel3.Visible = True
                Me.btnNext.Enabled = True
                Me.OK_Button.Enabled = False
        End Select
    End Sub

    Private Sub btnMovieAddFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFolder.Click
        Using dSource As New dlgMovieSource
            If dSource.ShowDialog = Windows.Forms.DialogResult.OK Then RefreshSources()
        End Using
    End Sub

    Private Sub btnMovieRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieRem.Click
        Me.RemoveSource()
    End Sub

    Private Sub RemoveSource()
        Try
            If Me.lvMovies.SelectedItems.Count > 0 Then
                If MsgBox(Master.eLang.GetString(418, "Are you sure you want to remove the selected sources? This will remove the movies from these sources from the Ember database."), MsgBoxStyle.Question Or MsgBoxStyle.YesNo, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then
                    Me.lvMovies.BeginUpdate()

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                            For i As Integer = lvMovies.SelectedItems.Count - 1 To 0 Step -1
                                parSource.Value = lvMovies.SelectedItems(i).SubItems(1).Text
                                SQLcommand.CommandText = String.Concat("DELETE FROM movies WHERE source = (?);")
                                SQLcommand.ExecuteNonQuery()
                                SQLcommand.CommandText = String.Concat("DELETE FROM sources WHERE name = (?);")
                                SQLcommand.ExecuteNonQuery()
                                lvMovies.Items.RemoveAt(i)
                            Next
                        End Using
                        SQLtransaction.Commit()
                    End Using

                    Me.lvMovies.Sort()
                    Me.lvMovies.EndUpdate()
                    Me.lvMovies.Refresh()
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgWizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()
        Me.LoadIntLangs()
        Me.FillSettings()
    End Sub

    Private Sub LoadIntLangs()

        If Directory.Exists(Path.Combine(Master.AppPath, "Langs")) Then
            Dim alL As New ArrayList
            Dim alLangs As New ArrayList
            Try
                alL.AddRange(Directory.GetFiles(Path.Combine(Master.AppPath, "Langs"), "*).xml"))
            Catch
            End Try
            alLangs.AddRange(alL.Cast(Of String)().Select(Function(AL) Path.GetFileNameWithoutExtension(AL)).ToArray)
            Me.cbIntLang.Items.AddRange(alLangs.ToArray)
        End If

    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(402, "Ember Startup Wizard")
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.btnBack.Text = Master.eLang.GetString(403, "< Back")
        Me.btnNext.Text = Master.eLang.GetString(404, "Next >")
        Me.Label1.Text = Master.eLang.GetString(405, "Welcome to Ember Media Manager")
        Me.GroupBox6.Text = Master.eLang.GetString(149, "Fanart")
        Me.GroupBox5.Text = Master.eLang.GetString(148, "Poster")
        Me.Label5.Text = Master.eLang.GetString(406, "TIP: Selections containing the text <movie> means that Ember Media Manager will use the filename of the movie.")
        Me.btnMovieRem.Text = Master.eLang.GetString(30, "Remove")
        Me.btnMovieAddFolder.Text = Master.eLang.GetString(407, "Add Source")
        Me.Label6.Text = Master.eLang.GetString(408, "That wasn't so hard was it?  As mentioned earlier, you can change these or any other options in the Settings dialog.")
        Me.Label7.Text = String.Format(Master.eLang.GetString(409, "That's it!{0}Ember Media Manager is Ready!"), vbNewLine)
        Me.colName.Text = Master.eLang.GetString(232, "Name")
        Me.colPath.Text = Master.eLang.GetString(410, "Path")
        Me.colRecur.Text = Master.eLang.GetString(411, "Recursive")
        Me.colFolder.Text = Master.eLang.GetString(412, "Use Folder Name")
        Me.colSingle.Text = Master.eLang.GetString(413, "Single Video")
        Me.Label2.Text = String.Format(Master.eLang.GetString(415, "This is either your first time running Ember Media Manager or you have upgraded to a newer version.  There are a few things Ember Media Manager needs to know to work properly.  This wizard will walk you through configuring Ember Media Manager to work for your set up.{0}{0}Only a handful of settings will be covered in this wizard. You can change these or any other setting at any time by selecting ""Settings..."" from the ""Edit"" menu."), vbNewLine)
        Me.Label4.Text = Master.eLang.GetString(416, "Now that Ember Media Manager knows WHERE to look for the files, we need to tell it WHAT files to look for.  Simply select any combination of files type you wish Ember Media Manager to load from and save to.  You can select more than one from each section if you wish.")
        Me.Label3.Text = Master.eLang.GetString(414, "First, let's tell Ember Media Manager where all our movies are.  If each of your movie files are in a separate folder, add a Folder Path pointing to the parent folder.  If all your movie files are in the same folder, add a Files Path pointing to the parent folder. You can select as many Folder Paths, File Paths, or any combination of both that you wish.")
        Me.Label8.Text = String.Format(Master.eLang.GetString(417, "Some options you may be interested in:{0}{0}Custom Filters - If your movie files have things like ""DVDRip"", ""BluRay"", ""x264"", etc in their folder or file name and you wish to filter the names when loading into the media list, you can utilize the Custom Filter option.  The custom filter is RegEx compatible for maximum usability.{0}{0}Images - This section allows you to select which websites to ""scrape"" images from as well as select a preferred size for the images Ember Media Manager selects.{0}{0}Locks - This section allows you to ""lock"" certain information so it does not get updated even if you re-scrape the movie. This is useful if you manually edit the title, outline, or plot of a movie and wish to keep your changes."), vbNewLine)
        Me.chkMovieNameMultiOnly.Text = Master.eLang.GetString(472, "Use <movie> Only for Folders with Multiple Movies")
        Me.Label32.Text = Master.eLang.GetString(430, "Interface Language")
    End Sub

    Private Sub FillSettings()
        Me.RefreshSources()

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
        Me.cbIntLang.SelectedItem = Master.eSettings.Language
    End Sub

    Private Sub RefreshSources()
        lvMovies.Items.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT * FROM sources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    Dim lvItem As New ListViewItem(SQLreader("ID").ToString)
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

    Private Sub SaveSettings()
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
        Master.eSettings.Language = Me.cbIntLang.Text
    End Sub

    Private Sub dlgWizard_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub lvMovies_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvMovies.DoubleClick
        If lvMovies.SelectedItems.Count > 0 Then
            Using dMovieSource As New dlgMovieSource
                If dMovieSource.ShowDialog(Convert.ToInt32(lvMovies.SelectedItems(0).Text)) = Windows.Forms.DialogResult.OK Then
                    Me.RefreshSources()
                End If
            End Using
        End If
    End Sub

    Private Sub cbIntLang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIntLang.SelectedIndexChanged
        If Not String.IsNullOrEmpty(Me.cbIntLang.SelectedItem.ToString) Then
            Master.eLang.LoadLanguage(Me.cbIntLang.SelectedItem.ToString)
            Me.SetUp()
        End If
    End Sub

    Private Sub lvMovies_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvMovies.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveSource()
    End Sub

End Class
