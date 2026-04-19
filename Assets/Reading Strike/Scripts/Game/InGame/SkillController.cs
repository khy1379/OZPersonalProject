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
        public SkillData Data { get; private set; }
        public int settingNum;
        public float curCooltime;
        public bool isCooltime;
        public void SkillDataSet()
        {
            if (skillSo != null)
                Data = skillSo.Data;
            else
                Debug.Log("skillSo 없음");
        }
    }
    public class SkillEvent
    {
        public event Action<int> RequestSkillUseImpossible;
        //public event Action<int> RequestSkillCooltime;
        public event Action<int> RequestSkillUsePossible;
        public event Action RequestSkillCharging;
        public event Action RequestSkillCancel;
        public void RaiseSkillUseImpossible(int skillIndex) => RequestSkillUseImpossible?.Invoke(skillIndex);
        //public void RaiseSkillCooltime(int skillIndex) => RequestSkillCooltime?.Invoke(skillIndex);
        public void RaiseSkillUsePossible(int skillIndex) => RequestSkillUsePossible?.Invoke(skillIndex);
        public void RaiseSkillCharging() => RequestSkillCharging?.Invoke();
        public void RaiseSkillCancel() => RequestSkillCancel?.Invoke();
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
        CancellationTokenSource cts;
        SkillEvent se = new SkillEvent();

        [SerializeField] Material[] materials;
        private void Start()
        {
            SkillControllerInit();
        }
        private void OnDestroy()
        {
            CTSSetter.CTSCancel(ref cts);
        }
        void SkillControllerInit()
        {
            IsSkillCharged = false;
            IsStifness = false;
            for(int i = 0; i < SkillSetList.Count; i++)
            {
                SkillSetList[i].SkillDataSet();
            }
            if (CurSkill == null && SkillSetList != null) CurSkill = SkillSetList[0];
            if (skillOrbRend != null && !skillOrbRend.gameObject.activeSelf)
            {
                skillOrbRend.gameObject.SetActive(true);
                skillOrbRend.material = materials[0];
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
            skillOrbRend.material = skillSetList[index].Data.skillMat;
            IsSkillCharged = true;
            se.RaiseSkillCharging();
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
            skillOrbRend.material = materials[0];
            se.RaiseSkillCancel();
        }
        async UniTaskVoid StifnessTask()
        {
            if (IsStifness) return;
            try
            {
                CTSSetter.CTSSet(ref cts);
                SkillReset();
                IsStifness = true;
                for (int i = 0; i < 3; i++)
                {
                    if (skillSetList[i].isCooltime) continue;
                    se.RaiseSkillUseImpossible(i);
                }
                skillOrbRend.material = materials[1];
                float awaitTime = IsSkillCharged ? CurSkill.Data.stifnessTime : SkillSetList[0].Data.stifnessTime;

                await UniTask.Delay((int)(awaitTime * 1000), cancellationToken: cts.Token);

                skillOrbRend.material = materials[0];
                IsStifness = false;

                for (int i = 0; i < 3; i++)
                {
                    se.RaiseSkillUsePossible(i);
                }
            }
            catch (OperationCanceledException)
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
            try
            {
                SkillSet temp = CurSkill;
                CTSSetter.CTSSet(ref cts);
                temp.isCooltime = true;
                SkillType getType = temp.Data.type;
                int getTypeNum = (int)getType;
                se.RaiseSkillUseImpossible(getTypeNum);
                await UniTask.Delay((int)(temp.Data.cooltime * 1000), cancellationToken: cts.Token);

                temp.isCooltime = false;
                if (!IsStifness) se.RaiseSkillUsePossible(getTypeNum);
            }
            catch (OperationCanceledException)
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
            skillOrbRend.material = materials[1];
            skillOrbRend.gameObject.SetActive(false);
        }
        public void AddEventSkillUseImpossible(Action<int> func) => se.RequestSkillUseImpossible += func;
        public void AddEventSkillUsePossible(Action<int> func) => se.RequestSkillUsePossible += func;
        public void AddEventSkillCharging(Action func) => se.RequestSkillCharging += func;
        public void AddEventSkillCancel(Action func) => se.RequestSkillCancel += func;
        public void RemoveEventSkillUseImpossible(Action<int> func) => se.RequestSkillUseImpossible -= func;
        public void RemoveEventSkillUsePossible(Action<int> func) => se.RequestSkillUsePossible -= func;
        public void RemoveEventSkillCharging(Action func) => se.RequestSkillCharging -= func;
        public void RemoveEventSkillCancel(Action func) => se.RequestSkillCancel -= func;
    }
}