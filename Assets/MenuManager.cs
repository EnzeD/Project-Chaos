using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject startMenuUI;
    public GameObject pauseMenuUI;
    public GameObject UIcanvas;

    public static bool IsGamePaused { get; private set; }

    private void Start()
    {
        Time.timeScale = 0f; // Pause the game
        startMenuUI.SetActive(true);
        UIcanvas.SetActive(false);
        IsGamePaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        UIcanvas.SetActive(false);
        Time.timeScale = 0f; // Pause the game
        IsGamePaused = true;
    }

    public void Resume()
    {
        startMenuUI.SetActive(false);
        UIcanvas.SetActive(true);
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
        IsGamePaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}