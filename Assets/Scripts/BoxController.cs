using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject particleSystemPrefab;
    [Tooltip("Reference to the LightningEffect script.")]
    public LightningEffect lightningEffect;
    [Tooltip("Reference to the WallController script.")]
    public WallController wallController;
    private bool isOpen = false;
    private Animator animator;

    // Reference to the AudioSource component on the chest
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;

    public PlayerLogic playerLogic;
    public MessageController messageController;

    float timestamp;
   
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timestamp = Time.time;

        // Get the AudioSource component on the chest
        audioSource1 = GetComponents<AudioSource>()[0];
        audioSource2 = GetComponents<AudioSource>()[1];
        audioSource3 = GetComponents<AudioSource>()[2];
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
            // Play both audio sources
            if (audioSource1 != null)
            {
                audioSource1.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource 1 is not assigned!");
            }

            if (audioSource2 != null)
            {
                audioSource2.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource 2 is not assigned!");
            }


        } else if (playerLogic.GetFragmentCount() >= 3)
            {
                animator.SetTrigger("Close");
                isOpen = false;
                lightningEffect.StopThunderstorm();
                if (audioSource3 != null)
                {
                    audioSource3.Play();
                }
                else
                {
                    Debug.LogWarning("AudioSource 3 is not assigned!");
                }
            }
                                  
        return null;
    }

    private IEnumerator ShowMessages()
    {
        yield return new WaitForSeconds(10);

        messageController.AddMessage("Oh no... Maybe it wasn't the best idea to open the box?!");
        messageController.AddMessage("WHAT ARE THE CHANCES THAT IT'S PANDORA'S BOX??");
        messageController.AddMessage("THEY SHOULD REALLY LABEL THIS KIND OF STUFF...");
        messageController.AddMessage("LET's TRY TO COLLECT ALL THE MISSING PARTS OF THE SEAL TO CLOSE IT AGAIN!");
        yield return new WaitForSeconds(1);
        if (lightningEffect != null)
        {
            lightningEffect.StartThunderstorm();
        }
        else
        {
            Debug.LogError("LightningEffect is not assigned in the Inspector!");
        }
        if (wallController != null)
        {
            wallController.OpenStartingWalls();
        }
        else
        {
            Debug.LogError("WallController is not assigned in the Inspector!");
        }
    }
}
