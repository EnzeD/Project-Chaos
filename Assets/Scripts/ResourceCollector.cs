using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceCollector : MonoBehaviour
{
    public GameObject floatingTextPrefab;

    public void DisplayFloatingText()
    {
        Vector3 textPosition = transform.position + new Vector3(0, 2, 0);
        GameObject textObj = Instantiate(floatingTextPrefab, textPosition, Quaternion.identity);
        textObj.transform.SetParent(null); // This ensures the text is not a child of the object
        Destroy(textObj, 2f); // Destroy the text after it has been displayed
    }
}
