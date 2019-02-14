cd %~dp0
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe AMQP_Exchange.exe
sqlcmd -E -i db_create.sql -o db_create.log
