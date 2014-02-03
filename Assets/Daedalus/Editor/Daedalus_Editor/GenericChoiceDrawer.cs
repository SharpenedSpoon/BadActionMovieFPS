using UnityEditor;
using UnityEngine;


// See http://catlikecoding.com/unity/tutorials/editor/custom-data/

public class GenericChoiceDrawer : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
#if UNITY_4_3
		return 16f;
#endif

#if UNITY_4_2
		return property.isExpanded && label != GUIContent.none ? 32f : 16f;
#endif
	}
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {		
		position.height = 16f;


#if UNITY_4_2
		if (label != GUIContent.none) {

			Rect foldoutPosition = position;
			
			//			foldoutPosition.x -= 14f;
			//			foldoutPosition.width += 14f;
			label = EditorGUI.BeginProperty(position, label, property);
			property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, label, true);
			EditorGUI.EndProperty();
			
			if (!property.isExpanded) {
				return;
			}
			position.y += 16f;
		}
#endif

		position = EditorGUI.IndentedRect(position);

		float totWidth = position.width;
		position.width = totWidth*2/3f;
		int oldIndentLevel = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

#if UNITY_4_3
		EditorGUIUtility.labelWidth = 50f;
#endif

#if UNITY_4_2
		EditorGUIUtility.LookLikeControls(50f);
#endif

		EditorGUI.PropertyField(position, property.FindPropertyRelative("choice"),new GUIContent("Choice"));
		position.x += position.width;
		position.width = totWidth*1/3f;
		EditorGUI.PropertyField(position, property.FindPropertyRelative("weight"),new GUIContent("Weight"));
		EditorGUI.indentLevel = oldIndentLevel;
	}
}
