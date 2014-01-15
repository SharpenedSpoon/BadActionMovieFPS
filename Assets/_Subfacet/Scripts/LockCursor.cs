using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

	public bool lockCursor = true;

	void Start () {
		if (lockCursor) {
			Screen.lockCursor = true;
		}
	}

	void Update () {

		if (! lockCursor) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Screen.lockCursor = false;
		}

		if (Input.GetMouseButtonDown(0)) {
			Screen.lockCursor = true;
		}
	}
}
