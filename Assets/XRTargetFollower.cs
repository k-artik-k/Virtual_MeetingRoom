using UnityEngine;

public class XRTargetFollower : MonoBehaviour
{
    [Header("XR References")]
    public Transform xrRoot;
    public Transform xrHead;
    public Transform xrLeftHand;
    public Transform xrRightHand;

    [Header("Avatar Targets")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform hipTarget;

    [Header("Offsets")]
    public Vector3 headPositionOffset;
    public Vector3 headRotationOffset;
    public Vector3 leftHandPositionOffset;
    public Vector3 leftHandRotationOffset;
    public Vector3 rightHandPositionOffset;
    public Vector3 rightHandRotationOffset;
    public Vector3 hipPositionOffset = new Vector3(0f, -0.9f, 0f);
    public Vector3 hipRotationOffset;

    void LateUpdate()
    {
        if (xrHead != null && headTarget != null)
        {
            headTarget.position = xrHead.position + headPositionOffset;
            headTarget.rotation = xrHead.rotation * Quaternion.Euler(headRotationOffset);
        }

        if (xrLeftHand != null && leftHandTarget != null)
        {
            leftHandTarget.position = xrLeftHand.position + leftHandPositionOffset;
            leftHandTarget.rotation = xrLeftHand.rotation * Quaternion.Euler(leftHandRotationOffset);
        }

        if (xrRightHand != null && rightHandTarget != null)
        {
            rightHandTarget.position = xrRightHand.position + rightHandPositionOffset;
            rightHandTarget.rotation = xrRightHand.rotation * Quaternion.Euler(rightHandRotationOffset);
        }

        if (xrRoot != null && hipTarget != null)
        {
            hipTarget.position = xrRoot.position + hipPositionOffset;
            hipTarget.rotation = Quaternion.Euler(0f, xrRoot.eulerAngles.y, 0f) * Quaternion.Euler(hipRotationOffset);
        }
    }
}