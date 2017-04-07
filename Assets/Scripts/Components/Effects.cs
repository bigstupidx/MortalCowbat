using UnityEngine;


namespace Battle.Comp
{
	public class Effects : CharacterComponent
	{
		public GameObject ShakeEffect;
		public GameObject HitBlink;

		public EffectManager EffectManager { get; private set;}

		void Awake()
		{
			EffectManager = GameObject.FindObjectOfType<EffectManager>();
		}
	}
}

