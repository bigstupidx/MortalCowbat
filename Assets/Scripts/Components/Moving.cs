using UnityEngine;
using System;

namespace Battle.Comp
{
	public class Moving : CharacterComponent
	{
		public bool Paused { get; set; }
		public bool Falling { get { return falling; }}

		float speedX;
		float speedY;
		bool falling;

		public float SpeedX()
		{
			return speedX;
		}

		public float SpeedY()
		{
			return speedY;
		}

		public void SetSpeedX(float speed)
		{
			speedX = speed;
		}

		public void SetSpeedY(float speed)
		{
			speedY = speed;
		}

		public void Stop()
		{
			SetSpeedX(0);
			SetSpeedY(0);
		}

		public bool IsMoving()
		{
			return SpeedX() > 0 || SpeedY() > 0.0f;
		}

		public void Fall ()
		{
			GetComp<Animating>().SetTrigger(Defs.Animations.Fall);
			falling = true;
		}

		public void FinishFall()
		{
			falling = false;
		}

		public void FaceTo (Vector3 position)
		{
			Flip(position.x > transform.position.x ? 1  : -1);
		}

		public int GetFlip()
		{
			return transform.localScale.x > 0 ? 1 : -1;
		}

		public void Flip(int dir)
		{
			var scale = transform.localScale;
			scale.x = dir * Math.Abs(scale.x);
			transform.localScale = scale;
		}

		public override void UpdateMe()
		{
			var pos = transform.position;
			pos.x += speedX * Time.deltaTime;
			pos.y += speedY * Time.deltaTime;
			transform.position = pos;
		}
	}
}

