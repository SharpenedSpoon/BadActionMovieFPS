using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

public class WeaponsController : MonoBehaviour {

	public List<oldWeaponData> weapons = new List<oldWeaponData>();
	public List<AudioClip> weaponSounds = new List<AudioClip>();

	public new static WeaponsController active;

	void Awake() {
		active = this;
	}

	void Start() {
		/*string weaponsString = FileIO.ReadFromFile("weapons.json");
		if (weaponsString != null && weaponsString != "" && weaponsString != "{}") {
			weapons = JsonConvert.DeserializeObject<List<WeaponData>>(weaponsString);
		}*/
	}

	public GameObject createBulletPrefab(BulletData bulletData) {
		// create the mesh or an empty gameobject
		GameObject go;
		if (bulletData.mesh == "") {
			go = new GameObject();
		} else {
			go = Instantiate(Resources.Load(bulletData.mesh)) as GameObject;
			if (! go) {
				go = new GameObject();
			}
		}
		Bullet bullet = go.AddComponent<Bullet>();

		// name the gameobject!
		go = "bullet_" + bulletData.bulletName;

		// transfer all the variables and data
		bullet.isHitScan = bulletData.isHitScan;
		bullet.isLinear = bulletData.isLinear;
		bullet.destroyOnHit = bulletData.destroyOnHit;
		bullet.freezeRotation = bulletData.freezeRotation;
		bullet.initialSpeed = bulletData.initialSpeed;
		bullet.range = bulletData.range;
		bullet.damage = bulletData.damage;
		bullet.lifetime = bulletData.lifetime;
		foreach (string objectName in bulletData.objectsToSpawnOnHit) {
			GameObject tempObject = Resources.Load(objectName) as GameObject;
			if (tempObject != null) {
				bullet.objectsToSpawnOnHit.Add(tempObject);
			}
		}
		foreach (string objectName in bulletData.objectsToSpawnOnDestroy) {
			GameObject tempObject = Resources.Load(objectName) as GameObject;
			if (tempObject != null) {
				bullet.objectsToSpawnOnDestroy.Add(tempObject);
            }
        }

		// create a prefab, and destroy the instantiated instance
		GameObject prefab = PrefabUtility.CreatePrefab("Assets/_Generated/Resources/"+go.gameObject.name+".prefab", go);
		Destroy(go);
		return prefab;
	}

	public GameObject createWeaponPrefab(WeaponData weaponData) {
		// create the gameobject container
		GameObject go = new GameObject();
		Weapon weapon = go.AddComponent<Weapon>();

		// name the gameobject!
		go.name = "weapon_" + weaponData.weaponName;

		// try and link the bullet
		GameObject foundBullet = Resources.Load(weaponData.bullet);
		if (foundBullet == null) {
			Debug.LogError("Could not find bullet to go along with weapon! (In weapon " + weaponData.weaponName + ")");
			return null;
		}
		weapon.bullet = foundBullet;

		// transfer all variables and data
		weapon.autofire = weaponData.autofire;
		weapon.ammoCount = weaponData.ammoCount;
		weapon.magazineSize = weaponData.magazineSize;
		weapon.bulletsPerShot = weaponData.bulletsPerShot;
		weapon.rateOfFire = weaponData.rateOfFire;
		weapon.reloadTime = weaponData.reloadTime;
		weapon.idleHoldObjectPosition = weaponData.idleHoldObjectPosition;
		weapon.muzzleFlash = Resources.Load(weaponData.muzzleFlash);
		weapon.idleHoldObject = Resources.Load(weaponData.idleHoldObject);
		weapon.gunColor = weaponData.gunColor;
		if (weaponSounds.Count >= 2) {
			switch (weaponData.sound) {
				case "":
					// do nothing.
					break;
				case "laser":
					weapon.sound = weaponSounds[0];
					break;
				case "grenade":
					weapon.sound = weaponSounds[1];
					break;
			}
		}

		// create a prefab, and destroy the instantiated instance
		GameObject prefab = PrefabUtility.CreatePrefab("Assets/_Generated/Resources/"+go.gameObject.name+".prefab", go);
		Destroy(go);
		return prefab;
	}

	/*public GameObject createBulletObject(WeaponData weap) {
		GameObject go = null;
		if (weap.bulletObjectNameString != "") {
			go = Instantiate(Resources.Load(weap.bulletObjectNameString)) as GameObject;
		}
		if (go == null) {
			go = new GameObject();
		}
		go.name = "weapon" + weap.weaponName;
		IsBullet bullet = go.AddComponent<IsBullet>();
		bullet.damage = weap.damage;
		bullet.raycastBullet = weap.isHitScan;
		bullet.isExplosive = weap.isExplosive;
		bullet.destroyOnHit = weap.destroyOnHit;
		bullet.linearSpeed = weap.usesLinearSpeed;
		bullet.limitedRange = weap.hasLimitedRange;
		bullet.freezeRotation = weap.freezeRotation;
		bullet.speed = weap.speed;
		bullet.force = weap.force;
		bullet.range = weap.range;
		bullet.lifetimeInSeconds = weap.lifetimeInSeconds;
		bullet.explosionRadius = weap.explosionRadius;
		if (weap.particleSystemObjectNameString != "") {
			//bullet.particleSystem = Instantiate(Resources.Load(weap.particleSystemObjectNameString)) as GameObject;
			bullet.particleSystem = Resources.Load(weap.particleSystemObjectNameString) as GameObject;
		}
		GameObject prefab = PrefabUtility.CreatePrefab("Assets/Temporary/"+go.gameObject.name+".prefab", go);

		Destroy (go);
		return prefab;
	}

	public isWeapon CreateWeaponWithBullet(WeaponData weap) {
		isWeapon thisWeap = new isWeapon();
		thisWeap.name = weap.weaponName;
		thisWeap.autofire = weap.autofire;
		thisWeap.shotsPerSecond = weap.shotsPerSecond;
		thisWeap.bulletObject = WeaponsController.active.createBulletObject(weap);
		switch (weap.soundString) {
		case "":
			// do nothing.
			break;
		case "laser":
			thisWeap.sound = weaponSounds[0];
			break;
		case "grenade":
			thisWeap.sound = weaponSounds[1];
			break;
		}
		return thisWeap;
    }*/
    
}
