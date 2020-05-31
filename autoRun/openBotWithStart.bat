start "" "%~dp0..\server\server\bin\Debug\server.exe" -auto

start "" "%~dp0..\BotForm\BotForm\bin\Debug/BotForm.exe" -auto 127.0.0.1
TIMEOUT /T 1 /NOBREAK
start "" "%~dp0..\BotForm\BotForm\bin\Debug/BotForm.exe" -auto 127.0.0.1