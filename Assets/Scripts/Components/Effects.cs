using UnityEngine;


namespace Battle.Comp
{
	public class Effects : CharacterComponent
	{
		public EffectManager EffectManager { get; private set;}

		void Awake()
		{
			EffectManager = GameObject.FindObjectOfType<EffectManager>();
		}
	}
}

