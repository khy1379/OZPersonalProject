using ReadingStrike.Game.InGame;
using UnityEngine;
namespace ReadingStrike.Game.GameData
{
    [CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSO")]
    public class SkillSO : ScriptableObject
    {
        string skillName;
        public string SkillName { get { return skillName; } }

        string triggerName;
        public string TriggerName { get { return triggerName; } }

        float power;
        public float Power { get { return power; } }

        SkillType type;
        public SkillType Type { get { return type; } }

        float cooltime;
        public float Cooltime { get { return cooltime; } }

        float stifnessTime;
        public float StifnessTime { get { return stifnessTime; } }

        Color color;
        public Color Color { get { return color; } }

        float knockBackPower;
        public float KnockBackPower { get { return knockBackPower; } }

        float battleFrameValue;
        public float BattleFrameValue { get { return battleFrameValue; } }

    }
}