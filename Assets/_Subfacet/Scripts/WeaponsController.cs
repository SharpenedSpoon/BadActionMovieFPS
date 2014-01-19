using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;

public class WeaponsController : MonoBehaviour {

	public List<WeaponData> weapons = new List<WeaponData>();

	public GameObject test;

	public new static WeaponsController active;

	void Awake() {
		active = this;
	}

	void Start() {
		string weaponsString = FileIO.ReadFromFile("weapons.txt");
		if (weaponsString != null && weaponsString != "" && weaponsString != "{}") {
			weapons = JsonConvert.DeserializeObject<List<WeaponData>>(weaponsString);
		} else {
			/*WeaponData weap = new WeaponData();
			weapons.Add(weap);
			string txt = JsonConvert.SerializeObject(weapons, Formatting.Indented);
			FileIO.SaveToFile("weapons.txt", txt);*/
		}
	}

	public GameObject createBulletObject(WeaponData weap) {
		GameObject go = null;
		if (weap.bulletObjectNameString != "") {
			go = Resources.Load(weap.bulletObjectNameString) as GameObject;
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
			bullet.particleSystem = Resources.Load(weap.particleSystemObjectNameString) as ParticleSystem;
		}
		Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/"+go.gameObject.name+".prefab");
		PrefabUtility.ReplacePrefab(go.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

		return go;
	}

}
