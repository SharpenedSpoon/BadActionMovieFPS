using UnityEngine;
using System.Collections;

public class PlayerShootingInput : MonoBehaviour {

	private CanShoot shooter = null;

	private WeaponInventory inventory = null;

	void Start () {
		inventory = GetComponent<WeaponInventory>();

		shooter = GetComponent<CanShoot>();
	}

	void Update () {
		bool fire = false;
		if (Input.GetButtonDown("Fire1")) {
			fire = true;
		}
		if (inventory.weapons[inventory.currentWeaponNumber].autofire && Input.GetButton("Fire1")) {
			fire = true;
		}

		if (fire) {
			shooter.Shoot(Quaternion.LookRotation(Camera.main.transform.forward));
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			inventory.PreviousWeapon();
		} else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			inventory.NextWeapon();
		}
	}
}
