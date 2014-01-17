﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CharacterController))]
public class AIWalker : MonoBehaviour {

	public float moveSpeed = 5;
	public float rotateSpeed = 5;
	public float closeEnoughDistance = 2;

	public float gravity = 10.0f;

	private GameObject player;
	private Transform playerTransform;

	private CharacterController characterController;

	private float currentMoveSpeed = 0.0f;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		playerTransform = player.transform;

		characterController = GetComponent<CharacterController>();

		if (GetComponent<AIShooter>() != null) {
			// if this is a shooting-based enemy, then only ever move close enough to shoot (and change)
			closeEnoughDistance = 0.8f * GetComponent<AIShooter>().range;
		}
	}

	void Update () {
		if (Vector3.Distance(transform.position, playerTransform.position) < closeEnoughDistance) {
			currentMoveSpeed = 0;
		} else {
			currentMoveSpeed = moveSpeed;
		}
		MoveTowards(playerTransform.position);
	}

	public void MoveTowards(GameObject go) {
		MoveTowards(go.transform.position);
	}

	public void MoveTowards(Vector3 pos) {
		TurnTowards(pos);

		// even if currentMoveSpeed is 0, this will still apply gravity.
		characterController.SimpleMove(currentMoveSpeed * (pos - transform.position).normalized);

		/* old, super-janky code:
		TurnTowards(pos);

		if (Vector3.Distance(transform.position, pos) > closeEnoughDistance) {
			rigidbody.AddForce(moveSpeed * (pos - transform.position).normalized);
		}*/
	}

	public void TurnTowards(Vector3 pos) {
		transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));

		//SlowlyTurnTowards(pos);
	}

	public void SlowlyTurnTowards(Vector3 pos) {
		/*
		 * For Funsies:
		 * 
		 * X> ---\
		 * |      ---\
		 * |      ____P
		 * | ____/
		 * |/
		 * T
		 * 
		 * X is current character, facing in direction of error.
		 * T is the target point we want to face.
		 * P is a point projected forward a distance of Dist(X, T).
		 * 
		 * We look at a point that is an appropriately Lerp-ed distance between P and T, based on rotateSpeed.
		 */

		Vector3 pointX = transform.position;
		Vector3 pointT = pos;
		float distXtoT = Vector3.Distance(pointX, pointT);
		Vector3 pointP = transform.position + (distXtoT * transform.forward);
		float distPtoT = Vector3.Distance(pointP, pointT);

		float angleBetweenPoints = Vector3.Angle(pointP - pointX, pointT - pointX);
		float rotateTotalTime = angleBetweenPoints / rotateSpeed;

		float amountToLerp = Time.deltaTime / rotateTotalTime;
		Vector3 lerpedPoint = Vector3.Lerp(pointP, pointT, amountToLerp);

		transform.LookAt(lerpedPoint);
	}

}
