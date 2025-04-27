using FatTray.Common;
using UnityEngine;

namespace FatTray
{
    [CreateAssetMenu(fileName = "TrayData", menuName = "SO/Tray/Tray Data")]
    public class TrayData : ScriptableObject
    {
        public const float darknessFactor = 0.25f;

        public GameObject modelPrefab;
        public Color color = Color.white;
        public Vector2Int size = new Vector2Int(1, 1);
        
        public Color DarkerColor => Utilities.GetDarkerColor(color, darknessFactor);

        public void SetColor(Renderer[] renderers)
        {
            foreach (var r in renderers)
            {
                r.materials[0].SetColor("_BaseColor", color);
                r.materials[1].SetColor("_BaseColor", DarkerColor);
            }
        }
    }
}
