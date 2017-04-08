using System;
using UnityEngine;
using System.Collections;
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
			nextAttack = Time.time + context.Sm.Settings.FirstAttackDelay;
		}

		public void Update()
		{
			if (Time.time > nextAttack) {

				var targets = context.Sm.FindTargets(context.Character.GetComp<Attacking>().GetBasicAttackRange() * 0.9f);
				if (targets.Count > 0) {
					context.Character.GetComp<Moving>().FaceTo(targets[0].GetPosition());

					PerformRandomAttack();
							
					nextAttack = Time.time + context.Sm.Settings.AttackInterval;
				} else {
					context.Character.GetComp<Attacking>().Stop();
					context.Sm.SetState(new ChasingState(context));
				}
			}
		}

		void PerformRandomAttack()
		{
			if (Utils.GetRandomBool()) {
				var duration = UnityEngine.Random.Range(0,context.Character.GetComp<Attacking>().MaxHeavyAttackChargedTime);
				context.Character.HeavyAttack(duration);
			}
			else {
				context.Character.FastAttack();
			}
		}
	}
}