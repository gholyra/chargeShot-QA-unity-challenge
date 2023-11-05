using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class InputManager : MonoBehaviour
{

    private GameControls gameControls;

    private void Awake()
    {
        gameControls = new GameControls();
        gameControls.Player.Enable();
    }

    public float GetMovementDirection()
    {
        float inputValue = gameControls.Player.Walk.ReadValue<float>();
        
        return inputValue;
    }
}
