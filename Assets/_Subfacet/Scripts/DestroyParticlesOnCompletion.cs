using UnityEngine;
using System.Collections;

public class DestroyParticlesOnCompletion : MonoBehaviour {

	public ParticleSystem particles;

	void Start () {
		particles = GetComponent<ParticleSystem>();

		// Collect all particle systems under the BulletManager for organizational purposes
		transform.parent = BulletManager.active.transform;
	}

	void Update () {
		if (particles.isStopped) {
			Destroy(gameObject);
		}
	}
}
