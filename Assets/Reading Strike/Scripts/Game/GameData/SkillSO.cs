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
        public Color color;
        public float knockBackPower;
        public float battleFrameValue;
    }
    [CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSO")]
    public class SkillSO : ScriptableObject
    {
        [SerializeField] SkillData data;
        public ref readonly SkillData Data => ref data;


        /*
        [SerializeField] string skillName;
        [SerializeField] float power;
        [SerializeField] SkillType type;
        [SerializeField] float cooltime;
        [SerializeField] float stifnessTime;
        [SerializeField] Color color;
        [SerializeField] float knockBackPower;
        [SerializeField] float battleFrameValue;

        public string SkillName => skillName;
        public float Power => power;
        public SkillType Type => type;
        public float Cooltime => cooltime;
        public float StifnessTime=>stifnessTime;
        public Color Color=>color;
        public float KnockBackPower=>knockBackPower;
        public float BattleFrameValue=>battleFrameValue;*/
    }
}