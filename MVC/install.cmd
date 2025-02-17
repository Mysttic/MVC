@echo off
setlocal EnableDelayedExpansion
cd /d %~dp0

REM Find the .exe file matching the pattern "MVC.*Listener.exe"
set "EXE_NAME="
for %%F in ("MVC.*Listener.exe") do (
    set "EXE_NAME=%%~nxF"
    goto :found
)
echo No file matching pattern MVC.*Listener.exe found.
pause
exit /b 1

:found
REM Derive SERVICE_NAME – remove ".exe" and append "Service"
set "SERVICE_NAME=!EXE_NAME:~0,-4!Service"
echo Found file: %EXE_NAME%
echo Assumed SERVICE_NAME: %SERVICE_NAME%

REM Prompt for the startup parameter (sensitive data; the value will not be displayed)
set /p "SERVICE_PARAM=Enter startup parameter for the service (sensitive, will not be displayed): "

REM Check if the service already exists; if so, stop and delete it
sc query "%SERVICE_NAME%" >nul 2>&1
if %errorlevel%==0 (
    echo Service %SERVICE_NAME% already exists. Stopping and deleting...
    sc stop "%SERVICE_NAME%"
    timeout /t 5 /nobreak >nul
    sc delete "%SERVICE_NAME%"
    timeout /t 5 /nobreak >nul
) else (
    echo Service %SERVICE_NAME% does not exist.
)

REM Create the service using the full path to the file
echo Installing service %SERVICE_NAME% from %EXE_NAME%
sc create "%SERVICE_NAME%" binPath= "\"%~dp0%EXE_NAME%\"" start= auto

REM Start the service with the provided parameter (the parameter value is not displayed)
echo Starting service %SERVICE_NAME% with parameter.
sc start "%SERVICE_NAME%" %SERVICE_PARAM%

echo Service %SERVICE_NAME% has been installed and started.
pause
