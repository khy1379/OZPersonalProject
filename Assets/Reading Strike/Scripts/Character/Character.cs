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
        public bool CurSkillUse
        {
            get
            {
                StartCurSkillAnimation(); 
                return sc.SkillUse();
            }
        }
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
            CtsCancel();
        }
        protected abstract void UpdateFeat();
        protected abstract void FixedUpdateFeat();
        protected virtual void StartSetting()
        {
            hp = stat.maxHp;
            cAnim.AddEventGetDamaged(GetDamageAnim);
        }
        public virtual void GetDamaged(int damage)
        {
            if (sc.IsStifness || IsDeath) return;
            Hp -= damage;
            IsGetDamaged = true;
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
        public void GetDamageAnim()
        {
            if (Hp == 0)
            {
                IsDeath = true;
                StartAnimation(AnimationTriggerType.Death);
                sc.OrbSetFalse();
            }
            else if (0 < Hp)
            {
                Stifness();
                StartAnimation(AnimationTriggerType.Damaged);
            }
        }
        #endregion

        #region Battle
        public virtual void Stifness() { sc.StartStifnessTask(); }
        #endregion

        #region CancellationToken
        protected void CtsSet()
        {
            if (cts != null) return;
            cts = new CancellationTokenSource();
        }
        protected void CtsCancel()
        {
            if (cts == null) return;
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
        #endregion
        #endregion
    }
}