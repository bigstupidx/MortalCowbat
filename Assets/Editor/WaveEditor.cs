using UnityEngine;
using UnityEditor;
using Ai;
using Ge;

[CustomEditor(typeof(Wave))]
[CanEditMultipleObjects]
public class WaveEditor : Editor 
{


	void OnEnable()
	{
		
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		serializedObject.Update();

		if(GUILayout.Button("Open Editor")) {
			WaveWindow.Open(target as Wave);
		}

		serializedObject.ApplyModifiedProperties();
	}
}