using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireController : MonoBehaviour {

	public new static FireController active;
	public float radius = 1.0f;

	private List<CanBeBurned> burnableObjects = new List<CanBeBurned>();
	private Dictionary<CanBeBurned, List<CanBeBurned>> burnableNeighbors = new Dictionary<CanBeBurned, List<CanBeBurned>>();

	void Awake() {
		active = this;
	}

	void Start() {
		
	}

	void Update() {
		RemoveDestroyedObjects();

		if (burnableObjects.Count > 0) {
			if ()
			FigureOutFire();
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
		foreach (CanBeBurned burner in burnableObjects) {
			if (Vector3.Distance(go.gameObject.transform.position, burner.gameObject.transform.position) <= radius) {
				output.Add(burner);
			}
		}
		return output;
	}

	private void RegisterAllBurnableNeighbors() {
		burnableNeighbors = new Dictionary<CanBeBurned, List<CanBeBurned>>();
		foreach (CanBeBurned burner in burnableObjects) {
			burnableNeighbors.Add(burner, FindNeighbors(burner, radius));
		}
	}
}
