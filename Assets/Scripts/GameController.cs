using UnityEngine;

public class GameController : MonoBehaviour
{
    public ResourceData resourceData;

    public GameObject UICamera;

    private void Start()
    {
        UICamera.SetActive(true);
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
