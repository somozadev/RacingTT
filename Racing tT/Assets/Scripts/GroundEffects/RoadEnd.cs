using UnityEngine;
using UnityEngine.Serialization;

namespace GroundEffects
{
    public class RoadEnd : CarTriggerDetection
    {
        [SerializeField] private TimeController timeController;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            timeController.StopTimer();
        }
    }
}