using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.GameData
{
    [System.Serializable]
    public struct StatData
    {
        public CharacterType charType;
        public int maxHP;
        public int atk;
        public int def;
        public float moveSpeed;
        public int criticalChance;
        public string StatValueText(StatType type)
        {
            string valueText = "";
            switch (type)
            {
                case StatType.MaxHP:
                    valueText = maxHP.ToString();
                    break;
                case StatType.Atk:
                    valueText = atk.ToString();
                    break;
                case StatType.Def:
                    valueText = def.ToString();
                    break;
                case StatType.MoveSpeed:
                    valueText = moveSpeed.ToString();
                    break;
                case StatType.CriticalChance:
                    valueText = criticalChance.ToString();
                    break;
            }
            return valueText;
        }
    }
    [CreateAssetMenu(fileName = "StatusSO", menuName = "SO/StatusSO")]
    public class StatSO : ScriptableObject
    {
        [SerializeField] StatData data;
        public StatData Data => data;

    }
}