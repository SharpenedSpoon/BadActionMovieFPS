using UnityEngine;
using System.Collections;

public class DestroyParticlesOnCompletion : MonoBehaviour {

	private ParticleSystem particles;
	private ParticleSystem[] allParticles;
	public bool applyToChildObjects = false;

	void Start () {
		if (applyToChildObjects) {
			allParticles = GetComponentsInChildren<ParticleSystem>();
		} else {
			particles = GetComponent<ParticleSystem>();
		}

		// Collect all particle systems under the BulletManager for organizational purposes
		transform.parent = BulletManager.active.transform;
	}

	void Update () {
		if (applyToChildObjects) {
			// assume all particles are stopped, then try to prove it wrong.
			bool particlesAreAllStopped = true;
			foreach (ParticleSystem part in allParticles) {
				if (! part.isStopped) {
					particlesAreAllStopped = false;
					break;
				}
			}
			if (particlesAreAllStopped) {
				Destroy(gameObject);
			}
		} else {
			if (particles.isStopped) {
				Destroy(gameObject);
			}
		}


	}
}
