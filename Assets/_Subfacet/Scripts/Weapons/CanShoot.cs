using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class CanShoot : MonoBehaviour {

	public Weapon initWeapon = null;
	public Weapon weapon { get; private set; }

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

	public Weapon foundChild;

	void Start () {
		SetWeapon(initWeapon);

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
		if (weapon != null) {
			canShoot = weapon.canShoot;
		} else {
			canShoot = false;
		}
	}

	public void SetWeapon(Weapon weap) {
		if (weap != null) {
			Transform foundChildTransform = transform.Find(weap.gameObject.name + "(Clone)"); // instances of prefabs have "(Clone)" at the end
			//GameObject foundChild;
			if (foundChildTransform == null) {
				foundChild = Instantiate(weap, transform.position, transform.rotation) as Weapon;
				foundChild.transform.parent = gameObject.transform;
			} else {
				foundChild = foundChildTransform.gameObject.GetComponent<Weapon>();
			}
			foundChild.gameObject.tag = gameObject.tag;
			weapon = foundChild;
		}
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
			weapon.Shoot(startPosition + shootOffset, directionToShootAt);
			
			if (muzzleFlashParticles != null) {
				muzzleFlashParticles.Play();
			}
			
			if (audioSource != null && weapon.sound != null) {
				audioSource.PlayOneShot(weapon.sound);
			}
		}
	}
}
