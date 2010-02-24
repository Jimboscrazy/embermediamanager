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
Imports System.Xml.Linq
Imports System.IO.IsolatedStorage


Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class Localization
    Private Shared _ISOLanguages As New List(Of _ISOLanguage)
    ' ************************************************************************************************
    ' This are functions for country/Language codes under ISO639 Alpha-2 (ie: Used by DVD/GoogleAPI)
    ' TODO: Change APIDVD so it use this functions and remove from APIDVD the language Code)
    ' TODO: Move language code2 Setup to XML file
    Shared Function ISOGetLangByCode2(ByVal code As String) As String
        Return (From x As _ISOLanguage In _ISOLanguages Where (x.Alpha2Code = code))(0).Language
    End Function
    Shared Function ISOGetLangByCode3(ByVal code As String) As String
        Return (From x As _ISOLanguage In _ISOLanguages Where (x.Alpha3Code = code))(0).Language
    End Function
    Public Shared Function ISOLangGetCode2ByLang(ByVal lang As String) As String
        Return (From x As _ISOLanguage In _ISOLanguages Where (x.Language = lang))(0).Alpha2Code
    End Function
    Public Shared Function ISOLangGetCode3ByLang(ByVal lang As String) As String
        Return (From x As _ISOLanguage In _ISOLanguages Where (x.Language = lang))(0).Alpha3Code
    End Function
    Public Shared Function ISOLangGetLanguagesList() As ArrayList
        Dim r As New ArrayList
        For Each x As _ISOLanguage In _ISOLanguages
            r.Add(x.Language)
        Next
        Return r
        'Return New ArrayList( _ISOLanguages)
    End Function
    Structure _ISOLanguage
        Public Language As String
        Public Alpha2Code As String
        Public Alpha3Code As String
    End Structure
    ' ************************************************************************************************
    Structure Locs
        Dim AssenblyName As String
        Dim htStrings As Hashtable
        Dim FileName As String
    End Structure

    Private Shared htStrings As New Hashtable
    Private Shared htArrayStrings As New List(Of Locs)
    Private _all As String
    Private _none As String
    Private _disabled As String

    Public Property All() As String
        Get
            Return _all
        End Get
        Set(ByVal value As String)
            _all = value
        End Set
    End Property

    Public Property None() As String
        Get
            Return _none
        End Get
        Set(ByVal value As String)
            _none = value
        End Set
    End Property

    Public Property Disabled() As String
        Get
            Return _disabled
        End Get
        Set(ByVal value As String)
            _disabled = value
        End Set
    End Property

    Public Sub Clear()
        _all = "All"
        _none = "[none]"
        _disabled = "[Disabled]"
    End Sub

    Public Sub LoadLanguage(ByVal Language As String)
        Dim _old_all As String = _all
        Me.Clear()
        Try
            If Not String.IsNullOrEmpty(Language) Then
                Dim Assembly As String = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
                Dim lPath As String
                htStrings = New Hashtable
                htStrings.Clear()
                If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
                    Assembly = "*EmberCORE"
                    lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                Else
                    lPath = String.Concat(Functions.AppPath, "Modules", Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, Assembly, ".", Language, ".xml")
                    If Not File.Exists(lPath) Then 'Fallback, maybe not good idea, but needed for now
                        lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                    End If
                End If
                If File.Exists(lPath) Then
                    Dim LangXML As XDocument = XDocument.Load(lPath)
                    Dim xLanguage = From xLang In LangXML...<strings>...<string> Select xLang.@id, xLang.Value
                    If xLanguage.Count > 0 Then
                        For i As Integer = 0 To xLanguage.Count - 1
                            htStrings.Add(Convert.ToInt32(xLanguage(i).id), xLanguage(i).Value)
                        Next
                        Dim _loc As New Locs With {.AssenblyName = Assembly, .htStrings = htStrings, .FileName = lPath}
                        htArrayStrings.Add(_loc)
                        _all = String.Format("[{0}]", GetString(569, Master.eLang.All))
                        _none = GetString(570, Master.eLang.None)
                        _disabled = GetString(571, Master.eLang.Disabled)
                    Else
                        Dim _loc As New Locs With {.AssenblyName = Assembly, .htStrings = htStrings, .FileName = lPath}
                        htArrayStrings.Add(_loc)
                    End If
                Else
                    MsgBox(String.Concat(String.Format("Cannot find {0}.xml.", Language), vbNewLine, vbNewLine, "Expected path:", vbNewLine, lPath), MsgBoxStyle.Critical, "File Not Found")
                End If
            End If

            ' Need to change Globaly Langs_all
            Master.eSettings.GenreFilter = Master.eSettings.GenreFilter.Replace(_old_all, _all)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Public Function GetString(ByVal ID As Integer, ByVal strDefault As String) As String
        Dim Assembly As String = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetCallingAssembly().Location)
        If Assembly = "Ember Media Manager" OrElse Assembly = "EmberAPI" Then
            Assembly = "*EmberCORE"
        End If
        htStrings = htArrayStrings.Where(Function(x) x.AssenblyName = Assembly)(0).htStrings
        If htStrings Is Nothing Then Return strDefault
        If htStrings.ContainsKey(ID) Then
            Return htStrings.Item(ID).ToString
        Else
            '*****************************************************************************************
            ' this will add strings not found *** Dev propose only, should not go to release version
            Try

                Dim lPath As String
                Dim Language As String = Master.eSettings.Language
                If Assembly = "*EmberCORE" Then
                    lPath = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, Language, ".xml")
                Else
                    lPath = String.Concat(Functions.AppPath, "Modules", Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, Assembly, ".", Language, ".xml")
                End If
                'AddNotExist(lPath, ID.ToString, strDefault)
            Catch ex As Exception
            End Try
            '*****************************************************************************************
            Return strDefault
        End If
    End Function
    '*****************************************************************************************
    ' this will add strings not found *** Dev propose only, should not go to release version
    Sub AddNotExist(ByVal lpath As String, ByVal id As String, ByVal value As String)
        Dim xdoc As New XmlDocument()
        xdoc.Load(lpath)
        Dim elem As XmlElement = xdoc.CreateElement("string")
        Dim attr As XmlNode = xdoc.CreateNode(XmlNodeType.Attribute, "id", "id", "")
        attr.Value = id
        elem.Attributes.SetNamedItem(attr)
        elem.InnerText = value
        xdoc.DocumentElement.AppendChild(elem)
        xdoc.Save(lpath)
    End Sub
    '*****************************************************************************************
    Sub addcode2(ByVal x As _ISOLanguage)
        Dim z As _ISOLanguage = (From y As _ISOLanguage In _ISOLanguages Where (y.Language = y.Language))(0)
        If Not String.IsNullOrEmpty(z.Language) Then
            z.Alpha2Code = x.Alpha2Code
        Else
            _ISOLanguages.Add(z)
        End If
    End Sub
    Public Sub New()
        Me.Clear()
        Dim lPath As String = String.Concat(Functions.AppPath, "Langs", Path.DirectorySeparatorChar, "Languages.xml")
        If File.Exists(lPath) Then
            Dim LanguageXML As XDocument = XDocument.Load(lPath)
            For Each e In From xGen In LanguageXML...<Language> Select xGen
                Dim c As String = e.<Code>.Value.ToString
                _ISOLanguages.Add(New _ISOLanguage With {.Alpha3Code = e.<Code>.Value.ToString, .Language = e.<Name>.Value.ToString})
            Next
        Else
            MsgBox(String.Concat("Cannot find Language.xml.", vbNewLine, vbNewLine, "Expected path:", vbNewLine, lPath), MsgBoxStyle.Critical, "File Not Found")
        End If


        ' SEE NOTES AT TOP
        addcode2(New _ISOLanguage With {.Alpha2Code = "aa", .Language = "Afar"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ab", .Language = "Abkhazian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "af", .Language = "Afrikaans"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ak", .Language = "Akan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "am", .Language = "Amharic"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ar", .Language = "Arabic"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "an", .Language = "Aragonese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "as", .Language = "Assamese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "av", .Language = "Avaric"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ae", .Language = "Avestan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ay", .Language = "Aymara"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "az", .Language = "Azerbaijani"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ba", .Language = "Bashkir"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bm", .Language = "Bambara"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "be", .Language = "Belarusian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bn", .Language = "Bengali"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bh", .Language = "Bihari"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bi", .Language = "Bislama"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bo", .Language = "Tibetan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bs", .Language = "Bosnian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "br", .Language = "Breton"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "bg", .Language = "Bulgarian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ca", .Language = "Catalan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ch", .Language = "Chamorro"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ce", .Language = "Chechen"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "cu", .Language = "Slavic"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "cv", .Language = "Chuvash"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kw", .Language = "Cornish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "co", .Language = "Corsican"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "cr", .Language = "Cree"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "cy", .Language = "Welsh"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "cs", .Language = "Czech"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "da", .Language = "Danish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "de", .Language = "German"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "dv", .Language = "Divehi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "dz", .Language = "Dzongkha"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "en", .Language = "English"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "eo", .Language = "Esperanto"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "et", .Language = "Estonian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "eu", .Language = "Basque"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ee", .Language = "Ewe"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fo", .Language = "Faroese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fa", .Language = "Persian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fj", .Language = "Fijian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fi", .Language = "Finnish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fr", .Language = "French"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "fy", .Language = "Western Frisian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ff", .Language = "Fulah"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "gd", .Language = "Gaelic"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ga", .Language = "Irish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "gl", .Language = "Galician"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "gv", .Language = "Manx"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "el", .Language = "Greek"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "gn", .Language = "Guarani"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "gu", .Language = "Gujarati"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ht", .Language = "Haitian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ha", .Language = "Hausa"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "he", .Language = "Hebrew"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "hz", .Language = "Herero"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "hi", .Language = "Hindi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ho", .Language = "Hiri Motu"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "hr", .Language = "Croatian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "hu", .Language = "Hungarian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "hy", .Language = "Armenian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ig", .Language = "Igbo"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "io", .Language = "Ido"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ii", .Language = "Sichuan Yi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "iu", .Language = "Inuktitut"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ie", .Language = "Occidental"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ia", .Language = "Interlingua"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "id", .Language = "Indonesian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ik", .Language = "Inupiaq"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "is", .Language = "Icelandic"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "it", .Language = "Italian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "jv", .Language = "Javanese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ja", .Language = "Japanese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kl", .Language = "Kalaallisut"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kn", .Language = "Kannada"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ks", .Language = "Kashmiri"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ka", .Language = "Georgian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kr", .Language = "Kanuri"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kk", .Language = "Kazakh"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "km", .Language = "Central Khmer"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ki", .Language = "Kikuyu"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "rw", .Language = "Kinyarwanda"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ky", .Language = "Kirghiz"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kv", .Language = "Komi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kg", .Language = "Kongo"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ko", .Language = "Korean"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "kj", .Language = "Kuanyama"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ku", .Language = "Kurdish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lo", .Language = "Lao"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "la", .Language = "Latin"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lv", .Language = "Latvian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "li", .Language = "Limburgan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ln", .Language = "Lingala"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lt", .Language = "Lithuanian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lb", .Language = "Luxembourgish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lu", .Language = "Luba-Katanga"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "lg", .Language = "Ganda"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mh", .Language = "Marshallese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ml", .Language = "Malayalam"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mr", .Language = "Marathi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mk", .Language = "Macedonian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mg", .Language = "Malagasy"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mt", .Language = "Maltese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mn", .Language = "Mongolian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "mi", .Language = "Maori"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ms", .Language = "Malay"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "my", .Language = "Burmese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "na", .Language = "Nauru"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nv", .Language = "Navajo"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nr", .Language = "Ndebele"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nd", .Language = "Ndebele"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ng", .Language = "Ndonga"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ne", .Language = "Nepali"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nl", .Language = "Dutch"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nn", .Language = "Norwegian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "nb", .Language = "Norwegian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "no", .Language = "Norwegian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ny", .Language = "Chichewa"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "oc", .Language = "Occitan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "oj", .Language = "Ojibwa"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "or", .Language = "Oriya"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "om", .Language = "Oromo"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "os", .Language = "Ossetian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "pa", .Language = "Panjabi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "pi", .Language = "Pali"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "pl", .Language = "Polish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "pt", .Language = "Portuguese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ps", .Language = "Pushto"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "qu", .Language = "Quechua"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "rm", .Language = "Romansh"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ro", .Language = "Romanian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "rn", .Language = "Rundi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ru", .Language = "Russian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sg", .Language = "Sango"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sa", .Language = "Sanskrit"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "si", .Language = "Sinhala"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sk", .Language = "Slovak"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sl", .Language = "Slovenian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "se", .Language = "Northern Sami"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sm", .Language = "Samoan"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sn", .Language = "Shona"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sd", .Language = "Sindhi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "so", .Language = "Somali"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "st", .Language = "Sotho"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "es", .Language = "Spanish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sq", .Language = "Albanian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sc", .Language = "Sardinian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sr", .Language = "Serbian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ss", .Language = "Swati"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "su", .Language = "Sundanese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sw", .Language = "Swahili"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "sv", .Language = "Swedish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ty", .Language = "Tahitian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ta", .Language = "Tamil"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tt", .Language = "Tatar"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "te", .Language = "Telugu"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tg", .Language = "Tajik"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tl", .Language = "Tagalog"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "th", .Language = "Thai"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ti", .Language = "Tigrinya"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "to", .Language = "Tonga"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tn", .Language = "Tswana"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ts", .Language = "Tsonga"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tk", .Language = "Turkmen"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tr", .Language = "Turkish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "tw", .Language = "Twi"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ug", .Language = "Uighur"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "uk", .Language = "Ukrainian"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ur", .Language = "Urdu"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "uz", .Language = "Uzbek"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "ve", .Language = "Venda"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "vi", .Language = "Vietnamese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "vo", .Language = "Volapük"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "wa", .Language = "Walloon"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "wo", .Language = "Wolof"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "xh", .Language = "Xhosa"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "yi", .Language = "Yiddish"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "yo", .Language = "Yoruba"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "za", .Language = "Zhuang"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "zh", .Language = "Chinese"})
        addcode2(New _ISOLanguage With {.Alpha2Code = "zu", .Language = "Zulu"})
        ' SEE NOTES AT TOP
    End Sub
End Class
