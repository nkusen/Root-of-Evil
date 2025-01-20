using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    private Animator animator;

    public PlayerLogic playerLogic;

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

        } else if (playerLogic.GetFragmentCount() >= 0)
            {
                animator.SetTrigger("Close");
                isOpen = false;
            }
                                  
        return null;
    }
}
