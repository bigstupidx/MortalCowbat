using UnityEngine;
using UnityEditor;

public static class EdUtils
{
	public static string TrimProjectPath(string fullPath)
	{
		int index = fullPath.IndexOf ("Assets", System.StringComparison.Ordinal);
		if (index != -1) {
			return fullPath.Remove(0, index);
		}
		return fullPath;
	}

	public static T CreateAndSave<T>(string path) where T : ScriptableObject
	{
		var instance = ScriptableObject.CreateInstance<T>();
		AssetDatabase.CreateAsset(instance, path);
		AssetDatabase.SaveAssets();
		return instance;
	}
}

