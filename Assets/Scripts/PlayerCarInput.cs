using System;
using UnityEngine;
using Zenject;

public class PlayerCarInput : MonoBehaviour
{
    [SerializeField] private CarController target;
    [Range(1e-9f, 1)]
    [SerializeField] private float steeringLerp = 0.1f;
    
    [Inject] private PlayerInput _input;

    private float _currentSteering;

    private void FixedUpdate()
    {
        if (Mathf.Abs(_input.Horizontal) > Mathf.Abs(_currentSteering))
            _currentSteering = Mathf.Lerp(_currentSteering, _input.Horizontal, steeringLerp);
        else _currentSteering = 0;
        
        target.SetSteering(_currentSteering);
        
        if ((target.IsMovingForward() && _input.Vertical < -0.01f) || 
            (target.IsMovingBackward() && _input.Vertical > 0.01f))
        {
            target.SetBreaks(true);
            return;
        }
        
        target.SetAcceleration(_input.Vertical);
        target.SetBreaks(_input.Breaks);
        
        target.AddRotation(_input.Rotation);
    }
}