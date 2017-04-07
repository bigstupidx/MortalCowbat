using System;
using UnityEngine;
using Ai;


namespace Battle.Comp
{
	public class Death : CharacterComponent
	{
		public Action<Character> DeathAction;

		public void Perform()
		{
			GetComp<Moving>().Stop();
			GetComp<Jumping>().SetSpeedX(0.0f);
			GetComp<Animating>().SetTrigger(Defs.Animations.Die);
			Destroy(gameObject.GetComponent<AiStateMachine>()); // TODO
			if (DeathAction != null) {
				DeathAction(GetCharacter());
			}
		}
	}
}
