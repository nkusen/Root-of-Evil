using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform enemyContainer;
    private List<EnemyLogic> enemyList = new List<EnemyLogic>(); // List of frozen enemies.

    [SerializeField] private PlayerLogic player; // Reference to the player.
    [SerializeField] private LightningEffect lightning; // Lightning effect.
    [SerializeField] private float unfreezeDistance = 10f; // Distance to unfreeze enemies.
    [SerializeField] private float checkInterval = 2f;


    void Start()
    {
        PopulateEnemyList();
    }

    public void StartEnemies()
    {
        InvokeRepeating(nameof(CheckAndUnfreezeEnemies), 0f, checkInterval);
    }

    private void PopulateEnemyList()
    {
        if (enemyContainer == null)
        {
            Debug.LogWarning("Enemy container is not assigned!");
            return;
        }

        // Clear the existing list to avoid duplicates.
        enemyList.Clear();

        // Add all EnemyLogic components from the container's children to the list.
        foreach (Transform child in enemyContainer)
        {
            EnemyLogic enemy = child.GetComponent<EnemyLogic>();
            if (enemy != null)
            {
                enemyList.Add(enemy);
            }
        }

        Debug.Log($"Enemy list populated with {enemyList.Count} enemies.");
    }

    private void CheckAndUnfreezeEnemies()
    {
        List<EnemyLogic> enemiesToRemove = new List<EnemyLogic>();

        foreach (var enemy in enemyList)
        {
            if (enemy == null || !enemy.IsFrozen) continue; // Skip if the enemy is already unfrozen or null.

            // Calculate distance from player to enemy.
            float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

            if (distanceToPlayer <= unfreezeDistance)
            {
                // Unfreeze the enemy.
                enemy.Freeze(false);

                // Play the lightning effect.
                if (lightning != null)
                {
                    lightning.SimulateLightning();
                }

                enemiesToRemove.Add(enemy);
            }
        }

        foreach (var enemy in enemiesToRemove)
        {
            enemyList.Remove(enemy);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
