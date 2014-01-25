using UnityEngine;
using System;
using System.Collections;

public class GunEffects : MonoBehaviour {

	public GameObject pieceToRotate = null;
	public GameObject[] piecesToColor;

	public void ChangeWeapon(Weapon weap) {

		// set the color
		foreach (GameObject piece in piecesToColor) {
			piece.renderer.material.color = weap.gunColor;
		}

		if (pieceToRotate != null) {
			AnimateWeapon();
		}

	}

	private void AnimateWeapon() {
		Quaternion qStartRotation = pieceToRotate.transform.localRotation;
		Vector3 startRotation = pieceToRotate.transform.rotation.eulerAngles;
		float rotateTimeInSeconds = 0.3f;
		Vector3 rotation = new Vector3(0.0f, 0.0f, -90.0f);

		Action<float> transformAction = delegate(float f) {
			pieceToRotate.transform.localRotation = qStartRotation;
			pieceToRotate.transform.Rotate(Vector3.Lerp(Vector3.zero, rotation, f));
		};
		StartCoroutine(SubfacetFunctions.TransformOverTime(transformAction, rotateTimeInSeconds));
	}

}
