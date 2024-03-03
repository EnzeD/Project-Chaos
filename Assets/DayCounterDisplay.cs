using UnityEngine;
using TMPro;

public class DayCounterDisplay : MonoBehaviour
{
    public LightingManager lightingManager;
    public TextMeshProUGUI dayCounterText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dayCounterText.text = "Day " + lightingManager.DayCounter.ToString("0");
    }
}
