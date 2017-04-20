using UnityEngine;
using System;

namespace Battle.Comp
{
	public class Moving : CharacterComponent
	{
		public bool blockIntersections;

		[SerializeField]
		float blockIntersectionsDistance = 0.5f;


		public bool Paused { get; set; }
		public bool Falling { get { return falling; }}
		public Collider2D Collider { get; private set; }

		float speedX;
		float speedY;
		bool falling;

		void Awake()
		{
			Collider = GetComponent<BoxCollider2D>();
		}

		public float SpeedX()
		{
			return speedX;
		}

		public float SpeedY()
		{
			return speedY;
		}

		public void SetSpeed(float speedX, float speedY)
		{
			SetSpeedX(speedX);
			SetSpeedY(speedY);
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
			if (!falling) {
				var pos = transform.position;
				pos.x += speedX * Time.deltaTime;
				pos.y += speedY * Time.deltaTime;

				bool canMove = !blockIntersections || CanMoveToPosition (pos);

				if (canMove) {
					transform.position = pos;
				}
			}
		}

		bool CanMoveToPosition (Vector3 pos)
		{
			var thisCharacterNextPivotPos = GetCharacter ().GetComp<Visual>().GetPoi("Pivot").position + (pos - GetCharacter ().GetPosition());
			var thisCharacterCurrentPivotPos = GetCharacter ().GetComp<Visual>().GetPoi("Pivot").position;

			bool canMove = true;
			for (int i = 0; i < GetCharacter ().Context.Characters.Count; ++i) {
				var otherCharacter = GetCharacter ().Context.Characters[i];
				if (otherCharacter != GetCharacter ()) {

					if (otherCharacter.GetComp<Moving>().Falling || otherCharacter.GetComp<Death>().IsDying)
						continue;

					var otherCharacterPivotPos = otherCharacter.GetComp<Visual>().GetPoi("Pivot").position;
					float nextDistance = (thisCharacterNextPivotPos - otherCharacterPivotPos).magnitude;
					float nextDistanceX = Mathf.Abs(thisCharacterNextPivotPos.x - otherCharacterPivotPos.x);
					float nextDistanceY = Mathf.Abs(thisCharacterNextPivotPos.y - otherCharacterPivotPos.y);
					float currentDistance = (thisCharacterCurrentPivotPos - otherCharacterPivotPos).magnitude;

					bool isTooClose = nextDistanceX < blockIntersectionsDistance && (nextDistanceY < blockIntersectionsDistance * 0.2f);

					bool movingOut = nextDistance > currentDistance;

					if (isTooClose && !movingOut)
						canMove = false;
				}
			}
			return canMove;
		}
	}
}

