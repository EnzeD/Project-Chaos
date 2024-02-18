using UnityEngine;

public class BuildingValidation : MonoBehaviour
{
    public bool isColliding = false;
    public bool isValidLocation = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Terrain"))
        {
            isColliding = true;
            isValidLocation = !isColliding;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Terrain"))
        {
            isColliding = false;
            isValidLocation = !isColliding;
        }
    }

    public void CheckCollision()
    {
        // This method forces a re-check of the collision status based on the current position.
        // Useful if you need to force update validation without waiting for OnTrigger events.
        // This could involve raycasting or overlap checks specific to your game's requirements.
    }
}