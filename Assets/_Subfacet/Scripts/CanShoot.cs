using UnityEngine;
using System.Collections;

public class CanShoot : MonoBehaviour {

	public GameObject bullet = null;

	void Start () {
	
	}

	void Update () {
	
	}



	public void Shoot() {
		Shoot(transform.position + transform.forward + 0.5f*transform.up, transform.rotation);
	}

	public void Shoot(Vector3 pos) {
		Shoot(pos, transform.rotation);
	}

	public void Shoot(Quaternion dir) {
		Shoot(transform.position + transform.forward + 0.5f*transform.up, dir);
	}

	public void Shoot(Vector3 pos, Quaternion dir) {
		GameObject newBullet = Instantiate(bullet, pos, dir) as GameObject;
		newBullet.SendMessage("SetOwnerTag", tag, SendMessageOptions.DontRequireReceiver);
	}
}
