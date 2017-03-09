using UnityEngine;
using System;

public partial class Character : MonoBehaviour
{
	public Action<Character> AttackAction;
	public CharacterSettings Settings { get { return settings; }}
	public Defs.HDirection HDirection { get { return hDirection; }}

	[SerializeField]
	AudioSource audioSource;

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
	bool attacking;
	bool specialAttacking;

	void Awake()
	{
		SetState(Defs.State.Idle);
	}

	public void MoveHorizontally(Defs.HDirection dir)
	{
		if (!specialAttacking) {
			SetState(Defs.State.Moving);
			SetHorizontalDirection(dir);
			SetHorizontalSpeed(movingSpeed);
		}
	}

	public void MoveVertically(Defs.VDirection dir)
	{
		if (!specialAttacking) {
			SetState(Defs.State.Moving);
			SetVerticalDirection(dir);
			SetVerticalSpeed(movingSpeed);
		}
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

	public void Attack()
	{
		if (!Atacking()) {
			animator.SetTrigger(Defs.Animations.Attack);	
			attacking = true;
			audioSource.clip = settings.AttackSfx;
			audioSource.Play();
			AttackAction(this);
		}
	}

	public void SpecialAttack()
	{
		if (!Atacking()) {
			StopMovingVertically();
			StopMovingHorizontally();

			animator.SetTrigger(Defs.Animations.SpecialAttack);	
			specialAttacking = true;
			audioSource.clip = settings.AttackSfx;
			audioSource.Play();
			if (settings.SpecialAttackEffect != null) {
				CreatEffect(settings.SpecialAttackEffect,"SpecialAttackEffect");
			}
			AttackAction(this);
		}
	}


	public void Hit(Defs.HDirection dir)
	{
		transform.AddPositionX((int)dir * 1.0f);
		audioSource.clip = settings.HitSfx;
		audioSource.Play();
	}

	void SetHorizontalDirection(Defs.HDirection dir)
	{
		//spriteRen.flipX = dir == Defs.HDirection.Left;
		transform.SetScaleX((int)dir);
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
		if (name.Equals(Defs.Events.AttackFinished)) {
			attacking = false;
		}
		else if (name.Equals(Defs.Events.SpecialAttackFinished)) {
			specialAttacking = false;
		}
	}

	void CreatEffect(GameObject prefab, string containerName)
	{
		var effect = Instantiate(prefab);
		effect.transform.SetParent(transform.FindChild(containerName));
		effect.transform.localPosition = Vector3.zero;
		effect.transform.localScale = Vector3.one;
	}


	bool Atacking()
	{
		return specialAttacking || attacking;
	}
}
