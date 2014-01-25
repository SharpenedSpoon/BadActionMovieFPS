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
		LoadWeaponsFromFile();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.L)) {
			LoadWeaponsFromFile();
		}
	}

	public void NextWeapon() {
		ChangeWeapon(currentWeaponNumber + 1);
	}

	public void PreviousWeapon() {
		ChangeWeapon(currentWeaponNumber - 1);
	}

	public void ChangeWeapon(int num) {
		currentWeaponNumber = Mathf.Clamp(num, 0, weapons.Count - 1);

		shooter.SetWeapon(weapons[currentWeaponNumber]);

		if (gunEffects != null) {
			gunEffects.ChangeWeapon(weapons[currentWeaponNumber]);
		}
	}

	private void LoadWeaponsFromFile() {
		if (WeaponsController.active.weapons.Count == 0) {
			Debug.Log("WeaponsController has no weapons in its list!");
			return;
		}
		weapons = WeaponsController.active.weapons;
		ChangeWeapon(0);
	}
}
