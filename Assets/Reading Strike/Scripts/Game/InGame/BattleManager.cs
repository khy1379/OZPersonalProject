using System;
using UnityEngine;
using ReadingStrike.Game.GameData;
using System.Threading;
namespace ReadingStrike.Game.InGame
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager instance;
        public event Action<BattleResultType> RequestBattleResult;
        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            instance = this;
        }
        public void RaiseBattleResult(BattleResultType battleResult) { RequestBattleResult?.Invoke(battleResult); }
        public void BattleStart(Character AChar, Character BChar)
        {
            BattleResultType resultType = BattleResultType.None;
            if (AChar.IsSkillCharged && BChar.IsSkillCharged)
            {
                resultType = BattleResult(AChar.ChargedSkill.data.type, BChar.ChargedSkill.data.type);
            }
            else if(AChar.IsSkillCharged && !BChar.IsSkillCharged)
            {
                resultType = BattleResultType.AWin;
            }
            else if(!AChar.IsSkillCharged && BChar.IsSkillCharged)
            {
                resultType = BattleResultType.BWin;
            }
            switch (resultType)
            {
                case BattleResultType.Draw:
                    AChar.BattleDrawAction();
                    BChar.BattleDrawAction();
                    RaiseBattleResult(BattleResultType.Draw);
                    break;
                case BattleResultType.AWin:
                    AChar.BattleWinAction(BChar);
                    break;
                case BattleResultType.BWin:
                    BChar.BattleWinAction(AChar);
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
    }
}