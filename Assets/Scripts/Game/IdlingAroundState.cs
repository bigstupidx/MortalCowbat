using System;
using UnityEngine;
using System.Collections;
using Battle.Comp;
using System.Net.Mail;

namespace Ai
{
	class IdlingAroundState : IAiState
	{
		AiStateContext context;

		Character chasedCharacter;

		Vector3 dstPosition;

		public IdlingAroundState(AiStateContext context)
		{
			this.context = context;

			Run();
		}

		public void Update()
		{
			var direction = (dstPosition - context.Character.GetPosition());
			var normDir = direction.normalized;
			context.Character.AiMove(normDir);

			var sqrDist = (dstPosition - context.Character.GetPosition()).sqrMagnitude;

		
			if (sqrDist < 0.2f * 0.2f) {
				context.Character.GetComp<Moving>().Stop();
				context.Sm.SetState(new WaitingState(context,UnityEngine.Random.Range(0.5f, 1.0f)));
			
				var target = context.Sm.FindTargets(100).Find(x=>x.Type == Defs.CharacterType.Player);
				if (target != null) {
					context.Character.GetComp<Moving>().FaceTo(target.GetPosition());
				}
			}
		}

		void Run()
		{
			var direction = UnityEngine.Random.insideUnitCircle;
			direction = direction.normalized;
			dstPosition = 
				context.Character.GetPosition() + 
				new Vector3(direction.x, direction.y, 0.0f)*
				UnityEngine.Random.Range(2.0f, 5.0f);


			dstPosition.x = Math.Max(dstPosition.x, context.Character.Context.Limits().XMin);
			dstPosition.x = Math.Min(dstPosition.x, context.Character.Context.Limits().XMax);
			dstPosition.y = Math.Max(dstPosition.y, context.Character.Context.Limits().YMin);
			dstPosition.y = Math.Min(dstPosition.y, context.Character.Context.Limits().YMax);

		}
	}
	
}