using UnityEngine;

public class OwnerComponentManager : MonoBehaviour
{
    private Camera playerCamera;
    private AudioListener audioListener;

    [SerializeField] private Camera _camera; // Reference to this player's camera
    private Camera mainCamera;

    private void Awake()
    {
        // Find the camera with the "MainCamera" tag and assign it to the mainCamera field
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        // Get the camera and audio listener components from the child objects
        playerCamera = GetComponentInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();

        // Check if this is the local player by comparing to the main camera's perspective
        if (IsLocalPlayer())
        {
            // Enable this player's camera and audio listener
            playerCamera.enabled = true;
            audioListener.enabled = true;

            // Disable the main camera
            if (mainCamera != null)
            {
                mainCamera.enabled = false;
                if (mainCamera.GetComponent<AudioListener>() != null)
                    mainCamera.GetComponent<AudioListener>().enabled = false;
            }
        }
        else
        {
            // Disable this player's camera and audio listener
            playerCamera.enabled = false;
            audioListener.enabled = false;
        }
    }

    // Example method to check if this is the local player
    // Replace this logic as needed to identify the "local player"
    private bool IsLocalPlayer()
    {
        // Example condition: Add logic for distinguishing local player (e.g., by tag, player ID, etc.)
        return true; // Replace with your condition to identify the local player
    }
}