using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class CarController : MonoBehaviour
{
    [SerializeField] private GameObject[] lights = new GameObject[4];
    [SerializeField] private bool isLightsOn;

    [SerializeField] private float driftAngle;
    [SerializeField] private List<Transform> frontWheels;
    [SerializeField] private float maxSteeringAngle = 30f;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private CarSuspension carSuspension;

    [SerializeField] private float acceleration;
    [SerializeField] private float turnForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAngularVelocity;
    [SerializeField] private float grip;

    [SerializeField] private List<TrailRenderer> skidmarks;
    [SerializeField] private CarControlsInputActions carControls;

    [SerializeField] private bool isDrivingEnabled;
    [SerializeField] private bool isDrivingForward;
    [SerializeField] private bool isTricksEnabled;
    [SerializeField] private bool skidmarksActive;

    [SerializeField] private float minVelForDrift = 0.5f;
    [SerializeField] private float driftAcc;
    [SerializeField] private float maxDriftSpeed;
    [SerializeField] private float driftTurnForce;
    [SerializeField] private float maxDriftAngularVelocity;

    [SerializeField] private float m_DriftGrip;

    [SerializeField] private float accelerationInput;
    [SerializeField] private float reverseInput;
    [SerializeField] private float resetInput;
    [SerializeField] private Vector2 steeringInput;
    public Vector2 velocity { get; private set; }
    public float driftVal { get; private set; }
    public float speedVal { get; private set; }
    public bool isDrifting { get; private set; }

    private void OnDisable()
    {
        carControls.Disable();
    }

    private void OnEnable()
    {
        carControls.Enable();
    }

    private void Awake()
    {
        carControls = new CarControlsInputActions();
        carControls.CarControls.Reseting.started += delegate { ResetCar(); };
        carControls.CarControls.Lights.started += delegate { ToggleLights(); };
    }

    private void Update()
    {
        accelerationInput = carControls.CarControls.Acceleration.ReadValue<float>();
        reverseInput = carControls.CarControls.ReverseAcceleration.ReadValue<float>();
        resetInput = carControls.CarControls.Reseting.ReadValue<float>();
        steeringInput = carControls.CarControls.Steering.ReadValue<Vector2>();
        RotateFrontWheels();
    }

    private void ResetCar()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
    }

    private void ToggleLights()
    {
        if (isLightsOn)
        {
            foreach (var light in lights)
                light.SetActive(false);
            isLightsOn = false;
        }
        else
        {
            foreach (var light in lights)
                light.SetActive(true);
            isLightsOn = true;
        }
    }


    private void RotateFrontWheels()
    {
        float targetSteeringAngle = steeringInput.x * maxSteeringAngle;

        foreach (var wheel in frontWheels)
        {
            Vector3 currentRotation = wheel.localRotation.eulerAngles;
            float clampedAngle = Mathf.Clamp(targetSteeringAngle, -maxSteeringAngle, maxSteeringAngle);
            currentRotation.y = clampedAngle;
            wheel.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    private void CalculateDrift()
    {
        driftAngle = isDrivingForward
            ? Vector3.Angle(transform.forward, rb.velocity)
            : Vector3.Angle(-transform.forward, rb.velocity);

        if (driftAngle > 50f && driftAngle < 180f && rb.velocity.magnitude > 14f)
            StartDrift();
        else
            EndDrift();
    }

    private void FixedUpdate()
    {
        CalculateDrift();


        if (!isDrivingEnabled)
            return;
        if (carSuspension.isGrounded)
        {
            Vector3 vector = transform.forward;
            if (Physics.Raycast(transform.position, -transform.up, out var hitInfo, 100f))
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.cyan);
                vector = Quaternion.AngleAxis(Vector3.SignedAngle(transform.up, hitInfo.normal, transform.right),
                    transform.right) * transform.forward;
                Debug.DrawLine(transform.position, transform.position + vector * 50f, Color.green);
            }

            Debug.DrawLine(transform.position, transform.position + rb.velocity.normalized * 100, Color.red);
            if (accelerationInput > 0f)
            {
                float num = (isDrifting ? driftAcc : acceleration);
                rb.AddForce(vector * num * accelerationInput, ForceMode.Acceleration);
            }

            if (reverseInput > 0f)
            {
                float num2 = (isDrifting ? driftAcc : acceleration);
                if (rb.velocity.magnitude > 0)
                    rb.AddForce(-vector * num2 * reverseInput, ForceMode.Acceleration);
                if (rb.velocity.magnitude < 0)
                    rb.AddForce(vector * num2 * reverseInput, ForceMode.Acceleration);
            }

            isDrivingForward = Vector3.Angle(rb.velocity, vector) < Vector3.Angle(rb.velocity, -vector);
            float num3 = (isDrifting ? maxDriftSpeed : maxSpeed);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, num3);
            speedVal = Mathf.InverseLerp(0f, num3, rb.velocity.magnitude);
            if (carSuspension.isGrounded)
            {
                steeringInput *= speedVal;
                if (!isDrivingEnabled)
                {
                    steeringInput = -steeringInput;
                }

                float num4 = (isDrifting ? driftTurnForce : turnForce);
                rb.AddTorque(transform.up * steeringInput.x * num4, ForceMode.Acceleration);
                float num5 = (isDrifting ? maxDriftAngularVelocity : maxAngularVelocity);
                rb.angularVelocity = new Vector3(rb.angularVelocity.x, num5 * steeringInput.x, rb.angularVelocity.z);
            }

            float value = Vector3.SignedAngle(rb.velocity, base.transform.forward, Vector3.up);
            driftVal = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-80f, 80f, value));
            if (rb.velocity.magnitude > minVelForDrift)
            {
                rb.AddForce(transform.right * driftVal * (isDrifting ? m_DriftGrip : grip),
                    ForceMode.Acceleration);
            }

            if (!skidmarksActive && isDrifting)
            {
                skidmarksActive = true;
                foreach (TrailRenderer skidmarkRenderer in skidmarks)
                {
                    skidmarkRenderer.emitting = true;
                }
            }
        }
        else if (isTricksEnabled)
        {
            if (accelerationInput > 0f)
            {
                if (steeringInput.x > 0.3f || steeringInput.x < -0.3f)
                    rb.AddTorque(transform.up * steeringInput.x * (acceleration / 5) * accelerationInput,
                        ForceMode.Acceleration);
                if (steeringInput.y > 0.3f || steeringInput.y < -0.3f)
                    rb.AddTorque(transform.right * steeringInput.y * (acceleration / 2.5f) * accelerationInput,
                        ForceMode.Acceleration);
            }
        }
        else if (skidmarksActive && isDrifting)
        {
            skidmarksActive = false;
            foreach (TrailRenderer skidmarkRenderer2 in skidmarks)
            {
                skidmarkRenderer2.emitting = false;
            }
        }

        velocity = rb.velocity;
    }

    private void StartDrift()
    {
        isDrifting = true;
        foreach (TrailRenderer skidmarkRenderer in skidmarks)
        {
            skidmarkRenderer.emitting = true;
        }
    }

    private void EndDrift()
    {
        isDrifting = false;
        foreach (TrailRenderer skidmarkRenderer in skidmarks)
        {
            skidmarkRenderer.emitting = false;
        }
    }
}