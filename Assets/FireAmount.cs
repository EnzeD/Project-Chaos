using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireAmount : MonoBehaviour
{
    public ResourceData resourceData; 
    public TextMeshProUGUI resourceText;

    // Update is called once per frame
    void Update()
    {
        resourceText.text = "Fire: " + resourceData.totalFireCollected.ToString();
    }
}
