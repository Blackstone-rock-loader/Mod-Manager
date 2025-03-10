using HarmonyLib;

namespace BlackStone
{
    [HarmonyPatch(typeof(AmongUs.Data.Player.PlayerData), nameof(AmongUs.Data.Player.PlayerData.FileName), MethodType.Getter)]
    public class SaveManagerPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = "BLACKROCK_"+ __result;
        }
    }
    [HarmonyPatch(typeof(AmongUs.Data.Legacy.LegacySaveManager), nameof(AmongUs.Data.Legacy.LegacySaveManager.GetPrefsName))]
    public class LegacySaveManagerPatch
    {
        public static void Postfix(ref string __result)
        {
            __result = "BLACKROCK_" + __result;
        }
    }
}