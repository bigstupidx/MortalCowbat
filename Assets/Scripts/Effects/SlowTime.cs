using System;
using UnityEngine;
using System.Collections;

public class SlowTime : Effect
{
	[SerializeField]
	float duration;

	[SerializeField]
	float slowCoeficient = 0.025f;

	public override void Run(GameObject go)
	{
		base.Run(go);
		StartCoroutine(PlayCoroutine(go));
	}

	IEnumerator PlayCoroutine(GameObject go)
	{
		bool lerping = true;
		yield return StartCoroutine(Utils.LerpWithEase(1.0f, slowCoeficient, 0.2f, (value) => {
			Time.timeScale = value;
			lerping = false;
		},null));

		while(lerping)
			yield return 0;

		yield return new WaitForSecondsRealtime(duration);

		Time.timeScale = 1.0f;

		OnEvent("finished");
	}
}

