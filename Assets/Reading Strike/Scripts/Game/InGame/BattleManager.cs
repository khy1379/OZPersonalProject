using System;
using UnityEngine;
using ReadingStrike.Game.GameData;
using System.Threading;
using ReadingStrike.Game.UI;
namespace ReadingStrike.Game.InGame
{
    public class BattleEvent
    {
        public event Action RequestPlayerWin;
        public void RaisePlayerWin() => RequestPlayerWin?.Invoke();
    }
    public class BattleManager : MonoBehaviour
    {
        public int curDungeonMonMaxCnt = 1;
        public int curDungeonMonCnt;
        public static BattleManager instance;
        public event Action<BattleResultType> RequestBattleResult;
        [SerializeField] MonsterHPBar monHpBar;
        BattleEvent be = new BattleEvent();
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            curDungeonMonCnt = curDungeonMonMaxCnt;
        }
        private void OnDestroy()
        {
            //instance = null;
        }
        public void MonsterCntDown()
        {
            curDungeonMonCnt--;
            if(curDungeonMonCnt == 0)
            {
                be.RaisePlayerWin();
            }
        }
        public void RaiseBattleResult(BattleResultType battleResult) { RequestBattleResult?.Invoke(battleResult); }
        public void BattleStart(Character pl, Character mon)
        {
            pl.MoveStop();
            mon.MoveStop();
            monHpBar.MonHPBarSetting(mon);
            BattleResultType resultType = BattleResultType.None;
            if (pl.IsSkillCharged && mon.IsSkillCharged)
            {
                resultType = BattleResult(pl.ChargedSkill.Data.type, mon.ChargedSkill.Data.type);
            }
            else if(pl.IsSkillCharged && !mon.IsSkillCharged)
            {
                resultType = BattleResultType.AWin;
            }
            else if(!pl.IsSkillCharged && mon.IsSkillCharged)
            {
                resultType = BattleResultType.BWin;
            }
            switch (resultType)
            {
                case BattleResultType.Draw:
                    pl.BattleDrawAction();
                    mon.BattleDrawAction();
                    RaiseBattleResult(BattleResultType.Draw);
                    break;
                case BattleResultType.AWin:
                    pl.BattleWinAction(mon);
                    break;
                case BattleResultType.BWin:
                    mon.BattleWinAction(pl);
                    break;
            }
            if (mon.IsDeath)
                MonsterCntDown();
        }
        BattleResultType BattleResult(SkillType plSkillType, SkillType monSkillType)
        {
            BattleResultType returnType = BattleResultType.None;
            switch (plSkillType)
            {
                case SkillType.StrongAtk:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            returnType = BattleResultType.Draw;
                            break;
                        case SkillType.Defense:
                            returnType = BattleResultType.AWin;
                            break;
                        default:
                            returnType = BattleResultType.BWin;
                            break;
                    }
                    break;
                case SkillType.Defense:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            returnType = BattleResultType.BWin;
                            break;
                        case SkillType.Defense:
                            returnType = BattleResultType.None;
                            break;
                        default:
                            returnType = BattleResultType.AWin;
                            break;
                    }
                    break;
                default:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            returnType = BattleResultType.AWin;
                            break;
                        case SkillType.Defense:
                            returnType = BattleResultType.BWin;
                            break;
                        default:
                            returnType = BattleResultType.Draw;
                            break;
                    }
                    break;
            }
            return returnType;
        }
        public void AddEventPlayerWin(Action func) => be.RequestPlayerWin += func;
        public void RemoveEventPlayerWin(Action func) => be.RequestPlayerWin -= func;
    }
}