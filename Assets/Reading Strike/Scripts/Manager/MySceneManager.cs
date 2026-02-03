using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        public SceneType sceneType = SceneType.Title;
        SceneChangeEvent changeEvent = new SceneChangeEvent();
        public void SceneChangeStartCo(int index)
        {
            SceneChangeTask(index, this.GetCancellationTokenOnDestroy()).Forget();
        }
        async UniTaskVoid SceneChangeTask(int index, CancellationToken token)
        {
            if (!IsSceneChangePossible(index)) return;
            isSceneChanging = true;
            try
            {
                await SceneManager.LoadSceneAsync(index);

                isSceneChanging = false;
                Debug.Log(SceneManager.GetActiveScene().name);
                changeEvent.RaiseRequesetSceneChange(index);
            }
            catch
            {
                Debug.LogWarning("Scene 변경 실패");
            }
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
            else return true;
        }
        public void AddRequestSceneChange(Action<int> func) { changeEvent.RequestSceneChange += func; }
    }
}