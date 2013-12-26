echo uninstall
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
echo UnInstalling ikende sn server...
echo ---------------------------------------------------
InstallUtil /u IKende.com.SNService.ServiceApp.exe
echo ---------------------------------------------------
echo Done.