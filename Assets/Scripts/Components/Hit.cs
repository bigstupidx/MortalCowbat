using UnityEngine;
using System;
using System.Collections;


namespace Battle.Comp
{
	public class Hit : CharacterComponent
	{
		public Action<Character> HitAction;
		public bool InDaze { get { return inDaze; }}

		[SerializeField]
		GameObject hitEffect;

		[SerializeField]
		float dazePlusTime;

		int lastAttackHitHId;
		bool inDaze;

		void Awake()
		{
			lastAttackHitHId = -1;
		}

		public void StopDaze()
		{
			float dazerRndDelay = GlobalBattleConfig.RandomizeDazeDuration ? 
				UnityEngine.Random.Range(0.0f,GlobalBattleConfig.DazeRandomDuration) : 0;
			StartAction(()=>inDaze = false, dazePlusTime + dazerRndDelay);
		}

		public bool CanBeHit(int attackId)
		{
			return (attackId ==-1 || lastAttackHitHId != attackId) && !GetComp<Falling>().IsFalling;
		}


		public IEnumerator Perform(Attack attack, Character attackingCharacter, int dir, float multiplicator, bool maxed, int attackId = -1)
		{
			if (CanBeHit(attackId)) {
				lastAttackHitHId = attackId;
			
				if (hitEffect != null) {
					GetComp<Effects>().EffectManager.CreateEffect(hitEffect).Run(gameObject);
				}

				//GetComp<Sound>().Play(settings.HitSfx);
				GetComp<Effects>().EffectManager.CreateEffect(GetComp<Effects>().HitBlink).Run(gameObject);

				for (int i = 0; i < attack.HitEffects.Count; ++i) {
					var effectDescr = attack.HitEffects[i];
					if (effectDescr.Effect != null) {

						Transform poi = 
							effectDescr.CustomData.Contains ("OnAttacker") ?
							attackingCharacter.GetComp<Visual>().GetPoi(effectDescr.Container) : 
							GetComp<Visual>().GetPoi (effectDescr.Container);

						GetComp<Effects>().EffectManager.CreateEffect(effectDescr,
							poi,
							gameObject
						)
							.Run(gameObject);
					}
				}

				if (maxed) {
					
				}

				bool alive = GetComp<Health>().ReduceHealth(attack.AttackPoints * multiplicator);
				GetComp<Attacking>().SetChargedAttackStartTime(-1);

				if (HitAction != null) {
					HitAction(GetCharacter());
				}

				if (alive) {
					if (attack.ShiftHitEnemy) {
						StartCoroutine(GetComp<Moving>().ShiftCoroutine((int)dir * attack.ShiftHitEnemyDistance, 0.1f));
					}

					GetComp<Attacking>().Stop();
					if (!GetComp<Jumping>().IsJumping()) {
						GetComp<Moving>().Stop();

						if (attack.EnemyFalls) {
							GetComp<Falling>().Fall();
						}
						else {
							inDaze = true;
							GetComp<Animating>().SetTrigger(Defs.Animations.Hit);
						}
					}
				} else {
					if (attack.ShiftHitEnemy) {
						StartCoroutine(GetComp<Moving>().ShiftCoroutine((int)dir * attack.ShiftHitEnemyDistance, 0.1f));
					}

					GetComp<Death>().Perform();
					yield break;
				}
			}
		}

	}
}
