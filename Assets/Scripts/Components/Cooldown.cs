using UnityEngine;
using System;


public class Cooldown : MonoBehaviour
{
	public Action<float> OnProgress;

	[SerializeField]
	float time;

	float startTime;

	void Awake()
	{
		Restart();
	}

	public void Restart()
	{
		startTime = Time.time;	
	}

	public bool IsReady()
	{
		return float.Equals(GetProgress(), 1.0f);
	}

	void Update()
	{
		float progress = GetProgress();

		if (OnProgress != null) {
			OnProgress(progress);		
		}
	}

	float GetProgress()
	{
		return Mathf.Min(1.0f, (Time.time - startTime) / time);
	}
}

