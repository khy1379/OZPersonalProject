using ReadingStrike.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Manager
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
        Death
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
    }
    public interface IBattleable : ISkillUser, IDamageable, IAnimatorable
    {

    }
    public class CommonContents
    {

    }
}