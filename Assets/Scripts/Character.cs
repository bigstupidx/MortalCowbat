using UnityEngine;
using System;
using Ai;
using System.Collections.Generic;
using Vis;
using Battle.Comp;


public partial class Character : MonoBehaviour, ICharacter
{
	public Action<Character, Attack, float, bool> HeavyAttackAction;
	public Action<Character, Attack> AttackAction;
	public Action<Character, Attack, bool> SpecialAttackAction;
	public Action<Character, Attack, int> JumpAttackAction;


	public Action<Character> DeathAction;

	public Defs.CharacterType Type;
	public Cooldown SpecialAttackCooldown { get { return specialAttackCooldown; }}

	public bool CheckLimits { get; set;}

	public Attack BasicAttack { get { return baseAttack; }}
	public Attack SpecialAttack { get { return specialAttack; }}
	public Attack JumpAttack { get { return jumpAttack; }}

	public CharacterSettings Settings { get { return settings; }}
	public Vector3 Position { get { return transform.position; }}


	[SerializeField]
	ComponentHolder componentHolder;

	[SerializeField]
	CameraShake shake;

	[SerializeField]
	ChargingBar chargingBar;

	[SerializeField]
	Attack specialAttack;

	[SerializeField]
	Attack baseAttack;

	[SerializeField]
	Attack jumpAttack;

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

	[SerializeField]
	Sprite jumpKick;

	CharacterContext context;

	int fastAttackCounter;

	bool jumpAttacking;
	int jumpId;

	bool chargedAttackReleased;
	float chargedAttackStartTime;
	float chargedDuration;

	const float maxTime = 1.0f;
	const float maxMultiplication = 3.0f;

	bool paused;

	float animatorSpeed;

	void Awake()
	{
		CheckLimits = true;
		componentHolder.components.ForEach(x=>x.Init(componentHolder));
	}

	public void Init(CharacterContext context)
	{
		this.context = context;
		GetComp<Health>().Init(settings.Health, settings.Health);
	}

	public T GetComp<T>() where T: CharacterComponent
	{
		return componentHolder.Get<T>();
	}

	public void AiMove(Vector2 dir)
	{
		var normDir = dir.normalized;

		GetComp<Moving>().SetSpeedX(normDir.x * settings.MovingSpeed);
		GetComp<Moving>().SetSpeedY(normDir.y * settings.MovingSpeed);
		GetComp<Moving>().Flip(dir.x > 0 ?  1 : -1);
	}

	public void MoveH(int dir)
	{
		if (!GetComp<Attacking>().IsAttacking() && !GetComp<Jumping>().IsJumping()) {
			GetComp<Moving>().Flip(dir);
			GetComp<Moving>().SetSpeedX(dir * settings.MovingSpeed);
		}
	}

	public void MoveV(int dir)
	{
		if (!GetComp<Attacking>().IsAttacking() && !GetComp<Jumping>().IsJumping()) {
			GetComp<Moving>().SetSpeedY(dir * settings.MovingSpeed);
		}
	}


	public void FastAttack()
	{
		if (!GetComp<Attacking>().IsAttacking()) {
			if (GetComp<Jumping>().IsJumping()) {
				jumpAttacking = true;
				StartAttack(jumpAttack);
				jumpId++;
			} else {
				var trigger = fastAttackCounter++ % 2 == 0 ? "fastpunch01" : "fastpunch02";
				animator.SetTrigger(trigger);
				StartAttack(baseAttack);
				GetComp<Attacking>().Perform();
			}
		}
	}

	public void HeavyAttack()
	{
		if (!GetComp<Attacking>().IsAttacking()) {
			GetComp<Attacking>().Perform();
			chargedAttackReleased = false;
			animator.SetTrigger(Defs.Animations.HeavyAttack);
			StartAttack(baseAttack);
		}
	}

	public void ChargedAttackReleased()
	{
		chargedAttackReleased = true;
		animator.speed = 1.0f;

		if (Charging()) {
			chargedDuration = Time.time - chargedAttackStartTime;
			SetChargedAttackStartTime(-1);
		} else {
			chargedDuration = 0;
		}
	}

	public void AttackSpecial()
	{
		if (!GetComp<Attacking>().IsAttacking() && specialAttackCooldown.IsReady()) {
			GetComp<Attacking>().Perform();
			animator.SetTrigger("specialattack");
			specialAttackCooldown.Restart();
			StartAttack(specialAttack);
		}
	}


	public void Jump()
	{
		GetComp<Jumping>().Perform();
	}

	public void Pause()
	{
		animatorSpeed = animator.speed;
		animator.speed = 0.0f;
		paused = true;
	}

	public void Resume()
	{
		animator.speed = animatorSpeed;
		animator.speed = 1.0f;

		paused = false;
	}


	void Update () 
	{
		if (!paused) {

			GetComp<Moving>().UpdateMe();
			GetComp<Animating>().UpdateMe();
			GetComp<Moving>().Stop();
			GetComp<Jumping>().UpdateMe();

			if (CheckLimits) {
				TrimPositionToLimits();
			}
			UpdateSortingOrder();
			UpdateCharging();
		}
	}

	void LateUpdate()
	{
		if (jumpAttacking) {
			ForceJumpKickFrame();
			JumpAttackAction(this, jumpAttack, jumpId);
		}
	}


	public void Hit(Attack attack, Character attackingCharacter, int dir, float multiplicator, bool maxed, int attackId = -1)
	{
		if (GetComp<Hit>().CanBeHit(attackId)) {
			GetComp<Hit>().Perform(attackId);

			if (attack.ShiftHitEnemy) {
				transform.AddPositionX((int)dir * 1.0f);
			}
		
			GetComp<Sound>().Play(settings.HitSfx);

			context.EffectManager.CreateEffect(hitBlink.gameObject).Run(gameObject);

			for (int i = 0; i < attack.HitEffects.Count; ++i) {
				var effectDescr = attack.HitEffects[i];
				if (effectDescr.Effect != null) {

					Transform poi = 
						effectDescr.CustomData.Contains ("OnAttacker") ?
						attackingCharacter.GetPoi (effectDescr.Container) : 
						GetPoi (effectDescr.Container);

					context.EffectManager.CreateEffect(effectDescr,
						poi,
						gameObject
					)
						.Run(gameObject);
				}
			}

			if (maxed) {
				context.EffectManager.CreateEffect(shake.gameObject);
			}


			bool alive = GetComp<Health>().ReduceHealth(attack.AttackPoints * multiplicator);
			SetChargedAttackStartTime(-1);

			if (alive) {
				GetComp<Attacking>().Stop();
				jumpAttacking = false;
				if (!GetComp<Jumping>().IsJumping()) {
					GetComp<Moving>().Stop();

					if (attack.EnemyFalls) {
						GetComp<Moving>().Fall();
					}
					else {
						animator.SetTrigger(Defs.Animations.Hit);
					}
				}
			} else {
				GetComp<Moving>().Stop();
				GetComp<Jumping>().SetSpeedX(0.0f);
				GetComp<Animating>().SetTrigger(Defs.Animations.Die);
				Destroy(GetComponent<AiStateMachine>());
				if (DeathAction != null) {
					DeathAction(this);
				}
			}
		}
	}


	public float GetBasicAttackRange()
	{
		var box =  baseAttack.Colliders.Find(x=> x is BoxCollider2D) as BoxCollider2D;
		return (box.size.x * 0.5f + box.offset.x) * Math.Abs(transform.localScale.x);
	}

	void AnimationEvent(string name)
	{
		if (name.Equals(Defs.Events.AttackFinished)) {
			GetComp<Attacking>().Stop();
		}
		else if (name.Equals(Defs.Events.SpecialAttackFinished)) {
			GetComp<Attacking>().Stop();
		}
		else if (name.Equals(Defs.Events.SpecialAttackHit)) {
			SpecialAttackAction(this, specialAttack, false);
		}
		else if (name.Equals(Defs.Events.FastAttackHit)) {
			AttackAction(this, baseAttack);
		}
		else if (name.Equals(Defs.Events.HeavyAttackHit)) {
			chargedAttackStartTime = -1;
			var chargedState = AttackMultiplicator(chargedDuration);
			HeavyAttackAction(this, baseAttack, chargedState.Key, chargedState.Value);
		}
		else if (name.Equals(Defs.Events.DieFinished)) {
			Destroy(gameObject);
		}
		else if (name.Equals(Defs.Events.JumpFinished)) {
			GetComp<Jumping>().Stop();
			jumpAttacking = false;
		}
		else if (name.Equals(Defs.Events.FallFinished)) {
			GetComp<Moving>().FinishFall();
		}
		else if (name.Equals(Defs.Events.AttackCharged)) {
			if (!IsChargedAttackReleased()) {
				SetChargedAttackStartTime(Time.time);
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
				attack.Effects[i],
				transform.Find("Root/" + attack.Effects[i].Container),
				gameObject);
		}
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

	void ForceJumpKickFrame()
	{
		spriteRen.sprite = jumpKick;
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


	static KeyValuePair<float, bool> AttackMultiplicator(float chargedTime)
	{
		float normalizedMultiplicator = Mathf.Min(chargedTime / maxTime, 1.0f);
		var value =  chargedTime < 0.05f ? 1 : 1 + maxMultiplication * normalizedMultiplicator;
		return new KeyValuePair<float, bool>(value, normalizedMultiplicator > 0.5f);
	}



	void OnDestroy()
	{
		Debug.Log (string.Format ("Character.OnDestroy(){0}", name));
	}	

	void SetChargedAttackStartTime(float time)
	{
		chargedAttackStartTime = time;
		animator.SetFloat("chargedattackastarttime", time);
	}
}
