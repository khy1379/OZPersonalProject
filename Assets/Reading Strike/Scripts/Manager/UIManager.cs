using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Manager
{
    public abstract class UIManager : MonoBehaviour
    {
        //[SerializeField] UISO uiList;

        [SerializeField] protected List<GameObject> uIList;
        protected abstract void BtnSetting();
        public abstract void BtnSelect(int index);
    }
}