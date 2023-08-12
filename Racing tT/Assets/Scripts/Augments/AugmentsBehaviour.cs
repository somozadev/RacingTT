using GroundEffects;
using UnityEngine;

namespace Augments
{
    public class AugmentsBehaviour : CarTriggerDetection
    {
        private void OnEnable()
        {
            TriggerEnterEvent += ApplyAgument;
        }

        private void OnDisable()
        {
            TriggerEnterEvent -= ApplyAgument;
        }

        private void ApplyAgument(Collider other)
        {
            
        }
    }
}