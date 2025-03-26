using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true; // By default alive
    public bool IsFrozen { get; private set; } = true;
    public bool IsLookedAt { get; private set; } = false;
    public Animator animator;

    private NavMeshAgent agent;
    public Transform player; // Reference to the player.

    private AudioSource audioSource;         // Reference to AudioSource

    private void Start()
    {
        // Cache the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (IsAlive && !IsFrozen && !IsLookedAt)
        {
            // Set the destination of the NavMeshAgent to the player's position
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
            agent.isStopped = false;
            animator.speed = 1f;
        }
        else
        {
            // Stop the NavMeshAgent when the enemy shouldn't move
            agent.isStopped = true;
            animator.speed = 0f;
        }
    }

    public void SetLookedAt(bool lookedAt)
    {
        IsLookedAt = lookedAt;
    }

    public void Freeze(bool freeze)
    {
        IsFrozen = freeze;
    }

    public void Kill()
    {
        if (!IsAlive) return; // Prevent multiple triggers
        IsAlive = false;
        Freeze(true); // Freeze when killed

        // Disable enemy visuals and remove after delay
        StartCoroutine(RemoveEnemy());
    }

    private IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(0.5f); // Wait a bit before removal
        Destroy(gameObject);
    }

    public void PlayFootstepSound()
    {
        if (audioSource != null)  // Prevent overlapping sounds
        {
            audioSource.Play();  // Play the footstep sound from the AudioSource attached to the enemy
        }
    }
}
