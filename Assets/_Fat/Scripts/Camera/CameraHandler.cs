using System;
using UnityEngine;

namespace FatTray
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector2 camHeightMultiplierRange = new Vector2(2.8f, 3.5f);

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
            float heightMul = Mathf.Lerp(camHeightMultiplierRange.x, camHeightMultiplierRange.y, Mathf.InverseLerp(9f / 16f, 9f / 21f, (float)Screen.width / (float)Screen.height));
            cam.transform.position = new Vector3(gridPosition.x + gridSize.x / 2f, Math.Max(gridSize.x, gridSize.y) / 2f * heightMul, gridPosition.z + gridSize.y / 2f);
        }
    }
}
