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
    Dim endsample As Integer
    Dim startAtTrig As Boolean
    Enum samplingType
        decimate
        averagenearest

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
                    bwParseCSV.RunWorkerAsync(args)
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

        bwParseCSV.WorkerReportsProgress = True
        ProgressBar1.Visible = True
        Dim mParser As New cParser(args.filename)
        mParser.bw = bwParseCSV
        mParser.OpenParseCSV()
        'OpenParseCSV(args.filename)
    End Sub

    Private Sub bwParseCSV_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles bwParseCSV.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        If e.ProgressPercentage = 100 Then
            ProgressBar1.Visible = False

        End If
    End Sub

    Private Sub bwParseCSV_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles bwParseCSV.RunWorkerCompleted
        'ProgressBar1.Visible = False
        lblLines.Text = "Lines: " & numsamples
        Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))
        FillSampleChart()
    End Sub




    Private Sub FillSampleChart()
        crtSamples.DisableAnimations = True
        crtSamples.Hoverable = False

        crtSamples.Series.Clear()

        Dim scol As New SeriesCollection
        Dim scatters As New LineSeries
        scatters.Title = "Samples"
        scatters.Values = New ChartValues(Of ObservablePoint)()
        scatters.PointGeometrySize = 1
        scatters.Fill = Windows.Media.Brushes.Transparent
        scatters.Stroke = Windows.Media.Brushes.DarkBlue


        'scale to size of plot on screen, no point trying to plot more points than are visible
        Dim skips As Integer = output.GetLength(1) / crtSamples.Width
        For i As Integer = 0 To crtSamples.Width - 1
            Dim sp As Integer = i * skips
            If sp > output.GetLength(1) - 1 Then sp = output.GetLength(1) - 1
            scatters.Values.Add(New ObservablePoint(output(idxSecond, sp), output(idxVolt, sp)))
        Next i

        scatters.DataLabels = False
        scol.Add(scatters)
        crtSamples.Series = scol

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
                output(idxSecond, i) = samples(idxSecond, i)
                output(idxVolt, i) = vgain * (samples(idxVolt, sp) - voffset)
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
        Dim fs As String = String.Empty
        For i As Integer = 0 To output.GetLength(1) - 1
            fs &= output(idxVolt, i) & Environment.NewLine
        Next i

        Try
            IO.File.WriteAllText(filename, fs)
        Catch ex As Exception
            MsgBox("Error during save " & ex.ToString)
        End Try
    End Sub

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
        If optDecimate.Checked Then
            optionSampling = samplingType.decimate
        End If
        If optAverageNearest.Checked Then
            optionSampling = samplingType.averagenearest

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

    End Sub

    Class cParser
        Private _filename As String
        'Private samples(1, 0) As Single
        'Private numsamples As Integer
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
            Debug.Print("read all lines " & sw.ElapsedMilliseconds)
            Dim endofheader As Boolean = False
            Dim sampleI As ULong
            For Each ln As String In fs
                Dim ls() As String = ln.Split(",")
                If ls(1).StartsWith("Analog:") Then
                    'Record Length,Analog:1400000
                    Dim l1() As String = ls(1).Split(":")
                    If l1.Length > 0 Then
                        numsamples = ls(1).Split(":")(1)
                        ReDim samples(1, numsamples)
                        sampleI = 0
                        bw.ReportProgress(0)
                    End If
                End If
                'test endofheader before setting true so that only lines after endofheader are analysed
                If endofheader Then
                    'parse sample line, eg
                    '-0.00070000000, 0.29600
                    Dim s() As String = ln.Split(",")
                    If s.Length = 2 Then
                        samples(idxSecond, sampleI) = Trim(s(idxSecond))
                        samples(idxVolt, sampleI) = Trim(s(idxVolt))
                        sampleI += 1
                        Dim percent As Single = 100 * sampleI / numsamples
                        If percent - prevpercent > 2 AndAlso percent <= 100 Then 'changed by 2%
                            bw.ReportProgress(percent)
                            prevpercent = percent
                        End If
                        If sampleI > numsamples Then

                            Exit For
                        End If
                    End If
                End If
                If ls(0) = "Second" Then 'the line before the data starts is "Second,Volt"
                    endofheader = True
                    sw.Reset()
                    sw.Start()
                End If
            Next ln
            sw.Stop()
            Debug.Print("parsing " & sw.ElapsedMilliseconds)

        End Sub
    End Class
End Class
