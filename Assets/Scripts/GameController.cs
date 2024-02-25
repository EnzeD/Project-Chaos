using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ResourceData resourceData;

    public GameObject UICamera;

    public GameObject winLoseCanvas;
    public GameObject winText;
    public GameObject gameOverText;
    public GameObject UIcanvas;

    private void Start()
    {
        UICamera.SetActive(true);
    }

    private void Update()
    {
        if (resourceData.chaosLevel >= 100)
        {
            winLoseCanvas.SetActive(true); // Show the game over menu
            UIcanvas.SetActive(false);
            gameOverText.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }

        if (resourceData.luckLevel >= 100)
        {
            winLoseCanvas.SetActive(true); // Show the win menu
            UIcanvas.SetActive(false);
            winText.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }

    }

    private void OnApplicationQuit()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        resourceData.ResetData();
       
    }
}
