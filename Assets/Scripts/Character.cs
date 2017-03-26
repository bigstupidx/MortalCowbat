using UnityEngine;
using System;
using Ai;
using System.Collections;
using System.Collections.Generic;
using Vis;


public partial class Character : MonoBehaviour
{
	public Action<Character, Attack, float> AttackAction;
	public Action<Character, Attack> SpecialAttackAction;
	public Action<float, float> HealthChangedAction;

	public Action<Character> DeathAction;

	public Defs.CharacterType Type;
	public Cooldown SpecialAttackCooldown { get { return specialAttackCooldown; }}

	public Attack BasicAttack { get { return baseAttack; }}
	public Attack SpecialAttack { get { return specialAttack; }}
	public CharacterSettings Settings { get { return settings; }}
	public Vector3 Position { get { return transform.position; }}

	[SerializeField]
	ChargingBar chargingBar;

	[SerializeField]
	Attack specialAttack;

	[SerializeField]
	Attack baseAttack;

	[SerializeField]
	HitBlink hitBlink;

	[SerializeField]
	AudioSource audioSource;

	[SerializeField]
	CharacterSettings settings;

	[SerializeField]
	SpriteRenderer spriteRen;

	[SerializeField]
	Animator animator;

	[SerializeField]
	GameObject shadow;

	[SerializeField]
	List<Transform> pois;

	[SerializeField]
	Cooldown specialAttackCooldown;

	CharacterContext context;

	bool dying;
	int actualHealth;
	int fastAttackCounter;
	float speedX;
	float speedY;
	float jumpSpeedX;
	bool jumping;
	bool attacking;

	bool chargedAttackReleased;
	float chargedAttackStartTime;
	float chargedDuration;

	const float maxTime = 1.0f;
	const float maxMultiplication = 3.0f;

	public void Init(CharacterContext context)
	{
		this.context = context;
		SetHealth(settings.Health);
	}

	public void AiMove(Vector2 dir)
	{
		var normDir = dir.normalized;
		SetSpeedX(normDir.x * settings.MovingSpeed);
		SetSpeedY(normDir.y * settings.MovingSpeed);
		Flip(dir.x > 0 ?  1 : -1);
	}

	public void MoveH(int dir)
	{
		if (!attacking && !jumping) {
			Flip(dir);
			SetSpeedX(dir * settings.MovingSpeed);
		}
	}

	public void MoveV(int dir)
	{
		if (!attacking && !jumping) {
			SetSpeedY(dir * settings.MovingSpeed);
		}
	}

	public void Stop()
	{
		SetSpeedX(0);
		SetSpeedY(0);
	}

	public int GetFlip()
	{
		return transform.localScale.x > 0 ? 1 : -1;
	}

	public void Flip(int dir)
	{
		var scale = transform.localScale;
		scale.x = dir;
		transform.localScale = scale;
	}

	void OldAttack()
	{
		if (!attacking) {
			attacking = true;
			animator.SetTrigger("attack");
			StartAttack(baseAttack);
			AttackAction(this, baseAttack, 1.0f);
		}
	}

	public void FastAttack()
	{
		if (!attacking) {
			attacking = true;
			chargedAttackReleased = false;
			var trigger = fastAttackCounter++ % 2 == 0 ? "fastpunch01" : "fastpunch02";
			animator.SetTrigger(trigger);
			StartAttack(baseAttack);
		}
	}

	public void ChargedAttackReleased()
	{
		chargedAttackReleased = true;
		animator.speed = 1.0f;

		if (Charging()) {
			chargedDuration = Time.time - chargedAttackStartTime;
			chargedAttackStartTime = -1;
		} else {
			chargedDuration = 0;
		}
	}

	public void AttackSpecial()
	{
		if (!attacking && specialAttackCooldown.IsReady()) {
			attacking = true;
			animator.SetTrigger("specialattack");
			specialAttackCooldown.Restart();
			StartAttack(specialAttack);
		}
	}

	public void FaceTo (Vector3 position)
	{
		Flip(position.x > transform.position.x ? 1  : -1);
	}

	public void Jump()
	{
		if (!jumping) {
			jumping = true;
			jumpSpeedX = speedX;
			animator.SetTrigger("jump");
			StartCoroutine(JumpMove());
		}
	}

	void SetSpeedX(float speed)
	{
		speedX = speed;
	}

	void SetSpeedY(float speed)
	{
		speedY = speed;
	}

	void Update () 
	{
		var pos = transform.position;
		pos.x += speedX * Time.deltaTime;
		pos.y += speedY * Time.deltaTime;
		transform.position = pos;

		animator.SetFloat("speed", (float)Math.Sqrt(speedX * speedX + speedY * speedY));

		speedX = 0;
		speedY = 0;

		TrimPositionToLimits();
		UpdateSortingOrder();
		UpdateCharging();

	}



	IEnumerator JumpMove()
	{
		while(jumping)
		{
			var pos = transform.position;
			pos.x += jumpSpeedX * Time.deltaTime;
			transform.position = pos;
			yield return 0;
		}
	}

	public void Hit(Attack attack, int dir, float multiplicator)
	{
		if (attack.ShiftHitEnemy) {
			transform.AddPositionX((int)dir * 1.0f);
		}
		audioSource.clip = settings.HitSfx;
		audioSource.Play();

		context.EffectManager.CreateEffect(hitBlink.gameObject).Run(gameObject);

		if (attack.HitEffect != null && attack.HitEffect.Effect != null) {
			context.EffectManager.CreateEffect(
				attack.HitEffect.Effect.gameObject,
				GetPoi(attack.HitEffect.Container))
				.Run(gameObject);
		}

		bool alive = SetHealth(actualHealth - (int)(attack.AttackPoints * multiplicator));
		chargedAttackStartTime = -1;

		if (alive) {
			attacking = false;

			if (!jumping) {
				Stop();
				animator.SetTrigger("hit");
			}
		} else {
			dying = true;
			Stop();
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

	void PlayAnimation(string animName)
	{
		animator.SetTrigger(animName);
	}

	bool IsMoving()
	{
		return speedX > 0 || speedY > 0;
	}

	void AnimationEvent(string name)
	{
		if (name.Equals(Defs.Events.AttackFinished)) {
			attacking = false;
		}
		else if (name.Equals(Defs.Events.SpecialAttackFinished)) {
			attacking = false;
		}
		else if (name.Equals(Defs.Events.SpecialAttackHit)) {
			SpecialAttackAction(this, specialAttack);
		}
		else if (name.Equals(Defs.Events.FastAttackHit)) {
			chargedAttackStartTime = -1;
			AttackAction(this, baseAttack, AttackMultiplicator(chargedDuration));
		}
		else if (name.Equals(Defs.Events.DieFinished)) {
			Destroy(gameObject);
		}
		else if (name.Equals(Defs.Events.JumpFinished)) {
			jumping = false;
		}
		else if (name.Equals(Defs.Events.AttackCharged)) {
			if (!IsChargedAttackReleased()) {
				animator.speed = 0;
				chargedAttackStartTime = Time.time;
			}
		}
	}

	bool IsChargedAttackReleased()
	{
		return Type == Defs.CharacterType.NPC || chargedAttackReleased;
	}

	void StartAttack(Attack attack)
	{
		audioSource.clip = attack.Sfx;
		audioSource.Play();

		for (int i = 0; i < attack.Effects.Count; ++i) {
			context.EffectManager.CreateEffect(
				attack.Effects[i].Effect.gameObject, 
				transform.Find("Root/" + attack.Effects[i].Container));
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

	void UpdateCharging()
	{
		if (Type == Defs.CharacterType.Player) {
			if (Charging()) {
				chargedDuration = Time.time - chargedAttackStartTime;
				float normalizedMultiplicator = Mathf.Min(chargedDuration / maxTime, 1.0f);
				if (normalizedMultiplicator > 0.2f) {
					chargingBar.gameObject.SetActive(true);
					chargingBar.SetValue(normalizedMultiplicator);
				}else {
					chargingBar.gameObject.SetActive(false);
				}
			} else {
				chargingBar.gameObject.SetActive(false);
			}
		}
	}

	void TrimPositionToLimits()
	{
		if (context != null) {
		var pos = transform.position;
			pos.y = Math.Max(Math.Min(context.Limits().YMax, pos.y), context.Limits().YMin);
			pos.x = Math.Max(Math.Min(context.Limits().XMax, pos.x), context.Limits().XMin);
			transform.position = pos;
		}
	}

	Transform GetPoi(string name)
	{
		return pois.Find(x=>x.name.Equals(name));
	}

	bool Charging()
	{
		return chargedAttackStartTime > 0;
	}


	static float AttackMultiplicator(float chargedTime)
	{
		float normalizedMultiplicator = Mathf.Min(chargedTime / maxTime);
		return chargedTime < 0.05f ? 1 : 1 + maxMultiplication * normalizedMultiplicator;
	}
}
