using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnJumpAction;
    public event EventHandler OnShootAction;
    
    private GameControls gameControls;

    private void Awake()
    {
        gameControls = new GameControls();
        gameControls.Player.Enable();
        
        gameControls.Player.Jump.performed += Jump_performed;

        gameControls.Player.Shoot.performed += Shoot_performed;
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        OnShootAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }
    
    public float GetMovementDirection()
    {
        float inputValue = gameControls.Player.Walk.ReadValue<float>();
        
        return inputValue;
    }
}
