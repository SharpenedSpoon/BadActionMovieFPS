using UnityEngine;
using System.Collections;
using System;

public class AIWalkerWithCoroutines : MonoBehaviour {

	public float speed = 5;
	public float rotateSpeed = 5;

	public float closeEnoughDistance = 2;

	private float part = 0.0f;
	private Vector3 prevPos;
	private GameObject player;

	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		prevPos = player.transform.position;
		//LerpMeOverTime();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (! isMoving) {
				MoveTowards(player);
				isMoving = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			isMoving = false;
		}
	}

	public void MoveTowards(GameObject go) {
		MoveTowards(go.transform.position);
	}

	public void MoveTowards(Vector3 pos) {
		Vector3 startPosition = transform.position;
		Vector3 endPosition = pos;

		if (Vector3.Distance(startPosition, endPosition) <= closeEnoughDistance) {
			// no need to move... we're close enough!
			return;
		}

		// look at me.... LOOK AT ME
		transform.LookAt(pos);

		float timeInSeconds = Vector3.Distance(startPosition, endPosition) / speed; // how long it should take to move based on speed

		Action<float> transformAction = delegate(float f) {
			transform.position = Vector3.Lerp(startPosition, endPosition, f);
		};

		Func<bool> checkIfMoving = delegate {
			return isMoving;
		};

		StartCoroutine(SubfacetFunctions.TransformOverTime(transformAction, timeInSeconds, checkIfMoving));
	}

	public void RotateTowards(GameObject go) {
		RotateTowards(go.transform.position);
	}

	public void RotateTowards(Vector3 pos) {
		Quaternion startRotation = transform.rotation;
		Vector3 lookPos = pos - transform.position;
		lookPos.y = transform.position.y;
		Quaternion endRotation = Quaternion.LookRotation(lookPos, transform.up);
		float timeInSeconds = 1.0f;

		Action<float> transformAction = delegate(float f) {
			transform.rotation = Quaternion.Lerp(startRotation, endRotation, f);
		};

		StartCoroutine(SubfacetFunctions.TransformOverTime(transformAction, timeInSeconds));
		//transform.rotation = Quaternion.Lerp(startRotation, endRotation, part);
	}

	protected void LerpMeOverTime() {
		Vector3 startPosition = gameObject.transform.position;
		Vector3 endPosition = startPosition + 10*Vector3.forward;
		float timeInSeconds = 5.0f;
		
		Action<float> transformAction = delegate(float f) {
			gameObject.transform.position = Vector3.Lerp(startPosition,endPosition,f);
		};
		
		StartCoroutine(SubfacetFunctions.TransformOverTime(transformAction,timeInSeconds));
	}
}
