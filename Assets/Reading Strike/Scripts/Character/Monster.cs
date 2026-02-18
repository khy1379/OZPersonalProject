using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using UnityEngine;
namespace ReadingStrike.Character
{
    public class Monster : Character
    {
        private int[] usePossibleSkillArr;
        [SerializeField] private Collider[] searchedPl = new Collider[1];
        [SerializeField] private bool isPlSearched = false;
        [SerializeField] private bool isSkillChargingTaskStart = false;
        [SerializeField] protected float searchRadius = 5f;

        protected override void StartSetting()
        {
            usePossibleSkillArr = new int[sc.SkillCount];
        }

        protected override void UpdateFeat()
        {
            TestSkillCharging();
        }

        protected override void FixedUpdateFeat()
        {

        }
        void TestSkillCharging()
        {
            if (IsSkillCharged) return;
            sc.SkillCharging(0);
        }
        void PlayerSearching()
        {
            if (sc.IsStifness) return;
            isPlSearched = 0 < Physics.OverlapSphereNonAlloc(transform.position, searchRadius, searchedPl, targetLm);
            if (isPlSearched)
            {
                if (isSkillChargingTaskStart) return;
                StartSkillCharhingTask();
            }
            else
            {
                if (!isSkillChargingTaskStart) return;
                CtsCancel();
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
            CtsCancel();
            CtsSet();
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
                    await UniTask.Delay(1000, cancellationToken: cts.Token);
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
            if (Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit, sc.searchedDistance, targetLm))
            {
                BattleManager.instance.BattleStart(hit.collider.GetComponent<IBattleable>(), this);
            }
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, searchRadius);
        //    //Gizmos.DrawLine(rb.position, rb.position + rb.transform.forward * sc.searchedDistance);
        //}
    }
}