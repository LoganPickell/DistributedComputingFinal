using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public RenderTexture[] monitorRenderTextures; // Array of Render Textures for monitors
    private bool[] isMonitorOccupied; // Tracks whether each monitor is in use

    void Start()
    {
        isMonitorOccupied = new bool[monitorRenderTextures.Length];
    }

    public bool AssignCameraToMonitor(Camera botCamera)
    {
        for (int i = 0; i < monitorRenderTextures.Length; i++)
        {
            if (!isMonitorOccupied[i]) // Find an available monitor
            {
                botCamera.targetTexture = monitorRenderTextures[i];
                isMonitorOccupied[i] = true; // Mark monitor as occupied
                return true;
            }
        }

        Debug.LogWarning("No available monitors!");
        return false; // No available monitors
    }

    public void ReleaseMonitor(Camera botCamera)
    {
        for (int i = 0; i < monitorRenderTextures.Length; i++)
        {
            if (botCamera.targetTexture == monitorRenderTextures[i])
            {
                botCamera.targetTexture = null;
                isMonitorOccupied[i] = false; // Mark monitor as available
                return;
            }
        }
    }
}