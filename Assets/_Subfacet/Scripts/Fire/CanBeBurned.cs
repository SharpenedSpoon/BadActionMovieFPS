﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanBeBurned : MonoBehaviour {

	public float fireDamagePerSecond = 0.1f;
	public bool onFire = false;
	public float health = 1.0f;

	public List<CanBeBurned> neighbors = new List<CanBeBurned>();

	void Start () {
		if (! FireController.active) {
			enabled = false;
			return;
		}

		// tell the firecontroller that I exist
		FireController.active.RegisterBurnableObject(gameObject);
	}

	void Update() {
		renderer.material.color = new Color(2.0f*(1.0f - health) - 1.0f,1,1);
	}

	public void HandleFireDamage(float dmg) {
		if (dmg > 0.0f) {
			TakeDamage(dmg);
		} else if (dmg < 0.0f) {
			HealDamage(Mathf.Abs(dmg));
		}
	}

	public void TakeDamage(float dmg) {
		if (onFire) {
			return;
		}
		health -= dmg;
		if (health <= 0.0f) {
			onFire = true;
		}
	}

	public void HealDamage(float dmg) {
		health = Mathf.Min(health + dmg, 1.0f);
	}
}
