using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReadingStrike.Game.InGame
{
    public class TitleUI : UIManager
    {
        [SerializeField] GameObject startTxt;
        protected override void StartFeat()
        {
            if (GameManager.instance != null) BtnOnClickEventSetting(0, GameManager.instance.SceneChangeVillage);
            if (startTxt != null) BtnOnClickEventSetting(0, () => startTxt.SetActive(false));
        }
        public override void DestroyFeat()
        {
            BtnOnClickEventRemover(0);
        }
    }
}
