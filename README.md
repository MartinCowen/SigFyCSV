# SigFyCSV
Converter of CSV files from Siglent Oscilloscopes to list of samples format used by FeelElec FY6900 Arbitrary Waveform Generator

For example, the file from the scope might look like this
Record Length,Analog:14000
Sample Rate,100000000.0
Vertical Scale,CH1: 0.05
Vertical Offset,CH1:0.00000
Horizontal Scale, 0.000010000
Second,Volt
-0.00007000000,-0.19400
-0.00006999000,-0.19200
-0.00006998000,-0.19200
-0.00006997000,-0.19400
-0.00006996000,-0.19400
-0.00006995000,-0.19600
-0.00006994000,-0.19800

but the FY6900 only needs to see the voltages, not the time points, so it requires a list like this
-0.194
-0.192
-0.194
-0.198
-0.196
-0.198
-0.2
-0.198

However, it is more complicated than that because the FY6900 has a maximum number of sample points of 8192 but the Siglent SDS 1104X-E can export 14 millions points.
So this program is needed to downsample and has some other useful options, see the SigFyCSVUserGuide.docx/pdf

To run this program, you need the files in the Release folder at
https://github.com/MartinCowen/SigFyCSV/tree/master/SigFyCSV/bin/x86/Release
There is no installer but it will on run on up to date Windows 10 systems.

To build this program from source:
This is a VB.NET desktop application (WinForms) built using Visual Studio Community 2019, with the .NET Framework 4.8.03752 and LiveCharts from https://lvcharts.net/ (MIT licenced).
