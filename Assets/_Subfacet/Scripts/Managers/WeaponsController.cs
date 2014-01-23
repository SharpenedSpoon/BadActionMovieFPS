using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

public class WeaponsController : MonoBehaviour {

	public List<WeaponData> weapons = new List<WeaponData>();
	public List<AudioClip> weaponSounds = new List<AudioClip>();

	public new static WeaponsController active;

	void Awake() {
		active = this;
	}

	void Start() {
		string weaponsString = FileIO.ReadFromFile("weapons.json");
		if (weaponsString != null && weaponsString != "" && weaponsString != "{}") {
			weapons = JsonConvert.DeserializeObject<List<WeaponData>>(weaponsString);
		}
	}

	public GameObject createBulletObject(WeaponData weap) {
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

	public Weapon CreateWeaponWithBullet(WeaponData weap) {
		Weapon thisWeap = new Weapon();
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
    }
    
}
