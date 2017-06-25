@echo off
@cls
@echo.
@echo -- INSTALL UMBRACO RELEASE PACKAGE--
@echo.

:: INITIALISE
	
	@set PACKAGE_SOURCE=E:\GAM.COM.RELEASE\SOURCE\PACKAGE\
	@set PACKAGE_TARGET=E:\GAM.COM.UMBRACO\

:: STOP IIS, TO PREVENT CACHE CORRUPTIONS OF UMBRACO

	net stop w3svc
	net stop iisadmin

:: TRANSFER UMBRACO FILES

	@echo.
	@echo -- Copying Umbraco Package 
	@echo    %PACKAGE_SOURCE% to %PACKAGE_TARGET%
	@echo.
	
	robocopy %PACKAGE_SOURCE% %PACKAGE_TARGET% /S /NFL /NDL /XO 

:: START IIS 

	net start iisadmin
	net start w3svc

	@echo.