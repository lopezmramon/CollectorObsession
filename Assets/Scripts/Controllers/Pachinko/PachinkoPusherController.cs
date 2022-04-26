using System;
using UnityEngine;
using DG.Tweening;

public class PachinkoPusherController : MonoBehaviour
{
	private Vector3 mousePosOnDown, restingPosition;
	private SpriteRenderer spriteRenderer;
	public float maxChargeYDelta, chargeOnRelease;
	public PachinkoBallController ball;
	public Color restingColor, intermediateColor, fullyChargedColor;
	private bool hasBallToShoot;
	
	private void Awake()
	{
		restingPosition = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer>();
		hasBallToShoot = true;
	}

	private void OnEnable()
	{
		PachinkoEvents.OnResetBallRequested += OnResetBallRequested;
	}

	private void OnDisable()
	{
		PachinkoEvents.OnResetBallRequested -= OnResetBallRequested;

	}

	private void OnResetBallRequested()
	{
		hasBallToShoot = true;
	}

	private void OnMouseDown()
	{
		mousePosOnDown = Input.mousePosition;
	}

	private void OnMouseDrag()
	{
		if (!hasBallToShoot) return;
		Vector3 mousePos = Input.mousePosition;
		AdjustPositionAndColor(mousePos);		
	}
	
	private void OnMouseUp()
	{
		if (!hasBallToShoot) return;
		Release();
	}

	private void AdjustPositionAndColor(Vector3 mousePos)
	{
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		if (mousePos.y >= restingPosition.y)
		{
			transform.position = restingPosition;
			spriteRenderer.color = restingColor;
		}
		else if(mousePos.y > restingPosition.y - maxChargeYDelta)
		{
			Vector3 position = transform.position;
			position.y = mousePos.y;
			spriteRenderer.color=  Color.Lerp(restingColor, intermediateColor, PercentCharge);
			transform.position = position;
		}
		else
		{
			Vector3 position = transform.position;
			position.y = restingPosition.y - maxChargeYDelta;
			transform.position = position;
			spriteRenderer.color = fullyChargedColor;
		}
	}

	public void Release()
	{
		hasBallToShoot = false;
		chargeOnRelease = PercentCharge;
		transform.DOMoveY(restingPosition.y, 0.25f).OnComplete(() =>
		{
			ball.Push(chargeOnRelease);
			spriteRenderer.color = restingColor;
		});
	}

	private float PercentCharge
	{
		get
		{
			float currentDelta = restingPosition.y - transform.position.y;
			return currentDelta/maxChargeYDelta;
		}
	}

}
