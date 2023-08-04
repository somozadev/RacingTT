using GroundEffects;
using UnityEngine;

public class CarEffectsController : MonoBehaviour
{
    private CarController carController;
    
    
    
    public void ApplyEffect(CarEffects carEffects)
    {
        switch (carEffects)
        {
            case CarEffects.SpeedEffect:
                break;
            default:
                break;
        }
    }
}