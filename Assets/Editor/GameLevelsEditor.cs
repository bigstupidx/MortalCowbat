using System;

using UnityEngine;
using UnityEditor;
using Ai;

[CustomEditor(typeof(GameLevels))]
public class GameLevelsEditor : Editor 
{
	void OnEnable()
	{

	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		serializedObject.Update();

		if(GUILayout.Button("Open Editor")) {
			GameLevelsWindow.Open(target as GameLevels);
		}

		serializedObject.ApplyModifiedProperties();
	}
}


