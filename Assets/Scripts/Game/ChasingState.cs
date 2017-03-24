﻿using System;
using UnityEngine;
using System.Collections;

namespace Ai
{
	class ChasingState : IAiState
	{
		AiStateContext context;
	
		Character chasedCharacter;

		public ChasingState(AiStateContext context)
		{
			this.context = context;
		}

		public void Update()
		{
			if (chasedCharacter == null) {
				var targets = context.Sm.FindTargets(100);
				if (targets.Count > 0) {
					chasedCharacter = targets[0];
				}
			}	
			if (chasedCharacter != null) {
				var dstPosition = chasedCharacter.Position;
				var direction = (dstPosition - context.Character.Position);
				var normDir = direction.normalized;
				context.Character.AiMove(normDir);

				var sqrDist = (context.Character.Position - chasedCharacter.Position).sqrMagnitude;

				var sqrRangeDist = context.Character.GetBasicAttackRange() * 0.9f;
				sqrRangeDist *= 2;

				if (sqrDist < sqrRangeDist) {
					context.Character.Stop();
					context.Sm.SetState(new AttackState(context));
				}
			}else {
				context.Character.Stop();				
			}
		}
	}
}