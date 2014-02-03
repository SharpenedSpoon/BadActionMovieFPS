using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IdLayerRef))]
public class IdLayerRefDrawer : PropertyDrawer {
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {	
		EditorGUILayout.BeginHorizontal();
		position.width = position.width/2;
		EditorGUI.PropertyField(position, property.FindPropertyRelative("id"),new GUIContent("Id"));
		position.x += position.width;
		EditorGUI.PropertyField(position, property.FindPropertyRelative("layer"),new GUIContent("Layer"));
		EditorGUILayout.EndHorizontal();
	}
}
