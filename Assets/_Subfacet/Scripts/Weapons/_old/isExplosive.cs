using UnityEngine;
using System.Collections;

public class isExplosive : MonoBehaviour {

	public bool pushObjects = true;
	public float pushForce = 100;
	public ExploderObject exploder;

	void Start() {
		if (! ExplosionController.active) {
			enabled = false;
			return;
		}
		exploder = ExplosionController.active.gameObject.GetComponent<ExploderObject>();
	}
	
	public void MyExplode(float radius, float damage) {
		MyExplode(transform.position, radius, damage);
	}

	public void MyExplode(Vector3 pos, float radius, float damage) {
		if (exploder) {
			exploder.Force = pushForce;
			exploder.Radius = radius;
			exploder.gameObject.transform.position = pos;
			exploder.Explode();
		}

		Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
		foreach (Collider hit in hitColliders) {
			if (hit) {
				HasHealth hitHealth = hit.GetComponent<HasHealth>();
				if (hitHealth) {
					// we want the ExploderObject to handle object destruction and explosion
					hitHealth.destroyOnDeath = false;
					hitHealth.explodeOnDeath = false;

					hitHealth.TakeDamage(damage);
				}
				hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
				if (hit.rigidbody) {
					hit.rigidbody.AddExplosionForce(pushForce, pos, radius, 3.0f);
				}
			}
		}
	}

}
