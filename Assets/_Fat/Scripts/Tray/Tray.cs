using UnityEngine;

namespace FatTray
{
    public class Tray : MonoBehaviour
    {
        private TrayData data;
        private Rigidbody rb;
        private Renderer[] trayRenderers;
        private float colliderSizeOffset;

        public TrayData Data => data;
        public BoxCollider[] Colliders { get; private set; }
        public Rigidbody Rb => rb;

        public bool IsPicked { get; private set; }

        public void Initialize(TrayData data, float colliderSizeOffset)
        {
            rb = GetComponent<Rigidbody>();
            this.data = data;
            this.colliderSizeOffset = colliderSizeOffset;
            trayRenderers = SpawnMesh().GetComponentsInChildren<Renderer>();
            SetColor();
            SetColliders();
        }

        private GameObject SpawnMesh()
        {
            return Instantiate(data.modelPrefab, transform);
        }

        private void SetColor()
        {
            if (trayRenderers is { Length: > 0 })
                data.SetColor(trayRenderers);
        }

        private void SetColliders()
        {
            if (trayRenderers == null)
                return;
            Colliders = new BoxCollider[trayRenderers.Length];
            for (int i = 0; i < trayRenderers.Length; i++)
            {
                var boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1 + colliderSizeOffset, 0.5f, 1 + colliderSizeOffset);
                boxCollider.center = new Vector3(trayRenderers[i].transform.localPosition.x, boxCollider.size.y / 2f, trayRenderers[i].transform.localPosition.z);
                Colliders[i] = boxCollider;
            }
        }

        public void MoveToPosition(Vector3 pos)
        {
            if (rb == null)
                return;
            rb.MovePosition(pos);
        }

        public void TogglePicked(bool isPicked)
        {
            IsPicked = isPicked;
            gameObject.layer = isPicked ? LayerMask.NameToLayer("Ignore Raycast") : LayerMask.NameToLayer("Tray");
            if (rb != null)
            { 
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = !isPicked;
            }
        }
    }
}
