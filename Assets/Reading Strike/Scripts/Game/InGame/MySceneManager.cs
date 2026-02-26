using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ReadingStrike.Game.GameData;

namespace ReadingStrike.Game.InGame
{
    public class SceneChangeEvent
    {
        public event Action<SceneType> RequestSceneChange;
        public void RaiseRequesetSceneChange(SceneType type) { RequestSceneChange?.Invoke(type); }
    }
    public class MySceneManager : MonoBehaviour
    {
        public bool IsSceneChanging { get; private set; }
        public SceneType sceneType = SceneType.Title;
        SceneChangeEvent changeEvent = new SceneChangeEvent();

        [SerializeField] Image fadeImg;
        float curFadeAlpha;
        [SerializeField] float fadeSpeed;
        public void SceneChangeStartCo(SceneType type)
        {
            SceneChangeTask(type, this.GetCancellationTokenOnDestroy()).Forget();
        }
        async UniTaskVoid SceneChangeTask(SceneType type, CancellationToken token)
        {
            try
            {
                string sceneName = type.ToString();
                if (!IsSceneChangePossible(sceneName)) return;
                IsSceneChanging = true;
                fadeImg.gameObject.SetActive(true);
                while(curFadeAlpha < 1)
                {
                    FadeOut();
                    await UniTask.Yield();
                }
                await SceneManager.LoadSceneAsync(sceneName);
                FadeReset();
                fadeImg.gameObject.SetActive(false);
                IsSceneChanging = false;
                changeEvent.RaiseRequesetSceneChange(type);
            }
            catch
            {
                Debug.LogWarning("Scene 변경 실패");
            }
        }
        void FadeOut()
        {
            curFadeAlpha = fadeImg.color.a;
            //curFadeAlpha += curFadeAlpha == 0 ? fadeSpeed : (curFadeAlpha * 1.1f);
            curFadeAlpha += fadeSpeed;
            if (1 < curFadeAlpha) curFadeAlpha = 1;
            fadeImg.color = new Color(0, 0, 0, curFadeAlpha);
        }
        void FadeReset()
        {
            curFadeAlpha = 0;
            fadeImg.color = new Color(0, 0, 0, 0);
        }
        bool IsSceneChangePossible(string sceneName)
        {
            if (IsSceneChanging)
            {
                Debug.Log("Scene 변경중");
                return false;
            }
            else if (SceneManager.GetActiveScene().name == sceneName)
            {
                Debug.Log("같은 Scene으로 이동 불가");
                return false;
            }
            else return true;
        }
        public void AddRequestSceneChange(Action<SceneType> func) { changeEvent.RequestSceneChange += func; }
    }
}