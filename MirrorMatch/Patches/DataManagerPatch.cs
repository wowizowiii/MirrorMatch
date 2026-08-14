using System.Linq;
using HarmonyLib;

namespace AetharNet.Mods.DeadlyTrick.MirrorMatch.Patches;

[HarmonyPatch(typeof(DataManager))]
public static class DataManagerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DataManager.Init))]
    public static void RetrieveCharacterData(DataManager __instance)
    {
        foreach (var characterID in Define.DEFAULT_OWNED_CHARACTER_IDS)
        {
            if (__instance.CharacterDic.TryGetValue(characterID, out var characterData))
            {
                Plugin.PlayableCharacters.TryAdd(characterID, characterData.Name);
            }
        }

        Plugin.CurrentlyChosenCharacter = Plugin.PlayableCharacters.First().Key;
    }
}
