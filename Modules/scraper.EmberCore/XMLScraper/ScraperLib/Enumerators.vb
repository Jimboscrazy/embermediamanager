Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace XMLScraper
    Namespace ScraperLib

#Region "Enumerations"

        Public Enum FunctionType
            NfoUrl = 0
            CreateSearchUrl = 1
            GetSearchResults = 2
            GetDetails = 3
            GetList = 4
            GetListDetails = 5
            CustomFunction = 6
        End Enum

        Public Enum IncludeContent
            music = 0
            video = 1
        End Enum

        Public Enum MediaType
            album = 0
            artist = 1
            movie = 2
            musicvideo = 3
            person = 4
            tvshow = 5
            tvepisode = 6
        End Enum

        Public Enum MissingSettingType
            [boolean]
            variable
        End Enum

        Public Enum ScraperContent
            albums = 0
            movies = 1
            musicvideos = 1
            tvshows = 2
        End Enum

        Public Enum ScraperFunctionType
            NfoUrl = 0
            CreateSearchUrl = 1
            GetSearchResults = 2
            GetDetails = 3
            GetList = 4
            CustomFunction = 5
        End Enum

#End Region 'Enumerations

    End Namespace
End Namespace
