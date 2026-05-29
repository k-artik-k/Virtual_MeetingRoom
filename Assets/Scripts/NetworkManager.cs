using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected! Joining room...");
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayers;
        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

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
    Vector3 spawnPos = new Vector3(-4f, 0f, 6f);
    PhotonNetwork.Instantiate("NetworkPlayer", spawnPos, Quaternion.Euler(0, 180, 0));
}
}