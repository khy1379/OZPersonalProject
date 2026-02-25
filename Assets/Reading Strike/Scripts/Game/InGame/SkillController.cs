using Cysharp.Threading.Tasks;
using ReadingStrike.Game.GameData;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ReadingStrike.Game.InGame
{
    [Serializable]
    public class SkillSet
    {
        [SerializeField] SkillSO skillSo;
        public readonly SkillData data;
        public int settingNum;
        public float curCooltime;
        public bool isCooltime;
        public SkillSet()
        {
            if (skillSo != null)
                data = skillSo.Data;
        }
    }
    public class SkillController : MonoBehaviour
    {
        [SerializeField] List<SkillSet> skillSetList;
        public List<SkillSet> SkillSetList => skillSetList;
        public SkillSet CurSkill { get; private set; }
        [SerializeField] public int SkillCount => skillSetList.Count; 
        [SerializeField] private MeshRenderer skillOrbRend;
        public bool IsSkillCharged { get; private set; }
        public bool IsStifness { get; private set; }
        public float searchedDistance = 1f;
        CancellationTokenSource cts;

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
                return;
            }
            else if (IsSkillCharged && CurSkill.settingNum == index)
            {
                return;
            }
            else if (IsStifness)
            {
                return;
            }
            else if (skillSetList[index].isCooltime)
            {
                return;
            }
            CurSkill = skillSetList[index];
            skillOrbRend.material.color = skillSetList[index].data.color;
            IsSkillCharged = true;
        }

        public void SkillCancel()
        {
            if (!IsSkillCharged) return;
            SkillReset();
        }
        public bool SkillUse()
        {
            if (!IsSkillCharged)
            {
                return false;
            }
            SkillReset();
            StartCooltimeTask();
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
                float awaitTime = IsSkillCharged ? CurSkill.data.stifnessTime : SkillSetList[0].data.stifnessTime;

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

                await UniTask.Delay((int)(temp.data.cooltime * 1000), cancellationToken: cts.Token);

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