using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor (typeof(MapInterpreterBehaviour))]
public class MapInterpreterBehaviourInterface : Editor {
	
	public GeneratorBehaviour g;
	public override void OnInspectorGUI() {		
		MapInterpreterBehaviour t = (MapInterpreterBehaviour)target;
		if (g == null) g = t.gameObject.GetComponent<GeneratorBehaviour>();
		
		t.drawRocks = 			EditorGUILayout.Toggle("Draw Rocks",t.drawRocks);
		t.drawWallCorners = 	EditorGUILayout.Toggle("Draw Wall Corners",t.drawWallCorners);
		t.drawDoors = 			EditorGUILayout.Toggle("Place Doors in Passages",t.drawDoors);

		if (g.createRooms) t.createColumnsInRooms = EditorGUILayout.Toggle("Draw Columns in Rooms",t.createColumnsInRooms);
		t.randomOrientations = EditorGUILayout.Toggle("Randomize Orientations",t.randomOrientations);
		
		t.useAdvancedFloors = 	EditorGUILayout.Toggle("Use Advanced Floors",t.useAdvancedFloors);

	}

}
