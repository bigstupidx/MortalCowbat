using System;

public static class EdUtils
{
	public static string TrimProjectPath(string fullPath)
	{
		int index = fullPath.IndexOf("Assets");
		if (index != -1) {
			return fullPath.Remove(0, index);
		}
		return fullPath;
	}

}

