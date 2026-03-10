using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
namespace SmoothPrint
{
    [BepInPlugin("topmass.smoothprint", "SmoothPrint", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private Harmony harmony;
        private static bool canRaiseAbovePlayerThisPress;
        private static bool raisedWhileHoldingThisPress;

        private void Awake()
        {
            Log = Logger;

            harmony = new Harmony("topmass.smoothprint");
            harmony.PatchAll();

            Log.LogInfo("SmoothPrint loaded");
        }

        internal static bool ShouldClampAbovePlayerRaise()
        {
            return !canRaiseAbovePlayerThisPress || raisedWhileHoldingThisPress;
        }

        internal static void RecordPrinterRaise(bool consumedAbovePlayerCredit)
        {
            if (InputMaster.input != null && InputMaster.input.UseHeld())
            {
                raisedWhileHoldingThisPress = true;
            }

            if (consumedAbovePlayerCredit)
            {
                canRaiseAbovePlayerThisPress = false;
            }
        }

        internal static void RegisterFreshUseClick()
        {
            canRaiseAbovePlayerThisPress = true;
            raisedWhileHoldingThisPress = false;
        }
    }
}
