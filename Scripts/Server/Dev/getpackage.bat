@echo off
@cls
@echo.
@echo -- GET UMBRACO INSTALL PACKAGE--
@echo.

:: INITIALISE
	
	@rem set PACKAGE_SOURCE=\\tsclient\C\gam.com.udeploy\package\
	@rem Dev only.
	@set PACKAGE_SOURCE=C:\gam.com.udeploy\package\
	@set PACKAGE_TARGET=E:\GAM.COM.RELEASE\SOURCE\PACKAGE

:: TRANSFER FROM C:

	@echo.
	@echo -- Copying Umbraco Package 
	@echo    %PACKAGE_SOURCE% to %PACKAGE_TARGET%
	@echo.
	
	@robocopy %PACKAGE_SOURCE% %PACKAGE_TARGET% /S /NFL /NDL /MIR 

	@echo.