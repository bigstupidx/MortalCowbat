using UnityEngine;
using System;
using System.Collections.Generic;


namespace Battle.Comp
{
	public class Jumping : CharacterComponent
	{
		[SerializeField]
		float jumpingSpeed;


		float jumpSpeedX;
		bool jumping;
		bool jumpStarted;

		public void Perform()
		{
			if (!jumping && !GetComp<Attacking>().IsAttacking()) {
				jumping = true;
				Debug.Log("Jumping: movingSpeedX " + GetComp<Moving>().SpeedX());
				jumpSpeedX = GetComp<Moving>().GetFlip() * jumpingSpeed;
				GetComp<Animating>().SetTrigger(Defs.Animations.Jump);
			}
		}

		public void Stop()
		{
			jumping = false;
			jumpStarted = false;
		}

		public bool HasJumpStarted()
		{
			return jumpStarted;
		}

		public void JumpStarted()
		{
			jumpStarted = true;
		}

		public bool IsJumping()
		{
			return jumping;
		}

		public void SetSpeedX (float speed)
		{
			jumpSpeedX = speed;
		}

		public override void UpdateMe()
		{
			if (jumping && jumpStarted) {
				var pos = transform.position;
				pos.x += jumpSpeedX * Time.deltaTime;
				transform.position = pos;
			}
		}
	}
}

