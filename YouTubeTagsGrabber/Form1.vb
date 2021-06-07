Imports System.Net

Public Class Form1

    Dim webB As New WebBrowser
    Dim htmlDOM As HtmlDocument
    Dim htmlColl As HtmlElementCollection
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles txtYTURL.KeyDown
        If e.KeyData = Keys.Enter Then
            txtYTURL.Enabled = False
            WebBrowser1.ScriptErrorsSuppressed = True
            WebBrowser1.DocumentText = New WebClient().DownloadString(txtYTURL.Text)

            htmlDOM = WebBrowser1.Document

        End If
    End Sub

    Dim videoID, videoTitle As String

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        htmlColl = htmlDOM.GetElementsByTagName("meta")
        For Each ele As HtmlElement In htmlColl
            If ele.Name = "keywords" Then
                txtTags.Text = ele.GetAttribute("content")
            End If
            If ele.GetAttribute("itemprop") = "duration" Then
                txtDuration.Text = ele.GetAttribute("content")
            End If
            If ele.GetAttribute("itemprop") = "videoId" Then
                videoID = ele.GetAttribute("content")
            End If
            If ele.GetAttribute("itemprop") = "name" Then
                videoTitle = ele.GetAttribute("content")
            End If
        Next
        txtYTURL.Enabled = True
        GenerateThumbnail()
    End Sub

    Public Function ConvertPTTime(ptval As String) As TimeSpan
        Dim tempVal As String = ptval.Replace("PT", "")
        Dim min As String = tempVal.Split("M")(0)
        Dim sec As String = tempVal.Split("M")(1).Split("S")(0)

        Dim timespan As New TimeSpan(0, min, sec)

        Return timespan
    End Function

    Public Function GetReadableTime(tSpan As TimeSpan) As String
        'removes 00: if hours is ZERO
        If tSpan.ToString().StartsWith("00") Then
            Return tSpan.ToString().Replace("00:", "")
        End If
        Return tSpan.ToString()
    End Function


    Private Sub WebBrowser1_ProgressChanged(sender As Object, e As WebBrowserProgressChangedEventArgs) Handles WebBrowser1.ProgressChanged
        prgBar.Maximum = e.MaximumProgress
        prgBar.Value = e.CurrentProgress
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        MsgBox("Time is formatted in PTXXMXXS.", MsgBoxStyle.Information, "Information:")
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        MsgBox("Click enter to start the process.", MsgBoxStyle.Information, "Information:")
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        If IsNothing(ytThumb.Image) Then
            'Nothing to Copy
        Else
            Clipboard.SetImage(ytThumb.Image)
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim svDiag As New SaveFileDialog()
        svDiag.Filter = "PNG Image (.png)|*.png|Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg"
        svDiag.FileName = videoID
        If svDiag.ShowDialog = DialogResult.OK Then
            ytThumb.Image.Save(svDiag.FileName)
        End If
    End Sub


    Public Sub GenerateThumbnail()
        Dim sF As StringFormat = New StringFormat
        sF.LineAlignment = StringAlignment.Near
        sF.Alignment = StringAlignment.Center

        Dim OpaqueBoxBrush = New SolidBrush(Color.FromArgb(128, 0, 0, 0))


        Dim bmp As New Bitmap(1280, 720)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.DrawImage(Image.FromStream(WebRequest.Create("https://img.youtube.com/vi/" & videoID & "/maxresdefault.jpg").GetResponse().GetResponseStream()), New Rectangle(0, 0, bmp.Width, bmp.Height))
            g.FillRectangle(OpaqueBoxBrush, New Rectangle(0, 640, bmp.Width, bmp.Height))
            g.FillRectangle(OpaqueBoxBrush, New Rectangle(1040, 0, 250, 120))
            g.DrawString(videoTitle, New Font(SystemFonts.DefaultFont.FontFamily, 26, FontStyle.Bold), Brushes.White, New Rectangle(0, 650, bmp.Width, bmp.Height), sF)

            sF.LineAlignment = StringAlignment.Center
            sF.Alignment = StringAlignment.Center
            g.DrawString(GetReadableTime(ConvertPTTime(txtDuration.Text)), New Font(SystemFonts.DialogFont.FontFamily, 30, FontStyle.Bold), Brushes.White, New Rectangle(1040, 0, 250, 120), sF)
        End Using

        ytThumb.Image = bmp

    End Sub
End Class
