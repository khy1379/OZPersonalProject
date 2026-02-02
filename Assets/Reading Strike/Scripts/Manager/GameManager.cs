using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        MySceneManager sceneMgr;
        SoundManager soundMgr;
        private void Awake()
        {
            if (instance != null) Destroy(gameObject);

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            sceneMgr = GetComponent<MySceneManager>();
            soundMgr = GetComponent<SoundManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneChange(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneChange(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneChange(2);
        }
        #region SceneChange 함수
        void SceneChange(int index)
        {
            sceneMgr.SceneChangeStartCo(index);
        }
        #endregion
    }
}