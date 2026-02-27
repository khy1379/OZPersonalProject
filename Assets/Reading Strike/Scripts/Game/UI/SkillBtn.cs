using ReadingStrike.Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.InGame
{
    public class SkillBtn : BtnSetter
    {
        [SerializeField] List<Image> skillMaskList;
        private void Start()
        {
            GameManager.instance.Pl.AddEventSkillUseImpossible(SkillMaskShow);
            GameManager.instance.Pl.AddEventSkillUsePossible(SkillMaskHide);
            for (int i = 0; i < 3; i++)
            {
                int targetIndex = i;
                BtnOnClickEventSetting(i, () => GameManager.instance.Pl.Sc.SkillCharging(targetIndex));
            }
        }
        public void SkillMaskShow(int index)
        {
            if (skillMaskList[index] == null) return;
            skillMaskList[index].gameObject.SetActive(true);
        }
        public void SkillMaskHide(int index)
        {
            if (skillMaskList[index] == null) return;
            skillMaskList[index].gameObject.SetActive(false);
        }
        /*public void SkillMaskFill(int index)
        {
            if (!skillMaskList[index].gameObject.activeSelf || 1 <= skillMaskList[index].fillAmount) return;
            skillMaskList[index].fillAmount += Time.deltaTime;
            if (1 < skillMaskList[index].fillAmount) skillMaskList[index].fillAmount = 1;
        }*/
        protected override void OnDestroyFeat()
        {
            base.OnDestroyFeat();
            GameManager.instance.Pl.RemoveEventSkillUseImpossible(SkillMaskShow);
            GameManager.instance.Pl.RemoveEventSkillUsePossible(SkillMaskHide);
        }
    }
}