@echo off
cd /d "%~dp0"
echo Starte Audio-Konvertierung...
python konverter.py
echo.
echo --- Prozess beendet! ---
pause
