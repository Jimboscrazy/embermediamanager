Imports System.IO

Namespace EBML
    Public MustInherit Class EbmlElement
        Protected _id As Long
        Private _streamPos As Long
        Protected Sub New(ByVal id As Long)
            _id = id
        End Sub

        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public Property StreamPosition()
            Get
                Return _streamPos
            End Get
            Set(ByVal value)
                _streamPos = value
            End Set
        End Property


        Public MustOverride ReadOnly Property IsContainer() As Boolean
        Public MustOverride Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
    End Class
End Namespace
