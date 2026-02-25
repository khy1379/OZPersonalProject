using ReadingStrike.Game.InGame;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.GameData
{
    public enum SceneType { Title, Village, PlaneDungeon }
    public enum InGameState { Playing, Pause, Death }
    public enum SkillType { NormalAtk, StrongAtk, Defense }
    public enum BattleResultType { None, AWin, BWin, Draw }
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
    //public interface ISkillUser
    //{
    //    public SkillSet ChargedSkill { get; }
    //    public bool CurSkillUse { get; }
    //    public bool IsSkillCharged { get; }
    //    public int CurSkillUseDamage { get; }
    //}
    //public interface IDamageable
    //{
    //    public bool IsDeath { get; }
    //    public void GetDamaged(int damage);
    //    public void Stifness();
    //}
    //public interface IAnimatorable
    //{
    //    public void StartAnimation(AnimationTriggerType type);
    //    public void StartCurSkillAnimation();
    //    public bool CheckBattleTiming { get; }
    //    public bool IsGetDamaged { get; }
    //}
    //public interface IBattleable : ISkillUser, IDamageable, IAnimatorable
    //{
    //    public void BattleDrawAction();
    //    public bool BattleWinAction(IBattleable cha);

    //}
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