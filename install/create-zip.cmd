set zippath=C:\Users\N.Polyagoshko\Documents\Bitbucket\AMQP_Exchange\install\

set fdate=%date:~-4%_%date:~3,2%_%date:~0,2%
set ftime=%time:~0,2%%time:~3,2%
if "%ftime:~0,1%" == " " set ftime=0%ftime:~1,3%

cd %zippath%
mkdir tmp
copy /b /y ..\AMQP_Exchange\bin\Release\*.dll %zippath%\tmp
copy /b /y ..\AMQP_Exchange\bin\Release\*.exe %zippath%\tmp
copy /b /y ..\AMQP_Exchange\bin\Release\*.config %zippath%\tmp
copy /b /y install.cmd %zippath%\tmp
copy /b /y db_create.sql %zippath%\tmp

cd tmp
..\7z a amqp_ex.zip *

cd ..
mkdir %fdate%_%ftime%
move /y tmp\amqp_ex.zip %fdate%_%ftime%

del /f /q tmp\*
