using UnityEngine;

namespace GroundEffects
{
    public class RoadStart : CarTriggerDetection
    {
        [SerializeField] private TimeController timeController;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            timeController.ResetTimer();
            timeController.StartTimer();
        }
    }
}