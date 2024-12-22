using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player.
    private NavMeshAgent agent; // Reference to the NavMeshAgent.

    private void Start()
    {
        // Cache the NavMeshAgent component.
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Continuously set the destination of the NavMeshAgent to the player's position.
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}
