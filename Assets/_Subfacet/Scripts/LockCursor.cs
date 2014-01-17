using UnityEngine;
using System.Collections;

public class LockCursor : MonoBehaviour {

	public bool useCursorLocking = true;

	void Start () {
		if (useCursorLocking) {
			Screen.lockCursor = true;
		}
	}

	void Update () {

		if (! useCursorLocking) {
			return;
		}

		if (Screen.lockCursor) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Screen.lockCursor = false;
			}
		} else {
			if (Input.GetMouseButtonDown(0)) {
				Screen.lockCursor = true;
			}
		}
	}
}
