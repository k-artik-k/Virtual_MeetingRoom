using UnityEngine;

public class AvatarFollowRig : MonoBehaviour
{
    public Transform xrOrigin;
    public float heightOffset = 0f;
    public bool followRotation = true;

    void LateUpdate()
    {
        if (xrOrigin == null) return;

        Vector3 targetPosition = xrOrigin.position;
        targetPosition.y += heightOffset;
        transform.position = targetPosition;

        if (followRotation)
        {
            Vector3 yOnlyRotation = new Vector3(0f, xrOrigin.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler(yOnlyRotation);
        }
    }
}