using UnityEngine;


namespace Battle.Comp
{
	public class Effects : CharacterComponent
	{
		public GameObject ShakeEffect;
		public GameObject HitBlink;
		public GameObject KilledBlink;


		public EffectManager EffectManager { get; private set;}

		void Awake()
		{
			EffectManager = GameObject.FindObjectOfType<EffectManager>();
		}


		public void OnEffectEvent(AnimationEvent animEvent)
		{
			if (animEvent.objectReferenceParameter != null) {
				string poi = animEvent.stringParameter;
				var container = GetComp<Visual>().GetPoi(poi);
				var desc = new EffectDescriptor();
				desc.Effect = (animEvent.objectReferenceParameter as GameObject).GetComponent<Effect>();
				desc.InWorldSpace = true;
				EffectManager.CreateEffect(desc, container, GetCharacter().gameObject).Run(GetCharacter().gameObject);
			}	
		}

	}
}

