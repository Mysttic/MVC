@echo off
setlocal EnableDelayedExpansion
cd /d %~dp0

REM Wyszukaj plik .exe spełniający wzorzec "MVC.*Listener.exe"
set "EXE_NAME="
for %%F in ("MVC.*Listener.exe") do (
    set "EXE_NAME=%%~nxF"
    goto :found
)
echo Nie znaleziono pliku spełniającego wzorzec MVC.*Listener.exe
pause
exit /b 1

:found
REM Wylicz SERVICE_NAME – usuwamy ".exe" i dopisujemy "Service"
set "SERVICE_NAME=!EXE_NAME:~0,-4!Service"
echo Znaleziono plik: %EXE_NAME%
echo Ustawiono SERVICE_NAME: %SERVICE_NAME%

REM Pobranie parametru uruchomieniowego (dane wrażliwe – nie wyświetlamy wartości)
set /p "SERVICE_PARAM=Podaj parametr uruchomieniowy dla usługi (dane wrażliwe, nie będą wyświetlone): "

REM Sprawdzenie, czy usługa już istnieje; jeśli tak, zatrzymaj i usuń ją
sc query "%SERVICE_NAME%" >nul 2>&1
if %errorlevel%==0 (
    echo Usługa %SERVICE_NAME% istnieje. Zatrzymywanie i usuwanie...
    sc stop "%SERVICE_NAME%"
    timeout /t 5 /nobreak >nul
    sc delete "%SERVICE_NAME%"
    timeout /t 5 /nobreak >nul
) else (
    echo Usługa %SERVICE_NAME% nie istnieje.
)

REM Instalacja usługi – używamy pełnej ścieżki do pliku
echo Instalacja usługi %SERVICE_NAME% z aplikacji %EXE_NAME%
sc create "%SERVICE_NAME%" binPath= "\"%~dp0%EXE_NAME%\"" start= auto

REM Uruchomienie usługi z przekazaniem parametru (wartość parametru nie jest wyświetlana)
echo Uruchamianie usługi %SERVICE_NAME% z przekazaniem parametru.
sc start "%SERVICE_NAME%" %SERVICE_PARAM%

echo Usługa %SERVICE_NAME% została zainstalowana i uruchomiona.
pause
