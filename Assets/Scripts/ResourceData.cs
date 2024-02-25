using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Game/Resource Data", order = 1)]
public class ResourceData : ScriptableObject
{
    public int totalFireCollected;
    public int chaosLevel;
    public int luckLevel;
    public bool chaosQuest = false;

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
        chaosLevel = 0;
        luckLevel = 0;
        chaosQuest = false;
    }
    public void IncrementChaos()
    {
        chaosLevel++;
        if (chaosLevel >= 100)
        {
            chaosLevel = 100;
        }
    }
    public void IncreaseLuck(int amount)
    {
        luckLevel+=amount;
        if (luckLevel >= 100)
        {
            luckLevel = 100;
        }
    }
    public void ReduceLuck(int amount)
    {
        luckLevel -= amount;
        if (luckLevel <= 0)
        {
            luckLevel=0;
        }
    }

    public void ReduceChaos(int amount)
    {
        chaosLevel -= amount;
        if (chaosLevel <= 0)
        {
            chaosLevel = 0;
        }
        chaosQuest = true;
    }
}