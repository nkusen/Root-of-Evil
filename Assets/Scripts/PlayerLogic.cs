using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] Transform enemyContainer;
    private List<EnemyLogic> enemyList = new List<EnemyLogic>();  // List of all enemies
    public float lookRadius = 10f; // Radius for "looked at" effect
    public float lookAngle = 45f;  // Angle of the cone in front of the player
    public LayerMask enemyLayer;   // Layer for identifying enemies
    public bool showLookAtArea = true;
    public float greenCrystalRange = 30f;
    public float redCrystalRange = 20f;

    public MessageController messageController;
    private bool enemyMet = false;

    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    private bool greenPickedUp, redPickedUp;

    public float InteractRange;
    public Transform InteractorSource;

    public Canvas interactUI;

    private AudioSource pickupSound;

    public TMP_Text fragment;
    public TMP_Text greenCrystal;
    public TMP_Text redCrystal;

    public EndMenu endMenu;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateGreenCrystal();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ActivateRedCrystal();
        }
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

    private void Start()
    {
        interactUI.enabled = false;
        pickupSound = GetComponent<AudioSource>();
        PopulateEnemyList();
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

    private void ActivateGreenCrystal()
    {
        // Consume one green crystal
        if (GetGreenCrystalCount() > 0)
        {
            // Loop through each enemy in the list and freeze the ones within the range
            foreach (EnemyLogic enemy in enemyList)
            {
                if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= greenCrystalRange)
                {
                    enemy.Freeze(true);  // Call the freeze method on the enemy
                }
            }
            inventory["greenCrystal"] -= 1;  // Reduce the green crystal count by 1
            RefreshHudCount();
        }
        else
        {
            Debug.Log("Not enough green crystals");
        }
    }

    private void ActivateRedCrystal()
    {
        // Consume one green crystal
        if (GetRedCrystalCount() > 0)
        {
            // Loop through each enemy in the list and kill the ones within the range
            foreach (EnemyLogic enemy in enemyList)
            {
                if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= redCrystalRange)
                {
                    enemy.Kill();  // Call the freeze method on the enemy
                }
            }
            inventory["redCrystal"] -= 1;  // Reduce the green crystal count by 1
            RefreshHudCount();
        }
        else
        {
            Debug.Log("Not enough red crystals");
        }
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
            RefreshHudCount();

            if (!greenPickedUp && objectID == "greenCrystal")
            {
                greenPickedUp = true;
                messageController.AddMessage("You just picked up a green crystal which freezes nearby enemies.");
                messageController.AddMessage("Press F to activate it. This will consume one green crystal!");
            }
            if (!redPickedUp && objectID == "redCrystal")
            {
                redPickedUp = true;
                messageController.AddMessage("You just picked up a red crystal which kills nearby enemies.");
                messageController.AddMessage("Press G to activate it. This will consume one red crystal!");
            }
        }
        else
        {
            interactObj.Interact();
        }
    }

    private void RefreshHudCount()
    {
        greenCrystal.text = GetGreenCrystalCount().ToString();
        redCrystal.text = GetRedCrystalCount().ToString();
        fragment.text = GetFragmentCount().ToString() + " / 3";
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(0.5f);

        messageController.AddMessage("WAIT...DID THE STATUE JUST MOVE??");
        messageController.AddMessage("Maybe I should try to stop them by pressing right click!");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Check if collided object is tagged as "Enemy"
        {
            EnemyLogic enemy = collision.gameObject.GetComponent<EnemyLogic>();
            if (!enemy.IsFrozen && !enemy.IsLookedAt && enemy.IsAlive)
            {
                endMenu.Lose();
            }
        }
    }
}
