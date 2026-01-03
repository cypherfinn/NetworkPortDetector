@echo off
echo Building Network Port Detector as portable .exe...
echo.

REM Restore packages
dotnet restore

REM Build portable single-file executable
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

echo.
echo Build complete!
echo.
echo Portable .exe location:
echo bin\Release\net8.0-windows\win-x64\publish\NetworkPortDetector.exe
echo.
pause
