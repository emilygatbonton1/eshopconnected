@echo off
@ECHO -----------------------------------------------------------------
@ECHO This console utility will register your installed plugin assembly
@ECHO on a target company database using the configuration tool and
@ECHO the plugin manager from Connected Business.
@ECHO.
@ECHO The registration process may take a while so please be patient.
@ECHO -----------------------------------------------------------------
@ECHO.

@ECHO If you wish to proceed with the registration please press any key to continue
@ECHO otherwise press Ctrl+C or Ctrl+Break and terminate this utility.
@ECHO.
pause
@ECHO.

set pluginManagerAppConfig="%~dp0Interprise.Presentation.Utility.PluginManager.exe.config"
set pluginAssembly="%*"
set pause=true

@ECHO Resetting the plugin manager configuration file . . .
Interprise.Console.Utility.AppConfig.exe -Path %pluginManagerAppConfig% -Res
@if errorlevel 1 goto :error

@ECHO Opening the configuration tool . . .
Interprise.Presentation.Utility.AppConfig.exe -Edit -ConfigFile %pluginManagerAppConfig%
@if errorlevel 1 goto :error
@ECHO.

@ECHO Registering the plugin assembly . . .
Interprise.Presentation.Utility.PluginManager.exe %pluginManagerAppConfig% -r %pluginAssembly%
@if errorlevel 1 goto :error
@ECHO.

@ECHO Registration complete !
@ECHO.
@goto :exit

:error
@ECHO An error occured in registerpluginassembly.bat - %errorLevel%
@ECHO.
@ECHO For manual registration you may use Connected Business.  Just open the Plugin Manager form
@ECHO from the System Manager menu once you have signed in to your company database and click
@ECHO the Add Plugin button on the toolbar menu.  This will show a dialog box to locate the
@ECHO plugin assembly that you wish to register.
@ECHO.
if %pause%==true PAUSE
@exit errorLevel

:exit
if %pause%==true PAUSE

set pluginManagerAppConfig=
set pluginAssembly=
set pause=

echo on