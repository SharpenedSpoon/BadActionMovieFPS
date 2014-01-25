using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CanShoot))]
public class AIShooter : MonoBehaviour {

	public float range = 5;

	private GameObject target;
	private CanShoot shooter;

	void Start () {
		FindTarget();
		shooter = GetComponent<CanShoot>();
	}

	void Update () {
		if (target == null) {
			// the player might have died, and is maybe going to respawn/has respawned so try finding them again
			FindTarget();
			
			// if we still don't have a target, then don't do the rest of the stuff in Update().
			if (target == null) {
				return;
			}
		}

		if (Vector3.Distance(transform.position, target.transform.position) < range) {
			shooter.Shoot(target);
		}
	}

	private void FindTarget() {
		target = GameObject.FindGameObjectWithTag("Player");
	}
}
