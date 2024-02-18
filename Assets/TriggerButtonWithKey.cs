using UnityEngine;
using UnityEngine.UI;

public class TriggerButtonWithKey : MonoBehaviour
{
    public Button buildButton; // Reference to your UI Button

    void Update()
    {
        // Check if the 'B' key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Check if the button is interactable to prevent triggering disabled buttons
            if (buildButton.interactable)
            {
                // Simulate a click on the button
                buildButton.onClick.Invoke();
            }
        }
    }
}