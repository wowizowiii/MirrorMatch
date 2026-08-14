using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Server.Game;

namespace AetharNet.Mods.DeadlyTrick.MirrorMatch.Patches;

[HarmonyPatch]
public static class GameRoom_StartPickPatch
{
    [HarmonyTargetMethod]
    public static MethodBase TargetStartPickDelegate()
    {
        return typeof(GameRoom)
            .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(method => Attribute.IsDefined(method, typeof(CompilerGeneratedAttribute)))
            .First(method => method.Name.Contains(nameof(GameRoom.StartPick)));
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> SkipPickCharacterState(IEnumerable<CodeInstruction> instructions)
    {
        var resetStopWatchMethod = AccessTools.DeclaredMethod(typeof(TimeManager), nameof(TimeManager.ResetStopWatch));
        var getStopWatchTimeMethod = AccessTools.DeclaredMethod(typeof(GameRoom_StartPickPatch), nameof(GetStopWatchTime));
        var setStopWatchMethod = AccessTools.DeclaredMethod(typeof(TimeManager), nameof(TimeManager.SetStopWatch));

        return new CodeMatcher(instructions)
            .MatchStartForward(new CodeMatch(OpCodes.Callvirt, resetStopWatchMethod))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Call, getStopWatchTimeMethod))
            .SetOperandAndAdvance(setStopWatchMethod)
            .InstructionEnumeration();
    }

    public static int GetStopWatchTime()
    {
        return Plugin.Enabled ? Define.PICK_CHARACTER_LIMIT_SECOND : 0;
    }
}
