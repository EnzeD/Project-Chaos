using UnityEngine;

public class PlayerSpellcasting : MonoBehaviour
{

    public void ChangeSpell(PowerSelection.Power power)
    {
        switch (power)
        {
            case PowerSelection.Power.Fire:
                // Set up fire spell
                break;
            case PowerSelection.Power.Water:
                // Set up water spell
                break;
            case PowerSelection.Power.Earth:
                // Set up earth spell
                break;
        }
    }
}