using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkBehaviour
{
    public GameObject playerPrefab;  // Reference to your player prefab
    public Transform[] spawnPoints;  // List of spawn points in the scene

    private void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager Singleton is not available. Ensure it is added to the scene.");
            return;
        }
        
        
        NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);

        // Register the callback for when a client connects
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
        else
        {
            Debug.LogWarning("NetworkManager.Singleton is null in OnDisable.");
        }
    }

    // This function is called when a client connects to the server
    private void OnClientConnected(ulong clientId)
    {
        // Only the server should handle spawning logic
        if (!NetworkManager.Singleton.IsServer) return;

        // Select a spawn point based on the number of connected clients
        int playerIndex = NetworkManager.Singleton.ConnectedClients.Count - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        // Debug log to check spawn point
        Debug.Log($"Spawning player {clientId} at {spawnPoint.position}");

        // Instantiate the player prefab at the spawn point
        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Ensure NetworkObject is properly set up for the player prefab
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("Player prefab does not have a NetworkObject attached.");
            return;
        }

        // Spawn the player object as a player object
        networkObject.SpawnAsPlayerObject(clientId);
    }
}
