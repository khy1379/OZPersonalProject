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
        #region Event 관련 함수
        public void AddRequestSceneChange(Action<SceneType> func) { sceneMgr.AddRequestSceneChange(func); }
        #endregion
        #region SceneChange 함수
        void SceneChange(SceneType type) 
        { 
            sceneMgr.SceneChangeStartCo(type);
            CurSceneType = type;
        }
        public void SceneChangeTitle() { sceneMgr.SceneChangeStartCo(SceneType.Title); }
        public void SceneChangeVillage() { sceneMgr.SceneChangeStartCo(SceneType.Village); }
        public void SceneChangeDungeon() { sceneMgr.SceneChangeStartCo(SceneType.PlaneDungeon); }
        #endregion
    }
}