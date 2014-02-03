using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {

	public bool explodeOnStart = true;
	public bool pushObjects = true;
	public float pushForce = 10;
	public float radius = 5.0f;
	public float damage = 1.0f;
	public ExploderObject exploder { get; private set; }
	
	void Start() {
		if (! ExplosionController.active) {
			enabled = false;
			return;
		}
		exploder = ExplosionController.active.gameObject.GetComponent<ExploderObject>();

		if (explodeOnStart) {
			MyExplode(radius, damage);
		}
	}
	
	public void MyExplode(float rad, float dmg) {
		MyExplode(transform.position, rad, dmg);
	}
	
	public void MyExplode(Vector3 pos, float rad, float dmg) {
		if (exploder) {
			exploder.Force = pushForce;
			exploder.Radius = rad;
			exploder.gameObject.transform.position = pos;
			exploder.Explode();
		}
		
		Collider[] hitColliders = Physics.OverlapSphere(pos, rad);
		foreach (Collider hit in hitColliders) {
			if (hit) {
				HasHealth hitHealth = hit.GetComponent<HasHealth>();
				if (hitHealth) {
					// we want the ExploderObject to handle object destruction and explosion
					hitHealth.destroyOnDeath = false;
					hitHealth.explodeOnDeath = false;
					
                    hitHealth.TakeDamage(dmg);
                }
                hit.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
                if (hit.rigidbody) {
                    hit.rigidbody.AddExplosionForce(pushForce, pos, rad, 3.0f);
                }
            }
        }
	}
}
