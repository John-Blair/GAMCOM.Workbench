@echo off
@cls
@echo.

    @set UMBRACO_ROOT_FOLDER=E:\GAM.COM.UMBRACO
	@set SAVE_TARGET=\\tsclient\C\gam.com.udeploy\dr\

@echo -- SAVE UMBRACO CODE BASE TO %SAVE_TARGET% --
@echo.

	@robocopy %UMBRACO_ROOT_FOLDER%\ %SAVE_TARGET% /S /NFL /NDL /XO /XD %UMBRACO_ROOT_FOLDER%\app_data
	
@rem pause
