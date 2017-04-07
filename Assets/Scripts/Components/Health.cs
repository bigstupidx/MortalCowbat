using System;
using UnityEngine;


namespace Battle.Comp
{
	public class Health : CharacterComponent
	{
		public Action<float, float> HealthChangedAction;

		float actualHealth;
		float fullHealth;
	
		public void Init(int health, int fullHealth)
		{
			this.fullHealth = fullHealth;
			SetHealth(health);
		}

		public bool SetHealth(float health)
		{
			actualHealth = Mathf.Max(0.0f, health);
			if (HealthChangedAction != null) {
				HealthChangedAction(actualHealth, fullHealth);
			}

			return actualHealth > 0;
		}

		public bool ReduceHealth(float amount)
		{
			return GetComp<Health>().SetHealth(actualHealth - amount);
		}
	}
}
