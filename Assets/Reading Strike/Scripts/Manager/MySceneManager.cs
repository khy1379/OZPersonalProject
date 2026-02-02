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
    public class MySceneManager : MonoBehaviour
    {
        public bool isSceneChanging;
        public SceneType sceneType;
        void Awake()
        {
            sceneType = SceneType.Title;
        }
        public void SceneChange(int index)
        {
            if (!IsSceneChangePossible(index)) return;
            isSceneChanging = true;
            SceneManager.LoadScene(index);
            sceneType = (SceneType)index;
            isSceneChanging = false;
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
    }
}