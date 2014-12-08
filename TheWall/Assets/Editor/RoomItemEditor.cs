using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomItem))]
public class RoomItemEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();
		//var controller = target as RoomItem;
		EditorGUIUtility.LookLikeInspector();
		SerializedProperty tps = serializedObject.FindProperty ("targetPoints");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(tps, true);
		if(EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
		EditorGUIUtility.LookLikeControls();
		// ...
	}
}
