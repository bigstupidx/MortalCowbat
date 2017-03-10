using UnityEngine;
using System;

public partial class Character : MonoBehaviour
{
	public Action<Character, Attack> AttackAction;
	public Action<Character, Attack> SpecialAttackAction;
	public Action<float, float> HealthChangedAction;
	public Action<Character> DeathAction;

	public CharacterSettings Settings { get { return settings; }}
	public Defs.HDirection HDirection { get { return hDirection; }}
	public Vector3 Position { get { return transform.position; }}

	[SerializeField]
	Attack specialAttack;

	[SerializeField]
	Attack baseAttack;

	[SerializeField]
	AudioSource audioSource;

	[SerializeField]
	CharacterSettings settings;

	[SerializeField]
	SpriteRenderer spriteRen;

	[SerializeField]
	Animator animator;


	CharacterContext context;
	Defs.HDirection hDirection;
	Defs.VDirection vDirection;

	Vector3 speed;
	bool attacking;
	bool dying;
	bool specialAttacking;
	int actualHealth;


	public void Init(CharacterContext context)
	{
		this.context = context;
		SetState(Defs.State.Idle);
		SetHealth(settings.Health);
	}

	public void MoveHorizontally(Defs.HDirection dir)
	{
		if (CanMove()) {
			SetState(Defs.State.Moving);
			SetHorizontalDirection(dir);
			SetHorizontalSpeed(settings.MovingSpeed);
		}
	}

	public void MoveVertically(Defs.VDirection dir)
	{
		if (CanMove()) {
			SetState(Defs.State.Moving);
			SetVerticalDirection(dir);
			SetVerticalSpeed(settings.MovingSpeed);
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
		if (CanAttack()) {
			attacking = true;
			animator.SetTrigger(Defs.Animations.Attack);	
			audioSource.clip = baseAttack.Sfx;
			audioSource.Play();
			AttackAction(this, baseAttack);
		}
	}

	public void SpecialAttack()
	{
		if (CanAttack()) {
			specialAttacking = true;
			StopMovingVertically();
			StopMovingHorizontally();
			PlayAnimation(Defs.Animations.SpecialAttack);
			StartAttack(specialAttack);
			SpecialAttackAction(this, specialAttack);
		}
	}


	public void Hit(Attack attack, Defs.HDirection dir)
	{
		if (attack.ShiftHitEnemy) {
			transform.AddPositionX((int)dir * 1.0f);
		}
		audioSource.clip = settings.HitSfx;
		audioSource.Play();

		SetHealth(actualHealth - attack.AttackPoints);
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
			0) * settings.MovingSpeed;
	}

	void AnimationEvent(string name)
	{
		if (name.Equals(Defs.Events.AttackFinished)) {
			attacking = false;
		}
		else if (name.Equals(Defs.Events.SpecialAttackFinished)) {
			specialAttacking = false;
		}
		else if (name.Equals(Defs.Events.DieFinished)) {
			if (DeathAction != null) {
				DeathAction(this);
			}
			Destroy(gameObject);
		}
	}

	void StartAttack(Attack attack)
	{
		audioSource.clip = attack.Sfx;
		audioSource.Play();

		for (int i = 0; i < attack.Effects.Count; ++i) {
			context.EffectManager.CreateEffect(
				attack.Effects[i].Effect.gameObject, 
				transform.Find(attack.Effects[i].Container));
		}
	}

	void SetHealth(int health)
	{
		actualHealth = Math.Max(0, health);
		if (HealthChangedAction != null) {
			HealthChangedAction(actualHealth, settings.Health);
		}

		if (actualHealth == 0) {
			dying = true;
			PlayAnimation(Defs.Animations.Die);
		}
	}

	bool CanAttack()
	{
		return !IsAttacking() && !IsDying();
	}

	bool CanMove()
	{
		return !IsDying() && !specialAttacking;
	}

	bool IsAttacking()
	{
		return specialAttacking || attacking;
	}

	bool IsDying()
	{
		return dying;
	}

}
