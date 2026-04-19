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
        Character player;
        Character foughtMonster;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            curDungeonMonCnt = curDungeonMonMaxCnt;
            player = GameManager.instance.Pl;
            DungeonUIManager.instanse.PlayerHPBarEventSet(player);
            player.HPValueSet();
        }
        public void DestroyFeatFromOutside()
        {
            instance = null;
            if (player != null) DungeonUIManager.instanse.PlayerHPBarEventRemove(player);
            if (foughtMonster != null) DungeonUIManager.instanse.MonsterHPBarEventRemove(foughtMonster);
        }
        public void MonsterCntDown()
        {
            curDungeonMonCnt--;
            if (curDungeonMonCnt == 0)
            {
                be.RaisePlayerWin();
            }
        }
        public void RaiseBattleResult(BattleResultType battleResult) { RequestBattleResult?.Invoke(battleResult); }
        public void BattleStart(Character mon)
        {
            bool isUpdateMonHpBar = false;
            bool isUpdatePlayerHpBar = false;
            if (mon != foughtMonster)
            {
                if(foughtMonster != null) DungeonUIManager.instanse.MonsterHPBarEventRemove(foughtMonster);
                foughtMonster = mon;
                isUpdateMonHpBar = true;
                DungeonUIManager.instanse.MonsterHPBarEventSet(foughtMonster);
            }
            player.MoveStop();
            foughtMonster.MoveStop();
            foughtMonster.LookEnemy(player.transform.position);
            BattleResultType resultType = BattleResultType.None;
            if (player.IsSkillCharged && foughtMonster.IsSkillCharged)
            {
                resultType = BattleResult(player.ChargedSkill.Data.type, foughtMonster.ChargedSkill.Data.type);
            }
            else if (player.IsSkillCharged && !foughtMonster.IsSkillCharged)
            {
                resultType = BattleResultType.AWin;
            }
            else if (!player.IsSkillCharged && foughtMonster.IsSkillCharged)
            {
                resultType = BattleResultType.BWin;
            }
            switch (resultType)
            {
                case BattleResultType.Draw:
                    player.BattleDrawAction();
                    foughtMonster.BattleDrawAction();
                    RaiseBattleResult(BattleResultType.Draw);
                    break;
                case BattleResultType.AWin:
                    player.BattleWinAction(foughtMonster);
                    isUpdateMonHpBar = true;
                    break;
                case BattleResultType.BWin:
                    foughtMonster.BattleWinAction(player);
                    isUpdatePlayerHpBar = true;
                    break;
            }
            if(isUpdatePlayerHpBar)
            {
                player.HPValueSet();
            }
            if(isUpdateMonHpBar)
            {
                foughtMonster.HPValueSet();
                if (foughtMonster.IsDeath)
                {
                    MonsterCntDown();
                    DungeonUIManager.instanse.MonsterHPBarEventRemove(foughtMonster);
                }
            }
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