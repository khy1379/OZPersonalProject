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
        PlayerWin,
        MonsterWin,
        Draw
    }
    public interface IBattleable
    {
        public SkillSet ChargedSkill { get; }
        public bool CurSkillUse { get; }
        public bool IsSkillCharged { get; }
        public void GetDamaged(int damage);
    }
    public class CommonContents
    {

    }
}