using UnityEngine;

public class AvatarFollowRig1 : MonoBehaviour
{
    public Transform xrRig;   // Drag OVRCameraRig here
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (xrRig == null) return;

        // Follow position (only X and Z, keep same height)
        Vector3 targetPosition = xrRig.position;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        // Follow rotation (only Y axis)
        Quaternion targetRotation = Quaternion.Euler(0, xrRig.eulerAngles.y, 0);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }
}