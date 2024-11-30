using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotSpawnerUI : MonoBehaviour
{
    public Spawn_BTN botSpawner; // Reference to the BotSpawner
    public TextMeshProUGUI botCountText; // UI Text to show the current bot count

    void Start()
    {
        botCountText.text = "Bots: " + botSpawner.currentBotCount + "/" + botSpawner.maxBots;
    }
    void Update()
    {
        botCountText.text = "Bots: " + botSpawner.currentBotCount + "/" + botSpawner.maxBots;
    }
}
