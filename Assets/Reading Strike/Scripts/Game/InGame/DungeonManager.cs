using ReadingStrike.Game.GameData;
using ReadingStrike.Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.InGame
{
    public class DungeonManager : MonoBehaviour
    {
        [SerializeField] List<DungeonDataSO> dungeonDataList;
        [SerializeField] List<DungeonData> curDungeonList = new List<DungeonData>(dungeonBtnCnt);
        [SerializeField] int curDungeonPageCnt;
        int maxPageCnt;

        [SerializeField] DungeonSelectBtn dsUI;
        const int dungeonBtnCnt = 6;
        [SerializeField] SceneType curSelectDungeonScene;
        void Start()
        {
            DungeonListShowSettingInit();
        }
        void DungeonListShowSettingInit()
        {
            if (dungeonDataList == null)
            {
                Debug.LogError("던전 데이터 없음");
                return;
            }
            if(dsUI == null)
            {
                Debug.LogError("던전UI 데이터 없음");
                return;
            }
            maxPageCnt = (dungeonDataList.Count / 6) + 1;
            DungeonListShowSetting();
            if(GameManager.instance != null)
            dsUI.BtnOnClickEventSetting(6, () => GameManager.instance.SceneChange(curSelectDungeonScene));
        }
        void DungeonListShowSetting()
        {
            curDungeonList.Clear();
            dsUI.DungeonSelectBtnEventAllClear();
            int saveDungeonCnt = dungeonDataList.Count;
            int dungeonShowCnt = curDungeonPageCnt * dungeonBtnCnt;
            for (int i = 0; i < dungeonBtnCnt; i++)
            {
                int addDungeonCnt = i + dungeonShowCnt;
                if (saveDungeonCnt <= addDungeonCnt) break;
                curDungeonList.Add(dungeonDataList[addDungeonCnt].Data);
                SceneType selectType = curDungeonList[i].dScene;
                dsUI.DungeonSelectBtnSetting(i, curDungeonList[i], () => SelectDungeon(selectType));
                //dsUI.DungeonSelectBtnSetting(i, curDungeonList[i]);
            }
        }
        void SelectDungeon(SceneType type)
        {
            curSelectDungeonScene = type;
        }
        public void NextPageShow()
        {
            if (maxPageCnt <= curDungeonPageCnt) return;
            curDungeonPageCnt++;
            DungeonListShowSetting();
        }
        public void PrevPageShow()
        {
            if (curDungeonPageCnt <= 0) return;
            curDungeonPageCnt--;
            DungeonListShowSetting();
        }
    }
}