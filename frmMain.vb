#Disable Warning IDE1006  ' To prevent required prefixes from receiving compiler warnings.
Public Class frmMain
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblMyCaption.ForeColor = Color.MintCream
        lblInputPrompt.ForeColor = Color.MintCream
        txtUserInput.BackColor = Color.MintCream
        lblOutputField.ForeColor = Color.MintCream
        btnGetResult.BackColor = Color.Wheat
        btnGetResult.ForeColor = Color.Black
        FormBorderStyle = FormBorderStyle.Fixed3D
        MaximizeBox = False
    End Sub

    Private Sub btnGetResult_Click(sender As Object, e As EventArgs) Handles btnGetResult.Click
        Try
            Dim outputArray = ExtractArrayFromInput(txtUserInput.Text).PartialDigest()
            If outputArray.Length = 0 Then
                lblOutputField.Text = "NO SOLUTIONS COULD BE FOUND."
            Else
                lblOutputField.Text = $"SOLUTION FOUND: [{String.Join(", ", outputArray)}]."
            End If
        Catch
            lblOutputField.Text = "INPUT FIELD CANNOT BE EMPTY. TRY AGAIN."
        End Try
    End Sub

    Private Sub picStartRunning_Click(sender As Object, e As EventArgs)
        btnGetResult_Click(sender, e)
    End Sub
End Class
#Enable Warning IDE1006