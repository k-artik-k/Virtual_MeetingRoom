using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using AvatarSDK.MetaPerson.Oculus;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Room Settings")]
    public string roomName = "VRMeetingRoom";
    public byte maxPlayers = 10;

    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // called when connected to Photon servers
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected! Joining room...");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    // called when successfully in a room
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room! Players: " + PhotonNetwork.CurrentRoom.PlayerCount);
        SpawnLocalPlayer();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    void SpawnLocalPlayer()
    {
        // spawns YOUR player prefab on the network
        // everyone in the room will see it appear
PhotonNetwork.Instantiate("NetworkPlayer", new Vector3(6.2f, 0f, -0.8f), Quaternion.identity);    }
}