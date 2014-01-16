using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private CanShoot shooter = null;

	void Start () {
		shooter = GetComponent<CanShoot>();
		if (shooter == null) {
			shooter = GetComponentInChildren<CanShoot>();
		}
		if (shooter == null) {
			Debug.LogError("Player needs to be able to shoot! Could not find a 'CanShoot' component, here or in its children.");
		}
	
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			shooter.Shoot(transform.position + 0.5f*transform.up, new Quaternion(Camera.main.transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
		}
	}
}
