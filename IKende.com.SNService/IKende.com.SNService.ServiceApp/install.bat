echo install
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
echo Installing ikende sn server to winservice...
echo ---------------------------------------------------
InstallUtil /i IKende.com.SNService.ServiceApp.exe
echo ---------------------------------------------------
echo Done.