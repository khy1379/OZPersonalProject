using ReadingStrike.Game.InGame;
using UnityEngine;
namespace ReadingStrike.Game.GameData
{
    [System.Serializable]
    public struct SkillData
    {
        public string skillName;
        public float power;
        public SkillType type;
        public float cooltime;
        public float stifnessTime;
        public Material skillMat;
        public float knockBackPower;
        public float battleFrameValue;
        public float skillRange;
    }
    [CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSO")]
    public class SkillSO : ScriptableObject
    {
        [SerializeField] SkillData data;
        public SkillData Data => data;
    }
}