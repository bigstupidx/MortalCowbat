using System;
using UnityEngine;

public static class Extensions
{
	public static void AddPositionX(this Transform tr, float x)
	{
		var pos = tr.position;
		pos.x += x;
		tr.position = pos;
	}

	public static void AddPositionY(this Transform tr, float y)
	{
		var pos = tr.position;
		pos.y += y;
		tr.position = pos;
	}


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

	public static void SetScaleX(this Transform tr, float x)
	{
		var scale = tr.localScale;
		scale.x = x;
		tr.localScale = scale;
	}

	public static void SetScaleY(this Transform tr, float y)
	{
		var scale = tr.localScale;
		scale.y = y;
		tr.localScale = scale;
	}
}
