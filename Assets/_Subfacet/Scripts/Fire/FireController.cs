using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireController : MonoBehaviour {

	public new static FireController active;
	public float radius = 2.0f;

	private List<CanBeBurned> burnableObjects = new List<CanBeBurned>();
	[SerializeField]
	private Dictionary<CanBeBurned, List<CanBeBurned>> burnableNeighbors = new Dictionary<CanBeBurned, List<CanBeBurned>>();

	void Awake() {
		active = this;
	}

	void Start() {
		MakeBurnableObjects(10, 10);
	}

	void Update() {
		RemoveDestroyedObjects();

		if (burnableObjects.Count > 0) {
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)) {
				FigureOutFire();
			}
			if (Input.GetKeyDown(KeyCode.Z)) {
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
					if (hit.transform) {
						if (hit.transform.gameObject) {
							if (hit.transform.gameObject.GetComponent<CanBeBurned>()) {
								hit.transform.gameObject.GetComponent<CanBeBurned>().onFire = true;
								Debug.Log("set " + hit.transform.gameObject + " on fire!");
							} else {
								Debug.Log("no canbeburned");
							}
						} else {
							Debug.Log("no gameobject");
						}
					} else {
						Debug.Log("no transform");
					}
				} else {
					Debug.Log("no hit");
				}
			}
		}
	}

	private void MakeBurnableObjects(int rows, int cols) {
		Vector3 startPosition = 5.0f * Vector3.up;
		float cubeSize = 1.0f;
		float margin = 0.05f;
		for (int i=0; i<rows; i++) {
			for (int j=0; j<cols; j++) {
				GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				newCube.name = "Cube " + i +" - " + j;
				newCube.transform.localScale = cubeSize * Vector3.one;
				newCube.transform.position = startPosition + ((i*cubeSize + i*margin) * Vector3.forward) + ((j*cubeSize + j*margin) * Vector3.right);
				newCube.AddComponent<CanBeBurned>();
			}
		}
	}

	private void RemoveDestroyedObjects() {
		burnableObjects.RemoveAll(item => item == null);
	}

	private void FigureOutFire() {
		Dictionary<CanBeBurned, float> damageDeltas = new Dictionary<CanBeBurned, float>();

		foreach (CanBeBurned burner in burnableObjects) {

			damageDeltas.Add(burner, 0.0f);
			foreach (CanBeBurned other in burnableNeighbors[burner]) {
				if (other.onFire) {
					damageDeltas[burner] += other.fireDamagePerSecond;
				} else {
					damageDeltas[burner] -= 0.5f * other.fireDamagePerSecond;
				}
			}
		}

		foreach (CanBeBurned burner in burnableObjects) {
			burner.HandleFireDamage(damageDeltas[burner]);
		}
	}

	public void RegisterBurnableObject(GameObject go) {
		CanBeBurned burner = go.GetComponent<CanBeBurned>();
		if (! burner) {
			Debug.LogError("Tried to add a burnable object but no CanBeBurned component was found.");
			return;
		}

		burnableObjects.Add(burner);

		RegisterAllBurnableNeighbors();
	}
	
	private List<CanBeBurned> FindNeighbors(CanBeBurned go, float radius) {
		List<CanBeBurned> output = new List<CanBeBurned>();
		/*foreach (CanBeBurned burner in burnableObjects) {
			if (Vector3.Distance(go.gameObject.transform.position, burner.gameObject.transform.position) <= radius) {
				output.Add(burner);
			}
		}*/
		foreach (Collider other in Physics.OverlapSphere(go.transform.position, radius)) {
			if (other.gameObject) {
				if (other.gameObject != go.gameObject && other.GetComponent<CanBeBurned>()) {
					output.Add(other.gameObject.GetComponent<CanBeBurned>());
				}
			}
		}
		return output;
	}

	private void RegisterAllBurnableNeighbors() {
		burnableNeighbors = new Dictionary<CanBeBurned, List<CanBeBurned>>();
		foreach (CanBeBurned burner in burnableObjects) {
			burnableNeighbors.Add(burner, FindNeighbors(burner, radius));
			burner.neighbors = FindNeighbors(burner, radius);
		}
	}
}
