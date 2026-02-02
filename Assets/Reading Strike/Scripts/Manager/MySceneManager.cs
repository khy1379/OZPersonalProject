using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ReadingStrike.Manager
{
    public enum SceneType
    {
        Title,
        Village,
        Dungeon
    }
    public class SceneChangeEvent
    {
        public event Action<int> RequestSceneChange;
        public void RaiseRequesetSceneChange(int sceneNum) { RequestSceneChange?.Invoke(sceneNum); }
    }
    public class MySceneManager : MonoBehaviour
    {
        public bool isSceneChanging;
        public SceneType sceneType;
        SceneChangeEvent changeEvent;
        void Awake()
        {
            sceneType = SceneType.Title;
            changeEvent = new SceneChangeEvent();
        }
        public void SceneChangeStartCo(int index)
        {
            StartCoroutine(SceneChangeCo(index));
        }
        IEnumerator SceneChangeCo(int index)
        {
            if (!IsSceneChangePossible(index)) yield break;
            isSceneChanging = true;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
            while (!asyncLoad.isDone) yield return null;

            isSceneChanging = false;
            Debug.Log(SceneManager.GetActiveScene().name);
            changeEvent.RaiseRequesetSceneChange(index);
        }
        bool IsSceneChangePossible(int index)
        {
            if (isSceneChanging)
            {
                Debug.Log("Scene 변경중");
                return false;
            }
            else if (SceneManager.GetActiveScene().buildIndex == index)
            {
                Debug.Log("같은 Scene으로 이동 불가");
                return false;
            }
            else
                return true;
        }
        public void AddRequestSceneChange(Action<int> func) { changeEvent.RequestSceneChange += func; }
    }
}