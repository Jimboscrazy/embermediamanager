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

Imports System.Collections.Generic
Imports System.IO

Namespace EBML
    Public Class EbmlDocument
        Private _roots As New List(Of EbmlElement)()
        Private _semantic As EbmlSemantic
        Private _loadPayLoad As Boolean
        Property LoadPayLoad() As Boolean
            Get
                Return _loadPayLoad
            End Get
            Set(ByVal value As Boolean)
                _loadPayLoad = value
            End Set
        End Property

        Public Sub New(ByVal semantic As EbmlSemantic)
            _semantic = semantic
        End Sub
        Public Sub New(ByVal semantic As EbmlSemantic, ByVal strm As Stream)
            Me.New(semantic, strm, False)
        End Sub
        Public Sub New(ByVal _loadPlayLoad As Boolean, ByVal semantic As EbmlSemantic, ByVal strm As Stream)
            Me.New(_loadPlayLoad, semantic, strm, False)
        End Sub
        Public Sub New(ByVal semantic As EbmlSemantic, ByVal strm As Stream, ByVal single_root As Boolean)
            Me.New(semantic)
            ReadEbml(semantic, strm, single_root)
        End Sub
        Public Sub New(ByVal _loadPayLoad As Boolean, ByVal semantic As EbmlSemantic, ByVal strm As Stream, ByVal single_root As Boolean)
            Me.New(semantic)
            LoadPayLoad = _loadPayLoad
            ReadEbml(semantic, strm, single_root)
        End Sub

        Private Sub ReadEbml(ByVal semantic As EbmlSemantic, ByVal strm As Stream, ByVal single_root As Boolean)
            Dim buffer As Byte() = New Byte(7) {}
            Dim stack1 As New Stack(Of Long)()
            Dim stack2 As New Stack(Of EbmlContainerElement)()
            Dim parent As EbmlContainerElement = Nothing
            Dim endPos As Long = -1, curPos As Long = 0
            Dim strmPos As Long = 0
            While True
                Dim usize As Long
                Dim width As Integer
                Dim ewidth As Integer
                strmPos = curPos
                Dim id As Long = EbmlUtility.ReadID(strm, usize, width)
                curPos += width
                If id < 0 Then
                    Exit While
                End If
                Dim size As Long = EbmlUtility.ReadVariableSizeInteger(strm, usize, ewidth)
                curPos += ewidth
                Dim element As EbmlElement = _semantic.CreateElement(id)
                element.StreamPosition = strmPos
                element.ElementSize = width + ewidth + size
                element.PayLoadSize = size
                Dim container As EbmlContainerElement = TryCast(element, EbmlContainerElement)
                If (Not _loadPayLoad AndAlso container IsNot Nothing AndAlso _semantic.IsPayLoadElement(container.ID)) Then
                    curPos += size
                    strm.Seek(size, SeekOrigin.Current)
                    endPos = -1
                    If parent Is Nothing Then
                        _roots.Add(element)
                    Else
                        parent.Children.Add(element)
                    End If
                    Continue While
                End If
                If element IsNot Nothing Then
                    If container Is Nothing Then
                        If size = usize Then
                            Throw New FormatException("Value element is infinity size")
                        End If
                        If size > Integer.MaxValue Then
                            Throw New NotSupportedException("Value size is larger than 2^31 - 1")
                        End If
                        If buffer.Length < size Then
                            Array.Resize(Of Byte)(buffer, CInt(size))
                        End If
                        If _loadPayLoad OrElse (Not _loadPayLoad AndAlso Not _semantic.IsPayLoadElement(id)) Then
                            element.Loaded = True
                            strm.Read(buffer, 0, CInt(size))
                            TryCast(element, EbmlValueElement).UpdateValue(buffer, 0, CInt(size))
                        Else
                            strm.Seek(size, SeekOrigin.Current)
                        End If
                        curPos += size
                    End If
                    If parent Is Nothing Then
                        _roots.Add(element)
                    Else
                        parent.Children.Add(element)
                    End If
                Else
                    If usize = size Then
                        Throw New NotSupportedException()
                    End If
                    strm.Seek(size, SeekOrigin.Current)
                    curPos += size
                End If
                If endPos = curPos OrElse element Is Nothing Then
                    While True
                        If stack1.Count = 0 Then
                            endPos = -1
                            parent = Nothing
                            Exit While
                        Else
                            endPos = stack1.Pop()
                            parent = stack2.Pop()
                        End If
                        If endPos > curPos OrElse endPos = -1 Then
                            Exit While
                        End If
                    End While
                ElseIf endPos >= 0 AndAlso endPos < curPos Then
                    Throw New FormatException()
                End If
                If container IsNot Nothing Then
                    element.Loaded = True
                    stack1.Push(endPos)
                    stack2.Push(parent)
                    If size = usize Then
                        'Console.WriteLine ("{0} ～", curPos);
                        endPos = -1
                    Else
                        'Console.WriteLine ("{0} ～ {1} ({2} bytes)", curPos, endPos, size);
                        endPos = curPos + size
                    End If
                    parent = container

                End If
                If single_root AndAlso stack1.Count = 0 Then
                    Exit While
                End If
            End While
        End Sub

        Public ReadOnly Property RootElements() As List(Of EbmlElement)
            Get
                Return _roots
            End Get
        End Property

        Public Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            For Each element As EbmlElement In _roots
                element.WriteTo(strm, options)
            Next
        End Sub
    End Class
End Namespace
