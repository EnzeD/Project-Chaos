using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class LuckUIManager : MonoBehaviour
{
    public ResourceData resourceData;
    public TextMeshProUGUI luckText; 
    public Image luckFillBar;
    public int maxLuckLevel = 100;

    private void Update()
    {
        if (resourceData != null)
        {
            UpdateLuckUI(resourceData.luckLevel);
        }
    }

    void UpdateLuckUI(int currentLuckLevel)
    {
        // Update text
        if (luckText != null)
        {
            luckText.text = "Luck (" + currentLuckLevel.ToString() + "%)";
        }

        // Update fill bar
        if (luckFillBar != null)
        {
            luckFillBar.fillAmount = (float)currentLuckLevel / maxLuckLevel;
        }
    }
}