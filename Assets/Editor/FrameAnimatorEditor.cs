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
		var myTarget = (FrameAnimator)target;

		showDefaultInspector = EditorGUILayout.Toggle("Advanced", showDefaultInspector);
		if (showDefaultInspector) {
			base.OnInspectorGUI();
		}

        myTarget.sprRenderer = (SpriteRenderer)EditorGUILayout.ObjectField(myTarget.sprRenderer, typeof(SpriteRenderer), true);
        EditorGUILayout.Space();
        var color = GUI.color;
        for (int i = 0; i < myTarget.Sprites.Count; ++i) {
            GUI.color = myTarget.SpriteIndex == i ? Color.green : Color.white;
           

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
        GUI.color = color;

		if(GUILayout.Button("+")) {
			myTarget.Add();
		}

        if (GUI.changed) {
            EditorUtility.SetDirty(myTarget);
        }

	}
}

