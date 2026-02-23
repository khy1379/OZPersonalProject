using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.InGame
{
    public abstract class UIManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> panelList;
        [SerializeField] protected List<Button> btnList;
        public void BtnOnClickEventSetting(int index, Action func) { btnList[index].onClick.AddListener(() => func?.Invoke()); }
        public void BtnOnClickEventRemover(int index) { btnList[index].onClick.RemoveAllListeners(); }
        private void Start() { StartFeat(); }
        protected abstract void StartFeat();
        private void OnDestroy() { DestroyFeat(); }
        public abstract void DestroyFeat();
    }
}