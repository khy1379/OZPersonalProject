using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.GameData
{
    [CreateAssetMenu(fileName = "StatusSO", menuName = "SO/StatusSO")]
    public class StatusSO : ScriptableObject
    {
        int maxHp;
        public int MaxHp { get { return maxHp; } }
        int atk;
        public int Atk { get { return atk; } }
        int def;
        public int Def { get { return def; } }
        float moveSpeed;
        public float MoveSpeed { get { return moveSpeed; } }
    }
}