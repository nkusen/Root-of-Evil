using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject particleSystemPrefab;
    [SerializeField] EnemyController enemyController;
    private bool isOpen = false;
    private Animator animator;

    public PlayerLogic playerLogic;
    public MessageController messageController;

    float timestamp;
   
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Interact()
    {
        if (Time.time - timestamp < 2) return null;

        if (!isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
            StartCoroutine(ShowMessages());
            if (particleSystemPrefab != null)
            {
                
                GameObject spawnedParticles = Instantiate(
                particleSystemPrefab,
                transform.position,    
                transform.rotation     
                );
            
            }

        } else if (playerLogic.GetFragmentCount() >= 3)
            {
                animator.SetTrigger("Close");
                isOpen = false;
            }
                                  
        return null;
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(3);

        messageController.AddMessage("Oh no... Maybe it wasn’t the best idea to open the box?!");
        messageController.AddMessage("WHAT ARE THE CHANCES THAT IT’S PANDORA’S BOX??");
        messageController.AddMessage("THEY SHOULD REALLY LABEL THIS KIND OF STUFF...");
        messageController.AddMessage("LET’s TRY TO COLLECT ALL THE MISSING PARTS OF THE SEAL TO CLOSE IT AGAIN!");
        enemyController.StartEnemies();
    }
}
