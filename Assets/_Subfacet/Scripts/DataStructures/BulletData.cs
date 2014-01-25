using System.Collections.Generic;

[System.Serializable]
public class BulletData {
	public string bulletName = "UntitledBullet";
	public string mesh = "";
	public bool isHitScan = false;
	public bool isLinear = true;
	public bool destroyOnHit = true;
	public bool freezeRotation = true;
	public float initialSpeed = 10;
	public float range = 0;
	public float damage = 1;
	public float lifetime = 20;
	public List<string> objectsToSpawnOnHit = new List<string>();
	public List<string> objectsToSpawnOnDestroy = new List<string>();
}