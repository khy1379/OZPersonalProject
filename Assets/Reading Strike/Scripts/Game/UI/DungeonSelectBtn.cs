using ReadingStrike.Game.GameData;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.UI
{
    public class DungeonSelectBtn : BtnSetter
    {
        [SerializeField] List<TextMeshProUGUI> btnTxtList;
        [SerializeField] Image showDungeonImg;
        [SerializeField] Sprite initDungeonImg;
        public void DungeonSelectBtnSetting(int index, DungeonData dd)
        {
            if (6 <= index) return;
            btnList[index].gameObject.SetActive(true);
            btnList[index].interactable = true;
            btnTxtList[index].text = dd.dType.ToString();
            BtnOnClickEventSetting(index, () => DungeonSelect(index, dd.dImage));
        }
        public void DungeonSelectBtnSetting(int index, DungeonData dd, Action func)
        {
            DungeonSelectBtnSetting(index, dd);
            BtnOnClickEventSetting(index, func);
        }
        public void DungeonSelectBtnEventClear(int index)
        {
            if (!btnList[index].gameObject.activeSelf) return;
            BtnOnClickEventRemover(index);
            btnList[index].gameObject.SetActive(false);
        }
        public void DungeonSelectBtnEventAllClear()
        {
            for (int i = 0; i < 6; i++)
            {
                DungeonSelectBtnEventClear(i);
            }
        }
        public void DungeonSelect(int index, Sprite targetImg)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!btnList[i].gameObject.activeSelf)
                    break;
                else if (i == index)
                {
                    btnList[i].interactable = false;
                    showDungeonImg.sprite = targetImg;
                }
                else
                    btnList[i].interactable = true;
            }
            if (!btnList[6].interactable)
                btnList[6].interactable = true;
        }
        public void AllClear()
        {
            DungeonSelectBtnEventAllClear();
            showDungeonImg.sprite = initDungeonImg;
        }
    }
}