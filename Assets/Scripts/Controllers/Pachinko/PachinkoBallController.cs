using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PachinkoBallController : MonoBehaviour
{
	private Rigidbody2D rigidbody2D;
	private Vector3 spawnPosition;
	public float maxCharge;

	private void Awake()
	{
		spawnPosition = transform.position;
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		PachinkoEvents.OnResetBallRequested += OnResetBallRequested;
	}

	private void OnResetBallRequested()
	{
		rigidbody2D.velocity = Vector3.zero;
		transform.position = spawnPosition;
	}

	public void Push(float chargePercent)
	{
		rigidbody2D.AddForce(Vector2.up * maxCharge * chargePercent);
	}

}
