using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerMovement : MonoBehaviourPun
{
    public Transform head;
    public float moveSpeed = 2f;
    public float snapTurnAngle = 30f;
    public float turnCooldown = 0.35f;

    [Header("Animation")]
    public Animator avatarAnimator;

    private CharacterController controller;
    private float nextTurnTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 turn = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Vector3 forward = head.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = head.right;
        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * move.y + right * move.x;
        direction.y = 0;

        controller.Move(direction * moveSpeed * Time.deltaTime);

        // update animation speed
        if (avatarAnimator != null)
        {
            float speed = new Vector2(move.x, move.y).magnitude;
            avatarAnimator.SetFloat("Speed", speed);
        }

        // lock height
        Vector3 pos = transform.position;
        pos.y = 0;
        transform.position = pos;

        // snap turn
        if (Time.time >= nextTurnTime)
        {
            if (turn.x > 0.7f)
            {
                transform.Rotate(0, snapTurnAngle, 0);
                nextTurnTime = Time.time + turnCooldown;
            }
            else if (turn.x < -0.7f)
            {
                transform.Rotate(0, -snapTurnAngle, 0);
                nextTurnTime = Time.time + turnCooldown;
            }
        }
    }
}