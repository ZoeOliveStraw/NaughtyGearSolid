using System.Collections.Generic;
using Actors;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractionZone : MonoBehaviour
{
    private List<InteractableObject> _interactableObjects;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Interactable")
        {
            if(_interactableObjects == null) _interactableObjects = new List<InteractableObject>();
            _interactableObjects.Add(col.GetComponent<InteractableObject>());
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Interactable")
        {
            if (_interactableObjects.Contains(col.GetComponent<InteractableObject>()))
            {
                _interactableObjects.Remove(col.GetComponent<InteractableObject>());
            }
        }
    }

    public InteractableObject GetCurrentInteraction()
    {
        return GetClosestInteraction();
    }

    private InteractableObject GetClosestInteraction()
    {
        InteractableObject closest = null;
        foreach (var interactable in _interactableObjects)
        {
            if(closest == null) closest = interactable;
            if (Vector3.Distance(interactable.transform.position, transform.position) <
                Vector3.Distance(closest.transform.position, transform.position))
                closest = interactable;
        }
        return closest;
    }
}
