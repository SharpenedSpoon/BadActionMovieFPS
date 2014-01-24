using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	// TODO convert isLinear to an enum
	
	public bool isHitScan = false;
	public bool isLinear = true; // otherwise it is a projectile
	public bool destroyOnHit = true;
	public float initialSpeed = 10.0f;
	public float range = 0.0f; // a range of 0 means don't worry about range
	public float damage = 1.0f;
	public float lifetime = 20.0f;
	public List<GameObject> objectsToSpawnOnHit = new List<GameObject>();
	public List<GameObject> objectsToSpawnOnDestroy = new List<GameObject>();
	
	void Start () {
		
	}
	
	void Update () {
		
	}
}
