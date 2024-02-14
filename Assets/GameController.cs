using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ResourceData resourceData;

    private void OnApplicationQuit()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        resourceData.ResetData();
       
    }
}
