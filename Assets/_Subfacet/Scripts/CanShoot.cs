using UnityEngine;
using System.Collections;

public class CanShoot : MonoBehaviour {

	public Weapon weapon = null;
	private GameObject bullet = null;

	public float shootOffsetForward = 1.0f;
	public float shootOffsetRight = 0.0f;
	public float shootOffsetUp = 0.5f;
	private Vector3 shootOffset;
	
	public bool shootFromMainCamera = false;
	public GameObject objectToShootFrom = null;
	private Transform tr;

	public bool useRateOfFire = true;
	public bool canShoot { get; private set; }
	public float timeTillNextShot { get; private set; }

	void Start() {
		timeTillNextShot = 0;
		canShoot = true;

		SetWeapon(weapon);

		if (objectToShootFrom == null) {
			if (shootFromMainCamera) {
				objectToShootFrom = Camera.main.gameObject;
			} else {
				objectToShootFrom = gameObject;
			}
		}

		tr = objectToShootFrom.transform;

		SetShootOffset();
	}

	void Update() {
		if (! canShoot) {
			if (Time.time > timeTillNextShot) {
				canShoot = true;
			}
		}
	}

	public void SetWeapon(Weapon weap) {
		weapon = weap;
		bullet = weapon.bulletObject;
		canShoot = true;
		timeTillNextShot = Time.time;
	}

	private void SetShootOffset() {
		shootOffset = (shootOffsetForward * tr.forward) + (shootOffsetRight * tr.right) + (shootOffsetUp * tr.up);
	}

	public void Shoot() {
		Shoot(tr.position, tr.rotation);
	}

	public void Shoot(Vector3 startPosition) {
		Shoot(startPosition, tr.rotation);
	}

	public void Shoot(Quaternion directionToShootAt) {
		Shoot(tr.position, directionToShootAt);
	}

	public void Shoot(GameObject targetGameObject) {
		Shoot(tr.position, Quaternion.LookRotation(targetGameObject.transform.position - tr.position));
	}

	public void Shoot(Vector3 startPosition, Quaternion directionToShootAt) {
		if (canShoot || ! useRateOfFire) {
			SetShootOffset();
			GameObject newBullet = Instantiate(bullet, startPosition + shootOffset, directionToShootAt) as GameObject;
			newBullet.SendMessage("SetOwnerTag", tag, SendMessageOptions.DontRequireReceiver);

			if (useRateOfFire) {
				canShoot = false;
				timeTillNextShot = Time.time + (1.0f / weapon.shotsPerSecond);
			}
		}
	}
}
