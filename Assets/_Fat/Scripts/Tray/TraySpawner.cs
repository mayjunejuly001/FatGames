using UnityEngine;

namespace FatTray
{
    public class TraySpawner : MonoBehaviour
    {
        [SerializeField] private Tray baseTrayPrefab;
        [SerializeField] private float trayColliderSizeOffset = -0.1f;

        public void Initialize(TraySpawnData[] traySpawnData)
        {
            foreach (var sd in traySpawnData)
            {
                var trayObject = Instantiate(baseTrayPrefab, new Vector3(sd.coordinate.x, 0, sd.coordinate.y), Quaternion.identity);
                trayObject.transform.SetParent(transform);
                trayObject.Initialize(sd.trayData, trayColliderSizeOffset);

                trayObject.name = $"Tray {sd.coordinate.x}, {sd.coordinate.y}_{sd.trayData.size}";
            }
        }
    }

    [System.Serializable]
    public class TraySpawnData
    {
        public Vector2Int coordinate;
        public TrayData trayData;
    }
}
