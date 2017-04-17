using UnityEngine;


namespace Battle.Comp
{
	public class Hit : CharacterComponent
	{
		[SerializeField]
		GameObject hitEffect;

		int lastAttackHitHId;

		void Awake()
		{
			lastAttackHitHId = -1;
		}

		public bool CanBeHit(int attackId)
		{
			return (attackId ==-1 || lastAttackHitHId != attackId) && !GetComp<Moving>().Falling;
		}


		public void Perform(Attack attack, Character attackingCharacter, int dir, float multiplicator, bool maxed, int attackId = -1)
		{
			if (CanBeHit(attackId)) {
				lastAttackHitHId = attackId;

				if (attack.ShiftHitEnemy) {
					transform.AddPositionX((int)dir * 1.0f);
				}

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
					GetComp<Effects>().EffectManager.CreateEffect(GetComp<Effects>().ShakeEffect);
				}

				bool alive = GetComp<Health>().ReduceHealth(attack.AttackPoints * multiplicator);
				GetComp<Attacking>().SetChargedAttackStartTime(-1);

				if (alive) {
					GetComp<Attacking>().Stop();
					if (!GetComp<Jumping>().IsJumping()) {
						GetComp<Moving>().Stop();

						if (attack.EnemyFalls) {
							GetComp<Moving>().Fall();
						}
						else {
							GetComp<Animating>().SetTrigger(Defs.Animations.Hit);
						}
					}
				} else {
					GetComp<Death>().Perform();
				}
			}
		}

	}
}
