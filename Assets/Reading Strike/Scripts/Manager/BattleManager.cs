using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReadingStrike.Manager
{
    public static class BattleManager
    {
        public static void BattleStart(Player.Player pl, Monster.Monster mon, float stifnessTime)
        {
            if (pl.IsSkillCharged && mon.IsSkillCharged)
            {
                switch (BattleResult(pl.ChargedSkill.type, mon.ChargedSkill.type))
                {
                    case 0:
                        pl.Stifness();
                        mon.Stifness();
                        break;
                    case 1:
                        if(pl.CurSkillUse())
                        {
                            mon.MonHit(pl.Atk);
                        }
                        break;
                    case -1:
                        if(mon.CurSkillUse())
                        {
                            pl.PlHit(mon.Atk);
                        }
                        break;
                }
            }
            else if(pl.IsSkillCharged && !mon.IsSkillCharged)
            {
                if(pl.CurSkillUse()) mon.MonHit(pl.Atk);
            }
            else if(!pl.IsSkillCharged && mon.IsSkillCharged)
            {
                if(mon.CurSkillUse()) pl.PlHit(mon.Atk);
            }
            Debug.Log("전투 종료");
        }
        static int BattleResult(SkillType plSkillType, SkillType monSkillType)
        {
            switch (plSkillType)
            {
                case SkillType.StrongAtk:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return 0;
                        case SkillType.Defense:
                            return 1;
                        default:
                            return -1;
                    }
                case SkillType.Defense:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return -1;
                        case SkillType.Defense:
                            return 0;
                        default:
                            return 1;
                    }
                default:
                    switch (monSkillType)
                    {
                        case SkillType.StrongAtk:
                            return 1;
                        case SkillType.Defense:
                            return -1;
                        default:
                            return 0;
                    }
            }
        }
    }
}