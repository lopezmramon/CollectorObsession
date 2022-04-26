using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PachinkoMovingNotchController : MonoBehaviour
{
    public bool rotate, usePath;
	private Vector3 rotationPerFrame = new Vector3(0, 0, 0.5f);
	public Vector3[] path;
	private void Awake()
	{
		SetupMovement();
	}

	private void SetupMovement()
	{
		transform.DOPath(path, 3).SetLoops(-1, LoopType.Yoyo);
	}

	private void Update()
	{
		if (rotate)
		{
			transform.eulerAngles += rotationPerFrame;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		for (int i = 0; i < path.Length; i++)
		{
			Gizmos.DrawSphere(path[i], 0.5f);
		}
	}
}
