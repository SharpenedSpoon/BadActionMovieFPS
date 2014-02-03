using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor (typeof(Meshes3DPhysicalMapBehaviour))]
public class Meshes3DPhysicalMapBehaviourInterface : PhysicalMapBehaviourInterface {
	
	SerializedObject m_Object;
	
    void OnEnable () {
        m_Object = new SerializedObject (target);
	}
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();	
		
		Meshes3DPhysicalMapBehaviour t = (Meshes3DPhysicalMapBehaviour)target;
				
		EditorGUILayout.BeginHorizontal();
		t.addCeilingToCorridors = 	EditorGUILayout.Toggle("Ceiling on Corridors",t.addCeilingToCorridors);
		if (g.createRooms) t.addCeilingToRooms = 		EditorGUILayout.Toggle("Ceiling on Rooms",t.addCeilingToRooms);
		EditorGUILayout.EndHorizontal();
		
		
        m_Object.Update ();
		base.DrawSpecificInspector(m_Object);
		if (t.addCeilingToCorridors) {
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorCeilingVariations"),true);
		}
		if(g.createRooms){
			if (t.addCeilingToRooms) {
				EditorGUILayout.PropertyField(m_Object.FindProperty("roomCeilingVariations"),true);
			}
		}
		m_Object.ApplyModifiedProperties ();
		
	}
	
}
