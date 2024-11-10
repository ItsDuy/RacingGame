using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private bool isBraking;
    private bool wasBoosting;

    private bool isDrifting = false;
    private int driftDirection = 0;
    public float driftTimer = 0f;
    public float driftDuration = 2f;
    public float driftSteerMultiplier = 1.5f;

    [Header("Motor Settings")]
    [SerializeField] public float motorForce = 15000f;
    [SerializeField] public float boostMotorForce = 50000f;
    [SerializeField] public float brakeForce = 10000f;
    [SerializeField] public float maxSteerAngle = 35f;
    [SerializeField] public float maxSpeed = 180f;
    [SerializeField] public float deceleration = 10000f;
    private float speed;

    [Header("Boost Settings")]
    [SerializeField] public float maxNitro = 100f;
    [SerializeField] public float currentNitro;
    [SerializeField] public float fuelChargeRate = 10f;
    [SerializeField] public float nitroConsumptionRate = 50f;
    [SerializeField] public float boostMaxSpeed = 300f;
    [SerializeField] private KeyCode boostKey = KeyCode.LeftShift;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [Header("Trail Renderers (Skid Marks)")]
    [SerializeField] private TrailRenderer rearLeftTrail, rearRightTrail;

    private Rigidbody rb;

    [Header("Nitro UI")]
    public NitroFuelSlider nitroFuelSlider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        currentNitro = maxNitro;
        wasBoosting = false;

        // Initially disable trails
        ToggleTrails(false);
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleBoost();
        HandleDrift();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleBoost()
    {
        if (Input.GetKey(boostKey) && currentNitro > 0 && verticalInput > 0)
        {
            if (!wasBoosting)
            {
                ActivateBoost();
            }

            wasBoosting = true;
            currentNitro -= nitroConsumptionRate * Time.fixedDeltaTime;
            currentNitro = Mathf.Clamp(currentNitro, 0, maxNitro);
            nitroFuelSlider.SetFuel(currentNitro);
            
        }
        else
        {
            wasBoosting = false;
            currentNitro += fuelChargeRate * Time.fixedDeltaTime;
            currentNitro = Mathf.Clamp(currentNitro, 0, maxNitro);
            nitroFuelSlider.SetFuel(currentNitro);
        }
    }

    private void ActivateBoost()
    {
        float boostSpeedMps = currentNitro == maxNitro ? boostMaxSpeed / 5.0f : boostMaxSpeed / 6.0f;

        Vector3 currentDirection = rb.velocity.normalized;

        rb.velocity = currentDirection * boostSpeedMps;

        rb.AddForce(transform.forward * boostMotorForce, ForceMode.Impulse);
    }

    private void HandleDrift()
    {
        if (Input.GetButtonDown("Jump") && !isDrifting)
        {
            StartDrift();
        }

        if (isDrifting)
        {
            driftTimer -= Time.fixedDeltaTime;
            if (driftTimer <= 0)
            {
                EndDrift();
            }

            float steer = horizontalInput * driftSteerMultiplier;
            frontLeftWheelCollider.steerAngle = Mathf.Clamp(steer, -maxSteerAngle, maxSteerAngle);
            frontRightWheelCollider.steerAngle = Mathf.Clamp(steer, -maxSteerAngle, maxSteerAngle);
        }
    }

    private void StartDrift()
    {
        isDrifting = true;
        driftDirection = horizontalInput > 0 ? 1 : -1;
        driftTimer = driftDuration;
        ToggleTrails(true);
    }

    private void EndDrift()
    {
        isDrifting = false;
        ToggleTrails(false);
    }

    private void HandleMotor()
    {
        speed = rb.velocity.magnitude * 3.6f;
        float effectiveMotorForce = wasBoosting ? boostMotorForce : motorForce;
        float effectiveMaxSpeed = wasBoosting ? boostMaxSpeed : maxSpeed;
        
        if (speed < effectiveMaxSpeed || verticalInput < 0)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * effectiveMotorForce;
            frontRightWheelCollider.motorTorque = verticalInput * effectiveMotorForce;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0;
            frontRightWheelCollider.motorTorque = 0;
        }

        if (isBraking)
        {
            ApplyBraking();
        }
        else if (verticalInput == 0)
        {
            ApplyDeceleration();
        }
        else
        {
            ReleaseBrakes();
        }
    }

    private void ApplyBraking()
    {
        ApplyBrakeTorque(brakeForce);
    }

    private void ApplyDeceleration()
    {
        ApplyBrakeTorque(deceleration);
    }

    private void ReleaseBrakes()
    {
        ApplyBrakeTorque(0f);
    }

    private void ApplyBrakeTorque(float brakeTorque)
    {
        frontLeftWheelCollider.brakeTorque = brakeTorque;
        frontRightWheelCollider.brakeTorque = brakeTorque;
        rearLeftWheelCollider.brakeTorque = brakeTorque;
        rearRightWheelCollider.brakeTorque = brakeTorque;
    }

    private void HandleSteering()
    {
        if (!isDrifting)
        {
            float currentSteerAngle = maxSteerAngle * horizontalInput;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    public float GetCurrentNitro()
    {
        return currentNitro;
    }

    public float GetMaxNitro()
    {
        return maxNitro;
    }

    public float GetCurrentSpeed()
    {
        return speed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    private void ToggleTrails(bool enable)
    {
        rearLeftTrail.emitting = enable;
        rearRightTrail.emitting = enable;
    }
}
