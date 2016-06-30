@ECHO OFF

IF NOT EXIST ..\RPGMakerDecrypter.Cli\bin\Release GOTO NOTCOMPILED
IF NOT EXIST ..\RPGMakerDecrypter.Gui\bin\Release GOTO NOTCOMPILED

IF NOT EXIST RPGMakerDecrypter MKDIR RPGMakerDecrypter

COPY ..\RPGMakerDecrypter.Gui\bin\Release\RPGMakerDecrypter.exe RPGMakerDecrypter
COPY ..\RPGMakerDecrypter.Gui\bin\Release\RPGMakerDecrypter.Decrypter.dll RPGMakerDecrypter
COPY ..\RPGMakerDecrypter.Gui\bin\Release\RPGMakerDecrypter.exe.config RPGMakerDecrypter
COPY ..\RPGMakerDecrypter.Cli\bin\Release\RPGMakerDecrypter-cli.exe RPGMakerDecrypter
COPY ..\RPGMakerDecrypter.Cli\bin\Release\RPGMakerDecrypter-cli.exe.config RPGMakerDecrypter
COPY ..\RPGMakerDecrypter.Cli\bin\Release\CommandLine.dll RPGMakerDecrypter

IF NOT EXIST release MKDIR release

7za a -tzip release\RPGMakerDecrypter.zip RPGMakerDecrypter

RMDIR /Q /S RPGMakerDecrypter

GOTO EXIT
   
:NOTCOMPILED
ECHO Solution needs to be compiled in Release mode for deploy.

:EXIT