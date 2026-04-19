using ReadingStrike.Game.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReadingStrike.Game.UI
{
    public class DungeonUIManager : MonoBehaviour
    {
        public static DungeonUIManager instanse;
        public HPBar playerHPBar;
        public HPBar monsterHPBar;
        [SerializeField] GameObject guidePanel;
        private void Awake()
        {
            instanse = this;
        }
        private void OnDestroy()
        {
            instanse = null;
        }
        public void PlayerHPBarEventSet(Character player) => player.AddEventChangeHP(playerHPBar.HPBarValueSet);
        public void PlayerHPBarEventRemove(Character player) => player.RemoveEventChangeHP(playerHPBar.HPBarValueSet);
        public void MonsterHPBarEventSet(Character monster) => monster.AddEventChangeHP(monsterHPBar.HPBarValueSet);
        public void MonsterHPBarEventRemove(Character monster) => monster.RemoveEventChangeHP(monsterHPBar.HPBarValueSet);
        public void GuidePanelShow() => guidePanel.SetActive(true);
    }
}