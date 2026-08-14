using HarmonyLib;
using Server.Game;

namespace AetharNet.Mods.DeadlyTrick.MirrorMatch.Patches;

[HarmonyPatch(typeof(GameRoom))]
public static class GameRoomPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameRoom.PickRandomOwnedCharacter))]
    public static void ForceChosenCharacter(ref int __result)
    {
        if (Plugin.Enabled)
        {
            __result = Plugin.CurrentlyChosenCharacter;
        }
    }
}
