using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[CustomEditor (typeof(PhysicalMapBehaviour))]
public class PhysicalMapBehaviourInterface : Editor {

	
	public GeneratorBehaviour g;
	public MapInterpreterBehaviour i;
	public override void OnInspectorGUI() {		
		PhysicalMapBehaviour t = (PhysicalMapBehaviour)target;
		if (g == null) g = t.gameObject.GetComponent<GeneratorBehaviour>();
		if (i == null) i = t.gameObject.GetComponent<MapInterpreterBehaviour>();

		if (g.createStartAndEnd) {
//			EditorGUILayout.BeginHorizontal();
			t.createPlayer = EditorGUILayout.Toggle("Add Player",t.createPlayer);
			if (t.createPlayer) t.playerPrefab = EditorGUILayout.ObjectField("Player Prefab",t.playerPrefab,typeof(GameObject),true) as GameObject;
//			EditorGUILayout.EndHorizontal();
		}
		if (!t.AutomaticBatching) t.enabledBatching = EditorGUILayout.Toggle("Perform Batching",t.enabledBatching);
		if (!t.AutomaticOrientation && !t.ForcedOrientation) t.mapPlaneOrientation = (MapPlaneOrientation)EditorGUILayout.EnumPopup("Orientation",t.mapPlaneOrientation);

	}
	
	protected void DrawSpecificInspector(SerializedObject m_Object){
//		PhysicalMapBehaviour t = (PhysicalMapBehaviour)target;
		
		EditorGUILayout.PropertyField(m_Object.FindProperty("corridorWallVariations"),true);
		if (!i.useAdvancedFloors){
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorVariations"),true);
		} else {
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorUVariations"),new GUIContent("Corridor Floor U Variations"),true);
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorIVariations"),new GUIContent("Corridor Floor I Variations"),true);
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorLVariations"),new GUIContent("Corridor Floor L Variations"),true);
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorTVariations"),new GUIContent("Corridor Floor T Variations"),true);
			EditorGUILayout.PropertyField(m_Object.FindProperty("corridorFloorXVariations"),new GUIContent("Corridor Floor X Variations"),true);
		}

		EditorGUILayout.PropertyField(m_Object.FindProperty("corridorColumnVariations"),true);
		
		if(g.createRooms){
			EditorGUILayout.PropertyField(m_Object.FindProperty("roomWallVariations"),true);

			if (!i.useAdvancedFloors){
				EditorGUILayout.PropertyField(m_Object.FindProperty("roomFloorVariations"),true);
			} else{
				EditorGUILayout.PropertyField(m_Object.FindProperty("roomFloorInsideVariations"),new GUIContent("Room Floor Inside Variations"),true);
				EditorGUILayout.PropertyField(m_Object.FindProperty("roomFloorBorderVariations"),new GUIContent("Room Floor Border Variations"),true);
				EditorGUILayout.PropertyField(m_Object.FindProperty("roomFloorCornerVariations"),new GUIContent("Room Floor Corner Variations"),true);
			}

			EditorGUILayout.PropertyField(m_Object.FindProperty("roomColumnVariations"),true);
			if (i.createColumnsInRooms) EditorGUILayout.PropertyField(m_Object.FindProperty("insideRoomColumnVariations"),true);
			if (i.drawDoors) {
				EditorGUILayout.PropertyField(m_Object.FindProperty("doorVariations"),true);
				EditorGUILayout.PropertyField(m_Object.FindProperty("doorColumnVariations"),true);	
			}
		}
		
		if (i.drawRocks) EditorGUILayout.PropertyField(m_Object.FindProperty("rockVariations"),true);
		
		if (g.createStartAndEnd){
			EditorGUILayout.PropertyField(m_Object.FindProperty("entrancePrefab"),true);
			EditorGUILayout.PropertyField(m_Object.FindProperty("exitPrefab"),true);
		}
	}
	
//	#region Save/Load configuration	
//	// Save configuration
//	public void Save<T> (T instance)
//	{
//		XmlSerializer serializer = new XmlSerializer(typeof(T));
//		
//		using (var ms = new MemoryStream())
//		{
//			serializer.Serialize(ms, instance);
//			string path = EditorUtility.SaveFilePanel("Save file...", "", "Level", "xml");
//			File.WriteAllText(path, System.Text.ASCIIEncoding.ASCII.GetString (ms.ToArray ()));
//			AssetDatabase.Refresh();
//		}
//	}
//	
//	// Load configuration
//	public T Load<T> ()
//	{
//		XmlSerializer serializer = new XmlSerializer (typeof(T));
//		
//        T instance;
//		
//		string path = EditorUtility.OpenFilePanel("Load file...", "", "xml");
//
//		using (var ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(File.ReadAllText(path))))
//		{
//			instance = (T)serializer.Deserialize(ms);
//		}
//
//       return instance;
//	}
//	#endregion
}
