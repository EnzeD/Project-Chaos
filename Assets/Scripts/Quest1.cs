using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest1 : MonoBehaviour
{
    public ResourceData resourceData; 
    public TextMeshProUGUI resourceText;
    public GameObject questUI;
    public GameObject questDone;
    public int questNumber;

    // Update is called once per frame
    void Update()
    {
        if (questNumber == 1)
        {

            if (Input.GetMouseButtonDown(1))
            {
                questDone.SetActive(true);
            }
        }
        if (questNumber == 2)
        {

            if (Input.GetMouseButtonDown(0))
            {
                questDone.SetActive(true);
            }
        }
        if (questNumber == 3)
        {
            resourceText.text = "COLLECT FIRE (" + resourceData.totalFireCollected.ToString() + "/10)";

            if (resourceData.totalFireCollected >= 10)
            {
                questDone.SetActive(true);
                resourceText.text = "COLLECT FIRE (10/10)";
            }
        }
    }
}
