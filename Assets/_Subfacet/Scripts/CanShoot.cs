using UnityEngine;
using System.Collections;

public class CanShoot : MonoBehaviour {

	public GameObject bullet = null;

	public float shootOffsetForward = 1.0f;
	public float shootOffsetRight = 0.0f;
	public float shootOffsetUp = 0.5f;
	private Vector3 shootOffset;

	void Start() {
		SetShootOffset();
	}

	private void SetShootOffset() {
		shootOffset = (shootOffsetForward * transform.forward) + (shootOffsetRight * transform.right) + (shootOffsetUp * transform.up);
	}

	public void Shoot() {
		Shoot(transform.position, transform.rotation);
	}

	public void Shoot(Vector3 startPosition) {
		Shoot(startPosition, transform.rotation);
	}

	public void Shoot(Quaternion directionToShootAt) {
		Shoot(transform.position, directionToShootAt);
	}

	public void Shoot(GameObject targetGameObject) {
		Shoot(transform.position, Quaternion.LookRotation(targetGameObject.transform.position - transform.position));
	}

	public void Shoot(Vector3 startPosition, Quaternion directionToShootAt) {
		SetShootOffset();
		GameObject newBullet = Instantiate(bullet, startPosition + shootOffset, directionToShootAt) as GameObject;
		newBullet.SendMessage("SetOwnerTag", tag, SendMessageOptions.DontRequireReceiver);
	}
}
