using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CanShoot))]
public class WeaponInventory : MonoBehaviour {

	public List<Weapon> weapons = new List<Weapon>();

	public int startingWeaponNumber = 0;

	public int currentWeaponNumber { get; private set; }
	private CanShoot shooter = null;

	public GameObject gun = null;
	public GunEffects gunEffects = null;

	void Start() {
		shooter = GetComponent<CanShoot>();
		ChangeWeapon(startingWeaponNumber);
		//LoadWeaponsFromFile();
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

		if (gunEffects != null) {
			gunEffects.ChangeWeapon(weapons[currentWeaponNumber]);
		}
	}

	private void LoadWeaponsFromFile() {
		weapons = new List<Weapon>();
		foreach (WeaponData weap in WeaponsController.active.weapons) {
			weapons.Add(WeaponsController.active.CreateWeaponWithBullet(weap));
		}
		ChangeWeapon(0);
	}
}
