﻿using UnityEngine;
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

		public void SetFloat(string name, float value)
		{
			animator.SetFloat(name, value);
		}

		public override void UpdateMe()
		{
			var speedX = GetComp<Moving>().SpeedX();
			var speedY = GetComp<Moving>().SpeedY();
			animator.SetFloat("speed", (float)Math.Sqrt(speedX * speedX + speedY * speedY));
		}

		public float GetAnimatorSpeed()
		{
			return animator.speed;
		}

		public void SetAnimatorSpeed(float speed)
		{
			animator.speed = speed;
		}
	}
}

