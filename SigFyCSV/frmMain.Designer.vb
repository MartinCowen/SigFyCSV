<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlOptions = New System.Windows.Forms.Panel()
        Me.btnConvert = New System.Windows.Forms.Button()
        Me.lblFile = New System.Windows.Forms.Label()
        Me.lblLines = New System.Windows.Forms.Label()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveAsToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'pnlOptions
        '
        Me.pnlOptions.BackColor = System.Drawing.SystemColors.ControlDark
        Me.pnlOptions.Location = New System.Drawing.Point(12, 150)
        Me.pnlOptions.Name = "pnlOptions"
        Me.pnlOptions.Size = New System.Drawing.Size(286, 232)
        Me.pnlOptions.TabIndex = 1
        '
        'btnConvert
        '
        Me.btnConvert.Location = New System.Drawing.Point(13, 400)
        Me.btnConvert.Name = "btnConvert"
        Me.btnConvert.Size = New System.Drawing.Size(285, 38)
        Me.btnConvert.TabIndex = 2
        Me.btnConvert.Text = "Convert"
        Me.btnConvert.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFile.Location = New System.Drawing.Point(13, 34)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(741, 30)
        Me.lblFile.TabIndex = 3
        Me.lblFile.Text = "File"
        '
        'lblLines
        '
        Me.lblLines.AutoSize = True
        Me.lblLines.Location = New System.Drawing.Point(12, 77)
        Me.lblLines.Name = "lblLines"
        Me.lblLines.Size = New System.Drawing.Size(35, 13)
        Me.lblLines.TabIndex = 4
        Me.lblLines.Text = "Lines:"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lblLines)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.btnConvert)
        Me.Controls.Add(Me.pnlOptions)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Siglent To FeelElec CSV Converter"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlOptions As Panel
    Friend WithEvents btnConvert As Button
    Friend WithEvents lblFile As Label
    Friend WithEvents lblLines As Label
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
End Class
