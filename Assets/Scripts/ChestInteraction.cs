using UnityEngine;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public GameObject pressEText;
    public bool isPlayerNear = false;

    private void Update()
    {
        // Display "Press E" text if the player is near
        if (isPlayerNear && !pressEText.activeInHierarchy)
        {
            pressEText.SetActive(true);
        }

        // Check for 'E' press and open chest menu
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Chest opened!");
            // Optionally, hide "Press E" text
            pressEText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            pressEText.SetActive(false); // Hide "Press E" text when the player leaves
        }
    }
}