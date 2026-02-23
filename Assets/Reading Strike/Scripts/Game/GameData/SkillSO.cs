using ReadingStrike.Game.InGame;
using UnityEngine;
namespace ReadingStrike.Game.GameData
{
    [CreateAssetMenu(fileName ="Skill",menuName ="SO/SkillSO" )]
    public class SkillSO : ScriptableObject
    {
        public string skillName;
        public string triggerName;
        public float power;
        public SkillType type;
        public float cooltime;
        public float stifnessTime;
        public Color color;
        public float knockBackPower;
        public float battleFrameValue;
    }
}