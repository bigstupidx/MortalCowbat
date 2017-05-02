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
		if (gameLevels == null) {
			Close();
			return;
		}

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		EditorGUILayout.BeginHorizontal();
		bool levelsModified = false;
		for (int i = 0; i < gameLevels.Levels.Count; ++i) {
			var level = gameLevels.Levels[i];
			if (level == null)
				continue;

			EditorGUILayout.BeginVertical();
			DrawEnvironment(level);

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

	void DrawEnvironment(Level level)
	{
		float labelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 40;
		EditorGUILayout.BeginVertical();
		int envIndex = Defs.GetEnvIndex(level.Environment);
		if (envIndex == -1)
			envIndex = 0;
		level.Environment = EditorGUILayout.EnumPopup("Env", (Defs.Env)envIndex,  GUILayout.Width (130)).ToString();
		level.Frame = EditorGUILayout.IntField("Frame", level.Frame,  GUILayout.Width (130));
		EditorGUILayout.EndVertical();
		EditorGUIUtility.labelWidth = labelWidth;

		if (GUI.changed) {
			EditorUtility.SetDirty(level);
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

			var path = EditorUtility.SaveFilePanel(
				"Save Wave",
				"Assets/Settings",
				"Wave.asset",
				"asset");

			if (path.Length != 0) {
				path = EdUtils.TrimProjectPath(path);
				waves.Add(EdUtils.CreateAndSave<Wave>(path));
				modified = true;
			}
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

			var path = EditorUtility.SaveFilePanel(
				"Save Level",
				"Assets/Settings",
				"Level" + levels.Count + ".asset",
				"asset");

			if (path.Length != 0) {
				path = EdUtils.TrimProjectPath(path);
				levels.Add(EdUtils.CreateAndSave<Level>(path));
				modified = true;
			}
		}
		GUI.color = color;
		return modified;
	}
}