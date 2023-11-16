using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameControls gameControls;

    public class OnHoldInteractionData : EventArgs
    {
        public GameControls gameControls;
    }
    
    public event EventHandler<OnHoldInteractionData> OnJumpAction;
    public event EventHandler<OnHoldInteractionData> OnShootAction;
    
    private void Awake()
    {
        gameControls = new GameControls();
        gameControls.Player.Enable();
        
        gameControls.Player.Jump.started += Jump_started;

        gameControls.Player.Shoot.started += Shoot_started;
    }

    private void Shoot_started(InputAction.CallbackContext obj)
    {
        OnShootAction?.Invoke(this, new OnHoldInteractionData()
        {
            gameControls = this.gameControls
        });
    }

    private void Jump_started(InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, new OnHoldInteractionData()
        {
            gameControls = this.gameControls
        });
    }
    
    public float GetMovementDirection()
    {
        float inputValue = gameControls.Player.Walk.ReadValue<float>();
        
        return inputValue;
    }
}
