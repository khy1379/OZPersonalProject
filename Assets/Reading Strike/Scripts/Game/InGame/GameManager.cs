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

        public bool IsPause { get; private set; }
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
                SceneChange(SceneType.Title);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneChange(SceneType.Village);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneChange(SceneType.PlaneDungeon);
        }
        #region Game 시작 시 함수
        void GameInit()
        {
            soundMgr.Init();
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
        public void AddRequestSceneChange(Action<SceneType> func) { sceneMgr.AddRequestSceneChange(func); }
        #endregion
        #region SceneChange 함수
        public void SceneChange(SceneType type) 
        { 
            sceneMgr.SceneChangeStartCo(type);
            CurSceneType = type;
        }
        public void SceneChangeTitle() { SceneChange(SceneType.Title); }
        public void SceneChangeVillage() { SceneChange(SceneType.Village); }
        public void SceneChangeDungeon() { SceneChange(SceneType.PlaneDungeon); }
        #endregion
    }
}