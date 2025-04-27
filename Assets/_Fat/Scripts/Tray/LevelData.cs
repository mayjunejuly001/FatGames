using UnityEngine;

namespace FatTray
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "SO/Levels/Level Data")]
    public class LevelData : ScriptableObject
    {
        public TraySpawnData[] traySpawnDatas;
    }
}
