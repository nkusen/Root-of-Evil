using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public List<EnemyLogic> enemyList;  // List of all enemies
    public float lookRadius = 10f; // Radius for "looked at" effect
    public float lookAngle = 45f;  // Angle of the cone in front of the player
    public LayerMask enemyLayer;   // Layer for identifying enemies
    public bool showLookAtArea = true;

    void Update()
    {
        UpdateLookedAtEnemies();
    }

    private void UpdateLookedAtEnemies()
    {
        if(enemyList != null){
            // Loop through each enemy in the list
            foreach (EnemyLogic enemy in enemyList)
            {
                if (enemy != null)
                {
                    if(!Input.GetMouseButton(1)){
                        if (enemy.IsLookedAt)
                        {
                            enemy.SetLookedAt(false); // Unset looked at
                        }
                    }
                    else{
                        // Get direction and angle to enemy
                        Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                        // Check if enemy is within the look radius and angle
                        if (Vector3.Distance(transform.position, enemy.transform.position) <= lookRadius && angleToEnemy < lookAngle)
                        {
                            // Set the enemy as looked at if within the area
                            if (!enemy.IsLookedAt)
                            {
                                enemy.SetLookedAt(true); // Set looked at to true
                            }
                        }
                        else
                        {
                            // Unset the looked at status if out of the area
                            if (enemy.IsLookedAt)
                            {
                                enemy.SetLookedAt(false); // Unset looked at
                            }
                        }
                    }
                }
            }
        }
    }
    // Gizmo for visualizing the "look at" area
    private void OnDrawGizmos()
    {
        if (!showLookAtArea) return;
        // Draw the cone shape for lookAngle
        Vector3 forward = transform.forward * lookRadius;
        float angleStep = 5f; // Angle step for drawing the cone lines
        int steps = Mathf.FloorToInt(lookAngle * 2 / angleStep); // Number of lines based on angleStep

        Gizmos.color = Color.green; // Set color for the look angle cone

        // Draw lines forming a cone shape
        for (float i = -lookAngle; i < lookAngle; i += angleStep)
        {
            Quaternion rotation = Quaternion.Euler(0, i, 0); // Rotate by angle step
            Vector3 direction = rotation * forward; // Get the direction based on the rotation
            Gizmos.DrawLine(transform.position, transform.position + direction); // Draw the line
        }
    }
}
