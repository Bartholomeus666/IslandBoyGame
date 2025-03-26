using UnityEngine;
using UnityEngine.AI;

public class NavMeshTry : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.transform.position;
    }

    private void Update()
    {
        agent.destination = target.transform.position;
    }
}
