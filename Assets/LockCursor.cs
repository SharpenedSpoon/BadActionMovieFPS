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
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Screen.lockCursor = false;
		}
	}

	void OnMouseDown() {
		if (lockCursor) {
			Screen.lockCursor = true;
		}
	}
}
