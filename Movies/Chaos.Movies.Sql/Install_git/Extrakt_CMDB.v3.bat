@echo off
set DatabaseName=CMDB
set DatabaseFolder=CMDB

set NoQuestion=%2
set NoPause=%1
call C:\GainIT_SVN\Interna_verktyg\Tools\ScriptToOne.v3\trunk\ScriptToOne.v3\RunExtraktdatabase.v3.bat %DatabaseName% %DatabaseFolder% %NoQuestion%
if "%NoPause%"!==!"1" (GOTO END)
pause
:END
pause