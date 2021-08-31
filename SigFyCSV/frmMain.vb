'Converts CSV files from Siglent SDS1104X-E Oscilloscope to list of samples format (.fy) file used by FeelElec FY6900 Arb Signal Generator
'to be imported into their DDS Signal PC Software to be sent to sig gen.

Imports LiveCharts
Imports LiveCharts.Wpf
Imports LiveCharts.WinForms
Imports LiveCharts.Defaults
Imports System.ComponentModel

Public Class frmMain
    Dim inFile As String
    Shared numsamples As Integer
    Private Shared samples(1, 0) As Single
    Dim output(1, 0) As Single
    Dim startsample As Integer

    Dim startAtTrig As Boolean
    Enum samplingType
        decimate
        averagenearest
        peakdetect
    End Enum
    Dim optionSampling As samplingType
    Enum verticalType
        nochange
        rescale01
        rescalem1p1
    End Enum
    Dim optionVertical As verticalType
    'indexes of values in the incoming CSV
    Const idxSecond As Integer = 0
    Const idxVolt As Integer = 1
    Private WithEvents bwParseCSV As New BackgroundWorker
    Public Class bwParseCSVArgType
        Public filename As String
    End Class

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim odlg As New OpenFileDialog
        Try
            With odlg
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                ' Add the default extension, if the user neglects to add an extension.
                .AddExtension = True
                ' Check to verify that the output path actually exists. Prompt before
                ' creating a new file? Prompt before overwriting? 
                .CheckPathExists = True
                .ValidateNames = True
                .ShowHelp = True
                ' If the user doesn't supply an extension, and if the AddExtension property is
                ' True, use this extension. The default is "".
                .DefaultExt = "csv"
                ' Prompt with the current file name if you've specified it.
                '.FileName = GetDocumentName()
                .Filter = "Comma Seperated Value (*.csv)|*.csv"
                .FilterIndex = 1 'default is csv
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.Cursor = Cursors.WaitCursor
                    inFile = .FileName
                    lblFile.Text = inFile

                    Dim args As bwParseCSVArgType = New bwParseCSVArgType()
                    args.filename = .FileName

                    If bwParseCSV.IsBusy = False Then
                        pbParsing.Value = 0 'UI interaction has to be on this thread, not the worker one
                        pbParsing.Visible = True
                        bwParseCSV.RunWorkerAsync(args)
                    End If
                    Me.Cursor = Cursors.Default
                End If
            End With
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Application.ProductName)
        End Try
    End Sub
    Private Sub bwParseCSV_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwParseCSV.DoWork
        Dim args As bwParseCSVArgType = e.Argument
        Dim mParser As New cParser(args.filename)
        mParser.bw = bwParseCSV
        mParser.OpenParseCSV()

    End Sub

    Private Sub bwParseCSV_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwParseCSV.ProgressChanged
        pbParsing.Value = e.ProgressPercentage
        If e.ProgressPercentage = 0 Then 'now know numsamples
            lblLines.Text = "Lines: " & numsamples
        End If
    End Sub

    Private Sub bwParseCSV_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwParseCSV.RunWorkerCompleted
        If e.Cancelled = False Then
            pbParsing.Visible = False
            Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
            FillSampleChart()
            SaveAsToolStripMenuItem.Enabled = True
            CopyToClipboardToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub FillSampleChart()
        crtOutput.DisableAnimations = True
        crtOutput.Hoverable = False

        crtOutput.AxisX(0).MaxValue = Double.NaN 'autoscale x axis to data, which is in seconds

        crtOutput.Series.Clear()

        Dim scol As New SeriesCollection
        Dim scatters As New LineSeries
        scatters.Title = "Samples"
        scatters.Values = New ChartValues(Of ObservablePoint)()
        scatters.PointGeometrySize = 1
        scatters.Fill = Windows.Media.Brushes.Transparent
        scatters.Stroke = Windows.Media.Brushes.DarkBlue

        'scale to size of plot on screen, no point trying to plot more points than are visible
        Dim skips As Integer = output.GetLength(1) / crtOutput.Width
        For i As Integer = 0 To crtOutput.Width - 1
            Dim sp As Integer = i * skips
            If sp > output.GetLength(1) - 1 Then sp = output.GetLength(1) - 1
            scatters.Values.Add(New ObservablePoint(output(idxSecond, sp), output(idxVolt, sp)))
        Next i

        scatters.DataLabels = False
        scol.Add(scatters)
        crtOutput.Series = scol

    End Sub

    Private Sub Convert(numOutputPoints As Integer)

        ReDim output(1, numOutputPoints - 1)

        If startAtTrig = False Then
            startsample = 0
        Else
            'scan through the time values looking for the first 0 or positive value which is where the trigger point on the scope was
            For i As Integer = 0 To numsamples - 1
                If samples(idxSecond, i) >= 0 Then
                    startsample = i
                    Exit For
                End If
            Next i
        End If

        'vertical rescaling, if needed

        Dim vgain As Single = 1 'default, and used for no change option
        Dim voffset As Single = 0  'default, and used for no change option

        If optionVertical <> verticalType.nochange Then

            Dim vmin As Single = Single.MaxValue
            Dim vmax As Single = Single.MinValue
            'find min and max of signal
            For i As Integer = startsample To numsamples - 1
                If samples(idxVolt, i) > vmax Then vmax = samples(idxVolt, i)
                If samples(idxVolt, i) < vmin Then vmin = samples(idxVolt, i)
            Next i
            If optionVertical = verticalType.rescale01 Then
                voffset = vmin
                vgain = 1 / (vmax - vmin)
            End If
            If optionVertical = verticalType.rescalem1p1 Then
                voffset = (vmin + vmax) / 2
                vgain = 2 / (vmax - vmin)
            End If

        End If

        If optionSampling = samplingType.decimate Then

            'precalculate this outside the loop to avoid doing the division inside the loop
            Dim skips As ULong = (numsamples - startsample) / numOutputPoints

            'loop round numops and get the values from the nearest points in array
            For i As Integer = 0 To numOutputPoints - 1
                Dim sp As Integer = (i * skips) + startsample
                If sp > numsamples - 1 Then sp = numsamples - 1 'ensure don't overrun sample array
                If sp < 0 Then sp = 0

                'copy over the samples into the output
                output(idxSecond, i) = samples(idxSecond, sp)
                output(idxVolt, i) = vgain * (samples(idxVolt, sp) - voffset)
            Next i

        ElseIf optionSampling = samplingType.averagenearest Then

            Dim skips As ULong = (numsamples - startsample) / numOutputPoints

            'loop round numops and get the values from the nearest points in array
            For i As Integer = 0 To numOutputPoints - 1
                Dim sp As Integer = (i * skips) + startsample
                If sp > numsamples - 1 Then sp = numsamples - 1 'ensure don't overrun sample array
                If sp < 0 Then sp = 0

                'calculate the sum of this set of samples
                Dim avg_mean As Single
                Dim avg_sum As Single = 0
                Dim avg_count As Integer = 0

                'look at the samples either side of the current sp point, by skips/2
                For j As Integer = -(skips / 2) To (skips / 2) - 1
                    'check that we are within array bounds
                    If (sp + j) < numsamples AndAlso (sp + j) > 0 Then
                        avg_sum += samples(idxVolt, sp + j)
                        avg_count += 1 'and only count those values which have been included in the sum
                        'Debug.Print("sampling at " & sp + j)
                    End If
                Next j

                'calculate the mean for this set of samples
                If avg_count > 0 Then   'prevent div by 0 in case no samples were in bounds
                    avg_mean = avg_sum / avg_count

                    'copy over the samples into the output
                    output(idxVolt, i) = vgain * (avg_mean - voffset)

                End If

                output(idxSecond, i) = samples(idxSecond, sp)

            Next i

        ElseIf optionSampling = samplingType.peakdetect Then

            Dim skips As ULong = (numsamples - startsample) / numOutputPoints

            'loop round numops and get the values from the nearest points in array
            For i As Integer = 0 To numOutputPoints - 1
                Dim sp As Integer = (i * skips) + startsample
                If sp > numsamples - 1 Then sp = numsamples - 1 'ensure don't overrun sample array
                If sp < 0 Then sp = 0

                'calculate the sum of this set of samples
                Dim avg_mean As Single
                Dim avg_sum As Single = 0
                Dim avg_count As Integer = 0
                Dim pk_max As Single = Single.MinValue
                Dim pk_min As Single = Single.MaxValue

                'look at the samples either side of the current sp point, by skips/2
                For j As Integer = -(skips / 2) To (skips / 2) - 1
                    'check that we are within array bounds
                    If (sp + j) < numsamples AndAlso (sp + j) > 0 Then
                        'look for peak max and peak min
                        If samples(idxVolt, sp + j) > pk_max Then pk_max = samples(idxVolt, sp + j)
                        If samples(idxVolt, sp + j) < pk_min Then pk_min = samples(idxVolt, sp + j)

                        'and need to still calc mean so that can see whether min or max is further from it
                        avg_sum += samples(idxVolt, sp + j)
                        avg_count += 1 'and only count those values which have been included in the sum
                    End If
                Next j

                'calculate the mean for this set of samples
                If avg_count > 0 Then   'prevent div by 0 in case no samples were in bounds
                    avg_mean = avg_sum / avg_count

                    'Debug.Print("Sample at " & sp & " avg=" & avg_mean & " pk_max=" & pk_max & " pk_min=" & pk_min)

                    'copy over the samples into the output
                    If (pk_max - avg_mean) > (avg_mean - pk_min) Then   'max is further from avg_mean than min is, so use max for this output point
                        output(idxVolt, i) = vgain * (pk_max - voffset)
                    Else    'min is further from avg_mean than max is, so use min for this output point
                        output(idxVolt, i) = vgain * (pk_min - voffset)
                    End If

                End If

                output(idxSecond, i) = samples(idxSecond, sp)

            Next i
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim sdlg As New SaveFileDialog
        Try
            With sdlg
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                ' Add the default extension, if the user neglects to add an extension.
                .AddExtension = True
                ' Check to verify that the output path actually exists. Prompt before
                ' creating a new file? Prompt before overwriting? 
                .CheckPathExists = True
                .CreatePrompt = False
                .OverwritePrompt = True
                .ValidateNames = True
                .ShowHelp = True
                ' If the user doesn't supply an extension, and if the AddExtension property is
                ' True, use this extension. The default is "".
                .DefaultExt = "fy"
                ' Prompt with the current file name if you've specified it.
                '.FileName = GetDocumentName()
                .Filter = "FeelTech Waveform List (*.fy)|*.fy"
                .FilterIndex = 1 'default is fy
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.Cursor = Cursors.WaitCursor
                    SaveOutput(.FileName)
                    Me.Cursor = Cursors.Default
                End If
            End With
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Application.ProductName)
        End Try

    End Sub
    Private Sub SaveOutput(filename As String)
        Dim fs As String = MakeOutputIntoString()

        Try
            IO.File.WriteAllText(filename, fs)
        Catch ex As Exception
            MsgBox("Error during save " & ex.ToString)
        End Try
    End Sub

    Private Function MakeOutputIntoString() As String
        Dim fs As String = String.Empty
        For i As Integer = 0 To output.GetLength(1) - 1
            fs &= output(idxVolt, i) & Environment.NewLine
        Next i

        Return fs
    End Function

    Private Sub optComplete_CheckedChanged(sender As Object, e As EventArgs) Handles optComplete.CheckedChanged
        If optComplete.Checked Then
            startAtTrig = False

        End If
        If optStartAtTrigger.Checked Then
            startAtTrig = True
        End If
        If inFile <> String.Empty Then
            Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
            FillSampleChart()
        End If

    End Sub

    Private Sub optDecimate_CheckedChanged(sender As Object, e As EventArgs) Handles optDecimate.CheckedChanged
        SamplingModeOption()
    End Sub
    Private Sub optAverageNearest_CheckedChanged(sender As Object, e As EventArgs) Handles optAverageNearest.CheckedChanged
        SamplingModeOption()
    End Sub

    Private Sub optPeakDetect_CheckedChanged(sender As Object, e As EventArgs) Handles optPeakDetect.CheckedChanged
        SamplingModeOption()
    End Sub
    Private Sub SamplingModeOption()
        If optDecimate.Checked Then
            optionSampling = samplingType.decimate
        End If
        If optAverageNearest.Checked Then
            optionSampling = samplingType.averagenearest
        End If
        If optPeakDetect.Checked Then
            optionSampling = samplingType.peakdetect
        End If

        If inFile <> String.Empty Then
            Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
            FillSampleChart()
        End If
    End Sub
    Private Sub ChangeVertOption()
        If optVertNoChange.Checked Then
            optionVertical = verticalType.nochange
        End If
        If optVertRescale01.Checked Then
            optionVertical = verticalType.rescale01
        End If
        If optVertRescaleM1P1.Checked Then
            optionVertical = verticalType.rescalem1p1
        End If
        If inFile <> String.Empty Then
            Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
            FillSampleChart()
        End If
    End Sub

    Private Sub optVertNoChange_CheckedChanged(sender As Object, e As EventArgs) Handles optVertNoChange.CheckedChanged
        ChangeVertOption()
    End Sub

    Private Sub optRescale01_CheckedChanged(sender As Object, e As EventArgs) Handles optVertRescale01.CheckedChanged
        ChangeVertOption()
    End Sub

    Private Sub optVertRescaleM1P1_CheckedChanged(sender As Object, e As EventArgs) Handles optVertRescaleM1P1.CheckedChanged
        ChangeVertOption()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        cmbOutputPoints.SelectedIndex = 0
        bwParseCSV.WorkerReportsProgress = True

    End Sub

    Class cParser
        Private ReadOnly _filename As String
        Public bw As BackgroundWorker
        Public Sub New(f As String)
            _filename = f
        End Sub
        Public Sub OpenParseCSV()

            Dim sw As New Stopwatch
            Dim prevpercent As Single = 0

            sw.Start()
            Dim fs() As String = IO.File.ReadAllLines(_filename)
            sw.Stop()
            'Debug.Print("read all lines " & sw.ElapsedMilliseconds)
            Dim endofheader As Boolean = False
            Dim sampleI As ULong

            'There are two different CSV formats produced by Siglent.
            'One has a 6 line header starting with Record Length,Analog:number
            'that is produced when you convert a bin to csv using their bin2csv program.
            'The other has 2 line header which does not state the record length
            'The header lines are
            'Source,CHx
            'Second,Volt
            'that is producted when you use on the on scope menu to save as CSV.
            'also in some cases the number of lines does not match the record length promised, so it is best to use the number of lines in the file minus the header
            'the simplest approach is to ignore the headers and just look for the numbers to start

            'first pass to find the number of lines starting with a number
            Dim countNumericLines As Integer = 0

            For Each ln As String In fs
                If ln.StartsWith("+") OrElse ln.StartsWith("-") OrElse ln.StartsWith(" 0") OrElse ln.StartsWith("0") Then
                    countNumericLines += 1
                    endofheader = True
                ElseIf endofheader Then 'drop out if second header section found
                    Exit For
                End If
            Next ln

            numsamples = countNumericLines
            ReDim samples(1, numsamples)
            sampleI = 0
            bw.ReportProgress(0)

            'second pass to extract the samples
            For Each ln As String In fs
                Dim ls() As String = ln.Split(",")

                'parse sample line, eg
                '-0.00070000000, 0.29600
                Dim s() As String = ln.Split(",")
                'the header lines have length 2 but not both items are numeric
                If s.Length = 2 AndAlso IsNumeric(s(0)) AndAlso IsNumeric(s(1)) Then
                    samples(idxSecond, sampleI) = Trim(s(idxSecond))
                    samples(idxVolt, sampleI) = Trim(s(idxVolt))
                    sampleI += 1

                    'only report when changed by significant amount so that reporting isn't taking up too much CPU time
                    Dim percent As Single = 100 * sampleI / numsamples
                    If percent - prevpercent >= 2 AndAlso percent <= 100 Then 'changed by 2%
                        bw.ReportProgress(percent)
                        'Debug.Print("percent " & percent)
                        prevpercent = CInt(percent)
                    End If

                    If sampleI > numsamples Then
                        Exit For
                    End If
                End If
            Next ln
            sw.Stop()
            'Debug.Print("parsing took " & sw.ElapsedMilliseconds)
        End Sub
    End Class

    Private Sub CopyToClipboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToClipboardToolStripMenuItem.Click
        Dim fs As String = MakeOutputIntoString()

        Try
            My.Computer.Clipboard.Clear()
            My.Computer.Clipboard.SetText(fs)
        Catch ex As Exception
            MsgBox("Could not copy to clipboard. " & ex.ToString)
        End Try
    End Sub

    Private Sub cmbOutputPoints_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOutputPoints.SelectedIndexChanged
        If cmbOutputPoints.SelectedIndex > 0 Then
            Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
            FillSampleChart()
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show(Application.ProductName & " Version " & Application.ProductVersion, Application.ProductName)
    End Sub

    Private Sub GuideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GuideToolStripMenuItem.Click
        Const helpfile As String = "SigFyCSVUserGuide.pdf"

        If My.Computer.FileSystem.FileExists(helpfile) Then
            Try
                Process.Start(helpfile)

            Catch ex As Exception
                MessageBox.Show("Could not open User Guide at " & helpfile, Application.ProductName)
            End Try
        Else
            MessageBox.Show("Could not find User Guide at " & helpfile, Application.ProductName)
        End If

    End Sub
End Class
