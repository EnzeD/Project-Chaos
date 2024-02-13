using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Game/Resource Data", order = 1)]
public class ResourceData : ScriptableObject
{
    public int totalFireCollected;

    // Method to add fire
    public void AddFire(int amount)
    {
        totalFireCollected += amount;
        Debug.Log("Total Fire Collected: " + totalFireCollected);
    }
}