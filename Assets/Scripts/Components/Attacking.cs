using UnityEngine;
using System;
using System.Collections.Generic;

namespace Battle.Comp
{
	public class Attacking : CharacterComponent
	{
		public Cooldown SpecialAttackCooldown { get { return specialAttackCooldown; }}
		public Action<Character, Attack, float, bool> HeavyAttackAction;
		public Action<Character, Attack, bool> SpecialAttackAction;
		public Action<Character, Attack, int> JumpAttackAction;
		public Action<Character, Attack> AttackAction;

		public float MaxHeavyAttackChargedTime = 1.0f;
		public float MaxHeavyAttackMultiplication = 3.0f;

		[SerializeField]
		List<Sprite> kickAttackSprites;

		[SerializeField]
		List<string> fastAttacksTriggers;

		[SerializeField]
		Cooldown specialAttackCooldown;

		public Attack BasicAttack;
		public Attack SpecialAttack;
		public Attack JumpAttack;

		bool chargedAttackReleased;
		bool attacking;
		bool jumpAttacking;
		int jumpId;
		int fastAttackCounter;
		int kickAttackFrame;
		float kickAttackTimer;
		float lastAttackTime;

		float chargedAttackStartTime;
		float chargedDuration;

		List<Effect> effectToStopOnHit =  new List<Effect>();

		public void Perform()
		{
			attacking = true;
		}

		public bool IsAttacking()
		{
			return attacking;
		}

		public bool AllowsFlipChange()
		{
			return !attacking || Charging();
		}


		public void StartFastAttack()
		{
			if (!IsAttacking()) {
				if (GetComp<Jumping>().IsJumping()) {
					jumpAttacking = true;
					kickAttackFrame = 0;
					kickAttackTimer = Time.time + 0.1f;
					StartAttackEffects(JumpAttack);
					jumpId++;
				} else {

					SetFastAttackCounter ();

					var trigger = GetFastAttack();
					GetComp<Animating>().SetTrigger(trigger);
					StartAttackEffects(BasicAttack);
					attacking = true;
				}
			}
		}

		string GetFastAttack()
		{
			int mod = fastAttackCounter % fastAttacksTriggers.Count;
			fastAttackCounter++;
			return fastAttacksTriggers[mod];
		}

		public void StartHeavyAttack(float duration = -1)
		{
			if (!IsAttacking()) {
				attacking = true;
				chargedAttackReleased = false;
				GetComp<Animating>().SetTrigger(Defs.Animations.HeavyAttack);
				StartAttackEffects(BasicAttack);
				// automatic chargin release
				if (duration > 0) {
					Invoke("ChargedAttackReleased", duration);			
				} 
			}
		}

		public void StartSpecialAttack()
		{
			if (!IsAttacking() && !GetComp<Jumping>().IsJumping() && specialAttackCooldown.IsReady()) {
				attacking = true;
				GetComp<Animating>().SetTrigger(Defs.Animations.SpecialAttack);
				specialAttackCooldown.Restart();
				StartAttackEffects(SpecialAttack);
			}
		}

		public void HeavyAttackHit()
		{
			var chargedState = AttackMultiplicator(chargedDuration);
			SetChargedAttackStartTime(-1.0f);
			HeavyAttackAction(GetCharacter(),  BasicAttack, chargedState.Key, chargedState.Value);
		}


		public void Stop()
		{
			attacking = false;
			jumpAttacking = false;

			effectToStopOnHit.ForEach(x=>GetComp<Effects>().EffectManager.StopEffect(x));

			ChargedAttackReleased();
		}
		public float GetBasicAttackRange()
		{
			var box =  BasicAttack.Colliders.Find(x=> x is BoxCollider2D) as BoxCollider2D;
			return (box.size.x * 0.5f + box.offset.x) * Math.Abs(transform.localScale.x);
		}

		public void StartAttackEffects(Attack attack)
		{
			GetComp<Sound>().Play(attack.Sfx);

			for (int i = 0; i < attack.Effects.Count; ++i) {
				var effectDestcriptor = attack.Effects[i];
				var effect = GetComp<Effects>().EffectManager.CreateEffect(
					effectDestcriptor,
					GetComp<Visual>().GetPoi(effectDestcriptor.Container),
					gameObject);
			
				if (effectDestcriptor.CustomData.Contains("stoponhit")) {
					effectToStopOnHit.Add(effect);
					effect.FinishAction += eff => effectToStopOnHit.Remove (eff);
				}
			}
		}

		public void StartAttackCharging ()
		{
			if (!IsChargedAttackReleased()) {
				SetChargedAttackStartTime(Time.time);
			}
		}

		public void ChargedAttackReleased()
		{
			chargedAttackReleased = true;
			if (Charging()) {
				chargedDuration = Time.time - chargedAttackStartTime;
				SetChargedAttackStartTime(-1);
			} else {
				chargedDuration = 0;
			}
		}

		public override void UpdateMe()
		{
			UpdateCharging();
		}

		public override void LateUpdateMe()
		{
			if (jumpAttacking) {
				ForceJumpKickFrame();
				JumpAttackAction(GetCharacter(), JumpAttack, jumpId);
			}
		}

		public void SetChargedAttackStartTime(float time)
		{
			chargedAttackStartTime = time;
			GetComp<Animating>().SetFloat("chargedattackastarttime", time);
		}

		void ForceJumpKickFrame()
		{
			GetComp<Visual>().Ren.sprite = kickAttackSprites[kickAttackFrame];
			if (Time.time > kickAttackTimer) {
				kickAttackFrame = Math.Min(kickAttackFrame + 1, kickAttackSprites.Count - 1);
			}
		}

		bool Charging()
		{
			return chargedAttackStartTime > 0;
		}

		bool IsChargedAttackReleased()
		{
			return chargedAttackReleased;
		}
			
		void UpdateCharging()
		{
			var chargingBar = GetComp<Visual>().ChargingBar;
			if (Charging()) {
				chargedDuration = Time.time - chargedAttackStartTime;
				float normalizedMultiplicator = Mathf.Min(chargedDuration / MaxHeavyAttackChargedTime, 1.0f);

				if (normalizedMultiplicator > 0.2f) {
					chargingBar.gameObject.SetActive(true);
					chargingBar.SetValue(normalizedMultiplicator);
				} else {
					chargingBar.gameObject.SetActive(false);
				}
			} else {
				chargingBar.gameObject.SetActive(false);
			}
		}

		void SetFastAttackCounter ()
		{
			if (Time.time > (lastAttackTime + GlobalBattleConfig.FastAttackChangeMaxDelay)) {
				fastAttackCounter = 0;
				lastAttackTime = Time.time;
			}
		}

		public KeyValuePair<float, bool> AttackMultiplicator(float chargedTime)
		{
			float normalizedMultiplicator = Mathf.Min(chargedTime / MaxHeavyAttackChargedTime, 1.0f);
			var value =  chargedTime < 0.05f ? 1 : 1 + MaxHeavyAttackMultiplication * normalizedMultiplicator;
			return new KeyValuePair<float, bool>(value, normalizedMultiplicator > 0.5f);
		}
	}
}
