using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CanShoot))]
public class WeaponInventory : MonoBehaviour {

	public List<GameObject> weapons = new List<GameObject>();

	public int startingWeaponNumber = 0;

	private int currentWeaponNumber;
	private CanShoot shooter = null;

	void Start () {
		shooter = GetComponent<CanShoot>();
		ChangeWeapon(startingWeaponNumber);
	}

	public void NextWeapon() {
		ChangeWeapon(currentWeaponNumber + 1);
	}

	public void PreviousWeapon() {
		ChangeWeapon(currentWeaponNumber - 1);
	}

	public void ChangeWeapon(int num) {
		currentWeaponNumber = num;
		if (currentWeaponNumber >= weapons.Count) {
			currentWeaponNumber = 0;
		} else if (currentWeaponNumber < 0) {
			currentWeaponNumber = weapons.Count - 1;
		}
		shooter.bullet = weapons[currentWeaponNumber];
	}
}
