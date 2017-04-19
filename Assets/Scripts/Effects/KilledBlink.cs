using UnityEngine;
using System.Collections;

public class KilledBlink : Effect
{
	[SerializeField]
	float duration;

	public override void Run(GameObject go)
	{
		base.Run(go);
		StartCoroutine(PlayCoroutine(go));
	}

	IEnumerator PlayCoroutine(GameObject go)
	{
		float time = duration;

		int index = 0;
		while (time > 0) {
			Utils.SetColor(go, (index % 2 == 0) ? Color.white : new Color(1,1,1,0));
			index++;
			time -= Time.deltaTime;
			yield return new WaitForSeconds(0.1f);
		}
		if (go != null)
			Utils.SetColor(go,new Color(1,1,1,0));

		OnEvent("finished");
	}
}

