using UnityEngine;

namespace GroundEffects
{
    public class CarTriggerDetection : MonoBehaviour
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        }
    }
}