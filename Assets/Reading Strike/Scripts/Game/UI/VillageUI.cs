using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.UI
{
    public class VillageUI : BtnSetter
    {
        [SerializeField] List<TextMeshProUGUI> btnTxtList;
        byte isBtnSelectedLayer;
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