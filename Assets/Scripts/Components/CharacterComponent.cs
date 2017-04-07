using UnityEngine;

namespace Battle.Comp
{
	public class CharacterComponent : MonoBehaviour
	{
		ComponentHolder componentHolder;
		Character character;

		public void Init(Character character, ComponentHolder componentHolder)
		{
			this.character = character;
			this.componentHolder = componentHolder;
		}

		protected T GetComp<T>() where T : CharacterComponent
		{
			return componentHolder.Get<T>();
		}

		protected Character GetCharacter()
		{
			return character;
		}

		public virtual void UpdateMe()
		{}
	}
}

