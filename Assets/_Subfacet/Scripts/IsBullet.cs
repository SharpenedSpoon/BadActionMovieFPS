using UnityEngine;
using System.Collections;

// TODO: add explosions. KABOOOOOOMMMMM

public class IsBullet : MonoBehaviour {

	public bool raycastBullet = false;
	public bool linearSpeed = true;
	public bool destroyOnHit = true;

	public bool freezeRotation = true;

	public int damage = 1;

	public float speed = 10;
	public float force = 1000;

	public bool limitedRange = false;
	public float range = 100;

	public float lifetimeInSeconds = 10;
	private float deathTime;

	public ParticleSystem particleSystem = null;

	private string ownerTag = "";

	private Vector3 startingPosition;

	void Start () {
		// if this bullet is a raycast bullet we can skip a bunch of stuff.
		if (!raycastBullet) {
			// make sure we have a rigidbody
			if (! rigidbody) {
				gameObject.AddComponent<Rigidbody>();
				Debug.LogWarning("You should have a rigidbody attached to this bullet.");
			}
			rigidbody.freezeRotation = freezeRotation;
			startingPosition = transform.position;
			if (linearSpeed) {
				rigidbody.useGravity = false;
			} else {
				rigidbody.AddForce(force * transform.forward);
			}
		}

		deathTime = Time.time + lifetimeInSeconds;

		// Collect all bullets under the BulletManager for organizational purposes
		transform.parent = BulletManager.active.transform;
	}

	void FixedUpdate() {
		if (raycastBullet) {
			// Do this in FixedUpdate instead of Start so we have time to set the owner
			// call HitPoint with a raycast
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit)) {
				GameObject hitObject = null;
				if (hit.rigidbody) {
					hitObject = hit.rigidbody.gameObject;
				}
				HitPoint(hitObject, hit.point, Quaternion.LookRotation(hit.normal));
			} else {
				// we missed!
				Destroy(gameObject);
			}
			// TODO: call HitPoint with a limited distance raycast if applicable
			return;
		}
		
		if (linearSpeed) {
			transform.position += speed * Time.fixedDeltaTime * transform.forward;
		}
	}

	void Update() {
		if (Time.time > deathTime) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (! col.gameObject.CompareTag(ownerTag)) {
			HitPoint(col.gameObject, transform.position, transform.rotation);
		}
	}

	private void HitPoint(GameObject obj, Vector3 point, Quaternion desiredRotation) {
		// spawn particle system if it exists
		if (particleSystem != null) {
			Instantiate(particleSystem, point, transform.rotation);
		}

		// damage person if applicable
		if (obj != null) {
			obj.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}

		// destory bullet if applicable or if it's a raycastbullet
		if (destroyOnHit || raycastBullet) {
			Destroy(gameObject);
		}
	}

	public void SetOwnerTag(string tag) {
		ownerTag = tag;
	}
}
