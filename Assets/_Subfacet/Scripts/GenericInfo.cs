using UnityEngine;
using System.Collections;

public class GenericInfo : MonoBehaviour {

	public new static GenericInfo active;

	private GUIText guiText;

	void Awake() {
		active = this;
	}

	void Start () {
		guiText = GetComponent<GUIText>();
	}

	public void SetText(string txt) {
		guiText.text = txt;
	}
}
