using UnityEngine;

namespace FatTray
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private TraySpawner traySpawner;
        [SerializeField] private LevelData levelData;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            traySpawner.Initialize(levelData.traySpawnDatas);
        }
    }
}
