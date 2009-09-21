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


Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class StringManip
    Public Shared Function ComputeLevenshtein(ByVal s As String, ByVal t As String) As Integer
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim d As Integer(,) = New Integer(n, m) {}

        If n = 0 Then
            Return (Master.eSettings.LevTolerance + 1)
        End If

        If m = 0 Then
            Return n
        End If

        Dim i As Integer = 0
        While i <= n
            d(i, 0) = System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While

        Dim j As Integer = 0
        While j <= m
            d(0, j) = System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)
        End While

        Dim cost As Integer = 0
        For k As Integer = 1 To n
            For l As Integer = 1 To m
                cost = If((t(l - 1) = s(k - 1)), 0, 1)

                d(k, l) = Math.Min(Math.Min(d(k - 1, l) + 1, d(k, l - 1) + 1), d(k - 1, l - 1) + cost)
            Next
        Next
        Return d(n, m) - 1
    End Function

    Public Shared Function FilterTokens(ByVal sTitle As String) As String
        Dim newTitle As String = sTitle
        If Master.eSettings.SortTokens.Count > 0 Then
            For Each sToken As String In Master.eSettings.SortTokens
                If Regex.IsMatch(sTitle, String.Concat("^", sToken), RegexOptions.IgnoreCase) Then
                    newTitle = String.Format("{0}, {1}", Regex.Replace(sTitle, String.Concat("^", sToken), String.Empty, RegexOptions.IgnoreCase).Trim, Regex.Match(sTitle, String.Concat("^", Regex.Replace(sToken, "\[(.*?)\]", String.Empty)), RegexOptions.IgnoreCase)).Trim
                    Exit For
                End If
            Next
        End If
        Return newTitle.Trim
    End Function

    Public Shared Function USACertToMPAA(ByVal sCert As String) As String
        Select Case sCert.ToLower
            Case "usa:g"
                Return "Rated G"
            Case "usa:pg"
                Return "Rated PG"
            Case "usa:pg-13"
                Return "Rated PG-13"
            Case "usa:r"
                Return "Rated R"
            Case "usa:nc-17"
                Return "Rated NC-17"
        End Select
        Return String.Empty
    End Function

    Public Shared Function CleanURL(ByVal sURL As String) As String
        If sURL.ToLower.Contains("themoviedb.org") Then
            Dim tURL As String = sURL.Replace("http://images.themoviedb.org/posters/", String.Empty)
            tURL = tURL.Replace("http://images.themoviedb.org/backdrops/", String.Empty)
            '$$ to sort first
            sURL = String.Concat("$$[themoviedb.org]", tURL)
        Else
            sURL = TruncateURL(sURL, 40, True)
        End If
        Return sURL.Replace(":", "$c$").Replace("/", "$s$")
    End Function

    Public Shared Function TruncateURL(ByVal sString As String, ByVal MaxLength As Integer, Optional ByVal EndOnly As Boolean = False) As String

        '//
        ' Shorten a URL to fit on the GUI
        '\\

        Try
            Dim sEnd As String = String.Empty
            If EndOnly Then
                Return Strings.Right(sString, MaxLength)
            Else
                sEnd = Strings.Right(sString, sString.Length - sString.LastIndexOf("/"))
                If ((MaxLength - sEnd.Length) - 3) > 0 Then
                    Return String.Format("{0}...{1}", Strings.Left(sString, (MaxLength - sEnd.Length) - 3), sEnd)
                Else
                    If sEnd.Length >= MaxLength Then
                        Return String.Format("...{0}", Strings.Right(sEnd, MaxLength - 3))
                    Else
                        Return sEnd
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return String.Empty
    End Function

    Public Shared Function Decode(ByVal encText As String) As String

        Try
            Dim dByte() As Byte
            dByte = System.Convert.FromBase64String(encText)
            Dim decText As String
            decText = System.Text.Encoding.ASCII.GetString(dByte)
            Return decText
        Catch
        End Try
        Return String.Empty
    End Function

    Public Shared Function Encode(ByVal decText As String) As String

        Dim eByte() As Byte
        ReDim eByte(decText.Length)
        eByte = System.Text.Encoding.ASCII.GetBytes(decText)
        Dim encText As String
        encText = System.Convert.ToBase64String(eByte)
        Return encText

    End Function

    Public Shared Function NumericOnly(ByVal KeyChar As Char, Optional ByVal isIP As Boolean = False) As Boolean
        If Char.IsNumber(KeyChar) OrElse Char.IsControl(KeyChar) OrElse Char.IsWhiteSpace(KeyChar) OrElse (isIP AndAlso Asc(KeyChar) = 46) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function AlphaNumericOnly(ByVal KeyChar As Char, Optional ByVal AllowSpecial As Boolean = False) As Boolean
        Dim test As Integer = Asc(KeyChar)
        If Char.IsLetterOrDigit(KeyChar) OrElse (AllowSpecial AndAlso (Char.IsControl(KeyChar) OrElse _
        Char.IsWhiteSpace(KeyChar) OrElse Asc(KeyChar) = 45 OrElse Asc(KeyChar) = 46 OrElse Asc(KeyChar) = 58)) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function FilterName(ByVal movieName As String, Optional ByVal doExtras As Boolean = True) As String

        '//
        ' Clean all the crap out of the name
        '\\
        Try

            If String.IsNullOrEmpty(movieName) Then Return String.Empty

            Dim strSplit() As String
            Try

                'run through each of the custom filters
                If Master.eSettings.FilterCustom.Count > 0 Then
                    For Each Str As String In Master.eSettings.FilterCustom

                        If Strings.InStr(Str, "[->]") > 0 Then
                            strSplit = Strings.Split(Str, "[->]")
                            movieName = Strings.Replace(movieName, Regex.Match(movieName, strSplit.First).ToString, strSplit.Last)
                        Else
                            movieName = Strings.Replace(movieName, Regex.Match(movieName, Str).ToString, String.Empty)
                        End If
                    Next
                End If

                'Convert String To Proper Case
                If Master.eSettings.ProperCase AndAlso doExtras Then
                    movieName = ProperCase(movieName)
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            If doExtras Then
                Return FilterTokens(CleanStackingMarkers(movieName.Trim))
            Else
                Return RemovePunctuation(CleanStackingMarkers(movieName.Trim))
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            ' Some error handling so EMM dont break on populate folderdata
            Return movieName.Trim
        End Try
    End Function

    Public Shared Function ProperCase(ByVal sString As String) As String
        If String.IsNullOrEmpty(sString) Then Return String.Empty
        Dim sReturn As String = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sString)
        Dim toUpper As String = "\b(hd|cd|dvd|bc|b\.c\.|ad|a\.d\.|sw|nw|se|sw|ii|iii|iv|vi|vii|viii|ix|x)\b"

        Dim mcUp As MatchCollection = Regex.Matches(sReturn, toUpper, RegexOptions.IgnoreCase)
        For Each M As Match In mcUp
            sReturn = sReturn.Replace(M.Value, Strings.StrConv(M.Value, VbStrConv.Uppercase))
        Next

        Return sReturn
    End Function

    Public Shared Function IsStacked(ByVal sName As String, Optional ByVal VTS As Boolean = False) As Boolean
        If String.IsNullOrEmpty(sName) Then Return False
        Dim bReturn As Boolean = False
        If VTS Then
            bReturn = Regex.IsMatch(sName, "[ _.-]+(cd|dvd|part|dis[ck])[ _.-]*([0-9a-d]+)", RegexOptions.IgnoreCase) OrElse Regex.IsMatch(sName, "^vts_[0-9]+_[0-9]+", RegexOptions.IgnoreCase)
        Else
            bReturn = Regex.IsMatch(sName, "[ _.-]+(cd|dvd|part|dis[ck])[ _.-]*([0-9a-d]+)", RegexOptions.IgnoreCase)
        End If
        Return bReturn
    End Function

    Public Shared Function CleanStackingMarkers(ByVal sPath As String, Optional ByVal Asterisk As Boolean = False) As String
        If String.IsNullOrEmpty(sPath) Then Return String.Empty
        Dim sReturn As String = Regex.Replace(sPath, "[ _.-]+(cd|dvd|part|dis[ck])[ _.-]*([0-9a-d]+)", If(Asterisk, "*", " "), RegexOptions.IgnoreCase).Trim
        Return Regex.Replace(sReturn, "\s\s(\s+)?", " ").Trim
    End Function

    Public Shared Function RemovePunctuation(ByVal sString As String) As String
        If String.IsNullOrEmpty(sString) Then Return String.Empty
        Dim sReturn As String = Regex.Replace(sString, "\W", " ")
        Return Regex.Replace(sReturn.ToLower, "\s\s(\s+)?", " ").Trim
    End Function

    Public Shared Function FilterYear(ByVal sString As String) As String
        Return Regex.Replace(sString, "([ _.-]\(?\d{4}\))?", String.Empty).Trim
    End Function
End Class
