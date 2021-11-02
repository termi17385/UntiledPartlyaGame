using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
	[SerializeField] private Transform startPos, endPos;
	[SerializeField] private bool playPressed = false;
	[SerializeField] private float animationSpeed;

	public void AnimateWindows() => playPressed = !playPressed;
	private void Start() => playPressed = false;

	private void Update()
	{
		var movePos = !playPressed ? startPos.position : endPos.position;
		transform.position = Vector3.Lerp(transform.position, movePos, animationSpeed * Time.deltaTime);
	}
}
