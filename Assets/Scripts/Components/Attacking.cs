namespace Battle.Comp
{
	public class Attacking : CharacterComponent
	{
		bool attacking;

		public void Perform()
		{
			attacking = true;
		}

		public bool IsAttacking()
		{
			return attacking;
		}

		public void Stop()
		{
			attacking = false;
		}
	}
}
