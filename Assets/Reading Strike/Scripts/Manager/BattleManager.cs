using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReadingStrike.Manager
{
    public static class BattleManager
    {
        public static event Action<BattleResultType> RequestBattleResult;
        public static void RaiseBattleResult(BattleResultType battleResult)
        {
            RequestBattleResult?.Invoke(battleResult);
        }
        public static void BattleStart(Player.Player pl, Monster.Monster mon, float stifnessTime)
        {
            if (pl.IsSkillCharged && mon.IsSkillCharged)
            {
                switch (BattleResult(pl.ChargedSkill.type, mon.ChargedSkill.type))
                {
                    case BattleResultType.Draw:
                        pl.Stifness();
                        mon.Stifness();
                        RaiseBattleResult(BattleResultType.Draw);
                        break;
                    case BattleResultType.PlayerWin:
                        if(pl.CurSkillUse())
                        {
                            mon.MonHit(pl.Atk);
                            RaiseBattleResult(BattleResultType.PlayerWin);
                        }
                        break;
                    case BattleResultType.MonsterWin:
                        if(mon.CurSkillUse())
                        {
                            pl.PlHit(mon.Atk);
                            RaiseBattleResult(BattleResultType.MonsterWin);
                        }
                        break;
                }
            }
            else if(pl.IsSkillCharged && !mon.IsSkillCharged)
            {
                if (pl.CurSkillUse())
                {
                    mon.MonHit(pl.Atk);
                    RaiseBattleResult(BattleResultType.PlayerWin);
                }
            }
            else if(!pl.IsSkillCharged && mon.IsSkillCharged)
            {
                if (mon.CurSkillUse())
                {
                    pl.PlHit(mon.Atk);
                    RaiseBattleResult(BattleResultType.MonsterWin);
                }
            }
            Debug.Log("전투 종료");
        }
        static BattleResultType BattleResult(SkillType plSkillType, SkillType monSkillType)
        {
            switch (plSkillType)
            {
                case SkillType.StrongAtk:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return BattleResultType.Draw;
                        case SkillType.Defense:
                            return BattleResultType.PlayerWin;
                        default:
                            return BattleResultType.MonsterWin;
                    }
                case SkillType.Defense:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return BattleResultType.MonsterWin;
                        case SkillType.Defense:
                            return BattleResultType.Draw;
                        default:
                            return BattleResultType.PlayerWin;
                    }
                default:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return BattleResultType.PlayerWin;
                        case SkillType.Defense:
                            return BattleResultType.MonsterWin;
                        default:
                            return BattleResultType.Draw;
                    }
            }
        }
    }
}