# RPG Maker Decrypter

RPG Maker Decrypter can be used to extract encrypted archives created with RPG Maker XP, RPG Maker VX or RPG Maker VX Ace.
Software can also generate correct type project file for decrypted data.

## Usage

RPG Maker Decrypter has Windows application and cli application which both provide same functionality
but with different interface.

### RPG Maker Decrypter CLI

Using RM2k2XP from commandline is simple. To get available commands, use:

	RPGMakerDecrypter-cli
	
To extract archive to same directory where it exists, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad
	
To extract archive to some other directory, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad --output=C:\OtherDirectory
	
To create project file with decrypted files, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad --project-file
	
### RPG Maker Decrypter GUI
	
Using Windows application is more straightforward. Open RGASSAD file by using File menu.

If you want to extract single files, select file from list and press extract under file name and size.

## Requirements

* .NET Framework 4.5.2

## Binaries

Latest builds can be found from [here](http://uuksu.kapsi.fi/other/RPGMakerDecrypter).

## Building

RPGMakerDecrypter will compile with Visual Studio 2015. Output deployment package can be created by running deploy.bat script
from Deploy folder.