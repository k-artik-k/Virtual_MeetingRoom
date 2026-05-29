using UnityEngine;

public class HandIKTarget : MonoBehaviour
{
    public Transform handAnchor; // drag OVRHandPrefab or LeftHandAnchor here

    void LateUpdate()
    {
        if (handAnchor == null) return;
        transform.position = handAnchor.position;
        transform.rotation = handAnchor.rotation;
    }
}