@echo off
@cls
@echo.
@echo -- BACKUP UMBRACO --
@echo.

:: INITIALISE

	@rem Get Date and Time e.g. YYYY-MM-DD_hh.mm.ss
    @for /f "tokens=1-3 delims=/- " %%a in ('date /t') do set XDate=%%c-%%b-%%a

    @for /f "tokens=1-3 delims=:. " %%a in ('echo %time%') do set XTime=%%a.%%b.%%c
    @set wholedate=%XDate%_%XTime%
	
	@set BACKUP_ROOT=\\Ldn13937pc\GAM.COM.RELEASE.GR\BACKUP\DBxfer\%wholedate%\
	@set SQL_SERVERNAME=LDN13936PC\SQL2008R2
	@set SQL_DATABASENAME=gam.com.umbraco
	@set SQL_BAK=gam.com.umbraco.bak

	@set SQL_BACKUP_FILENAME=%BACKUP_ROOT%%SQL_BAK%

:: REMOVE OLD BAK FILE (quietly)

	del %SQL_BACKUP_FILENAME% /F /Q 2>NUL
	mkdir %BACKUP_ROOT% 2>NUL
	
:: SQL BACKUP

	@echo.
	@echo -- Backing up SQL Database
	@echo    %SQL_SERVERNAME% %SQL_DATABASENAME% to %SQL_BACKUP_FILENAME%
	@echo.

	sqlcmd -E -S %SQL_SERVERNAME% -d master -Q "BACKUP DATABASE [%SQL_DATABASENAME%] TO DISK = N'%SQL_BACKUP_FILENAME%' WITH INIT , NOUNLOAD , NAME = N'%SQL_DATABASENAME% backup', NOSKIP , STATS = 10, NOFORMAT"
	@echo.

