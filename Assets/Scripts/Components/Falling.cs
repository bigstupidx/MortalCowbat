using System;

namespace Battle.Comp
{
	public class Falling : CharacterComponent
	{
		public bool IsFalling { get { return falling; }}
		public bool ShakeCamera;
		bool falling;

		public void Fall ()
		{
			GetComp<Animating>().SetTrigger(Defs.Animations.Fall);
			falling = true;
		}

		public void FinishFall()
		{
			falling = false;
		}
	}
}

