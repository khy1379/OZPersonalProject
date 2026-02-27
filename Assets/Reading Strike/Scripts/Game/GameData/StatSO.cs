using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.GameData
{
    [System.Serializable]
    public struct StatData
    {
        public int maxHp;
        public int atk;
        public int def;
        public float moveSpeed;
        public CharacterType charType;
    }
    [CreateAssetMenu(fileName = "StatusSO", menuName = "SO/StatusSO")]
    public class StatSO : ScriptableObject
    {
        [SerializeField] StatData data;
        public ref readonly StatData Data => ref data;
    }
}