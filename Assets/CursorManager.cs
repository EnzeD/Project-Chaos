using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor; // Assign in the inspector
    public Texture2D monsterCursor; // Assign in the inspector

    private void Start()
    {
        // Set the default cursor on start
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the raycast hit a GameObject tagged as "Monster"
            if (hit.collider.CompareTag("Monster"))
            {
                Cursor.SetCursor(monsterCursor, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}