@echo off
@cls
@echo.

    @set UMBRACO_ROOT_FOLDER=E:\GAM.COM.UMBRACO
	@rem set SAVE_TARGET=\\tsclient\C\gam.com.udeploy\dev\
	@rem dev only
	@set SAVE_TARGET=C:\gam.com.udeploy\dev

@echo -- SAVE UMBRACO CODE BASE TO %SAVE_TARGET% --
@echo.

	@robocopy %UMBRACO_ROOT_FOLDER%\ %SAVE_TARGET% /S /NFL /NDL /XO /XD %UMBRACO_ROOT_FOLDER%\app_data
	
@rem pause
