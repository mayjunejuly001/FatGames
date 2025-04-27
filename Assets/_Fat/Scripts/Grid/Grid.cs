using System;
using UnityEditor;
using UnityEngine;

namespace FatTray
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private SpriteRenderer gridGraphicsSprite;
        [Header("Debug")]
        [SerializeField] private bool debugGridCoorfinates = true;

        public Vector2Int GridSize => gridSize;

        public static event Action<Vector3, Vector2Int> OnGridInitialized;

        private void Start()
        {
            Initialize(gridSize);
        }

        private void Initialize(Vector2Int size)
        {
            gridGraphicsSprite.transform.position = new Vector3(size.x / 2f, 0, size.y / 2f);
            gridGraphicsSprite.size = new Vector2(size.x, size.y);

            OnGridInitialized?.Invoke(transform.position, size);
        }

        private void OnDrawGizmos()
        {
            if (!debugGridCoorfinates)
                return;
#if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;

            Handles.color = Color.red;
            for (int i = 0; i < gridSize.y; i++)
            {
                for (int j = 0; j < gridSize.x; j++)
                {
                    Handles.Label(new Vector3(j, 0, i), $"({j},{i})", style);
                }
            }
#endif
        }
    }
}
