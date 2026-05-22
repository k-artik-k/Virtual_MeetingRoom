using UnityEngine;

public class VRAvatarMapper : MonoBehaviour
{
    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    public Transform vrHead;
    public Transform leftController;
    public Transform rightController;

    public Transform xrRoot;

    void LateUpdate()
    {
        if (xrRoot != null)
        {
            transform.position = xrRoot.position;
            transform.rotation = Quaternion.Euler(0, xrRoot.eulerAngles.y, 0);
        }

        if (headBone != null && vrHead != null)
        {
            headBone.position = vrHead.position;
            headBone.rotation = vrHead.rotation;
        }

        if (leftHandBone != null && leftController != null)
        {
            leftHandBone.position = leftController.position;
            leftHandBone.rotation = leftController.rotation;
        }

        if (rightHandBone != null && rightController != null)
        {
            rightHandBone.position = rightController.position;
            rightHandBone.rotation = rightController.rotation;
        }
    }
}