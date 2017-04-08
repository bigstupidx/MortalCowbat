using System;
using UnityEngine;
using Battle.Comp;

namespace Ai
{
	class AttackState : IAiState
	{
		AiStateContext context;

		float nextAttack;

		public AttackState(AiStateContext context)
		{
			this.context = context;
			nextAttack = Time.time + context.Sm.Preset.FirstAttackDelay;
		}

		public void Update()
		{
			if (Time.time > nextAttack) {

				var targets = context.Sm.FindTargets(context.Character.GetComp<Attacking>().GetBasicAttackRange() * context.Sm.Preset.AttackRangeCoeficient);
				if (targets.Count > 0) {
					context.Character.GetComp<Moving>().FaceTo(targets[0].GetPosition());

					PerformRandomAttack();
							
					nextAttack = Time.time + context.Sm.Preset.AttackInterval;
				} else {
					context.Character.GetComp<Attacking>().Stop();
					context.Sm.SetState(new ChasingState(context));
				}
			}
		}

		void PerformRandomAttack()
		{
			var aiPreset = context.Sm.Preset;
			// fast x heavy first
			var rnd = UnityEngine.Random.Range(0.0f, 1.0f);

			if (rnd < aiPreset.FastAttackProbability) {
				context.Character.FastAttack();
			} else if (rnd < (aiPreset.FastAttackProbability + aiPreset.HeavyAttackProbability)) {
				var chargeDuration = UnityEngine.Random.Range(aiPreset.HeavyAttackMinCharge, aiPreset.HeavyAttackMaxCharge);
				context.Character.HeavyAttack(chargeDuration);
			} else {
				PerformKickAttack();
			}
		}

		void PerformKickAttack()
		{
			context.Character.GetComp<Moving>().SetSpeedX(context.Character.GetComp<Moving>().GetFlip());
			context.Character.Jump();
			context.Character.FastAttack();
		}
	}
}