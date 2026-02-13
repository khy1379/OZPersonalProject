using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadingStrike.Character;
namespace ReadingStrike.Manager
{
    public static class BattleManager
    {
        public static event Action<BattleResultType> RequestBattleResult;
        public static void RaiseBattleResult(BattleResultType battleResult) { RequestBattleResult?.Invoke(battleResult); }
        public static void BattleStart(Player pl, Monster mon)
        {
            if (pl.IsSkillCharged && mon.IsSkillCharged)
            {
                switch (BattleResult(pl.ChargedSkill.skillSo.type, mon.ChargedSkill.skillSo.type))
                {
                    case BattleResultType.Draw:
                        pl.Stifness();
                        mon.Stifness();
                        RaiseBattleResult(BattleResultType.Draw);
                        break;
                    case BattleResultType.PlayerWin:
                        if(pl.CurSkillUse)
                        {
                            mon.GetDamaged(pl.Atk);
                            RaiseBattleResult(BattleResultType.PlayerWin);
                        }
                        break;
                    case BattleResultType.MonsterWin:
                        if(mon.CurSkillUse)
                        {
                            pl.GetDamaged(mon.Atk);
                            RaiseBattleResult(BattleResultType.MonsterWin);
                        }
                        break;
                }
            }
            else if(pl.IsSkillCharged && !mon.IsSkillCharged)
            {
                if (pl.CurSkillUse)
                {
                    mon.GetDamaged(pl.Atk);
                    RaiseBattleResult(BattleResultType.PlayerWin);
                }
            }
            else if(!pl.IsSkillCharged && mon.IsSkillCharged)
            {
                if (mon.CurSkillUse)
                {
                    pl.GetDamaged(mon.Atk);
                    RaiseBattleResult(BattleResultType.MonsterWin);
                }
            }
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