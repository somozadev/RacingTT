using UnityEngine;

namespace GroundEffects
{
    public class CarTriggerDetection : MonoBehaviour
    {
        protected delegate void TriggerEnterEventHandler(Collider other);

        protected delegate void TriggerExitEventHandler(Collider other);

        protected event TriggerEnterEventHandler TriggerEnterEvent;
        protected event TriggerExitEventHandler TriggerExitEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerSensor"))
            {
                Debug.Log(gameObject.name + " enter!");
                TriggerEnterEvent?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerSensor"))
            {
                Debug.Log(gameObject.name + " exit!");
                TriggerExitEvent?.Invoke(other);
            }
        }
    }
}