using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	// TODO convert isLinear to an enum
	
	public bool isHitScan = false;
	public bool isLinear = true; // otherwise it is a projectile. always true if isHitScan is true.
	public bool destroyOnHit = true; // always true if isHitScan is true.
	public bool freezeRotation = true;
	public float initialSpeed = 10.0f;
	public float range = 0.0f; // a range of 0 means don't worry about range
	public float damage = 1.0f;
	public float lifetime = 20.0f;
	public List<GameObject> objectsToSpawnOnHit = new List<GameObject>();
	public List<GameObject> objectsToSpawnOnDestroy = new List<GameObject>();

	public string ownerTag = "";

	private float deathTime;
	private Vector3 startPosition;
	
	void Start () {
		deathTime = Time.time + lifetime;
		startPosition = transform.position;

		// Collect bullets under one manager for organizational reasons
		if (BulletManager.active) {
			transform.parent = BulletManager.active.transform;
		}

		if (isHitScan) {
			isLinear = true;
			destroyOnHit = true;
		}
		if (! isHitScan && ! rigidbody) {
			gameObject.AddComponent<Rigidbody>();
			Debug.LogWarning("You should have a rigidbody attached to this bullet. Adding one for you.");
		}
		if (rigidbody) {
			rigidbody.freezeRotation = freezeRotation;
			if (isLinear) {
				rigidbody.useGravity = false;
				rigidbody.drag = 0;
			}
			PropelBulletForward(initialSpeed);
		}
	}

	void FixedUpdate() {
		// Do this in FixedUpdate instead of Start so we have time to set the owner
		if (isHitScan) {
			// call HitPoint with a raycast
			RaycastHit hit;
			bool hitResult;
			if (range <= 0.0f) {
				hitResult = Physics.Raycast(transform.position, transform.forward, out hit);
			} else {
				hitResult = Physics.Raycast(transform.position, transform.forward, out hit, range);
			}
			if (hitResult) {
				GameObject hitObject = null;
				if (hit.rigidbody) {
					hitObject = hit.rigidbody.gameObject;
				}
				HitPoint(hit.point, hitObject);
			} else {
				// we missed!
				DestroySelf();
			}
		}

		// check if we have exceeded our lifetime
		if (Time.time > deathTime) {
			DestroySelf();
		}

		// check if we have exceeded our range, if applicable
		if (range != 0.0f) {
			if (Vector3.Distance(startPosition, transform.position) > range) {
				DestroySelf(false); // do not spawn the death objects
			}
		}
	}

	void OnCollisionEnter(Collision col) {
		if (! col.gameObject.CompareTag(ownerTag)) {
			HitPoint(col.gameObject, transform.position);
		}
	}

	public void PropelBulletForward(float speed) {
		if (rigidbody) {
			rigidbody.AddForce(speed * transform.forward, ForceMode.VelocityChange);
		}
	}

	private void HitPoint(Vector3 hitPoint) {
		HitPoint(hitPoint, null);
	}

	private void HitPoint(Vector3 hitPoint, GameObject hitObject) {
		// Possibly damage our target
		if (hitObject != null) {
			hitObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

		if (objectsToSpawnOnHit.Count != 0) {
			foreach (GameObject go in objectsToSpawnOnHit) {
				Instantiate(go, transform.position, transform.rotation);
			}
		}

		if (destroyOnHit) {
			DestroySelf();
		}
	}

	private void DestroySelf() {
		DestroySelf(true);
	}

	private void DestroySelf(bool spawnDeathObjects) {
		if (spawnDeathObjects && objectsToSpawnOnDestroy.Count != 0) {
			foreach (GameObject go in objectsToSpawnOnDestroy) {
				Instantiate(go, transform.position, transform.rotation);
			}
		}
		Destroy(gameObject);
	}


}
