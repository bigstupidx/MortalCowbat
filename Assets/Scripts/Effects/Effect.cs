using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
	public string Id;
	public Action<Effect> FinishAction { get; set; }

	[SerializeField]
	Animator animator;

	GameObject runOn;

	public virtual void Run(GameObject go)
	{
		runOn = go;
	}

	public void OnEvent(string name)
	{
		if (name.Equals("finished")) {
			if (FinishAction != null) {
				FinishAction(this);
			}
			Destroy(gameObject);
		}
	}
}

