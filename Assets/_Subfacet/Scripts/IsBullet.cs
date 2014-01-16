using UnityEngine;
using System.Collections;

public class IsBullet : MonoBehaviour {

	public bool raycastBullet = false;
	public bool linearSpeed = true;
	public bool destroyOnHit = true;

	public bool freezeRotation = true;

	public float speed = 10;
	public float force = 1000;

	public bool limitedRange = false;
	public float range = 100;

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
	}

	void FixedUpdate() {
		if (raycastBullet) {
			// Do this in FixedUpdate instead of Start so we have time to set the owner
			// call HitPoint with a raycast
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit)) {
				HitPoint(hit.point, Quaternion.LookRotation(hit.normal));
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

	void OnCollisionEnter(Collision col) {
		if (! col.gameObject.CompareTag(ownerTag)) {
			HitPoint(transform.position);
		}
	}

	private void HitPoint(Vector3 point) {
		HitPoint(point, transform.rotation);
	}

	private void HitPoint(Vector3 point, Quaternion desiredRotation) {
		// spawn particle system if it exists
		if (particleSystem != null) {
			Instantiate(particleSystem, point, transform.rotation);
		}

		// TODO: damage person if applicable

		// destory bullet if applicable or if it's a raycastbullet
		if (destroyOnHit || raycastBullet) {
			Destroy(gameObject);
		}
	}

	public void SetOwnerTag(string tag) {
		ownerTag = tag;
	}
}
