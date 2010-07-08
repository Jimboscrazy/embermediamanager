Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports EBML.Serialization

Namespace EBML.Serialization
    ' TODO: Optimize
    Public Class EbmlFormatter
        Implements IFormatter
        Shared _cache As New Dictionary(Of Long, Type)()
        Shared _elementType As New Dictionary(Of Type, Type)()
        Shared _defaultSemantic As New EbmlSemantic()
        Shared _fieldCache As New Dictionary(Of Type, FieldInfoAndAttribute())()

        ' Type cache
        Shared TypeOfByteArray As Type = GetType(Byte())
        Shared TypeOfEbmlFieldSemanticAttribute As Type = GetType(EbmlFieldSemanticAttribute)
        Shared TypeOfEbmlTypeAttribute As Type = GetType(EbmlTypeAttribute)
        Shared TypeOfEbmlContainerElement As Type = GetType(EbmlContainerElement)
        Shared TypeOfInt64 As Type = GetType(Long)
        Shared TypeOfObject As Type = GetType(Object)

        Shared _instance As New EbmlFormatter()
        Public Shared ReadOnly Property Instance() As EbmlFormatter
            Get
                Return _instance
            End Get
        End Property

        Shared Sub New()
            Dim allAsm As Assembly() = AppDomain.CurrentDomain.GetAssemblies()
            Dim fieldSemanticTypes As New List(Of KeyValuePair(Of Type, EbmlFieldSemanticAttribute))()
            Dim fieldCache As New Dictionary(Of Type, Object)()

            For i As Integer = 0 To allAsm.Length - 1
                Dim types As Type() = allAsm(i).GetTypes()
                For q As Integer = 0 To types.Length - 1
                    Dim atts As Object() = types(q).GetCustomAttributes(TypeOfEbmlFieldSemanticAttribute, False)
                    If atts.Length = 1 Then
                        fieldSemanticTypes.Add(New KeyValuePair(Of Type, EbmlFieldSemanticAttribute)(types(q), DirectCast(atts(0), EbmlFieldSemanticAttribute)))
                    End If

                    atts = types(q).GetCustomAttributes(TypeOfEbmlTypeAttribute, False)
                    For k As Integer = 0 To atts.Length - 1
                        Dim att As EbmlTypeAttribute = DirectCast(atts(k), EbmlTypeAttribute)
                        _elementType(att.Type) = types(q)
                    Next
                Next
            Next
            For Each pair As KeyValuePair(Of Type, EbmlFieldSemanticAttribute) In fieldSemanticTypes
                _cache.Add(pair.Value.ID, If(IsArray(pair.Key), pair.Key.GetElementType(), pair.Key))
                If pair.Value.ElementType Is Nothing Then
                    SetupSemantic(pair.Key, pair.Value)
                End If
                _defaultSemantic.AddSemantic(pair.Value.ID, pair.Value.ElementType, pair.Key.ToString())
                FindFieldSemanticRecursive(pair.Key, fieldCache)
            Next
        End Sub
        Private Sub New()
        End Sub

        Private Shared Sub FindFieldSemanticRecursive(ByVal type As Type, ByVal fieldCache As Dictionary(Of Type, Object))
            Dim fields As FieldInfo() = type.GetFields(BindingFlags.Instance Or BindingFlags.[Public] Or BindingFlags.NonPublic)
            If fieldCache.ContainsKey(type) Then
                Return
            End If
            fieldCache.Add(type, Nothing)
            Dim serializableFields As New List(Of FieldInfoAndAttribute)()
            For i As Integer = 0 To fields.Length - 1
                Dim field As FieldInfo = fields(i)
                Dim att As EbmlFieldSemanticAttribute
                Dim fieldSemantics As Object() = field.GetCustomAttributes(TypeOfEbmlFieldSemanticAttribute, True)
                att = (If(fieldSemantics.Length <> 1, Nothing, TryCast(fieldSemantics(0), EbmlFieldSemanticAttribute)))
                If att Is Nothing Then
                    Continue For
                End If
                serializableFields.Add(New FieldInfoAndAttribute(field, att))
                If _defaultSemantic.ContainsID(att.ID) Then
                    Continue For
                End If
                If att.ElementType Is Nothing Then
                    SetupSemantic(field.FieldType, att)
                End If
                _defaultSemantic.AddSemantic(att.ID, att.ElementType, type.ToString() & "." & field.Name)
                If att.ElementType Is TypeOfEbmlContainerElement Then
                    If Not _cache.ContainsKey(att.ID) Then
                        _cache.Add(att.ID, If(IsArray(field.FieldType), field.FieldType.GetElementType(), field.FieldType))
                        FindFieldSemanticRecursive(field.FieldType, fieldCache)
                    End If
                End If
            Next
            _fieldCache.Add(type, serializableFields.ToArray())
        End Sub

        Private Shared Function IsArray(ByVal type As Type) As Boolean
            Return type.IsArray AndAlso type IsNot TypeOfByteArray
        End Function

        Private Shared Sub SetupSemantic(ByVal type As Type, ByVal att As EbmlFieldSemanticAttribute)
            Dim t As Type = Nothing
            If IsArray(type) Then
                type = type.GetElementType()
            End If
            If _elementType.TryGetValue(type, t) Then
                att.ElementType = t
            Else
                att.ElementType = TypeOfEbmlContainerElement
            End If
        End Sub

        Public Sub Serialize(ByVal serializationStream As Stream, ByVal graph As Object) Implements IFormatter.Serialize
            Dim graphType As Type = graph.[GetType]()
            Dim typeSemantics As Object() = graphType.GetCustomAttributes(TypeOfEbmlFieldSemanticAttribute, False)
            If typeSemantics.Length <> 1 Then
                Throw New SerializationException()
            End If

            Dim doc As New EbmlDocument(_defaultSemantic)
            doc.LoadPayLoad = True
            Serialize(doc.RootElements, graph, graphType, DirectCast(typeSemantics(0), EbmlFieldSemanticAttribute))
            doc.WriteTo(serializationStream, EBMLWriteOptions.WriteFixedSizeUseBigMemory)
        End Sub

        Private Sub Serialize(ByVal list As List(Of EbmlElement), ByVal graph As Object, ByVal graphType As Type, ByVal semanticAtt As EbmlFieldSemanticAttribute)
            If Not IsArray(graphType) Then
                If semanticAtt.ElementType Is Nothing Then
                    SetupSemantic(graphType, semanticAtt)
                End If

                If semanticAtt.ElementType Is TypeOfEbmlContainerElement Then
                    Dim element As New EbmlContainerElement(semanticAtt.ID)
                    list.Add(element)
                    Dim fields As FieldInfoAndAttribute() = _fieldCache(graphType)
                    For i As Integer = 0 To fields.Length - 1
                        Dim fieldValue As Object = fields(i).Field.GetValue(graph)
                        If fieldValue IsNot Nothing Then
                            Serialize(element.Children, fieldValue, fields(i).Field.FieldType, fields(i).SemanticAttribute)
                        End If
                    Next
                Else
                    Dim element As EbmlValueElement = DirectCast(semanticAtt.ElementType.GetConstructor(New Type() {TypeOfInt64}).Invoke(New Object() {semanticAtt.ID}), EbmlValueElement)
                    element.UpdateValue(graph)
                    list.Add(element)
                End If
            Else
                Dim array As Array = DirectCast(graph, Array)
                Dim itemType As Type = graphType.GetElementType()

                If semanticAtt.ElementType Is Nothing Then
                    SetupSemantic(itemType, semanticAtt)
                End If

                If semanticAtt.ElementType Is TypeOfEbmlContainerElement Then
                    For k As Integer = 0 To array.Length - 1
                        Dim element As New EbmlContainerElement(semanticAtt.ID)
                        list.Add(element)
                        Dim fields As FieldInfoAndAttribute() = _fieldCache(itemType)
                        For i As Integer = 0 To fields.Length - 1
                            Dim fieldValue As Object = fields(i).Field.GetValue(array.GetValue(k))
                            If fieldValue IsNot Nothing Then
                                Serialize(element.Children, fieldValue, fields(i).Field.FieldType, fields(i).SemanticAttribute)
                            End If
                        Next
                    Next
                Else
                    For k As Integer = 0 To array.Length - 1
                        Dim element As EbmlValueElement = DirectCast(semanticAtt.ElementType.GetConstructor(New Type() {TypeOfInt64}).Invoke(New Object() {semanticAtt.ID}), EbmlValueElement)
                        element.UpdateValue(array.GetValue(k))
                        list.Add(element)
                    Next
                End If
            End If
        End Sub

        Public Function Deserialize(ByVal serializationStream As Stream) As Object Implements IFormatter.Deserialize
            Dim doc As New EbmlDocument(True, _defaultSemantic, serializationStream, True)
            Return Deserialize(doc.RootElements(0))
        End Function

        Private Function Deserialize(ByVal element As EbmlElement) As Object
            Dim celement As EbmlContainerElement = Nothing
            Dim velement As EbmlValueElement = Nothing

            If (InlineAssignHelper(celement, TryCast(element, EbmlContainerElement))) IsNot Nothing Then
                Dim type As Type = _cache(celement.ID)
                Dim ret As Object = FormatterServices.GetUninitializedObject(type)
                Dim fields As FieldInfoAndAttribute() = _fieldCache(type)
                For i As Integer = 0 To fields.Length - 1
                    Dim field As FieldInfo = fields(i).Field
                    Dim att As EbmlFieldSemanticAttribute = fields(i).SemanticAttribute
                    If IsArray(field.FieldType) Then
                        Dim length As Integer = celement.CountElement(att.ID)
                        If length > 0 Then
                            Dim elementType As Type = field.FieldType.GetElementType()
                            Dim ary As Array = Array.CreateInstance(elementType, length)
                            Dim k As Integer = 0, j As Integer = 0
                            While k < length
                                If celement.Children(j).ID = att.ID Then
                                    Dim value As Object = Deserialize(celement.Children(j))
                                    If elementType.Equals(TypeOfObject) Then
                                        ary.SetValue(value, k)
                                        System.Threading.Interlocked.Increment(k)
                                    Else
                                        ary.SetValue(Convert.ChangeType(value, elementType), k)
                                        System.Threading.Interlocked.Increment(k)
                                    End If
                                End If
                                j += 1
                            End While
                            field.SetValue(ret, ary)
                        End If
                    Else
                        Dim tmp As EbmlElement = celement.FindElement(att.ID)
                        If tmp IsNot Nothing Then
                            If field.FieldType.Equals(TypeOfObject) Then
                                field.SetValue(ret, Deserialize(tmp))
                            Else
                                field.SetValue(ret, Convert.ChangeType(Deserialize(tmp), field.FieldType))
                            End If
                        End If
                    End If
                Next
                Return ret
            ElseIf (InlineAssignHelper(velement, TryCast(element, EbmlValueElement))) IsNot Nothing Then
                Return velement.GetValue()
            Else
                Throw New SerializationException()
            End If
        End Function

        Public Function Serialize(ByVal graph As Object) As Byte()
            Using ms As New MemoryStream()
                Serialize(ms, graph)
                ms.Close()
                Return ms.ToArray()
            End Using
        End Function

        Public Function Deserialize(ByVal data As Byte()) As Object
            Return Deserialize(data, 0, data.Length)
        End Function

        Public Function Deserialize(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer) As Object
            Using ms As New MemoryStream(data, offset, size)
                Return Deserialize(ms)
            End Using
        End Function

#Region "Properties"

        Public Property Binder() As SerializationBinder Implements IFormatter.Binder
            Get
                Throw New NotImplementedException()
            End Get
            Set(ByVal value As SerializationBinder)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Context() As StreamingContext Implements IFormatter.Context
            Get
                Throw New NotImplementedException()
            End Get
            Set(ByVal value As StreamingContext)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property SurrogateSelector() As ISurrogateSelector Implements IFormatter.SurrogateSelector
            Get
                Throw New NotImplementedException()
            End Get
            Set(ByVal value As ISurrogateSelector)
                Throw New NotImplementedException()
            End Set
        End Property

#End Region

#Region "Internal Class"
        Private Class FieldInfoAndAttribute
            Public Field As FieldInfo
            Public SemanticAttribute As EbmlFieldSemanticAttribute

            Public Sub New(ByVal field__1 As FieldInfo, ByVal att As EbmlFieldSemanticAttribute)
                Field = field__1
                SemanticAttribute = att
            End Sub
        End Class
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
            target = value
            Return value
        End Function
#End Region
    End Class
End Namespace
