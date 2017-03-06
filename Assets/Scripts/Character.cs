using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer spriteRen;

	[SerializeField]
	Animator animator;

	[SerializeField]
	float movingSpeed;


	Defs.Direction direction;
	float speed;
	bool punching;

	void Awake()
	{
		SetState(Defs.State.Idle);
	}

	public void Move(Defs.Direction dir)
	{
		SetDirection(dir);
		SetState(Defs.State.Moving);
		SetSpeed(movingSpeed);
	}


	public void StopMoving()
	{
		SetSpeed(0);
		SetState(Defs.State.Idle);
	}

	public void Punch()
	{
		if (!punching) {
			animator.SetTrigger("punch");	
			punching = true;
		}
	}

	void SetDirection(Defs.Direction dir)
	{
		spriteRen.flipX = dir == Defs.Direction.Left;
		direction = dir;
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

	void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	void PlayAnimation(string animName)
	{
		animator.SetTrigger(animName);
	}

	void Update()
	{
		transform.position += new Vector3(speed * (int)direction, 0, 0 ) * movingSpeed;
	}

	void AnimationEvent(string name)
	{
		if (name.Equals(Defs.Events.PunchFinshed)) {
			punching = false;
		}
	}

}
