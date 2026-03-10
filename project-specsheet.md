# SmoothPrint Spec Sheet

## Purpose

`SmoothPrint` adjusts dirt printer use so held printing does not keep climbing above the player level, while fresh clicks can still manually build upward.

## Scope

- Dirt Printer `925`
- Improved Dirt Printer `1179`
- Empty Improved Dirt Printer `1180`

## Current Implementation

- Bootstrap lives in `code/Plugin.cs`
- Harmony patches live in `code/SmoothPrintPatches.cs`
- Build project is `code/SmoothPrint.csproj`

## Current Behavior

- The mod only intervenes for dirt printers that raise terrain
- `CharInteract.CheckIfCanDamage()` is the main gate
- If the targeted tile is below the player's standing height, vanilla behavior is left alone
- If the targeted tile is at or above the player's standing height, SmoothPrint can block the raise
- A fresh click is registered from `InputMaster.Use()`
- While the player keeps holding the same press, a successful raise during that hold marks the press as a held raise session
- Once that held raise session is marked, further raises at or above the player height are blocked for that same press
- Fresh clicks reset that state, which preserves manual upward printing behavior as much as possible without changing the native printer flow

## Height Comparison

- Player height is read from `Mathf.RoundToInt(interact.transform.root.position.y)`
- Target height is read from `WorldManager.Instance.heightMap[targetX, targetY]`
- The patch only cares about the case where `targetHeight >= playerHeight`

## Files To Edit

- `code/Plugin.cs`
  - `ShouldClampAbovePlayerRaise()`
  - `RecordPrinterRaise(bool consumedAbovePlayerCredit)`
  - `RegisterFreshUseClick()`
- `code/SmoothPrintPatches.cs`
  - `SmoothPrintCheckIfCanDamagePatch`
  - `SmoothPrintUseInputPatch`
  - `SmoothPrintChangeTileHeightPatch`
  - `SmoothPrintHeightUtility`

## Important Rules

- Keep target framework at `net472`
- Keep language version at `7.3`
- Keep `Mirror.dll` referenced because `CharInteract` inherits from Mirror networking types
- Do not change the printer item ids without re-checking current game ids
- Do not move this logic out of `CheckIfCanDamage()` unless you also re-check clang and animation behavior in-game

## Build Output

- Local build DLL: `code/bin/Release/net472/topmass.smoothprint.dll`
- Release DLL: `mod/topmass.smoothprint.dll`
