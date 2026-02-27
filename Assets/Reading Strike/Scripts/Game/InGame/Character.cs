using System;
using System.Threading;
using ReadingStrike.Game.GameData;
using ReadingStrike.Game.UI;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Game.InGame
{
    public class CharacterEvent
    {
        public event Action RequestDie;
        public void RaiseDie() => RequestDie?.Invoke();
    }
    public abstract class Character : MonoBehaviour
    {
        #region Variable & Property
        protected virtual bool CheckPlayer => false;
        #region Stat
        [SerializeField] protected StatSO statSO;
        protected StatData stat;
        [SerializeField] protected int hp;
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                else if (stat.maxHp < hp) hp = stat.maxHp;
                float hpFValue = (float)hp / (float)stat.maxHp;
                Debug.Log(hpFValue);
                if (hpBar != null) hpBar.HPBarValueSet(hpFValue);
            }
        }
        #endregion

        #region IBattleable Property
        public bool IsMove { get; protected set; }
        public SkillSet ChargedSkill => sc.CurSkill;
        public bool CurSkillUse => sc.SkillUse();
        public bool IsSkillCharged => sc.IsSkillCharged;
        public int CurSkillUseDamage => (int)(stat.atk * sc.CurSkill.Data.power);
        public bool IsDeath { get; protected set; }
        public bool CheckBattleTiming { get; }
        public bool IsGetDamaged { get { return cAnim.isGetDamaged; } set { cAnim.isGetDamaged = value; } }
        #endregion

        #region event
        CharacterEvent ce = new CharacterEvent();

        #endregion

        #region GetComponent Target
        [SerializeField] protected CharacterAnimController cAnim;
        [SerializeField] protected SkillController sc;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected CharacterControllerMove ccm;
        [SerializeField] protected HPBar hpBar;
        #endregion

        #region Other
        [SerializeField] protected SpriteRenderer skillRange;
        [SerializeField] protected LayerMask targetLm;
        protected CancellationTokenSource cts;
        #endregion

        #endregion

        #region Function

        private void Start()
        {
            if (statSO != null)
            {
                stat = statSO.Data;
                hp = stat.maxHp;
            }
            if (cAnim != null)
            {
                cAnim.OwnerSet(gameObject);
                cAnim.AddEventGetDamaged(GetDamagedAnim);
            }
            if (ccm != null)
            {
                ccm.InitSpeedSetting(stat.moveSpeed);
            }
            sc.AddEventSkillCharging(SkillRangeShow);
            sc.AddEventSkillCancel(SkillRangeHide);
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
        private void OnDestroy()
        {
            OnDestroySetting();
        }
        protected abstract void UpdateFeat();
        protected abstract void FixedUpdateFeat();
        protected abstract void StartSetting();
        protected virtual void OnDestroySetting()
        {
            CTSSetter.CTSCancel(ref cts);
            sc.RemoveEventSkillCharging(SkillRangeShow);
            sc.RemoveEventSkillCancel(SkillRangeHide);
        }
        public void GetDamaged(int damage)
        {
            if (IsDeath) return;
            Hp -= damage;
            IsGetDamaged = true;
            if (Hp <= 0)
            {
                IsDeath = true;
                sc.OrbSetFalse();
                ce.RaiseDie();
            }
            else
            {
                Stifness();
            }
        }
        #region Animation
        public void StartAnimation(AnimationTriggerType type)
        {
            cAnim.SetAnimTrigger(type.ToString());
        }
        public void StartCurSkillAnimation()
        {
            if (IsSkillCharged)
            {
                switch (ChargedSkill.Data.type)
                {
                    case SkillType.NormalAtk:
                        StartAnimation(AnimationTriggerType.NormalAtk);
                        break;
                    case SkillType.StrongAtk:
                        StartAnimation(AnimationTriggerType.StrongAtk);
                        break;
                    case SkillType.Defense:
                        StartAnimation(AnimationTriggerType.Defense);
                        break;
                }
            }
            else
            {
                StartAnimation(AnimationTriggerType.Idle);
            }
        }
        public virtual void GetDamagedAnim()
        {
            if (IsDeath)
            {
                StartAnimation(AnimationTriggerType.Death);
                cAnim.StartDeathAnimTask(CheckPlayer);
            }
            else
            {
                StartAnimation(AnimationTriggerType.Damaged);
            }
        }
        #endregion

        #region Battle
        public virtual void Stifness() { sc.StartStifnessTask(); }
        public void BattleDrawAction()
        {
            StartCurSkillAnimation();
            Stifness();
        }
        public bool BattleWinAction(Character cha)
        {
            StartCurSkillAnimation();
            if (!CurSkillUse) return false;

            cha.StartCurSkillAnimation();
            cha.GetDamaged(CurSkillUseDamage);
            return true;
        }
        protected void SkillUseSearching()
        {
            if (!IsSkillCharged) return;
            if (Physics.Raycast(rb.transform.position, rb.transform.forward, out RaycastHit hit, sc.CurSkill.Data.skillRange, targetLm))
            {
                if (hit.rigidbody.TryGetComponent(out Character temp))
                {
                    if (CheckPlayer)
                    {
                        BattleManager.instance.BattleStart(this, temp);
                    }
                    else
                    {
                        BattleManager.instance.BattleStart(temp, this);
                    }
                }
            }
        }
        void SkillRangeShow()
        {
            skillRange.gameObject.SetActive(true);
            float sRange = sc.CurSkill.Data.skillRange;
            skillRange.transform.localScale = new Vector3(0.1f, sRange);
            skillRange.transform.localPosition = new Vector3(0, 0.01f, sRange / 2f);
            skillRange.material.color = sc.CurSkill.Data.skillMat.color;
        }
        void SkillRangeHide()
        {
            skillRange.gameObject.SetActive(false);
        }
        public virtual void MoveStop()
        {
            IsMove = false;
            cAnim.SetAnimFloat("Speed", 0);
        }
        #endregion
        #region Event
        public void AddEventDie(Action func) => ce.RequestDie += func;
        public void RemoveEventDie(Action func) => ce.RequestDie -= func;

        #endregion

        #endregion
    }
}