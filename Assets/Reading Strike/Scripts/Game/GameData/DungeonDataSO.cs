using UnityEngine;
using UnityEngine.UI;

namespace ReadingStrike.Game.GameData
{
    [System.Serializable]
    public struct DungeonData
    {
        public int dIndex;
        public DungeonType dType;
        public SceneType dScene;
        public Sprite dImage;
    }
    [CreateAssetMenu(fileName = "DungeonDataSO", menuName = "SO/DungeonDataSO")]
    public class DungeonDataSO : ScriptableObject
    {
        [SerializeField] DungeonData data;
        public ref readonly DungeonData Data => ref data;
    }
}