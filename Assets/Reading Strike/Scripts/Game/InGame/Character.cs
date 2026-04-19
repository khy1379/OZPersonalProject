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
        public event Action<float> RequestHPChange;
        public void RaiseDie() => RequestDie?.Invoke();
        public void RaiseHPChange(float value) => RequestHPChange?.Invoke(value);
    }
    public abstract class Character : MonoBehaviour
    {
        #region Variable & Property
        protected virtual bool CheckPlayer => false;
        #region Stat
        [SerializeField] protected CharacterType type;
        protected StatData stat;
        [SerializeField] protected int hp;
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp < 0) hp = 0;
                else if (stat.maxHP < hp) hp = stat.maxHP;
                //Debug.Log(CurHPValue());
                //if (hpBar != null) hpBar.HPBarValueSet(hpFValue);
                ce.RaiseHPChange(CurHPValue());
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
        //[SerializeField] protected HPBar hpBar;
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
            StatSetting();
            hp = stat.maxHP;
            if (cAnim != null)
            {
                cAnim.OwnerSet(gameObject);
                cAnim.AddEventGetDamaged(GetDamagedAnim);
            }
            if (ccm != null)
            {
                ccm.InitSpeedSetting(stat.moveSpeed);
            }
            if (CheckPlayer) sc.AddEventSkillCharging(SkillRangeShow);
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
        protected virtual void StatSetting()
        {
            if (CheckPlayer)
                stat = GameManager.instance.GetPlayerStat();
            else
                stat = GameManager.instance.GetMonsterStat(type);
        }
        protected virtual void OnDestroySetting()
        {
            CTSSetter.CTSCancel(ref cts);
            sc.RemoveEventSkillCancel(SkillRangeHide);
        }
        public void GetDamaged(int damage)
        {
            if (IsDeath) return;
            if (!IsCritical())
            {
                damage -= stat.def;
            }
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
        // 지정된 확률 미만의 값이 나올 경우 크리티컬
        bool IsCritical()
        {
            int checkCritical = UnityEngine.Random.Range(0, 100);
            if (checkCritical < stat.criticalChance)
                return true;
            else return false;
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
                        BattleManager.instance.BattleStart(temp);
                    }
                    else
                    {
                        BattleManager.instance.BattleStart(this);
                    }
                }
            }
        }
        protected void SkillRangeShow()
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
        public void LookEnemy(Vector3 pos)
        {
            //Vector3 dir = pos - transform.position;
            transform.LookAt(pos);
        }
        public void HPValueSet()
        {
            if (CheckPlayer) DungeonUIManager.instanse.playerHPBar.HPBarValueSet(CurHPValue());
            else DungeonUIManager.instanse.monsterHPBar.HPBarValueSet(CurHPValue());
        }
        float CurHPValue()
        {
            return (float)hp / (float)stat.maxHP;
        }
        #endregion
        #region Event
        public void AddEventDie(Action func) => ce.RequestDie += func;
        public void RemoveEventDie(Action func) => ce.RequestDie -= func;
        public void AddEventChangeHP(Action<float> func) => ce.RequestHPChange += func;
        public void RemoveEventChangeHP(Action<float> func) => ce.RequestHPChange -= func;
        #endregion

        #endregion
    }
}