using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public GameObject bullet = null;
	public bool autofire = false;
	public int ammoCount = 1000;
	public int magazineSize = 10;
	public int bulletsPerShot = 1;
	public float rateOfFire = 30; // shots per second
	public float reloadTime = 0.5f; // time to reload magazines. use 0 for no reload time
	public Color gunColor = Color.white;
	public Vector3 idleHoldObjectPosition = Vector3.zero;
	public GameObject muzzleFlash = null;
	public GameObject idleHoldObject = null;
	public AudioClip sound = null;

	public float reloadTimeNeeded { get; private set; }
	public float timeBetweenShots { get; private set; }
	public float nextShotTimeNeeded { get; private set; }
	public int ammoInMagazine { get; private set; }



	void Start () {
		reloadTimeNeeded = 0.0f;
		nextShotTimeNeeded = 0.0f;
		ammoInMagazine = magazineSize;

		if (rateOfFire == 0.0f) {
			timeBetweenShots = 0.0f;
		} else {
			timeBetweenShots = 1.0f / rateOfFire;
		}
	}

	void Update () {
		if (reloadTimeNeeded > 0.0f) {
			reloadTimeNeeded -= Time.deltaTime;
		}

		if (timeBetweenShots > 0.0f) {
			nextShotTimeNeeded -= Time.deltaTime;
		}
	}

	public void Shoot(Vector3 pos, Quaternion rot) {
		if (bullet == null) {
			return;
		}

		for (int i=0; i<bulletsPerShot; i++) {
			GameObject spawnedBullet = Instantiate(bullet, pos, rot) as GameObject;
			Bullet bulletComponent = spawnedBullet.GetComponent<Bullet>();
			if (bulletComponent) {
				bulletComponent.ownerTag = gameObject.tag;
			}
			ExpendAmmo(1);
		}

		nextShotTimeNeeded = timeBetweenShots;
		
	}

	private void ExpendAmmo(int ammoToExpend) {
		ammoCount = Mathf.Max(0, ammoCount - ammoToExpend);
		ammoInMagazine = Mathf.Max(0, ammoInMagazine - ammoToExpend);

		if (ammoInMagazine <= 0) {
			reloadTimeNeeded = reloadTime;
		}
	}
}
