using UnityEngine;
using Photon.Pun;
using AvatarSDK.MetaPerson.Oculus;

public class NetworkedPlayer : MonoBehaviourPun, IPunObservable
{
    [Header("Local Player References")]
    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    [Header("OVR Hand Tracking")]
    public OVRHand leftOVRHand;
    public OVRHand rightOVRHand;
    public OVRSkeleton leftHandSkeleton;
    public OVRSkeleton rightHandSkeleton;

    [Header("Avatar Reference")]
    public AvatarRootFollower avatarRootFollower;
    public OVRToMetaPersonSkeletonSync skeletonSync;

    [Header("Remote Avatar Bones")]
    public Transform remoteLeftHand;
    public Transform remoteRightHand;
    public Transform remoteHead;

    // received data
    private Vector3 receivedPosition;
    private Quaternion receivedRotation;
    private Vector3 receivedHeadPos;
    private Quaternion receivedHeadRot;
    private Vector3 receivedLeftHandPos;
    private Quaternion receivedLeftHandRot;
    private Vector3 receivedRightHandPos;
    private Quaternion receivedRightHandRot;

    // finger bones - 24 bones per hand
    private Quaternion[] receivedLeftFingers = new Quaternion[24];
    private Quaternion[] receivedRightFingers = new Quaternion[24];

    void Start()
    {
        if (photonView.IsMine)
        {
            Debug.Log("Local player ready.");
        }
        else
        {
            DisableLocalOnlyComponents();
        }
    }

    void DisableLocalOnlyComponents()
    {
        if (skeletonSync != null)
            skeletonSync.enabled = false;

        if (avatarRootFollower != null)
            avatarRootFollower.enabled = false;

        VRPlayerMovement movement = GetComponent<VRPlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        OVRCameraRig cameraRig = GetComponentInChildren<OVRCameraRig>();
        if (cameraRig != null)
            cameraRig.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            // move root
            transform.position = Vector3.Lerp(transform.position, receivedPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, receivedRotation, Time.deltaTime * 15f);

            // move hands and head on remote avatar
            if (remoteHead != null)
            {
                remoteHead.position = Vector3.Lerp(remoteHead.position, receivedHeadPos, Time.deltaTime * 20f);
                remoteHead.rotation = Quaternion.Lerp(remoteHead.rotation, receivedHeadRot, Time.deltaTime * 20f);
            }
            if (remoteLeftHand != null)
            {
                remoteLeftHand.position = Vector3.Lerp(remoteLeftHand.position, receivedLeftHandPos, Time.deltaTime * 20f);
                remoteLeftHand.rotation = Quaternion.Lerp(remoteLeftHand.rotation, receivedLeftHandRot, Time.deltaTime * 20f);
            }
            if (remoteRightHand != null)
            {
                remoteRightHand.position = Vector3.Lerp(remoteRightHand.position, receivedRightHandPos, Time.deltaTime * 20f);
                remoteRightHand.rotation = Quaternion.Lerp(remoteRightHand.rotation, receivedRightHandRot, Time.deltaTime * 20f);
            }

            // apply finger rotations
            ApplyFingerRotations();
        }
    }

    void ApplyFingerRotations()
    {
        if (leftHandSkeleton != null && leftHandSkeleton.Bones != null)
        {
            for (int i = 0; i < Mathf.Min(24, leftHandSkeleton.Bones.Count); i++)
                leftHandSkeleton.Bones[i].Transform.localRotation = receivedLeftFingers[i];
        }

        if (rightHandSkeleton != null && rightHandSkeleton.Bones != null)
        {
            for (int i = 0; i < Mathf.Min(24, rightHandSkeleton.Bones.Count); i++)
                rightHandSkeleton.Bones[i].Transform.localRotation = receivedRightFingers[i];
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // root
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            // head
            stream.SendNext(headTransform.position);
            stream.SendNext(headTransform.rotation);

            // hands
            stream.SendNext(leftHandTransform.position);
            stream.SendNext(leftHandTransform.rotation);
            stream.SendNext(rightHandTransform.position);
            stream.SendNext(rightHandTransform.rotation);

            // left fingers
            bool leftTracked = leftOVRHand != null && leftOVRHand.IsTracked;
            stream.SendNext(leftTracked);
            if (leftTracked && leftHandSkeleton != null)
            {
                for (int i = 0; i < Mathf.Min(24, leftHandSkeleton.Bones.Count); i++)
                    stream.SendNext(leftHandSkeleton.Bones[i].Transform.localRotation);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                    stream.SendNext(Quaternion.identity);
            }

            // right fingers
            bool rightTracked = rightOVRHand != null && rightOVRHand.IsTracked;
            stream.SendNext(rightTracked);
            if (rightTracked && rightHandSkeleton != null)
            {
                for (int i = 0; i < Mathf.Min(24, rightHandSkeleton.Bones.Count); i++)
                    stream.SendNext(rightHandSkeleton.Bones[i].Transform.localRotation);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                    stream.SendNext(Quaternion.identity);
            }
        }
        else
        {
            // root
            receivedPosition = (Vector3)stream.ReceiveNext();
            receivedRotation = (Quaternion)stream.ReceiveNext();

            // head
            receivedHeadPos = (Vector3)stream.ReceiveNext();
            receivedHeadRot = (Quaternion)stream.ReceiveNext();

            // hands
            receivedLeftHandPos = (Vector3)stream.ReceiveNext();
            receivedLeftHandRot = (Quaternion)stream.ReceiveNext();
            receivedRightHandPos = (Vector3)stream.ReceiveNext();
            receivedRightHandRot = (Quaternion)stream.ReceiveNext();

            // left fingers
            bool leftTracked = (bool)stream.ReceiveNext();
            for (int i = 0; i < 24; i++)
                receivedLeftFingers[i] = (Quaternion)stream.ReceiveNext();

            // right fingers
            bool rightTracked = (bool)stream.ReceiveNext();
            for (int i = 0; i < 24; i++)
                receivedRightFingers[i] = (Quaternion)stream.ReceiveNext();
        }
    }
}