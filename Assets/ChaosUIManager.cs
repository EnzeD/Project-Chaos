using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class ChaosUIManager : MonoBehaviour
{
    public ResourceData resourceData;
    public TextMeshProUGUI chaosText; 
    public Image chaosFillBar;
    public int maxChaosLevel = 100;

    private void Update()
    {
        if (resourceData != null)
        {
            UpdateChaosUI(resourceData.chaosLevel);
        }
    }

    void UpdateChaosUI(int currentChaosLevel)
    {
        // Update text
        if (chaosText != null)
        {
            chaosText.text = "Chaos (" + currentChaosLevel.ToString() + "%)";
        }

        // Update fill bar
        if (chaosFillBar != null)
        {
            chaosFillBar.fillAmount = (float)currentChaosLevel / maxChaosLevel;
        }
    }
}