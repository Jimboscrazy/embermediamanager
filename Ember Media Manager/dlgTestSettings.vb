Imports System.Windows.Forms

Public Class dlgTestSettings

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Scrapes As List(Of Panel)

    Class SEntrie
        Private WithEvents b As New Button
        Private p As New Panel
        Private l As New Label
        Private myorder As Integer
        Private myheight As Integer
        Private scraper As EmberAPI.ModulesManager._externalScraperModuleClass
        Private control As dlgTestSettings.EntrieController
        Sub add(ByRef basep As Panel, ByVal order As Integer, ByVal scraperID As Integer, ByRef controller As dlgTestSettings.EntrieController)
            scraper = ModulesManager.Instance.GetScraperByIdx(order)
            Dim p As New Panel
            basep.Controls.Add(p)
            p.Top = 80 + (order * 30)
            p.Left = 3
            p.Width = 530
            p.Height = 25
            p.BorderStyle = BorderStyle.FixedSingle
            Dim l As New Label
            l.Text = scraper.ProcessorModule.ModuleName
            p.Controls.Add(l)
            l.Left = 25
            l.Visible = True
            'Dim b As New Button
            b = New Button
            'AddHandler b.Click, AddressOf scraper_Click
            b.Text = "+"
            b.Tag = p
            'b.ImageList = ImageList1
            b.ImageIndex = 1
            b.Width = 22
            b.Height = 22
            p.Controls.Add(b)
            p.Visible = True
            myorder = order
            p.Tag = order
            myheight = scraper.ProcessorModule.testSetupScraper(p)
            control = controller
        End Sub
        Private Sub scraper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles b.Click
            Dim s As Button
            s = DirectCast(sender, Button)
            If s.Text = "+" Then
                s.Text = "-"
                control("Expand", myorder, myheight)
            Else
                s.Text = "+"
                control("Collapse", myorder, myheight)
            End If
        End Sub
    End Class
    Delegate Sub EntrieController(ByVal action As String, ByVal order As Integer, ByVal h As Integer)
    Sub MyConytroller(ByVal action As String, ByVal order As Integer, ByVal h As Integer)
        Dim p As Panel = Nothing
        For Each p1 As Panel In Panel1.Controls
            If Not p1.Tag Is Nothing AndAlso Convert.ToInt64(p1.Tag) = order Then
                p = p1
                Exit For
            End If
        Next

        For Each pp As Panel In Panel1.Controls
            If pp.Tag Is Nothing Then Continue For
            Select Case action
                Case "Expand"
                    If pp.Tag.ToString = p.Tag.ToString Then
                        p.Height += h + 5 'fixed for testing
                    ElseIf pp.Top > p.Top Then
                        pp.Top += h + 5 'fixed for testing
                    End If

                Case "Collapse"
                    If pp.Tag.ToString = p.Tag.ToString Then
                        p.Height -= h + 5 'fixed for testing
                    ElseIf pp.Top > p.Top Then
                        pp.Top -= h + 5 'fixed for testing
                    End If


            End Select
        Next
    End Sub
    Private Sub dlgTestSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim nScrapers As Integer = ModulesManager.Instance.ScrapersCount
        Dim top As Integer = 80

        For c = 0 To nScrapers - 1
            Dim ss As New SEntrie
            ss.add(Panel1, c, c, AddressOf MyConytroller)
        Next
    End Sub
 
End Class
