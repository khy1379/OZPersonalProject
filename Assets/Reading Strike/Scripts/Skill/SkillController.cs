using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using ReadingStrike.SOFrame;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ReadingStrike.Skill
{
    [Serializable]
    public class SkillSet
    {
        public int settingNum;
        public SkillSO skillSo;
        public float curCooltime;
        public bool isCooltime;
    }
    public class SkillController : MonoBehaviour
    {
        [SerializeField] List<SkillSet> skillSetList;
        public List<SkillSet> SkillSetList { get { return skillSetList; } }
        public SkillSet CurSkill { get; private set; }
        [SerializeField] public int SkillCount { get { return skillSetList.Count; } }
        [SerializeField] private MeshRenderer skillOrbRend;
        public bool IsSkillCharged { get; private set; }
        public bool IsStifness { get; private set; }
        public float searchedDistance = 1f;
        CancellationTokenSource cts;

        //byte isAnounceMask;
        private void Start()
        {
            SkillControllerInit();
        }
        void SkillControllerInit()
        {
            IsSkillCharged = false;
            IsStifness = false;
            if (CurSkill == null && SkillSetList != null) CurSkill = SkillSetList[0];
            if (skillOrbRend != null && !skillOrbRend.gameObject.activeSelf)
            {
                skillOrbRend.gameObject.SetActive(true);
                skillOrbRend.material.color = Color.white;
            }
        }
        public void SkillCharging(int index)
        {
            if (SkillCount <= index)
            {
                //if ((isAnounceMask & (1 << 0)) == 0)
                //{
                //    Debug.LogWarning($"{gameObject.name} 해당 스킬은 없음");
                //    isAnounceMask |= 1 << 0;
                //}
                return;
            }
            else if (IsSkillCharged && CurSkill.settingNum == index)
            {
                //if ((isAnounceMask & (1 << 1)) == 0)
                //{
                //    Debug.LogWarning($"{CurSkill.skillSo.name} 이미 차징됨");
                //    isAnounceMask |= 1 << 1;
                //}
                return;
            }
            else if (IsStifness)
            {
                //if ((isAnounceMask & (1 << 2)) == 0)
                //{
                //    Debug.LogWarning($"{gameObject.name} 경직 상태");
                //    isAnounceMask |= 1 << 2;
                //}
                return;
            }
            else if (skillSetList[index].isCooltime)
            {
                //if ((isAnounceMask & (1 << 3)) == 0)
                //{
                //    Debug.LogWarning($"{CurSkill.skillSo.name} 쿨타임");
                //    isAnounceMask |= 1 << 3;
                //}
                return;
            }
            //isAnounceMask = 0;
            CurSkill = skillSetList[index];
            skillOrbRend.material.color = skillSetList[index].skillSo.color;
            IsSkillCharged = true;
            //Debug.Log($"{index}번 스킬 차징");
        }

        public void SkillCancel()
        {
            if (!IsSkillCharged) return;
            SkillReset();
            //Debug.Log("스킬 차징 취소");
        }
        public bool SkillUse()
        {
            if (!IsSkillCharged)
            {
                //Debug.LogWarning("스킬 차징된 상태가 아님");
                return false;
            }
            SkillReset();
            StartCooltimeTask();
            //Debug.Log($"{CurSkill.skillSo.name} 스킬 사용");
            return true;
        }
        void SkillReset()
        {
            IsSkillCharged = false;
            skillOrbRend.material.color = Color.white;
        }
        async UniTaskVoid StifnessTask()
        {
            if (IsStifness) return;
            try
            {
                CTSSetter.CTSSet(ref cts);
                SkillReset();
                IsStifness = true;
                skillOrbRend.material.color = Color.gray;
                float awaitTime = IsSkillCharged ? CurSkill.skillSo.stifnessTime : SkillSetList[0].skillSo.stifnessTime;

                await UniTask.Delay((int)(awaitTime * 1000), cancellationToken: cts.Token);

                skillOrbRend.material.color = Color.white;
                IsStifness = false;
            }
            catch(OperationCanceledException)
            {
                CTSSetter.CTSCancel(ref cts);
            }
        }
        public void StartStifnessTask()
        {
            StifnessTask().Forget();
        }
        async UniTaskVoid CooltimeTask()
        {
            SkillSet temp = CurSkill;
            try
            {
                CTSSetter.CTSSet(ref cts);
                temp.isCooltime = true;

                await UniTask.Delay((int)(temp.skillSo.cooltime * 1000), cancellationToken: cts.Token);

                temp.isCooltime = false;
            }
            catch(OperationCanceledException)
            {
                CTSSetter.CTSCancel(ref cts);
            }
        }
        void StartCooltimeTask()
        {
            CooltimeTask().Forget();
        }
        public void OrbSetFalse()
        {
            skillOrbRend.material.color = Color.gray;
            skillOrbRend.gameObject.SetActive(false);
        }
    }
}