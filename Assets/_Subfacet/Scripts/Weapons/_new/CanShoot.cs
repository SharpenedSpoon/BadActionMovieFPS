using UnityEngine;
using System.Collections;

public class CanShoot : MonoBehaviour {

	public Weapon weapon = null;

	public float shootOffsetForward = 1.0f;
	public float shootOffsetRight = 0.0f;
	public float shootOffsetUp = 0.5f;
	private Vector3 shootOffset;

	public bool shootFromMainCamera = false;
	public GameObject objectToShootFrom = null;
	private Transform tr;
	
	public bool useRateOfFire = true;
	public ParticleSystem muzzleFlashParticles = null;
	
	private AudioSource audioSource = null;

	public bool canShoot = true;

	void Start () {
		//SetWeapon(weapon); // unnecessary in Start() unless we do more stuff in the SetWeapon function than just assigning the weapon

		if (objectToShootFrom == null) {
			if (shootFromMainCamera && ! Camera.main) {
				objectToShootFrom = Camera.main.gameObject;
			} else {
				objectToShootFrom = gameObject;
			}
		}
		
		tr = objectToShootFrom.transform;
		
		SetShootOffset();

		audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		canShoot = weapon.canShoot;
	}

	public void SetWeapon(Weapon weap) {
		weapon = weap;
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
		if (canShoot) {
			SetShootOffset();
			weapon.Shoot(startPosition, directionToShootAt);
			
			if (muzzleFlashParticles != null) {
				muzzleFlashParticles.Play();
			}
			
			if (audioSource != null && weapon.sound != null) {
				audioSource.PlayOneShot(weapon.sound);
			}
		}
	}
}
