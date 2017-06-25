echo off
cls
echo.
echo -- BACKUP UMBRACO --
echo.

:: INITIALISE

	set BACKUP_ROOT=E:\GAM.COM.RELEASE\BACKUP\
	set SQL_SERVERNAME=.
	set SQL_DATABASENAME=gam.com.umbraco
	set UMBRACO_ROOT_FOLDER=E:\GAM.COM.UMBRACO

:: GENERATE THE NAME OF THE BACKUP_FOLDER FROM THE ROOT PLUS THE CURRENT DATETIME

	@rem Get Date and Time e.g. YYYY-MM-DD_hh.mm.ss
    @for /f "tokens=1-3 delims=/- " %%a in ('date /t') do set XDate=%%c-%%b-%%a

    @for /f "tokens=1-3 delims=:. " %%a in ('echo %time%') do set XTime=%%a.%%b.%%c
    @set wholedate=%XDate%_%XTime%
	
	set BACKUP_FOLDER=%BACKUP_ROOT%%wholedate%\
	set BACKUP_MEDIA_FOLDER=%BACKUP_ROOT%\media\
	set UMBRACO_BACKUP_FOLDER=%BACKUP_FOLDER%GAM.COM.UMBRACO\

	set SQL_BACKUP_FILENAME=%BACKUP_FOLDER%gam.com.umbraco.bak


:: COPY UMBRACO FOLDER TO BACKUP FOLDER EXCLUDING MEDIA AND TEMP FILES
:: a single rolling media archive is updated with the current media content

	echo -- Updating %BACKUP_MEDIA_FOLDER% with latest media
	@robocopy %UMBRACO_ROOT_FOLDER%\media %BACKUP_MEDIA_FOLDER% /S /NFL /NDL /NJH /XO 
	
	echo.
	echo.
	echo -- Backing up %UMBRACO_ROOT_FOLDER% to %UMBRACO_BACKUP_FOLDER%
	echo robocopy %UMBRACO_ROOT_FOLDER%\ %UMBRACO_BACKUP_FOLDER% /S /NFL /NDL /NJH /XO /XD %UMBRACO_ROOT_FOLDER%\app_data\TEMP %UMBRACO_ROOT_FOLDER%\media
	@robocopy %UMBRACO_ROOT_FOLDER%\ %UMBRACO_BACKUP_FOLDER% /S /NFL /NDL /NJH /XO /XD %UMBRACO_ROOT_FOLDER%\app_data\TEMP %UMBRACO_ROOT_FOLDER%\media
	
	echo.
	echo.------------------------------------------------------------------------------
	echo.
	
:: SQL BACKUP


	echo.
	echo -- Backing up SQL Database
	echo    %SQL_SERVERNAME% %SQL_DATABASENAME% to %SQL_BACKUP_FILENAME%
	echo.

	sqlcmd -E -S %SQL_SERVERNAME% -d master -Q "BACKUP DATABASE [%SQL_DATABASENAME%] TO DISK = N'%SQL_BACKUP_FILENAME%' WITH INIT , NOUNLOAD , NAME = N'%SQL_DATABASENAME% backup', NOSKIP , STATS = 10, NOFORMAT"
	echo.
rem pause
