using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.GameData
{
    [CreateAssetMenu(fileName = "StatusSO", menuName = "SO/StatusSO")]
    public class StatusSO : ScriptableObject
    {
        public int maxHp;
        public int atk;
        public int def;
        public float moveSpeed;
    }
}