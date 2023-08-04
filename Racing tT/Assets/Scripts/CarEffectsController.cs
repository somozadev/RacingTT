using System;
using System.Collections;
using GroundEffects;
using UnityEngine;

public class CarEffectsController : MonoBehaviour
{
    private CarController carController;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }


    public void ApplyEffect(CarEffects carEffects)
    {
        switch (carEffects)
        {
            case CarEffects.SpeedEffect:
                break;
            case CarEffects.SlideEffect:
                StartCoroutine(NoGrip());
                break;
            case CarEffects.JumpEffect:
                carController.rb.AddForce(Vector3.up * 50f,ForceMode.Impulse);
                break;
            case CarEffects.NoSteeringEffect:
                StartCoroutine(NoSteering());
                break;
            default:
                break;
        }
    }

    private IEnumerator NoGrip()
    {
        float elapsedTime = 0f;
        carController.CarStats.SetGrip(0,0);
        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        carController.CarStats.ResetGrip();
    }
    private IEnumerator NoSteering()
    {
        float elapsedTime = 0f;
        carController.canSteer = false;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        carController.canSteer = true;
    }
}
