using UnityEngine;
using UnityEngine.UI;

public class PowerSelection : MonoBehaviour
{
    public SpellShooter spellShooter;
    public enum Power { Fire, Water, Earth }
    public Power currentPower = Power.Fire;

    public GameObject fireImage;
    public GameObject waterImage;
    public GameObject earthImage; 

    public PlayerSpellcasting playerSpellcasting; 

    void Update()
    {
        // Detect mouse wheel scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                ChangePower(-1); // Scroll up
            }
            else
            {
                ChangePower(1); // Scroll down
            }
            UpdateUI();
            playerSpellcasting.ChangeSpell(currentPower); // Update the spellcasting
            spellShooter.SetCurrentPower((SpellShooter.PowerType)currentPower); // Cast enum to match type
        }
    }

    private void ChangePower(int direction)
    {
        currentPower += direction;

        // Make sure the enum value stays within its defined range
        if (currentPower > Power.Earth)
        {
            currentPower = Power.Fire;
        }
        else if (currentPower < Power.Fire)
        {
            currentPower = Power.Earth;
        }
    }

    private void UpdateUI()
    {
        fireImage.SetActive(currentPower == Power.Fire);
        waterImage.SetActive(currentPower == Power.Water);
        earthImage.SetActive(currentPower == Power.Earth);
    }
}