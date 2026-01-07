<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        lblMyCaption = New Label()
        lblInputPrompt = New Label()
        txtUserInput = New TextBox()
        lblOutputField = New Label()
        btnGetResult = New Button()
        SuspendLayout()
        ' 
        ' lblMyCaption
        ' 
        lblMyCaption.AutoSize = True
        lblMyCaption.Font = New Font("Lucida Console", 14.0F)
        lblMyCaption.ForeColor = Color.White
        lblMyCaption.Location = New Point(228, 43)
        lblMyCaption.Name = "lblMyCaption"
        lblMyCaption.Size = New Size(437, 28)
        lblMyCaption.TabIndex = 0
        lblMyCaption.Text = "-- PARTIAL DIGEST ALGORITHM --"
        ' 
        ' lblInputPrompt
        ' 
        lblInputPrompt.AutoSize = True
        lblInputPrompt.Font = New Font("Lucida Console", 14.0F)
        lblInputPrompt.ForeColor = Color.White
        lblInputPrompt.Location = New Point(54, 119)
        lblInputPrompt.Name = "lblInputPrompt"
        lblInputPrompt.Size = New Size(692, 28)
        lblInputPrompt.TabIndex = 1
        lblInputPrompt.Text = "ENTER DISTANCES IN FORMAT [d1, d2, d3, ...]:"
        ' 
        ' txtUserInput
        ' 
        txtUserInput.BackColor = SystemColors.Control
        txtUserInput.Font = New Font("Lucida Console", 14.0F)
        txtUserInput.Location = New Point(54, 163)
        txtUserInput.Name = "txtUserInput"
        txtUserInput.Size = New Size(883, 35)
        txtUserInput.TabIndex = 2
        ' 
        ' lblOutputField
        ' 
        lblOutputField.AutoSize = True
        lblOutputField.Font = New Font("Lucida Console", 14.0F)
        lblOutputField.ForeColor = Color.White
        lblOutputField.Location = New Point(54, 268)
        lblOutputField.Name = "lblOutputField"
        lblOutputField.Size = New Size(556, 28)
        lblOutputField.TabIndex = 3
        lblOutputField.Text = "THE RESULT WILL BE DISPLAYED HERE."
        ' 
        ' btnGetResult
        ' 
        btnGetResult.BackColor = SystemColors.Control
        btnGetResult.Font = New Font("Lucida Console", 14.0F)
        btnGetResult.ForeColor = Color.Black
        btnGetResult.Location = New Point(342, 340)
        btnGetResult.Name = "btnGetResult"
        btnGetResult.Size = New Size(298, 77)
        btnGetResult.TabIndex = 5
        btnGetResult.Text = "GET RESULT!"
        btnGetResult.UseVisualStyleBackColor = False
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(11.0F, 24.0F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        BackColor = Color.CornflowerBlue
        ClientSize = New Size(999, 450)
        Controls.Add(btnGetResult)
        Controls.Add(lblOutputField)
        Controls.Add(txtUserInput)
        Controls.Add(lblInputPrompt)
        Controls.Add(lblMyCaption)
        ForeColor = Color.Black
        Name = "frmMain"
        Text = "Partial Digest VB"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblMyCaption As Label
    Friend WithEvents lblInputPrompt As Label
    Friend WithEvents txtUserInput As TextBox
    Friend WithEvents lblOutputField As Label
    Friend WithEvents btnGetResult As Button

End Class
