mkdir package\lib\net40
copy ..\RawInput\bin\Debug\RawInput.dll package\lib\net40
NuGet.EXE pack package\RawInput.nuspec