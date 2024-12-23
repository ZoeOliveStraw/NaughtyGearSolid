using UnityEngine;
using UnityEngine.Events;

namespace Actors
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] public UnityEvent onInteract;
    }
}
