using UnityEngine;
using System.Collections;

public class HasHealth : MonoBehaviour {
	
	public int MaxHP = 1;
	public float health { get; private set; }
	public bool destroyOnDeath = true;
	public bool explodeOnDeath = true;

	private ExploderObject exploder = null;

	private GivesScore givesScore = null;
	
	void Awake() {
		health = MaxHP;
	}
	
	void Start () {
		if (ExplosionController.active) {
			exploder = ExplosionController.active.gameObject.GetComponent<ExploderObject>();
		}

		givesScore = GetComponent<GivesScore>();
	}
	
	void Update () {
		if (health <= 0) {
			Die();
		}
	}
	
	protected virtual void Die() {
		if (givesScore) {
			givesScore.DeathOccured();
		}

		if (exploder && explodeOnDeath) {
			exploder.gameObject.transform.position = transform.position;
			exploder.Radius = 0.5f;
			exploder.Force = 1;
			exploder.Explode();
		} else if (destroyOnDeath) {
			Destroy(gameObject);
		}
	}
	
	public void TakeDamage(float dmg) {
		if (givesScore) {
			givesScore.GotShot();
		}

		health -= dmg;
	}
}