using ReadingStrike.Manager;
using ReadingStrike.Skill;
using ReadingStrike.SOFrame;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Character
{
    public abstract class Character : MonoBehaviour, IBattleable
    {
        #region Status
        [SerializeField] protected StatusSO stat;
        [SerializeField] protected int hp;
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                else if (stat.maxHp < hp) hp = stat.maxHp;
                tmp.text = $"{hp}";
            }
        }
        public int Atk { get { return stat.atk; } }
        #endregion

        #region Cur State
        protected bool isDeath;

        #endregion

        #region Skill Controller
        public SkillSet ChargedSkill { get { return sc.CurSkill; } }
        public bool CurSkillUse { get { return sc.SkillUse(); } }
        public bool IsSkillCharged { get { return sc.IsSkillCharged; } }
        #endregion

        #region Other
        [SerializeField] protected LayerMask targetLm;

        [SerializeField] protected Animator anim;
        [SerializeField] protected SkillController sc;
        [SerializeField] protected Rigidbody rb;

        protected CancellationTokenSource cts;

        public TextMeshProUGUI tmp;
        #endregion


        private void Start()
        {
            EventSubscribe();
            StartSetting();
        }
        private void Update()
        {
            UpdateFeat();
        }
        private void FixedUpdate()
        {
            FixedUpdateFeat();
        }
        protected abstract void UpdateFeat();
        protected abstract void FixedUpdateFeat();
        protected virtual void EventSubscribe()
        {
            BattleManager.RequestBattleResult += BattleResult;
        }
        protected virtual void StartSetting()
        {
            hp = stat.maxHp;
        }
        protected virtual void BattleResult(BattleResultType resultType)
        {
            switch (resultType)
            {
                case BattleResultType.PlayerWin:
                    break;
                case BattleResultType.MonsterWin:
                    break;
                case BattleResultType.Draw:
                    break;
            }
        }
        public virtual void GetDamaged(int damage)
        {
            if (sc.IsStifness || isDeath) return;
            Hp -= damage;
            if (Hp == 0)
            {
                isDeath = true;
                anim.SetTrigger("Death");
                sc.OrbSetFalse();
            }
            else if (0 < Hp)
            {
                Stifness();
                anim.SetTrigger("Damaged");
            }
        }
        public virtual void Stifness() { sc.StartStifnessTask(); }
    }
}