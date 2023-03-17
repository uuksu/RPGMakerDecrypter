# RPG Maker Decrypter

RPG Maker Decrypter can be used to extract encrypted archives created with RPG Maker XP, RPG Maker VX or RPG Maker VX Ace.
Software can also generate correct type project file for decrypted data.

## Usage

RPG Maker Decrypter has a Windows application and a cli application which both provide the same functionality but with different interface.

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

![Screenshots of the GUI application](/screenshots/gui1.png)

### RPG Maker Decrypter GTK GUI (Experiemental)
	
Repository also contains [experiemental UI](https://github.com/uuksu/RPGMakerDecrypter/tree/master/RPGMakerDecrypter.Gui.Gtk) that uses GtkSharp under the hood.

There is no prebuilt binary for this UI, but it is functional and can be built from the sources if needed.

Thank you @tyrone-sudeium for porting the UI!

## Requirements

* GUI version works only on Windows, CLI version works with any platform .NET 6.0 targets.
* .NET Runtime is not required for prebuilt binaries on selected platforms as it is packaged in the executable.

## Binaries

Prebuilt binaries are available for selected platforms. See [Releases](https://github.com/uuksu/RPGMakerDecrypter/releases).

## Building

RPGMakerDecrypter will compile with .NET 6.0 SDK.