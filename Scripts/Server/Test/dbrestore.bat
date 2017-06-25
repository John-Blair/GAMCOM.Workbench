@echo off
@cls
@echo.
@echo -- RESTORE UMBRACO DB --
@echo.

:: INITIALISE

	@set SQL_BAK_FILE=E:\GAM.COM.RELEASE\SOURCE\GAM.COM.UMBRACO.BAK
	@set SQL_SERVER=.
	@set SQL_DATABASE=gam.com.umbraco
	
:: SQL RESTORE

	@echo.
	@echo -- Restoring SQL Database
	@echo    %SQL_BAK_FILE% to %SQL_SERVER%.%SQL_DATABASE% 
	@echo.

	sqlcmd -E -S %SQL_SERVER% -d master -Q "ALTER DATABASE [%SQL_DATABASE%] SET OFFLINE WITH ROLLBACK IMMEDIATE"

	sqlcmd -E -S %SQL_SERVER% -d master -Q "RESTORE DATABASE [%SQL_DATABASE%] FROM DISK = N'%SQL_BAK_FILE%' WITH FILE = 1, MOVE N'gam.com.umbraco' TO N'E:\Microsoft SQL Server\MSSQL.1\MSSQL\Data\gam.com.umbraco.mdf', MOVE N'gam.com.umbraco_log' TO N'E:\Microsoft SQL Server\MSSQL.1\MSSQL\Data\gam.com.umbraco_log.ldf', NOUNLOAD, REPLACE, STATS = 10"

	sqlcmd -E -S %SQL_SERVER% -d master -Q "ALTER DATABASE [%SQL_DATABASE%] SET ONLINE"	


:: IIS RESET

	@echo.
	@echo -- Resetting IIS
	@echo    To prevent umbraco cache problems
	@echo.

	iisreset

	@echo.
	

