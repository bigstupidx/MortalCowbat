using System;
using UnityEngine;
using System.Collections;
using Battle.Comp;

namespace Ai
{
	class WaitingState : IAiState
	{
		AiStateContext context;

		float duration;

		public WaitingState(AiStateContext context, float duration)
		{
			this.context = context;
			this.duration = duration;
		}

		public void Update()
		{
			duration -= Time.deltaTime;
			context.Character.GetComp<Moving>().Stop();

			if (duration < 0) {
				context.Sm.SetState(new ChasingState(context));
			}
		}
	}
}