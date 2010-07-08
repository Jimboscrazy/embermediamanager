Imports System.Collections.Generic
Imports System.IO

Namespace EBML
    Public Class EbmlContainerElement
        Inherits EbmlElement
        Private _children As List(Of EbmlElement)

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
            _children = New List(Of EbmlElement)()
        End Sub

        Public Overrides ReadOnly Property IsContainer() As Boolean
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property Children() As List(Of EbmlElement)
            Get
                Return _children
            End Get
        End Property

        Public Function FindElement(ByVal id As Long) As EbmlElement
            For i As Integer = 0 To _children.Count - 1
                If _children(i).ID = id Then
                    Return _children(i)
                End If
            Next
            Return Nothing
        End Function

        Public Function CountElement(ByVal id As Long) As Integer
            Dim counter As Integer = 0
            For i As Integer = 0 To _children.Count - 1
                If _children(i).ID = id Then
                    counter += 1
                End If
            Next
            Return counter
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            EbmlUtility.WriteID(strm, _id)
            Select Case options
                Case EBMLWriteOptions.WriteFixedSizeUseBigMemory
                    Using ms As New MemoryStream()
                        For Each element As EbmlElement In _children
                            element.WriteTo(ms, options)
                        Next
                        ms.Flush()
                        EbmlUtility.WriteVariableSizeInteger(strm, ms.Length)
                        ms.WriteTo(strm)
                        ms.Close()
                    End Using
                    Exit Select
                Case EBMLWriteOptions.WriteFixedSizeUseSeek
                    Throw New NotImplementedException()
                Case EBMLWriteOptions.WriteUnknownSize
                    Throw New NotImplementedException()
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Sub
    End Class
End Namespace
