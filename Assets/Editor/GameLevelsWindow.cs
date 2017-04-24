using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Ge;


class GameLevelsWindow : EditorWindow 
{
	Vector2 scrollPos;
	GameLevels gameLevels;

	List<Color> Colors = new List<Color>() {
		new Color(1.0f,0.8f,0.8f,1.0f),
		new Color(1.0f,1.0f,1.0f,1.0f)
	};

	public static void Open(GameLevels gameLevels) 
	{
		var window = EditorWindow.GetWindow(typeof(GameLevelsWindow)) as GameLevelsWindow;
		window.Init(gameLevels);
	}

	public void Init(GameLevels gameLevels)
	{
		this.titleContent = new GUIContent(gameLevels.name);
		this.gameLevels = gameLevels;
	}

	void OnGUI () 
	{
		DrawLevels();
	}

	void DrawLevels()
	{

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		EditorGUILayout.BeginHorizontal();
		bool levelsModified = false;
		for (int i = 0; i < gameLevels.Levels.Count; ++i) {
			EditorGUILayout.BeginVertical();
			var level = gameLevels.Levels[i];

			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.yellow;
			gameLevels.Levels[i] = (Level)EditorGUILayout.ObjectField (level, typeof(Level), true, GUILayout.Width (150));
			GUI.color = Color.white;
			levelsModified = DrawRemoveLevelButton(gameLevels.Levels, i);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			bool modified = false;

			if (level != null) {
				modified = DrawWaves(level.Waves);
				EditorGUILayout.Space();
				modified |= DrawAddNewWave(level.Waves);
			}

			EditorGUILayout.EndVertical();
			if (modified) {
				EditorUtility.SetDirty(gameLevels.Levels[i]);
			}
			if (levelsModified)
				break;
		}

		levelsModified |= DrawAddLevelButton(gameLevels.Levels);
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
	
		if (GUI.changed || levelsModified) {
			EditorUtility.SetDirty(gameLevels);
		}
	}

	bool DrawWaves(List<Wave> waves)
	{
		for (int i = 0; i < waves.Count; ++i ){
			EditorGUILayout.BeginHorizontal();
			waves[i] = (Wave)EditorGUILayout.ObjectField (waves [i], typeof(Wave), false, GUILayout.Width (150));
			bool modified = false;

			DrawOpenWaveButton(waves, i);

			if (DrawRemoveWaveButton(waves, i))
				modified = true;
			EditorGUILayout.EndHorizontal();
			if (modified)
				return true;
		}
		return false;
	}

	bool DrawAddNewWave(List<Wave> waves)
	{	
		var color = GUI.color;
		GUI.color = Color.green;
		bool modified = false;
		if (GUILayout.Button("+", GUILayout.Width(50))) {
			waves.Add(null);
			modified = true;
		}
		GUI.color = color;
		return modified;
	}

	bool DrawRemoveWaveButton(List<Wave> waves, int index)
	{	var color = GUI.color;
		GUI.color = Color.red;
		bool modified = false;
		if (GUILayout.Button("X", GUILayout.Width(50))) {
			waves.RemoveAt(index);
			modified = true;
		}
		GUI.color = color;
		return modified;
	}

	void DrawOpenWaveButton(List<Wave> waves, int index)
	{	var color = GUI.color;
		GUI.color = Color.white;

		if (GUILayout.Button("O", GUILayout.Width(50))) {
			WaveWindow.Open(waves[index]);
		}
		GUI.color = color;
	}

	bool DrawRemoveLevelButton(List<Level> levels, int index)
	{	var color = GUI.color;
		GUI.color = Color.red;
		bool modified = false;
		if (GUILayout.Button("X", GUILayout.Width(50))) {
			levels.RemoveAt(index);
			modified = true;
		}
		GUI.color = color;
		return modified;
	}

	bool DrawAddLevelButton (List<Level> levels)
	{
		var color = GUI.color;
		GUI.color = Color.green;
		bool modified = false;
		if (GUILayout.Button("+", GUILayout.Width(50))) {
			levels.Add(null);
			modified = true;
		}
		GUI.color = color;
		return modified;
	}
}