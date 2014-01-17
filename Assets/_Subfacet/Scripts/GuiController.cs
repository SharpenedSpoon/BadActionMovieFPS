using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {

	public GUIText guiPlayerHealth;
	public GUIText guiEnemyInfo;
	public GUIText guiPlayerWeapon;

	void Start () {
	
	}

	void Update () {
		guiEnemyInfo.text = "Enemies: " + EnemySpawner.active.enemyCount + "\n" + "Wave: " + EnemySpawner.active.wave + "\n(press N to spawn)";

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player) {
			WeaponInventory inventory = player.GetComponent<WeaponInventory>();
			CanShoot shooter = player.GetComponent<CanShoot>();
			SetWeaponText(inventory, shooter);

			HasHealth playerHealth = player.GetComponent<HasHealth>();
			guiPlayerHealth.text = "HP: " + playerHealth.health;
		} else {
			guiPlayerWeapon.text = "Dead!";
			guiPlayerHealth.text = "";
		}
	}

	private void SetWeaponText(WeaponInventory inventory, CanShoot shooter) {
		string txt = "";
		txt += inventory.weapons[inventory.currentWeaponNumber].name;
		
		if (! shooter.canShoot) {
			txt += "\n";
			txt += "Reloading: ";
			
			float reloadTime = 1.0f / inventory.weapons[inventory.currentWeaponNumber].shotsPerSecond;
			float lastShotTime = shooter.timeTillNextShot - reloadTime;
			float elapsedTime = Time.time - lastShotTime;
			float elapsedPercentTime = elapsedTime / reloadTime;
			
			int totalDots = 6;
			txt += "[";
			for (int i=1; i<=totalDots; i++) {
				if (elapsedPercentTime <= (1.0f * i) / (totalDots + 1)) {
					txt += " ";
				} else {
					txt += "*";
				}
			}
			txt += "]";
		}
		
		guiPlayerWeapon.text = txt;
	}
}
