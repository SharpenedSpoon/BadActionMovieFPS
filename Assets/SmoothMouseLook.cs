/*
 * From here: http://wiki.unity3d.com/index.php/SmoothMouseLook
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class SmoothMouseLook : MonoBehaviour {

	public bool deactivateOnLockCursor = true;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 10F;
	public float sensitivityY = 10F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationX = 0F;
	float rotationY = 0F;
	
	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0F;	
	
	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0F;
	
	public int frameCounter = 20;
	
	Quaternion originalRotation;

	void Start () {			
		if (rigidbody) {
			rigidbody.freezeRotation = true;
		}
		originalRotation = transform.localRotation;
	}
	
	void Update() {
		if (deactivateOnLockCursor && ! Screen.lockCursor) {
			return;
		}

		if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseX) {
			rotAverageX = 0f;
			
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);

			rotArrayX = UpdateListValues(rotArrayX, rotationX, frameCounter, minimumX, maximumX);

			rotAverageX = ListAverage(rotArrayX);
			
			rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);
			
			Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}

		if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseY) {
			rotAverageY = 0f;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);

			rotArrayY = UpdateListValues(rotArrayY, rotationY, frameCounter, minimumY, maximumY);

			rotAverageY = ListAverage(rotArrayY);
			
			rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
			
			Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}
	
	public static float ClampAngle (float angle, float min, float max) {
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}			
		}
		return Mathf.Clamp (angle, min, max);
	}

	public float ListAverage(List<float> floatList) {
		float avg = 0;
		for (int i = 0; i < floatList.Count; i++) {
			avg += floatList[i];
		}
		return (avg / floatList.Count);
	}

	public List<float> UpdateListValues(List<float> floatList, float newVal, int maxListSize, float minVal, float maxVal) {
		// unnecessary when we clamp the roation in the first place
		//floatList.Add(ClampAngle(newVal, minVal, maxVal));

		floatList.Add(newVal);

		// Cull the list down to the correct size
		if (floatList.Count >= maxListSize) {
			floatList.RemoveAt(0);
		}

		// unnecessary when we clamp the roation in the first place
		/*
		// Check and see if the list is small
		if (floatList.Count == 1) {
			floatList[0] = ClampAngle(floatList[0], minVal, maxVal);
		} else {
			floatList.RemoveAll(item => (item < minVal || item > maxVal));
		}
		*/

		return floatList;
	}
}
