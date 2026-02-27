using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadingStrike.Game.GameData;

namespace ReadingStrike.Game.InGame
{
    public class GameManagerEvent
    {


    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public SceneType CurSceneType { get; private set; }
        public InGameState CurInGameState { get; private set; }

        [SerializeField] private MySceneManager sceneMgr;
        [SerializeField] private SoundManager soundMgr;

        GameManagerEvent gameMgrEvent = new GameManagerEvent();

        public GameObject volumePanel;
        public bool IsPause { get; private set; }
        public Player Pl { get; private set; }
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Application.targetFrameRate = 60;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            GameInit();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneChangeStart(SceneType.Title);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneChangeStart(SceneType.Village);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneChangeStart(SceneType.PlaneDungeon);
        }
        private void OnDestroy()
        {
            GameQuit();
        }
        #region Game 시작, 종료 시 함수
        void GameInit()
        {
            soundMgr.Init();
            AddEventSceneChange(SceneChangeCompletedFeat);
            AddEventSceneChangeCompleted(GameResume);
        }
        void GameQuit()
        {
            soundMgr.DestroyFeat();
            RemoveEventSceneChange(SceneChangeCompletedFeat);
            RemoveEventSceneChangeCompleted(GameResume);
        }
        #endregion
        #region Game 전반적 기능
        public void GamePause()
        {
            if (IsPause) return;
            IsPause = true;
            Time.timeScale = 0;
        }
        public void GameResume()
        {
            if (!IsPause) return;
            IsPause = false;
            Time.timeScale = 1;
        }
        #endregion
        #region Event 관련 함수
        public void AddEventSceneChange(Action<SceneType> func) { sceneMgr.AddEventSceneChange(func); }
        public void RemoveEventSceneChange(Action<SceneType> func) { sceneMgr.RemoveEventSceneChange(func); }
        public void AddEventSceneChangeCompleted(Action func) { sceneMgr.AddEventSceneChangeCompleted(func); }
        public void RemoveEventSceneChangeCompleted(Action func) { sceneMgr.RemoveEventSceneChangeCompleted(func); }
        #endregion
        #region SceneChange 함수
        public void SceneChangeStart(SceneType type) 
        { 
            sceneMgr.SceneChangeStartCo(type);
        }
        void SceneChangeCompletedFeat(SceneType type)
        {
            SceneType preType = CurSceneType;
            CurSceneType = type;
            Debug.Log($"{type} Scene으로 이동");

            switch (type)
            {
                case SceneType.Title:
                case SceneType.Village:
                    break;
                default:
                    if (Pl == null) Debug.Log("Player 없음");
                    Pl.AddEventDie(GamePause);
                    if (BattleManager.instance == null) Debug.Log("BattleManager 없음");
                    BattleManager.instance.AddEventPlayerWin(GamePause);
                    break;
            }
            switch (preType)
            {
                case SceneType.Title:
                case SceneType.Village:
                    break;
                default:
                    BattleManager.instance.RemoveEventPlayerWin(GamePause);
                    BattleManager.instance = null;
                    Pl.RemoveEventDie(GamePause);
                    Pl = null;
                    break;
            }
        }
        public void SceneChangeTitle() => SceneChangeStart(SceneType.Title); 
        public void SceneChangeVillage() => SceneChangeStart(SceneType.Village); 
        public void SceneChangeDungeon() => SceneChangeStart(SceneType.PlaneDungeon);
        #endregion
        #region 주입 관련 함수
        public void PlayerSetting(Player pl)
        {
            Pl = pl;
        }
        #endregion
    }
}