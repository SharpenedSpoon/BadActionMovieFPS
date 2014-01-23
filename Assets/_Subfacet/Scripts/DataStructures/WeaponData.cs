using UnityEngine;

[System.Serializable]
public class WeaponData {

	public string weaponName = "weapon template";
	public float damage = 1;
	public float shotsPerSecond = 10;
	public bool isHitScan = false;
	public bool isExplosive = false;
	public bool destroyOnHit = true;
	public bool usesLinearSpeed = true;
	public bool hasLimitedRange = false;
	public bool freezeRotation = true;
	public float speed = 20;
	public float force = 1000;
	public float range = 100;
	public float lifetimeInSeconds = 10;
	public float explosionRadius = 8;
	public string particleSystemObjectNameString = "";
	public string bulletObjectNameString = "";
	public bool autofire = false;
	public Color color = Color.red;
	public string sound = ""; // empty string, "laser", or "grenade"
}
