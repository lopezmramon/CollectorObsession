using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, PlayerInputs.IPachinkoInputsActions
{
    private PlayerInputs inputActions;

	private void Awake()
	{
		inputActions = new PlayerInputs();
		SetupPachinkoInputs();
	}

	private void SetupPachinkoInputs()
	{
		inputActions.PachinkoInputs.Enable();
		inputActions.PachinkoInputs.ResetBall.performed += OnResetBall;
	}

	public void OnResetBall(InputAction.CallbackContext context)
	{
		PachinkoEvents.OnResetBallRequested?.Invoke();
	}

	

}
