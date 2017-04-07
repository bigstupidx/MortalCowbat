using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
	public Action FinishAction { get; set; }

	[SerializeField]
	Animator animator;

	GameObject runOn;


	public virtual void Run(GameObject go)
	{
		runOn = go;
	}

	protected void OnEvent(string name)
	{
		if (name.Equals("finished")) {
			if (FinishAction != null) {
				FinishAction();
			}
			Destroy(gameObject);
		}
	}
}

