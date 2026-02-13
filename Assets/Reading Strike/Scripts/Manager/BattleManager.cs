using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadingStrike.Character;
using Cysharp.Threading.Tasks;
using System.Threading;
namespace ReadingStrike.Manager
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
                    pl.Stifness();
                    mon.Stifness();
                    RaiseBattleResult(BattleResultType.Draw);
                    break;
                case BattleResultType.PlayerWin:
                    if (pl.CurSkillUse)
                    {
                        mon.GetDamaged(pl.CurSkillUseDamage);
                        RaiseBattleResult(BattleResultType.PlayerWin);
                    }
                    break;
                case BattleResultType.MonsterWin:
                    if (mon.CurSkillUse)
                    {
                        pl.GetDamaged(mon.CurSkillUseDamage);
                        RaiseBattleResult(BattleResultType.MonsterWin);
                    }
                    break;
                default:
                    Debug.Log("전투 미실행");
                    break;
            }
        }
        //async UniTaskVoid BattleTask(IBattleable pl, IBattleable mon)
        //{
        //    if (pl.IsDeath || mon.IsDeath) return;
        //    CtsSet();
        //    pl.StartCurSkillAnimation();
        //    mon.StartCurSkillAnimation();

        //}
        //public void StartBattleTask(IBattleable pl, IBattleable mon)
        //{
        //    BattleTask(pl, mon).Forget();
        //}
        BattleResultType BattleResult(SkillType plSkillType, SkillType monSkillType)
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
                            return BattleResultType.None;
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