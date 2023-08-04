using UnityEngine;

namespace GroundEffects
{
    public class RoadStart : CarTriggerDetection
    {
        //[SerializeField] private TimeController timeController;
        [SerializeField] private VoidEvent startTimerEvent;

        private void OnEnable()
        {
            TriggerExitEvent += ApplyEffect;
        }

        private void OnDisable()
        {
            TriggerExitEvent -= ApplyEffect;
        }

        private void ApplyEffect(Collider other)
        {
            startTimerEvent.Raise();
            //timeController.StartTimer();
        }

    }
}