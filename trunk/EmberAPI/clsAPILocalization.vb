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
    Private Shared _ISOLanguages As New Hashtable
    ' ************************************************************************************************
    ' This are functions for country/Language codes under ISO639 Alpha-2 (ie: Used by DVD/GoogleAPI)
    ' TODO: Change APIDVD so it use this functions and remove from APIDVD the language Code)
    ' TODO: Move APIXML Languages to here also, as they are ISO693 Alpha-3
    ' TODO: Move Dictionary Setup to XML and make it load as in APIXML
    Shared Function ISOLangGetLangByCode(ByVal code As String) As String
        Return _ISOLanguages.Item(code).ToString()
    End Function
    Public Shared Function ISOLangGetCodeByLang(ByVal lang As String) As String
        For Each entrie As DictionaryEntry In _ISOLanguages
            If entrie.Value.ToString = lang Then Return entrie.Key.ToString
        Next
        Return String.Empty
    End Function
    Public Shared Function ISOLangGetLanguages() As ArrayList
        Return New ArrayList(_ISOLanguages.Values)
    End Function
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
    Public Sub New()
        Me.Clear()
        ' SEE NOTES AT TOP
        _ISOLanguages.Add("aa", "Afar")
        _ISOLanguages.Add("ab", "Abkhazian")
        _ISOLanguages.Add("af", "Afrikaans")
        _ISOLanguages.Add("ak", "Akan")
        _ISOLanguages.Add("am", "Amharic")
        _ISOLanguages.Add("ar", "Arabic")
        _ISOLanguages.Add("an", "Aragonese")
        _ISOLanguages.Add("as", "Assamese")
        _ISOLanguages.Add("av", "Avaric")
        _ISOLanguages.Add("ae", "Avestan")
        _ISOLanguages.Add("ay", "Aymara")
        _ISOLanguages.Add("az", "Azerbaijani")
        _ISOLanguages.Add("ba", "Bashkir")
        _ISOLanguages.Add("bm", "Bambara")
        _ISOLanguages.Add("be", "Belarusian")
        _ISOLanguages.Add("bn", "Bengali")
        _ISOLanguages.Add("bh", "Bihari")
        _ISOLanguages.Add("bi", "Bislama")
        _ISOLanguages.Add("bo", "Tibetan")
        _ISOLanguages.Add("bs", "Bosnian")
        _ISOLanguages.Add("br", "Breton")
        _ISOLanguages.Add("bg", "Bulgarian")
        _ISOLanguages.Add("ca", "Catalan")
        _ISOLanguages.Add("ch", "Chamorro")
        _ISOLanguages.Add("ce", "Chechen")
        _ISOLanguages.Add("cu", "Slavic")
        _ISOLanguages.Add("cv", "Chuvash")
        _ISOLanguages.Add("kw", "Cornish")
        _ISOLanguages.Add("co", "Corsican")
        _ISOLanguages.Add("cr", "Cree")
        _ISOLanguages.Add("cy", "Welsh")
        _ISOLanguages.Add("cs", "Czech")
        _ISOLanguages.Add("da", "Danish")
        _ISOLanguages.Add("de", "German")
        _ISOLanguages.Add("dv", "Divehi")
        _ISOLanguages.Add("dz", "Dzongkha")
        _ISOLanguages.Add("en", "English")
        _ISOLanguages.Add("eo", "Esperanto")
        _ISOLanguages.Add("et", "Estonian")
        _ISOLanguages.Add("eu", "Basque")
        _ISOLanguages.Add("ee", "Ewe")
        _ISOLanguages.Add("fo", "Faroese")
        _ISOLanguages.Add("fa", "Persian")
        _ISOLanguages.Add("fj", "Fijian")
        _ISOLanguages.Add("fi", "Finnish")
        _ISOLanguages.Add("fr", "French")
        _ISOLanguages.Add("fy", "Western Frisian")
        _ISOLanguages.Add("ff", "Fulah")
        _ISOLanguages.Add("gd", "Gaelic")
        _ISOLanguages.Add("ga", "Irish")
        _ISOLanguages.Add("gl", "Galician")
        _ISOLanguages.Add("gv", "Manx")
        _ISOLanguages.Add("el", "Greek")
        _ISOLanguages.Add("gn", "Guarani")
        _ISOLanguages.Add("gu", "Gujarati")
        _ISOLanguages.Add("ht", "Haitian")
        _ISOLanguages.Add("ha", "Hausa")
        _ISOLanguages.Add("he", "Hebrew")
        _ISOLanguages.Add("hz", "Herero")
        _ISOLanguages.Add("hi", "Hindi")
        _ISOLanguages.Add("ho", "Hiri Motu")
        _ISOLanguages.Add("hr", "Croatian")
        _ISOLanguages.Add("hu", "Hungarian")
        _ISOLanguages.Add("hy", "Armenian")
        _ISOLanguages.Add("ig", "Igbo")
        _ISOLanguages.Add("io", "Ido")
        _ISOLanguages.Add("ii", "Sichuan Yi")
        _ISOLanguages.Add("iu", "Inuktitut")
        _ISOLanguages.Add("ie", "Occidental")
        _ISOLanguages.Add("ia", "Interlingua")
        _ISOLanguages.Add("id", "Indonesian")
        _ISOLanguages.Add("ik", "Inupiaq")
        _ISOLanguages.Add("is", "Icelandic")
        _ISOLanguages.Add("it", "Italian")
        _ISOLanguages.Add("jv", "Javanese")
        _ISOLanguages.Add("ja", "Japanese")
        _ISOLanguages.Add("kl", "Kalaallisut")
        _ISOLanguages.Add("kn", "Kannada")
        _ISOLanguages.Add("ks", "Kashmiri")
        _ISOLanguages.Add("ka", "Georgian")
        _ISOLanguages.Add("kr", "Kanuri")
        _ISOLanguages.Add("kk", "Kazakh")
        _ISOLanguages.Add("km", "Central Khmer")
        _ISOLanguages.Add("ki", "Kikuyu")
        _ISOLanguages.Add("rw", "Kinyarwanda")
        _ISOLanguages.Add("ky", "Kirghiz")
        _ISOLanguages.Add("kv", "Komi")
        _ISOLanguages.Add("kg", "Kongo")
        _ISOLanguages.Add("ko", "Korean")
        _ISOLanguages.Add("kj", "Kuanyama")
        _ISOLanguages.Add("ku", "Kurdish")
        _ISOLanguages.Add("lo", "Lao")
        _ISOLanguages.Add("la", "Latin")
        _ISOLanguages.Add("lv", "Latvian")
        _ISOLanguages.Add("li", "Limburgan")
        _ISOLanguages.Add("ln", "Lingala")
        _ISOLanguages.Add("lt", "Lithuanian")
        _ISOLanguages.Add("lb", "Luxembourgish")
        _ISOLanguages.Add("lu", "Luba-Katanga")
        _ISOLanguages.Add("lg", "Ganda")
        _ISOLanguages.Add("mh", "Marshallese")
        _ISOLanguages.Add("ml", "Malayalam")
        _ISOLanguages.Add("mr", "Marathi")
        _ISOLanguages.Add("mk", "Macedonian")
        _ISOLanguages.Add("mg", "Malagasy")
        _ISOLanguages.Add("mt", "Maltese")
        _ISOLanguages.Add("mn", "Mongolian")
        _ISOLanguages.Add("mi", "Maori")
        _ISOLanguages.Add("ms", "Malay")
        _ISOLanguages.Add("my", "Burmese")
        _ISOLanguages.Add("na", "Nauru")
        _ISOLanguages.Add("nv", "Navajo")
        _ISOLanguages.Add("nr", "Ndebele")
        _ISOLanguages.Add("nd", "Ndebele")
        _ISOLanguages.Add("ng", "Ndonga")
        _ISOLanguages.Add("ne", "Nepali")
        _ISOLanguages.Add("nl", "Dutch")
        _ISOLanguages.Add("nn", "Norwegian")
        _ISOLanguages.Add("nb", "Norwegian")
        _ISOLanguages.Add("no", "Norwegian")
        _ISOLanguages.Add("ny", "Chichewa")
        _ISOLanguages.Add("oc", "Occitan")
        _ISOLanguages.Add("oj", "Ojibwa")
        _ISOLanguages.Add("or", "Oriya")
        _ISOLanguages.Add("om", "Oromo")
        _ISOLanguages.Add("os", "Ossetian")
        _ISOLanguages.Add("pa", "Panjabi")
        _ISOLanguages.Add("pi", "Pali")
        _ISOLanguages.Add("pl", "Polish")
        _ISOLanguages.Add("pt", "Portuguese")
        _ISOLanguages.Add("ps", "Pushto")
        _ISOLanguages.Add("qu", "Quechua")
        _ISOLanguages.Add("rm", "Romansh")
        _ISOLanguages.Add("ro", "Romanian")
        _ISOLanguages.Add("rn", "Rundi")
        _ISOLanguages.Add("ru", "Russian")
        _ISOLanguages.Add("sg", "Sango")
        _ISOLanguages.Add("sa", "Sanskrit")
        _ISOLanguages.Add("si", "Sinhala")
        _ISOLanguages.Add("sk", "Slovak")
        _ISOLanguages.Add("sl", "Slovenian")
        _ISOLanguages.Add("se", "Northern Sami")
        _ISOLanguages.Add("sm", "Samoan")
        _ISOLanguages.Add("sn", "Shona")
        _ISOLanguages.Add("sd", "Sindhi")
        _ISOLanguages.Add("so", "Somali")
        _ISOLanguages.Add("st", "Sotho")
        _ISOLanguages.Add("es", "Spanish")
        _ISOLanguages.Add("sq", "Albanian")
        _ISOLanguages.Add("sc", "Sardinian")
        _ISOLanguages.Add("sr", "Serbian")
        _ISOLanguages.Add("ss", "Swati")
        _ISOLanguages.Add("su", "Sundanese")
        _ISOLanguages.Add("sw", "Swahili")
        _ISOLanguages.Add("sv", "Swedish")
        _ISOLanguages.Add("ty", "Tahitian")
        _ISOLanguages.Add("ta", "Tamil")
        _ISOLanguages.Add("tt", "Tatar")
        _ISOLanguages.Add("te", "Telugu")
        _ISOLanguages.Add("tg", "Tajik")
        _ISOLanguages.Add("tl", "Tagalog")
        _ISOLanguages.Add("th", "Thai")
        _ISOLanguages.Add("ti", "Tigrinya")
        _ISOLanguages.Add("to", "Tonga")
        _ISOLanguages.Add("tn", "Tswana")
        _ISOLanguages.Add("ts", "Tsonga")
        _ISOLanguages.Add("tk", "Turkmen")
        _ISOLanguages.Add("tr", "Turkish")
        _ISOLanguages.Add("tw", "Twi")
        _ISOLanguages.Add("ug", "Uighur")
        _ISOLanguages.Add("uk", "Ukrainian")
        _ISOLanguages.Add("ur", "Urdu")
        _ISOLanguages.Add("uz", "Uzbek")
        _ISOLanguages.Add("ve", "Venda")
        _ISOLanguages.Add("vi", "Vietnamese")
        _ISOLanguages.Add("vo", "Volapük")
        _ISOLanguages.Add("wa", "Walloon")
        _ISOLanguages.Add("wo", "Wolof")
        _ISOLanguages.Add("xh", "Xhosa")
        _ISOLanguages.Add("yi", "Yiddish")
        _ISOLanguages.Add("yo", "Yoruba")
        _ISOLanguages.Add("za", "Zhuang")
        _ISOLanguages.Add("zh", "Chinese")
        _ISOLanguages.Add("zu", "Zulu")
        ' SEE NOTES AT TOP
    End Sub
End Class
