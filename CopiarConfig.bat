@echo off
echo Copiando App.config actualizado a bin\Debug...
copy /Y "View\UI\App.config" "View\UI\bin\Debug\UI.exe.config"
echo.
echo App.config copiado exitosamente!
echo.
pause
