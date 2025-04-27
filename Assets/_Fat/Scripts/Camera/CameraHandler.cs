using System;
using UnityEngine;

namespace FatTray
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float camHeightMultiplier = 1.5f;

        private void OnEnable()
        {
            Grid.OnGridInitialized += InitializeCamera;
        }

        private void OnDisable()
        {
            Grid.OnGridInitialized -= InitializeCamera;
        }

        private void InitializeCamera(Vector3 gridPosition, Vector2Int gridSize)
        {
            cam.transform.position = new Vector3(gridPosition.x + gridSize.x / 2f, Math.Max(gridSize.x, gridSize.y) / 2f * camHeightMultiplier, gridPosition.z + gridSize.y / 2f);
        }
    }
}
