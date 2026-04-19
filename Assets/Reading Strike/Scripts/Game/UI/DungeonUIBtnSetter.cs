using ReadingStrike.Game.InGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.UI
{
    public enum DungeonUIBtnType
    {
        PauseAndMenu,
        PauseClose,
        PausePanel,
        Volume,
        VillageMoveThroughPuase,
        VillageMoveThroughWin,
        VillageMoveThroughLose,
    }
    public class DungeonUIBtnSetter : BtnSetter
    {
        [SerializeField] List<GameObject> panelList;
        private void Start()
        {
            if(btnList == null || panelList == null)
            {
                Debug.LogError("버튼 등이 제대로 적용 안 됨");
                return;
            }
            BtnOnClickEventSetting((int)DungeonUIBtnType.PauseAndMenu, PausePanelShow);
            BtnOnClickEventSetting((int)DungeonUIBtnType.PauseClose, PausePanelClose);
            BtnOnClickEventSetting((int)DungeonUIBtnType.PausePanel, PausePanelClose);
            BtnOnClickEventSetting((int)DungeonUIBtnType.Volume, VolumePanelShow);
            for (int i = 4; i < btnList.Count; i++)
            {
                int targetBtnNum = i;
                BtnOnClickEventSetting(targetBtnNum, GameManager.instance.SceneChangeVillage);
            }
            panelList[1] = GameManager.instance.volumePanel;
            BattleManager.instance.AddEventPlayerWin(WinPanelShow);
            GameManager.instance.Pl.AddEventDie(LosePanelShow);
        }
        void PausePanelShow()
        {
            GameManager.instance.GamePause();
            panelList[0].SetActive(true);
        }
        void PausePanelClose()
        {
            GameManager.instance.GameResume();
            panelList[0].SetActive(false);
        }
        void VolumePanelShow()
        {
            panelList[1].SetActive(true);
        }
        void WinPanelShow()
        {
            panelList[2].gameObject.SetActive(true);
        }
        void LosePanelShow()
        {
            panelList[3].gameObject.SetActive(true);
        }
        protected override void OnDestroyFeat()
        {
            base.OnDestroyFeat();
            BattleManager.instance.RemoveEventPlayerWin(WinPanelShow);
            GameManager.instance.Pl.RemoveEventDie(LosePanelShow);
        }
    }
}