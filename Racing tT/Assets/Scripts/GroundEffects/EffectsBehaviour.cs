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
            other.GetComponent<CarEffectsController>().ApplyEffect(carEffect);
        }
    }
}