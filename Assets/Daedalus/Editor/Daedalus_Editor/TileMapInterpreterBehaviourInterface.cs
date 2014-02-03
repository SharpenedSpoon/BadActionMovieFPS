using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor (typeof(TileMapInterpreterBehaviour))]
public class TileMapInterpreterBehaviourInterface : MapInterpreterBehaviourInterface {
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		TileMapInterpreterBehaviour t = (TileMapInterpreterBehaviour)target;
		t.fillWithFloors = 	EditorGUILayout.Toggle("Fill With Floors",t.fillWithFloors);
	}

}
