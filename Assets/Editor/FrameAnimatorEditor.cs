using Vis;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

[CustomEditor(typeof(FrameAnimator))]
public class FrameAnimatorEditor : Editor 
{
	const int ButtonWidth = 70;
	const int SmallButtonWidth = 40;

	bool showDefaultInspector;

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		var myTarget = (FrameAnimator)target;

		showDefaultInspector = EditorGUILayout.Toggle("Debug", showDefaultInspector);
		if (showDefaultInspector) {
			base.OnInspectorGUI();
		}


		for (int i = 0; i < myTarget.Sprites.Count; ++i) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(10));
			myTarget.Sprites[i] = (Sprite)EditorGUILayout.ObjectField("", myTarget.Sprites[i], typeof(Sprite), false, GUILayout.Width(100));
			EditorGUILayout.BeginVertical();
			if(GUILayout.Button("Up", GUILayout.Width(ButtonWidth))) {
				myTarget.MoveUp(i);
				break;
			}
			if(GUILayout.Button("Down", GUILayout.Width(ButtonWidth))) {
				myTarget.MoveDown(i);
				break;
			}
			EditorGUILayout.EndVertical();

			if(GUILayout.Button("-", GUILayout.Width(SmallButtonWidth))) {
				myTarget.RemoveFrame(i);
				break;
			}

			EditorGUILayout.EndHorizontal();
		}

		if(GUILayout.Button("+")) {
			myTarget.Add();
		}
	}
}

