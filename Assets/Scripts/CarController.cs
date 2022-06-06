using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    [SerializeField] private List<WheelAxis> wheelAxes;

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform centerOfMass;

    [SerializeField] private float maxSteeringAngle = 30;
    [SerializeField] private float motorMaxTorque = 30;
    [SerializeField] private float breakMaxTorque = 0.5f;

    [SerializeField] private float torqueLoseSpeed;
    [SerializeField] private float topSpeed;

    [SerializeField] private Vector3 flyingTorque;
    [SerializeField] private Vector3 maxApplicableFlyingVelocity;


    private void Start()
    {
        rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        UpdateAllWheelGraphics();
    }

    public void SetSteering(float value)
    {
        foreach (var axis in from axis in wheelAxes where axis.isSteering select axis)
        {
            axis.leftWheelCollider.steerAngle = value * maxSteeringAngle;
            axis.rightWheelCollider.steerAngle = value * maxSteeringAngle;
        }
    }

    public void SetAcceleration(float value)
    {
        float moveSpeed = GetMoveSpeed().magnitude;
        float speedLossFactor = (moveSpeed - torqueLoseSpeed) / (topSpeed - torqueLoseSpeed);
        float torque = Mathf.Lerp(0, motorMaxTorque, 1 - speedLossFactor);

        foreach (var axis in from axis in wheelAxes where axis.isDrive select axis)
        {
            axis.leftWheelCollider.motorTorque = value * torque;
            axis.rightWheelCollider.motorTorque = value * torque;
        }
    }

    public void AddRotation(Vector2 value)
    {
        if (IsGrounded()) return;
        float xTorque = flyingTorque.x * value.y;
        float yTorque = flyingTorque.z * value.x;
        rigidbody.AddRelativeTorque(xTorque, 0, yTorque, ForceMode.Acceleration);
    }

    public void SetBreaks(bool value)
    {
        foreach (var axis in from axis in wheelAxes where axis.hasBrakes select axis)
        {
            axis.leftWheelCollider.brakeTorque = value ? breakMaxTorque : 0;
            axis.rightWheelCollider.brakeTorque = value ? breakMaxTorque : 0;
        }
    }

    public void ResetSpeed()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.ResetInertiaTensor();

        foreach (var axis in wheelAxes)
        {
            axis.leftWheelCollider.enabled = false;
            axis.rightWheelCollider.enabled = false;
            axis.leftWheelCollider.enabled = true;
            axis.rightWheelCollider.enabled = true;
        }
    }

    public bool IsMovingForward() => GetMoveSpeed().z > 0.01f;
    public bool IsMovingBackward() => GetMoveSpeed().z < -0.01f;

    public Vector3 GetMoveSpeed()
    {
        return transform.InverseTransformVector(rigidbody.velocity);
    }
    
    public Vector3 GetRotationSpeed()
    {
        return transform.InverseTransformVector(rigidbody.angularVelocity);
    }

    private bool IsGrounded() =>
        wheelAxes.Any(axis => axis.leftWheelCollider.isGrounded || axis.rightWheelCollider.isGrounded);

    private void UpdateAllWheelGraphics()
    {
        foreach (var axis in wheelAxes)
        {
            UpdateWheelGraphics(axis.leftWheelCollider, axis.leftWheelTransform);
            UpdateWheelGraphics(axis.rightWheelCollider, axis.rightWheelTransform);
        }
    }

    private void UpdateWheelGraphics(WheelCollider wheelCollider, Transform wheelTransform)
    {
        wheelCollider.GetWorldPose(out var position, out var rotation);
        wheelTransform.SetPositionAndRotation(position, rotation);
    }


    [Serializable]
    public class WheelAxis
    {
        public WheelCollider leftWheelCollider;
        public WheelCollider rightWheelCollider;
        public Transform leftWheelTransform;
        public Transform rightWheelTransform;
        public bool isSteering;
        public bool isDrive;
        public bool hasBrakes;
    }
}