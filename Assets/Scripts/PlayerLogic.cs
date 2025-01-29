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

    public MessageController messageController;
    private bool enemyMet = false;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public float InteractRange;
    public Transform InteractorSource;

    public Canvas interactUI;

    private AudioSource pickupSound;

    void Update()
    {
        UpdateLookedAtEnemies();


        //interaction logic
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            if (hitObject.TryGetComponent<IInteractable>(out IInteractable interactObj))
            {
                //dodaj UI popup za interact

                interactUI.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractHandler(interactObj, hitObject.tag);
                }
            } else { interactUI.enabled = false; }
        } else { interactUI.enabled = false; }

    }

    private void Start()
    {
        interactUI.enabled = false;
        pickupSound = GetComponent<AudioSource>();
    }

    public int GetFragmentCount()
    {
        inventory.TryGetValue("fragment", out int count);
        return count;
    }

    public int GetGreenCrystalCount()
    {
        inventory.TryGetValue("greenCrystal", out int count);
        return count;
    }

    public int GetRedCrystalCount()
    {
        inventory.TryGetValue("redCrystal", out int count);
        return count;
    }


    private void UpdateLookedAtEnemies()
    {
        if(enemyList != null){
            // Loop through each enemy in the list
            foreach (EnemyLogic enemy in enemyList)
            {
                if (enemy != null)
                {
                    if (!enemyMet)
                    {
                        // Get direction and angle to enemy
                        Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                        float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                        // Check if enemy is within the look radius and angle and is moving
                        if (enemy.IsAlive && !enemy.IsFrozen && Vector3.Distance(transform.position, enemy.transform.position) <= lookRadius && angleToEnemy < lookAngle)
                        {
                            enemyMet = true;
                            StartCoroutine(ShowMessages());
                        }
                    }

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

    private void InteractHandler(IInteractable interactObj, string tag)
    {
        if (tag.Equals("Collectible"))
        {
            string objectID = interactObj.Interact();
            GetComponent<PlayerLogic>().inventory.TryGetValue(objectID, out int currentCount);
            GetComponent<PlayerLogic>().inventory[objectID] = currentCount + 1;
            pickupSound.Play();
        }
        else
        {
            interactObj.Interact();
        }
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(0.5f);

        messageController.AddMessage("WAIT...DID THE STATUE JUST MOVE??");
        messageController.AddMessage("Maybe I should try to stop them by pressing right click!");
    }
}
