using UnityEngine;
using System;

namespace Battle.Comp
{
	public class Attacking : CharacterComponent
	{
		public Action<Character, Attack, float, bool> HeavyAttackAction;
		public Action<Character, Attack> AttackAction;
		public Action<Character, Attack, bool> SpecialAttackAction;
		public Action<Character, Attack, int> JumpAttackAction;


		public Attack BasicAttack;
		public Attack SpecialAttack;
		public Attack JumpAttack;

		bool attacking;

		public void Perform()
		{
			attacking = true;
		}

		public bool IsAttacking()
		{
			return attacking;
		}

		public void Stop()
		{
			attacking = false;
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
	}
}
