using System;
using UnityEngine;

namespace GroundEffects
{
    public class EffectsBehaviour : CarTriggerDetection
    {
        [SerializeField] private CarEffects carEffect;

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
            if (other.GetComponent<CarEffectsController>())
                other.GetComponent<CarEffectsController>().ApplyEffect(carEffect);
            else
                other.GetComponentInParent<CarEffectsController>().ApplyEffect(carEffect);
        }
    }
}