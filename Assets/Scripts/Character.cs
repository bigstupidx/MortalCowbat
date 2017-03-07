using UnityEngine;
using System;

public class Character : MonoBehaviour
{
	public Action<Character> AttackAction;
	public CharacterSettings Settings { get { return settings; }}

	[SerializeField]
	CharacterSettings settings;

	[SerializeField]
	SpriteRenderer spriteRen;

	[SerializeField]
	Animator animator;

	[SerializeField]
	float movingSpeed;

	Defs.HDirection hDirection;
	Defs.VDirection vDirection;

	Vector3 speed;
	bool punching;

	void Awake()
	{
		SetState(Defs.State.Idle);
	}

	public void MoveHorizontally(Defs.HDirection dir)
	{
		SetState(Defs.State.Moving);
		SetHorizontalDirection(dir);
		SetHorizontalSpeed(movingSpeed);
	}

	public void MoveVertically(Defs.VDirection dir)
	{
		SetState(Defs.State.Moving);
		SetVerticalDirection(dir);
		SetVerticalSpeed(movingSpeed);
	}

	public void StopMovingHorizontally()
	{
		SetHorizontalSpeed(0);
		if (!IsMoving())
			SetState(Defs.State.Idle);
	}

	public void StopMovingVertically()
	{
		SetVerticalSpeed(0);
		if (!IsMoving())
			SetState(Defs.State.Idle);
	}

	public void Punch()
	{
		if (!punching) {
			animator.SetTrigger("punch");	
			punching = true;
			AttackAction(this);
		}
	}

	public void Hit()
	{
		transform.AddPositionX(-(int)hDirection * 1.0f);
	}

	void SetHorizontalDirection(Defs.HDirection dir)
	{
		spriteRen.flipX = dir == Defs.HDirection.Left;
		hDirection = dir;
	}

	void SetVerticalDirection(Defs.VDirection dir)
	{
		vDirection = dir;
	}

	void SetState(Defs.State state)
	{
		if (state == Defs.State.Idle) {
			PlayAnimation(Defs.Animations.Idle);
		}
		else if (state == Defs.State.Moving) {
			PlayAnimation(Defs.Animations.Walk);
		}
	}

	void SetHorizontalSpeed(float speed)
	{
		this.speed.x = speed;
	}

	void SetVerticalSpeed(float speed)
	{
		this.speed.y = speed;
	}

	void PlayAnimation(string animName)
	{
		animator.SetTrigger(animName);
	}

	bool IsMoving()
	{
		return speed.sqrMagnitude > 0;
	}


	void Update()
	{
		transform.position += new Vector3(
			speed.x * (int)hDirection, 
			speed.y * (int)vDirection, 
			0) * movingSpeed;
	}

	void AnimationEvent(string name)
	{
		if (name.Equals(Defs.Events.PunchFinshed)) {
			punching = false;
		}
	}

}
