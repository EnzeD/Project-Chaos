using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceBuilding : MonoBehaviour
{
    public GameObject buildingPrefab;
    private GameObject previewInstance;
    private bool isPlacing = false;
    private bool isValidLocation = true;

    // UI elements for feedback.
    public GameObject greenCheckUI;
    public GameObject redXUI;

    // Layer names for clarity and easier management.
    private const string previewLayerName = "Preview";
    private const string placedBuildingLayerName = "PlacedBuilding";
  
    // Define a layer mask for collision checks & ground
    private LayerMask placedBuildingLayerMask;
    private LayerMask groundLayerMask;

    // Boundaries
    private readonly Vector3 minBoundary = new Vector3(-27, 0, -28);
    private readonly Vector3 maxBoundary = new Vector3(27, 0, 18);

    // Price
    public int priceInFire = 10;
    public ResourceData resourceData;
    public GameObject notEnoughFirePanel;
    public Transform uiCanvasTransform;

    public ToggleConstructionMenu menu;

    // Quest
    public static bool AFireTowerHasBeenBuilt = false;

    private void Start()
    {
        placedBuildingLayerMask = LayerMask.GetMask(placedBuildingLayerName);
        groundLayerMask = LayerMask.GetMask("Terrain");

        if (menu == null)
        {
            menu = FindObjectOfType<ToggleConstructionMenu>();
            if (menu == null)
            {
                Debug.LogError("ToggleConstructionMenu component not found in the scene.");
            }
        }
    }

    private void Update()
    {
        if (isPlacing)
        {
            PositionPreviewToMouse();
            UpdateUIFeedback();

            if (Input.GetMouseButtonDown(0))
            {
                TryPlaceBuilding();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
        else if (ToggleConstructionMenu.isOpen && !isPlacing)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                menu.ToggleMenu();
            }
        }
    }

    public void StartPlacingBuilding(GameObject prefab)
    {
        if (isPlacing) CancelPlacement();

        buildingPrefab = prefab;
        isPlacing = true;
        previewInstance = Instantiate(buildingPrefab);
        previewInstance.layer = LayerMask.NameToLayer(previewLayerName);

        // Disable collider on the preview instance
        Collider previewCollider = previewInstance.GetComponent<Collider>();
        if (previewCollider != null)
        {
            previewCollider.enabled = false;
        }

        ToggleUIFeedback(true);
    }

    private void PositionPreviewToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 position = hit.point;
            position.y = hit.point.y; // Ensure the building is placed at ground level
            bool withinBoundaries = IsWithinBoundaries(position);

            // Perform an additional raycast to check for obstacles
            bool isObstacleHit = Physics.Raycast(ray, out RaycastHit obstacleHit, Mathf.Infinity, placedBuildingLayerMask);

            isValidLocation = withinBoundaries && !isObstacleHit;
            previewInstance.transform.position = position;
        }
    }

    private bool IsWithinBoundaries(Vector3 position)
    {
        return position.x >= minBoundary.x && position.x <= maxBoundary.x && position.z >= minBoundary.z && position.z <= maxBoundary.z;
    }

    private void UpdateUIFeedback()
    {
        greenCheckUI.SetActive(isValidLocation);
        redXUI.SetActive(!isValidLocation);

        Vector3 uiPosition = Camera.main.WorldToScreenPoint(previewInstance.transform.position) + new Vector3(30, 30, 0);
        greenCheckUI.transform.position = uiPosition;
        redXUI.transform.position = uiPosition;
    }

    private void ToggleUIFeedback(bool isActive)
    {
        greenCheckUI.SetActive(isActive);
        redXUI.SetActive(isActive);
    }

    private void TryPlaceBuilding()
    {
        if (isValidLocation)
        {
            if (resourceData.totalFireCollected >= priceInFire)
            {
                AFireTowerHasBeenBuilt = true;
                resourceData.RemoveFire(priceInFire);

                previewInstance.layer = LayerMask.NameToLayer(placedBuildingLayerName);

                // Enable collider on the placed building
                Collider buildingCollider = previewInstance.GetComponent<Collider>();
                if (buildingCollider != null)
                {
                    buildingCollider.enabled = true;
                }

                // Ensure the building is at the correct height when placed
                Vector3 finalPosition = previewInstance.transform.position;
                finalPosition.y = previewInstance.transform.position.y; // Adjust if necessary based on ground level
                previewInstance.transform.position = finalPosition;
                previewInstance = null;
                ToggleUIFeedback(false);
                isPlacing = false;
            }
            else
            {
                GameObject panelInstance = Instantiate(notEnoughFirePanel, uiCanvasTransform.position, Quaternion.identity, uiCanvasTransform);
                Destroy(panelInstance, 1.5f);
                CancelPlacement();
            }
        }
    }

    private void CancelPlacement()
    {
        Destroy(previewInstance);
        ToggleUIFeedback(false);
        isPlacing = false;
    }
}