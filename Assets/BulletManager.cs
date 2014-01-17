using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	public new static BulletManager active;

	void Awake() {
		active = this;
	}
}
