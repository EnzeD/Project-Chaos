using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Game/Resource Data", order = 1)]
public class ResourceData : ScriptableObject
{
    public int totalFireCollected;

    // Method to add fire
    public void AddFire(int amount)
    {
        totalFireCollected += amount;
    }

    public void RemoveFire(int amount)
    {
        totalFireCollected -= amount;
    }

    // Resets the resource data to its default values
    public void ResetData()
    {
        totalFireCollected = 0;
        // Add any other fields you wish to reset and set their default values here
    }
}