﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GuideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlOptions = New System.Windows.Forms.Panel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.optVertRescaleM1P1 = New System.Windows.Forms.RadioButton()
        Me.optVertRescale01 = New System.Windows.Forms.RadioButton()
        Me.optVertNoChange = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.optPeakDetect = New System.Windows.Forms.RadioButton()
        Me.optAverageNearest = New System.Windows.Forms.RadioButton()
        Me.optDecimate = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbOutputPoints = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.optStartAtTrigger = New System.Windows.Forms.RadioButton()
        Me.optComplete = New System.Windows.Forms.RadioButton()
        Me.lblFile = New System.Windows.Forms.Label()
        Me.lblLines = New System.Windows.Forms.Label()
        Me.crtOutput = New LiveCharts.WinForms.CartesianChart()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pbParsing = New System.Windows.Forms.ProgressBar()
        Me.MenuStrip1.SuspendLayout()
        Me.pnlOptions.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(773, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.CopyToClipboardToolStripMenuItem})
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
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Enabled = False
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save As"
        '
        'CopyToClipboardToolStripMenuItem
        '
        Me.CopyToClipboardToolStripMenuItem.Enabled = False
        Me.CopyToClipboardToolStripMenuItem.Name = "CopyToClipboardToolStripMenuItem"
        Me.CopyToClipboardToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CopyToClipboardToolStripMenuItem.Text = "Copy To Clipboard"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GuideToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'GuideToolStripMenuItem
        '
        Me.GuideToolStripMenuItem.Name = "GuideToolStripMenuItem"
        Me.GuideToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.GuideToolStripMenuItem.Text = "Guide"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'pnlOptions
        '
        Me.pnlOptions.BackColor = System.Drawing.SystemColors.ControlDark
        Me.pnlOptions.Controls.Add(Me.GroupBox3)
        Me.pnlOptions.Controls.Add(Me.GroupBox2)
        Me.pnlOptions.Controls.Add(Me.GroupBox1)
        Me.pnlOptions.Location = New System.Drawing.Point(15, 68)
        Me.pnlOptions.Name = "pnlOptions"
        Me.pnlOptions.Size = New System.Drawing.Size(338, 232)
        Me.pnlOptions.TabIndex = 1
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.optVertRescaleM1P1)
        Me.GroupBox3.Controls.Add(Me.optVertRescale01)
        Me.GroupBox3.Controls.Add(Me.optVertNoChange)
        Me.GroupBox3.Location = New System.Drawing.Point(174, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(145, 105)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Vertical Scale"
        '
        'optVertRescaleM1P1
        '
        Me.optVertRescaleM1P1.AutoSize = True
        Me.optVertRescaleM1P1.Location = New System.Drawing.Point(7, 72)
        Me.optVertRescaleM1P1.Name = "optVertRescaleM1P1"
        Me.optVertRescaleM1P1.Size = New System.Drawing.Size(127, 17)
        Me.optVertRescaleM1P1.TabIndex = 2
        Me.optVertRescaleM1P1.TabStop = True
        Me.optVertRescaleM1P1.Text = "Rescale to -1.0 ... 1.0"
        Me.optVertRescaleM1P1.UseVisualStyleBackColor = True
        '
        'optVertRescale01
        '
        Me.optVertRescale01.AutoSize = True
        Me.optVertRescale01.Location = New System.Drawing.Point(7, 48)
        Me.optVertRescale01.Name = "optVertRescale01"
        Me.optVertRescale01.Size = New System.Drawing.Size(118, 17)
        Me.optVertRescale01.TabIndex = 1
        Me.optVertRescale01.TabStop = True
        Me.optVertRescale01.Text = "Rescale to 0 .... 1.0"
        Me.optVertRescale01.UseVisualStyleBackColor = True
        '
        'optVertNoChange
        '
        Me.optVertNoChange.AutoSize = True
        Me.optVertNoChange.Checked = True
        Me.optVertNoChange.Location = New System.Drawing.Point(7, 24)
        Me.optVertNoChange.Name = "optVertNoChange"
        Me.optVertNoChange.Size = New System.Drawing.Size(79, 17)
        Me.optVertNoChange.TabIndex = 0
        Me.optVertNoChange.TabStop = True
        Me.optVertNoChange.Text = "No Change"
        Me.optVertNoChange.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.optPeakDetect)
        Me.GroupBox2.Controls.Add(Me.optAverageNearest)
        Me.GroupBox2.Controls.Add(Me.optDecimate)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 123)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(140, 95)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sampling"
        '
        'optPeakDetect
        '
        Me.optPeakDetect.AutoSize = True
        Me.optPeakDetect.Location = New System.Drawing.Point(7, 66)
        Me.optPeakDetect.Name = "optPeakDetect"
        Me.optPeakDetect.Size = New System.Drawing.Size(85, 17)
        Me.optPeakDetect.TabIndex = 2
        Me.optPeakDetect.Text = "Peak Detect"
        Me.optPeakDetect.UseVisualStyleBackColor = True
        '
        'optAverageNearest
        '
        Me.optAverageNearest.AutoSize = True
        Me.optAverageNearest.Location = New System.Drawing.Point(7, 44)
        Me.optAverageNearest.Name = "optAverageNearest"
        Me.optAverageNearest.Size = New System.Drawing.Size(103, 17)
        Me.optAverageNearest.TabIndex = 1
        Me.optAverageNearest.Text = "Average nearest"
        Me.optAverageNearest.UseVisualStyleBackColor = True
        '
        'optDecimate
        '
        Me.optDecimate.AutoSize = True
        Me.optDecimate.Checked = True
        Me.optDecimate.Location = New System.Drawing.Point(7, 20)
        Me.optDecimate.Name = "optDecimate"
        Me.optDecimate.Size = New System.Drawing.Size(109, 17)
        Me.optDecimate.TabIndex = 0
        Me.optDecimate.TabStop = True
        Me.optDecimate.Text = "Decimate/Stretch"
        Me.optDecimate.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbOutputPoints)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.optStartAtTrigger)
        Me.GroupBox1.Controls.Add(Me.optComplete)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(140, 105)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Width Options"
        '
        'cmbOutputPoints
        '
        Me.cmbOutputPoints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOutputPoints.FormattingEnabled = True
        Me.cmbOutputPoints.Items.AddRange(New Object() {"8192", "4096", "2048", "1024", "512"})
        Me.cmbOutputPoints.Location = New System.Drawing.Point(51, 71)
        Me.cmbOutputPoints.Name = "cmbOutputPoints"
        Me.cmbOutputPoints.Size = New System.Drawing.Size(59, 21)
        Me.cmbOutputPoints.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 76)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Output"
        '
        'optStartAtTrigger
        '
        Me.optStartAtTrigger.AutoSize = True
        Me.optStartAtTrigger.Location = New System.Drawing.Point(6, 47)
        Me.optStartAtTrigger.Name = "optStartAtTrigger"
        Me.optStartAtTrigger.Size = New System.Drawing.Size(95, 17)
        Me.optStartAtTrigger.TabIndex = 1
        Me.optStartAtTrigger.TabStop = True
        Me.optStartAtTrigger.Text = "Start at Trigger"
        Me.optStartAtTrigger.UseVisualStyleBackColor = True
        '
        'optComplete
        '
        Me.optComplete.AutoSize = True
        Me.optComplete.Checked = True
        Me.optComplete.Location = New System.Drawing.Point(6, 24)
        Me.optComplete.Name = "optComplete"
        Me.optComplete.Size = New System.Drawing.Size(121, 17)
        Me.optComplete.TabIndex = 0
        Me.optComplete.TabStop = True
        Me.optComplete.Text = "Complete Waveform"
        Me.optComplete.UseVisualStyleBackColor = True
        '
        'lblFile
        '
        Me.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFile.Location = New System.Drawing.Point(13, 24)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(741, 18)
        Me.lblFile.TabIndex = 3
        '
        'lblLines
        '
        Me.lblLines.AutoSize = True
        Me.lblLines.Location = New System.Drawing.Point(14, 48)
        Me.lblLines.Name = "lblLines"
        Me.lblLines.Size = New System.Drawing.Size(35, 13)
        Me.lblLines.TabIndex = 4
        Me.lblLines.Text = "Lines:"
        '
        'crtOutput
        '
        Me.crtOutput.Location = New System.Drawing.Point(359, 68)
        Me.crtOutput.Name = "crtOutput"
        Me.crtOutput.Size = New System.Drawing.Size(390, 232)
        Me.crtOutput.TabIndex = 5
        Me.crtOutput.Text = "CartesianChart1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(384, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Output:"
        '
        'pbParsing
        '
        Me.pbParsing.Location = New System.Drawing.Point(154, 45)
        Me.pbParsing.Name = "pbParsing"
        Me.pbParsing.Size = New System.Drawing.Size(201, 19)
        Me.pbParsing.TabIndex = 7
        Me.pbParsing.Value = 100
        Me.pbParsing.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 312)
        Me.Controls.Add(Me.pbParsing)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.crtOutput)
        Me.Controls.Add(Me.lblLines)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.pnlOptions)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Siglent To FeelElec CSV Converter"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.pnlOptions.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlOptions As Panel
    Friend WithEvents lblFile As Label
    Friend WithEvents lblLines As Label
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents optStartAtTrigger As RadioButton
    Friend WithEvents optComplete As RadioButton
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents optVertRescaleM1P1 As RadioButton
    Friend WithEvents optVertRescale01 As RadioButton
    Friend WithEvents optVertNoChange As RadioButton
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents optAverageNearest As RadioButton
    Friend WithEvents optDecimate As RadioButton
    Friend WithEvents cmbOutputPoints As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents crtOutput As LiveCharts.WinForms.CartesianChart
    Friend WithEvents Label2 As Label
    Friend WithEvents pbParsing As ProgressBar
    Friend WithEvents optPeakDetect As RadioButton
    Friend WithEvents CopyToClipboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GuideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
End Class
