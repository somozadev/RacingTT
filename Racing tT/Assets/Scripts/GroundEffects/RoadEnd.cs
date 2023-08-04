using UnityEngine;
using UnityEngine.Serialization;

namespace GroundEffects
{
    public class RoadEnd : CarTriggerDetection
    {
        //[SerializeField] private TimeController timeController;
        [SerializeField] private VoidEvent stopTimerEvent;
        [SerializeField] private VoidEvent resetEvent;

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
            //timeController.StopTimer();
            stopTimerEvent.Raise();
            resetEvent.Raise();
        }

    }
}