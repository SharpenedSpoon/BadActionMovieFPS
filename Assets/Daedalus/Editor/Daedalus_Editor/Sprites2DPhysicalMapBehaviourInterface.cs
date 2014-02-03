using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor (typeof(Sprites2DPhysicalMapBehaviour))]
public class Sprites2DPhysicalMapBehaviourInterface : PhysicalMapBehaviourInterface {
	SerializedObject m_Object;
    void OnEnable () {
        m_Object = new SerializedObject (target);
	}
	
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		Sprites2DPhysicalMapBehaviour t = (Sprites2DPhysicalMapBehaviour)target;
		
		m_Object.Update ();

		t.tileSize = EditorGUILayout.FloatField("Tile Size",t.tileSize);
		
		t.spritePrefab = EditorGUILayout.ObjectField("Sprite Prefab",t.spritePrefab,typeof(GameObject),true) as GameObject;

		base.DrawSpecificInspector(m_Object);
		m_Object.ApplyModifiedProperties ();
		
		
	}
	
}
