using System;
using HarmonyLib;
using UnityEngine;

namespace SmoothPrint
{
    public static class SmoothPrintItemIds
    {
        public const int DirtPrinterId = 925;
        public const int ImprovedDirtPrinterId = 1179;
        public const int EmptyImprovedDirtPrinterId = 1180;
    }

    public static class SmoothPrintHeightUtility
    {
        public static int GetPlayerStandingHeight(CharInteract interact)
        {
            return Mathf.RoundToInt(interact.transform.root.position.y);
        }
    }

    [HarmonyPatch(typeof(CharInteract), "CheckIfCanDamage")]
    public static class SmoothPrintCheckIfCanDamagePatch
    {
        static void Postfix(CharInteract __instance, Vector2 selectedTile, ref bool __result)
        {
            if (!__result || __instance == null || !__instance.isLocalPlayer)
            {
                return;
            }

            if (!IsSmoothPrintRaiseBlocked(__instance, selectedTile))
            {
                return;
            }

            __result = false;
        }

        private static bool IsSmoothPrintRaiseBlocked(CharInteract interact, Vector2 selectedTile)
        {
            if (interact.myEquip == null || interact.myEquip.currentlyHoldingItemId < 0)
            {
                return false;
            }

            int heldItemId = interact.myEquip.currentlyHoldingItemId;
            if (heldItemId != SmoothPrintItemIds.DirtPrinterId &&
                heldItemId != SmoothPrintItemIds.ImprovedDirtPrinterId &&
                heldItemId != SmoothPrintItemIds.EmptyImprovedDirtPrinterId)
            {
                return false;
            }

            InventoryItem heldItem = interact.myEquip.itemCurrentlyHolding;
            if (heldItem == null || heldItem.changeToHeightTiles <= 0)
            {
                return false;
            }

            if (WorldManager.Instance == null)
            {
                return false;
            }

            int targetX = Mathf.RoundToInt(selectedTile.x);
            int targetY = Mathf.RoundToInt(selectedTile.y);
            if (!WorldManager.Instance.isPositionOnMap(targetX, targetY))
            {
                return false;
            }

            int playerHeight = SmoothPrintHeightUtility.GetPlayerStandingHeight(interact);
            int targetHeight = WorldManager.Instance.heightMap[targetX, targetY];
            if (targetHeight < playerHeight)
            {
                return false;
            }

            return Plugin.ShouldClampAbovePlayerRaise();
        }
    }

    [HarmonyPatch(typeof(CharMovement), "checkUseItemButton")]
    public static class SmoothPrintUseInputPatch
    {
        static void Prefix()
        {
            if (InputMaster.input == null)
            {
                return;
            }

            if (InputMaster.input.Use())
            {
                Plugin.RegisterFreshUseClick();
            }
        }
    }

    [HarmonyPatch(typeof(CharInteract), "ChangeTileHeigh")]
    public static class SmoothPrintChangeTileHeightPatch
    {
        [ThreadStatic]
        private static bool lastRaiseWasAtOrAbovePlayerHeight;

        static void Prefix(CharInteract __instance, int heightDif)
        {
            lastRaiseWasAtOrAbovePlayerHeight = false;

            if (__instance == null || !__instance.isLocalPlayer || heightDif <= 0)
            {
                return;
            }

            if (IsTrackedDirtPrinter(__instance.myEquip))
            {
                lastRaiseWasAtOrAbovePlayerHeight = IsTargetAtOrAbovePlayerHeight(__instance);
            }
        }

        static void Postfix(CharInteract __instance, int heightDif)
        {
            if (__instance == null || !__instance.isLocalPlayer || heightDif <= 0)
            {
                return;
            }

            if (IsTrackedDirtPrinter(__instance.myEquip))
            {
                Plugin.RecordPrinterRaise(lastRaiseWasAtOrAbovePlayerHeight);
            }
        }

        private static bool IsTargetAtOrAbovePlayerHeight(CharInteract interact)
        {
            int targetX = Mathf.RoundToInt(interact.selectedTile.x);
            int targetY = Mathf.RoundToInt(interact.selectedTile.y);

            if (!WorldManager.Instance.isPositionOnMap(targetX, targetY))
            {
                return false;
            }

            return WorldManager.Instance.heightMap[targetX, targetY] >=
                   SmoothPrintHeightUtility.GetPlayerStandingHeight(interact);
        }

        private static bool IsTrackedDirtPrinter(EquipItemToChar equip)
        {
            if (equip == null)
            {
                return false;
            }

            int heldItemId = equip.currentlyHoldingItemId;
            return heldItemId == SmoothPrintItemIds.DirtPrinterId ||
                   heldItemId == SmoothPrintItemIds.ImprovedDirtPrinterId ||
                   heldItemId == SmoothPrintItemIds.EmptyImprovedDirtPrinterId;
        }
    }
}
