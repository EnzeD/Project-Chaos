using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTranform;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerTranform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTranform.position + offset;
    }
}
