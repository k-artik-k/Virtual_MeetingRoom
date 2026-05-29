using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class NetworkUI : MonoBehaviour
{
    private PunVoiceClient voiceClient;

    void Start()
    {
        voiceClient = FindObjectOfType<PunVoiceClient>();
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        style.normal.textColor = Color.green;

        GUI.Label(new Rect(10, 10, 400, 30), "Room: " + PhotonNetwork.CurrentRoom?.Name, style);
        GUI.Label(new Rect(10, 40, 400, 30), "Players: " + PhotonNetwork.CurrentRoom?.PlayerCount, style);
        GUI.Label(new Rect(10, 70, 400, 30), "Ping: " + PhotonNetwork.GetPing() + "ms", style);

        if (voiceClient != null)
            GUI.Label(new Rect(10, 100, 400, 30), "Voice: " + voiceClient.Client.State, style);
    }
}