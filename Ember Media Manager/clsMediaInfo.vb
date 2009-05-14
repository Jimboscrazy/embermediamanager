Option Explicit On

'' MediaInfoDLL - All info about media files, for DLL
'' Copyright (C) 2002-2006 Jerome Martinez, Zen@MediaArea.net
'' Portions (C) 2008-2009 MDI under GPL
'' This library is free software; you can redistribute it and/or
'' modify it under the terms of the GNU Lesser General Public
'' License as published by the Free Software Foundation; either
'' version 2.1 of the License, or (at your option) any later version.
''
'' This library is distributed in the hope that it will be useful,
'' but WITHOUT ANY WARRANTY; without even the implied warranty of
'' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
'' Lesser General Public License for more details.
''
'' You should have received a copy of the GNU Lesser General Public
'' License along with this library; if not, write to the Free Software
'' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
''
''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
''
'' Microsoft Visual Basic wrapper for MediaInfo Library
'' See MediaInfo.h for help
''
'' To make it working, you must put MediaInfo.Dll
'' in the executable folder
''
''+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

'MODIFIED FOR UMM

Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Xml.Serialization

Namespace MediaInfo



#Region "Enums"

    ' ########################################
    ' ################# ENUMS ################
    ' ########################################

    Public Enum StreamKind As UInteger
        General
        Visual
        Audio
        Text
        Chapters
        Image
        Menu
        Max
    End Enum

    Public Enum InfoKind As UInteger
        Name
        Text
        Measure
        Options
        NameText
        MeasureText
        Info
        HowTo
        Max
    End Enum

    Public Enum InfoOptions As UInteger
        ShowInInform
        Reserved
        ShowInSupported
        TypeOfValue
        Max
    End Enum
#End Region '*** Enums



#Region "MInfo Class"

    Public Class MInfo



#Region "MInfo - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private Declare Unicode Function MediaInfo_New Lib "Bin\MediaInfo.DLL" () As IntPtr
        Private Declare Unicode Sub MediaInfo_Delete Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr)
        Private Declare Unicode Function MediaInfo_Open Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal FileName As String) As UIntPtr
        Private Declare Unicode Sub MediaInfo_Close Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr)
        Private Declare Unicode Function MediaInfo_Get Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As UIntPtr, ByVal Parameter As String, ByVal KindOfInfo As UIntPtr, ByVal KindOfSearch As UIntPtr) As IntPtr
        Private Declare Unicode Function MediaInfo_Count_Get Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As IntPtr) As UIntPtr 'see MediaInfoDLL.h for enumeration values

        Private Handle As IntPtr

#End Region '*** MInfo - Declarations



#Region "MInfo - Routines/Functions"

        ' ########################################
        ' ###### GENERAL ROUTINES/FUNCTIONS ######
        ' ########################################

        Public Sub New()
            Handle = MediaInfo_New()
        End Sub

        Protected Overrides Sub Finalize()
            MediaInfo_Delete(Handle)
        End Sub

        Private Function Open(ByVal FileName As String) As Integer
            Return MediaInfo_Open(Handle, FileName)
        End Function

        Private Sub Close()
            MediaInfo_Close(Handle)
        End Sub

        Private Function Get_(ByVal StreamKind As StreamKind, ByVal StreamNumber As Integer, ByVal Parameter As String, Optional ByVal KindOfInfo As InfoKind = InfoKind.Text, Optional ByVal KindOfSearch As InfoKind = InfoKind.Name) As String
            Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, StreamKind, StreamNumber, Parameter, KindOfInfo, KindOfSearch))
        End Function

        Private Function Count_Get(ByVal StreamKind As StreamKind, Optional ByVal StreamNumber As UInteger = UInteger.MaxValue) As Integer
            If StreamNumber = UInteger.MaxValue Then
                Dim A As Long
                A = 0
                A = A - 1 'If you know how to have (IntPtr)(-1) easier, I am interested ;-)
                Return MediaInfo_Count_Get(Handle, StreamKind, A)
            Else
                Return MediaInfo_Count_Get(Handle, StreamKind, StreamNumber)
            End If
        End Function

        Public Sub GetMovieMIFromPath(ByRef fiInfo As MediaInfo.Fileinfo, ByVal sPath As String)

            Dim MI As New MInfo

            'open the file
            If Not File.Exists(sPath) Then
                Exit Sub
            End If

            MI.Open(sPath)

            'find the longest stream in the file
            'find the number of video streams in the video file
            Dim VideoStreams As Integer = MI.Count_Get(StreamKind.Visual)
            Dim mivideo As New MediaInfo.Video
            For v As Integer = 0 To VideoStreams - 1
                mivideo = New MediaInfo.Video
                'get video data
                miVideo.Width = MI.Get_(StreamKind.Visual, v, "Width")
                miVideo.Height = MI.Get_(StreamKind.Visual, v, "Height")
                'switch avs to h264
                Dim miFormat As String = MI.Get_(StreamKind.Visual, v, "Format")
                miVideo.Codec = If(miFormat.ToLower = "avc", "h264", miFormat)
                mivideo.FormatInfo = miFormat
                miVideo.Duration = MI.Get_(StreamKind.Visual, v, "Duration/String1")
                miVideo.Bitrate = MI.Get_(StreamKind.Visual, v, "BitRate/String")
                miVideo.BitrateMode = MI.Get_(StreamKind.Visual, v, "BitRate_Mode/String")
                miVideo.BitrateMax = MI.Get_(StreamKind.Visual, v, "BitRate_Maximum/String")
                miVideo.CodecID = MI.Get_(StreamKind.Visual, v, "CodecID")
                miVideo.CodecidInfo = MI.Get_(StreamKind.Visual, v, "CodecID/Info")
                miVideo.ScanType = MI.Get_(StreamKind.Visual, v, "ScanType")
                mivideo.AspectDisplayRatio = MI.Get_(StreamKind.Visual, v, "DisplayAspectRatio")

                With miVideo
                    If Not String.IsNullOrEmpty(.Bitrate) OrElse Not String.IsNullOrEmpty(.BitrateMax) OrElse Not String.IsNullOrEmpty(.BitrateMode) OrElse _
                            Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.CodecID) OrElse Not String.IsNullOrEmpty(.CodecidInfo) OrElse _
                            Not String.IsNullOrEmpty(.Duration) OrElse Not String.IsNullOrEmpty(.FormatInfo) OrElse Not String.IsNullOrEmpty(.Height) OrElse _
                            Not String.IsNullOrEmpty(.ScanType) OrElse Not String.IsNullOrEmpty(.Width) Then
                        fiInfo.StreamDetails.Video.Add(mivideo)
                    End If
                End With
            Next

            Dim AudioStreams As Integer = MI.Count_Get(StreamKind.Audio)
            Dim miAudio As New MediaInfo.Audio
            For a As Integer = 0 To AudioStreams - 1
                'get audio data
                miAudio = New MediaInfo.Audio
                miAudio.Codec = MI.Get_(StreamKind.Audio, a, "Format")
                miAudio.Channels = MI.Get_(StreamKind.Audio, a, "Channel(s)")
                miAudio.Bitrate = MI.Get_(StreamKind.Audio, a, "BitRate/String")
                miAudio.Language = GetLangCode(MI.Get_(StreamKind.Audio, a, "Language/String"))
                With miAudio
                    If Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.Channels) OrElse Not String.IsNullOrEmpty(.Bitrate) OrElse Not String.IsNullOrEmpty(.Language) Then
                        fiInfo.StreamDetails.Audio.Add(miAudio)
                    End If
                End With
            Next

            Dim SubtitleStreams As Integer = MI.Count_Get(StreamKind.Text)
            Dim miSubtitle As New MediaInfo.Subtitle
            For s As Integer = 0 To SubtitleStreams - 1
                'get subtitle data
                miSubtitle = New MediaInfo.Subtitle
                miSubtitle.Language = GetLangCode(MI.Get_(StreamKind.Text, s, "Language/String"))
                If Not String.IsNullOrEmpty(miSubtitle.Language) Then
                    fiInfo.StreamDetails.Subtitle.Add(miSubtitle)
                End If
            Next

            MI.Close()
            MI.Finalize()
            MI = Nothing
        End Sub

        Private Function GetLangCode(ByVal strLang As String) As String
            Select Case strLang.ToLower
                Case "english"
                    Return "eng"
                Case "german"
                    Return "deu"
                Case "afar"
                    Return "aar"
                Case "abkhazian"
                    Return "abk"
                Case "achinese"
                    Return "ace"
                Case "acoli"
                    Return "ach"
                Case "adangme"
                    Return "ada"
                Case "adyghe", "adygei"
                    Return "ady"
                Case "afro-asiatic (other)"
                    Return "afa"
                Case "afrihili"
                    Return "afh"
                Case "afrikaans"
                    Return "afr"
                Case "ainu"
                    Return "ain"
                Case "akan"
                    Return "aka"
                Case "akkadian"
                    Return "akk"
                Case "albanian"
                    Return "alb"
                Case "aleut"
                    Return "ale"
                Case "algonquian languages"
                    Return "alg"
                Case "southern altai"
                    Return "alt"
                Case "amharic"
                    Return "amh"
                Case "english"
                    Return "ang"
                Case "angika"
                    Return "anp"
                Case "apache languages"
                    Return "apa"
                Case "arabic"
                    Return "ara"
                Case "official aramaic (700-300 bce)", "imperial aramaic (700-300 bce)"
                    Return "arc"
                Case "aragonese"
                    Return "arg"
                Case "armenian"
                    Return "arm"
                Case "mapudungun", "mapuche"
                    Return "arn"
                Case "arapaho"
                    Return "arp"
                Case "artificial (other)"
                    Return "art"
                Case "arawak"
                    Return "arw"
                Case "assamese"
                    Return "asm"
                Case "asturian", "bable", "leonese", "asturleonese"
                    Return "ast"
                Case "athapascan languages"
                    Return "ath"
                Case "australian languages"
                    Return "aus"
                Case "avaric"
                    Return "ava"
                Case "avestan"
                    Return "ave"
                Case "awadhi"
                    Return "awa"
                Case "aymara"
                    Return "aym"
                Case "azerbaijani"
                    Return "aze"
                Case "banda languages"
                    Return "bad"
                Case "bamileke languages"
                    Return "bai"
                Case "bashkir"
                    Return "bak"
                Case "baluchi"
                    Return "bal"
                Case "bambara"
                    Return "bam"
                Case "balinese"
                    Return "ban"
                Case "basque"
                    Return "baq"
                Case "basa"
                    Return "bas"
                Case "baltic (other)"
                    Return "bat"
                Case "beja", "bedawiyet"
                    Return "bej"
                Case "belarusian"
                    Return "bel"
                Case "bemba"
                    Return "bem"
                Case "bengali"
                    Return "ben"
                Case "berber (other)"
                    Return "ber"
                Case "bhojpuri"
                    Return "bho"
                Case "bihari"
                    Return "bih"
                Case "bikol"
                    Return "bik"
                Case "bini", "edo"
                    Return "bin"
                Case "bislama"
                    Return "bis"
                Case "siksika"
                    Return "bla"
                Case "bantu (other)"
                    Return "bnt"
                Case "bosnian"
                    Return "bos"
                Case "braj"
                    Return "bra"
                Case "breton"
                    Return "bre"
                Case "batak languages"
                    Return "btk"
                Case "buriat"
                    Return "bua"
                Case "buginese"
                    Return "bug"
                Case "bulgarian"
                    Return "bul"
                Case "burmese"
                    Return "bur"
                Case "blin", "bilin"
                    Return "byn"
                Case "caddo"
                    Return "cad"
                Case "central american indian (other)"
                    Return "cai"
                Case "galibi carib"
                    Return "car"
                Case "catalan", "valencian"
                    Return "cat"
                Case "caucasian (other)"
                    Return "cau"
                Case "cebuano"
                    Return "ceb"
                Case "celtic (other)"
                    Return "cel"
                Case "chamorro"
                    Return "cha"
                Case "chibcha"
                    Return "chb"
                Case "chechen"
                    Return "che"
                Case "chagatai"
                    Return "chg"
                Case "chinese"
                    Return "chi"
                Case "chuukese"
                    Return "chk"
                Case "mari"
                    Return "chm"
                Case "chinook jargon"
                    Return "chn"
                Case "choctaw"
                    Return "cho"
                Case "chipewyan", "dene suline"
                    Return "chp"
                Case "cherokee"
                    Return "chr"
                Case "church slavic", "old slavonic", "church slavonic", "old bulgarian", "old church slavonic"
                    Return "chu"
                Case "chuvash"
                    Return "chv"
                Case "cheyenne"
                    Return "chy"
                Case "chamic languages"
                    Return "cmc"
                Case "coptic"
                    Return "cop"
                Case "cornish"
                    Return "cor"
                Case "corsican"
                    Return "cos"
                Case "creoles and pidgins"
                    Return "cpe"
                Case "creoles and pidgins"
                    Return "cpf"
                Case "creoles and pidgins"
                    Return "cpp"
                Case "cree"
                    Return "cre"
                Case "crimean tatar", "crimean turkish"
                    Return "crh"
                Case "creoles and pidgins (other)"
                    Return "crp"
                Case "kashubian"
                    Return "csb"
                Case "cushitic (other)"
                    Return "cus"
                Case "czech"
                    Return "cze"
                Case "dakota"
                    Return "dak"
                Case "danish"
                    Return "dan"
                Case "dargwa"
                    Return "dar"
                Case "land dayak languages"
                    Return "day"
                Case "delaware"
                    Return "del"
                Case "slave (athapascan)"
                    Return "den"
                Case "dogrib"
                    Return "dgr"
                Case "dinka"
                    Return "din"
                Case "divehi", "dhivehi", "maldivian"
                    Return "div"
                Case "dogri"
                    Return "doi"
                Case "dravidian (other)"
                    Return "dra"
                Case "lower sorbian"
                    Return "dsb"
                Case "duala"
                    Return "dua"
                Case "dutch"
                    Return "dum"
                Case "dutch", "flemish"
                    Return "dut"
                Case "dyula"
                    Return "dyu"
                Case "dzongkha"
                    Return "dzo"
                Case "efik"
                    Return "efi"
                Case "egyptian (ancient)"
                    Return "egy"
                Case "ekajuk"
                    Return "eka"
                Case "elamite"
                    Return "elx"
                Case "english"
                    Return "eng"
                Case "english"
                    Return "enm"
                Case "esperanto"
                    Return "epo"
                Case "estonian"
                    Return "est"
                Case "ewe"
                    Return "ewe"
                Case "ewondo"
                    Return "ewo"
                Case "fang"
                    Return "fan"
                Case "faroese"
                    Return "fao"
                Case "fanti"
                    Return "fat"
                Case "fijian"
                    Return "fij"
                Case "filipino", "pilipino"
                    Return "fil"
                Case "finnish"
                    Return "fin"
                Case "finno-ugrian (other)"
                    Return "fiu"
                Case "fon"
                    Return "fon"
                Case "french"
                    Return "fre"
                Case "french"
                    Return "frm"
                Case "french"
                    Return "fro"
                Case "northern frisian"
                    Return "frr"
                Case "eastern frisian"
                    Return "frs"
                Case "western frisian"
                    Return "fry"
                Case "fulah"
                    Return "ful"
                Case "friulian"
                    Return "fur"
                Case "ga"
                    Return "gaa"
                Case "gayo"
                    Return "gay"
                Case "gbaya"
                    Return "gba"
                Case "germanic (other)"
                    Return "gem"
                Case "georgian"
                    Return "geo"
                Case "german"
                    Return "ger"
                Case "geez"
                    Return "gez"
                Case "gilbertese"
                    Return "gil"
                Case "gaelic", "scottish gaelic"
                    Return "gla"
                Case "irish"
                    Return "gle"
                Case "galician"
                    Return "glg"
                Case "manx"
                    Return "glv"
                Case "german"
                    Return "gmh"
                Case "german"
                    Return "goh"
                Case "gondi"
                    Return "gon"
                Case "gorontalo"
                    Return "gor"
                Case "gothic"
                    Return "got"
                Case "grebo"
                    Return "grb"
                Case "greek"
                    Return "grc"
                Case "greek"
                    Return "gre"
                Case "guarani"
                    Return "grn"
                Case "swiss german", "alemannic", "alsatian"
                    Return "gsw"
                Case "gujarati"
                    Return "guj"
                Case "gwich'in"
                    Return "gwi"
                Case "haida"
                    Return "hai"
                Case "haitian", "haitian creole"
                    Return "hat"
                Case "hausa"
                    Return "hau"
                Case "hawaiian"
                    Return "haw"
                Case "hebrew"
                    Return "heb"
                Case "herero"
                    Return "her"
                Case "hiligaynon"
                    Return "hil"
                Case "himachali"
                    Return "him"
                Case "hindi"
                    Return "hin"
                Case "hittite"
                    Return "hit"
                Case "hmong"
                    Return "hmn"
                Case "hiri motu"
                    Return "hmo"
                Case "croatian"
                    Return "hrv"
                Case "upper sorbian"
                    Return "hsb"
                Case "hungarian"
                    Return "hun"
                Case "hupa"
                    Return "hup"
                Case "iban"
                    Return "iba"
                Case "igbo"
                    Return "ibo"
                Case "icelandic"
                    Return "ice"
                Case "ido"
                    Return "ido"
                Case "sichuan yi", "nuosu"
                    Return "iii"
                Case "ijo languages"
                    Return "ijo"
                Case "inuktitut"
                    Return "iku"
                Case "interlingue", "occidental"
                    Return "ile"
                Case "iloko"
                    Return "ilo"
                Case "interlingua (international auxiliary language association)"
                    Return "ina"
                Case "indic (other)"
                    Return "inc"
                Case "indonesian"
                    Return "ind"
                Case "indo-european (other)"
                    Return "ine"
                Case "ingush"
                    Return "inh"
                Case "inupiaq"
                    Return "ipk"
                Case "iranian (other)"
                    Return "ira"
                Case "iroquoian languages"
                    Return "iro"
                Case "italian"
                    Return "ita"
                Case "javanese"
                    Return "jav"
                Case "lojban"
                    Return "jbo"
                Case "japanese"
                    Return "jpn"
                Case "judeo-persian"
                    Return "jpr"
                Case "judeo-arabic"
                    Return "jrb"
                Case "kara-kalpak"
                    Return "kaa"
                Case "kabyle"
                    Return "kab"
                Case "kachin", "jingpho"
                    Return "kac"
                Case "kalaallisut", "greenlandic"
                    Return "kal"
                Case "kamba"
                    Return "kam"
                Case "kannada"
                    Return "kan"
                Case "karen languages"
                    Return "kar"
                Case "kashmiri"
                    Return "kas"
                Case "kanuri"
                    Return "kau"
                Case "kawi"
                    Return "kaw"
                Case "kazakh"
                    Return "kaz"
                Case "kabardian"
                    Return "kbd"
                Case "khasi"
                    Return "kha"
                Case "khoisan (other)"
                    Return "khi"
                Case "central khmer"
                    Return "khm"
                Case "khotanese", "sakan"
                    Return "kho"
                Case "kikuyu", "gikuyu"
                    Return "kik"
                Case "kinyarwanda"
                    Return "kin"
                Case "kirghiz", "kyrgyz"
                    Return "kir"
                Case "kimbundu"
                    Return "kmb"
                Case "konkani"
                    Return "kok"
                Case "komi"
                    Return "kom"
                Case "kongo"
                    Return "kon"
                Case "korean"
                    Return "kor"
                Case "kosraean"
                    Return "kos"
                Case "kpelle"
                    Return "kpe"
                Case "karachay-balkar"
                    Return "krc"
                Case "karelian"
                    Return "krl"
                Case "kru languages"
                    Return "kro"
                Case "kurukh"
                    Return "kru"
                Case "kuanyama", "kwanyama"
                    Return "kua"
                Case "kumyk"
                    Return "kum"
                Case "kurdish"
                    Return "kur"
                Case "kutenai"
                    Return "kut"
                Case "ladino"
                    Return "lad"
                Case "lahnda"
                    Return "lah"
                Case "lamba"
                    Return "lam"
                Case "lao"
                    Return "lao"
                Case "latin"
                    Return "lat"
                Case "latvian"
                    Return "lav"
                Case "lezghian"
                    Return "lez"
                Case "limburgan", "limburger", "limburgish"
                    Return "lim"
                Case "lingala"
                    Return "lin"
                Case "lithuanian"
                    Return "lit"
                Case "mongo"
                    Return "lol"
                Case "lozi"
                    Return "loz"
                Case "luxembourgish", "letzeburgesch"
                    Return "ltz"
                Case "luba-lulua"
                    Return "lua"
                Case "luba-katanga"
                    Return "lub"
                Case "ganda"
                    Return "lug"
                Case "luiseno"
                    Return "lui"
                Case "lunda"
                    Return "lun"
                Case "luo (kenya and tanzania)"
                    Return "luo"
                Case "lushai"
                    Return "lus"
                Case "macedonian"
                    Return "mac"
                Case "madurese"
                    Return "mad"
                Case "magahi"
                    Return "mag"
                Case "marshallese"
                    Return "mah"
                Case "maithili"
                    Return "mai"
                Case "makasar"
                    Return "mak"
                Case "malayalam"
                    Return "mal"
                Case "mandingo"
                    Return "man"
                Case "maori"
                    Return "mao"
                Case "austronesian (other)"
                    Return "map"
                Case "marathi"
                    Return "mar"
                Case "masai"
                    Return "mas"
                Case "malay"
                    Return "may"
                Case "moksha"
                    Return "mdf"
                Case "mandar"
                    Return "mdr"
                Case "mende"
                    Return "men"
                Case "irish"
                    Return "mga"
                Case "mi'kmaq", "micmac"
                    Return "mic"
                Case "minangkabau"
                    Return "min"
                Case "uncoded languages"
                    Return "mis"
                Case "mon-khmer (other)"
                    Return "mkh"
                Case "malagasy"
                    Return "mlg"
                Case "maltese"
                    Return "mlt"
                Case "manchu"
                    Return ("mnc")
                Case "manipuri"
                    Return "mni"
                Case "manobo languages"
                    Return "mno"
                Case "mohawk"
                    Return "moh"
                Case "mongolian"
                    Return "mon"
                Case "mossi"
                    Return "mos"
                Case "multiple languages"
                    Return "mul"
                Case "munda languages"
                    Return "mun"
                Case "creek"
                    Return "mus"
                Case "mirandese"
                    Return "mwl"
                Case "marwari"
                    Return "mwr"
                Case "mayan languages"
                    Return "myn"
                Case "erzya"
                    Return "myv"
                Case "nahuatl languages"
                    Return "nah"
                Case "north american indian"
                    Return "nai"
                Case "neapolitan"
                    Return "nap"
                Case "nauru"
                    Return "nau"
                Case "navajo", "navaho"
                    Return "nav"
                Case "ndebele"
                    Return "nbl"
                Case "ndebele"
                    Return "nde"
                Case "ndonga"
                    Return "ndo"
                Case "low german", "low saxon", "german"
                    Return "nds"
                Case "nepali"
                    Return "nep"
                Case "nepal bhasa", "newari"
                    Return "new"
                Case "nias"
                    Return "nia"
                Case "niger-kordofanian (other)"
                    Return "nic"
                Case "niuean"
                    Return "niu"
                Case "norwegian nynorsk", "nynorsk"
                    Return "nno"
                Case "bokmål"
                    Return "nob"
                Case "nogai"
                    Return "nog"
                Case "norse"
                    Return "non"
                Case "norwegian"
                    Return "nor"
                Case "n'ko"
                    Return "nqo"
                Case "pedi", "sepedi", "northern sotho"
                    Return "nso"
                Case "nubian languages"
                    Return "nub"
                Case "classical newari", "old newari", "classical nepal bhasa"
                    Return "nwc"
                Case "chichewa", "chewa", "nyanja"
                    Return "nya"
                Case "nyamwezi"
                    Return "nym"
                Case "nyankole"
                    Return "nyn"
                Case "nyoro"
                    Return "nyo"
                Case "nzima"
                    Return "nzi"
                Case "occitan (post 1500)", "provençal"
                    Return "oci"
                Case "ojibwa"
                    Return "oji"
                Case "oriya"
                    Return "ori"
                Case "oromo"
                    Return "orm"
                Case "osage"
                    Return "osa"
                Case "ossetian", "ossetic"
                    Return "oss"
                Case "turkish"
                    Return "ota"
                Case "otomian languages"
                    Return "oto"
                Case "papuan (other)"
                    Return "paa"
                Case "pangasinan"
                    Return "pag"
                Case "pahlavi"
                    Return "pal"
                Case "pampanga", "kapampangan"
                    Return "pam"
                Case "panjabi", "punjabi"
                    Return "pan"
                Case "papiamento"
                    Return "pap"
                Case "palauan"
                    Return "pau"
                Case "persian"
                    Return "peo"
                Case "persian"
                    Return "per"
                Case "philippine (other)"
                    Return "phi"
                Case "phoenician"
                    Return "phn"
                Case "pali"
                    Return "pli"
                Case "polish"
                    Return "pol"
                Case "pohnpeian"
                    Return "pon"
                Case "portuguese"
                    Return "por"
                Case "prakrit languages"
                    Return "pra"
                Case "provençal"
                    Return "pro"
                Case "pushto", "pashto"
                    Return "pus"
                Case "reserved for local use"
                    Return "qaa-qtz"
                Case "quechua"
                    Return "que"
                Case "rajasthani"
                    Return "raj"
                Case "rapanui"
                    Return "rap"
                Case "rarotongan", "cook islands maori"
                    Return "rar"
                Case "romance (other)"
                    Return "roa"
                Case "romansh"
                    Return "roh"
                Case "romany"
                    Return "rom"
                Case "romanian", "moldavian", "moldovan"
                    Return "rum"
                Case "rundi"
                    Return "run"
                Case "aromanian", "arumanian", "macedo-romanian"
                    Return "rup"
                Case "russian"
                    Return "rus"
                Case "sandawe"
                    Return "sad"
                Case "sango"
                    Return "sag"
                Case "yakut"
                    Return "sah"
                Case "south american indian (other)"
                    Return "sai"
                Case "salishan languages"
                    Return "sal"
                Case "samaritan aramaic"
                    Return "sam"
                Case "sanskrit"
                    Return "san"
                Case "sasak"
                    Return "sas"
                Case "santali"
                    Return "sat"
                Case "sicilian"
                    Return "scn"
                Case "scots"
                    Return "sco"
                Case "selkup"
                    Return "sel"
                Case "semitic (other)"
                    Return "sem"
                Case "irish"
                    Return "sga"
                Case "sign languages"
                    Return "sgn"
                Case "shan"
                    Return "shn"
                Case "sidamo"
                    Return "sid"
                Case "sinhala", "sinhalese"
                    Return "sin"
                Case "siouan languages"
                    Return "sio"
                Case "sino-tibetan (other)"
                    Return "sit"
                Case "slavic (other)"
                    Return "sla"
                Case "slovak"
                    Return "slo"
                Case "slovenian"
                    Return "slv"
                Case "southern sami"
                    Return "sma"
                Case "northern sami"
                    Return "sme"
                Case "sami languages (other)"
                    Return "smi"
                Case "lule sami"
                    Return "smj"
                Case "inari sami"
                    Return "smn"
                Case "samoan"
                    Return "smo"
                Case "skolt sami"
                    Return "sms"
                Case "shona"
                    Return "sna"
                Case "sindhi"
                    Return "snd"
                Case "soninke"
                    Return "snk"
                Case "sogdian"
                    Return "sog"
                Case "somali"
                    Return "som"
                Case "songhai languages"
                    Return "son"
                Case "sotho"
                    Return "sot"
                Case "spanish", "castilian"
                    Return "spa"
                Case "sardinian"
                    Return "srd"
                Case "sranan tongo"
                    Return "srn"
                Case "serbian"
                    Return "srp"
                Case "serer"
                    Return "srr"
                Case "nilo-saharan (other)"
                    Return "ssa"
                Case "swati"
                    Return "ssw"
                Case "sukuma"
                    Return "suk"
                Case "sundanese"
                    Return "sun"
                Case "susu"
                    Return "sus"
                Case "sumerian"
                    Return "sux"
                Case "swahili"
                    Return "swa"
                Case "swedish"
                    Return "swe"
                Case "classical syriac"
                    Return "syc"
                Case "syriac"
                    Return "syr"
                Case "tahitian"
                    Return "tah"
                Case "tai (other)"
                    Return "tai"
                Case "tamil"
                    Return "tam"
                Case "tatar"
                    Return "tat"
                Case "telugu"
                    Return "tel"
                Case "timne"
                    Return "tem"
                Case "tereno"
                    Return "ter"
                Case "tetum"
                    Return "tet"
                Case "tajik"
                    Return "tgk"
                Case "tagalog"
                    Return "tgl"
                Case "thai"
                    Return "tha"
                Case "tibetan"
                    Return "tib"
                Case "tigre"
                    Return "tig"
                Case "tigrinya"
                    Return "tir"
                Case "tiv"
                    Return "tiv"
                Case "tokelau"
                    Return "tkl"
                Case "klingon", "tlhingan-hol"
                    Return "tlh"
                Case "tlingit"
                    Return "tli"
                Case "tamashek"
                    Return "tmh"
                Case "tonga (nyasa)"
                    Return "tog"
                Case "tonga (tonga islands)"
                    Return "ton"
                Case "tok pisin"
                    Return "tpi"
                Case "tsimshian"
                    Return "tsi"
                Case "tswana"
                    Return "tsn"
                Case "tsonga"
                    Return "tso"
                Case "turkmen"
                    Return "tuk"
                Case "tumbuka"
                    Return "tum"
                Case "tupi languages"
                    Return "tup"
                Case "turkish"
                    Return "tur"
                Case "altaic (other)"
                    Return "tut"
                Case "tuvalu"
                    Return "tvl"
                Case "twi"
                    Return "twi"
                Case "tuvinian"
                    Return "tyv"
                Case "udmurt"
                    Return "udm"
                Case "ugaritic"
                    Return "uga"
                Case "uighur", "uyghur"
                    Return "uig"
                Case "ukrainian"
                    Return "ukr"
                Case "umbundu"
                    Return "umb"
                Case "undetermined"
                    Return "und"
                Case "urdu"
                    Return "urd"
                Case "uzbek"
                    Return "uzb"
                Case "vai"
                    Return "vai"
                Case "venda"
                    Return "ven"
                Case "vietnamese"
                    Return "vie"
                Case "volapük"
                    Return "vol"
                Case "votic"
                    Return "vot"
                Case "wakashan languages"
                    Return "wak"
                Case "walamo"
                    Return "wal"
                Case "waray"
                    Return "war"
                Case "washo"
                    Return "was"
                Case "welsh"
                    Return "wel"
                Case "sorbian languages"
                    Return "wen"
                Case "walloon"
                    Return "wln"
                Case "wolof"
                    Return "wol"
                Case "kalmyk", "oirat"
                    Return "xal"
                Case "xhosa"
                    Return "xho"
                Case "yao"
                    Return "yao"
                Case "yapese"
                    Return "yap"
                Case "yiddish"
                    Return "yid"
                Case "yoruba"
                    Return "yor"
                Case "yupik languages"
                    Return "ypk"
                Case "zapotec"
                    Return "zap"
                Case "blissymbols", "blissymbolics", "bliss"
                    Return "zbl"
                Case "zenaga"
                    Return "zen"
                Case "zhuang", "chuang"
                    Return "zha"
                Case "zande languages"
                    Return "znd"
                Case "zulu"
                    Return "zul"
                Case "zuni"
                    Return "zun"
                Case "no linguistic content", "not applicable"
                    Return "zxx"
                Case "zaza", "dimili", "dimli", "kirdki", "kirmanjki", "zazaki"
                    Return "zza"
                Case Else
                    Return String.Empty
            End Select

        End Function

#End Region '*** MInfo - Routines/Functions



    End Class

#End Region '*** MInfo Class



#Region "Fileinfo Class"

    <XmlRoot("fileinfo")> _
    Public Class Fileinfo

#Region "Fileinfo - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private _streamdetails As New StreamData

#End Region '*** Fileinfo - Declarations


#Region "Properties"

        ' ########################################
        ' ############## PROPERTIES ##############
        ' ########################################

        <XmlElement("streamdetails")> _
        Property StreamDetails() As StreamData
            Get
                Return _streamdetails
            End Get
            Set(ByVal value As StreamData)
                _streamdetails = value
            End Set
        End Property

#End Region '*** Fileinfo - Properties

    End Class

#End Region '*** Fileinfo Class



#Region "StreamData Class"

    <XmlRoot("streamdata")> _
    Public Class StreamData

#Region "StreamData - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private _video As New List(Of Video)
        Private _audio As New List(Of Audio)
        Private _subtitle As New List(Of Subtitle)

#End Region '*** StreamData - Declarations



#Region "StreamData - Properties"

        ' ########################################
        ' ############## PROPERTIES ##############
        ' ########################################

        <XmlElement("video")> _
        Public Property Video() As List(Of Video)
            Get
                Return Me._video
            End Get
            Set(ByVal Value As List(Of Video))
                Me._video = Value
            End Set
        End Property

        <XmlElement("audio")> _
         Public Property Audio() As List(Of Audio)
            Get
                Return Me._audio
            End Get
            Set(ByVal Value As List(Of Audio))
                Me._audio = Value
            End Set
        End Property

        <XmlElement("subtitle")> _
        Public Property Subtitle() As List(Of Subtitle)
            Get
                Return Me._subtitle
            End Get
            Set(ByVal Value As List(Of Subtitle))
                Me._subtitle = Value
            End Set
        End Property

#End Region '*** StreamData - Properties


    End Class

#End Region '*** StreamData Class



#Region "Video Class"

    Public Class Video

#Region "Video - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private _width As String
        Private _height As String
        Private _codec As String
        Private _formatinfo As String
        Private _duration As String
        Private _bitrate As String
        Private _bitratemode As String
        Private _bitratemax As String
        Private _codecid As String
        Private _codecidinfo As String
        Private _scantype As String
        Private _aspectdisplayratio As String

#End Region '*** Video - Declarations



#Region "Video - Properties"

        ' ########################################
        ' ############## PROPERTIES ##############
        ' ########################################

        <XmlElement("width")> _
        Public Property Width() As String
            Get
                Return Me._width
            End Get
            Set(ByVal Value As String)
                Me._width = Value
            End Set
        End Property

        <XmlElement("height")> _
        Public Property Height() As String
            Get
                Return Me._height
            End Get
            Set(ByVal Value As String)
                Me._height = Value
            End Set
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlElement("formatinfo")> _
        Public Property FormatInfo() As String
            Get
                Return Me._formatinfo
            End Get
            Set(ByVal Value As String)
                Me._formatinfo = Value
            End Set
        End Property

        <XmlElement("duration")> _
        Public Property Duration() As String
            Get
                Return Me._duration
            End Get
            Set(ByVal Value As String)
                Me._duration = Value
            End Set
        End Property

        <XmlElement("bitrate")> _
        Public Property Bitrate() As String
            Get
                Return Me._bitrate
            End Get
            Set(ByVal Value As String)
                Me._bitrate = Value
            End Set
        End Property

        <XmlElement("bitratemode")> _
        Public Property BitrateMode() As String
            Get
                Return Me._bitratemode
            End Get
            Set(ByVal Value As String)
                Me._bitratemode = Value
            End Set
        End Property

        <XmlElement("bitratemax")> _
        Public Property BitrateMax() As String
            Get
                Return Me._bitratemax
            End Get
            Set(ByVal Value As String)
                Me._bitratemax = Value
            End Set
        End Property

        <XmlElement("codecid")> _
        Public Property CodecID() As String
            Get
                Return Me._codecid
            End Get
            Set(ByVal Value As String)
                Me._codecid = Value
            End Set
        End Property

        <XmlElement("codecidinfo")> _
        Public Property CodecidInfo() As String
            Get
                Return Me._codecidinfo
            End Get
            Set(ByVal Value As String)
                Me._codecidinfo = Value
            End Set
        End Property

        <XmlElement("scantype")> _
        Public Property ScanType() As String
            Get
                Return Me._scantype
            End Get
            Set(ByVal Value As String)
                Me._scantype = Value
            End Set
        End Property

        <XmlElement("aspectdisplayratio")> _
        Public Property AspectDisplayRatio() As String
            Get
                Return Me._aspectdisplayratio
            End Get
            Set(ByVal Value As String)
                Me._aspectdisplayratio = Value
            End Set
        End Property

#End Region 'Video - Properties


    End Class

#End Region '*** Video Class



#Region "Audio Class"

    Public Class Audio

#Region "Audio - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private _codec As String
        Private _channels As String
        Private _bitrate As String
        Private _language As String

#End Region '*** Audio - Declarations



#Region "Audio - Properties"

        ' ########################################
        ' ############## PROPERTIES ##############
        ' ########################################

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlElement("channels")> _
        Public Property Channels() As String
            Get
                Return Me._channels
            End Get
            Set(ByVal Value As String)
                Me._channels = Value
            End Set
        End Property

        <XmlElement("bitrate")> _
        Public Property Bitrate() As String
            Get
                Return Me._bitrate
            End Get
            Set(ByVal Value As String)
                Me._bitrate = Value
            End Set
        End Property

#End Region '*** Audio - Properties

    End Class

#End Region '*** Audio Class



#Region "Subtitle Class"


    Public Class Subtitle

#Region "Subtitle - Declarations"

        ' ########################################
        ' ############# DECLARATIONS #############
        ' ########################################

        Private _language As String

#End Region '*** Subtitle - Declarations



#Region "Subtitle - Properties"

        ' ########################################
        ' ############## PROPERTIES ##############
        ' ########################################

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

#End Region '*** Subtitle - Properties

    End Class

#End Region '*** Subtitle Class



End Namespace
