start "" "%~dp0..\server\server\bin\Debug\server.exe" -auto
TIMEOUT /T 0.2 /NOBREAK
start "" "%~dp0..\BotForm\BotForm\bin\Debug/BotForm.exe" -auto 127.0.0.1
TIMEOUT /T 0.2 /NOBREAK
start "" "%~dp0..\BotForm\BotForm\bin\Debug/BotForm.exe" -auto 127.0.0.1