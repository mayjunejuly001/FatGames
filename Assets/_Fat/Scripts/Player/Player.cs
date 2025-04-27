using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace FatTray
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private Camera cam;
        [SerializeField] private LayerMask trayLayerMask;
        [SerializeField] private float trayLerpSpeed = 15f;
        [SerializeField] private float trayGridSnapTime = 0.2f;
        [SerializeField] private float moveSpeed = 30f;
        [SerializeField] private float stopDistance = 0.1f;
        [Header("Debug")]
        [SerializeField] private bool debugTrayPositions = false;
        [SerializeField] private bool debugOverlapBoxes = false;

        private RaycastHit[] hits = new RaycastHit[1];
        private int rayHitCount;
        private Tray selectedTray;
        private bool isTrayPicked = false;
        private Vector3 mouseOffset;
        private Vector3 targetPosition;
        private Vector3 pickupPosition;
        private bool isSnapping;

        private void Start()
        {
            targetPosition = transform.position;
        }
        private void Update()
        {
            if (!isSnapping)
            {
                HandleInputs();    
            }
        }

        private void FixedUpdate()
        {
            MoveTray();
        }

        private void HandleInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                rayHitCount = Physics.RaycastNonAlloc(ray, hits, int.MaxValue, trayLayerMask);
                if (rayHitCount > 0)
                {
                    if (hits[0].collider != null)
                    {
                        selectedTray = hits[0].collider.GetComponent<Tray>();
                        if (selectedTray != null)
                        {
                            pickupPosition = selectedTray.transform.position;
                            //boxHitColls = new Collider[selectedTray.Colliders.Length];
                            selectedTray.TogglePicked(true);
                            mouseOffset = selectedTray.transform.position - GetMouseWorldPosition();
                            mouseOffset.y = 0;
                            isTrayPicked = true;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isTrayPicked)
                {
                    isTrayPicked = false;
                    if (selectedTray != null)
                    {
                        selectedTray.TogglePicked(false);
                        SnapToGrid(selectedTray, trayGridSnapTime, () => { selectedTray = null; });                           
                    }                   
                }
            }
        }

        private void SnapToGrid(Tray tray, float snapTime, Action onDone = null)
        {
            isSnapping = true;
            StartCoroutine(SnapToGridCoroutine(snapTime));

            IEnumerator SnapToGridCoroutine(float snapTime = 0.2f)
            {
                float timeElapsed = 0;
                Vector3 snappedPosition = GetSnappedPosition(tray.transform.position);
                snappedPosition = IsValidPosition(snappedPosition, grid.GridSize, selectedTray.Data.size) ? snappedPosition : pickupPosition;
                while (timeElapsed < snapTime)
                {
                    tray.MoveToPosition(Vector3.Lerp(tray.transform.position, snappedPosition, timeElapsed / snapTime));
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                tray.MoveToPosition(snappedPosition);
                isSnapping = false;
                onDone?.Invoke();
            }
        }

        private void MoveTray()
        {
            if (selectedTray == null)
                return;

            //if (IsValidPosition(GetMouseWorldPosition() + mouseOffset))
            //{
            //    selectedTray.MoveToPosition(Vector3.Lerp(selectedTray.transform.position, GetMouseWorldPosition() + mouseOffset, trayLerpSpeed * Time.deltaTime));
            //}

            targetPosition = GetMouseWorldPosition() + mouseOffset;

            Vector3 direction = (targetPosition - selectedTray.transform.position);

            if (direction.magnitude > stopDistance)
            {
                selectedTray.Rb.linearVelocity = direction.normalized * moveSpeed;
            }
            else
            {
                selectedTray.Rb.linearVelocity = Vector3.zero;
                selectedTray.MoveToPosition(Vector3.Lerp(selectedTray.transform.position, GetMouseWorldPosition() + mouseOffset, trayLerpSpeed * Time.deltaTime));
            }
        }

        //private Vector3 boxPos;
        //private Collider[] boxHitColls;
        //[SerializeField] private float boxSizeDividerAddition = 0.1f;
        //int boxHitCount = 0;
        //private bool IsValidPosition(Vector3 pos)
        //{
        //    for (int i = 0; i < selectedTray.Colliders.Length; i++)
        //    {
        //        boxPos = pos + selectedTray.Colliders[i].center;
        //        boxHitCount = Physics.OverlapBoxNonAlloc(boxPos, selectedTray.Colliders[i].size / (2f + boxSizeDividerAddition), boxHitColls, Quaternion.identity, trayLayerMask);
        //        if (boxHitCount > 0)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private bool IsValidPosition(Vector3 snappedPosition, Vector2Int gridSize, Vector2Int traySize)
        {
            int x = (int)snappedPosition.x;
            int y = (int)snappedPosition.z;

            // Check if within tray bounds
            return (x >= 0 && x <= (gridSize.x - traySize.x)) && (y >= 0 && y <= (gridSize.y - traySize.y));
        }

        private Vector3 GetMouseWorldPosition()
        {
            return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));
        }

        public Vector3 GetSnappedPosition(Vector3 worldPosition)
        {
            return new Vector3(Mathf.RoundToInt(worldPosition.x), worldPosition.y, Mathf.RoundToInt(worldPosition.z));
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (selectedTray != null)
            {
                if (debugTrayPositions)
                { 
                    // LOG (POSITION, SNAP POSITION)
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.red;

                    Handles.Label(selectedTray.transform.position, $"{selectedTray.transform.position}, {GetSnappedPosition(selectedTray.transform.position)}", style);
                }

                if (debugOverlapBoxes)
                { 
                    // DRAW DEBUG BOX POSITIONS
                    Gizmos.color = Color.red;
                    for (int i = 0; i < selectedTray.Colliders.Length; i++)
                    {
                        var boxPos = selectedTray.transform.position + selectedTray.Colliders[i].center;
                        Gizmos.DrawCube(boxPos, selectedTray.Colliders[i].size);
                    }
                }
            }
#endif
        }
    }
}
