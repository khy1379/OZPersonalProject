using ReadingStrike.Game.GameData;
using ReadingStrike.Game.InGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace ReadingStrike.Game.UI
{
    public class StatInfoUIManager : MonoBehaviour
    {
        [SerializeField] List<TextMeshProUGUI> statValueTextList;
        string[] statTextList;
        [SerializeField] bool isUpdate = true;
        public void StatShow()
        {
            if (statValueTextList == null) return;
            int cnt = statValueTextList.Count;
            if (isUpdate)
            {
                StatData stat = GameManager.instance.PlayerStat;
                if (statTextList == null)
                    statTextList = new string[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    if (statValueTextList[i] == null) continue;
                    string tempString = stat.StatValueText((StatType)i);
                    statTextList[i] = tempString;
                    statValueTextList[i].text = tempString;
                    //Debug.Log(tempString);
                }
                isUpdate = false;
            }
            else
            {
                for (int i = 0; i < cnt; i++)
                {
                    if (statValueTextList[i] == null) continue;
                    statValueTextList[i].text = statTextList[i];
                }
            }
        }
    }
}