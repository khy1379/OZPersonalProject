using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.UI
{
    public class BtnSetter : MonoBehaviour
    {
        [SerializeField] protected List<Button> btnList;
        private void OnDestroy()
        {
            OnDestroyFeat();
        }
        public void BtnOnClickEventSetting(int index, Action func, bool isClear = false) 
        {
            if (isClear) btnList[index].onClick.RemoveAllListeners();
            btnList[index].onClick.AddListener(() => func?.Invoke()); 
        }
        public void BtnOnClickEventRemover(int index) 
        { 
            btnList[index].onClick.RemoveAllListeners(); 
        }
        void BtnOnClickEventAllRemover()
        {
            foreach (Button btn in btnList)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
        protected virtual void OnDestroyFeat()
        {
            BtnOnClickEventAllRemover();
        }
    }
}