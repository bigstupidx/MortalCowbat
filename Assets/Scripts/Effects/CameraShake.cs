using UnityEngine;

public class CameraShake : Effect
{
	public float shakeDuration = 0f;
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void OnEnable()
	{
		originalPos = Camera.main.transform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			Camera.main.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			Camera.main.transform.localPosition = originalPos;
			OnEvent("finished");
		}
	}
}