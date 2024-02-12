using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject buildingPrefab;
    private GameObject previewInstance;
    private bool isPlacing = false;
    public void StartPlacingBuilding(GameObject prefab)
    {
        if (isPlacing && previewInstance != null)
        {
            Destroy(previewInstance); // Destroy the current preview if already placing another building
        }

        buildingPrefab = prefab; // Set the building prefab to the one selected
        isPlacing = true;
        previewInstance = Instantiate(buildingPrefab);
        // TO DO: add transparency?
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlacing)
        {
            MovePreviewToMouse();

            if (Input.GetMouseButtonDown(0)) // Confirm placement with left-click
            {
                Place();
            }
            else if (Input.GetMouseButtonDown(1)) // Cancel placement with right-click
            {
                StopPlacing(true);
            }
        }
    }

    void MovePreviewToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 position = hit.point;
            previewInstance.transform.position = position;
        }
    }

    void Place()
    {
        isPlacing = false;
        previewInstance = null; // Clear the preview instance so we're ready to place a new one
    }
    void StopPlacing(bool cancel)
    {
        if (cancel && previewInstance != null)
        {
            Destroy(previewInstance); // Destroy preview instance if placement is cancelled
        }
        isPlacing = false;
    }
}
