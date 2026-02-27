using ReadingStrike.Game.InGame;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.UI
{
    public class MonsterHPBar : HPBar
    {
        Monster mon;
        public void MonHPBarSetting(Monster mon)
        {
            if (this.mon == mon) return;
            if (this.mon != null) this.mon.HPBarClear();
            this.mon = mon;
            this.mon.HPBarSetting(this);
        }
        public void MonHPBarSetting(Character character)
        {
            if (character is Monster tMon) MonHPBarSetting(tMon);
        }
    }
}