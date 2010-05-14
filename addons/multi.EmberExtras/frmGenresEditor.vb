Imports System.Windows.Forms

Public Class frmGenresEditor
    Public Event ModuleSettingsChanged()
    Private Genres As New List(Of cGenre)


    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GetGenres()
    End Sub

    Sub GetGenres()
        Try
            'Dim xGenre = From xGen In GenreXML...<supported>.Descendants Select xGen.Value
            'If xGenre.Count > 0 Then
            'retGenre.AddRange(xGenre.ToArray)
            'End If

            Dim xGenre = From xGen In APIXML.GenreXML...<name> Select xGen.@searchstring, xGen.@language, xGen.Value
            If xGenre.Count > 0 Then
                Dim genre As New cGenre
                For i As Integer = 0 To xGenre.Count - 1
                    genre.langs.AddRange(xGenre(i).language.Split(Convert.ToChar("|")))
                    genre.searchstring = xGenre(i).searchstring
                    genre.image = xGenre(i).Value
                    Genres.Add(genre)
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Class cGenre
        Public searchstring As String
        Public langs As New List(Of String)
        Public image As String
    End Class
End Class
