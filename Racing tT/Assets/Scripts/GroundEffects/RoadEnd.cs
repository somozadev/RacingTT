using UnityEngine;
using UnityEngine.Serialization;

namespace GroundEffects
{
    public class RoadEnd : CarTriggerDetection
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
            timeController.StopTimer();
        }

    }
}