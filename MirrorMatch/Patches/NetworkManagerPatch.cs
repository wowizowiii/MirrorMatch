using HarmonyLib;

namespace AetharNet.Mods.DeadlyTrick.MirrorMatch.Patches;

[HarmonyPatch(typeof(NetworkManager))]
public static class NetworkManagerPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(NetworkManager.Leave))]
    public static void DisableModifierOnLeave(NetworkManager __instance)
    {
        if (Managers.Network.Lobby.IsHost)
        {
            Plugin.Enabled = false;
        }
    }
}
