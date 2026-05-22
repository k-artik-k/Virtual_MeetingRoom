using UnityEngine;

public class XRHumanoidAvatarMapper : MonoBehaviour
{
    [Header("XR Targets")]
    public Transform xrRoot;
    public Transform vrHead;
    public Transform leftController;
    public Transform rightController;

    [Header("Main Body Settings")]
    public bool followRootPosition = true;
    public bool followRootRotation = true;
    public float heightOffset = 0f;
    public Vector3 rootPositionOffset;
    public Vector3 rootRotationOffset;

    [Header("Hip Settings")]
    public bool moveHips = true;
    public Vector3 hipsPositionOffset;
    public Vector3 hipsRotationOffset;
    public float hipsHeightFromRoot = 0.9f;

    [Header("Head Settings")]
    public bool moveHead = true;
    public Vector3 headPositionOffset;
    public Vector3 headRotationOffset;

    [Header("Hand Settings")]
    public bool moveLeftHand = true;
    public bool moveRightHand = true;
    public Vector3 leftHandPositionOffset;
    public Vector3 leftHandRotationOffset;
    public Vector3 rightHandPositionOffset;
    public Vector3 rightHandRotationOffset;

    [Header("Auto-Filled Humanoid Bones")]
    public Animator animator;
    public Transform hips;
    public Transform spine;
    public Transform chest;
    public Transform upperChest;
    public Transform neck;
    public Transform head;

    public Transform leftShoulder;
    public Transform leftUpperArm;
    public Transform leftLowerArm;
    public Transform leftHand;

    public Transform rightShoulder;
    public Transform rightUpperArm;
    public Transform rightLowerArm;
    public Transform rightHand;

    public Transform leftUpperLeg;
    public Transform leftLowerLeg;
    public Transform leftFoot;

    public Transform rightUpperLeg;
    public Transform rightLowerLeg;
    public Transform rightFoot;

    void Reset()
    {
        AutoFillBones();
    }

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null)
            AutoFillBones();
    }

    [ContextMenu("Auto Fill Humanoid Bones")]
    public void AutoFillBones()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null || !animator.isHuman)
        {
            Debug.LogWarning("Animator is missing or avatar is not Humanoid.");
            return;
        }

        hips = animator.GetBoneTransform(HumanBodyBones.Hips);
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        upperChest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        neck = animator.GetBoneTransform(HumanBodyBones.Neck);
        head = animator.GetBoneTransform(HumanBodyBones.Head);

        leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
        leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

        rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
        rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
        rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);

        leftUpperLeg = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        leftLowerLeg = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);

        rightUpperLeg = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        rightLowerLeg = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    }

    void LateUpdate()
    {
        if (xrRoot == null) return;

        UpdateRoot();
        UpdateHips();
        UpdateHead();
        UpdateHands();
    }

    void UpdateRoot()
    {
        if (followRootPosition)
        {
            Vector3 targetPosition = xrRoot.position + rootPositionOffset;
            targetPosition.y += heightOffset;
            transform.position = targetPosition;
        }

        if (followRootRotation)
        {
            Vector3 euler = xrRoot.eulerAngles + rootRotationOffset;
            transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
        }
    }

    void UpdateHips()
    {
        if (!moveHips || hips == null) return;

        Vector3 rootBasedHipPosition = transform.position + transform.up * hipsHeightFromRoot;
        hips.position = rootBasedHipPosition + hipsPositionOffset;

        Vector3 hipEuler = transform.eulerAngles + hipsRotationOffset;
        hips.rotation = Quaternion.Euler(hipEuler);
    }

    void UpdateHead()
    {
        if (!moveHead || head == null || vrHead == null) return;

        head.position = vrHead.position + headPositionOffset;
        head.rotation = vrHead.rotation * Quaternion.Euler(headRotationOffset);
    }

    void UpdateHands()
    {
        if (moveLeftHand && leftHand != null && leftController != null)
        {
            leftHand.position = leftController.position + leftHandPositionOffset;
            leftHand.rotation = leftController.rotation * Quaternion.Euler(leftHandRotationOffset);
        }

        if (moveRightHand && rightHand != null && rightController != null)
        {
            rightHand.position = rightController.position + rightHandPositionOffset;
            rightHand.rotation = rightController.rotation * Quaternion.Euler(rightHandRotationOffset);
        }
    }
}