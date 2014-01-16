using UnityEngine;
using System.Collections;

public class DestroyParticlesOnCompletion : MonoBehaviour {

	public ParticleSystem particles;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (particles.isStopped) {
			Destroy(gameObject);
		}
	}
}
