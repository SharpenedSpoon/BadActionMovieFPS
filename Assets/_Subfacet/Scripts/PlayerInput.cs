using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private CanShoot shooter = null;

	private WeaponInventory inventory = null;

	void Start () {
		inventory = GetComponent<WeaponInventory>();

		shooter = GetComponent<CanShoot>();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			shooter.Shoot(Quaternion.LookRotation(Camera.main.transform.forward));
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			inventory.PreviousWeapon();
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			inventory.NextWeapon();
		}
	}
}
