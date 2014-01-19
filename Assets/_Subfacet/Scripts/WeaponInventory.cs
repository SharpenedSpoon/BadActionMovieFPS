using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CanShoot))]
public class WeaponInventory : MonoBehaviour {

	public List<Weapon> weapons = new List<Weapon>();

	public int startingWeaponNumber = 0;

	public int currentWeaponNumber { get; private set; }
	private CanShoot shooter = null;

	void Start() {
		shooter = GetComponent<CanShoot>();
		ChangeWeapon(startingWeaponNumber);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.L)) {
			weapons = new List<Weapon>();
			foreach (WeaponData weap in WeaponsController.active.weapons) {
				Weapon thisWeap = new Weapon();
				thisWeap.name = weap.weaponName;
				thisWeap.bulletObject = WeaponsController.active.createBulletObject(weap);
				weapons.Add(thisWeap);
			}
			ChangeWeapon(0);
			//foreach (WeaponsController)
		}
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

		shooter.SetWeapon(weapons[currentWeaponNumber]);
	}
}
