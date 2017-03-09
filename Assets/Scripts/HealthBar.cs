using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	Transform fill;

	float fullScale;


	void Awake()
	{
		fullScale = fill.localScale.x;
	}

	public void Set(float value01)
	{
		fill.SetScaleX(fullScale * value01);
	}

	void LateUpdate()
	{
		//if (transform.lossyScale.x < 0) {
		//	transform.SetScaleX(transform.localScale.x * -1);
		//}
	}

}

