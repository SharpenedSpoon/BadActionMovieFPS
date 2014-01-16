using UnityEngine;
using System.Collections;

public class HasHealth : MonoBehaviour {
	
	public int MaxHP = 1;
	private int health;
	
	private ExploderObject exploder = null;
	
	void Awake() {
		health = MaxHP;
	}
	
	void Start () {
		exploder = GetComponent<ExploderObject>();
	}
	
	void Update () {
		if (health <= 0) {
			Die ();
		}
	}
	
	private void Die() {
		if (exploder != null) {
			exploder.Explode();
		} else {
			Destroy(gameObject);
		}
	}
	
	public void TakeDamage(int dmg) {
		health -= dmg;
	}
}