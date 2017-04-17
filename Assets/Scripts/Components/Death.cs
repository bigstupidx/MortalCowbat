using System;
using Ai;
using UnityEngine;
using System.Collections;


namespace Battle.Comp
{
	public class Death : CharacterComponent
	{
		public GameObject Effect;
		public Action<Character> DeathFinishedAction;
		public Action<Character> DeathStartedAction;


		public bool IsDying { get; private set; }

		public void Perform()
		{
			IsDying = true;
			GetComp<Moving>().Stop();
			GetComp<Jumping>().SetSpeedX(0.0f);
			GetComp<Animating>().SetTrigger(Defs.Animations.Die);

			if (Effect != null)
				GetComp<Effects>().EffectManager.CreateEffect(Effect).Run(gameObject);
			Destroy(gameObject.GetComponent<AiStateMachine>()); // TODO

			CallDeathStartedAction();
			CallDeathFinishedAction();
		}

		void CallDeathStartedAction()
		{
			if (DeathStartedAction != null) {
				DeathStartedAction(GetCharacter());
			}
		}

		void CallDeathFinishedAction()
		{
			if (DeathFinishedAction != null) {
				DeathFinishedAction(GetCharacter());
			}
		}
	}
}
