Imports System.IO

Public Class Master
    Public Shared eSettings As New Settings
    Public Shared eAdvancedSettings As New AdvancedSettings

    Public Shared eLang As New Localization
    Public Shared SourcesList As New List(Of String)
    Public Shared TempPath As String = Path.Combine(Functions.AppPath, "Temp")
    Public Shared DB As New Database
    Public Shared DefaultOptions As New Structures.ScrapeOptions
    Public Shared DefaultTVOptions As New Structures.TVScrapeOptions
    Public Shared GlobalScrapeMod As New Structures.ScrapeModifier

    Public Shared isWindows As Boolean = Functions.CheckIfWindows

    Public Shared currMovie As New Structures.DBMovie
    Public Shared currShow As New Structures.DBTV
    Public Shared CanScanDiscImage As Boolean
    Public Shared tmpMovie As New MediaContainers.Movie

    Public Shared TVScraper As New TVDB.Scraper

End Class
