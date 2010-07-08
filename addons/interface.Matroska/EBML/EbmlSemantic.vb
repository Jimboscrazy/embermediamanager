Imports System.Collections.Generic
Imports System.Reflection

Namespace EBML
    Public NotInheritable Class EbmlSemantic
        Private _table As New Dictionary(Of Long, Entry)()
        Shared ConstructorArgs As Type() = New Type() {GetType(Long)}
        Shared ContainerType As Type = GetType(EbmlContainerElement)
        Shared ElementType As Type = GetType(EbmlElement)

        Public Sub New()
            AddSemantic(&H1A45DFA3, GetType(EbmlContainerElement), "EBML")
            AddSemantic(&H4286, GetType(EbmlUIntElement), "EBMLVersion")
            AddSemantic(&H42F7, GetType(EbmlUIntElement), "EBMLReadVersion")
            AddSemantic(&H42F2, GetType(EbmlUIntElement), "EBMLMaxIDWidth")
            AddSemantic(&H42F3, GetType(EbmlUIntElement), "EBMLMaxSizeWidth")
            AddSemantic(&H4282, GetType(EbmlAsciiElement), "DocType")
            AddSemantic(&H4287, GetType(EbmlUIntElement), "DocTypeVersion")
            AddSemantic(&H4285, GetType(EbmlUIntElement), "DocTypeReadVersion")
            AddSemantic(&HC3, GetType(EbmlContainerElement), "CRC32")
            AddSemantic(&H42FE, GetType(EbmlBinaryElement), "CRC32Value")
            AddSemantic(&HBF, GetType(EbmlBinaryElement), "CRC-32")
            AddSemantic(&HEC, GetType(EbmlVoidElement), "Void")
        End Sub

        Public Sub AddSemantic(ByVal id As Long, ByVal type As Type, ByVal name As String, Optional ByVal IsPayload As Boolean = False)
            If Not type.IsSubclassOf(ElementType) Then
                Throw New ArgumentException()
            End If
            Dim info As ConstructorInfo = type.GetConstructor(ConstructorArgs)
            Dim entry As New Entry(id, type, info, name, IsPayload)
            _table.Add(id, entry)
        End Sub

        Public Function CreateElement(ByVal id As Long) As EbmlElement
            Dim entry As New Entry
            If Not _table.TryGetValue(id, entry) Then
                Return Nothing
            End If
            Return TryCast(entry.Constructor.Invoke(New Object() {id}), EbmlElement)
        End Function

        Public Function GetElementName(ByVal id As Long) As String
            Dim entry As New Entry
            If Not _table.TryGetValue(id, entry) Then
                Return "Unknown"
            End If
            Return entry.Name
        End Function

        Public Function IsPayLoadElement(ByVal id As Long) As Boolean
            Dim entry As New Entry
            If Not _table.TryGetValue(id, entry) Then
                Return "Unknown"
            End If
            Return entry.IsPayLoad
        End Function

        Public Function ContainsID(ByVal id As Long) As Boolean
            Return _table.ContainsKey(id)
        End Function

        Private Structure Entry
            Public Constructor As ConstructorInfo
            Public Type As Type
            Public Name As String
            Public ID As Long
            Public IsValueElement As Boolean
            Public IsPayLoad As Boolean

            Public Sub New(ByVal id__1 As Long, ByVal type__2 As Type, ByVal info As ConstructorInfo, ByVal name__3 As String, Optional ByVal ispayload__4 As Boolean = False)
                ID = id__1
                Type = type__2
                Constructor = info
                Name = name__3
                IsValueElement = Not type__2.IsSubclassOf(ContainerType)
                IsPayLoad = ispayload__4
            End Sub
        End Structure
    End Class
End Namespace
