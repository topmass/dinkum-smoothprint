# SmoothPrint v1.0.0

Smooths Dirt Printer and Improved Dirt Printer terrain raising in Dinkum.

## Installation

Copy `topmass.smoothprint.dll` to your Dinkum `BepInEx/plugins/` folder.

**Linux**

```bash
cp topmass.smoothprint.dll /home/matthew/.local/share/Steam/steamapps/common/Dinkum/BepInEx/plugins/
```

**Windows**

```text
Copy to: C:\Program Files (x86)\Steam\steamapps\common\Dinkum\BepInEx\plugins\
```

## Features

- Applies to Dirt Printer `925`
- Applies to Improved Dirt Printer `1179`
- Caps sustained held raising so printing does not keep climbing above the player's standing level
- Uses the native tool flow and patches the existing terrain damage checks instead of replacing the printer system

## Requirements

- Dinkum
- BepInEx 6 Unity Mono
