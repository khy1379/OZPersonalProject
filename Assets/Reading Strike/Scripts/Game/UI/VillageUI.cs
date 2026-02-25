using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.InGame
{
    public class VillageUI : UIPanelBase
    {
        [SerializeField] List<TextMeshProUGUI> btnTxtList;
        [SerializeField] List<Image> imgList;
        byte isBtnSelectedLayer;
        protected override void StartFeat()
        {

        }
        public override void DestroyFeat()
        {

        }
        void DungeonSelected(int index)
        {
            if (isBtnSelectedLayer == 0)
            {
                return;
            }
            else if ((isBtnSelectedLayer & (1 << index)) == 1)
            {
                return;
            }
        }
    }
}