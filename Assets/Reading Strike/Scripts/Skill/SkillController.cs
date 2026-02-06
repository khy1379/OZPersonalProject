using Cysharp.Threading.Tasks;
using ReadingStrike.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public Color CurColor { get { return rend.material.color; } }
        [SerializeField] private MeshRenderer rend;
        [SerializeField] public int SkillCount { get { return skillSetList.Count; } }
        public bool IsSkillCharged { get; private set; }
        public bool IsStifness { get; private set; }
        bool isAnounceSkillCancelState = false;
        public float searchedDistance = 1f;
        private void Start()
        {
            rend.material.color = Color.white;
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
            isAnounceSkillCancelState = false;
            rend.material.color = skillSetList[index].color;
            IsSkillCharged = true;
            Debug.Log($"{index}번 스킬 차징");
        }

        public void SkillCancel()
        {
            if (!IsSkillCharged)
            {
                if (!isAnounceSkillCancelState)
                {
                    Debug.LogWarning("스킬 차징된 상태가 아님");
                    isAnounceSkillCancelState = true;
                }
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
            rend.material.color = Color.white;
        }
        async UniTaskVoid StifnessTask()
        {
            SkillReset();
            IsStifness = true;
            await UniTask.Delay((int)(CurSkill.stifnessTime * 1000));
            IsStifness = false;
        }
        public void StartStifnessTask()
        {
            StifnessTask().Forget();
        }
        async UniTaskVoid CooltimeTask()
        {
            SkillSet temp = CurSkill;
            temp.isCooltime = true;
            await UniTask.Delay((int)(temp.cooltime * 1000));
            temp.isCooltime = false;
        }
        void StartCooltimeTask()
        {
            CooltimeTask().Forget();
        }
    }
}