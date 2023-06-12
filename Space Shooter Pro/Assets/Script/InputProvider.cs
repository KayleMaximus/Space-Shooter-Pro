using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private static GameInputActions _input = new();

    public void Enable()
    {
        _input.Player.Move.Enable();
        _input.Player.Shoot.Enable();
    }


    public void Disable()
    {
        _input.Player.Move.Disable();
        _input.Player.Shoot.Disable();
    }


    public event Action<InputAction.CallbackContext> shootPerform
    {
        add
        {
            _input.Player.Shoot.performed += value;
        }
        remove
        {
            _input.Player.Shoot.performed -= value;
        }
    }

    public Vector2 MovementInput()
    {

        return _input.Player.Move.ReadValue<Vector2>();
    }
}
