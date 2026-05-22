using UnityEngine;

public class VRLaserPointer : MonoBehaviour
{
    public Transform handAnchor;
    public LineRenderer lineRenderer;
    public float maxDistance = 5f;

    void Update()
    {
        if (handAnchor == null || lineRenderer == null) return;

        Ray ray = new Ray(handAnchor.position, handAnchor.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // Show laser only on whiteboard
            if (hit.collider.CompareTag("Whiteboard"))
            {
                lineRenderer.enabled = true;

                lineRenderer.SetPosition(0, handAnchor.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}