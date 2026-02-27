using UnityEngine;
using UnityEngine.UI;
namespace ReadingStrike.Game.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] Slider hpBar;
        public void HPBarValueSet(float value)
        {
            hpBar.value = value;
        }
    }
}