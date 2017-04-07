using UnityEngine;
using System;

namespace Battle.Comp
{
	public class Animating : CharacterComponent
	{
		Animator animator;

		void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void SetTrigger(String trigger)
		{
			animator.SetTrigger(trigger);
		}

		public override void UpdateMe()
		{
			var speedX = GetComp<Moving>().SpeedX();
			var speedY = GetComp<Moving>().SpeedY();
			animator.SetFloat("speed", (float)Math.Sqrt(speedX * speedX + speedY * speedY));
		}
	}
}

