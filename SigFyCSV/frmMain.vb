Public Class frmMain
    Dim inFile As String
    Dim numsamples As Integer
    Dim samples() As Single
    Dim output() As Single
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
                    ReDim samples(numsamples)
                    sampleI = 0
                End If
            End If
            'test endofheader before setting true so that only lines after endofheader are analysed
            If endofheader Then
                'parse sample line, eg
                '-0.00070000000, 0.29600
                Dim s() As String = ln.Split(",")
                If s.Length = 2 Then
                    samples(sampleI) = Trim(s(1))
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
        Convert()

    End Sub

    Private Sub Convert()
        ReDim output(numsamples - 1)
        For i As Integer = 0 To numsamples - 1
            output(i) = samples(i)
        Next i
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
End Class
