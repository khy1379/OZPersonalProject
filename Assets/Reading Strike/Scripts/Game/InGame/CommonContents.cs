using System.Threading;
using UnityEngine;

namespace ReadingStrike.Game.InGame
{
    public enum SceneType
    {
        Title,
        Village,
        Dungeon
    }
    public enum SkillType
    {
        NormalAtk,
        StrongAtk,
        Defense
    }
    public enum BattleResultType
    {
        None,
        PlayerWin,
        MonsterWin,
        Draw
    }
    public enum AnimationTriggerType
    {
        NormalAtk,
        StrongAtk,
        Defense,
        Damaged,
        Death,
        Idle
    }
    public interface ISkillUser
    {
        public SkillSet ChargedSkill { get; }
        public bool CurSkillUse { get; }
        public bool IsSkillCharged { get; }
        public int CurSkillUseDamage { get; }
    }
    public interface IDamageable
    {
        public bool IsDeath { get; }
        public void GetDamaged(int damage);
        public void Stifness();
    }
    public interface IAnimatorable
    {
        public void StartAnimation(AnimationTriggerType type);
        public void StartCurSkillAnimation();
        public bool CheckBattleTiming { get; }
        public bool IsGetDamaged { get; }
    }
    public interface IBattleable : ISkillUser, IDamageable, IAnimatorable
    {
        public void BattleDrawAction();
        public bool BattleWinAction(IBattleable cha);

    }
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