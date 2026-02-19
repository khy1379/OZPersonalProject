using ReadingStrike.Manager;
using ReadingStrike.Skill;
using ReadingStrike.SOFrame;
using System.Threading;
using TMPro;
using UnityEngine;
using ReadingStrike.Animation;
namespace ReadingStrike.Character
{
    public abstract class Character : MonoBehaviour, IBattleable
    {
        #region Variable & Property
        protected virtual bool CheckPlayer { get { return false; } }
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
        #endregion

        #region IBattleable Property
        public SkillSet ChargedSkill { get { return sc.CurSkill; } }
        public bool CurSkillUse { get { return sc.SkillUse(); } }
        public bool IsSkillCharged { get { return sc.IsSkillCharged; } }
        public int CurSkillUseDamage { get { return (int)(stat.atk * sc.CurSkill.skillSo.power); } }
        public bool IsDeath { get; protected set; }
        public bool CheckBattleTiming { get; }
        public bool IsGetDamaged { get { return cAnim.isGetDamaged; } set { cAnim.isGetDamaged = value; } }
        #endregion

        #region Other
        [SerializeField] protected LayerMask targetLm;

        [SerializeField] protected CharacterAnimController cAnim;
        [SerializeField] protected SkillController sc;
        [SerializeField] protected Rigidbody rb;

        protected CancellationTokenSource cts;

        public TextMeshProUGUI tmp;
        #endregion

        #endregion

        #region Function

        private void Start()
        {
            hp = stat.maxHp;
            cAnim.OwnerSet(gameObject);
            cAnim.AddEventGetDamaged(GetDamagedAnim);
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
            CTSSetter.CTSCancel(ref cts);
        }
        protected abstract void UpdateFeat();
        protected abstract void FixedUpdateFeat();
        protected abstract void StartSetting();
        public virtual void GetDamaged(int damage)
        {
            if (IsDeath) return;
            Hp -= damage;
            IsGetDamaged = true;
            if (Hp <= 0)
            {
                IsDeath = true;
                sc.OrbSetFalse();
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
                switch (ChargedSkill.skillSo.type)
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
        public bool BattleWinAction(IBattleable cha)
        {
            StartCurSkillAnimation();
            if (!CurSkillUse) return false;

            cha.StartCurSkillAnimation();
            cha.GetDamaged(CurSkillUseDamage);
            return true;
        }
        #endregion
        #endregion
    }
}