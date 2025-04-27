using UnityEngine;

namespace FatTray.Common
{
    public class Utilities
    {
        public static Color GetDarkerColor(Color originalColor, float darknessFactor)
        {
            darknessFactor = Mathf.Clamp01(darknessFactor);

            return new Color(
                originalColor.r * (1f - darknessFactor),
                originalColor.g * (1f - darknessFactor),
                originalColor.b * (1f - darknessFactor),
                originalColor.a
            );
        }
    }
}
