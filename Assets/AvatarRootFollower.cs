using UnityEngine;

public class AvatarRootFollower : MonoBehaviour
{
    public Transform xrRoot;
    public float yOffset = 0f;

    void LateUpdate()
    {
        if (xrRoot == null) return;

        Vector3 p = xrRoot.position;
        p.y += yOffset;
        transform.position = p;

        transform.rotation = Quaternion.Euler(0f, xrRoot.eulerAngles.y, 0f);
    }
}