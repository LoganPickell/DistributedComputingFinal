using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Enable to move the camera by holding the right mouse button. Does not work with joysticks.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel. Does not work with joysticks.")]
    public bool canZoom = true;
    [Space]
    [Tooltip("The higher it is, the faster the camera moves. It is recommended to increase this value for games that uses joystick.")]
    public float sensitivity = 5f;

    [Tooltip("Camera Y rotation limits. The X axis is the maximum it can go up and the Y axis is the maximum it can go down.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);

    float mouseX;
    float mouseY;
    float offsetDistanceY;

    Transform player;
    private NetworkObject networkObject;
    private Camera cam;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Start()
    {
        // Get the NetworkObject component attached to the player prefab
        networkObject = GetComponentInParent<NetworkObject>();

        // Only run this logic if the client owns this player object
        if (networkObject != null && networkObject.IsOwner)
        {
            player = GameObject.FindWithTag("Player").transform;
            offsetDistanceY = transform.position.y;

            // Lock and hide cursor with option isn't checked
            if (!clickToMoveCamera)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
        }
        else
        {
            // Disable the camera controller on non-owner clients
            this.enabled = false;
        }
    }

    void Update()
    {
        // Only allow the owner to control the camera
        if (networkObject != null && !networkObject.IsOwner) return;
        
        // Follow player - camera offset
        transform.position = player.position + new Vector3(0, offsetDistanceY, 0);

        // Set camera zoom when mouse wheel is scrolled
        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;

        // Checker for right click to move camera
        if (clickToMoveCamera)
            if (Input.GetAxisRaw("Fire2") == 0)
                return;

        // Calculate new position
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        // Apply camera limits
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }
}