using ReadingStrike.Manager;
using UnityEngine;
namespace ReadingStrike.SOFrame
{
    [CreateAssetMenu(fileName ="Skill",menuName ="SO/SkillSO" )]
    public class SkillSO : ScriptableObject
    {
        public string skillName;
        public string triggerName;
        public int power;
        public SkillType type;
        public float cooltime;
        public float stifnessTime;
        public Color color;
        public float knockBackPower;
    }
}