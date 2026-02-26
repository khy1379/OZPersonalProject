using UnityEngine;
using ReadingStrike.Game.InGame;
namespace ReadingStrike.Game.UI
{
    public class TitleBtn : BtnSetter
    {
        [SerializeField] GameObject startTxt;
        private void Start()
        {
            if (GameManager.instance != null)
            {
                BtnOnClickEventSetting(0, GameManager.instance.SceneChangeVillage);
            }
            if (startTxt != null)
            {
                BtnOnClickEventSetting(0, () => startTxt.SetActive(false));
            }
        }
    }
}
