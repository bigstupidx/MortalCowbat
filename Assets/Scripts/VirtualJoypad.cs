using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class VirtualJoypad : MonoBehaviour
{
	[SerializeField]
	Transform innerCircle;

	bool dragging;
	Vector2 dir;
	float distance;
	int angle;

	const float maxInnerShift = 70;

	public Vector2 Direction()
	{
		return dir;
	}

	public bool LeftPressed()
	{
		return dragging && (angle > 120 && angle < 240);
	}

	public bool RightPressed()
	{
		return dragging && ( (angle > 0 && angle < 60) || (angle > 300 && angle < 360));
	}

	public bool UpPressed()
	{
		return dragging && (angle > 30 && angle < 150);
	}

	public bool DownPressed()
	{
		return dragging && (angle > 210 && angle < 330);
	}

	public void OnBeginDrag(BaseEventData data)
	{
		dir = GetDirection(data);
		angle = GetAngle(dir);
		dragging = true;
	}

	public void OnDrag(BaseEventData data)
	{
		dir = GetDirection(data);
		angle = GetAngle(dir);
	}


	public void OnEndDrag(BaseEventData data)
	{
		dir = GetDirection(data);
		angle = GetAngle(dir);
		dragging = false;
		dir = Vector2.zero;
		distance = 0.0f;
	}

	Vector2 GetDirection(BaseEventData data)
	{
		var globalTouchScreenPos = ((PointerEventData)data).position;
		var dir = globalTouchScreenPos - GlobalJoyPadCenterScreenPosition();
		distance = dir.magnitude;
		return dir.normalized;
	}

	int GetAngle(Vector2 dir)
	{
		return 	2 * (int)Quaternion.FromToRotation(Vector3.up, new Vector3(dir.x, dir.y, 0) - Vector3.right).eulerAngles.z;
	}

	Vector2 GlobalJoyPadCenterScreenPosition()
	{
		return RectTransformUtility.WorldToScreenPoint(null, transform.position);
	}

	void Update()
	{
		innerCircle.localPosition = Math.Min(maxInnerShift, distance)* dir;
	}


}

