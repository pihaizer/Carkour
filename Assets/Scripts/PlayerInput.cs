using UnityEngine;
using Zenject;

public class PlayerInput
{
    public float Horizontal => CalculateHorizontalInput();

    public float Vertical => CalculateVerticalInput();
    public Vector2 Rotation => CalculateRotationalInput();

    public bool Breaks => _breaksButtonPressed || Input.GetKey(KeyCode.Space);

    private Vector2 _joystickDirection;
    private Vector2 _flyingJoystickDirection;
    private bool _breaksButtonPressed;

    public void SetJoystickDirection(Vector2 input)
    {
        _joystickDirection = input.magnitude > 1 ? input.normalized : input;
    }

    public void SetFlyingJoystickDirection(Vector2 input)
    {
        _flyingJoystickDirection = input.magnitude > 1 ? input.normalized : input;
    }

    public void SetBreaksButton(bool value)
    {
        _breaksButtonPressed = value;
    }

    private float CalculateHorizontalInput()
    {
        if (Mathf.Abs(_joystickDirection.x) < 0.01f) return Input.GetAxisRaw("Horizontal");
        float rawValue = Mathf.Abs(_joystickDirection.x) > 1 ? Mathf.Sign(_joystickDirection.x) : _joystickDirection.x;
        return Mathf.Sign(rawValue) * Mathf.Pow(Mathf.Abs(rawValue), 2);
    }

    private float CalculateVerticalInput()
    {
        if (Mathf.Abs(_joystickDirection.y) < 0.01f) return Input.GetAxisRaw("Vertical");
        return Mathf.Sign(_joystickDirection.y) * _joystickDirection.magnitude;
    }

    private Vector2 CalculateRotationalInput()
    {
        float x = Mathf.Abs(_flyingJoystickDirection.x) > 0.01f ? 
            _flyingJoystickDirection.x : Input.GetAxisRaw("Rotation Horizontal");
        float y = Mathf.Abs(_flyingJoystickDirection.y) > 0.01f ? 
            _flyingJoystickDirection.y : Input.GetAxisRaw("Rotation Vertical");
        return new Vector2(x, y);
    }
}