using UnityEngine;
using System;
using Battle.Comp;


public partial class Character : MonoBehaviour, ICharacter
{
	public Defs.CharacterType Type;
	public bool CheckLimits { get; set;}
	public CharacterSettings Settings { get { return settings; }}

	[SerializeField]
	ComponentHolder componentHolder;

	[SerializeField]
	CharacterSettings settings;

	CharacterContext context;

	void Awake()
	{
		CheckLimits = true;
		InitComponents();
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
		GetComp<Moving>().SetSpeed(normDir.x * settings.MovingSpeed, normDir.y * settings.MovingSpeed);
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

	public Vector3 GetPosition()
	{
		return GetComp<Visual>().Position;
	}

	public void FastAttack()
	{
		GetComp<Attacking>().StartFastAttack();
	}

	public void HeavyAttack()
	{
		GetComp<Attacking>().StartHeavyAttack();
	}

	public void ChargedAttackReleased()
	{
		GetComp<Attacking>().ChargedAttackReleased();
	}

	public void AttackSpecial()
	{
		GetComp<Attacking>().StartSpecialAttack();
	}

	public void Jump()
	{
		GetComp<Jumping>().Perform();
	}

	void Update () 
	{
		if (!GetComp<Pause>().Paused) {
			GetComp<Moving>().UpdateMe();
			GetComp<Animating>().UpdateMe();
			GetComp<Moving>().Stop();
			GetComp<Jumping>().UpdateMe();
		

			if (CheckLimits) {
				TrimPositionToLimits();
			}
			GetComp<Attacking>().UpdateMe();
			GetComp<Visual>().UpdateMe();
		}
	}

	void LateUpdate()
	{
		GetComp<Attacking>().LateUpdateMe();
	}


	public void Hit(Attack attack, Character attackingCharacter, int dir, float multiplicator, bool maxed, int attackId = -1)
	{
		GetComp<Hit>().Perform(attack, attackingCharacter, dir,multiplicator, maxed, attackId);
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
			GetComp<Attacking>().SpecialAttackAction(this, GetComp<Attacking>().SpecialAttack, false);
		}
		else if (name.Equals(Defs.Events.FastAttackHit)) {
			GetComp<Attacking>().AttackAction(this,  GetComp<Attacking>().BasicAttack);
		}
		else if (name.Equals(Defs.Events.HeavyAttackHit)) {
			GetComp<Attacking>().HeavyAttackHit();
		}
		else if (name.Equals(Defs.Events.DieFinished)) {
			Destroy(gameObject);
		}
		else if (name.Equals(Defs.Events.JumpFinished)) {
			GetComp<Jumping>().Stop();
			GetComp<Attacking>().Stop();
		}
		else if (name.Equals(Defs.Events.FallFinished)) {
			GetComp<Moving>().FinishFall();
		}
		else if (name.Equals(Defs.Events.AttackCharged)) {
			GetComp<Attacking>().StartAttackCharging();
		}
		else if (name.Equals(Defs.Events.JumpStarted)) {
			GetComp<Jumping>().JumpStarted();
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

	void OnDestroy()
	{
		Debug.Log (string.Format ("Character.OnDestroy(){0}", name));
	}	

	void InitComponents()
	{
		componentHolder.components.Add(gameObject.AddComponent<Pause>());
		componentHolder.components.Add(gameObject.AddComponent<Death>());
		componentHolder.components.ForEach(x=>x.Init(this, componentHolder));
	}
}
