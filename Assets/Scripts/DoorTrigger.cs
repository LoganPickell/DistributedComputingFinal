using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator doorAnimator; // Assign the door's Animator in the Inspector
    private bool playerInRange = false;
    private ThirdPersonController playerController;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<ThirdPersonController>();
            playerController.SetCurrentDoor(this);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<ThirdPersonController>();
            playerInRange = false;
            playerController.SetCurrentDoor(null);
        }
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }
}
