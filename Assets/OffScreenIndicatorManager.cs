using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class OffScreenIndicatorManager : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public Transform playerTransform;
    private Camera mainCamera;
    private List<GameObject> indicators = new List<GameObject>();
    private List<Transform> monsters = new List<Transform>();
    public RectTransform canvasRectTransform;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateIndicators();
        //Debug.Log($"Updating indicators. Monsters count: {monsters.Count}, Indicators count: {indicators.Count}");
    }

    public void AddMonster(Transform monsterTransform)
    {
        if (!monsters.Contains(monsterTransform))
        {
            monsters.Add(monsterTransform);
            GameObject newIndicator = Instantiate(indicatorPrefab, canvasRectTransform, false);
            indicators.Add(newIndicator); // Add the new indicator to the list
        }
    }

    public void RemoveMonster(Transform monsterTransform)
    {
        if (monsters.Contains(monsterTransform))
        {
            int index = monsters.IndexOf(monsterTransform);
            if (index != -1)
            {
                // Remove the monster and its corresponding indicator
                Destroy(indicators[index]);
                indicators.RemoveAt(index);
                monsters.RemoveAt(index);
            }
        }
    }

    void UpdateIndicators()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        float padding = 30f; // Padding from the edge of the screen

        for (int i = 0; i < monsters.Count; i++)
        {
            var screenPoint = mainCamera.WorldToViewportPoint(monsters[i].position);
            bool isOffScreen = screenPoint.x <= 0 || screenPoint.x >= 1 || screenPoint.y <= 0 || screenPoint.y >= 1;
            indicators[i].SetActive(isOffScreen);

            if (isOffScreen)
            {
                Vector3 directionFromPlayerToWorld = (monsters[i].position - playerTransform.position).normalized;
                Vector2 direction = new Vector2(directionFromPlayerToWorld.x, directionFromPlayerToWorld.z);
                Vector2 screenBounds = screenCenter - new Vector2(padding, padding);

                // Calculate the intersection point with the screen bounds
                Vector2 intersection = CalculateIntersectionPointWithScreenBounds(screenBounds, direction, screenCenter);

                // Adjust the position to canvas space
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, intersection, null, out canvasPosition);

                indicators[i].GetComponent<RectTransform>().anchoredPosition = canvasPosition;

                // Calculate the actual distance between the player and the monster
                float actualDistance = Vector3.Distance(playerTransform.position, monsters[i].position);
                // Determine the alpha value based on the distance
                float alpha = Mathf.Lerp(255, 50, Mathf.InverseLerp(10, 100, actualDistance));

                // Set the new color with adjusted alpha
                Image indicatorImage = indicators[i].GetComponent<Image>();
                Color currentColor = indicatorImage.color;
                indicatorImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha / 255f); // Convert alpha to 0-1 range


                // Calculate rotation so it points towards the monster's direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                indicators[i].transform.rotation = Quaternion.Euler(0, 0, angle - 180); // Adjust rotation if necessary
            }
        }
    }

    Vector2 CalculateIntersectionPointWithScreenBounds(Vector2 screenBounds, Vector2 direction, Vector2 screenCenter)
    {
        float m = direction.y / direction.x;

        Vector2 intersection = new Vector2();
        if (Mathf.Abs(m) > screenBounds.y / screenBounds.x)
        {
            // Intersect top/bottom screen edge
            intersection.y = Mathf.Sign(direction.y) * screenBounds.y;
            intersection.x = intersection.y / m;
        }
        else
        {
            // Intersect left/right screen edge
            intersection.x = Mathf.Sign(direction.x) * screenBounds.x;
            intersection.y = m * intersection.x;
        }

        // Convert to screen space
        intersection += screenCenter;

        return intersection;
    }

    /* CIRCLE
    void UpdateIndicators()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            var screenPoint = mainCamera.WorldToViewportPoint(monsters[i].position);
            bool isOffScreen = screenPoint.x <= 0 || screenPoint.x >= 1 || screenPoint.y <= 0 || screenPoint.y >= 1;
            indicators[i].SetActive(isOffScreen);

            if (isOffScreen)
            {
                // Calculate the direction from the player to the monster in world space
                Vector3 directionFromPlayerToWorld = (monsters[i].position - playerTransform.position).normalized;

                // Project this direction onto the screen
                Vector3 screenDirectionFromPlayer = mainCamera.WorldToScreenPoint(playerTransform.position + directionFromPlayerToWorld) - mainCamera.WorldToScreenPoint(playerTransform.position);
                // Convert to Vector2 for 2D calculations
                Vector2 screenDirectionFromPlayer2D = new Vector2(screenDirectionFromPlayer.x, screenDirectionFromPlayer.y).normalized;

                // Use the player's screen position as the base for the indicator's position
                Vector2 playerScreenPosition = new Vector2(0, 0);
                // Calculate the indicator's position on a circle around the player's screen position
                Vector2 indicatorPositionOnCircle = playerScreenPosition + screenDirectionFromPlayer2D * 300; // 300 pixels radius

                // Convert this screen position to a position within the canvas
                indicators[i].GetComponent<RectTransform>().anchoredPosition = indicatorPositionOnCircle;

                // Calculating the rotation of the indicator so it points towards the monster
                float angle = Mathf.Atan2(screenDirectionFromPlayer2D.y, screenDirectionFromPlayer2D.x) * Mathf.Rad2Deg;
                indicators[i].transform.rotation = Quaternion.Euler(0, 0, angle-180);
            }
        }
    }*/
}
