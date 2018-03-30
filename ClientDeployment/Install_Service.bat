@echo off

REM This is the directory where the eShopCONNECT Service dlls have been placed

set SERVICE_HOME=D:\Interprise\LerrynImportExport\eShopCONNECT\Server\Lerryn.WindowsService.eShopCONNECT\bin\Debug\

REM ---------------------------------------------------------------------------------------------------

REM This is the file name of the eShopCONNECT Service exe - it should not need to be changed

set SERVICE_EXE=Lerryn.WindowsService.eShopCONNECT.exe

REM ---------------------------------------------------------------------------------------------------

REM This is the internal Windows Service Name - It should ideally be a combination of eShopCONNECT and the user company, 
REM but MUST NOT contain any spaces

set SERVICE_NAME=eShopCONNECT-Test

REM ---------------------------------------------------------------------------------------------------

REM Start of code added TJS 17/02/12
REM This is the displayed Windows Service Display Name - It should ideally be a combination of eShopCONNECT and the user company 
REM and CAN contain spaces

set SERVICE_DISPLAYNAME=eShopCONNECT Test

REM ---------------------------------------------------------------------------------------------------
REM End of code added TJS 17/02/12

REM This is the displayed Windows Service Description - it should not need to be changed
REM and CAN contain spaces

REM set SERVICE_DESC=eShopCONNECT description

REM ---------------------------------------------------------------------------------------------------

REM the following directory is for .NET 4 i.e. IS 6.x  Change to C:\Windows\Microsoft.NET\Framework\v2.0.50727\ for IS 5.6.xx

set INSTALL_UTIL_HOME=C:\Windows\Microsoft.NET\Framework\v4.0.30319\

REM ---------------------------------------------------------------------------------------------------

REM Account credentials if the service uses a user account - NOTE need to change the installutil call below if you enable this
REM set ACCOUNT_TYPE=<account type>
REM set USER_NAME=<user account>
REM set PASSWORD=<user password>

REM ---------------------------------------------------------------------------------------------------

set PATH=%PATH%;%INSTALL_UTIL_HOME%

cd %SERVICE_HOME%

echo Installing Service...


REM use this call to InstallUtil if you have set the user account credentials above and disable the call below

REM installutil /name=%SERVICE_NAME% /displayname"=%SERVICE_DISPLAYNAME%" /desc="%SERVICE_DESC%" /account=%ACCOUNT_TYPE% /user=%USER_NAME% /password=%PASSWORD% %SERVICE_EXE%


REM This is the defauult call to InstallUtil - disable this call and enable the call above if you have set the user account credentials above

installutil /name=%SERVICE_NAME% /displayname"=%SERVICE_DISPLAYNAME%" /desc="%SERVICE_DESC%" %SERVICE_EXE% 


echo Done.

PAUSE