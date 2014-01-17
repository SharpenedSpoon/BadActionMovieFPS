using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private CanShoot shooter = null;

	private WeaponInventory inventory = null;

	private float test = 0;
	public float upscale = 0.5f;
	public float forwardscale = 1f;

	void Start () {
		inventory = GetComponent<WeaponInventory>();

		shooter = GetComponent<CanShoot>();
		if (shooter == null) {
			shooter = GetComponentInChildren<CanShoot>();
		}
		if (shooter == null) {
			Debug.LogError("Player needs to be able to shoot! Could not find a 'CanShoot' component, here or in its children.");
			enabled = false;
		}
	
	}

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			//shooter.Shoot(transform.position + 0.5f*transform.up, new Quaternion(Camera.main.transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
			//shooter.Shoot(Camera.main.transform.position - 0.1f*Camera.main.transform.up + 1.0f*Camera.main.transform.forward, Quaternion.LookRotation(Camera.main.transform.forward));
			shooter.Shoot(Quaternion.LookRotation(Camera.main.transform.forward));
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			inventory.PreviousWeapon();
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			inventory.NextWeapon();
		}
	}
}
