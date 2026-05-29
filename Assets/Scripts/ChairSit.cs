using UnityEngine;
using Photon.Pun;

public class ChairSit : MonoBehaviour
{
    [Header("Chair Settings")]
    public Transform sitPoint;
    public float sitRange = 1.5f;
    public GameObject promptUI;

    [Header("Animation")]
    public string sitAnimParam = "Sitting";

    private Transform player;
    private bool isSitting = false;
    private CharacterController controller;
    private VRPlayerMovement movement;
    private Animator avatarAnimator;
    private AvatarRootFollower rootFollower;

    void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (promptUI != null && promptUI.activeSelf && player != null)
        {
            Vector3 dir = promptUI.transform.position - player.position;
            dir.y = 0;
            if (dir != Vector3.zero)
                promptUI.transform.rotation = Quaternion.LookRotation(dir);
        }

        if (player == null)
        {
            NetworkedPlayer[] netPlayers = FindObjectsOfType<NetworkedPlayer>();
            foreach (var np in netPlayers)
            {
                if (np.photonView.IsMine)
                {
                    player = np.transform;
                    controller = player.GetComponent<CharacterController>();
                    movement = player.GetComponent<VRPlayerMovement>();
                    avatarAnimator = player.GetComponentInChildren<Animator>();
                    rootFollower = player.GetComponentInChildren<AvatarRootFollower>();
                    break;
                }
            }
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (!isSitting)
        {
            if (promptUI != null)
                promptUI.SetActive(dist < sitRange);

            if (dist < sitRange && (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.F)))
                Sit();
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.F))
                StandUp();
        }
    }

    void Sit()
    {
        isSitting = true;
        if (promptUI != null) promptUI.SetActive(false);

        if (movement != null) movement.enabled = false;
        if (controller != null) controller.enabled = false;

        float yOff = rootFollower != null ? rootFollower.yOffset : 0f;
        player.position = sitPoint.position - new Vector3(0, yOff, 0);
        player.rotation = sitPoint.rotation;

        if (avatarAnimator != null)
        {
            avatarAnimator.SetBool("Sitting", true);
            avatarAnimator.SetFloat("Speed", 0f);
        }
    }

    void StandUp()
    {
        isSitting = false;

        if (controller != null) controller.enabled = true;
        if (movement != null) movement.enabled = true;

        if (avatarAnimator != null)
            avatarAnimator.SetBool("Sitting", false);
    }
}