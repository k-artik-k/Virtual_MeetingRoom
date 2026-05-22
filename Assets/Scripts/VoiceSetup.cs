using UnityEngine;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class VoiceSetup : MonoBehaviour
{
    private PunVoiceClient punVoiceClient;

    void Awake()
    {
        punVoiceClient = FindObjectOfType<PunVoiceClient>();
    }

    void Start()
    {
        if (punVoiceClient == null)
        {
            Debug.LogWarning("PunVoiceClient not found. Add it to the scene.");
            return;
        }

        // voice connects automatically with PUN when PunVoiceClient is in scene
        Debug.Log("Voice ready.");
    }
}