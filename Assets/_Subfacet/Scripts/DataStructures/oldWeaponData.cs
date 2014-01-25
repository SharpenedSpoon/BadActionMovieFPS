using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class oldWeaponData {

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
	public string soundString = ""; // empty string, "laser", or "grenade"

	public List<string> blah = new List<string>{"1", "hello", "whateve sdfadsf"};
	public List<string> yada = new List<string>();
}
