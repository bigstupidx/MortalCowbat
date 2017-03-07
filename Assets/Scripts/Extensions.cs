using System;
using UnityEngine;

public static class Extensions
{
	public static void SetPositionX(this Transform tr, float x)
	{
		var pos = tr.position;
		pos.x = x;
		tr.position = pos;
	}

	public static void SetPositionY(this Transform tr, float y)
	{
		var pos = tr.position;
		pos.y = y;
		tr.position = pos;
	}
}
