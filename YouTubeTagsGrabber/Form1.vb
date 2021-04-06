Imports System.Net

Public Class Form1

    Dim webB As New WebBrowser
    Dim htmlDOM As HtmlDocument
    Dim htmlColl As HtmlElementCollection
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyData = Keys.Enter Then
            TextBox1.Enabled = False
            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = New WebClient().DownloadString(TextBox1.Text)

            htmlDOM = WebBrowser1.Document

        End If
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        htmlColl = htmlDOM.GetElementsByTagName("meta")
        For Each ele As HtmlElement In htmlColl
            If ele.Name = "keywords" Then
                TextBox2.Text = ele.GetAttribute("content")
            End If
            If ele.GetAttribute("itemprop") = "duration" Then
                TextBox3.Text = ele.GetAttribute("content")
            End If
        Next
        TextBox1.Enabled = True
    End Sub

    Private Sub WebBrowser1_ProgressChanged(sender As Object, e As WebBrowserProgressChangedEventArgs) Handles WebBrowser1.ProgressChanged
        ProgressBar1.Maximum = e.MaximumProgress
        ProgressBar1.Value = e.CurrentProgress
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        MsgBox("Time is formatted in PTXXMXXS.", MsgBoxStyle.Information, "Information:")
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        MsgBox("Click enter to start the process.", MsgBoxStyle.Information, "Information:")
    End Sub
End Class
