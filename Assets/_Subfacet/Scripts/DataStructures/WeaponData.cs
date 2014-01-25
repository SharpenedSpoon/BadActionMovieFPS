using UnityEngine;

[System.Serializable]
public class WeaponData {
	public string weaponName = "UntitledWeapon";
	public string bullet = "";
	public bool autofire = false;
	public int ammoCount = 1000;
	public int magazineSize = 10;
	public int bulletsPerShot = 1;
	public float rateOfFire = 30;
	public float reloadTime = 0.5f;
	public Vector3 idleHoldObjectPosition = Vector3.zero;
	public string muzzleFlash = "";
	public string idleHoldObject = "";
	public string sound = "";
	public Color gunColor = Color.white;
}