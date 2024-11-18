using UnityEngine;
using Unity.Netcode;

public class OwnerComponentManager : NetworkBehaviour
{
    private Camera playerCamera;
    private AudioListener audioListener;
    
    [SerializeField] private Camera _camera;
    private Camera main_camera;
    
    private void Awake()
    {
        // Find the camera with the "MainCamera" tag and assign it to the mainCamera field
        main_camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; } // ALL players will read this method, only player owner will execute past this line
        _camera.enabled = true; // only enable YOUR PLAYER'S camera, all others will stay disabled
        main_camera.GetComponent<AudioListener>().enabled = false;
        main_camera.enabled = false;
    }
    
    void Start()
    {
        // Get the camera and audio listener components
        playerCamera = GetComponentInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();

        // Enable camera and audio listener only if this is the local player
        if (IsOwner)
        {
            playerCamera.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
            audioListener.enabled = false;
        }
    }
}