using System.Threading;
using ReadingStrike.Game.GameData;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Game.InGame
{
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
                tmp.text = $"{hp}";
            }
        }
        #endregion

        #region IBattleable Property
        public SkillSet ChargedSkill => sc.CurSkill; 
        public bool CurSkillUse => sc.SkillUse(); 
        public bool IsSkillCharged => sc.IsSkillCharged;
        public int CurSkillUseDamage => (int)(stat.atk * sc.CurSkill.data.power);
        public bool IsDeath { get; protected set; }
        public bool CheckBattleTiming { get; }
        public bool IsGetDamaged { get { return cAnim.isGetDamaged; } set { cAnim.isGetDamaged = value; } }
        #endregion

        #region GetComponent Target
        [SerializeField] protected CharacterAnimController cAnim;
        [SerializeField] protected SkillController sc;
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected CharacterControllerMove ccm;
        #endregion

        #region Other
        [SerializeField] protected LayerMask targetLm;
        protected CancellationTokenSource cts;
        public TextMeshProUGUI tmp;
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
                ccm.InitSetting(stat.moveSpeed);
            }
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
                switch (ChargedSkill.data.type)
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
            if (Physics.Raycast(rb.transform.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, targetLm))
            {
                if (hit.rigidbody.TryGetComponent(out Character temp))
                {
                    BattleManager.instance.BattleStart(this, temp);
                }
                else
                {
                    Debug.Log("IBattleable 없음");
                }
            }
        }
        #endregion
        #endregion
    }
}