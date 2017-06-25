@echo off
@cls
@echo.
@echo -- BACKUP UMBRACO --
@echo.

:: INITIALISE

	@set BACKUP_ROOT=E:\GAM.COM.RELEASE\BACKUP\DBxfer\
	@rem set SAVE_TARGET=\\tsclient\C\gam.com.udeploy\dbdev\
	@rem Dev only - running script locally not via a RDP connection - so \\tsclient not relevant.
	@set SAVE_TARGET=C:\gam.com.udeploy\dbdev\


	@set SQL_SERVERNAME=.
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

:: TRANSFER TO C:

	@echo.
	@echo -- Copying SQL Backup to Client C drive
	@echo    %SQL_SERVERNAME%.%SQL_DATABASENAME% to %SAVE_TARGET%
	@echo.
	
	XCOPY %SQL_BACKUP_FILENAME% %SAVE_TARGET% /R /Y

	@echo.