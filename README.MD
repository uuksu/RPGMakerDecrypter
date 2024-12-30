# RPG Maker Decrypter

RPG Maker Decrypter can be used to extract encrypted archives and files created with **RPG Maker XP, VX VX Ace, MV or MZ**.
It can be also used to recreate a best guess of the original project.

## Usage

RPG Maker Decrypter is an advanced CLI application. To use it, you have a have basic skills of running terminal applications.

To decrypt RPG Maker XP, VX and VX Ace games, give the encrypted archive file as input (usually files with .rgssad, .rgss2a or .rgss3a extension, but these might also be renamed to something else). To decrypt MV or MZ games, give the game deployment root directory as the input.

To get available commands, use:

	RPGMakerDecrypter-cli
	
To extract archive to same directory where it exists, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad
	
To extract RPG Maker XP, VX or VX Ace archive to some other directory, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad --output=C:\OtherDirectory

To extract RPG Maker MV or MZ game to some other directory, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game --output=C:\OtherDirectory
	
To give a best guess in creating the original project, use

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad --recreate-project

You must always specify the output directory when recreating a project for MV or MZ.

	RPGMakerDecrypter-cli C:\MyRPGMakerGame\Game.rgssad --recreate-project --output=C:\OtherDirectory

## RPG Maker Decrypter GUI

Unfortunately, support for GUI has ended. Sources still exist in [history](https://github.com/uuksu/RPGMakerDecrypter/tree/1a24e8c0a9bbf7b9b1cb030a6a2eb20882e6df15). Maybe you would like to continue the development of the Windows-only Forms GUI or create a totally new cross-platform alternative?

## Requirements

*  Application works with any platform .NET 8.0 targets.
* .NET Runtime is not required for prebuilt binaries on selected platforms as it is packaged in the executable.

## Binaries

Prebuilt binaries are available for selected platforms. See [Releases](https://github.com/uuksu/RPGMakerDecrypter/releases).

## Building

CLI application will compile with .NET 8.0 SDK, libraries target .NET Standard 2.0.