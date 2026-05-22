using UnityEngine;

public class MarkerRayLaser : MonoBehaviour
{
    public Transform handAnchor;
    public LineRenderer lineRenderer;
    public float maxDistance = 5f;

    void Update()
    {
        Ray ray = new Ray(handAnchor.position, handAnchor.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.CompareTag("Marker"))
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