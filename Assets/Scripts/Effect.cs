using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
	public Action FinishAction { get; set; }

	[SerializeField]
	Animator animator;

	void Awake()
	{
		animator.SetTrigger(Defs.Animations.Play);
	}


	void OnEvent(string name)
	{
		if (name.Equals("finished")) {
			if (FinishAction != null) {
				FinishAction();
			}
			Destroy(gameObject);
		}
	}

}

