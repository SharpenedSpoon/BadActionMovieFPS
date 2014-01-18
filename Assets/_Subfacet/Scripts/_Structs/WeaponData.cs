using UnityEngine;

public class WeaponData {

	public string weaponName = "weapon template";
	public bool isHitScan = false;
	public bool linearSpeed = true;
	public bool destroyOnHit = true;
	public bool freezeRotation = true;
	public float damage = 1;
	public float speed = 20;
	public float force = 1000;
	public bool limitedRange = false;
	public float range = 100;
	public float lifetimeInSeconds = 10;
	public ParticleSystem particleSystem = null;
	public bool isExplosive = false;
	public float explosionRadius = 8;

}
