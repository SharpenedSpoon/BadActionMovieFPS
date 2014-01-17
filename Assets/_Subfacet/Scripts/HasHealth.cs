using UnityEngine;
using System.Collections;

public class HasHealth : MonoBehaviour {
	
	public int MaxHP = 1;
	public int health { get; private set; }
	public bool destroyOnDeath = true;
	public bool explodeOnDeath = true;
	public int pointsForScore = 1;
	
	private ExploderObject exploder = null;
	
	void Awake() {
		health = MaxHP;
	}
	
	void Start () {
		exploder = ExplosionController.active.gameObject.GetComponent<ExploderObject>();
	}
	
	void Update () {
		if (health <= 0) {
			Die ();
		}
	}
	
	private void Die() {
		if (exploder && explodeOnDeath) {
			exploder.gameObject.transform.position = transform.position;
			exploder.Radius = 0.5f;
			exploder.Force = 1;
			exploder.Explode();
		} else if (destroyOnDeath) {
			Destroy(gameObject);
		}
	}
	
	public void TakeDamage(int dmg) {
		health -= dmg;
	}
}