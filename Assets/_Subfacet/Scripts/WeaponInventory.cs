using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CanShoot))]
public class WeaponInventory : MonoBehaviour {

	public List<Weapon> weapons = new List<Weapon>();

	public int startingWeaponNumber = 0;

	public GUIText weaponGuiText = null;

	private int currentWeaponNumber;
	private CanShoot shooter = null;

	void Start () {
		shooter = GetComponent<CanShoot>();
		ChangeWeapon(startingWeaponNumber);
	}

	void Update() {
		SetWeaponText();
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
		
		SetWeaponText();
	}

	private void SetWeaponText() {
		if (weaponGuiText == null) {
			return;
		}

		string txt = "";
		txt += weapons[currentWeaponNumber].name;

		if (! shooter.canShoot) {
			txt += "\n";
			txt += "Reloading: ";

			float reloadTime = 1.0f / weapons[currentWeaponNumber].shotsPerSecond;
			float lastShotTime = shooter.timeTillNextShot - reloadTime;
			float elapsedTime = Time.time - lastShotTime;
			float elapsedPercentTime = elapsedTime / reloadTime;

			int totalDots = 6;
			txt += "[";
			for (int i=1; i<=totalDots; i++) {
				if (elapsedPercentTime <= (1.0f * i) / (totalDots + 1)) {
					txt += "*";
				} else {
					txt += " ";
				}
			}
			txt += "]";
		}

		weaponGuiText.text = txt;
	}
}
