using System;
using UnityEngine;
using System.Collections;

namespace Ai
{
	class AttackState : IAiState
	{
		AiStateContext context;

		float nextAttack;

		public AttackState(AiStateContext context)
		{
			this.context = context;
			nextAttack = Time.time + 0.5f;
		}

		public void Update()
		{
			if (Time.time > nextAttack) {

				var targets = context.Sm.FindTargets(context.Character.GetBasicAttackRange() * 0.9f);
				if (targets.Count > 0) {
					context.Character.FaceTo(targets[0].Position);
					context.Character.Attack();
					nextAttack = Time.time + 2.0f;
				} else {
					context.Sm.SetState(new ChasingState(context));
				}
			}

		}
	}
}