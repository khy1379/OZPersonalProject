using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Manager
{
    public class GameManagerEvent
    {
        public event Action RequestGameInit;
        public void RaiseRequesetGameInit() { RequestGameInit?.Invoke(); }

    }
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] private MySceneManager sceneMgr;
        [SerializeField] private SoundManager soundMgr;
        GameManagerEvent gameMgrEvent;
        private void Awake()
        {
            if (instance != null) Destroy(gameObject);

            instance = this;
            gameMgrEvent = new GameManagerEvent();
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            GameInit();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneChange(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneChange(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneChange(2);
        }
        #region Game 시작 시 함수
        void GameInit()
        {
            gameMgrEvent.RaiseRequesetGameInit();
        }
        #endregion
        #region Event 관련 함수
        public void AddRequestGameInit(Action func) { gameMgrEvent.RequestGameInit += func; }
        public void AddRequestSceneChange(Action<int> func) { sceneMgr.AddRequestSceneChange(func); }
        #endregion
        #region SceneChange 함수
        void SceneChange(int index)
        {
            sceneMgr.SceneChangeStartCo(index);
        }
        #endregion
    }
}