using CellMenu;
using Globals;

#if MELONLOADER
using MelonLoader;
using Harmony;
[assembly: MelonInfo(typeof(GTFO_Intro_Skip.Skipper), "BetterStart", "1.0.0", "LapinoLapidus#3262")]
[assembly: MelonGame("10 Chambers Collective", "GTFO")]
#else
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnhollowerRuntimeLib;

#endif
namespace GTFO_Intro_Skip
{
    #if MELONLOADER
    class Skipper : MelonMod
    {
    #else
    [BepInPlugin("ovh.lapinolapidus.betterstart", "Better Start", "1.0.0")]
    class Skipper : BasePlugin
    {
        public override void Load()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("ovh.lapinolapidus.betterstart");
            harmony.PatchAll();
            ClassInjector.RegisterTypeInIl2Cpp<Skipper>();
        }
        #endif
        [HarmonyPatch(typeof(GlobalSetup), "Setup")]
        public class GlobalSetup_Setup
        {
            [HarmonyPrefix]
            public static void Prefix(GlobalSetup __instance)
            {
                __instance.m_skipIntro = true;
                __instance.m_showStartupScreen = false;
            }
        }
        [HarmonyPatch(typeof(CM_PageIntro), "Setup")]
        public class CM_PageIntro_Setup
        {
            public static void Postfix(CM_PageIntro __instance)
            {
                __instance.EXT_PressInject(0);
            }
        }
        [HarmonyPatch(typeof(CM_PageRundown_New), "Setup")]
        public class CM_PageRundown_New_Setup
        {
            public static void Prefix(CM_PageRundown_New __instance)
            {
                __instance.m_cortexIntroIsDone = true;
                __instance.m_rundownIntroIsDone = true;
            }
        }


        [HarmonyPatch(typeof(CM_TimedButton), "OnHoverUpdate")]
        public class CM_TimedButton_OnHoverUpdate
        {
            public static bool Prefix(CM_TimedButton __instance, ref iCellMenuInputHandler inputHandler)
            {
                if (!inputHandler.MainButtonStatus)
                    return false;
                __instance.DoBtnPress(inputHandler);
                __instance.m_holdBtnActive = false;
                CM_PageBase.PostSound(__instance.SOUND_CLICK_HOLD_DONE, "Click hold DONE");
                return false;
            }
        }
    }
}
