# SmoothPrint

A BepInEx mod for Dinkum that smooths Dirt Printer and Improved Dirt Printer terrain raising.

## Quick Install

Copy `mod/topmass.smoothprint.dll` to your Dinkum `BepInEx/plugins/` folder.

## What It Does

- Caps held dirt-printer raising so sustained printing does not keep climbing above the player's current standing level
- Keeps the native dirt printer flow intact by patching the existing terrain damage checks instead of replacing the printer tool logic
- Applies to both the Dirt Printer and Improved Dirt Printer

## Folder Structure

```text
SmoothPrint/
├── README.md
├── project-specsheet.md
├── mod/
│   ├── README.md
│   ├── topmass.smoothprint.dll
│   └── topmass-smoothprint-v1.0.0.zip
└── code/
    ├── Plugin.cs
    ├── SmoothPrintPatches.cs
    ├── SmoothPrint.csproj
    ├── .gitignore
    └── Libs/
        └── README.md
```

## Building From Source

```bash
cd code
dotnet build -c Release
```

Output:

```text
code/bin/Release/net472/topmass.smoothprint.dll
```

## Requirements

- Dinkum
- BepInEx 6 Unity Mono

