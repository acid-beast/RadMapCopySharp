# RadMapCopySharp

A high-performance C# port of the RadMapCopy tool for Ultima Online (UO). This utility allows you to copy map regions and statics between different UO map files, create empty maps, and extend existing maps to the ML (Mondain's Legacy) size.

## Features

- **Map Copying**: Copy land tiles from a source map to a destination map with various altitude (Z) adjustment modes:
    - **Keep Z**: Preserves original altitude.
    - **Add Fixed**: Adds a specific offset to all tiles.
    - **Random Z**: Sets a random altitude within a given range.
    - **Add Random**: Adds a random offset within a given range to each tile.
- **Statics Copying**: Synchronized copying of statics data for the selected map region.
- **Skip Tiles**: Use presets (defined in `definitions.ini`) to skip specific tile IDs during the copy process (e.g., skip water or lava).
- **Create Empty Map**: Quickly generate a new, blank map file with specified dimensions and default tile/altitude.
- **Extend to ML**: Upgrades a standard map (e.g., 6144x4096) to the ML size (7168x4096).
- **UOP Support**: Supports both legacy `.mul` and modern `.uop` map formats for land tiles.
- **Auto-Detection**: Automatically detects map profiles (dimensions) based on file size and naming.

## Requirements

- **Runtime**: .NET 9.0 Windows
- **Operating System**: Windows (Uses Windows Forms)
- **UO Files**: Access to Ultima Online map files (`map*.mul`, `map*.uop`, `staidx*.mul`, `statics*.mul`).

## Setup

1.  **Download/Clone**: Clone this repository or download the latest release.
2.  **Configuration**:
    - Rename `radmapcopy.ini.example` to `radmapcopy.ini` if you wish to pre-configure paths.
    - Ensure `definitions.ini` is in the same directory as the executable to use skip-tile presets.
3.  **Build**: Open `RadMapCopySharp.sln` or `RadMapCopySharp.csproj` in JetBrains Rider or Visual Studio 2022 and build the solution.

## Usage

1.  Run `RadMapCopySharp.exe`.
2.  Select your **Source** and **Destination** map files. The tool will attempt to auto-detect the map profile.
3.  Define the **Source Region** (X/Y coordinates) and the **Destination Start** (X/Y).
4.  Choose whether to copy **Map** data, **Statics**, or both.
5.  (Optional) Select a **Skip Preset** if you want to avoid overwriting certain tiles.
6.  Click **Copy** to start the operation.

## Project Structure

- `Core/`: Business logic for UO file IO and map operations.
    - `IO/`: Handlers for `.mul` and `.uop` file formats, including UOP hash calculations and index reading.
    - `Operations/`: Implementation of specific tasks like `MapCopyOperation`, `StaticsCopyOperation`, and `ExtendToMLOperation`.
    - `Validation/`: Coordinate and block alignment validation.
- `Dialogs/`: Windows Forms dialogs for various tools (Create Map, Extend Map, Help, About).
- `MainForm.cs`: The primary user interface.
- `definitions.ini`: Configuration for tile ID skip presets.
- `radmapcopy.ini.example`: Example configuration for path persistence.

## Development & Scripts

### Build
```powershell
dotnet build
```

### Run
```powershell
dotnet run --project RadMapCopySharp.csproj
```

### Publish
```powershell
dotnet publish -c Release -r win-x64 --self-contained
```

## Environment Variables

This project does not currently use environment variables for configuration. All settings are managed via `.ini` files.
