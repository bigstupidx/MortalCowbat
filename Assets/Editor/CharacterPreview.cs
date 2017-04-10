using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public static class CharacterPreview
{

	public static void PreviewCharactersInCurrentScene()
	{
		ClearScene();
		var prefabs = LoadCharacterPrefabs();
		var instances = MakeInstances(prefabs);
		PlaceToPositions(instances);
	}


	public static List<GameObject> LoadCharacterPrefabs()
	{
		var prefabs = new List<GameObject>();
		var characterPrefabsPath = Directory.GetFiles(Path.Combine(Application.dataPath,"Prefabs/Characters")).ToList();
		characterPrefabsPath.RemoveAll(x=>x.EndsWith(".meta",StringComparison.Ordinal));

		foreach(var fullPath in characterPrefabsPath)
		{
			var assetPath = EdUtils.TrimProjectPath(fullPath);
			var prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
			prefabs.Add(prefab);
		}
		return prefabs;
	}

	public static List<GameObject> MakeInstances(List<GameObject> prefabs)
	{
		var instances = new List<GameObject>();
		for (int i = 0; i < prefabs.Count; ++i) {
			instances.Add(PrefabUtility.InstantiatePrefab(prefabs[i]) as GameObject);
		}
		return instances;
	}

	public static void PlaceToPositions(List<GameObject> instances)
	{
		Vector3 pos = Vector3.zero;
		const float stepX = 3;

		for (int i = 0; i < instances.Count; ++i) {
			instances[i].transform.position = pos;
			pos.x += stepX;
		}
	}

	public static void ClearScene()
	{
		var transforms = GameObject.FindObjectsOfType<Transform>();
		for (int i = 0; i < transforms.Length; ++i) {
			GameObject.DestroyImmediate(transforms[i].gameObject);
		}
	}
}

