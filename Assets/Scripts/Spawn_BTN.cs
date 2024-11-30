using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Spawn_BTN : MonoBehaviour
{
    public GameObject botPrefab; 
    public Transform spawnPoint;
    public MonitorManager monitorManager;
    public int maxBots = 6;
    public int currentBotCount = 0;

    private Queue<GameObject> botPool = new Queue<GameObject>(); // Pool for bots
    public GameObject[] targets; 
    private int targetIndex = 0;
    private bool playerInRange = false;

    void Start()
    {
        // Initialize the bot pool
        for (int i = 0; i < maxBots; i++)
        {
            GameObject bot = Instantiate(botPrefab);
            bot.SetActive(false); // Keep bots inactive initially
            botPool.Enqueue(bot);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (currentBotCount < maxBots)
            {
                currentBotCount++;
            }
            SpawnBot();
        }
    }

    private void SpawnBot()
    {
        if (botPool.Count > 0)
        {
            GameObject bot = botPool.Dequeue(); // Get a bot from the pool
            bot.transform.position = spawnPoint.position;
            bot.SetActive(true); // Activate the bot

            AIDestinationSetter aiDestinationSetter = bot.GetComponent<AIDestinationSetter>();
            if (aiDestinationSetter != null)
            {
                aiDestinationSetter.target = targets[targetIndex].transform;
            }

            targetIndex = (targetIndex + 1) % targets.Length;

            Camera botCamera = bot.GetComponentInChildren<Camera>();
            if (botCamera != null && !monitorManager.AssignCameraToMonitor(botCamera))
            {
                Debug.LogWarning("No Monitors available for the bot camera!");
            }
        }
        else
        {
            Debug.LogWarning("No bots available in the pool.");
        }
    }

    public void ReturnBotToPool(GameObject bot)
    {
        bot.SetActive(false); // Deactivate the bot
        botPool.Enqueue(bot); // Return it to the pool
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press 'E' to spawn a bot.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}