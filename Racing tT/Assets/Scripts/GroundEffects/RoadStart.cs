using UnityEngine;

namespace GroundEffects
{
    public class RoadStart : CarTriggerDetection
    {
        [SerializeField] private TimeController timeController;

        private void OnEnable()
        {
            TriggerEnterEvent += ApplyEffect;
        }

        private void OnDisable()
        {
            TriggerEnterEvent -= ApplyEffect;
        }

        private void ApplyEffect(Collider other)
        {
            timeController.StartTimer();
        }

    }
}