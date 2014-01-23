using UnityEngine;

[System.Serializable]
public class Weapon {
	public string name;
	public GameObject bulletObject;
	public Color color;
	public float shotsPerSecond = 2;
	public float reloadTimeNeeded = 0;
	public bool autofire = false;
	public AudioClip sound = null;
}