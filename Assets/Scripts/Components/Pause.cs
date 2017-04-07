
namespace Battle.Comp
{
	public class Pause : CharacterComponent
	{
		public bool Paused { get; set; }

	
		public void Perform()
		{
			GetComp<Animating>().SetAnimatorSpeed(0.0f);
			Paused = true;
		}

		public void Cancel()
		{
			GetComp<Animating>().SetAnimatorSpeed(1.0f);
			Paused = false;
		}
	}
}

