@echo off

REM This is the directory where the eShopCONNECT Service dlls have been placed

set SERVICE_HOME=D:\Interprise\LerrynImportExport\eShopCONNECT\Server\Lerryn.WindowsService.eShopCONNECT\bin\Release\

REM ---------------------------------------------------------------------------------------------------

REM This is the file name of the eShopCONNECT Service exe - it should not need to be changed

set SERVICE_EXE=Lerryn.WindowsService.eShopCONNECT.exe

REM ---------------------------------------------------------------------------------------------------

REM This is the internal Windows Service Name - It must match the internal name of the service to be un-installed
REM but MUST NOT contain any spaces

set SERVICE_NAME=eShopCONNECT-Test

REM ---------------------------------------------------------------------------------------------------

REM the following directory is for .NET 4 i.e. IS 6.x  Change to C:\Windows\Microsoft.NET\Framework\v2.0.50727\ for IS 5.6.xx

set INSTALL_UTIL_HOME=C:\Windows\Microsoft.NET\Framework\v4.0.30319\

REM ---------------------------------------------------------------------------------------------------

set PATH=%PATH%;%INSTALL_UTIL_HOME%

cd %SERVICE_HOME%

echo Uninstalling Service...

installutil /u /name=%SERVICE_NAME% %SERVICE_EXE%

echo Done.

PAUSE