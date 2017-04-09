using UnityEditor;
using UnityEngine;
using Ai;
using System.Collections.Generic;

class WaveWindow : EditorWindow 
{
	Wave wave;

	Vector2 scrollPos;

	List<Color> colors = new List<Color>() {
		new Color(1.0f,0.8f,0.8f,1.0f),
		new Color(1.0f,1.0f,1.0f,1.0f)
	};

	public static void Open(Wave wave) 
	{
		var window = EditorWindow.GetWindow(typeof(WaveWindow)) as WaveWindow;
		window.Init(wave);
	}

	public void Init(Wave wave)
	{
		this.wave = wave;
		this.titleContent = new GUIContent(wave.name);
	}

	void OnGUI () 
	{
		DrawAddNewEvent();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		for (int i = 0; i < wave.Events.Count; ++i) {
			bool removed = DrawWaveEvent(wave.Events[i]);

		if (removed) {
			wave.Events.RemoveAt(i);
			break;
		}
			
		GUI.color = colors[i % 2];
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndScrollView();

		if (GUI.changed) {
			EditorUtility.SetDirty(wave);
		}
	}

	bool DrawWaveEvent(Wave.TimeEvent evt)
	{
		bool removed = false;

		EditorGUILayout.BeginHorizontal();
		evt.Time = EditorGUILayout.FloatField(evt.Time,  GUILayout.Width(30));

		if (GUI.changed) {
			Sort();
		}

		evt.NPCPrefab = EditorGUILayout.ObjectField(evt.NPCPrefab, typeof(GameObject), false) as GameObject;
		evt.AiPreset = EditorGUILayout.ObjectField(evt.AiPreset, typeof(AiPreset), false) as AiPreset;
		evt.Dir = EditorGUILayout.IntField(evt.Dir, GUILayout.Width(25));


		GUI.color = Color.red;
		if (GUILayout.Button("X")) {
			removed = true;
		}

		DrawDuplicateEvent(evt);

		EditorGUILayout.EndHorizontal();
		return removed;
	}

	void DrawAddNewEvent()
	{	var color = GUI.color;
		GUI.color = Color.green;
		if (GUILayout.Button("+")) {
			wave.Events.Add(new Wave.TimeEvent());
		}
		GUI.color = color;
	}

	void DrawDuplicateEvent(Wave.TimeEvent evt)
	{	var color = GUI.color;
		GUI.color = Color.cyan;
		if (GUILayout.Button("D")) {
			wave.Events.Add(new Wave.TimeEvent(evt));
		}
		GUI.color = color;
	}

	void Sort()
	{
		wave.Events.Sort((x, y) => x.Time.CompareTo (y.Time));
	}
}