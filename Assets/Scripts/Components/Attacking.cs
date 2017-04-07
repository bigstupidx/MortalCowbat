﻿using UnityEngine;
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

		[SerializeField]
		Sprite kickAttackSprite;

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

		float chargedAttackStartTime;
		float chargedDuration;

		const float maxTime = 1.0f;
		const float maxMultiplication = 3.0f;

		public void Perform()
		{
			attacking = true;
		}

		public bool IsAttacking()
		{
			return attacking;
		}

		public void StartFastAttack()
		{
			if (!IsAttacking()) {
				if (GetComp<Jumping>().IsJumping()) {
					jumpAttacking = true;
					StartAttackEffects(JumpAttack);
					jumpId++;
				} else {
					var trigger = fastAttackCounter++ % 2 == 0 ? "fastpunch01" : "fastpunch02";
					GetComp<Animating>().SetTrigger(trigger);
					StartAttackEffects(BasicAttack);
					attacking = true;
				}
			}
		}

		public void StartHeavyAttack()
		{
			if (!IsAttacking()) {
				attacking = true;
				chargedAttackReleased = false;
				GetComp<Animating>().SetTrigger(Defs.Animations.HeavyAttack);
				StartAttackEffects(BasicAttack);
			}
		}

		public void StartSpecialAttack()
		{
			if (!IsAttacking() && specialAttackCooldown.IsReady()) {
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
				GetComp<Effects>().EffectManager.CreateEffect(
					attack.Effects[i],
					transform.Find("Root/" + attack.Effects[i].Container),
					gameObject);
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
			GetComp<Visual>().Ren.sprite = kickAttackSprite;
		}

		bool Charging()
		{
			return chargedAttackStartTime > 0;
		}

		bool IsChargedAttackReleased()
		{
			return GetCharacter().Type == Defs.CharacterType.NPC || chargedAttackReleased;
		}
			
		void UpdateCharging()
		{
			if (GetCharacter().Type == Defs.CharacterType.Player) {
				var chargingBar = GetComp<Visual>().ChargingBar;
				if (Charging()) {
					chargedDuration = Time.time - chargedAttackStartTime;
					float normalizedMultiplicator = Mathf.Min(chargedDuration / maxTime, 1.0f);

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
		}

		public KeyValuePair<float, bool> AttackMultiplicator(float chargedTime)
		{
			float normalizedMultiplicator = Mathf.Min(chargedTime / maxTime, 1.0f);
			var value =  chargedTime < 0.05f ? 1 : 1 + maxMultiplication * normalizedMultiplicator;
			return new KeyValuePair<float, bool>(value, normalizedMultiplicator > 0.5f);
		}
	}
}
