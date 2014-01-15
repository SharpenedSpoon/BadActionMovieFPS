using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class FPSWalker : MonoBehaviour {

	public float maxSpeed = 7;
	public float force = 8;
	public float jumpSpeed = 5;

	private int state = 0;
	private bool grounded = false;
	private float jumpLimit = 0;


	public virtual bool jump {
		get {
			return Input.GetButtonDown("Jump");
		}
	}
	
	public virtual float horizontal {
		get {
			return Input.GetAxis("Horizontal") * force;
		}
	}
	
	public virtual float vertical {
		get {
			return Input.GetAxis("Vertical") * force;
		}
	}


	void Awake() {
		rigidbody.freezeRotation = true;
	}

	void OnCollisionEnter() {
		state++;
		if (state > 0) {
			grounded = true;
		}
	}
	
	void OnCollisionExit() {
		state--;
		if (state < 1) {
			grounded = false;
			state = 0;
		}
	}

	void FixedUpdate() {
		if (rigidbody.velocity.magnitude < maxSpeed && grounded == true) {
			rigidbody.AddForce(transform.rotation * Vector3.forward * vertical);
			rigidbody.AddForce(transform.rotation * Vector3.right * horizontal);
		}

		if (jumpLimit < 10) {
			jumpLimit++;
		}

		if (jump && grounded && jumpLimit >= 10) {
			rigidbody.velocity = rigidbody.velocity + (jumpSpeed * Vector3.up);
			jumpLimit = 0;
		}
	}
}
