using UnityEngine;
using System;
using System.Collections.Generic;

namespace Battle.Comp
{
	public class CharacterComponent : MonoBehaviour
	{
		ComponentHolder componentHolder;
		Character character;

		List<Type> requiredComponents = new List<Type>();

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

		public virtual void LateUpdateMe()
		{}
	}
}

