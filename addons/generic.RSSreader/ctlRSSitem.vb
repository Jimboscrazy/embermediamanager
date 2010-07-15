Public Class ctlRSSitem
    Public Event Event_Expand(ByVal expanded As Boolean)

    Public Property Title() As String
        Get
            Return lblTitle.Text
        End Get
        Set(ByVal value As String)
            lblTitle.Text = value
        End Set
    End Property
    Public Property URL() As String
        Get
            Return lblURL.Text
        End Get
        Set(ByVal value As String)
            lblURL.Text = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return lblDesc.Text
        End Get
        Set(ByVal value As String)
            lblDesc.Text = value
        End Set
    End Property
    Public Property MovieTitle() As String
        Get
            Return lblMovie.Text
        End Get
        Set(ByVal value As String)
            lblMovie.Text = value
        End Set
    End Property
    Public Property MovieYear() As String
        Get
            Return lblYear.Text
        End Get
        Set(ByVal value As String)
            lblYear.Text = value
            If Not String.IsNullOrEmpty(value) Then
                Label6.Visible = True
                lblYear.Visible = True
            End If
        End Set
    End Property
    Public Property Channel() As String
        Get
            Return lblChannel.Text
        End Get
        Set(ByVal value As String)
            lblChannel.Text = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return lblStatus.Text
        End Get
        Set(ByVal value As String)
            lblStatus.Text = value
        End Set
    End Property
    Private _IMDBid As String
    Public Property IMDBID() As String
        Get
            Return _IMDBid
        End Get
        Set(ByVal value As String)
            _IMDBid = value
            If Not String.IsNullOrEmpty(value) Then
                'lblSearchType.Visible = False
                lblSearchType.ForeColor = Color.Blue
                lblSearchType.Font = New Font(lblSearchType.Font, FontStyle.Underline)
                lblSearchType.Text = String.Concat("IMDB ID: ", value)
            Else
                lblSearchType.Text = "Not Found"
            End If

        End Set
    End Property
    Private _Crc As String
    Public Property CRC() As String
        Get
            Return _Crc
        End Get
        Set(ByVal value As String)
            _Crc = value
        End Set
    End Property
    Private _Expanded As Boolean = False
    Public Property Expanded() As Boolean
        Get
            Return _Expanded
        End Get
        Set(ByVal value As Boolean)
            _Expanded = value
            Height = If(value, 120, 60)
        End Set
    End Property

    Private Sub btnExpand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpand.Click
        Expanded = Not Expanded
        btnExpand.ImageIndex = If(Expanded, 1, 0)
        RaiseEvent Event_Expand(Expanded)
    End Sub

    Private Sub lblURL_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblURL.LinkClicked
        If Not String.IsNullOrEmpty(lblURL.Text) Then
            If Master.isWindows Then
                Process.Start(lblURL.Text)
            Else
                Using Explorer As New Process
                    Explorer.StartInfo.FileName = "xdg-open"
                    Explorer.StartInfo.Arguments = lblURL.Text
                    Explorer.Start()
                End Using
            End If
        End If
    End Sub
    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Height = 60
        lblMovie.Text = String.Empty
        lblYear.Text = String.Empty
        lblStatus.Text = String.Empty
    End Sub

    Private Sub lblSearchType_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblSearchType.MouseClick
        If Not String.IsNullOrEmpty(_IMDBid) Then
            If Not String.IsNullOrEmpty(lblURL.Text) Then
                If Master.isWindows Then
                    Process.Start(String.Format("http://www.imdb.com/title/tt{0}/", _IMDBid))
                Else
                    Using Explorer As New Process
                        Explorer.StartInfo.FileName = "xdg-open"
                        Explorer.StartInfo.Arguments = String.Format("http://www.imdb.com/title/tt{0}/", _IMDBid)
                        Explorer.Start()
                    End Using
                End If
            End If
        End If
    End Sub

    Private Sub lblSearchType_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblSearchType.MouseHover
        If Not String.IsNullOrEmpty(_IMDBid) Then
            Cursor = Cursors.Hand
        End If
    End Sub

    Private Sub lblSearchType_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblSearchType.MouseLeave
        Cursor = Cursors.Default
    End Sub
End Class
