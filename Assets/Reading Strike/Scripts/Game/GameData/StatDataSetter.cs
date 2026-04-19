using ReadingStrike.Game.GameData;
using System.Collections.Generic;
using UnityEngine;
namespace ReadingStrike.Game.GameData
{
    public class StatDataSetter : MonoBehaviour
    {
        [SerializeField] StatSO playerStatSo;
        [SerializeField] StatSO[] monsterStatSo;
        public StatData GetPlayerStatData()
        {
            if (playerStatSo == null)
            {
                Debug.LogWarning("Player Stat 데이터 없음");
                return default;
            }
            return playerStatSo.Data;
        }
        public StatData GetMonsterStatData(CharacterType type)
        {
            if (monsterStatSo == null)
            {
                Debug.LogWarning("Monster Stat 데이터 없음");
                return default;
            }
            int monNum = (int)type - 1;
            switch (type)
            {
                case CharacterType.Goblin_Normal:
                case CharacterType.Goblin_Boss:
                    return monsterStatSo[monNum].Data;
                default:
                    Debug.LogWarning($"{monNum} index의 Monster Stat 데이터 없음");
                    return default;
            }
        }
    }
}