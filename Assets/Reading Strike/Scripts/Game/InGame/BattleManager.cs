using System;
using UnityEngine;
using System.Threading;
namespace ReadingStrike.Game.InGame
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;
        public event Action<BattleResultType> RequestBattleResult;

        CancellationTokenSource cts;
        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            instance = this;
        }
        private void OnDestroy()
        {
            CtsCancel();
        }
        public void RaiseBattleResult(BattleResultType battleResult) { RequestBattleResult?.Invoke(battleResult); }
        public void BattleStart(IBattleable pl, IBattleable mon)
        {
            BattleResultType resultType = BattleResultType.None;
            if (pl.IsSkillCharged && mon.IsSkillCharged)
            {
                resultType = BattleResult(pl.ChargedSkill.skillSo.type, mon.ChargedSkill.skillSo.type);
            }
            else if(pl.IsSkillCharged && !mon.IsSkillCharged)
            {
                resultType = BattleResultType.PlayerWin;
            }
            else if(!pl.IsSkillCharged && mon.IsSkillCharged)
            {
                resultType = BattleResultType.MonsterWin;
            }
            switch (resultType)
            {
                case BattleResultType.Draw:
                    //pl.StartCurSkillAnimation();
                    //pl.Stifness();
                    //mon.StartCurSkillAnimation();
                    //mon.Stifness();
                    pl.BattleDrawAction();
                    mon.BattleDrawAction();
                    RaiseBattleResult(BattleResultType.Draw);
                    break;
                case BattleResultType.PlayerWin:
                    //if (pl.CurSkillUse)
                    //{
                    //    mon.StartCurSkillAnimation();
                    //    mon.GetDamaged(pl.CurSkillUseDamage);
                    //    RaiseBattleResult(BattleResultType.PlayerWin);
                    //}
                    pl.BattleWinAction(mon);
                    break;
                case BattleResultType.MonsterWin:
                    //if (mon.CurSkillUse)
                    //{
                    //    pl.StartCurSkillAnimation();
                    //    pl.GetDamaged(mon.CurSkillUseDamage);
                    //    RaiseBattleResult(BattleResultType.MonsterWin);
                    //}
                    mon.BattleWinAction(pl);
                    break;
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
                            returnType = BattleResultType.PlayerWin;
                            break;
                        default:
                            returnType = BattleResultType.MonsterWin;
                            break;
                    }
                    break;
                case SkillType.Defense:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            returnType = BattleResultType.MonsterWin;
                            break;
                        case SkillType.Defense:
                            returnType = BattleResultType.None;
                            break;
                        default:
                            returnType = BattleResultType.PlayerWin;
                            break;
                    }
                    break;
                default:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            returnType = BattleResultType.PlayerWin;
                            break;
                        case SkillType.Defense:
                            returnType = BattleResultType.MonsterWin;
                            break;
                        default:
                            returnType = BattleResultType.Draw;
                            break;
                    }
                    break;
            }
            return returnType;
        }
        void CtsSet()
        {
            if (cts != null) return;
            cts = new CancellationTokenSource();
        }
        void CtsCancel()
        {
            if (cts == null) return;
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }
}