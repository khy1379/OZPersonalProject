using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ReadingStrike.Skill
{
    [Serializable]
    public class SkillSet
    {
        public int settingNum;
        public string name;
        public SkillType type;
        public bool isCooltime;
        public float cooltime;
        public float stifnessTime;
        public Color color;
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
                Debug.LogWarning($"{gameObject.name} 해당 스킬은 없음");
                return;
            }
            else if (IsSkillCharged && CurSkill.settingNum == index)
            {
                Debug.LogWarning($"{CurSkill.name} 이미 차징됨");
                return;
            }
            else if (IsStifness)
            {
                Debug.LogWarning($"{gameObject.name} 경직 상태");
                return;
            }
            else if (skillSetList[index].isCooltime)
            {
                Debug.LogWarning($"{CurSkill.name} 쿨타임");
                return;
            }
            CurSkill = skillSetList[index];
            skillOrbRend.material.color = skillSetList[index].color;
            IsSkillCharged = true;
            Debug.Log($"{index}번 스킬 차징");
        }

        public void SkillCancel()
        {
            if (!IsSkillCharged)
            {
                return;
            }
            SkillReset();
            Debug.Log("스킬 차징 취소");
        }
        public bool SkillUse()
        {
            if (!IsSkillCharged)
            {
                Debug.LogWarning("스킬 차징된 상태가 아님");
                return false;
            }
            SkillReset();
            StartCooltimeTask();
            Debug.Log($"{CurSkill.name} 스킬 사용");
            return true;
        }
        void SkillReset()
        {
            IsSkillCharged = false;
            skillOrbRend.material.color = Color.white;
        }
        async UniTaskVoid StifnessTask(CancellationTokenSource cts)
        {
            try
            {
                if (cts == null)
                {
                    cts = new CancellationTokenSource();
                }
                SkillReset();
                IsStifness = true;
                skillOrbRend.material.color = Color.gray;
                float awaitTime = IsSkillCharged ? CurSkill.stifnessTime : SkillSetList[0].stifnessTime;
                await UniTask.Delay((int)(awaitTime * 1000), cancellationToken: cts.Token);
            }
            catch
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
            finally
            {
                skillOrbRend.material.color = Color.white;
                IsStifness = false;
            }
        }
        public void StartStifnessTask()
        {
            StifnessTask(cts).Forget();
        }
        async UniTaskVoid CooltimeTask(CancellationTokenSource cts)
        {
            SkillSet temp = CurSkill;
            try
            {
                if (cts == null)
                {
                    cts = new CancellationTokenSource();
                }
                temp.isCooltime = true;
                await UniTask.Delay((int)(temp.cooltime * 1000), cancellationToken : cts.Token);
            }
            catch
            {
                cts.Cancel();
                cts.Dispose();
                cts = null;
            }
            finally
            {
                temp.isCooltime = false;
            }
        }
        void StartCooltimeTask()
        {
            CooltimeTask(cts).Forget();
        }
        public void OrbSetFalse()
        {
            skillOrbRend.material.color = Color.gray;
            skillOrbRend.gameObject.SetActive(false);
        }
    }
}