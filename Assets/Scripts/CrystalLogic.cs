using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableLogic : MonoBehaviour, IInteractable
{
    [Tooltip("How close the player needs to be to collect the crystal.")]
  
    public string collectableId;

    public string Interact()
    {
        Destroy(gameObject);
        return collectableId;
    }
}
