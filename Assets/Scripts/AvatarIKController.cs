using UnityEngine;

public class AvatarIKController : MonoBehaviour
{
    private Animator animator;

    [Header("IK Targets")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform headTarget;

    [Header("IK Weights")]
    [Range(0, 1)] public float handIKWeight = 1f;
    [Range(0, 1)] public float headIKWeight = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // all IK disabled — getting avatar standing first
        animator.SetLookAtWeight(0);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    }
}