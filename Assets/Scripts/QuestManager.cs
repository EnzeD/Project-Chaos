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
    private Animator parentAnimator;
    public LightingManager lightingManager;

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
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }
        if (questNumber == 2)
        {

            if (Input.GetMouseButtonDown(1) || Input.GetKey("space"))
            {
                questDone.SetActive(true);
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }
        if (questNumber == 3)
        {
            resourceText.text = "Collect \"Fire\" (" + resourceData.totalFireCollected.ToString() + "/10)";

            if (resourceData.totalFireCollected >= 10 || questDone.activeInHierarchy)
            {
                questDone.SetActive(true);
                resourceText.text = "Collect \"Fire\" (10/10)";
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }
        if (questNumber == 4)
        {
            resourceText.text = "Build a Fire Tower (0/1)";
            if (PlaceBuilding.AFireTowerHasBeenBuilt)
            {
                questDone.SetActive(true);
                resourceText.text = "Build a Fire Tower (1/1)";
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }
        if (questNumber == 5)
        {
            if (lightingManager.DayCounter >= 2 && spawnManager.totalMonstersAlive == 0)
            {
                questDone.SetActive(true);
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }

        if (questNumber == 6)
        {
            if (resourceData.chaosQuest)
            {
                questDone.SetActive(true);
                parentAnimator = GetComponentInParent<Animator>();
                parentAnimator.SetTrigger("QuestCompleted");
            }
        }
    }
}
