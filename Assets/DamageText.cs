using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;

    // Start is called before the first frame update
    void Awake()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        Destroy(gameObject, 1f); // Destroy after 1 second
    }

    // Update is called once per frame
    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString();
    }

    private void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
