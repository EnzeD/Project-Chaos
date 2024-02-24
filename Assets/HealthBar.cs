using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;
    public GameObject healthBarCanvas;
    public Vector3 offset = new Vector3(0, 2, 0);
    public GameObject damageTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    // Update is called once per frame
    public void UpdateHealth(float healthChange)
    {
        currentHealth = currentHealth + healthChange;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        fillImage.fillAmount = currentHealth / maxHealth;
        ShowDamageText(-healthChange, transform.position + Vector3.up*4);
    }

    void Update()
    {
        if (currentHealth == maxHealth)
        {
            healthBarCanvas.SetActive(false);
        }
        else
        {
            healthBarCanvas.SetActive(true);
        }
        healthBarCanvas.transform.position = transform.position + offset;
        healthBarCanvas.transform.LookAt(healthBarCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
    private void ShowDamageText(float damage, Vector3 position)
    {
        // Instantiate the damage text prefab at the position without setting it as a child of the monster.
        GameObject damageTextInstance = Instantiate(damageTextPrefab, position, Quaternion.identity);

        DamageText damageTextScript = damageTextInstance.GetComponent<DamageText>();
        if (damageTextScript != null)
        {
            damageTextScript.SetDamage(damage);
        }
    }
}
