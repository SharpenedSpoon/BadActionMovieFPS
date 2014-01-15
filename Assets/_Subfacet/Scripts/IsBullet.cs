using UnityEngine;
using System.Collections;

public class IsBullet : MonoBehaviour {

	public bool raycastBullet = false;
	public bool linearSpeed = true;
	public bool destroyOnHit = true;

	public float speed = 1;
	public float force = 1;

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
			startingPosition = transform.position;
			if (! linearSpeed) {
				rigidbody.AddForce(force * Vector3.forward);
			}
		}
	}

	void FixedUpdate() {
		if (raycastBullet) {
			// Do this in FixedUpdate instead of Start so we have time to set the owner
			// call HitPoint with a raycast
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.localRotation.eulerAngles, out hit)) {
				HitPoint(hit.point);
			} else {
				// we missed!
				Destroy(gameObject);
			}
			// TODO: call HitPoint with a limited distance raycast if applicable
			return;
		}
		
		if (linearSpeed) {
			transform.position += speed * Time.fixedDeltaTime * Vector3.forward;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (! col.gameObject.CompareTag(ownerTag)) {
			HitPoint(transform.position);
		}
	}

	private void HitPoint(Vector3 point) {
		// spawn particle system if it exists
		if (particleSystem != null) {
			Instantiate(particleSystem, point, Quaternion.identity);
		}

		// TODO: damage person if applicable

		// destory bullet if applicable or if it's a raycastbullet
		if (destroyOnHit || raycastBullet) {
			Destroy(gameObject);
		}
	}

	public void SetOwner(string tag) {
		ownerTag = tag;
	}
}
