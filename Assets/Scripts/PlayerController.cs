using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject clickEffectPrefab;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Instantiate(clickEffectPrefab, hitInfo.point + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0));
                agent.SetDestination(hitInfo.point);
            }
        }
        bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
    }
}
