using UnityEngine;

namespace GroundEffects
{
    public class EffectsBehaviour : CarTriggerDetection
    {
        [SerializeField] private CarEffects carEffect;
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            other.GetComponent<CarEffectsController>().ApplyEffect(CarEffects.SpeedEffect);
        }
    }
}