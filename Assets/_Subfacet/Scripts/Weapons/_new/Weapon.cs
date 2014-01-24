using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public GameObject bullet = null;
	public bool autofire = false;
	public int ammoCount = 1000;
	public int magazineSize = 10;
	public int bulletsPerShot = 1;
	public float rateOfFire = 30; // shots per second
	public float reloadTime = 0.5f;
	public Color gunColor = Color.white;
	public Vector3 idleHoldObjectPosition = Vector3.zero;
	public GameObject muzzleFlash = null;
	public GameObject idleHoldObject = null;

	void Start () {
	
	}

	void Update () {
	
	}
}
