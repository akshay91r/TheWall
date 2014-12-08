using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VisionCone))]

public class VisionConeEditor : Editor {
	public override void OnInspectorGUI() {

		//all other public variables
		VisionCone myTarget = (VisionCone)target;
		
		myTarget.speed = EditorGUILayout.FloatField("Speed", myTarget.speed);
		myTarget.coneSize = EditorGUILayout.FloatField("Cone Size", myTarget.coneSize);
		myTarget.length = EditorGUILayout.FloatField("Length", myTarget.length);
		myTarget.waitTime = EditorGUILayout.FloatField("Wait Time", myTarget.waitTime);
		myTarget.initialDelay = EditorGUILayout.FloatField("Initial Delay", myTarget.initialDelay);
		myTarget.startAngle = EditorGUILayout.FloatField("Start Angle", myTarget.startAngle);

		//for displaying the angle array
		serializedObject.Update();
		//var controller = target as RoomItem;
		EditorGUIUtility.LookLikeInspector();
		SerializedProperty tps = serializedObject.FindProperty ("angleValues");

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(tps, true);
		if(EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
		EditorGUIUtility.LookLikeControls();

		GUILayout.Label ("eg format 45-C : where 45 is the target angle, direction clockwise");
	}
}
//