using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.UI
{
    public enum VillageUIBtnType
    {
        DungeonSelect,
        Stat,
        Skill,
        //Item
    }
    public class VillageUI : BtnSetter
    {
        [SerializeField] TextMeshProUGUI uiTitleTxt;
        [SerializeField] List<GameObject> panelList;
        List<int> setBtnList;
        private void Start()
        {
            VillageUIBtnSet();
            PanelShow(VillageUIBtnType.DungeonSelect);
        }
        public void VillageUIBtnSet()
        {
            int enumCnt = System.Enum.GetValues(typeof(VillageUIBtnType)).Length;
            setBtnList = new List<int>(enumCnt);
            for (int i = 0; i < enumCnt; i++)
            {
                if (panelList.Count <= i)
                {
                    Debug.Log("Btn 세팅 완료");
                    break;
                }
                if (!btnList[i].gameObject.activeSelf)
                {
                    Debug.Log($"{(VillageUIBtnType)i}번 버튼 setting");
                    continue;
                }
                VillageUIBtnType getType = (VillageUIBtnType)i;
                BtnOnClickEventSetting(i, () => PanelShow(getType));
                Debug.Log($"{getType}번 버튼 setting");
                setBtnList.Add((int)getType);
            }
        }
        public void PanelShow(VillageUIBtnType type)
        {
            uiTitleTxt.text = type.ToString();
            VillageUIBtnType getType = type;
            int setBtnCnt = setBtnList.Count;
            for (int i = 0; i < setBtnCnt; i++)
            {
                if((VillageUIBtnType)setBtnList[i] == getType)
                {
                    panelList[setBtnList[i]].SetActive(true);
                    btnList[setBtnList[i]].interactable = false;
                }
                else
                {
                    panelList[setBtnList[i]].SetActive(false);
                    btnList[setBtnList[i]].interactable = true;
                }
            }
        }
    }
}