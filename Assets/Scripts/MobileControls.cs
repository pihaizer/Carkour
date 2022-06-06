using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MobileControls : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private HoldButton breaksButton;

    [Inject] private PlayerInput _input;

    private void Update()
    {
        _input.SetJoystickDirection(joystick.Direction);
        _input.SetBreaksButton(breaksButton.IsDown);
    }
}