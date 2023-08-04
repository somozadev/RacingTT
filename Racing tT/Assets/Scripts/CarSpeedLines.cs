using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CarSpeedLines : MonoBehaviour
    {
        [SerializeField] private ParticleSystem speedLines;
        private ParticleSystem.EmissionModule partEmission;
        private ParticleSystem.ShapeModule shapeModule;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float maxEmission = 185f;
        [SerializeField] private float minVelocity = 28f;
        [SerializeField] private float maxVelocity = 49f;
        [SerializeField] private float minRadius = 15f;
        [SerializeField] private float maxRadius = 12f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            partEmission = speedLines.emission;
            shapeModule = speedLines.shape;
        }

        private void Update()
        {
            UpdateLines();
        }

        private void UpdateLines()
        {
            if (rb.velocity.magnitude >= minVelocity)
            {
                var t = Mathf.InverseLerp(minVelocity, maxVelocity, rb.velocity.magnitude);
                partEmission.rateOverTime = (maxEmission * rb.velocity.magnitude) / maxVelocity;
                shapeModule.radius = Mathf.Lerp(minRadius, maxRadius, t);
            }
            else
            {
                partEmission.rateOverTime = 0;
            }
        }
    }
}