using ReadingStrike.Game.InGame;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.GameData
{
    public enum SceneType { Title, Village, PlaneDungeon, }
    public enum InGameState { Playing, Pause, Death }
    public enum SkillType { NormalAtk, StrongAtk, Defense }
    public enum BattleResultType { None, AWin, BWin, Draw }
    public enum CharacterType { Player, Monster_Normal, Monster_Boss }
    public enum AnimationTriggerType
    {
        NormalAtk,
        StrongAtk,
        Defense,
        Damaged,
        Death,
        Idle
    }
    public enum DungeonType { Plane, }
    public static class CTSSetter
    {
        public static void CTSSet(ref CancellationTokenSource cts)
        {
            if (cts != null) return;
            cts = new CancellationTokenSource();
        }
        public static void CTSCancel(ref CancellationTokenSource cts)
        {
            if (cts == null) return;
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }
    public class CommonContents
    {

    }
}