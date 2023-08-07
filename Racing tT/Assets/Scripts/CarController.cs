using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class CarController : MonoBehaviour
{
    [SerializeField] private float downwardForce = Physics.gravity.magnitude;

    [SerializeField] private GameObject[] lights = new GameObject[4];
    [SerializeField] private bool isLightsOn;

    [SerializeField] private float driftAngle;
    [SerializeField] private List<Transform> frontWheels;
    [SerializeField] private float maxSteeringAngle = 30f;

    [SerializeField] private CarSuspension carSuspension;
    [SerializeField] private CarStats carStats;

    [SerializeField] private List<TrailRenderer> skidmarks;
    [SerializeField] private List<TrailRenderer> airmarks;
    [SerializeField] private CarControlsInputActions carControls;

    [SerializeField] private bool isDrivingEnabled;
    [SerializeField] private bool isDrivingForward;
    [SerializeField] private bool isTricksEnabled;
    [SerializeField] private bool skidmarksActive;
    [SerializeField] private bool onlyStops;
    public bool canSteer { get; set; }
    public bool canAccel { get; set; }


    [SerializeField] private float accelerationInput;
    [SerializeField] private float reverseInput;
    [SerializeField] private float resetInput;
    [SerializeField] private Vector2 steeringInput;

    [SerializeField] private Vector2 velocity;
    public float driftVal { get; private set; }
    public float speedVal { get; private set; }
    public bool isDrifting { get; private set; }
    public Rigidbody rb { get; private set; }
    public CarStats CarStats => carStats;

    private Vector3 accelerationDirection;

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
        isTricksEnabled = true;
        canSteer = true;
        canAccel = true;
        carStats.LoadCarStats();
        rb = GetComponent<Rigidbody>();
        carControls = new CarControlsInputActions();
        carControls.CarControls.Reseting.started += delegate { ResetCar(); };
        carControls.CarControls.Lights.started += delegate { ToggleLights(); };
    }

    private void Update()
    {
        accelerationInput = carControls.CarControls.Acceleration.ReadValue<float>();
        reverseInput = carControls.CarControls.ReverseAcceleration.ReadValue<float>();
        resetInput = carControls.CarControls.Reseting.ReadValue<float>();
        if (canSteer)
            steeringInput = carControls.CarControls.Steering.ReadValue<Vector2>();
        else
            steeringInput = Vector2.zero;

        RotateFrontWheels();
    }

    private void FixedUpdate()
    {
        if (!isDrivingEnabled)
            return;
        CalculateDrift();
        ExtraGravity();
        Vector3 direction = CalculateDirection();
        CalculateMovement(direction);
        CalculateAirLines();
        CalculateRotation();
        CalculateAirTricks();
        velocity = rb.velocity;
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

    private void CalculateRotation()
    {
        if (carSuspension.isGrounded)
        {
            steeringInput *= speedVal;
            if (!isDrivingEnabled)
            {
                steeringInput = -steeringInput;
            }

            float linearSpeed = rb.velocity.magnitude;
            float normalizedSpeed = 1.0f - Mathf.Clamp01((linearSpeed - carStats.minAngularVelocity) /
                                                         (carStats.maxAngularVelocity - carStats.minAngularVelocity));
            float angularSpeed = Mathf.Lerp(carStats.minAngularVelocity, carStats.maxAngularVelocity, normalizedSpeed);
            // Debug.Log(angularSpeed);
            // float num4 = (isDrifting ? carStats.driftTurnForce : carStats.turnForce);
            // // rb.AddTorque(transform.up * steeringInput.x * normalizedSpeed * num4, ForceMode.Acceleration);
            // // float num5 = (isDrifting ? carStats.maxDriftAngularVelocity : carStats.maxAngularVelocity);
            rb.angularVelocity =
                new Vector3(rb.angularVelocity.x, angularSpeed * steeringInput.x, rb.angularVelocity.z);
        }

        float value = Vector3.SignedAngle(rb.velocity, base.transform.forward, Vector3.up);
        driftVal = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-90f, 90f, value));
        if (rb.velocity.magnitude > carStats.minVelForDrift)
        {
            rb.AddForce(transform.right * driftVal * (isDrifting ? carStats.driftGrip : carStats.grip),
                ForceMode.Acceleration);
        }
    }

    private void CalculateMovement(Vector3 direction)
    {
        if (!canAccel) return;

        if (carSuspension.isGrounded)
        {
            if (accelerationInput > 0f)
            {
                float num = (isDrifting ? carStats.driftAcc : carStats.acceleration);
                rb.AddForce(direction * num * accelerationInput, ForceMode.Acceleration);
            }

            if (reverseInput > 0f)
            {
                if (onlyStops)
                {
                    float currentSpeed = rb.velocity.magnitude;
                    float brakeForce = currentSpeed * 3f * reverseInput;
                    rb.AddForce(-rb.velocity.normalized * brakeForce, ForceMode.Acceleration);
                }
                else
                {
                    float num2 = (isDrifting ? carStats.driftAcc : carStats.acceleration);
                    if (rb.velocity.magnitude > 0)
                        rb.AddForce(-direction * num2 * reverseInput, ForceMode.Acceleration);
                    if (rb.velocity.magnitude < 0)
                        rb.AddForce(direction * num2 * reverseInput, ForceMode.Acceleration);
                }
            }

            accelerationDirection = direction.normalized;
        }
        else // Car is in the air
        {
            if (accelerationInput > 0f)
            {
                float num = (isDrifting ? carStats.driftAcc : carStats.acceleration);

                // Usar la dirección de aceleración almacenada mientras está en el suelo
                rb.AddForce(accelerationDirection * num * accelerationInput, ForceMode.Acceleration);
            }
        }

        isDrivingForward = Vector3.Angle(rb.velocity, direction) < Vector3.Angle(rb.velocity, -direction);
        float num3 = (isDrifting ? carStats.maxDriftSpeed : carStats.maxSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, num3);
        speedVal = Mathf.InverseLerp(0f, num3, rb.velocity.magnitude);
    }

    private Vector3 CalculateDirection()
    {
        Vector3 direction = transform.forward;

        if (carSuspension.isGrounded) //adapts car forward to the current surface the car is in.
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hitInfo, 100f))
                direction = Quaternion.AngleAxis(Vector3.SignedAngle(transform.up, hitInfo.normal, transform.right),
                    transform.right) * transform.forward;
        }

        return direction;
    }

    private void ExtraGravity()
    {
        if (!carSuspension.isGrounded)
        {
            Vector3 downDirection = -Physics.gravity.normalized;
            float upwardDotProduct = Vector3.Dot(transform.up, downDirection);
            if (upwardDotProduct < 0.979999f)
            {
                rb.AddForce(downDirection * downwardForce, ForceMode.Acceleration);
                canAccel = false;
            }
            else
                rb.AddForce(downDirection * downwardForce / 10, ForceMode.Acceleration);
        }
        else
            canAccel = true;
    }

    private void CalculateAirTricks()
    {
        if (isTricksEnabled && !carSuspension.isGrounded)
        {
            if (accelerationInput > 0f)
            {
                if (steeringInput.x > 0.3f || steeringInput.x < -0.3f)
                    rb.AddTorque(transform.up * steeringInput.x * (carStats.acceleration / 5) * accelerationInput,
                        ForceMode.Acceleration);
                // if (steeringInput.y > 0.3f || steeringInput.y < -0.3f)
                //     rb.AddTorque(transform.right * steeringInput.y * (carStats.acceleration / 2.5f) * accelerationInput,
                //         ForceMode.Acceleration);
            }
        }
    }


    private void CalculateDrift()
    {
        if (!skidmarksActive)
        {
            isDrifting = false;
            foreach (TrailRenderer skidmarkRenderer in skidmarks)
                skidmarkRenderer.emitting = false;
        }

        driftAngle = isDrivingForward
            ? Vector3.Angle(transform.forward, rb.velocity)
            : Vector3.Angle(-transform.forward, rb.velocity);
        int i = 0;
        foreach (TrailRenderer skidmarkRenderer in skidmarks)
        {
            if (driftAngle > 50f && driftAngle < 180f && rb.velocity.magnitude > 14f)
            {
                if (carSuspension.IndividualGrounded[i])
                {
                    skidmarkRenderer.emitting = true;
                    skidmarkRenderer.time = 1;
                }
                else
                {
                    skidmarkRenderer.emitting = false;
                    skidmarkRenderer.time = 0;
                }
            }
            else
            {
                skidmarkRenderer.emitting = false;
                skidmarkRenderer.time = 1;
            }

            i++;
        }
    }

    private void CalculateAirLines()
    {
        if (!carSuspension.isGrounded)
        {
            if (rb.velocity.magnitude > 8f)
            {
                foreach (var airLine in airmarks)
                {
                    airLine.emitting = true;
                    airLine.time = 1;
                }
            }
            else
            {
                foreach (var airLine in airmarks)
                {
                    airLine.emitting = false;
                    airLine.time = 0;
                }
            }
        }
        else
        {
            foreach (var airLine in airmarks)
            {
                airLine.emitting = false;
                airLine.time = 0;
            }
        }
    }
}