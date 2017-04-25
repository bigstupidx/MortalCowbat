using UnityEditor;
using UnityEngine;
using Ai;
using System.Collections.Generic;
using System;
using Ge;

class WaveWindow : EditorWindow 
{
	Wave wave;

	Vector2 scrollPos;

	List<Color> colors = new List<Color>() {
		new Color(1.0f,1.0f,1.0f,1.0f),
		new Color(1.0f,1.0f,1.0f,1.0f)
		//new Color(1.0f,1.0f,1.0f,1.0f)
		//new Color(1.0f,0.8f,0.8f,1.0f)
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
			bool removed = DrawEvent(wave.Events[i], 0, true);

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

	bool DrawEvent(Wave.Event evt, int space, bool timeEvent)
	{
		
		float labelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 40;
		bool removed = false;
		EditorGUILayout.BeginHorizontal();
		for (int i = 0; i < space; ++i)
			EditorGUILayout.Space();

		if (timeEvent) {
			float newTime = EditorGUILayout.FloatField(evt.Time,  GUILayout.Width(30));
			if (Math.Abs(newTime - evt.Time) > 0.01f) {
				evt.Time = newTime;
				Sort();
			}
		} else {
			EditorGUIUtility.labelWidth = 70;
			evt.Trigger = (Wave.Event.Condition)EditorGUILayout.EnumPopup ("Trigger", evt.Trigger);
			EditorGUIUtility.labelWidth = 40;
		}

		DrawSpawnData (evt);

		GUI.color = Color.red;
		if (GUILayout.Button("X")) {
			removed = true;
		}
		GUI.color = Color.white;

		DrawDuplicateEvent(evt);

		GUI.color = Color.green;

		DrawAddDependentEvent(evt);

		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
	

		for (int i = 0; i < evt.DependentEvents.Count; ++i) {
			if (DrawEvent(evt.DependentEvents[i], space + 1, false)) {
				evt.DependentEvents.RemoveAt(i);
				break;
			}
		}


		EditorGUIUtility.labelWidth = labelWidth;
		return removed;
	}

	static void DrawSpawnData (Wave.Event evt)
	{
		evt.SpawnData.NPCPrefab = EditorGUILayout.ObjectField (evt.SpawnData.NPCPrefab, typeof(GameObject), false) as GameObject;
		evt.SpawnData.AiPreset = EditorGUILayout.ObjectField (evt.SpawnData.AiPreset, typeof(AiPreset), false) as AiPreset;
		evt.SpawnData.HP = EditorGUILayout.IntField ("HP", evt.SpawnData.HP, GUILayout.Width (80));
		evt.SpawnData.Speed = EditorGUILayout.IntField ("Speed", evt.SpawnData.Speed, GUILayout.Width (80));
		evt.SpawnData.Dir = EditorGUILayout.IntField ("Side", evt.SpawnData.Dir, GUILayout.Width (80));
		evt.SpawnData.Delay = EditorGUILayout.FloatField ("Delay", evt.SpawnData.Delay, GUILayout.Width (80));
	}

//	bool DrawNPCEvent(Wave.NPCEvent npcEvent)
//	{
//		float labelWidth = EditorGUIUtility.labelWidth;
//		EditorGUIUtility.labelWidth = 50;
//		bool removed = false;
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.Space();
//		npcEvent.Trigger = (Wave.NPCEvent.Condition)EditorGUILayout.EnumPopup ("Trigger", npcEvent.Trigger, GUILayout.Width (150));
//
//		npcEvent.NPCPrefab = EditorGUILayout.ObjectField(npcEvent.NPCPrefab, typeof(GameObject), false) as GameObject;
//		npcEvent.AiPreset = EditorGUILayout.ObjectField(npcEvent.AiPreset, typeof(AiPreset), false) as AiPreset;
//		npcEvent.HP = EditorGUILayout.IntField("HP", npcEvent.HP, GUILayout.Width(80));
//		npcEvent.Speed = EditorGUILayout.IntField("Speed",npcEvent.Speed, GUILayout.Width(80));
//		npcEvent.Dir = EditorGUILayout.IntField("Side", npcEvent.Dir, GUILayout.Width(80));
//
//		GUI.color = Color.red;
//		if (GUILayout.Button("X")) {
//			removed = true;
//		}
//		GUI.color = Color.white;
//
//		EditorGUILayout.EndHorizontal();
//		EditorGUIUtility.labelWidth = labelWidth;
//		return removed;
//	}

	void DrawAddNewEvent()
	{	var color = GUI.color;
		GUI.color = Color.green;
		if (GUILayout.Button("+")) {
			wave.Events.Add(new Wave.Event());
		}
		GUI.color = color;
	}

	void DrawAddDependentEvent(Wave.Event evt)
	{	var color = GUI.color;
		GUI.color = Color.green;
		if (GUILayout.Button("+")) {
			evt.DependentEvents.Add(new Wave.Event());
		}
		GUI.color = color;
	}

	void DrawDuplicateEvent(Wave.Event evt)
	{	var color = GUI.color;
		GUI.color = Color.cyan;
		if (GUILayout.Button("D")) {
			wave.Events.Add(evt.Clone());
		}
		GUI.color = color;
	}

	void Sort()
	{
		wave.Events.Sort((x, y) => {
			var compare = x.Time.CompareTo (y.Time);
			return compare != 0 ? compare : x.GetHashCode ().CompareTo (y.GetHashCode ());
		});
	}
}