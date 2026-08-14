using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace AetharNet.Mods.DeadlyTrick.MirrorMatch;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static bool Enabled;
    public static int CurrentlyChosenCharacter;
    public static readonly Dictionary<int, string> PlayableCharacters = new();

    private bool isMenuOpen;
    private readonly Rect MenuRect = new(40f, 145f, 180f, 290f);

    private void Awake()
    {
        Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, Info.Metadata.GUID);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            isMenuOpen = !isMenuOpen;
        }
    }

    private void OnGUI()
    {
        if (!isMenuOpen) return;

        GUILayout.BeginArea(MenuRect, GUI.skin.box);

        GUI.enabled = Managers.Network.Lobby.IsHost;
        GUILayout.Label("Toggle");
        GameModeToggle();

        GUILayout.Space(4f);
        GUI.enabled = Enabled;

        GUILayout.Label("Chosen Character");
        foreach (var (characterID, characterName) in PlayableCharacters)
        {
            CharacterOption(characterID, characterName);
        }

        GUILayout.Space(4f);

        if (GUILayout.Button("Randomize Character"))
        {
            RandomizeCharacter();
        }

        GUILayout.EndArea();
    }

    private static void GameModeToggle()
    {
        var isPreviouslySelected = Enabled;
        var isCurrentlySelected = GUILayout.Toggle(isPreviouslySelected, "Enable Mirror Match");

        if (!isPreviouslySelected && isCurrentlySelected)
        {
            Enabled = true;
        }
        else if (isPreviouslySelected && !isCurrentlySelected)
        {
            Enabled = false;
        }
    }

    private static void CharacterOption(int characterID, string characterName)
    {
        var isPreviouslySelected = CurrentlyChosenCharacter == characterID;
        var isCurrentlySelected = GUILayout.Toggle(isPreviouslySelected, characterName);

        if (!isPreviouslySelected && isCurrentlySelected)
        {
            CurrentlyChosenCharacter = characterID;
        }
    }

    private static void RandomizeCharacter()
    {
        var characterIDs = PlayableCharacters.Keys.ToArray();
        var randomIndex = Util.GetRandomNumber(0, characterIDs.Length);
        CurrentlyChosenCharacter = characterIDs[randomIndex];
    }
}
