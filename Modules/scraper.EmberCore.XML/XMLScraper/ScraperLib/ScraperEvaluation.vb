Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace XMLScraper
    Namespace ScraperLib

        Public Class ScraperEvaluation
            Implements IComparable(Of ScraperEvaluation)

#Region "Fields"

            Private m_ErrorType As NodeErrorType
            Private m_Location As String
            Private m_Message As String

#End Region 'Fields

#Region "Constructors"

            Public Sub New(ByVal ErrorType As NodeErrorType, ByVal Path As String, ByVal Message As String)
                Me.ErrorType = ErrorType
                Me.Message = Message
                Me.Location = Path
            End Sub

#End Region 'Constructors

#Region "Enumerations"

            Public Enum NodeErrorType
                DuplicateSetting
                Expression
                Input
                Output
                Conditional
                RegExpDestination
                FunctionDestination
                StandardFunctionMissing
                DuplicateFunction
            End Enum

#End Region 'Enumerations

#Region "Properties"

            Public Property ErrorType() As NodeErrorType
                Get
                    Return m_ErrorType
                End Get
                Friend Set(ByVal value As NodeErrorType)
                    m_ErrorType = Value
                End Set
            End Property

            Public Property Location() As String
                Get
                    Return m_Location
                End Get
                Friend Set(ByVal value As String)
                    m_Location = Value
                End Set
            End Property

            Public Property Message() As String
                Get
                    Return m_Message
                End Get
                Friend Set(ByVal value As String)
                    m_Message = Value
                End Set
            End Property

#End Region 'Properties

#Region "Methods"

            Public Function CompareTo(ByVal other As ScraperEvaluation) As Integer Implements IComparable(Of ScraperEvaluation).CompareTo
                If IsNothing(Me) Then
                    If IsNothing(other) Then
                        Return 0
                    Else
                        Return 1
                    End If
                Else
                    If IsNothing(other) Then
                        Return -1
                    Else
                        Return Location.CompareTo(other.Location)
                    End If
                End If
            End Function

#End Region 'Methods

        End Class

    End Namespace
End Namespace
