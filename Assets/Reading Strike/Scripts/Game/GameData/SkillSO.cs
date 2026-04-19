using ReadingStrike.Game.InGame;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.GameData
{
    [System.Serializable]
    public struct SkillData
    {
        public SkillType type;
        public Material skillMat;
        public float power;
        public float cooltime;
        public float stifnessTime;
        //public float knockBackPower;
        //public float battleFrameValue;
        public float skillRange;
    }
    [CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSO")]
    public class SkillSO : ScriptableObject
    {
        [SerializeField] SkillData data;
        public SkillData Data => data;
        [Header("Skill 설명")]
        public Sprite skillIcon;
        public string skillName;
        [TextArea] public string skillDescription;
        //public int skillNum;
        //public bool isGetSkill;
    }
}