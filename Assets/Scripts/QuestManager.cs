using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public ResourceData resourceData; 
    public TextMeshProUGUI resourceText;
    public GameObject questUI;
    public GameObject questDone;
    private SpawnManager spawnManager;
    public int questNumber;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (questNumber == 1)
        {

            if (Input.GetMouseButtonDown(0))
            {
                questDone.SetActive(true);
            }
        }
        if (questNumber == 2)
        {

            if (Input.GetMouseButtonDown(1))
            {
                questDone.SetActive(true);
            }
        }
        if (questNumber == 3)
        {
            resourceText.text = "Collect \"Fire\" (" + resourceData.totalFireCollected.ToString() + "/10)";

            if (resourceData.totalFireCollected >= 10)
            {
                questDone.SetActive(true);
                resourceText.text = "Collect \"Fire\" (10/10)";
            }
        }
        if (questNumber == 4)
        {
            resourceText.text = "Build a Fire Tower (0/1)";
            if (PlaceBuilding.AFireTowerHasBeenBuilt)
            {
                questDone.SetActive(true);
                resourceText.text = "Build a Fire Tower (0/1)";
            }
        }
        if (questNumber == 5)
        {
            if (LightingManager.DayCounter >= 2 && spawnManager.totalMonstersAlive == 0)
            {
                questDone.SetActive(true);
            }
        }
    }
}
