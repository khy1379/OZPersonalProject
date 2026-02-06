using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using ReadingStrike.Skill;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Monster
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask plLm = 1 << 6;
        [SerializeField] private SkillController sc;
        private int[] usePossibleSkillArr;
        public SkillSet ChargedSkill { get { return sc.CurSkill; } }
        public bool IsSkillCharged { get { return sc.IsSkillCharged; } }
        [SerializeField] private Collider[] searchedPl = new Collider[1];
        [SerializeField] private bool isPlSearched = false;
        [SerializeField] private bool isSkillChargingTaskStart = false;
        [SerializeField] private CancellationTokenSource tokenS;
        #region 상속 대상
        [SerializeField] protected float searchRadius = 5f;
        [SerializeField] private int hp = 100;
        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                if (hp <= 0) hp = 0;
                tmp.text = $"{hp}";
                if (hp == 0) Destroy(gameObject);
            }
        }
        [SerializeField] private int atk = 10;
        public TextMeshProUGUI tmp;
        public int Atk { get { return atk; } }

        #endregion
        private void Start()
        {
            usePossibleSkillArr = new int[sc.SkillCount];
        }
        void Update()
        {
            //PlayerSearching();
            //SkillUseSearching();
            TestSkillCharging();
        }
        private void OnDestroy()
        {
            SkillChgargingTaskCancel();
        }
        void TestSkillCharging()
        {
            if (IsSkillCharged) return;
            sc.SkillCharging(0);
        }
        void PlayerSearching()
        {
            if (sc.IsStifness) return;
            isPlSearched = 0 < Physics.OverlapSphereNonAlloc(transform.position, searchRadius, searchedPl, plLm);
            if (isPlSearched)
            {
                if (isSkillChargingTaskStart) return;
                StartSkillCharhingTask();
            }
            else
            {
                if (!isSkillChargingTaskStart) return;
                SkillChgargingTaskCancel();
            }
        }
        int SkillUsePossibleNum()
        {
            int index = 0;
            for (int i = 0; i < sc.SkillCount; i++)
            {
                usePossibleSkillArr[i] = 0;
                if (!sc.SkillSetList[i].isCooltime)
                {
                    usePossibleSkillArr[index++] = i;
                }
            }
            return index;
        }
        void StartSkillCharhingTask()
        {
            SkillChgargingTaskCancel();
            tokenS = new CancellationTokenSource();
            SkillChgargingTask().Forget();
        }
        async UniTaskVoid SkillChgargingTask()
        {
            isSkillChargingTaskStart = true;
            try
            {
                while (isPlSearched)
                {
                    sc.SkillCharging(Random.Range(1, sc.SkillCount));
                    await UniTask.Delay(1000, cancellationToken: tokenS.Token);
                }
            }
            finally
            {
                sc.SkillCancel();
                isSkillChargingTaskStart = false;
            }
        }
        void SkillUseSearching()
        {
            if (Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, plLm))
            {
                BattleManager.BattleStart(hit.collider.GetComponent<Player.Player>(), this, 1);
            }
        }
        void SkillChgargingTaskCancel()
        {
            if (tokenS == null) return;

            tokenS.Cancel();
            tokenS.Dispose();
            tokenS = null;
        }
        public void MonHit(int damage)
        {
            Hp -= damage;
            Stifness();
            if (Hp < 0)
            {
                Destroy(gameObject);
            }
        }
        public void Stifness()
        {
            sc.StartStifnessTask();
            SkillChgargingTaskCancel();
        }
        public bool CurSkillUse()
        {
            return sc.SkillUse();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
            //Gizmos.DrawLine(rb.position, rb.position + rb.transform.forward * sc.searchedDistance);
        }
    }
}