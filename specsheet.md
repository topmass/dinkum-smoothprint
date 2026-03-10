# SmoothPrint Spec Sheet

## What This Mod Does

`SmoothPrint` patches the Dirt Printer and Improved Dirt Printer so held printing does not keep stepping above the player's current standing height, while fresh clicks still reset the manual raise state.

## Files

- `code/Plugin.cs`
- `code/SmoothPrintPatches.cs`
- `code/SmoothPrint.csproj`
- `mod/topmass.smoothprint.dll`

## Patched Game Flow

1. Dinkum still drives the printers through the native animation and `doDamage()` flow
2. `SmoothPrintUseInputPatch` watches `InputMaster.Use()` and treats that as a fresh click
3. `SmoothPrintCheckIfCanDamagePatch` blocks only above-player printer raises
4. `SmoothPrintChangeTileHeightPatch` records whether the current press already raised at or above the player height

## Item Ids

- Dirt Printer: `925`
- Improved Dirt Printer: `1179`
- Empty Improved Dirt Printer: `1180`

## Height Logic

- Player standing height comes from `Mathf.RoundToInt(interact.transform.root.position.y)`
- Target tile height comes from `WorldManager.Instance.heightMap`
- If the target is below the player height, vanilla behavior is preserved
- If the target is at or above the player height, the mod can clamp it based on the current press state

## Current Editing Rules

- Keep `net472`
- Keep C# `7.3`
- Keep `Mirror.dll` referenced in the project
- Re-check `InputMaster.Use()` and `InputMaster.UseHeld()` before changing click or hold behavior
- Re-check `CharInteract.CheckIfCanDamage()` and `CharInteract.ChangeTileHeigh()` before changing terrain logic

## Build Paths

- Build output: `code/bin/Release/net472/topmass.smoothprint.dll`
- Release output: `mod/topmass.smoothprint.dll`
