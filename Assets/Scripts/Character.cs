using UnityEngine;
using System;
using Ai;

public partial class Character : MonoBehaviour
{
	public Action<Character, Attack> AttackAction;
	public Action<Character, Attack> SpecialAttackAction;
	public Action<float, float> HealthChangedAction;
	public Action<Character> DeathAction;

	public Defs.CharacterType Type;
	public Attack BasicAttack { get { return baseAttack; }}
	public Attack SpecialAttack { get { return specialAttack; }}
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

	Defs.State currentState;

	public void Init(CharacterContext context)
	{
		this.context = context;
		SetState(Defs.State.Idle);
		SetHealth(settings.Health);
	}

	public void Move(Vector2 dir)
	{
		SetState(Defs.State.Moving);
		SetHorizontalDirection(dir.x > 0 ? Defs.HDirection.Right : Defs.HDirection.Left);
		SetHorizontalSpeed(Math.Abs(dir.x) * settings.MovingSpeed);

		SetVerticalDirection(dir.y > 0 ? Defs.VDirection.Up : Defs.VDirection.Down);
		SetVerticalSpeed(Math.Abs(dir.y) * settings.MovingSpeed);
	}

	public void StopMoving()
	{
		SetHorizontalSpeed(0);
		SetVerticalSpeed(0);
		SetState(Defs.State.Idle);
	}

	public void FaceTo(Vector3 pos)
	{
		SetHorizontalDirection(pos.x > Position.x ? Defs.HDirection.Right : Defs.HDirection.Left);
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

	public void Idle()
	{
		StopMoving();
		SetState(Defs.State.Idle);
	}


	public void PerformSpecialAttack()
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

		bool alive = SetHealth(actualHealth - attack.AttackPoints);
	
		if (alive) {
			PlayAnimation(Defs.Animations.Hit);
		} else {
			dying = true;
			PlayAnimation(Defs.Animations.Die);
			Destroy(GetComponent<AiStateMachine>());
			if (DeathAction != null) {
				DeathAction(this);
			}
		}
	}


	public float GetBasicAttackRange()
	{
		var circleColl =  baseAttack.Colliders.Find(x=> x is CircleCollider2D) as CircleCollider2D;
		return circleColl.radius + circleColl.offset.magnitude;
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
		if (currentState != state) {
			if (state == Defs.State.Idle) {
				PlayAnimation(Defs.Animations.Idle);
			}
			else if (state == Defs.State.Moving) {
				PlayAnimation(Defs.Animations.Walk);
			}
		}
		currentState = state;
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

		UpdateSortingOrder();
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

	bool SetHealth(int health)
	{
		actualHealth = Math.Max(0, health);
		if (HealthChangedAction != null) {
			HealthChangedAction(actualHealth, settings.Health);
		}

		return actualHealth > 0;
	}

	public bool CanAttack()
	{
		return !IsAttacking() && !IsDying();
	}

	public bool CanMove()
	{
		return !IsDying() && !specialAttacking;
	}

	public bool IsAttacking()
	{
		return specialAttacking || attacking;
	}

	bool IsDying()
	{
		return dying;
	}

	void UpdateSortingOrder()
	{
		const float maxY = 10;
		const float minY = -10;
		const int minSortingOrder = 10;
		const int maxSortingOrder = 100;

		float c = (Position.y - minY) / (maxY - minY);
		spriteRen.sortingOrder = minSortingOrder + (int)((maxSortingOrder - minSortingOrder) * (1 - c));
	}

}
