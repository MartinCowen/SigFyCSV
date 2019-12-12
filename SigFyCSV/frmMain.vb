Public Class frmMain
    Dim inFile As String
    Dim numsamples As Integer
    Dim samples(2, 0) As Single
    Dim output() As Single
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
                    OpenParseCSV(.FileName)
                    lblLines.Text = "Lines: " & numsamples
                    Me.Cursor = Cursors.Default
                End If
            End With
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, Application.ProductName)
        End Try
    End Sub

    Private Sub OpenParseCSV(filename As String)
        Dim fs() As String = IO.File.ReadAllLines(filename)
        Dim endofheader As Boolean = False
        Dim sampleI As ULong
        For Each ln As String In fs
            Dim ls() As String = ln.Split(",")
            If ls(1).StartsWith("Analog:") Then
                'Record Length,Analog:1400000
                Dim l1() As String = ls(1).Split(":")
                If l1.Length > 0 Then
                    numsamples = ls(1).Split(":")(1)
                    ReDim samples(2, numsamples)
                    sampleI = 0
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
                    If sampleI > numsamples Then Exit For
                End If
            End If
            If ls(0) = "Second" Then 'the line before the data starts is "Second,Volt"
                endofheader = True
            End If
        Next ln
    End Sub

    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        Convert(CInt(cmbOutputPoints.Items(cmbOutputPoints.SelectedIndex).ToString))

    End Sub

    Private Sub Convert(numOps As Integer)

        ReDim output(numOps - 1)
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

        If optionSampling = samplingType.decimate Then

            'precalculate this outside the loop to avoid doing the division inside the loop
            Dim skips As ULong = (numsamples - startsample) / numOps

            'loop round numops and get the values from the nearest points in array
            For i As Integer = 0 To numOps - 1
                Dim sp As Integer = (i * skips) + startsample
                If sp > numsamples - 1 Then sp = numsamples - 1 'ensure dont overrun sample array
                If sp < 0 Then sp = 0

                'copy over the samples into the output
                output(i) = samples(idxVolt, sp)
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
        For Each sg As Single In output
            fs &= sg & Environment.NewLine
        Next sg

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
    End Sub

    Private Sub optDecimate_CheckedChanged(sender As Object, e As EventArgs) Handles optDecimate.CheckedChanged
        If optDecimate.Checked Then
            optionSampling = samplingType.decimate
        End If
        If optAverageNearest.Checked Then
            optionSampling = samplingType.averagenearest

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
End Class
